using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet
{
    public partial class InvoiceSummary : BaseMSINetPage
    {
        private Invoice _invoice = new Invoice();
        private Invoice _invoiceParms = new Invoice();
        private InvoiceBL _invoiceBL = new InvoiceBL();
        private InvoiceDetail _boundDetail = new InvoiceDetail();
        private InvoiceDetail _nextDetail = new InvoiceDetail();
        private int _currentDepartmentId = 0;
        private int _currentShiftType = 0;
        private decimal[] _totShift = new decimal[3];
        private decimal[] _grandTot = new decimal[3];
        private bool _loadedInvoice = false;
        private bool _displayExcel = false;
        private HelperFunctions _common = new HelperFunctions();

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            _invoiceParms.ClientID = _clientInfo.ClientID;
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.Clients = _clients;
            this.ctlSubHeader.ClientInfo = _clientInfo;

            if (!this.IsPostBack)
            {
                //check if a weekend date was sent in
                if (Context.Items["weekend"] != null)
                {
                    DateTime weekEnd = new DateTime(1900, 1, 1);
                    weekEnd = DateTime.Parse(Context.Items["weekend"].ToString());
                    if (weekEnd != null && weekEnd != new DateTime(1900, 1, 1))
                    {
                        //we came from the hours report so we need to get the invoice for the
                        //given week end date
                        _invoiceParms.WeekEndDate = weekEnd;
                        this.ctlWeekEnding.SelectedDate = weekEnd;
                    }

                    Context.Items.Remove("weekend");

                    //get the invoice
                    this.getInvoice();
                }
                else if (Request.QueryString["excel"] != null)
                {
                    if (Request.QueryString["excel"] == "1")
                    {
                        if (Request.QueryString["date"] != null)
                        {
                            _invoiceParms.WeekEndDate = DateTime.Parse(Server.UrlDecode(Request.QueryString["date"].ToString()));
                            _displayExcel = true;
                            //get the invoice
                            this.getInvoice();
                        }
                    }
                }
                else
                {
                    this.ctlWeekEnding.SelectedDate = _common.GetCSTCurrentWeekEndingDate();
                }
            }
        }

        protected void rptrInvoice_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundDetail = (InvoiceDetail)e.Item.DataItem;
                HtmlTableRow trDepartment = (HtmlTableRow)e.Item.FindControl("trDepartment");
                HtmlTableRow trDepartmentTotals = (HtmlTableRow)e.Item.FindControl("trDepartmentTotals");

                if (_boundDetail.DepartmentInfo.DepartmentID != _currentDepartmentId || _boundDetail.ShiftTypeInfo.ShiftTypeId != _currentShiftType)
                {
                    trDepartment.Visible = true;
                    ((Label)e.Item.FindControl("lblDepartment")).Text = _boundDetail.CostCenter + " " + _boundDetail.DepartmentInfo.DepartmentName + " - " + _boundDetail.ShiftTypeInfo.ToString();

                    _currentDepartmentId = _boundDetail.DepartmentInfo.DepartmentID;
                    _currentShiftType = _boundDetail.ShiftTypeInfo.ShiftTypeId;

                    if (!_displayExcel)
                    {
                        ((LinkButton)e.Item.FindControl("lnkChangePayRates")).Visible = true;
                        ((LinkButton)e.Item.FindControl("lnkChangePayRates")).CommandArgument = _currentShiftType.ToString() + "_" + _currentDepartmentId.ToString();
                    }
                    else
                    {
                        ((LinkButton)e.Item.FindControl("lnkChangePayRates")).Visible = false;
                    }
                }
                else
                {
                    trDepartment.Visible = false;
                }

                //calculate totals
                this.CalculateTotals();

                //write out the details
                ((Label)e.Item.FindControl("lblBadgeNumber")).Text = _boundDetail.BadgeNumber;
                ((Label)e.Item.FindControl("lblLastName")).Text = _boundDetail.LastName;
                ((Label)e.Item.FindControl("lblFirstName")).Text = _boundDetail.FirstName;
                ((Label)e.Item.FindControl("lblJobCode")).Text = _boundDetail.JobCode;
                ((Label)e.Item.FindControl("lblPayRate")).Text = _boundDetail.PayRate.ToString("$#,##.00");
                decimal tempBill = Math.Round(_boundDetail.PayRate * _boundDetail.RegularMultiplier, 2, MidpointRounding.AwayFromZero);
                decimal tempOT = Math.Round(_boundDetail.PayRate * _boundDetail.OTMultiplier * 1.5M, 2, MidpointRounding.AwayFromZero);
                //tempOT = Math.Round(tempOT * 1.5M, 2, MidpointRounding.AwayFromZero);
                ((Label)e.Item.FindControl("lblBillRate")).Text = tempBill.ToString("$#,##0.00");
                ((Label)e.Item.FindControl("lblOTBillRate")).Text = tempOT.ToString("$#,##0.00");
                String temp = _boundDetail.TotalRegularHours.ToString(" #,##0.00");
                ((Label)e.Item.FindControl("lblRegHours")).Text = temp;// _boundDetail.TotalRegularHours.ToString("N2");// ("#,##0.00");
                ((Label)e.Item.FindControl("lblOTHours")).Text = _boundDetail.TotalOTHours.ToString(" #,##0.00");// ("#,##0.00");
                ((Label)e.Item.FindControl("lblTotalBilling")).Text = _boundDetail.TotalBilling.ToString("$#,##0.00");

                //check if we display totals
                _nextDetail = null;
                if (e.Item.ItemIndex < _invoice.DetailInfo.Count - 1)
                {
                    _nextDetail = _invoice.DetailInfo[e.Item.ItemIndex + 1];
                }

                if (_nextDetail == null || (e.Item.ItemIndex == _invoice.DetailInfo.Count - 1) || (_nextDetail.DepartmentInfo.DepartmentID != _boundDetail.DepartmentInfo.DepartmentID || _nextDetail.ShiftTypeInfo.ShiftTypeId != _boundDetail.ShiftTypeInfo.ShiftTypeId))
                {
                    trDepartmentTotals.Visible = true;
                    ((Label)e.Item.FindControl("lblDepartmentTotalLabel")).Text = _boundDetail.CostCenter + " " + _boundDetail.DepartmentInfo.DepartmentName + " - " + _boundDetail.ShiftTypeInfo.ToString() + " Totals:";

                    ((Label)e.Item.FindControl("lblDeptTotTotalHours")).Text = _totShift[0].ToString("###,###,##0.00");
                    _totShift[0] = 0M;
                    ((Label)e.Item.FindControl("lblDeptTotOTHours")).Text = _totShift[1].ToString("N2"/*"###,###,##0.00"*/);
                    _totShift[1] = 0M;
                    ((Label)e.Item.FindControl("lblDeptTotBilling")).Text = _totShift[2].ToString("$###,###,##0.00");
                    _totShift[2] = 0M;
                }
                else
                {
                    trDepartmentTotals.Visible = false;
                }

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                ((Label)e.Item.FindControl("lblGrandTotalHours")).Text = _grandTot[0].ToString("###,##0.00");
                _grandTot[0] = 0M;
                //Sat
                ((Label)e.Item.FindControl("lblGrandOTHours")).Text = _grandTot[1].ToString("###,##0.00");
                _grandTot[1] = 0M;
                //Sun
                ((Label)e.Item.FindControl("lblGrandTotBilling")).Text = _grandTot[2].ToString("$###,###,##0.00");
                _grandTot[2] = 0M;
            }
        }

        protected void CalculateTotals()
        {
            //totals
            _totShift[0] += _boundDetail.TotalRegularHours;
            _grandTot[0] += _boundDetail.TotalRegularHours;

            _totShift[1] += _boundDetail.TotalOTHours;
            _grandTot[1] += _boundDetail.TotalOTHours;

            _totShift[2] += _boundDetail.TotalBilling;
            _grandTot[2] += _boundDetail.TotalBilling;
        }

        private void getInvoice()
        {
            _invoice = _invoiceBL.GetInvoice(_invoiceParms, Context.User);
            _loadedInvoice = true;
        }


        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.pnlHoursNotApproved.Visible = false;
            this.pnlNoInvoice.Visible = false;
            this.pnlInvoiceHeader.Visible = false;
            this.pnlInvoiceDetail.Visible = false;
            this.lnkCreateCSV.Visible = false;

            if (_loadedInvoice)
            {
                if (_invoice.ClientApprovalId == 0 && !Context.User.Identity.Name.ToUpper().Equals("ITDEPT"))
                {
                    this.pnlHoursNotApproved.Visible = true;
                }
                else if (_invoice.InvoiceHeaderId == 0)
                {
                    this.pnlNoInvoice.Visible = true;
                }
                else
                {
                    this.pnlInvoiceHeader.Visible = true;
                    this.lnkCreateCSV.Visible = true;
                    this.hdnWeekEnd.Value = _invoice.WeekEndDate.ToString("MM/dd/yyyy");
                    this.lblClientHead.Text = _clientInfo.ClientName.ToUpper() + " INVOICE";
                    this.lblInvoiceDate.Text = _invoice.InvoiceDateTime.ToString("MM/dd/yyyy");
                    this.lblInvoiceNumber.Text = _invoice.InvoiceHeaderId.ToString();
                    this.lblWeekEnd.Text = "Week Ending: " + _invoice.WeekEndDate.ToString("MM/dd/yyyy");
                    this.lblInvoiceDollars.Text = _invoice.TotalDollars.ToString("$ ###,###,##0.00");

                    this.pnlInvoiceDetail.Visible = true;
                    this.rptrInvoice.DataSource = _invoice.DetailInfo;
                    this.rptrInvoice.DataBind();

                    this.lnkExportDetail.NavigateUrl = "InvoiceSummary.aspx?excel=1&date=" + Server.UrlEncode(_invoice.WeekEndDate.ToString("MM/dd/yyyy"));
                }

                if (_displayExcel)
                {
                    this.pnlMSIControls.Visible = false;
                    this.pnlHoursNotApproved.Visible = false;
                    this.pnlNoInvoice.Visible = false;
                    this.pnlHeader.Visible = false;
                }
                else
                {
                    this.pnlMSIControls.Visible = true;
                    this.pnlHeader.Visible = true;
                }

            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (_displayExcel)
            {
                string dateTime = Server.UrlDecode((string)Request.QueryString["date"]);

                Response.ContentType = "application/ms-excel";

                Response.AddHeader("Content-Disposition", "inline;filename=HoursReport" + dateTime + ".xls");
            }

            this.RenderChildren(writer);
        }

        //
        protected void btnGo_Click(object sender, EventArgs e)
        {
            //get the week end date
            _invoiceParms.WeekEndDate = this.ctlWeekEnding.SelectedDate;
            this.getInvoice();
        }

        protected void lnkChangePayRates_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            string[] info = lnk.CommandArgument.Split('_');
            //get the week end date
            Context.Items["weekend"] = this.ctlWeekEnding.SelectedDate;
            Context.Items["shifttype"] = info[0];
            Context.Items["departmentid"] = info[1];
            Server.Transfer("PayRateMaintenance.aspx", false);
        }


        protected void lnkCreateCSV_Click(object sender, EventArgs e)
        {
            _invoiceParms.WeekEndDate = DateTime.Parse(this.hdnWeekEnd.Value);
            this.getInvoice();

            StringBuilder csvInfo = new StringBuilder();
            //add headers

            bool bonus = false;
            if (_clientInfo.ClientID != 178 && _clientInfo.ClientID != 256 &&
                _clientInfo.ClientID != 280 && _clientInfo.ClientID != 281 &&
                _clientInfo.ClientID != 292 && _clientInfo.ClientID != 293)
                bonus = false;
            else
                bonus = true;
            if (bonus)
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID,BONUS_PAY");
            else
                csvInfo.Append("AIDENT NUMBER,SSN,LASTNAME,FIRSTNAME,REGULAR HOURS,OVERTIME HOURS,PAY RATE,BILL RATE,RETRO REGULAR,RETRO OT,DEPARTMENT ID");

            foreach (InvoiceDetail detail in _invoice.DetailInfo)
            {
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
                csvInfo.Append(detail.LastName.ToUpper());
                csvInfo.Append("\",");
                //firstname
                csvInfo.Append("\"");
                csvInfo.Append(detail.FirstName.ToUpper());
                csvInfo.Append("\",");
                //regular hours
                csvInfo.Append(detail.TotalRegularHours.ToString("0.00"));
                csvInfo.Append(",");
                //ot hours
                csvInfo.Append(detail.TotalOTHours.ToString("0.00"));
                csvInfo.Append(",");
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
                    csvInfo.Append(detail.Bonus.ToString("0.00"));
            }

            string fileName = "TWImport_" + _clientInfo.ClientName.Replace(",", "_").Replace("'", "_").Replace(".", "_").Replace(" ", "_").ToUpper() + "_" + _invoice.WeekEndDate.ToString("MMddyyyy") + ".csv";
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

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = true;

            if (!Context.User.IsInRole("Administrator") && !Context.User.IsInRole("Manager"))
            {
                base._isAuthorized = false;
            }
            else if (!(Context.User.Identity.Name.ToUpper().Equals("IMOLINA")) &&
                !(Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ")) && !(Context.User.Identity.Name.ToUpper().Equals("MOAKES")) &&
                !(Context.User.Identity.Name.ToUpper().Equals("ITDEPT")) && !(Context.User.Identity.Name.ToUpper().Equals("JPAA")) &&
                !(Context.User.Identity.Name.ToUpper().Equals("LISA")) && !(Context.User.Identity.Name.ToUpper().Equals("MAGDALENOY")) &&
                !(Context.User.Identity.Name.ToUpper().Equals("FERRERM")) && !(Context.User.Identity.Name.ToUpper().Equals("GARCIALI")) &&
                !(Context.User.Identity.Name.ToUpper().Equals("SANCHEZM")) && !(Context.User.Identity.Name.ToUpper().Equals("MENDOZAC")))
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
    }
}
