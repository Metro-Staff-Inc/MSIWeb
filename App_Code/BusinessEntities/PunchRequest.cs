using System;
using System.Collections.Generic;

namespace ApiWebServices_PunchData
{
    public class PunchResponseFlat
    {
        public PunchResponseFlat()
        {
            Data = new List<PunchData>();
        }
        public string Msg { get; set; }
        public bool Success { get; set; }
        public List<PunchData> Data { get; set; }
    }

    public class PunchResponse
    {
        public PunchResponse()
        {
        }
        public string Msg { get; set; }
        public bool Success { get; set; }
        public Client Client { get; set; }
    }

    public class PunchRequest
    {
        public int ClientID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class Client
    {
        public Client(int ID=0, string Name = "")
        {
            this.ID = ID;
            this.Name = Name;
            Locations = new Dictionary<int, Location>();
        }
        public int ID { get; set; }
        public String Name { get; set; }
        public Dictionary<int, Location> Locations { get; set; }
    }
    public class Location
    {
        public int ID { get; set; }
        public String Name { get; set; }
        public Dictionary<int, Department> Departments { get; set; }
        public Location() { }
        public Location(int ID, string LocationName = null)
        {
            this.ID = ID;
            this.Name = LocationName;
            Departments = new Dictionary<int, Department>();
        }
    }
    public class Department
    {
        public Department() { }
        public Department(int id, string name = "", decimal basePay = 0)
        {
            ID = id;
            Name = name;
            Shifts = new Dictionary<int, Shift>();
            BasePay = basePay;
        }
        public int ID { get; set; }
        public String Name { get; set; }
        public Dictionary<int, Shift> Shifts { get; set; }
        public decimal BasePay { get; set; }
    }
    public class Shift
    {
        public Shift(int id = 0, int type = 0, string startTime = "", string endTime = "", string startTracking = "",
                string endTracking = "", string name = "")
        {
            ID = id;
            ShiftType = type;
            Name = name;
            Employees = new Dictionary<string, Employee>();
            StartTime = startTime;
            EndTime = endTime;
            StartTracking = startTracking;
            EndTracking = endTracking;
        }
        public int ID { get; set; }
        public String Name { get; set; }
        public int ShiftType { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public String StartTracking { get; set; }
        public String EndTracking { get; set; }

        public Dictionary<string, Employee> Employees { get; set; } 
    }
    public class Employee
    {
        public Employee(string ID=null, string LastName=null, string FirstName= null)
        {
            this.ID = ID;
            this.LastName = LastName;
            this.FirstName = FirstName;
            Punches = new List<Punch>();
        }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String ID { get; set; }
        public DateTime FirstPunch { get; set; }
        public List<Punch> Punches { get; set; }
    }
    public class Punch
    {
        public Punch(DateTime exact, DateTime rounded, string createdBy = "", string roundedBy = "", string movedBy = "")
        {
            Exact = exact;
            Rounded = rounded;
            CreatedBy = createdBy;
            RoundedBy = roundedBy;
            MovedBy = movedBy;
        }
        public DateTime Exact { get; set; }
        public DateTime Rounded { get; set; }
        public String CreatedBy { get; set; }
        public String RoundedBy { get; set; }
        public String MovedBy { get; set; }
    }

//    SELECT cr.client_id, cr.location_id, cr.client_roster_id, ep.punch_dt, ep.rounded_punch_dt, ep.aident_number, e.last_name, e.first_name,
//                ep.department_id, ep.shift_type, o.office_cd, o.office_name, cj.job_code, cjo.job_code as job_code_override, cp.pay_rate, cpo.pay_rate as pay_rate_override,
//                au.UserName, d.department_name

    public class PunchData
    {
        public int ClientID { get; set; }
        public string LocationName { get; set; }
        public int LocationID { get; set; }
        public string ClientName { get; set; }
        public string Aident { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime PunchDate { get; set; }
        public DateTime RoundedPunchDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ShiftType { get; set; }
        public decimal BreakTime { get; set; }
        public decimal PayRate { get; set; }
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public override string ToString()
        {
            return "ClientID: " + ClientID +
                "\nClientName: " + ClientName +
                "\nAident: " + Aident +
                "\nLastName: " + LastName +
                "\nDepartmentId: " + DepartmentId +
                "\nDepartmentName: " + DepartmentName +
                "\nPunchDate: " + PunchDate +
                "\nRoundedPunchDate: " + RoundedPunchDate +
                "\nCreatedBy: " + CreatedBy +
                "\nCreatedDate: " + CreatedDate +
                "\nShiftType: " + ShiftType +
                "\nBreakTime: " + BreakTime +
                "\nLocationID: " + LocationID +
                "\nPayRate: " + PayRate +
                "\nShiftStart: " + ShiftStart +
                "\nShiftEnd: " + ShiftEnd;
        }
    }
}

