using MSI.Web.MSINet.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

/// <summary>
/// Summary description for ClockWS
/// </summary>
namespace ClockWebServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ClockWS : IClockWS
    {
        public ClockWS()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public ClientRosterLastUpdate EmployeeList(string clientId, string createdDate)
        {
            ClientRosterLastUpdate clientRosterLastUpdate = new ClientRosterLastUpdate();
            ClockBL clockBL = new ClockBL();
            try
            {
                return clockBL.EmployeeList(Convert.ToInt32(clientId), Convert.ToDateTime(createdDate));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                clientRosterLastUpdate.Success = false;
                clientRosterLastUpdate.Msg = ex.ToString();
                return clientRosterLastUpdate;
            }
        }

        public ClockTask TaskAvailable(string clientId)
        {
            ClockTask clockTask = new ClockTask();
            clockTask.Result = true;
            clockTask.TaskName = "UpdateEmployees";
            clockTask.Time = "" + DateTimeHelpers.CSTToClockTicks(DateTime.Now);
            return clockTask;
        }
        public ClientRosterLastUpdate ClientRosterLastUpdate(string clientIdStr, string locationIdStr)
        {
            int clientId = Convert.ToInt32(clientIdStr);
            int locationId = Convert.ToInt32(locationIdStr);

            ClockBL clockBL = new ClockBL();

            ClientRosterLastUpdate clientRosterLastUpdate = clockBL.ClientRosterLastUpdate(clientId, locationId);
            return clientRosterLastUpdate;
        }
        public TextFile SaveTextFile(string aident, string content)
        {
            ClockBL clockBL = new ClockBL();
            TextFile tf = clockBL.SaveTextFile(aident, content);
            return tf;
        }

        public TextFile SaveTextAndImageConvert(string aident, string content)
        {
            ClockBL clockBL = new ClockBL();
            TextFile tf = clockBL.SaveTextFile(aident, content);
            tf.Extension = "jpg";
            clockBL.SaveB64AsJpg(tf);
            return tf;
        }

        public string SetICCardAident(string aident, string icCardNum)
        {
            string msg = "";
            ClockBL clockBL = new ClockBL();
            msg = clockBL.SetICCardAident(aident, icCardNum);
            return msg;
        }

        public TextFile GetTextFile(string aident)
        {
            ClockBL clockBL = new ClockBL();
            TextFile tf = clockBL.GetTextFile(aident);
            return tf;
        }
    }
}