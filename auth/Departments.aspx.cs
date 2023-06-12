using System;

namespace MSI.Web.MSINet
{
    public partial class Departments : BaseMSINetPage
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;
        }
        protected override bool IsAuthorizedAccess()
        {
            _isAuthorized = true;
            if( !Context.User.Identity.Name.ToLower().Equals("itdept"))
                _isAuthorized = false;
            return base.IsAuthorizedAccess();
        }
    }
}