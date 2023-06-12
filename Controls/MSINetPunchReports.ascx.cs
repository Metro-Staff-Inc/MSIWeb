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
    public partial class MSINetPunchReports : BaseMSINetControl
    {
        private string _labelText = "Select a Date:";
        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
            }
        }
        public string LabelText
        {
            get
            {
                return _labelText;
            }
            set
            {
                _labelText = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cdrEndDate.Visible = false; //hiding the calendar.
            if (!this.IsPostBack)
            {
                DateTime dt = new DateTime(1, 1, 1);
                if (this.cdrEndDate.SelectedDate == null || this.cdrEndDate.SelectedDate.Equals(dt))
                {
                    this.cdrEndDate.SelectedDate = DateTime.Now;
                    this.txtDateTime.Text = this.cdrEndDate.SelectedDate.ToString("MM/dd/yyyy");
                }
                LoadPunchRecordCreators(sender, e);
                if( this.cboUserList.Items.Count == 0 )
                    this.btnGo.Enabled = false;
                else
                    this.btnGo.Enabled = true;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this._selectedDate != null && this._selectedDate.Date != new DateTime(1, 1, 1).Date)
            {
                this.txtDateTime.Text = this._selectedDate.ToString("MM/dd/yyyy");
            }
        }

        public void LoadPunchRecords()
        {
            PunchReport punchReport = new PunchReport();
            punchReport.EndDate = cdrEndDate.SelectedDate;
            punchReport.StartDate = cdrEndDate.SelectedDate.AddDays( 
                (-1) * int.Parse(ctlStartDate.Items[this.ctlStartDate.SelectedIndex].Value) );

            punchReport.ClientID = this._clientId;
            punchReport.UserID = this.cboUserList.Items[this.cboUserList.SelectedIndex].ToString();

            PunchReportBL punchReportBL = new PunchReportBL();
            punchReport = punchReportBL.GetPunchRecords(punchReport, Context.User);

            gvPunchRecord.DataSource = punchReport.PunchRecord;
            gvPunchRecord.DataBind();

            /***/
        }

        protected void LoadPunchRecordCreators(object sender, EventArgs e)
        {
            PunchReport punchReport = new PunchReport();
            punchReport.EndDate = this.cdrEndDate.SelectedDate;
            punchReport.StartDate = this.cdrEndDate.SelectedDate.AddDays( 
               (-1) * Int32.Parse(this.ctlStartDate.Items[this.ctlStartDate.SelectedIndex].Value));

            punchReport.ClientID = this._clientId;

            //DropDownList ddlUsers = (DropDownList)this.FindControl("cboUserList");
            if (this.cboUserList.Items.Count == 0)
                punchReport.UserID = "%";
            else
                punchReport.UserID = this.cboUserList.Items[this.cboUserList.SelectedIndex].ToString();

            PunchReportBL punchReportBL = new PunchReportBL();
            punchReport = punchReportBL.GetPunchRecordCreators(punchReport, Context.User);

            this.cboUserList.DataSource = punchReport.UserIdList;
            this.cboUserList.DataBind();
            if (this.cboUserList.Items.Count > 0)
                this.btnGo.Enabled = true;
            else
                this.btnGo.Enabled = false;
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDateTime.Text.Trim() != "")
                {
                    cdrEndDate.VisibleDate = DateTime.Parse(txtDateTime.Text);
                    this.SelectedDate = DateTime.Parse(txtDateTime.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cdrEndDate.Visible = true;
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
           // DateTime dt = this.ctlStartDate.SelectedDate;
            LoadPunchRecords();
        }

        protected void cdrEndDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            this.cdrEndDate.Visible = true;
        }

        protected void cdrEndDate_SelectionChanged(object sender, EventArgs e)
        {
            this.txtDateTime.Text = cdrEndDate.SelectedDate.ToString("MM/dd/yyyy");
            this._selectedDate = cdrEndDate.SelectedDate;
            cdrEndDate.Visible = false; //hiding the calendar.
            LoadPunchRecordCreators(sender, e);
        }
    }
}