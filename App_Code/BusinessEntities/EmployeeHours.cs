using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSI.Web.MSINet.BusinessEntities
{
    public class EmployeeHoursItem
    {
        public EmployeeHoursItem() {}
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AidentNumber { get; set; }
        public string BadgeNumber { get; set; }
        public int RosterID { get; set; }
        public Decimal[] DailyHours { get; set; }
        public Decimal TotalHours { get; set; }
        public string Notes { get; set; }
        public string Office { get; set; }
        public List<DateTime> StartDate { get; set; }
        public List<DateTime> EndDate { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public bool Temp { get; set; }
        public Decimal PayRate { get; set; }
    }

    public class EmployeeHours
    {
        public EmployeeHours() {}
        public int EmployeeHoursHeaderId { get; set; }
        public string Notes { get; set; }
        public Decimal TotalHours { get; set; }
        public string Supervisor { get; set; }
        public bool Submitted { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime weekEnding { get; set; }
        public List<EmployeeHoursItem> Employees { get; set; }
        public string DefaultStart { get; set; }
        public string DefaultEnd { get; set; }
        public Decimal Multiplier { get; set; }
        public int ShiftMapping { get; set; }
    }
}
