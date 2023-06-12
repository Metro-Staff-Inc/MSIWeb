using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;

using MSIToolkit.Logging;
//using MSIToolkit.Logging;

namespace MSI.Web.Controls
{
    public partial class MSINetDaysWorkedReport : BaseMSINetControl
    {
        public enum ExportType
        {
            None,
            Excel,
            Print
        }
        private DateTime _periodStart;
        private DateTime _firstPunch;
        private Client _clientInfo = new Client();
        private DaysWorkedItem _boundDaysWorkedItem = new DaysWorkedItem();
        HelperFunctions _helper = new HelperFunctions();
        private ExportType _exportType = ExportType.None;
        private DaysWorkedReport _daysworkedReport;
        private bool _hoursLoaded = false;
        private string _activate = "";
        private string _badgeNum = "";
        private bool _noLink = false;    
        public string BadgeNum
        {
            get
            {
                return _badgeNum;
            }
            set
            {
                _badgeNum = value;
            }
        }
        public DateTime FirstPunch
        {
            get
            {
                return _firstPunch;
            }
            set
            {
                _firstPunch = value;
            }
        }
        public string Activate
        {
            get
            {
                return _activate;
            }
            set
            {
                _activate = value;
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
        PerformanceLogger log = new PerformanceLogger("MSINetDaysWorkedReport");

        public void LoadDaysWorkedReport()
        {
            log.Info("LoadDaysWorkedReport", "Start");
            _hoursLoaded = true;
            string userId = Context.User.Identity.Name.ToLower();
            if (userId.Equals("melody") || userId.Equals("bethy") || userId.Equals("tapiaj") || userId.Equals("nowaks") ||
                userId.Equals("vargasa") || userId.Equals("christina") || userId.Equals("raya"))
                _noLink = true;
            DaysWorkedReport daysworkedReportInput = new DaysWorkedReport();
            daysworkedReportInput.MinDays = 1;// Convert.ToInt32(this.ddlDaysBack.SelectedValue);
            daysworkedReportInput.ClientID = _clientInfo.ClientID;

            string dtS = Request.QueryString["startDate"];
            if (dtS != null)
            {
                daysworkedReportInput.StartDateTime = Convert.ToDateTime(dtS);
            }
            else
            {
                daysworkedReportInput.StartDateTime = this.ctlStartDate.SelectedDate;
            }
            int hoursBack = Convert.ToInt32(ddlDaysBack.SelectedValue);
            string hoursBackS = Request.QueryString["daysBack"];
            if ( hoursBackS != null )
            {
                hoursBack = Convert.ToInt32(hoursBackS);
            }
            DateTime dt = DateTime.Now;
            TimeSpan ts = new TimeSpan(dt.Hour, dt.Minute, dt.Hour);
            ts.Add(new TimeSpan(1, 0, 0));
            if ( hoursBack == 0 )
            {
                daysworkedReportInput.LastDayWorked = dt.Subtract(ts); // daysworkedReportInput.StartDateTime;
            }
            else
            {
                ts = new TimeSpan(hoursBack, 0, 0);
                dt = dt.Subtract(ts);
                daysworkedReportInput.LastDayWorked = new DateTime(dt.Year, dt.Month, dt.Day, 0,0,0);
            }

            string dtE = Request.QueryString["endDate"];
            if (dtE != null)
            {
                daysworkedReportInput.EndDateTime = Convert.ToDateTime(dtE);
            }
            else
            {
                daysworkedReportInput.EndDateTime = this.ctlEndDate.SelectedDate;
            }

            DaysWorkedReportBL daysworkedReportBL = new DaysWorkedReportBL();
            log.Info("LoadDaysWorkedReport", "DaysWorkedReportBL created");
            _daysworkedReport = daysworkedReportBL.GetDaysWorkedReport(daysworkedReportInput, null/*, log*/);
            log.Info("LoadDaysWorkedReport", "_daysWorkedReport Filled in");
            this.rptrDaysWorkedReport.DataSource = _daysworkedReport.DaysWorkedCollection;
            this.rptrDaysWorkedReport.DataBind();

            //After databind - we have the JS ready...   
            /*
            if (!Page.IsClientScriptBlockRegistered("daysworkedReport_toggle"))
            {
                Page.RegisterClientScriptBlock("daysworkedReport_toggle", this.outputJS());
            }*/
        }

        private string outputJS()
        {
            if (_exportType == ExportType.None)
            {
                StringBuilder js = new StringBuilder();
                js.Append("<script type=\"text/javascript\">" + Environment.NewLine);
                js.Append("function ToggleDisplay(id)" + Environment.NewLine);
                js.Append("{" + Environment.NewLine);
                js.Append("var elem = document.getElementById('d' + id);" + Environment.NewLine);
                js.Append("var imgElem = document.getElementById('i' + id);" + Environment.NewLine);
                js.Append("if (elem)" + Environment.NewLine);
                js.Append("{" + Environment.NewLine);
                js.Append("if (elem.style.display != 'block')" + Environment.NewLine);
                js.Append("{" + Environment.NewLine);
                js.Append("elem.style.display = 'block';" + Environment.NewLine);
                js.Append("elem.style.visibility = 'visible';" + Environment.NewLine);
                js.Append("imgElem.src = '../images/minus.gif';" + Environment.NewLine);
                js.Append("}" + Environment.NewLine);
                js.Append("else" + Environment.NewLine);
                js.Append("{" + Environment.NewLine);
                js.Append("elem.style.display = 'none';" + Environment.NewLine);
                js.Append("elem.style.visibility = 'hidden';" + Environment.NewLine);
                js.Append("imgElem.src = '../images/plus.gif';" + Environment.NewLine);
                js.Append("}" + Environment.NewLine);
                js.Append("}" + Environment.NewLine);
                js.Append("}" + Environment.NewLine);
                js.Append("</script>" + Environment.NewLine);
                return js.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetStartDate()
        {
            return Server.UrlEncode(this.ctlStartDate.SelectedDate.ToString("MM-dd-yyyy"));
        } 

        public string GetEndDate()
        {
            return Server.UrlEncode(this.ctlEndDate.SelectedDate.ToString("MM-dd-yyyy"));
            //return Server.UrlEncode(DateTime.Now.ToString("MM-dd-yyyy"));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _hoursLoaded = false;

            /* if 'finished' then data on Request is old. set activate flag and badge number to null */
            if (!_activate.Equals("finished"))
            {
                _activate = Request.QueryString["activate"];
                _badgeNum = Request.QueryString["employeeId"];
            }
            else
            {
                _activate = null;
                _badgeNum = null;
            }
            if (_badgeNum != null)
            {
                DaysWorkedReportBL daysworkedReportBL = new DaysWorkedReportBL();
                int tmp = daysworkedReportBL.ClientDNR_CheckForExistingRecords(_clientId, _badgeNum);
                if (tmp == 0 && _activate.Equals("deactivate"))
                {
                    daysworkedReportBL.ClientDNR_DeactivateEmployee(_clientId, _badgeNum);
                    /* remove from any current rosters */
                    ClientBL clientBL = new ClientBL();
                    String employeeId = "";
                    Char[] badge = _badgeNum.ToCharArray();
                    for( int i=0; i<badge.Length; i++ )
                        if( badge[i] >= '0' && badge[i] <= '9' )
                            employeeId += badge[i];
                    clientBL.DnrTrim( employeeId, Convert.ToString(_clientId), "0");
                }
                else if (tmp == 1 && _activate.Equals("activate"))
                {
                    daysworkedReportBL.ClientDNR_ActivateEmployee(_clientId, _badgeNum);
                }
                else
                {
                    _activate = null;
                    _badgeNum = null;
                }
            }

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
                this.ctlEndDate.SelectedDate = DateTime.Parse(_helper.GetCSTCurrentDateTime().ToString("MM/dd/yyyy") + " 00:00:00");
                this.ctlStartDate.SelectedDate = DateTime.Parse(_helper.GetCSTCurrentDateTime().ToString("MM/dd/yyyy") + " 00:00:00");

                int year = this.ctlStartDate.SelectedDate.Year;

                if (this.ctlStartDate.SelectedDate.Month >= 13)
                {
                    this.ctlStartDate.SelectedDate = new DateTime(year, 10, 1);
                }
                else
                {                 
                    this.ctlStartDate.SelectedDate = new DateTime(year-1, 11, 1);                
                }
                //this.ctlStartDate.SelectedDate = this.ctlStartDate.SelectedDate.AddDays(-120);
            }
            if (this.ctlStartDate.SelectedDate == null || this.ctlStartDate.SelectedDate.Date == new DateTime(1, 1, 1).Date)
            {
                //get the week ending date
                this.ctlStartDate.SelectedDate = DateTime.Parse(_helper.GetCSTCurrentWeekEndingDate().ToString("MM/dd/yyyy") + " 00:00:00");
            }

            if (_exportType != ExportType.None || _activate != null)
            {
                this.EnableViewState = false;
                string startDateTime = Server.UrlDecode((string)Request.QueryString["startdate"]);
                string endDateTime = Server.UrlDecode((string)Request.QueryString["enddate"]);

                this.ctlStartDate.SelectedDate = DateTime.Parse(startDateTime);
                this.ctlEndDate.SelectedDate = DateTime.Parse(endDateTime);
                this.LoadDaysWorkedReport();
                if (_activate == null)
                    this.pnlHeader.Visible = false;
                /* clear value */
                _activate = "finished";
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.lnkExport.Visible = false;

            this.lnkExport.NavigateUrl = "~/auth/daysworkedReportExcel.aspx?detail=0&startDate=" + this.GetStartDate() + 
                    "&daysBack="+ Convert.ToInt32(ddlDaysBack.SelectedValue) + "&endDate=" + this.GetEndDate();
            if (this.ClientInfo.ClientID == 121 || this.ClientInfo.ClientID == 186 
                || this._clientInfo.ClientID == 158 || this._clientInfo.ClientID == 166 
                || this._clientInfo.ClientID == 206 || this._clientInfo.ClientID == 92 
                || this._clientInfo.ClientID == 181 || this._clientInfo.ClientID == 226 
                || this._clientInfo.ClientID == 211 || this._clientInfo.ClientID == 30 
                || this._clientInfo.ClientID == 229 || this._clientInfo.ClientID == 243 
                || this._clientInfo.ClientID == 237 || this._clientInfo.ClientID == 340)
            {
                if (_hoursLoaded)
                {
                    if (_exportType == ExportType.None)
                    {
                        this.lnkExport.Visible = true;
                    }
                }
            }
            else
            {
                this.lnkExport.Visible = true;
            }
        }
        
        protected void rptrDaysWorkedReport_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundDaysWorkedItem = (DaysWorkedItem)e.Item.DataItem;
                int daysWorked = Convert.ToInt32(_boundDaysWorkedItem.TotalDaysWorked);
                //this._exportType.
                ((Label)e.Item.FindControl("lblStartDate")).Text = _boundDaysWorkedItem.FirstPunch;
                ((Label)e.Item.FindControl("lblEndDate")).Text = _boundDaysWorkedItem.LastPunch;
                ((Label)e.Item.FindControl("lblBadgeNumber")).Text = Convert.ToString(_boundDaysWorkedItem.BadgeNumber);
                ((Label)e.Item.FindControl("lblFirstName")).Text = Convert.ToString(_boundDaysWorkedItem.FirstName);
                ((Label)e.Item.FindControl("lblLastName")).Text = Convert.ToString(_boundDaysWorkedItem.LastName);
                ((Label)e.Item.FindControl("lblDaysWorked")).Text = Convert.ToString(_boundDaysWorkedItem.TotalDaysWorked);
                ((Label)e.Item.FindControl("lblPunchCount")).Text = Convert.ToString(_boundDaysWorkedItem.PunchCount);
                ((Label)e.Item.FindControl("lblBreakEnd")).Text = _boundDaysWorkedItem.EndOfBreak;
                ((Label)e.Item.FindControl("lblDaysWorked")).ToolTip = "Total Punches: " + _boundDaysWorkedItem.PunchCount;
                ((Label)e.Item.FindControl("lblShift")).Text = _boundDaysWorkedItem.Shift;

                if (_boundDaysWorkedItem.Depts.Count <= 1 /*|| this._exportType != ExportType.None*/)
                {
                    ((DropDownList)e.Item.FindControl("ddlDepartment")).Visible = false;
                    if( _boundDaysWorkedItem.Depts.Count == 1 )
                    {
                        ((Label)e.Item.FindControl("lblDepartment")).Text = Convert.ToString(_boundDaysWorkedItem.Depts[0]);
                    }
                    else
                    {
                        for (int i = 0; i < _boundDaysWorkedItem.Depts.Count; i++)
                        {
                            ((Label)e.Item.FindControl("lblDepartment")).Text += _boundDaysWorkedItem.Depts[i] + " - " +
                                    _boundDaysWorkedItem.DaysWorked[i];

                            if( _boundDaysWorkedItem.DaysWorked[i] == 1 )
                                    ((Label)e.Item.FindControl("lblDepartment")).Text += " day";
                            else
                                ((Label)e.Item.FindControl("lblDepartment")).Text += " days";

                            if (i < _boundDaysWorkedItem.Depts.Count - 1)
                                ((Label)e.Item.FindControl("lblDepartment")).Text += "<br/>";
                        }
                    }
                }
                else
                {
                    ((Label)e.Item.FindControl("lblDepartment")).Visible = false;
                    for (int i = 0; i < _boundDaysWorkedItem.DaysWorked.Count; i++ )
                    {
                        ((DropDownList)e.Item.FindControl("ddlDepartment")).Items.Add(_boundDaysWorkedItem.Depts[i] + " - " + 
                                    _boundDaysWorkedItem.DaysWorked[i] );
                        if (_boundDaysWorkedItem.DaysWorked[i] == 1)
                            ((Label)e.Item.FindControl("lblDepartment")).Text += " day";
                        else
                            ((Label)e.Item.FindControl("lblDepartment")).Text += " days";
                    }
                }

                ((HyperLink)e.Item.FindControl("lbtnDnrReason")).Text = Convert.ToString(_boundDaysWorkedItem.DnrReason);
                ((Label)e.Item.FindControl("lblDnrReason")).Text = Convert.ToString(_boundDaysWorkedItem.DnrReason);
                if (_noLink == false)
                {
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).ToolTip = "Click to DEACTIVATE";
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).NavigateUrl = "~/auth/DaysWorkedReport.aspx?employeeId=" +
                                                                                   ((Label)e.Item.FindControl("lblBadgeNumber")).Text +
                                                                                   "&startdate=" + this.ctlStartDate.SelectedDate.ToString("MM/dd/yyyy") +
                                                                                   "&enddate=" + this.ctlEndDate.SelectedDate.ToString("MM/dd/yyyy")/*DateTime.Now.ToString("MM/dd/yyyy")*/ +
                                                                                   "&activate=";
                    ((Label)e.Item.FindControl("lblDnrReason")).Visible = false;
                }
                else
                {
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).Visible = false;
                }

                if (!_boundDaysWorkedItem.DnrReason.ToUpper().Equals("ACTIVE"))
                {
                    //((HtmlTableRow)e.Item.FindControl("trDaysWorkedInfo")).Visible = false;
                    ((Label)e.Item.FindControl("lblBadgeNumber")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblFirstName")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblLastName")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblDaysWorked")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).ToolTip = "Click to ACTIVATE";
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).NavigateUrl += "activate";
                    ((Label)e.Item.FindControl("lblDepartment")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblDnrReason")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblStartDate")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                    ((Label)e.Item.FindControl("lblEndDate")).ForeColor = System.Drawing.Color.FromArgb(210, 0, 0);
                }
                else
                {
                    ((HyperLink)e.Item.FindControl("lbtnDnrReason")).NavigateUrl += "deactivate";
                }
                if (_boundDaysWorkedItem.FirstName.Contains("(W)") || (_boundDaysWorkedItem.FirstName.Contains("( W )") || _boundDaysWorkedItem.FirstName.Contains(" W ") ))
                {
                    HtmlTableRow tr = ((HtmlTableRow)e.Item.FindControl("empRow"));
                    if (tr != null)
                    {
                        tr.Attributes.Add("hire", "true");
                    }
                }
                if( !_boundDaysWorkedItem.DnrReason.ToUpper().StartsWith("ACTIVE") )
                {
                    HtmlTableRow tr = ((HtmlTableRow)e.Item.FindControl("empRow"));//
                    if (tr != null)
                    {
                        tr.Attributes.Add("ACTIVE", "false");
                    }
                }
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlTableRow trExcelTitle = (HtmlTableRow)e.Item.FindControl("trExcelTitle");
                HtmlTableRow trExcelDate = (HtmlTableRow)e.Item.FindControl("trExcelDate");
                if (_exportType != ExportType.None)
                {
                    trExcelTitle.Visible = true;
                    trExcelDate.Visible = true;

                    ((Label)e.Item.FindControl("lblExcelDate")).Text = "Start Date: " + this.ctlStartDate.SelectedDate.ToString("MM/dd/yyyy");

                    //if (this.ctlEndDate.SelectedDate != null && this.ctlEndDate.SelectedDate.Date != new DateTime(1, 1, 1).Date)
                    //{
                        ((Label)e.Item.FindControl("lblExcelDate")).Text = ((Label)e.Item.FindControl("lblExcelDate")).Text + " - End Date: " + this.ctlEndDate.SelectedDate.ToString("MM/dd/yyyy") /* DateTime.Now.ToString("MM/dd/yyyy") */;
                    //}
                }
                else
                {
                    trExcelTitle.Visible = false;
                    trExcelDate.Visible = false;
                    //((Label)e.Item.FindControl("lblDaysWorked")).
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //set the shift type
            LoadDaysWorkedReport();
        }
    }
}