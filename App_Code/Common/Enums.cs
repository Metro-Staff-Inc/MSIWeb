using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PunchExceptions
/// </summary>
namespace MSI.Web.MSINet.Common
{
    public static class Enums
    {
        public enum PunchTypes
        {
            Invalid,
            CheckIn,
            CheckOut,
            SameDepartment,
            NoGeneralPunch
        }

        public enum EmployeePunchStatus
        {
            CheckedIn,
            NotCheckedIn,
            CheckedOut,
            EarlyCheckIn
        }

        public enum TrackingTypes
        {
            Roster,
            DayLabor,
            Exceptions
        }
    }
}