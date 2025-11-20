using System;
using System.Collections.Generic;
using System.Web.Security;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;

namespace MSI.Web.MSINet.BusinessLogic
{
    public class UserManagementBL : BaseMSINetPage
    {
        private UserManagementDB _userManagementDB = new UserManagementDB();
        
        #region User Operations
        
        public List<UserManagementInfo> GetAllUsers()
        {
            return _userManagementDB.GetAllUsers();
        }
        
        public UserManagementInfo GetUserByName(string username)
        {
            return _userManagementDB.GetUserByName(username);
        }
        
        public UserManagementInfo GetUserByEmail(string email)
        {
            return _userManagementDB.GetUserByEmail(email);
        }
        
        public UserManagementInfo GetUserById(string userId)
        {
            return _userManagementDB.GetUserById(userId);
        }
        
        public OperationResult CreateUser(string username, string password, string email, bool isApproved)
        {
            try
            {
                MembershipCreateStatus status;
                Membership.CreateUser(username, password, email, null, null, isApproved, out status);
                
                if (status == MembershipCreateStatus.Success)
                {
                    return new OperationResult { Success = true, Message = "User created successfully", ErrorCode = 0 };
                }
                else
                {
                    return new OperationResult { Success = false, Message = GetErrorMessage(status), ErrorCode = (int)status };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult UpdateUser(string username, string email, bool isApproved)
        {
            try
            {
                MembershipUser user = Membership.GetUser(username);
                if (user != null)
                {
                    user.Email = email;
                    user.IsApproved = isApproved;
                    Membership.UpdateUser(user);
                    
                    return new OperationResult { Success = true, Message = "User updated successfully", ErrorCode = 0 };
                }
                else
                {
                    return new OperationResult { Success = false, Message = "User not found", ErrorCode = 404 };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult DeleteUser(string username)
        {
            try
            {
                if (Membership.DeleteUser(username, true))
                {
                    return new OperationResult { Success = true, Message = "User deleted successfully", ErrorCode = 0 };
                }
                else
                {
                    return new OperationResult { Success = false, Message = "Failed to delete user", ErrorCode = 500 };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult ResetUserPassword(string username, string newPassword)
        {
            try
            {
                MembershipUser user = Membership.GetUser(username);
                if (user != null)
                {
                    string resetPassword = user.ResetPassword();
                    if (user.ChangePassword(resetPassword, newPassword))
                    {
                        return new OperationResult { Success = true, Message = "Password reset successfully", ErrorCode = 0 };
                    }
                    else
                    {
                        return new OperationResult { Success = false, Message = "Failed to change password", ErrorCode = 500 };
                    }
                }
                else
                {
                    return new OperationResult { Success = false, Message = "User not found", ErrorCode = 404 };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        private string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists.";
                case MembershipCreateStatus.DuplicateEmail:
                    return "Email already exists.";
                case MembershipCreateStatus.InvalidPassword:
                    return "Password is invalid.";
                case MembershipCreateStatus.InvalidEmail:
                    return "Email is invalid.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "Password answer is invalid.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "Password question is invalid.";
                case MembershipCreateStatus.InvalidUserName:
                    return "Username is invalid.";
                case MembershipCreateStatus.ProviderError:
                    return "Provider error.";
                case MembershipCreateStatus.UserRejected:
                    return "User has been rejected.";
                default:
                    return "Unknown error.";
            }
        }
        
        #endregion
        
        #region Role Operations
        
        public List<string> GetAllRoles()
        {
            return new List<string>(Roles.GetAllRoles());
        }
        
        public List<string> GetRolesForUser(string username)
        {
            return new List<string>(Roles.GetRolesForUser(username));
        }
        
        public List<string> GetUsersInRole(string roleName)
        {
            return new List<string>(Roles.GetUsersInRole(roleName));
        }
        
        public OperationResult AddUserToRole(string username, string roleName)
        {
            try
            {
                Roles.AddUserToRole(username, roleName);
                return new OperationResult { Success = true, Message = "User added to role successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult RemoveUserFromRole(string username, string roleName)
        {
            try
            {
                Roles.RemoveUserFromRole(username, roleName);
                return new OperationResult { Success = true, Message = "User removed from role successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public bool IsUserInRole(string username, string roleName)
        {
            return Roles.IsUserInRole(username, roleName);
        }
        
        #endregion
        
        #region Client Membership Operations
        
        public List<ClientInfo> GetClientsByUser(string username)
        {
            return _userManagementDB.GetClientsByUser(username);
        }
        
        public List<ClientInfo> GetActiveClients()
        {
            return _userManagementDB.GetActiveClients();
        }
        
        public OperationResult AddUserToClient(string username, int clientId, bool preferredClient, bool canViewVoidedClients)
        {
            try
            {
                _userManagementDB.AddUserToClient(username, clientId, preferredClient, canViewVoidedClients);
                return new OperationResult { Success = true, Message = "User added to client successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult RemoveUserFromClient(string username, int clientId)
        {
            try
            {
                _userManagementDB.RemoveUserFromClient(username, clientId);
                return new OperationResult { Success = true, Message = "User removed from client successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult SetPreferredClient(string username, int clientId)
        {
            try
            {
                _userManagementDB.SetPreferredClient(username, clientId);
                return new OperationResult { Success = true, Message = "Preferred client set successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        #endregion
        
        #region Department Operations
        
        public List<UserDeptInfo> GetClientDepartments(int clientId)
        {
            return _userManagementDB.GetClientDepartments(clientId);
        }
        
        public List<UserDeptInfo> GetDepartmentsByUser(string username, int clientId)
        {
            return _userManagementDB.GetDepartmentsByUser(username, clientId);
        }
        
        public OperationResult AddUserToDepartment(string username, int clientId, int departmentId, Guid? overrideRoleId)
        {
            try
            {
                _userManagementDB.AddUserToDepartment(username, clientId, departmentId, overrideRoleId);
                return new OperationResult { Success = true, Message = "User added to department successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        public OperationResult RemoveUserFromDepartment(string username, int clientId, int departmentId)
        {
            try
            {
                _userManagementDB.RemoveUserFromDepartment(username, clientId, departmentId);
                return new OperationResult { Success = true, Message = "User removed from department successfully", ErrorCode = 0 };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message, ErrorCode = -1 };
            }
        }
        
        #endregion
    }
}