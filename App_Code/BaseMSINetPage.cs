using System;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using System.Collections;

/// <summary>
/// Summary description for BaseMSINetPage
/// </summary>
namespace MSI.Web.MSINet
{
    public class BaseMSINetPage : System.Web.UI.Page
    {
        public BaseMSINetPage()
        {
        }

        protected ClientPreferences _clientPrefs;
        protected Client _clientInfo;
        protected ArrayList _clients = new ArrayList();
        protected Hashtable _punchExceptions = null;
        protected ArrayList _punchMaintenanceReasons = null;
        protected bool _isPDA = false;
        protected bool _isAuthorized = false;
        protected bool _logOff = false;
        protected bool _pageRequiresClientInfo = true;

        public Client ClientInfo
        {
            get{ return _clientInfo; }
            set { _clientInfo = value; }
        }
        protected ArrayList _minimumWageHistory = null;

        protected override void OnLoad(EventArgs e)
        {
            if (!this.IsAuthorizedAccess())
            {
                Response.Redirect("~/auth/Unauthorized.aspx");
            }
            
            InitSession();
            /* set the timeout period to 8 minutes */
            //Context.Session.Timeout = 3;


            if (!(Request.CurrentExecutionFilePath.IndexOf("/auth/Unauthorized.aspx") >= 0) && Context.User.Identity.IsAuthenticated)
            {
                if (_clientInfo == null)
                {
                    //if client is null then user is unauthorized.
                    Response.Redirect("~/auth/Unauthorized.aspx");
                }
            }

            // Be sure to call the base class's OnLoad method!
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.SecureForm();
            // Be sure to call the base class's OnPreRender method!
            base.OnPreRender(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            if (!this._logOff)
            {
                this.SaveSession();
            }
            // Be sure to call the base class's OnLoad method!
            base.OnUnload(e);
        }

        protected void LogOff()
        {
            if (this._logOff)
            {
                Context.Items.Clear();
                Context.Items.Add("LogOff", true);
                Server.Transfer("~/anon/Login.aspx", false);
            }
        }

        protected void InitSession()
        {
            //set whether the source is pda
            if (Request.Cookies["pda"] != null)
            {
                string pda = Request.Cookies["pda"].Value;
                if (pda == "true")
                {
                    _isPDA = true;
                    Session["PDA"] = true;
                }
            }

            if (Context.User.Identity.IsAuthenticated)
            {
                object cacheTest = null;
                StaticDataBL staticDataBL = new StaticDataBL();

                //punch exceptions
                cacheTest = Application["PunchExceptions"];
                if (cacheTest == null)
                {
                    try
                    {
                        //get the punch exceptions
                        _punchExceptions = staticDataBL.GetPunchExceptions();
                        Application["PunchExceptions"] = _punchExceptions;
                    }
                    catch (Exception e)
                    {
                        _punchExceptions = null;
                    }
                }
                else
                {
                    _punchExceptions = (Hashtable)cacheTest;
                }

                //minimum wage
                /*
                cacheTest = Application["MinimumWageHistory"];
                if (cacheTest == null)
                {
                    try
                    {
                        //Get the minimum wage
                        _minimumWageHistory = staticDataBL.GetMinimumWageHistory();
                        Application["MinimumWageHistory"] = _minimumWageHistory;
                    }
                    catch (Exception e)
                    {
                        _minimumWageHistory = null;
                        throw ex;
                    }
                }
                else
                {
                    _minimumWageHistory = (ArrayList)cacheTest;
                }
                */

                //get the session info
                //get the client info for this user
                try
                {
                    if (Session["ClientInfo"] == null)
                    {
                        ClientBL clientBL = new ClientBL();
                        //_clientInfo = clientBL.GetClientByUserName(Context.User.Identity.Name);
                        _clients = clientBL.GetClientsByUserName(Context.User.Identity.Name);
                        Session["Clients"] = _clients;
                        this.SetPreferredClient();
                        //_clientInfo = clientBL.GetClientIn
                        if( _clientInfo != null )
                            Session["ClientInfo"] = _clientInfo;
                        _clientPrefs = clientBL.GetClientPreferencesByID(_clientInfo.ClientID);
                        Session["ClientPrefs"] = _clientPrefs;
                        /* get client preferences */
                    }
                    else
                    {
                        _clientInfo = (Client)Session["ClientInfo"];
                        _clients = (ArrayList)Session["Clients"];
                        _clientPrefs = (ClientPreferences)Session["ClientPrefs"];
                    }
                }
                catch (Exception ex)
                {
                    Session["ClientInfo"] = null;
                    Session["ClientPrefs"] = null;
                    Session["Clients"] = null;
                }
            }
        }

        protected void SetPreferredClient()
        {
            foreach (Client client in _clients)
            {
                if (client.PreferredClient)
                {
                    _clientInfo = client;
                    break;
                }
            }
        }
        protected void LoadClientLocationTypes()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                try
                {
                    if (this._clientInfo == null)
                    {
                        this.InitSession();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void LoadClientShiftTypes(int loc = 0)  
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                //get the session info
                //get the client info for this user
                try
                {
                    if (this._clientInfo == null)
                    {
                        this.InitSession();
                    }
                    //load the client shift types
                    if (this._clientInfo.ShiftTypes.Count == 0 ) //|| this._clientInfo.Departments.Count == 0)
                    {
                        ClientBL clientBL = new ClientBL();
                        _clientInfo = clientBL.GetClientShiftTypes(_clientInfo, loc);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void GetClientShifts(int locIdx)
        {
            try
            {
                //get the shifts
                ClientBL clientBL = new ClientBL();
                _clientInfo = clientBL.GetClientShiftsByLocation(_clientInfo, locIdx);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected ArrayList GetClientDepartmentsByShiftType(ShiftType selectedShiftType, int loc)
        {
            if (selectedShiftType == null) return new ArrayList();
            ArrayList departments = null;
            try
            {
                //get the departments
                ClientBL clientBL = new ClientBL();
                departments = clientBL.GetClientDepartmentsByShiftType(_clientInfo, selectedShiftType, Context.User.Identity.Name, loc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return departments;
        }

        protected void SaveSession()
        {
            //store the client in session
            //Session["ClientInfo"] = _clientInfo;
            //Session["Clients"] = _clients;
            Session["PDA"] = _isPDA;
        }

        protected void EndSession()
        {
            Session["ClientInfo"] = null;
            Session["Clients"] = null;
            Session["PDA"] = null;
            Session.Clear();
            Session.Abandon();
        }

        protected virtual void SecureForm()
        {
            //default security goes here
        }

        protected virtual bool IsAuthorizedAccess()
        {
            return this._isAuthorized;
        }

        protected void SendHTMLEMail(string subject, string htmlBody, string toAddress)
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("eticket", "eticket");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mailClient.Host = "smtp.msistaff.com";
                mailClient.Port = 5190;
                message = new System.Net.Mail.MailMessage("eticket@msistaff.com", toAddress);
                message.Subject = subject + toAddress.ToString();
                message.Body = htmlBody;
                message.IsBodyHtml = true;
                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }
    }
}