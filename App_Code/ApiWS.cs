using MSI.Web.MSINet.BusinessLogic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ApiWebServices_PunchData;
using Newtonsoft.Json;
using ApiWebServices_HoursData;
using System;
using System.Text;
using System.IO;
using ApiWebServices_EmployeeInfo;
using PunchClock;
using System.Web.Services;
using MSI.Web.MSINet.DataAccess;
using System.Collections.Generic;

/// <summary>
/// Summary description for ApiWS
/// </summary>
namespace ApiWebServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [WebService(Namespace = "https://msiwebtrax.com/")]


    public class ApiWS : IApiWS
    {
        public ApiWS()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public PunchResponseFlat PunchesFlat(PunchRequest pr)
        {
            ApiBL apibl = new ApiBL();
            PunchResponseFlat punchResponse = apibl.RetrievePunchesFlat(pr);
            return punchResponse;
        }
        public string ClientPunches(PunchRequest pr)
        {
            ApiBL apibl = new ApiBL();
            PunchResponse punchResponse = apibl.RetrievePunches(pr);

            string s = JsonConvert.SerializeObject(punchResponse);
            return s;
        }
        public HoursResponseFlat ClientHoursFlat(HoursRequest pr)
        {
            ApiBL apibl = new ApiBL();
            HoursResponseFlat hoursResponse = apibl.ClientHours(pr);

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';
                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, hoursResponse);
            }
            return hoursResponse;// sb.ToString();
        }
        public HoursResponseFlat ClientHoursFlatGet(string clientId, string weekEndDate, string userName, string pwd)
        {
            ApiBL apibl = new ApiBL();
            HoursRequest hr = new HoursRequest();
            hr.ClientID = Convert.ToInt32(clientId);
            if (weekEndDate != null)
            {
                hr.WeekEndDate = weekEndDate;
            }
            else
            {
                hr.WeekEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            hr.UserName = userName;
            hr.Password = pwd;
            return ClientHoursFlat(hr);
        }
        public string ClientPunchesTest()
        {
            PunchRequest pr = new PunchRequest();
            pr.StartDate = "03/01/2020";
            pr.EndDate = "03/06/2020";

            pr.ClientID = 299;
            pr.UserName = "hawksd";
            pr.Password = "whitenoon";
            return ClientPunches(pr);
        }

        public EmployeeInfoResponse UpdateEmployeeInfo(EmployeeInfo e)
        {
            //Console.WriteLine(e.LastName + "," + e.FirstName);
            ApiBL apiBL = new ApiBL();
            EmployeeInfoResponse resp = apiBL.UpdateEmployeeInfo(e);
            resp.Data = e;
            return resp;
        }
        public EmployeeInfoResponse GetEmployeeInfo(EmployeeInfo e)
        {
            ApiBL apibl = new ApiBL();
            EmployeeInfoResponse eir = apibl.GetEmployeeInfo(e);
            return eir;
        }

        public MobileDataOut MPunch()
        {
            MobileDataOut mdo = new MobileDataOut();
            mdo.ClientName = "Jonathan Murfey";
            mdo.Message = "Successfully contacted server";
            return mdo;
        }
        public MobileDataOut MobilePunch(MobileDataIn mp)
        {
            ApiBL apiBL = new ApiBL();
            MobileDataOut mdo = apiBL.MobilePunch(mp);
            return mdo;
        }
        public BridgfordOut BridgfordEmployeeData(BridgfordIn brIn)
        {
            ApiBL apiBL = new ApiBL();
            BridgfordOut bfo = new BridgfordOut();
            bfo = apiBL.BridgfordEmployeeData(brIn);
            bfo.Date = brIn.Date;
            return bfo;
        }
    }
}