using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using MSI.Web.MSINet.BusinessEntities;
using System.ServiceModel.Web;
using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using Twilio;
using Twilio.TwiML;
using MSI.Web.MSINet.DataAccess;

namespace RosterWebServices
{
    [ServiceContract]
    public interface IRosterWS
    {
        [OperationContract]
        [WebGet(UriTemplate = "/PBUpdate?aident={aident}&notes={notes}&userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Update employee notes used in the phone blast app.")]
        String UpdateEmployeeNotes(String aident, String notes, String userId);

        [OperationContract]
        [WebGet(UriTemplate = "/SetGMPInfo?csvStr={csvStr}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Set the Gmp table using a csv list. 3 items per record - id, hire_dt, gmp_dt")]
        String SetGMPInfo(String csvStr);

        [OperationContract]
        [WebGet(UriTemplate = "/GMPInfo?start={start}&end={end}&client={client}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve GMP Date and Hire Date of client (likely JBSS)")]
        List<GMPInfo> GetGMPInfo(string start, string end, string client);

        [OperationContract]
        [WebGet(UriTemplate = "/MovePunch/{punchId}?departmentId={departmentId}&shiftType={shiftType}&userName={userName}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Move punch to another department / shift in the hours report")]
        string MovePunchDeptShift(String punchId, String departmentId, String shiftType, String userName);

        [OperationContract]
        [WebGet(UriTemplate = "/PBListUpdate/{aident}?customListID={customListID}&action={action}&userID={userID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Add or remove employees from custom phone list.")]
        void PBListUpdate(string aident, string customListID, string action, string userID);

        [OperationContract]
        [WebGet(UriTemplate = "/SendHtmlEmail?title={title}&body={body}&emailAddrs={emailAddrs}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Send an email with an HTML body")]
        void SendHTMLEMail(string title, string body, string emailAddrs);

        [OperationContract]
        [WebGet(UriTemplate = "/FingerprintInfo/{type}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get names of fingerprint files - 0 = .fpt, 1 = .fst, 2 = .xml")]
        List<String> GetFingerprintInfo(String type);

        [OperationContract]
        [WebGet(UriTemplate = "/SkillList/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve employees given Skill descriptions")]
        RecruitPool GetSkillList(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/SetSkillList/{aident}?skillDescriptionId={skillDescriptionId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Set an employee's given skill descriptions")]
        void SetSkillList(string aident, string skillDescriptionId);

        [OperationContract]
        [WebGet(UriTemplate = "/SkillDescriptions/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve Skill descriptions to be placed in a drop down list")]
        List<SkillDescription> GetSkillDescriptions(string id);

        [OperationContract]
        [WebGet(UriTemplate = "/WorkSchedule/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve employees upcoming work schedule")]
        String GetWorkSchedule(String id);

        [OperationContract]
        [WebGet(UriTemplate = "/PhoneBlastLists/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve the names of custom phone blast lists")]
        List<PhoneBlastList> GetPhoneBlastLists(string id);
        
        [OperationContract]
        [WebGet(UriTemplate = "/PhoneBlastList/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve the names of custom phone blast lists")]
        RecruitPool GetPhoneBlastList(string id);

        [OperationContract]
        [WebGet(UriTemplate = "CreateETicket?clientID={clientID}&locID={locID}&shiftType={shiftType}&deptID={deptID}&weekEnd={weekEnd}",
                                    ResponseFormat = WebMessageFormat.Json)]
        [Description("Create an ETicket Header record - returns the header ID")]
        int CreateETicketRoster(string clientID, string locID, string shiftType, string deptID, string weekEnd);

        [OperationContract]
        [WebGet(UriTemplate = "TTDelete?id={id}&punchID={punchID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Delete an employee punch")]
        bool DeletePunch(string id, string punchID);

        [OperationContract]
        [WebGet(UriTemplate = "ETicket?clientID={clientID}&locID={locID}&weekEnd={weekEnd}&deptID={deptID}&shiftType={shiftType}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve eticket info for a particular shift")]
        EmployeeHours GetETicketRoster(string clientID, string locID, string weekEnd, string deptID, string shiftType);

//        public int UpdateEmployeeHours(String employeeHoursHeaderId, String aidentNumber, String userName, String weekEnd, String notes,
//                String hours1, String hours2, String hours3, String hours4, String hours5, String hours6, String hours7)
        [OperationContract]
        [WebGet(UriTemplate = "UpdateETicket?employeeHoursHeaderId={employeeHoursHeaderId}&aidentNumber={aidentNumber}&userName={userName}&weekEnd={weekEnd}&notes={notes}&" + 
            "hours1={hours1}&hours2={hours2}&hours3={hours3}&hours4={hours4}&hours5={hours5}&hours6={hours6}&hours7={hours7}", 
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Update ETicket row given the employee hours header ID")]
        int UpdateEmployeeHours(String employeeHoursHeaderId, String aidentNumber, String userName, String weekEnd, String notes,
                String hours1, String hours2, String hours3, String hours4, String hours5, String hours6, String hours7);

        [OperationContract]
        [WebGet(UriTemplate = "DWGetDaysWorked?id={id}&clientID={clientID}&startDate={startDate}&endDate={endDate}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get the number of days worked at a client for a particular employee id")]
        DaysWorkedItem GetDaysWorkedReportSingleUser(string id, string clientID, string startDate, string endDate);

        [OperationContract]
        [WebGet(UriTemplate = "PBGetWorkStatus?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get information on a particular work status")]
        string GetWork(string id);

        [OperationContract]
        [WebGet(UriTemplate = "PBGetCall?id={id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get information on a particular call")]
        string GetCall(string id);

        [OperationContract]
        [WebGet(UriTemplate = "PBMakeCall?to={to}&msgUrl={msgUrl}&callback={callback}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Make a call through the Twilio interface")]
        string MakeCall(string to, string msgUrl, string callback);

        [OperationContract]
        [WebGet(UriTemplate = "PBSendSMS?to={to}&msgTxt={msgTxt}&fromText={fromText}&fromCall={fromCall}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Make a text message through the Twilio interface")]
        string SendSMS(string to, string msgTxt, string fromText, string fromCall);

        [OperationContract]
        [WebGet(UriTemplate = "PBJobGreeting", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Message to play when MakeCall runs")]
        XElement JobGreeting();

        [OperationContract]
        [WebGet(UriTemplate = "PBJobQuery", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Message to play when MakeCall runs")]
        XElement JobQuery();

        [OperationContract]
        [WebGet(UriTemplate = "PBCallQueue", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Handle waiting to talk to dispatch operator")]
        XElement CallQueue();

        //JHM [OperationContract]
        //JHM [WebGet(UriTemplate = "PBCallback", ResponseFormat = WebMessageFormat.Xml)]
        //JHM [Description("callback for the PBMakeCall method -- update status of call here")]
        //JHM Action<Call> PBCallback();

        [OperationContract]
        [WebGet(UriTemplate = "ReceiveText", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Receive text and decide how to respond.")]
        XElement ReceiveText();

        [OperationContract]
        [WebGet(UriTemplate = "ReceiveCall", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Receive call and decide how to route.")]
        XElement ReceiveCall();

        [OperationContract]
        [WebGet(UriTemplate = "BrowserCall", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Make a call from the browser")]
        XElement BrowserCall();

        [OperationContract]
        [WebGet(UriTemplate = "HandleRecruitResponse", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Caller Selected option to connect to Jobs Listings.")]
        XElement HandleRecruitResponse();

        [OperationContract]
        [WebGet(UriTemplate = "HandleITResponse", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Caller selected option to connect to IT Dept.")]
        XElement HandleITResponse();

        [OperationContract]
        [WebGet(UriTemplate = "RecruitMessageRecorded", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Callback for when message to recruiting department is finished.")]
        XElement RecruitMessageRecorded();

        [OperationContract]
        [WebGet(UriTemplate = "ITMessageRecorded", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Callback for when message to I.T. department is finished.")]
        XElement ITMessageRecorded();

        [OperationContract]
        [WebGet(UriTemplate = "EmailSuncast?clientID={clientID}&tempsOnly={tempsOnly}&deptID={deptID}&shiftType={shiftType}&shiftID={shiftID}&date={date}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Send an email of all employees for a shift, using Suncast format")]
        String EmailSuncast(string clientID, string tempsOnly, string deptID, string shiftType, string shiftID, string date);
        
        [OperationContract]
        [WebGet(UriTemplate = "Validate?userName={userName}&pwd={pwd}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Validate User's name and password - for mobile logon")]
        String ValidateUser(string userName, string pwd);

        [OperationContract]
        [WebGet(UriTemplate = "EmailEmployeeRoster?clientID={clientID}&deptID={deptID}&addr={addr}&start={start}&end={end}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Send out an email of all employees in a department")]
        void EmailEmployeeRoster(string clientID, string deptID, string addr, string start, string end);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/EmployeeRosters?startDate={startDate}&endDate={endDate}&clientID={clientID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("retrieve any and all rosters for employee with id between the start date and end date")]
        List<Roster> GetEmployeeRosters(string id, string startDate, string endDate, string clientID);

        [OperationContract]
        [WebGet(UriTemplate = "Fingerprints/{id}")]
        [Description("Return true if fingerprints exist for given ID")]
        string FingerprintsExist(string id);

        [OperationContract]
        [WebGet(UriTemplate = "FingerprintList/{id}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Get all fingerprints in the shiftdata directory and tempShift directory")]
        FingerprintInfo GetFingerprints(string id);

        [OperationContract]
        [WebGet(UriTemplate = "Photo/{id}")]
        [Description("Return true if photots exist for given ID")]
        string PhotoExists(string id);

        [OperationContract]
        [WebGet(UriTemplate = "VideoUrl?clientID={clientId}&date={date}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Return the url for a security video given ID, date/time, and client")]
        string VideoUrl(string clientId, string date);

        [OperationContract]
        [WebGet(UriTemplate="Hello?time={time}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Return string Hello!!! (for testing)")]
        string SayHello(string time);

        [OperationContract]
        [WebGet(UriTemplate = "Hello/{name}")]
        [Description("Return string Hello {name}!!! (for testing)")]
        string SayHelloWithName(string name);

        [OperationContract]
        [WebGet(UriTemplate = "{client}/Files?id={id}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Get List of all images of ID at CLIENT")]
        string[] PunchPics(string client, string id);

        [OperationContract]
        [WebGet(UriTemplate = "{id}?numDaysStr={numDaysStr}&dept={dept}&shiftType={shiftType}&loc={loc}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Get all employees who worked at given client over the past numDays")]
        string GetAvailableEmployees(string id, string numDaysStr, string shiftType, string dept, string loc);
        
        [OperationContract]
        [WebGet(UriTemplate = "EmployeeInfo/{id}")]
        [Description("Get employee info given aident number.")]
        EmployeeHistory GetEmployee(string id);

        [OperationContract]
        [WebGet(UriTemplate = "Dispatch", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get All Dispatch Office Names and IDs")]
        string GetDispatchOffices();

        [OperationContract]
        [WebGet(UriTemplate = "FullDispatch", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get all info for all Dispatch Offices")]
        List<Office> GetFullDispatchOffices();

        [OperationContract]
        [WebGet(UriTemplate = "Id/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info given aident number.")]
        string GetEmployeeNameAndID(string id);

        [OperationContract]
        [WebGet(UriTemplate = "PBId/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info for Phone Blast given aident number.")]
        RecruitPool GetEmployeeNameAndIDPhoneBlast(string id);

        [OperationContract]
        [WebGet(UriTemplate = "Name/{name}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info given name.")]
        string GetEmployeeNameAndIDFromName(string name);

        [OperationContract]
        [WebGet(UriTemplate = "PBName/{LastName}?FirstName={FirstName}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info given name.")]
        RecruitPool GetEmployeeNameAndIDFromNamePhoneBlast(string lastName, string firstName);

        [OperationContract]
        [WebGet(UriTemplate = "Id/Pic/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info given aident number.")]
        string GetEmployeeNameAndID_Pics(string id);

        [OperationContract]
        [WebGet(UriTemplate = "Name/Pic/{name}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get employee info given name.")]
        string GetEmployeeNameAndIDFromName_Pics(string name);

        [OperationContract]
        [WebGet(UriTemplate = "Date/Pic/{days}?clientID={clientID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get all employees who worked within the given number of days.")]
        string GetEmployeeNameAndIDFromDays_Pics(string days, string clientID);

        [OperationContract]
        [WebGet(UriTemplate = "PayRate?aident={aident}&startDate={startDate}&endDate={endDate}&clientId={clientId}&dept={dept}&shift={shift}&payRate={payRate}&user={user}&punchDate={punchDate}", 
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Update Pay Rate for employee for particular department/shift")]
        string UpdatePayRates(string aident, string startDate, string endDate, string clientId, string dept, string shift, string payRate, string user, string punchDate);

        [OperationContract]
        [WebGet(UriTemplate = "Punch/{id}?pwd={pwd}&swipe={swipe}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Wrapper for the WCF RecordSwipe method")]
        RecordSwipeReturn PunchClock(string id, string pwd, string swipe);

        [OperationContract]
        [WebGet(UriTemplate = "Punches/{id}", ResponseFormat=WebMessageFormat.Json)]
        [Description("Given the clientRosterID, return true if any punch data exists for that record")]
        string PunchesExist(string id);

        [OperationContract]
        [WebGet(UriTemplate = "PunchesOutside/{crID}?startDate={startDate}&endDate={endDate}&startTime={startTime}&endTime={endTime}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Given the range of dates and client roster id, return any punches outside the range")]
        string PunchesOutside(string crID, string startDate, string endDate, string startTime, string endTime);

        [OperationContract]
        [WebGet(UriTemplate = "Trim/{crId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Trim the roster to the start and enddate of punches, routine adds in tracking end time of shift plus one minute")]
        string TrimRoster(string crId);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/DnrTrim?clientID={clientID}&shiftID={shiftID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Trim any rosters that are affected by DNR records")]
        string DnrTrim(string id, string clientID, string shiftID);

        [OperationContract]
        [WebGet(UriTemplate = "Update/{crId}?startDate={StartDate}&endDate={EndDate}&startTime={startTime}&endTime={endTime}&trackStart={trackStart}&trackEnd={trackEnd}&subs={subs}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Update the client roster, ALL parameters can be passed in")]
        string UpdateRoster(string crId, string startDate, string endDate, string startTime, string endTime, string trackStart, string trackEnd, string subs);

        [OperationContract]
        [WebGet(UriTemplate = "Remove/{crId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Remove crID from the client roster")]
        string RemoveRoster(string crId);

        [OperationContract]
        [WebGet(UriTemplate = "EmployeeName/{id}/Set?FirstName={FirstName}&LastName={LastName}&addr1={Addr1}&addr2={Addr2}&city={City}&state={State}&zip={Zip}&zip4={Zip4}&phoneAreaCode={PhoneAreaCode}&phonePrefix={PhonePrefix}&phoneLast4={PhoneLast4}&update={Update}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Set information in database about employee")]
        int SetEmployee(string id, string firstName, string lastName, string addr1, string addr2, string city, string state,
                        string zip, string zip4, string phoneAreaCode, string phonePrefix, string phoneLast4, string update);

        [OperationContract]
        [WebGet(UriTemplate = "EmployeeInfo/{id}/Set?FirstName={FirstName}&LastName={LastName}&addr1={Addr1}&addr2={Addr2}&city={City}&state={State}&zip={Zip}&zip4={Zip4}&phoneAreaCode={PhoneAreaCode}&phonePrefix={PhonePrefix}&phoneLast4={PhoneLast4}&update={Update}&updatedBy={UpdatedBy}&email={Email}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Set information in database about employee")]
        int SetEmployeeInfo(string id, string firstName, string lastName, string addr1, string addr2, string city, string state,
                        string zip, string zip4, string phoneAreaCode, string phonePrefix, string phoneLast4, string update, string updatedBy, string email);

        [OperationContract]
        [WebGet(UriTemplate = "{clientId}/DailyInfo?dept={Dept}&shiftType={ShiftType}&date={Date}&badge={BadgeNum}",
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve punch data for a given date, dept, and shift")]
        DailyTracker GetDailyInfo(string clientId, string dept, string shiftType, string date, string badgeNum);

        [OperationContract]
        [WebGet(UriTemplate = "{punchId}/UpdatePunch?userID={userID}&month={Month}&day={Day}&year={Year}&hour={Hour}&min={Min}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Add or update punchData")]
        String UpdatePunch(string userID, string punchId, string month, string day, string year, string hour, string min);

        [OperationContract]
        [WebGet(UriTemplate = "RecruitPool?clientID={clientID}&locationID={locationID}&shiftType={shiftType}&departmentID={departmentID}&startDate={startDate}&endDate={endDate}&dnrClient={dnrClient}", 
            ResponseFormat=WebMessageFormat.Json)]
        [Description("Get list of all employees who have worked at Client, Location, Department, Shift-Type between given dates")]
        RecruitPool GetRecruitPool(string clientID, string locationID, string departmentID, string shiftType, string startDate, string endDate, string dnrClient); 
    }
}
