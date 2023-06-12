using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PunchData
/// </summary>
namespace OpenWebServices
{
    public class HoursReport
    {
        public List<Client> Client { get; set; }
        public String DateStart;
        public String DateEnd;
    }
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public List<Department> Department { get; set; }
    }
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Shift> Shift { get; set; }
    }
    public class Shift
    {
        public int Type { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public List<Employee> Employee { get; set; }
    }
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Punch> Day { get; set; }
        public string Id { get; set; }
    }
    public class Punch
    {
        //public List<DateTime> Swipe { get; set; }
        public DateTime Swipe { get; set; }
        public DateTime Swipe2 { get; set; }
        public double Hours { get; set; }
        public String Date { get; set; }
    }

    public class PunchData
    {
        public PunchData()
        {
            //
            // TODO: Add constructor logic here
            //
            Console.WriteLine("Creating punch data!");
        }
        public DateTime ReportDate { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime PunchExact { get; set; }
        public DateTime PunchRound { get; set; }
        public String ShiftStart { get; set; }
        public String ShiftEnd { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ShiftType { get; set; }
        public string ShiftName { get; set; }
        public Boolean Overnight { get; set; }
        public Double PayRate { get; set; }
        public Double BreakTime { get; set; }
        public Double MarkUp { get; set; }

    }
}