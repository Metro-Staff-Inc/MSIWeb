using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using System.ServiceModel.Activation;
using MSI.Web.Controls;
using System.ServiceModel;

/// <summary>
/// Summary description for ClientWS
/// </summary>

namespace ClientWebServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ClientWS : IClientWS
    {
        public List<DepartmentInfo> DepartmentInfo(String clientId)
        {
            ClientBL clientBL = new ClientBL();
            int client = Convert.ToInt32(clientId);
            List<DepartmentInfo> di = clientBL.GetDepartmentInfo(clientId);
            return di;
        }
        public List<EmpInfo> GetHoursReport(String clientId, String uid, String endDate)
        {
            ClientBL clientBl = new ClientBL();
            HoursReport hr = clientBl.GetHoursReport(Convert.ToInt32(clientId), uid, Convert.ToDateTime(endDate));
            return null;
        }
        public List<EmpInfo> GetHoursReports(String clientId, String uid, String startDate, String endDate)
        {
            ClientBL clientBl = new ClientBL();
            HoursReport hr= clientBl.GetHoursReports(Convert.ToInt32(clientId), uid, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
            HoursReportMin hrm = new HoursReportMin();
            hrm.employees = new List<EmpInfo>();
            for( int i=0; i<hr.EmployeeHistoryCollection.Count; i++ )
            {
                EmpInfo ei = new EmpInfo();
                EmployeeHistory eh = (EmployeeHistory)hr.EmployeeHistoryCollection[i];
                ei.Id = eh.TempNumber;
                ei.LName = eh.LastName;
                ei.FName = eh.FirstName;
                ei.Reg = eh.TotalRegularHours;
                ei.OT = eh.TotalOTHours;
                ei.Dept = ((EmployeeWorkSummary)eh.WorkSummaries[0]).DepartmentInfo.DepartmentName;
                ei.Shift = ((EmployeeWorkSummary)eh.WorkSummaries[0]).ShiftTypeInfo.ShiftTypeId;
                ei.Pay = eh.PayRate;
                //ei.MaxPay = eh.PayRate;
                hrm.employees.Add(ei);
            }
            return hrm.employees;
        }

        public List<LocationDDL> GetClientLocations(string userID, string clientID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetClientLocations(userID, clientID);
        }

        public List<ShiftDepartment> GetShiftDepartments(String clientId)
        {
            int client = Convert.ToInt32(clientId);
            HoursReportBL hrbl = new HoursReportBL();
            return hrbl.GetShiftDepartments(client);
        }

        public string RequestEmployees()
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.RequestEmployees();
        }
        public string SetClientMultiplier(string clientID, string multiplier, string multiplier2,
                string otMultiplier, string otMultiplier2, string bonusMultiplier, string otherMultiplier, 
            string passThruMultiplier, string vacationMultiplier)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.SetClientMultiplier(clientID, multiplier, multiplier2, otMultiplier, otMultiplier2,
                            bonusMultiplier, otherMultiplier, passThruMultiplier, vacationMultiplier);
        }
        public string AddPunch(String idIn, String crIDIn, String pd, String deptIDIn, String userID, String shiftTypeIn)
        {
            ClientBL clientBL = new ClientBL();
            DateTime punchDate = DateTime.Now;
            int deptID = Convert.ToInt32(deptIDIn);
            int shiftType = Convert.ToInt32(shiftTypeIn);
            try
            {
                punchDate = Convert.ToDateTime(pd);
            }
            catch (Exception ex)
            {
                /* invalid date */
                Console.WriteLine(ex);
                return null;
            }
            return clientBL.AddPunch(idIn, crIDIn, punchDate, deptID, userID, shiftType);
        }

        public void SetEmail(string id, string email, string name)
        {
            ClientBL cbl = new ClientBL();
            cbl.SetEmail(id, email, name);
        }
        public List<String> GetEmail(string id)
        {
            ClientBL cbl = new ClientBL();
            return cbl.GetEmail(id);
        }

        public string UpdateMapping(string clientID, string startDate, string userID, string list)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.UpdateMapping(clientID, startDate, userID, list);
        }

        public string UpdateBonuses(string clientID, string weekEnd, string list, string userID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.UpdateBonuses(clientID, weekEnd, list, userID);
        }

        public ClientDDLMobile GetClientsByUserName(String userName)
        {
            ClientBL cbl = new ClientBL();
            ClientDDLMobile Clients = cbl.MobileGetClientsByUserName(userName);
            return Clients;
        }
        public DateTime CheckIPClock()
        {
            ClientBL cbl = new ClientBL();
            return cbl.CheckIPClock();
        }
        public string SayGoodbye()
        {
            return "Goodbye";
        }

        public DepartmentMapping GetDepartmentMapping(String clientID)
        {
            ClientBL cbl = new ClientBL();
            return cbl.GetDepartmentMapping(Convert.ToInt32(clientID));
        }


        public WeeklyReport GetWeeklyReport(string clientId, string id, string start, string end)
        {
            ClientBL clientBL = new ClientBL();
            string[] d = end.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(d[2]), Convert.ToInt32(d[0]), Convert.ToInt32(d[1]));
            DateTime st;

            if (start != null && start.Length > 0)
            {
                d = start.Split('/');
                st = new DateTime(Convert.ToInt32(d[2]), Convert.ToInt32(d[0]), Convert.ToInt32(d[1]));
            }
            else
                st = new DateTime(1900, 1, 1);
            return clientBL.GetWeeklyReport(clientId, id, st, dt);
        }
        public int DeleteDnr(string dnrRec, string userId)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.DeleteDnr(dnrRec, userId);
        }

        public List<ShiftData> GetClientShifts(string clientID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetClientShifts(clientID);
        }

        public int SetDnr(string name, string id, string client, string shift, string reason, string supervisor, string start, string location)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.SetDnr(name, id, client, shift, reason, supervisor, start, location);
        }
        public List<DNRInfo> GetDnr(string aident, string clientID, string shiftID, string deptID, string start, string end, string locationID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetDnr(aident, clientID, shiftID, deptID, start, end, locationID);
        }
        public List<string> GetDnrReasons()
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetDnrReasons();
        }
        public List<Department> GetDepartments(string clientID, string shiftType)
        {
            ClientBL clientBL = new ClientBL();
            int id = Convert.ToInt32(clientID);
            int st = Convert.ToInt32(shiftType);
            Client clientInfo = new Client();
            clientInfo.ClientID = id;
            clientInfo.Departments = new ArrayList();
            ShiftType sType = new ShiftType(st);

            clientInfo.Departments = clientBL.GetClientDepartmentsByShiftType(clientInfo, sType);
            List<Department> depts = new List<Department>();
            foreach (Department dpt in clientInfo.Departments)
                depts.Add(dpt);
            return depts;
        }
        public List<DepartmentSupervisor> GetDepartmentSupervisors(DepartmentSupervisorReq dsr)
        {
            ClientBL clientBL = new ClientBL();
            List<DepartmentSupervisor> deptSupervisor = clientBL.GetDepartmentSupervisors(dsr.ClientId, dsr.UserId);
            return deptSupervisor;
        }
        public List<Department> GetAllDepartments(string clientID)
        {
            ClientBL clientBL = new ClientBL();
            int id = Convert.ToInt32(clientID);

            List<Department> depts = clientBL.GetDepartments(id);
            return depts;
        }

        public List<Department> GetDepartmentsCLS(string clientID, string locationID, string shiftType)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetDepartments(clientID, locationID, shiftType);
        }

        public string CreateRoster(string id, string loc, string aident, string shift, string dept, 
            string office, string startDate, string endDate)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.CreateRoster(Convert.ToInt32(id), Convert.ToInt32(loc), aident, Convert.ToInt32(shift),
                        Convert.ToInt32(dept), office, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
        }

        public List<ClientDDL> GetActiveClients(string userID)
        {
            ClientBL clientBL = new ClientBL();

            return clientBL.GetActiveClients(userID);
        }
        public string GetShiftTimes(string id, string loc, string dept, string shiftType)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetShiftTimes(id, loc, dept, shiftType);
        }
        public string GetIDFromSuncastNum(string id)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetIDFromSuncastNum(id);
        }
        public List<ShiftType> GetShiftTypes(string clientID)
        {
            ClientBL clientBL = new ClientBL();
            int id = Convert.ToInt32(clientID);

            Client clientInfo = new Client();
            clientInfo.ClientID = id;
            clientInfo.ShiftTypes = new ArrayList();
            //clientInfo.ShiftTypes.Clear();
            clientInfo = clientBL.GetClientShiftTypes(clientInfo,0);
            List<ShiftType> shiftType = new List<ShiftType>();
            foreach (ShiftType st in clientInfo.ShiftTypes)
                shiftType.Add(st);
            return shiftType;
        }

        public List<ShiftType> GetShiftTypesClientLocation(string clientID, string locID)
        {
            ClientBL clientBL = new ClientBL();
            int id = Convert.ToInt32(clientID);

            int loc = Convert.ToInt32(locID);

            Client clientInfo = new Client();
            clientInfo.ClientID = id;
            clientInfo.ShiftTypes = new ArrayList();
            //clientInfo.ShiftTypes.Clear();
            clientInfo = clientBL.GetClientShiftTypes(clientInfo, loc);
            List<ShiftType> shiftType = new List<ShiftType>();
            foreach (ShiftType st in clientInfo.ShiftTypes)
                shiftType.Add(st);
            return shiftType;
        }

        public string GetClientRosters(string clientID, string aident, string startDate, string endDate)
        {
            ClientBL clientBL = new ClientBL();
            int id = Convert.ToInt32(clientID);
            if (Convert.ToDateTime(endDate) < DateTime.Now)
                return "";
            return clientBL.GetRosters(id, aident, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
        }
        public string GetOtherRosters(string clientRosterID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetRosters(Convert.ToInt32(clientRosterID));
        }

        public List<Roster> GetPrintRosters(string id, string startDate, string endDate, string dept, string shift)
        {
            ClientBL clientBL = new ClientBL();
            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);

            int ID = Convert.ToInt32(id);
            int Shift = Convert.ToInt32(shift);
            int Dept = Convert.ToInt32(dept);

            return clientBL.GetPrintRosters(ID, sDate, eDate, Dept, Shift);
        }
        public RosterInfo GetRostersWithLocation(string idIn, string dateIn, string locIn, string deptIn, string shiftIn)
        {
            ClientBL clientBL = new ClientBL();

            DateTime date = Convert.ToDateTime(dateIn);

            int id = Convert.ToInt32(idIn);
            int loc = Convert.ToInt32(locIn);
            int shift = Convert.ToInt32(shiftIn);
            int dept = Convert.ToInt32(deptIn);

            return clientBL.GetRostersWithLocation(id, date, loc, dept, shift);
        }

        public string GetRosters(string id, string loc, string startDate, string endDate, string dept, string shift)
        {
            ClientBL clientBL = new ClientBL();
            /* startDate and endDate is in format yyyyMMdd */
            //string dt = startDate.Substring(4, 2) + "/" + startDate.Substring(6, 2) + "/" + startDate.Substring(0, 4);
            DateTime sDate = Convert.ToDateTime(startDate);

            //dt = endDate.Substring(4, 2) + "/" + endDate.Substring(6, 2) + "/" + endDate.Substring(0, 4);
            DateTime eDate = Convert.ToDateTime(endDate);
            int ID = Convert.ToInt32(id);
            int Shift = Convert.ToInt32(shift);
            int Dept = Convert.ToInt32(dept);
            int Loc = Convert.ToInt32(loc);
            string list;// = new List<Roster>();
            list = clientBL.GetRosters(ID, Loc, sDate, eDate, Dept, Shift);

            return list;
        }
    }
}