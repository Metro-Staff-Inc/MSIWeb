using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;


namespace MSI.Web.MSINet
{
    public partial class ClientRoster : BaseMSINetPage
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            if (!this.IsPostBack)
            {

            }
        }
        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            return base.IsAuthorizedAccess();
        }

    }
}