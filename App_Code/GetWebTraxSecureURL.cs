using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace MSI.Web.Services
{

    /// <summary>
    /// Summary description for MSIWebTraxCheckIn
    /// </summary>
    [WebService(Namespace = "http://msiwebtrax.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetWebTraxSecureURL : System.Web.Services.WebService
    {
        public GetWebTraxSecureURL()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string GetWebTraxCheckInSecureURL()
        {
            return "INVALID WEB ADDRESS";
            //return "https://msiweb.sslcert19.com/Services/MSIWebTraxCheckIn.asmx";
            //return "http://localhost:49212/www/Services/MSIWebTraxCheckIn.asmx";
        }
    }
}
