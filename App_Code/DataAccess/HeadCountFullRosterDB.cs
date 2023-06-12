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
using System.Collections.Generic;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
/// 
namespace MSI.Web.MSINet.DataAccess
{
    public class DailyPunchData
    {
        public string ID { get; set; }
        public string Last { get; set; }
        public string First { get; set; }
        public string Department { get; set; }
        public string Shift { get; set; }
        public List<DateTime> punches { get; set; }
    }

    public class HeadCountFullRosterDB
    {
        public HeadCountFullRosterDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

    }
}