using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data.Common;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
/// <summary>
/// Summary description for PunchReport
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    public class PunchRecord : IComparer
    {
        public PunchRecord()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Compare(object o1, object o2)
        {
            PunchRecord pr1 = (PunchRecord)o1; 
            PunchRecord pr2 = (PunchRecord)o2;

            return pr1.CreatedDate.CompareTo(pr2.CreatedDate);
        }

        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public int Shift { get; set; }
        public string AidentNumber { get; set; }
        public DateTime PunchDate { get; set; }
        public DateTime RoundedPunchDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class PunchRecordDisplay
    {
        public string AidentNumber { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int Shift { get; set; }
        public DateTime PunchIn { get; set; }
        public DateTime PunchOut { get; set; }
        public double Hours { get; set; }
    }

    public class PunchReport
    {

        public PunchReport()
        {
            UserIdList = new ArrayList();
            PunchRecord = new ArrayList();
        }
        public ArrayList PunchRecord { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ArrayList UserIdList { get; set; }
        public int ClientID { get; set; }
        public string UserID { get; set; }
    }
}