using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for Roster
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    public class RosterInfo
    {
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public string TrackingStart { get; set; }
        public string TrackingEnd { get; set; }
        public String DeptName { get; set; }
        public string ShiftType { get; set; }
        public string ClientName { get; set; }
        public List<Roster> rosters { get; set; }
    }
    public class Roster
    {
        public int RosterID { get; set; }
        public string LastName { get;set; }
        public string FirstName { get;set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public string ID { get;set; }
        public string SubID { get; set; }
        public string Office { get; set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get;set; }
        public int Dept { get;set; }
        public int Shift { get;set; }  // shift type -- 1st, 2nd, etc...
        public string ShiftStart { get; set; }
        public string ShiftEnd { get; set; }
        public double HoursWorked { get; set; }  //hours worked in CURRENT pay period
        public int SubstituteFor { get; set; }
        public string Phone { get; set; }
        public int WorkStatus { get; set; }
        public string SID { get; set; }             //Twilio call ID
        public string ErrorMsg { get; set; }           //Twilio Error Message
    }
}