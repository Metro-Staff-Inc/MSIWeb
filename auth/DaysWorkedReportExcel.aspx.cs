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
    public partial class DaysWorkedReportExcel : BaseMSINetPage
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.EnableViewState = false;
            this.ctlDaysWorkedReport.ClientInfo = base._clientInfo;
            this.ctlDaysWorkedReport.ExportDisplayType = MSI.Web.Controls.MSINetDaysWorkedReport.ExportType.Excel;

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
            string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);

            Response.ContentType = "application/ms-excel";

            Response.AddHeader("Content-Disposition", "inline;filename=DaysWorkedReport" + dateTime + ".xls");
            this.RenderChildren(writer);
        } 
    }
}