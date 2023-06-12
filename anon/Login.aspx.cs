using System;
using System.Web.UI;

namespace MSI.Web.MSINet
{
    public partial class Login : BaseMSINetPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ctlSubHeader.ChangeClients = false;

            if ( Page.IsPostBack )
               base.InitSession();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            base.SaveSession();
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;
            return base.IsAuthorizedAccess();
        }
    }
}