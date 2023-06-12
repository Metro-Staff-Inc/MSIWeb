using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class GroupHoursReport : BaseMSINetPage
    {
        private string _xls = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string printerFriendly = Request.QueryString["print"];
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientInfo = _clientInfo;
            if (printerFriendly != null && printerFriendly == "1")
            {
                this.ctlHoursReport.ExportDisplayType = MSI.Web.Controls.MSINetGroupHoursReport.ExportType.Print;
                //this.ctlSubHeader.ShowLogoffLink = false;
                this.ctlMastHead.DisplayPrinterFriendly = true;
                this.pnlHoursReport.Width = new Unit(850);
                this.lnkPrinterFriendly.Visible = false;
                this.MainMenu2.Visible = false;
                this.lblTitle.Visible = false;
            }

            this.ctlHoursReport.ClientInfo = base._clientInfo;
            this.ctlHoursReport.ClientPrefs = base._clientPrefs;
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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
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
            this.lnkPrinterFriendly.NavigateUrl = "~/auth/GroupHoursReport.aspx?print=1&detail=0&date=" + this.ctlHoursReport.GetSelectedDate();
        }
    }
}