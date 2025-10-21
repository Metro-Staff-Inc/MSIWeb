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
using MSI.Web.MSINet.DataAccess;
using MSI.Web.MSINet.Common;

namespace MSI.Web.Controls
{
    public partial class MSINetAdministrative : BaseMSINetControl
    {
        private ClientPreferences _clientPrefs;
        AdministrativeBL administrativeBL = new AdministrativeBL();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if( ClientPrefs == null )
                ClientPrefs = (ClientPreferences)Session["ClientPrefs"];

            if (IsPostBack)
            {
                return;
            }
            if (ClientPrefs != null)
            {
                if (ClientPrefs.DisplayJobCode)
                {
                    this.djcYes.Checked = true;
                    this.djcNo.Checked = false;
                }
                else
                {
                    this.djcNo.Checked = true;
                    this.djcYes.Checked = false;
                }
                if (ClientPrefs.DisplayPayRate)
                {
                    this.dprYes.Checked = true;
                    this.dprNo.Checked = false;
                }
                else
                {
                    this.dprNo.Checked = true;
                    this.dprYes.Checked = false;
                }
                if (ClientPrefs.NotifyHoursReady)
                {
                    this.nhrYes.Checked = true;
                    this.nhrNo.Checked = false;
                }
                else
                {
                    this.nhrNo.Checked = true;
                    this.nhrYes.Checked = false;
                }

                if (ClientPrefs.DisplayInvoice)
                {
                    this.giYes.Checked = true;
                    this.giNo.Checked = false;
                }
                else
                {
                    this.giNo.Checked = true;
                    this.giYes.Checked = false;
                }
                if (ClientPrefs.ApproveHours)
                {
                    this.ahYes.Checked = true;
                    this.ahNo.Checked = false;
                }
                else
                {
                    this.ahYes.Checked = false;
                    this.ahNo.Checked = true;
                }
                if (ClientPrefs.DisplaySchedule)
                {
                    this.dssYes.Checked = true;
                    this.dssNo.Checked = false;
                }
                else
                {
                    this.dssYes.Checked = false;
                    this.dssNo.Checked = true;
                }
                if (ClientPrefs.EnablePunchReporting)
                {
                    this.eprYes.Checked = true;
                    this.eprNo.Checked = false;
                }
                else
                {
                    this.eprYes.Checked = false;
                    this.eprNo.Checked = true;
                }
                if (ClientPrefs.EmployeeHistoryExactPunchTimes)
                {
                    this.septYes.Checked = true;
                    this.septNo.Checked = false;
                }
                else
                {
                    this.septYes.Checked = false;
                    this.septNo.Checked = true;
                }
                if (ClientPrefs.TicketTrackingExactLatePunches)
                {
                    this.ttelpYes.Checked = true;
                    this.ttelpNo.Checked = false;
                }
                else
                {
                    this.ttelpYes.Checked = false;
                    this.ttelpNo.Checked = true;
                }
                if (ClientPrefs.DisplayWeeklyReportsSundayToSaturday)
                {
                    this.dwrYes.Checked = true;
                    this.dwrNo.Checked = false;
                }
                else
                {
                    this.dwrYes.Checked = false;
                    this.dwrNo.Checked = true;
                }
                if (ClientPrefs.DisplayWeeklyReportsSaturdayToFriday)
                {
                    this.dwrSFYes.Checked = true;
                    this.dwrSFNo.Checked = false;
                }
                else
                {
                    this.dwrSFYes.Checked = false;
                    this.dwrSFNo.Checked = true;
                }
                if (ClientPrefs.DisplayWeeklyReportsWednesdayToTuesday)
                {
                    this.dwrWTYes.Checked = true;
                    this.dwrWTNo.Checked = false;
                }
                else
                {
                    this.dwrWTYes.Checked = false;
                    this.dwrWTNo.Checked = true;
                }
                if (ClientPrefs.DisplayWeeklyReportsFridayToThursday)
                {
                    this.dwrFTYes.Checked = true;
                    this.dwrFTNo.Checked = false;
                }
                else
                {
                    this.dwrFTYes.Checked = false;
                    this.dwrFTNo.Checked = true;
                }
                if (ClientPrefs.DisplayBonuses)
                {
                    this.dbonNo.Checked = false;
                    this.dbonYes.Checked = true;
                }
                else
                {
                    this.dbonNo.Checked = true;
                    this.dbonYes.Checked = false;
                }
                if (ClientPrefs.DisplayPayRateMaintenance)
                {
                    this.prmlNo.Checked = false;
                    this.prmlYes.Checked = true;
                }
                else
                {
                    this.prmlNo.Checked = true;
                    this.prmlYes.Checked = false;
                }
                if (ClientPrefs.DisplayTemps)
                {
                    this.dTempsNo.Checked = false;
                    this.dTempsYes.Checked = true;
                }
                else
                {
                    this.dTempsNo.Checked = true;
                    this.dTempsYes.Checked = false;
                }
                if (ClientPrefs.DisplayStartDate)
                {
                    this.dsdNo.Checked = false;
                    this.dsdYes.Checked = true;
                }
                else
                {
                    this.dsdNo.Checked = true;
                    this.dsdYes.Checked = false;
                }
                if (ClientPrefs.DisplayBreakTimes)
                {
                    this.dbtNo.Checked = false;
                    this.dbtYes.Checked = true;
                }
                else
                {
                    this.dbtNo.Checked = true;
                    this.dbtYes.Checked = false;
                }
                if (ClientPrefs.UseExactTimes)
                {
                    this.deptNo.Checked = false;
                    this.deptYes.Checked = true;
                }
                else
                {
                    this.deptNo.Checked = true;
                    this.deptYes.Checked = false;
                }
                if (ClientPrefs.RosterBasedPayRates)
                {
                    this.rbprNo.Checked = false;
                    this.rbprYes.Checked = true;
                }
                else
                {
                    this.rbprNo.Checked = true;
                    this.rbprYes.Checked = false;
                }
                if (ClientPrefs.ShowLocationsHoursReport)
                {
                    this.slhrNo.Checked = false;
                    this.slhrYes.Checked = true;
                }
                else
                {
                    this.slhrNo.Checked = true;
                    this.slhrYes.Checked = false;
                }
            }
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            Boolean result = administrativeBL.SetClientPreferences(ClientPrefs.ClientID, this.djcYes.Checked == true, this.dprYes.Checked == true,
                                                    this.nhrYes.Checked == true, this.giYes.Checked == true, this.ahYes.Checked == true, 
                                                    this.dssYes.Checked == true, this.eprYes.Checked == true, 
                                                    this.septYes.Checked == true, this.ttelpYes.Checked == true, 
                                                    this.dwrYes.Checked == true, this.dbonYes.Checked == true, this.prmlYes.Checked == true,
                                                    this.dwrWTYes.Checked == true, this.dTempsYes.Checked == true, this.dsdYes.Checked == true,
                                                    this.dbtYes.Checked == true, this.deptYes.Checked == true, this.rbprYes.Checked == true,
                                                    this.slhrYes.Checked == true, this.dwrSFYes.Checked == true, this.dwrFTYes.Checked == true);

            ClientPrefs.DisplayInvoice = this.giYes.Checked;
            ClientPrefs.DisplayPayRate = this.dprYes.Checked;
            ClientPrefs.DisplayJobCode = this.djcYes.Checked;
            ClientPrefs.NotifyHoursReady = this.nhrYes.Checked;
            ClientPrefs.ApproveHours = this.ahYes.Checked;
            ClientPrefs.DisplaySchedule = this.dssYes.Checked;
            ClientPrefs.EnablePunchReporting = this.eprYes.Checked;
            ClientPrefs.TicketTrackingExactLatePunches = this.ttelpYes.Checked;
            ClientPrefs.DisplayWeeklyReportsSundayToSaturday = this.dwrYes.Checked;
            ClientPrefs.EmployeeHistoryExactPunchTimes = this.septYes.Checked;
            ClientPrefs.DisplayBonuses = this.dbonYes.Checked;
            ClientPrefs.DisplayPayRateMaintenance = this.prmlYes.Checked;
            ClientPrefs.DisplayWeeklyReportsWednesdayToTuesday = this.dwrWTYes.Checked;
            ClientPrefs.DisplayTemps = this.dTempsYes.Checked;
            ClientPrefs.DisplayStartDate = this.dsdYes.Checked;
            ClientPrefs.DisplayBreakTimes = this.dbtYes.Checked;
            ClientPrefs.UseExactTimes = this.deptYes.Checked;
            ClientPrefs.RosterBasedPayRates = this.rbprYes.Checked;
            ClientPrefs.ShowLocationsHoursReport = this.slhrYes.Checked;
            ClientPrefs.DisplayWeeklyReportsSaturdayToFriday = this.dwrSFYes.Checked;
            ClientPrefs.DisplayWeeklyReportsFridayToThursday = this.dwrFTYes.Checked;
            if (result)
            {
                this.lblPreferencesUpdated.Text = "Client Preferences Updated!";
            }
            else
            {
                this.lblPreferencesUpdated.Text = "Client Preferences Not Updated!";
            }
            //this.lblPreferencesUpdated.Visible = true;
            Session["ClientPrefs"] = ClientPrefs;
            Response.Redirect("~/auth/Administrative.aspx");
            //Response.Redirect(Request.RawUrl);
        }
    }
}