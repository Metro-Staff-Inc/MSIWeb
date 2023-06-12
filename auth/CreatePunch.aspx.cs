using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSI.Web.Services;

public partial class auth_CreatePunch : System.Web.UI.Page
{
    protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        //log.Info("Page_Load");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string badge = txtBadge.Text;
        string date = txtDate.Text;
        MSIWebTraxCheckIn msi = new MSIWebTraxCheckIn();
        msi.CredentialsHeader = new UserCredentials();
        msi.CredentialsHeader.PWD = "alitho";
        msi.CredentialsHeader.UserName = "alithoClock";
        msi.RecordSwipeBiometric(badge + "|*|" + date + "|*|" + 1);
    }
}