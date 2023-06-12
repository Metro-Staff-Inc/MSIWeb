using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class DispatchData : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            userID.Value = Context.User.Identity.Name.ToString();
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = Context.User.Identity.Name.ToString().ToUpper().Equals("ITDEPT");
            base._isAuthorized |= Context.User.Identity.Name.ToString().ToUpper().Equals("RIZNERD");
            base._isAuthorized |= Context.User.Identity.Name.ToString().ToUpper().Equals("MOAKES");
            base._isAuthorized |= Context.User.Identity.Name.ToString().ToUpper().Equals("MCHAVEZ");
            base._isAuthorized |= Context.User.Identity.Name.ToString().ToUpper().Equals("BADANIS");
            base._isAuthorized |= Context.User.IsInRole("DailyDispatch");
            return base.IsAuthorizedAccess();
        }
   }
}