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

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class HeadCountReportBL
    {
        public HeadCountReportBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private HeadCountReportDB headCountReportDB = new HeadCountReportDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public HeadCountReport GetHeadCountReport(HeadCountReport headCountReport, IPrincipal userPrincipal)
        {
            return headCountReportDB.GetHeadCountReport(headCountReport, userPrincipal);
        }

        public DataSet SelectRosterAndHeadCountReport(int clientId)
        {
            DataSet ds = headCountReportDB.GetRosterAndHeadCountData(clientId);
            Console.WriteLine(ds.ToString());
            return ds;
            //return headCountReportDB.GetRosterAndHeadCountReport(clientId);
        }
        public List<DailyPunchDepartmentShiftInfo> SelectListRosterAndHeadCountReport(int clientId)
        {
            List<DailyPunchDepartmentShiftInfo> ds = headCountReportDB.SelectListRosterAndHeadCountReport(clientId);
            Console.WriteLine(ds.ToString());
            return ds;
        }
    }
}