using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class TestPage : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;
            return base.IsAuthorizedAccess();
        }
    }
}