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
using MSIToolkit.Logging;
//style="mso-number-format:\#\,\#\#0\.00; text-align:right"  
namespace MSI.Web.Controls
{
    public partial class MSINetHoursReport : BaseMSINetControl
    {
        public enum ExportType
        {
            None,
            Excel,
            Print, 
            PeekABoo,
            PeekABooTop,
            DetailFlat
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
        private Boolean departmentInvalid = false;
        private Boolean _showStartDate = true;
        

        private String[] weekDay = { "Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat" };
        public Boolean payRateJobCodeEligible = true;

        public int[] AmericanLithoShifts = { 4, 7, 79, 264, 366, 376, 440, 442, 443, 441, 265, 80, 8, 5, 6, 210, 263, 266, 367 };
        public int[] BerlinShifts = { 1011, 1066, 1067, 978, 1012, 979, 984, 1013, 1068, 1069 };

        public int[] CoMailShifts = { 1, 2, 3, 314, 315, 325, 326, 316, 317, 1037, 1038, 1039, 1091, 1092, 1093, 1206, 1207, 1208, 1329, 1330, 1442, 1443, 1444, 1445, 1058, 1059, 1060, 1457, 1458, 1459, 1696, 1697 };
        public int[] CoPalShifts = { 318, 319, 327 };
        public bool sortByDept = true;
        public bool showExact = false;
        public bool showSupervisors = false;
        public bool showAllEmployees = false;
        public int daysWorked = 0;

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

        PerformanceLogger log = new PerformanceLogger("Hours Report");
        //private static readonly ILog Logger = LogManager.GetLogger("Hours Report");

        public List<EmployeeHistoryWeekSupervisor> GetEmployeeSupervisorList()
        {
            List<EmployeeHistoryWeekSupervisor> al = new List<EmployeeHistoryWeekSupervisor>();
            return al;
        }

        public void LoadHoursReport()
        {
            log.Info("LoadHoursReport", "Start, Client: " + ClientInfo.ClientName);

            if (_exportType == ExportType.None)
            {                
                pnlJSCode.Visible = true;
                if (ClientInfo.ClientID == 2)
                {
                    if (rbtnDept.Checked)
                        ClientInfo.IgnoreShiftList = new List<int>(AmericanLithoShifts);
                    else if (rbtnDept2.Checked)
                        ClientInfo.IgnoreShiftList = new List<int>(BerlinShifts);
                    else
                        ClientInfo.IgnoreShiftList = null;
                }
                else if (ClientInfo.ClientID == 166)
                {
                    if (rbtnDept.Checked)
                        ClientInfo.IgnoreShiftList = new List<int>(CoMailShifts);
                    else if (rbtnDept2.Checked)
                        ClientInfo.IgnoreShiftList = new List<int>(CoPalShifts);
                    else
                        ClientInfo.IgnoreShiftList = null;
                }
            }
           
            Session["ClientInfo"] = ClientInfo; 
            _firstPunch = DATE_NOT_SET;
            _lastPunch = DATE_NOT_SET;
            _hoursLoaded = true;
            HoursReport hoursReportInput = new HoursReport();
            hoursReportInput.clientPrefs = _clientPrefs;
            hoursReportInput.ClientID = _clientInfo.ClientID;
            hoursReportInput.RosterEmployeeFlag = true;
            SetWeekEnding();
            hoursReportInput.EndDateTime = WeekEnding;
            TimeSpan ts;
            if (_clientId < 325 || _clientId > 327 || WeekEnding < new DateTime(2016, 7, 27))
            {
                ts = new TimeSpan(6, 0, 0, 0);
            }
            else 
            {
                ts = new TimeSpan(6, 2, 0, 0);
            }
            hoursReportInput.StartDateTime = hoursReportInput.EndDateTime.Subtract(ts);
            ts = new TimeSpan(0, 0, 0);
            //hoursReportInput.StartDateTime = hoursReportInput.StartDateTime.Date + ts;
            DateTime dt = WeekEnding;
            hoursReportInput.EndDateTime = DateTime.Parse(dt.ToString("MM/dd/yyyy") + " 23:59:59");
            hoursReportInput.DaysWorked = 0;
            hoursReportInput.DaysWorked = Convert.ToInt32(selectEmployees.Value);
            if (_clientPrefs.DisplayStartDate)
            {
                _showStartDate = true;
                selectEmployees.Visible = true;
            }
            else
            {
                _showStartDate = false;
                selectEmployees.Visible = false;
            }
            if (_exportType == ExportType.None)
            {
                if (rbtnDepts.Checked == true)
                    sortByDept = true;
                else
                    sortByDept = false;
            }
            hoursReportInput.UseExactTimes = showExact;
            if (_clientInfo.ClientID == 381)
            {
                hoursReportInput.UseExactTimes = _clientPrefs.UseExactTimes;
            }
            hoursReportInput.ShowAllEmployees = showAllEmployees;

            string badgeNum = "";
            string userId = Context.User.Identity.Name.ToLower();
            //log.Info("LoadHoursReport", "Creating HoursReportBL");
            HoursReportBL hoursReportBL = new HoursReportBL();
            //log.Info("LoadHoursReport", "Created HoursReportBL - hoursReportBL.GetHoursReport");
            _hoursReport = hoursReportBL.GetHoursReport(hoursReportInput, userId, badgeNum, sortByDept, log);
            //log.Info("LoadHoursReport", "returned from hoursReportBL.GetHoursReport");
            userId = userId.ToLower();
            Boolean savedDisplayPayRate = ClientPrefs.DisplayPayRate;
            /* creative works and creative works in-house */
            if (userId.Equals("garayr") || userId.Equals("hardinb") || userId.Equals("nievesl")
                || userId.Equals("roldanj") || userId.Equals("winterfeldr") || userId.Equals("castillogu") ||
                (_clientId == 399 && !userId.Equals("shelly") && !userId.Equals("itdept") && !userId.Equals("virginia")) || 
                Context.User.IsInRole("NoViewPayRates") )
            {
                ClientPrefs.DisplayPayRate = false;
                payRateJobCodeEligible = false;
            }
            if( _clientId == 113 )
            {
                ClientPrefs.DisplayPayRate = false;
                if (userId.Equals("verac") || userId.Equals("itdept") || userId.Equals("imolina") || userId.Equals("ferrerm") || userId.Equals("floreses"))
                {
                    ClientPrefs.DisplayPayRate = true;
                    payRateJobCodeEligible = true;
                }
            }
            if( _clientId == 324 || _clientId == 329 )
            {
                if( userId.Equals("ortize") || userId.Equals("osoriof") || userId.Equals("itdept") ||
                    userId.Equals("sanchezm") || userId.Equals("tellov") )
                {
                    ClientPrefs.DisplayPayRate = true;
                    payRateJobCodeEligible = true;
                }
                else
                {
                    ClientPrefs.DisplayPayRate = false;
                    payRateJobCodeEligible = false;
                }
            }
            if ( userId.Equals("itdept") )
            {
                ClientPrefs.DisplayPayRate = true;
                payRateJobCodeEligible = true;
            }
            if (((_clientId == 256 || _clientId == 178 || _clientId == 280 || _clientId == 281 || _clientId == 292 || _clientId == 293) && 
                ( userId.Equals("dougm") ||
                userId.Equals("vanessag") || userId.Equals("scottb") ||
                userId.Equals("anitas") || userId.Equals("chrisp") ||
                userId.Equals("iqbalk") || userId.Equals("dottieg") ||
                userId.Equals("hillmannt") || userId.Equals("drodriguez") ||
                userId.Equals("efarfan")) || userId.Equals("ltimmons") ||
                userId.Equals("jbandelow") || userId.Equals("abbasyj") ||
                userId.Equals("gurneyd") || userId.Equals("ballardc") ||
                userId.Equals("balderasc") || userId.Equals("lopezj") ||
                userId.Equals("zavalar") || userId.Equals("pilladog") ||
                userId.Equals("perezr") || userId.Equals("garciamar") ||
                userId.Equals("sanchezgi") || userId.Equals("arnoldj") ||
                userId.Equals("vegaa")) || userId.Equals("nievesl") || userId.Equals("winterfeldr"))
                payRateJobCodeEligible = false;
            if (userId.ToLower().Equals("seent") || userId.ToLower().Equals("feckj") || userId.ToLower().Equals("kalejtaa")
                || userId.ToLower().Equals("coccimiglioa") || userId.Equals("rodriguezer") || userId.ToLower().Equals("garciac")
                || userId.ToLower().Equals("nolest") || userId.ToLower().Equals("marios"))
                payRateJobCodeEligible = false;
            /* tangent technologies only shired, moakes, medwards, and itdept can see job codes and pay rates */
            if ((_clientId == 258) && !(userId.Equals("shired")
                || userId.Equals("moakes") || userId.Equals("mchavez")
                || userId.Equals("itdept") || userId.Equals("glaserm") || userId.Equals("lisa")
                || userId.Equals("imolina") || userId.Equals("ferrerm") || userId.Equals("garciali")
                || userId.Equals("rojasj")))
                payRateJobCodeEligible = false;

            if (_clientId == 226 && userId.Equals("huertam"))
                payRateJobCodeEligible = false;

            if (userId.Equals("mchavez") || userId.Equals("julio") || userId.Equals("itdept") || userId.Equals("moakes")
                || userId.Equals("vfabela") || userId.Equals("vanessag") || userId.Equals("imolina") || userId.Equals("whitel") || userId.Equals("riveras")
                || userId.Equals("badanis") || userId.Equals("scharpfk") || userId.Equals("maria") || userId.Equals("verac") || userId.Equals("floreses") 
                || userId.Equals("ferrerm") 
                )
            {
                if (_clientPrefs.DisplayBonuses)
                    this.hdnUpdBonuses.Value = "true";
                else
                    this.hdnUpdBonuses.Value = "false";
            }

            if (Context.User.IsInRole("PayRates") ||
                
                userId.ToUpper().Equals("IMOLINA") || userId.ToUpper().Equals("TCAMPBELL") || userId.ToUpper().Equals("MCHAVEZ") ||
                userId.ToUpper().Equals("GLASERM") || userId.ToUpper().Equals("MOAKES") || userId.ToUpper().Equals("ITDEPT") || userId.ToUpper().Equals("JULIO") ||
                userId.ToUpper().Equals("LISA") || userId.ToUpper().Equals("RAFA") || userId.ToUpper().Equals("MARIA") || 
                userId.ToUpper().Equals("BADANIS") || userId.ToUpper().Equals("MAGDALENOY") || userId.ToUpper().Equals("WHEELING") || userId.ToUpper().Equals("SZUNIGA") ||
                userId.ToUpper().Equals("CASTILLOM") || userId.ToUpper().Equals("RIVERAS") || userId.ToUpper().Equals("MENDOZAC") || userId.ToUpper().Equals("FERRERM") ||
                userId.ToUpper().Equals("HERRERAS") || userId.ToUpper().Equals("FERNANDO") || userId.ToUpper().Equals("MORENOM") || userId.ToUpper().Equals("GARCIALI") ||
                userId.ToUpper().Equals("CASTILLOGU"))
            {
                this.hdnUpdPayRates.Value = "true";
            }
            else
            {
                this.hdnUpdPayRates.Value = "false";
            }
            this.hdnApproveList.Value = "";

            //log.Info("LoadHoursReport", "setting DataSource");

            if(chkboxSupervisor.Checked == false && showSupervisors == false)
            {
                this.rptrHoursReport.DataSource = _hoursReport.EmployeeHistoryCollection;
                this.rptrHoursReport.DataBind();
                this.rptrHoursReport.Visible = true;
                this.rptrSupervisorList.Visible = false;
            }
            else
            {
                this.rptrSupervisorList.DataSource = _hoursReport.EmployeeHistoryCollection;
                this.rptrSupervisorList.DataBind();
                this.rptrHoursReport.Visible = false;
                this.rptrSupervisorList.Visible = true;
                pnlTotals.Visible = false;
            }
            //log.Info("LoadHoursReport", "Finished with DataBind");

            if (chkboxUnassigned.Checked && _exportType == ExportType.None)
            {
                pnlUnassignedPunches.Visible = true;
                TicketTrackerException ticketTrackerInput = new TicketTrackerException();
                TicketTrackerException _trackerResult;
                ticketTrackerInput.ClientID = _clientInfo.ClientID;
                ticketTrackerInput.PeriodStartDateTime = DateTime.Parse(hoursReportInput.StartDateTime.ToString("MM/dd/yyyy 00:00:00"));
                ticketTrackerInput.PeriodEndDateTime = DateTime.Parse(hoursReportInput.EndDateTime.ToString("MM/dd/yyyy 23:59:59"));
                TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
                //get the exception tracking
                _trackerResult = ticketTrackerBL.GetTicketTrackingExceptions(ticketTrackerInput, Context.User, null);
                this.rptrTicketTrackerException.DataSource = _trackerResult.Employees;
                this.rptrTicketTrackerException.DataBind();
            }
            else
            {
                pnlUnassignedPunches.Visible = false;
            }
            ClientPrefs.DisplayPayRate = savedDisplayPayRate;
            if( Context.User.IsInRole("HoursReportHTML") )
            {
                btnExportHTML.Attributes.Remove("hidden");
            }
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
            if (WeekEnding == null || WeekEnding == new DateTime(1, 1, 1))
                WeekEnding = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString());
        }

        public string GetSelectedDate()
        {
            return WeekEnding.ToString("MM-dd-yyyy");  //Server.UrlEncode(this.WeekEnding.ToString("MM-dd-yyyy"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int count = this.rptrHoursReport.Items.Count;
            _hoursLoaded = false;
            this.pnlTotals.Visible = false;

            this.lnkCreateInvoice.Visible = false;
            Session["clientapprovalid"] = null;

            if ( ClientPrefs.DisplayStartDate )
            {
                _showStartDate = true;
                selectEmployees.Visible = true;
            }
            else
            {
                _showStartDate = false;
                selectEmployees.Visible = false;
            }

            if (!this.IsPostBack)
            {
                if (ClientInfo.ClientID == 121)
                {
                    this.lnkExport.Visible = false;
                    this.lnkExportDetail.Visible = false;
                }
                else
                {
                    this.lnkExport.Visible = true;
                    this.lnkExportDetail.Visible = true;
                }
                if (ClientInfo.ClientID == 302)
                {
                    chkboxSupervisor.Visible = true;
                    lblSupervisor.Visible = true;
                    this.lnkExportPeekABoo.Visible = true;
                    this.peekABoo.Visible = true;
                }
                else
                {
                    this.lnkExportPeekABoo.Visible = false;
                }
                /* init the drop down list */
                DateTime dt = _helper.GetCSTCurrentDateTime();
                if( ClientPrefs.DisplayWeeklyReportsSundayToSaturday)
                {
                    while (dt.DayOfWeek != DayOfWeek.Saturday)
                        dt = dt.AddDays(1);
                }
                else if (ClientPrefs.DisplayWeeklyReportsWednesdayToTuesday)
                {
                    while (dt.DayOfWeek != DayOfWeek.Tuesday)
                        dt = dt.AddDays(1);
                }
                else if (ClientPrefs.DisplayWeeklyReportsSaturdayToFriday)
                {
                    while (dt.DayOfWeek != DayOfWeek.Friday)
                        dt = dt.AddDays(1);
                }
                else if (ClientPrefs.DisplayWeeklyReportsFridayToThursday)
                {
                    while (dt.DayOfWeek != DayOfWeek.Thursday)
                        dt = dt.AddDays(1);
                }
                else
                {
                    while (dt.DayOfWeek != DayOfWeek.Sunday)
                        dt = dt.AddDays(1);
                }
                DateTime fDt = dt.AddDays(7);
                for (int i = 0; i < 256; i++)
                {
                    this.txtCalendar.Items.Add(dt.ToString("MM/dd/yyyy"));
                    dt = dt.AddDays(-7);
                }
                this.txtCalendar.Items.Add(fDt.ToString("MM/dd/yyyy"));
            }
            else
            {
                //log.Info("Page_Load", "Start - IsPostBack");
            }
            if (this.WeekEnding == null || this.WeekEnding == new DateTime(1, 1, 1).Date)
            {
                //get the week ending date - initialization
                if (txtCalendar.Items.Count == 0)
                    this.WeekEnding = DateTime.Parse(_helper.GetCSTCurrentWeekEndingDate().ToString("MM/dd/yyyy") + " 00:00:00");
                else
                {
                    this.WeekEnding = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString());
                    this.weekEndDate.Value = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString()).AddDays(1).ToString("MM/dd/yyyy 14:00");
                    this.weekStartDate.Value = DateTime.Parse(txtCalendar.Items[txtCalendar.SelectedIndex].ToString()).AddDays(-7).ToString("MM/dd/yyyy");
                }
            }

            if (_exportType != ExportType.None)
            {
                this.EnableViewState = false;
                string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);
                string exportDetail = Server.UrlDecode((string)Request.QueryString["detail"]);
                sortByDept = Server.UrlDecode((string)Request.QueryString["sortOrder"]).ToUpper().Equals("DEPT");
                showExact = Server.UrlDecode((string)Request.QueryString["exact"]).ToUpper().Equals("TRUE");
                selectEmployees.Value = Server.UrlDecode((string)Request.QueryString["daysWorked"]);
                if( exportDetail != "1" && exportDetail != "2" )
                    showSupervisors = Server.UrlDecode((string)Request.QueryString["showSupervisors"]).ToUpper().Equals("TRUE");
                if (exportDetail == "1")
                {
                    _exportDetail = true;
                }
                else if (exportDetail == "2")
                {
                    _exportType = ExportType.PeekABoo;
                }
                else if (exportDetail == "3")
                {
                    _exportType = ExportType.PeekABooTop;
                }
                else if( exportDetail == "4")
                {
                    _exportType = ExportType.DetailFlat;
                }

                WeekEnding = DateTime.Parse(dateTime);
                LoadHoursReport();
                pnlHeader.Visible = false;
                mnuHoursReport.Visible = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string userId = Context.User.Identity.Name.ToLower();
            this.btnSubmitApproved.Enabled = false;
            this.lnkExport.Visible = false;
            this.lnkExportDetail.Visible = false;
            this.lnkExportPeekABoo.Visible = false;

            this.lnkCreateInvoice.Visible = false;
            this.pnlApprovalInfo.Visible = false;
            this.btnCSV.Visible = false;
            this.btnOffice.Visible = false;
            this.peekABoo.Visible = false;
            if (this.pnlTotals.Visible)
            {
                this.lblRegularHrs.Text = _reportHours.ToString("N2");
                this.lblOTHrs.Text = _reportOTHours.ToString("N2");
            }
            string sortOrder = "shift";
            if( rbtnDepts.Checked == true )
                sortOrder = "dept";
            string daysWorked = "&daysWorked=" + selectEmployees.Value;
            string supervisorList = "&showSupervisors=" + chkboxSupervisor.Checked;
            this.lnkExport.NavigateUrl = "~/auth/HoursReportExcel.aspx?detail=0&date=" + this.GetSelectedDate() + "&sortOrder=" + sortOrder 
                + "&exact=false" + daysWorked + supervisorList;
            this.lnkExportDetail.NavigateUrl = "~/auth/HoursReportExcel.aspx?detail=1&date=" + this.GetSelectedDate() + "&sortOrder=" + sortOrder
                + "&exact=false" + daysWorked;
            this.lnkExportPeekABoo.NavigateUrl = "~/auth/HoursReportExcel.aspx?detail=2&date=" + this.GetSelectedDate() + "&sortOrder=" + sortOrder
                + "&exact=false" + daysWorked;
            Boolean codePermissionOverride = true;
            String user = Context.User.Identity.Name.ToLower();
            if ((user.Equals("dalep") || user.Equals("ericks") || user.Equals("julioc")
                || user.Equals("jiml") || user.Equals("josem") || user.Equals("manuelp")
                 || user.Equals("markw") || user.Equals("teresab") || user.Equals("shaund"))
                   && _clientId == 7)
                codePermissionOverride = false;
            if( _clientId == 258 && !user.Equals("shired") && !user.Equals("villarreald"))
                codePermissionOverride = false;
            if( user.Equals("pereza") || user.Equals("guenthers") || user.Equals("lopezr") || user.Equals("dipponl") || user.Equals("chezewskib") ||
                user.Equals("karolewiczd") || user.Equals("elliotr") || user.Equals("espinozam") || user.Equals("almanzao") || user.Equals("melgozar") )
                _allowElectronicApproval = false;
            if (_clientId == 287 && !user.Equals("mendozae") && !user.Equals("wagnera") && !user.Equals("itdept") && !user.Equals("pachterk") && !user.Equals("jamesl") && !user.Equals("mccarthyc"))
                _allowElectronicApproval = false;
            if (_clientId == 245 && !user.Equals("torresa") && !user.Equals("luvins"))
                _allowElectronicApproval = false;
            if (_clientId == 302 && !user.Equals("itdept") && !user.Equals("magdalenoy") && !user.Equals("ferrerm") && !user.Equals("garciali") && !user.Equals("quichimbol") && !user.Equals("mchavez") && !user.Equals("lisa") && !user.Equals("sanchezm"))
                _allowElectronicApproval = false;

            if (user.Equals("medwards") || user.Equals("mendozac") || user.Equals("mchavez") || user.Equals("itdept") ||
                user.Equals("imolina") || user.Equals("glaserm"))
                codePermissionOverride = true;

            if(_hoursLoaded)
            {
                if ( Context.User.IsInRole("GenerateCSV") || userId.Equals("moakes") || userId.Equals("mchavez") || userId.Equals("imolina") ||
                    userId.Equals("itdept") || userId.Equals("mendozac") || userId.Equals("glaserm") || userId.Equals("lisa") ||
                    userId.Equals("castillom") || userId.Equals("magdalenoy") || userId.Equals("ferrerm") || userId.Equals("garciali") || userId.Equals("quichimbol"))
                {
                    this.btnCSV.Visible = true;
                    this.btnOffice.Visible = true;
                }
                if ((ClientPrefs.DisplayInvoice || ClientPrefs.NotifyHoursReady) && codePermissionOverride)
                {
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
                            if (this._clientInfo.ClientID == 302)
                            {
                                this.lnkExportPeekABoo.Visible = true;
                                this.peekABoo.Visible = true;
                            }
                        }
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
                        if (user.Equals("magdalenoy") || user.Equals("ferrerm") || user.Equals("garciali") || user.Equals("quichimbol") || user.Equals("mchavez") || 
                            user.Equals("moakes") || user.Equals("itdept") || user.Equals("imolina") ||
                            user.Equals("lisa") || user.Equals("jpaa") || user.Equals("castillom") || user.Equals("sanchezm") || user.Equals("mendozac") 
                            || Context.User.IsInRole("GenerateInvoice"))
                        {
                            this.lnkCreateInvoice.Visible = true;
                        }
                        else
                        {
                            this.lnkCreateInvoice.Visible = false;
                        }
                    }
                    if( this.Context.User.IsInRole("UnapproveHours"))
                    {
                        this.btnSubmitApproved.Enabled = true;
                        this.btnSubmitApproved.Text = "UnApprove Hours";
                        this.btnSubmitApproved.CommandName = "UnSubmitApproved";
                        this.btnSubmitApproved.BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        this.btnSubmitApproved.Enabled = false;
                    }
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
            if (e.CommandName == "approve___d")
            {
                CheckBox chkApprove = null;
                string employeePunchList = string.Empty;
                HiddenField hdnEmployeePunchList = null;
                string punchesStr = "";
                foreach (RepeaterItem item in rptrHoursReport.Items)
                {
                    //chkApprove = (CheckBox)item.FindControl("chkBoxApprove");
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
                    hrApproval.ClientID = _clientId;

                    HoursReportBL hoursReportBL = new HoursReportBL();
                    hoursReportBL.ApprovePunchRange(hrApproval, Context.User);
                }
            }
            LoadHoursReport();
        }

        protected void rptrSupervisorList_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DateTime dt = _hoursReport.EndDateTime.AddDays(-6);
                ((HtmlTableCell)e.Item.FindControl("hdrOne")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime.AddDays(-5);
                ((HtmlTableCell)e.Item.FindControl("hdrTwo")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime.AddDays(-4);
                ((HtmlTableCell)e.Item.FindControl("hdrThree")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime.AddDays(-3);
                ((HtmlTableCell)e.Item.FindControl("hdrFour")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime.AddDays(-2);
                ((HtmlTableCell)e.Item.FindControl("hdrFive")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime.AddDays(-1);
                ((HtmlTableCell)e.Item.FindControl("hdrSix")).InnerText = dt.Month + "/" + dt.Day;
                dt = _hoursReport.EndDateTime;
                ((HtmlTableCell)e.Item.FindControl("hdrSeven")).InnerText = dt.Month + "/" + dt.Day;
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundEmployeeIdx++;
                _boundEmployeeHistory = (EmployeeHistory)e.Item.DataItem;
                ((HtmlTableCell)e.Item.FindControl("cellEmpCount")).InnerText = Convert.ToString(_boundEmployeeIdx);
                ((HtmlTableCell)e.Item.FindControl("cellBadge")).InnerText = Convert.ToString(_boundEmployeeHistory.TempNumber);
                ((HtmlTableCell)e.Item.FindControl("cellName")).InnerText = Convert.ToString(_boundEmployeeHistory.LastName + ", " + _boundEmployeeHistory.FirstName);
                ((HtmlTableCell)e.Item.FindControl("cellCostCenter")).InnerText = Convert.ToString(_boundEmployeeHistory.CostCenter);
                ((HtmlTableCell)e.Item.FindControl("cellDept")).InnerText = ((EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0]).DepartmentInfo.DepartmentName;
                ((HtmlTableCell)e.Item.FindControl("cellShift")).InnerText = ((EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0]).ShiftTypeInfo.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellPayRate")).InnerText = (_boundEmployeeHistory.PayRate).ToString();
                ((HtmlTableCell)e.Item.FindControl("cellSupervisor")).InnerText = (_boundEmployeeHistory.Supervisor).ToString();
                ((HtmlTableCell)e.Item.FindControl("cellOne")).InnerText = _boundEmployeeHistory.MondayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellTwo")).InnerText = _boundEmployeeHistory.TuesdayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellThree")).InnerText = _boundEmployeeHistory.WednesdayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellFour")).InnerText = _boundEmployeeHistory.ThursdayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellFive")).InnerText = _boundEmployeeHistory.FridayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellSix")).InnerText = _boundEmployeeHistory.SaturdayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellSeven")).InnerText = _boundEmployeeHistory.SundayHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellReg")).InnerText = _boundEmployeeHistory.TotalRegularHours.ToString();
                ((HtmlTableCell)e.Item.FindControl("cellOT")).InnerText = _boundEmployeeHistory.TotalOTHours.ToString();
            }
        }
        protected void rptrHoursReport_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (ClientPrefs.ApproveHours && _exportType == ExportType.None && (_hoursReport.ApprovalDateTime == null || _hoursReport.ApprovalDateTime < new DateTime(2010, 01, 01)) )
                {
                    /* create a new table data item */

                    /*HtmlTableCell tc = new HtmlTableCell();
                    tc.BgColor = "#003776";
                    Button btnAppr = (Button)e.Item.FindControl("btnApprove");
                    btnAppr.Visible = true;
                    btnAppr.CommandName = "approve";
                    tc.Controls.Add(btnAppr);
                    */
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdApproveHeader");
                    td.Visible = true;
                    //PlaceHolder ph = ((PlaceHolder)e.Item.FindControl("tdApproveHeaderPH"));
                    //ph.Controls.Add(tc);
                }
                ((HtmlTableCell)e.Item.FindControl("tdTotalHours")).Visible = false;

                if( ClientInfo.ClientID == 113 && _exportType == ExportType.Excel )
                {
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("hdrDispatch");
                    if (td != null)
                        td.Visible = true;
                }
                if (ClientInfo.ClientID == 325 && _exportType == ExportType.Excel)
                {
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("tdClientNum");
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                }
                if (( ClientInfo.ClientID == 78 || ClientInfo.ClientID == 383) && _exportType == ExportType.Excel)
                {
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("hdrDept");//.Visible = false;
                    if (td != null)
                        td.Visible = true;
                    td = (HtmlTableCell)e.Item.FindControl("hdrShift");//.Visible = false;
                    if (td != null)
                        td.Visible = true;
                    td = (HtmlTableCell)e.Item.FindControl("hdrLoc");//.Visible = false;
                    if (td != null)
                        td.Visible = true;

                    td = (HtmlTableCell)e.Item.FindControl("tdApprovedBy");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("exlShift");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("exlDept");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("exlApproved");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("exlLoc");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                }

                if (this.ClientPrefs.DisplayBonuses == true && (this.Context.User.Identity.Name.ToLower().Equals("itdept")
                        || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                        || this.Context.User.Identity.Name.ToUpper().Equals("BADANIS")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VERAC")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FERRERM")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FLORESES")
                        || this.Context.User.Identity.Name.ToUpper().Equals("SCHARPF")
                        || this.Context.User.Identity.Name.ToUpper().Equals("IMOLINA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VANESSAG") 
                        || this.Context.User.Identity.Name.ToUpper().Equals("WHITEL")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MARIA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("RIVERAS")))
                {
                    ((HtmlTableCell)e.Item.FindControl("tdBonusPay")).Visible = true;
                }
                else
                    ((HtmlTableCell)e.Item.FindControl("tdBonusPay")).Visible = false;
            }
            btnApproveEnabled = true;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundEmployeeIdx++;
                _boundEmployeeHistory = (EmployeeHistory)e.Item.DataItem;
                if (_boundEmployeeIdx == 1)
                    this.hdnUpdPunches.Value = _boundEmployeeHistory.Modify.ToString();
                if (_clientId == 302)
                {
                    if (_boundEmployeeHistory.TotalHoursAllDepts > 55 || _boundEmployeeHistory.TotalHours > 50)
                    {
                        HtmlTableRow r = (HtmlTableRow)e.Item.FindControl("emp");
                        if (r != null)
                        {
                            r.Attributes.Remove("Class");
                            r.Attributes.Add("Class", "DlyHrsOT2");
                        }
                    }
                    else if (_boundEmployeeHistory.TotalHoursAllDepts >= 32 || _boundEmployeeHistory.TotalHours >= 32)
                    {
                        HtmlTableRow r = (HtmlTableRow)e.Item.FindControl("emp");
                        if (r != null)
                        {
                            r.Attributes.Remove("Class");
                            r.Attributes.Add("Class", "DlyHrsOT");
                        }
                    }
                }
                if ( ClientInfo.ClientID == 383 && _exportType == ExportType.Excel)
                { 
                    HtmlTableCell pnl = (HtmlTableCell)e.Item.FindControl(/*"pnlCheckBoxes"*/ "tdApproveHours");
                    pnl.Visible = false;
                    HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("rowLoc");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                        if (_boundEmployeeHistory.LocationId == 80)  /* ugh...FIX THIS! */
                            td.InnerText = "Tonne Rd.";
                        else if (_boundEmployeeHistory.LocationId == 253)
                            td.InnerText = "Estes Ave.";
                        else if (_boundEmployeeHistory.LocationId == 371)
                            td.InnerText = "Elk Grove";
                    }
                    td = (HtmlTableCell)e.Item.FindControl("exlApprovedBy");
                    if (td != null)
                    {
                        if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                        {
                            td.InnerText = ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).approvedBy.ToUpper();
                            td.Visible = true;
                        }
                    }

                    td = (HtmlTableCell)e.Item.FindControl("rowDept");//.Visible = false;
                    if (td != null)
                    {
                        td.InnerText = ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName;
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("rowShift");//.Visible = false;
                    if (td != null)
                    {
                        td.InnerText = ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).ShiftTypeInfo.ToString();
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("ftShift");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("ftDept");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                    td = (HtmlTableCell)e.Item.FindControl("ftLoc");//.Visible = false;
                    if (td != null)
                    {
                        td.Visible = true;
                    }
                }

                if (ClientInfo.IgnoreShiftList != null)
                {
                    if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                    {
                        EmployeeWorkSummary summary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        if (ClientInfo.IgnoreShiftList.Contains(summary.ShiftInfo.ShiftID))
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                }
                /*if (ClientInfo.ClientID == 127) //IP
                {
                    if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                    {
                        EmployeeWorkSummary summary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        if (summary.ShiftInfo.ShiftID == 1201) //plant2
                        {
                            e.Item.Visible = false;
                            return;
                        }
                    }
                }*/

                if (_exportType == ExportType.Excel || _exportType == ExportType.PeekABoo)
                {
                    HtmlInputControl inp = (HtmlInputControl)e.Item.FindControl("txtbonusMultiplier");
                    if (inp != null)
                        inp.Visible = false;
                    HtmlTableCell bonus = (HtmlTableCell)e.Item.FindControl("bonusMultiplyLabel");
                    if (bonus != null)
                        bonus.Visible = false;
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

                if (_exportType == ExportType.PeekABooTop)
                {
                    ((HtmlTableRow)e.Item.FindControl("trPeek")).Visible = true;
                    ((Label)e.Item.FindControl("spPeekName")).Text = _boundEmployeeHistory.LastName + _boundEmployeeHistory.FirstName; 
                }

                if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                {
                    // put the employee count into the panel/label
                    ((Label)e.Item.FindControl("lblEmpCnt")).Text = Convert.ToString(_boundEmployeeIdx);

                    //see if we need to display payrate
                    if ((ClientPrefs.DisplayPayRate && payRateJobCodeEligible == true) ) 
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdPayRate")).Visible = true;
                        Decimal payRate = _boundEmployeeHistory.GetEmployeePayRate();
                        ((HiddenField)e.Item.FindControl("hdnPayRate")).Value = payRate.ToString("$#,##0.00");
                        Decimal multiplier = (Decimal)ClientInfo.Multiplier;
                        /* This was removed as of 10-15-2024 by Dan. Should NEVER be hardcoded and is calculating incorrectly.
                        if (_clientId == 309)  //if show bill rate instead of pay rate
                        {
                            payRate = Math.Round((Decimal).15 + payRate * (Decimal)1.233, 2, MidpointRounding.AwayFromZero);
                            multiplier = (Decimal)1.2506;
                        }
                        */
                        multiplier = 1.0M;
                        //payRate = Math.Round(payRate, 2);
                        ((Label)e.Item.FindControl("lblPayRate")).Text = payRate.ToString("$#,##0.00");
                        if( (_clientId == 165) && !Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") && 
                                !Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") && 
                                !Context.User.Identity.Name.ToUpper().Equals("ITDEPT") && 
                                !Context.User.Identity.Name.ToUpper().Equals("MOAKES") && 
                                !Context.User.Identity.Name.ToUpper().Equals("RIZNERD"))
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdPayRate")).Visible = false;
                        }

                        ((HiddenField)e.Item.FindControl("hdnBillRateRatio")).Value = multiplier.ToString();                        
                    }
                    if( _showStartDate )
                    {
                        ((Label)e.Item.FindControl("lblFirstPunch")).Text = _boundEmployeeHistory.FirstPunch.ToString("MM/dd/yyyy");
                        ((HtmlTableCell)e.Item.FindControl("tdFirstPunch")).Visible = true;
                    }
                    else
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdFirstPunch")).Visible = false;
                    }
                    //see if we need to display jobcode
                    if (ClientPrefs.DisplayJobCode && payRateJobCodeEligible == true)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdJobCode")).Visible = true;
                        ((Label)e.Item.FindControl("lblJobCode")).Text = _boundEmployeeHistory.GetEmployeeJobCode();
                        ((HtmlTableCell)e.Item.FindControl("tdDeptSpacer")).Visible = true;
                        ((HtmlTableCell)e.Item.FindControl("tdShiftSpacer")).Visible = true;
                    }
                    if (_clientId == 113) //Arthur Schumann 
                    {
                        //((Label)e.Item.FindControl("lblBadgeNumber")).Text = ((Label)e.Item.FindControl("lblBadgeNumber")).Text.Substring(2);
                        ((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text = ((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text.Substring(2);
                        ((Label)e.Item.FindControl("lblDispatchExcel")).Text = ((Label)e.Item.FindControl("lblDispatchExcel")).Text.Substring(0,2);
                    }
                    if (ClientPrefs.ApproveHours && _exportType == ExportType.None)
                    {
                        HtmlTableCell tc = (HtmlTableCell)e.Item.FindControl("tdApproveHours");
                        tc.Visible = true;
                        if (_boundEmployeeHistory.HasInvalidWorkSummaries)
                        {
                            HtmlInputControl cont = (HtmlInputControl)e.Item.FindControl("btnLineApprove");
                            //cont = null;
                            cont.Disabled = true;
                            cont.Visible = false;
                            //CheckBox cb = (CheckBox)e.Item.FindControl("chkBoxApprove");
                            //cb.Enabled = false;
                            //cb.Checked = false;
                        }
                    }
                    if (_clientId == 302)
                    {
                        if( _exportType.Equals(ExportType.PeekABoo))
                        {
                            if ( _boundEmployeeHistory.WorkSummaries.Count > 0)
                            {
                                ((Label)e.Item.FindControl("lblCostCenter")).Text = "" + ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).CostCenter;
                                //((Panel)e.Item.FindControl("pnlCostCenter")).Visible = true;
                                ((Label)e.Item.FindControl("lblDept")).Text = "" + ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName;
                                //((Panel)e.Item.FindControl("pnlDept")).Visible = true;
                                ((Label)e.Item.FindControl("lblShift")).Text = "" + ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).ShiftTypeInfo.ToString();
                                //((Panel)e.Item.FindControl("pnlShift")).Visible = true;
                            }
                        }
                    }

                    if (_boundEmployeeHistory.HasLatePunches && _clientPrefs.DisplayBreakTimes)
                    {
                        ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Green;
                        ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Green;
                    }
                    if (  _clientId == 302 && _boundEmployeeHistory.MissingBreakPunches && 
                        ((EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0]).DepartmentInfo.DepartmentID != 1353)
                    {
                        ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Red;
                    }

                    //set the color of the labels if there are any invalid summaries
                    if (_boundEmployeeHistory.HasInvalidWorkSummaries)
                    {
                        _allowElectronicApproval = false;
                        ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblBadgeNumberExcel")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Red;
                        departmentInvalid = true;
                    }
                    else if (this.ClientPrefs.ApproveHours == true)
                    {
                        if (_boundEmployeeHistory.HasUnapprovedHours && 
                            !(_clientId == 302 && _boundEmployeeHistory.MissingBreakPunches &&
                            ((EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0]).DepartmentInfo.DepartmentID != 1353))
                        {
                            ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.Blue;
                            ((Label)e.Item.FindControl("lblBadgeNumberExcel")).ForeColor = System.Drawing.Color.Blue;
                            ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Blue;
                            ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Blue;
                            ((Label)e.Item.FindControl("lblUnApproved")).ForeColor = System.Drawing.Color.Blue;
                            ((Label)e.Item.FindControl("lblUnApproved")).Visible = false;
                            _allowElectronicApproval = false;
                            if (_exportType == ExportType.None)
                            {
                                HtmlTableCell tc = (HtmlTableCell)e.Item.FindControl("tdApproveHours");
                                tc.Visible = true;//lineApproveBtn
                                ((HtmlInputButton)e.Item.FindControl("btnLineApprove")).Visible = true;
                                ((HtmlInputButton)e.Item.FindControl("btnLineApprove")).Disabled = false;
                                ((Label)e.Item.FindControl("lblUnApproved")).Visible = true;
                            }
                        }
                    }

                    EmployeeWorkSummary summary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                    HtmlTableRow trDepartment = (HtmlTableRow)e.Item.FindControl("trDepartment");
                    HtmlTableCell tdDepartmentHead = (HtmlTableCell)e.Item.FindControl("tdDepartmentHead");
                    HtmlTableRow trDepartmentTotals = (HtmlTableRow)e.Item.FindControl("trDepartmentTotals");
                    HtmlInputButton btnTimeline = (HtmlInputButton)e.Item.FindControl("btnTimeline");
                    HtmlTableCell tdDepartmentTotalLabel = (HtmlTableCell)e.Item.FindControl("tdDepartmentTotalLabel");
                    //HtmlInputControl inputBonusMultiply = (HtmlInputControl)e.Item.FindControl("txtBonusMultiply");

                    
                    if (summary.DepartmentInfo.DepartmentID != _currentDepartmentId || summary.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                    {
                        //set start and end times of new dept/shift
                        EmployeeWorkSummary workSummary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                        String strtTime = workSummary.SummaryShiftStartDateTime.TimeOfDay.ToString();
                        String endTime = workSummary.SummaryShiftEndDateTime.TimeOfDay.ToString();
                        String strtDate = weekStartDate.Value;
                        String endDate;
                        if (strtTime.CompareTo(endTime) > 0)
                        {
                            endDate = this.WeekEnding.AddDays(1).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            endDate = this.WeekEnding.ToString("MM/dd/yyyy");
                        }

                        HtmlGenericControl pDeptStart = (HtmlGenericControl)e.Item.FindControl("pDeptStart");
                        pDeptStart.InnerText = strtDate + " " + strtTime;
                        HtmlGenericControl pDeptEnd = (HtmlGenericControl)e.Item.FindControl("pDeptEnd");
                        pDeptEnd.InnerText = endDate + " " + endTime;
                        if (_exportType != ExportType.None)
                        {
                            pDeptEnd.InnerHtml = "";
                            pDeptStart.InnerHtml = "";
                        }
                        //show the department break;
                        trDepartment.Visible = true;
                        
                            if (ClientPrefs.DisplayJobCode && ClientPrefs.DisplayPayRate)
                                tdDepartmentHead.ColSpan = 11;
                            else if (ClientPrefs.DisplayJobCode || ClientPrefs.DisplayPayRate)
                            {
                                tdDepartmentHead.ColSpan = 10;
                            }
                            if (ClientPrefs.ApproveHours && _exportType != ExportType.Excel)
                                tdDepartmentHead.ColSpan++;


                            if (_clientId != 302)
                            {
                                if (ClientPrefs.ShowLocationsHoursReport == false)
                                    summary.locationName = "";
                                else
                                    summary.locationName += " - ";
                                if (_showApproved)
                                {
                                    ((Label)e.Item.FindControl("lblDepartment")).Text = summary.locationName + summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString() + ": APPROVED";
                                }
                                else
                                {
                                    ((Label)e.Item.FindControl("lblDepartment")).Text = summary.locationName + summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString();
                                }
                                ((HiddenField)e.Item.FindControl("hdnDept")).Value = summary.DepartmentInfo.DepartmentID.ToString();
                                ((HiddenField)e.Item.FindControl("hdnShift")).Value = "" + summary.ShiftTypeInfo.ShiftTypeId;
                            }
                            else
                            {
                                if (ClientPrefs.ShowLocationsHoursReport == false)
                                    summary.locationName = "";
                                else
                                    summary.locationName += " - ";
                                if (_exportType != ExportType.PeekABoo)
                                {
                                    if (_showApproved)
                                    {
                                        ((Label)e.Item.FindControl("lblDepartment")).Text = summary.locationName + summary.DepartmentInfo.DepartmentName + " - " + summary.CostCenter + " - " + summary.ShiftTypeInfo.ToString() + ": APPROVED";
                                    }
                                    else
                                    {
                                        ((Label)e.Item.FindControl("lblDepartment")).Text = summary.locationName + summary.DepartmentInfo.DepartmentName + " - " + summary.CostCenter + " - " + summary.ShiftTypeInfo.ToString();
                                    }
                                    ((HiddenField)e.Item.FindControl("hdnDept")).Value = summary.DepartmentInfo.DepartmentID.ToString();
                                }
                                else
                                {
                                    trDepartment.Visible = false;
                                }
                                ((HiddenField)e.Item.FindControl("hdnDept")).Value = summary.DepartmentInfo.DepartmentID.ToString();
                                ((HiddenField)e.Item.FindControl("hdnShift")).Value = "" + summary.ShiftTypeInfo.ShiftTypeId;
                            }
                            if( _exportType != ExportType.None )
                            {
                                ((HiddenField)e.Item.FindControl("hdnDept")).Visible = false;
                                ((HiddenField)e.Item.FindControl("hdnShift")).Visible = false;
                            }
                            
                            _currentDepartmentId = summary.DepartmentInfo.DepartmentID;
                            _currentShiftType = summary.ShiftTypeInfo.ShiftTypeId;
                        //}
                    }
                    else
                    {
                        trDepartment.Visible = false;
                    }
                    if (_exportType == ExportType.Excel)
                    {
                        tdDepartmentHead.ColSpan--;
                        if( _clientId == 113)
                        {
                            if (ClientPrefs.DisplayPayRate)
                                tdDepartmentTotalLabel.ColSpan += 2;
                            else
                                tdDepartmentTotalLabel.ColSpan += 1;
                        }
                    }
                    else if( _exportType == ExportType.None )
                    {
                        if( Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") ||
                            Context.User.Identity.Name.ToUpper().Equals("BADANIS") ||
                            Context.User.Identity.Name.ToUpper().Equals("ITDEPT") )
                        btnTimeline.Visible = true;
                    }

                    //calculate total hours
                    this.CalculateTotalHours();

                    if (!_exportDetail)
                    {
                        //previous code
                        //((Label)e.Item.FindControl("lblMonHours")).Text = _boundEmployeeHistory.MondayHours.ToString();
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
                        else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Friday))
                        {
                            displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.SundaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.MondaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.TuesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.ThursdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.FridaySummary);

                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay1HiddenHours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay2HiddenHours")), _boundEmployeeHistory.SundaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay3HiddenHours")), _boundEmployeeHistory.MondaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay4HiddenHours")), _boundEmployeeHistory.TuesdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay5HiddenHours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay6HiddenHours")), _boundEmployeeHistory.ThursdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay7HiddenHours")), _boundEmployeeHistory.FridaySummary);
                        }
                        else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Tuesday))
                        {
                            displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.ThursdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.FridaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.SundaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.MondaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.TuesdaySummary);

                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay1HiddenHours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay2HiddenHours")), _boundEmployeeHistory.ThursdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay3HiddenHours")), _boundEmployeeHistory.FridaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay4HiddenHours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay5HiddenHours")), _boundEmployeeHistory.SundaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay6HiddenHours")), _boundEmployeeHistory.MondaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay7HiddenHours")), _boundEmployeeHistory.TuesdaySummary);
                        }
                        else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Thursday))
                        {
                            displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.FridaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.SundaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.MondaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.TuesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.ThursdaySummary);

                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay1HiddenHours")), _boundEmployeeHistory.FridaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay2HiddenHours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay3HiddenHours")), _boundEmployeeHistory.SundaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay4HiddenHours")), _boundEmployeeHistory.MondaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay5HiddenHours")), _boundEmployeeHistory.TuesdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay6HiddenHours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHiddenHours(((HiddenField)e.Item.FindControl("lblWeekDay7HiddenHours")), _boundEmployeeHistory.ThursdaySummary);
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
                    //if (_boundEmployeeHistory.TotalHoursAllDepts > 0)
                    //{
                    /*
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
                    */
                        ((HtmlTableCell)e.Item.FindControl("tdTotalWeek")).Visible = false;
                    if (this.ClientPrefs.DisplayBonuses && (this.Context.User.Identity.Name.ToUpper().Equals("ITDEPT")
                        || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                        || this.Context.User.Identity.Name.ToUpper().Equals("BADANIS")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VERAC")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FERRERM")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FLORESES")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MARIA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("SCHARPF")
                        || this.Context.User.Identity.Name.ToUpper().Equals("IMOLINA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VANESSAG")
                        || this.Context.User.Identity.Name.ToUpper().Equals("WHITEL")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")
                        || this.Context.User.Identity.Name.ToUpper().Equals("RIVERAS")))
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdBonusItem")).Visible = true;
                        ((Label)e.Item.FindControl("lblBonusItem")).Text = _boundEmployeeHistory.Bonus.ToString("N2");
                        BonusTotals += _boundEmployeeHistory.Bonus;
                        BonusGrandTotal += _boundEmployeeHistory.Bonus;
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lblBonusItem")).Parent.Visible = false;
                    }
                    //}
                    ((Label)e.Item.FindControl("lblOTHours")).Text = _boundEmployeeHistory.TotalOTHours.ToString("N2");

                    Repeater detail = e.Item.FindControl("rptrHoursReportDetail") as Repeater;
                    Repeater detailExcel = e.Item.FindControl("rptrHoursReportDetail_Excel") as Repeater;

                    if (detail != null)
                    {
                        Label lblBadgeNumberExcel = (Label)e.Item.FindControl("lblBadgeNumberExcel");
                        HtmlTableCell tdDispatch = (HtmlTableCell)e.Item.FindControl("dispatch");
                        Label lblDispatchExcel = (Label)e.Item.FindControl("lblDispatchExcel");
                        if (_exportDetail)
                        {
                            detailExcel.Visible = true;
                            detail.Visible = false;
                            ((Panel)e.Item.FindControl("pnlPlusMinus")).Visible = false;
                            //detail.Visible = false;
                            lblBadgeNumberExcel.Visible = true;
                            if( _clientInfo.ClientID == 113 )
                            {
                                tdDispatch.Visible = true;
                                lblDispatchExcel.Visible = true;
                            }
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
                                tdDispatch.Visible = false;
                                lblDispatchExcel.Visible = false;
                                detail.DataSource = _boundEmployeeHistory.WorkSummaries;
                                detail.DataBind();
                            }
                            else
                            {
                                detail.Visible = false;
                                lblBadgeNumberExcel.Visible = true;
                                if (_clientInfo.ClientID == 113)
                                {
                                    lblDispatchExcel.Visible = true;
                                    tdDispatch.Visible = true;
                                }
                                ((Panel)e.Item.FindControl("pnlPlusMinus")).Visible = false;
                            }
                        }
                    }

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

                    if (nextSummary == null || (e.Item.ItemIndex == _hoursReport.EmployeeHistoryCollection.Count - 1) || (nextSummary.DepartmentInfo.DepartmentID != summary.DepartmentInfo.DepartmentID || nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId))
                    {
                        //always display department in this case
                        if (_exportType.Equals(ExportType.PeekABoo) && _clientId == 302)
                            trDepartmentTotals.Visible = false;
                        else
                            trDepartmentTotals.Visible = true;

                        /* set up bonus totals */
                        if (this.ClientPrefs.DisplayBonuses == true && (
                               this.Context.User.Identity.Name.ToLower().Equals("itdept")
                            || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                            || this.Context.User.Identity.Name.ToUpper().Equals("BADANIS")
                            || this.Context.User.Identity.Name.ToUpper().Equals("VERAC")
                            || this.Context.User.Identity.Name.ToUpper().Equals("FERRERM")
                            || this.Context.User.Identity.Name.ToUpper().Equals("FLORESES")
                            || this.Context.User.Identity.Name.ToUpper().Equals("MARIA")
                            || this.Context.User.Identity.Name.ToUpper().Equals("SCHARPF")
                            || this.Context.User.Identity.Name.ToUpper().Equals("VANESSAG")
                            || this.Context.User.Identity.Name.ToUpper().Equals("IMOLINA")
                            || this.Context.User.Identity.Name.ToUpper().Equals("BAJEKS")
                            || this.Context.User.Identity.Name.ToUpper().Equals("WHITEL")
                            || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")
                            || this.Context.User.Identity.Name.ToUpper().Equals("RIVERAS")))
                        {
                            ((Label)e.Item.FindControl("lblDeptTotBonus")).Text = "$ " + BonusTotals.ToString("N2");
                            BonusTotals = 0;
                        }
                        else
                        {
                            ((Label)e.Item.FindControl("lblDeptTotBonus")).Parent.Visible = false;
                        }

                        if (ClientPrefs.DisplayPayRate && payRateJobCodeEligible)
                        {
                            tdDepartmentTotalLabel.ColSpan = 3;
                            if (_clientId == 113 && _exportType == ExportType.Excel)
                            {
                                tdDepartmentTotalLabel.ColSpan = 4;
                            }
                            if (_clientId == 165 && !Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") &&
                                    !Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") && 
                                    !Context.User.Identity.Name.ToUpper().Equals("ITDEPT"))
                                tdDepartmentTotalLabel.ColSpan = 2;
                        }
                        System.Drawing.Color col;
                        if (departmentInvalid)
                            col = System.Drawing.Color.Red;
                        else
                            col = System.Drawing.Color.Black;
                        departmentInvalid = false;
                        Label l = ((Label)e.Item.FindControl("lblDepartmentTotalLabel"));
                        if (_showApproved)
                        {
                            l.Text = summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString() + " Totals APPROVED:";
                            l.ForeColor = col;
                        }
                        else
                        {
                            l.Text = summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString() + " Totals:";
                            l.ForeColor = col;
                        }
                        if( _showStartDate )
                        {
                            HtmlTableCell tc = ((HtmlTableCell)e.Item.FindControl("tdDepartmentTotalLabel"));
                            tc.ColSpan += 1;
                        }

                        if (nextSummary == null || (e.Item.ItemIndex == _hoursReport.EmployeeHistoryCollection.Count - 1) || (nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId))
                        {

                        }

                        if (!_exportDetail)
                        {
                            int offset = 0;
                            if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Saturday))
                                offset = 6;
                            else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Friday))
                                offset = 5;
                            else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Tuesday))
                                offset = 2;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay1Hours"));
                            l.Text = _totDailyHours[(0 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(0 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay2Hours"));
                            l.Text = _totDailyHours[(1 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(1 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay3Hours"));
                            l.Text = _totDailyHours[(2 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(2 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay4Hours"));
                            l.Text = _totDailyHours[(3 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(3 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay5Hours"));
                            l.Text = _totDailyHours[(4 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(4 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay6Hours"));
                            l.Text = _totDailyHours[(5 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
                            _totDailyHours[(5 + offset) % 7] = 0M;
                            l = ((Label)e.Item.FindControl("lblDeptTotWeekDay7Hours"));
                            l.Text = _totDailyHours[(6 + offset) % 7].ToString("N2");
                            l.ForeColor = col;
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
                        l = ((Label)e.Item.FindControl("lblDeptTotTotalHours"));
                        l.Text = _totDailyHours[7].ToString("N2");
                        l.ForeColor = col;

                        _totDailyHours[7] = 0M;
                        l = ((Label)e.Item.FindControl("lblDeptTotOTHours"));
                        l.Text = _totDailyHours[8].ToString("N2");
                        l.ForeColor = col;
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
                /* This was removed as of 10-15-2024 by Dan. Should NEVER be hardcoded and is calculating incorrectly.
                if ( _clientId == 309 )
                    ((HtmlTableCell)e.Item.FindControl("tdPayRateHead")).InnerText = "Bill Rate";
                */
                if (ClientPrefs.DisplayPayRate && payRateJobCodeEligible)
                {
                    if( _clientId != 165 || Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") ||
                        Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") || 
                        Context.User.Identity.Name.ToUpper().Equals("ITDEPT"))
                        ((HtmlTableCell)e.Item.FindControl("tdPayRateHead")).Visible = true;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdPayRateHead")).Visible = false;
                }
                if (ClientPrefs.DisplayJobCode && payRateJobCodeEligible)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdJobCodeHead")).Visible = true;
                    if( _clientId >= 332 && _clientId <= 334 || _clientId == 350)
                    {
                        ((Label)e.Item.FindControl("lblJobCodeHead")).Text = "GL Code";
                    }

                }
                else
                    ((HtmlTableCell)e.Item.FindControl("tdJobCodeHead")).Visible = false;
                if( _showStartDate == false )
                {
                    ((HtmlTableCell)e.Item.FindControl("tdFirstPunchHead")).Visible = false;
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdFirstPunchHead")).Visible = true;
                }

                if (_clientId == 302 && _exportType == ExportType.PeekABoo)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdCostCenterHead")).Visible = true;
                    ((HtmlTableCell)e.Item.FindControl("tdShiftHead")).Visible = true;
                    ((HtmlTableCell)e.Item.FindControl("tdDeptHead")).Visible = true;
                }

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
                else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Friday))
                {
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Tue";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Sun";
                }
                else if( this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Tue";
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Sun";
                }
                else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Tuesday))
                {
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Sun";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Tue";
                }
                else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Thursday))
                {
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Mon";
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Tue";
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Wed";
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Thr";
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Fri";
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Sat";
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Sun";
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
                        {
                            if( ClientPrefs.DisplayStartDate)
                            {
                                ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 5;
                            }
                            else
                            {
                                ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 4;
                            }
                        }
                        else if( ClientPrefs.DisplayStartDate /*_clientId >= 325 && _clientId <= 327*/ )
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 4;
                        }
                        else if (ClientPrefs.DisplayPayRate || ClientPrefs.DisplayJobCode)
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 3;
                            if (_clientId == 165 && !Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") &&
                                    !Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ"))
                                ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 2;
                        }
                        else
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 2;
                        }
                    }
                    if( _clientId == 113 )
                    {
                        if (ClientPrefs.DisplayPayRate)
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 5;
                        else
                            ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 4;
                    }
                    if (_clientId == 302 && _exportType.Equals(ExportType.PeekABoo))
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan += 3;
                    }

                    ((Label)e.Item.FindControl("lblClientName")).Text = ClientInfo.ClientName;
                    //((HtmlTableCell)e.Item.FindControl("tdExcelClientName")).ColSpan = 2;

                    if (_exportType == ExportType.Excel || _exportType == ExportType.PeekABoo)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdExcelTitleSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdClientNameSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("tdDateSpc")).Visible = false;
                        ((HtmlTableCell)e.Item.FindControl("seqNum")).Visible = false;
                    }
                    if (_exportDetail == false)
                    {
                        ((Label)e.Item.FindControl("lblExcelWeekEnding")).Text = "Week Ending " + this.WeekEnding.ToString("MM/dd/yyyy");
                        ((Label)e.Item.FindControl("lblExcelWeekDay1")).Text = this.WeekEnding.AddDays(-6).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay2")).Text = this.WeekEnding.AddDays(-5).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay3")).Text = this.WeekEnding.AddDays(-4).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay4")).Text = this.WeekEnding.AddDays(-3).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay5")).Text = this.WeekEnding.AddDays(-2).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay6")).Text = this.WeekEnding.AddDays(-1).Date.ToString("M/d");
                        ((Label)e.Item.FindControl("lblExcelWeekDay7")).Text = this.WeekEnding.AddDays(-0).Date.ToString("M/d");
                    }
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
                if (_exportType == ExportType.Excel || _exportType == ExportType.PeekABoo )
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGrandTotalsSpc")).Visible = false;
                    if (_clientId == 78 || _clientId == 383 ) 
                    {
                        HtmlTableCell td = (HtmlTableCell)e.Item.FindControl("totDept");//.Visible = false;
                        if (td != null)
                        {
                            td.InnerText = ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName;
                            td.Visible = true;
                        }
                        td = (HtmlTableCell)e.Item.FindControl("totShift");//.Visible = false;
                        if (td != null)
                        {
                            td.InnerText = ((EmployeeWorkSummary)(_boundEmployeeHistory.WorkSummaries[0])).ShiftTypeInfo.ToString();
                            td.Visible = true;
                        }
                        td = (HtmlTableCell)e.Item.FindControl("totLoc");//.Visible = false;
                        if (td != null)
                        {
                            td.Visible = true;
                        }
                    }
                    if (_clientId == 113)
                    {
                        if( ClientPrefs.DisplayPayRate)
                            ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan += 3;
                        else
                            ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan += 2;
                    }
                }
                if (ClientPrefs.DisplayPayRate && payRateJobCodeEligible)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan = 3;
                    if (_exportType == ExportType.Excel && _clientId == 113)
                        ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan = 4;
                    if ( _exportType == ExportType.PeekABoo && _clientId == 302 )
                        ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan += 3;
                }
                if (ClientPrefs.DisplayJobCode /*_clientId == 226 || ((_clientId == 178 || _clientId == 256 )*/
                    && payRateJobCodeEligible)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGTSpacer")).Visible = true;
                }
                if( _showStartDate)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan += 1;
                    if (_exportType != ExportType.None && ClientPrefs.DisplayPayRate == false)
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdGrandTotalLabel")).ColSpan -= 1;
                    }
                }

                if (!_exportDetail)
                {
                    int offset = 0;
                    if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Saturday))
                        offset = 6;
                    else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Friday))
                        offset = 5;
                    else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Thursday))
                        offset = 4;
                    else if (this.WeekEnding.DayOfWeek.Equals(DayOfWeek.Tuesday))
                        offset = 2;
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

                if (this.ClientPrefs.DisplayBonuses && (
                           this.Context.User.Identity.Name.ToUpper().Equals("ITDEPT")
                        || this.Context.User.Identity.Name.ToUpper().Equals("JULIO")
                        || this.Context.User.Identity.Name.ToUpper().Equals("BADANIS")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VERAC")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FERRERM")
                        || this.Context.User.Identity.Name.ToUpper().Equals("FLORESES")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MARIA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("SCHARPF")
                        || this.Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")
                        || this.Context.User.Identity.Name.ToUpper().Equals("IMOLINA")
                        || this.Context.User.Identity.Name.ToUpper().Equals("VANESSAG")
                        || this.Context.User.Identity.Name.ToUpper().Equals("WHITEL")
                        || this.Context.User.Identity.Name.ToUpper().Equals("RIVERAS")
                        ))
                {
                    ((Label)e.Item.FindControl("lblGrandTotBonus")).Text = "$ " + BonusGrandTotal.ToString("N2");
                }
                else
                {
                    ((Label)e.Item.FindControl("lblGrandTotBonus")).Parent.Visible = false;
                }
            }
        }

        protected void rptrHoursReportDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblCheckIn = (Label)e.Item.FindControl("lblCheckIn");
                Label lblCheckInType = (Label)e.Item.FindControl("lblCheckInType");
                Label lblCheckOut = (Label)e.Item.FindControl("lblCheckOut");
                Label lblCheckOutType = (Label)e.Item.FindControl("lblCheckOutType");
                Label lblCheckHours = (Label)e.Item.FindControl("lblCheckHours");
                Label lblCheckBreak = (Label)e.Item.FindControl("lblCheckBreak");
                Label lblLatePunch = (Label)e.Item.FindControl("lblLatePunch");

                Label hdnApprovedBy = (Label)e.Item.FindControl("lblApprovedBy");
                HtmlTableRow rowDetail = (HtmlTableRow)e.Item.FindControl("tableRow");

                //EmployeeHistory eh = (EmployeeHistory)e.Item.DataItem;
                EmployeeWorkSummary workSummary = (EmployeeWorkSummary)e.Item.DataItem;// WorkSummaries; //(EmployeeWorkSummary)eh.WorkSummaries;// (EmployeeWorkSummary)e.Item.DataItem;
                if( rowDetail != null )
                    rowDetail.Attributes.Add("badge", workSummary.Badge.ToString());

                DateTime punchIn = new DateTime(workSummary.RoundedCheckInDateTime.Year, workSummary.RoundedCheckInDateTime.Month, workSummary.RoundedCheckInDateTime.Day, workSummary.RoundedCheckInDateTime.Hour, workSummary.RoundedCheckInDateTime.Minute, 0);
                DateTime punchInExact = new DateTime(workSummary.CheckInDateTime.Year, workSummary.CheckInDateTime.Month, workSummary.CheckInDateTime.Day, workSummary.CheckInDateTime.Hour, workSummary.CheckInDateTime.Minute, workSummary.CheckInDateTime.Second, workSummary.CheckInDateTime.Millisecond);
                if (_firstPunch.Equals(DATE_NOT_SET))
                {
                    _firstPunch = punchIn;
                }
                
                lblCheckIn.Text = punchIn.ToString("MM/dd/yyyy h:mm tt");
                if( lblCheckInType != null )
                {
                    if( workSummary.CheckInManualOverrideFlag )
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdCheckIn")).BgColor =
                            "#FFB6C1";
                        ((HtmlTableCell)e.Item.FindControl("tdCheckInType")).BgColor =
                            "#FFB6C1";
                        lblCheckInType.Text = workSummary.CheckInCreatedBy.ToString();
                    }
                }
                lblCheckIn.Attributes.Add("Exact", punchInExact.ToString("MM/dd/yyyy hh:mm:ss tt"));
                lblCheckIn.Attributes.Add("Round", lblCheckIn.Text);
                lblCheckIn.Attributes.Add("PunchId", "" + workSummary.CheckInEmployeePunchID);
                lblCheckIn.Attributes.Add("Manual", "" + workSummary.CheckInManualOverrideFlag);
                lblCheckIn.Attributes.Add("Ticks", "" + DateTimeHelpers.CSTToClockTicks(punchInExact));
                lblCheckIn.Attributes.Add("Loc", "" + workSummary.CheckInLocation.ToString());

                if (workSummary.CheckInManualOverrideFlag == true)
                {
                    lblCheckIn.Attributes.Add("CreatedDt", "" + workSummary.CheckInCreatedDt.ToString("MM/dd/yyyy h:mm tt"));
                    lblCheckIn.Attributes.Add("CreatedBy", "" + workSummary.CheckInCreatedBy.ToString());
                    lblCheckIn.Attributes.Add("UpdatedBy", "" + workSummary.CheckInLastUpdatedBy.ToString());
                    lblCheckIn.Attributes.Add("UpdatedDt", "" + workSummary.CheckInLastUpdatedDate.ToString("MM/dd/yyyy h:mm tt"));
                    if ( _clientInfo.ClientID == 310 )
                        lblCheckIn.Text = "**" + punchIn.ToString("MM/dd/yyyy hh:mm tt");
                }
                if (hdnApprovedBy != null)
                {
                    if (workSummary.approvedBy.Length > 0)
                        hdnApprovedBy.Text += workSummary.approvedBy;
                    else if( workSummary.ClientID == 302 )
                        hdnApprovedBy.Text += "N/A ";
                }
                if (workSummary.CheckOutDateTime != null && workSummary.CheckOutDateTime.Date != new DateTime(1, 1, 1).Date)
                {
                    DateTime punchOut = new DateTime(workSummary.RoundedCheckOutDateTime.Year, workSummary.RoundedCheckOutDateTime.Month, workSummary.RoundedCheckOutDateTime.Day, workSummary.RoundedCheckOutDateTime.Hour, workSummary.RoundedCheckOutDateTime.Minute, 0);
                    DateTime punchOutExact = new DateTime(workSummary.CheckOutDateTime.Year, workSummary.CheckOutDateTime.Month, workSummary.CheckOutDateTime.Day, workSummary.CheckOutDateTime.Hour, workSummary.CheckOutDateTime.Minute, workSummary.CheckOutDateTime.Second, workSummary.CheckOutDateTime.Millisecond);
                    _lastPunch = punchOut;
                    lblCheckOut.Text = punchOut.ToString("MM/dd/yyyy hh:mm tt");
                    if( lblCheckOutType != null )
                    {
                        if (workSummary.CheckOutManualOverrideFlag)
                        {
                            ((HtmlTableCell)e.Item.FindControl("tdCheckOut")).BgColor =
                                "#FFB6C1";
                            ((HtmlTableCell)e.Item.FindControl("tdCheckOutType")).BgColor =
                                "#FFB6C1";
                            lblCheckOutType.Text = workSummary.CheckOutCreatedBy.ToString();
                        }
                    }
                    lblCheckOut.Attributes.Add("Exact", punchOutExact.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    lblCheckOut.Attributes.Add("Round", lblCheckOut.Text);
                    lblCheckOut.Attributes.Add("PunchId", "" + workSummary.CheckOutEmployeePunchID);
                    lblCheckOut.Attributes.Add("Manual", "" + workSummary.CheckOutManualOverrideFlag);
                    lblCheckOut.Attributes.Add("Ticks", "" + DateTimeHelpers.CSTToClockTicks(punchOutExact));
                    lblCheckOut.Attributes.Add("Loc", "" + workSummary.CheckOutLocation.ToString());
                    if (workSummary.CheckOutManualOverrideFlag == true)
                    {
                        lblCheckOut.Attributes.Add("CreatedDt", "" + workSummary.CheckOutCreatedDt.ToString("MM/dd/yyyy h:mm tt"));
                        lblCheckOut.Attributes.Add("CreatedBy", "" + workSummary.CheckOutCreatedBy.ToString());
                        lblCheckOut.Attributes.Add("UpdatedDt", "" + workSummary.CheckOutLastUpdatedDate.ToString("MM/dd/yyyy h:mm tt"));
                        lblCheckOut.Attributes.Add("UpdatedBy", "" + workSummary.CheckOutLastUpdatedBy.ToString());
                        if ( _clientInfo.ClientID == 310 )
                            lblCheckOut.Text = "**" + punchOut.ToString("MM/dd/yyyy hh:mm tt");
                    }

                    //sum up the hours
                    TimeSpan difference = punchOut - punchIn;
                    TimeSpan differenceExact = punchOutExact - punchInExact;
                    if (punchIn.IsDaylightSavingTime() != punchOut.IsDaylightSavingTime())
                    {
                        if( punchOut.IsDaylightSavingTime())
                        {
                            TimeSpan ts = new TimeSpan(1,0,0);
                            difference = difference.Subtract(ts);
                            differenceExact = difference.Subtract(ts);
                        }
                        else
                        {
                            TimeSpan ts = new TimeSpan(1, 0, 0);
                            difference = difference.Add(ts);
                            differenceExact = difference.Add(ts);
                        }
                    }
                    String summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2).ToString("N2");
                    String summaryHoursExact = Math.Round(Convert.ToDecimal(differenceExact.TotalMinutes / 60), 2).ToString("N2");
                    //String summaryHours = difference.ToString(@"hh\:mm");
                    //String summaryHoursExact = differenceExact.ToString();
                    double summaryHoursWithDeduction = Math.Round(Convert.ToDouble(difference.TotalMinutes / 60), 2);
                    if ( (summaryHoursWithDeduction >= 5.0 && _clientInfo.ClientID != 310) ||
                        (summaryHoursWithDeduction > 5.0 && _clientInfo.ClientID == 310))
                    {
                        summaryHoursWithDeduction -= workSummary.BreakTime;
                    }

                    if (_clientInfo.ClientID != 310)
                    {
                        lblCheckHours.Text = summaryHours;
                        lblCheckHours.Attributes.Add("Round", summaryHours);
                        lblCheckHours.Attributes.Add("Exact", summaryHoursExact);
                        if (workSummary.BreakAmount.TotalSeconds > 0)
                            lblCheckBreak.Text = workSummary.BreakAmount.ToString(@"hh\:mm\:ss");
                        if( workSummary.MinutesLate.TotalSeconds > 0 )
                        {
                            lblLatePunch.Text = workSummary.MinutesLate.ToString(@"hh\:mm\:ss");
                        }
                    }
                    else
                    {
                        lblCheckHours.Text = summaryHoursWithDeduction.ToString("N2");
                        lblCheckHours.Attributes.Add("Round", summaryHours);
                        lblCheckHours.Attributes.Add("Exact", summaryHoursExact);
                    }
                    if (ClientPrefs.ApproveHours)
                    {
                        if (!workSummary.CheckInApproved || !workSummary.CheckOutApproved)
                        {
                            lblCheckIn.ForeColor = System.Drawing.Color.Blue;
                            lblCheckOut.ForeColor = System.Drawing.Color.Blue;
                            lblCheckHours.ForeColor = System.Drawing.Color.Blue;
                            lblCheckBreak.ForeColor = System.Drawing.Color.Blue;
                            lblCheckHours.Text += " (Unapproved)";
                            lblCheckHours.Attributes.Add("Round", summaryHours + " (Unapproved)");
                            lblCheckHours.Attributes.Add("Exact", summaryHoursExact + " (Unapproved)");
                        }
                    }
                }
                else
                {
                    lblCheckIn.ForeColor = System.Drawing.Color.Red;
                    lblCheckOut.Text = "N/A";
                    lblCheckOut.ForeColor = System.Drawing.Color.Red;
                    lblCheckHours.Text = "N/A";
                    lblCheckHours.ForeColor = System.Drawing.Color.Red;
                    lblCheckBreak.Text = "N/A";
                    lblCheckBreak.ForeColor = System.Drawing.Color.Red;
                }
                if (_clientPrefs.DisplayBreakTimes == false)
                {
                    lblCheckBreak.Text = " ";
                    lblCheckBreak.Style.Add("Display", "None");
                    lblLatePunch.Text = " ";
                    lblLatePunch.Style.Add("Display", "None");
                }

            }
            else if (e.Item.ItemType == ListItemType.Header )
            {
                if( _clientPrefs.DisplayBreakTimes == false )
                {
                    ((Label)e.Item.FindControl("lbl1stPunch")).Visible = false;
                    ((Label)e.Item.FindControl("lblBreak")).Visible = false;
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
            DateTime punchOut = new DateTime(1, 1, 1);
            DateTime roundedPunchOut = new DateTime(1, 1, 1);

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

            _totDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totShiftDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totDailyHours[8] += _boundEmployeeHistory.TotalOTHours;
            _totShiftDailyHours[8] += _boundEmployeeHistory.TotalOTHours;

            _reportHours += _boundEmployeeHistory.TotalRegularHours;

            _reportOTHours += _boundEmployeeHistory.TotalOTHours;
        }
        protected void displayHours(Label lbl, DailySummary dailySummary)
        {
            if (_clientId >= 325 && _clientId <= 327)
            {
                if (dailySummary.TotalHoursWorked > 12)
                {
                    lbl.ForeColor = System.Drawing.Color.Red;
                    lbl.BorderStyle = BorderStyle.Solid;
                    lbl.BorderWidth = Unit.Pixel(1);
                }
            }
            else if (dailySummary.TotalHoursWorked > 16)
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
                this.lnkExport.Visible = false;
                this.lnkExportDetail.Visible = false;
                this.lnkExportPeekABoo.Visible = false;
                _showApproved = false;

                if( btn.CommandName == "UnSubmitApproved")
                {
                    if (ClientPrefs.NotifyHoursReady)
                    {
                        //send an email
                        this.SendEMail(false);
                    }
                    HoursReport hoursReportInput = new HoursReport();
                    hoursReportInput.ClientID = _clientInfo.ClientID;
                    hoursReportInput.EndDateTime = this.WeekEnding;
                    HoursReportBL hoursReportBL = new HoursReportBL();
                    hoursReportBL.UnSubmitClientHours(hoursReportInput, Context.User);
                    btnSubmitApproved.Text = "Submit Approved Hours";
                    btnSubmitApproved.BackColor = System.Drawing.Color.White;
                    btnSubmitApproved.CommandName = "SubmitApproved";
                }
                else if (btn.CommandName == "SubmitApproved")
                {
                    this.lnkExport.Visible = true;
                    this.lnkExportDetail.Visible = true;
                    _showApproved = true;
                    if (ClientPrefs.NotifyHoursReady)
                    {
                        //send an email
                        this.SendEMail(true);
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
                                if (summary.approvedBy != null && summary.approvedBy.Length > 0)
                                    continue;
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
                    lnkExport.Visible = false;
                    lnkExportDetail.Visible = false;
                    lnkExportPeekABoo.Visible = false;
                    _showApproved = false;
                }
            }
            LoadHoursReport();
            if( !chkboxSupervisor.Checked == true && !showSupervisors)
                this.pnlTotals.Visible = true;
        }

        private void SendEMail(bool approve)
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;

                Configuration rootWebConfig1 =
                                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

                KeyValueConfigurationElement email =
                    rootWebConfig1.AppSettings.Settings["submitApprovedEmail"];
                KeyValueConfigurationElement userId =
                    rootWebConfig1.AppSettings.Settings["submitApprovedUserId"];
                KeyValueConfigurationElement pwd =
                    rootWebConfig1.AppSettings.Settings["submitApprovedPwd"];
                //System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential(userId.Value, pwd.Value, "msistaff.com");
                //System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("MetroStaff@msistaff.com", "Emp!0ysr(");
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("metrostaffinc@gmail.com", "isouhrfseigmwnzs");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //mailClient.Host = "smtp.office365.com";
                mailClient.Host = "smtp.gmail.com";
                mailClient.Port = 587; 
                mailClient.EnableSsl = true;
                message = new System.Net.Mail.MailMessage();
                //message.From = new System.Net.Mail.MailAddress(email.Value);
                message.From = new System.Net.Mail.MailAddress("MetroStaff@msistaff.com");
                message.To.Add("medwards@msistaff.com");
                message.To.Add("imolina@msistaff.com");
                message.To.Add("cmendoza@msistaff.com");
                //message.To.Add("virginia@msistaff.com");
                message.To.Add("anperez@msistaff.com");
                message.To.Add("lgarcia@msistaff.com");
                message.To.Add("msanchez@msistaff.com");
                message.To.Add("lisa@msistaff.com");
                message.To.Add("vtello@msistaff.com");
                message.To.Add("atovar@msistaff.com");
                message.To.Add("jmurfey@msistaff.com");

                //message.To.Add("approvedhours@msistaff.com");
                switch (ClientInfo.ClientID)
                {
                    case 30:
                        message.To.Add("list-ah-elitemanufacturing@msistaff.com");  //chris@emt333.com
                        break;
                    case 121:
                        message.To.Add("list-ah-wiseplastics@msistaff.com"); //Rich.Burfield@wiseplastics.com, Dan.Clark@wiseplastics.com
                        break;
                    case 186:
                        message.To.Add("list-ah-tutco@msistaff.com"); //DShire@Fastheat.com
                        break;
                    case 158:
                        message.To.Add("list-ah-pmall@msistaff.com"); //deborahw@pmall.com, mariah@pmall.com, billr@pmall.com
                        break;
                    case 211:
                        message.To.Add("list-ah-continentalweb@msistaff.com"); //asanchez@continentalweb.com
                        break;
                }
                if( approve )
                {
                    message.Subject = ClientInfo.ClientName + " Hours are approved";
                    message.Body = "Hours have been approved for " + ClientInfo.ClientName + " for week ending " + this.WeekEnding.ToString("MM/dd/yyyy");
                }
                else
                {
                    message.Subject = ClientInfo.ClientName + " Hours are UNSUBMITTED";
                    message.Body = "NOTICE: Hours have been UNSUBMITTED by USER: " + Context.User.Identity.Name + " for CLIENT: " + ClientInfo.ClientName + " for week ending " + this.WeekEnding.ToString("MM/dd/yyyy");
                }
                message.IsBodyHtml = false;

                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
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
            if( sender is Button )
                btn = (Button)sender;
            //System.Windows.Forms.MessageBox.Show(Session["hoursreport"].ToString());
            if (Session["hoursreport"] != null)
            {
                _hoursReport = (HoursReport)Session["hoursreport"];
                _clientPrefs = (ClientPreferences)Session["ClientPrefs"];
                //create the invoice from the approved hours
                Invoice invoice = new Invoice();
                invoice.ClientID = _clientInfo.ClientID;
                invoice.ClientApprovalId = int.Parse(this.hdnClientApprovalId.Value);
                invoice.WeekEndDate = DateTime.Parse(this.hdnWeekEndDate.Value);
                InvoiceDetail detail = null;
                InvoiceBL invoiceBL = new InvoiceBL();
                EmployeeWorkSummary summary = new EmployeeWorkSummary();
                foreach (EmployeeHistory empHist in _hoursReport.EmployeeHistoryCollection)
                {
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
                        detail.ClientId = invoice.ClientID;
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
            
            return invoiceBL.GetInvoice( invoice, Context.User);
            //_loadedInvoice = true;
        }

        protected void createCSV(object sender, EventArgs e)
        {
            invoice.ClientID = ClientInfo.ClientID;
            invoice.WeekEndDate = WeekEnding;//DateTime.Parse(this.WeekEnding);
            invoice = this.getInvoice();

            StringBuilder csvInfo = new StringBuilder();
            //add headers

            bool bonus = false;
            if( _clientPrefs.DisplayBonuses == false )
                bonus = false;
            else
                bonus = true;
            if (_clientPrefs.DisplayBonuses)
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID, BONUS_PAY");
            else
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID");
            

            foreach (InvoiceDetail detail in invoice.DetailInfo)
            {
                StringBuilder shiftDifferential = null;
                StringBuilder empLine = new StringBuilder();

                bool use2ndShiftPayDifferential = detail.ShiftTypeInfo.ShiftTypeId == 2 &&
                    (_clientPrefs.ClientID == 332 || _clientPrefs.ClientID == 333 || _clientPrefs.ClientID == 334 ||
                        _clientPrefs.ClientID == 402 || _clientPrefs.ClientID == 407);

                if (!btnOffice.SelectedValue.Equals("*") && !btnOffice.SelectedValue.Equals(detail.Office))
                    continue;
                if (ClientInfo.IgnoreShiftList != null && ClientInfo.IgnoreShiftList.Contains(detail.ShiftInfo.ShiftID))
                {
                    continue;
                }

                empLine.Append(Environment.NewLine);
                //aident
                empLine.Append(detail.BadgeNumber.Substring(2) + ",");

                //ssn
                empLine.Append("\"\",");

                //lastname
                if( detail.LastName.Length > 20 )
                {
                    empLine.Append("\"" + detail.LastName.Substring(0, 19).Trim() + "\",");
                }
                else
                {
                    empLine.Append("\"" + detail.LastName.ToUpper().Trim() + "\",");
                }

                //firstname
                if (detail.FirstName.Length > 20)
                {
                    empLine.Append("\"" + detail.FirstName.Substring(0, 19).Trim() + "\",");
                }
                else
                {
                    empLine.Append("\"" + detail.FirstName.ToUpper().Trim() + "\",");
                }
                //regular hours
                empLine.Append(detail.TotalRegularHours.ToString("0.00") + ",");
                //ot hours
                empLine.Append(detail.TotalOTHours.ToString("0.00") + ",");

                if( use2ndShiftPayDifferential )
                {
                    shiftDifferential = new StringBuilder(empLine.ToString());
                }

                //pay rate
                empLine.Append(detail.PayRate.ToString("0.00") + ",");
                //bill rate
                /* HACK for multiple bill rates */
                decimal tempBill;
                if( _clientId == 213 )
                {
                    decimal mult;
                    if (detail.PayRate == (decimal)10.55) mult = (decimal)(14.25 / 10.55);
                    else if (detail.PayRate == (decimal)11.30) mult = (decimal)(15.37 / 11.30);
                    else if (detail.PayRate == (decimal)12.00) mult = (decimal)(16.44 / 12.00);
                    else if (detail.PayRate == (decimal)12.25) mult = (decimal)(16.42 / 12.25);
                    else mult = detail.RegularMultiplier;
                    tempBill = Math.Round(detail.PayRate * mult, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    tempBill = Math.Round(detail.PayRate * detail.RegularMultiplier, 2, MidpointRounding.AwayFromZero);
                }
                empLine.Append(tempBill.ToString("0.00") + ",");
                /* pay and bill for 2ndshift differential */
                if (use2ndShiftPayDifferential)
                {
                    shiftDifferential.Append("1.00,1.28,");
                }


                //retro reg and retro OT
                empLine.Append("0.00,0.00,");
                if (use2ndShiftPayDifferential)
                {
                    shiftDifferential.Append("0.00,0.00,");
                }

                //temp works deparment id
                empLine.Append("\"" + detail.ShiftInfo.TempWorksMappingId.ToString() + "\"");

                if (use2ndShiftPayDifferential)
                {
                    shiftDifferential.Append("\"" + detail.ShiftInfo.TempWorksMappingId.ToString() + "\"");
                }

                //bonus pay
                if (_clientPrefs.DisplayBonuses)
                    empLine.Append("," + detail.Bonus.ToString("0.00"));
                csvInfo.Append(empLine.ToString());
                if( use2ndShiftPayDifferential )
                {
                    csvInfo.Append(shiftDifferential.ToString());
                }
            }

            string deptName = "_";
            if (this.ClientInfo.ClientID == 166)
            {
                if (this.rbtnDept2.Checked == true)
                    deptName = "_" + this.rbtnDept2.Text + "_";
                if (this.rbtnDept.Checked == true)
                    deptName = "_" + this.rbtnDept.Text + "_";
            }

            string fileName;
            if( btnOffice.SelectedValue.Equals("*"))
                fileName = "TWImport_" + _clientInfo.ClientName.Replace("'", "_").Replace(".", "_").Replace(" ", "_").Replace(",", "_").ToUpper() + deptName + invoice.WeekEndDate.ToString("MMddyyyy") + ".csv";
            else
                fileName = btnOffice.SelectedValue.Replace(" ", "_") + "_TWImport_" +  _clientInfo.ClientName.Replace("'", "_").Replace(".", "_").Replace(" ", "_").Replace(",", "_").ToUpper() + deptName + invoice.WeekEndDate.ToString("MMddyyyy") + ".csv";
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
        protected void rptrTicketTrackerException_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            DateTime currentTime = _helper.GetCSTCurrentDateTime();
            // This event is raised for the header, the footer, separators, and items.
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EmployeeTrackerException employee = (EmployeeTrackerException)e.Item.DataItem;

                //set the item index
                Label lblItem = (Label)e.Item.FindControl("lblItem");
                lblItem.Text = (e.Item.ItemIndex + 1).ToString() + ". ";
                Label lblSwipeDateTime = (Label)e.Item.FindControl("lblSwipeDateTime");
                lblSwipeDateTime.Text = employee.PunchDateTime.ToString("MM/dd/yyyy hh:mm ss tt");
                Label lblExceptionMessage = (Label)e.Item.FindControl("lblExceptionMessage");
                lblExceptionMessage.Text = employee.PunchExceptionInfo.PunchExceptionMessage;
            }
        }
    }
}