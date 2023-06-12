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
    public class EmployeeHistoryBL
    {
        public EmployeeHistoryBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private EmployeeHistoryDB employeeHistoryDB = new EmployeeHistoryDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public EmployeeHistory GetEmployeeHistory(EmployeeHistory employeeHistory, IPrincipal userPrincipal)
        {
            return employeeHistoryDB.GetEmployeeHistory(employeeHistory, userPrincipal);
        }
    }
}