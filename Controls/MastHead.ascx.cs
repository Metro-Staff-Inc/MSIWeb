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
using MSI.Web.MSINet.BusinessEntities;
using WebServicesLocation;

namespace MSI.Web.Controls
{
    public partial class MastHead : BaseMSINetControl
    {
        private bool _displayPrinterFriendly;
        protected Client _clientInfo;
        private ArrayList _clients = new ArrayList();
        private ClientPreferences _clientPrefs;

        public bool DisplayPrinterFriendly
        {
            get
            {
                return _displayPrinterFriendly;
            }
            set
            {
                _displayPrinterFriendly = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base._isPDA || _displayPrinterFriendly)
            {
                this.pnlMastHeadMobile.Visible = true;
                this.pnlMastHead.Visible = false;
            }
            else
            {
                this.pnlMastHeadMobile.Visible = false;
                this.pnlMastHead.Visible = true;
            }
            if (_clientInfo == null)
            {
                _clientInfo = (Client)Session["ClientInfo"];
            }
            if (_clientInfo != null)
            {
                string clientDir = WebServiceLocation.GetClient(_clientInfo.ClientID.ToString());
                this.clientDir.Value = clientDir;
                this.clientID.Value = _clientInfo.ClientID.ToString();
                this.userID.Value = this.Context.User.Identity.Name.ToLower().Trim();
                this.multiplier.Value = this._clientInfo.Multiplier.ToString();
            }
            if (_clientPrefs == null)
                _clientPrefs = (ClientPreferences)Session["ClientPrefs"];
            if (_clientPrefs != null)
            {
                if (this._clientPrefs.DisplayWeeklyReportsSundayToSaturday == false &&
                    this._clientPrefs.DisplayWeeklyReportsWednesdayToTuesday == false &&
                    this._clientPrefs.DisplayWeeklyReportsSaturdayToFriday == false)
                    this.weekEnd.Value = "7";
                else if (this._clientPrefs.DisplayWeeklyReportsSundayToSaturday == true)
                    this.weekEnd.Value = "6";
                else if (this._clientPrefs.DisplayWeeklyReportsWednesdayToTuesday == true)
                    this.weekEnd.Value = "3";
                else if (this._clientPrefs.DisplayWeeklyReportsSaturdayToFriday == true)
                    this.weekEnd.Value = "5";
            }
        }
    }
}