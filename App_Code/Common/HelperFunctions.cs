using System;
using MSI.Web.MSINet.BusinessEntities;
using System.Collections.Generic;
using System.IO;
using OpenPop.Pop3;
using OpenPop.Mime;
using System.Net.Mail;
using System.Net.Mime;
using System.Timers;
using MSI.Web.MSINet.BusinessLogic;
//using MSIToolkit.Logging;
using System.Linq;
using System.Runtime.Serialization;
using System.Drawing;
using System.Web;
using System.Drawing.Imaging;
using PunchClock;

namespace MSI.Web.MSINet.Common
{
    public class DateRange
    {
        public string startDate;
        public string endDate;
    }

    [DataContract]
    public class MyClass
    {
        public MyClass() { }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
    }

    public class EventItem
    {
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
        public ElapsedEventHandler handler { get; set; }
        public EventItem(int hour, int minute, int second, ElapsedEventHandler handler)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.handler = handler;
        }
    }
    [DataContract]
    public class TextFile
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    public static class Email
    {
        /// <summary>
        /// Example showing:
        ///  - how to fetch all messages from a POP3 server
        /// </summary>
        /// <param name="hostname">Hostname of the server. For example: pop3.live.com</param>
        /// <param name="port">Host port to connect to. Normally: 110 for plain POP3, 995 for SSL POP3</param>
        /// <param name="useSsl">Whether or not to use SSL to connect to server</param>
        /// <param name="username">Username of the user on the server</param>
        /// <param name="password">Password of the user on the server</param>
        /// <returns>All Messages on the POP3 server</returns>
        public static List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                //Notice that the for loop starts at one, and goes up to and includes the messageCount number.
                //This is because POP3 is 1-based.This is the case for all methods taking a message number as an argument.        
                for (int i = messageCount; i > 0; i--)
                {
                    client.GetMessageHeaders(i);
                    allMessages.Add(client.GetMessage(i));
                }

                // Now return the fetched messages
                return allMessages;
            }
        }
    }

    public static class DateTimeHelpers
    {
        public static DateTime DT1970 = new DateTime(1970, 1, 1);

        public static DateTime MillisecondsSince1970ToDateTime(double javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dateTime;
        }
        public static long CSTToClockTicks(DateTime dt)
        {
            long ticks = dt.ToUniversalTime().Ticks;
            ticks -= DT1970.Ticks;
            ticks /= 10000;
            return ticks;
        }

        public static TimeSpan RoundUp(this TimeSpan ts, TimeSpan d)
        {
            var delta = (d.Ticks - (ts.Ticks % d.Ticks)) % d.Ticks;
            return new TimeSpan(ts.Ticks + delta);
        }
        public static TimeSpan RoundDown(this TimeSpan dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new TimeSpan(dt.Ticks - delta);
        }
        public static TimeSpan RoundToNearest(this TimeSpan dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;
            return roundUp ? dt.RoundUp(d) : dt.RoundDown(d);
        }

        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            var delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }
        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;
            return roundUp ? dt.RoundUp(d) : dt.RoundDown(d);
        }
    }
    /// <summary>
    /// Summary description for HelperFunctions
    /// </summary>

    public class HelperFunctions
    {
        static string style99 =
            "<style>" +
            "table, th, td {" +
            "border: 1px solid black;" +
            "border-collapse: collapse;" +
            "}" +
            "th, td {" +
            "padding: 15px;" +
            "text-align: left;" +
            "}" +
            "th {" +
            "background-color: #f1f1c1;" +
            "}" +
            "table {" +
            "width: 100%;" +
            "}" +
            "</style>";

        ////PerformanceLogger log = new PerformanceLogger("AdoNetAppender");
        static Timer dailyTimer;

        public HelperFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string shiftType(int shift)
        {
            switch (shift)
            {
                case 1:
                    return "1st Shift";
                case 2:
                    return "2nd Shift";
                case 3:
                    return "3rd Shift";
                case 4:
                    return "Shift A";
                case 5:
                    return "Shift B";
                case 6:
                    return "Shift C";
                case 7:
                    return "Shift D";
                default:
                    return "Shift";
            }
        }

        public static int eventIdx = 0;
        public static EventItem[] events;

        public static void initDailyTimer(EventItem[] eventList)//, int hour, int mins
        {
            if (eventList == null || eventList.Length == 0) return;
            events = eventList;

            HelperFunctions hf = new HelperFunctions();
            //hf.log.Info("Init Daily Timer", hour + ": " + mins);
            for (int i = 0; i < eventList.Length; i++)
            {
                //hf.log.Info("Init Daily Timer", eventList[i].hour + ", " + eventList[i].minute);
            }
            //DateTime eventTime = DateTime.Today.Add(new TimeSpan(hour, mins, 0));
            DateTime eventTime = DateTime.Today.Add(new TimeSpan(eventList[0].hour, eventList[0].minute, eventList[0].second));
            double millis = (eventTime - DateTime.Now).TotalMilliseconds;
            if (millis <= 0)
            {
                eventTime = eventTime.Add(new TimeSpan(24, 0, 0));
                millis = (eventTime - DateTime.Now).TotalMilliseconds;
            }
            dailyTimer = new System.Timers.Timer();
            dailyTimer.Interval = millis;
            // Hook up the Elapsed event for the timer. 
            //dailyTimer.Elapsed += FixInterval;  // fix interval
            //dailyTimer.Elapsed += sendVehicleUseReport;
            dailyTimer.Elapsed += eventList[0].handler;

            // Have the timer fire repeated events (true is the default)
            dailyTimer.AutoReset = true;
            // Start the timer
            dailyTimer.Enabled = true;
            //hf.log.Info("Init Daily Timer", "in " + millis/(1000*60.0*60.0) + " hours." );
        }

        public static void FixInterval(Object source, ElapsedEventArgs e)
        {
            dailyTimer.Stop();
            dailyTimer.Interval = 24 * 60 * 60 * 1000;
            dailyTimer.Start();
        }

        public static void sendDailyDriverData(Object source, ElapsedEventArgs e)
        {
            List<String> emailList = new List<String>();
            List<String> bccList = new List<String>();
            emailList.Add("jmurfey@msistaff.com");
            bccList.Add("transportList@msistaff.com");
            bccList.Add("kscharpf@msistaff.com");
            DateTime rideDate = DateTime.Today.AddDays(-1);
            TransportationBL tbl = new TransportationBL();
            dailyTimer.Stop();
            List<DriverData> drivers = tbl.getDailyDriverData(rideDate);

            HelperFunctions hf = new HelperFunctions();
            //hf.log.Info("sendDailyDriverData", "" + drivers.Count);

            string body = "";
            body += "<h2>Daily driver info summary report</h2>";

            body += "<table style=\"width: 100 %; border: 1px solid black; border - collapse: collapse\">";
            body += "<caption>Drivers who have not yet uploaded ride info</caption>";
            body += "<thead><tr  style=\"border: 1px solid black; border - collapse: collapse\"><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Driver Name</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>FleetMatics ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Dispatch Office</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Passenger Count</th></tr>";
            body += "</thead><tbody>";

            foreach (DriverData dd in drivers)
            {
                if (dd.passengerCount == 0)
                    body += "<tr><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.lastName + ", " + dd.firstName + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.vehicleId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.officeName + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.passengerCount + "</td></tr>";
            }
            body += "</tbody></table><br/>";

            body += "<table style=\"width: 100 %; border: 1px solid black; border - collapse: collapse\">";
            body += "<caption>Drivers with uploaded ride data</caption>";
            body += "<thead><tr  style=\"border: 1px solid black; border - collapse: collapse\"><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Driver Name</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>FleetMatics ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Dispatch Office</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Passenger Count</th></tr>";
            body += "</thead><tbody>";

            int totalPassengers = 0;
            foreach (DriverData dd in drivers)
            {
                if (dd.passengerCount > 0)
                    body += "<tr><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.lastName + ", " + dd.firstName + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.vehicleId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.officeName + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + dd.passengerCount + "</td></tr>";
                totalPassengers += dd.passengerCount;
            }
            body += "</tbody>";

            body += "<tfoot><tr><td colspan='3' style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Total Passengers</td><td  style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>" + totalPassengers + "</td></tr></tfoot>";
            body += "</table><br/>";

            body += "<hr/>";
            body += "<p style='color:red'>This is an automated daily email, please do not respond.</p>";
            body += "<p style='color:black'>Additional vehicle and ride data will increase this email's usefulness<br/>";
            body += "<em>Export the Transport Report from www.msiwebtrax.com for additional details.</em><br/><br/>";
            body += "Any changes or updates, please contact <br/>Jonathan Murfey<br/>(773) 354-2056</p>";
            hf.SendHTMLEMail(rideDate.ToShortDateString() + " Driver summary report", body, null, emailList, null, true, bccList);

            eventIdx += 1;
            if (eventIdx >= events.Length)
            {
                eventIdx = 0;
            }

            DateTime eventTime =
                DateTime.Today.Add(new TimeSpan(events[eventIdx].hour, events[eventIdx].minute, events[eventIdx].second));
            double millis = (eventTime - DateTime.Now).TotalMilliseconds;
            if (millis <= 0)
            {
                eventTime = eventTime.Add(new TimeSpan(24, 0, 0));
                millis = (eventTime - DateTime.Now).TotalMilliseconds;
            }
            //dailyTimer.Stop();
            dailyTimer.Interval = millis;
            dailyTimer.Start();
        }
        public static void sendVehicleUseReport99(Object source, ElapsedEventArgs e)
        {
            List<String> emailList = new List<String>();
            List<String> bccList = new List<String>();
            emailList.Add("jmurfey@msistaff.com");
            //bccList.Add("transportList@msistaff.com");

            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today;
            TransportationBL tbl = new TransportationBL();
            Dictionary<string, Vehicle> vehicles = tbl.getVehicleUseInfo(startDate, endDate);

            string body = "";
            body += "<h2>Daily vehicle use summary report</h2>";

            body += "<table style=\"width: 100 %; border: 1px solid black; border - collapse: collapse\">";
            body += "<caption>Vehicles in fleet, but no uploaded ride data</caption>";
            body += "<thead><tr  style=\"border: 1px solid black; border - collapse: collapse\"><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Vehicle ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>FleetMatics ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Dispatch Office</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Max Passengers</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Total Passenger Count</th></tr>";
            body += "</thead><tbody>";
            foreach (string vid in vehicles.Keys.ToArray())
            {
                Vehicle v = vehicles[vid];
                if (v.inFleet == true && v.transportList.Count() == 0)
                    body += "<tr><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.vehicleId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.fleetMaticsId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.office + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.numPassengers + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.transportList.Count + "</td></tr>";
            }
            body += "</tbody></table><br/>";

            body += "<table style=\"width: 100 %; border: 1px solid black; border - collapse: collapse\">";
            body += "<caption>Vehicles in fleet, with uploaded ride data</caption>";
            body += "<thead><tr  style=\"border: 1px solid black; border - collapse: collapse\"><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Vehicle ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>FleetMatics ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Dispatch Office</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Max Passengers</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Total Passenger Count</th></tr>";
            body += "</thead><tbody>";

            foreach (string vid in vehicles.Keys.ToArray())
            {
                Vehicle v = vehicles[vid];
                if (v.inFleet == true && v.transportList.Count() > 0)
                    body += "<tr><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.vehicleId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.fleetMaticsId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.office + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.numPassengers + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.transportList.Count + "</td></tr>";
            }
            body += "</tbody></table><br/>";

            String bodyTemp = "<table style=\"width: 100 %; border: 1px solid black; border - collapse: collapse\">";
            bodyTemp += "<caption>Vehicle IDs not in fleet, but with uploaded ride data (possible incorrectly entered ID?)</caption>";
            bodyTemp += "<thead><tr  style=\"border: 1px solid black; border - collapse: collapse\"><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Vehicle ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>FleetMatics ID</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Dispatch Office</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Max Passengers</th><th style='border: 1px solid black; border-collapse: collapse; background-color: #f1f1c1; padding: 15px;text-align:left'>Total Passenger Count</th></tr>";
            bodyTemp += "</thead><tbody>";
            Boolean fnd = false;
            foreach (string vid in vehicles.Keys.ToArray())
            {
                Vehicle v = vehicles[vid];
                if (v.inFleet == false)
                {
                    fnd = true;
                    bodyTemp += "<tr><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.vehicleId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.fleetMaticsId + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.office + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.numPassengers + "</td><td style='border: 1px solid black; border-collapse: collapse; padding: 15px;text-align:left'>" + v.transportList.Count + "</td></tr>";
                }
            }
            bodyTemp += "</tbody></table><br/>";
            if (fnd)
            {
                body += bodyTemp;
            }

            body += "<hr/>";
            body += "<p style='color:red'>This is an automated daily email, please do not respond.</p>";
            body += "<p style='color:black'>Additional vehicle and ride data will increase this email's usefulness<br/>";
            body += "<em>Export the Transport Report from www.msiwebtrax.com for additional details.</em><br/><br/>";
            body += "Any changes or updates, please contact <br/>Jonathan Murfey<br/>(773) 354-2056</p>";
            HelperFunctions hf = new HelperFunctions();
            hf.SendHTMLEMail(startDate.ToShortDateString() + " Vehicle use summary report", body, null, emailList, null, true, bccList);
        }

        public void adjustJBSSPayRates(EmployeeHistory employeeHistory, EmployeeWorkSummary workSummary)
        {
            if (workSummary.ShiftTypeInfo.ShiftTypeId == 1)
            {
                if (employeeHistory.PayRate == (decimal)8.25 || employeeHistory.PayRate == (decimal)0.00)
                {
                    employeeHistory.PayRate = (decimal)9.00;
                }
                else if (employeeHistory.PayRate == (decimal)8.50)
                {
                    employeeHistory.PayRate = (decimal)9.50;
                }
            }
            else if (workSummary.ShiftTypeInfo.ShiftTypeId == 2)
            {
                if (employeeHistory.PayRate == (decimal)8.50 || employeeHistory.PayRate == (decimal)0.00)
                {
                    employeeHistory.PayRate = (decimal)9.25;
                }
                else if (employeeHistory.PayRate == (decimal)8.75)
                {
                    employeeHistory.PayRate = (decimal)9.75;
                }
                else if (employeeHistory.PayRate == (decimal)10.50)
                {
                    employeeHistory.PayRate = (decimal)10.75;
                }
                /*else if (employeeHistory.PayRate == (decimal)11.00)
                {
                    employeeHistory.PayRate = (decimal)11.25;
                }
                */
            }
            else
            {
                if (employeeHistory.PayRate == (decimal)8.75 || employeeHistory.PayRate == (decimal)0.00)
                {
                    employeeHistory.PayRate = (decimal)9.50;
                }
                else if (employeeHistory.PayRate == (decimal)9.00)
                {
                    employeeHistory.PayRate = (decimal)10.00;
                }
                else if (employeeHistory.PayRate == (decimal)11.00)
                {
                    employeeHistory.PayRate = (decimal)11.50;
                }
                else if (employeeHistory.PayRate == (decimal)10.50)
                {
                    employeeHistory.PayRate = (decimal)11.00;
                }
            }
        }

        public DateTime GetCSTCurrentDateTime()
        {
            TimeSpan cstOffset;

            if (DateTime.Now.IsDaylightSavingTime())
            {
                cstOffset = new TimeSpan(-5, 0, 0);
            }
            else
            {
                cstOffset = new TimeSpan(-6, 0, 0);
            }

            DateTime utcNow = DateTime.UtcNow;
            //get the cst time zone offset

            return utcNow.Add(cstOffset);
        }

        public void SendHTMLEMail(string title, string body, string from, List<String> emailAddrs, TextFile textFile = null, bool isHtml = false, List<String> bccEmailAddrs = null)
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    SmtpClient mailClient = new SmtpClient();
                    mailClient.UseDefaultCredentials = false;

                    System.Configuration.Configuration rootWebConfig1 =
                                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
                    System.Configuration.KeyValueConfigurationElement email =
                    rootWebConfig1.AppSettings.Settings["submitApprovedEmail"];
                    String fromEmail = null;
                    if (email == null || email.Value == null || email.Value.Length == 0)
                    {
                        //email = new KeyValueConfigurationElement(  new KeyValuePair<string, string>("email", "metrostaffinc@gmail.com"));
                        fromEmail = "metrostaffinc@gmail.com";
                    }
                    else
                    {
                        fromEmail = email.Value;
                    }
                    System.Configuration.KeyValueConfigurationElement pwd =
                    rootWebConfig1.AppSettings.Settings["submitApprovedPwd"];
                    String fromPwd = null;
                    if (pwd == null || pwd.Value == null || pwd.Value.Length == 0)
                    {
                        //fromPwd = "Metro2019";
                        fromPwd = "isouhrfseigmwnzs";
                    }
                    else
                    {
                        fromPwd = pwd.Value;
                    }
                    System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential(fromEmail, fromPwd);
                    mailClient.Credentials = credentials;
                    mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    mailClient.Host = "smtp.gmail.com";
                    mailClient.Port = 587;
                    mailClient.EnableSsl = true;
                    message = new MailMessage();
                    if (from == null || from.Length == 0)
                        message.From = new System.Net.Mail.MailAddress(fromEmail);
                    else
                        message.From = new System.Net.Mail.MailAddress(from);
                    foreach (String addr in emailAddrs)
                    {
                        message.To.Add(addr);
                    }
                    if (bccEmailAddrs != null)
                    {
                        foreach (String addr in bccEmailAddrs)
                        {
                            message.Bcc.Add(addr);
                        }
                    }
                    message.Subject = title;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    if (textFile != null)
                    {
                        using (StreamWriter writer = new StreamWriter(ms))
                        {
                            writer.WriteLine(textFile.Content);
                            writer.Flush();
                            ms.Position = 0;

                            ContentType ct = new ContentType(MediaTypeNames.Text.Plain);
                            Attachment attach = new Attachment(ms, ct);
                            attach.ContentDisposition.FileName = textFile.FileName;
                            message.Attachments.Add(attach);
                            mailClient.Send(message);
                        }
                    }
                    else
                    {
                        mailClient.Send(message);
                    }
                }
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

        public DateTime GetCSTCurrentWeekEndingDate()
        {
            return this.GetCSTWeekEndingDateFromDate(this.GetCSTCurrentDateTime());
        }
        public DateTime GetCSTWeekEndingSaturdayDateFromDate(DateTime dateIn)
        {
            DateTime retDate = new DateTime(1, 1, 1);
            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retDate = dateIn.AddDays(5);
                    break;
                case DayOfWeek.Tuesday:
                    retDate = dateIn.AddDays(4);
                    break;
                case DayOfWeek.Wednesday:
                    retDate = dateIn.AddDays(3);
                    break;
                case DayOfWeek.Thursday:
                    retDate = dateIn.AddDays(2);
                    break;
                case DayOfWeek.Friday:
                    retDate = dateIn.AddDays(1);
                    break;
                case DayOfWeek.Saturday:
                    retDate = dateIn.AddDays(0);
                    break;
                case DayOfWeek.Sunday:
                    retDate = dateIn.AddDays(6);
                    break;
            }
            return retDate;
        }
        public DateTime GetCSTWeekEndingFridayDateFromDate(DateTime dateIn)
        {
            DateTime retDate = new DateTime(1, 1, 1);
            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retDate = dateIn.AddDays(4);
                    break;
                case DayOfWeek.Tuesday:
                    retDate = dateIn.AddDays(3);
                    break;
                case DayOfWeek.Wednesday:
                    retDate = dateIn.AddDays(2);
                    break;
                case DayOfWeek.Thursday:
                    retDate = dateIn.AddDays(1);
                    break;
                case DayOfWeek.Friday:
                    retDate = dateIn.AddDays(0);
                    break;
                case DayOfWeek.Saturday:
                    retDate = dateIn.AddDays(6);
                    break;
                case DayOfWeek.Sunday:
                    retDate = dateIn.AddDays(5);
                    break;
            }
            return retDate;
        }
        public DateTime GetCSTWeekEndingThursdayDateFromDate(DateTime dateIn)
        {
            DateTime retDate = new DateTime(1, 1, 1);
            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retDate = dateIn.AddDays(3);
                    break;
                case DayOfWeek.Tuesday:
                    retDate = dateIn.AddDays(2);
                    break;
                case DayOfWeek.Wednesday:
                    retDate = dateIn.AddDays(1);
                    break;
                case DayOfWeek.Thursday:
                    retDate = dateIn.AddDays(0);
                    break;
                case DayOfWeek.Friday:
                    retDate = dateIn.AddDays(6);
                    break;
                case DayOfWeek.Saturday:
                    retDate = dateIn.AddDays(5);
                    break;
                case DayOfWeek.Sunday:
                    retDate = dateIn.AddDays(4);
                    break;
            }
            return retDate;
        }
        public DateTime GetCSTWeekEndingTuesdayDateFromDate(DateTime dateIn)
        {
            DateTime retDate = new DateTime(1, 1, 1);
            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retDate = dateIn.AddDays(1);
                    break;
                case DayOfWeek.Tuesday:
                    retDate = dateIn.AddDays(0);
                    break;
                case DayOfWeek.Wednesday:
                    retDate = dateIn.AddDays(6);
                    break;
                case DayOfWeek.Thursday:
                    retDate = dateIn.AddDays(5);
                    break;
                case DayOfWeek.Friday:
                    retDate = dateIn.AddDays(4);
                    break;
                case DayOfWeek.Saturday:
                    retDate = dateIn.AddDays(3);
                    break;
                case DayOfWeek.Sunday:
                    retDate = dateIn.AddDays(2);
                    break;
            }
            return retDate;
        }
        public DateTime GetCSTWeekEndingDateFromDate(DateTime dateIn)
        {
            DateTime retDate = new DateTime(1, 1, 1);
            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retDate = dateIn.AddDays(6);
                    break;
                case DayOfWeek.Tuesday:
                    retDate = dateIn.AddDays(5);
                    break;
                case DayOfWeek.Wednesday:
                    retDate = dateIn.AddDays(4);
                    break;
                case DayOfWeek.Thursday:
                    retDate = dateIn.AddDays(3);
                    break;
                case DayOfWeek.Friday:
                    retDate = dateIn.AddDays(2);
                    break;
                case DayOfWeek.Saturday:
                    retDate = dateIn.AddDays(1);
                    break;
                case DayOfWeek.Sunday:
                    retDate = dateIn.AddDays(0);
                    break;
            }
            return retDate;
        }

        public DateTime GetExact15PunchTime(DateTime punchTime)
        {
            DateTime roundedDateTime = new DateTime(1, 1, 1);
            roundedDateTime = punchTime;
            if (punchTime.Minute >= 35 && punchTime.Minute <= 45)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 45, 0);
            }
            else if (punchTime.Minute >= 15 && punchTime.Minute <= 25)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 15, 0);
            }
            //else 
            //    roundedDateTime = GetRoundedPunchTime(punchTime);
            return roundedDateTime;
        }

        public DateTime GetRoundedPunchTime(DateTime punchTime)
        {
            DateTime roundedDateTime = new DateTime(1, 1, 1);

            roundedDateTime = punchTime; // if none of the conditions are true, i.e. Minutes < 0 meaning error
            if (punchTime.Minute < 7 || (punchTime.Minute == 7 && punchTime.Second <= 30))
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 0, 0);
            }
            else if (punchTime.Minute < 22 || (punchTime.Minute == 22 && punchTime.Second <= 30))
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 15, 0);
            }
            else if (punchTime.Minute < 37 || (punchTime.Minute == 37 && punchTime.Second <= 30))
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 30, 0);
            }
            else if (punchTime.Minute < 52 || (punchTime.Minute == 52 && punchTime.Second <= 30))
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 45, 0);
            }
            else if (punchTime.Minute >= 52)
            {
                if (punchTime.Hour == 23)
                {
                    if (this.IsLastDayOfMonth(punchTime))
                    {
                        if (punchTime.Month == 12)
                        {
                            roundedDateTime = new DateTime(punchTime.Year + 1, 1, 1, 0, 0, 0);
                        }
                        else
                        {
                            roundedDateTime = new DateTime(punchTime.Year, punchTime.Month + 1, 1, 0, 0, 0);
                        }
                    }
                    else
                    {
                        roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day + 1, 0, 0, 0);
                    }
                }
                else
                {
                    roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour + 1, 0, 0);
                }
            }

            return roundedDateTime;
        }

        public DateTime Get10MinRoundedPunchTime(DateTime punchTime)
        {
            DateTime roundedDateTime = new DateTime(1, 1, 1);

            if (punchTime.Minute <= 10)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 0, 0);
            }
            else if (punchTime.Minute > 10 && punchTime.Minute <= 25)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 15, 0);
            }
            else if (punchTime.Minute > 25 && punchTime.Minute <= 40)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 30, 0);
            }
            else if (punchTime.Minute > 40 && punchTime.Minute <= 55)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 45, 0);
            }
            else if (punchTime.Minute > 55)
            {
                if (punchTime.Hour == 23)
                {
                    if (this.IsLastDayOfMonth(punchTime))
                    {
                        if (punchTime.Month == 12)
                        {
                            roundedDateTime = new DateTime(punchTime.Year + 1, 1, 1, 0, 0, 0);
                        }
                        else
                        {
                            roundedDateTime = new DateTime(punchTime.Year, punchTime.Month + 1, 1, 0, 0, 0);
                        }
                    }
                    else
                    {
                        roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day + 1, 0, 0, 0);
                    }
                }
                else
                {
                    roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour + 1, 0, 0);
                }
            }
            else
                roundedDateTime = punchTime;

            return roundedDateTime;
        }

        public DateTime GetShiftStartRoundedPunchTime(DateTime punchTime)
        {
            //Shift start rounds to nearest quarter hour
            DateTime roundedDateTime = new DateTime(1, 1, 1);
            //JHM . WHY IS THIS METHOD HERE?  CAN'T WE USE --
            return GetRoundedPunchTime(punchTime);
            /*
            if (punchTime.Minute >= 8 && punchTime.Minute <= 22)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 15, 0);
            }
            else if (punchTime.Minute > 22 && punchTime.Minute <= 30)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 30, 0);
            }
            else if (punchTime.Minute > 30 && punchTime.Minute <= 45)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 45, 0);
            }
            else if (punchTime.Minute > 45)
            {
                if (punchTime.Hour == 23)
                {
                    if (this.IsLastDayOfMonth(punchTime))
                    {
                        if (punchTime.Month == 12)
                        {
                            roundedDateTime = new DateTime(punchTime.Year + 1, 1, 1, 0, 0, 0);
                        }
                        else
                        {
                            roundedDateTime = new DateTime(punchTime.Year, punchTime.Month + 1, 1, 0, 0, 0);
                        }
                    }
                    else
                    {
                        roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day + 1, 0, 0, 0);
                    }
                }
                else
                {
                    roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour + 1, 0, 0);
                }
            }
            else
                roundedDateTime = punchTime;

            return roundedDateTime;
             */
        }

        public static DateTime GetCheckOutRoundedTime(DateTime punchTime, ref bool timeWasRounded)
        {
            DateTime roundedDateTime = new DateTime(1, 1, 1);

            timeWasRounded = true;
            if (punchTime.Minute <= 14)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 0, 0);
            }
            else if (punchTime.Minute > 30 && punchTime.Minute <= 44)
            {
                roundedDateTime = new DateTime(punchTime.Year, punchTime.Month, punchTime.Day, punchTime.Hour, 30, 0);
            }
            else
            {
                timeWasRounded = false;
                roundedDateTime = punchTime;
            }

            return roundedDateTime;
        }

        public bool IsLastDayOfMonth(DateTime dt)
        {
            bool lastDay = false;

            switch (dt.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (dt.Day == 31)
                    {
                        lastDay = true;
                    }
                    break;
                case 2:
                    if (DateTime.IsLeapYear(dt.Year))
                    {
                        if (dt.Day == 29)
                        {
                            lastDay = true;
                        }
                    }
                    else if (dt.Day == 28)
                    {
                        lastDay = true;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (dt.Day == 30)
                    {
                        lastDay = true;
                    }
                    break;
            }

            return lastDay;
        }

        public bool IsNotCheckedInAfterShiftStartError(EmployeeTracker employee)
        {
            bool result = false;
            DateTime shiftStartDateTime = DateTime.Parse(employee.ShiftStartTime);
            DateTime shiftEndDateTime = DateTime.Parse(employee.ShiftEndTime);

            if (shiftEndDateTime < shiftStartDateTime)
            {
                DateTime todayShiftStart = this.GetCSTCurrentDateTime();

                if (shiftStartDateTime > todayShiftStart)
                {
                    TimeSpan difference = shiftStartDateTime - todayShiftStart;
                    if (difference.TotalHours > 4)
                    {
                        //if the difference is greater than 4 hours then  
                        //make shift start date = current date - 1
                        shiftStartDateTime = shiftStartDateTime.AddDays(-1);
                        //leave the shift end date = current date
                    }
                }
            }

            DateTime cstNow = this.GetCSTCurrentDateTime();
            // Difference in minutes.
            TimeSpan ts = shiftStartDateTime - cstNow;
            if (ts.Hours <= 0)
            {
                if (ts.Hours <= -1)
                {
                    //Error
                    result = true;
                }
            }
            return result;
        }

        public bool IsNotCheckedOutAfterShiftEndError(EmployeeTracker employee)
        {
            bool result = false;
            DateTime shiftEndDateTime = DateTime.Parse(employee.ShiftEndTime);
            DateTime cstNow = this.GetCSTCurrentDateTime();
            DateTime shiftStartDateTime = DateTime.Parse(employee.ShiftStartTime);

            if (shiftEndDateTime < shiftStartDateTime)
            {
                DateTime todayShiftStart = this.GetCSTCurrentDateTime();

                if (shiftStartDateTime > todayShiftStart)
                {
                    TimeSpan difference = shiftStartDateTime - todayShiftStart;
                    if (difference.TotalHours <= 4)
                    {
                        shiftEndDateTime = shiftEndDateTime.AddDays(1);
                    }
                }
                else
                {
                    shiftEndDateTime = shiftEndDateTime.AddDays(1);
                }
            }

            // Difference in minutes.
            TimeSpan ts = shiftEndDateTime - cstNow;
            if (ts.Hours < 0)
            {
                if (ts.Hours <= -1)
                {
                    //Error
                    result = true;
                }
            }

            return result;
        }

        public double GetValidShiftHours(string startTime, string endTime)
        {
            double shiftHours = this.CalculateShiftHours(startTime, endTime);
            if (shiftHours <= 16)
            {
                return 16;
            }
            else
            {
                return shiftHours;
            }
            //allow a 30 minute buffer for check out.
            //return shiftHours += 4.50;
        }

        public double CalculateShiftHours(string startTime, string endTime)
        {
            DateTime startDateTime = DateTime.Parse(startTime);
            DateTime endDateTime = DateTime.Parse(endTime);

            if (endDateTime < startDateTime)
                endDateTime = endDateTime.AddDays(1);

            TimeSpan timeSpanStart = new TimeSpan(startDateTime.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(endDateTime.Ticks);
            double shiftMinutes = (timeSpanEnd.Ticks - timeSpanStart.Ticks) / TimeSpan.TicksPerMinute;
            return shiftMinutes / 60;
        }
        //jhm
        public DateTime GetShiftWeekEndingFridayDate(DateTime dateIn, string shiftStartTime, string shiftEndTime)
        {
            double addDays = 0;
            DateTime retVal = dateIn;

            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    addDays = 4;
                    break;
                case DayOfWeek.Tuesday:
                    addDays = 3;
                    break;
                case DayOfWeek.Wednesday:
                    addDays = 2;
                    break;
                case DayOfWeek.Thursday:
                    addDays = 1;
                    break;
                case DayOfWeek.Friday:
                    addDays = 0;
                    break;
                case DayOfWeek.Saturday:
                    addDays = 6;
                    DateTime tempShiftStart = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - dateIn;
                        if (shiftStartDif.TotalHours > 6)
                        {
                            addDays = -1;
                        }
                    }
                    break;
                case DayOfWeek.Sunday:
                    addDays = 5;
                    break;
            }

            if (addDays > 0)
            {
                retVal = dateIn.AddDays(addDays);
            }

            return retVal;
        }

        //jhm
        public DateTime GetShiftWeekEndingSaturdayDate(DateTime dateIn, string shiftStartTime, string shiftEndTime)
        {
            double addDays = 0;
            DateTime retVal = dateIn;

            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    addDays = 5;
                    break;
                case DayOfWeek.Tuesday:
                    addDays = 4;
                    break;
                case DayOfWeek.Wednesday:
                    addDays = 3;
                    break;
                case DayOfWeek.Thursday:
                    addDays = 2;
                    break;
                case DayOfWeek.Friday:
                    addDays = 1;
                    break;
                case DayOfWeek.Saturday:
                    addDays = 0;
                    break;
                case DayOfWeek.Sunday:
                    addDays = 6;
                    DateTime tempShiftStart = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - dateIn;
                        if (shiftStartDif.TotalHours > 6)
                        {
                            addDays = -1;
                        }
                    }
                    break;
            }

            if (addDays > 0)
            {
                retVal = dateIn.AddDays(addDays);
            }

            return retVal;
        }
        //jhm
        public DateTime GetShiftWeekEndingTuesdayDate(DateTime dateIn, string shiftStartTime, string shiftEndTime)
        {
            double addDays = 0;
            DateTime retVal = dateIn;

            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    addDays = 1;
                    break;
                case DayOfWeek.Tuesday:
                    addDays = 0;
                    break;
                case DayOfWeek.Wednesday:
                    addDays = 6;
                    DateTime tempShiftStart = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - dateIn;
                        if (shiftStartDif.TotalHours > 6)
                        {
                            addDays = -1;
                        }
                    }
                    break;
                case DayOfWeek.Thursday:
                    addDays = 5;
                    break;
                case DayOfWeek.Friday:
                    addDays = 4;
                    break;
                case DayOfWeek.Saturday:
                    addDays = 3;
                    break;
                case DayOfWeek.Sunday:
                    addDays = 2;
                    break;
            }

            if (addDays > 0)
            {
                retVal = dateIn.AddDays(addDays);
            }

            return retVal;
        }
        public DateTime GetShiftWeekEndingDate(DateTime dateIn, string shiftStartTime, string shiftEndTime)
        {
            double addDays = 0;
            DateTime retVal = dateIn;

            switch (dateIn.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    addDays = 6;
                    DateTime tempShiftStart = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(dateIn.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - dateIn;
                        if (shiftStartDif.TotalHours > 6)
                        {
                            addDays = -1;
                        }
                    }
                    break;
                case DayOfWeek.Tuesday:
                    addDays = 5;
                    break;
                case DayOfWeek.Wednesday:
                    addDays = 4;
                    break;
                case DayOfWeek.Thursday:
                    addDays = 3;
                    break;
                case DayOfWeek.Friday:
                    addDays = 2;
                    break;
                case DayOfWeek.Saturday:
                    addDays = 1;
                    break;
                case DayOfWeek.Sunday:
                    break;
            }

            if (addDays > 0)
            {
                retVal = dateIn.AddDays(addDays);
            }

            return retVal;
        }

        //jhm
        public bool NewSummaryAppliesToBillingPeriodSaturday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, DateTime punchWeekEnd, DateTime weekEnd, int clientId, int shiftType, int wsCount)
        {
            bool sameWeekEnd = false;
            bool appliesToPeriod = true;
            DateTime weekEnd2 = new DateTime(1900, 1, 1);
            DateTime nullDate = new DateTime(1900, 1, 1);

            if (punchDateTime.DayOfWeek == DayOfWeek.Saturday)
            {
                if (punchWeekEnd != null && punchWeekEnd != nullDate)
                {
                    if (weekEnd.Day != punchWeekEnd.Day)
                    {
                        appliesToPeriod = false;
                    }
                    else
                    {
                        sameWeekEnd = true;
                    }
                }
            }
            else
            { /*
                if (this.GetCSTWeekEndingSaturdayDateFromDate(punchDateTime).Day != weekEnd.Day)
                {
                    appliesToPeriod = false;
                }
               */
                /* handle third shift starting on or after midnight on the last day of the roster term */
                bool endOfWeekThirdShift = (shiftType >= 3 && punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime > periodStartDateTime.AddHours(24) && shiftStartTime.CompareTo(shiftEndTime) > 0);
                bool endOfWeekSecondShift = (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime > periodStartDateTime.AddHours(24));

                bool startOfWeek = (punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime < periodStartDateTime.AddHours(9) && shiftType == 1) /*||
                    (punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime > periodStartDateTime.AddHours(7) && (shiftType == 2 || shiftType == 3)) ||
                    (punchDateTime.Date == periodStartDateTime.Date)*/;

                TimeSpan shiftStart = new TimeSpan(0, Convert.ToInt32(shiftStartTime.Substring(0, 2)),
                            Convert.ToInt32(shiftStartTime.Substring(3, 2)), 0, 0);

                bool tooSoon = (punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime < periodStartDateTime.Add(shiftStart) && (shiftType != 1));

                bool startOfWeekThirdShift = (shiftType != 1 && punchDateTime.DayOfWeek == DayOfWeek.Sunday &&
                    punchDateTime > periodStartDateTime.AddHours(9) &&
                    punchDateTime < periodStartDateTime.AddHours(36));

                if (endOfWeekThirdShift && punchDateTime.TimeOfDay.Hours <= 9)
                    weekEnd2 = weekEnd.AddDays(7);
                else if (endOfWeekSecondShift && punchDateTime.TimeOfDay.Hours <= 5)
                    weekEnd2 = weekEnd.AddDays(7);
                else
                    weekEnd2 = weekEnd;
                DateTime pdt = this.GetCSTWeekEndingSaturdayDateFromDate(punchDateTime);
                if ((pdt.Day != weekEnd.Day && pdt.Day != weekEnd2.Day) || startOfWeekThirdShift)
                {
                    appliesToPeriod = false;
                }
                if (startOfWeekThirdShift)
                {
                    appliesToPeriod = true;
                }
                if (startOfWeek && shiftType > 1 && clientId != 258 || tooSoon)
                {
                    appliesToPeriod = false;
                }
            }

            if (sameWeekEnd)
            {
                return true;
            }
            else if (appliesToPeriod)
            {
                return NewSummaryAppliesToBillingPeriodSaturday(shiftStartTime, shiftEndTime, punchDateTime, periodStartDateTime, periodEndDateTime, clientId, shiftType, wsCount);
            }
            else
            {
                return false;
            }
        }

        //jhm
        public bool NewSummaryAppliesToBillingPeriodFriday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, DateTime punchWeekEnd, DateTime weekEnd, int clientId, int shiftType, int wsCount)
        {
            bool sameWeekEnd = false;
            bool appliesToPeriod = true;
            DateTime weekEnd2 = new DateTime(1900, 1, 1);
            DateTime nullDate = new DateTime(1900, 1, 1);

            if (punchDateTime.DayOfWeek == DayOfWeek.Friday)
            {
                if (punchWeekEnd != null && punchWeekEnd != nullDate)
                {
                    if (weekEnd.Day != punchWeekEnd.Day)
                    {
                        appliesToPeriod = false;
                    }
                    else
                    {
                        sameWeekEnd = true;
                    }
                }
            }
            else
            { /*
                if (this.GetCSTWeekEndingSaturdayDateFromDate(punchDateTime).Day != weekEnd.Day)
                {
                    appliesToPeriod = false;
                }
               */
                /* handle third shift starting on or after midnight on the last day of the roster term */
                bool endOfWeekThirdShift = (shiftType >= 3 && punchDateTime.DayOfWeek == DayOfWeek.Saturday &&
                    punchDateTime > periodStartDateTime.AddHours(24) && shiftStartTime.CompareTo(shiftEndTime) > 0);
                bool endOfWeekSecondShift = (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Saturday &&
                    punchDateTime > periodStartDateTime.AddHours(24));

                bool startOfWeek = (punchDateTime.DayOfWeek == DayOfWeek.Saturday &&
                    punchDateTime < periodStartDateTime.AddHours(9));

                bool startOfWeekThirdShift = (shiftType != 1 && punchDateTime.DayOfWeek == DayOfWeek.Saturday &&
                    punchDateTime < periodStartDateTime.AddHours(9));

                if (endOfWeekThirdShift && punchDateTime.TimeOfDay.Hours <= 9)
                    weekEnd2 = weekEnd.AddDays(7);
                else if (endOfWeekSecondShift && punchDateTime.TimeOfDay.Hours <= 5)
                    weekEnd2 = weekEnd.AddDays(7);
                else
                    weekEnd2 = weekEnd;
                DateTime pdt = this.GetCSTWeekEndingFridayDateFromDate(punchDateTime);
                if ((pdt.Day != weekEnd.Day && pdt.Day != weekEnd2.Day) || startOfWeekThirdShift)
                {
                    appliesToPeriod = false;
                }
                if (startOfWeekThirdShift)
                {
                    appliesToPeriod = true;
                }
                if (startOfWeek && shiftType > 1 && clientId != 258)
                {
                    appliesToPeriod = false;
                }
            }

            if (sameWeekEnd)
            {
                return true;
            }
            else if (appliesToPeriod)
            {
                return NewSummaryAppliesToBillingPeriodFriday(shiftStartTime, shiftEndTime, punchDateTime, periodStartDateTime, periodEndDateTime, clientId, shiftType, wsCount);
            }
            else
            {
                return false;
            }
        }

        public bool NewSummaryAppliesToBillingPeriodThursday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, DateTime punchWeekEnd, DateTime weekEnd, int clientId, int shiftType, int wsCount)
        {
            bool sameWeekEnd = false;
            bool appliesToPeriod = true;
            DateTime weekEnd2 = new DateTime(1900, 1, 1);
            DateTime nullDate = new DateTime(1900, 1, 1);

            if (punchDateTime.DayOfWeek == DayOfWeek.Thursday)
            {
                if (punchWeekEnd != null && punchWeekEnd != nullDate)
                {
                    if (weekEnd.Day != punchWeekEnd.Day)
                    {
                        appliesToPeriod = false;
                    }
                    else
                    {
                        sameWeekEnd = true;
                    }
                }
            }
            else
            { /*
                if (this.GetCSTWeekEndingSaturdayDateFromDate(punchDateTime).Day != weekEnd.Day)
                {
                    appliesToPeriod = false;
                }
               */
                /* handle third shift starting on or after midnight on the last day of the roster term */
                bool endOfWeekThirdShift = (shiftType >= 3 && punchDateTime.DayOfWeek == DayOfWeek.Friday &&
                    punchDateTime > periodStartDateTime.AddHours(24) && shiftStartTime.CompareTo(shiftEndTime) > 0);
                bool endOfWeekSecondShift = (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Friday &&
                    punchDateTime > periodStartDateTime.AddHours(24));

                bool startOfWeek = (punchDateTime.DayOfWeek == DayOfWeek.Friday &&
                    punchDateTime < periodStartDateTime.AddHours(9));

                bool startOfWeekThirdShift = (shiftType != 1 && punchDateTime.DayOfWeek == DayOfWeek.Friday &&
                    punchDateTime < periodStartDateTime.AddHours(9));

                if (endOfWeekThirdShift && punchDateTime.TimeOfDay.Hours <= 9)
                    weekEnd2 = weekEnd.AddDays(7);
                else if (endOfWeekSecondShift && punchDateTime.TimeOfDay.Hours <= 5)
                    weekEnd2 = weekEnd.AddDays(7);
                else
                    weekEnd2 = weekEnd;
                DateTime pdt = this.GetCSTWeekEndingThursdayDateFromDate(punchDateTime);
                if ((pdt.Day != weekEnd.Day && pdt.Day != weekEnd2.Day) || startOfWeekThirdShift)
                {
                    appliesToPeriod = false;
                }
                if (startOfWeekThirdShift)
                {
                    appliesToPeriod = true;
                }
                if (startOfWeek && shiftType > 1 && clientId != 258)
                {
                    appliesToPeriod = false;
                }
            }

            if (sameWeekEnd)
            {
                return true;
            }
            else if (appliesToPeriod)
            {
                return NewSummaryAppliesToBillingPeriodThursday(shiftStartTime, shiftEndTime, punchDateTime, periodStartDateTime, periodEndDateTime, clientId, shiftType, wsCount);
            }
            else
            {
                return false;
            }
        }

        int count = 0;
        public bool NewSummaryAppliesToBillingPeriod(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, DateTime punchWeekEnd, DateTime weekEnd, int clientId, int shiftType)
        {
            bool sameWeekEnd = false;
            count++;
            bool appliesToPeriod = true;
            DateTime weekEnd2 = new DateTime(1900, 1, 1);
            DateTime nullDate = new DateTime(1900, 1, 1);

            if (punchDateTime.DayOfWeek == DayOfWeek.Monday)
            {
                if (punchWeekEnd != null && punchWeekEnd != nullDate)
                {
                    if (weekEnd.Day != punchWeekEnd.Day)
                    {
                        appliesToPeriod = false;
                    }
                    else
                    {
                        sameWeekEnd = true;
                    }
                }
            }
            //else
            {/* handle third shift starting on or after midnight on the last day of the roster term */
                bool endOfWeekThirdShift = (shiftType == 3 && punchDateTime.DayOfWeek == DayOfWeek.Monday &&
                    punchDateTime > periodStartDateTime.AddHours(24));
                bool endOfWeekSecondShift = (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Monday &&
                    punchDateTime > periodStartDateTime.AddHours(24));
                bool startOfWeekThirdShift = (shiftType == 3 && punchDateTime.DayOfWeek == DayOfWeek.Monday &&
                    punchDateTime < periodStartDateTime.AddHours(9));
                startOfWeekThirdShift |= (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Monday &&
                    punchDateTime < periodStartDateTime.AddHours(5));
                if ((clientId == 279 || clientId == 286) && startOfWeekThirdShift == true && punchDateTime > periodStartDateTime.AddHours(5))
                    startOfWeekThirdShift = false;

                /* check if punch occurs before the shift starts for the week */
                TimeSpan shiftStartTS = new TimeSpan(Convert.ToInt32(shiftStartTime.Substring(0,2)), Convert.ToInt32(
                    shiftStartTime.Substring(3,2)),0);
                bool beforeStartOfWeekSecondShift = punchDateTime.DayOfWeek == DayOfWeek.Monday &&
                    punchDateTime < periodStartDateTime.Add(shiftStartTS) && shiftType == 2;
                if (beforeStartOfWeekSecondShift) appliesToPeriod = false;

                if (endOfWeekThirdShift && punchDateTime.TimeOfDay.Hours <= 9)
                    weekEnd2 = weekEnd.AddDays(7);
                else if (endOfWeekSecondShift && punchDateTime.TimeOfDay.Hours <= 5)
                    weekEnd2 = weekEnd.AddDays(7);
                else
                    weekEnd2 = weekEnd;
                DateTime pdt = this.GetCSTWeekEndingDateFromDate(punchDateTime);
                if ((pdt.Day != weekEnd.Day && pdt.Day != weekEnd2.Day) || startOfWeekThirdShift)
                {
                    appliesToPeriod = false;
                }
            }

            if (sameWeekEnd)
            {
                return true;
            }
            else if (appliesToPeriod)
            {
                return NewSummaryAppliesToBillingPeriod(shiftStartTime, shiftEndTime, punchDateTime, periodStartDateTime, periodEndDateTime, clientId, shiftType);
            }
            else
            {
                return false;
            }
        }
        public bool NewSummaryAppliesToBillingPeriodTuesday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, DateTime punchWeekEnd, DateTime weekEnd, int clientId, int shiftType)
        {
            bool sameWeekEnd = false;
            count++;
            bool appliesToPeriod = true;
            DateTime weekEnd2 = new DateTime(1900, 1, 1);
            DateTime nullDate = new DateTime(1900, 1, 1);

            if (punchDateTime.DayOfWeek == DayOfWeek.Tuesday)
            {
                if (punchWeekEnd != null && punchWeekEnd != nullDate)
                {
                    if (weekEnd.Day != punchWeekEnd.Day)
                    {
                        appliesToPeriod = false;
                    }
                    else
                    {
                        sameWeekEnd = true;
                    }
                }
            }
            else
            {/* handle third shift starting on or after midnight on the last day of the roster term */
                bool endOfWeekThirdShift = (shiftType == 3 && punchDateTime.DayOfWeek == DayOfWeek.Wednesday &&
                    punchDateTime > periodStartDateTime.AddHours(24));
                bool endOfWeekSecondShift = (shiftType == 2 && punchDateTime.DayOfWeek == DayOfWeek.Wednesday &&
                    punchDateTime > periodStartDateTime.AddHours(24));
                bool startOfWeekThirdShift = (shiftType != 1 && punchDateTime.DayOfWeek == DayOfWeek.Wednesday &&
                    punchDateTime < periodStartDateTime.AddHours(9));

                if (endOfWeekThirdShift && punchDateTime.TimeOfDay.Hours <= 9)
                    weekEnd2 = weekEnd.AddDays(7);
                else if (endOfWeekSecondShift && punchDateTime.TimeOfDay.Hours <= 5)
                    weekEnd2 = weekEnd.AddDays(7);
                else
                    weekEnd2 = weekEnd;
                DateTime pdt = this.GetCSTWeekEndingTuesdayDateFromDate(punchDateTime);
                if ((pdt.Day != weekEnd.Day && pdt.Day != weekEnd2.Day) || startOfWeekThirdShift)
                {
                    appliesToPeriod = false;
                }
            }

            if (sameWeekEnd)
            {
                return true;
            }
            else if (appliesToPeriod)
            {
                return NewSummaryAppliesToBillingPeriod(shiftStartTime, shiftEndTime, punchDateTime, periodStartDateTime, periodEndDateTime, clientId, shiftType);
            }
            else
            {
                return false;
            }
        }
        //jhm
        public bool NewSummaryAppliesToBillingPeriodSaturday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, int clientId, int shiftType, int wsCount)
        {
            bool retVal = true;

            if (clientId >= 325 && clientId <= 327 && punchDateTime.Date.Equals(periodStartDateTime.Date) && periodEndDateTime.Date > new DateTime(2016, 07, 25))
            {
                if (punchDateTime.Date.Equals(periodStartDateTime.Date))
                {
                    int start = Convert.ToInt32(shiftStartTime.Substring(0, shiftStartTime.IndexOf(":")));
                    if (start > 3 || ((punchDateTime.Hour - (start + 24)) % 24 > 2 && wsCount == 0))
                        return false;
                }
            }

            //make sure punch date/time is not for the next period
            //if (punchDateTime.Date >= periodEndDateTime.AddDays(1).Date)
            //{
            //    //should we check if the punch is missing a check in?
            //    retVal = false;
            //}
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 4))
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else if (punchDateTime.Date == periodEndDateTime.Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 23))
            {
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (shiftType != 1 && punchDateTime.TimeOfDay.Hours <= 6)
                        tempShiftStart = tempShiftStart.AddDays(-1);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        int diff = 6;
                        if (clientId == 7) /* JHM TEMPORARY - Special case for Simms */
                            diff = 9;
                        if (shiftStartDif.TotalHours > diff)
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        //jhm
        public bool NewSummaryAppliesToBillingPeriodFriday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, int clientId, int shiftType, int wsCount)
        {
            bool retVal = true;

            //make sure punch date/time is not for the next period
            //if (punchDateTime.Date >= periodEndDateTime.AddDays(1).Date)
            //{
            //    //should we check if the punch is missing a check in?
            //    retVal = false;
            //}
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 4))
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else if (punchDateTime.Date == periodEndDateTime.Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 23))
            {
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Saturday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (shiftType != 1 && punchDateTime.TimeOfDay.Hours <= 6)
                        tempShiftStart = tempShiftStart.AddDays(-1);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        int diff = 6;
                        if (clientId == 7) /* JHM TEMPORARY - Special case for Simms */
                            diff = 9;
                        if (shiftStartDif.TotalHours > diff)
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        public bool NewSummaryAppliesToBillingPeriodThursday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, int clientId, int shiftType, int wsCount)
        {
            bool retVal = true;

            //make sure punch date/time is not for the next period
            //if (punchDateTime.Date >= periodEndDateTime.AddDays(1).Date)
            //{
            //    //should we check if the punch is missing a check in?
            //    retVal = false;
            //}
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 4))
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else if (punchDateTime.Date == periodEndDateTime.Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 23))
            {
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Friday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (shiftType != 1 && punchDateTime.TimeOfDay.Hours <= 6)
                        tempShiftStart = tempShiftStart.AddDays(-1);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        int diff = 6;
                        if (shiftStartDif.TotalHours > diff)
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        public bool NewSummaryAppliesToBillingPeriod(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, int clientId, int shiftType)
        {
            bool retVal = true;

            //make sure punch date/time is not for the next period
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 4))
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else if (punchDateTime.Date <= periodStartDateTime.Date && (shiftType == 3 && punchDateTime.TimeOfDay.Hours <= 11))
            {
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Monday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    if (shiftType != 1 && punchDateTime.TimeOfDay.Hours <= 10)
                        tempShiftStart = tempShiftStart.AddDays(-1);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        int diff = 8;
                        if (clientId == 7 || clientId == 279 || clientId == 286) /* JHM TEMPORARY - Special case for Simms and ALG */
                            diff = 9;
                        if (shiftStartDif.TotalHours >= diff)
                        {
                            retVal = false;
                        }
                    }
                }
            }

            return retVal;
        }
        public bool NewSummaryAppliesToBillingPeriodTuesday(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime,
            DateTime periodEndDateTime, int clientId, int shiftType)
        {
            bool retVal = true;

            //make sure punch date/time is not for the next period
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date && (shiftType == 1 && punchDateTime.TimeOfDay.Hours >= 4))
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Wednesday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    if (shiftType != 1 && punchDateTime.TimeOfDay.Hours < 4)
                        tempShiftStart = tempShiftStart.AddDays(-1);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        int diff = 8;
                        if (clientId == 7 || clientId == 279 || clientId == 286) /* JHM TEMPORARY - Special case for Simms and ALG */
                            diff = 9;
                        if (shiftStartDif.TotalHours >= diff)
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }

        public static void SavePunchClockB64AsJpg(MobileDataIn mdi)
        {
            if (mdi.Image == null || mdi.Image.Length == 0) return;

            byte[] bytes = Convert.FromBase64String(mdi.Image);
            string path = GetClientEmployeeIDPath(mdi.Id, mdi.ClientId, mdi._phoneDateTime);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = mdi.Id + mdi._phoneDateTime.ToString("yyyyMMdd_HHmmss");

            Image image = null;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                image.Save(path + "\\" + filename + ".jpg", ImageFormat.Jpeg);
            }
        }

        internal static string GetClientEmployeeIDPath(string aident, string clientId, DateTime date)
        {
            int idVal = Convert.ToInt32(aident);
            idVal /= 100;
            string dirPath = "/";
            for (int i = 0; i < 4; i++)
            {
                dirPath = "/" + (idVal % 10) + dirPath;
                idVal /= 10;
            }
            string dt = date.ToString("yyyyMM");

            return HttpContext.Current.Server.MapPath("..\\" + "EmployeeImages\\" + clientId + "\\" 
                    + dirPath + "\\" + dt + "\\");
        }
    }
}