using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace MSI.Web.MSINet
{
    public partial class TicketSummary : BaseMSINetPage
    {
        private HelperFunctions _helper = new HelperFunctions();
        private bool _sendEmail = false;
        //private Department _selectedDepartment = null;
        private ArrayList _departments = new ArrayList();
        private ArrayList _shifts = new ArrayList();

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            //base.LoadClientLocationTypes();
            ctlSubHeader.SectionHeader = _clientInfo.ToString();
            ctlSubHeader.ClientInfo = _clientInfo;
            ctlSubHeader.Clients = _clients;
            ctlSubHeader.ClientPrefs = _clientPrefs;
            btnSwap.Attributes.Add("OnClick", "return false;");
            if (!this.IsPostBack)
            {
                // load the locations
                int locIdx = this.loadLocationTypeCombo(_clientInfo.Location);
                base.LoadClientShiftTypes(locIdx);
                //load the shift types and departments assume all locations initially 
                int idx = this.loadShiftTypeCombo(_clientInfo.ShiftTypes);
                _departments = base.GetClientDepartmentsByShiftType((ShiftType)_clientInfo.ShiftTypes[idx-1], locIdx);
                //Session["departments"] = _departments;
                this.loadDepartmentCombo(_departments, (ShiftType)_clientInfo.ShiftTypes[idx - 1], locIdx);    
                this.ctlTicketTracker.Visible = false;
                
                //check if we have returned from 
                if (Context.Items["modeTag"] != null)
                {
                    string modeTag = (string)Context.Items["modeTag"];
                    if (modeTag == "employeeSummaryReturn" || modeTag == "ticketSummaryReload" || modeTag == "ticketSummaryChangeTimes" || modeTag == "employeeHistoryReturn")
                    {
                        //get the fields out of context
                        int locationId = int.Parse((string)Context.Items["locationId"]);
                        int departmentId = int.Parse((string)Context.Items["departmentId"]);
                        int shiftType = int.Parse((string)Context.Items["shiftType"]);
                        DateTime startDate = DateTime.Parse((string)Context.Items["startDate"]);
                        this.ctlPeriodStart.SelectedDate = startDate;

                        if (shiftType != 0)
                        {
                            //default the shift drop-down
                            this.processShiftTypeChange(shiftType.ToString(), departmentId, locationId);

                            //load the ticket tracking
                            this.getTicketTracking();
                        }
                    }
                }
                else
                {
                    if (this.ctlPeriodStart.SelectedDate == null || this.ctlPeriodStart.SelectedDate.Date == new DateTime(1, 1, 1).Date)
                    {
                        this.ctlPeriodStart.SelectedDate = _helper.GetCSTCurrentDateTime();
                    }
                    //if (this.ctlPeriodStart.Value == null || this.ctlPeriodStart.Value.Length == 0)
                    //{
                    //    this.ctlPeriodStart.Value = _helper.GetCSTCurrentDateTime().ToString();
                    //}
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_clientInfo.ClientID != 205)
            {
                this.btnSendEmail.Visible = false;
            }
            else
            {
                this.btnSendEmail.Visible = true;
            }
            //Session["departments"] = _departments;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this._sendEmail)
            {
                //send the email
                foreach (Department dept in _departments)
                {
                    if (dept.DepartmentID.ToString() == this.cboDepartment.SelectedValue)
                    {
                        StringBuilder htmlBuilder = new StringBuilder();
                        StringWriter htmlWriter = new StringWriter(htmlBuilder);
                        System.Web.UI.HtmlTextWriter ticketWriter = new HtmlTextWriter(htmlWriter);
                        //MSI.Web.Controls.MSINetTicketTracker ctlEmailTicket = (MSI.Web.Controls.MSINetTicketTracker)this.LoadControl("~\\controls\\MSINetTicketTracker.ascx");
                        UserControl ctlEmailHolder = (UserControl)LoadControl("~\\controls\\EmailHolder.ascx");
                        Type ctlEmailHolderType = ctlEmailHolder.GetType();
                        PropertyInfo propTitle = ctlEmailHolderType.GetProperty("Title");
                        string checkInOut = string.Empty;
                        if (ctlPeriodStart.SelectedDate.Date < DateTime.Now.Date)
                        {
                            checkInOut = "Check Out";
                        }
                        else if (ctlPeriodStart.SelectedDate.Date > DateTime.Now.Date)
                        {
                            checkInOut = "Check In";
                        }
                        else
                        {
                            if (cboShift.SelectedValue == "1")
                            {
                                if (DateTime.Now.Hour > 12)
                                {
                                    checkInOut = "Check Out";
                                }
                                else
                                {
                                    checkInOut = "Check In";
                                }
                            }
                            else
                            {
                                if (DateTime.Now.Hour > 12)
                                {
                                    checkInOut = "Check In";
                                }
                                else
                                {
                                    checkInOut = "Check Out";
                                }
                            }
                        }

                        propTitle.SetValue(ctlEmailHolder, "Metro Staff " + dept.DepartmentName + " " + checkInOut + " for Date " + ctlPeriodStart.SelectedDate.ToString("M/dd/yyyy"), null);

                        UserControl ctlEmailTicket = (UserControl)LoadControl("~\\controls\\MSINetTicketTracker.ascx");
                        Type ctlEmailTicketType = ctlEmailTicket.GetType();
                        PropertyInfo deptProp = ctlEmailTicketType.GetProperty("DepartmentID");
                        deptProp.SetValue(ctlEmailTicket, int.Parse(this.cboDepartment.SelectedValue), null);

                        PropertyInfo locProp = ctlEmailTicketType.GetProperty("LocationID");
                        locProp.SetValue(ctlEmailTicket, int.Parse(this.cboLocation.SelectedValue), null);

                        PropertyInfo deptShift = ctlEmailTicketType.GetProperty("ShiftTypeID");
                        deptShift.SetValue(ctlEmailTicket, int.Parse(this.cboShift.SelectedValue), null);

                        PropertyInfo deptClient = ctlEmailTicketType.GetProperty("ClientInfo");
                        deptClient.SetValue(ctlEmailTicket, _clientInfo, null);

                        PropertyInfo deptClientPrefs = ctlEmailTicketType.GetProperty("ClientPrefs");
                        deptClientPrefs.SetValue(ctlEmailTicket, _clientPrefs, null);

                        PropertyInfo deptStartDate = ctlEmailTicketType.GetProperty("StartDateTime");
                        deptStartDate.SetValue(ctlEmailTicket, this.ctlPeriodStart.SelectedDate, null);

                        PropertyInfo deptTrackingType = ctlEmailTicketType.GetProperty("TrackingType");
                        deptTrackingType.SetValue(ctlEmailTicket, Enums.TrackingTypes.Roster, null);

                        //DisplayMode
                        PropertyInfo deptDisplayMode = ctlEmailTicketType.GetProperty("DisplayMode");
                        deptDisplayMode.SetValue(ctlEmailTicket, MSI.Web.Controls.MSINetTicketTracker.TrackingDisplayMode.Email, null);

                        //set the properties
                        MethodInfo method = ctlEmailTicketType.GetMethod("LoadTicketTracker");
                        method.Invoke(ctlEmailTicket, null);

                        /*
                        ctlEmailTicket.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
                        ctlEmailTicket.ShiftTypeID = int.Parse(this.cboShift.SelectedValue);
                        ctlEmailTicket.ClientInfo = _clientInfo;
                        ctlEmailTicket.StartDateTime = this.ctlPeriodStart.SelectedDate;
                        ctlEmailTicket.TrackingType = Enums.TrackingTypes.Roster;
                        ctlEmailTicket.LoadTicketTracker();
                         */

                        ctlEmailHolder.Controls.Add(ctlEmailTicket);
                        ctlEmailHolder.RenderControl(ticketWriter);
                        //ctlEmailTicket.RenderControl(ticketWriter);
                        string html = htmlBuilder.ToString();
                        try
                        {
                            base.SendHTMLEMail(dept.DepartmentName + " " + checkInOut + " for Date " + ctlPeriodStart.SelectedDate.ToString("M/dd/yyyy"), htmlBuilder.ToString(), dept.EmailAddress);
                        }
                        catch (Exception ex)
                        {
                            
                            //ignore
                        }
                    }
                }
            }
            base.Render(writer);
        }

        public override void VerifyRenderingInServerForm(Control control) 
        { 
            /* Do nothing */ 
        }        
        
        public override bool EnableEventValidation { 
            get 
            { 
                return false; 
            } 
            set 
            { 
                /* Do nothing */
            } 
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //_departments.Clear();
            //_departments = base.GetClientDepartmentsByShiftType((ShiftType)_clientInfo.ShiftTypes[0]);
            this.getTicketTracking();
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            this._sendEmail = true;
            ShiftType selectedShiftType = this.processShiftTypeChange(this.cboShift.SelectedValue);

            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, 0);
            //Session["departments"] = _departments;
            this.getTicketTracking();
        }

        protected void cboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            int locIdx = Convert.ToInt32(this.cboLocation.SelectedValue);
            ShiftType selectedShiftType = this.processShiftTypeChange(this.cboShift.SelectedValue);


            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, locIdx);
            //Session["departments"] = _departments;
            //load the department drop-down
            this.loadDepartmentCombo(_departments, selectedShiftType, locIdx);
        }

        protected void cboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            int locIdx = Convert.ToInt32(this.cboLocation.SelectedValue);

            ClientBL clientBL = new ClientBL();
            this._clientInfo.ShiftTypes.Clear();
            _clientInfo = clientBL.GetClientShiftTypes(_clientInfo, locIdx);
            this.cboShift.SelectedIndex = 0;
            ShiftType selectedShiftType = this.processShiftTypeChange("1");
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, locIdx);
            //Session["departments"] = _departments;
            //load the department drop-down
            this.loadDepartmentCombo(_departments, selectedShiftType, locIdx);
        }

        private ShiftType processShiftTypeChange(string shiftTypeId)
        {
            if (this.ctlTicketTracker.Visible)
            {
                this.ctlTicketTracker.Visible = false;
            }
            //get the selected shift type
            ShiftType selectedShiftType = null;
            foreach (ShiftType shiftType in _clientInfo.ShiftTypes)
            {
                if (shiftType.ShiftTypeId.ToString() == shiftTypeId)
                {
                    selectedShiftType = shiftType;
                    //break;
                }
            }
            return selectedShiftType;
        }

        private ShiftType processShiftTypeChange(string shiftType, int departmentId)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(shiftType);
            int locIdx = Convert.ToInt32(this.cboLocation.SelectedValue);
            foreach (ListItem item in cboShift.Items)
            {
                if (item.Value == shiftType)
                {
                    item.Selected = true;
                    //break;
                }
            }

            //load the department drop-down for the selected shifttype//JONATHANMURFEY
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, locIdx);
            this.loadDepartmentCombo(_departments, departmentId.ToString());

            return selectedShiftType;
        }

        private ShiftType processShiftTypeChange(string shiftType, int departmentId, int locIdx)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(shiftType);

            foreach (ListItem item in cboShift.Items)
            {
                if (item.Value == shiftType)
                {
                    item.Selected = true;
                    //break;
                }
            }

            //load the department drop-down for the selected shifttype//JONATHANMURFEY
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, locIdx);
            this.loadDepartmentCombo(_departments, departmentId.ToString());

            foreach( ListItem item in cboLocation.Items )
            {
                if( item.Value == locIdx.ToString() )
                {
                    item.Selected = true;
                }
            }

            return selectedShiftType;
        }

        private void getTicketTracking()
        {
            //set the shift type
            this.ctlTicketTracker.LocationId = int.Parse(this.cboLocation.SelectedValue);
            this.ctlTicketTracker.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
            this.ctlTicketTracker.ShiftTypeID = int.Parse(this.cboShift.SelectedValue);
            this.ctlTicketTracker.ClientInfo = _clientInfo;
            this.ctlTicketTracker.ClientPrefs = _clientPrefs;
            this.ctlTicketTracker.StartDateTime = this.ctlPeriodStart.SelectedDate;
            this.ctlTicketTracker.ClearEmptyRows = this.chkClearEmptyRows.Checked;
            this.cboDepartment.Enabled = true;
            this.cboShift.Enabled = true;
            if (this.mnuTrackingType.SelectedItem.Value == "Roster")
            {
                this.ctlTicketTracker.TrackingType = Enums.TrackingTypes.Roster;
                this.ctlTicketTracker.LoadTicketTracker();
                this.ctlTicketTracker.Visible = true;
                this.ctlTicketTrackerException.Visible = false;
            }
            else if (this.mnuTrackingType.SelectedItem.Value == "DayLabor")
            {
                this.ctlTicketTracker.TrackingType = Enums.TrackingTypes.DayLabor;
            }
            else if (this.mnuTrackingType.SelectedItem.Value == "Exceptions")
            {
                this.ctlTicketTrackerException.ClientInfo = _clientInfo;
                this.ctlTicketTrackerException.StartDateTime = this.ctlPeriodStart.SelectedDate;
                this.cboDepartment.Enabled = false;
                this.cboShift.Enabled = false;
                this.ctlTicketTrackerException.LoadTicketTrackerExceptions();
                this.ctlTicketTrackerException.Visible = true;
                this.ctlTicketTracker.Visible = false;
            }
        }
        private int loadLocationTypeCombo(Dictionary<int, string> locs)
        {
            string userName = Context.User.Identity.Name.ToLower();
            this.cboLocation.Items.Clear();
            //if (locs.Count > 1)
            //{
            //    this.cboLocation.Items.Add(new ListItem("ALL LOCATIONS", "-1"));
            //}
            foreach (KeyValuePair<int, String> loc in locs)
            {
                if (userName.Equals("palomoc") && loc.Key != 354) continue;
                if (userName.Equals("cruzd") && loc.Key != 355) continue;
                if (userName.Equals("villalpandoa") && loc.Key != 354) continue;
                if (userName.Equals("namecheb") && loc.Key != 358) continue;
                if (userName.Equals("azconaf") && loc.Key != 356 && loc.Key != 357) continue;
                if (userName.Equals("carmonaj") && loc.Key != 354) continue;
                if (userName.Equals("ibarram") && loc.Key != 354) continue;

                if (userName.Equals("rodriguezr") && loc.Key != 355) continue;
                if (userName.Equals("lasartee") && loc.Key != 355) continue;
                if (userName.Equals("cortezs") && loc.Key != 355) continue;
                if (userName.Equals("maldonadoe") && loc.Key != 355) continue;
                if (userName.Equals("santillaneso") && loc.Key != 355) continue;
                if (userName.Equals("hernandezs") && loc.Key != 355) continue;
                this.cboLocation.Items.Add(new ListItem(loc.Value.ToString(), loc.Key.ToString()));
            }
            return Convert.ToInt32(this.cboLocation.Items[0].Value);
        }

        private int loadLocationTypeCombo(Dictionary<int, string> locs, int locIdx)
        {
            this.cboLocation.Items.Clear();
            string userName = Context.User.Identity.Name.ToLower();

            if (locs.Count > 1)
            {
                if( !userName.Equals("palomoc") && !userName.Equals("carmonaj") && !userName.Equals("namecheb") )
                    this.cboLocation.Items.Add(new ListItem("ALL LOCATIONS", "-1"));
            }
            foreach (KeyValuePair<int, String> loc in locs)
            {
                if (userName.Equals("palomoc") && loc.Key != 354) continue;
                if (userName.Equals("cruzd") && loc.Key != 355) continue;
                if (userName.Equals("villalpandoa") && loc.Key != 354) continue;
                if (userName.Equals("namecheb") && loc.Key != 358) continue;
                if (userName.Equals("azconaf") && loc.Key != 356 && loc.Key != 357) continue;
                if (userName.Equals("carmonaj") && loc.Key != 354) continue;
                if (userName.Equals("ibarram") && loc.Key != 354) continue;

                if (userName.Equals("rodriguezr") && loc.Key != 355) continue;
                if (userName.Equals("lasartee") && loc.Key != 355) continue;
                if (userName.Equals("cortezs") && loc.Key != 355) continue;
                if (userName.Equals("maldonadoe") && loc.Key != 355) continue;
                if (userName.Equals("santillaneso") && loc.Key != 355) continue;
                if (userName.Equals("hernandezs") && loc.Key != 355) continue;

                this.cboLocation.Items.Add(new ListItem(loc.Value.ToString(), loc.Key.ToString()));
            }
            cboLocation.Items.FindByValue(locIdx.ToString()).Selected = true;
            return Convert.ToInt32(this.cboLocation.Items[0].Value);
        }

        private int loadShiftTypeCombo(ArrayList shiftTypes)
        {
            this.cboShift.Items.Clear();
            string name = Context.User.Identity.Name.ToLower();
            foreach (ShiftType shiftType in shiftTypes)
            {
                if (name.Equals("lasartee") && shiftType.ShiftTypeId != 6 )
                    continue;
                else if (name.Equals("cortezs") && shiftType.ShiftTypeId != 5)
                    continue;
                //else if (name.Equals("salazarj") && shiftType.ShiftTypeId != 1)
                //    continue;
                else if (name.Equals("maldonadoe") && shiftType.ShiftTypeId != 7)
                    continue;
                else if (name.Equals("carmonaj") && shiftType.ShiftTypeId != 5)
                    continue;
                else if (name.Equals("cruzd") && shiftType.ShiftTypeId != 4)
                    continue;
                else if (name.Equals("villalpandoa") && shiftType.ShiftTypeId != 7)
                    continue;                
                else if (name.Equals("jackmand") && shiftType.ShiftTypeId != 6)
                    continue;
                else if (name.Equals("fraleyt") && shiftType.ShiftTypeId != 2)
                    continue;
                else if (name.Equals("nievag") && shiftType.ShiftTypeId != 7)
                    continue;
                else if (name.Equals("rodriguezj") && shiftType.ShiftTypeId != 4)
                    continue;
                else if (name.Equals("ibarram") && shiftType.ShiftTypeId != 4)
                    continue;
                else if (name.Equals("palomoc") && shiftType.ShiftTypeId != 6)
                    continue;
                else
                    this.cboShift.Items.Add(new ListItem(shiftType.ToString(), shiftType.ShiftTypeId.ToString()));
            }
            return Convert.ToInt32(this.cboShift.Items[0].Value);
        }

        private void loadDepartmentCombo(ArrayList departments, string departmentToSelect)
        {
            this.cboDepartment.Items.Clear();
            ListItem item = null;
            if (departments.Count > 0)
            {
                //item = new ListItem("All Departments", "0");
                //this.cboDepartment.Items.Add(item);
            }
            foreach (Department department in departments)
            {
                item = new ListItem(department.DepartmentName, department.DepartmentID.ToString());
                if (this.cboDepartment.Items.Contains(item))
                    continue;
                this.cboDepartment.Items.Add(item);
                if (item.Value == departmentToSelect)
                {
                    item.Selected = true;
                }
            }
        }

        private void loadDepartmentCombo(ArrayList departments, ShiftType shift, int locationId)
        {
            //Department allDepts;
            this.cboDepartment.Items.Clear();
            if (departments.Count == 0)
            {
                Department dpt = new Department(-999, "--Unavailable--");
                departments.Add(dpt);
            }
            else if( departments.Count > 1 )
            {
                //Department allDepts = new Department(0, "Show All Departments");
                //departments.Add(allDepts);
                //this.cboDepartment.Items.Add(new ListItem("All Departments", "0"));
            }
            foreach (Department department in departments)
            {
                if (_clientInfo.ClientID == 2)
                {
                    if (department.DepartmentID == 806)
                        continue;
                }
                if( _clientInfo.ClientID == 165 && (department.DepartmentName.Equals("Addison") || department.DepartmentName.Equals("Schaumburg")))
                    continue; /* hardcoded for now because a lot of data already exists with these two department names */
                if( _clientInfo.ClientID == 127 && department.DepartmentName.Equals("PLANT 2") )
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("cruzd") && (locationId != 355))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("villalpandoa") && (locationId != 354))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("santillaneso") && 
                    !department.DepartmentName.Equals("Line Tool Team"))
                    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("namecheb") && !(department.DepartmentName.Contains("100-Grinder") || department.DepartmentName.Contains("100-Utility")))
                //    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("cortezs") && !department.DepartmentName.Contains("100"))
                //    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("maldonadoe") && !department.DepartmentName.Contains("100"))
                //    continue;
                if (Context.User.Identity.Name.ToLower().Equals("jackmand") && !department.DepartmentName.Contains("100"))
                    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("rodriguezj") && !department.DepartmentName.Contains("100"))
                //    continue;
                if (Context.User.Identity.Name.ToLower().Equals("hernandezs") && 
                    !(department.DepartmentName.Contains("QC") || 
                            department.DepartmentName.Contains("QA")))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("galvane") && !department.DepartmentName.Equals("100-Shipping"))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("cuadradoe") &&
                        !department.DepartmentName.ToUpper().Equals("500-SKID BUILDER") && !department.DepartmentName.ToUpper().Equals("300-FABRICATION") &&
                        !department.DepartmentName.ToUpper().Contains("100-UTILITY") && !department.DepartmentName.ToUpper().Equals("100-SKID BUILDER") &&
                        !department.DepartmentName.ToUpper().Contains("100-GRINDER")
                    )
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("bentonc") && !department.DepartmentName.Equals("500-Receiving"))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("holmesr") && !department.DepartmentName.Contains("500"))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("palomoc") && locationId != 354)
                    continue;
                if (_clientPrefs.ClientID == 92 && department.DepartmentID == 100)
                    continue;
                ListItem li = new ListItem(department.DepartmentName, department.DepartmentID.ToString());
                if( !this.cboDepartment.Items.Contains(li))
                    this.cboDepartment.Items.Add(li);
            }
            //Session["departments"] = departments;
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
        protected void mnuTrackingType_MenuItemClick(object sender, MenuEventArgs e)
        {
            this.getTicketTracking();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }
    }
}