using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MSI.Web.MSINet.PDA
{
    public partial class CheckIn : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ctlCheckIn.ClientInfo = base._clientInfo;
            this.ctlCheckIn.PunchExceptions = base._punchExceptions;

            if (!this.IsPostBack)
            {
                //get the client shift types
                
            }
        }
    }
}