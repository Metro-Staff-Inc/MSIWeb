using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace MSI.Web.Controls
{
    public partial class MSINetUserRoles : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ID = "ctlUserRoles";
            //MembershipUser user = Membership.CreateUser("jmurfey2", "lkjhlkjh1A", "jmurfey@msistaff.com");
            //if( !this.IsPostBack)
                //GetUsers();
        }
    }
}
