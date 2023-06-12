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
    public partial class HeadCountReport : BaseMSINetPage
    {
        private string _xls = "";

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            string printerFriendly = Request.QueryString["print"];
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientInfo = _clientInfo;

            if (printerFriendly != null && printerFriendly == "1")
            {
                this.ctlHeadCountReport.ExportDisplayType = MSI.Web.Controls.MSINetHeadCountReport.ExportType.Print;
                this.ctlMastHead.DisplayPrinterFriendly = true;
                this.pnlHeadCountReport.Width = new Unit(850);
                this.lnkPrinterFriendly.Visible = false;
                //this.lblTitle.Visible = false;
            }

            this.ctlHeadCountReport.ClientInfo = base._clientInfo;
            if (!this.IsPostBack)
            {

            }
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            if (Context.User.IsInRole("TimeClock"))
            {
                base._isAuthorized = false;
            }

            return base.IsAuthorizedAccess();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer ) 
        {
            if (_xls == "1")
            {
                Response.ContentType = "application/ms-excel";

                Response.AddHeader("Content-Disposition", "inline;filename=HoursReport.xls");

                //this.RenderChildren(writer);
            }
            //else
            //{
                this.RenderChildren(writer);
            //}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lnkPrinterFriendly.NavigateUrl = "~/auth/HeadCountReport.aspx?print=1&detail=0&date=" + this.ctlHeadCountReport.GetSelectedDate();
        }

        protected void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx");
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnLoad");
        }
    }
}