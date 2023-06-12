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
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MSI.Web.MSINet
{
    public partial class HoursReportExcel : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.EnableViewState = false;
            this.ctlHoursReport.ClientInfo = base._clientInfo;
            this.ctlHoursReport.showAllEmployees = false;
            this.ctlHoursReport.ClientPrefs = base._clientPrefs;
            this.ctlHoursReport.ExportDisplayType = MSI.Web.Controls.MSINetHoursReport.ExportType.Excel;
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
        /*
        protected override void Render(HtmlTextWriter writer)
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                    base.Render(hWriter);
                    string html = sb.ToString();
                    html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATE)\"[^>]*>", string.Empty, RegexOptions.IgnoreCase);
                    writer.Write(html);
                } 
         */
        protected override void Render(System.Web.UI.HtmlTextWriter writer ) 
        {
            string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);
            //string daysWorked = Server.UrlDecode((string)Request.QueryString["daysWorked"]);

            Response.ContentType = "application/ms-excel";
            Response.AddHeader("Content-Disposition", "inline;filename=HoursReport" + _clientInfo.ClientID + "_" + dateTime + ".xls");
            //this.RenderChildren(writer);
            
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);

            base.Render(hWriter);
            string html = sb.ToString();

            html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATE)\"[^>]*>", string.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "id=\"ctlHoursReport_[^\"]+\"", String.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, " {2,}", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!---->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"^\s*$[\r\n]*", "", RegexOptions.Multiline);
            html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATEGENERATOR)\"[^>]*>", String.Empty, RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<input[^>]*id=\"(__EVENTVALIDATION)\"[^>]*>", String.Empty, RegexOptions.IgnoreCase);

            writer.Write(html);
        } 
    }
}