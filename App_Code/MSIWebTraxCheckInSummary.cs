using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Services.Protocols;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;
//using MSIToolkit.Logging;

namespace MSI.Web.Services
{
    /// <summary>
    /// Summary description for MSIWebTraxCheckInSummary
    /// </summary>
    [WebService(Namespace = "http://msiwebtrax.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MSIWebTraxCheckInSummary : System.Web.Services.WebService
    {
        public UserCredentials CredentialsHeader;
        //PerformanceLogger log = new PerformanceLogger("AdoNetAppender");        
        public MSIWebTraxCheckInSummary()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public RecordSwipeReturnSummary RecordSwipeSummary(string swipeInput)
        {
            //log.Info("RecordSwipeReturnSummary", swipeInput);
            RecordSwipeReturnSummary returnVal = new RecordSwipeReturnSummary();
            HttpRequest req = Context.Request;
            Uri uri = req.Url;
            //return values
            //System Error Codes
            //-1:connection not secure
            //-2:input parameters not found
            //-3:client not authorized
            //-4:invalid input parameter format
            //-5:too few input parameters
            //-6:invalid date

            //if (Context.Request.IsSecureConnection)
            //{
            bool isValid = false;

            if (swipeInput != null && swipeInput.Length > 0)
            {
                //validate the credentials
                //validate the password
                if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
                {
                    GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);

                    //get the client id from the user name
                    ClientBL clientBL = new ClientBL();
                    Client client = clientBL.GetClientByUserName(CredentialsHeader.UserName);

                    if (client.ClientID > 0)
                    {
                        string[] inputParms = swipeInput.Split(new string[] { "|*|" }, StringSplitOptions.None);
                        string badgeNumber = String.Empty;
                        DateTime punchDateTime = new DateTime(1, 1, 1);
                        if (inputParms != null && inputParms.Length > 0)
                        {
                            if (inputParms.Length == 2)
                            {
                                //get the badge number
                                badgeNumber = inputParms[0];
                                //get the punch date/time
                                try
                                {
                                    punchDateTime = DateTime.Parse(inputParms[1]);
                                    isValid = true;
                                }
                                catch (Exception dateEx)
                                {
                                    //ERROR: invalid date
                                    returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-6";
                                }

                                if (isValid)
                                {
                                    //record the swipe
                                    EmployeePunchSummary punchInfo = new EmployeePunchSummary();
                                    punchInfo.ClientID = client.ClientID;
                                    punchInfo.TempNumber = badgeNumber;
                                    punchInfo.PunchDateTime = punchDateTime;
                                    HelperFunctions helper = new HelperFunctions();
                                    punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchDateTime);
                                    punchInfo.ManualOverride = false;

                                    if (client.CalculateSummaryHours)
                                    {
                                        punchInfo.CalculateWeeklyHours = true;
                                    }

                                    EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                                    EmployeePunchResult result = employeePunchBL.RecordEmployeePunch(punchInfo, userPrincipal);
                                    returnVal.RecordSwipeReturnInfo.PunchSuccess = result.PunchSuccess;
                                    returnVal.RecordSwipeReturnInfo.PunchType = result.PunchType.ToString();
                                    returnVal.RecordSwipeReturnInfo.PunchException = result.PunchException;
                                    returnVal.RecordSwipeReturnInfo.FirstName = result.EmployeePunchSummaryInfo.EmployeeFirstName;
                                    returnVal.RecordSwipeReturnInfo.LastName = result.EmployeePunchSummaryInfo.EmployeeLastName;
                                    returnVal.CalculateWeeklyHours = client.CalculateSummaryHours;
                                    returnVal.CurrentWeeklyHours = result.EmployeePunchSummaryInfo.CurrentWeeklyHours;
                                }
                            }
                            else
                            {
                                //ERROR too few input parameters
                                returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-5";
                            }
                        }
                        else
                        {
                            //ERROR:invalid input parameter format
                            returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-4";
                        }
                    }
                    else
                    {
                        //ERROR: client not authorized to use check in
                        returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-3";
                    }
                }
            }
            else
            {
                //ERROR input parameters not found
                returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-2";
            }
            //}
            //else
            //{
            //ERROR Connection not secure
            //    returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-1";
            // }
            //log.Info("RecordSwipeReturnSummary", "Return val: " + returnVal.RecordSwipeReturnInfo.SystemErrorCode);
            return returnVal;
        }
        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public RecordSwipeReturnSummary RecordSwipeSummaryDepartmentOverride(string swipeInput)
        {
            //log.Info("RecordSwipeReturnSummary", swipeInput);
            RecordSwipeReturnSummary returnVal = new RecordSwipeReturnSummary();
            HttpRequest req = Context.Request;
            Uri uri = req.Url;

            //return values
            //System Error Codes
            //-1:connection not secure
            //-2:input parameters not found
            //-3:client not authorized
            //-4:invalid input parameter format
            //-5:too few input parameters
            //-6:invalid date

            //if (Context.Request.IsSecureConnection)
            //{
            bool isValid = false;

            if (swipeInput != null && swipeInput.Length > 0)
            {
                //validate the credentials
                //validate the password
                if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
                {
                    GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);

                    //get the client id from the user name
                    ClientBL clientBL = new ClientBL();
                    Client client = clientBL.GetClientByUserName(CredentialsHeader.UserName);

                    if (client.ClientID > 0)
                    {
                        string[] inputParms = swipeInput.Split(new string[] { "|*|" }, StringSplitOptions.None);
                        string badgeNumber = String.Empty;
                        DateTime punchDateTime = new DateTime(1, 1, 1);
                        int deptOverride = 0;
                        if (inputParms != null && inputParms.Length > 0)
                        {
                            if (inputParms.Length == 3)
                            {
                                //get the badge number
                                badgeNumber = inputParms[0];
                                //get the punch date/time
                                try
                                {
                                    punchDateTime = DateTime.Parse(inputParms[1]);
                                    deptOverride = Int32.Parse(inputParms[2]);
                                    isValid = true;
                                }
                                catch (Exception dateEx)
                                {
                                    //ERROR: invalid date
                                    returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-6";
                                }

                                if (isValid)
                                {
                                    //record the swipe
                                    EmployeePunchSummary punchInfo = new EmployeePunchSummary();
                                    punchInfo.ClientID = client.ClientID;
                                    punchInfo.TempNumber = badgeNumber;
                                    punchInfo.PunchDateTime = punchDateTime;
                                    punchInfo.DeptOverride = deptOverride;
                                    HelperFunctions helper = new HelperFunctions();
                                    punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchDateTime);
                                    punchInfo.ManualOverride = false;

                                    if (client.CalculateSummaryHours)
                                    {
                                        punchInfo.CalculateWeeklyHours = true;
                                    }

                                    EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                                    EmployeePunchResult result = employeePunchBL.RecordEmployeePunchDepartmentOverride(punchInfo, userPrincipal);
                                    returnVal.RecordSwipeReturnInfo.PunchSuccess = result.PunchSuccess;
                                    returnVal.RecordSwipeReturnInfo.PunchType = result.PunchType.ToString();
                                    returnVal.RecordSwipeReturnInfo.PunchException = result.PunchException;
                                    returnVal.RecordSwipeReturnInfo.FirstName = result.EmployeePunchSummaryInfo.EmployeeFirstName;
                                    returnVal.RecordSwipeReturnInfo.LastName = result.EmployeePunchSummaryInfo.EmployeeLastName;
                                    returnVal.CalculateWeeklyHours = client.CalculateSummaryHours;
                                    returnVal.CurrentWeeklyHours = result.EmployeePunchSummaryInfo.CurrentWeeklyHours;
                                }
                            }
                            else
                            {
                                //ERROR too few input parameters
                                returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-5";
                            }
                        }
                        else
                        {
                            //ERROR:invalid input parameter format
                            returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-4";
                        }
                    }
                    else
                    {
                        //ERROR: client not authorized to use check in
                        returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-3";
                    }
                }
            }
            else
            {
                //ERROR input parameters not found
                returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-2";
            }
            //}
            //else
            //{
            //ERROR Connection not secure
            //    returnVal.RecordSwipeReturnInfo.SystemErrorCode = "-1";
            // }

            //log.Info("RecordSwipeReturnSummary", "Return val: " + returnVal.RecordSwipeReturnInfo.SystemErrorCode);
            return returnVal;
        }

    }
}
