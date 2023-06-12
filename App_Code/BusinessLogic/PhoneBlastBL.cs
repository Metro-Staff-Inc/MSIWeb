using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.Collections.Specialized;
using MSI.Web.MSINet.BusinessEntities;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using System.Text.RegularExpressions;
using MSI.Web.MSINet.DataAccess;

/// <summary>
/// Summary description for PhoneBlastBL
/// </summary>
/// 
public class PhoneBlastBL
{
    string sid;
    string auth_token;
    string valid_phone;

    public String UpdateEmployeeNotes(String aident, String notes, String userId)
    {
        PhoneBlastDB pdb = new PhoneBlastDB();
        return pdb.UpdateEmployeeNotes(aident, notes, userId);
    }
	public PhoneBlastBL()
	{
        /* added 999 to start */
        sid = "ACbbe4b0445241034d79ffbf217abe0298";//System.Environment.GetEnvironmentVariable("TWILIO_SID");
        auth_token = "7bccca09f73114e3c631291b9c546fc0";// System.Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
        valid_phone = "7082610001";
    }

    public String GetWorkSchedule(String id)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        String newId = "";
        for (int i = 0; i < id.Length; i++)
            if (id[i] >= '0' && id[i] <= '9')
                newId = newId + id[i];
        return pbdb.GetWorkSchedule(newId);
    }

    public void PBListUpdate(string aident, string customListID, string action, string userID)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        pbdb.PBListUpdate(aident, Convert.ToInt32(customListID), Convert.ToInt32(action), userID);
    }

    public void SetSkillList(string aident, string skillDescriptionId)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        pbdb.SetSkillList(aident, skillDescriptionId);
    }

    public RecruitPool GetSkillList(string id)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        return pbdb.GetSkillList(id);
    }

    public List<PhoneBlastList> GetPhoneBlastLists()
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        return pbdb.GetPhoneBlastLists();
    }

    public RecruitPool GetPhoneBlastList(string id)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        return pbdb.GetPhoneBlastList(id);
    }

    public int GetPhoneBlastLanguage(string id)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        int val = pbdb.GetPhoneBlast(id).Language;
        return val;
    }

    public string GetPhoneBlast(string id)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        int val = pbdb.GetPhoneBlast(id).Response;
        if (val == 2)
            return "Available!";
        else if (val == 3)
            return "Not Available";
        else if (val == 4)
            return "Invalid Response";
        else
            return "No Response";
    }

    public void InitPhoneBlast(string id, int val)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        pbdb.SetPhoneBlast(id, val, -1); /* same as set Phone blast, just leave language selection alone... */
    }

    public void SetPhoneBlast(string id, int val, int lang)
    {
        PhoneBlastDB pbdb = new PhoneBlastDB();
        pbdb.SetPhoneBlast(id, val, lang);
    }

    /* get info on a call.  Twilio call ID required */
    public string GetCall(string id)
    {
        //JHM var client = new TwilioRestClient(sid, auth_token);
        //JHM Call call = client.GetCall(id);
        //JHM if (call != null)
        //JHM {
        //JHM return call.Status;
        //JHM }
        /* didn't find the matching callID in the roster */
        return "Not Available";
    }
    public string SendSMS(String to, String txtBody, String fromText, String fromCall)
    {
        //JHM var twilio = new TwilioRestClient(sid, auth_token);
        TwilioClient.Init(sid, auth_token);
        int start = 0;
        int sz = 1600;
        //JHM var message = new Twilio.SMSMessage();
        String newBody = "";
        for (int i = 0; i < txtBody.Length; i++ )
        {
            char ch = txtBody[i];
            newBody = newBody + ch;
        }

        while (newBody.Length > 0)
        {
            if ((start + sz) > newBody.Length)
            {
                //twilio.
                //JHM message = twilio.SendSmsMessage("+17082610001", to, newBody.Substring(start));
                try
                {
                    var message = MessageResource.Create(to: to,
                                                        from: "+17082610001",
                                                        body: newBody.Substring(start));
                    newBody = "";
                }
                catch( Exception e )
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    newBody = "";
                }
            }
            else
            {
                String temp = newBody.Substring(start, sz);
                //JHM message = twilio.SendSmsMessage("+17082610001", to, temp);
                var message = MessageResource.Create(to: to,
                                                    from: "+17082610001",
                                                    body: temp);
                newBody = newBody.Substring(start + sz);
            }
        }

        /* strip non-numerics */
        string newTo = "";
        string textback = "";
        string callback = "";

        for (int i = 0; i < to.Length; i++)
        {
            if (i < to.Length && to[i] >= '0' && to[i] <= '9')
            {
                newTo += to[i];
            }
        }
        if (fromText == null) fromText = "";
        if (fromCall == null) fromCall = "";
        for (int i = 0; i < fromText.Length; i++)
        {
            if (i < fromText.Length && fromText[i] >= '0' && fromText[i] <= '9')
            {
                textback += fromText[i];
            }
        }
        for (int i = 0; i < fromCall.Length; i++)
        {
            if (i < fromCall.Length && fromCall[i] >= '0' && fromCall[i] <= '9')
            {
                callback += fromCall[i];
            }
        }
        if (newTo.Length > 0 && newTo[0] == '1') newTo = newTo.Substring(1);
        if (callback.Length > 0 && callback[0] == '1') callback = callback.Substring(1);
        if (textback.Length > 0 && textback[0] == '1') textback = textback.Substring(1);
        if (newTo.Length == 10 && callback.Length == 10 && textback.Length == 10) /* 3 (area) 7 ( number) */
        {
            PhoneBlastDB pbdb = new PhoneBlastDB();
            pbdb.UpdateEmployeeContact(newTo, textback, callback, DateTime.Now);
        }
        return "";
        //JHM return message.ToString();
    }

    public string MakeCall(string to, string msgUrl, string callback)
    {
        //JHM var client = new TwilioRestClient(sid, auth_token);
        //JHM Call call = null;
        //JHM CallOptions c = new CallOptions();
        //JHM c.From = valid_phone;
        //JHM c.To = to;
        //JHM c.Url = msgUrl;
        //JHM c.StatusCallbackMethod = "get";
        //JHM c.Method = "get";

        //if( callback != null )
        //JHM call = client.InitiateOutboundCall(c);
        //else
        //    call = client.InitiateOutboundCall(valid_phone, to, msgUrl);

        //JHM if (call == null)
        //JHM return "waiting";

        //JHM if (call.RestException == null){
        //JHM InitPhoneBlast(call.To, 1);
        //JHM return call.Sid; //Response.Write(string.Format("Started call: {0}", call.Sid));
        //JHM } else {
        return "-1";
            //return call.RestException.Message; //Response.Write(string.Format("Error: {0}", call.RestException.Message));
        }
    //JHM     }

    public string GetCapabillityToken()
    {
        TwilioCapability capabilityTokenGenerator = new TwilioCapability(sid, auth_token);
        capabilityTokenGenerator.AllowClientIncoming("msiDispatch");
        capabilityTokenGenerator.AllowClientOutgoing("APa2ef9ca61072a23417f93ed0f9046e21");
        return capabilityTokenGenerator.GenerateToken();
    }

    public XElement ReceiveText()
    {
        XElement xmlTree = null;
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string from = d.Get("from");
        string fromIn = "";
        string bodyIn = d.Get("body");
        string body = "";
        string resp = "Sorry! invalid input!";
        for (int i = 0; i < bodyIn.Length; i++)
        {
            if (bodyIn.ToCharArray()[i] >= '0' && bodyIn.ToCharArray()[i] <= '9')
                body += bodyIn.ToCharArray()[i];
        }
        if (body.Length > 3 && body.Length < 7)
        {
            PhoneBlastDB pdb = new PhoneBlastDB();
            resp = pdb.GetWorkSchedule(body);
        }
        else
        {
            for (int i = 0; i < from.Length; i++)
            {
                if (from[i] >= '0' && from[i] <= '9')
                    fromIn += from[i];
            }
            if (fromIn[0] == '1') fromIn = fromIn.Substring(1);
            PhoneBlastDB pdb = new PhoneBlastDB();
            PBEmployeeContact contact = pdb.GetEmployeeContact(fromIn);
            if (contact == null || contact.MSITextNum == null || contact.MSITextNum.Length < 10)
            {
                resp = "Input not recognized, text your ID to this number for your work schedule";
            }
            else
            {
                bodyIn = "Message From:\n" + contact.LastName + ", " + contact.FirstName + " - " + contact.Id + 
                    "\n" + "(" + contact.EmployeePhoneNum.Substring(0, 3) +") " + contact.EmployeePhoneNum.Substring(3,3) + " - " + contact.EmployeePhoneNum.Substring(6) + 
                    "\nLast Contacted: " + contact.ContactDate + "\n------------\n" + bodyIn;
                //JHM var twilio = new TwilioRestClient(sid, auth_token);
                TwilioClient.Init(sid, auth_token);
                var message = MessageResource.Create(to: contact.MSITextNum,
                                                    from: "+17082610001",
                                                    body: bodyIn);
                //JHM var message = twilio.SendSmsMessage("+17082610001", contact.MSITextNum, bodyIn);
                //twilio.
                resp = "Message forwarded."; /* "Message sent to: (" + contact.MSIPhoneNum.Substring(0, 3) + ")" +
                     contact.MSIPhoneNum.Substring(3, 3) + "-" + contact.MSIPhoneNum.Substring(6, 4); */
            }
        }
        xmlTree =
            new XElement("Response",
                new XElement("Message", resp));
        return xmlTree;
    }

    public TwilioResponse PBCallQueue()
    {
        NameValueCollection nvc = HttpContext.Current.Request.Form;
        string digits = nvc.Get("Digits");
        var response = new TwilioResponse();
        if (digits == null)
        {
            var pos = Convert.ToInt32(nvc.Get("QueuePosition")) - 1;
            var posStatement = "";
            if (pos == 0)
                posStatement = "You are the next caller in line.";
            else if (pos == 1)
                posStatement = "There is one caller ahead of you";
            else
                posStatement = "There are " + pos + " callers ahead of you.";
            response.BeginGather(new { numDigits = "1", method = "get", timeout = "30" })
                .Play("https://metrostaff.fwd.wf/RestServiceWWW/Music/30UneSimpleCartePostale.mp3")
                .Say("Thank you for holding")
                .Pause(1)
                .Say(posStatement)
                .EndGather();
            response.Redirect("https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBCallQueue");
        }
        else
        {
            response.Say("Thank you.  A dispatch operator will call you.");
        }
        return response;
    }
    public TwilioResponse PBJobGreeting()
    {
        //NameValueCollection nvc = HttpContext.Current.Request.Form;
        string digits = HttpContext.Current.Request["Digits"];
        string phoneFrom = HttpContext.Current.Request["From"];
        string phoneTo = HttpContext.Current.Request["To"];
        var response = new TwilioResponse();
        int lang = -1;
        lang = GetPhoneBlastLanguage(phoneTo); 

        response.BeginGather(new { /*action = "https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobQuery",*/
                                   action = "http://www.msiwebtrax.com/Roster/PBJobQuery",
            numDigits = "1", method = "get", timeout="2" });
        if (lang == 0)
        {
            response.Play("http://www.msiwebtrax.com/Audio/Pick%20Up%20-%20English.MP3");
            //response.Say("Hello, This is an automated call from Metro Staff");
            //response.Say("Pair uh Es pan yol, pressio new murrow oh cho");
        }
        else
        {
            response.Play("http://www.msiwebtrax.com/Audio/Pickup%20Scripting%20-%20Spanish.MP3");
            //response.Say("Hola, Este es un telefona call para Metro Staff");
            //response.Say("For English, press seven");
        }
        response.EndGather();
        //response.Redirect("https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobQuery");
        response.Redirect("http://www.msiwebtrax.com/Roster/PBJobQuery","get");
        return response;
    }

    public TwilioResponse PBJobQuery()
    {
        //NameValueCollection nvc = HttpContext.Current.Request.Url.;// HttpContext.Current.Request.Form;
        string digits = HttpContext.Current.Request["Digits"]; //nvc.Get("Digits");
        string phoneFrom = HttpContext.Current.Request["From"];
        string phoneTo = HttpContext.Current.Request["To"];
        var response = new TwilioResponse();
        int lang = -1;
        lang = GetPhoneBlastLanguage(phoneTo);
        if (digits == null )
        {
            response.BeginGather(new
            {
                /*action = "https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobQuery",*/
                action = "http://www.msiwebtrax.com/Roster/PBJobQuery",
                numDigits = "1", method = "get", timeout = "30" });
            if (lang == 0)
            {
                response.Play("http://www.msiwebtrax.com/Audio/Jobs%20Available%20-%20English.MP3");
                //response.Say("There are jobs available today.  If you are available and want to work, please press 1");
                //response.Say("If you are NOT available to work, please press 2.");
                response.EndGather();
            }
            else
            {
                response.Play("http://www.msiwebtrax.com/Audio/Jobs%20Available%20Scripting%20-%20Spanish.MP3");
                //response.Say("Este es trabajos!  yahmeh me, pour favor!");
                //response.Say("Pressio uno oh dosay");
                response.EndGather();
            }
        }
        else
        {

            if (digits.Equals("1"))
            {
                if (lang == 0)
                {
                    response.Play("http://www.msiwebtrax.com/Audio/Available%20Scripting%20-%20English.MP3");
                    response.Play("http://www.msiwebtrax.com/Audio/Thank%20You%20Scripting%20-%20English.MP3");
                    //response.Say("You are available to work.  An operator will call you shortly to confirm. Thank you and have a nice day.");
                }
                else
                {
                    response.Play("http://www.msiwebtrax.com/Audio/Available%20Scripting%20-%20Spanish.MP3");
                    response.Play("http://www.msiwebtrax.com/Audio/Thank%20You%20Scripting%20-%20Spanish.MP3");
                }
                //response.Say("Tu eres trabajo.  Gracias e buenas dias!");
                SetPhoneBlast(phoneTo, 2, lang);
            }
            else if (digits.Equals("2"))
            {
                if (lang == 0)
                {
                    response.Play("http://www.msiwebtrax.com/Audio/Not%20Available%20Scripting%20-%20English.MP3");
                    response.Play("http://www.msiwebtrax.com/Audio/Thank%20You%20Scripting%20-%20English.MP3");
                    //response.Say("You are unavailable to work.  Thank you and have a nice day.");
                }
                else
                {
                    response.Play("http://www.msiwebtrax.com/Audio/Not%20Available%20Scripting%20-%20Spanish.MP3");
                    response.Play("http://www.msiwebtrax.com/Audio/Thank%20You%20Scripting%20-%20English.MP3");
                    //response.Say("Too no es trabajo.  Gracias e buenas dias!");
                }
                SetPhoneBlast(phoneTo, 3, lang);
            }

            if (digits.Equals("8"))
            {
                /* spanish */
                lang = 1;
                SetPhoneBlast(phoneTo, 0, lang);
                //response.Redirect("https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobQuery");
                response.Redirect("http://www.msiwebtrax.com/Roster/PBJobQuery", "get");
            }
            else if( digits.Equals("7"))
            {
                /* english */
                lang = 0;
                SetPhoneBlast(phoneTo, 0, lang);
                //response.Redirect("https://metrostaff.fwd.wf/RestServiceWWW/Roster/PBJobQuery");
                response.Redirect("http://www.msiwebtrax.com/Roster/PBJobQuery", "get");
            }
        }
        return response;
    }

    //JHM public Action<Call> PBCallBack()
    //JHM {
    //JHM NameValueCollection nvc = HttpContext.Current.Request.QueryString;
    //JHM Console.WriteLine(nvc.ToString());
    //JHM     return null;
    //JHM }

    public XElement BrowserCall()
    {
        //NameValueCollection d = HttpContext.Current.Request.QueryString;
        //string digits = d.Get("Digits");
        string phoneNum = HttpContext.Current.Request["PhoneNumber"];
        XElement xmlTree = initXMLTree();
            xmlTree = new XElement("Response",
                        new XElement("Dial", 
                            new XAttribute("callerId", "17082610001"), phoneNum))
                            ;
        return xmlTree;
    }

    /* Phone reception */
    public TwilioResponse ReceiveCall()
    {
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string digits = d.Get("Digits");
        string from = d.Get("from");
        string fromIn = "";
        //XElement xmlTree = initXMLTree();
        TwilioResponse response = new TwilioResponse();
        if (digits == null) /* fresh call, not a key press response... */
        {
            for (int i = 0; i < from.Length; i++)
            {
                if (from[i] >= '0' && from[i] <= '9')
                    fromIn += from[i];
            }
            if (fromIn[0] == '1') fromIn = fromIn.Substring(1);
            PhoneBlastDB pdb = new PhoneBlastDB();
            PBEmployeeContact contact = pdb.GetEmployeeContact(fromIn);
            if (contact != null && contact.MSIPhoneNum != null && contact.MSIPhoneNum.Length > 9)
            {
                response.
                    Say("Thank you for calling the M.S.I. Dispatch Office. Transfering your call").
                    Dial(contact.MSIPhoneNum);
            }
            else
            {
                response.BeginGather(new { numDigits = "1", method = "GET", timeout = "40" }).
                                Say("Thank you for calling the Metro Staff Dispatch Offices. ").
                                Pause(1).
                                Say("For uh Roar rah, please press one.").
                                Pause(1).
                                Say("For Bartlett, please press two. ").
                                Pause(1).
                                Say("For Bolingbrook, please press three.").
                                Pause(1).
                                Say("For Elgin, please press four.").
                                Pause(1).
                                Say("For Elk Grove, please press five.").
                                Pause(1).
                                Say("For Schaumburg, please press six.").
                                Pause(1).
                                Say("For Villa Park, please press seven.").
                                Pause(1).
                                Say("For Wheeling, please press eight.").
                                Pause(1).
                                Say("For the corporate office, please press zero.").
                                Pause(1).
                                Say("To Leave a message, please press nine. ").
                                EndGather();
            }
             
        }
        else /* there is digit data */
        {
            char digit = '0';
            digit = (digits.ToCharArray())[digits.Length-1];

            if (digit == '1')
            {
                response.
                    Say("Transferring to the uh roar rah Dispatch Office. ").
                    Dial("6304997000");
            }
            else if (digit == '2')
            {
                response.
                    Say("Transferring to the Bartlett Dispatch Office. ").
                    Dial("6303725900");
            }
            else if (digit == '3')
            {
                response.
                    Say("Transferring to the Bolingbrook Dispatch Office. ").
                    Dial("8477599834");
            }
            else if (digit == '4')
            {
                response.
                    Say("Transferring to the Elgin Dispatch Office. ").
                    Dial("8477424405");
            }
            else if (digit == '5')
            {
                response.
                    Say("Transferring to the Elk Grove Dispatch Office. ").
                    Dial("8477348300");
            }
            else if (digit == '6')
            {
                response.
                    Say("Transferring to the West Chicago Dispatch Office. ").
                    Dial("6305622000");
            }
            else if (digit == '7')
            {
                response.
                    Say("Transferring to the Villa Park Dispatch Office. ").
                    Dial("6307058000");
            }
            else if (digit == '8')
            {
                response.
                    Say("Transferring to the Wheeling Dispatch Office. ").
                    Dial("8474597788");
            }
            else if (digit == '0')
            {
                response.
                    Say("Transferring to the Metro Staff Corporate Office. ").
                    Dial("8477429900");
            }
            else if (digit == '9')
            {
                response.
                    Say("Please leave a message after the tone.").
                    Record(new { maxLength = "30", action = "http://www.msiwebtrax.com/Roster/RecruitMessageRecorded", method = "GET" });
            }
        }
        return response;
    }

    public XElement HandleITResponse()
    {
        XElement xmlTree = initXMLTree();
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string digits = d.Get("Digits");
        if (digits != null)
        {
            char digit = digits.ToCharArray()[digits.Length - 1];
            if (digit >= '0' && digit <= '9')
            {
                if (digit == '2')
                {
                    xmlTree = RecordITMessage();
                }
                else if (digit == '1')
                {
                    xmlTree = 
                        new XElement("Response",
                           new XElement("Say", "Transferring to the I.T. help desk."),
                           new XElement("Dial", "8477429900"));
                }
            }
        }
        return xmlTree;
    }
    public XElement HandleRecruitResponse()
    {
        XElement xmlTree = initXMLTree();
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string digits = d.Get("Digits");
        if (digits != null)
        {
            char digit = digits.ToCharArray()[digits.Length - 1];
            if (digit >= '0' && digit <= '9')
            {
                if (digit == '2')
                {
                    xmlTree = RecordRecruitMessage();
                }
                else if (digit == '1')
                {
                    xmlTree = 
                        new XElement("Response",
                            new XElement("Say", "Transferring to the recruiting department."),
                            new XElement("Dial", "8477429900"));
                }
            }
        }
        return xmlTree;
    }

    public XElement RecordRecruitMessage()
    {
        XElement xmlTree = 
            new XElement("Response",
                new XElement("Say", "Please leave your message for the Recruiting Department at the beep. "),
                new XElement("Pause", 1),
                new XElement("Say", "Press any key when finished."),
                new XElement("Record", 
                                new XAttribute("method", "GET"), new XAttribute("maxLength", 300)),
                new XElement("Say", "I did not receive a recording. Transferring to an operator."),
                new XElement("Dial", "8477429900")
                );
        return xmlTree;
    }

    public XElement RecordITMessage()
    {
        XElement xmlTree = 
            new XElement("Response",
                new XElement("Say", "Please leave your message or request for the I.T. Department at the beep. " + 
                                    "Press any key when finished."),
                new XElement("Record", new XAttribute("action", "ITMessageRecorded"), new XAttribute("method", "GET"), new XAttribute("maxLength", 300)),
                new XElement("Say", "I did not receive a recording. Transferring to an operator."),
                new XElement("Dial", "8477429900")
                );
        return xmlTree;
    }

    public XElement RecruitMessageRecorded()
    {
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string recordingSID = d.Get("RecordingSID");
        XElement xmlTree =
            new XElement("Response",
                new XElement("Sms", "Thank you for contacting the MSI Dispatch Offices."
                    + "  We will review your message and get back to you as soon as possible. "
                    + " Reference ID #" + recordingSID.Substring(recordingSID.Length - 8)
                ),
                new XElement("Say", "Thank you for calling!  Goodbye.")
            );
        return xmlTree;
    }
    
    public XElement ITMessageRecorded()
    {
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        string recordingSID = d.Get("RecordingSID");
        XElement xmlTree =
            new XElement("Response",
                new XElement("Sms", "Thank you for contacting the MSI I.T. Department." 
                    + "  We will address your request as soon as possible. "
                    + " Reference ID #" + recordingSID.Substring(recordingSID.Length-8) 
                ),
                new XElement("Say", "Thank you for calling!  Goodbye.")
            );
        return xmlTree;
    }

    public XElement initXMLTree()
    {
        XElement xmlTree = new XElement("Response",
                            new XElement("Say", "Thank you for calling.  Goodbye."));
        return xmlTree;
    }

    public XElement ReceiveCallSpanish()
    {
        NameValueCollection d = HttpContext.Current.Request.QueryString;
        Console.WriteLine(d.ToString());
        XElement xmlTree1 = new XElement("Response",
            new XElement("Say", "Thank you for call the M. S. I. I.T. Department."),
            new XElement("Say", new XAttribute("voice", "female"), "Here we go")
        );
        return xmlTree1;
    }
}