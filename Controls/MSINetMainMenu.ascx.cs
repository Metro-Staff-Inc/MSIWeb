using System;
using System.Web;
using System.Web.UI.WebControls;
using MSI.Web.MSINet.BusinessEntities;


namespace MSI.Web.Controls
{
    public partial class MSINetMainMenu : BaseMSINetControl
    {
        public enum MSINetSections
        {
            MainMenu,
            CheckInOut,
            TicketTracking,
            EmployeeHistory,
            HoursReport,
            HoursReport2,
            HoursReportFlat,
            HeadCount,
            HeadCountFullRoster,
            DaysWorkedReport,
            InvoiceProcessing,
            EnrollEmployee,
            TestPage,
            DepartmentMapping,
            ClientInfo,
            Administrative,
            ETicket,
            Departments,
            UserRoles, 
            Supervisors
        }

        private MSINetSections _selectedSection = MSINetSections.MainMenu;
        private ClientPreferences _clientPrefs;

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

        public MSINetSections SelectedSection
        {
            get
            {
                return _selectedSection;
            }
            set
            {
                _selectedSection = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClientPrefs == null)
            {
                ClientPrefs = (ClientPreferences)this.Session["ClientPrefs"];
            }
            //if (ClientPrefs == null) return;
            //this.Menu1.CssClass = "MSINetMenu";
            
            this.Menu1.Load += new EventHandler(Menu1_Load);
            this.Menu1.DataSource = this.SiteMapDataSource1;
            this.Menu1.DataBind();

            if (Context.User.IsInRole("SupportIcon"))
                pnlFreshDesk.Visible = true;
            else
                pnlFreshDesk.Visible = false;
        }

        void Menu1_Load(object sender, EventArgs e)
        {

            Boolean administrator = Context.User.IsInRole("AdministratorTab") ||
                                        Context.User.IsInRole("Administrator") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MOAKES") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ITDEPT") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("CARLOS") ||
                                        Context.User.Identity.Name.ToUpper().Equals("JULIO") ||
                                        Context.User.Identity.Name.ToUpper().Equals("BADANIS") ||
                                        Context.User.Identity.Name.ToUpper().Equals("CASTILLOM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("CMARTINEZ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MARIA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("SHIRED") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ARWEBTRAX") ||
                                        Context.User.Identity.Name.ToUpper().Equals("WCWEBTRAX") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ELGIN") ||
                                        Context.User.Identity.Name.ToUpper().Equals("RAFA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ELKGROVE") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ANTONIA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("BARTLETT") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VILLAPARK") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MARGARITA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ZAVALAR") ||
                                        Context.User.Identity.Name.ToUpper().Equals("PEREZV") ||
                                        Context.User.Identity.Name.ToUpper().Equals("RAMOSM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("GARCIAM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("WATKINSK") ||
                                        Context.User.Identity.Name.ToUpper().Equals("IMOLINA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("LBROGGI") ||
                                        Context.User.Identity.Name.ToUpper().Equals("RAFA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MMARTURELL") ||
                                        Context.User.Identity.Name.ToUpper().Equals("GARCIAMI") ||
                                        Context.User.Identity.Name.ToUpper().Equals("LISA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("FERNANDO") ||
                                        Context.User.Identity.Name.ToUpper().Equals("PILLADOG") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VAZQUEZG") ||
                                        Context.User.Identity.Name.ToUpper().Equals("AURORA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("BELTRANJ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VVEGA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("AFIGUEROA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("HERNANDEZC") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MAGDALENOY") ||
                                        Context.User.Identity.Name.ToUpper().Equals("FERRERM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("GARCIALI") ||
                                        Context.User.Identity.Name.ToUpper().Equals("WHEELING") ||
                                        Context.User.Identity.Name.ToUpper().Equals("CHAVEZZ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("QUINTOB") ||
                                        Context.User.Identity.Name.ToUpper().Equals("SZUNIGA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("CASTILLOM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("PECHSTEINR") ||
                                        Context.User.Identity.Name.ToUpper().Equals("NAJERAA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("BOLINGBROOK") ||
                                        Context.User.Identity.Name.ToUpper().Equals("HERRERAS") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VEGAA") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MPILLADO") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MORENOM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ROCHAM") ||
                                        Context.User.Identity.Name.ToUpper().Equals("MARTINEZJ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("VALDEZJ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("ELOPEZ") ||
                                        Context.User.Identity.Name.ToUpper().Equals("LUNAJ")
                                        ;
            /* remove administrative if necessary */
            if (!administrator)
            {
                removeItem("Administrative");
            }

            /* remove invoice processing if necessary */
            Boolean invoiceAuthorized = (Context.User.Identity.Name.ToUpper() == "MCHAVEZ"
                || Context.User.Identity.Name.ToUpper() == "MOAKES"
                || Context.User.Identity.Name.ToUpper() == "ITDEPT");

            if (!Context.User.Identity.Name.ToUpper().Equals("FELICIANOY") && !Context.User.Identity.Name.ToUpper().Equals("HULBACKT")
                && !Context.User.Identity.Name.ToUpper().Equals("ZEMAITISA") && !Context.User.Identity.Name.ToUpper().Equals("RICOF")
                && !Context.User.Identity.Name.ToUpper().Equals("FILARSKIR") && !Context.User.Identity.Name.ToUpper().Equals("MILLERR")
                && !Context.User.Identity.Name.ToUpper().Equals("EDOGAN") && !Context.User.Identity.Name.ToUpper().Equals("GRIFFINC")
                && !Context.User.Identity.Name.ToUpper().Equals("DELUTRIM") && !invoiceAuthorized && !Context.User.Identity.Name.ToUpper().Equals("KELLYT")
                && !Context.User.Identity.Name.ToUpper().Equals("VAZQUEZR") && !Context.User.Identity.Name.ToUpper().Equals("GOLJICZ")
                && !Context.User.Identity.Name.ToUpper().Equals("ALANISH") && !Context.User.Identity.Name.ToUpper().Equals("PRICEB")
                && !Context.User.Identity.Name.ToUpper().Equals("VALLES") && !Context.User.Identity.Name.ToUpper().Equals("GARZAVELAA")
                && !Context.User.Identity.Name.ToUpper().Equals("FRENCHMEDICAL") && !Context.User.Identity.Name.ToUpper().Equals("ORTIZM")
                && !Context.User.Identity.Name.ToUpper().Equals("SECHRISTK") && !Context.User.IsInRole("PunchOnly"))
            {
                removeItem("Check In/Out");
            }
            if (!Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") &&
                !Context.User.Identity.Name.ToUpper().Equals("ITDEPT") &&
                !Context.User.Identity.Name.ToUpper().Equals("IMOLINA") &&
                !Context.User.Identity.Name.ToUpper().Equals("JULIO") &&
                !Context.User.Identity.Name.ToUpper().Equals("WHITEL") &&
                !Context.User.Identity.Name.ToUpper().Equals("VANESSAG"))
            {
                removeItem("Group Hours Report");
            }
            if (ClientPrefs != null && ClientPrefs.DisplayInvoice == false || !invoiceAuthorized)
            {
                removeItem("Invoice Processing");
            }
            if ((_clientId == 165 &&
                (!Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") ||
                 !Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") ||
                 !Context.User.Identity.Name.ToUpper().Equals("JMURFEY")))
              || Context.User.IsInRole("PayRates") == false
                )
            {
                removeItem("Maintain Pay Rates");
            }
            if (ClientPrefs != null && ClientPrefs.EnablePunchReporting == false)
            {
                removeItem("Punch Report");
            }
            if (!Context.User.Identity.Name.ToUpper().Equals("ITDEPT") && !Context.User.Identity.Name.ToUpper().Equals("MOAKES") &&
                !Context.User.Identity.Name.ToUpper().Equals("MCHAVEZ") && !Context.User.Identity.Name.ToUpper().Equals("CARLOS") &&
                !Context.User.Identity.Name.ToUpper().Equals("IMOLINA") &&
                /*!Context.User.Identity.Name.ToUpper().Equals("CMARTINEZ") &&*/ !Context.User.Identity.Name.ToUpper().Equals("LBROGGI") &&
                !Context.User.Identity.Name.ToUpper().Equals("VIRGINIA") && !Context.User.Identity.Name.ToUpper().Equals("LISA") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("HERNANDEZC") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("MAGDALENOY") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("FERRERM") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("GARCIALI") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("WHEELING") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("SZUNIGA") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("CHAVEZZ") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("CASTILLOM") &&
                                        !Context.User.Identity.Name.ToUpper().Equals("BELTRANJ") &&
                                        !Context.User.IsInRole("DNR")
                )
            {
                removeItem("Employee DNR");
            }
            if (!Context.User.IsInRole("DNR"))
            {
                removeItem("Employee DNR");
            }
            string user = Context.User.Identity.Name.ToUpper();
            if (!user.Equals("ITDEPT"))
            {
                removeItem("Ajax Test Page");
                removeItem("Weekly Hours Report 2");
                removeItem("Bootstrap");
                removeItem("Create/Modify Clients");
            }
            if (!Context.User.IsInRole("DepartmentsAndSupervisors"))
            {
                removeItem("Supervisor/Dept");
            }
            if (!(user.Equals("ITDEPT") || user.Equals("FERRERM")))
            {
                removeItem("Set User Roles");
            }
            if (!Context.User.IsInRole("PhoneBlast") &&
                !user.Equals("ITDEPT") && !user.Equals("WCWEBTRAX") && !user.Equals("CARLOS") && !user.Equals("MCHAVEZ") && !user.Equals("FERNANDO")
                && !user.Equals("BADANIS") && !user.Equals("MOAKES") && !user.Equals("VIRGINIA") && !user.Equals("MARIA") && !user.Equals("BARTLETT")
                && !user.Equals("ELGIN") && !user.Equals("BOLINGBROOK") && !user.Equals("AURORA") && !user.Equals("BETTY") && !user.Equals("ELKGROVE")
                && !user.Equals("ANTONIA") && !user.Equals("ARWEBTRAX") && !user.Equals("JULIO") && !user.Equals("MMARTURELL") && !user.Equals("MARGARITA")
                && !user.Equals("LBROGGI") && !user.Equals("RAFA") && !user.Equals("CASTILLOM") && !user.Equals("GARCIAMI") && !user.Equals("VAZQUEZG")
                && !user.Equals("PILLADOG") && !user.Equals("ZAVALAR") && !user.Equals("CMARTINEZ") && !user.Equals("BELTRANJ") && !user.Equals("VVEGA")
                && !user.Equals("VILLAPARK") && !user.Equals("HERNANDEZC") && !user.Equals("MAGDALENOY") && !user.Equals("WHEELING") && !user.Equals("SZUNIGA")
                && !user.Equals("CHAVEZZ") && !user.Equals("QUINTOB") && !user.Equals("FERRERM") && !user.Equals("GARCIALI")
                && !user.Equals("AFIGUEROA") && !user.Equals("HERRERAS") && !user.Equals("BOLINGBROOK") && !user.Equals("MORENOM") && !user.Equals("ELOPEZ"))
            {
                removeItem("Phone Blast");
            }
            if (!Context.User.IsInRole("GenerateSuncastIds"))
            {
                removeItem("Map Suncast ID");
            }
            
            if ( _clientId != 2 && _clientId != 226 && _clientId != 178 && _clientId != 272 && _clientId != 274 &&
                _clientId != 259 && _clientId != 256 && _clientId != 166 && _clientId != 127 && _clientId != 275 &&
                _clientId != 279 && _clientId != 280 && _clientId != 281 && _clientId != 286 && _clientId != 158 &&
                _clientId != 292 && _clientId != 293 && _clientId != 302 && _clientId != 295 && _clientId != 325 && 
                _clientId != 326 && _clientId != 327 )
            {
                removeItem("Punch Photos");
            }
            if( ( _clientId == 245 && (Context.User.Identity.Name.ToUpper().Equals("HULBACKT") ||
                Context.User.Identity.Name.ToUpper().Equals("ZEMAITISA") ||
                Context.User.Identity.Name.ToUpper().Equals("RICOF") ||
                Context.User.Identity.Name.ToUpper().Equals("EDOGAN")  || 
                Context.User.Identity.Name.ToUpper().Equals("GRIFFINC")) ) ||
                Context.User.Identity.Name.ToUpper().Equals("MILLERR") ||
                Context.User.Identity.Name.ToUpper().Equals("FILARSKIR") )
            {
                removeItem("Reports");
                removeItem("Ticket Tracker");
            }
            if (Context.User.Identity.Name.ToUpper().Equals("FILARSKIR"))
                removeItem("Employee History");

            if ( !Context.User.IsInRole("ETicketTab") )
                removeItem("ETicket Report");
            if( Context.User.IsInRole("PunchOnly") )
            {
                removeItem("Employee History");
                removeItem("Ticket Tracker");
                removeItem("Reports");
            }
        }

        protected void Menu1_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            System.Web.UI.WebControls.Menu menu = (System.Web.UI.WebControls.Menu)sender;
            SiteMapNode mapNode = (SiteMapNode)e.Item.DataItem;

            System.Web.UI.WebControls.MenuItem itemToRemove = menu.FindItem(mapNode.Title);

            System.Web.UI.WebControls.MenuItem parent = e.Item.Parent;
        }

        private void removeItem(String item)
        {
            for (int i = 0; i < this.Menu1.Items.Count; i++)
            {
                if (this.Menu1.Items[i].Value.ToUpper().Equals(item.ToUpper()))
                {
                    this.Menu1.Items.RemoveAt(i);
                }
                else
                {
                    MenuItemCollection children = this.Menu1.Items[i].ChildItems;
                    if (children != null)
                    {
                        for (int j = 0; j < children.Count; j++)
                        {
                            if (children[j].Value.ToUpper().Equals(item.ToUpper()))
                            {
                                children.RemoveAt(j);
                            }
                        }
                    }
                }
            }
        }
        protected override void SecureControl()
        {
            //this.mnuInvoiceProcessing.Visible = false;

            if (Context.User.IsInRole("TimeClock"))
            {
                //this.mnuEmployeeHistory.Visible = false;
                //this.mnuHoursReport.Visible = false;
                removeItem("Hours Report");
                //this.mnuTicketTracking.Visible = false;
                removeItem("Ticket Tracker");
            }
            else if( Context.User.IsInRole("ManagerNoPunchIn"))
            {
                //this.mnuCheckInOut.Visible = false;
                removeItem("Check In/Out");
            }
            else if ( ClientPrefs != null && ClientPrefs.DisplayInvoice )
            {
                if (Context.User.Identity.Name.ToUpper() == "TCAMPBELL" || Context.User.Identity.Name.ToUpper() == "MCHAVEZ" 
                    || Context.User.Identity.Name.ToUpper() == "MOAKES" || Context.User.Identity.Name.ToUpper() == "ITDEPT")
                {
                    //this.mnuInvoiceProcessing.Visible = true;
                }
            }
        }
    }
}