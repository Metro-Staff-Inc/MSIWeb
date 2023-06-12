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
    public partial class EmployeeHistory : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.ctlEmployeeHistory.ClientInfo = base._clientInfo;

            if (!this.IsPostBack)
            {
                //get the client shift types
                
            }
        }
    }
}