//JHM//using MSIToolkit.Logging;
using System;

namespace MSI.Web.MSINet
{
    public partial class MainMenu : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.LoadClientShiftTypes();
            
            this.ctlSubHeader.SectionHeader = ClientInfo.ToString();
            this.ctlSubHeader.ClientInfo = ClientInfo;
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientPrefs = _clientPrefs;
        }

        protected override void SecureForm()
        {
            //this.lnkInvoiceProcessing.Visible = false;

            if (Context.User.IsInRole("TimeClock"))
            {
                //this.lnkDaysWorkedReport.Visible = false;
            }
            else if (Context.User.IsInRole("ManagerNoPunchIn"))
            {
                //this.lnkCheckIn.Visible = false;
            }
            else if( _clientPrefs != null && _clientPrefs.DisplayInvoice ) //_clientInfo.ClientID == 178 || _clientInfo.ClientID == 92 || _clientInfo.ClientID == 181 || _clientInfo.ClientID == 226 || _clientInfo.ClientID == 229 || _clientInfo.ClientID == 258)
            {
                if (Context.User.Identity.Name.ToUpper() == "MCHAVEZ" || Context.User.Identity.Name.ToUpper() == "MOAKES" || Context.User.Identity.Name.ToUpper() == "ITDEPT")
                {
                  //  this.lnkInvoiceProcessing.Visible = true;
                }
            }
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            return base.IsAuthorizedAccess();
        }
    }
}