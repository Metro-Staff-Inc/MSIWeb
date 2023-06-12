using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GMPInfo
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class GMPInfo
    {
        public String Aident { get; set; }
        public DateTime GMPDate { get; set; }
        public DateTime HireDate { get; set; }
        public int ShiftType { get; set; }
        public String Name { get; set; }
        public Boolean NLA { get; set; }

        public GMPInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}