using System;

namespace MSI.Web.MSINet
{
    public partial class MapEmployeeID : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctlSubHeader.SectionHeader = _clientInfo.ToString();
            ctlSubHeader.Clients = _clients;
            ctlSubHeader.ClientInfo = _clientInfo;
            if (!this.IsPostBack)
            {

            }
        }
        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            return base.IsAuthorizedAccess();
        }
    }
}