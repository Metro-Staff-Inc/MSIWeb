using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;
using System.Collections;
using System.Net.Mail;
using System.Collections.Generic;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class EmployeeBL
    {
        public EmployeeBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public List<String> GetFingerprintInfo(String type)
        {
            EmployeeDB edb = new EmployeeDB();
            if (type.Equals("0"))
                return edb.GetFPTList();
            else if (type.Equals("1"))
                return edb.GetFSTList();
            else if (type.Equals("2"))
                return edb.GetXMLList();
            return null;
        }
        public List<SkillDescription> GetSkillDescriptions()
        {
            RosterDB rdb = new RosterDB();
            return rdb.GetSkillDescriptions();
        }

        public FingerprintInfo GetFingerprints()
        {
            EmployeeDB edb = new EmployeeDB();


            return edb.GetFingerprints();
        }

        public int CreateETicketRoster(string clientID, string locID, string shiftType, string deptID, string weekEnd)
        {
            EmployeeDB edb = new EmployeeDB();
            return edb.CreateETicketRoster(Convert.ToInt32(clientID), Convert.ToInt32(locID), Convert.ToInt32(shiftType), Convert.ToInt32(deptID),
                                                Convert.ToDateTime(weekEnd));
        }
/*        public int UpdateEmployeeHours(int employeeHoursHeaderId, String aidentNumber, String userName, DateTime weekEnd, String notes, double hours1, double hours2, double hours3, double hours4,
                                            int hours5, int hours6, int hours7)
*/
        public int UpdateEmployeeHours(int employeeHoursHeaderId, String aidentNumber, String userName, DateTime weekEnd, String notes,
            double hours1, double hours2, double hours3, double hours4, double hours5, double hours6, double hours7)
        {
            EmployeeDB edb = new EmployeeDB();
            return edb.UpdateEmployeeHours(employeeHoursHeaderId, aidentNumber, userName, notes, hours1, hours2, hours3,
                            hours4, hours5, hours6, hours7);
        }

        public EmployeeHours GetETicketRoster(string clientID, string locID, string weekEnd, string deptID, string shiftType)
        {
            EmployeeDB edb = new EmployeeDB();
            return edb.GetETicketRoster(Convert.ToInt32(clientID), Convert.ToInt32(locID), 
                    Convert.ToDateTime(weekEnd), Convert.ToInt32(deptID), Convert.ToInt32(shiftType));
        }

        public String EmailSuncast(int clientID, int tempsOnly, int deptID, int shiftType, int shiftID, DateTime date)
        {
            RosterDB rdb = new RosterDB();
            return rdb.EmailSuncast(clientID, tempsOnly, deptID, shiftType, shiftID, date);
        }

        public string ValidateUser(string userName, string pwd)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.ValidateUser(userName, pwd);
        }

        public string GetEmployeeByName(string name)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.GetEmployeeByName(name);
        }

        public RecruitPool GetEmployeeByNamePhoneBlast(string lastName, string firstName)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.GetEmployeeByNamePhoneBlast(lastName, firstName);
        }
        public List<EmployeeStatus> GetEmployeeStatus(DateTime date, int clientID)
        {
            EmployeeDB edb = new EmployeeDB();
            return edb.GetEmployeeStatus(date, clientID);
        }    

        public string GetEmployeeByName_Pics(string name)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.GetEmployeeByName_Pics(name);
        }

        public string GetEmployeeByDate_Pics(string days, string clientID)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.GetEmployeeByDate_Pics(days, clientID);
        }

        public EmployeeHistory GetEmployeeByAident(EmployeeLookup lookup)
        {
            EmployeeDB empDB = new EmployeeDB();

            return empDB.GetEmployeeByAident(lookup);
        }

        public int SetEmployee(EmployeeHistory empHist, int action)
        {
            EmployeeDB empDB = new EmployeeDB();
            return empDB.SetEmployee(empHist, action);
        }

        public void SendHTMLEMail(string title, string body, List<String> emailAddrs)
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("eticket", "eticket");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mailClient.Host = "smtp.msistaff.com";
                mailClient.Port = 5190;
                mailClient.EnableSsl = true;

                message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress("eticket@msistaff.com");

                foreach (String addr in emailAddrs)
                {
                    message.To.Add(addr);
                }
                message.Subject = title;
                message.Body = body;
                message.IsBodyHtml = true;
                // Send Mail via SmtpClient
                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }



        public void EmailEmployeeRoster(int clientID, int dept, string addr, DateTime start, DateTime end)
        {
            try
            {
                /* create body */
                string body = "<div><table><thead><tr><td>" + "MSI Employee Roster " + start.ToString("MM/dd/yyyy") + 
                    " to " + end.ToString("MM/dd/yyyy") + "</td></tr></thead>"; 
                //for( int i=0; i<results.
                //body += "<div><table><thead><tr><td>" + results.Department[i] + "</td></tr></thead>";
                //MemoryStream stream = new MemoryStream();
                //pictBoxEmailImage.Image.Save(stream, ImageFormat.Jpeg);

                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("jonathanmurfey@gmail.com", "lkjhlkjh");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                mailClient.Host = "smtp.gmail.com";
                mailClient.Port = 587;
                mailClient.EnableSsl = false;
                mailClient.EnableSsl = true;

                //The From address (Email ID)    
                string str_from_address = "jmurfey@msistaff.com";
                //The Display Name    
                string str_name = "MSI Roster List";
                //Create MailMessage Object
                MailMessage email_msg = new MailMessage();
                //Specifying From,Sender & Reply to address
                email_msg.From = new MailAddress(str_from_address, str_name);
                email_msg.Sender = new MailAddress(str_from_address, str_name);
                email_msg.ReplyToList.Add(new MailAddress(str_from_address, str_name));
                //The To Email id    
                email_msg.To.Add("jonathanmurfey@gmail.com");
                email_msg.To.Add("jmurfey@msistaff.com");
                email_msg.Subject = "Hello There! Testing...";
                email_msg.Priority = MailPriority.High;
                //first we create the Plain Text part
                //AlternateView plainView = AlternateView.CreateAlternateViewFromString("This is my plain text content, viewable by those clients that don't support html", null, "text/plain");

                email_msg.Body = "<h1>At " + DateTime.Now.ToString() + ", </h1><h1>";
                email_msg.Body += "Hello there!</h1>";
                email_msg.IsBodyHtml = true;
                //Now Send the message    
                mailClient.Send(email_msg);
            }
            catch (Exception ex)
            {    //Some error occured    
                //MessageBox.Show(ex.Message.ToString());
                Console.WriteLine(ex);
            }
        }
    }
}