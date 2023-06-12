using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data.Common;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using System.Runtime.Remoting.Contexts;

/// <summary>
/// Summary description for WeeklyReportDB
/// </summary>
/// 
namespace MSI.Web.MSINet.DataAccess
{
    public class WeeklyReportDB
    {
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();
        //private decimal _minShiftBreakHours = 5;
        //private bool UseExactTimes = false;

        public HoursReport GetWeeklyReport(HoursReport hoursReport, string name, string badgeNum, bool sortByDept)
        {
            HoursReportDB hr = new HoursReportDB();
            return hr.GetHoursReport(hoursReport, name, badgeNum, sortByDept);
        }
    }
}
