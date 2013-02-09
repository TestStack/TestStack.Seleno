using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;


namespace ErikEJ
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ce")]
    public sealed class SqlCeRoleProvider : RoleProvider
    {

        //
        // Global connection string, generic exception message, event log info.
        //

        private string eventSource = "SqlCeRoleProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        private ConnectionStringSettings pConnectionStringSettings;
        private string connectionString;
        private Guid pApplicationId;


        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog = false;

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
                name = "SqlCeRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SqlCe Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);


            if (string.IsNullOrWhiteSpace(config["applicationName"]))
            {
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                pApplicationName = config["applicationName"];
            }


            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpperInvariant() == "TRUE")
                {
                    pWriteExceptionsToEventLog = true;
                }
            }


            //
            // Initialize SqlCeConnection.
            //

            pConnectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (pConnectionStringSettings == null || string.IsNullOrWhiteSpace(pConnectionStringSettings.ConnectionString))
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = pConnectionStringSettings.ConnectionString;

            SqlCeMembershipUtils.CreateDatabaseIfRequired(connectionString, ApplicationName);
            pApplicationId = SqlCeMembershipUtils.GetApplicationId(connectionString, pApplicationName);
        }

        //
        // System.Web.Security.RoleProvider properties.
        //

        private string pApplicationName;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        //
        // System.Web.Security.RoleProvider methods.
        //

        //
        // RoleProvider.AddUsersToRoles
        //

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                if (username.IndexOf(',') > 0)
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }

                foreach (string rolename in roleNames)
                {
                    if (IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("INSERT INTO [aspnet_UsersInRoles] " +
                        " (UserId, RoleId) " +
                        " Values(@UserId, @RoleId)", conn))
                {
                    SqlCeParameter userParm = cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier);
                    SqlCeParameter roleParm = cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier);

                    SqlCeTransaction tran = null;

                    try
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        foreach (string username in usernames)
                        {
                            foreach (string rolename in roleNames)
                            {
                                userParm.Value = GetUserId(username);
                                roleParm.Value = GetRoleId(rolename);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch (SqlCeException e)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch { }


                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "AddUsersToRoles");
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
        // RoleProvider.CreateRole
        //

        public override void CreateRole(string roleName)
        {
            if (roleName.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(roleName))
            {
                throw new ProviderException("Role name already exists.");
            }

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("INSERT INTO [aspnet_Roles] " +
                        " (Rolename, LoweredRoleName, ApplicationId) " +
                        " Values(@Rolename, @LoweredRoleName, @ApplicationId)", conn))
                {
                    cmd.Parameters.Add("@Rolename", SqlDbType.NVarChar, 256).Value = roleName;
                    cmd.Parameters.Add("@LoweredRolename", SqlDbType.NVarChar, 256).Value = roleName.ToLowerInvariant();
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
                            WriteToEventLog(e, "CreateRole");
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
        // RoleProvider.DeleteRole
        //

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (!RoleExists(roleName))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("DELETE FROM [aspnet_Roles] " +
                        " WHERE RoleId = @RoleId AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = GetRoleId(roleName);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;


                    using (SqlCeCommand cmd2 = new SqlCeCommand("DELETE FROM [aspnet_UsersInRoles]" +
                            " WHERE RoleId = @RoleId", conn))
                    {

                        cmd2.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = GetRoleId(roleName);

                        SqlCeTransaction tran = null;

                        try
                        {
                            conn.Open();
                            tran = conn.BeginTransaction();
                            cmd.Transaction = tran;
                            cmd2.Transaction = tran;

                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();

                            tran.Commit();
                        }
                        catch (SqlCeException e)
                        {
                            try
                            {
                                tran.Rollback();
                            }
                            catch { }


                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(e, "DeleteRole");

                                return false;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return true;
        }


        //
        // RoleProvider.GetAllRoles
        //

        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Rolename FROM [aspnet_Roles]" +
                          " WHERE ApplicationId = @ApplicationId", conn))
                {

                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    SqlCeDataReader reader = null;

                    try
                    {
                        conn.Open();

                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            tmpRoleNames += reader.GetString(0) + ",";
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetAllRoles");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.GetRolesForUser
        //

        public override string[] GetRolesForUser(string username)
        {
            string tmpRoleNames = "";

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Rolename FROM [aspnet_Roles] r, [aspnet_UsersInRoles] ur " +
                        " WHERE r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId and ur.UserId = @UserId ORDER BY RoleName", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = GetUserId(username);
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmpRoleNames += reader.GetString(0) + ",";
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "GetRolesForUser");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.GetUsersInRole
        //

        public override string[] GetUsersInRole(string roleName)
        {
            string tmpUserNames = "";

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                        using (SqlCeCommand cmd = new SqlCeCommand(
                            @" SELECT u.UserName
                                FROM   aspnet_Users u, aspnet_UsersInRoles ur
                                WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
                                ORDER BY u.UserName", conn))
                                                    {
                            cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                        
                            try
                            {
                                conn.Open();
                                Guid roleId = GetRoleId(roleName);
                                cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;

                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        tmpUserNames += reader.GetString(0) + ",";
                                    }
                                }
                            }

                            catch (SqlCeException e)
                            {
                                if (WriteExceptionsToEventLog)
                                {
                                    WriteToEventLog(e, "GetUsersInRole");
                                }
                                else
                                {
                                    throw;
                                }
                            }

            
                }
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }


        //
        // RoleProvider.IsUserInRole
        //

        public override bool IsUserInRole(string username, string roleName)
        {
            bool userIsInRole = false;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"SELECT COUNT(*) FROM [aspnet_UsersInRoles], [aspnet_Users], [aspnet_Roles]
                    WHERE [aspnet_UsersInRoles].UserId = [aspnet_Users].UserId AND [aspnet_UsersInRoles].RoleId = aspnet_Roles.RoleId
                    AND [aspnet_Users].Username = @Username AND [aspnet_Roles].Rolename = @Rolename", conn))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value = username;
                    cmd.Parameters.Add("@Rolename", SqlDbType.NVarChar, 256).Value = roleName;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        int numRecs = (int)cmd.ExecuteScalar();

                        if (numRecs > 0)
                        {
                            userIsInRole = true;
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "IsUserInRole");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return userIsInRole;
        }


        //
        // RoleProvider.RemoveUsersFromRoles
        //

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string rolename in roleNames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in roleNames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("DELETE FROM [aspnet_UsersInRoles]" +
                        " WHERE UserId = @UserId AND RoleId = @RoleId", conn))
                {

                    SqlCeParameter userParm = cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier);
                    SqlCeParameter roleParm = cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier);

                    SqlCeTransaction tran = null;

                    try
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        cmd.Transaction = tran;

                        foreach (string username in usernames)
                        {
                            foreach (string rolename in roleNames)
                            {
                                userParm.Value = GetUserId(username);
                                roleParm.Value = GetRoleId(rolename);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch (SqlCeException e)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch { }


                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "RemoveUsersFromRoles");
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
        // RoleProvider.RoleExists
        //

        public override bool RoleExists(string roleName)
        {
            bool exists = false;

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT COUNT(*) FROM [aspnet_Roles]" +
                          " WHERE Rolename = @Rolename AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@Rolename", SqlDbType.NVarChar, 256).Value = roleName;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();

                        int numRecs = (int)cmd.ExecuteScalar();

                        if (numRecs > 0)
                        {
                            exists = true;
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "RoleExists");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return exists;
        }

        //
        // RoleProvider.FindUsersInRole
        //

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            string tmpUserNames = "";

            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT Username FROM [aspnet_UsersInRoles]" +
                          "WHERE Username LIKE @UsernameSearch AND Rolename = @Rolename AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@UsernameSearch", SqlDbType.NVarChar, 256).Value = usernameToMatch;
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = roleName;
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;

                    try
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmpUserNames += reader.GetString(0) + ",";
                            }
                        }
                    }
                    catch (SqlCeException e)
                    {
                        if (WriteExceptionsToEventLog)
                        {
                            WriteToEventLog(e, "FindUsersInRole");
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

        private Guid GetRoleId(string roleName)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"SELECT  RoleId
                                FROM  aspnet_Roles
                                WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId", conn))
                {
                    cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = pApplicationId;
                    cmd.Parameters.Add("@Rolename", SqlDbType.NVarChar, 256).Value = roleName;
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
                                WriteToEventLog(e, "GetRoleId");
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

        private void WriteToEventLog(SqlCeException e, string action)
        {
            using (EventLog log = new EventLog())
            {
                log.Source = eventSource;
                log.Log = eventLog;

                string message = exceptionMessage + "\n\n";
                message += "Action: " + action + "\n\n";
                message += "Exception: " + e.ToString();

                log.WriteEntry(message);
            }
        }

    }
}