using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TransportationBL
/// </summary>
/// 
namespace MSI.Web.MSINet.BusinessLogic
{
    public class TransportationBL
    {
        public TransportationBL() {}
        public string uploadTransport(String rides)
        {
            TransportationDB tdb = new TransportationDB();
            String ret = tdb.UpdateTransportInfo(rides).ToString();
            if( ret.ToUpper().Contains("ERROR") || ret[0] == '0' )
            {
                HelperFunctions hf = new HelperFunctions();
                List<String> emailList = new List<string>();
                emailList.Add("jmurfey@msistaff.com");
                hf.SendHTMLEMail("ERROR IN UPLOADING TRANSPORT DATA", ret + "\n---------\n" + rides, "jmurfey@msistaff.com", emailList);
            }
            return ret;
        }
        public Dictionary<string, Vehicle> getVehicleUseInfo(DateTime start, DateTime end)
        {
            TransportationDB tdb = new TransportationDB();
            Dictionary<string, Vehicle> list = tdb.getVehicleUseInfo(start, end);
            return list;
        }
        public List<DriverData> getDailyDriverData(DateTime date)
        {
            TransportationDB tdb = new TransportationDB();
            List<DriverData> list = tdb.getDailyDriverData(date);
            return list;
        }
        public List<TransportationPunch> getTransportationInfo(DateTime start, DateTime end)
        {
            TransportationDB tdb = new TransportationDB();
            List<TransportationPunch> list = tdb.getTransportationInfo(start, end);
            return list;
        }
    }
}
