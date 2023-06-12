using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace MSI.Web.MSINet
{
    public partial class UserRoles : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                MembershipUserCollection users = Membership.GetAllUsers();
                foreach( MembershipUser user in users)
                {
                    //String s = user.GetPassword();
                    //Membership.UpdateUser(user);
                }
            }
        }


        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;
            return base.IsAuthorizedAccess();
        }
    }
}
