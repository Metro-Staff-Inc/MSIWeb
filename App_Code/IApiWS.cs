using System.ServiceModel;
using System.ServiceModel.Web;
using System.ComponentModel;
using ApiWebServices_PunchData;
using ApiWebServices_HoursData;
using ApiWebServices_EmployeeInfo;
using PunchClock;
using MSI.Web.MSINet.DataAccess;

using System.Web.Services;
//using Api

/// <summary>
/// Summary description for IClientWS
/// </summary>

namespace ApiWebServices
{

    [ServiceContract]
    interface IApiWS
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ClientPunchesFlat", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve flat list of employee punches at client for a given time period")]
        PunchResponseFlat PunchesFlat(PunchRequest pr);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ClientPunches", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve nested list of employee punches at client for a given time period")]
        string ClientPunches(PunchRequest pr);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/HoursFlat", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve list of employee hours at client for a given payperiod")]
        HoursResponseFlat ClientHoursFlat(HoursRequest pr);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/HoursFlat?clientId={clientId}&weekEndDate={weekEndDate}&userName={userName}&pwd={pwd}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve list of employee hours at client for a given payperiod")]
        HoursResponseFlat ClientHoursFlatGet(string clientId, string weekEndDate, string userName, string pwd);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/ClientPunchesTest", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve nested list of employee punches at client for a given time period")]
        string ClientPunchesTest();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/UpdateEmployee", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Update an employee's information")]
        EmployeeInfoResponse UpdateEmployeeInfo(EmployeeInfo e);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/MobilePunch", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Receive punch-in data from mobile phone")]
        MobileDataOut MobilePunch(MobileDataIn mp);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/MPunch", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Receive punch-in data from mobile phone")]
        MobileDataOut MPunch();

        [OperationContract]
        [WebInvoke(Method ="POST", BodyStyle=WebMessageBodyStyle.WrappedRequest, 
            UriTemplate = "/BridgfordEmployeeData", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description( "Retrieve info for Bridgford Employees")]
        BridgfordOut BridgfordEmployeeData(BridgfordIn brIn);

        /*
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetEmployee", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve an employee's information")]
        EmployeeInfoResponse DaysWorked(EmployeeInfo e);
        */
        /*
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetEmployee", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [Description("Retrieve an employee's information")]
        EmployeeInfoResponse GetEmployeeInfo(EmployeeInfo e);
        */
    }
}