using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    /* some desc/value info for drop down lists */
    public class User
    {
        public User()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LastActive { get; set; }
        public Guid UserID { get; set; }
        public int ClientMembershipId { get; set; }
    }
    [DataContract]
    public class FileData
    {
        public FileData() { }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public String Data { get; set; }
        [DataMember]
        public String Directory { get; set; }
    }

    [DataContract]
    public class UserInfo
    {
        public UserInfo() { }
        [DataMember]
        public String UserName;
        [DataMember]
        public String[] Roles { get; set; }
    }
    
    [DataContract]
    public class UserCredentials
    {
        public UserCredentials() { }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Pwd { get; set; }
    }

    [DataContract]
    public class PunchInfo
    {
        public PunchInfo() { }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String PWD { get; set; }
        [DataMember]
        public String BadgeNumber { get; set; }
        [DataMember]
        public String PunchDate { get; set; }
        [DataMember]
        public int Department { get; set; }
        [DataMember]
        public int Shift { get; set; }
    }
}