using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class PunchReports : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string printerFriendly = Request.QueryString["print"];
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientInfo = _clientInfo;
        }
        protected override bool IsAuthorizedAccess()
        {

            base._isAuthorized = true;
            String name = Context.User.Identity.Name.ToUpper();
            if (Context.User.IsInRole("TimeClock")||
                name.Equals("KELLYT") || name.Equals("VALLES") || 
                name.Equals("DELUTRIM") || name.Equals("GARZAVELAA") || 
                name.Equals("ALANISH") || name.Equals("PRICEB") 
                )
            {
                base._isAuthorized = false;
            }
            return base.IsAuthorizedAccess();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
    }

}