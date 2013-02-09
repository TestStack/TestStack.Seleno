using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlServerCe;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Profile;

namespace ErikEJ
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ce")]
    public class SqlCeProfileProvider : ProfileProvider
    {
        private string connectionString;
        private Guid pApplicationId;

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (name == null || name.Length < 1)
                name = "SqlCeProfileProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SqlCeProfileProvider");
            }

            base.Initialize(name, config);

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

            ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

            SqlCeMembershipUtils.CreateDatabaseIfRequired(connectionString, ApplicationName);

            pApplicationId = SqlCeMembershipUtils.GetApplicationId(connectionString, ApplicationName);

            config.Remove("connectionStringName");
            config.Remove("applicationName");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(SR.GetString(SR.Provider_unrecognized_attribute, attribUnrecognized));
            }
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override string ApplicationName { get; set; }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException("profiles");
            }

            if (profiles.Count < 1)
            {
                throw new ArgumentException("The collection parameter 'profiles' should not be empty.", "profiles");
            }

            string[] usernames = new string[profiles.Count];

            int iter = 0;
            foreach (ProfileInfo profile in profiles)
            {
                usernames[iter++] = profile.UserName;
            }

            return DeleteProfiles(usernames);
        }

        public override int DeleteProfiles(string[] usernames)
        {
            int rowsAffected = 0;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("DELETE FROM aspnet_Profile WHERE @UserId = UserId", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier);

                    SqlCeTransaction tran = null;

                    try
                    {
                        conn.Open();

                        tran = conn.BeginTransaction();

                        cmd.Transaction = tran;

                        foreach (string username in usernames)
                        {
                            cmd.Parameters[0].Value = GetUserId(username);

                            rowsAffected += cmd.ExecuteNonQuery();
                        }

                        tran.Commit();

                    }
                    catch (SqlCeException)
                    {
                        throw;
                    }
                    finally
                    {
                        if (tran != null)
                            tran.Rollback();
                    }
                }
            }
            return rowsAffected;
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                conn.Open();

                SqlCeDataReader reader;

                List<string> userids = new List<string>();

                using (SqlCeCommand cmd = new SqlCeCommand("SELECT UserId FROM aspnet_Users u WHERE ApplicationId = @ApplicationId AND LastActivityDate <= @InactiveSinceDate AND (@ProfileAuthOptions = 2 OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1) OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0))", conn))
                {
                    cmd.Parameters.Add(CreateInputParam("@ApplicationId", SqlDbType.UniqueIdentifier, SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName)));
                    cmd.Parameters.Add(CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption));
                    cmd.Parameters.Add(CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime()));

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        userids.Add("'" + ((Guid)reader["UserId"]).ToString() + "'");
                    }
                }

                using (SqlCeCommand cmd = new SqlCeCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM aspnet_Profile WHERE UserId IN (" + string.Join(",", userids.ToArray()) + ")";

                    object o = cmd.ExecuteScalar();
                    if (o == null || !(o is int))
                        return 0;
                    return (int)o;
                }
            }
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                conn.Open();

                SqlCeDataReader reader;

                List<string> userids = new List<string>();

                using (SqlCeCommand cmd = new SqlCeCommand("SELECT UserId FROM aspnet_Users u WHERE ApplicationId = @ApplicationId AND LastActivityDate <= @InactiveSinceDate AND (@ProfileAuthOptions = 2 OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1) OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0))", conn))
                {
                    cmd.Parameters.Add(CreateInputParam("@ApplicationId", SqlDbType.UniqueIdentifier, SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName)));
                    cmd.Parameters.Add(CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption));
                    cmd.Parameters.Add(CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime()));

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        userids.Add("'" + ((Guid)reader["UserId"]).ToString() + "'");
                    }
                }

                using (SqlCeCommand cmd = new SqlCeCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT COUNT(UserId) FROM aspnet_Profile WHERE UserId IN (" + string.Join(",", userids.ToArray()) + ")";

                    object o = cmd.ExecuteScalar();
                    if (o == null || !(o is int))
                        return 0;
                    return (int)o;
                }
            }
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            return GetProfilesForQuery(new SqlCeParameter[0], authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            SqlCeParameter[] args = new SqlCeParameter[1];
            args[0] = CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime());
            return GetProfilesForQuery(args, authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            SqlCeParameter[] args = new SqlCeParameter[1];
            args[0] = CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch);
            return GetProfilesForQuery(args, authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            SqlCeParameter[] args = new SqlCeParameter[2];
            args[0] = CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch);
            args[1] = CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime());
            return GetProfilesForQuery(args, authenticationOption, pageIndex, pageSize, out totalRecords);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext sc, SettingsPropertyCollection properties)
        {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            if (properties.Count < 1)
                return svc;

            string username = (string)sc["UserName"];

            foreach (SettingsProperty prop in properties)
            {
                if (prop.SerializeAs == SettingsSerializeAs.ProviderSpecific)
                    if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(string))
                        prop.SerializeAs = SettingsSerializeAs.String;
                    else
                        prop.SerializeAs = SettingsSerializeAs.Xml;

                svc.Add(new SettingsPropertyValue(prop));
            }
            if (!String.IsNullOrEmpty(username))
                GetPropertyValuesFromDatabase(username, svc);
            return svc;
        }

        public override void SetPropertyValues(SettingsContext sc, SettingsPropertyValueCollection properties)
        {
            string username = (string)sc["UserName"];
            bool userIsAuthenticated = (bool)sc["IsAuthenticated"];

            if (username == null || username.Length < 1 || properties.Count < 1)
                return;

            string names = String.Empty;
            string values = String.Empty;
            byte[] buf = null;

            PrepareDataForSaving(ref names, ref values, ref buf, true, properties, userIsAuthenticated);
            if (names.Length == 0)
                return;

            Guid userId = Guid.Empty;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {

                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Count(UserId) FROM aspnet_Users WHERE ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)", conn))
                {
                    conn.Open();

                    cmd.Parameters.Add(CreateInputParam("@ApplicationId", SqlDbType.UniqueIdentifier, SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName)));
                    cmd.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));

                    int userCount = (int)cmd.ExecuteScalar();
                    if (userCount.Equals(0))
                    {
                        // create user: pinched from SqlCeMembershipProvider.CreateUser method
                        userId = Guid.NewGuid();

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
                            cmd2.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName);
                            cmd2.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = (Guid)userId;
                            cmd2.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = username;
                            cmd2.Parameters.Add("@LoweredUserName", SqlDbType.NVarChar, 256).Value = username.ToLowerInvariant();
                            cmd2.Parameters.Add("@IsAnonymous", SqlDbType.Bit).Value = !userIsAuthenticated;
                            cmd2.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = DateTime.UtcNow;

                            int rowsAffected = cmd2.ExecuteNonQuery();

                            if (!rowsAffected.Equals(1))
                            {
                                // oops!	
                            }
                        }
                    }
                    else if (userCount > 1)
                    {
                        throw new Exception(string.Format("Duplicate user records found for username '{0}' and application '{1}'", username, ApplicationName));
                    }

                    cmd.CommandText = "SELECT UserId FROM aspnet_Users WHERE ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)";
                    userId = (Guid)cmd.ExecuteScalar();
                }

                using (SqlCeCommand cmd = new SqlCeCommand("UPDATE aspnet_Users SET LastActivityDate=@CurrentTimeUtc WHERE UserId = @UserId", conn))
                {
                    cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                    cmd.Parameters.Add(CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
                    cmd.ExecuteNonQuery();
                }

                int profileCount = 0;

                using (SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(UserId) FROM aspnet_Profile WHERE UserId = @UserId", conn))
                {
                    cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));

                    object o = cmd.ExecuteScalar();
                    profileCount = (int)o;
                }

                switch (profileCount)
                {
                    case 0:
                        using (SqlCeCommand cmd = new SqlCeCommand("INSERT INTO aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate) VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)", conn))
                        {
                            cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                            cmd.Parameters.Add(CreateInputParam("@PropertyNames", SqlDbType.NText, names));
                            cmd.Parameters.Add(CreateInputParam("@PropertyValuesString", SqlDbType.NText, values));
                            cmd.Parameters.Add(CreateInputParam("@PropertyValuesBinary", SqlDbType.Image, buf));
                            cmd.Parameters.Add(CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
                            cmd.ExecuteNonQuery();
                        }
                        break;
                    case 1:
                        using (SqlCeCommand cmd = new SqlCeCommand("UPDATE aspnet_Profile SET PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString, PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc WHERE UserId = @UserId", conn))
                        {
                            cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                            cmd.Parameters.Add(CreateInputParam("@PropertyNames", SqlDbType.NText, names));
                            cmd.Parameters.Add(CreateInputParam("@PropertyValuesString", SqlDbType.NText, values));
                            cmd.Parameters.Add(CreateInputParam("@PropertyValuesBinary", SqlDbType.Image, buf));
                            cmd.Parameters.Add(CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
                            cmd.ExecuteNonQuery();
                        }
                        break;
                }
            }
        }

        private SqlCeParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlCeParameter param = new SqlCeParameter(paramName, dbType);
            if (objValue == null)
                objValue = String.Empty;
            param.Value = objValue;
            return param;
        }

        private static void ParseDataFromDB(string[] names, string values, byte[] buf, SettingsPropertyValueCollection properties)
        {
            if (names == null || values == null || buf == null || properties == null)
                return;
            try
            {
                for (int iter = 0; iter < names.Length / 4; iter++)
                {
                    string name = names[iter * 4];
                    SettingsPropertyValue pp = properties[name];

                    if (pp == null) // property not found
                        continue;

                    int startPos = Int32.Parse(names[iter * 4 + 2], CultureInfo.InvariantCulture);
                    int length = Int32.Parse(names[iter * 4 + 3], CultureInfo.InvariantCulture);

                    if (length == -1 && !pp.Property.PropertyType.IsValueType) // Null Value
                    {
                        pp.PropertyValue = null;
                        pp.IsDirty = false;
                        pp.Deserialized = true;
                    }
                    if (names[iter * 4 + 1] == "S" && startPos >= 0 && length > 0 && values.Length >= startPos + length)
                    {
                        pp.SerializedValue = values.Substring(startPos, length);
                    }

                    if (names[iter * 4 + 1] == "B" && startPos >= 0 && length > 0 && buf.Length >= startPos + length)
                    {
                        byte[] buf2 = new byte[length];

                        Buffer.BlockCopy(buf, startPos, buf2, 0, length);
                        pp.SerializedValue = buf2;
                    }
                }
            }
            catch
            { // Eat exceptions
            }
        }

        private static void PrepareDataForSaving(ref string allNames, ref string allValues, ref byte[] buf, bool binarySupported, SettingsPropertyValueCollection properties, bool userIsAuthenticated)
        {
            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            MemoryStream ms = (binarySupported ? new System.IO.MemoryStream() : null);
            try
            {
                try
                {
                    bool anyItemsToSave = false;

                    foreach (SettingsPropertyValue pp in properties)
                    {
                        if (pp.IsDirty)
                        {
                            if (!userIsAuthenticated)
                            {
                                bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                                if (!allowAnonymous)
                                    continue;
                            }
                            anyItemsToSave = true;
                            break;
                        }
                    }

                    if (!anyItemsToSave)
                        return;

                    foreach (SettingsPropertyValue pp in properties)
                    {
                        if (!userIsAuthenticated)
                        {
                            bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                            if (!allowAnonymous)
                                continue;
                        }

                        if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                            continue;

                        int len = 0, startPos = 0;
                        string propValue = null;

                        if (pp.Deserialized && pp.PropertyValue == null) // is value null?
                        {
                            len = -1;
                        }
                        else
                        {
                            object sVal = pp.SerializedValue;

                            if (sVal == null)
                            {
                                len = -1;
                            }
                            else
                            {
                                if (!(sVal is string) && !binarySupported)
                                {
                                    sVal = Convert.ToBase64String((byte[])sVal);
                                }

                                if (sVal is string)
                                {
                                    propValue = (string)sVal;
                                    len = propValue.Length;
                                    startPos = values.Length;
                                }
                                else
                                {
                                    byte[] b2 = (byte[])sVal;
                                    startPos = (int)ms.Position;
                                    ms.Write(b2, 0, b2.Length);
                                    ms.Position = startPos + b2.Length;
                                    len = b2.Length;
                                }
                            }
                        }

                        names.Append(pp.Name + ":" + ((propValue != null) ? "S" : "B") +
                                         ":" + startPos.ToString(CultureInfo.InvariantCulture) + ":" + len.ToString(CultureInfo.InvariantCulture) + ":");
                        if (propValue != null)
                            values.Append(propValue);
                    }

                    if (binarySupported)
                    {
                        buf = ms.ToArray();
                    }
                }
                finally
                {
                    if (ms != null)
                        ms.Close();
                }
            }
            catch
            {
                throw;
            }
            allNames = names.ToString();
            allValues = values.ToString();
        }

        private ProfileInfoCollection GetProfilesForQuery(SqlCeParameter[] args, ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("PageIndex must be greater than -1", "pageIndex");
            if (pageSize < 1)
                throw new ArgumentException("PageSize must be greater than 0", "pageSize");

            long lowerBound = pageIndex * pageSize;
            long upperBound = (long)pageSize - 1 + lowerBound;

            if (upperBound > Int32.MaxValue)
            {
                throw new ArgumentException("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32.", "pageIndex and pageSize");
            }

            SqlCeDataReader reader = null;
            totalRecords = 0;

            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCeCommand cmd = new SqlCeCommand(
                            @"
							SELECT
								aspnet_Users.UserName,
								aspnet_Users.IsAnonymous,
								aspnet_Users.LastActivityDate,
								aspnet_Profile.LastUpdatedDate,
								DATALENGTH(aspnet_Profile.PropertyNames) + DATALENGTH(aspnet_Profile.PropertyValuesString) + DATALENGTH(aspnet_Profile.PropertyValuesBinary)
							FROM
								aspnet_Users,
								aspnet_Profile
							WHERE
								aspnet_Users.UserId = aspnet_Profile.UserId
								AND ApplicationId = @ApplicationId
								AND
									(
									@ProfileAuthOptions = 2
									OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
									OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
									)"
                                , conn))
                    {
                        cmd.Parameters.Add(CreateInputParam("@ApplicationId", SqlDbType.UniqueIdentifier, SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName)));
                        cmd.Parameters.Add(CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption));
                        cmd.Parameters.Add(CreateInputParam("@PageLowerBound", SqlDbType.Int, lowerBound));
                        cmd.Parameters.Add(CreateInputParam("@PageSize", SqlDbType.Int, pageSize));

                        foreach (SqlCeParameter arg in args)
                        {
                            cmd.Parameters.Add(arg);
                            switch (arg.ParameterName)
                            {
                                case "@InactiveSinceDate":
                                    cmd.CommandText += " AND (@InactiveSinceDate IS NULL OR aspnet_Users.LastActivityDate <= @InactiveSinceDate)";
                                    break;
                                case "@UserNameToMatch":
                                    cmd.CommandText += " AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))";
                                    break;
                            }
                        }

                        // append paging 
                        cmd.CommandText += " ORDER BY aspnet_Users.Username	OFFSET @PageLowerBound ROWS FETCH NEXT @PageSize ROWS ONLY";

                        reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                        ProfileInfoCollection profiles = new ProfileInfoCollection();

                        while (reader.Read())
                        {
                            string username;
                            DateTime dtLastActivity, dtLastUpdated;
                            bool isAnon;

                            username = reader.GetString(0);
                            isAnon = reader.GetBoolean(1);
                            dtLastActivity = DateTime.SpecifyKind(reader.GetDateTime(2), DateTimeKind.Utc);
                            dtLastUpdated = DateTime.SpecifyKind(reader.GetDateTime(3), DateTimeKind.Utc);
                            int size = reader.GetInt32(4);
                            profiles.Add(new ProfileInfo(username, isAnon, dtLastActivity, dtLastUpdated, size));
                        }

                        totalRecords = profiles.Count;
                        if (reader.NextResult())
                            if (reader.Read())
                                totalRecords = reader.GetInt32(0);

                        return profiles;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetPropertyValuesFromDatabase(string userName, SettingsPropertyValueCollection svc)
        {
            HttpContext context = HttpContext.Current;
            string[] names = null;
            string values = null;
            byte[] buf = null;
            string sName = null;

            if (context != null)
                sName = (context.Request.IsAuthenticated ? context.User.Identity.Name : context.Request.AnonymousID);

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                Guid userId;

                using (SqlCeCommand cmd = new SqlCeCommand("SELECT ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName", conn))
                {
                    conn.Open();

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(CreateInputParam("@ApplicationId", SqlDbType.UniqueIdentifier, SqlCeMembershipUtils.GetApplicationId(conn.ConnectionString, ApplicationName)));
                    cmd.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, userName));
                    cmd.CommandText = "SELECT UserId FROM aspnet_Users WHERE ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)";

                    object o = cmd.ExecuteScalar();
                    if (o != null && o is Guid)
                    {
                        userId = (Guid)o;
                    }
                    else
                    {
                        return;
                    }

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                    cmd.CommandText = "SELECT PropertyNames, PropertyValuesString, PropertyValuesBinary FROM aspnet_Profile WHERE UserId = @UserId";

                    using (SqlCeDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {

                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
                        cmd.Parameters.Add(CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, userId));
                        cmd.CommandText = "UPDATE aspnet_Users SET LastActivityDate = @CurrentTimeUtc WHERE UserId = @UserId";

                        cmd.ExecuteNonQuery();

                        if (reader.Read())
                        {
                            names = reader.GetString(0).Split(':');
                            values = reader.GetString(1);

                            int size = (int)reader.GetBytes(2, 0, null, 0, 0);

                            buf = new byte[size];
                            reader.GetBytes(2, 0, buf, 0, size);
                        }
                        ParseDataFromDB(names, values, buf, svc);
                    }
                }
            }
        }

        #region Functions 'borrowed' from SqlCeMembershipProvider

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
                        conn.Open();
                        Guid? result = cmd.ExecuteScalar() as Guid?;
                        if (result.HasValue)
                            return result.Value;
                    }
                }
            }
            return Guid.Empty;
        }

        #endregion
    }

    internal class SR
    {
        internal static string GetString(string strString)
        {
            return strString;
        }
        internal static string GetString(string strString, string param1)
        {
            return string.Format(strString, param1);
        }

        internal static string GetString(string strString, string param1, string param2)
        {
            return string.Format(strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, string param2, string param3)
        {
            return string.Format(strString, param1, param2, param3);
        }

        internal const string Provider_application_name_too_long = "The application name is too long.";
        internal const string Provider_bad_password_format = "Password format specified is invalid.";
        internal const string Provider_can_not_retrieve_hashed_password = "Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set supportsPasswordRetrieval to false.";
        internal const string Provider_Error = "The Provider encountered an unknown error.";
        internal const string Provider_Not_Found = "Provider '{0}' was not found.";
        internal const string Provider_role_already_exists = "The role '{0}' already exists.";
        internal const string Provider_role_not_found = "The role '{0}' was not found.";
        internal const string Provider_Schema_Version_Not_Match = "The '{0}' requires a database schema compatible with schema version '{1}'.  However, the current database schema is not compatible with this version.  You may need to either install a compatible schema with aspnet_regsql.exe (available in the framework installation directory), or upgrade the provider to a newer version.";
        internal const string Provider_this_user_already_in_role = "The user '{0}' is already in role '{1}'.";
        internal const string Provider_this_user_not_found = "The user '{0}' was not found.";
        internal const string Provider_unknown_failure = "Stored procedure call failed.";
        internal const string Provider_unrecognized_attribute = "Attribute not recognized '{0}'";
        internal const string Provider_user_not_found = "The user was not found in the database.";
        internal const string Parameter_array_empty = "The array parameter '{0}' should not be empty.";
        internal const string Parameter_can_not_be_empty = "The parameter '{0}' must not be empty.";
        internal const string Parameter_can_not_contain_comma = "The parameter '{0}' must not contain commas.";
        internal const string Parameter_duplicate_array_element = "The array '{0}' should not contain duplicate values.";
        internal const string Parameter_too_long = "The parameter '{0}' is too long: it must not exceed {1} chars in length.";
        internal const string PageIndex_PageSize_bad = "The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32.";
        internal const string PageSize_bad = "The pageSize must be greater than zero.";
        internal const string PageIndex_bad = "The pageIndex must be greater than or equal to zero.";
    }
}




