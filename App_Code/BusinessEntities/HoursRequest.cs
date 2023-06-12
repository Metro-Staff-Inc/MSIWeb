using System;
using System.Collections.Generic;

namespace ApiWebServices_HoursData
{
    public class HoursResponseFlat
    {
        public HoursResponseFlat()
        {
            Data = new List<HoursDataFlat>();
        }
        public string Msg { get; set; }
        public bool Success { get; set; }
        public List<HoursDataFlat> Data { get; set; }
    }

    public class HoursRequest
    {
        public HoursRequest()
        {
            _weekEndDate = DateTime.Now;

        }
        public int ClientID { get; set; }
        public string WeekEndDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime _weekEndDate { get; set; }
    }
    /*
     * 	SELECT e.last_name, e.first_name, e.aident_number, d.department_name, cid.shift_type, 
		cid.total_regular_hours, cid.total_ot_hours, cih.week_end_dt
    */
    public class HoursDataFlat
    {
        public string ClientName { get; set; }
        public string LocationName { get; set; }
        public string Aident { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DepartmentName { get; set; }
        public int ShiftType { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OTHours { get; set; }
        public string WeekEndDate { get; set; }
        public string StartDate { get; set; }
        public string DNRDate { get; set; }
        public string DNRReason { get; set; }
        public string PrimaryDepartment { get; set; }
        public string JobTitle { get; set; }
        public string TerminationReason { get; set; }
        public override string ToString()
        {
            return "ClientName: " + ClientName +
                "\nLocationName: " + LocationName +
                "\nAident: " + Aident +
                "\nLastName: " + LastName +
                "\nDepartmentName: " + DepartmentName +
                "\nShiftType: " + ShiftType +
                "\nWeekEndDate: " + WeekEndDate +
                "\nRegularHours: " + RegularHours +
                "\nOTHours: " + OTHours +
                "\nStart Date: " + StartDate +
                "\nDNR Date: " + DNRDate +
                "\nTermination Reason: " + TerminationReason + 
                "\nPrimary Department: " + PrimaryDepartment + 
                "\nJob Title: " + JobTitle;
        }
    }
}
