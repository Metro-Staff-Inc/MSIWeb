using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using System.Security.Principal;

namespace MSI.Web.MSINet.Common
{
    public class WebTraxPing
    {
        public event EventHandler PingCompleted;

        protected void OnPingCompleted(RecordSwipeReturn result)
        {
            if (PingCompleted != null)
            {
                PingCompleted(result, EventArgs.Empty);
            }
        }

        public void ExecutePing()
        {
            RecordSwipeReturn pingResult = this.sendPing();

            //raise event that ping was finished
            this.OnPingCompleted(pingResult);
        }

        private RecordSwipeReturn sendPing()
        {
            RecordSwipeReturn returnVal = new RecordSwipeReturn(); ;

            //ping the web service
            DateTime punchInputDT = new DateTime(1, 1, 1);
            DateTime punchReturnDT = new DateTime(1, 1, 1);
            HelperFunctions helper = new HelperFunctions();
            try
            {
                //record the swipe
                EmployeePunchSummary punchInfo = new EmployeePunchSummary();
                punchInfo.ClientID = 165;
                punchInfo.TempNumber = "TR00000";
                punchInfo.PunchDateTime = helper.GetCSTCurrentDateTime();
                punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchInfo.PunchDateTime);
                punchInfo.ManualOverride = false;
                EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity("msitimeclock"), null);
                EmployeePunchResult result = employeePunchBL.RecordEmployeePunch(punchInfo, userPrincipal);
                returnVal.PunchSuccess = result.PunchSuccess;
                returnVal.PunchType = result.PunchType.ToString();
                returnVal.PunchException = result.PunchException;
                returnVal.FirstName = result.EmployeePunchSummaryInfo.EmployeeFirstName;
                returnVal.LastName = result.EmployeePunchSummaryInfo.EmployeeLastName;
            }
            catch (Exception ex)
            {
                returnVal.PunchSuccess = false;
                returnVal.SystemErrorCode = ex.ToString();
            }
            finally
            {

            }

            return returnVal;

        }
    }
}
