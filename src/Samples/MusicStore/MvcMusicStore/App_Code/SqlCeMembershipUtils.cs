using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Data;

namespace ErikEJ
{
    public class SqlCeMembershipUtils
    {

        private static readonly object _lock = new object();
        public static void CreateDatabaseIfRequired(string connection, string applicationName)
        {
            lock (_lock)
            {
                SqlCeConnectionStringBuilder builder = new SqlCeConnectionStringBuilder();
                builder.ConnectionString = connection;
                
                string sdfPath = ReplaceDataDirectory(builder.DataSource);

                if (string.IsNullOrWhiteSpace(sdfPath))
                    return;

                if (!System.IO.File.Exists(sdfPath))
                {
                    //OK, try to create the database file
                    using (var engine = new SqlCeEngine(connection))
                    {
                        engine.CreateDatabase();
                    }
                }
                ValidateDatabase(connection, applicationName);

            }
        }

        private static string ReplaceDataDirectory(string inputString)
        {
            string str = inputString.Trim();
            if (string.IsNullOrEmpty(inputString) || !inputString.StartsWith("|DataDirectory|", StringComparison.InvariantCultureIgnoreCase))
            {
                return str;
            }
            string data = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(data))
            {
                data = AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory;
            }
            if (string.IsNullOrEmpty(data))
            {
                data = string.Empty;
            }
            int length = "|DataDirectory|".Length;
            if ((inputString.Length > "|DataDirectory|".Length) && ('\\' == inputString["|DataDirectory|".Length]))
            {
                length++;
            }
            return System.IO.Path.Combine(data, inputString.Substring(length));
        }

        private static void ValidateDatabase(string connection, string applicationName)
        {
            using (var conn = new SqlCeConnection(connection))
            {
                conn.Open();
                IList<string> tables = GetTableNames(conn);
                using (var cmd = new SqlCeCommand())
                {
                    cmd.Connection = conn;

                    if (!tables.Contains("aspnet_Applications"))
                        CreateApplications(cmd, applicationName);

                    if (!tables.Contains("aspnet_Membership"))
                        CreateUsers(cmd);

                    if (!tables.Contains("aspnet_Roles"))
                        CreateRoles(cmd);

                    if (!tables.Contains("aspnet_UsersInRoles"))
                        CreateUsersInRoles(cmd);
					
					// new table check
					if (!tables.Contains("aspnet_Profile"))
						CreateProfile(cmd);

                }
            }
        }

        private static void CreateApplications(SqlCeCommand cmd, string applicationName)
        {
            cmd.CommandText =
                @"CREATE TABLE [aspnet_Applications] (
                        [ApplicationName] nvarchar(256) NOT NULL
                    , [LoweredApplicationName] nvarchar(256) NOT NULL
                    , [ApplicationId] uniqueidentifier NOT NULL DEFAULT (newid()) PRIMARY KEY
                    , [Description] nvarchar(256) NULL
                    );
						";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE INDEX [aspnet_Applications_Index] ON [aspnet_Applications] ([LoweredApplicationName] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE UNIQUE INDEX [UQ__aspnet_A__3091033107020F21] ON [aspnet_Applications] ([ApplicationName] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"INSERT INTO [aspnet_Applications] ([ApplicationName], [LoweredApplicationName] ,[ApplicationId])
                    VALUES
                    (@ApplicationName
                    ,@LoweredApplicationName
                    ,@ApplicationId
                    );
            ";
            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = applicationName;
            cmd.Parameters.Add("@LoweredApplicationName", SqlDbType.NVarChar, 256).Value = applicationName.ToLowerInvariant();
            cmd.Parameters.Add("@ApplicationId", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            cmd.ExecuteNonQuery();

        }

        private static void CreateUsers(SqlCeCommand cmd)
        {

            cmd.CommandText =
                    @"CREATE TABLE [aspnet_Users] (
                      [ApplicationId] uniqueidentifier NOT NULL
                    , [UserId] uniqueidentifier NOT NULL DEFAULT (newid()) PRIMARY KEY
                    , [UserName] nvarchar(256) NOT NULL
                    , [LoweredUserName] nvarchar(256) NOT NULL
                    , [MobileAlias] nvarchar(16) NULL DEFAULT (NULL)
                    , [IsAnonymous] bit NOT NULL DEFAULT ((0))
                    , [LastActivityDate] datetime NOT NULL
                    );
                    ";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE UNIQUE INDEX [aspnet_Users_Index] ON [aspnet_Users] ([ApplicationId] ASC,[LoweredUserName] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE INDEX [aspnet_Users_Index2] ON [aspnet_Users] ([ApplicationId] ASC,[LastActivityDate] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_Users] ADD CONSTRAINT [FK__aspnet_Us__Appli__0DAF0CB0] FOREIGN KEY ([ApplicationId]) REFERENCES [aspnet_Applications]([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE TABLE [aspnet_Membership] (
                      [ApplicationId] uniqueidentifier NOT NULL
                    , [UserId] uniqueidentifier NOT NULL
                    , [Password] nvarchar(128) NOT NULL
                    , [PasswordFormat] int NOT NULL DEFAULT ((0))
                    , [PasswordSalt] nvarchar(128) NOT NULL
                    , [MobilePIN] nvarchar(16) NULL
                    , [Email] nvarchar(256) NULL
                    , [LoweredEmail] nvarchar(256) NULL
                    , [PasswordQuestion] nvarchar(256) NULL
                    , [PasswordAnswer] nvarchar(128) NULL
                    , [IsApproved] bit NOT NULL
                    , [IsLockedOut] bit NOT NULL
                    , [CreateDate] datetime NOT NULL
                    , [LastLoginDate] datetime NOT NULL
                    , [LastPasswordChangedDate] datetime NOT NULL
                    , [LastLockoutDate] datetime NOT NULL
                    , [FailedPasswordAttemptCount] int NOT NULL
                    , [FailedPasswordAttemptWindowStart] datetime NOT NULL
                    , [FailedPasswordAnswerAttemptCount] int NOT NULL
                    , [FailedPasswordAnswerAttemptWindowStart] datetime NOT NULL
                    , [Comment] ntext NULL
                    );	";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE INDEX [aspnet_Membership_index] ON [aspnet_Membership] ([ApplicationId] ASC,[LoweredEmail] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__Appli__21B6055D] FOREIGN KEY ([ApplicationId]) REFERENCES [aspnet_Applications]([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__UserI__22AA2996] FOREIGN KEY ([UserId]) REFERENCES [aspnet_Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

        }

        private static void CreateUsersInRoles(SqlCeCommand cmd)
        {
            cmd.CommandText =
                @"CREATE TABLE [aspnet_UsersInRoles] (
                  [UserId] uniqueidentifier NOT NULL
                , [RoleId] uniqueidentifier NOT NULL
                );
	                ";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_UsersInRoles] ADD CONSTRAINT [PK__aspnet_U__AF2760AD3C69FB99] PRIMARY KEY ([RoleId],[UserId]);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE INDEX [aspnet_UsersInRoles_index] ON [aspnet_UsersInRoles] ([RoleId] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_UsersInRoles] ADD CONSTRAINT [FK__aspnet_Us__RoleI__3F466844] FOREIGN KEY ([RoleId]) REFERENCES [aspnet_Roles]([RoleId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_UsersInRoles] ADD CONSTRAINT [FK__aspnet_Us__UserI__3E52440B] FOREIGN KEY ([UserId]) REFERENCES [aspnet_Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

        }

        private static void CreateProfile(SqlCeCommand cmd)
        {
            cmd.CommandText =
                 @"	CREATE TABLE aspnet_Profile (
						UserId uniqueidentifier NOT NULL,
						PropertyNames ntext NOT NULL,
						PropertyValuesString ntext NOT NULL,
						PropertyValuesBinary image NOT NULL,
						LastUpdatedDate datetime NOT NULL
						)";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                 @"ALTER TABLE [aspnet_Profile] ADD CONSTRAINT [PK__aspnet_Profile] PRIMARY KEY ([UserId]);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                 @"ALTER TABLE [aspnet_Profile] ADD CONSTRAINT [Profile_Users] FOREIGN KEY ([UserId]) REFERENCES [aspnet_Users]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION";
            cmd.ExecuteNonQuery();
        }

        private static void CreateRoles(SqlCeCommand cmd)
        {
            cmd.CommandText =
                @"CREATE TABLE [aspnet_Roles] (
                  [ApplicationId] uniqueidentifier NOT NULL
                , [RoleId] uniqueidentifier NOT NULL PRIMARY KEY DEFAULT (newid())
                , [RoleName] nvarchar(256) NOT NULL
                , [LoweredRoleName] nvarchar(256) NOT NULL
                , [Description] nvarchar(256) NULL
                );
            	";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"CREATE UNIQUE INDEX [aspnet_Roles_index1] ON [aspnet_Roles] ([ApplicationId] ASC,[LoweredRoleName] ASC);";
            cmd.ExecuteNonQuery();

            cmd.CommandText =
                @"ALTER TABLE [aspnet_Roles] ADD CONSTRAINT [FK__aspnet_Ro__Appli__38996AB5] FOREIGN KEY ([ApplicationId]) REFERENCES [aspnet_Applications]([ApplicationId]) ON DELETE NO ACTION ON UPDATE NO ACTION;";
            cmd.ExecuteNonQuery();

        }

        public static Guid GetApplicationId(string connectionString, string applicationName)
        {
            using (SqlCeConnection conn = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT ApplicationId FROM [aspnet_Applications] " +
                          "WHERE ApplicationName = @ApplicationName", conn))
                {
                    cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = applicationName;

                    conn.Open();
                    var applicationId = cmd.ExecuteScalar();
                    if (applicationId == null)
                    {
                        throw new System.Configuration.Provider.ProviderException("Unable to find application id for provided application name: " + applicationName);
                    }
                    return (Guid)(applicationId);

                }
            }
        }

        private static IList<string> GetTableNames(SqlCeConnection testConn)
        {
            var dt = testConn.GetSchema("Tables");
            IList<string> tables = new List<string>();
            foreach (DataRow r in dt.Rows)
            {
                tables.Add(r["TABLE_NAME"].ToString());
            }
            return tables;
        }
    }
}
