using System;

namespace MSI.Web.MSINet
{
    public partial class RecruitingPool : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;
            return base.IsAuthorizedAccess();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
    }
}
