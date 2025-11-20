using System;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// User management information
    /// </summary>
    public class UserManagementInfo
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastActivityDate { get; set; }
    }

    /// <summary>
    /// Client information for user
    /// </summary>
    public class ClientInfo
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public bool PreferredClient { get; set; }
        public bool CanViewVoidedClients { get; set; }
    }

    /// <summary>
    /// Department information for user management
    /// </summary>
    public class UserDeptInfo
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ClientId { get; set; }
    }

    /// <summary>
    /// User-Department relationship
    /// </summary>
    public class UserDepartmentInfo
    {
        public int UserDepartmentId { get; set; }
        public int ClientMembershipId { get; set; }
        public int DepartmentId { get; set; }
        public Guid? OverrideRoleId { get; set; }
    }

    /// <summary>
    /// Operation result
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }
}