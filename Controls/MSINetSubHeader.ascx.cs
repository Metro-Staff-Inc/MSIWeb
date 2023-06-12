using System;
using System.Collections;
using System.Web.UI.WebControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;

namespace MSI.Web.Controls
{
    public partial class MSINetSubHeader : BaseMSINetControl
    {
        private ArrayList _clients = new ArrayList();
        private Client _clientInfo;
        private ClientPreferences _clientPrefs;
        private string _sectionHeader = String.Empty;

        private Boolean _changeClients = true;
        private Boolean _msiLabel = true;

        public string SectionHeader
        {
            get
            {
                return _sectionHeader;
            }
            set
            {
                _sectionHeader = value;
            }
        }

        public Boolean ChangeClients
        {
            get
            {
                return _changeClients;
            }
            set
            {
                _changeClients = value;
            }
        }

        public Boolean MsiLabel
        {
            get
            {
                return _msiLabel;
            }
            set
            {
                _msiLabel = value;
            }
        }

        public Client ClientInfo
        {
            get
            {
                return _clientInfo;
            }
            set
            {
                _clientInfo = value;
            }
        }

        public ClientPreferences ClientPrefs
        {
            get
            {
                return _clientPrefs;
            }
            set
            {
                _clientPrefs = value;
            }
        }

        public ArrayList Clients
        {
            get
            {
                return _clients;
            }
            set
            {
                _clients = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string header = "Metro Staff Web Trax - " + this.SectionHeader;
            this.tdLabel.Visible = MsiLabel;
            this.tdChangeClient.Visible = ChangeClients;
            if (base._isPDA)
            {
                this.pnlPDASubHeader.Visible = true;
                this.tblSubHeader.Visible = false;
                this.lblPDASubHeader.Text = header;
            }
            else
            {
                this.pnlPDASubHeader.Visible = false;
                this.tblSubHeader.Visible = true;
                this.lblSubHeader.Text = header;
                
                if (_clientPrefs == null)
                    ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
            }

            if (true)
            {
                if (!this.IsPostBack)
                {
                    //load the client drop down.
                    this.cboClient.Items.Clear();
                    ListItem item = null;
                    item = new ListItem("--Select a Client--", "-1");
                    item.Selected = true;
                    this.cboClient.Items.Add(item);
                    if (_clients != null)
                    { 
                        foreach (Client client in _clients)
                        {
                            item = new ListItem(client.ToString(), client.ClientID.ToString());
                            this.cboClient.Items.Add(item);
                        }
                    }
                }
            }
        }
        public bool setClient(string clientID)
        {
            _clients = (ArrayList)Session["Clients"];
            if (_clients == null)
            {
                ClientBL clientBL = new ClientBL();
                _clients = clientBL.GetClientsByUserName(Context.User.Identity.Name);
                Session["Clients"] = _clients;
            }
            foreach (Client client in _clients)
            {
                if (client.ClientID.ToString() == clientID)
                {
                    Session["ClientInfo"] = client;
                    /* set client preferences */
                    ClientBL cbl = new ClientBL();
                    _clientPrefs = cbl.GetClientPreferencesByID(client.ClientID);
                    _clientPrefs.DisplayPayRate = _clientPrefs.DisplayPayRate && 
                        Context.User.IsInRole("NoViewPayRates") == false;
                    Session["ClientPrefs"] = _clientPrefs;
                    return true;
                }
            }
            return false;
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (setClient(this.cboClient.SelectedValue))
                Response.Redirect("~/auth/MainMenu.aspx");
        }

    }
}