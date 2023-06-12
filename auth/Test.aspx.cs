using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class Test : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientPrefs = _clientPrefs;
            this.clientID.Value = "" + _clientInfo.ClientID;
            if (clientID.Value.Equals("178") || clientID.Value.Equals("256"))
                clientID.Value = "178-256";
            if (clientID.Value.Equals("259") || clientID.Value.Equals("272"))
                clientID.Value = "259-272";
            if (clientID.Value.Equals("166") || clientID.Value.Equals("279"))
                clientID.Value = "166-279";

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

        protected void btnGo_Click(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //this.lnkPrinterFriendly.NavigateUrl = "~/auth/HoursReport.aspx?print=1&detail=0&date=" + this.ctlHoursReport.GetSelectedDate();
        }


    }
}