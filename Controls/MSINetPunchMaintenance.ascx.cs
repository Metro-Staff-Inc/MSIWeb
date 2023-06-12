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
    public partial class MSINetPunchMaintenance : BaseMSINetControl
    {
        private Client _clientInfo = new Client();
        private int _clientRosterId;
        private DateTime _startDate = new DateTime(1, 1, 1);
        private DateTime _endDate = new DateTime(1, 1, 1);
        HelperFunctions _helper = new HelperFunctions();
        private DropDownList _cboHour;
        private DropDownList _cboMinute;
        private DropDownList _cboAMPM;
        //private DropDownList _cboMaintenanceReason;
        private EmployeePunch _employeePunch = null;
        private Enums.PunchTypes _currentPunchType = Enums.PunchTypes.CheckIn;
        private string _badgeNumber = string.Empty;
        private string _employeeName = string.Empty;
        private EmployeePunchMaintenance _maintenanceResult = new EmployeePunchMaintenance();
        private Hashtable _punchExceptions = new Hashtable();
        private ArrayList _punchMaintenanceReasons = new ArrayList();

        public Hashtable PunchExceptions
        {
            get
            {
                return _punchExceptions;
            }
            set
            {
                _punchExceptions = value;
            }
        }

        public ArrayList PunchMaintenanceReasons
        {
            get
            {
                return _punchMaintenanceReasons;
            }
            set
            {
                _punchMaintenanceReasons = value;
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

        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }
            set
            {
                _employeeName = value;
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

        public Enums.PunchTypes CurrentStatus
        {
            get
            {
                return _currentPunchType;
            }
        }

        public int ClientRosterID
        {
            get
            {
                return _clientRosterId;
            }
            set
            {
                _clientRosterId = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        public void LoadEmployeePunches()
        {

            //validate that the week ending date is a Sunday.
            EmployeePunchMaintenance maintenance = new EmployeePunchMaintenance();
            maintenance.ClientRosterID = _clientRosterId;
            maintenance.StartDateTime = this._startDate;
            maintenance.EndDateTime = this._endDate;
            EmployeePunchMaintenanceBL maintenanceBL = new EmployeePunchMaintenanceBL();
            _maintenanceResult = maintenanceBL.GetEmployeePunchMaintenance(maintenance, Context.User);
            this.rptrEmployeePunchResults.DataSource = _maintenanceResult.EmployeePunches;
            this.rptrEmployeePunchResults.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lblBadgeNumber.Text = this._badgeNumber;
            this.lblName.Text = this._employeeName;
            if (_maintenanceResult.EmployeePunches.Count % 2 != 0)
            {
                this.lblStatus.Text = "Checked In";
            }
            else if (_maintenanceResult.EmployeePunches.Count > 0)
            {
                this.lblStatus.Text = "Checked Out";
            }
            else
            {
                this.lblStatus.Text = "Not Checked In";
            }

            this.lblDate.Text = this._startDate.ToString("MM/dd/yyyy");
        }

        protected void rptrEmployeePunchResults_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblType = (Label)e.Item.FindControl("lblType");
                if (_currentPunchType == Enums.PunchTypes.CheckIn)
                {
                    lblType.Text = "CHECK IN";
                    _currentPunchType = Enums.PunchTypes.CheckOut;
                }
                else if (_currentPunchType == Enums.PunchTypes.CheckOut)
                {
                    lblType.Text = "CHECK OUT";
                    _currentPunchType = Enums.PunchTypes.CheckIn;
                }

                _employeePunch = (EmployeePunch)e.Item.DataItem;
                ((Label)e.Item.FindControl("lblDate")).Text = _employeePunch.RoundedPunchDateTime.ToString("MM/dd/yyyy");
                _cboHour = (DropDownList)e.Item.FindControl("cboHour");
                _cboMinute = (DropDownList)e.Item.FindControl("cboMinute");
                _cboAMPM = (DropDownList)e.Item.FindControl("cboAMPM");
                this.LoadCheckInTimes();

                //maintenance reason codes
                //_cboMaintenanceReason = (DropDownList)e.Item.FindControl("cboMaintenanceReason");
                //this.LoadMaintenanceReasonCodes();

                //wire up the confirm message
                ((Button)e.Item.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm_delete();");
                
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                _employeePunch = new EmployeePunch();
                DropDownList cboDate = (DropDownList)e.Item.FindControl("cboDate");
                cboDate.Items.Add(new ListItem(_startDate.ToString("MM/dd/yyyy"), _startDate.ToString("MM/dd/yyyy")));
                if (_startDate.Date != _endDate.Date)
                {
                    TimeSpan dif = DateTime.Parse(_endDate.ToString("MM/dd/yyyy 00:00:00")) - DateTime.Parse(_startDate.ToString("MM/dd/yyyy 00:00:00"));
                    DateTime recordDate = new DateTime(1, 1, 1);
                    for (int dayIdx = 1; dayIdx <= dif.Days; dayIdx++)
                    {
                        recordDate = _startDate.AddDays(dayIdx);
                        cboDate.Items.Add(new ListItem(recordDate.ToString("MM/dd/yyyy"), recordDate.ToString("MM/dd/yyyy")));
                    }
                }
                _cboHour = (DropDownList)e.Item.FindControl("cboHour");
                _cboMinute = (DropDownList)e.Item.FindControl("cboMinute");
                _cboAMPM = (DropDownList)e.Item.FindControl("cboAMPM");
                this.LoadCheckInTimes();

                //maintenance reason codes
                //_cboMaintenanceReason = (DropDownList)e.Item.FindControl("cboMaintenanceReason");
                //this.LoadMaintenanceReasonCodes();
            }
          
        }

        protected void rptrEmployeePunchResults_ItemCommand(Object src, RepeaterCommandEventArgs e)
        {
            int punchID = int.Parse(e.CommandArgument.ToString());
            EmployeePunchMaintenanceBL maintenanceBL = null;
            EmployeePunchMaintenanceResult result = null;
            EmployeePunch punchInfo = null;

            try
            {
                if (e.CommandName.ToLower() == "delete")
                {
                    //delete the punch record
                    punchInfo = new EmployeePunch();
                    punchInfo.EmployeePunchID = punchID;
                    maintenanceBL = new EmployeePunchMaintenanceBL();
                    result = maintenanceBL.DeleteEmployeePunch(punchInfo, Context.User);
                }
                else if (e.CommandName.ToLower() == "save")
                {
                    //update the time
                    DateTime punchDate = DateTime.Parse(((Label)e.Item.FindControl("lblDate")).Text);

                    DateTime punchDateTime = getSubmittedPunchDateTime(punchDate, e);
                    punchInfo = new EmployeePunch();
                    punchInfo.EmployeePunchID = punchID;
                    punchInfo.RoundedPunchDateTime = punchDateTime;
                    punchInfo.ManualOverride = true;
                    punchInfo.EmployeePunchID = punchID;
                    punchInfo.LastUpdatedDateTime = _helper.GetCSTCurrentDateTime();
                    maintenanceBL = new EmployeePunchMaintenanceBL();
                    result = maintenanceBL.SaveEmployeePunch(punchInfo, Context.User);
                }

                else if (e.CommandName.ToLower() == "add")
                {
                    DateTime punchDate = DateTime.Parse(((DropDownList)e.Item.FindControl("cboDate")).SelectedValue);
                    EmployeePunchSummary punchSummary = new EmployeePunchSummary();
                    punchSummary.ClientID = _clientInfo.ClientID;
                    punchSummary.ClientRosterID = _clientRosterId;
                    punchSummary.TempNumber = _badgeNumber;
                    punchSummary.PunchDateTime = getSubmittedPunchDateTime(punchDate, e);
                    punchSummary.RoundedPunchDateTime = punchSummary.PunchDateTime;
                    punchSummary.ManualOverride = true;
                    EmployeePunchBL punchBL = new EmployeePunchBL();
                    EmployeePunchResult punchResult = punchBL.RecordEmployeePunch(punchSummary, Context.User);
                    //check the result
                    string departmentName = "General Labor";
                    if (punchResult.PunchSuccess)
                    {
                        this.lblConfirmationMessage.Visible = true;
                        this.lblValidationMessage.Visible = false;
                        if (punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID > 0)
                        {
                            departmentName = punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentName;
                        }

                        //update the confirmation message
                        this.lblConfirmationMessage.Visible = true;
                        if (punchResult.PunchType == Enums.PunchTypes.CheckIn)
                        {
                            this.lblConfirmationMessage.Text = "Check In for Badge " + punchResult.EmployeePunchSummaryInfo.TempNumber + " SUCCESSFUL.  Please proceed to " + departmentName;
                        }
                        else if (punchResult.PunchType == Enums.PunchTypes.CheckOut)
                        {
                            this.lblConfirmationMessage.Text = "Check Out for Badge " + punchResult.EmployeePunchSummaryInfo.TempNumber + " SUCCESSFUL.";
                        }
                    }
                    else
                    {
                        this.lblConfirmationMessage.Visible = false;
                        this.lblValidationMessage.Visible = true;
                        PunchException punchException = (PunchException)_punchExceptions[punchResult.PunchException];
                        this.lblValidationMessage.Visible = true;
                        this.lblValidationMessage.Text = "Check in NOT SUCCESSFUL.  " + punchException.PunchExceptionMessage;
                    }
                }

                //reload
                this.LoadEmployeePunches();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //error saving/deleting record
            }
        }

        protected void LoadCheckInTimes()
        {
            DateTime currentRounded = new DateTime(1,1,1);
            //default to the current time
            if (_employeePunch.EmployeePunchID > 0)
            {
                currentRounded = _employeePunch.RoundedPunchDateTime;
            }
            else
            {
                currentRounded = _helper.GetRoundedPunchTime(DateTime.Parse(_startDate.ToString("MM/dd/yyyy " + DateTime.Now.ToString("HH:mm"))));
            }

            int currenthour = currentRounded.Hour;

            if (currenthour > 12)
            {
                currenthour -= 12;
            }
            else if (currenthour == 0)
            {
                currenthour = 12;
            }

            for (int hrIdx = 1; hrIdx <= 12; ++hrIdx)
            {
                _cboHour.Items.Add(new ListItem(hrIdx.ToString(), hrIdx.ToString()));

                if (currenthour == hrIdx)
                {
                    _cboHour.SelectedIndex = hrIdx - 1;
                }

            }

            string itemValue = null;
            for (int minIdx = 0; minIdx < 12; ++minIdx)
            {
                switch (minIdx)
                {
                    case 0:
                        itemValue = "00";
                        break;
                    case 1:
                        itemValue = "05";
                        break;
                    case 2:
                        itemValue = "10";
                        break;
                    case 3:
                        itemValue = "15";
                        break;
                    case 4:
                        itemValue = "20";
                        break;
                    case 5:
                        itemValue = "25";
                        break;
                    case 6:
                        itemValue = "30";
                        break;
                    case 7:
                        itemValue = "35";
                        break;
                    case 8:
                        itemValue = "40";
                        break;
                    case 9:
                        itemValue = "45";
                        break;
                    case 10:
                        itemValue = "50";
                        break;
                    case 11:
                        itemValue = "55";
                        break;
                }

                _cboMinute.Items.Add(new ListItem(itemValue, itemValue));

                if (currentRounded.Minute.ToString() == itemValue)
                {
                    _cboMinute.SelectedIndex = minIdx;
                }

            }
            _cboAMPM.Items.Add(new ListItem("AM", "AM"));
            _cboAMPM.Items.Add(new ListItem("PM", "PM"));

            if (currentRounded.Hour < 12)
            {
                _cboAMPM.SelectedIndex = 0;
            }
            else
            {
                _cboAMPM.SelectedIndex = 1;
            }

            //this.ctlCheckInDate.SelectedDate = _helperFunctions.GetCSTCurrentDateTime().Date;
        }
        /*
        protected void LoadMaintenanceReasonCodes()
        {
            _cboMaintenanceReason.Items.Clear();
            ListItem item = null;
            item = new ListItem("--Select a Reason--", "-1");
            item.Selected = true;
            _cboMaintenanceReason.Items.Add(item);
            foreach (PunchMaintenanceReason reason in _punchMaintenanceReasons)
            {
                _cboMaintenanceReason.Items.Add(new ListItem(reason.ToString(), reason.PunchMaintenanceReasonId.ToString()));
            }
        }
        */
        private DateTime getSubmittedPunchDateTime(DateTime punchDate, RepeaterCommandEventArgs e)
        {
            _cboHour = (DropDownList)e.Item.FindControl("cboHour");
            int hour = int.Parse(_cboHour.SelectedValue);
            _cboMinute = (DropDownList)e.Item.FindControl("cboMinute");
            int minute = int.Parse(_cboMinute.SelectedValue);
            _cboAMPM = (DropDownList)e.Item.FindControl("cboAMPM");
            string AMPM = _cboAMPM.SelectedValue;

            if (AMPM.ToUpper() == "PM" && hour < 12)
            {
                hour += 12;
            }

            else if (AMPM.ToUpper() == "AM" && hour == 12)
            {
                hour = 0;
            }

            return new DateTime(punchDate.Year, punchDate.Month, punchDate.Day, hour, minute, 0);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     