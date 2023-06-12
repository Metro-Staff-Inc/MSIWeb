using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;
using MSI.Web.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Linq;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class ClientBL : BaseMSINetPage
    {
        public ClientBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public List<DepartmentInfo> GetDepartmentInfo(string clientID)
        {
            ClientDB cdb = new ClientDB();
            int clientId = Convert.ToInt32(clientID);
            return cdb.GetDepartmentInfo(clientId);
        }

        public List<DepartmentSupervisor> GetDepartmentSupervisors(int clientId, Guid userId)
        {
            ClientDB cdb = new ClientDB();
            return cdb.GetDepartmentSupervisors(clientId, userId);
        }
        public List<Department> GetDepartments(string clientID, string locationID, string shiftType)
        {
            ClientDB cdb = new ClientDB();
            return cdb.GetDepartments(clientID, locationID, shiftType);
        }
        public List<Department> GetDepartments(int clientID)
        {
            ClientDB cdb = new ClientDB();
            return cdb.GetDepartments(clientID);
        }
        public HoursReport GetHoursReport(int clientId, String uid, DateTime endDate)
        {
            DateTime DATE_NOT_SET = new DateTime(1, 1, 1);
            DateTime startDate = endDate.Add(new TimeSpan(-6,0,0,0));
            HoursReport _runningHoursReport;
            HoursReport _hoursReport;
            //Session["ClientInfo"] = ClientInfo;
            DateTime _firstPunch = DATE_NOT_SET;
            DateTime _lastPunch = DATE_NOT_SET;
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);

            _runningHoursReport = new HoursReport();
            _runningHoursReport.EmployeeHistoryCollection = new ArrayList();

            HoursReport hoursReportInput = new HoursReport();

            hoursReportInput.ClientID = clientId;
            hoursReportInput.RosterEmployeeFlag = true;
            //hoursReportInput.EmployeeHistoryCollection = null;
            hoursReportInput.EndDateTime = endDate;
            ts = new TimeSpan(6, 0, 0, 0);
            hoursReportInput.StartDateTime = hoursReportInput.EndDateTime.Subtract(ts);
            //ts = new TimeSpan(0, 0, 0);
            hoursReportInput.StartDateTime = hoursReportInput.StartDateTime.Date;// + ts;
            DateTime dt = endDate;
            hoursReportInput.EndDateTime = DateTime.Parse(dt.ToString("MM/dd/yyyy") + " 23:59:59");

            hoursReportInput.UseExactTimes = false;
            if( _clientInfo.ClientID == 381 )
            {
                hoursReportInput.UseExactTimes = _clientPrefs.UseExactTimes;
            }
            hoursReportInput.ShowAllEmployees = false;

            HoursReportBL hoursReportBL = new HoursReportBL();
            string userId = Context.User.Identity.Name.ToLower();
            userId = uid;
            bool sortByDept = true;
            _hoursReport = hoursReportBL.GetHoursReport(hoursReportInput, userId, "", sortByDept);
            return null;
        }
        public HoursReport GetHoursReports(int clientId, String uid, DateTime startDate, DateTime endDate)
        {
            DateTime DATE_NOT_SET = new DateTime(1, 1, 1);
            HoursReport _runningHoursReport;
            HoursReport _hoursReport;
            DateTime _firstPunch = DATE_NOT_SET;
            DateTime _lastPunch = DATE_NOT_SET;
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);

            int count = 10;
            _runningHoursReport = new HoursReport();
            _runningHoursReport.EmployeeHistoryCollection = new ArrayList();
            while (endDate > startDate)
            {
                HoursReport hoursReportInput = new HoursReport();

                hoursReportInput.ClientID = clientId;
                hoursReportInput.RosterEmployeeFlag = true;
                hoursReportInput.EndDateTime = endDate;
                ts = new TimeSpan(6, 0, 0, 0);
                hoursReportInput.StartDateTime = hoursReportInput.EndDateTime.Subtract(ts);
                ts = new TimeSpan(0, 0, 0);
                hoursReportInput.StartDateTime = hoursReportInput.StartDateTime.Date + ts;
                DateTime dt = endDate;
                hoursReportInput.EndDateTime = DateTime.Parse(dt.ToString("MM/dd/yyyy") + " 23:59:59");

                hoursReportInput.UseExactTimes = false;
                if (_clientInfo.ClientID == 381)
                {
                    hoursReportInput.UseExactTimes = _clientPrefs.UseExactTimes;
                }
                hoursReportInput.ShowAllEmployees = false;

                HoursReportBL hoursReportBL = new HoursReportBL();
                string userId = Context.User.Identity.Name.ToLower();
                userId = uid;
                bool sortByDept = true;
                _hoursReport = hoursReportBL.GetHoursReport(hoursReportInput, userId, "", sortByDept);
                for (int i = 0; i < _hoursReport.EmployeeHistoryCollection.Count; i++)
                {
                    EmployeeHistory eh = (EmployeeHistory)_hoursReport.EmployeeHistoryCollection[i];
                    if (eh.WorkSummaries.Count < 1) continue;
                    String fName = eh.FirstName;
                    String lName = eh.LastName;
                    String bNum = eh.TempNumber;
                    String dept = ((EmployeeWorkSummary)eh.WorkSummaries[0]).DepartmentInfo.DepartmentName;
                    Decimal payRate = eh.PayRate;
                    if (payRate < (Decimal)0.5)
                        payRate = eh.DefaultPayRate;
                    int shift = ((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftTypeInfo.ShiftTypeId;
                    Decimal totReg = eh.TotalRegularHours;
                    Decimal totOT = eh.TotalOTHours;
                    int pos = 0;
                    bool added = false;
                    for (int j = 0; j < _runningHoursReport.EmployeeHistoryCollection.Count && !added; j++, pos++)
                    {

                        EmployeeHistory reh = (EmployeeHistory)_runningHoursReport.EmployeeHistoryCollection[j];
                        if (reh.WorkSummaries.Count < 1) continue;
                        String deptTot = ((EmployeeWorkSummary)reh.WorkSummaries[0]).DepartmentInfo.DepartmentName;
                        if (deptTot.CompareTo(dept) > 0) break;
                        if (deptTot.Equals(dept))
                        {
                            int shiftTot = ((EmployeeWorkSummary)reh.WorkSummaries[0]).ShiftTypeInfo.ShiftTypeId;
                            if (shiftTot > shift) break;
                            if (shiftTot == shift)
                            {
                                if (eh.EmployeeID == reh.EmployeeID)
                                {
                                    // add to total record
                                    reh.TotalOTHours += eh.TotalOTHours;
                                    reh.TotalRegularHours += eh.TotalRegularHours;
                                    if( payRate < reh.DefaultPayRate )
                                        reh.DefaultPayRate = payRate;
                                    if( payRate > reh.PayRate ) 
                                        reh.PayRate = payRate;
                                    added = true;
                                }
                            }
                        }
                    }
                    if (!added)
                    {
                        EmployeeHistory ehTemp = new EmployeeHistory();
                        ehTemp.TempNumber = bNum;
                        ehTemp.LastName = lName;
                        ehTemp.FirstName = fName;
                        ehTemp.EmployeeID = eh.EmployeeID;
                        ehTemp.TotalRegularHours = totReg;
                        ehTemp.TotalOTHours = totOT;
                        ehTemp.PayRate = payRate;
                        ehTemp.DefaultPayRate = payRate;
                        ehTemp.AIdentNumber = ((EmployeeHistory)_hoursReport.EmployeeHistoryCollection[i]).AIdentNumber;
                        ehTemp.WorkSummaries = new ArrayList();
                        EmployeeWorkSummary ews = new EmployeeWorkSummary();
                        ews.DepartmentInfo = new Department();
                        ews.DepartmentInfo.DepartmentName = dept;
                        ews.ShiftTypeInfo = new ShiftType();
                        ews.ShiftTypeInfo.ShiftTypeId = shift;
                        ews.Badge = ((EmployeeHistory)_hoursReport.EmployeeHistoryCollection[i]).TempNumber;
                        ews.approvedBy = "";
                        ehTemp.WorkSummaries.Add(ews);
                        _runningHoursReport.EmployeeHistoryCollection.Insert(pos, ehTemp);
                    }
                }
                count--;
                endDate = endDate.AddDays(-7);
            }
            //this.rptrHoursReport.DataSource = _runningHoursReport.EmployeeHistoryCollection;
            //this.rptrHoursReport.DataBind();
            return _runningHoursReport;
            //return GetHoursReports(Convert.ToInt32(clientId), Convert.ToInt32(uid), Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
        }

        public List<LocationDDL> GetClientLocations(string userID, string clientID)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetClientLocations(userID, clientID);
        }
        public DepartmentMapping GetDepartmentMapping(int clientID)
        {
            ClientDB cdb = new ClientDB();
            return cdb.GetDepartmentMapping(clientID);
        }

        public void SetEmail(string id, string email, string name)
        {
            ClientDB cdb = new ClientDB();
            cdb.SetEmail(id, email, name);
        }
        public List<String> GetEmail(string id)
        {
            ClientDB cdb = new ClientDB();
            return cdb.GetEmail(id);
        }
        private void SendEMail()
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("jonathanmurfey@gmail.com", "lkjhlkjh");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mailClient.Host = "smtp.msistaff.com";
                mailClient.Port = 5190; //587
                message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress("jmurfey@msistaff.com");
                message.To.Add("jmurfey@msistaff.com");
                message.Subject = "Request For Employees";
                message.Body = "Employees have been requested for week ending " + DateTime.Now.ToString("MM/dd/yyyy");
                message.IsBodyHtml = false;

                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }


        public string RequestEmployees()
        {
            SendEMail();
            return "Request Submitted!";
        }


        public string SetClientMultiplier(string clientID, string multiplier, string multiplier2, string otMultiplier, string otMultiplier2,
                                        string bonusMultiplier, string otherMultiplier, string passThruMultiplier, string vacationMultiplier)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.SetClientMultiplier(clientID, multiplier, multiplier2, otMultiplier, otMultiplier2, bonusMultiplier, otherMultiplier, passThruMultiplier, vacationMultiplier);
        }

        //clientBL.AddPunch(idIn, crIDIn, punchDate, deptID, userID);
        public String AddPunch(string id, string crID, DateTime punchDate, int deptID, String userID, int shiftType)
        {
            TicketTrackerDB ttdb = new TicketTrackerDB();
            return ttdb.AddPunch(id, crID, punchDate, deptID, userID, shiftType);
        }

        public DateTime CheckIPClock()
        {
            ClientDB cdb = new ClientDB();
            return cdb.CheckIPClock();
        }

        public string UpdateMapping(string clientID, string startDate, string userID, string list)
        {
            string[] maps = list.Split(',');
            XElement xmlTree = null;

            xmlTree = new XElement("mappings");

            for (int i = 0; i < maps.Length; i++)
            {
                string[] map = maps[i].Split(':');

                XElement xmlMap = new XElement("map");

                XAttribute xmlShiftID = new XAttribute("shift_id", map[0]);
                XAttribute xmlClientID = new XAttribute("client_id", clientID);
                XAttribute xmlBadge = new XAttribute("temp_works_id", map[1]);
                XAttribute xmlEffectiveDate = new XAttribute("effective_date", DateTime.Now.ToShortDateString());
                XAttribute xmlExpirationDate = new XAttribute("expiration_dt", "9999-01-01");

                xmlMap.Add(xmlShiftID);
                xmlMap.Add(xmlClientID);
                xmlMap.Add(xmlBadge);
                xmlMap.Add(xmlEffectiveDate);
                xmlMap.Add(xmlExpirationDate);

                xmlTree.Add(xmlMap);
            }
            ClientDB clientDB = new ClientDB();
            return clientDB.UpdateMapping(xmlTree, maps.Length);
        }

        public string UpdateBonuses(string clientID, string weekEnd, string list, string userID)
        {
            int month = Convert.ToInt32(weekEnd.Substring(0, 2));
            int day = Convert.ToInt32(weekEnd.Substring(3, 2));
            int year = Convert.ToInt32(weekEnd.Substring(6));
            DateTime date = new DateTime(year, month, day);
            int id = Convert.ToInt32(clientID);

            string[] updates = list.Split(',');
           // XElement xmlRoot = null;
            XElement xmlTree = null;

            xmlTree = new XElement("bonuses");

            for( int i=0; i<updates.Length; i++ )
            {
                string[] info = updates[i].ToString().Split(':');
                
                XElement xmlEmployee = new XElement("employee");
                XAttribute xmlDept = new XAttribute("dept_id", info[2]);
                XAttribute xmlBadge = new XAttribute("aident_number", info[1]);
                XAttribute xmlBonus = new XAttribute("bonus", info[0]);
                XAttribute xmlWeekEnd = new XAttribute("week_ending", weekEnd);
                XAttribute xmlClientID = new XAttribute("client_id", clientID);
                XAttribute xmlCreatedBy = new XAttribute("created_by", userID);
                XAttribute xmlDesc = new XAttribute("descript", "No Description");
                XAttribute xmlCreatedDate = new XAttribute("created_dt", DateTime.Now.ToShortDateString());

                xmlEmployee.Add(xmlBonus);
                xmlEmployee.Add(xmlBadge);
                xmlEmployee.Add(xmlDept);
                xmlEmployee.Add(xmlWeekEnd);
                xmlEmployee.Add(xmlClientID);
                xmlEmployee.Add(xmlCreatedBy);
                xmlEmployee.Add(xmlCreatedDate);
                xmlEmployee.Add(xmlDesc);

                xmlTree.Add(xmlEmployee);
            }
            HoursReportDB hrDB = new HoursReportDB();
            return hrDB.UpdateBonuses(xmlTree, updates.Length);
        }


        public WeeklyReport GetWeeklyReport(string clientId, string id, DateTime start, DateTime end)
        {
            WeeklyReportDB wrDB = new WeeklyReportDB();
            HoursReport hr = new HoursReport();
            hr.ClientID = Convert.ToInt32(clientId);
            if (start < new DateTime(1999,1,1))
                hr.StartDateTime = end.AddDays(-6);
            else
                hr.StartDateTime = start;
            hr.EndDateTime = end.AddDays(1).AddSeconds(-1);
            hr.RosterEmployeeFlag = true;
            hr = wrDB.GetWeeklyReport(hr, id, "", true);
            if( hr.EmployeeHistoryCollection.Count == 0 )
                return null;

            WeeklyReport report = new WeeklyReport();
            report.shifts = new List<ShiftData>();
            ShiftData shiftData = null;

            foreach( EmployeeHistory eh in hr.EmployeeHistoryCollection)
            {
                if (shiftData == null || shiftData.ID != ((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftInfo.ShiftID ) /* first time */
                {
                    if (shiftData != null)
                    {
                        report.shifts.Add(shiftData);  //add to shifts
                        report.Reg += shiftData.Reg;
                        report.OT += shiftData.OT;
                        report.Total += shiftData.Total;
                    }
                    // new shift 
                    shiftData = new ShiftData();
                    shiftData.Desc = ((EmployeeWorkSummary)eh.WorkSummaries[0]).DepartmentInfo.DepartmentName + " " + ((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftTypeInfo.ShiftTypeDesc;
                    //((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftInfo.
                    shiftData.ID = ((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftInfo.ShiftID;
                    shiftData.Employees = new List<EmployeeData>();
                }
                EmployeeData ed = new EmployeeData();
                ed.Badge = eh.TempNumber;
                ed.JobCode = eh.JobCode;
                ed.Name = eh.LastName + ", " + eh.FirstName;
                // set up worksummaries
                List<DailySummary> ewsList = new List<DailySummary>();
                ewsList.Add(eh.MondaySummary);
                ewsList.Add(eh.TuesdaySummary);
                ewsList.Add(eh.WednesdaySummary);
                ewsList.Add(eh.ThursdaySummary);
                ewsList.Add(eh.FridaySummary);
                ewsList.Add(eh.SaturdaySummary);
                ewsList.Add(eh.SundaySummary);

                ed.Days = new List<DailyPunches>();

                for (int i = 0; i < 7; i++)
                {
                    DailyPunches dp = new DailyPunches();
                    dp.Punches = new List<PunchData>();
                    dp.Rounded = 0.0;   // hours worked (Rounded)
                    dp.Exact = 0.0;
                    foreach (EmployeeWorkSummary ews in ewsList[i].WorkSummaries)
                    {
                        PunchData pd = new PunchData();
                        pd.Exception = 0;
                        pd.ExactDate = ews.CheckInDateTime.ToString("MM/dd/yyyy hh:mm tt");
                        pd.RoundedDate = ews.RoundedCheckInDateTime.ToString("MM/dd/yyyy hh:mm tt");
                        dp.Punches.Add(pd);

                        pd = new PunchData();
                        pd.Exception = 0;
                        pd.ExactDate = ews.CheckOutDateTime.ToString("MM/dd/yyyy hh:mm tt");
                        pd.RoundedDate = ews.RoundedCheckOutDateTime.ToString("MM/dd/yyyy hh:mm tt");
                        dp.Punches.Add(pd);
                    }
                    dp.Rounded += Convert.ToDouble(ewsList[i].TotalHoursWorked);
                    //dp.Exact += dp.Punches
                    ed.Days.Add(dp);
                }

                ed.Reg = Convert.ToDouble(eh.TotalRegularHours);
                ed.OT = Convert.ToDouble(eh.TotalOTHours);
                ed.Total = Convert.ToDouble(eh.TotalHours);

                shiftData.Reg += ed.Reg;
                shiftData.OT += ed.OT;
                shiftData.Total += ed.Total;

                shiftData.Employees.Add(ed);
            }
            if (shiftData != null)
            {
                report.Reg += shiftData.Reg;
                report.OT += shiftData.OT;
                report.Total += shiftData.Total;
                report.shifts.Add(shiftData);
            }
            return report;
        }

        /* DNR methods are actually in the EmployeeInfoDNRClass */
        public List<DNRInfo> GetDnr(string aident, string clientID, string shiftID, string deptID, string start, string end, string locationID)
        {
            EmployeeInfoDB empInfoDB = new EmployeeInfoDB();
            return empInfoDB.GetEmployeeInfoByAident(aident, clientID, shiftID, deptID, start, end, locationID);
        }

        public int SetDnr(string name, string id, string client, string shift, string reason, string supervisor, string start, string loc)
        {
            EmployeeInfoDB empInfoDB = new EmployeeInfoDB();
            return empInfoDB.SetDnr(name, id, client, shift, reason, supervisor, start, loc);
        }

        public List<string> GetDnrReasons()
        {
            EmployeeInfoDB empInfoDB = new EmployeeInfoDB();
            return empInfoDB.GetDNRReasons();
        }

        public int DeleteDnr(string dnrRec, string userId)
        {
            EmployeeInfoDB empInfoDB = new EmployeeInfoDB();
            return empInfoDB.DeleteDnrRecord(dnrRec, userId);
        }
        public int UpdateClockTick(string dt)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.UpdateClockTick(dt);
        }

        public Client GetClientByUserName(string userName)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetClientByUserName(userName);
        }
        public ClientDDLMobile MobileGetClientsByUserName(string userName)
        {
            ClientDB cdb = new ClientDB();
            return cdb.MobileGetClientsByUserName(userName);
        }

        public ArrayList GetClientsByUserName(string userName)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetClientsByUserName(userName);
        }

        public string PunchesOutside(int clientRosterID, DateTime startDate, DateTime endDate, string startTime, string endTime)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.PunchesOutside(clientRosterID, startDate, endDate, startTime, endTime);
        }

        public string CreateRoster(int clientID, int loc, string aident, int shiftType, int dept, string office, DateTime startDate, DateTime endDate)
        {
            RosterDB rosterDB = new RosterDB();

            return rosterDB.CreateRoster(clientID, loc, aident, shiftType, dept, office, startDate, endDate);
        }

        public ClientPreferences GetClientPreferencesByID(int clientID)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetClientPreferencesByID(clientID);
        }

        public string GetShiftTimes(string id, string loc, string dept, string shiftType)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetShiftTimes(id, loc, dept, shiftType);
        }

        public Client GetClientShiftTypes(Client clientInfo, int loc)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetClientShiftTypes(clientInfo, loc);
        }

        public Client GetClientShiftsByLocation( Client client, int locIdx )
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetClientShiftTypes(client, locIdx);
        }
        public ArrayList GetClientDepartmentsByShiftType(Client clientInfo, ShiftType shiftType, int loc=0)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetClientDepartmentsByShiftType(clientInfo, shiftType, loc);
        }

        public ArrayList GetClientDepartmentsByShiftType(Client clientInfo, ShiftType shiftType, string UserName, int loc=0)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetClientDepartmentsByShiftType(clientInfo, shiftType, UserName, loc);
        }
        public string GetIDFromSuncastNum(string id)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetIDFromSuncastNum(id);
        }
        public string DnrTrim(string id, string clientID, string shiftID)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.DnrTrim(id, clientID, shiftID);
        }

        public string TrimRoster(int rosterID)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.TrimRoster(rosterID);
        }
        public List<Roster> GetEmployeeRosters(String id, String startDate, String endDate, String clientID)
        {
            RosterDB rosterDB = new RosterDB();
            DateTime start = Convert.ToDateTime(startDate);
            while (start.DayOfWeek != DayOfWeek.Sunday)
                start = start.AddDays(-1);
            DateTime end = Convert.ToDateTime(endDate);
            while (end.DayOfWeek != DayOfWeek.Saturday)
                end = end.AddDays(1);
            return rosterDB.GetEmployeeRosters(id, start, end, clientID);
        }

        public string GetRosters(int clientID, string aident, DateTime startDate, DateTime endDate)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetRosters(clientID, aident, startDate, endDate);
        }
        public string GetRosters(int clientRosterID)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetRosters(clientRosterID);
        }
        public List<Roster> GetPrintRosters(int clientID, DateTime startDate, DateTime endDate, int dept, int shift)
        {
            //Response.Redirect("~/Reports/Roster.aspx", "_blank", "menubar=0,scrollbars=1,width=780,height=900,top=10");
            Server.Transfer("~/auth/InvoiceSummary.aspx", false);
            RosterDB rdb = new RosterDB();
            return rdb.GetPrintRosters(clientID, startDate, endDate, dept, shift);
        }
        public RosterInfo GetRostersWithLocation(int id, DateTime date, int loc, int dept, int shift)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetRostersWithLocation(id, date, loc, dept, shift);
        }
        public string GetRosters(int clientID, int loc, DateTime startDate, DateTime endDate, int dept, int shift)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetRosters(clientID, loc, startDate, endDate, dept, shift);
        }
        public string GetAvailableEmployees(int clientID, int numDays, int dpt, int shift, int loc)
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetAvailableEmployees(clientID, numDays, dpt, shift, loc);
        }
        public int UpdateRoster(int rosterID, DateTime start, DateTime end, string startTime, string endTime, string trackStart, string trackEnd, string subs)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.UpdateRoster(rosterID, start, end, startTime, endTime, trackStart, trackEnd, subs);
        }
        public int RemoveRoster(int rosterID)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.RemoveRoster(rosterID);
        }

        public List<ClientDDL> GetActiveClients(string userID)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetActiveClients(userID);
        }

        /* for DNR */
        public List<ShiftData> GetClientShifts(string clientID)
        {
            EmployeeInfoDB empInfDB = new EmployeeInfoDB();
            return empInfDB.GetClientShifts(clientID);
        }


        public Client GetClientShifts(Client clientInfo, int locationId = 0)
        {
            ClientDB clientDB = new ClientDB();
            return clientDB.GetClientShifts(clientInfo, locationId);
        }

        public ArrayList GetSwipeDepartments(Client client)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetSwipeDepartments(client);
        }

        public DepartmentPayRate GetDepartmentPayRates(PayRateInput inputInfo, string UserName)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetDepartmentPayRates(inputInfo, UserName);
        }

        public DepartmentJobCode GetDepartmentJobCodes(JobCodeInput inputInfo, string UserName)
        {
            ClientDB clientDB = new ClientDB();

            return clientDB.GetDepartmentJobCodes(inputInfo, UserName);
        }

        public List<Office> GetFullDispatchOffices()
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetFullDispatchOffices();
        }

        public string GetDispatchOffices()
        {
            RosterDB rosterDB = new RosterDB();
            return rosterDB.GetDispatchOffices();
        }

    }
}