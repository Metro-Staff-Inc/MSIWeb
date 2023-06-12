using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSI.Web.Controls;
using MSI.Web.MSINet.BusinessEntities;
using WebServicesLocation;


namespace MSI.Web.Controls
{
    public partial class MSINetClientRoster : BaseMSINetControl
    {
        //protected string UserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            userID.Value = Context.User.Identity.Name.ToString();
            webServiceLoc.Value = WebServicesLocation.WebServiceLocation.WebServiceLoc;
        }
    }
}