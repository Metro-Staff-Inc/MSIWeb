using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System.Security.Principal;
using System.Collections.Generic;
using MSIToolkit.Logging;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class HoursReportBL
    {
        public HoursReportBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private HoursReportDB hoursReportDB = new HoursReportDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public string MovePunchDeptShift(String punchId, String departmentId, String shiftType, String userName)
        {
            int punch = Convert.ToInt32(punchId);
            int department = Convert.ToInt32(departmentId);
            int shift = Convert.ToInt32(shiftType);
            return hoursReportDB.MovePunchDeptShift(punch, department, shift, userName);
        }

        public List<ShiftDepartment> GetShiftDepartments(int clientId)
        {
            return hoursReportDB.GetShiftDepartments(clientId);
        }
        public HoursReport GetHoursReport(HoursReport hoursReport, string userId, string badgeNum, bool sortByDept, PerformanceLogger log = null)
        {
            return hoursReportDB.GetHoursReport(hoursReport, userId, badgeNum, sortByDept, log);
        }

        public bool ApprovePunchRange(HoursReport hoursReport, IPrincipal userPrincipal)
        {
            return hoursReportDB.ApprovePunchRange(hoursReport, userPrincipal.Identity.Name);
        }
        public bool ApproveClientHours(HoursReport hoursReport, IPrincipal userPrincipal)
        {
            return hoursReportDB.ApproveClientHours(hoursReport, userPrincipal.Identity.Name);
        }
        public bool UnSubmitClientHours(HoursReport hoursReport, IPrincipal userPrincipal)
        {
            return hoursReportDB.UnSubmitClientHours(hoursReport, userPrincipal.Identity.Name);
        }
    }
}