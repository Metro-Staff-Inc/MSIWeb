using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class TransportExcel : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void Render(HtmlTextWriter writer)
        {
            string stDt = Server.UrlDecode((string)Request.QueryString["startDate"]);
            string endDt = Server.UrlDecode(Request.QueryString["endDate"]);
            string clientId = Server.UrlDecode((string)Request.QueryString["clientId"]);

            //ctlHeadCountExcel.dt = dt;

            Response.ContentType = "application/ms-excel";
            Response.AddHeader("Content-Disposition", "inline;filename=HeadCountReport_" + clientId + "_" + ".xls");

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);

            base.Render(hWriter);
            string html = sb.ToString();
            html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATE)\"[^>]*>", string.Empty, RegexOptions.IgnoreCase);
            writer.Write(html);

        }
    }
}