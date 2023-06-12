using MSI.Web.MSINet.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

/// <summary>
/// Summary description for ICLock
/// </summary>
namespace ClockWebServices
{
    [ServiceContract]
    interface IClockWS
    {
        [OperationContract]
        [WebGet(UriTemplate = "Employees?clientId={clientId}&createdDate={createdDate}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve list of employees scheduled to work at a particular client")]
        ClientRosterLastUpdate EmployeeList(string clientId, string createdDate);

        [OperationContract]
        [WebGet(UriTemplate = "TaskAvailable?clientId={clientId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve the next available task request - result=false means no task")]
        ClockTask TaskAvailable(string clientId);

        [OperationContract]
        [WebGet(UriTemplate = "ClientRosterLastUpdate?clientId={clientId}&locationId={locationId}", ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve the time of last update to client_roster table")]
        ClientRosterLastUpdate ClientRosterLastUpdate(string clientId, string locationId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "FileSave", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [Description("Save content to path/file, extension .b64 added")]
        TextFile SaveTextFile(string aident, string content);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "FileSaveB64AndJpg", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [Description("Save content to path/file, extension .b64 added")]
        TextFile SaveTextAndImageConvert(string aident, string content);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "FileGet")]
        [Description("Retrieve content from path/file - extension .b64 added")]
        TextFile GetTextFile(string aident);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "SetICCardAident")]
        [Description("Match IC Card serial number to Employee ID")]
        string SetICCardAident(string aident, string icCardNum);
    }
}