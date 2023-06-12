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
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;

namespace MSI.Web.Controls
{
    public partial class MSINetEmployeeHistory : BaseMSINetControl
    {
        private static DateTime DATE_NOT_SET = new DateTime(1, 1, 1);
        private int _locationId;
        private int _departmentId;
        private int _shiftTypeId;
        private Client _clientInfo;// = new Client();
        private ClientPreferences _clientPrefs;// = new ClientPreferences();
        private EmployeeHistory _employeeHistory = new EmployeeHistory();
        private EmployeeWorkSummary _workSummary = new EmployeeWorkSummary();
        private DateTime _punchIn = new DateTime(1, 1, 1);
        private DateTime _punchOut = new DateTime(1, 1, 1);
        private decimal _summaryHours = 0M;
        private decimal _totalHours = 0M;
        private DateTime _nextPunchDateTime = new DateTime(1, 1, 1);
        private DateTime _punchDateTime = new DateTime(1, 1, 1);
        private TimeSpan _difference = new TimeSpan();
        HelperFunctions _helper = new HelperFunctions();
        ShiftType _selectedShiftType = null;
        string _shiftClientID = string.Empty;
        string _badgeNumber = string.Empty;
        DateTime _weekEndingDate = new DateTime(1, 1, 1);
        Hashtable _cachedDepartments = new Hashtable();
        DateTime _firstPunch = DATE_NOT_SET;
        DateTime _lastPunch = DATE_NOT_SET;

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

        public int LocationID
        {
            get
            {
                return _locationId;
            }
            set
            {
                _locationId = value;
            }
        }
        public int DepartmentID
        {
            get
            {
                return _departmentId;
            }
            set
            {
                _departmentId = value;
            }
        }

        public int ShiftTypeID
        {
            get
            {
                return _shiftTypeId;
            }
            set
            {
                _shiftTypeId = value;
            }
        }

        public string BadgeNumber
        {
            get
            {
                return _badgeNumber;
            }
            set
            {
                _badgeNumber = value;
            }
        }

        public DateTime WeekEndingDate
        {
            get
            {
                return _weekEndingDate;
            }
            set
            {
                _weekEndingDate = value;
                this.ctlWeekEnding.SelectedDate = _weekEndingDate;
            }
        }

        public void LoadEmployeeHistory()
        {
            //check that client preferences are loaded...
            if (ClientPrefs == null)
                ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
            //validate that the week ending date is a Sunday.
            EmployeeHistory employeeHistoryIn = new EmployeeHistory();
            employeeHistoryIn.TempNumber = this._badgeNumber;
            employeeHistoryIn.EndDateTime = _weekEndingDate;
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);
            employeeHistoryIn.StartDateTime = employeeHistoryIn.EndDateTime.Subtract(ts);
            employeeHistoryIn.EndDateTime = DateTime.Parse(this.ctlWeekEnding.SelectedDate.ToString("MM/dd/yyyy") + " 23:59:59");
            employeeHistoryIn.ClientID = _clientInfo.ClientID;
            EmployeeHistoryBL employeeHistoryBL = new EmployeeHistoryBL();
            _employeeHistory = employeeHistoryBL.GetEmployeeHistory(employeeHistoryIn, Context.User);
            this.txtTempNumber.Text = this._badgeNumber;
            this.lblBadge.Text = _employeeHistory.TempNumber;
            this.lblName.Text = _employeeHistory.LastName + ", " + _employeeHistory.FirstName;
            this.lblBadge.Visible = true;
            this.lblBadgeHeading.Visible = true;
            this.lblName.Visible = true;
            this.lblNameHeading.Visible = true;
            this.rptrEmployeeResults.DataSource = _employeeHistory.WorkSummaries;
            this.rptrEmployeeResults.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClientPrefs == null)
            {
                ClientPrefs = (ClientPreferences)this.Session["ClientPrefs"];
            }
            if (!this.IsPostBack)
            {
                //default to the current week end date
                if (this.lblBadge.Text.Length == 0)
                {
                    this.lblBadge.Visible = false;
                    this.lblBadgeHeading.Visible = false;
                }
                if (this.lblName.Text.Length == 0)
                {
                    this.lblName.Visible = false;
                    this.lblNameHeading.Visible = false;
                }
            }

            if (_weekEndingDate == null || _weekEndingDate.Date == new DateTime(1, 1, 1).Date)
            {
                //get the week ending date
                this._weekEndingDate = DateTime.Parse(_helper.GetCSTCurrentWeekEndingDate().ToString("MM/dd/yyyy") + " 00:00:00");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.txtTempNumber.Focus();
        }

        protected void rptrEmployeeResults_ItemCommand(Object src, RepeaterCommandEventArgs e)
        {
            //move the swipes to the new department
            EmployeePunchMaintenanceBL maintenanceBL = null;
            EmployeePunchMaintenanceResult result = null;
            EmployeePunchMove movePunchInfo = null;
            EmployeePunch deletePunchInfo = null;

            try
            {
                if (e.CommandName.ToLower() == "move")
                {
                    //delete the punch record
                    movePunchInfo = new EmployeePunchMove();
                    string checkInId = ((HiddenField)e.Item.FindControl("hdnCheckInPunchID")).Value;
                    string checkOutId = ((HiddenField)e.Item.FindControl("hdnCheckOutPunchID")).Value;

                    movePunchInfo.MovePunchList = checkInId;
                    if (checkOutId != null && checkOutId.Length > 0 && checkOutId != "-1" && checkOutId != "0")
                    {
                        movePunchInfo.MovePunchList += "," + checkOutId;
                    }

                    movePunchInfo.MoveToDepartment.DepartmentID = int.Parse(((DropDownList)e.Item.FindControl("cboDepartment")).SelectedValue);
                    movePunchInfo.MoveDateTime = _helper.GetCSTCurrentDateTime();
                    maintenanceBL = new EmployeePunchMaintenanceBL();
                    result = maintenanceBL.MoveEmployeePunch(movePunchInfo, Context.User);
                }

                else if (e.CommandName.ToLower() == "deletecheckin" || e.CommandName.ToLower() == "deletecheckout")
                {
                    //delete the punch record
                    deletePunchInfo = new EmployeePunch();
                    string checkInId = ((HiddenField)e.Item.FindControl("hdnCheckInPunchID")).Value;
                    string checkOutId = ((HiddenField)e.Item.FindControl("hdnCheckOutPunchID")).Value;

                    if (e.CommandName.ToLower() == "deletecheckin")
                    {
                        deletePunchInfo.EmployeePunchID = int.Parse(checkInId);
                    }
                    else if ( e.CommandName.ToLower() == "deletecheckout" )
                    {
                        deletePunchInfo.EmployeePunchID = int.Parse(checkOutId);
                    }

                    maintenanceBL = new EmployeePunchMaintenanceBL();
                    result = maintenanceBL.DeleteEmployeePunch(deletePunchInfo, Context.User);
                }

                //reload
                this.LoadEmployeeHistory();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //error saving/deleting record
            }
        }

        protected void rptrEmployeeResults_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                /* -1 because list is zero indexed */
                int shiftId = ((EmployeeWorkSummary)e.Item.DataItem).ShiftTypeInfo.ShiftTypeId-1;
                _workSummary = (EmployeeWorkSummary)e.Item.DataItem;

                if (ClientPrefs.EmployeeHistoryExactPunchTimes)
                    _punchIn = (DateTime)_workSummary.CheckInDateTime;
                else _punchIn = (DateTime)_workSummary.RoundedCheckInDateTime;
                if (_firstPunch.Equals(DATE_NOT_SET))
                    _firstPunch = _punchIn;

                ((Label)e.Item.FindControl("lblCheckIn")).Text = _punchIn.ToString("MM/dd/yyyy h:mm:ss tt");
                if (_workSummary.CheckOutDateTime != null && _workSummary.CheckOutDateTime.Date != new DateTime(1, 1, 1).Date)
                {
                    if (ClientPrefs.EmployeeHistoryExactPunchTimes)
                        _punchOut = (DateTime)_workSummary.CheckOutDateTime;
                    else _punchOut = (DateTime)_workSummary.RoundedCheckOutDateTime;
                    _lastPunch = _punchOut;
                    ((Label)e.Item.FindControl("lblCheckOut")).Text = _punchOut.ToString("MM/dd/yyyy h:mm:ss tt");
                    //sum up the hours JHM
                    _difference = _punchOut - _punchIn;
                    _summaryHours = Math.Round(Convert.ToDecimal(_difference.TotalMinutes / 60), 2);
                    /* subtract break time from summaryHours var */
                    _totalHours += _summaryHours;
                    ((Label)e.Item.FindControl("lblTotalHours")).Text = _summaryHours.ToString();
                }
                else
                {
                    ((Label)e.Item.FindControl("lblCheckOut")).Text = "N/A";
                    ((Label)e.Item.FindControl("lblTotalHours")).Text = "N/A";
                    ((ImageButton)e.Item.FindControl("btnImageDeleteCheckOut")).Visible = false;
                    //((Button)e.Item.FindControl("btnCheckOut")).Text = "Record Check Out";
                }

                Label lblshift = (Label)e.Item.FindControl("lblShift");
                Label lblDepartment = (Label)e.Item.FindControl("lblDepartment");

                lblshift.Text = _workSummary.ShiftTypeInfo.ShiftTypeDesc;
                lblDepartment.Text = _workSummary.DepartmentInfo.DepartmentName;

                //DropDownList cboShift = (DropDownList)e.Item.FindControl("cboShift");
                DropDownList cboDepartment = (DropDownList)e.Item.FindControl("cboDepartment");

                if (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager") && !Context.User.IsInRole("ManagerNoPunchIn"))
                {
                    //lblshift.Visible = true;
                    lblDepartment.Visible = true;
                    //cboShift.Visible = false;
                    cboDepartment.Visible = false;
                    lblshift.Text = _workSummary.ShiftTypeInfo.ToString();
                    lblDepartment.Text = _workSummary.DepartmentInfo.ToString();
                    
                    ((HtmlTableCell)e.Item.FindControl("tdAction")).Visible = false;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdAction")).Visible = true;
                    //lblshift.Visible = false;
                    lblDepartment.Visible = false;
                    //cboShift.Visible = true;
                    cboDepartment.Visible = true;

                    //wire up the event
                    //cboShift.SelectedIndexChanged += new EventHandler(cboShift_SelectedIndexChanged);

                    //load the shift types
                    //if (this.IsPostBack && _shiftClientID == cboShift.ClientID && _selectedShiftType != null)
                    //{
                    //    this.loadShiftTypeCombo(cboShift, _clientInfo.ShiftTypes, _selectedShiftType);
                        //load the department drop-down
                    //    this.loadDepartmentCombo(cboDepartment, base.GetClientDepartmentsByShiftType(_clientInfo, _selectedShiftType), _workSummary.DepartmentInfo.DepartmentID.ToString());
                    //}
                    //else
                    //{
                        //this.loadShiftTypeCombo(cboShift, _clientInfo.ShiftTypes, _workSummary.ShiftTypeInfo);
                        this.loadDepartmentCombo(cboDepartment, (ShiftType)_clientInfo.ShiftTypes[shiftId], _workSummary.DepartmentInfo.DepartmentID.ToString());
                    //}

                    //move confirm
                    ((ImageButton)e.Item.FindControl("btnImageMove")).Attributes.Add("onclick", "return confirm_move();");
                    ((LinkButton)e.Item.FindControl("lnkMove")).Attributes.Add("onclick", "return confirm_move();");
                    
                    //delete confirm
                    ((ImageButton)e.Item.FindControl("btnImageDeleteCheckIn")).Attributes.Add("onclick", "return confirm_delete();");
                    if (((ImageButton)e.Item.FindControl("btnImageDeleteCheckOut")).Visible)
                    {
                        ((ImageButton)e.Item.FindControl("btnImageDeleteCheckOut")).Attributes.Add("onclick", "return confirm_delete();");
                    }
                }

            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager") && !Context.User.IsInRole("ManagerNoPunchIn"))
                {
                    ((HtmlTableCell)e.Item.FindControl("tdActionHeader")).Visible = false;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdActionHeader")).Visible = true;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                {
                    ((Label)e.Item.FindControl("lblPeriodTotal")).Text = _totalHours.ToString();
                }
            }
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            this._badgeNumber = txtTempNumber.Text;
            this._weekEndingDate = this.ctlWeekEnding.SelectedDate;
            this.lblBadge.Visible = true;
            this.lblBadgeHeading.Visible = true;
            this.lblName.Visible = true;
            this.lblNameHeading.Visible = true;
            this.LoadEmployeeHistory();
        }

        protected void cboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList cboShift = (DropDownList)sender;
            _shiftClientID = cboShift.ClientID;
            _selectedShiftType = this.processShiftTypeChange(cboShift.SelectedValue);
            this.LoadEmployeeHistory();

        }

        private ShiftType processShiftTypeChange(string shiftTypeId)
        {
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

        private void loadShiftTypeCombo(DropDownList cboShift, ArrayList shiftTypes)
        {
            cboShift.Items.Clear();
            foreach (ShiftType shiftType in shiftTypes)
            {
                cboShift.Items.Add(new ListItem(shiftType.ToString(), shiftType.ShiftTypeId.ToString()));
            }
        }

        private void loadShiftTypeCombo(DropDownList cboShift, ArrayList shiftTypes, ShiftType shiftTypeToSelect)
        {
            cboShift.Items.Clear();
            ListItem shiftTypeItem = null;
            foreach (ShiftType shiftType in shiftTypes)
            {
                shiftTypeItem = new ListItem(shiftType.ToString(), shiftType.ShiftTypeId.ToString());
                if (shiftType.ShiftTypeId == shiftTypeToSelect.ShiftTypeId)
                {
                    shiftTypeItem.Selected = true;
                }
                else
                {
                    shiftTypeItem.Selected = false;
                }
                cboShift.Items.Add(shiftTypeItem);
            }
        }

        private void loadDepartmentCombo(DropDownList cboDepartment, ShiftType shiftType, string departmentToSelect)
        {
            this.loadDepartmentCombo(cboDepartment, shiftType);

            foreach (ListItem item in cboDepartment.Items)
            {
                if (item.Value == departmentToSelect)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
        //_cachedDepartments
        private void loadDepartmentCombo(DropDownList cboDepartment, ArrayList departments)
        {
            cboDepartment.Items.Clear();
            foreach (Department department in departments)
            {
                cboDepartment.Items.Add(new ListItem(department.DepartmentName, department.DepartmentID.ToString()));
            }
        }

        private void loadDepartmentCombo(DropDownList cboDepartment, ShiftType shiftType)
        {
            ArrayList departments = new ArrayList();
            object departmentCache = _cachedDepartments[shiftType.ShiftTypeId];
            if (departmentCache == null)
            {
                departments = base.GetClientDepartmentsByShiftType(_clientInfo, shiftType);
            }
            else
            {
                departments = (ArrayList)departmentCache;
            }
            this.loadDepartmentCombo(cboDepartment, departments);
        }
    }
}
