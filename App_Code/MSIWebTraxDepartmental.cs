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
    /// Summary description for MSIWebTraxDepartmental
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class MSIWebTraxDepartmental : System.Web.Services.WebService
    {
        public UserCredentials CredentialsHeader;
        //PerformanceLogger log = new PerformanceLogger("AdoNetAppender");        
        public MSIWebTraxDepartmental()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        [System.Xml.Serialization.XmlInclude(typeof(Department))]
        public ArrayList GetSwipeDepartments()
        {
            ArrayList returnVal = new ArrayList();

            if (Membership.ValidateUser(CredentialsHeader.UserName, CredentialsHeader.PWD))
            {
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(CredentialsHeader.UserName), null);

                //get the client id from the user name
                ClientBL clientBL = new ClientBL();
                Client client = clientBL.GetClientByUserName(CredentialsHeader.UserName);

                if (client.ClientID > 0)
                {
                    returnVal = clientBL.GetSwipeDepartments(client);
                }
                else
                {
                    //ERROR: client not authorized to use check in
                }
            }

            return returnVal;
        }

        [WebMethod]
        [SoapHeader("CredentialsHeader")]
        public RecordDepartmentSwipeReturn RecordDepartmentSwipe(string swipeInput)
        {
            //log.Info("RecordDepartmentSwipe", swipeInput);

            RecordDepartmentSwipeReturn returnVal = new RecordDepartmentSwipeReturn();

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
                        int departmentId = 0;
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
                                    isValid = true;
                                }
                                catch (Exception dateEx)
                                {
                                    //ERROR: invalid date
                                    returnVal.SystemErrorCode = "-6";
                                }

                                if (isValid)
                                {
                                    //get the department id
                                    departmentId = int.Parse(inputParms[2]);
                                    //record the swipe
                                    EmployeePunchSummary punchInfo = new EmployeePunchSummary();
                                    punchInfo.ClientID = client.ClientID;
                                    punchInfo.TempNumber = badgeNumber;
                                    punchInfo.PunchDateTime = punchDateTime;
                                    HelperFunctions helper = new HelperFunctions();
                                    punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchDateTime);
                                    punchInfo.ManualOverride = false;
                                    punchInfo.TicketInfo.DepartmentInfo.DepartmentID = departmentId;
                                    EmployeePunchBL employeePunchBL = new EmployeePunchBL();
                                    EmployeeDepartmentPunchResult result = employeePunchBL.RecordEmployeeDepartmentPunch(punchInfo, userPrincipal);
                                    returnVal.PunchSuccess = result.PunchSuccess;
                                    returnVal.PunchType = result.PunchType.ToString();
                                    returnVal.PunchException = result.PunchException;
                                    returnVal.FirstName = result.EmployeePunchSummaryInfo.EmployeeFirstName;
                                    returnVal.LastName = result.EmployeePunchSummaryInfo.EmployeeLastName;
                                    returnVal.PunchDisplayText = result.PunchDisplayText;
                                }
                            }
                            else
                            {
                                //ERROR too few input parameters
                                returnVal.SystemErrorCode = "-5";
                            }
                        }
                        else
                        {
                            //ERROR:invalid input parameter format
                            returnVal.SystemErrorCode = "-4";
                        }
                    }
                    else
                    {
                        //ERROR: client not authorized to use check in
                        returnVal.SystemErrorCode = "-3";
                    }
                }
            }
            else
            {
                //ERROR input parameters not found
                returnVal.SystemErrorCode = "-2";
            }
            //}
            //else
            //{
            //ERROR Connection not secure
            //    returnVal.SystemErrorCode = "-1";
            // }
            //log.Info("RecordSwipeReturnSummary", "Return val: " + returnVal.SystemErrorCode);
            return returnVal;
        }
    }
}
