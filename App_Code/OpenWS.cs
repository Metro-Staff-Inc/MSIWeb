using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;
using MSI.Web.MSINet.BusinessLogic;
using System.IO;
using System.Security.Principal;
using System.Web.Security;
using System.Web;
using MSI.Web.MSINet.Common;
//using MSIToolkit.Logging;
using PunchClock;
using ClientWebServices;
using System.Data;

namespace OpenWebServices
{
    [DataContract]
    public class Greeting
    {
        [DataMember]
        public string name;
        [DataMember]
        public string greeting;
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OpenWS : IOpenWS
    {
        public OpenWS()
        {
        }

        private decimal _totalHours = 0M;
        private string filePath = HttpContext.Current.Server.MapPath("..\\TEST\\NEIU\\CS200\\FALL2016\\");

        //private string filePath = HttpContext.Current.Server.MapPath("C:\\inetpub\\wwwroot\\");
        //PerformanceLogger log = new PerformanceLogger("AdoNetAppender");

        public List<SuncastInfo> SuncastId(string id)
        {
            OpenBL obl = new OpenBL();
            return obl.SuncastId(id);
        }

        public string CreateSuncastId(string id)
        {
            OpenBL obl = new OpenBL();
            return obl.CreateSuncastId(id);
        }

        public PunchClockResponse UploadPunchData(PunchClockData pcd)
        {
            PunchClockResponse resp = new PunchClockResponse();
            resp.DeviceKey = pcd.DeviceKey;
            resp.Msg = "There are " + pcd.Data.Records.Count + " records.\n";

            foreach (PunchClock.Record r in pcd.Data.Records)
            {
                r.Time = (r.Time / 10) * 10;
                resp.Msg += r.Time + ": ";
                DateTime dt = PunchClockData.ClockTicksToTime(r.Time);
                dt = dt.ToLocalTime();
                r.PunchDt = dt;
                //resp.Msg += r.PersonId + ": " + dt.ToString("MM/dd/yyyy hh:mm:ss tt") + ", ";
            }
            ClockDataDB cdb = new ClockDataDB();
            cdb.InsertClockData(resp, pcd.Data.Records);
            return resp;
        }
        public string ClearClientRoster(string clientRosterId)
        {
            EmployeePunchBL epbl = new EmployeePunchBL();
            string ret = epbl.ClearClientRoster(clientRosterId);
            return ret;
        }

        public List<EmployeeStatus> GetEmployeeStatus(string client, string date)
        {

            EmployeeBL ebl = new EmployeeBL();
            DateTime dt = new DateTime(2018, 4, 10);
            try
            {
                //log.Info("GetEmployeeStatus", client + " |**| " + date);
                dt = Convert.ToDateTime(date);// new DateTime(2018, 04, 02);// Convert.ToDateTime(date);
            }
            catch (Exception e) { }
            int clientID = Convert.ToInt32(client);
            //log.Info("GetEmployeeStatus", dt + ", " + clientID);
            return ebl.GetEmployeeStatus(dt, clientID);
        }
        public string LineApprove(List<string> punchIds)
        {
            string ret = "";// (punchIds.Count-1) + " punches.";
            EmployeePunchBL epbl = new EmployeePunchBL();
            ret += "<br/>------------------<br/>" + epbl.LineApprove(punchIds) + " Punches Approved";
            return ret;
        }
        public List<DailyDispatchInfo> GetDailyDispatchInfo(DailyDispatchSettings data)
        {
            DailyDispatchBL dbl = new DailyDispatchBL();
            return dbl.getDailyDispatchData(data.date, data.officeCd, data.shiftType, data.weeklyReport);
        }
        public string UpdateDailyDispatchInfo(List<DailyDispatchInfo> data)
        {
            DailyDispatchBL dbl = new DailyDispatchBL();
            /* set date */
            DateTime dt = DateTime.Now;
            return dbl.updateDailyDispatchData(data);
        }
        public string UploadTransport(string data)
        {
            //String data = "('34', '2019-01-03 20:48:51', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('35', '2019-01-03 20:48:54', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('36', '2019-01-03 20:48:58', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('37', '2019-01-03 20:49:52', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('38', '2019-01-03 20:49:56', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('39', '2019-01-03 20:49:58', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),('40', '2019-01-03 20:50:00', '77357', '41.94690010976046', '-87.72041209973395', '44', '2', '29', '00.02.000'),";

            TransportationBL tbl = new TransportationBL();
            //string input = new StreamReader(stream).ReadToEnd();
            string result = tbl.uploadTransport(data);
            return result;
        }
        public List<TransportationPunch> GetTransport(string startDate, string endDate)
        {
            TransportationBL tbl = new TransportationBL();
            DateTime sd = Convert.ToDateTime(startDate);
            DateTime ed = Convert.ToDateTime(endDate);
            return tbl.getTransportationInfo(sd, ed);
        }

        public RecordSwipeReturn RecordPunch(PunchInfo swipeInput)
        {
            RecordSwipeReturn returnVal = new RecordSwipeReturn();

            if (swipeInput != null)
            {
                //validate the credentials
                //validate the password
                if (Membership.ValidateUser(swipeInput.UserName, swipeInput.PWD))
                {
                    GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(swipeInput.UserName), null);

                    //get the client id from the user name
                    ClientBL clientBL = new ClientBL();
                    MSI.Web.MSINet.BusinessEntities.Client client = clientBL.GetClientByUserName(swipeInput.UserName);

                    if (client.ClientID > 0)
                    {
                        DateTime punchDateTime = new DateTime(1, 1, 1);

                        punchDateTime = DateTime.Parse(swipeInput.PunchDate);

                        //record the swipe
                        EmployeePunchSummary punchInfo = new EmployeePunchSummary();
                        punchInfo.ClientID = client.ClientID;
                        punchInfo.TempNumber = swipeInput.BadgeNumber;
                        punchInfo.PunchDateTime = punchDateTime;
                        HelperFunctions helper = new HelperFunctions();
                        punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchDateTime);
                        punchInfo.ManualOverride = false;
                        punchInfo.DeptOverride = swipeInput.Department;
                        punchInfo.ShiftOverride = swipeInput.Shift + 1;
                        EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                        EmployeePunchResult result = employeePunchBL.RecordEmployeePunchDepartmentOverride(punchInfo, userPrincipal);
                        returnVal.PunchSuccess = result.PunchSuccess;
                        returnVal.PunchType = result.PunchType.ToString();
                        returnVal.PunchException = result.PunchException;
                        returnVal.FirstName = result.EmployeePunchSummaryInfo.EmployeeFirstName;
                        returnVal.LastName = result.EmployeePunchSummaryInfo.EmployeeLastName;
                        _totalHours = result.EmployeePunchSummaryInfo.CurrentWeeklyHours;
                    }
                    else
                    {
                        //ERROR: client not authorized to use check in
                        returnVal.SystemErrorCode = "-3";
                    }
                }
            }
            else
            {
                //ERROR input parameters not found
                returnVal.SystemErrorCode = "-2";
            }
            return returnVal;
        }

        public String UploadTextFile(FileData fileData)
        {
            filePath += fileData.Directory;
            string file = Path.Combine(filePath, Path.GetFileName(fileData.FileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                byte[] b = Encoding.ASCII.GetBytes(fileData.Data);
                fs.Write(b, 0, (int)fileData.Data.Length);
            }
            return fileData.Data.Length + " bytes written to file - " + file.ToString();
        }

        public Boolean UpdateUser(UserInfo userInfo)
        {
            Boolean retVal = false;
            List<String> remList = new List<String>();
            List<String> addList = new List<String>();
            String[] roles = Roles.GetAllRoles();

            foreach (String role in roles)
            {
                if (userInfo.Roles.Contains(role))
                {
                    addList.Add(role);
                }
            }
            try
            {
                Roles.RemoveUserFromRoles(userInfo.UserName, Roles.GetRolesForUser(userInfo.UserName));
                Roles.AddUserToRoles(userInfo.UserName, addList.ToArray());
                retVal = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retVal;
        }

        public List<RoleInfo> ALithoRoles(String start, String end)
        {
            DaysWorkedReportBL dwr = new DaysWorkedReportBL();
            return dwr.ALithoRoles(start, end);
        }
        public String ResetPassword(String userName, String email)
        {
            if (email == null)
                email = "";
            OpenBL obl = new OpenBL();
            return obl.ResetPassword(userName, email);
        }

        public String CreateUser(string clientId, string userName, String email)
        {
            if (email == null)
                email = "";
            OpenBL obl = new OpenBL();
            String mu = obl.CreateUser(clientId, userName, email);
            return mu;
        }
        public String UpdateRoles(string clientId, string strinp)
        {
            DaysWorkedReportBL dwr = new DaysWorkedReportBL();
            return dwr.ALithoUpdateRoles(strinp);
        }
        public void Email(string clientId, string header, string body, string from, string list)
        {
            OpenBL obl = new OpenBL();
            obl.Email(clientId, header, body, from, list);
        }
        public string Test1(MyClass p)
        {
            return "Hello " + p.firstName + " " + p.lastName + "!";
        }
        public string Test2(string p)
        {
            return "Hello " + p + "!";
        }
        public string Test3()
        {
            return "Hello There!";
        }
        public string Test4(Stream str)
        {
            StreamReader reader = new StreamReader(str);
            string s = reader.ReadToEnd();

            return "***" + s + "***";
        }
        public HoursReport Punches(string start, string end, string client, string rawSt)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);
            int clientId = Convert.ToInt32(client);
            bool raw = Convert.ToBoolean(rawSt);

            OpenBL obl = new OpenBL();
            return obl.Punches(startDate, endDate, clientId, raw);
        }
        public List<String> GetUsersRoles(String userName)
        {
            OpenBL obl = new OpenBL();
            return obl.GetUsersRoles(userName);
        }
        public List<User> GetAllUsers()
        {
            OpenBL obl = new OpenBL();
            return obl.GetAllUsers();
        }
        public List<User> GetUsersByClient(int id)
        {
            OpenBL obl = new OpenBL();
            List<User> ret = obl.GetUsersByClient(id);
            return ret;
        }

        public List<String> GetAllRoles()
        {
            OpenBL obl = new OpenBL();
            return obl.GetAllRoles();
        }

        public List<PunchData> PunchData(string start, string end, string client)
        {
            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);
            int clientId = Convert.ToInt32(client);

            OpenBL obl = new OpenBL();
            return obl.PunchData(startDate, endDate, clientId);
        }

        public string UpdateResourceGroupHours(String inpList)
        {
            PunchReportBL prbl = new PunchReportBL();
            return prbl.UpdateResourceGroupHours(inpList);
        }

        public List<ResourceGroupInfo> GetResourceGroupInfo()
        {
            PunchReportBL prbl = new PunchReportBL();
            return prbl.GetResourceGroupInfo();
        }
        public List<ResourceGroupHours> GetResourceGroupHours(String aidentNumber, String cwAidentNumber, String resourceGroup)
        {
            PunchReportBL prbl = new PunchReportBL();
            int rg = Convert.ToInt32(resourceGroup);
            return prbl.GetResourceGroupHours(aidentNumber, cwAidentNumber, rg);
        }
        public List<FirstPunchDnr> GetFirstPunchDnr(String clientId, String endDate)
        {
            OpenBL obl = new OpenBL();
            int c = Convert.ToInt32(clientId);
            DateTime d = Convert.ToDateTime(endDate);
            return obl.GetFirstPunchDnr(c, d);
        }
        public List<PunchException> RetrievePunchExceptions(string date)
        {
            OpenBL obl = new OpenBL();
            List<PunchException> list = obl.RetrievePunchExceptions(date);
            return list;
        }
        public List<PunchData> RetrievePunches(string date)
        {
            return new List<PunchData>();
        }
        public string DepartmentViewHide(UserDepartment info)
        {
            OpenBL obl = new OpenBL();
            return obl.DepartmentViewHide(info);
        }
        public string SetSupervisor(SupervisorParams info)
        {
            OpenBL obl = new OpenBL();
            return obl.SetSupervisor(info);
        }
        public List<DepartmentSupervisor> GetDepartmentSupervisors(DepartmentSupervisorReq dsr)
        {
            List<DepartmentSupervisor> deptSupervisor = null;
            try
            {
                ClientBL clientBL = new ClientBL();
                deptSupervisor = clientBL.GetDepartmentSupervisors(dsr.ClientId, dsr.UserId);
                return deptSupervisor;
            }
            catch(Exception ex)
            {
                deptSupervisor = new List<DepartmentSupervisor>();
                DepartmentSupervisor ds = new DepartmentSupervisor
                {
                    ClientId = -1,
                    DepartmentName = ex.ToString()
                };
                deptSupervisor.Add(ds);
                return deptSupervisor;
            }
        }
    }
}