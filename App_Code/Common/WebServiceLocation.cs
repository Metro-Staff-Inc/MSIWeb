using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WebServiceLocation
/// </summary>
namespace WebServicesLocation
{
    public class WebServiceLocation
    {
        //public static string WebServiceLoc = "msiwebtrax.com/";
        public static string WebServiceLoc = "msiwebtrax.com/";
        //public static string WebServiceLoc = "localhost:57282/RestServiceWWW/";
        //public static string WebServiceLoc = "172.18.10.242/";
        public static string GetClient(string client)
        {
            if (client.Equals("178") || client.Equals("256") || client.Equals("280") || 
                        client.Equals("281") || client.Equals("292") || client.Equals("293"))
                client = "Creative";
            if (client.Equals("259") || client.Equals("272"))
                client = "259-272";
            if (client.Equals("166") || client.Equals("279") || client.Equals("286"))
                client = "166-279";
            if (client.Equals("307") || client.Equals("308"))
                client = "307";
            if (client.Equals("332") || client.Equals("333") || client.Equals("334"))
                client = "332";
            return client;
        }
        public WebServiceLocation()
        {
        }
    }
}