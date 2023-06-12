using System;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class DaysWorkedReport
	{
        public DaysWorkedReport()
        {
            DaysWorkedCollection = new ArrayList();
            BackgroundColor = "";
        }
        public DateTime LastDayWorked { get; set; }
        public int MinDays { get; set; }
        public string BackgroundColor { get; set; }
        public ArrayList DaysWorkedCollection { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ClientID { get; set; }
    }
    public class DaysWorkedItem : System.IComparable
    {
        public DaysWorkedItem()
        {
            DaysWorked = new List<int>();
            Depts = new List<string>();
        }
        public int DeptId { get; set; }
        public List<String> Depts { get; set; }
        public string LastPunch { get; set; }
        public string FirstPunch { get; set; }
        public string BadgeNumber { get; set; }
        public string DnrReason { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> DaysWorked { get; set; }
        public string Shift { get; set; }
        public int PunchCount { get; set; }
        public int TotalDaysWorked { get; set; }
        public string EndOfBreak { get; set; }
        public int CompareTo(object obj)
        {
            if (!(obj is DaysWorkedItem))
                throw new InvalidCastException("This object is not of type Days Worked Item!");

            DaysWorkedItem myItem = (DaysWorkedItem)obj;

            return myItem.TotalDaysWorked.CompareTo(this.TotalDaysWorked);
        }
    }
}
