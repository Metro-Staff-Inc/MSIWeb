using System;
using MSI.Web.MSINet.Common;

namespace MSI.Web.Controls
{
    public partial class MSINetHeadCount : BaseMSINetControl
    {
        HelperFunctions _helper = new HelperFunctions();
        protected void Page_Load(object sender, EventArgs e)
        {
            //lnkExport.NavigateUrl = "~/auth/HeadCountExcel.aspx?clientId=" + 302 + "&startDate=" + hcDate.Value;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
    }
}