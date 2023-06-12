using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using MSI.Web.MSINet;

namespace MSI.Web.MSINet
{
    public partial class PhoneBlast : BaseMSINetPage
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
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }
    }
}
