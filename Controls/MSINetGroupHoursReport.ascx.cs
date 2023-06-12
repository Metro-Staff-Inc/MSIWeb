using System;
using System.Text;
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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MSI.Web.Controls
{
    public partial class MSINetGroupHoursReport : BaseMSINetControl
    {
        public enum ExportType
        {
            None,
            Excel,
            Print
        }
        private static DateTime DATE_NOT_SET = new DateTime(1, 1, 1);
        private DateTime _firstPunch = DATE_NOT_SET;
        private DateTime _lastPunch = DATE_NOT_SET;
        private DateTime _periodStart;
        private DateTime _periodEnd;
        private Client _clientInfo = new Client();
        private ClientPreferences _clientPrefs = new ClientPreferences();
        private EmployeeHistory _boundEmployeeHistory = new EmployeeHistory();
        HelperFunctions _helper = new HelperFunctions();
        private decimal _reportHours = 0M;
        private decimal _reportOTHours = 0M;
        private ExportType _exportType = ExportType.None;
        private int _currentDepartmentId = 0;
        private int _currentShiftType = 0;
        private decimal[] _totDailyHours = new decimal[9];
        private decimal[] _totShiftDailyHours = new decimal[9];
        private decimal[] _grandTotDailyHours = new decimal[7];
        private bool _exportDetail = false;
        private HoursReport _hoursReport;
        private int _boundEmployeeIdx = 0;
        private bool _showApproved = false;
        private bool _allowElectronicApproval = true;
        private bool _hoursLoaded = false;
        private DateTime _weekEnding;
        private double _bonusTotals = 0.0;
        private double _bonusGrandTotal = 0.0;
        private int clientLocation = 0;

        private String[] weekDay = { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };
        public Boolean payRateJobCodeEligible = true;

        public int[] AmericanLithoShifts = { 4, 7, 79, 264, 366, 376, 440, 442, 443, 441, 265, 80, 8, 5, 6, 210, 263, 266, 367 };
        public int[] BerlinShifts = { 1011, 1066, 1067, 978, 1012, 979, 984, 1013, 1068, 1069 };

        public int[] CoMailShifts = { 1, 2, 3, 314, 315, 325, 326, 316, 317, 1037, 1038, 1039, 1091, 1092, 1093, 1206, 1207, 1208, 1329, 1330, 1058, 1059, 1060 };
        public int[] CoPalShifts = { 318, 319, 327 };
        public bool sortByDept = true;
        public bool showExact = false;

        private Boolean btnApproveEnabled = true;

        public double BonusGrandTotal
        {
            get
            {
                return _bonusGrandTotal;
            }
            set
            {
                _bonusGrandTotal = value;
            }
        }
        public double BonusTotals
        {
            get
            {
                return _bonusTotals;
            }
            set
            {
                _bonusTotals = value;
            }
        }

        public DateTime WeekEnding
        {
            get
            {
                return _weekEnding;
            }
            set
            {
                _weekEnding = value;
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

        public DateTime PeriodStart
        {
            get
            {
                return _periodStart;
            }
            set
            {
                _periodStart = value;
            }
        }

        public DateTime PeriodEnd
        {
            get
            {
                return _periodEnd;
            }
            set
            {
                _periodEnd = value;
            }
        }


        public void LoadHoursReport()
        {
            if (_exportType == ExportType.None)
            {
                pnlJSCode.Visible = true;
            }
            Session["ClientInfo"] = ClientInfo;
            _firstPunch = DATE_NOT_SET;
            _lastPunch = DATE_NOT_SET;
            _hoursLoaded = true;
            HoursReport hoursReportInput = new HoursReport();
            hoursReportInput.ClientID = _clientInfo.ClientID;
            hoursReportInput.RosterEmployeeFlag = true;
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);
            SetWeekEnding();
            hoursReportInput.EndDateTime = WeekEnding;
            hoursReportInput.StartDateTime = hoursReportInput.EndDateTime.Subtract(ts);
            ts = new TimeSpan(0, 0, 0);
            hoursReportInput.StartDateTime = hoursReportInput.StartDateTime.Date + ts;
            DateTime dt = WeekEnding;
            hoursReportInput.EndDateTime = DateTime.Parse(dt.ToString("MM/dd/yyyy") + " 23:59:59");
            //bool sortByDept;
            if (_exportType == ExportType.None)
            {
                if (rbtnDepts.Checked == true)
                    sortByDept = true;
                else
                    sortByDept = false;
                if (chkBoxExact.Checked == true)
                    showExact = true;
            }
            hoursReportInput.UseExactTimes = showExact;
            string badgeNum = "";
            GroupHoursReportBL groupHoursReportBL = new GroupHoursReportBL();
            string userId = Context.User.Identity.Name.ToLower();
            _hoursReport = groupHoursReportBL.GetHoursReport(hoursReportInput, userId, badgeNum, sortByDept);

            bool generateCSV = userId.Equals("mchavez") || userId.Equals("moakes") || userId.Equals("imolina") || userId.Equals("itdept") 
                            || userId.Equals("gomezs");
            /* creative works and creative works in-house */
            if ((_clientId == 256 || _clientId == 178) && (userId.Equals("dougm") ||
              userId.Equals("vanessag") || userId.Equals("scottb") ||
                userId.Equals("anitas") || userId.Equals("chrisp") ||
                  userId.Equals("iqbalk") || userId.Equals("dottieg") ||
                  userId.Equals("hillmannt") || userId.Equals("drodriguez") ||
                  userId.Equals("efarfan")) || userId.Equals("ltimmons") ||
                userId.Equals("jbandelow") || userId.Equals("abbasyj") ||
                userId.Equals("gurneyd") || userId.Equals("ballardc") ||
                userId.Equals("balderasc") || userId.Equals("lopezj") ||
                userId.Equals("perezr"))
                payRateJobCodeEligible = false;
            if (userId.ToLower().Equals("seent") || userId.ToLower().Equals("feckj") || userId.ToLower().Equals("kalejtaa")
                || userId.ToLower().Equals("coccimiglioa"))
                payRateJobCodeEligible = false;

            /* tangent technologies only shired, moakes, medwards, and itdept can see job codes and pay rates */
            if ((_clientId == 258) && !(userId.Equals("shired")
                || userId.Equals("moakes") || userId.Equals("mchavez")
                || userId.Equals("itdept") || userId.Equals("glaserm")))
                payRateJobCodeEligible = false;

            if (_clientId == 226 && userId.Equals("huertam"))
                payRateJobCodeEligible = false;

            if (userId.Equals("mchavez") || userId.Equals("julio") || userId.Equals("itdept") || userId.Equals("moakes")
                || userId.Equals("vfabela"))
                this.hdnUpdBonuses.Value = "true";
            this.hdnApproveList.Value = "";

            this.rptrHoursReport.DataSource = _hoursReport.EmployeeHistoryCollection;
            this.rptrHoursReport.DataBind();
        }

        private string outputJS()
        {
            if (_exportType == ExportType.None)
            {
                String nl = Environment.NewLine;
                StringBuilder js = new StringBuilder();
                js.Append("<script type=\"text/javascript\">" + nl);
                js.Append("function Toggle(id)" + nl);
                js.Append("{" + nl);
                js.Append("var elem = document.getElementById('d999' + id);" + nl);
                js.Append("var imgElem = document.getElementById('i' + id);" + nl);
                js.Append("if (elem)" + nl);
                js.Append("{" + nl);
                js.Append("if (elem.style.display != 'block')" + nl);
                js.Append("{" + nl);
                js.Append("elem.style.display = 'block';" + nl);
                js.Append("elem.style.visibility = 'visible';" + nl);
                js.Append("imgElem.src = '../images/minus.gif';" + nl);
                js.Append("}" + nl);
                js.Append("else" + nl);
                js.Append("{" + nl);
                js.Append("elem.style.display = 'none';" + nl);
                js.Append("elem.style.visibility = 'hidden';" + nl);
                js.Append("imgElem.src = '../images/plus.gif';" + nl);
                js.Append("}" + nl);
                js.Append("}" + nl);
                js.Append("}" + nl);

                /* create list of checked employees */
                js.Append("function AppendID(id) {" + nl);
                //js.Append("var elem = document.getElementById('cb' + id);" + nl);
                js.Append("var hdnElem = document.getElementById('ctlHoursReport_hdnApproveList');" + nl);
                //js.Append("if (elem)" + nl);
                //js.Append("{" + nl);
                js.Append("  hdnElem.value += id + \" \";" + nl);
                //js.Append("}" + nl);
                js.Append("}" + nl);
                js.Append("</script>" + nl);
                return js.ToString();
            }
            else
            {
                return "";
            }
        }

        public void SetWeekEnding()
        {
            //if( txtCalendar.Items.Count != 0 )
            if (WeekEnding == null || WeekEnding == new DateTime(1, 1, 1))
                WeekEnding = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString());
        }

        public string GetSelectedDate()
        {
            return WeekEnding.ToString("MM-dd-yyyy");  //Server.UrlEncode(this.WeekEnding.ToString("MM-dd-yyyy"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //int x = Page.Items.Count;
            //Object o;
            //String s;
            //for (int i = 0; i < Page.Items.Count; i++)
            //    s = Page.Items[i].ToString();
            int count = this.rptrHoursReport.Items.Count;
            clientLocation = Convert.ToInt32(this.hdnClientLocation.Value);
            _hoursLoaded = false;
            this.pnlTotals.Visible = false;
            this.hdnClientLocation.Value = "" + _clientInfo.ClientID;

            this.lnkCreateInvoice.Visible = false;
            Session["clientapprovalid"] = null;
            //for (int i = 0; i < this.rptrHoursReport.Items.Count; i++)
            //{
            //    CheckBox c = new CheckBox();
            //    c.ID = "chkBoxPageLoad";
            //    rptrHoursReport.Items[i].Controls.Add(c);
            //}
            if (!this.IsPostBack)
            {
                this.hdnClientLocation.Value = "" + _clientInfo.ClientID;

                this.lnkExport.Visible = true;
                this.lnkExportDetail.Visible = true;
                /* init the drop down list */
                DateTime dt = _helper.GetCSTCurrentDateTime();
                //if (ClientInfo.ClientID == 121 || ClientInfo.ClientID == 256 || ClientInfo.ClientID == 258 || ClientInfo.ClientID == 205)
                if (ClientPrefs.DisplayWeeklyReportsSundayToSaturday)
                {
                    while (dt.DayOfWeek != DayOfWeek.Saturday)
                        dt = dt.AddDays(1);
                }
                else
                {
                    while (dt.DayOfWeek != DayOfWeek.Sunday)
                        dt = dt.AddDays(1);
                }
                for (int i = 0; i < 32; i++)
                {
                    this.txtCalendar.Items.Add(dt.ToString("MM/dd/yyyy"));
                    dt = dt.AddDays(-7);
                }
            }
            if (this.WeekEnding == null || this.WeekEnding == new DateTime(1, 1, 1).Date)
            {
                //get the week ending date - initialization
                if (txtCalendar.Items.Count == 0)
                    this.WeekEnding = DateTime.Parse(_helper.GetCSTCurrentWeekEndingDate().ToString("MM/dd/yyyy") + " 00:00:00");
                else
                    this.WeekEnding = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString());
                /* for clients ending their week on Saturday */
                //if (ClientInfo.ClientID == 256 || ClientInfo.ClientID == 121 || ClientInfo.ClientID == 258 || ClientInfo.ClientID == 205 )
                //    WeekEnding = WeekEnding.AddDays(-1);
            }

            //clientLocation = Convert.ToInt32(ddlMembers.SelectedValue);
            if (_exportType != ExportType.None)
            {
                this.EnableViewState = false;
                string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);
                string exportDetail = Server.UrlDecode((string)Request.QueryString["detail"]);
                sortByDept = Server.UrlDecode((string)Request.QueryString["sortOrder"]).ToUpper().Equals("DEPT");
                showExact = Server.UrlDecode((string)Request.QueryString["exact"]).ToUpper().Equals("TRUE");
                clientLocation = Convert.ToInt32(Server.UrlDecode((string)Request.QueryString["loc"]));

                if (exportDetail == "1")
                {
                    _exportDetail = true;
                }

                this.WeekEnding = DateTime.Parse(dateTime);
                this.LoadHoursReport();
                this.pnlHeader.Visible = false;
                this.mnuHoursReport.Visible = false;
            }
            else
            {

                //this.lblExcelTitle.Visible = false;
                //this.lblExcelWeekEnding.Visible = false;
            }
            //this.txtCalendar.Text = this.WeekEnding.ToString();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string userId = Context.User.Identity.Name.ToLower();
            this.btnSubmitApproved.Enabled = false;
            this.lnkExport.Visible = false;
            this.lnkExportDetail.Visible = false;
            this.lnkCreateInvoice.Visible = false;
            this.pnlApprovalInfo.Visible = false;
            this.btnCSV.Visible = false;

            if (this.pnlTotals.Visible)
            {
                this.lblRegularHrs.Text = _reportHours.ToString("N2");
                this.lblOTHrs.Text = _reportOTHours.ToString("N2");
            }
            string sortOrder = "shift";
            if (rbtnDepts.Checked == true)
                sortOrder = "dept";
            this.lnkExport.NavigateUrl = "~/auth/GroupHoursReportExcel.aspx?detail=0&date=" + this.GetSelectedDate() + "&sortOrder=" + sortOrder
                + "&exact=" + chkBoxExact.Checked.ToString() + "&loc=" + clientLocation; // ddlMembers.SelectedValue;
            this.lnkExportDetail.NavigateUrl = "~/auth/GroupHoursReportExcel.aspx?detail=1&date=" + this.GetSelectedDate() + "&sortOrder=" + sortOrder
                + "&exact=" + chkBoxExact.Checked.ToString() + "&loc=" + clientLocation; // ddlMembers.SelectedValue;
            Boolean codePermissionOverride = true;
            String user = Context.User.Identity.Name.ToLower();
            if ((user.Equals("dalep") || user.Equals("ericks") || user.Equals("julioc")
                || user.Equals("jiml") || user.Equals("josem") || user.Equals("manuelp")
                 || user.Equals("markw") || user.Equals("teresab") || user.Equals("shaund"))
                   && _clientId == 7)
                codePermissionOverride = false;
            if (_clientId == 258 && !user.Equals("shired"))
                codePermissionOverride = false;
            if ((ClientPrefs.DisplayInvoice || ClientPrefs.NotifyHoursReady) && codePermissionOverride)
            {
                if (_hoursLoaded)
                {
                    if( userId.Equals("moakes") || userId.Equals("mchavez") || 
                        userId.Equals("imolina") || userId.Equals("itdept") || userId.Equals("gomezs"))
                        this.btnCSV.Visible = true;
                    if (_exportType == ExportType.None)
                    {
                        this.btnSubmitApproved.Visible = true;
                        if (_allowElectronicApproval)
                            this.btnSubmitApproved.Enabled = true;
                        else
                            this.btnSubmitApproved.Enabled = false;
                        if (_hoursReport != null && _hoursReport.EmployeeHistoryCollection.Count > 0)
                        {
                            this.lnkExport.Visible = true;
                            this.lnkExportDetail.Visible = true;
                        }
                        /*
                        if (ClientPrefs.NotifyHoursReady || ClientPrefs.DisplayInvoice )
                        {
                            if ((Context.User.Identity.Name.ToUpper() == "TCAMPBELL") || (Context.User.Identity.Name.ToUpper() == "MCHAVEZ") || (Context.User.Identity.Name.ToUpper() == "MOAKES") || (Context.User.Identity.Name.ToUpper() == "ITDEPT") || (Context.User.Identity.Name.ToUpper() == "JPAA"))
                            {

                                this.btnSubmitApproved.Enabled = true;
                            }
                            else
                            {
                                this.btnSubmitApproved.Visible = false;
                            }
                        }*/
                        //else if (  _allowElectronicApproval  &&  _hoursReport != null && _hoursReport.EmployeeHistoryCollection.Count > 0)
                        //{
                        //    this.btnSubmitApproved.Enabled = true;
                        //    this.lnkExport.Visible = true;
                        //    this.lnkExportDetail.Visible = true;
                        //}
                    }
                }
            }
            else
            {
                this.btnSubmitApproved.Visible = true;
                this.btnSubmitApproved.Enabled = false;
            }

            if (_hoursReport != null)
            {
                if (_exportType == ExportType.None)
                {
                    this.lnkExport.Visible = true;
                    this.lnkExportDetail.Visible = true;
                }
                if (_hoursReport.IsApproved /*|| Context.User.Identity.Name.ToUpper().Equals("ITDEPT")*/)
                {
                    if (ClientPrefs.DisplayInvoice)
                    {
                        if (user.Equals("tcampbell") || (user.Equals("mchavez")) ||
                            user.Equals("moakes") || user.Equals("itdept") ||
                            user.Equals("lisa") || user.Equals("jpaa"))
                        {
                            this.lnkCreateInvoice.Visible = true;
                        }
                        else
                        {
                            this.lnkCreateInvoice.Visible = false;
                        }
                    }
                    this.btnSubmitApproved.Enabled = false;
                    this.pnlApprovalInfo.Visible = true;
                    this.lblApprovalDate.Text = _hoursReport.ApprovalDateTime.ToString("M/dd/yyyy h:mm tt");
                    this.lblApprovedBy.Text = _hoursReport.ApprovalUserName.ToUpper();
                }

                this.hdnClientApprovalId.Value = _hoursReport.ClientApprovalId.ToString();
                this.hdnWeekEndDate.Value = this.WeekEnding.ToString("M/dd/yyyy");

                //store the hours report for approval
                Session["hoursreport"] = _hoursReport;
            }

            if (_exportType == ExportType.Excel)
            {
                this.lnkCreateInvoice.Visible = false;
                this.hdnClientApprovalId.Visible = false;
                this.hdnWeekEndDate.Visible = false;
                this.pnlApprovalInfo.Visible = false;
            }
        }

        protected void rptrHoursReport_ItemCommand(Object src, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "approve")
            {
                CheckBox chkApprove = null;
                string employeePunchList = string.Empty;
                HiddenField hdnEmployeePunchList = null;
                string punchesStr = "";
                foreach (RepeaterItem item in rptrHoursReport.Items)
                {
                    chkApprove = (CheckBox)item.FindControl("chkBoxApprove");
                    if (chkApprove != null && chkApprove.Checked)
                    {
                        /* get punches for the day */
                        hdnEmployeePunchList = (HiddenField)item.FindControl("hdnEmployeePunchList");
                        string[] punchListStr = hdnEmployeePunchList.Value.ToString().Split(',');
                        List<DateTime> punchList = new List<DateTime>();
                        for (int i = 0; i < punchListStr.Length; i++)
                        {
                            DateTime dt = DateTime.Parse(punchListStr[i]);
                            if (dt.Year > 2000)
                                punchList.Add(dt);
                        }
                        punchList.Sort();
                        DateTime firstPunch = punchList[0];
                        DateTime lastPunch = punchList[punchList.Count - 1];
                        if (punchesStr != string.Empty)
                            punchesStr += ", ";
                        punchesStr += ((HiddenField)item.FindControl("hdnBadgeNumber")).Value.ToString() + ", ";
                        punchesStr += firstPunch + ", " + lastPunch;
                    }
                }
                if (punchesStr != string.Empty)
                {
                    //approve the hours
                    StringBuilder approvalXML = new StringBuilder();
                    approvalXML.Append("<approvalxml><employeepunchtimes>");

                    string[] list = punchesStr.Split(new char[] { ',' });
                    int i = 0;
                    while (i < list.Length)
                    {
                        approvalXML.Append("<employee>");
                        string l = "";
                        for (int j = 0; j < list[i].Length; j++)
                            if (list[i][j] >= '0' && list[i][j] <= '9')
                                l += list[i][j];
                        l = '%' + l;

                        approvalXML.Append("<ID>" + l + "</ID>");
                        i++;
                        approvalXML.Append("<firstpunch>" + list[i] + "</firstpunch>");
                        i++;
                        list[i] = DateTime.Parse(list[i]).AddMinutes(2).ToString();
                        approvalXML.Append("<lastpunch>" + list[i] + "</lastpunch>");
                        i++;
                        approvalXML.Append("</employee>");
                    }
                    approvalXML.Append("</employeepunchtimes></approvalxml>");
                    HoursReport hrApproval = new HoursReport();
                    hrApproval.ApprovalXML = approvalXML.ToString();

                    HoursReportBL hoursReportBL = new HoursReportBL();
                    hoursReportBL.ApprovePunchRange(hrApproval, Context.User);
                }
            }
            LoadHoursReport();
        }

        protected void rptrHoursReport_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (ClientPrefs.ApproveHours && _showApproved == false && _exportType == ExportType.None)
                {
                    /* create a new table data item */
                    HtmlTableCell tc = new HtmlTableCell();
                    tc.BgColor = "#003776";
                    Button btnAppr = (Button)e.Item.FindControl("btnApprove");
                    btnAppr.Visible = true;
                    //btnAppr.BackColor = System.Drawing.Color.LightBlue;
                    btnAppr.CommandName = "approve";
                    tc.Controls.Add(btnAppr);

                    PlaceHolder ph = ((PlaceHolder)e.Item.FindControl("tdApproveHeaderPH"));
                    ph.Controls.Add(tc);
                }
                if (this.Context.User.Identity.Name.ToUpper().Equals("WATKINSK") && this._hoursReport.MultiDepts)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdTotalHours")).Visible = true;
                }
                else
                    ((HtmlTableCell)e.Item.FindControl("tdTotalHours")).Visible = false;

                if (this.ClientPrefs.DisplayBonuses == true && (this.Context.User.Identity.Name.ToLower().Equals("itdept")
                        || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")))
                {
                    ((HtmlTableCell)e.Item.FindControl("tdBonusPay")).Visible = true;
                    if (_exportType == ExportType.Excel)
                        ((HtmlTableCell)e.Item.FindControl("tdBonusPayMU")).Visible = true;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdBonusPay")).Visible = false;
                    ((HtmlTableCell)e.Item.FindControl("tdBonusPayMU")).Visible = false;
                }
            }
            btnApproveEnabled = true;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundEmployeeIdx++;
                _boundEmployeeHistory = (EmployeeHistory)e.Item.DataItem;

                if (ClientInfo.IgnoreShiftList != null)
                {
                    if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                    {
                        EmployeeWorkSummary summary2 = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        if (ClientInfo.IgnoreShiftList.Contains(summary2.ShiftInfo.ShiftID))
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                }
                if (ClientInfo.ClientID == 127) //IP
                {
                    if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                    {
                        EmployeeWorkSummary summary2 = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        if (summary2.ShiftInfo.ShiftID == 1201) //plant2
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                }

                if (_exportType == ExportType.Excel)
                {
                    if (((HtmlTableCell)e.Item.FindControl("tdDeptSpc")) != null)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdDeptSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdDeptSpc")).ColSpan = 12;
                    }
                    if (((HtmlTableCell)e.Item.FindControl("tdDepartmentTotalsSpc")) != null)
                        ((HtmlTableCell)e.Item.FindControl("tdDepartmentTotalsSpc")).Visible = false;
                    if (((HtmlTableCell)e.Item.FindControl("tdEmpCntSpc")) != null)
                        ((HtmlTableCell)e.Item.FindControl("tdEmpCntSpc")).Visible = false;
                    if (((Label)e.Item.FindControl("lblEmpCnt")) != null)
                        ((Label)e.Item.FindControl("lblEmpCnt")).Visible = false;
                }

                if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                {
                    if ( clientLocation > 0 &&
                        ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).ClientID != clientLocation)//this.ddlMembers.SelectedValue
                    {
                        e.Item.Visible = false;
                        return;
                    }
                    else
                    {
                        // put the employee count into the panel/label
                        ((Label)e.Item.FindControl("lblEmpCnt")).Text = Convert.ToString(_boundEmployeeIdx);

                        //see if we need to display payrate
                        if (ClientPrefs.DisplayPayRate && payRateJobCodeEligible == true)
                        {
                            ((Panel)e.Item.FindControl("pnlPayRate")).Visible = true;
                            ((Label)e.Item.FindControl("lblPayRate")).Text = _boundEmployeeHistory.GetEmployeePayRate().ToString("$#,##0.00");
                        }
                        //see if we need to display jobcode
                        if (ClientPrefs.DisplayJobCode && payRateJobCodeEligible == true)
                        {
                            ((Panel)e.Item.FindControl("pnlJobCode")).Visible = true;
                            ((Label)e.Item.FindControl("lblJobCode")).Text = _boundEmployeeHistory.GetEmployeeJobCode();
                            ((HtmlTableCell)e.Item.FindControl("tdDeptSpacer")).Visible = true;
                            ((HtmlTableCell)e.Item.FindControl("tdShiftSpacer")).Visible = true;
                        }
                        if (_clientId == 256) //256 = CWInhouse, they want CW instead of TE or TA...
                        {
                            ((Label)e.Item.FindControl("lblBadgeNumber")).Text = "CW" + ((Label)e.Item.FindControl("lblBadgeNumber")).Text.Substring(2);
                            ((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text = "CW" + ((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text.Substring(2);
                        }
                        if (ClientPrefs.ApproveHours)
                        {
                            HtmlTableCell tc = (HtmlTableCell)e.Item.FindControl("tdApproveHours");
                            tc.Visible = true;
                            if (_boundEmployeeHistory.HasInvalidWorkSummaries)
                            {
                                CheckBox cb = (CheckBox)e.Item.FindControl("chkBoxApprove");
                                cb.Enabled = false;
                                cb.Checked = false;
                            }
                        }
                        //set the color of the labels if there are any invalid summaries
                        if (_boundEmployeeHistory.HasInvalidWorkSummaries)
                        {
                            _allowElectronicApproval = false;
                            ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.Red;
                            ((Label)e.Item.FindControl("lblBadgeNumberExcel")).ForeColor = System.Drawing.Color.Red;
                            ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Red;
                            ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Red;
                        }
                        else if (this.ClientPrefs.ApproveHours == true && _showApproved == false )
                        {
                            if (_boundEmployeeHistory.HasUnapprovedHours)
                            {
                                _allowElectronicApproval = false;
                                if (_exportType == ExportType.None)
                                {
                                    HtmlTableCell tc = (HtmlTableCell)e.Item.FindControl("tdApproveHours");
                                    tc.Visible = true;
                                    ((CheckBox)e.Item.FindControl("chkBoxApprove")).Visible = true;

                                    HiddenField hdnEmpPnchList = (HiddenField)e.Item.FindControl("hdnEmployeePunchList");
                                    hdnEmpPnchList.ID = "hdnEmployeePunchList";
                                    hdnEmpPnchList.Value = "";
                                    hdnEmpPnchList.Value += _boundEmployeeHistory.SundaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.SundaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.MondaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.MondaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.TuesdaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.TuesdaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.WednesdaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.WednesdaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.ThursdaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.ThursdaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.FridaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.FridaySummary.LastPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.SaturdaySummary.FirstPunch;
                                    hdnEmpPnchList.Value += ", " + _boundEmployeeHistory.SaturdaySummary.LastPunch;

                                    HiddenField hdnBadgeNumber = (HiddenField)e.Item.FindControl("hdnBadgeNumber");
                                    hdnBadgeNumber.Value = _boundEmployeeHistory.TempNumber;
                                }
                                else
                                {
                                    ((HtmlTableCell)e.Item.FindControl("tdApproveHours")).Visible = false;
                                }

                                ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.Blue;
                                ((Label)e.Item.FindControl("lblBadgeNumberExcel")).ForeColor = System.Drawing.Color.Blue;
                                ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Blue;
                                ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Blue;
                                ((Label)e.Item.FindControl("lblUnApproved")).ForeColor = System.Drawing.Color.Blue;
                                ((Label)e.Item.FindControl("lblUnApproved")).Visible = true;
                            }
                        }
                    //}
                        HtmlTableRow trDepartment = (HtmlTableRow)e.Item.FindControl("trDepartment");
                        HtmlTableCell tdDepartmentHead = (HtmlTableCell)e.Item.FindControl("tdDepartmentHead");
                        EmployeeWorkSummary summary2 = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        HtmlTableRow trDepartmentTotals2 = (HtmlTableRow)e.Item.FindControl("trDepartmentTotals");
                        HtmlTableCell tdDepartmentTotalLabel2 = (HtmlTableCell)e.Item.FindControl("tdDepartmentTotalLabel");
                        if (summary2.DepartmentInfo.DepartmentID != _currentDepartmentId || summary2.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                        {
                            //show the department break;
                            if (summary2.DepartmentInfo.DepartmentID != _currentDepartmentId || summary2.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                            {
                                trDepartment.Visible = true;
                                if (ClientPrefs.DisplayJobCode && ClientPrefs.DisplayPayRate)
                                    tdDepartmentHead.ColSpan = 13;
                                else if (ClientPrefs.DisplayJobCode || ClientPrefs.DisplayPayRate)/*_clientId == 92 || _clientId == 181 || _clientId == 226 || _clientId == 178 || 
                                    _clientId == 256 || _clientId == 258)*/
                                {
                                    tdDepartmentHead.ColSpan = 12;
                                }
                                if (ClientPrefs.ApproveHours && _exportType != ExportType.Excel)
                                    tdDepartmentHead.ColSpan++;

                                if (_showApproved)
                                {
                                    ((Label)e.Item.FindControl("lblDepartment")).Text = summary2.DepartmentInfo.DepartmentName + " - " + summary2.ShiftTypeInfo.ToString() + ": APPROVED";
                                }
                                else
                                {
                                    ((Label)e.Item.FindControl("lblDepartment")).Text = summary2.DepartmentInfo.DepartmentName + " - " + summary2.ShiftTypeInfo.ToString();
                                }
                                ((HiddenField)e.Item.FindControl("hdnDept")).Value = summary2.DepartmentInfo.DepartmentID.ToString();

                                _currentDepartmentId = summary2.DepartmentInfo.DepartmentID;
                                _currentShiftType = summary2.ShiftTypeInfo.ShiftTypeId;
                            }
                        }
                        else
                        {
                            trDepartment.Visible = false;
                        }
                        if (_exportType == ExportType.Excel)
                            tdDepartmentHead.ColSpan--;

                        //calculate total hours
                        this.CalculateTotalHours();

                        if (!_exportDetail)
                        {
                            //previous code
                            if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Sunday))
                            {
                                displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.MondaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.TuesdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.WednesdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.ThursdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.FridaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.SaturdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.SundaySummary);

                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay1HiddenHours")), _boundEmployeeHistory.MondaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay2HiddenHours")), _boundEmployeeHistory.TuesdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay3HiddenHours")), _boundEmployeeHistory.WednesdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay4HiddenHours")), _boundEmployeeHistory.ThursdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay5HiddenHours")), _boundEmployeeHistory.FridaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay6HiddenHours")), _boundEmployeeHistory.SaturdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay7HiddenHours")), _boundEmployeeHistory.SundaySummary);
                            }
                            else
                            {
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay2HiddenHours")), _boundEmployeeHistory.MondaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay3HiddenHours")), _boundEmployeeHistory.TuesdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay4HiddenHours")), _boundEmployeeHistory.WednesdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay5HiddenHours")), _boundEmployeeHistory.ThursdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay6HiddenHours")), _boundEmployeeHistory.FridaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay7HiddenHours")), _boundEmployeeHistory.SaturdaySummary);
                                displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay1HiddenHours")), _boundEmployeeHistory.SundaySummary);

                                displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.MondaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.TuesdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.WednesdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.ThursdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.FridaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.SaturdaySummary);
                                displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.SundaySummary);
                            }
                        }
                        else
                        {
                            //Mon
                            ((HtmlTableCell)e.Item.FindControl("tdMon")).Visible = false;
                            //Tue
                            ((HtmlTableCell)e.Item.FindControl("tdTue")).Visible = false;
                            //Wed
                            ((HtmlTableCell)e.Item.FindControl("tdWed")).Visible = false;
                            //Thu
                            ((HtmlTableCell)e.Item.FindControl("tdThu")).Visible = false;
                            //Fri
                            ((HtmlTableCell)e.Item.FindControl("tdFri")).Visible = false;
                            //Sat
                            ((HtmlTableCell)e.Item.FindControl("tdSat")).Visible = false;
                            //Sun
                            ((HtmlTableCell)e.Item.FindControl("tdSun")).Visible = false;
                        }

                        ((Label)e.Item.FindControl("lblTotalHours")).Text = _boundEmployeeHistory.TotalRegularHours.ToString("N2");
                        if (this.Context.User.Identity.Name.ToUpper().Equals("WATKINSK"))
                        {
                            if (_boundEmployeeHistory.TotalHoursAllDepts > 0)
                                ((Label)e.Item.FindControl("lblTotalWeek")).Text = _boundEmployeeHistory.TotalHoursAllDepts.ToString("N2");
                            else
                                ((Label)e.Item.FindControl("lblTotalWeek")).Text = " ";
                        }
                        else
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdTotalWeek")).Visible = false;
                        }

                        if (this.ClientPrefs.DisplayBonuses && (this.Context.User.Identity.Name.ToUpper().Equals("ITDEPT")
                            || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                            || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")))
                        {
                            ((Label)e.Item.FindControl("lblBonusItem")).Text = _boundEmployeeHistory.Bonus.ToString("N2");
                            ((Label)e.Item.FindControl("lblBonusItemMU")).Text = (_boundEmployeeHistory.Bonus * 1.1).ToString();
                            BonusTotals += _boundEmployeeHistory.Bonus;
                            BonusGrandTotal += _boundEmployeeHistory.Bonus;
                            if (_exportType == ExportType.Excel)
                            {
                                if (_boundEmployeeHistory.Bonus == 0)
                                {
                                    ((Label)e.Item.FindControl("lblBonusItem")).Parent.Visible = false;
                                    ((Label)e.Item.FindControl("lblBonusItemMU")).Parent.Visible = false;
                                }
                                else
                                {
                                    ((Label)e.Item.FindControl("lblBonusItem")).Parent.Visible = true;
                                    ((Label)e.Item.FindControl("lblBonusItemMU")).Parent.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblBonusItem")).Parent.Visible = false;
                            ((Label)e.Item.FindControl("lblBonusItemMU")).Parent.Visible = false;
                        }
                        ((Label)e.Item.FindControl("lblOTHours")).Text = _boundEmployeeHistory.TotalOTHours.ToString("N2");

                        Repeater detail = e.Item.FindControl("rptrHoursReportDetail") as Repeater;
                        Repeater detailExcel = e.Item.FindControl("rptrHoursReportDetail_Excel") as Repeater;

                        if (detail != null)
                        {
                            Label lblBadgeNumberExcel = (Label)e.Item.FindControl("lblBadgeNumberExcel");
                            if (_exportDetail)
                            {
                                detailExcel.Visible = true;
                                detail.Visible = false;
                                ((Panel)e.Item.FindControl("pnlPlusMinus")).Visible = false;
                                lblBadgeNumberExcel.Visible = true;
                                detailExcel.DataSource = _boundEmployeeHistory.WorkSummaries;
                                detailExcel.DataBind();
                            }
                            else
                            {
                                detailExcel.Visible = false;
                                if (_exportType == ExportType.None)
                                {
                                    detail.Visible = true;
                                    lblBadgeNumberExcel.Visible = false;
                                    detail.DataSource = _boundEmployeeHistory.WorkSummaries;
                                    detail.DataBind();
                                }
                                else
                                {
                                    detail.Visible = false;
                                    lblBadgeNumberExcel.Visible = true;
                                    ((Panel)e.Item.FindControl("pnlPlusMinus")).Visible = false;
                                }
                            }
                        }
                    }

                    EmployeeWorkSummary summary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                    HtmlTableRow trDepartmentTotals = (HtmlTableRow)e.Item.FindControl("trDepartmentTotals");
                    HtmlTableCell tdDepartmentTotalLabel = (HtmlTableCell)e.Item.FindControl("tdDepartmentTotalLabel");

                    //check if we should show totals
                    EmployeeHistory nextEmployee = null;
                    EmployeeWorkSummary nextSummary = null;

                    
                    if (e.Item.ItemIndex < _hoursReport.EmployeeHistoryCollection.Count - 1)
                    {
                        nextEmployee = (EmployeeHistory)_hoursReport.EmployeeHistoryCollection[e.Item.ItemIndex + 1];

                        if (nextEmployee.WorkSummaries.Count > 0)
                        {
                            nextSummary = (EmployeeWorkSummary)nextEmployee.WorkSummaries[0];
                        }
                        else
                        {
                            nextSummary = null;
                        }
                    }

                    if (nextSummary == null || (e.Item.ItemIndex == _hoursReport.EmployeeHistoryCollection.Count - 1) || 
                        nextSummary.DepartmentInfo.DepartmentID != summary.DepartmentInfo.DepartmentID || 
                        nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId ||
                        ( clientLocation > 0 && nextSummary.ClientID != clientLocation) )
                    {
                        //always display department in this case
                        trDepartmentTotals.Visible = true;

                        /* set up bonus totals */
                        if (this.ClientPrefs.DisplayBonuses == true && (this.Context.User.Identity.Name.ToLower().Equals("itdept")
                            || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                            || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")))
                        {
                            ((Label)e.Item.FindControl("lblDeptTotBonus")).Text = "$ " + BonusTotals.ToString("N2");
                            ((Label)e.Item.FindControl("lblDeptTotBonusMU")).Text = "$ " + (BonusTotals*1.1).ToString("N2");
                            if( _exportType == ExportType.Excel )
                            {
                                ((Label)e.Item.FindControl("lblDeptTotBonusMU")).Parent.Visible = true;
                                if (BonusTotals == 0)
                                {
                                    ((Label)e.Item.FindControl("lblDeptTotBonus")).Text = "";
                                    ((Label)e.Item.FindControl("lblDeptTotBonusMU")).Text = "";
                                }
                            }
                            BonusTotals = 0;
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblDeptTotBonus")).Parent.Visible = false;
                        }

                        if( ClientPrefs.DisplayPayRate && payRateJobCodeEligible )
                        {
                            tdDepartmentTotalLabel.ColSpan = 3;
                        }

                        if (_showApproved)
                        {
                            ((Label)e.Item.FindControl("lblDepartmentTotalLabel")).Text = summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString() + " Totals APPROVED:";
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblDepartmentTotalLabel")).Text = summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString() + " Totals:";
                        }

                        if (nextSummary == null || (e.Item.ItemIndex == _hoursReport.EmployeeHistoryCollection.Count - 1) || (nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId))
                        {

                        }

                        if (!_exportDetail)
                        {
                            int offset = 0;
                            if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Saturday))
                                offset = 6;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay1Hours")).Text = _totDailyHours[(0 + offset) % 7].ToString("N2");
                            _totDailyHours[(0 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay2Hours")).Text = _totDailyHours[(1 + offset) % 7].ToString("N2");
                            _totDailyHours[(1 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay3Hours")).Text = _totDailyHours[(2 + offset) % 7].ToString("N2");
                            _totDailyHours[(2 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay4Hours")).Text = _totDailyHours[(3 + offset) % 7].ToString("N2");
                            _totDailyHours[(3 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay5Hours")).Text = _totDailyHours[(4 + offset) % 7].ToString("N2");
                            _totDailyHours[(4 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay6Hours")).Text = _totDailyHours[(5 + offset) % 7].ToString("N2");
                            _totDailyHours[(5 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay7Hours")).Text = _totDailyHours[(6 + offset) % 7].ToString("N2");
                            _totDailyHours[(6 + offset) % 7] = 0M;
                        }
                        else
                        {
                            //Mon
                            ((HtmlTableCell)e.Item.FindControl("tdDeptMonTotal")).Visible = false;
                            _totDailyHours[0] = 0M;
                            //Tue
                            ((HtmlTableCell)e.Item.FindControl("tdDeptTueTotal")).Visible = false;
                            _totDailyHours[1] = 0M;
                            //Wed
                            ((HtmlTableCell)e.Item.FindControl("tdDeptWedTotal")).Visible = false;
                            _totDailyHours[2] = 0M;
                            //Thu
                            ((HtmlTableCell)e.Item.FindControl("tdDeptThuTotal")).Visible = false;
                            _totDailyHours[3] = 0M;
                            //Fri
                            ((HtmlTableCell)e.Item.FindControl("tdDeptFriTotal")).Visible = false;
                            _totDailyHours[4] = 0M;
                            //Sat
                            ((HtmlTableCell)e.Item.FindControl("tdDeptSatTotal")).Visible = false;
                            _totDailyHours[5] = 0M;
                            //Sun
                            ((HtmlTableCell)e.Item.FindControl("tdDeptSunTotal")).Visible = false;
                            _totDailyHours[6] = 0M;
                        }
                        ((Label)e.Item.FindControl("lblDeptTotTotalHours")).Text = _totDailyHours[7].ToString("N2");
                        _totDailyHours[7] = 0M;
                        ((Label)e.Item.FindControl("lblDeptTotOTHours")).Text = _totDailyHours[8].ToString("N2");
                        _totDailyHours[8] = 0M;

                    }
                    else
                    {
                        trDepartmentTotals.Visible = false;
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if ( /*( _clientId == 92 || _clientId == 181 || _clientId == 256 || 
                    _clientId == 178 || _clientId == 226 || _clientId == 258) */
                    ClientPrefs.DisplayPayRate && payRateJobCodeEligible)
                {
                    ((Panel)e.Item.FindControl("pnlPayRateHead")).Visible = true;
                }
                if ( /*_clientId == 226 || ((_clientId == 178 || _clientId == 256) */
                    ClientPrefs.DisplayJobCode && payRateJobCodeEligible)
                    ((Panel)e.Item.FindControl("pnlJobCodeHead")).Visible = true;
                else
                    ((Panel)e.Item.FindControl("pnlJobCodeHead")).Visible = false;



                /* put proper day in column */
                if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Saturday))
                {
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Tue";
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Sun";
                }
                else
                {
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Tue";
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Sun";
                }

                HtmlTableRow trExcelTitle = (HtmlTableRow)e.Item.FindControl("trExcelTitle");
                HtmlTableRow trExcelWeekEnding = (HtmlTableRow)e.Item.FindControl("trExcelWeekEnding");
                if (_exportType != ExportType.None)
                {
                    trExcelTitle.Visible = true;
                    trExcelWeekEnding.Visible = true;
                    if (payRateJobCodeEligible)
                    {
                        if (ClientPrefs.DisplayPayRate && ClientPrefs.DisplayJobCode)
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 4;
                        else if (ClientPrefs.DisplayPayRate || ClientPrefs.DisplayJobCode)
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 3;
                        }
                        else
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 2;
                        }
                    }

                    ((Label)e.Item.FindControl("lblClientName")).Text = ClientInfo.ClientName;
                    //((HtmlTableCell)e.Item.FindControl("tdExcelClientName")).ColSpan = 2;

                    if (_exportType == ExportType.Excel)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdExcelTitleSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdClientNameSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdDateSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("seqNum")).Visible = false;
                    }
                    ((Label)e.Item.FindControl("lblExcelWeekEnding")).Text = "Week Ending " + this.WeekEnding.ToString("MM/dd/yyyy");
                    ((Label)e.Item.FindControl("lblExcelWeekDay1")).Text = this.WeekEnding.AddDays(-6).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay2")).Text = this.WeekEnding.AddDays(-5).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay3")).Text = this.WeekEnding.AddDays(-4).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay4")).Text = this.WeekEnding.AddDays(-3).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay5")).Text = this.WeekEnding.AddDays(-2).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay6")).Text = this.WeekEnding.AddDays(-1).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblExcelWeekDay7")).Text = this.WeekEnding.AddDays(-0).Date.ToString("M/d");
                }
                else
                {
                    trExcelTitle.Visible = false;
                    trExcelWeekEnding.Visible = false;
                }

                if (_exportDetail)
                {
                    //Mon
                    ((HtmlTableCell)e.Item.FindControl("tdMonHeader")).Visible = false;
                    //Tue
                    ((HtmlTableCell)e.Item.FindControl("tdTueHeader")).Visible = false;
                    //Wed
                    ((HtmlTableCell)e.Item.FindControl("tdWedHeader")).Visible = false;
                    //Thu
                    ((HtmlTableCell)e.Item.FindControl("tdThuHeader")).Visible = false;
                    //Fri
                    ((HtmlTableCell)e.Item.FindControl("tdFriHeader")).Visible = false;
                    //Sat
                    ((HtmlTableCell)e.Item.FindControl("tdSatHeader")).Visible = false;
                    //Sun
                    ((HtmlTableCell)e.Item.FindControl("tdSunHeader")).Visible = false;
                }
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                if (_exportType == ExportType.Excel)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGrandTotalsSpc")).Visible = false;
                }
                if (ClientPrefs.DisplayPayRate && payRateJobCodeEligible)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan = 3;
                }
                if (ClientPrefs.DisplayJobCode /*_clientId == 226 || ((_clientId == 178 || _clientId == 256 )*/
                    && payRateJobCodeEligible)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGTSpacer")).Visible = true;
                }

                if (!_exportDetail)
                {
                    int offset = 0;
                    if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Saturday))
                        offset = 6;
                    //Mon
                    ((Label)e.Item.FindControl("lblGrandWeekDay1Hours")).Text = _grandTotDailyHours[(0 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(0 + offset) % 7] = 0M;
                    //Tue
                    ((Label)e.Item.FindControl("lblGrandWeekDay2Hours")).Text = _grandTotDailyHours[(1 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(1 + offset) % 7] = 0M;
                    //Wed
                    ((Label)e.Item.FindControl("lblGrandWeekDay3Hours")).Text = _grandTotDailyHours[(2 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(2 + offset) % 7] = 0M;
                    //Thu
                    ((Label)e.Item.FindControl("lblGrandWeekDay4Hours")).Text = _grandTotDailyHours[(3 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(3 + offset) % 7] = 0M;
                    //Fri
                    ((Label)e.Item.FindControl("lblGrandWeekDay5Hours")).Text = _grandTotDailyHours[(4 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(4 + offset) % 7] = 0M;
                    //Sat
                    ((Label)e.Item.FindControl("lblGrandWeekDay6Hours")).Text = _grandTotDailyHours[(5 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(5 + offset) % 7] = 0M;
                    //Sun
                    ((Label)e.Item.FindControl("lblGrandWeekDay7Hours")).Text = _grandTotDailyHours[(6 + offset) % 7].ToString("N2");
                    _grandTotDailyHours[(6 + offset) % 7] = 0M;
                }
                else
                {
                    //Mon
                    ((HtmlTableCell)e.Item.FindControl("tdMonGrandTotal")).Visible = false;
                    //Tue
                    ((HtmlTableCell)e.Item.FindControl("tdTueGrandTotal")).Visible = false;
                    //Wed
                    ((HtmlTableCell)e.Item.FindControl("tdWedGrandTotal")).Visible = false;
                    //Thu
                    ((HtmlTableCell)e.Item.FindControl("tdThuGrandTotal")).Visible = false;
                    //Fri
                    ((HtmlTableCell)e.Item.FindControl("tdFriGrandTotal")).Visible = false;
                    //Sat
                    ((HtmlTableCell)e.Item.FindControl("tdSatGrandTotal")).Visible = false;
                    //Sun
                    ((HtmlTableCell)e.Item.FindControl("tdSunGrandTotal")).Visible = false;
                }
                ((Label)e.Item.FindControl("lblGrandTotalHours")).Text = _reportHours.ToString("N2");
                ((Label)e.Item.FindControl("lblGrandOTHours")).Text = _reportOTHours.ToString("N2");

                if (this.ClientPrefs.DisplayBonuses && (this.Context.User.Identity.Name.ToUpper().Equals("ITDEPT")
                        || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")))
                {
                    ((Label)e.Item.FindControl("lblGrandTotBonus")).Text = "$ " + BonusGrandTotal.ToString("N2");
                    ((Label)e.Item.FindControl("lblGrandTotBonusMU")).Text = "$ " + (BonusGrandTotal * 1.1).ToString("N2");
                    if( _exportType != ExportType.Excel )
                        ((Label)e.Item.FindControl("lblGrandTotBonusMU")).Parent.Visible = false;
                }
                else
                {
                    ((Label)e.Item.FindControl("lblGrandTotBonus")).Parent.Visible = false;
                    ((Label)e.Item.FindControl("lblGrandTotBonusMU")).Parent.Visible = false;
                }
            }
        }

        protected void rptrHoursReportDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblCheckIn = (Label)e.Item.FindControl("lblCheckIn");
                Label lblCheckOut = (Label)e.Item.FindControl("lblCheckOut");
                Label lblDetailHours = (Label)e.Item.FindControl("lblDetailHours");
                Label lblCheckInExact = (Label)e.Item.FindControl("lblCheckInExact");
                Label lblCheckOutExact = (Label)e.Item.FindControl("lblCheckOutExact");
                Label lblDetailHoursExact = (Label)e.Item.FindControl("lblDetailHoursExact");

                HtmlInputHidden hdnCheckInExact = (HtmlInputHidden)e.Item.FindControl("hdnCheckInExact");
                HtmlInputHidden hdnCheckOutExact = (HtmlInputHidden)e.Item.FindControl("hdnCheckOutExact");
                HtmlInputHidden hdnPunchInId = (HtmlInputHidden)e.Item.FindControl("hdnPunchInId");
                HtmlInputHidden hdnPunchOutId = (HtmlInputHidden)e.Item.FindControl("hdnPunchOutId");

                EmployeeWorkSummary workSummary = (EmployeeWorkSummary)e.Item.DataItem;
                if (clientLocation > 0 && workSummary.ClientID != clientLocation)
                    return;
                DateTime punchIn = new DateTime(workSummary.RoundedCheckInDateTime.Year, workSummary.RoundedCheckInDateTime.Month, workSummary.RoundedCheckInDateTime.Day, workSummary.RoundedCheckInDateTime.Hour, workSummary.RoundedCheckInDateTime.Minute, 0);
                DateTime punchInExact = new DateTime(workSummary.CheckInDateTime.Year, workSummary.CheckInDateTime.Month, workSummary.CheckInDateTime.Day, workSummary.CheckInDateTime.Hour, workSummary.CheckInDateTime.Minute, workSummary.CheckInDateTime.Second);
                if (_firstPunch.Equals(DATE_NOT_SET))
                {
                    _firstPunch = punchIn;
                }
                lblCheckIn.Text = punchIn.ToString("MM/dd/yyyy h:mm tt");
                if (lblCheckInExact != null)
                {
                    lblCheckInExact.Text = punchInExact.ToString("MM/dd/yyyy h:mm tt");
                    //hdnCheckInExact.Value = punchInExact.ToString("MM/dd/yyyy hh:mm ss tt");
                }
                //hdnPunchInId.Value = "" + workSummary.CheckInEmployeePunchID;
                if (workSummary.CheckOutDateTime != null && workSummary.CheckOutDateTime.Date != new DateTime(1, 1, 1).Date)
                {
                    DateTime punchOut = new DateTime(workSummary.RoundedCheckOutDateTime.Year, workSummary.RoundedCheckOutDateTime.Month, workSummary.RoundedCheckOutDateTime.Day, workSummary.RoundedCheckOutDateTime.Hour, workSummary.RoundedCheckOutDateTime.Minute, 0);
                    DateTime punchOutExact = new DateTime(workSummary.CheckOutDateTime.Year, workSummary.CheckOutDateTime.Month, workSummary.CheckOutDateTime.Day, workSummary.CheckOutDateTime.Hour, workSummary.CheckOutDateTime.Minute, workSummary.CheckOutDateTime.Second);
               
                    _lastPunch = punchOut;
                    lblCheckOut.Text = punchOut.ToString("MM/dd/yyyy h:mm tt");
                    if (lblCheckOutExact != null)
                    {
                        lblCheckOutExact.Text = punchOutExact.ToString("MM/dd/yyyy h:mm tt");
                        //hdnCheckOutExact.Value = punchOutExact.ToString("MM/dd/yyyy hh:mm ss tt");
                    }
                    //hdnPunchOutId.Value = "" + workSummary.CheckOutEmployeePunchID;
                    //sum up the hours
                    TimeSpan difference = punchOut - punchIn;
                    TimeSpan differenceExact = punchOutExact - punchInExact;
                    decimal summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                    decimal summaryHoursExact = Math.Round(Convert.ToDecimal(differenceExact.TotalMinutes / 60), 2);

                    if (lblDetailHoursExact != null)
                        lblDetailHoursExact.Text = summaryHoursExact.ToString("N2");
                    lblDetailHours.Text = summaryHours.ToString("N2");
                    if (ClientPrefs.ApproveHours)
                    {
                        if (!workSummary.CheckInApproved || !workSummary.CheckOutApproved)
                        {
                            lblCheckIn.ForeColor = System.Drawing.Color.Blue;
                            lblCheckOut.ForeColor = System.Drawing.Color.Blue;
                            lblDetailHours.ForeColor = System.Drawing.Color.Blue;
                            if (lblCheckInExact != null)
                                lblCheckInExact.ForeColor = System.Drawing.Color.Blue;
                            if (lblCheckOutExact != null)
                                lblCheckOutExact.ForeColor = System.Drawing.Color.Blue;
                            if (lblDetailHoursExact != null)
                                lblDetailHoursExact.ForeColor = System.Drawing.Color.Blue;
                            lblDetailHours.Text += " (Unapproved)";
                        }
                    }
                }
                else
                {
                    lblCheckIn.ForeColor = System.Drawing.Color.Red;
                    lblCheckOut.Text = "N/A";
                    lblCheckOut.ForeColor = System.Drawing.Color.Red;
                    if (lblCheckOutExact != null)
                    {
                        lblCheckOutExact.ForeColor = System.Drawing.Color.Red;
                        lblCheckOutExact.Text = "N/A";
                    }
                    lblDetailHours.Text = "N/A";
                    lblDetailHours.ForeColor = System.Drawing.Color.Red;
                    if (lblDetailHoursExact != null)
                    {
                        lblDetailHoursExact.Text = "N/A";
                        lblDetailHoursExact.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        protected Boolean GetApproveEnabled()
        {
            return (Boolean)btnApproveEnabled;
        }

        protected string GetBoundEmployeeID()
        {
            EmployeeHistory currentEmployee = null;
            currentEmployee = (EmployeeHistory)_hoursReport.EmployeeHistoryCollection[_boundEmployeeIdx];
            return currentEmployee.EmployeeID.ToString();
        }

        protected string GetBoundEmployeeID(bool boundToEmployee)
        {
            EmployeeHistory currentEmployee = null;
            if (boundToEmployee)
            {
                currentEmployee = _boundEmployeeHistory;
            }
            else
            {
                currentEmployee = (EmployeeHistory)_hoursReport.EmployeeHistoryCollection[_boundEmployeeIdx];
            }

            if (currentEmployee.WorkSummaries.Count > 0)
            {
                EmployeeWorkSummary summary = (EmployeeWorkSummary)currentEmployee.WorkSummaries[0];
                return currentEmployee.EmployeeID.ToString() + "_" +
                    summary.ShiftTypeInfo.ShiftTypeId.ToString() + "_" +
                        summary.DepartmentInfo.DepartmentID.ToString();
            }
            return currentEmployee.EmployeeID.ToString();
        }

        protected void CalculateTotalHours()
        {
            ArrayList workSummaries = _boundEmployeeHistory.WorkSummaries;
            //decimal totalHours = 0M;
            //DateTime punchIn;
            DateTime punchOut = new DateTime(1, 1, 1);
            DateTime roundedPunchOut = new DateTime(1, 1, 1);
            //TimeSpan difference;
            //decimal summaryHours = 0M;
            /*
            foreach (EmployeeWorkSummary summary in workSummaries)
            {
                punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, 0);
                if (summary.CheckOutDateTime != null && summary.CheckOutDateTime.Date != new DateTime(1, 1, 1).Date)
                {
                    punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, summary.RoundedCheckOutDateTime.Minute, 0); 
                    if (_clientInfo.ClientID == 168)
                    {
                        //round to the shift end
                        
                        if (punchOut.Minute >= 18 && punchOut.Minute <= 35)
                        {
                            roundedPunchOut = punchOut.AddMinutes(30 - punchOut.Minute);
                        }
                        else
                        {
                            roundedPunchOut = punchOut;
                        }

                        //sum up the hours
                        difference = roundedPunchOut - punchIn;
                        summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                    }
                    else
                    {
                        //sum up the hours
                        difference = punchOut - punchIn;
                        summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                        //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);
                    }

                    //deduct 1/2 break time for certain clients hour for each day
                    if (_clientInfo.ClientID == 165 || _clientInfo.ClientID == 168 || _clientInfo.ClientID == 181)
                    {
                        decimal breakTime = .5M;

                        if (_clientInfo.ClientID == 168)
                        {
                            //if (summaryHours >= 8.25M && summaryHours <= 8.5M)
                           // {
                           //     summaryHours = 8.0M;
                           // }
                            if (summaryHours >= 8.5M)
                            {
                                summaryHours -= breakTime;
                            }
                            else if (summaryHours > 6)
                            {
                                summaryHours -= breakTime;
                            }
                        }
                        else
                        {
                            if (summaryHours > 6)
                            {
                                summaryHours -= breakTime;
                            }
                        }
                    }

                    //round to nearest quarter hour after break time
                    //summaryHours = Math.Round(summaryHours * 4, 0, MidpointRounding.ToEven) / 4;

                    switch ( punchIn.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            _boundEmployeeHistory.MondayHours += summaryHours;
                            _totDailyHours[0] += summaryHours;
                            _grandTotDailyHours[0] += summaryHours;
                            break;
                        case DayOfWeek.Tuesday:
                            _boundEmployeeHistory.TuesdayHours += summaryHours;
                            _totDailyHours[1] += summaryHours;
                            _grandTotDailyHours[1] += summaryHours;
                            break;
                        case DayOfWeek.Wednesday:
                            _boundEmployeeHistory.WednesdayHours += summaryHours;
                            _totDailyHours[2] += summaryHours;
                            _grandTotDailyHours[2] += summaryHours;
                            break;
                        case DayOfWeek.Thursday:
                            _boundEmployeeHistory.ThursdayHours += summaryHours;
                            _totDailyHours[3] += summaryHours;
                            _grandTotDailyHours[3] += summaryHours;
                            break;
                        case DayOfWeek.Friday:
                            _boundEmployeeHistory.FridayHours += summaryHours;
                            _totDailyHours[4] += summaryHours;
                            _grandTotDailyHours[4] += summaryHours;
                            break;
                        case DayOfWeek.Saturday:
                            _boundEmployeeHistory.SaturdayHours += summaryHours;
                            _totDailyHours[5] += summaryHours;
                            _grandTotDailyHours[5] += summaryHours;
                            break;
                        case DayOfWeek.Sunday:
                            _boundEmployeeHistory.SundayHours += summaryHours;
                            _totDailyHours[6] += summaryHours;
                            _grandTotDailyHours[6] += summaryHours;
                            break;
                    }

                    totalHours += summaryHours;
                }
            }
            */

            //Monday
            _totDailyHours[0] += _boundEmployeeHistory.MondaySummary.TotalHoursWorked;
            _totShiftDailyHours[0] += _boundEmployeeHistory.MondaySummary.TotalHoursWorked;
            _grandTotDailyHours[0] += _boundEmployeeHistory.MondaySummary.TotalHoursWorked;
            //Tuesday
            _totDailyHours[1] += _boundEmployeeHistory.TuesdaySummary.TotalHoursWorked;
            _totShiftDailyHours[1] += _boundEmployeeHistory.TuesdaySummary.TotalHoursWorked;
            _grandTotDailyHours[1] += _boundEmployeeHistory.TuesdaySummary.TotalHoursWorked;
            //Wednesday
            _totDailyHours[2] += _boundEmployeeHistory.WednesdaySummary.TotalHoursWorked;
            _totShiftDailyHours[2] += _boundEmployeeHistory.WednesdaySummary.TotalHoursWorked;
            _grandTotDailyHours[2] += _boundEmployeeHistory.WednesdaySummary.TotalHoursWorked;
            //Thursday
            _totDailyHours[3] += _boundEmployeeHistory.ThursdaySummary.TotalHoursWorked;
            _totShiftDailyHours[3] += _boundEmployeeHistory.ThursdaySummary.TotalHoursWorked;
            _grandTotDailyHours[3] += _boundEmployeeHistory.ThursdaySummary.TotalHoursWorked;
            //Friday
            _totDailyHours[4] += _boundEmployeeHistory.FridaySummary.TotalHoursWorked;
            _totShiftDailyHours[4] += _boundEmployeeHistory.FridaySummary.TotalHoursWorked;
            _grandTotDailyHours[4] += _boundEmployeeHistory.FridaySummary.TotalHoursWorked;
            //Saturday
            _totDailyHours[5] += _boundEmployeeHistory.SaturdaySummary.TotalHoursWorked;
            _totShiftDailyHours[5] += _boundEmployeeHistory.SaturdaySummary.TotalHoursWorked;
            _grandTotDailyHours[5] += _boundEmployeeHistory.SaturdaySummary.TotalHoursWorked;
            //Sunday
            _totDailyHours[6] += _boundEmployeeHistory.SundaySummary.TotalHoursWorked;
            _totShiftDailyHours[6] += _boundEmployeeHistory.SundaySummary.TotalHoursWorked;
            _grandTotDailyHours[6] += _boundEmployeeHistory.SundaySummary.TotalHoursWorked;

            //_boundEmployeeHistory.TotalRegularHours =

            _totDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totShiftDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totDailyHours[8] += _boundEmployeeHistory.TotalOTHours;
            _totShiftDailyHours[8] += _boundEmployeeHistory.TotalOTHours;

            _reportHours += _boundEmployeeHistory.TotalRegularHours;

            _reportOTHours += _boundEmployeeHistory.TotalOTHours;

        }

        protected void displayHours(Label lbl, DailySummary dailySummary)
        {
            if (dailySummary.TotalHoursWorked > 16)
            {
                lbl.ForeColor = System.Drawing.Color.Red;
                lbl.BorderStyle = BorderStyle.Solid;
                lbl.BorderWidth = Unit.Pixel(1);
            }
            String temp = dailySummary.TotalHoursWorked.ToString("N2");
            lbl.Text = temp;
        }

        protected void displayHiddenHours(HiddenField txt, DailySummary dailySummary)
        {
            String temp = (dailySummary.TotalHoursWorked).ToString("N2");
            //txt.Value = "*" + temp + "*";
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.CommandName == "GenerateCSV")
            {
                createInvoice(sender, e);
            }
            if (ClientPrefs.DisplayInvoice || ClientPrefs.NotifyHoursReady)
            {
                if (btn.CommandName == "SubmitApproved")
                {
                    this.lnkExport.Visible = true;
                    this.lnkExportDetail.Visible = true;
                    _showApproved = true;
                    if (ClientPrefs.NotifyHoursReady)
                    {
                        //send an email
                        this.SendEMail();
                    }
                    this.lblHoursConfirmation.Visible = true;
                    //save the approval
                    HoursReport hoursReportInput = new HoursReport();
                    hoursReportInput.ClientID = _clientInfo.ClientID;
                    hoursReportInput.EndDateTime = this.WeekEnding;
                    HoursReportBL hoursReportBL = new HoursReportBL();
                    HoursReport savedReport = (HoursReport)Session["hoursreport"];
                    StringBuilder approvalXML = new StringBuilder();
                    approvalXML.Append("<approvalxml><employeepunches>");
                    foreach (EmployeeHistory history in savedReport.EmployeeHistoryCollection)
                    {
                        if (history.WorkSummaries.Count > 0)
                        {
                            foreach (EmployeeWorkSummary summary in history.WorkSummaries)
                            {
                                if (summary.CheckInEmployeePunchID > 0)
                                {
                                    //check in id
                                    approvalXML.Append("<employeepunch><ID>");
                                    approvalXML.Append(summary.CheckInEmployeePunchID.ToString());
                                    approvalXML.Append("</ID></employeepunch>");
                                }
                                if (summary.CheckOutEmployeePunchID > 0)
                                {
                                    //check out id
                                    approvalXML.Append("<employeepunch><ID>");
                                    approvalXML.Append(summary.CheckOutEmployeePunchID.ToString());
                                    approvalXML.Append("</ID></employeepunch>");
                                }
                            }
                        }
                    }
                    approvalXML.Append("</employeepunches></approvalxml>");
                    hoursReportInput.ApprovalXML = approvalXML.ToString();
                    hoursReportBL.ApproveClientHours(hoursReportInput, Context.User);
                }
                else
                {
                    this.lnkExport.Visible = false;
                    this.lnkExportDetail.Visible = false;
                    _showApproved = false;
                }
            }
            LoadHoursReport();
            this.pnlTotals.Visible = true;
        }

        private void SendEMail()
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                System.Configuration.Configuration rootWebConfig1 =
                                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
                System.Configuration.KeyValueConfigurationElement email =
                    rootWebConfig1.AppSettings.Settings["submitApprovedEmail"];
                System.Configuration.KeyValueConfigurationElement pwd =
                    rootWebConfig1.AppSettings.Settings["submitApprovedPwd"];
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential(email.Value, pwd.Value);
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mailClient.Host = "smtp.msistaff.com";
                mailClient.Port = 5190; //587
                message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress(email.Value);
                message.To.Add("approvedhours@msistaff.com");
                switch (ClientInfo.ClientID)
                {
                    case 30:
                        message.To.Add("list-ah-elitemanufacturing@msistaff.com");  //chris@emt333.com
                        break;
                    case 121:
                        message.To.Add("list-ah-wiseplastics@msistaff.com"); 
                        break;
                    case 186:
                        message.To.Add("list-ah-tutco@msistaff.com"); //DShire@Fastheat.com
                        break;
                    case 158:
                        message.To.Add("list-ah-pmall@msistaff.com"); //deborahw@pmall.com, mariah@pmall.com, billr@pmall.com
                        break;
                    case 166:
                        message.To.Add("list-ah-printmailingsolutions@msistaff.com"); //jprebis@algworldwide.com
                        break;
                    case 211:
                        message.To.Add("list-ah-continentalweb@msistaff.com"); //asanchez@continentalweb.com
                        break;
                }
                message.Subject = ClientInfo.ClientName + " Hours are approved";
                message.Body = "Hours have been approved for " + ClientInfo.ClientName + " for week ending " + this.WeekEnding.ToString("MM/dd/yyyy");
                message.IsBodyHtml = false;

                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }

        protected void createInvoice(object sender, EventArgs e)
        {
            Button btn = null;
            if (sender is Button)
                btn = (Button)sender;
            //System.Windows.Forms.MessageBox.Show(Session["hoursreport"].ToString());
            if (Session["hoursreport"] != null)
            {
                _hoursReport = (HoursReport)Session["hoursreport"];
                _clientPrefs = (ClientPreferences)Session["ClientPrefs"];
                //create the invoice from the approved hours
                Invoice invoice = new Invoice();
                invoice.ClientID = Convert.ToInt32(/*ddlMembers.SelectedValue*/clientLocation);//_clientInfo.ClientID;
                invoice.ClientApprovalId = int.Parse(this.hdnClientApprovalId.Value);
                invoice.WeekEndDate = DateTime.Parse(this.hdnWeekEndDate.Value);
                InvoiceDetail detail = null;
                InvoiceBL invoiceBL = new InvoiceBL();
                EmployeeWorkSummary summary = new EmployeeWorkSummary();
                foreach (EmployeeHistory empHist in _hoursReport.EmployeeHistoryCollection)
                {
                    if (empHist.ClientID != Convert.ToInt32(clientLocation))
                        continue;
                    if (empHist.WorkSummaries.Count > 0)
                    {
                        detail = new InvoiceDetail();
                        detail.BadgeNumber = empHist.TempNumber;
                        detail.JobCode = empHist.GetEmployeeJobCode();
                        detail.ClientRosterID = empHist.ClientRosterId;
                        detail.OTMultiplier = decimal.Parse(_clientInfo.OTMultiplier.ToString());
                        detail.RegularMultiplier = decimal.Parse(_clientInfo.Multiplier.ToString());
                        detail.PayRate = empHist.GetEmployeePayRate();
                        detail.JobCode = empHist.GetEmployeeJobCode();
                        detail.TotalRegularHours = empHist.TotalRegularHours;
                        detail.TotalOTHours = empHist.TotalOTHours;
                        summary = (EmployeeWorkSummary)empHist.WorkSummaries[0];
                        detail.DepartmentInfo = summary.DepartmentInfo;
                        detail.ShiftTypeInfo = summary.ShiftTypeInfo;
                        detail.ShiftInfo = summary.ShiftInfo;
                        invoice.DetailInfo.Add(detail);
                    }
                }

                InvoiceReturn ret = invoiceBL.CreateInvoice(invoice, Context.User);
                if (btn == null || btn.CommandName != "GenerateCSV")
                {
                    Session["hoursreport"] = null;
                    if (ret.IsSuccess)
                    {
                        Context.Items.Add("weekend", invoice.WeekEndDate.ToString("MM/dd/yyyy"));
                        //redirect to show the invoice
                        Server.Transfer("~/auth/InvoiceSummary.aspx", false);
                    }
                }
                else
                {
                    createCSV(sender, e);
                }
            }
        }

        Invoice invoice = new Invoice();

        protected void lnkCreateInvoice_Click(object sender, EventArgs e)
        {
            createInvoice(sender, e);
        }

        private Invoice getInvoice()
        {
            InvoiceBL invoiceBL = new InvoiceBL();

            return invoiceBL.GetInvoice(invoice, Context.User);
            //_loadedInvoice = true;
        }

        protected void createCSV(object sender, EventArgs e)
        {
            invoice.ClientID = ClientInfo.ClientID;
            invoice.WeekEndDate = WeekEnding;//DateTime.Parse(this.WeekEnding);
            invoice = this.getInvoice();

            StringBuilder csvInfo = new StringBuilder();
            bool bonus = false;
            if (_clientInfo.ClientID != 178 && _clientInfo.ClientID != 256 &&
                _clientInfo.ClientID != 280 && _clientInfo.ClientID != 281 &&
                _clientInfo.ClientID != 292 && _clientInfo.ClientID != 293 /*|| true*/)
                bonus = false;
            else
                bonus = true;
            if (bonus)
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID,BONUS, BONUS_BILL, PAYCODE");
            else
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID");
            foreach (InvoiceDetail detail in invoice.DetailInfo)
            {
                int r = 1;
                if (bonus)
                    r = 2;
                for (int i = 0; i < r; i++)
                {
                    if (i == 1 && detail.Bonus == 0)
                        continue;
                    csvInfo.Append(Environment.NewLine);
                    //aident
                    csvInfo.Append(detail.BadgeNumber.Substring(2));
                    csvInfo.Append(",");
                    //ssn
                    csvInfo.Append("\"");
                    csvInfo.Append("");
                    csvInfo.Append("\",");
                    //lastname
                    csvInfo.Append("\"");
                    detail.LastName = detail.LastName.Trim().ToUpper().Replace('\t', ' ');
                    while( detail.LastName.Contains("  ") )
                    {
                        detail.LastName = detail.LastName.Replace("  ", " ");
                    }
                    csvInfo.Append(detail.LastName.Substring(0,1));
                    csvInfo.Append("\",");
                    //firstname
                    csvInfo.Append("\"");
                    detail.FirstName = detail.FirstName.Trim().ToUpper().Replace('\t', ' ');
                    while (detail.FirstName.Contains("  "))
                    {
                        detail.FirstName = detail.FirstName.Replace("  ", " ");
                    }
                    csvInfo.Append(detail.FirstName.Substring(0,1));
                    csvInfo.Append("\",");
                    if (i == 0)
                    {
                        //regular hours
                        csvInfo.Append(detail.TotalRegularHours.ToString("0.00"));
                        csvInfo.Append(",");
                        //ot hours
                        csvInfo.Append(detail.TotalOTHours.ToString("0.00"));
                        csvInfo.Append(",");
                    }
                    else
                    {
                        //0 for regular and OT hours
                        csvInfo.Append("0.00,0.00,");
                    }
                    //pay rate
                    csvInfo.Append(detail.PayRate.ToString("0.00"));
                    csvInfo.Append(",");
                    //bill rate
                    decimal tempBill = Math.Round(detail.PayRate * detail.RegularMultiplier, 2, MidpointRounding.AwayFromZero);
                    csvInfo.Append(tempBill.ToString("0.00"));
                    csvInfo.Append(",");
                    //retro reg
                    csvInfo.Append("0.00");
                    csvInfo.Append(",");
                    //retro ot
                    csvInfo.Append("0.00");
                    csvInfo.Append(",");
                    //temp works deparment id
                    csvInfo.Append("\"");
                    csvInfo.Append(detail.ShiftInfo.TempWorksMappingId.ToString());
                    csvInfo.Append("\"");
                    //bonus pay
                    if (bonus)
                    {
                        if (i == 1)
                        {
                            csvInfo.Append(",");
                            csvInfo.Append(detail.Bonus.ToString("0.00"));
                            csvInfo.Append(",");
                            double bon = ((double)(detail.Bonus)) * 1.1;
                            csvInfo.Append(bon.ToString("0.00") + ",\"BONUS\"");
                        }
                        else
                        {
                            csvInfo.Append(",0.00,0.00,\"REG\"");
                        }
                    }
                }
            }

            string fileName = "TWImport_" + _clientInfo.ClientName.Replace(" ", "_").ToUpper().Replace(",","_") + "_" + invoice.WeekEndDate.ToString("MMddyyyy") + ".csv";
            string fullPath = Server.MapPath("~/invfiles/");

            using (StreamWriter sw = new StreamWriter(fullPath + fileName))
            {
                // Add some text to the file.
                sw.Write(csvInfo.ToString());
            }

            //create the file
            System.IO.FileStream fs = null;
            fs = System.IO.File.Open(fullPath + fileName, System.IO.FileMode.Open);
            byte[] btFile = new byte[fs.Length];
            fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(btFile);
            Response.End();
        }
    }
}