using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MSI.Web.MSINet
{
    public partial class HeadCountFullRoster : BaseMSINetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            ctlSubHeader.SectionHeader = _clientInfo.ToString();
            ctlSubHeader.Clients = _clients;
            ctlSubHeader.ClientInfo = _clientInfo;
            ctlSubHeader.ClientPrefs = _clientPrefs;

            //GridView1.DataSource = employeeList;
            //GridView gv2 = GridView1.FindControl("GridView2") as GridView;
            //GridView1.AutoGenerateColumns = false;
            //GridView1.DataBind();
            //ListView1.DataSource = SelectDepartmentsAndShifts();
            //ListView1.DataBind();
            rptrHeadCount.DataSource = GetDepartmentsAndShifts();
            rptrHeadCount.DataBind();
        }

        public List<DailyPunchDepartmentShiftInfo> GetDepartmentsAndShifts()
        {
            HeadCountReportBL hcr = new HeadCountReportBL();
            List<DailyPunchDepartmentShiftInfo> list = hcr.SelectListRosterAndHeadCountReport(_clientInfo.ClientID);
            return list;
        }
        public DataSet SelectDepartmentsAndShifts()
        {
            HeadCountReportBL hcr = new HeadCountReportBL();
            return hcr.SelectRosterAndHeadCountReport(_clientInfo.ClientID);
            //return employeeList;
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            return base.IsAuthorizedAccess();
        }

        protected void btntoExcel_Click(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=gvtoexcel.xls");
            Response.ContentType = "application/excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //GridView1.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        string[] shift = { "", "1st Shift", "2nd Shift", "3rd Shift" };
        string departmentName = "";
        int shiftType = -1;
        bool topRow = true;
        protected void rptrHeadCount_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            Repeater rep = source as Repeater;
            RepeaterItem ri = e.Item;

            DailyPunchDepartmentShiftInfo item = (DailyPunchDepartmentShiftInfo)ri.DataItem;
            List<DailyPunchDepartmentShiftInfo> list = rep.DataSource as List<DailyPunchDepartmentShiftInfo>;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                /* is this a new shift/department? */
                if( !item.departmentName.Equals(departmentName) || !item.shiftType.Equals(shiftType) )
                {
                    ((HtmlTableCell)e.Item.FindControl("tdDepartmentShift")).InnerText = 
                            item.departmentName + " " + shift[item.shiftType];
                    ((HtmlTableCell)e.Item.FindControl("tdReportTime")).InnerText =
                            item.requestDate.ToString("yyyy-MM-dd hh:mm tt");
                    ((HtmlTableRow)e.Item.FindControl("trDepartmentInfo")).Visible = true;
                    ((HtmlTableRow)e.Item.FindControl("trEmployeeHeader")).Visible = true;
                    if( !topRow )
                    {
                        ((HtmlTableRow)e.Item.FindControl("trDepartmentSpace")).Visible = true;
                        ((HtmlTableRow)e.Item.FindControl("trDepartmentSpace")).Height = "24px";
                    }
                    topRow = false;
                    departmentName = item.departmentName;
                    shiftType = item.shiftType;
                }
                else
                {
                    ((HtmlTableRow)e.Item.FindControl("trDepartmentInfo")).Visible = false;
                    ((HtmlTableRow)e.Item.FindControl("trEmployeeHeader")).Visible = false;
                    ((HtmlTableRow)e.Item.FindControl("trDepartmentSpace")).Visible = false;
                }
                /* set employee Info - every pass of the repeater */
                ((HtmlTableCell)e.Item.FindControl("tdId")).InnerHtml = item.aidentNumber;
                ((HtmlTableCell)e.Item.FindControl("tdName")).InnerHtml = item.lastName.ToUpper() + ", " 
                    + item.firstName.ToUpper();
                bool onPremises = item.punches.Count % 2 != 0;
                if (onPremises)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdStatus")).InnerHtml = "On Premises";
                    ((HtmlTableCell)e.Item.FindControl("tdPunchTime")).InnerHtml =
                        item.punches[item.punches.Count - 1].punchExact.ToString("MM/dd/yyyy hh:mm tt");
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdStatus")).InnerHtml = "Off Premises";
                    ((HtmlTableCell)e.Item.FindControl("tdPunchTime")).InnerHtml = "";
                }

            }
        }
        protected void rptrEmployeeList_ItemDataBound1(object source, RepeaterItemEventArgs e )
        {
            Repeater rep = source as Repeater;
            DailyPunchEmployeeInfo item = (DailyPunchEmployeeInfo)e.Item.DataItem;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((HtmlTableCell)e.Item.FindControl("tdId")).InnerHtml = item.aidentNumber;
                ((HtmlTableCell)e.Item.FindControl("tdName")).InnerHtml = item.lastName.ToUpper() 
                    + ", " + item.firstName.ToUpper();
                bool onPremises = item.punches.Count % 2 != 0;
                if (onPremises)
                {
                    ((HtmlTableCell)e.Item.FindControl("tdStatus")).InnerHtml = "On Premises";
                    ((HtmlTableCell)e.Item.FindControl("tdPunchTime")).InnerHtml = 
                        item.punches[item.punches.Count-1].punchExact.ToString("MM/dd/yyyy hh:mm tt");
                }
                else
                {
                    ((HtmlTableCell)e.Item.FindControl("tdStatus")).InnerHtml = "Off Premises";
                    ((HtmlTableCell)e.Item.FindControl("tdPunchTime")).InnerHtml = "";
                }
            }
        }

        protected void btnWord_Click(object sender, EventArgs e)
        {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformatsofficedocument.wordprocessingml.document";
                Response.AddHeader("Content-Disposition", "attachment; filename=WORK_ORDER.docx");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Charset = "";
                EnableViewState = false;
                System.IO.StringWriter writer = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter html = new System.Web.UI.HtmlTextWriter(writer);

                rptrHeadCount.RenderControl(html);
                Response.Write(writer);
                Response.End();
        }
    }
}
