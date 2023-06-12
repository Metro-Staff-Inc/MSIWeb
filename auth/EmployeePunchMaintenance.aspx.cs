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

namespace MSI.Web.MSINet
{
    public partial class EmployeePunchMaintenance : BaseMSINetPage
    {
        private string _badgeNumber = string.Empty;
        private string _employeeName = string.Empty;
        private int _clientRosterId;
        private string _modeTag = string.Empty;
        private int _departmentId;
        private int _locationId;
        private int _shiftType;

        private DateTime _startDate = new DateTime(1,1,1);
        private DateTime _endDate = new DateTime(1, 1, 1);

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            if (!this.IsPostBack)
            {
                if (Context.Items["modeTag"] != null)
                {
                    _modeTag = (string)Context.Items["modeTag"];
                    if (_modeTag == "employeeSummary")
                    {
                        this._badgeNumber = (string)Context.Items["badgeNumber"];
                        this._employeeName = (string)Context.Items["employeeName"];
                        this._clientRosterId = int.Parse(((string)Context.Items["clientRosterId"]));
                        this._startDate = DateTime.Parse(((string)Context.Items["startDate"]));
                        this._endDate = DateTime.Parse(((string)Context.Items["endDate"]));
                        this._departmentId = int.Parse(((string)Context.Items["departmentId"]));
                        this._shiftType = int.Parse(((string)Context.Items["shiftType"]));
                        this._locationId = int.Parse(((string)Context.Items["locationId"]));

                        this.ViewState.Add("modeTag", "employeeSummary");
                        this.ViewState.Add("badgeNumber", this._badgeNumber);
                        this.ViewState.Add("employeeName", this._employeeName);
                        this.ViewState.Add("clientRosterId", this._clientRosterId.ToString());
                        this.ViewState.Add("startDate", this._startDate.ToString("MM/dd/yyyy HH:mm:ss"));
                        this.ViewState.Add("endDate", this._endDate.ToString("MM/dd/yyyy HH:mm:ss"));
                        this.ViewState.Add("shiftType", this._shiftType.ToString());
                        this.ViewState.Add("departmentId", this._departmentId.ToString());
                        this.ViewState.Add("locationId", this._locationId.ToString());
                    }
                }
            }
            else
            {
                this._modeTag = (string)this.ViewState["modeTag"];
                this._badgeNumber = (string)this.ViewState["badgeNumber"];
                this._employeeName = (string)this.ViewState["employeeName"];
                this._clientRosterId = int.Parse(((string)this.ViewState["clientRosterId"]));
                this._startDate = DateTime.Parse(((string)this.ViewState["startDate"]));
                this._endDate = DateTime.Parse(((string)this.ViewState["endDate"]));
                this._shiftType = int.Parse(((string)this.ViewState["shiftType"]));
                this._departmentId = int.Parse(((string)this.ViewState["departmentId"]));
                this._locationId = int.Parse(((string)this.ViewState["locationId"]));
            }

            this.ctlEmployeePunchMaintenance.BadgeNumber = this._badgeNumber;
            this.ctlEmployeePunchMaintenance.EmployeeName = this._employeeName;
            this.ctlEmployeePunchMaintenance.ClientRosterID = this._clientRosterId;
            this.ctlEmployeePunchMaintenance.StartDate = this._startDate;
            this.ctlEmployeePunchMaintenance.EndDate = this._endDate;
            this.ctlEmployeePunchMaintenance.ClientInfo = _clientInfo;
            this.ctlEmployeePunchMaintenance.PunchExceptions = base._punchExceptions;
            //this.ctlEmployeePunchMaintenance.PunchMaintenanceReasons = base._punchMaintenanceReasons;

            if (!this.IsPostBack)
            {
                this.ctlEmployeePunchMaintenance.LoadEmployeePunches();
            }
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = false;

            if (Context.User.IsInRole("Administrator") || Context.User.IsInRole("Manager") || Context.User.IsInRole("ManagerNoPunchIn") ||
                Context.User.IsInRole("UpdatePunches"))
            {
                base._isAuthorized = true;
            }
            
            return base.IsAuthorizedAccess();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Context.Items.Add("modeTag", "employeeSummaryReturn");
            Context.Items.Add("shiftType", this._shiftType.ToString());
            Context.Items.Add("departmentId", this._departmentId.ToString());
            Context.Items.Add("locationId", this._locationId.ToString());
            Context.Items.Add("startDate", this._startDate.ToString("MM/dd/yyyy HH:mm"));

            Server.Transfer("~/auth/TicketSummary.aspx", false);
        }
}
}