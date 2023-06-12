using System;
using System.Collections.Generic;
using MSI.Web.MSINet.BusinessEntities;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using System.IO;
using MSI.Web.MSINet.Common;
using PunchClock;
using ClientWebServices;

/// <summary>
/// Summary description for IClientWS
/// </summary>

namespace OpenWebServices
{
    [ServiceContract]
    interface IOpenWS
    {
        [OperationContract]
        [WebGet(UriTemplate = "SuncastId?id={aidentNum}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get Suncast Id(s) for an employee based on aident")]
        List<SuncastInfo> SuncastId(String aidentNum);

        [OperationContract]
        [WebGet(UriTemplate = "CreateSuncastId?id={aidentNum}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Create Suncast Id for an employee based on aident")]
        string CreateSuncastId(String aidentNum);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "ClearClientRoster")]
        [Description("Clear the client roster record")]
        string ClearClientRoster(string clientRosterId);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "UploadTransport")]
        [Description("Upload transportation info for employees riding MSI buses")]
        string UploadTransport(string data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "DailyDispatchInfo")]
        [Description("Retrieve dispatch data based on office, date and shift")]
        List<DailyDispatchInfo> GetDailyDispatchInfo(DailyDispatchSettings data);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "DepartmentSupervisors",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Get List of Departments and Supervisors for a given client")]
        List<DepartmentSupervisor> GetDepartmentSupervisors(DepartmentSupervisorReq dsr); //

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "SetSupervisor")]
        [Description("Set supervisor for a given department and shift")]
        string SetSupervisor(SupervisorParams info);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "DepartmentViewHide")]
        [Description("Hide or Unhide a department for a given user at a particular client")]
        string DepartmentViewHide(UserDepartment info);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "LineApprove")]
        [Description("Retrieve dispatch data based on office, date and shift")]
        string LineApprove(List<string> punchIds);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateDailyDispatchInfo")]
        [Description("Update dispatch data based on office, date and shift")]
        string UpdateDailyDispatchInfo(List<DailyDispatchInfo> data);
        
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EmployeeStatus")]
        [Description("Get Status of Employees at a client for a particular date.")]
        List<EmployeeStatus> GetEmployeeStatus(String client, String date);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "GetTransport")]
        [Description("Get all bus riders between start and end dates")]
        List<TransportationPunch> GetTransport(String startDate, String endDate);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "RecordPunch")]
        [Description("Record Clock Punch With Selected Department")]
        RecordSwipeReturn RecordPunch(PunchInfo info);

        [OperationContract]
        [WebInvoke(Method="POST", RequestFormat=WebMessageFormat.Json, BodyStyle=WebMessageBodyStyle.Wrapped, UriTemplate = "UpdateUser")]
        [Description("Update accessible clients and roles for a user")]
        Boolean UpdateUser(UserInfo userInfo);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle=WebMessageBodyStyle.Wrapped, UriTemplate = "Upload")]
        [Description("Upload a text file")]
        String UploadTextFile(FileData fileData);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Test1")]
        [Description("Testing = return Hello along with LastName and FirstName")]
        string Test1(MyClass p);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Test2")]
        [Description("Testing - return string Hello + p")]
        string Test2(string p);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Test3")]
        [Description("Testing - return string Hello There!")]
        string Test3();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "Test4")]
        [Description("return string Hello + str contents")]
        string Test4(Stream str);

        [OperationContract]
        [WebGet(UriTemplate = "PunchData/{clientId}?start={start}&end={end}", ResponseFormat=WebMessageFormat.Xml)]
        [Description("Raw Punches for client between start and end dates.")]
        List<PunchData> PunchData(string start, string end, string clientId);

        [OperationContract]
        [WebGet(UriTemplate = "Punches/{clientId}?start={start}&end={end}&raw={raw}")]
        [Description("Punches for client between start and end dates.  Raw means one record per punch.")]
        HoursReport Punches(string start, string end, string clientId, string raw);

        [OperationContract]
        [WebGet(UriTemplate = "Email/{clientId}?header={header}&body={body}&from={from}&list={list}")]
        [Description("Send an email (body is HTML format) to addresses in list (CSVs)")]
        void Email(string clientId, string header, string body, string from, string list);
        
        [OperationContract]
        [WebGet(UriTemplate = "UpdateRoles/{clientId}?strinp={strinp}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Update description and ratings in employee role table (Used with American Litho")]
        String UpdateRoles(string clientId, string strinp);

        [OperationContract]
        [WebGet(UriTemplate = "CreateUser/{clientId}?userName={userName}&email={email}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Create a new user and set the default client (client id required)")]
        String CreateUser(string clientId, string userName, String email);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword?userName={userName}&email={email}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Reset password for the given username.  Password is emailed to user.")]
        String ResetPassword(String userName, String email);
        
        [OperationContract]
        [WebGet(UriTemplate = "/UpdateResourceGroup?strinp={strinp}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Update the list of employees with specific creative werks skills." + 
                " Input is: resource group name, hours worked, start date, end date.")]
        String UpdateResourceGroupHours(string strinp);

        [OperationContract]
        [WebGet(UriTemplate = "/ResourceGroupHours?cwAidentNumber={cwAidentNumber}&aidentNumber={aidentNumber}&ResourceGroup={resourceGroup}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve number of hours at specific creative werks skills")]
        List<ResourceGroupHours> GetResourceGroupHours(String aidentNumber, String cwAidentNumber, String resourceGroup);

        [OperationContract]
        [WebGet(UriTemplate = "/ResourceGroupInfo", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve names of specific creative werks skills")]
        List<ResourceGroupInfo> GetResourceGroupInfo();

        [OperationContract]
        [WebGet(UriTemplate = "/Roles/?startDate={startDate}&endDate={endDate}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Retrieve American Litho roles for employees who have worked there between start and end dates.")]
        List<RoleInfo> ALithoRoles(string startDate, string endDate);

        [OperationContract]
        [WebGet(UriTemplate = "/GetFirstPunchAndDnr?clientId={clientId}&endDate={endDate}", ResponseFormat = WebMessageFormat.Xml)]
        [Description("Get list of employees at client whose first punch is in the week before endding date and also who has been DNR'd")]
        List<FirstPunchDnr> GetFirstPunchDnr(String clientId, String endDate);

        [OperationContract]
        [WebGet(UriTemplate = "/Users", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of all user names and ids for webtrax users")]
        List<User> GetAllUsers();

        [OperationContract]
        [WebGet(UriTemplate = "/AllRoles", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of all roles available to webtrax users")]
        List<String> GetAllRoles();

        [OperationContract]
        [WebGet(UriTemplate = "/UserRoles?userName={userName}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of all roles assigned to a particular webtrax user")]
        List<String> GetUsersRoles(String userName);

        [OperationContract]
        [WebInvoke(Method="POST",BodyStyle = WebMessageBodyStyle.Bare, UriTemplate="/UploadPunchData", RequestFormat=WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Upload data from Face Recognition (RA05) clocks to server")]
        PunchClockResponse UploadPunchData(PunchClockData pcd);


        [OperationContract]
        [WebInvoke(Method="POST", BodyStyle=WebMessageBodyStyle.Bare, UriTemplate = "/UsersByClient", RequestFormat=WebMessageFormat.Json, 
                ResponseFormat = WebMessageFormat.Json)]
        [Description("Get list of all userNames and ids for webtrax users by client")]
        List<User> GetUsersByClient(int clientId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/Weber/PunchExceptions", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("")]
        List<PunchException> RetrievePunchExceptions(string date);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/Weber/GetPunches", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("")]
        List<PunchData> RetrievePunches(string date);
    }
}