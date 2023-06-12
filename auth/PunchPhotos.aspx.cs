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
using MSI.Web.Controls;


namespace MSI.Web.MSINet
{
    public partial class TestPage : BaseMSINetPage
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientPrefs = _clientPrefs;
            this.clientID.Value = "" + _clientInfo.ClientID;
            if (clientID.Value.Equals("178") || clientID.Value.Equals("256") || clientID.Value.Equals("280") || clientID.Value.Equals("281"))
                clientID.Value = "Creative";
            //if (clientID.Value.Equals("259") || clientID.Value.Equals("272") || clientID.Value.Equals("286"))
            //    clientID.Value = "259-272";

            this.webServiceLoc.Value = WebServicesLocation.WebServiceLocation.WebServiceLoc;
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
            this.RenderChildren(writer);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //this.lnkPrinterFriendly.NavigateUrl = "~/auth/HoursReport.aspx?print=1&detail=0&date=" + this.ctlHoursReport.GetSelectedDate();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }

    }
}