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
using System.Drawing;

namespace MSI.Web.Controls
{
    public partial class MSINetTicketTracker : BaseMSINetControl
    {
        public enum ExportType
        {
            None,
            Excel,
            Print,
            PeekABoo,
            PeekABooTop
        }

        public enum TrackingDisplayMode
        {
            EmployeeSummary,
            ShiftSummary,
            ChangeTimes,
            Email
        }

        private int _departmentId;
        private int _shiftTypeId;
        private int _locationId;
        private int _clientRosterId;
        private Client _clientInfo = new Client();
        private ClientPreferences _clientPrefs = new ClientPreferences();
        private HelperFunctions _helper = new HelperFunctions();
        private DateTime _startDateTime;
        private Enums.TrackingTypes _trackingType = Enums.TrackingTypes.Roster;
        private TrackingDisplayMode _displayMode = TrackingDisplayMode.ShiftSummary;
        private TicketTracker _trackerResult = new TicketTracker();
        private ExportType _exportType = ExportType.None;
        private Button btnApprove = null;
        private int _requiresApprovalCount = 0;
        private bool _userAllowedToApprove = false;
        private bool _userCannotRecordPunchIn = false;
        protected Repeater rptrPunchDetail;
        private bool _showDetail = false;
        private string _editBadgeNumber = "";

        public int LocationId
        {
            get { return _locationId; }
            set { _locationId = value; }
        }
        public TrackingDisplayMode DisplayMode
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
        public ExportType ExportDisplayType
        {
            get
            {
                return _exportType;
            }
            set
            {
                _exportType = value;
            }
        }

        public Enums.TrackingTypes TrackingType
        {
            get
            {
                return _trackingType;
            }
            set
            {
                _trackingType = value;
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return _startDateTime;
            }
            set
            {
                _startDateTime = value;
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
        public Boolean ClearEmptyRows { get; set; }
        public void LoadTicketTracker()
        {
            String badgeNum = "";
            if (Context.Items["modeTag"] != null)
            {
                string mode = (string)Context.Items["modeTag"];
                if (mode == "ticketSummaryChangeTimes")
                {
                    this._displayMode = TrackingDisplayMode.ChangeTimes;
                    _editBadgeNumber = (string)Context.Items["badgeNumber"];
                }
            }

            TicketTracker ticketTrackerInput = new TicketTracker();

            if (_displayMode == TrackingDisplayMode.EmployeeSummary)
            {
                ticketTrackerInput.ClientRosterID = _clientRosterId;
            }
            else
            {
                ticketTrackerInput.ClientRosterID = 0;
            }

            ticketTrackerInput.ClientID = _clientInfo.ClientID;
            ticketTrackerInput.TrackingType = _trackingType;
            ticketTrackerInput.PeriodStartDateTime = DateTime.Parse(this._startDateTime.ToString("MM/dd/yyyy 00:00:00"));
            ticketTrackerInput.PeriodEndDateTime = DateTime.Parse(this._startDateTime.ToString("MM/dd/yyyy 23:59:59"));
            TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
            ticketTrackerInput.DepartmentInfo.DepartmentID = _departmentId;
            ticketTrackerInput.ShiftTypeInfo.ShiftTypeId = _shiftTypeId;
            ticketTrackerInput.Location = _locationId;
            _trackerResult = ticketTrackerBL.GetTicketTracking(ticketTrackerInput, Context.User, badgeNum, ClientPrefs.DisplayTemps, ClearEmptyRows);
            this.rptrTicketTracker.DataSource = _trackerResult.Employees;
            this.rptrTicketTracker.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            userID.Value = Context.User.Identity.Name.ToString();
            webServiceLoc.Value = WebServicesLocation.WebServiceLocation.WebServiceLoc;

            if (ClientPrefs.ClientID == 0)
                ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
            /*if (Context.Items["pageAnchor"] != null)
            {
                string pageAnchor = (string)Context.Items["pageAnchor"];
                if (pageAnchor != null && pageAnchor.Trim().Length > 0)
                {
                    string startUpScript = "<script language=Javascript>location.href='#";
                    startUpScript += pageAnchor + "';</script>";
                    this.Page.RegisterStartupScript(this.UniqueID + "StartUp", startUpScript);
                }
            }*/
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ClientPrefs.ClientID == 0)
                ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
            if (ClientPrefs.ApproveHours && _userAllowedToApprove)
            {
                if (_requiresApprovalCount > 0)
                {
                    btnApprove.Visible = true;
                }
                else
                {
                    btnApprove.Visible = false;
                }
            }
        }

        protected void rptrTicketTracker_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (ClientPrefs.ClientID == 0)
                ClientPrefs = (ClientPreferences)Session["ClientPrefs"];
            if (_trackerResult.OverrideRoleName.ToLower() != "viewonly")
            {
                if (Context.User.IsInRole("Administrator") || Context.User.IsInRole("Manager") ||
                    Context.User.IsInRole("ManagerNoPunchIn") || Context.User.IsInRole("UpdatePunches"))
                {
                    _userAllowedToApprove = true;
                }
                if (Context.User.IsInRole("ManagerNoPunchIn"))
                {
                    _userCannotRecordPunchIn = true;
                }
                else
                {
                    _userCannotRecordPunchIn = false;
                }
            }

            DateTime currentTime = _helper.GetCSTCurrentDateTime();
            // This event is raised for the header, the footer, separators, and items.
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EmployeeTracker employee = (EmployeeTracker)e.Item.DataItem;


                _showDetail = this.showDetail(employee);

                setTrackingStyle(e.Item);

                HtmlTableRow trTrackingItem = (HtmlTableRow)e.Item.FindControl("trTrackingItem");
                trTrackingItem.Attributes.Add("CR_ID", employee.ClientRosterID.ToString());


                //check if the swipe is for this department
                bool swipedInAnotherDepartment = this.swipedMoved(employee);

                //if (swipedInAnotherDepartment)
                //{
                //    setLabelColors(e.Item, System.Drawing.Color.Black);
                //}
                if (!swipedInAnotherDepartment)
                {
                    switch (employee.PunchStatus)
                    {
                        case Enums.EmployeePunchStatus.NotCheckedIn:
                            if (_helper.IsNotCheckedInAfterShiftStartError(employee))
                            {
                                setLabelColors(e.Item, System.Drawing.Color.Red);
                            }
                            else
                            {
                                setLabelColors(e.Item, System.Drawing.Color.Green);
                            }
                            //((Button)e.Item.FindControl("btnAction")).Text = "Check In";
                            break;
                        case Enums.EmployeePunchStatus.CheckedOut:
                            setLabelColors(e.Item, System.Drawing.Color.Blue);
                            //((Button)e.Item.FindControl("btnAction")).Text = "Check In";
                            break;
                        case Enums.EmployeePunchStatus.CheckedIn:
                            if (_helper.IsNotCheckedOutAfterShiftEndError(employee))
                            {
                                setLabelColors(e.Item, System.Drawing.Color.Red);
                            }
                            else
                            {
                                setLabelColors(e.Item, System.Drawing.Color.Green);
                            }
                            //((Button)e.Item.FindControl("btnAction")).Text = "Check Out";
                            break;
                        case Enums.EmployeePunchStatus.EarlyCheckIn:
                            //if (_helper.IsNotCheckedOutAfterShiftEndError(employee))
                            //{
                            //    setLabelColors(e.Item, System.Drawing.Color.Red);
                            //}
                            //else
                            //{
                            setLabelColors(e.Item, System.Drawing.Color.Purple);
                            //}
                            //((Button)e.Item.FindControl("btnAction")).Text = "Check Out";
                            break;
                    }
                }

                //setBiometricBackground(e.Item);
                setTimeDisparityBackground(e.Item);

                if (!_clientInfo.RequiresLunchInOut)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdLunchStart")).Visible = false;
                    ((HtmlTableCell)e.Item.FindControl("tdLunchEnd")).Visible = false;
                }

                if (_trackerResult.OverrideRoleName.ToLower() == "viewonly" || (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager")
                    && !Context.User.IsInRole("ManagerNoPunchIn")) ||
                    (_displayMode == TrackingDisplayMode.EmployeeSummary || _displayMode == TrackingDisplayMode.Email))
                {
                    ((HtmlTableCell)e.Item.FindControl("tdAction")).Visible = false;
                    if (_displayMode == TrackingDisplayMode.EmployeeSummary)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdBadgeNumber")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdName")).Visible = false;
                    }
                }
                HiddenField hdnStartDate = (HiddenField)e.Item.FindControl("hdnStartDate");
                hdnStartDate.Value = _trackerResult.PeriodStartDateTime.ToString("MM/dd/yyyy HH:mm");

                HiddenField hdnEndDate = (HiddenField)e.Item.FindControl("hdnEndDate");
                hdnEndDate.Value = _trackerResult.PeriodEndDateTime.ToString("MM/dd/yyyy HH:mm");

                HiddenField hdnShiftType = (HiddenField)e.Item.FindControl("hdnShiftType");
                hdnShiftType.Value = this.ShiftTypeID.ToString();

                HiddenField hdnDepartmentID = (HiddenField)e.Item.FindControl("hdnDepartmentID");
                hdnDepartmentID.Value = _departmentId.ToString();

                HiddenField hdnLocationID = (HiddenField)e.Item.FindControl("hdnLocationID");
                hdnLocationID.Value = _locationId.ToString();

                HiddenField hdnBadgeNumber = (HiddenField)e.Item.FindControl("hdnBadgeNumber");
                hdnBadgeNumber.Value = employee.TempNumber.ToString();

                HiddenField hdnEmployeeName = (HiddenField)e.Item.FindControl("hdnEmployeeName");
                hdnEmployeeName.Value = employee.LastName + ", " + employee.FirstName;

                HiddenField hdnClientID = (HiddenField)e.Item.FindControl("hdnClientID");
                hdnClientID.Value = (this.ClientInfo.ClientID).ToString();

                //set the item index
                Label lblItem = (Label)e.Item.FindControl("lblItem");
                lblItem.Text = (e.Item.ItemIndex + 1).ToString() + ". ";

                HiddenField hdnEmployeePunchList = (HiddenField)e.Item.FindControl("hdnEmployeePunchList");
                hdnEmployeePunchList.Value = "";

                HtmlTableCell tdApprove = (HtmlTableCell)e.Item.FindControl("tdApprove");
                if (ClientPrefs.ApproveHours && _userAllowedToApprove)
                {
                    ImageButton btnChangeTimes = (ImageButton)e.Item.FindControl("btnImageEdit");
                    LinkButton lnkChangeTimes = (LinkButton)e.Item.FindControl("lnkChangeTimes");
                    //jhm
                    Button btnApprove = (Button)e.Item.FindControl("btnAction");
                    btnChangeTimes.Enabled = true;
                    lnkChangeTimes.Enabled = true;
                    CheckBox chkApprove = (CheckBox)e.Item.FindControl("chkApprove");
                    Button btnUnlock = (Button)e.Item.FindControl("btnUnlock");
                    HiddenField hdnShiftDate = (HiddenField)e.Item.FindControl("hdnShiftDate");
                    hdnShiftDate.Value = this._startDateTime.ToString("MM/dd/yyyy");
                    tdApprove.Visible = true;
                    if (employee.Punches.Count > 1 && (employee.Punches.Count % 2 == 0))
                    {
                        string list = string.Empty;
                        int approvedCount = 0;
                        foreach (EmployeePunchSummary summary in employee.Punches)
                        {
                            if (summary.IsApproved)
                            {
                                approvedCount++;
                            }
                            if (list == string.Empty)
                            {
                                list = summary.EmployeePunchID.ToString();
                            }
                            else
                            {
                                list += "," + summary.EmployeePunchID.ToString();
                            }
                        }
                        if (approvedCount == employee.Punches.Count)
                        {
                            chkApprove.Visible = false;
                            btnUnlock.Visible = true;
                            btnChangeTimes.Enabled = false;
                            lnkChangeTimes.Enabled = false;
                        }
                        else
                        {
                            _requiresApprovalCount++;
                        }
                        hdnEmployeePunchList.Value = list;
                    }
                    else
                    {
                        chkApprove.Visible = false;
                        btnUnlock.Visible = false;
                    }
                }
                else
                {
                    tdApprove.Visible = false;
                }

                rptrPunchDetail = (Repeater)e.Item.FindControl("rptrPunchDetail");
                rptrPunchDetail.Visible = false;
                if (!swipedInAnotherDepartment && _showDetail)
                {
                    rptrPunchDetail.Visible = true;
                    rptrPunchDetail.DataSource = employee.WorkSummaries;
                    rptrPunchDetail.DataBind();
                }

                //set the change times link
                ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnImageEdit");
                btnEdit.Attributes.Add("name", btnEdit.UniqueID);

                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkChangeTimes");

                if (_displayMode == TrackingDisplayMode.Email)
                {
                    btnEdit.Visible = false;
                    lnkEdit.Visible = false;
                }

                //check if the role is allowed to change times
                if (_userCannotRecordPunchIn)
                {
                    if (employee.Punches.Count == 0)
                    {
                        btnEdit.Visible = false;
                        lnkEdit.Visible = false;
                    }
                }

                if (ClientPrefs.DisplaySchedule)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdSchedule")).Visible = true;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdSchedule")).Visible = false;
                }

                //set the columns if the swipe has been moved
                LinkButton lnkLinkToDepartment = (LinkButton)e.Item.FindControl("lnkLinkToDepartment");
                Label lblMoved = (Label)e.Item.FindControl("lblMoved");
                if (swipedInAnotherDepartment)
                {
                    HtmlTableCell tdCheckIn = (HtmlTableCell)e.Item.FindControl("tdCheckIn");
                    tdCheckIn.ColSpan = 2;
                    if (_clientInfo.RequiresLunchInOut)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdLunchStart")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdLunchEnd")).Visible = false;
                        tdCheckIn.ColSpan = 4;
                    }
                    ((HtmlTableCell)e.Item.FindControl("tdCheckOut")).Visible = false;
                    ((HtmlTableCell)e.Item.FindControl("tdAction")).Visible = false;
                    ((HtmlTableCell)e.Item.FindControl("tdStatus")).ColSpan = 2;
                    if (tdApprove.Visible)
                    {
                        tdApprove.Visible = false;
                    }

                    EmployeePunchSummary checkIn = (EmployeePunchSummary)employee.CheckInPunches[0];
                    lnkLinkToDepartment.CommandArgument = checkIn.TicketInfo.DepartmentInfo.DepartmentID.ToString();
                    lblMoved.Text = "Swipes have been moved to " + checkIn.TicketInfo.DepartmentInfo.DepartmentName;
                    ((Label)e.Item.FindControl("lblCheckIn")).Visible = false;
                }
                else
                {
                    lblMoved.Visible = false;
                    lnkLinkToDepartment.Visible = false;
                }

                if (_displayMode == TrackingDisplayMode.Email)
                {
                    LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
                    lblName.Visible = false;
                    Label lblNameNoLink = (Label)e.Item.FindControl("lblNameNoLink");
                    lblNameNoLink.Visible = true;
                    lblNameNoLink.Text = employee.LastName + ", " + employee.FirstName;
                    ((LinkButton)e.Item.FindControl("lblBadgeNumber")).Visible = false;
                    ((Label)e.Item.FindControl("lblBadgeNumberNoLink")).Visible = true;

                }
                else
                {
                    LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
                    lblName.Text = employee.LastName + ", " + employee.FirstName;
                    lblName.CommandArgument = employee.TempNumber;
                    Label lblNameNoLink = (Label)e.Item.FindControl("lblNameNoLink");
                    lblNameNoLink.Visible = false;

                    ((LinkButton)e.Item.FindControl("lblBadgeNumber")).Visible = true;
                    ((Label)e.Item.FindControl("lblBadgeNumberNoLink")).Visible = false;
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if (!_clientInfo.RequiresLunchInOut)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdLunchStartHeader")).Visible = false;
                    ((HtmlTableCell)e.Item.FindControl("tdLunchEndHeader")).Visible = false;
                }

                if (_trackerResult.OverrideRoleName.ToLower() == "viewonly" || (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager") && !Context.User.IsInRole("ManagerNoPunchIn")) || (_displayMode == TrackingDisplayMode.EmployeeSummary || _displayMode == TrackingDisplayMode.Email))
                {
                    ((HtmlTableCell)e.Item.FindControl("tdActionHeader")).Visible = false;
                    if (_displayMode == TrackingDisplayMode.EmployeeSummary)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdBadgeNumberHeader")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdNameHeader")).Visible = false;
                    }
                }

                if (ClientPrefs.ApproveHours && _userAllowedToApprove)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdApproveHeader")).Visible = true;
                    btnApprove = (Button)e.Item.FindControl("btnApprove");
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdApproveHeader")).Visible = false;
                    btnApprove = (Button)e.Item.FindControl("btnApprove");
                }

                if (ClientPrefs.DisplaySchedule)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdScheduleHeader")).Visible = true;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdScheduleHeader")).Visible = false;
                }
            }
        }

        protected void rptrPunchDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            // This event is raised for the header, the footer, separators, and items.
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EmployeeWorkSummary workSummary = (EmployeeWorkSummary)e.Item.DataItem;
                ((Label)e.Item.FindControl("lblCheckIn")).Text = workSummary.CheckInDateTime.ToString("MM/dd/yyyy h:mm tt");
                if (workSummary.DepartmentInfo.DepartmentID != _departmentId)
                {
                    Label lblCheckInDetailDeptId = (Label)e.Item.FindControl("lblCheckInDetailDeptId");
                    lblCheckInDetailDeptId.Visible = true;
                    lblCheckInDetailDeptId.Text = " - (" + workSummary.DepartmentInfo.DepartmentName + ")";
                }

                if (workSummary.CheckOutDateTime == new DateTime(1, 1, 1))
                {
                    ((Label)e.Item.FindControl("lblCheckOut")).Text = "N/A";
                }
                else
                {
                    ((Label)e.Item.FindControl("lblCheckOut")).Text = workSummary.CheckOutDateTime.ToString("MM/dd/yyyy h:mm tt");
                    if (workSummary.DepartmentInfo.DepartmentID != _departmentId)
                    {
                        Label lblCheckOutDetailDeptId = (Label)e.Item.FindControl("lblCheckOutDetailDeptId");
                        lblCheckOutDetailDeptId.Visible = true;
                        lblCheckOutDetailDeptId.Text = " - (" + workSummary.DepartmentInfo.DepartmentName + ")";
                    }
                }
            }
        }
        protected void rptrTicketTracker_ItemCommand(Object src, RepeaterCommandEventArgs e)
        {
            /*
            Context.Items.Add("modeTag", "manual");
            Context.Items.Add( "badgeNumber", e.CommandArgument.ToString());
            HiddenField hdnStartDate = (HiddenField)e.Item.FindControl("hdnStartDate");
            Context.Items.Add("swipeDate", hdnStartDate.Value);
            Server.Transfer("~/auth/ManualCheckIn.aspx", false);
            */
            if (e.CommandName == "showDetail")
            {
                setContextValues(e.Item);
                Context.Items.Add("modeTag", "ticketSummaryChangeTimes");
                Context.Items.Add("pageAnchor", e.CommandArgument.ToString());
                Server.Transfer("~/auth/TicketSummary.aspx", false);
            }

            if (e.CommandName == "linkToDepartment")
            {
                setContextValues(e.Item);
                Context.Items.Add("modeTag", "ticketSummaryReload");
                Context.Items["departmentId"] = e.CommandArgument.ToString();
                //Context.Items.Add("pageAnchor", Context.Items["badgeNumber"]);
                Server.Transfer("~/auth/TicketSummary.aspx", false);
            }

            if (e.CommandName == "linkToHistory")
            {
                setContextValues(e.Item);
                Context.Items.Add("modeTag", "employeeHistory");
                int clientID = Int32.Parse((String)Context.Items["clientID"]);
                //if( clientID == 258 || clientID == 121 || clientID == 256 || clientID == 205 )
                if (ClientPrefs.DisplayWeeklyReportsSundayToSaturday)
                    Context.Items.Add("weekEndDate", _helper.GetCSTWeekEndingSaturdayDateFromDate(DateTime.Parse(Context.Items["startDate"].ToString())));
                else if (ClientPrefs.DisplayWeeklyReportsWednesdayToTuesday)
                {
                    Context.Items.Add("weekEndDate", _helper.GetCSTWeekEndingTuesdayDateFromDate(DateTime.Parse(Context.Items["startDate"].ToString())));
                }
                else if (ClientPrefs.DisplayWeeklyReportsSaturdayToFriday)
                {
                    Context.Items.Add("weekEndDate", _helper.GetCSTWeekEndingFridayDateFromDate(DateTime.Parse(Context.Items["startDate"].ToString())));
                }
                else
                    Context.Items.Add("weekEndDate", _helper.GetCSTWeekEndingDateFromDate(DateTime.Parse(Context.Items["startDate"].ToString())));
                Server.Transfer("~/auth/EmployeeHistory.aspx", false);
            }

            if (e.CommandName == "approve" || e.CommandName == "unlock")
            {
                CheckBox chkApprove = null;
                string employeePunchList = string.Empty;
                string noShowList = "";
                HiddenField hdnEmployeePunchList = null;
                HiddenField hdnShiftDate = null;
                DateTime shiftDate = new DateTime(1, 1, 1);

                if (e.CommandName == "approve")
                {
                    foreach (RepeaterItem item in rptrTicketTracker.Items)
                    {
                        chkApprove = (CheckBox)item.FindControl("chkApprove");
                        if (chkApprove.Checked)
                        {
                            hdnEmployeePunchList = (HiddenField)item.FindControl("hdnEmployeePunchList");
                            if (hdnEmployeePunchList.Value.Length > 0)

                            {
                                hdnShiftDate = (HiddenField)item.FindControl("hdnShiftDate");
                                if (shiftDate == new DateTime(1, 1, 1))
                                {
                                    shiftDate = DateTime.Parse(hdnShiftDate.Value);
                                    setContextValues(item);
                                }

                                if (employeePunchList == string.Empty)
                                {
                                    employeePunchList = hdnEmployeePunchList.Value;
                                }
                                else
                                {
                                    employeePunchList += "," + hdnEmployeePunchList.Value;
                                }
                            }
                        }
                    }

                    if (employeePunchList != string.Empty)
                    {
                        //approve the hours
                        StringBuilder approvalXML = new StringBuilder();
                        approvalXML.Append("<approvalxml><employeepunches>");

                        string[] list = employeePunchList.Split(new char[] { ',' });
                        foreach (string listItem in list)
                        {
                            approvalXML.Append("<employeepunch><ID>");
                            approvalXML.Append(listItem);
                            approvalXML.Append("</ID></employeepunch>");
                        }

                        approvalXML.Append("</employeepunches></approvalxml>");

                        TicketTrackerApproval approvalInfo = new TicketTrackerApproval();
                        approvalInfo.ApprovedDateTime = _helper.GetCSTCurrentDateTime();
                        approvalInfo.ApprovedNoShowList = noShowList;
                        approvalInfo.ApprovedPunchList = employeePunchList;
                        approvalInfo.ApprovedPunchListXML = approvalXML.ToString();
                        approvalInfo.ShiftDate = shiftDate;
                        TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
                        if (ticketTrackerBL.ApproveTrackingHours(approvalInfo, Context.User))
                        {
                            Context.Items.Add("modeTag", "ticketSummaryReload");
                            //reload the page
                            Server.Transfer("~/auth/TicketSummary.aspx", false);
                        }
                    }
                }
                else
                {
                    hdnEmployeePunchList = (HiddenField)e.Item.FindControl("hdnEmployeePunchList");
                    employeePunchList = hdnEmployeePunchList.Value;
                    TicketTrackerUnlock unlockInfo = new TicketTrackerUnlock();
                    unlockInfo.UnlockPunchList = employeePunchList;
                    TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
                    if (ticketTrackerBL.UnlockTrackingHours(unlockInfo, Context.User))
                    {
                        setContextValues(e.Item);
                        Context.Items.Add("modeTag", "ticketSummaryReload");
                        //reload the page
                        Server.Transfer("~/auth/TicketSummary.aspx", false);
                    }
                }
            }
            else
            {
                Context.Items.Add("modeTag", "employeeSummary");
                Context.Items.Add("clientRosterId", e.CommandArgument.ToString());

                setContextValues(e.Item);

                Server.Transfer("~/auth/EmployeePunchMaintenance.aspx", false);
            }
        }

        private void setContextValues(RepeaterItem item)
        {
            HiddenField hdnStartDate = (HiddenField)item.FindControl("hdnStartDate");
            Context.Items.Add("startDate", hdnStartDate.Value);
            HiddenField hdnEndDate = (HiddenField)item.FindControl("hdnEndDate");
            Context.Items.Add("endDate", hdnEndDate.Value);
            HiddenField hdnShiftType = (HiddenField)item.FindControl("hdnShiftType");
            Context.Items.Add("shiftType", hdnShiftType.Value);
            HiddenField hdnDepartmentID = (HiddenField)item.FindControl("hdnDepartmentID");
            Context.Items.Add("departmentId", hdnDepartmentID.Value);
            HiddenField hdnLocationID = (HiddenField)item.FindControl("hdnLocationID");
            Context.Items.Add("locationId", hdnLocationID.Value);
            HiddenField hdnBadgeNumber = (HiddenField)item.FindControl("hdnBadgeNumber");
            Context.Items.Add("badgeNumber", hdnBadgeNumber.Value);
            HiddenField hdnEmployeeName = (HiddenField)item.FindControl("hdnEmployeeName");
            Context.Items.Add("employeeName", hdnEmployeeName.Value);
            HiddenField hdnClientID = (HiddenField)item.FindControl("hdnClientID");
            Context.Items.Add("clientID", hdnClientID.Value);
        }

        protected string TranslatePunchStatus(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (this.swipedMoved(employee))
            {
                retStatus = "Moved";
            }
            else
            {
                switch (employee.PunchStatus.ToString())
                {
                    case "CheckedIn":
                        retStatus = "Checked In";
                        break;
                    case "CheckedOut":
                        retStatus = "Checked Out";
                        break;
                    case "NotCheckedIn":
                        retStatus = "Not Checked In";
                        break;
                    case "EarlyCheckIn":
                        retStatus = "Early Check In";
                        break;
                }
            }
            return retStatus;
        }

        protected string GetCheckIn(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;

            if (employee.HasCheckIn)
            {
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[0];
                DateTime startDateTime = DateTime.Parse(latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy")
                    + " " + employee.ShiftStartTime);
                if (latestPunch.RoundedPunchDateTime > startDateTime &&
                    (ClientPrefs.TicketTrackingExactLatePunches == true))
                {
                    retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                }
                else
                {
                    retStatus = latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                }
            }
            else
                retStatus = "";

            return retStatus;
        }
        protected string GetPicNameCheckIn(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;

            if (employee.HasCheckIn)
            {
                string tn = employee.TempNumber;
                tn = tn.Remove(0, 2);
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[0];
                retStatus = tn + latestPunch.PunchPic.ToString("__yyyyMMdd_HHmmss") + ".jpg";
            }
            return retStatus;
        }
        protected string GetPicNameCheckOut(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;

            if (employee.HasCheckOut)
            {
                string tn = employee.TempNumber;
                tn = tn.Remove(0, 2);
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[employee.CheckOutPunches.Count - 1];
                retStatus = tn + latestPunch.PunchPic.ToString("__yyyyMMdd_HHmmss") + ".jpg";
            }
            return retStatus;
        }
        protected string GetExactCheckIn(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;

            if (employee.HasCheckIn)
            {
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[0];
                retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
            }
            return retStatus;
        }

        protected string GetBreakIn(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckOut)
            {
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[0];
                retStatus = latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy h:mm tt");
            }

            return retStatus;
        }
        protected string GetExactBreakIn(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckOut)
            {
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[0];
                retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
            }

            return retStatus;
        }

        protected string GetBreakOut(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckIn)
            {
                if (employee.CheckInPunches.Count > 1)
                {
                    EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[1];
                    retStatus = latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                }
            }
            else
                retStatus = "";

            return retStatus;
        }

        protected string GetExactBreakOut(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckIn)
            {
                if (employee.CheckInPunches.Count > 1)
                {
                    EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[1];
                    retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                }
            }
            else
                retStatus = "";

            return retStatus;
        }
        protected string GetCheckInPunchId(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;

            if (employee.HasCheckIn)
            {
                EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckInPunches[0];
                retStatus = latestPunch.EmployeePunchID.ToString();
            }
            return retStatus;
        }

        protected string GetCheckOutPunchId(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            EmployeePunchSummary latestPunch = null;
            if (employee.HasCheckOut)
            {
                if (employee.PunchStatus == Enums.EmployeePunchStatus.CheckedOut || employee.PunchStatus == Enums.EmployeePunchStatus.EarlyCheckIn)
                {
                    if (employee.CheckOutPunches.Count > 1)
                    {
                        latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[employee.CheckOutPunches.Count - 1];
                    }
                    else if (employee.CheckOutPunches.Count == 1)
                    {
                        latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[0];
                    }
                }
            }
            if (latestPunch != null)
                retStatus = latestPunch.EmployeePunchID.ToString();
            return retStatus;
        }

        protected string GetCheckOut(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckOut)
            {
                if (employee.PunchStatus == Enums.EmployeePunchStatus.CheckedOut || employee.PunchStatus == Enums.EmployeePunchStatus.EarlyCheckIn)
                {
                    if (employee.CheckOutPunches.Count > 1)
                    {
                        EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[employee.CheckOutPunches.Count - 1];
                        retStatus = latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                    }
                    else if (employee.CheckOutPunches.Count == 1)
                    {
                        EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[0];
                        retStatus = latestPunch.RoundedPunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                    }
                }
            }
            return retStatus;
        }

        protected string GetExactCheckOut(object dataItem)
        {
            string retStatus = "";
            EmployeeTracker employee = (EmployeeTracker)dataItem;
            if (employee.HasCheckOut)
            {
                if (employee.PunchStatus == Enums.EmployeePunchStatus.CheckedOut || employee.PunchStatus == Enums.EmployeePunchStatus.EarlyCheckIn)
                {
                    if (employee.CheckOutPunches.Count > 1)
                    {
                        EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[employee.CheckOutPunches.Count - 1];
                        retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                    }
                    else if (employee.CheckOutPunches.Count == 1)
                    {
                        EmployeePunchSummary latestPunch = (EmployeePunchSummary)employee.CheckOutPunches[0];
                        retStatus = latestPunch.PunchDateTime.ToString("MM/dd/yyyy h:mm tt");
                    }
                }
            }
            return retStatus;
        }

        protected string FormatSchedule(object startTime, object endTime)
        {
            DateTime startDateTime = DateTime.Parse(_helper.GetCSTCurrentDateTime().ToString("MM/dd/yyyy") + " " + startTime);
            DateTime endDateTime = DateTime.Parse(_helper.GetCSTCurrentDateTime().ToString("MM/dd/yyyy") + " " + endTime);
            return startDateTime.ToString("h:mm tt") + "-" + endDateTime.ToString("h:mm tt");
        }

        private void setTimeDisparityBackground(RepeaterItem item)
        {
            EmployeePunchSummary eps = null;
            if (((EmployeeTracker)(item.DataItem)).CheckInPunches.Count > 0)
            {
                eps = (EmployeePunchSummary)((EmployeeTracker)(item.DataItem)).CheckInPunches[0];

                TimeSpan ts = eps.PunchDateTime.Subtract(eps.RoundedPunchDateTime);
                if (ts.Days != 0 || ts.Hours != 0 || ts.Minutes >= 38 || ts.Minutes <= -38)
                //if (eps.BiometricResult != 0)
                {
                    ((Label)item.FindControl("lblCheckIn")).BackColor = ((Label)item.FindControl("lblCheckIn")).ForeColor;
                    ((Label)item.FindControl("lblCheckIn")).ForeColor = Color.White;
                    //((Label)item.FindControl("lblCheckIn")).S
                    //((Label)item.FindControl("lblCheckIn")).BorderColor = System.Drawing.Color.LightYellow;
                }
            }
            if (((EmployeeTracker)(item.DataItem)).CheckOutPunches.Count > 0)
            {
                int count = ((EmployeeTracker)(item.DataItem)).CheckOutPunches.Count;
                eps = (EmployeePunchSummary)((EmployeeTracker)(item.DataItem)).CheckOutPunches[count - 1];
                TimeSpan ts = eps.PunchDateTime.Subtract(eps.RoundedPunchDateTime);
                if (ts.Days != 0 || ts.Hours != 0 || ts.Minutes >= 38 || ts.Minutes <= -38)
                //if (eps.BiometricResult != 0 && ((Label)item.FindControl("lblCheckOut")).Text.Length > 0)
                {
                    ((Label)item.FindControl("lblCheckOut")).BackColor = ((Label)item.FindControl("lblCheckIn")).ForeColor;
                    ((Label)item.FindControl("lblCheckOut")).ForeColor = Color.White;
                    //((HtmlTableCell)item.FindControl("tdCheckIn")).BgColor = "0xDDDDFF";
                    //((Label)item.FindControl("lblCheckOut")).BorderStyle = BorderStyle.Solid;
                    //((Label)item.FindControl("lblCheckOut")).BorderWidth = 2;
                    //((Label)item.FindControl("lblCheckOut")).BorderColor = System.Drawing.Color.LightYellow;
                }
            }
        }

        private void setBiometricBackground(RepeaterItem item)
        {
            EmployeePunchSummary eps = null;
            if (((EmployeeTracker)(item.DataItem)).CheckInPunches.Count > 0)
            {
                eps = (EmployeePunchSummary)((EmployeeTracker)(item.DataItem)).CheckInPunches[0];
                if (eps.BiometricResult != 0)
                {
                    ((Label)item.FindControl("lblCheckIn")).BorderStyle = BorderStyle.Solid;
                    ((Label)item.FindControl("lblCheckIn")).BorderWidth = 2;
                    //((Label)item.FindControl("lblCheckIn")).S
                    //((Label)item.FindControl("lblCheckIn")).BorderColor = System.Drawing.Color.LightYellow;
                }
            }
            if (((EmployeeTracker)(item.DataItem)).CheckOutPunches.Count > 0)
            {
                int count = ((EmployeeTracker)(item.DataItem)).CheckOutPunches.Count;
                eps = (EmployeePunchSummary)((EmployeeTracker)(item.DataItem)).CheckOutPunches[count - 1];
                if (eps.BiometricResult != 0 && ((Label)item.FindControl("lblCheckOut")).Text.Length > 0)
                {
                    //((HtmlTableCell)item.FindControl("tdCheckIn")).BgColor = "0xDDDDFF";
                    ((Label)item.FindControl("lblCheckOut")).BorderStyle = BorderStyle.Solid;
                    ((Label)item.FindControl("lblCheckOut")).BorderWidth = 2;
                    //((Label)item.FindControl("lblCheckOut")).BorderColor = System.Drawing.Color.LightYellow;
                }
            }
        }
        private void setLabelColors(RepeaterItem item, System.Drawing.Color color)
        {
            ((LinkButton)item.FindControl("lblBadgeNumber")).ForeColor = color;
            ((Label)item.FindControl("lblBadgeNumberNoLink")).ForeColor = color;
            ((LinkButton)item.FindControl("lblName")).ForeColor = color;
            ((Label)item.FindControl("lblNameNoLink")).ForeColor = color;
            ((Label)item.FindControl("lblCheckIn")).ForeColor = color;
            ((Label)item.FindControl("lblBreakIn")).ForeColor = color;
            ((Label)item.FindControl("lblBreakOut")).ForeColor = color;
            ((Label)item.FindControl("lblCheckOut")).ForeColor = color;
            ((Label)item.FindControl("lblPunchStatus")).ForeColor = color;
        }

        private bool showDetail(EmployeeTracker employee)
        {
            bool retVal = false;

            if (employee.WorkSummaries.Count > 0)
            {
                if (_clientInfo.RequiresLunchInOut)
                {
                    if (employee.Punches.Count > 4)
                    {
                        retVal = true;
                    }
                }
                else if (employee.Punches.Count > 2)
                {
                    retVal = true;
                }
            }

            if (!retVal && this._displayMode == TrackingDisplayMode.ChangeTimes && employee.TempNumber.ToUpper() == _editBadgeNumber.ToUpper())
            {
                retVal = true;
            }

            return retVal;
        }

        private void setTrackingStyle(RepeaterItem trackingItem)
        {
            HtmlTableRow trTrackingItem = (HtmlTableRow)trackingItem.FindControl("trTrackingItem");

            for (int idx = 0; idx < trTrackingItem.Cells.Count; idx++)
            {
                if (_showDetail)
                {
                    if (trackingItem.ItemType == ListItemType.AlternatingItem)
                    {
                        trTrackingItem.Cells[idx].Attributes["class"] = "ResultTableDetailAlternateRowText";
                    }
                    else
                    {
                        trTrackingItem.Cells[idx].Attributes["class"] = "ResultTableDetailRowText";
                    }
                }
                else
                {
                    if (trackingItem.ItemType == ListItemType.AlternatingItem)
                    {
                        trTrackingItem.Cells[idx].Attributes["class"] = "ResultTableAlternateRowText";
                    }
                    else
                    {
                        trTrackingItem.Cells[idx].Attributes["class"] = "ResultTableRowText";
                    }
                }
            }
        }

        private bool swipedMoved(EmployeeTracker employee)
        {
            bool retVal = false;

            if (employee.HasCheckIn)
            {
                EmployeePunchSummary checkInPunch = (EmployeePunchSummary)employee.CheckInPunches[0];
                if (checkInPunch.TicketInfo.DepartmentInfo.DepartmentID != _departmentId)
                {
                    retVal = true;
                }
            }

            return retVal;
        }
    }
}