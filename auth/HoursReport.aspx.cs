using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
//JHM//using MSIToolkit.Logging;

namespace MSI.Web.MSINet
{
    public partial class HoursReport : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string printerFriendly = Request.QueryString["print"];
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientInfo = _clientInfo;
            if (printerFriendly != null && printerFriendly == "1")
            {
                this.ctlHoursReport.ExportDisplayType = MSI.Web.Controls.MSINetHoursReport.ExportType.Print;
                this.ctlMastHead.DisplayPrinterFriendly = true;
                this.pnlHoursReport.Width = new Unit(850);
                this.lnkPrinterFriendly.Visible = false;
                this.MainMenu2.Visible = false;
                this.lblTitle.Visible = false;
            }

            this.ctlHoursReport.ClientInfo = base._clientInfo;
            this.ctlHoursReport.ClientPrefs = base._clientPrefs;
            if (this.IsPostBack)
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
            if( _clientInfo.ClientID != 30299 )
            {
                RenderChildren(writer);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);

                RenderChildren(hWriter);
                string html = sb.ToString();
                try
                {
                    html = Regex.Replace(html, "ctlHoursReport_rptrHoursReport_", String.Empty, RegexOptions.None);
                    html = Regex.Replace(html, "id=\"lblWeekDay[^\"]+\"", String.Empty, RegexOptions.None);
                    String reg = "[MTWFSOR]";
                    if (_clientInfo.ClientID == 302)
                        reg = "[MTWFS]";
                    html = Regex.Replace(html, "id=\"td" + reg + ".+[0-9]\"", String.Empty, RegexOptions.None);
                    writer.Write(html);
                }
                catch (Exception e)
                {
                    writer.Write(sb.ToString());
                }
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lnkPrinterFriendly.NavigateUrl = "~/auth/HoursReport.aspx?print=1&detail=0&date=" + this.ctlHoursReport.GetSelectedDate();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
    }
}