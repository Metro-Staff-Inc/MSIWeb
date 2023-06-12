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
    public partial class ManualCheckIn : BaseMSINetPage
    {
        private bool _isValid = false;

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.ctlCheckIn.ClientInfo = base._clientInfo;
            this.ctlCheckIn.PunchExceptions = base._punchExceptions;
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;

            string modeTag = null;
            if (!this.IsPostBack)
            {
                if (Context.Items["modeTag"] != null)
                {
                    modeTag = (string)Context.Items["modeTag"];
                    if (modeTag == "manual")
                    {
                        _isValid = true;
                        this.ctlCheckIn.BadgeNumber = (string)Context.Items["badgeNumber"];
                        this.ctlCheckIn.SwipeDateTime = DateTime.Parse((string)Context.Items["swipeDate"]);
                        this.ViewState.Add("modeTag", "manual");
                    }
                }
            }
            else
            {
                modeTag = (string)this.ViewState["modeTag"];
                if (modeTag == "manual")
                {
                    _isValid = true;
                }
            }
            if (!_isValid)
            {
                throw new Exception("Unauthorized access.");
            }
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            if (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager"))
            {
                base._isAuthorized = false;
            }

            return base.IsAuthorizedAccess();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }
    }
}