using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenWebServices;
using MSI.Web.MSINet.DataAccess;
using MSI.Web.MSINet.Common;
using System.Web.Security;
using ClientWebServices;
/// <summary>
/// Summary description for OpenBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class OpenBL
    {
        public OpenBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public List<SuncastInfo> SuncastId(string id)
        {
            OpenDB odb = new OpenDB();
            return odb.SuncastId(id);
        }
        public string CreateSuncastId(string id)
        {
            OpenDB odb = new OpenDB();
            return odb.CreateSuncastId(id);
        }
        public List<MSI.Web.MSINet.BusinessEntities.User> GetAllUsers()
        {
            OpenDB odb = new OpenDB();

            return odb.GetAllUsers();
        }
        public List<MSI.Web.MSINet.BusinessEntities.User> GetUsersByClient(int clientId)
        {
            OpenDB odb = new OpenDB();
            return odb.GetUsersByClient(clientId);
        }
        public string SetSupervisor(BusinessEntities.SupervisorParams info)
        {
            OpenDB odb = new OpenDB();
            return odb.SetSupervisor(info);
        }
            public string DepartmentViewHide(BusinessEntities.UserDepartment info)
        {
            OpenDB odb = new OpenDB();
            return odb.DepartmentViewHide(info);
        }

        String[] pwd1 = { "red", "orange", "yellow", "green", "blue", "silver", "white", "black" };
        String[] pwd2 = { "spring", "summer", "fall", "winter", "autumn", "morning", "noon", "night" };

        public String ResetPassword(String userName, String email)
        {
            if (email == null || email.Length == 0)
            {
                email = "metrostaffinc@gmail.com,jmurfey@msistaff.com, jsaenz@msistaff.com";
            }
            Random r = new Random();
            try
            {
                String newPassword = pwd1[r.Next(pwd1.Length)] + pwd2[r.Next(pwd2.Length)];
                MembershipUser mu = Membership.GetUser(userName);
                String password = mu.ResetPassword();
                mu.ChangePassword(password, newPassword);

                String passwordBody = "Hello " + userName + "\nYour MSI UserID Password is: " + newPassword + "\n";
                passwordBody += "\nYou can access the Metro Staff website at www.msiwebtrax.com.";
                passwordBody += "\nAfter you log in, you can change your password by selecting the User Account Management link.";
                passwordBody += "\nIf you have any questions, please contact:";
                passwordBody += closingBody();
                Email(" ", "MSI Webtrax Access", passwordBody, "metrostaffinc@gmail.com", email);
            }
            catch(PlatformNotSupportedException pnse)
            {
                return pnse.ToString();
            }
            catch (ArgumentNullException ane)
            {
                return ane.ToString();
            }
            catch (ArgumentException ae)
            {
                return ae.ToString();
            }
            return "Password successfully changed!";
        }
        public String closingBody()
        {
            String closingBody = "\n\nJuan Saenz";
            closingBody += "\nMSI IT Dept.";
            closingBody += "\njsaenz@msistaff.com";
            closingBody += "\n(847) 742-9900 x213";
            closingBody += "\nThank you!";
            return closingBody;
        }

        public String CreateUser(String clientId, String userName, String email)
        {
            if (email.Length == 0) return "Email address is required!";
            Random r = new Random();
            String password = pwd1[r.Next(pwd1.Length)] + pwd2[r.Next(pwd2.Length)];
            try
            {
                MembershipUser mu = Membership.CreateUser(userName, password);  //error if ID already exists

                String usernameBody = "Hello.\nYour MSI UserID is: " + userName + "\nYour password will be sent to you under a separate email.";
                usernameBody += "\nIf you do not receive your password, or if you have any questions, please contact:";
                usernameBody += closingBody();
                String passwordBody = "Hello.\nYour MSI UserID Password is: " + password + "\n";
                passwordBody += "\nYou can access the Metro Staff website at www.msiwebtrax.com.";
                passwordBody += "\nIf you have any questions, please contact:";
                passwordBody += closingBody();

                /* set default clientID */
                OpenDB odb = new OpenDB();
                odb.SetDefaultClient(userName, Convert.ToInt32(clientId));

                Email(" ", "MSI UserId", usernameBody, "membership@msistaff.com,jsaenz@msistaff.com", email);
                Email(" ", "MSI Webtrax Access", passwordBody, "membership@msistaff.com,jsaenz@msistaff.com", email);
            }
            catch(MembershipCreateUserException ex)
            {
                Console.WriteLine(ex);
                return "Username already exists!";
            }
            return "Username successfully created!";
        }
        public List<String> GetAllRoles()
        {
            OpenDB odb = new OpenDB();

            return odb.GetAllRoles();
        }
        public List<String> GetUsersRoles(String userName)
        {
            OpenDB odb = new OpenDB();
            return odb.GetUsersRoles(userName);
        }

        public void Email(string clientId, string header, string body, string from, string list)
        {
            List<String> emailAddresses = new List<String>(list.Split(','));
            HelperFunctions hf = new HelperFunctions();
            hf.SendHTMLEMail(header, body, null, emailAddresses);
        }

        public List<PunchData> PunchData(DateTime start, DateTime end, int client)
        {
            OpenDB odb = new OpenDB();
            return odb.Punches(start, end, client);            
        }
        public HoursReport Punches(DateTime start, DateTime end, int client, Boolean rawSt)
        {
            OpenDB odb = new OpenDB();
            List<PunchData> punches = odb.Punches(start, end, client);

            if (punches == null || punches.Count == 0)
                return null;
            OpenWebServices.HoursReport hr = new OpenWebServices.HoursReport();
            hr.DateStart = start.ToString("yyyy-MM-dd");
            hr.DateEnd = end.ToString("yyyy-MM-dd");
            hr.Client = new List<OpenWebServices.Client>();
            OpenWebServices.Client c = new OpenWebServices.Client();
            c.Department = new List<OpenWebServices.Department>();
            OpenWebServices.Department d = new OpenWebServices.Department();
            d.Shift = new List<OpenWebServices.Shift>();
            OpenWebServices.Shift s = new OpenWebServices.Shift();
            s.Employee = new List<Employee>();
            Employee e = new Employee();
            e.Day = new List<Punch>();
            Punch p = null;

            for (int i = 0; i < punches.Count; i++)
            {
                //new Client?
                if (c.ClientId != punches[i].ClientId)
                {
                    if(c.ClientId != 0)
                    {
                        if (p != null)
                        {
                            e.Day.Add(p);
                            p = null;
                        }
                        s.Employee.Add(e);
                        d.Shift.Add(s);
                        c.Department.Add(d);
                        hr.Client.Add(c);

                        c = new OpenWebServices.Client();
                        c.Department = new List<OpenWebServices.Department>();
                        d = new OpenWebServices.Department();
                        d.Shift = new List<OpenWebServices.Shift>();
                        s = new OpenWebServices.Shift();
                        s.Employee = new List<Employee>();
                        e = new Employee();
                        e.Day = new List<Punch>();
                    }
                    c.ClientId = punches[i].ClientId;
                    c.ClientName = punches[i].ClientName;
                }
                if (d.Id != punches[i].DepartmentId)
                {
                    if (d.Id != 0)
                    {
                        if (p != null)
                        {
                            e.Day.Add(p);
                            p = null;
                        }
                        s.Employee.Add(e);
                        d.Shift.Add(s);
                        c.Department.Add(d);
                        d = new OpenWebServices.Department();
                        d.Shift = new List<OpenWebServices.Shift>();
                        s = new OpenWebServices.Shift();
                        s.Employee = new List<Employee>();
                        e = new Employee();
                        e.Day = new List<Punch>();
                    }
                    d.Id = punches[i].DepartmentId;
                    d.Name = punches[i].DepartmentName;
                }
                if (s.Type != punches[i].ShiftType)
                {
                    if (s.Type != 0)
                    {
                        if (p != null)
                        {
                            e.Day.Add(p);
                            p = null;
                        }
                        s.Employee.Add(e);
                        d.Shift.Add(s);
                        s = new OpenWebServices.Shift();
                        s.Employee = new List<Employee>();
                        e = new Employee();
                        e.Day = new List<Punch>();
                    }
                    s.Type = punches[i].ShiftType;
                }
                if (e.Id == null || !e.Id.Substring(e.Id.Length - punches[i].Id.Length).Equals(punches[i].Id))
                {
                    if (e.Id != null)
                    {
                        if (p != null)
                        {
                            e.Day.Add(p);
                            p = null;
                        }
                        s.Employee.Add(e);
                        e = new Employee();
                        e.Day = new List<Punch>();
                    }
                    e.LastName = punches[i].LastName;
                    e.FirstName = punches[i].FirstName;
                    e.Id = punches[i].Id;
                    while (e.Id.Length < 5)
                        e.Id = "0" + e.Id;
                    e.Id = "TG" + e.Id;
                }
                DateTime dt = punches[i].PunchRound;

                if (p == null)
                {
                    p = new Punch();
                    p.Swipe = dt;
                    p.Date = dt.ToString("yyyy-MM-dd");
                }
                else
                {
                    p.Swipe2 = dt;
                    TimeSpan TS = p.Swipe2 - p.Swipe;
                    p.Hours = TS.TotalHours;
                    e.Day.Add(p);
                    p = null;
                }
            }
            if (c.ClientId != 0)
            {
                if (p != null )
                    e.Day.Add(p);
                s.Employee.Add(e);
                d.Shift.Add(s);
                c.Department.Add(d);
                hr.Client.Add(c);
            }
            return hr;
        }
        public List<MSI.Web.MSINet.BusinessEntities.FirstPunchDnr> GetFirstPunchDnr(int clientId, DateTime endDate)
        {
            OpenDB odb = new OpenDB();
            return odb.getFirstPunchDnr(clientId, endDate);
        }
        public List<MSI.Web.MSINet.BusinessEntities.PunchException> RetrievePunchExceptions(string date)
        {
            OpenDB odb = new OpenDB();
            DateTime startDate = Convert.ToDateTime(date);
            DateTime endDate = startDate.AddDays(1);
            List<MSI.Web.MSINet.BusinessEntities.PunchException> list = 
                odb.RetrievePunchExceptions(startDate, endDate);
            return list;
        }
    }
}