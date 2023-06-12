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

namespace MSI.Web.MSINet
{
    public partial class CheckIn : BaseMSINetPage
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.ctlCheckIn.ClientInfo = base._clientInfo;
            this.ctlCheckIn.PunchExceptions = base._punchExceptions;
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            
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