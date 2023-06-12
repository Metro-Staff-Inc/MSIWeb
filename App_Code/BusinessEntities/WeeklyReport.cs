using System;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// This is the class info for retreiving hours report data via web-service
    /// </summary>
    public class WeeklyReport
    {
        public List<ShiftData> shifts { get; set; }
        public Double Reg { get; set; }
        public Double OT { get; set; }
        public Double Total { get; set; } /* regular + overtime */
        public Double ExactReg { get; set; }
        public Double ExactOT { get; set; }
        public Double ExactTotal { get; set; }
    }

    public class ShiftData
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public List<EmployeeData> Employees { get; set; }
        public Double Reg { get; set; }
        public Double OT { get; set; }
        public Double Total { get; set; } /* regular + overtime */
        public Double ExactReg { get; set; }
        public Double ExactOT { get; set; }
        public Double ExactTotal { get; set; }
    }

    public class EmployeeData
    {
        public String Name { get; set; }
        public String Badge { get; set; }
        public List<DailyPunches> Days { get; set; }
        public Double Reg { get; set; }
        public Double OT { get; set; }
        public Double Total { get; set; }
        public Double ExactReg { get; set; }
        public Double ExactOT { get; set; }
        public Double ExactTotal { get; set; }
        public Double PayRate { get; set; }
        public String JobCode { get; set; }
    }

    public class DailyPunches
    {
        public List<PunchData> Punches { get; set; }
        public Double Exact { get; set; }
        public Double Rounded { get; set; }
    }

    public class PunchData
    {
        public String ExactDate { get; set; }
        public String RoundedDate { get; set; }
        public String PictureLoc { get; set; }
        public int  Bio { get; set; }
        public bool Manual { get; set; }
        public int  Exception { get; set; }
        public int  Type { get; set; }
    }
}
