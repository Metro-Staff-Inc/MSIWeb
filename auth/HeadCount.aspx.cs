using System;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class HeadCount : BaseMSINetPage
    {

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            string printerFriendly = Request.QueryString["print"];
            ctlSubHeader.SectionHeader = _clientInfo.ToString();
            ctlSubHeader.Clients = _clients;
            ctlSubHeader.ClientInfo = _clientInfo;

            if (printerFriendly != null && printerFriendly == "1")
            {
                //this.ctlHeadCount.ExportDisplayType = MSI.Web.Controls.MSINetHeadCount.ExportType.Print;
                ctlMastHead.DisplayPrinterFriendly = true;
                pnlHeadCountReport.Width = new Unit(850);
                lnkPrinterFriendly.Visible = false;
                //this.lblTitle.Visible = false;
            }
            //this.ctlHeadCount.ClientInfo = base._clientInfo;
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;
            if (Context.User.IsInRole("TimeClock"))
            {
                _isAuthorized = false;
            }
            return base.IsAuthorizedAccess();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.RenderChildren(writer);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx");
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }
    }
}