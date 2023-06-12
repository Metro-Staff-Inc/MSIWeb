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
    public class EmployeePunchMaintenanceBL
    {
        public EmployeePunchMaintenanceBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private EmployeePunchMaintenanceDB employeePunchMaintenanceDB = new EmployeePunchMaintenanceDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public EmployeePunchMaintenance GetEmployeePunchMaintenance(EmployeePunchMaintenance employeePunchMaintenance, IPrincipal userPrincipal)
        {
            return employeePunchMaintenanceDB.GetEmployeePunchMaintenance(employeePunchMaintenance, userPrincipal);
        }

        public EmployeePunchMaintenanceResult SaveEmployeePunch(EmployeePunch employeePunch, IPrincipal userPrincipal)
        {
            EmployeePunchMaintenanceResult returnResult = employeePunchMaintenanceDB.SaveEmployeePunch(employeePunch, userPrincipal);
            return returnResult;
        }

        public EmployeePunchMaintenanceResult MoveEmployeePunch(EmployeePunchMove employeePunchMove, IPrincipal userPrincipal)
        {
            //Move the punch
            return employeePunchMaintenanceDB.MoveEmployeePunch(employeePunchMove, userPrincipal);
        }

        public EmployeePunchMaintenanceResult DeleteEmployeePunch(EmployeePunch employeePunch, IPrincipal userPrincipal)
        {
            //EmployeePunchMaintenanceResult returnResult = null;

            return employeePunchMaintenanceDB.DeleteEmployeePunch(employeePunch, userPrincipal);
        }
    }
}