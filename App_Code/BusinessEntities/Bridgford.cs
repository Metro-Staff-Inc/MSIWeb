using System.Collections.Generic;

/// <summary>
/// Summary description for Bridgford
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class Bridgford
    {
        public Bridgford()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    public class BridgfordOut
    {
        public BridgfordOut()
        {
            EmployeeData = new List<BridgfordEmployeeData>();
            Msg = "";
        }
        public List<BridgfordEmployeeData> EmployeeData { get; set; }
        public string Date { get; set; }
        public string Msg { get; set; }
    }
    public class BridgfordIn
    {
        public string Date { get; set; }
    }
    public class BridgfordEmployeeData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Aident { get; set; }
        public string Department { get; set; }
        public string Shift { get; set; }
        public decimal TotalHours { get; set; }
    }
}