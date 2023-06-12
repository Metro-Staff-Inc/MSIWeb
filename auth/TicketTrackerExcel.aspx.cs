using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using MSI.Web.MSINet.Common;
using System.Collections;
using MSI.Web.MSINet.BusinessEntities;


namespace MSI.Web.MSINet
{
    public partial class TicketSummaryExcel : BaseMSINetPage
    {
        private HelperFunctions _helper = new HelperFunctions();
        private ArrayList _departments = new ArrayList();

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            if (Context.User.IsInRole("TimeClock"))
            {
                base._isAuthorized = false;
            }
            return base.IsAuthorizedAccess();
        }
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            base.LoadClientShiftTypes();

            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientPrefs = _clientPrefs;
            this.btnSwap.Attributes.Add("OnClick", "return false;");
            if (!this.IsPostBack)
            {
                //load the shift types
                int idx = this.loadShiftTypeCombo(_clientInfo.ShiftTypes);
                _departments = base.GetClientDepartmentsByShiftType((ShiftType)_clientInfo.ShiftTypes[idx - 1], 0);
                //Session["departments"] = _departments;
                this.loadDepartmentCombo(_departments, (ShiftType) _clientInfo.ShiftTypes[idx - 1], 0);
                this.ctlTicketTracker.Visible = false;

                //check if we have returned from 
                if (Context.Items["modeTag"] != null)
                {
                    string modeTag = (string)Context.Items["modeTag"];
                    if (modeTag == "employeeSummaryReturn" || modeTag == "ticketSummaryReload" || modeTag == "ticketSummaryChangeTimes" || modeTag == "employeeHistoryReturn")
                    {
                        //get the fields out of context
                        int departmentId = int.Parse((string)Context.Items["departmentId"]);
                        int shiftType = int.Parse((string)Context.Items["shiftType"]);
                        DateTime startDate = DateTime.Parse((string)Context.Items["startDate"]);
                        this.ctlPeriodStart.SelectedDate = startDate;

                        if (shiftType != 0)
                        {
                            //default the shift drop-down
                            this.processShiftTypeChange(shiftType.ToString(), departmentId);

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
            //Session["departments"] = _departments;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);
            string daysWorked = Server.UrlDecode((string)Request.QueryString["daysWorked"]);

            Response.ContentType = "application/ms-excel";
             
            Response.AddHeader("Content-Disposition", "inline;filename=TicketTracker" + _clientInfo.ClientID + "_" + dateTime + ".html");
            //this.RenderChildren(writer);
            _departments.Clear();
            _departments = base.GetClientDepartmentsByShiftType((ShiftType)_clientInfo.ShiftTypes[0], 0);
            this.getTicketTracking();

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hWriter = new HtmlTextWriter(sw);
            base.Render(hWriter);
            string html = sb.ToString();
            html = Regex.Replace(html, "<input[^>]*id=\"(__VIEWSTATE)\"[^>]*>", string.Empty, RegexOptions.IgnoreCase);
            
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            html = rRemScript.Replace(html, "");
            writer.Write(html);
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
            //this._sendEmail = true;
            ShiftType selectedShiftType = this.processShiftTypeChange(this.cboShift.SelectedValue);

            //_departments = base.GetClientDepartmentsByShiftType(selectedShiftType);
            ////Session["departments"] = _departments;
            this.getTicketTracking();
        }

        protected void cboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(this.cboShift.SelectedValue);
            
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, 0);
            //Session["departments"] = _departments;
            //load the department drop-down
            this.loadDepartmentCombo(_departments, selectedShiftType, 0);
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
                    break;
                }
            }
            return selectedShiftType;
        }

        private ShiftType processShiftTypeChange(string shiftType, int departmentId)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(shiftType);
            
            foreach (ListItem item in cboShift.Items)
            {
                if (item.Value == shiftType)
                {
                    item.Selected = true;
                    break;
                }
            }

            //load the department drop-down for the selected shifttype//JONATHANMURFEY
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, 0);
            this.loadDepartmentCombo(_departments, departmentId.ToString());

            return selectedShiftType;
        }

        private void getTicketTracking()
        {
            //set the shift type
            this.ctlTicketTracker.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
            this.ctlTicketTracker.ShiftTypeID = int.Parse(this.cboShift.SelectedValue);
            this.ctlTicketTracker.ClientInfo = _clientInfo;
            this.ctlTicketTracker.ClientPrefs = _clientPrefs;
            this.ctlTicketTracker.StartDateTime = this.ctlPeriodStart.SelectedDate;
            this.ctlTicketTracker.ClearEmptyRows = this.chkClearEmptyRows.Checked;
            this.ctlTicketTracker.FindControl("empPopupWrapper").Visible = false;
            this.ctlTicketTracker.ExportDisplayType = Web.Controls.MSINetTicketTracker.ExportType.Excel;

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

        private int loadShiftTypeCombo(ArrayList shiftTypes)
        {
            this.cboShift.Items.Clear();
            string name = Context.User.Identity.Name.ToLower();
            foreach (ShiftType shiftType in shiftTypes)
            {
                if (name.Equals("lasartee") && shiftType.ShiftTypeId != 6)
                    continue;
                else if (name.Equals("carmonaj") && shiftType.ShiftTypeId != 5)
                    continue;
                else if (name.Equals("cortezs") && shiftType.ShiftTypeId != 5)
                    continue;
                //else if (name.Equals("salazarj") && shiftType.ShiftTypeId != 1)
                //    continue;
                else if (name.Equals("maldonadoe") && shiftType.ShiftTypeId != 7)
                    continue;
                else if (name.Equals("jackmand") && shiftType.ShiftTypeId != 6)
                    continue;
                else if (name.Equals("fraleyt") && shiftType.ShiftTypeId != 2)
                    continue;
                else if (name.Equals("nievag") && shiftType.ShiftTypeId != 7)
                    continue;
                else if (name.Equals("rodriguezj") && shiftType.ShiftTypeId != 3)
                    continue;
                else if (name.Equals("cruzd") && shiftType.ShiftTypeId != 4)
                    continue;
                else if (name.Equals("villalpandoa") && shiftType.ShiftTypeId != 7)
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
            foreach (Department department in departments)
            {
                item = new ListItem(department.DepartmentName, department.DepartmentID.ToString());
                
                this.cboDepartment.Items.Add(item);
                if (item.Value == departmentToSelect)
                {
                    item.Selected = true;
                    //break;
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
            //else
            //{
               // allDepts = new Department(0, "** Show All Departments **");
               // departments.Add(allDepts);
            //}
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
                if (Context.User.Identity.Name.ToLower().Equals("cruzd") && locationId != 355)
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("villalpandoa") && locationId != 354)
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("santillaneso") &&
                    !department.DepartmentName.Equals("Line Tool Team")) continue;
                //if (Context.User.Identity.Name.ToLower().Equals("cortezs") && !department.DepartmentName.Contains("100"))
                //    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("maldonadoe") && !department.DepartmentName.Contains("100"))
                //    continue;
                if (Context.User.Identity.Name.ToLower().Equals("ibarram") && locationId != 354)
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("jackmand") && !department.DepartmentName.Contains("100"))
                    continue;
                //if (Context.User.Identity.Name.ToLower().Equals("rodriguezj") && !department.DepartmentName.Contains("100"))
                //    continue;
                if (Context.User.Identity.Name.ToLower().Equals("hernandezs") && !department.DepartmentName.Contains("QC"))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("galvane") && !department.DepartmentName.Equals("100-Shipping"))
                    continue;
                if (Context.User.Identity.Name.ToLower().Equals("cuadradoe") &&
                        !department.DepartmentName.ToUpper().Equals("500-SKID BUILDER") && !department.DepartmentName.ToUpper().Equals("300-FABRICATION") &&
                        !department.DepartmentName.ToUpper().Contains("100-UTILITY") && !department.DepartmentName.ToUpper().Equals("100-SKID BUILDER") &&
                        !department.DepartmentName.ToUpper().Contains("100-GRINDER")
                    )
                    continue;
                /* if (Context.User.Identity.Name.ToLower().Equals("jackmand") &&
                        !department.DepartmentName.ToUpper().Equals("500-SKID BUILDER") && !department.DepartmentName.ToUpper().Equals("300-FABRICATION") &&
                        !department.DepartmentName.ToUpper().Contains("100-UTILITY") && !department.DepartmentName.ToUpper().Equals("100-SKID BUILDER") &&
                        !department.DepartmentName.ToUpper().Contains("100-GRINDER")
                    )
                    continue;*/
                if (Context.User.Identity.Name.ToLower().Equals("palomoc") 
                    && (locationId != 354))
                    continue;
                if (_clientPrefs.ClientID == 92 && department.DepartmentID == 100)
                    continue;
                this.cboDepartment.Items.Add(new ListItem(department.DepartmentName, department.DepartmentID.ToString()));
            }
            //Session["departments"] = departments;
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