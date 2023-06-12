using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
    public class EmployeeWorkSummarySortByCheckIn : IComparer
    {
        public int Compare(object o1, object o2)
        {
            EmployeeWorkSummary summary1 = (EmployeeWorkSummary)o1;
            EmployeeWorkSummary summary2 = (EmployeeWorkSummary)o2;

            return summary1.CheckInDateTime.CompareTo(summary2.CheckInDateTime);
        }
    }
}