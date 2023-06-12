using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSI.Web.Controls
{
    public partial class MSINetDailyPunchesReport : BaseMSINetControl
    {
        //private DateTime _selectedDate;
        public DateTime SelectedDate { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            cdrEndDate.Visible = false; //hiding the calendar.
            if (!this.IsPostBack)
            {
                DateTime dt = new DateTime(1, 1, 1);
                if (cdrEndDate.SelectedDate == null || cdrEndDate.SelectedDate.Equals(dt))
                {
                    this.cdrEndDate.SelectedDate = DateTime.Now.AddDays(-1);
                    this.txtDateTime.Text = this.cdrEndDate.SelectedDate.ToString("MM/dd/yyyy");
                }
                btnGo.Enabled = true;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SelectedDate != null && SelectedDate.Date != new DateTime(1, 1, 1).Date)
            {
                this.txtDateTime.Text = SelectedDate.ToString("MM/dd/yyyy");
            }
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
        public void LoadDailyPunches()
        {
            PunchReport punchReport = new PunchReport();
            punchReport.EndDate = this.cdrEndDate.SelectedDate;
            punchReport.StartDate = this.cdrEndDate.SelectedDate;
            punchReport.ClientID = this._clientId;

            PunchReportBL punchReportBL = new PunchReportBL();
            ArrayList punches = punchReportBL.GetDailyPunches(punchReport);

            gvPunchRecord.DataSource = punches;
            gvPunchRecord.RowDataBound += GridView1_RowDataBound;
            gvPunchRecord.DataBind();

            /***/
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            LoadDailyPunches();
        }

        protected void cdrEndDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            this.cdrEndDate.Visible = true;
        }

        protected void cdrEndDate_SelectionChanged(object sender, EventArgs e)
        {
            txtDateTime.Text = cdrEndDate.SelectedDate.ToString("MM/dd/yyyy");
            SelectedDate = cdrEndDate.SelectedDate;
            cdrEndDate.Visible = false; //hide the calendar.
        }

        int employeeCount = 0;
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var data = e.Row.FindControl("Name");

                if ((double)DataBinder.Eval(e.Row.DataItem, "Hours") < .25)
                {
                    e.Row.BackColor = Color.OrangeRed;
                }
                employeeCount++;
                e.Row.Cells[0].Text = employeeCount + ".";
            }
        }
    }
}