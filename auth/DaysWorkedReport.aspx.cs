using System;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class DaysWorkedReport : BaseMSINetPage
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
                this.ctlDaysWorkedReport.ExportDisplayType = MSI.Web.Controls.MSINetDaysWorkedReport.ExportType.Print;
                this.ctlMastHead.DisplayPrinterFriendly = true;
                this.pnlDaysWorkedReport.Width = new Unit(850);
                this.lnkPrinterFriendly.Visible = false;
            }

            this.ctlDaysWorkedReport.ClientInfo = base._clientInfo;
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

                Response.AddHeader("Content-Disposition", "inline;filename=DaysWorkedReport.xls");
            }
            this.RenderChildren(writer);
         }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lnkPrinterFriendly.NavigateUrl = "~/auth/DaysWorkedReport.aspx?print=1&detail=0&date=" + this.ctlDaysWorkedReport.GetStartDate();
        }
        protected void btnBackToMainMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("MainMenu.aspx");
        }
    }
}