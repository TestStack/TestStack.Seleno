using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Data.SqlTypes;

namespace ErikEJ
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ce")]
    public sealed class SqlCeMembershipProvider : MembershipProvider
    {
        private const int SALT_SIZE = 64;
        private string encryptionKey = "AE09F72BA97CBBB5EEAAFF";

        private int newPasswordLength = 8;
        private string eventSource = "SqlCeMembershipProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";
        private string connectionString;

        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }


        //
        // System.Configuration.Provider.ProviderBase.Initialize Method
        //

        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SqlCeMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SqlCe Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            ApplicationName = GetConfigValue(config["applicationName"],
                                            System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"), CultureInfo.InvariantCulture);
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"), CultureInfo.InvariantCulture);
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"), CultureInfo.InvariantCulture);
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"), CultureInfo.InvariantCulture);
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""), CultureInfo.InvariantCulture);
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"), CultureInfo.InvariantCulture);
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"), CultureInfo.InvariantCulture);
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"), CultureInfo.InvariantCulture);
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"), CultureInfo.InvariantCulture);
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"), CultureInfo.InvariantCulture);
            encryptionKey = GetConfigValue(config["encryptionKey"],
                                            encryptionKey);

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //
            // Initialize SqlCeConnection.
            //

            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || string.IsNullOrWhiteSpace(ConnectionStringSettings.ConnectionString))
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;

            SqlCeMembershipUtils.CreateDatabaseIfRequired(connectionString, ApplicationName);

            pApplicationId = SqlCeMembershipUtils.GetApplicationId(connectionString, ApplicationName);
        }

        //
        // A helper function to retrieve config values from the configuration file.
        //

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        private Guid pApplicationId;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;

        //
        // System.Web.Security.MembershipProvider properties.
        //

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }


        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }


        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }


        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }


        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }


        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }


        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        private int pMinRequiredNonAlphanumericCharacters;

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        private int pMinRequiredPasswordLength;

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        private string pPasswordStrengthRegularExpression;

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        //
        // System.Web.Security.MembershipProvider methods.
        //

        //
        // MembershipProvider.ChangePassword
        //

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            string salt = GetSaltForUser(username);
            int rowsAffected = 0;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("UPDATE [aspnet_Membership] " +
                        " SET Password = @Password, LastPasswordChangedDate = @LastPasswordChangedDate " +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 128).Value = EncodePassword(newPassword, salt);
                    cmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "ChangePassword");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }



        //
        // MembershipProvider.ChangePasswordQuestionAndAnswer
        //

        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPasswordQuestion,
                      string newPasswordAnswer)
        {

            int rowsAffected = 0;

            if (!ValidateUser(username, password))
                return false;

            string salt = GetSaltForUser(username);

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("UPDATE [aspnet_Membership] " +
                        " SET PasswordQuestion = @Question, PasswordAnswer = @Answer" +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@Question", SqlDbType.NVarChar, 256).Value = newPasswordQuestion;
                    cmd.Parameters.Add("@Answer", SqlDbType.NVarChar, 128).Value = EncodePassword(newPasswordAnswer, salt);
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }



        //
        // MembershipProvider.CreateUser
        //

        public override MembershipUser CreateUser(string username,
                 string password,
                 string email,
                 string passwordQuestion,
                 string passwordAnswer,
                 bool isApproved,
                 object providerUserKey,
                 out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && !string.IsNullOrWhiteSpace(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
                DateTime createDate = DateTime.UtcNow;

                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                passwordQuestion = string.IsNullOrWhiteSpace(passwordQuestion) ? string.Empty : passwordQuestion;
                passwordAnswer = string.IsNullOrWhiteSpace(passwordAnswer) ? string.Empty : passwordAnswer;
                string salt = GenerateSalt();

                using (SqlCeConnection conn = new SqlCeConnection(connectionString))
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(
                        @"INSERT INTO [aspnet_Membership]
                           ([ApplicationId]
                           ,[UserId]
                           ,[Password]
                           ,[PasswordFormat]
                           ,[PasswordSalt]
                           ,[Email]
                           ,[LoweredEmail]
                           ,[PasswordQuestion]
                           ,[PasswordAnswer]
                           ,[IsApproved]
                           ,[IsLockedOut]
                           ,[CreateDate]
                           ,[LastLoginDate]
                           ,[LastPasswordChangedDate]
                           ,[LastLockoutDate]
                           ,[FailedPasswordAttemptCount]
                           ,[FailedPasswordAttemptWindowStart]
                           ,[FailedPasswordAnswerAttemptCount]
                           ,[FailedPasswordAnswerAttemptWindowStart]
                           ,[Comment])
                     VALUES
                           (@ApplicationId
                           ,@UserId
                           ,@Password
                           ,@PasswordFormat
                           ,@PasswordSalt
                           ,@Email
                           ,@LoweredEmail
                           ,@PasswordQuestion
                           ,@PasswordAnswer
                           ,@IsApproved
                           ,@IsLockedOut
                           ,@CreateDate
                           ,@LastLoginDate
                           ,@LastPasswordChangedDate
                           ,@LastLockoutDate
                           ,@FailedPasswordAttemptCount
                           ,@FailedPasswordAttemptWindowStart
                           ,@FailedPasswordAnswerAttemptCount
                           ,@FailedPasswordAnswerAttemptWindowStart
                           ,@Comment)", conn))
                    {
                        cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                        cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)providerUserKey;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 128).Value = EncodePassword(password, salt);
                        cmd.Parameters.Add("@PasswordFormat", SqlDbType.Int).Value = PasswordFormat.GetHashCode();
                        cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 128).Value = salt;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;
                        cmd.Parameters.Add("@LoweredEmail", SqlDbType.NVarChar, 256).Value = email.ToLowerInvariant();
                        cmd.Parameters.Add("@PasswordQuestion", SqlDbType.NVarChar, 256).Value = passwordQuestion;
                        cmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 128).Value = EncodePassword(passwordAnswer, salt);
                        cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = isApproved;
                        cmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = false;
                        cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = createDate;
                        cmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = createDate;
                        cmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = createDate;
                        cmd.Parameters.Add("@LastLockoutDate", SqlDbType.DateTime).Value = SqlDateTime.MinValue;
                        cmd.Parameters.Add("@FailedPasswordAttemptCount", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@FailedPasswordAttemptWindowStart", SqlDbType.DateTime).Value = SqlDateTime.MinValue;
                        cmd.Parameters.Add("@FailedPasswordAnswerAttemptCount", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@FailedPasswordAnswerAttemptWindowStart", SqlDbType.DateTime).Value = SqlDateTime.MinValue;
                        cmd.Parameters.Add("@Comment", SqlDbType.NText).Value = "";


                        using (SqlCeCommand cmd2 = new SqlCeCommand(
                            @"INSERT INTO [aspnet_Users]
                                   ([ApplicationId]
                                   ,[UserId]
                                   ,[UserName]
                                   ,[LoweredUserName]
                                   ,[IsAnonymous]
                                   ,[LastActivityDate])
                             VALUES
                                   (@ApplicationId
                                   ,@UserId
                                   ,@UserName
                                   ,@LoweredUserName
                                   ,@IsAnonymous
                                   ,@LastActivityDate)
                        ", conn))
                        {
                            cmd2.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                            cmd2.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)providerUserKey;
                            cmd2.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
                            cmd2.Parameters.Add("@LoweredUserName", SqlDbType.NVarChar, 256).Value = username.ToLowerInvariant();
                            cmd2.Parameters.Add("@IsAnonymous", SqlDbType.Bit).Value = false;
                            cmd2.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = createDate;

                            SqlCeTransaction tran = null;

                            try
                            {
                                conn.Open();

                                tran = conn.BeginTransaction();
                                cmd.Transaction = tran;
                                cmd2.Transaction = tran;

                                int recAdded = cmd2.ExecuteNonQuery();
                                int recAdded2 = cmd.ExecuteNonQuery();

                                if (recAdded > 0 && recAdded2 > 0)
                                {
                                    status = MembershipCreateStatus.Success;
                                    tran.Commit();
                                }
                                else
                                {
                                    status = MembershipCreateStatus.UserRejected;
                                    tran.Rollback();
                                }
                            }

                            catch (SqlCeException e)
                            {
                                if (WriteExceptionsToEventLog)
                                {
                                    WriteToEventLog(e, "CreateUser");
                                }
                                status = MembershipCreateStatus.ProviderError;
                            }
                            catch (Exception)
                            {
                                if (tran != null)
                                    tran.Rollback();
                                status = MembershipCreateStatus.ProviderError;
                            }
                        }
                    }
                }
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        //
        // MembershipProvider.DeleteUser
        //

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            int rowsAffected = 0;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("DELETE FROM [aspnet_UsersInRoles] " +
                            " WHERE UserId = @UserId AND @ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    SqlCeTransaction tran = null;

                    try
                    {
                        conn.Open();

                        tran = conn.BeginTransaction();

                        cmd.Transaction = tran;

                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DELETE FROM [aspnet_Membership] " +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                        cmd.ExecuteNonQuery();

                        if (deleteAllRelatedData)
                        {
							cmd.Parameters.Clear();
							cmd.CommandText = "DELETE FROM aspnet_Profile WHERE UserId = @userId";

							cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value =  GetUserId(username);
							cmd.ExecuteNonQuery();

							// reinstate the application id parameter
							cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                        }

                        cmd.CommandText = "DELETE FROM [aspnet_Users] " +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                        rowsAffected = cmd.ExecuteNonQuery();

                        tran.Commit();

                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "DeleteUser");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        if (tran != null)
                            tran.Rollback();
                    }
                }
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        //
        // MembershipProvider.GetAllUsers
        //

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Count(*) FROM [aspnet_Membership] " +
                                                  "WHERE ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    totalRecords = 0;

                    try
                    {
                        conn.Open();
                        totalRecords = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);

                        if (totalRecords <= 0) { return users; }

                        cmd.CommandText = "SELECT [aspnet_Membership].UserId, Username, Email, PasswordQuestion," +
                                 " Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate," +
                                 " LastActivityDate, LastPasswordChangedDate, LastLockoutDate " +
                                 " FROM [aspnet_Membership], [aspnet_Users] " +
                                 " WHERE [aspnet_Membership].ApplicationId = @ApplicationId " +
                                 " AND [aspnet_Membership].UserId = [aspnet_Users].UserId " +
                                 " ORDER BY Username Asc OFFSET @PageStart ROWS FETCH NEXT @PageSize ROWS ONLY ";

                        cmd.Parameters.Add("@PageStart", SqlDbType.Int).Value = pageIndex * pageSize;
                        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetAllUsers");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return users;
        }


        //
        // MembershipProvider.GetNumberOfUsersOnline
        //

        public override int GetNumberOfUsersOnline()
        {
            int numOnline = 0;
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.UtcNow.Subtract(onlineSpan);

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Count(*) FROM [aspnet_Users] " +
                        " WHERE LastActivityDate > @CompareDate AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@CompareDate", SqlDbType.DateTime).Value = compareTime;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        numOnline = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetNumberOfUsersOnline");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return numOnline;
        }



        //
        // MembershipProvider.GetPassword
        //

        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            string password = "";
            string passwordAnswer = "";
            string salt = "";

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Password, PasswordAnswer, PasswordSalt, IsLockedOut FROM [aspnet_Membership] " +
                      " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                if (reader.GetBoolean(3))
                                    throw new MembershipPasswordException("The supplied user is locked out.");
                                password = reader.GetString(0);
                                passwordAnswer = reader.GetString(1);
                                salt = reader.GetString(2);
                            }
                            else
                            {
                                throw new MembershipPasswordException("The supplied user name is not found.");
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetPassword");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer, salt))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }



        //
        // MembershipProvider.GetUser(string, bool)
        //

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser u = null;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT [aspnet_Membership].UserId, Username, Email, PasswordQuestion," +
                     " Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate," +
                     " LastActivityDate, LastPasswordChangedDate, LastLockoutDate" +
                     " FROM [aspnet_Membership], [aspnet_Users] WHERE Username = @Username AND [aspnet_Membership].ApplicationId = @ApplicationId AND [aspnet_Membership].UserId = [aspnet_Users].UserId ", conn))
                {

                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value = username;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                u = GetUserFromReader(reader);

                                if (userIsOnline)
                                {
                                    using (SqlCeCommand updateCmd = new SqlCeCommand("UPDATE [aspnet_Users] " +
                                              "SET LastActivityDate = @LastActivityDate " +
                                              "WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                                    {

                                        updateCmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                                        updateCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = reader.GetGuid(0);
                                        updateCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetUser(String, Boolean)");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            return u;
        }


        //
        // MembershipProvider.GetUser(object, bool)
        //

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser u = null;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT [aspnet_Membership].UserId, Username, Email, PasswordQuestion," +
                      " Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate," +
                      " LastActivityDate, LastPasswordChangedDate, LastLockoutDate" +
                      " FROM [aspnet_Membership], [aspnet_Users] WHERE [aspnet_Membership].UserId = @UserId AND [aspnet_Membership].UserId = [aspnet_Users].UserId ", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)providerUserKey;
                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                u = GetUserFromReader(reader);

                                if (userIsOnline)
                                {
                                    using (SqlCeCommand updateCmd = new SqlCeCommand("UPDATE [aspnet_Users] " +
                                              "SET LastActivityDate = @LastActivityDate " +
                                              "WHERE UserId = @UserId", conn))
                                    {
                                        updateCmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                                        updateCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)providerUserKey;

                                        updateCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetUser(Object, Boolean)");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return u;
        }


        //
        // GetUserFromReader
        //    A helper function that takes the current row from the SqlCeDataReader
        // and hydrates a MembershipUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        //

        private MembershipUser GetUserFromReader(SqlCeDataReader reader)
        {
            if (string.IsNullOrWhiteSpace(reader.GetString(1))) return null;
            object providerUserKey = null;
            string strGooid = Guid.NewGuid().ToString();

            if (reader.GetValue(0).ToString().Length > 0)
                providerUserKey = new Guid(reader.GetValue(0).ToString());
            else
                providerUserKey = new Guid(strGooid);

            string username = reader.GetString(1);

            string email = reader.GetString(2);

            string passwordQuestion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);

            string comment = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);

            bool isApproved = reader.IsDBNull(5) ? false : reader.GetBoolean(5);

            bool isLockedOut = reader.IsDBNull(6) ? false : reader.GetBoolean(6);

            DateTime creationDate = reader.IsDBNull(7) ? DateTime.UtcNow : reader.GetDateTime(7);

            DateTime lastLoginDate = reader.IsDBNull(8) ? DateTime.UtcNow : reader.GetDateTime(8);

            DateTime lastActivityDate = reader.IsDBNull(9) ? DateTime.UtcNow : reader.GetDateTime(9);

            DateTime lastPasswordChangedDate = reader.IsDBNull(10) ? DateTime.UtcNow : reader.GetDateTime(10);

            DateTime lastLockedOutDate = reader.IsDBNull(11) ? DateTime.UtcNow : reader.GetDateTime(11);

            MembershipUser u = new MembershipUser(this.Name,
                                                  username,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate);

            return u;
        }


        //
        // MembershipProvider.UnlockUser
        //

        public override bool UnlockUser(string userName)
        {
            int rowsAffected = 0;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("UPDATE [aspnet_Membership] " +
                                                  " SET IsLockedOut = 0, LastLockoutDate = @LastLockedOutDate " +
                                                  " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {

                    cmd.Parameters.Add("@LastLockedOutDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(userName);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "UnlockUser");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        //
        // MembershipProvider.GetUserNameByEmail
        //

        public override string GetUserNameByEmail(string email)
        {
            string username = "";

            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Username" +
                      " FROM [aspnet_Users], [aspnet_Membership] WHERE Email = @Email AND [aspnet_Membership].ApplicationId = @ApplicationId AND [aspnet_Membership].UserId = [aspnet_Users].UserId ", conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();
                        object o = cmd.ExecuteScalar();
                        if (o != null) username = Convert.ToString(o, CultureInfo.InvariantCulture);
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetUserNameByEmail");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (username == null)
                username = "";

            return username;
        }

        //
        // MembershipProvider.ResetPassword
        //

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword =
              System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");

            int rowsAffected = 0;
            string passwordAnswer = "";
            string salt = GetSaltForUser(username);

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT PasswordAnswer, IsLockedOut FROM [aspnet_Membership] " +
                      " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                //object val = reader.GetValue(1);

                                if (Convert.ToBoolean(reader.GetValue(1)))
                                    throw new MembershipPasswordException("The supplied user is locked out.");

                                passwordAnswer = reader.GetString(0);
                            }
                            else
                            {
                                throw new MembershipPasswordException("The supplied user name is not found.");
                            }
                            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer, salt))
                            {
                                UpdateFailureCount(username, "passwordAnswer");

                                throw new MembershipPasswordException("Incorrect password answer.");
                            }
                        }

                        using (SqlCeCommand updateCmd = new SqlCeCommand("UPDATE [aspnet_Membership] " +
                            " SET Password = @Password, LastPasswordChangedDate = @LastPasswordChangedDate" +
                            " WHERE UserId = @UserId AND ApplicationId = @ApplicationId AND IsLockedOut = 0", conn))
                        {

                            updateCmd.Parameters.Add("@Password", SqlDbType.NVarChar, 128).Value = EncodePassword(newPassword, salt);
                            updateCmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                            updateCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                            updateCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                            rowsAffected = updateCmd.ExecuteNonQuery();
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "ResetPassword");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                if (rowsAffected > 0)
                {
                    return newPassword;
                }
                else
                {
                    throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
                }
            }
        }


        //
        // MembershipProvider.UpdateUser
        //

        public override void UpdateUser(MembershipUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (RequiresUniqueEmail)
            {
                string userName = GetUserNameByEmail(user.Email);
                if (!string.IsNullOrWhiteSpace(userName) && !userName.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase))
                    throw new ProviderException("The e-mail address that you entered is already in use. Please enter a different e-mail address.");
            }

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("UPDATE [aspnet_Membership] " +
                        " SET Email = @Email, LoweredEmail = @LoweredEmail, Comment = @Comment," +
                        " IsApproved = @IsApproved, LastLoginDate = @LastLoginDate " +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = user.Email;
                    cmd.Parameters.Add("@LoweredEmail", SqlDbType.NVarChar, 256).Value = user.Email.ToLowerInvariant();
                    cmd.Parameters.Add("@Comment", SqlDbType.NText).Value = user.Comment;
                    cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = user.IsApproved;
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)user.ProviderUserKey;
                    cmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = user.LastLoginDate;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "UpdateUser");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        //
        // MembershipProvider.ValidateUser
        //

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Password, PasswordSalt, IsApproved FROM [aspnet_Membership] " +
                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId AND IsLockedOut = 0", conn))
                {

                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    bool isApproved = false;
                    string pwd = "";
                    string salt = "";

                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                pwd = reader.GetString(0);
                                salt = reader.GetString(1);
                                isApproved = reader.GetBoolean(2);
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if (CheckPassword(password, pwd, salt))
                        {
                            if (isApproved)
                            {
                                isValid = true;

                                using (SqlCeCommand updateCmd = new SqlCeCommand("UPDATE [aspnet_Membership] SET LastLoginDate = @LastLoginDate" +
                                                                        " WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                                {

                                    updateCmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                                    updateCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                                    updateCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                                    updateCmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            UpdateFailureCount(username, "password");
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "ValidateUser");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return isValid;
        }

        //
        // SqlCeMembershipProvider.UpdateUserName
        //

        public void UpdateUserName(string oldUserName, string newUserName)
        {
            if (String.IsNullOrWhiteSpace(oldUserName))
                throw new ArgumentException("Parameter is null or empty", "oldUserName");
            if (String.IsNullOrWhiteSpace(newUserName))
                throw new ArgumentException("Parameter is null or empty", "newUserName");

            if (GetUser(newUserName, false) != null)
                throw new ProviderException("User Name already exists");

            Guid userId = GetUserId(oldUserName);

            if (userId == Guid.Empty)
                throw new ProviderException("User does not exist");

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"
                        UPDATE [aspnet_Users]
                        SET UserName = @Username, LoweredUserName = @LoweredUserName
                        WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value = newUserName;
                    cmd.Parameters.Add("@LoweredUserName", SqlDbType.NVarChar, 256).Value = newUserName.ToLowerInvariant();
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException ex)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(ex, "UpdateUserName");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(newUserName, false);
                }
            }
        }


        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //

        private void UpdateFailureCount(string username, string failureType)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT FailedPasswordAttemptCount, " +
                                                   "  FailedPasswordAttemptWindowStart, " +
                                                   "  FailedPasswordAnswerAttemptCount, " +
                                                   "  FailedPasswordAnswerAttemptWindowStart " +
                                                   "  FROM [aspnet_Membership] " +
                                                   "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId", conn))
                {

                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    DateTime windowStart = new DateTime();
                    int failureCount = 0;

                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {

                            if (reader.Read())
                            {
                                if (failureType == "password")
                                {
                                    failureCount = reader.GetInt32(0);
                                    try
                                    {
                                        windowStart = reader.GetDateTime(1);
                                    }
                                    catch
                                    {
                                        windowStart = DateTime.UtcNow;
                                    }
                                }

                                if (failureType == "passwordAnswer")
                                {
                                    failureCount = reader.GetInt32(2);
                                    windowStart = reader.GetDateTime(3);
                                }
                            }
                        }

                        DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                        if (failureCount == 0 || DateTime.UtcNow > windowEnd)
                        {
                            // First password failure or outside of PasswordAttemptWindow. 
                            // Start a new password failure count from 1 and a new window starting now.

                            if (failureType == "password")
                                cmd.CommandText = "UPDATE [aspnet_Membership] " +
                                                  "  SET FailedPasswordAttemptCount = @Count, " +
                                                  "      FailedPasswordAttemptWindowStart = @WindowStart " +
                                                  "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                            if (failureType == "passwordAnswer")
                                cmd.CommandText = "UPDATE [aspnet_Membership] " +
                                                  "  SET FailedPasswordAnswerAttemptCount = @Count, " +
                                                  "      FailedPasswordAnswerAttemptWindowStart = @WindowStart " +
                                                  "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                            cmd.Parameters.Clear();

                            cmd.Parameters.Add("@Count", SqlDbType.Int).Value = 1;
                            cmd.Parameters.Add("@WindowStart", SqlDbType.DateTime).Value = DateTime.UtcNow;
                            cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                            cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                            if (cmd.ExecuteNonQuery() < 0)
                                throw new ProviderException("Unable to update failure count and window start.");
                        }
                        else
                        {
                            if (failureCount++ >= MaxInvalidPasswordAttempts)
                            {
                                // Password attempts have exceeded the failure threshold. Lock out
                                // the user.

                                cmd.CommandText = "UPDATE [aspnet_Membership] " +
                                                  "  SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockedOutDate " +
                                                  "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                                cmd.Parameters.Clear();

                                cmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = true;
                                cmd.Parameters.Add("@LastLockedOutDate", SqlDbType.DateTime).Value = DateTime.UtcNow;
                                cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                                cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                                if (cmd.ExecuteNonQuery() < 0)
                                    throw new ProviderException("Unable to lock out user.");
                            }
                            else
                            {
                                // Password attempts have not exceeded the failure threshold. Update
                                // the failure counts. Leave the window the same.

                                if (failureType == "password")
                                    cmd.CommandText = "UPDATE [aspnet_Membership] " +
                                                      "  SET FailedPasswordAttemptCount = @Count" +
                                                      "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                                if (failureType == "passwordAnswer")
                                    cmd.CommandText = "UPDATE [aspnet_Membership] " +
                                                      "  SET FailedPasswordAnswerAttemptCount = @Count" +
                                                      "  WHERE UserId = @UserId AND ApplicationId = @ApplicationId";

                                cmd.Parameters.Clear();

                                cmd.Parameters.Add("@Count", SqlDbType.Int).Value = failureCount;
                                cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                                cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                                if (cmd.ExecuteNonQuery() < 0)
                                    throw new ProviderException("Unable to update failure count.");
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "UpdateFailureCount");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //

        private bool CheckPassword(string password, string dbpassword, string salt)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password, salt);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        //
        // GetSaltForUser
        //      Gets the current salt used to encrypt or hash the password, question, and answer

        private string GetSaltForUser(string username)
        {
            Guid userId = GetUserId(username);

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(
                    @"SELECT PasswordSalt FROM aspnet_Membership
                      WHERE ApplicationId = @ApplicationId AND UserId = @UserId", conn))
                {
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar() as string;
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetSaltForUser");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        //
        // GenerateSalt
        //      Generates a random salt string using a random number generator
        //

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[SALT_SIZE];
            new RNGCryptoServiceProvider().GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);
            return (salt.Length > 128) ? salt.Substring(0, 128) : salt;
        }

        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        private string EncodePassword(string password, string salt)
        {
            if (password == null) password = "";
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    byte[] passBytes = Encoding.Unicode.GetBytes(password);
                    byte[] saltBytes = Convert.FromBase64String(salt);
                    byte[] allBytes = new byte[saltBytes.Length + passBytes.Length];
                    Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
                    Buffer.BlockCopy(passBytes, 0, allBytes, saltBytes.Length, passBytes.Length);
                    encodedPassword = Convert.ToBase64String(EncryptPassword(allBytes));
                    break;
                case MembershipPasswordFormat.Hashed:
                    using (HMACSHA1 hash = new HMACSHA1())
                    {
                        hash.Key = (salt.IndexOfAny("=+/".ToCharArray()) > -1) ? Convert.FromBase64String(salt) : HexToByte(salt);
                        encodedPassword =
                          Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    }
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return (encodedPassword.Length > 128) ? encodedPassword.Substring(0, 128) : encodedPassword;
        }


        //
        // UnEncodePassword
        //   Decrypts or leaves the password clear based on the PasswordFormat.
        //

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    byte[] allBytes = Convert.FromBase64String(password);
                    byte[] decryptedBytes = DecryptPassword(allBytes);
                    password = (decryptedBytes == null) ? null :
                        Encoding.Unicode.GetString(decryptedBytes, SALT_SIZE, decryptedBytes.Length - SALT_SIZE);
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot decode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        //
        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        //
        // MembershipProvider.FindUsersByName
        //

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Count(*) FROM [aspnet_Users] " +
                          "WHERE [aspnet_Users].Username LIKE @UsernameSearch AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@UsernameSearch", SqlDbType.NVarChar, 256).Value = usernameToMatch;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();
                        totalRecords = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);

                        if (totalRecords <= 0) { return users; }

                        cmd.CommandText = "SELECT [aspnet_Membership].UserId, [aspnet_Users].Username, Email, PasswordQuestion," +
                          " Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate," +
                          " LastActivityDate, LastPasswordChangedDate, LastLockoutDate " +
                          " FROM [aspnet_Membership], [aspnet_Users] " +
                          " WHERE [aspnet_Users].Username LIKE @UsernameSearch AND [aspnet_Membership].ApplicationId = @ApplicationId " +
                          " AND [aspnet_Membership].UserId = [aspnet_Users].UserId " +
                          " ORDER BY [aspnet_Users].Username Asc OFFSET @PageStart ROWS FETCH NEXT @PageSize ROWS ONLY ";

                        cmd.Parameters.Add("@PageStart", SqlDbType.Int).Value = pageIndex * pageSize;
                        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "FindUsersByName");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return users;
        }

        //
        // MembershipProvider.FindUsersByEmail
        //

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Count(*) FROM [aspnet_Membership] " +
                                                  "WHERE Email LIKE @EmailSearch AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@EmailSearch", SqlDbType.NVarChar, 256).Value = emailToMatch;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    totalRecords = 0;

                    try
                    {
                        conn.Open();
                        totalRecords = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);

                        if (totalRecords <= 0) { return users; }

                        cmd.CommandText = "SELECT [aspnet_Membership].UserId, [aspnet_Users].Username, Email, PasswordQuestion," +
                                 " Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate," +
                                 " LastActivityDate, LastPasswordChangedDate, LastLockoutDate " +
                                 " FROM [aspnet_Membership], [aspnet_Users] " +
                                 " WHERE Email LIKE @EmailSearch AND [aspnet_Membership].ApplicationId = @ApplicationId " +
                                 " AND [aspnet_Membership].UserId = [aspnet_Users].UserId " +
                                 " ORDER BY [aspnet_Users].Username Asc OFFSET @PageStart ROWS FETCH NEXT @PageSize ROWS ONLY ";

                        cmd.Parameters.Add("@PageStart", SqlDbType.Int).Value = pageIndex * pageSize;
                        cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipUser u = GetUserFromReader(reader);
                                users.Add(u);
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "FindUsersByEmail");

                            throw new ProviderException(exceptionMessage);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return users;
        }

        private Guid GetUserId(string userName)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"SELECT  UserId
                                FROM  aspnet_Users
                                WHERE LOWER(@UserName) = LoweredUserName AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = userName;
                    {
                        try
                        {
                            conn.Open();
                            Guid? result = cmd.ExecuteScalar() as Guid?;
                            if (result.HasValue)
                                return result.Value;
                        }
                        catch (SqlCeException e)
                        {
                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(e, "GetUserId");
                            }
                            else
                            {
                                throw;
                            }
                        }

                    }
                }
            }
            return Guid.Empty;

        }

       /// <summary>
       ///      Migrates the data in a membership database from ASP.NET 2.0 to ASP.NET 4.0 compatibility
       ///      by copying the data from the old database to a new database
       /// </summary>
       /// <param name="oldConnStr"></param>
       /// <param name="newConnStr"></param>
        public void MigrateMembershipDatabaseToAspNet40(string oldConnStr, string newConnStr)
        {
            if (String.IsNullOrWhiteSpace(oldConnStr))
                throw new ArgumentException("Parameter cannot be null or empty", "oldConnStr");

            if (String.IsNullOrWhiteSpace(newConnStr))
                throw new ArgumentException("Parameter cannot be null or empty", "newConnStr");

            SqlCeConnection oldConn = new SqlCeConnection(oldConnStr);
            SqlCeConnection newConn = new SqlCeConnection(newConnStr);

            SqlCeTransaction newTrans = null;

            SqlCeCommand oldCmd = null;
            SqlCeCommand newCmd = null;

            System.Collections.Generic.Dictionary<String, Guid> apps = new System.Collections.Generic.Dictionary<String, Guid>();
            System.Collections.Generic.Dictionary<String, Guid> roles = new System.Collections.Generic.Dictionary<String, Guid>();

            try
            {
                oldConn.Open();
                newConn.Open();

                newTrans = newConn.BeginTransaction();

                /**********************************************************************************
                 * aspnet_Applications
                 *********************************************************************************/
                oldCmd = new SqlCeCommand("SELECT DISTINCT ApplicationName FROM aspnet_Users", oldConn);

                using (SqlCeDataReader reader = oldCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        Guid id = Guid.NewGuid();

                        newCmd = new SqlCeCommand(@"
                            SELECT ApplicationId FROM aspnet_Applications
                            WHERE ApplicationName = @ApplicationName", newConn);

                        newCmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = name;

                        object value = newCmd.ExecuteScalar();

                        if (value != null && value != DBNull.Value)
                        {
                            apps.Add(name, (Guid)value);
                        }
                        else
                        {
                            apps.Add(name, id);

                            newCmd = new SqlCeCommand(
                            @"INSERT INTO aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName, Description)
                                VALUES (@ApplicationId, @ApplicationName, @LoweredApplicationName, NULL)", newConn, newTrans);
                            newCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = id;
                            newCmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = name;
                            newCmd.Parameters.Add("@LoweredApplicationName", SqlDbType.NVarChar, 256).Value = name.ToLowerInvariant();

                            newCmd.ExecuteNonQuery();
                        }
                    }
                }

                /**********************************************************************************
                 * aspnet_Users
                 *********************************************************************************/
                oldCmd = new SqlCeCommand(
                    @"SELECT PKID, ApplicationName, Username, LastActivityDate
                        FROM aspnet_Users", oldConn);

                using (SqlCeDataReader reader = oldCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid userId = reader.GetGuid(0);
                        string applicationName = reader.GetString(1);
                        string userName = reader.GetString(2);
                        DateTime lastActivityDate = reader.GetDateTime(3);

                        newCmd = new SqlCeCommand(
                            @"INSERT INTO [aspnet_Users]
                                   ([ApplicationId]
                                   ,[UserId]
                                   ,[UserName]
                                   ,[LoweredUserName]
                                   ,[IsAnonymous]
                                   ,[LastActivityDate])
                             VALUES
                                   (@ApplicationId
                                   ,@UserId
                                   ,@UserName
                                   ,@LoweredUserName
                                   ,@IsAnonymous
                                   ,@LastActivityDate)", newConn, newTrans);

                        newCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = apps[applicationName];
                        newCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                        newCmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = userName;
                        newCmd.Parameters.Add("@LoweredUserName", SqlDbType.NVarChar, 256).Value = userName.ToLowerInvariant();
                        newCmd.Parameters.Add("@IsAnonymous", SqlDbType.Bit).Value = false;
                        newCmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = lastActivityDate;

                        newCmd.ExecuteNonQuery();
                    }
                }

                /**********************************************************************************
                 * aspnet_Membership
                 *********************************************************************************/
                oldCmd = new SqlCeCommand(
                    @"SELECT ApplicationName, PKID, Password, Email, PasswordQuestion, PasswordAnswer, 
                        IsApproved, IsLockedOut, CreationDate, LastLoginDate, LastPasswordChangedDate, LastLockedOutDate,
                        FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, 
                        FailedPasswordAnswerAttemptWindowStart, Comment
                    FROM aspnet_Users", oldConn);

                using (SqlCeDataReader reader = oldCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string applicationName = reader.GetString(0);
                        Guid userId = reader.GetGuid(1);
                        string password = reader.GetString(2);
                        string email = reader.GetString(3);
                        string passwordQuestion = reader.GetString(4);
                        string passwordAnswer = reader.GetString(5);
                        bool isApproved = reader.GetBoolean(6);
                        bool isLockedOut = reader.GetBoolean(7);
                        DateTime creationDate = reader.GetDateTime(8);
                        DateTime lastLoginDate = reader.GetDateTime(9);
                        DateTime lastPasswordChangedDate = reader.GetDateTime(10);
                        DateTime lastLockedOutDate = reader.GetDateTime(11);
                        int failedPasswordAttemptCount = reader.GetInt32(12);
                        DateTime failedPasswordAttemptWindowsStart = reader.GetDateTime(13);
                        int failedPasswordAnswerAttemptCount = reader.GetInt32(14);
                        DateTime failedPasswordAnswerAttemptWindowStart = reader.GetDateTime(15);
                        string comment = reader.GetString(16);

                        newCmd = new SqlCeCommand(
                            @"INSERT INTO [aspnet_Membership]
                               ([ApplicationId]
                               ,[UserId]
                               ,[Password]
                               ,[PasswordFormat]
                               ,[PasswordSalt]
                               ,[Email]
                               ,[LoweredEmail]
                               ,[PasswordQuestion]
                               ,[PasswordAnswer]
                               ,[IsApproved]
                               ,[IsLockedOut]
                               ,[CreateDate]
                               ,[LastLoginDate]
                               ,[LastPasswordChangedDate]
                               ,[LastLockoutDate]
                               ,[FailedPasswordAttemptCount]
                               ,[FailedPasswordAttemptWindowStart]
                               ,[FailedPasswordAnswerAttemptCount]
                               ,[FailedPasswordAnswerAttemptWindowStart]
                               ,[Comment])
                             VALUES
                                   (@ApplicationId
                                   ,@UserId
                                   ,@Password
                                   ,@PasswordFormat
                                   ,@PasswordSalt
                                   ,@Email
                                   ,@LoweredEmail
                                   ,@PasswordQuestion
                                   ,@PasswordAnswer
                                   ,@IsApproved
                                   ,@IsLockedOut
                                   ,@CreateDate
                                   ,@LastLoginDate
                                   ,@LastPasswordChangedDate
                                   ,@LastLockoutDate
                                   ,@FailedPasswordAttemptCount
                                   ,@FailedPasswordAttemptWindowStart
                                   ,@FailedPasswordAnswerAttemptCount
                                   ,@FailedPasswordAnswerAttemptWindowStart
                                   ,@Comment)", newConn, newTrans);
                        newCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = apps[applicationName];
                        newCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                        newCmd.Parameters.Add("@Password", SqlDbType.NVarChar, 128).Value = password;
                        newCmd.Parameters.Add("@PasswordFormat", SqlDbType.Int).Value = PasswordFormat.GetHashCode();
                        newCmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 128).Value =
                            (PasswordFormat == MembershipPasswordFormat.Encrypted) ? String.Empty : encryptionKey;
                        newCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = email;
                        newCmd.Parameters.Add("@LoweredEmail", SqlDbType.NVarChar, 256).Value = email.ToLowerInvariant();
                        newCmd.Parameters.Add("@PasswordQuestion", SqlDbType.NVarChar, 256).Value = passwordQuestion;
                        newCmd.Parameters.Add("@PasswordAnswer", SqlDbType.NVarChar, 128).Value = passwordAnswer;
                        newCmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = isApproved;
                        newCmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = isLockedOut;
                        newCmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = creationDate;
                        newCmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = lastLoginDate;
                        newCmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = lastPasswordChangedDate;
                        newCmd.Parameters.Add("@LastLockoutDate", SqlDbType.DateTime).Value = lastLockedOutDate;
                        newCmd.Parameters.Add("@FailedPasswordAttemptCount", SqlDbType.Int).Value = failedPasswordAttemptCount;
                        newCmd.Parameters.Add("@FailedPasswordAttemptWindowStart", SqlDbType.DateTime).Value = failedPasswordAttemptWindowsStart;
                        newCmd.Parameters.Add("@FailedPasswordAnswerAttemptCount", SqlDbType.Int).Value = failedPasswordAnswerAttemptCount;
                        newCmd.Parameters.Add("@FailedPasswordAnswerAttemptWindowStart", SqlDbType.DateTime).Value = failedPasswordAnswerAttemptWindowStart;
                        newCmd.Parameters.Add("@Comment", SqlDbType.NText).Value = comment;

                        newCmd.ExecuteNonQuery();
                    }
                }



                /**********************************************************************************
                 * aspnet_Roles
                 *********************************************************************************/
                oldCmd = new SqlCeCommand(@"SELECT Rolename, ApplicationName FROM aspnet_Roles", oldConn);

                using (SqlCeDataReader reader = oldCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string roleName = reader.GetString(0);
                        string applicationName = reader.GetString(1);
                        Guid roleId = Guid.NewGuid();
                        roles.Add(applicationName + ":" + roleName, roleId);

                        newCmd = new SqlCeCommand(
                            @"INSERT INTO apsnet_Roles (ApplicationId, RoleId, RoleName, LoweredRoleName, Description)
                                VALUES (@ApplicationId, @RoleId @RoleName, @LoweredRoleName, NULL)", newConn, newTrans);

                        newCmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = apps[applicationName];
                        newCmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;
                        newCmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = roleName;
                        newCmd.Parameters.Add("@LoweredRoleName", SqlDbType.NVarChar, 256).Value = roleName.ToLowerInvariant();

                        newCmd.ExecuteNonQuery();
                    }
                }

                /**********************************************************************************
                 * aspnet_UsersInRoles
                 *********************************************************************************/
                oldCmd = new SqlCeCommand(
                    @"SELECT U.PKID, UR.Rolename, UR.ApplicationName 
                        FROM aspnet_UsersInRoles UR 
                            INNER JOIN aspnet_Users U ON UR.Username = U.UserName 
                                AND UR.ApplicationName = U.ApplicationName", oldConn);

                using (SqlCeDataReader reader = oldCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid userId = reader.GetGuid(0);
                        string roleName = reader.GetString(1);
                        string applicationName = reader.GetString(2);

                        newCmd = new SqlCeCommand(
                            @"INSERT INTO aspnet_UsersInRoles (UserId, RoleId)
                                VALUES (@UserId, @RoleId)", newConn, newTrans);

                        newCmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
                        newCmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roles[applicationName + ":" + roleName];

                        newCmd.ExecuteNonQuery();
                    }
                }

                newTrans.Commit();
            }
            catch (Exception ex)
            {
                if (newTrans != null)
                    newTrans.Rollback();

                throw ex;
            }
            finally
            {
                if (oldCmd != null)
                    oldCmd.Dispose();
                oldConn.Dispose();
                newConn.Dispose();
            }
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

        private void WriteToEventLog(Exception e, string action)
        {
            using (EventLog log = new EventLog())
            {
                log.Source = eventSource;
                log.Log = eventLog;

                string message = "An exception occurred communicating with the data source.\n\n";
                message += "Action: " + action + "\n\n";
                message += "Exception: " + e.ToString();
            }
        }
    }
}