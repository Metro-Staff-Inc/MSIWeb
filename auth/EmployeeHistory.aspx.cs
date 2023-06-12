using System;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet
{
    public partial class EmployeeHistory : BaseMSINetPage
    {
        private string _badgeNumber = string.Empty;
        private string _employeeName = string.Empty;
        private string _modeTag = string.Empty;
        private int _departmentId;
        private int _shiftType;
        private int _locationId;
        private DateTime _startDate = new DateTime(1,1,1);
        private DateTime _endDate = new DateTime(1,1,1);
        private DateTime _weekEndingDate = new DateTime(1, 1, 1);
        private object _test = null;
        private HelperFunctions _helper = new HelperFunctions();

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            this.ctlEmployeeHistory.ClientInfo = base._clientInfo;
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientPrefs = _clientPrefs;

            this.pnlReturnToTracking.Visible = false;

            if (!this.IsPostBack)
            {
                if (Context.Items["modeTag"] != null)
                {
                    _modeTag = (string)Context.Items["modeTag"];
                    if (_modeTag == "employeeHistory")
                    {
                        this._badgeNumber = (string)Context.Items["badgeNumber"];
                        this._startDate = DateTime.Parse(((string)Context.Items["startDate"]));
                        this._endDate = DateTime.Parse(((string)Context.Items["endDate"]));
                        this._departmentId = int.Parse(((string)Context.Items["departmentId"]));
                        this._locationId = int.Parse(((string)Context.Items["locationId"]));
                        this._shiftType = int.Parse(((string)Context.Items["shiftType"]));
                        this._weekEndingDate = DateTime.Parse(DateTime.Parse(Context.Items["weekEndDate"].ToString()).ToString("MM/dd/yyyy 00:00:00"));

                        this.ctlEmployeeHistory.BadgeNumber = this._badgeNumber;
                        this.ctlEmployeeHistory.WeekEndingDate = this._weekEndingDate;

                        this.ctlEmployeeHistory.LoadEmployeeHistory();
                        this.pnlReturnToTracking.Visible = true;
                    }
                }
                else
                {
                    this.ctlEmployeeHistory.WeekEndingDate = _helper.GetCSTCurrentWeekEndingDate();
                }
            }
            else
            {
                if (this.ViewState["modeTag"] != null)
                {
                    this._modeTag = (string)this.ViewState["modeTag"];
                    this._badgeNumber = (string)this.ViewState["badgeNumber"];
                    this._employeeName = (string)this.ViewState["employeeName"];
                    this._startDate = DateTime.Parse(((string)this.ViewState["startDate"]));
                    this._endDate = DateTime.Parse(((string)this.ViewState["endDate"]));
                    this._shiftType = int.Parse(((string)this.ViewState["shiftType"]));
                    this._departmentId = int.Parse(((string)this.ViewState["departmentId"]));
                    this._locationId = int.Parse(((string)this.ViewState["locationId"]));
                    this._weekEndingDate = DateTime.Parse(((string)this.ViewState["weekEndDate"]));

                    this.ctlEmployeeHistory.BadgeNumber = this._badgeNumber;
                    this.ctlEmployeeHistory.WeekEndingDate = this._weekEndingDate;

                    //this.ctlEmployeeHistory.LoadEmployeeHistory();
                    this.pnlReturnToTracking.Visible = true;
                }
            }

            this.ViewState.Add("modeTag", _modeTag);
            this.ViewState.Add("badgeNumber", this._badgeNumber);
            this.ViewState.Add("startDate", this._startDate.ToString("MM/dd/yyyy HH:mm:ss"));
            this.ViewState.Add("endDate", this._endDate.ToString("MM/dd/yyyy HH:mm:ss"));
            this.ViewState.Add("shiftType", this._shiftType.ToString());
            this.ViewState.Add("locationId", this._locationId.ToString());
            this.ViewState.Add("departmentId", this._departmentId.ToString());
            this.ViewState.Add("weekEndDate", this._weekEndingDate.ToString("MM/dd/yyyy"));
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
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            //log.Info("OnUnload");
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Context.Items.Add("modeTag", "employeeHistoryReturn");
            Context.Items.Add("shiftType", this._shiftType.ToString());
            Context.Items.Add("departmentId", this._departmentId.ToString());
            Context.Items.Add("locationId", this._locationId.ToString());
            Context.Items.Add("startDate", this._startDate.ToString("MM/dd/yyyy HH:mm"));

            Server.Transfer("~/auth/TicketSummary.aspx", false);
        }

    }
}