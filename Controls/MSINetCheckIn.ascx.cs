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
using System.Text;

namespace MSI.Web.Controls
{
    public partial class MSINetCheckIn : BaseMSINetControl
    {
        public enum CheckInDisplayMode
        {
            Swipe,
            Manual
        }

        private CheckInDisplayMode _displayMode = CheckInDisplayMode.Swipe;

        private Client _clientInfo = new Client();
        private Hashtable _punchExceptions = new Hashtable();
        private bool _cancelCheckInClick = false;
        private string _badgeNumber = "";
        private HelperFunctions _helperFunctions = new HelperFunctions();
        private DateTime _swipeDateTime;

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

        public DateTime SwipeDateTime
        {
            get
            {
                return _swipeDateTime;
            }
            set
            {
                _swipeDateTime = value;
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

        public CheckInDisplayMode DisplayMode
        {
            get
            {
                return _displayMode;
            }
            set
            {
                _displayMode = value;
            }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ID = "ctlCheckIn";
            if (this._displayMode == CheckInDisplayMode.Swipe)
            {
                this.ltrlBR1.Visible = false;
                this.ltrlBR2.Visible = false;
                this.ltrlBR3.Visible = false;
                this.lblCheckInTime.Visible = false;
                this.ctlCheckInDate.Visible = false;
                this.cboHour.Visible = false;
                this.cboMinute.Visible = false;
                this.lblColonChar.Visible = false;
                this.lblSpaceChar.Visible = false;
                this.cboAMPM.Visible = false;
            }

            if (!this.IsPostBack)
            {
                InitControl();
            }

            if (base._isPDA)
            {
                pnlValidation.Width = new Unit("230px");
                pnlConfirmation.Width = new Unit("230px");
            }
            else
            {
                pnlValidation.Width = new Unit("660px");
                pnlConfirmation.Width = new Unit("660px");
            }

            StringBuilder script = new StringBuilder();
            script.Append("<script language='JavaScript'>");
            script.Append("document.forms[0]." + this.ID.ToString() + "_txtBadgeNumber.focus()");
            script.Append("</script>");
            //Page.RegisterStartupScript("Focus", script.ToString());
        }

        protected void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (!this._cancelCheckInClick)
            {
                initializeMessage(this.lblValidationMessage);
                initializeMessage(this.lblConfirmationMessage);

                //validate that a shift was selected
                //check the badge in
                EmployeePunchSummary employeePunchSummary = new EmployeePunchSummary();
                employeePunchSummary.ClientID = _clientInfo.ClientID;
                if (txtBadgeNumber != null)
                    employeePunchSummary.TempNumber = txtBadgeNumber.Text;
                //employeePunch.ShiftTypeInfo.ShiftTypeId = int.Parse(cboShift.SelectedValue);
                if (this._displayMode == CheckInDisplayMode.Manual)
                {
                    //time is rounded
                    employeePunchSummary.PunchDateTime = DateTime.Parse(this.ctlCheckInDate.SelectedDate.ToString("MM/dd/yyyy ") + this.cboHour.SelectedValue + ":" + this.cboMinute.SelectedValue + ":00 " + this.cboAMPM.SelectedValue);
                    employeePunchSummary.ManualOverride = true;
                }
                else
                {
                    employeePunchSummary.PunchDateTime = _helperFunctions.GetCSTCurrentDateTime();
                    //employeePunchSummary.PunchDateTime = new DateTime(2017, 12, 21, 8, 46, 20);
                    //employeePunchSummary.PunchDateTime = new DateTime(2017, 12, 21, 8, 16, 59, 500);
                    //employeePunchSummary.PunchDateTime = new DateTime(2017, 12, 21, 23, 46, 59, 500);
                    //employeePunchSummary.PunchDateTime = new DateTime(2012, 06, 07, 07, 15, 09);
                    //employeePunchSummary.PunchDateTime = new DateTime(2019, 05, 14, 15, 08, 34);
                    //employeePunchSummary.PunchDateTime = new DateTime(2019, 05, 15, 17, 6, 34);
                    employeePunchSummary.ManualOverride = false;
                }

                if ( (_clientId >= 325 && _clientId <= 327) || _clientId == 302)
                {
                    //employeePunchSummary.PunchDateTime = new DateTime(2016, 07, 17, 23, 17, 0);
                    //employeePunchSummary.PunchDateTime = new DateTime(2016, 07, 17, 23, 11, 0);
                    //employeePunchSummary.PunchDateTime = new DateTime(2016, 07, 17, 23, 27, 0);
                    /* this call is too soon, we should round the punch based on shift start, which we don't yet know */
                    employeePunchSummary.RoundedPunchDateTime = _helperFunctions.GetExact15PunchTime(employeePunchSummary.PunchDateTime);
                }
                else
                    employeePunchSummary.RoundedPunchDateTime = _helperFunctions.GetRoundedPunchTime(employeePunchSummary.PunchDateTime);

                EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                DateTime checkInStart = DateTime.Now;

                EmployeePunchResult punchResult = null;
                if (_clientId != 3020)
                    punchResult = employeePunchBL.RecordEmployeePunch(employeePunchSummary, Context.User);
                else  /* JBSS will be first client with new punch recording */
                    punchResult = employeePunchBL.RecordEmployeePunchNew(employeePunchSummary, Context.User);

                string departmentName = "General Labor";
                if (punchResult.PunchSuccess)
                {
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
                    PunchException punchException = (PunchException)_punchExceptions[punchResult.PunchException];
                    this.lblValidationMessage.Visible = true;
                    this.lblValidationMessage.Text = "Check in NOT SUCCESSFUL.  " + punchException.PunchExceptionMessage;
                }

                //initialize the badge field for a new check-in
                if (this.txtBadgeNumber != null)
                {
                    this.txtBadgeNumber.Text = "";
                    this.txtBadgeNumber.Focus();
                }
                TimeSpan timeSpent = DateTime.Now - checkInStart;
                lblTimeInterval.Text = "Time Interval: " + timeSpent.TotalSeconds;
            }
        }

        protected void InitControl()
        {
            initializeMessage(this.lblValidationMessage);
            initializeMessage(this.lblConfirmationMessage);

            if (this._displayMode == CheckInDisplayMode.Manual)
            {
                if( this.txtBadgeNumber != null )
                    this.txtBadgeNumber.Text = this._badgeNumber;
                this.ctlCheckInDate.SelectedDate = this._swipeDateTime;
                this.LoadCheckInTimes();
            }
            //load the shift drop down
            //foreach ( ShiftType shiftType in _clientInfo.ShiftTypes)
            //{
            //    this.cboShift.Items.Add(new ListItem(shiftType.ToString(), shiftType.ShiftTypeId.ToString()));
            //}
        }

        private void initializeMessage(Label lbl)
        {
            lbl.Visible = false;
            lbl.Text = "";
        }
        protected void txtBadgeNumber_TextChanged(object sender, EventArgs e)
        {
            this.btnCheckIn_Click(sender, e);
            this._cancelCheckInClick = true;
        }

        protected void LoadCheckInTimes()
        {
            //default to the current time
            DateTime currentRounded = _helperFunctions.GetRoundedPunchTime(_helperFunctions.GetCSTCurrentDateTime());
            int currenthour = currentRounded.Hour;

            if (currenthour > 12)
            {
                currenthour -= 12;
            }

            for (int hrIdx = 1; hrIdx <= 12; ++hrIdx)
            {
                this.cboHour.Items.Add(new ListItem(hrIdx.ToString(), hrIdx.ToString()));

                if (currenthour == hrIdx)
                {
                    this.cboHour.SelectedIndex = hrIdx-1;
                }

            }

            string itemValue = null;
            for (int minIdx = 0; minIdx < 4; ++minIdx)
            {
                switch (minIdx)
                {
                    case 0:
                        itemValue = "00";
                        break;
                    case 1:
                        itemValue = "15";
                        break;
                    case 2:
                        itemValue = "30";
                        break;
                    case 3:
                        itemValue = "45";
                        break;
                }

                this.cboMinute.Items.Add(new ListItem(itemValue, itemValue));

                if (currentRounded.Minute.ToString() == itemValue)
                {
                    this.cboMinute.SelectedIndex = minIdx;
                }
                
            }
            this.cboAMPM.Items.Add(new ListItem("AM", "AM"));
            this.cboAMPM.Items.Add(new ListItem("PM", "PM"));

            if (currentRounded.Hour < 12)
            {
                this.cboAMPM.SelectedIndex = 0;
            }
            else
            {
                this.cboAMPM.SelectedIndex = 1;
            }

            //this.ctlCheckInDate.SelectedDate = _helperFunctions.GetCSTCurrentDateTime().Date;
        }
    }
}