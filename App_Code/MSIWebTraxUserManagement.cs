using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Services.Protocols;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;

namespace MSI.Web.Services
{
    /// <summary>
    /// User Management API for handling users, roles, client memberships, and departments
    /// </summary>
    [WebService(Namespace = "http://msiwebtrax.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MSIWebTraxUserManagement : System.Web.Services.WebService
    {
        public UserCredentials CredentialsHeader;
        
        public MSIWebTraxUserManagement()
        {
            // Constructor
        }
        
        #region User Operations
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserManagementInfo[] GetAllUsers()
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<UserManagementInfo> users = userManagementBL.GetAllUsers();
                
                return users.ToArray();
            }
            
            return new UserManagementInfo[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserManagementInfo GetUserByName(string username)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                return userManagementBL.GetUserByName(username);
            }
            
            return null;
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserManagementInfo GetUserByEmail(string email)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                return userManagementBL.GetUserByEmail(email);
            }
            
            return null;
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserManagementInfo GetUserById(string userId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                return userManagementBL.GetUserById(userId);
            }
            
            return null;
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult CreateUser(string username, string password, string email, bool isApproved)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to create users
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.CreateUser(username, password, email, isApproved);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult UpdateUser(string username, string email, bool isApproved)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to update users
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.UpdateUser(username, email, isApproved);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult DeleteUser(string username)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to delete users
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.DeleteUser(username);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult ResetUserPassword(string username, string newPassword)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to reset passwords
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.ResetUserPassword(username, newPassword);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        #endregion
        
        #region Role Operations
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string[] GetAllRoles()
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<string> roles = userManagementBL.GetAllRoles();
                
                return roles.ToArray();
            }
            
            return new string[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string[] GetRolesForUser(string username)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<string> roles = userManagementBL.GetRolesForUser(username);
                
                return roles.ToArray();
            }
            
            return new string[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public string[] GetUsersInRole(string roleName)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<string> users = userManagementBL.GetUsersInRole(roleName);
                
                return users.ToArray();
            }
            
            return new string[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult AddUserToRole(string username, string roleName)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage roles
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.AddUserToRole(username, roleName);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult RemoveUserFromRole(string username, string roleName)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage roles
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.RemoveUserFromRole(username, roleName);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public bool IsUserInRole(string username, string roleName)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                return userManagementBL.IsUserInRole(username, roleName);
            }
            
            return false;
        }
        
        #endregion
        
        #region Client Membership Operations
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public ClientInfo[] GetClientsByUser(string username)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<ClientInfo> clients = userManagementBL.GetClientsByUser(username);
                
                return clients.ToArray();
            }
            
            return new ClientInfo[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public ClientInfo[] GetActiveClients()
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<ClientInfo> clients = userManagementBL.GetActiveClients();
                
                return clients.ToArray();
            }
            
            return new ClientInfo[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult AddUserToClient(string username, int clientId, bool preferredClient, bool canViewVoidedClients)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage client memberships
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.AddUserToClient(username, clientId, preferredClient, canViewVoidedClients);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult RemoveUserFromClient(string username, int clientId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage client memberships
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.RemoveUserFromClient(username, clientId);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult SetPreferredClient(string username, int clientId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage client memberships
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.SetPreferredClient(username, clientId);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        #endregion
        
        #region Department Operations
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserDeptInfo[] GetClientDepartments(int clientId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<UserDeptInfo> departments = userManagementBL.GetClientDepartments(clientId);
                
                return departments.ToArray();
            }
            
            return new UserDeptInfo[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public UserDeptInfo[] GetDepartmentsByUser(string username, int clientId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                UserManagementBL userManagementBL = new UserManagementBL();
                List<UserDeptInfo> departments = userManagementBL.GetDepartmentsByUser(username, clientId);
                
                return departments.ToArray();
            }
            
            return new UserDeptInfo[0];
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult AddUserToDepartment(string username, int clientId, int departmentId, Guid? overrideRoleId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage department access
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.AddUserToDepartment(username, clientId, departmentId, overrideRoleId);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public OperationResult RemoveUserFromDepartment(string username, int clientId, int departmentId)
        {
            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);
                
                // Check if user has permission to manage department access
                if (Roles.IsUserInRole(CredentialsHeader.UserName, "Administrator"))
                {
                    UserManagementBL userManagementBL = new UserManagementBL();
                    return userManagementBL.RemoveUserFromDepartment(username, clientId, departmentId);
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Unauthorized", ErrorCode = 401 };
                }
            }
            
            return new OperationResult { Success = false, Message = "Invalid credentials", ErrorCode = 401 };
        }
        
        #endregion
    }
}