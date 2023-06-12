using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSI.Web.MSINet;

public partial class auth_EmployeeDNR : BaseMSINetPage
{
    protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        //log.Info("Page_Load");
        this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
        this.ctlSubHeader.ClientInfo = _clientInfo;
        this.ctlSubHeader.Clients = _clients;
        this.ctlSubHeader.ClientPrefs = _clientPrefs;
        bool officeHours = DateTime.Now.Hour >= 7 && (DateTime.Now.Hour < 15 || (DateTime.Now.Hour == 15 && DateTime.Now.Minute < 30));
        //RemoveDNRAllAccess is Maria Mendez and Aylin, RemoveDNRSupervisor is Supervisors
        if(Context.User.IsInRole("RemoveDNRAllAccess") || (Context.User.IsInRole("RemoveSupervisor") && !officeHours ) )
        {
            hdnRemoveDnr.Value = "true";
        }
        userID.Value = Context.User.Identity.Name.ToString();
        webServiceLoc.Value = WebServicesLocation.WebServiceLocation.WebServiceLoc;
    }
    public override bool EnableEventValidation
    {
        get
        {
            return false;
        }
        set
        {
            /* Do nothing */
        }
    }
    protected override bool IsAuthorizedAccess()
    {
        base._isAuthorized = true;

        if (Context.User.IsInRole("TimeClock"))
        {
            base._isAuthorized = false;
        }

        return base.IsAuthorizedAccess();
    }
}