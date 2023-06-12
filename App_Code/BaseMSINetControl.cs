using System;
using System.Collections;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;

namespace MSI.Web.Controls
{
    public class BaseMSINetControl : System.Web.UI.UserControl
    {
        
        protected bool _isPDA = false;
        protected int _clientId = 0;
        //private ClientPreferences _clientPrefs;

        public BaseMSINetControl()
        {
            //if (Session["ClientPrefs"] != null)
            //    ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
        }
        /*
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
        */
        protected override void OnLoad(EventArgs e)
        {
            if (Session["ClientInfo"] != null)
            {
                Client clientInfo = (Client)Session["ClientInfo"];
                _clientId = clientInfo.ClientID;
            }
            //if (Session["ClientPrefs"] != null)
            //    ClientPrefs = (ClientPreferences)Session["ClientPrefs"];

            CheckForPDA();
            // Be sure to call the base class's OnLoad method!
            base.OnLoad(e);
        }

        protected void CheckForPDA()
        {
            if (Session["PDA"] != null)
            {
                _isPDA = (bool)Session["PDA"];
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.SecureControl();
            // Be sure to call the base class's OnPreRender method!
            base.OnPreRender(e);
        }

        protected virtual void SecureControl()
        {
            //default secudsarity goes here
        }

        protected ArrayList GetClientDepartmentsByShiftType(Client clientInfo, ShiftType selectedShiftType)
        {
            ArrayList departments = null;
            try
            {
                //get the departments
                ClientBL clientBL = new ClientBL();
                departments = clientBL.GetClientDepartmentsByShiftType(clientInfo, selectedShiftType);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return departments;
        }
    }
}