using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using MSI.Web.MSINet.BusinessEntities;
//using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.DataAccess;
using MSI.Web.MSINet.BusinessLogic;
using System.IO;
using WebServicesLocation;
using MSI.Web.Services;
using System.Xml.Linq;
using Twilio;
//JHM//using MSIToolkit.Logging;

namespace RosterWebServices 
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class RosterWS : IRosterWS
    {
        
        public String UpdateEmployeeNotes(String aident, String notes, String userId)
        {
            PhoneBlastBL pbl = new PhoneBlastBL();
            return pbl.UpdateEmployeeNotes(aident, notes, userId);
        }
        public String GetWorkSchedule(String id)
        {
            PhoneBlastBL pbl = new PhoneBlastBL();
            return pbl.GetWorkSchedule(id);
        }
        public String SetGMPInfo(String csvStr)
        {
            EmployeePunchBL epbl = new EmployeePunchBL();
            return epbl.SetGMPInfo(csvStr);
        }

        public List<GMPInfo> GetGMPInfo(String start, String end, String clientId)
        {
            EmployeePunchBL epbl = new EmployeePunchBL();
            return epbl.GetGMPInfo(start, end, clientId);
        }

        public string MovePunchDeptShift(String punchId, String departmentId, String shiftType, String userName)
        {
            HoursReportBL hrbl = new HoursReportBL();
            return hrbl.MovePunchDeptShift(punchId, departmentId, shiftType, userName);
        }
        public void PBListUpdate(string aident, string customListID, string action, string userID)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            pbbl.PBListUpdate(aident, customListID, action, userID);
        }
        public List<PhoneBlastList> GetPhoneBlastLists(string id)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.GetPhoneBlastLists();
        }

        public List<SkillDescription> GetSkillDescriptions(string id)
        {
            EmployeeBL ebl = new EmployeeBL();
            return ebl.GetSkillDescriptions();
        }
        public RecruitPool GetSkillList(string id)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.GetSkillList(id);
        }
        public void SetSkillList(string aident, string skillDescriptionId)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            pbbl.SetSkillList(aident, skillDescriptionId);
        }

        public RecruitPool GetPhoneBlastList(string id)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            RecruitPool rp = pbbl.GetPhoneBlastList(id);
            return rp;
        }
        public List<String> GetFingerprintInfo(String type)
        {
            EmployeeBL ebl = new EmployeeBL();
            return ebl.GetFingerprintInfo(type);
        }
        public FingerprintInfo GetFingerprints(String id)
        {
            EmployeeBL ebl = new EmployeeBL();
            FingerprintInfo f =  ebl.GetFingerprints();
            f = new FingerprintInfo();
            return f;
        }
        public RecruitPool GetRecruitPool(string clientID, string locationID, string departmentID, string shiftType, string startDate, string endDate, string dnrClient)
        {
            //JHM//PerformanceLogger log = new PerformanceLogger("AdoNetAppender");
            DaysWorkedReportBL dwbl = new DaysWorkedReportBL();
            //JHMlog.Info("GetRecruitPool", "Created DaysWorkedReportBL object");

            RecruitPool pool = dwbl.GetRecruitPool(clientID, locationID, departmentID, shiftType, startDate, endDate, dnrClient/*JHM, log*/);
            //JHMlog.Info("GetRecruitPool", "Pool data returned");

            return pool;
        }
        public bool DeletePunch(string id, string punchID)
        {
            EmployeePunchBL empbl = new EmployeePunchBL();
            return empbl.DeletePunch(id, punchID);
        }

        public DaysWorkedItem GetDaysWorkedReportSingleUser(string id, string clientID, string start, string end)
        {
            DaysWorkedReportBL dwrBL = new DaysWorkedReportBL();

            return dwrBL.GetDaysWorkedReportSingleUser(clientID, id, start, end);
        }
        //        public int CreateETicketRoster(string clientID, string locID, string shiftType, string deptID, string weekEnd)
        public int CreateETicketRoster(string clientID, string locID, string shiftType, string deptID, string weekEnd)
        {
            EmployeeBL ebl = new EmployeeBL();
            return ebl.CreateETicketRoster(clientID, locID, shiftType, deptID, weekEnd);
        }   
        //        public RosterInfo GetETicketRoster(string clientID, string locID, string weekEnd, string deptID, string shiftType)
        public EmployeeHours GetETicketRoster(string clientID, string locID, string weekEnd, string deptID, string shiftType)
        {
            EmployeeBL ebl = new EmployeeBL();
            return ebl.GetETicketRoster(clientID, locID, weekEnd, deptID, shiftType);
        }
        public int UpdateEmployeeHours(String employeeHoursHeaderId, String aidentNumber, String userName, String weekEnd, String notes,
                String hours1, String hours2, String hours3, String hours4, String hours5, String hours6, String hours7)
        {
            EmployeeBL ebl = new EmployeeBL();
            int ehId = Convert.ToInt32(employeeHoursHeaderId);
            DateTime we = Convert.ToDateTime(weekEnd);
            double h1 = Convert.ToDouble(hours1);
            double h2 = Convert.ToDouble(hours2);
            double h3 = Convert.ToDouble(hours3);
            double h4 = Convert.ToDouble(hours4);
            double h5 = Convert.ToDouble(hours5);
            double h6 = Convert.ToDouble(hours6);
            double h7 = Convert.ToDouble(hours7);
            return ebl.UpdateEmployeeHours(ehId, aidentNumber, userName, we, notes, h1, h2, h3, h4, h5, h6, h7);
        }
        public string GetWork(string id)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            string newId = "+1";
            for (int i = 0; i < id.Length; i++)
                if (id[i] >= '0' && id[i] <= '9')
                    newId += id[i];
            return pbbl.GetPhoneBlast(newId);
        }
        public string GetCall(string id)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.GetCall(id);
        }

        public string MakeCall(string to, string msgUrl, string callback)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.MakeCall(to, msgUrl, callback);
        }

        public string SendSMS(string to, string msgTxt, string fromText, string fromCall)
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.SendSMS(to, msgTxt, fromText, fromCall);
        }

        public XElement JobQuery()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            var response = pbbl.PBJobQuery();
            return response.Element;
        }

        public XElement JobGreeting()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            var response = pbbl.PBJobGreeting();
            return response.Element;
        }

        public XElement CallQueue()
        {
            var response = (new PhoneBlastBL()).PBCallQueue();
            return response.Element;
        }

        //JHM public Action<Call> PBCallback()
        //JHM {
        //JHM PhoneBlastBL pbbl = new PhoneBlastBL();
        //JHM     return pbbl.PBCallBack();
        //JHM }

        public XElement ReceiveText()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.ReceiveText();
        }

        public XElement ReceiveCall()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.ReceiveCall().Element;
        }

        public XElement BrowserCall()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.BrowserCall();
        }
        public XElement HandleITResponse()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.HandleITResponse();
        }
        public XElement HandleRecruitResponse()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.HandleRecruitResponse();
        }
        public XElement ITMessageRecorded()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.ITMessageRecorded();
        }
        public XElement RecruitMessageRecorded()
        {
            PhoneBlastBL pbbl = new PhoneBlastBL();
            return pbbl.RecruitMessageRecorded();
        }

        public string ValidateUser(string userName, string pwd)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.ValidateUser(userName, pwd);
        }
        public String EmailSuncast(string clientID, string tempsOnly, string deptID, string shiftType, string shiftID, string date)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.EmailSuncast(Convert.ToInt32(clientID), Convert.ToInt32(Convert.ToBoolean(tempsOnly)), Convert.ToInt32(deptID), Convert.ToInt32(shiftType),
                Convert.ToInt32(shiftID), Convert.ToDateTime(date));
        }
        public void EmailEmployeeRoster(string clientID, string deptID, string addr, string start, string end)
        {
            EmployeeBL empBL = new EmployeeBL();
            empBL.EmailEmployeeRoster(Convert.ToInt32(clientID), Convert.ToInt32(deptID), addr, Convert.ToDateTime(start), Convert.ToDateTime(end));
        }

        public List<Roster> GetEmployeeRosters(string id, string startDate, string endDate, string clientID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetEmployeeRosters(id, startDate, endDate, clientID); 
        }
        public String PhoneBlast()
        {
            return "<Response><Say>This is a very exciting technology!</Say></Response>";
        }
        public RecordSwipeReturn PunchClock(string clientId, string pwd, string swipe)
        {
            MSIWebTraxCheckIn check = new MSIWebTraxCheckIn();
            return check.RecordSwipe(swipe);
        }
        public TicketTrackerException Exceptions(string aident, string startDate, string endDate, string clientID)
        {
            return null;
        }

        public string RemoveRoster(string crId)
        {
            ClientBL clientBL = new ClientBL();
            return "" + clientBL.RemoveRoster(Convert.ToInt32(crId));
        }
        public string UpdateRoster(string crId, string startDate, string endDate, string startTime, string endTime, string trackStart, string trackEnd, string subs)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.UpdateRoster(Convert.ToInt32(crId), Convert.ToDateTime(startDate), 
                    Convert.ToDateTime(endDate), startTime, endTime, trackStart, trackEnd, subs).ToString();
        }
        public string GetDispatchOffices()
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetDispatchOffices();
        }

        public List<Office> GetFullDispatchOffices()
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.GetFullDispatchOffices();
        }

        public String PunchesOutside(string clientRosterID, string startDate, string endDate, string startTime, string endTime)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.PunchesOutside(Convert.ToInt32(clientRosterID), Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), startTime, endTime);
        }

        public String PunchesExist(string clientRosterID)
        {
            PunchReportBL prBL = new PunchReportBL();
            return prBL.PunchesExist(clientRosterID);
        }
        public string DnrTrim(string id, string clientID, string shiftID)
        {
            ClientBL clientBL = new ClientBL();
            return clientBL.DnrTrim(id, clientID, shiftID);
        }

        public String TrimRoster(string clientRosterID)
        {
            ClientBL clientBL = new ClientBL();

            return clientBL.TrimRoster(Convert.ToInt32(clientRosterID));
        }

        public DailyTracker GetDailyInfo(string clientId, string dept, string shiftType, string date, string badge)
        {
            DailyTracker dtInput = new DailyTracker();

            dtInput.ClientId = Convert.ToInt32(clientId);
            dtInput.PeriodStart = DateTime.Parse(Convert.ToDateTime(date).ToString("MM/dd/yyyy 00:00:00"));
            dtInput.PeriodEnd = DateTime.Parse(Convert.ToDateTime(date).ToString("MM/dd/yyyy 23:59:59"));
            TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
            dtInput.Dept.DepartmentID = Convert.ToInt32(dept);
            dtInput.ShiftType.ShiftTypeId = Convert.ToInt32(shiftType);

            DailyTracker dtResult = ticketTrackerBL.GetDailyInfo(dtInput, badge);

            return dtResult;
        }


        public string[] PunchPics(string client, string id)
        {
            string pathName = @"C:/inetpub/wwwroot/Dropbox/images/" + WebServiceLocation.GetClient(client) + @"/";
            string file = id + "_" + "*.jpg";
            string[] files = Directory.GetFiles(pathName, file);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = "http://msiwebtrax.com/" + files[i].Substring(19);
            }
            return files;
        }

        public string FingerprintsExist(string id)
        {
            String f = @"C:\inetpub\wwwroot\Dropbox\shiftdata";
            return Directory.GetFiles(f, id + "*.fpt").Length > 0 /*|| 
                Directory.GetFiles(f, id + "*.fpt").Length > 0 */? 
                    true.ToString(): false.ToString();
        }

        public string PhotoExists(string id)
        {
            String f = @"C:\inetpub\wwwroot\Dropbox\ids\";
            return Directory.GetFiles(f, "*" + id + ".jpg").Length > 0 ? true.ToString() : false.ToString();
        }

        public string VideoUrl(string clientId, string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            
            String f = @"C:\inetpub\wwwroot\Dropbox\Videos\" + clientId + @"\" + dt.Year +  @"\" + dt.Month + @"\" + dt.Day + @"\";
            f = @"c:/inetpub/wwwroot/Dropbox/Videos/" + clientId + @"/" + dt.Year + @"/" + dt.Month + @"/" + dt.Day + @"/";
            String[] vids;
            try
            {
                vids = Directory.GetFiles(f, "*.mp4");
            }
            catch(System.IO.DirectoryNotFoundException dnfe)
            {
                vids = null;
            }
            finally
            {
            }
            if (vids == null)
                return "";
            for( int i=0; i<vids.Length; i++ )
            {
                // format of video file name is CameraName_20150922 22:58 _YEARMONTHDAY.mp4
                string startDt = vids[i].Substring(vids[i].IndexOf('_')+1, 14);
                string endDt = vids[i].Substring(vids[i].IndexOf('_') + 16, 14);

                DateTime start = toDateTime(startDt);
                DateTime end = toDateTime(endDt);

                if(dt >= start && dt <= end )
                {
                    //return "http://www.msiwebtrax.com/Dropbox/Videos/326/2015/10/15/CamOne_20151015135822_20151015135827.mp4";
                    return "http://www.msiwebtrax.com/Dropbox/" + vids[i].Substring(vids[i].IndexOf("Videos/"));
                }
            }
            return "";
        }
        public DateTime toDateTime(string dt)
        {
            // format of string is 20150922095812 --> 09/22/2015 09:58am 12 seconds
            int year = Convert.ToInt32(dt.Substring(0, 4));
            int month = Convert.ToInt32(dt.Substring(4, 2));
            int day = Convert.ToInt32(dt.Substring(6, 2));
            int hour = Convert.ToInt32(dt.Substring(8, 2));
            int min = Convert.ToInt32(dt.Substring(10, 2));
            int sec = Convert.ToInt32(dt.Substring(12, 2));

            DateTime ret = new DateTime(year, month, day, hour, min, sec);
            return ret;
        }

        public string SayHello(string time)
        {
            DateTime dateTime = Convert.ToDateTime(time);
            ClientBL clientBL = new ClientBL();
            return "Number of rows set = " + clientBL.UpdateClockTick(time);
        }

        public string SayHelloWithName(string name)
        {
            return String.Format("<body>Hello {0}!!!</body>", name);
        }

        public int SetEmployee(string id, string firstName, string lastName, string addr1, string addr2,
                            string city, string state, string zip, string zip4, string phoneAreaCode,
                            string phonePrefix, string phoneLast4, string update)
        {
            EmployeeBL empBL = new EmployeeBL();
            EmployeeHistory empHist = new EmployeeHistory();
            empHist.AIdentNumber = id;
            empHist.FirstName = firstName;
            empHist.LastName = lastName;
            empHist.Addr1 = addr1;
            empHist.Addr2 = addr2;
            empHist.Zip = zip;
            empHist.Zip4 = zip4;
            empHist.PhoneAreaCode = phoneAreaCode;
            empHist.PhoneLast4 = phoneLast4;
            empHist.PhonePrefix = phonePrefix;
            empHist.City = city;
            empHist.State = state;
            if (update == null || update.Length == 0)
                update = "false";
            empHist.Update = Convert.ToBoolean(update);
            empHist.UpdatedBy = "";
            return empBL.SetEmployee(empHist, 2);
        }

        public string UpdatePayRates(string aident, string startDate, string endDate, string clientId, string dept, string shift, string payRate, string user, string punchDate)
        {
            string aidentTemp = "";
            for (int i = 0; i < aident.Length; i++)
                if (aident[i] < '0' || aident[i] > '9')
                    continue;
                else
                    aidentTemp += aident[i];
            InvoiceBL ibl = new InvoiceBL();
            ClientPayOverride cpo = new ClientPayOverride();
            cpo.ShiftType = Convert.ToInt32(shift);
            cpo.DepartmentId = Convert.ToInt32(dept);
            cpo.ExpirationDate = DateTime.Parse(endDate);
            cpo.EffectiveDate = DateTime.Parse(startDate);
            cpo.AidentNumber = aidentTemp;
            cpo.ClientId = Convert.ToInt32(clientId);
            cpo.PayRate = Convert.ToDecimal(payRate);
            cpo.firstPunch = DateTime.Parse(punchDate);
            //cpo.ClientId = 302;
            ClientPayOverride res = ibl.AddClientPayOverride(cpo, null, user);
            if( res.ClientPayOverrideId >= 0 )
                return "Pay Rate Updated!";
            return "Pay Rate Already Exists!";
        }

        public int SetEmployeeInfo(string id, string firstName, string lastName, string addr1, string addr2,
                            string city, string state, string zip, string zip4, string phoneAreaCode,
                            string phonePrefix, string phoneLast4, string update, string updatedBy, string email)
        {
            EmployeeBL empBL = new EmployeeBL();
            EmployeeHistory empHist = new EmployeeHistory();
            empHist.AIdentNumber = id;
            empHist.FirstName = firstName;
            empHist.LastName = lastName;
            empHist.Addr1 = addr1;
            empHist.Addr2 = addr2;
            empHist.Zip = zip;
            empHist.Zip4 = zip4;
            empHist.PhoneAreaCode = phoneAreaCode;
            empHist.PhoneLast4 = phoneLast4;
            empHist.PhonePrefix = phonePrefix;
            empHist.City = city;
            empHist.State = state;
            if (update == null || update.Length == 0)
                update = "false";
            empHist.Update = Convert.ToBoolean(update);
            empHist.UpdatedBy = updatedBy;
            empHist.Email = email;
            return empBL.SetEmployee(empHist, 2);
        }

        public string GetAvailableEmployees(string idStr, string numDaysStr, string shiftType, string dept, string loc)
        {
            int locId = Convert.ToInt32(loc);
            int st = Convert.ToInt32(shiftType);
            int d = Convert.ToInt32(dept);
            int id = Convert.ToInt32(idStr);
            int numDays = Convert.ToInt32(numDaysStr);
            ClientBL clientBL = new ClientBL();
            return clientBL.GetAvailableEmployees(id, numDays, d, st, locId);
        }

        public string GetEmployeeNameAndID(string ID)
        {
            EmployeeHistory empHist = GetEmployee(ID);
            string s = "<table id='tblAvailableEmployees'><thead><tr><th>ID</th><th>Name</th><th>Add</th></tr></thead>";

            s = s + "<tbody><tr id='" + empHist.AIdentNumber + "'><td>" + empHist.AIdentNumber + "</td><td>" + empHist.LastName + ", " + empHist.FirstName + "</td>"
                /* + "(" + empHist.PhoneAreaCode + ") " + empHist.PhonePrefix + " - " + empHist.PhoneLast4 + "</td>"*/
                + "<td><input type='button' onclick='addToRoster(\"" + empHist.AIdentNumber + "\")' value='Add'/></td></tr></tbody>";
            s = s + "</table>";
            return s;
        }
        public RecruitPool GetEmployeeNameAndIDFromNamePhoneBlast(string lastName, string firstName)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.GetEmployeeByNamePhoneBlast(lastName, firstName);
        }

        public RecruitPool GetEmployeeNameAndIDPhoneBlast(string ID)
        {
            EmployeeHistory empHist = GetEmployee(ID);
            RecruitPool rp = new RecruitPool();
            rp.RecruitPoolCollection = new List<RecruitPoolItem>();
            RecruitPoolItem rpi = new RecruitPoolItem();
            rpi.BadgeNumber = empHist.AIdentNumber;
            rpi.DaysWorked = new List<int>();
            rpi.DaysWorked.Add(0);
            rpi.DeptId = 0;
            rpi.Depts = new List<string>();
            rpi.Depts.Add("--");
            rpi.FingerprintsExist = "--";
            rpi.FirstName = empHist.FirstName;
            rpi.LastName = empHist.LastName;
            rpi.DnrReason = "active";
            rpi.PhoneNum = "(" + empHist.PhoneAreaCode + ") " + empHist.PhonePrefix + "-" + empHist.PhoneLast4;
            rp.RecruitPoolCollection.Add(rpi);
            return rp;
        }

        public string GetEmployeeNameAndID_Pics(string ID)
        {
            EmployeeHistory empHist = GetEmployee(ID);
            string s = "<table id='tblAvailableEmployees'><thead><tr><th>ID</th><th>Name</th><th>View</th></tr></thead>";

            s = s + "<tbody><tr id='" + empHist.AIdentNumber + "'><td>" + empHist.AIdentNumber + "</td><td>" + empHist.LastName + ", " + empHist.FirstName + "</td>"
                /* + "(" + empHist.PhoneAreaCode + ") " + empHist.PhonePrefix + " - " + empHist.PhoneLast4 + "</td>"*/
                + "<td><input type='button' onclick='getPics(\"" + empHist.AIdentNumber + "\")' value='View'/></td></tr></tbody>";
            s = s + "</table>";
            return s;
        }

        public EmployeeHistory GetEmployee(string id)
        {
            EmployeeLookup lookup = new EmployeeLookup();
            lookup.AidentNumber = id;
            EmployeeBL empBL = new EmployeeBL();
            return empBL.GetEmployeeByAident(lookup);
        }

        public string GetEmployeeNameAndIDFromName(string name)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.GetEmployeeByName(name);
        }

        public string GetEmployeeNameAndIDFromName_Pics(string name)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.GetEmployeeByName_Pics(name);
        }
        public String UpdatePunch(string userID, string punchId, string month, string day, string year, string hour, string min)
        {
            EmployeePunchBL empBL = new EmployeePunchBL();
            return empBL.UpdatePunch(userID, punchId, month, day, year, hour, min);

        }
        public string GetEmployeeNameAndIDFromDays_Pics(string days, string clientID)
        {
            EmployeeBL empBL = new EmployeeBL();
            return empBL.GetEmployeeByDate_Pics(days, clientID);
        }
        public void SendHTMLEMail(string title, string body, string emailAddrs)
        {
            EmployeeBL ebl = new EmployeeBL();
            String[] emails = emailAddrs.Split(',');
            List<String> emailList = new List<string>(emails);
            ebl.SendHTMLEMail(title, body, emailList);
        }
    }
}