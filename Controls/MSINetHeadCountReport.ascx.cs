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

namespace MSI.Web.Controls
{
    public partial class MSINetHeadCountReport : BaseMSINetControl
    {
        public enum ExportType
        {
            None,
            Excel,
            Print
        }

        private DateTime _periodStart;
        private DateTime _periodEnd;
        private Client _clientInfo = new Client();
        private EmployeeHistory _boundEmployeeHistory = new EmployeeHistory();
        HelperFunctions _helper = new HelperFunctions();
        private ExportType _exportType = ExportType.None;
        private int _currentDepartmentId = 0;
        private int _currentShiftType = 0;
        private decimal[] _totDailyHours = new decimal[9];
        private decimal[] _totShiftDailyHours = new decimal[9];
        private decimal[] _grandTotDailyHours = new decimal[7];
        private decimal[] _productionTotDailyHours = new decimal[7];
        private decimal[] _packagingTotDailyHours = new decimal[7];
        private decimal[] _indirectTotDailyHours = new decimal[7];
        private bool _exportDetail = false;
        private HeadCountReport _headCountReport;
        private int _boundEmployeeIdx = 0;
        private bool _showApproved = false;
        private bool _allowElectronicApproval = true;
        private bool _headCountLoaded = false;
        private bool _currentOnly = false;

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
        public bool CurrentOnly
        {
            get
            {
                return _currentOnly;
            }
            set
            {
                _currentOnly = value;
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

        public void LoadHeadCountReport()
        {
            _headCountLoaded = true;
            HeadCountReport headCountReportInput = new HeadCountReport();
            headCountReportInput.ClientID = _clientInfo.ClientID;
            headCountReportInput.RosterEmployeeFlag = true;
            headCountReportInput.EndDateTime = this.ctlWeekEnding.SelectedDate;
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);
            headCountReportInput.StartDateTime = headCountReportInput.EndDateTime.Subtract(ts);
            headCountReportInput.EndDateTime = DateTime.Parse(this.ctlWeekEnding.SelectedDate.ToString("MM/dd/yyyy") + " 23:59:59");
            HeadCountReportBL headCountReportBL = new HeadCountReportBL();
            _headCountReport = headCountReportBL.GetHeadCountReport(headCountReportInput, Context.User);
            this.rptrHeadCountReport.DataSource = _headCountReport.EmployeeHistoryCollection;
            this.rptrHeadCountReport.DataBind();
        }

       
        public string GetSelectedDate()
        {
            return Server.UrlEncode(this.ctlWeekEnding.SelectedDate.ToString("MM-dd-yyyy"));
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            _headCountLoaded = false;

            this.pnlTotals.Visible = false;

            if (this.ClientInfo.ClientID == 256)
                this.btnGoCurrent.Visible = true;
            else
                this.btnGoCurrent.Visible = false;


            if (!this.IsPostBack)
            {
                if (ClientInfo.ClientID == 121)
                {
                    this.lnkExport.Visible = false;
                }
                else
                {
                    this.lnkExport.Visible = true;
                }
            }
            if (this.ctlWeekEnding.SelectedDate == null || this.ctlWeekEnding.SelectedDate.Date == new DateTime(1, 1, 1).Date)
            {
                //get the week ending date
                this.ctlWeekEnding.SelectedDate = DateTime.Parse(_helper.GetCSTCurrentWeekEndingDate().ToString("MM/dd/yyyy") + " 00:00:00");
                /* for clients ending their week on Saturday */
                if (this.ClientInfo.ClientID == 256 || this.ClientInfo.ClientID == 121 || this.ClientInfo.ClientID == 258 || this.ClientInfo.ClientID == 205)
                    this.ctlWeekEnding.SelectedDate = this.ctlWeekEnding.SelectedDate.AddDays(-1);
            }

            if (_exportType != ExportType.None)
            {
                this.EnableViewState = false;
                string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);
                string exportDetail = Server.UrlDecode((string)Request.QueryString["detail"]);
                if (exportDetail == "1")
                {
                    _exportDetail = true;
                }

                this.ctlWeekEnding.SelectedDate = DateTime.Parse(dateTime);
                this.LoadHeadCountReport();
                this.pnlHeader.Visible = false;
            }
     
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lnkExport.Visible = false;



            this.lnkExport.NavigateUrl = "~/auth/HeadCountReportExcel.aspx?detail=0&date=" + this.GetSelectedDate();

            if (this.ClientInfo.ClientID == 121 || this.ClientInfo.ClientID == 186 
                || this._clientInfo.ClientID == 158 || this._clientInfo.ClientID == 166 
                || this._clientInfo.ClientID == 206 || this._clientInfo.ClientID == 92 
                || this._clientInfo.ClientID == 181 || this._clientInfo.ClientID == 226 
                || this._clientInfo.ClientID == 211 || this._clientInfo.ClientID == 30 
                || this._clientInfo.ClientID == 229 || this._clientInfo.ClientID == 243 
                || this._clientInfo.ClientID == 237 || this._clientInfo.ClientID == 245
                || this._clientInfo.ClientID == 340)
            {
                if (_headCountLoaded)
                {
                    if (_exportType == ExportType.None)
                    {
                        if (this._clientInfo.ClientID == 92 || this._clientInfo.ClientID == 181 || this._clientInfo.ClientID == 226)
                        {
                            this.lnkExport.Visible = true;
                        }
                        else if (_allowElectronicApproval && _headCountReport != null && _headCountReport.EmployeeHistoryCollection.Count > 0)
                        {
                            this.lnkExport.Visible = true;
                        }
                    }
                }
            }
            else
            {
                this.lnkExport.Visible = true;
            }

        }

        protected void rptrHeadCountReport_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundEmployeeIdx++;
                _boundEmployeeHistory = (EmployeeHistory)e.Item.DataItem;

                if (_boundEmployeeHistory.WorkSummaries.Count > 0)
                {

                    // put the employee count into the panel/label
                    ((Label)e.Item.FindControl("lblEmpCnt")).Text = Convert.ToString(_boundEmployeeIdx);

                    if (_clientId == 2)
                    {
                        ((Panel)e.Item.FindControl("pnlPrimaryJob")).Visible = true;
                        ((Label)e.Item.FindControl("lblPrimaryJob")).Text = _boundEmployeeHistory.PrimaryRole;
                        ((HtmlTableCell)e.Item.FindControl("tdDeptSpacer")).Visible = true;
                    }
                    //see if we need to display jobcode
                    if (_clientId == 226)
                    {
                        ((Panel)e.Item.FindControl("pnlJobCode")).Visible = true;
                        ((Label)e.Item.FindControl("lblJobCode")).Text = _boundEmployeeHistory.GetEmployeeJobCode();
                        //spacer to make the total lines display ok
                        ((HtmlTableCell)e.Item.FindControl("tdShiftSpacer")).Visible = true;
                    }
                    if (_clientId == 256) //256 = CWInhouse, they want CW instead of TE or TA...
                    {
                        ((Label)e.Item.FindControl("lblBadgeNumber")).Text = "CW" + ((Label)e.Item.FindControl("lblBadgeNumber")).Text.Substring(2);
                        ///((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text = "CW" + ((Label)e.Item.FindControl("lblBadgeNumberExcel")).Text.Substring(2);
                    }
                    //set the color of the labels if there are any invalid summaries
                    if (_boundEmployeeHistory.HasInvalidWorkSummaries)
                    {
                        ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.Red;
                        ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.Red;
                    }

                    EmployeeWorkSummary summary = (EmployeeWorkSummary)_boundEmployeeHistory.WorkSummaries[0];
                    HtmlTableRow trDepartment = (HtmlTableRow)e.Item.FindControl("trDepartment");
                    HtmlTableCell tdDepartmentHead = (HtmlTableCell)e.Item.FindControl("tdDepartmentHead");
                    HtmlTableRow trDepartmentTotals = (HtmlTableRow)e.Item.FindControl("trDepartmentTotals");
                    HtmlTableCell tdDepartmentTotalLabel = (HtmlTableCell)e.Item.FindControl("tdDepartmentTotalLabel");
                    if (summary.DepartmentInfo.DepartmentID != _currentDepartmentId || summary.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                    {
                        
                        //show the department break;
                        if (summary.DepartmentInfo.DepartmentID != _currentDepartmentId || summary.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                        {
                            trDepartment.Visible = true;
                            if (_clientId == 92 || _clientId == 181 || _clientId == 340 || _clientId == 226 )
                            {
                                tdDepartmentHead.ColSpan = 12;
                            }

                                ((Label)e.Item.FindControl("lblDepartment")).Text = summary.DepartmentInfo.DepartmentName + " - " + summary.ShiftTypeInfo.ToString();

                            _currentDepartmentId = summary.DepartmentInfo.DepartmentID;
                            _currentShiftType = summary.ShiftTypeInfo.ShiftTypeId;
                        }
                    }
                    else
                    {
                        trDepartment.Visible = false;
                    }

                    //calculate total hours
                    this.CalculateTotalHours(summary.DepartmentInfo.DepartmentName.ToUpper());

                    if (!_exportDetail)
                    {
                        if (this.ctlWeekEnding.SelectedDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                        {
                            displayHours(((Label)e.Item.FindControl("lblWeekDay1Hours")), _boundEmployeeHistory.MondaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay2Hours")), _boundEmployeeHistory.TuesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay3Hours")), _boundEmployeeHistory.WednesdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay4Hours")), _boundEmployeeHistory.ThursdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay5Hours")), _boundEmployeeHistory.FridaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay6Hours")), _boundEmployeeHistory.SaturdaySummary);
                            displayHours(((Label)e.Item.FindControl("lblWeekDay7Hours")), _boundEmployeeHistory.SundaySummary);
                        }
                        else
                        {
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

                    //check if we should show totals
                    EmployeeHistory nextEmployee = null;
                    EmployeeWorkSummary nextSummary = null;
                    if (e.Item.ItemIndex < _headCountReport.EmployeeHistoryCollection.Count - 1)
                    {
                        nextEmployee = (EmployeeHistory)_headCountReport.EmployeeHistoryCollection[e.Item.ItemIndex + 1];

                        if (nextEmployee.WorkSummaries.Count > 0)
                        {
                            nextSummary = (EmployeeWorkSummary)nextEmployee.WorkSummaries[0];
                        }
                        else
                        {
                            nextSummary = null;
                        }
                    }

                    if (nextSummary == null || (e.Item.ItemIndex == _headCountReport.EmployeeHistoryCollection.Count - 1) || (nextSummary.DepartmentInfo.DepartmentID != summary.DepartmentInfo.DepartmentID || nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId))
                    {
                        //always display department in this case
                        trDepartmentTotals.Visible = true;
                        if (_clientId == 92 || _clientId == 181 || _clientId == 340 || _clientId == 226 )
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

                        if (nextSummary == null || (e.Item.ItemIndex == _headCountReport.EmployeeHistoryCollection.Count - 1) || (nextSummary.ShiftTypeInfo.ShiftTypeId != summary.ShiftTypeInfo.ShiftTypeId))
                        {

                        }

                        if (!_exportDetail)
                        {
                            int offset = 0;
                            if (this.ctlWeekEnding.SelectedDate.DayOfWeek.Equals(DayOfWeek.Saturday))
                                offset = 6;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay1Hours")).Text = _totDailyHours[(0 + offset) % 7].ToString("N0");
                            _totDailyHours[(0 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay2Hours")).Text = _totDailyHours[(1 + offset) % 7].ToString("N0");
                            _totDailyHours[(1 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay3Hours")).Text = _totDailyHours[(2 + offset) % 7].ToString("N0");
                            _totDailyHours[(2 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay4Hours")).Text = _totDailyHours[(3 + offset) % 7].ToString("N0");
                            _totDailyHours[(3 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay5Hours")).Text = _totDailyHours[(4 + offset) % 7].ToString("N0");
                            _totDailyHours[(4 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay6Hours")).Text = _totDailyHours[(5 + offset) % 7].ToString("N0");
                            _totDailyHours[(5 + offset) % 7] = 0M;
                            ((Label)e.Item.FindControl("lblDeptTotWeekDay7Hours")).Text = _totDailyHours[(6 + offset) % 7].ToString("N0");
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
                        
                    }
                    else
                    {
                        trDepartmentTotals.Visible = false;
                    }

                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                if (_clientId == 226 )
                {
                    ((Panel)e.Item.FindControl("pnlJobCodeHead")).Visible = true;
                }
                if( _clientId == 2 )
                {
                    ((Panel)e.Item.FindControl("pnlPrimaryJobHead")).Visible = true;
                }

                HtmlTableRow trExcelTitle = (HtmlTableRow)e.Item.FindControl("trExcelTitle");
                HtmlTableRow trExcelWeekEnding = (HtmlTableRow)e.Item.FindControl("trExcelWeekEnding");
                if (_exportType != ExportType.None)
                {
                    trExcelTitle.Visible = true;
                    trExcelWeekEnding.Visible = true;
                    if (_clientId == 92 || _clientId == 181 || _clientId == 340 || _clientId == 226 )
                    {
                        ((HtmlTableCell)e.Item.FindControl("tdExcelWeekEnding")).ColSpan = 3;
                    }

                    ((Label)e.Item.FindControl("lblExcelWeekEnding")).Text = "Week Ending " + this.ctlWeekEnding.SelectedDate.ToString("MM/dd/yyyy");
                    ((Label)e.Item.FindControl("lblWeekDay1")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-6).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay2")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-5).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay3")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-4).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay4")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-3).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay5")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-2).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay6")).Text = this.ctlWeekEnding.SelectedDate.AddDays(-1).Date.ToString("M/d");
                    ((Label)e.Item.FindControl("lblWeekDay7")).Text = this.ctlWeekEnding.SelectedDate.Date.ToString("M/d");
                }
                else
                {
                    if (this.ctlWeekEnding.SelectedDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Mon";
                        ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Tue";
                        ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Wed";
                        ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Thr";
                        ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Fri";
                        ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Sat";
                        ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Sun";
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lblWeekDay2")).Text = "Mon";
                        ((Label)e.Item.FindControl("lblWeekDay3")).Text = "Tue";
                        ((Label)e.Item.FindControl("lblWeekDay4")).Text = "Wed";
                        ((Label)e.Item.FindControl("lblWeekDay5")).Text = "Thr";
                        ((Label)e.Item.FindControl("lblWeekDay6")).Text = "Fri";
                        ((Label)e.Item.FindControl("lblWeekDay7")).Text = "Sat";
                        ((Label)e.Item.FindControl("lblWeekDay1")).Text = "Sun";
                    }

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
                if( _clientId >= 325 && _clientId <= 327 )
                {
                    ((HtmlTableRow)e.Item.FindControl("productionTotals")).Visible = true;
                    ((HtmlTableRow)e.Item.FindControl("indirectTotals")).Visible = true;
                    ((HtmlTableRow)e.Item.FindControl("packagingTotals")).Visible = true;
                }
                if (!_exportDetail)
                {
                    //Mon
                    ((Label)e.Item.FindControl("lblGrandWeekDay1Hours")).Text = _grandTotDailyHours[0].ToString();
                    //Tue
                    ((Label)e.Item.FindControl("lblGrandWeekDay2Hours")).Text = _grandTotDailyHours[1].ToString();
                    //Wed
                    ((Label)e.Item.FindControl("lblGrandWeekDay3Hours")).Text = _grandTotDailyHours[2].ToString();
                    //Thu
                    ((Label)e.Item.FindControl("lblGrandWeekDay4Hours")).Text = _grandTotDailyHours[3].ToString();
                    //Fri
                    ((Label)e.Item.FindControl("lblGrandWeekDay5Hours")).Text = _grandTotDailyHours[4].ToString();
                    //Sat
                    ((Label)e.Item.FindControl("lblGrandWeekDay6Hours")).Text = _grandTotDailyHours[5].ToString();
                    //Sun
                    ((Label)e.Item.FindControl("lblGrandWeekDay7Hours")).Text = _grandTotDailyHours[6].ToString();

                    //Mon
                    ((Label)e.Item.FindControl("lblProductionWeekDay1Hours")).Text = _productionTotDailyHours[0].ToString();
                    //Tue
                    ((Label)e.Item.FindControl("lblProductionWeekDay2Hours")).Text = _productionTotDailyHours[1].ToString();
                    //Wed
                    ((Label)e.Item.FindControl("lblProductionWeekDay3Hours")).Text = _productionTotDailyHours[2].ToString();
                    //Thu
                    ((Label)e.Item.FindControl("lblProductionWeekDay4Hours")).Text = _productionTotDailyHours[3].ToString();
                    //Fri
                    ((Label)e.Item.FindControl("lblProductionWeekDay5Hours")).Text = _productionTotDailyHours[4].ToString();
                    //Sat
                    ((Label)e.Item.FindControl("lblProductionWeekDay6Hours")).Text = _productionTotDailyHours[5].ToString();
                    //Sun
                    ((Label)e.Item.FindControl("lblProductionWeekDay7Hours")).Text = _productionTotDailyHours[6].ToString();

                    //Mon
                    ((Label)e.Item.FindControl("lblIndirectWeekDay1Hours")).Text = _indirectTotDailyHours[0].ToString();
                    //Tue
                    ((Label)e.Item.FindControl("lblIndirectWeekDay2Hours")).Text = _indirectTotDailyHours[1].ToString();
                    //Wed
                    ((Label)e.Item.FindControl("lblIndirectWeekDay3Hours")).Text = _indirectTotDailyHours[2].ToString();
                    //Thu
                    ((Label)e.Item.FindControl("lblIndirectWeekDay4Hours")).Text = _indirectTotDailyHours[3].ToString();
                    //Fri
                    ((Label)e.Item.FindControl("lblIndirectWeekDay5Hours")).Text = _indirectTotDailyHours[4].ToString();
                    //Sat
                    ((Label)e.Item.FindControl("lblIndirectWeekDay6Hours")).Text = _indirectTotDailyHours[5].ToString();
                    //Sun
                    ((Label)e.Item.FindControl("lblIndirectWeekDay7Hours")).Text = _indirectTotDailyHours[6].ToString();

                    //Mon
                    ((Label)e.Item.FindControl("lblPackagingWeekDay1Hours")).Text = _packagingTotDailyHours[0].ToString();
                    //Tue
                    ((Label)e.Item.FindControl("lblPackagingWeekDay2Hours")).Text = _packagingTotDailyHours[1].ToString();
                    //Wed
                    ((Label)e.Item.FindControl("lblPackagingWeekDay3Hours")).Text = _packagingTotDailyHours[2].ToString();
                    //Thu
                    ((Label)e.Item.FindControl("lblPackagingWeekDay4Hours")).Text = _packagingTotDailyHours[3].ToString();
                    //Fri
                    ((Label)e.Item.FindControl("lblPackagingWeekDay5Hours")).Text = _packagingTotDailyHours[4].ToString();
                    //Sat
                    ((Label)e.Item.FindControl("lblPackagingWeekDay6Hours")).Text = _packagingTotDailyHours[5].ToString();
                    //Sun
                    ((Label)e.Item.FindControl("lblPackagingWeekDay7Hours")).Text = _packagingTotDailyHours[6].ToString();
                }
            }
        }

        protected int GetHeadCountValue(int numberOfPunches)
        {
            int mytemp = 0;
            if (!CurrentOnly)
            {

                if (numberOfPunches == 0)
                    mytemp = 0;
                if (numberOfPunches >= 2)
                    mytemp = 1;
                if (numberOfPunches % 2 == 1)
                    mytemp = 1;
            }
            else
            {
                mytemp = numberOfPunches % 2;
            }

                return mytemp;

                
        }


        protected string DisplayHeadCountValue(int numberOfPunches)
        {
            string mytemp = "";



            if (numberOfPunches == 0)
            {
                mytemp = "0";
            }
            else
            {
                if (numberOfPunches % 2 == 0 && !this.CurrentOnly)
                {
                    mytemp = "1";
                }
                else if (numberOfPunches % 2 == 0)
                {
                    mytemp = "0";
                }
                else
                {
                    mytemp = "C";
                }
            }
            if (mytemp.Length > 0)
                return mytemp;
            else
                return "0";
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
                currentEmployee = (EmployeeHistory)_headCountReport.EmployeeHistoryCollection[_boundEmployeeIdx];
            }

            if (currentEmployee.WorkSummaries.Count > 0)
            {
                EmployeeWorkSummary summary = (EmployeeWorkSummary)currentEmployee.WorkSummaries[0];
                return currentEmployee.EmployeeID.ToString() + "_" + summary.ShiftTypeInfo.ShiftTypeId.ToString() + "_" + summary.DepartmentInfo.DepartmentID.ToString();
            }
            return currentEmployee.EmployeeID.ToString();
        }

        protected void CalculateTotalHours( String departmentName )
        {
            ArrayList workSummaries = _boundEmployeeHistory.WorkSummaries;
            DateTime punchOut = new DateTime(1,1,1);
            DateTime roundedPunchOut = new DateTime(1, 1, 1);
            bool isAryzta = _clientId >= 325 && _clientId <= 327;

            if (departmentName.Contains("PRODUCTION") && _clientId >= 325 && _clientId <= 327)
            {
                _productionTotDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
                _productionTotDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
                _productionTotDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
                _productionTotDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
                _productionTotDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
                _productionTotDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
                _productionTotDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);
            }
            else if (departmentName.Contains("PACK") && _clientId >= 325 && _clientId <= 327)
            {
                _packagingTotDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
                _packagingTotDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
                _packagingTotDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
                _packagingTotDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
                _packagingTotDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
                _packagingTotDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
                _packagingTotDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);
            }
            else if( _clientId >= 325 && _clientId <= 327 )
            {
                _indirectTotDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
                _indirectTotDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
                _indirectTotDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
                _indirectTotDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
                _indirectTotDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
                _indirectTotDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
                _indirectTotDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);
            }
            //Monday
            _totDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
            _totShiftDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
            _grandTotDailyHours[0] += GetHeadCountValue(_boundEmployeeHistory.MondaySummary.NumberOfPunches);
            //Tuesday
            _totDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
            _totShiftDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
            _grandTotDailyHours[1] += GetHeadCountValue(_boundEmployeeHistory.TuesdaySummary.NumberOfPunches);
            //Wednesday
            _totDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
            _totShiftDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
            _grandTotDailyHours[2] += GetHeadCountValue(_boundEmployeeHistory.WednesdaySummary.NumberOfPunches);
            //Thursday
            _totDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
            _totShiftDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
            _grandTotDailyHours[3] += GetHeadCountValue(_boundEmployeeHistory.ThursdaySummary.NumberOfPunches);
            //Friday
            _totDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
            _totShiftDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
            _grandTotDailyHours[4] += GetHeadCountValue(_boundEmployeeHistory.FridaySummary.NumberOfPunches);
            //Saturday
            _totDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
            _totShiftDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
            _grandTotDailyHours[5] += GetHeadCountValue(_boundEmployeeHistory.SaturdaySummary.NumberOfPunches);
            //Sunday
            _totDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);
            _totShiftDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);
            _grandTotDailyHours[6] += GetHeadCountValue(_boundEmployeeHistory.SundaySummary.NumberOfPunches);

            _totDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totShiftDailyHours[7] += _boundEmployeeHistory.TotalRegularHours;
            _totDailyHours[8] += _boundEmployeeHistory.TotalOTHours;
            _totShiftDailyHours[8] += _boundEmployeeHistory.TotalOTHours;
            

            
        }

        protected void displayHours(Label lbl, DailySummary dailySummary)
        {
            if (DisplayHeadCountValue(dailySummary.NumberOfPunches) == "0"|| DisplayHeadCountValue(dailySummary.NumberOfPunches) == "C")
            {
                lbl.ForeColor = System.Drawing.Color.Red;
                //lbl.BorderStyle = BorderStyle.Solid;
                //lbl.BorderWidth = Unit.Pixel(1);
            }
            lbl.Text = DisplayHeadCountValue(dailySummary.NumberOfPunches);
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //set the shift type

            if (ClientInfo.ClientID == 121 || ClientInfo.ClientID == 186 
                || this._clientInfo.ClientID == 158 || this._clientInfo.ClientID == 166 
                || this._clientInfo.ClientID == 206 || this._clientInfo.ClientID == 92
                || this._clientInfo.ClientID == 181 || this._clientInfo.ClientID == 226 
                || this._clientInfo.ClientID == 211 || this._clientInfo.ClientID == 30 
                || this._clientInfo.ClientID == 229 || this._clientInfo.ClientID == 237 
                || this._clientInfo.ClientID == 243 || this._clientInfo.ClientID == 245
                || this._clientInfo.ClientID == 340)
            {
                this.lnkExport.Visible = false;
                _showApproved = false;
            }
            Button btn = (Button)sender;
            if (btn.Text.Equals("On Site"))    //temporary hack!!!JHM
                this.CurrentOnly = true;

            LoadHeadCountReport();
            this.pnlTotals.Visible = true;
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               