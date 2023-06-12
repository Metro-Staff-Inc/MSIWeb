using System;
using System.Collections;
using System.Web.UI.WebControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet
{
    public partial class PayRateMaintenance : BaseMSINetPage
    {
        private ClientBL _clientBL = new ClientBL();
        private DepartmentPayRate _payRateOverrides = new DepartmentPayRate();
        private ClientPayOverride _boundOverride = new ClientPayOverride();
        private PayRateInput _inputParms = new PayRateInput();
        private ArrayList _departments = new ArrayList();
        private MSINet.BusinessEntities.EmployeeHistory _empLookup = new MSINet.BusinessEntities.EmployeeHistory();

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            //log.Info("Page_Load");
            base.LoadClientShiftTypes();

            this.pnlPayRateHeader.Visible = false;
            this.pnlPayRateOverrides.Visible = false;
            this.ctlSubHeader.SectionHeader = _clientInfo.ToString();
            this.ctlSubHeader.ClientInfo = _clientInfo;
            this.ctlSubHeader.Clients = _clients;

            _inputParms.ClientID = _clientInfo.ClientID;
            HelperFunctions hf = new HelperFunctions();
            ctlWeekEnding.SelectedDate = hf.GetCSTWeekEndingDateFromDate(DateTime.Now);

            if (!this.IsPostBack)
            {
                if (Context.Items["weekend"] != null)
                {
                    DateTime weekEnd = new DateTime(1900, 1, 1);
                    weekEnd = DateTime.Parse(Context.Items["weekend"].ToString());
                    if (weekEnd != null && weekEnd != new DateTime(1900, 1, 1))
                    {
                        //we came from the hours report so we need to get the invoice for the
                        //given week end date
                        _inputParms.WeekEndDate = weekEnd;
                        this.ctlWeekEnding.SelectedDate = weekEnd;
                    }
                    this.hdnWeekEnd.Value = weekEnd.ToString("MM/dd/yyyy");
                    Context.Items.Remove("weekend");

                    _inputParms.DepartmentID = int.Parse(Context.Items["departmentid"].ToString());
                    Context.Items.Remove("departmentid");
                    _inputParms.ShiftTypeId = int.Parse(Context.Items["shifttype"].ToString());
                    Context.Items.Remove("shifttype");

                    //load the shift types
                    this.loadShiftTypeCombo(_clientInfo.ShiftTypes);
                    this.processShiftTypeChange(_inputParms.ShiftTypeId.ToString(), _inputParms.DepartmentID);
                    this.getPayRateOverrides();
                    this.hdnShowReturn.Value = "1";
                }
                else
                {
                    //load the shift types
                    this.loadShiftTypeCombo(_clientInfo.ShiftTypes);
                    _departments = base.GetClientDepartmentsByShiftType((ShiftType)_clientInfo.ShiftTypes[0], 0);
                    this.loadDepartmentCombo(_departments);
                    this.hdnShowReturn.Value = "0";
                }
            }            
        }

        protected void getPayRateOverrides()
        {
            _payRateOverrides = _clientBL.GetDepartmentPayRates(_inputParms, Context.User.Identity.Name);
            this.pnlPayRateHeader.Visible = true;
            this.pnlPayRateOverrides.Visible = true;
        }

        protected void rptrOverrides_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                _boundOverride = (ClientPayOverride)e.Item.DataItem;

                //write out the details
                ((Label)e.Item.FindControl("lblAidentNumber")).Text = _boundOverride.AidentNumber;
                ((Label)e.Item.FindControl("lblLastName")).Text = _boundOverride.LastName;
                ((Label)e.Item.FindControl("lblFirstName")).Text = _boundOverride.FirstName;
                ((TextBox)e.Item.FindControl("txtEmpPayRate")).Text = _boundOverride.PayRate.ToString("#,##.00");
                ((TextBox)e.Item.FindControl("txtEffectiveDate")).Text = _boundOverride.EffectiveDate.ToString("MM/dd/yyyy");
                if (_boundOverride.ExpirationDate == new DateTime(9999, 12, 31))
                {
                    ((TextBox)e.Item.FindControl("txtExpirationDate")).Text = string.Empty;
                    ((RadioButton)e.Item.FindControl("optIndefinite")).Checked = true;
                }
                else
                {
                    ((TextBox)e.Item.FindControl("txtExpirationDate")).Text = _boundOverride.ExpirationDate.ToString("MM/dd/yyyy");
                    ((RadioButton)e.Item.FindControl("optWeekEnd")).Checked = true;
                }
                ((LinkButton)e.Item.FindControl("lnkSave")).CommandArgument = _boundOverride.ClientPayOverrideId.ToString();
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                ((HiddenField)e.Item.FindControl("hdnNewEmployeeId")).Value = _empLookup.EmployeeID.ToString();
                ((Panel)e.Item.FindControl("pnlAddNewEmployee")).Visible = false;
                ((Label)e.Item.FindControl("lblNewAidentNumber")).Text = "";
                ((Label)e.Item.FindControl("lblNewLastName")).Text = "";
                ((Label)e.Item.FindControl("lblNewFirstName")).Text = "";
                if (_empLookup.EmployeeID > 0)
                {
                    ((Panel)e.Item.FindControl("pnlAddNewEmployee")).Visible = true;
                    ((Label)e.Item.FindControl("lblNewAidentNumber")).Text = _empLookup.AIdentNumber;
                    ((Label)e.Item.FindControl("lblNewLastName")).Text = _empLookup.LastName;
                    ((Label)e.Item.FindControl("lblNewFirstName")).Text = _empLookup.FirstName;
                    _empLookup = new MSINet.BusinessEntities.EmployeeHistory();
                }

            }
        }

        private void loadShiftTypeCombo(ArrayList shiftTypes)
        {
            this.cboShift.Items.Clear();
            foreach (ShiftType shiftType in shiftTypes)
            {
                this.cboShift.Items.Add(new ListItem(shiftType.ToString(), shiftType.ShiftTypeId.ToString()));
            }
        }

        protected void cboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(this.cboShift.SelectedValue);

            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, 0);
            //load the department drop-down
            this.loadDepartmentCombo(_departments);
        }

        private ShiftType processShiftTypeChange(string shiftTypeId)
        {
            //get the selected shift type
            ShiftType selectedShiftType = null;
            foreach (ShiftType shiftType in _clientInfo.ShiftTypes)
            {
                if (shiftType.ShiftTypeId.ToString() == shiftTypeId)
                {
                    selectedShiftType = shiftType;
                    break;
                }
            }
            return selectedShiftType;
        }

        private ShiftType processShiftTypeChange(string shiftType, int departmentId)
        {
            ShiftType selectedShiftType = this.processShiftTypeChange(shiftType);

            foreach (ListItem item in cboShift.Items)
            {
                if (item.Value == shiftType)
                {
                    item.Selected = true;
                    break;
                }
            }

            //load the department drop-down for the selected shifttype
            _departments = base.GetClientDepartmentsByShiftType(selectedShiftType, 0);
            this.loadDepartmentCombo(_departments, departmentId.ToString());

            return selectedShiftType;
        }

        private void loadDepartmentCombo(ArrayList departments, string departmentToSelect)
        {
            this.cboDepartment.Items.Clear();
            ListItem item = null;
            //if (departments.Count > 1)
            //    this.cboDepartment.Items.Add(new ListItem("All Departments", "0"));
            foreach (Department department in departments)
            {
                item = new ListItem(department.DepartmentName, department.DepartmentID.ToString());
                if( this._clientInfo.ClientID == 165 && department.DepartmentName.Equals("Addison") ||
                                            department.DepartmentName.Equals("Schaumburg") )
                    continue;
                this.cboDepartment.Items.Add(item);
                if (item.Value == departmentToSelect)
                {
                    item.Selected = true;
                    //break;
                }
            }
        }

        private void loadDepartmentCombo(ArrayList departments)
        {
            this.cboDepartment.Items.Clear();
            //if (departments.Count > 1)
            //    this.cboDepartment.Items.Add(new ListItem("All Departments", "0"));
            foreach (Department department in departments)
            {
                if( _clientInfo.ClientID == 165 && (department.DepartmentName.Equals("Addison") || department.DepartmentName.Equals("Schaumburg")) )
                    continue;
                else
                    this.cboDepartment.Items.Add(new ListItem(department.DepartmentName, department.DepartmentID.ToString()));
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.hdnShowReturn.Value == "1")
            {
                this.lnkGoBack.Visible = true;
            }
            else
            {
                this.lnkGoBack.Visible = false;
            }

            this.btnSaveDepartmentPayRate.CommandArgument = _payRateOverrides.ClientPayId.ToString();
            this.txtDepartmentPayRate.Text = _payRateOverrides.PayRate.ToString("#,##0.00");
            this.rptrOverrides.DataSource = _payRateOverrides.PayRateOverrides;
            this.rptrOverrides.DataBind();
            this.hdnWeekEnd.Value = this.hdnWeekEnd.Value;
            this.hdnShowReturn.Value = this.hdnShowReturn.Value;
        }


        protected void btnGo_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.CommandName == "departmentpayrate")
            {
                //update the department pay rate
                DepartmentPayRate dept = new DepartmentPayRate();
                dept.ClientId = _clientInfo.ClientID;
                dept.ClientPayId = int.Parse(btn.CommandArgument);
                dept.ShiftType = int.Parse(this.cboShift.SelectedValue);
                dept.DepartmentInfo = new Department();
                dept.DepartmentInfo.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
                dept.EffectiveDate = this.ctlWeekEnding.SelectedDate;//.Add(new TimeSpan(7,0,0,0));
                dept.ExpirationDate = new DateTime(9999, 12, 1);
                dept.PayRate = decimal.Parse(this.txtDepartmentPayRate.Text);
                InvoiceBL invBL = new InvoiceBL();
                dept = invBL.UpdateDepartmentPayRate(dept, Context.User);
            }

            //get the week end date
            _inputParms.WeekEndDate = this.ctlWeekEnding.SelectedDate;
            _inputParms.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
            _inputParms.ShiftTypeId = int.Parse(this.cboShift.SelectedValue);
            this.getPayRateOverrides();
        }

        protected void rptrOverrides_ItemCommand(Object src, RepeaterCommandEventArgs e)
        {
            InvoiceBL invoiceBL = new InvoiceBL();
            ClientPayOverride clientPay = new ClientPayOverride();
            RadioButton optIndefinite = null;
            this.pnlPayRateCollision.Visible = false;
            switch (e.CommandName)
            {
                case "save":
                    HelperFunctions helper = new HelperFunctions();
                    clientPay = new ClientPayOverride();
                    clientPay.ClientPayOverrideId = int.Parse(((LinkButton)e.Item.FindControl("lnkSave")).CommandArgument.ToString());
                    //effective for week ending
                    clientPay.EffectiveDate = DateTime.Parse(((TextBox)e.Item.FindControl("txtEffectiveDate")).Text);
                    clientPay.PayRate = decimal.Parse(((TextBox)e.Item.FindControl("txtEmpPayRate")).Text);
                    optIndefinite = (RadioButton)e.Item.FindControl("optIndefinite");
                    if (optIndefinite.Checked)
                    {
                        clientPay.ExpirationDate = new DateTime(9999, 12, 31);
                    }
                    else
                    {
                        clientPay.ExpirationDate = DateTime.Parse(((TextBox)e.Item.FindControl("txtExpirationDate")).Text);
                    }
                    //update the pay rate
                    clientPay = invoiceBL.UpdateClientPayOverride(clientPay, Context.User, null);
                    break;
                case "lookup":
                    _empLookup = new MSINet.BusinessEntities.EmployeeHistory();
                    EmployeeLookup lookup = new EmployeeLookup();
                    TextBox txtAidentNumber = (TextBox)e.Item.FindControl("txtNewAidentNumber");
                    lookup.AidentNumber = txtAidentNumber.Text.Trim();
                    if (lookup.AidentNumber.Length > 0)
                    {
                        EmployeeBL empBL = new EmployeeBL();
                        _empLookup = empBL.GetEmployeeByAident(lookup);
                    }
                    break;
                case "newsave":
                    //insert a new override
                    clientPay = new ClientPayOverride();
                    clientPay.EmployeeId = int.Parse(((HiddenField)e.Item.FindControl("hdnNewEmployeeId")).Value);
                    clientPay.ClientId = _clientInfo.ClientID;
                    clientPay.ShiftType = int.Parse(this.cboShift.SelectedValue);
                    clientPay.DepartmentId = int.Parse(this.cboDepartment.SelectedValue);
                    //effective for week ending
                    clientPay.EffectiveDate = DateTime.Parse(((TextBox)e.Item.FindControl("txtNewEffectiveDate")).Text);
                    clientPay.PayRate = decimal.Parse(((TextBox)e.Item.FindControl("txtNewEmpPayRate")).Text);
                    optIndefinite = (RadioButton)e.Item.FindControl("optNewIndefinite");
                    if (((CheckBox)e.Item.FindControl("optFullClient")).Checked)
                    {
                        clientPay.DepartmentId = 0;
                        //clientPay.ShiftType = 0;
                    }
                    if (optIndefinite.Checked)
                    {
                        clientPay.ExpirationDate = new DateTime(9999, 12, 28);
                    }
                    else
                    {
                        clientPay.ExpirationDate = DateTime.Parse(((TextBox)e.Item.FindControl("txtNewExpirationDate")).Text);
                    }
                    //update the pay rate
                    clientPay = invoiceBL.AddClientPayOverride(clientPay, Context.User, null);
                    if (clientPay.ClientPayOverrideId == -1)
                    {
                        this.pnlPayRateCollision.Visible = true;
                        this.lblPayRateCollision.Text = "Pay Override for Employee " +
                            ((Label)e.Item.FindControl("lblNewFirstName")).Text + " " + ((Label)e.Item.FindControl("lblNewLastName")).Text + 
                            " already exists!";
                    }
                    break;
            }

            _inputParms.WeekEndDate = this.ctlWeekEnding.SelectedDate;
            _inputParms.DepartmentID = int.Parse(this.cboDepartment.SelectedValue);
            _inputParms.ShiftTypeId = int.Parse(this.cboShift.SelectedValue);
            this.getPayRateOverrides();
        }

        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            //get the week end date
            Context.Items["weekend"] = this.hdnWeekEnd.Value;
            Server.Transfer("InvoiceSummary.aspx", false);
        }

        protected override bool IsAuthorizedAccess()
        {
            base._isAuthorized = false;

            if (Context.User.IsInRole("Administrator") || Context.User.IsInRole("Manager") || Context.User.IsInRole("PayRates"))
            {
                base._isAuthorized = true;
            }
            else if ((Context.User.Identity.Name.ToUpper() == "IMOLINA") || (Context.User.Identity.Name.ToUpper() == "TCAMPBELL") || 
                (Context.User.Identity.Name.ToUpper() == "MCHAVEZ") || 
                (Context.User.Identity.Name.ToUpper() == "GLASERM") || (Context.User.Identity.Name.ToUpper() == "MOAKES") || 
                (Context.User.Identity.Name.ToUpper() == "ITDEPT") || (Context.User.Identity.Name.ToUpper() == "JULIO") ||
                (Context.User.Identity.Name.ToUpper() == "LISA") || (Context.User.Identity.Name.ToUpper() == "RAFAEL") || 
                (Context.User.Identity.Name.ToUpper() == "MARIA") || 
                (Context.User.Identity.Name.ToUpper() == "BADANIS") || (Context.User.Identity.Name.ToUpper() == "MAGDALENOY") ||
                (Context.User.Identity.Name.ToUpper() == "FERRERM") || (Context.User.Identity.Name.ToUpper() == "GARCIALI") || 
                (Context.User.Identity.Name.ToUpper() == "SZUNIGA") || (Context.User.Identity.Name.ToUpper() == "WHEELING") ||
                (Context.User.Identity.Name.ToUpper() == "CASTILLOM") || (Context.User.Identity.Name.ToUpper() == "BELTRANJ") ||
                (Context.User.Identity.Name.ToUpper() == "MENDOZAC") || (Context.User.Identity.Name.ToUpper() == "HERRERAS") ||
                (Context.User.Identity.Name.ToUpper() == "FERNANDO") || (Context.User.Identity.Name.ToUpper() == "MORENOM")                
                )
            {
                base._isAuthorized = true;
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