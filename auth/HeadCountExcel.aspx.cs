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
    public partial class HeadCountExcel : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            excelId.Visible = false;
            String s = Server.UrlDecode((string)Request.QueryString["startDate"]);
            //s = "2016-8-9";
            DateTime dt = Convert.ToDateTime(s);
            string clientId = Server.UrlDecode((string)Request.QueryString["clientId"]);
            this.ctlHeadCountExcel.clientId = Convert.ToInt32(clientId);
            this.ctlHeadCountExcel.dt = dt;
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
            string dt = Server.UrlDecode((string)Request.QueryString["startDate"]);
            string clientId = Server.UrlDecode((string)Request.QueryString["clientId"]);
            string output = Server.UrlDecode((String)Request.QueryString["output"]);

            this.ctlHeadCountExcel.clientId = Convert.ToInt32(clientId);
            //ctlHeadCountExcel.dt = dt;

            //string daysWorked = Server.UrlDecode((string)Request.QueryString["daysWorked"]);
            if( output.ToUpper().Equals("WORD") )
            {
                Response.ContentType = "application/ms-word";
                Response.AddHeader("Content-Disposition", "inline;filename=HeadCountReport_" + clientId + "_" + dt + ".doc");
            }
            else 
            {
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", "inline;filename=HeadCountReport_" + clientId + "_" + dt + ".xls");
            }
            //this.RenderChildren(writer);

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
