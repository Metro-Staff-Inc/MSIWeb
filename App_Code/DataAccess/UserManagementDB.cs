using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;

namespace MSI.Web.MSINet.DataAccess
{
    public class UserManagementDB
    {
        private DataAccessHelper _dbHelper = new DataAccessHelper();

        #region User Operations

        public List<UserManagementInfo> GetAllUsers()
        {
            List<UserManagementInfo> users = new List<UserManagementInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("aspnet_Membership_GetAllUsers");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@ApplicationName", DbType.String, "ETicket");
            dbSvc.AddInParameter(cmd, "@PageIndex", DbType.Int32, 0);
            dbSvc.AddInParameter(cmd, "@PageSize", DbType.Int32, 1000); // Adjust as needed

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    while (dr.Read())
                    {
                        UserManagementInfo user = new UserManagementInfo
                        {
                            UserName = dr.GetString(dr.GetOrdinal("UserName")),
                            Email = dr.GetString(dr.GetOrdinal("Email")),
                            IsApproved = dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                            IsLockedOut = dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                            CreateDate = dr.GetDateTime(dr.GetOrdinal("CreateDate")),
                            LastLoginDate = dr.GetDateTime(dr.GetOrdinal("LastLoginDate")),
                            LastActivityDate = dr.GetDateTime(dr.GetOrdinal("LastActivityDate")),
                            UserId = dr.GetGuid(dr.GetOrdinal("UserId"))
                        };
                        users.Add(user);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return users;
        }

        public UserManagementInfo GetUserByName(string username)
        {
            UserManagementInfo user = null;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("aspnet_Membership_GetUserByName");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@ApplicationName", DbType.String, "ETicket");
            dbSvc.AddInParameter(cmd, "@UserName", DbType.String, username);
            dbSvc.AddInParameter(cmd, "@CurrentTimeUtc", DbType.DateTime, DateTime.UtcNow);
            dbSvc.AddInParameter(cmd, "@UpdateLastActivity", DbType.Boolean, false);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    if (dr.Read())
                    {
                        user = new UserManagementInfo
                        {
                            UserName = username,
                            Email = dr.GetString(dr.GetOrdinal("Email")),
                            IsApproved = dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                            IsLockedOut = dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                            CreateDate = dr.GetDateTime(dr.GetOrdinal("CreateDate")),
                            LastLoginDate = dr.GetDateTime(dr.GetOrdinal("LastLoginDate")),
                            LastActivityDate = dr.GetDateTime(dr.GetOrdinal("LastActivityDate")),
                            UserId = dr.GetGuid(dr.GetOrdinal("UserId"))
                        };
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return user;
        }

        public UserManagementInfo GetUserByEmail(string email)
        {
            UserManagementInfo user = null;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("aspnet_Membership_GetUserByEmail");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@ApplicationName", DbType.String, "ETicket");
            dbSvc.AddInParameter(cmd, "@Email", DbType.String, email);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    if (dr.Read())
                    {
                        user = new UserManagementInfo
                        {
                            UserName = dr.GetString(dr.GetOrdinal("UserName")),
                            Email = email,
                            IsApproved = dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                            IsLockedOut = dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                            CreateDate = dr.GetDateTime(dr.GetOrdinal("CreateDate")),
                            LastLoginDate = dr.GetDateTime(dr.GetOrdinal("LastLoginDate")),
                            LastActivityDate = dr.GetDateTime(dr.GetOrdinal("LastActivityDate")),
                            UserId = dr.GetGuid(dr.GetOrdinal("UserId"))
                        };
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return user;
        }

        public UserManagementInfo GetUserById(string userId)
        {
            UserManagementInfo user = null;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("aspnet_Membership_GetUserByUserId");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@UserId", DbType.Guid, new Guid(userId));
            dbSvc.AddInParameter(cmd, "@CurrentTimeUtc", DbType.DateTime, DateTime.UtcNow);
            dbSvc.AddInParameter(cmd, "@UpdateLastActivity", DbType.Boolean, false);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    if (dr.Read())
                    {
                        user = new UserManagementInfo
                        {
                            UserName = dr.GetString(dr.GetOrdinal("UserName")),
                            Email = dr.GetString(dr.GetOrdinal("Email")),
                            IsApproved = dr.GetBoolean(dr.GetOrdinal("IsApproved")),
                            IsLockedOut = dr.GetBoolean(dr.GetOrdinal("IsLockedOut")),
                            CreateDate = dr.GetDateTime(dr.GetOrdinal("CreateDate")),
                            LastLoginDate = dr.GetDateTime(dr.GetOrdinal("LastLoginDate")),
                            LastActivityDate = dr.GetDateTime(dr.GetOrdinal("LastActivityDate")),
                            UserId = new Guid(userId)
                        };
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return user;
        }

        #endregion

        #region Client Membership Operations

        public List<ClientInfo> GetClientsByUser(string username)
        {
            List<ClientInfo> clients = new List<ClientInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_GetClientsByUserName");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@userName", DbType.String, username);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    while (dr.Read())
                    {
                        ClientInfo client = new ClientInfo
                        {
                            ClientId = dr.GetInt32(dr.GetOrdinal("client_id")),
                            ClientName = dr.GetString(dr.GetOrdinal("client_name")),
                            PreferredClient = dr.GetBoolean(dr.GetOrdinal("preferred_client")),
                            CanViewVoidedClients = dr.GetBoolean(dr.GetOrdinal("CanViewVoidedClients"))
                        };
                        clients.Add(client);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return clients;
        }

        public List<ClientInfo> GetActiveClients()
        {
            List<ClientInfo> clients = new List<ClientInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_GetActiveClients");
            cmd.CommandTimeout = 120; // 2 minutes

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    while (dr.Read())
                    {
                        ClientInfo client = new ClientInfo
                        {
                            ClientId = dr.GetInt32(dr.GetOrdinal("client_id")),
                            ClientName = dr.GetString(dr.GetOrdinal("client_name")),
                            PreferredClient = false,
                            CanViewVoidedClients = false
                        };
                        clients.Add(client);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return clients;
        }

        public void RemoveUserFromClient(string username, int clientId)
{
    Database dbSvc = DatabaseFactory.CreateDatabase();
    DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_RemoveUserFromClient");
    cmd.CommandTimeout = 120; // 2 minutes

    dbSvc.AddInParameter(cmd, "@username", DbType.String, username);
    dbSvc.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);

    try
    {
        dbSvc.ExecuteNonQuery(cmd);
    }
    finally
    {
        cmd.Dispose();
    }
}

        public void SetPreferredClient(string username, int clientId)
{
    Database dbSvc = DatabaseFactory.CreateDatabase();
    DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_SetPreferredClient");
    cmd.CommandTimeout = 120; // 2 minutes

    dbSvc.AddInParameter(cmd, "@username", DbType.String, username);
    dbSvc.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);

    try
    {
        dbSvc.ExecuteNonQuery(cmd);
    }
    finally
    {
        cmd.Dispose();
    }
}

        #endregion

        #region Department Operations

        public List<UserDeptInfo> GetClientDepartments(int clientId)
        {
            List<UserDeptInfo> departments = new List<UserDeptInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_GetClientDepartments");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@clientID", DbType.Int32, clientId);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    while (dr.Read())
                    {
                        UserDeptInfo department = new UserDeptInfo
                        {
                            DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id")),
                            DepartmentName = dr.GetString(dr.GetOrdinal("department_name")),
                            ClientId = clientId
                        };
                        departments.Add(department);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return departments;
        }

        public List<UserDeptInfo> GetDepartmentsByUser(string username, int clientId)
        {
            List<UserDeptInfo> departments = new List<UserDeptInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_GetDepartmentsByUser");
            cmd.CommandTimeout = 120; // 2 minutes
            dbSvc.AddInParameter(cmd, "@userName", DbType.String, username);
            dbSvc.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cmd);
                try
                {
                    while (dr.Read())
                    {
                        UserDeptInfo department = new UserDeptInfo
                        {
                            DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id")),
                            DepartmentName = dr.GetString(dr.GetOrdinal("department_name")),
                            ClientId = clientId
                        };
                        departments.Add(department);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }

            return departments;
        }

        public void AddUserToClient(string username, int clientId, bool preferredClient, bool canViewVoidedClients)
{
    Database dbSvc = DatabaseFactory.CreateDatabase();
    DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_AddUserToClient");
    cmd.CommandTimeout = 120; // 2 minutes

    dbSvc.AddInParameter(cmd, "@username", DbType.String, username);
    dbSvc.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);
    dbSvc.AddInParameter(cmd, "@preferredClient", DbType.Boolean, preferredClient);
    dbSvc.AddInParameter(cmd, "@canViewVoidedClients", DbType.Boolean, canViewVoidedClients);

    try
    {
        dbSvc.ExecuteNonQuery(cmd);
    }
    finally
    {
        cmd.Dispose();
    }
}

        public void AddUserToDepartment(string username, int clientId, int departmentId, Guid? overrideRoleId)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_AddUserToDepartment");
            cmd.CommandTimeout = 120; // 2 minutes

            dbSvc.AddInParameter(cmd, "@username", DbType.String, username);
            dbSvc.AddInParameter(cmd, "@clientId", DbType.Int32, clientId);
            dbSvc.AddInParameter(cmd, "@departmentId", DbType.Int32, departmentId);
            dbSvc.AddInParameter(cmd, "@overrideRoleId", DbType.Guid,
                overrideRoleId.HasValue ? (object)overrideRoleId.Value : DBNull.Value);

            try
            {
                dbSvc.ExecuteNonQuery(cmd);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public void RemoveUserFromDepartment(string username, int clientId, int departmentId)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cmd = dbSvc.GetStoredProcCommand("msinet_RemoveFromUserDepartment");
            cmd.CommandTimeout = 120; // 2 minutes
            
            // Get the user ID from the username
            Guid userId = Guid.Empty;
            DbCommand userCmd = dbSvc.GetStoredProcCommand("GetUserIdByName");
            dbSvc.AddInParameter(userCmd, "@UserName", DbType.String, username);
            
            try
            {
                object result = dbSvc.ExecuteScalar(userCmd);
                if (result != null && result != DBNull.Value)
                {
                    userId = (Guid)result;
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            finally
            {
                userCmd.Dispose();
            }
            
            // Get the client_membership_id
            int clientMembershipId = 0;
            DbCommand membershipCmd = dbSvc.GetStoredProcCommand("msinet_GetClientMembershipId");
            dbSvc.AddInParameter(membershipCmd, "@UserId", DbType.Guid, userId);
            dbSvc.AddInParameter(membershipCmd, "@ClientId", DbType.Int32, clientId);
            
            try
            {
                object result = dbSvc.ExecuteScalar(membershipCmd);
                if (result != null && result != DBNull.Value)
                {
                    clientMembershipId = (int)result;
                }
                else
                {
                    throw new Exception("Client membership not found");
                }
            }
            finally
            {
                membershipCmd.Dispose();
            }
            
            // Add parameters for the RemoveUserFromDepartment stored procedure
            dbSvc.AddInParameter(cmd, "@ClientMembershipId", DbType.Int32, clientMembershipId);
            dbSvc.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
            
            try
            {
                dbSvc.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        #endregion
    }
}