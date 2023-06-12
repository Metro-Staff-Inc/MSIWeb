using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;

/// <summary>
/// Summary description for StaticData
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class StaticDataBL
    {
	    public StaticDataBL()
	    {
		    //
		    // TODO: Add constructor logic here
		    //
	    }

        private StaticDataDB _staticDataDB = new StaticDataDB();

        public Hashtable GetPunchExceptions()
        {
            return _staticDataDB.GetPunchExceptions();
        }

        public ArrayList GetPunchMaintenanceReasons()
        {
            return _staticDataDB.GetPunchMaintenanceReasons();
        }

        public ArrayList GetMinimumWageHistory()
        {
            return _staticDataDB.GetMinimumWageHistory();
        }
    }
}