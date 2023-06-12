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

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class GroupHoursReportBL
    {
        public GroupHoursReportBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private GroupHoursReportDB groupHoursReportDB = new GroupHoursReportDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public HoursReport GetHoursReport(HoursReport hoursReport, /*IPrincipal userPrincipal*/string userId, string badgeNum, bool sortByDept)
        {
            return groupHoursReportDB.GetGroupHoursReport(hoursReport, /*userPrincipal*/userId, badgeNum, sortByDept);
        }

        public bool ApprovePunchRange(HoursReport groupHoursReport, IPrincipal userPrincipal)
        {
            return groupHoursReportDB.ApprovePunchRange(groupHoursReport, userPrincipal.Identity.Name);
        }
        public bool ApproveClientHours(HoursReport groupHoursReport, IPrincipal userPrincipal)
        {
            return groupHoursReportDB.ApproveClientHours(groupHoursReport, userPrincipal.Identity.Name);
        }
    }
}