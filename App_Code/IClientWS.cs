using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSI.Web.MSINet.BusinessEntities;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using System.ServiceModel.Activation;

/// <summary>
/// Summary description for IClientWS
/// </summary>

namespace ClientWebServices
{
    [ServiceContract]
    interface IClientWS
    {
        [OperationContract]
        [WebGet(UriTemplate = "RequestEmployees", ResponseFormat = WebMessageFormat.Json)]
        [Description("Send An email to Dispatch")]
        string RequestEmployees();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "DepartmentSupervisors",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Get List of Departments and Supervisors for a given client")]
        List<DepartmentSupervisor> GetDepartmentSupervisors(DepartmentSupervisorReq dsr);

        [OperationContract]
        [WebGet(UriTemplate = "DepartmentInfo?clientId={clientId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get List of Departments, Shifts, and PayRates for a client")]
        List<DepartmentInfo> DepartmentInfo(String clientId);

        [OperationContract]
        [WebGet(UriTemplate = "ShiftDepartments?clientId={clientId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve all shifts and their respective departments given client ID")]
        List<ShiftDepartment> GetShiftDepartments(String clientId);

        [OperationContract]
        [WebGet(UriTemplate = "GetHoursReports/{clientId}?uid={uid}&startDate={startDate}&endDate={endDate}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve multiple hours reports")]
        List<EmpInfo> GetHoursReports(String clientId, String uid, String startDate, String endDate);

        [OperationContract]
        [WebGet(UriTemplate = "GetHoursReport/{clientId}?uid={uid}&endDate={endDate}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve weekly hours report")]
        List<EmpInfo> GetHoursReport(String clientId, String uid, String endDate);

        [OperationContract]
        [WebGet(UriTemplate = "SetEmail/{id}?email={email}&name={name}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Add an email address for a client")]
        void SetEmail(string id, string email, string name);

        [OperationContract]
        [WebGet(UriTemplate = "GetEmail/{id}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get email list for a particular client")]
        List<String> GetEmail(string id);
        
        [OperationContract]
        [WebGet(UriTemplate = "GetDepartmentMapping?clientID={clientID}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get tempwerks to msi dept. mappings")]
        DepartmentMapping GetDepartmentMapping(String clientID);

        [OperationContract]
        [WebGet(UriTemplate = "SetClientMultiplier?clientID={clientID}&multiplier={multiplier}&multiplier2={multiplier2}" + 
                "&otMultiplier={otMultiplier}&otMultiplier2={otMultiplier2}&bonusMultiplier={bonusMultiplier}" + 
                "&otherMultiplier={otherMultiplier}&passThruMultiplier={passThruMultiplier}&vacationMultiplier={vacationMultiplier}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Set client pay rate multiplier")]
        string SetClientMultiplier(String clientID, string multiplier, string multiplier2, string otMultiplier, string otMultiplier2,
                                String bonusMultiplier, String otherMultiplier, String passThruMultiplier, String vacationMultiplier);

        [OperationContract]
        [WebGet(UriTemplate = "SetDepartmentMapping?clientID={clientID}&startDate={startDate}&userID={userID}&list={list}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Set tempwerks to msi dept. mappings")]
        string UpdateMapping(string clientID, string startDate, string userID, string list);

        [OperationContract]
        [WebGet(UriTemplate = "UpdateBonuses?clientID={clientID}&weekEnd={weekEnd}&list={list}&userID={userID}", 
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Update Bonuses")]
        string UpdateBonuses(string clientID, string weekEnd, string list, string userID);

        [OperationContract]
        [WebGet(UriTemplate = "GetAll/{userName}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Return all clients accessible by user name")]
        ClientDDLMobile GetClientsByUserName(String userName);

        [OperationContract]
        [WebGet(UriTemplate = "IP", ResponseFormat = WebMessageFormat.Json)]
        [Description("Return time of last click made by International Paper's computer")]
        DateTime CheckIPClock();

        [OperationContract]
        [WebGet(UriTemplate = "Goodbye")]
        [Description("Return string Goodbye (for testing)")]
        string SayGoodbye();

        [OperationContract]
        [WebGet(UriTemplate = "{id}/ShiftTypes",
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get shift types by client id")]
        List<ShiftType> GetShiftTypes(string id);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/ShiftsByLocation?loc={loc}",
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get shift types by client id and client location")]
        List<ShiftType> GetShiftTypesClientLocation(string id, string loc);

        [OperationContract]
        [WebGet(UriTemplate = "{aident}/GetDnr?clientID={clientID}&shiftID={shiftID}&deptID={deptID}&start={start}&end={end}&locationID={locationID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get all DNR Info for given ID")]
        List<DNRInfo> GetDnr(string aident, string clientID, string shiftID, string deptID, string start, string end, string locationID);

        [OperationContract]
        [WebGet(UriTemplate = "{dnrID}/DeleteDnr?userID={userID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get all DNR Info for given ID")]
        int DeleteDnr(string dnrID, string userID);

        [OperationContract]
        [WebGet(UriTemplate = "SetDnr?id={id}&name={name}&client={client}&shift={shift}&reason={reason}&supervisor={supervisor}&start={start}&location={location}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Set DNR for a particular employee (id)")]
        int SetDnr(string name, string id, string client, string shift, string reason, string supervisor, string start, string location);

        [OperationContract]
        [WebGet(UriTemplate = "GetDnrReasons", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get ALL DNR Reasons")]
        List<string> GetDnrReasons();

        [OperationContract]
        [WebGet(UriTemplate = "Shifts/{clientID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get ALL shifts for given client ID")]
        List<ShiftData> GetClientShifts(string clientID);

        [OperationContract]
        [WebGet(UriTemplate = "{clientID}/WeeklyReport?id={id}&start={start}&end={end}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retreive all punches for all shifts between start and end dates")]
        WeeklyReport GetWeeklyReport(string clientID, string id, string start, string end);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/ShiftTime?loc={loc}&dept={dept}&shiftType={shiftType}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get shift start and end times")]
        String GetShiftTimes(string id, string loc, string dept, string shiftType);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/Departments/{shiftType}",
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get departments by client id and shift")]
        List<Department> GetDepartments(string id, string shiftType);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/AllDepartments",
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get departments by client id and shift")]
        List<Department> GetAllDepartments(string id);

        [OperationContract]
        [WebGet(UriTemplate = "{clientID}/GetDepartments?locationID={locationID}&shiftType={shiftType}", 
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get departments by client id, location, and shift")]
        List<Department> GetDepartmentsCLS(string clientID, string locationID, string shiftType);

        [OperationContract]
        [WebGet(UriTemplate = "{userID}/Active", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of clients with client_roster activity in the last year")]
        List<ClientDDL> GetActiveClients(string userID);

        [OperationContract]
        [WebGet(UriTemplate = "{userID}/Locations?clientID={clientID}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of locations for given client")]
        List<LocationDDL> GetClientLocations(string userID, string clientID);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/CreateRoster?loc={loc}&aident={aident}&shift={shift}&dept={dept}&office={office}&startDate={startDate}&endDate={endDate}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Create a roster for employee with aident number")]
        string CreateRoster(string id, string loc, string aident, string shift, string dept, string office, string startDate, string endDate);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/PrintRosters?startDate={startDate}&endDate={endDate}&shift={shift}&dept={dept}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get Rosters between start and end date inclusive, for the given department, shift and client id")]
        List<Roster> GetPrintRosters(string id, string startDate, string endDate, string dept, string shift);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/Rosters?loc={loc}&startDate={startDate}&endDate={endDate}&shift={shift}&dept={dept}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get Rosters between start and end date inclusive, for the given department, shift and client id")]
        string GetRosters(string id, string loc, string startDate, string endDate, string dept, string shift);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/RostersWithLocation?date={date}&shift={shift}&dept={dept}&loc={loc}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get Rosters for given date, for the given department, shift, location and client id")]
        RosterInfo GetRostersWithLocation(string id, string date, string loc, string dept, string shift);

        [OperationContract]
        [WebGet(UriTemplate = "RostersAt?clientRosterID={clientRosterID}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get ALL Rosters at given client between start and end date")]
        string GetOtherRosters(string clientRosterID);

        [OperationContract]
        [WebGet(UriTemplate = "SuncastNum/{id}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Get Employee ID # from the temporary Suncast number.")]
        string GetIDFromSuncastNum(string id);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/RostersAt?aident={aident}&startDate={startDate}&endDate={endDate}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Get ALL Rosters at given client between start and end date")]
        string GetClientRosters(string id, string aident, string startDate, string endDate);

        [OperationContract]
        [WebGet(UriTemplate = "{id}/AddPunch?crID={crID}&punchDate={punchDate}&deptID={deptID}&userID={userID}&shiftType={shiftType}",
            ResponseFormat = WebMessageFormat.Json)]
        [Description("Manually add a punch for an employee")]
        string AddPunch(string id, string crID, string punchDate, String deptID, String userID, String shiftType);
    }
}
