using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for DailyTracker
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// Summary description for Content.
    /// </summary>
    /// 
    [DataContract]
    public class DailyTracker
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int OfficeId { get; set; }
        [DataMember]
        public List<EmployeeTracker> Employees { get; set; }
        [DataMember]
        public Department Dept { get; set; }
        [DataMember]
        public ShiftType ShiftType { get; set; }
        [DataMember]
        public DateTime PeriodStart { get; set; }
        [DataMember]
        public DateTime PeriodEnd { get; set; }

        [DataMember]
        public int ClientRosterId { get; set; }
        //private Enums.TrackingTypes _trackingType = Enums.TrackingTypes.Roster;
        //private string _overrideRoleName = string.Empty;

        public DailyTracker()
        {
            Dept = new Department();
            ShiftType = new ShiftType();
            Employees = new List<EmployeeTracker>();
        }
	}
}