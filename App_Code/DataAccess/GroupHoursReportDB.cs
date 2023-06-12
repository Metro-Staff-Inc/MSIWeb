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
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    /*
    public class EmployeeWorkSummaryGroupComparer : IComparer
    {
        
        public int Compare(object o1, object o2)
        {
            EmployeeWorkSummaryGroup summary1 = (EmployeeWorkSummaryGroup)o1;
            EmployeeWorkSummaryGroup summary2 = (EmployeeWorkSummaryGroup)o2;

            return summary1.CheckInDateTime.CompareTo(summary2.CheckInDateTime);

        }
    }
    public class EmployeeWorkSummaryGroup
    {
        public DateTime CheckInDateTime = new DateTime(1900, 1, 1);
        public decimal TotalHours = 0M;
        public int EmployeeHistoryRefIdx = 0;
    }
    */
    public class GroupHoursReportDB
    {
        public GroupHoursReportDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();
        private decimal _minShiftBreakHours = 5;
        private bool UseExactTimes = false;

        public bool removeEmployeeHistory(EmployeeHistory employeeHistory, string userId)
        {
            bool remove = false;
            if (employeeHistory == null)
                return false;
            if (employeeHistory.ClientID == 258 &&
                (userId.Equals("CORTEZS") &&
                (((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 5)) ||
                (userId.Equals("KRETTP") &&
                (((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 4)) ||
                (userId.Equals("HERNANDEZJU") &&
                (((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 7)) ||
                (userId.Equals("VELEZD") &&
                (((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 4)) ||
                (userId.Equals("RODRIGUEZR") &&
                (((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 7)) ||
                (userId.Equals("PALOMOC") &&
                ( ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 6)) ||
                (userId.Equals("CUADRADOETEMP") &&
                ((
                    ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 1 &&
                    ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 2 &&
                    ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 3 &&
                    ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 5 &&
                    ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId != 6
                ) ||
                (
                    !((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName.Contains("100") &&
                    !((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName.Contains("400")
                )
                ))
                )
                remove = true;
            return remove;
        }


        public HoursReport GetGroupHoursReport(HoursReport hoursReport, /*IPrincipal userPrincipal*/string userId, string badgeNum, bool sortByDept)
        {
            HoursReport returnInfo = hoursReport;
            EmployeeHistory employeeHistory = null;
            EmployeeWorkSummary workSummary = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            int currentShift = 0;
            UseExactTimes = hoursReport.UseExactTimes;
            DateTime currentDate;
            DateTime punchExact;
            DateTime punchRound;
            int clientID = -1;
            int savedShift = 0;
            DateTime savedDate = new DateTime(1, 1, 1);
            DateTime savedRoundedDate = new DateTime(1, 1, 1);
            DateTime shiftStart = new DateTime(1, 1, 1);
            DateTime shiftEnd = new DateTime(1, 1, 1);
            bool processedCheckIn = false;
            bool newWorkSummary = false;
            double validShiftHours = 0;
            Hashtable employeeTotalHours = new Hashtable();
            Hashtable employeesMultiDept = new Hashtable();
            DateTime weekEndingDate = hoursReport.EndDateTime;
            DateTime weekEndingDate2 = weekEndingDate;
            int histIdx = 0;

            bool hideRecord = false;
            if (userId.ToLower().Equals("petersd") ||
                userId.ToLower().Equals("lopezm") ||
                userId.ToLower().Equals("jaimesj") ||
                userId.ToLower().Equals("grovern") ||
                userId.ToLower().Equals("lopezw") ||
                userId.ToLower().Equals("gutierrezg") ||
                userId.ToLower().Equals("mackeyp") ||
                userId.ToLower().Equals("houstonj") ||
                userId.ToLower().Equals("weldond") ||
                userId.ToLower().Equals("williamp") ||
                userId.ToLower().Equals("wesemanne") ||
                userId.ToLower().Equals("filarskir")
                )
                hideRecord = true;

            //tutco provides paid 20 min break when hours 
            //worked are less than 6 but greater than 5.
            if (hoursReport.ClientID == 186)
            {
                _minShiftBreakHours = 6;
            }
            /* break deducted if hours worked are 6 or more at Weber Huntley */
            if (hoursReport.ClientID == 92 || hoursReport.ClientID == 181 || hoursReport.ClientID == 340)
            {
                _minShiftBreakHours = 6;
            }
            /* break after 10 hours for Chicago American */
            if (hoursReport.ClientID == 273)
            {
                _minShiftBreakHours = 10;
            }

            //cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHoursReport);
            //cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHoursReportSummary);
            if (badgeNum.Length == 0)
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetGroupHoursReport);
            else
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetIndividualsHoursReportSummaryByUser);
                dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, badgeNum);
            }
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@sortByDept", DbType.Boolean, sortByDept);
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, hoursReport.ClientID);
            dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, hoursReport.RosterEmployeeFlag);
            //hoursReport.StartDateTime = new DateTime(2007, 11, 15, 0, 0, 0);
            //hoursReport.EndDateTime = new DateTime(2007, 12, 31, 0, 0, 0);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, hoursReport.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, hoursReport.EndDateTime.AddDays(1));
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userId);
            weekEndingDate = weekEndingDate.AddSeconds(0);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, weekEndingDate.ToString("MM/dd/yyyy"));
            dbSvc.AddOutParameter(cw, "@clientHoursApprovalId", DbType.Int32, 4);
            dbSvc.AddOutParameter(cw, "@approvalDateTime", DbType.DateTime, 8);
            dbSvc.AddOutParameter(cw, "@approvalUserName", DbType.String, 100);

            try
            {

                IDataReader dr = dbSvc.ExecuteReader(cw);
                
                TimeSpan dif;
                EmployeeWorkSummary prevSummary = null;
                DayOfWeek daySummaryHoursApplyTo = DayOfWeek.Sunday; //JHM from Monday (does initializing do anything?)
                try
                {
                    Boolean finished = false;
                    while (!finished)
                    {
                        finished = !(dr.Read());
                        Boolean newDept = true;
                        Boolean newShift = false;
                        Boolean newClient = false;
                        if (finished || (prevSummary != null && prevSummary.DepartmentInfo.DepartmentID != dr.GetInt32(dr.GetOrdinal("punch_department_id"))))
                            newDept = true;
                        else newDept = false;
                        if (finished == false && prevSummary != null && prevSummary.ShiftTypeInfo.ShiftTypeId != dr.GetInt32(dr.GetOrdinal("shift_type")))
                            newShift = true;
                        else
                            newShift = false;
                        if (finished || prevSummary != null && dr.GetInt32(dr.GetOrdinal("client_id")) != prevSummary.ClientID)
                            newClient = true;
                        if (finished || (employeeHistory == null || (employeeHistory.EmployeeID != dr.GetInt32(dr.GetOrdinal("employee_id")) || newDept || newShift || newClient)))
                        {
                            //check if the previous employee had work summaries
                            if ((employeeHistory != null && employeeHistory.WorkSummaries.Count == 0) ||
                                (hideRecord && employeeHistory != null && employeeHistory.EmployeeID == 102405) /* don't show Reda's hours */
                               || removeEmployeeHistory(employeeHistory, userId.ToUpper()))
                            {
                                returnInfo.EmployeeHistoryCollection.Remove(employeeHistory);
                                //employee was removed so decrement the index
                                histIdx--;
                            }

                            else if (employeeHistory != null && employeeHistory.WorkSummaries.Count > 0)
                            {
                                //check if there are any missed swipes
                                DateTime punchIn = new DateTime(1, 1, 1);
                                DateTime punchOut = new DateTime(1, 1, 1);
                                DateTime roundedPunchOut = new DateTime(1, 1, 1);
                                decimal summaryHours = 0M;
                                TimeSpan difference;

                                foreach (EmployeeWorkSummary summary in employeeHistory.WorkSummaries)
                                {
                                    if (!summary.CheckInApproved || !summary.CheckOutApproved)
                                    {
                                        employeeHistory.HasUnapprovedHours = true;
                                    }

                                    if (summary.CheckOutDateTime == null || summary.CheckOutDateTime.Date == new DateTime(1, 1, 1).Date)
                                    {
                                        if (summary.CheckInDateTime.Date != _helper.GetCSTCurrentDateTime().Date)
                                        {
                                            //indicate that there is a missing punch
                                            employeeHistory.HasInvalidWorkSummaries = true;
                                        }
                                    }
                                    else
                                    {
                                        punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, 0);
                                        punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, summary.RoundedCheckOutDateTime.Minute, 0);
                                        daySummaryHoursApplyTo = punchIn.DayOfWeek;

                                        daySummaryHoursApplyTo = summary.SummaryShiftStartDateTime.DayOfWeek;

                                        //sum up the hours
                                        difference = punchOut - punchIn;
                                        if (hoursReport.UseExactTimes == false)
                                            summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                        else
                                            summaryHours = Convert.ToDecimal(difference.TotalMinutes / 60.0);
                                        //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);

                                        switch (daySummaryHoursApplyTo)
                                        {
                                            case DayOfWeek.Monday:
                                                if (employeeHistory.MondaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.MondaySummary.FirstPunch = punchIn;
                                                employeeHistory.MondaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.MondaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.MondaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.MondaySummary.NumberOfPunches += 2;
                                                employeeHistory.MondaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Tuesday:
                                                if (employeeHistory.TuesdaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.TuesdaySummary.FirstPunch = punchIn;
                                                employeeHistory.TuesdaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    //difference = punchOut - employeeHistory.TuesdaySummary.FirstPunch;
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.TuesdaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.TuesdaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.TuesdaySummary.NumberOfPunches += 2;
                                                employeeHistory.TuesdaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Wednesday:
                                                if (employeeHistory.WednesdaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.WednesdaySummary.FirstPunch = punchIn;
                                                employeeHistory.WednesdaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    //difference = punchOut - employeeHistory.WednesdaySummary.FirstPunch;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.WednesdaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.WednesdaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.WednesdaySummary.NumberOfPunches += 2;
                                                employeeHistory.WednesdaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Thursday:
                                                if (employeeHistory.ThursdaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.ThursdaySummary.FirstPunch = punchIn;
                                                employeeHistory.ThursdaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    //difference = punchOut - employeeHistory.ThursdaySummary.FirstPunch;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.ThursdaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.ThursdaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.ThursdaySummary.NumberOfPunches += 2;
                                                employeeHistory.ThursdaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Friday:
                                                if (employeeHistory.FridaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.FridaySummary.FirstPunch = punchIn;
                                                employeeHistory.FridaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    //difference = punchOut - employeeHistory.FridaySummary.FirstPunch;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.FridaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.FridaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.FridaySummary.NumberOfPunches += 2;
                                                employeeHistory.FridaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Saturday:
                                                if (employeeHistory.SaturdaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.SaturdaySummary.FirstPunch = punchIn;
                                                employeeHistory.SaturdaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    //difference = punchOut - employeeHistory.SaturdaySummary.FirstPunch;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.SaturdaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.SaturdaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.SaturdaySummary.NumberOfPunches += 2;
                                                employeeHistory.SaturdaySummary.WorkSummaries.Add(summary);
                                                break;
                                            case DayOfWeek.Sunday:
                                                if (employeeHistory.SundaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                    employeeHistory.SundaySummary.FirstPunch = punchIn;
                                                employeeHistory.SundaySummary.LastPunch = punchOut;
                                                if (employeeHistory.ClientID == 245)
                                                {
                                                    difference = summary.RoundedCheckOutDateTime - summary.SummaryShiftStartDateTime;
                                                    //difference = punchOut - employeeHistory.SundaySummary.FirstPunch;
                                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                                    employeeHistory.SundaySummary.TotalHoursWorked = summaryHours;
                                                }
                                                else
                                                    employeeHistory.SundaySummary.TotalHoursWorked += summaryHours;
                                                employeeHistory.SundaySummary.NumberOfPunches += 2;
                                                employeeHistory.SundaySummary.WorkSummaries.Add(summary);
                                                break;
                                        }
                                    }
                                    summary.ClientID = clientID;
                                    prevSummary = summary;
                                }
                                //get the total hours
                                calculateTotalHours(employeeHistory, employeeTotalHours, employeesMultiDept);
                            }
                            if (!finished)
                            {
                                //a work summary = same shift same date
                                employeeHistory = new EmployeeHistory();
                                employeeHistory.ItemIdx = histIdx;
                                histIdx++;
                                //employeeHistory.ClientID = hoursReport.ClientID;
                                employeeHistory.ClientID = dr.GetInt32(dr.GetOrdinal("client_id"));
                                employeeHistory.FirstName = dr.GetString(dr.GetOrdinal("first_name")).ToUpper();
                                employeeHistory.LastName = dr.GetString(dr.GetOrdinal("last_name")).ToUpper();
                                employeeHistory.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                employeeHistory.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));

                                employeeHistory.OverrideBreakTime = dr.GetDecimal(dr.GetOrdinal("override_break_time"));

                                int bonusLoc = dr.GetOrdinal("bonus");
                                var t = dr.GetValue(bonusLoc);
                                employeeHistory.Bonus = Convert.ToDouble(t);

                                savedShift = 0;
                                returnInfo.EmployeeHistoryCollection.Add(employeeHistory);
                                prevSummary = null;
                            }
                        }

                        if (!finished)
                        {
                            /* this is the exact and rounded punch times */
                            punchExact = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                            punchRound = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            clientID = dr.GetInt32(dr.GetOrdinal("client_id"));

                            currentShift = dr.GetInt32(dr.GetOrdinal("shift_type"));

                            //JHMif (hoursReport.UseExactTimes == false)
                            //    currentDate = punchRound;
                            //else
                            currentDate = punchExact;

                            if (currentShift == savedShift)
                            {
                                if (processedCheckIn)
                                {
                                    //if (hoursReport.ClientID == 2 || hoursReport.ClientID == 207)
                                    //{
                                    //    workSummary.CheckOutDateTime = punchExact;
                                    //    workSummary.RoundedCheckOutDateTime = punchRound;
                                    //J    workSummary.CheckOutEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                    //H    workSummary.CheckOutApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                    //M    //force a new summary
                                    //    savedShift = 0;
                                    //    newWorkSummary = false;
                                    //}
                                    //else
                                    //{
                                    //check if the time is before the next check in for the shift
                                    //if after the check in for the next day then this is a new summary
                                    //if less than the check in for the next day then check out.
                                    if (currentDate < shiftStart.AddDays(1))
                                    {
                                        dif = currentDate - savedDate;

                                        if (dif.TotalHours > validShiftHours && hoursReport.ClientID != 168)
                                        {
                                            newWorkSummary = true;
                                        }
                                        else
                                        {
                                            workSummary.ClientID = clientID;
                                            workSummary.CheckOutDateTime = currentDate;
                                            if (hoursReport.UseExactTimes == false)
                                                workSummary.RoundedCheckOutDateTime = punchRound;
                                            else
                                                workSummary.RoundedCheckOutDateTime = punchExact;

                                            workSummary.CheckOutApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                            workSummary.CheckOutEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                            // if (hoursReport.ClientID == 113 || hoursReport.ClientID == 178 && hoursReport.UseExactTimes == false) //JHM added creative to art. schumann
                                            // {
                                            //     //round the time
                                            //     workSummary.RoundedCheckOutDateTime = _helper.GetRoundedPunchTime(workSummary.CheckOutDateTime);
                                            //     workSummary.CheckOutDateTime = workSummary.RoundedCheckOutDateTime;
                                            // }
                                            //force a new summary
                                            savedShift = 0;
                                            newWorkSummary = false;
                                        }
                                    }
                                    else
                                    {
                                        newWorkSummary = true;
                                    }
                                    //}
                                }
                            }
                            else
                            {
                                newWorkSummary = true;
                            }

                            if (newWorkSummary)
                            {
                                //new work summary.
                                //check that the punch applies for this period

                                bool sumVal = false;
                                if (weekEndingDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodSaturday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchExact /*currentDate*/, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift, employeeHistory.WorkSummaries.Count);
                                }
                                else
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchExact /*currentDate*/, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift);
                                }

                                //if (_helper.NewSummaryAppliesToBillingPeriodSaturday(//dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate))
                                if (sumVal)
                                {
                                    if (employeeHistory.GetEmployeePayRate() == 0M)
                                    {
                                        employeeHistory.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                                        employeeHistory.DefaultPayRate = dr.GetDecimal(dr.GetOrdinal("default_pay_rate"));
                                        employeeHistory.MinimumWage = dr.GetDecimal(dr.GetOrdinal("minimum_wage"));
                                        employeeHistory.ClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                                    }

                                    if (employeeHistory.GetEmployeeJobCode() == string.Empty)
                                    {
                                        employeeHistory.DefaultJobCode = dr.GetString(dr.GetOrdinal("def_job_code"));
                                        if (dr.GetString(dr.GetOrdinal("client_job_code")) != "0")
                                            employeeHistory.JobCode = dr.GetString(dr.GetOrdinal("client_job_code"));
                                        if (dr.GetString(dr.GetOrdinal("employee_job_code_override")) != "0")
                                            employeeHistory.JobCode = dr.GetString(dr.GetOrdinal("employee_job_code_override"));
                                        employeeHistory.ClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                                    }

                                    //new work summary
                                    workSummary = new EmployeeWorkSummary();
                                    workSummary.ShiftTypeInfo.ShiftTypeId = currentShift;
                                    workSummary.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                                    workSummary.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("punch_department_name"));
                                    workSummary.CheckInDateTime = punchExact; //was currentDate...

                                    //jhmif (hoursReport.UseExactTimes == false)
                                    //    workSummary.RoundedCheckInDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    //else

                                    workSummary.RoundedCheckInDateTime = punchRound;// was part of if block above...  = currentDate;
                                    workSummary.CheckInEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                    workSummary.ShiftInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                                    //if (hoursReport.ClientID == 113 || hoursReport.ClientID == 178 && hoursReport.UseExactTimes == false) // added creative werks to art.schumann
                                    //{
                                    //round the time
                                    //    workSummary.RoundedCheckInDateTime = _helper.GetRoundedPunchTime(workSummary.CheckInDateTime);
                                    //    workSummary.CheckInDateTime = workSummary.RoundedCheckInDateTime;
                                    //}

                                    workSummary.CheckInApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                    processedCheckIn = true;
                                    employeeHistory.WorkSummaries.Add(workSummary);
                                    savedDate = punchExact;// currentDate;
                                    savedShift = currentShift;
                                    //savedRoundedDate = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    savedRoundedDate = punchRound;
                                    /* check if we need to set net shiftStart and shiftEnd DateTimes. */
                                    //if( employeeHistory.e
                                    shiftStart = DateTime.Parse(savedRoundedDate.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_start_time")));
                                    shiftEnd = DateTime.Parse(savedRoundedDate.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_end_time")));

                                    if (shiftEnd < shiftStart)
                                    {
                                        ///*
                                        int revCount = 0;
                                        int forCount = 0;
                                        DateTime d2 = workSummary.CheckInDateTime;
                                        while (d2 > shiftStart.AddDays(-1) && revCount < 24)
                                        {
                                            d2 = d2.AddHours(-1);
                                            revCount++;
                                        }
                                        d2 = workSummary.CheckInDateTime;
                                        while (d2 < shiftStart && forCount < 24)
                                        {
                                            d2 = d2.AddHours(1);
                                            forCount++;
                                        }
                                        if (revCount > forCount)
                                            shiftEnd = shiftEnd.AddDays(1);
                                        else
                                            shiftStart = shiftStart.AddDays(-1);
                                        //*/
                                        //shiftEnd = shiftEnd.AddDays(1);
                                    }
                                    workSummary.SummaryShiftStartDateTime = shiftStart;
                                    workSummary.SummaryShiftEndDateTime = shiftEnd;

                                    if (employeeHistory.ClientID == 275 && newWorkSummary && prevSummary != null)
                                    {
                                        if (workSummary.CheckInDateTime < prevSummary.CheckOutDateTime.AddHours(3)) //is this a check in from lunch?
                                        {
                                            if (workSummary.CheckInDateTime >= prevSummary.CheckOutDateTime.AddMinutes(40))
                                                workSummary.RoundedCheckInDateTime = _helper.GetRoundedPunchTime(workSummary.CheckInDateTime.AddMinutes(-10));
                                            else if (workSummary.CheckInDateTime >= prevSummary.CheckOutDateTime.AddMinutes(30))
                                                workSummary.RoundedCheckInDateTime = _helper.GetRoundedPunchTime(prevSummary.CheckOutDateTime.AddMinutes(30));
                                        }
                                    }

                                    if ((employeeHistory.ClientID == 1589 /*JHM*/) && prevSummary != null)
                                    {
                                        if (workSummary.CheckInDateTime < prevSummary.SummaryShiftEndDateTime)
                                        {
                                            workSummary.SummaryShiftStartDateTime = prevSummary.SummaryShiftStartDateTime;
                                            workSummary.SummaryShiftEndDateTime = prevSummary.SummaryShiftEndDateTime;
                                        }
                                    }
                                    if (employeeHistory.ClientID == 245) /* Willy Wonka */
                                    {
                                        if (prevSummary != null)
                                        {
                                            if (workSummary.CheckInDateTime < prevSummary.SummaryShiftEndDateTime && workSummary.DepartmentInfo.DepartmentID == prevSummary.DepartmentInfo.DepartmentID)
                                            {
                                                workSummary.SummaryShiftStartDateTime = prevSummary.SummaryShiftStartDateTime;
                                                workSummary.SummaryShiftEndDateTime = prevSummary.SummaryShiftEndDateTime;
                                            }
                                        }
                                        if (prevSummary == null || workSummary.SummaryShiftStartDateTime != prevSummary.SummaryShiftStartDateTime) //new shift
                                        {
                                            workSummary.SummaryShiftStartDateTime = workSummary.RoundedCheckInDateTime;
                                        }
                                    }

                                    validShiftHours = _helper.GetValidShiftHours(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")));
                                    workSummary.ClientID = clientID;
                                    prevSummary = workSummary;
                                }
                            }
                        }
                    }
                    processMultiDepartments(employeesMultiDept, hoursReport);
                    if (employeesMultiDept.Count > 0)
                        returnInfo.MultiDepts = true;
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                        //get the approval id if there is one
                        returnInfo.ClientApprovalId = int.Parse(cw.Parameters["@clientHoursApprovalId"].Value.ToString());
                        if (returnInfo.ClientApprovalId > 0)
                        {
                            returnInfo.IsApproved = true;
                            returnInfo.ApprovalDateTime = DateTime.Parse(cw.Parameters["@approvalDateTime"].Value.ToString());
                            returnInfo.ApprovalUserName = cw.Parameters["@approvalUserName"].Value.ToString();
                        }
                    }
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return returnInfo;
        }
        public bool ApprovePunchRange(HoursReport hoursReport, /*IPrincipal userPrincipal*/string userId)
        {
            bool retVal = false;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ApprovePunchRange);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@XML_punch_data", DbType.String, hoursReport.ApprovalXML);
            dbSvc.AddInParameter(cw, "@approved_by", DbType.String, userId);
            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                retVal = true;
            }
            catch (Exception ex)
            {
                
                retVal = false;
            }
            finally
            {
                cw.Dispose();
            }

            return retVal;
        }
        public bool ApproveClientHours(HoursReport hoursReport, /*IPrincipal userPrincipal*/string userId)
        {
            DateTime weekEndingDate = hoursReport.EndDateTime;
            bool retVal = false;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ApproveClientHours);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@hoursXML", DbType.String, hoursReport.ApprovalXML);
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, hoursReport.ClientID);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, weekEndingDate.ToString("MM/dd/yyyy"));
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userId);

            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                retVal = true;
            }
            catch (Exception ex)
            {
                
                retVal = false;
            }
            finally
            {
                cw.Dispose();
            }

            return retVal;
        }

        private void calculateTotalHours(EmployeeHistory employeeHistory, Hashtable employeeHours, Hashtable employeesMultiDept)
        {
            employeeHistory.WorkSummaries.Sort(new EmployeeWorkSummarySortByCheckIn());

            EmployeeWorkSummary summary = null;

            bool calculateBreak = false;
            bool existingEmp = false;
            ArrayList employeeInfo = new ArrayList();
            decimal[] weeklyHours = new decimal[] { 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M };

            string strippedNumber = employeeHistory.TempNumber.Substring(2);

            employeeInfo = (ArrayList)employeeHours[strippedNumber];

            if (employeeInfo == null)
            {
                employeeInfo = new ArrayList();
                employeeInfo.Add(weeklyHours);
                //employeeInfo.Add(employeeHistory.FridaySummary);
                existingEmp = false;
            }
            else
            {
                weeklyHours = (decimal[])employeeInfo[0];
                existingEmp = true;
            }
            // add clientID here to exempt the client from having breaks automatically deducted
            if (employeeHistory.ClientID != 2 && employeeHistory.ClientID != 121 && employeeHistory.ClientID != 122
                && employeeHistory.ClientID != 205 && employeeHistory.ClientID != 207 && employeeHistory.ClientID != 1589 /*jhm*/
                && employeeHistory.ClientID != 243 && employeeHistory.ClientID != 245 && employeeHistory.ClientID != 113
                && employeeHistory.ClientID != 178 && employeeHistory.ClientID != 256 && employeeHistory.ClientID != 166 
                && employeeHistory.ClientID != 292 && employeeHistory.ClientID != 293 && employeeHistory.ClientID != 280
                && employeeHistory.ClientID != 281)
            {
                calculateBreak = true;
            }
            //calculate the break time for each day
            //Monday
            if (employeeHistory.MondaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Print Mailing Solutions */
            if (employeeHistory.MondayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }
            calculateBreakTime(employeeHistory.MondaySummary, calculateBreak, weeklyHours[0] + employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[0] += employeeHistory.MondaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.MondaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.MondaySummary.TotalHoursWorked;

            //Tuesday
            if (employeeHistory.TuesdaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.TuesdaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Print Mailing Solutions */
            if (employeeHistory.TuesdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;

            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.TuesdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.TuesdaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }

            calculateBreakTime(employeeHistory.TuesdaySummary, calculateBreak, weeklyHours[1] + employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[1] += employeeHistory.TuesdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.TuesdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.TuesdaySummary.TotalHoursWorked;

            //Wednesday
            /* Print Mailing Solutions */
            if (employeeHistory.WednesdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            if (employeeHistory.WednesdaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.WednesdaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.WednesdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.WednesdaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }

            calculateBreakTime(employeeHistory.WednesdaySummary, calculateBreak, weeklyHours[2] + employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[2] += employeeHistory.WednesdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.WednesdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.WednesdaySummary.TotalHoursWorked;

            //Thursday
            /* Print Mailing Solutions */
            if (employeeHistory.ThursdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            if (employeeHistory.ThursdaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.ThursdaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.ThursdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.ThursdaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }

            calculateBreakTime(employeeHistory.ThursdaySummary, calculateBreak, weeklyHours[3] + employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[3] += employeeHistory.ThursdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.ThursdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.ThursdaySummary.TotalHoursWorked;

            //Friday
            /* Print Mailing Solutions */
            if (employeeHistory.FridayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            if (employeeHistory.FridaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.FridaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.FridaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.FridaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }

            calculateBreakTime(employeeHistory.FridaySummary, calculateBreak, weeklyHours[4] + employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[4] += employeeHistory.FridaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.FridaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.FridaySummary.TotalHoursWorked;

            //Saturday
            /* Print Mailing Solutions */
            if (employeeHistory.SaturdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            if (employeeHistory.SaturdaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.SaturdaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.SaturdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.SaturdaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }
            if (employeeHistory.ClientID == 251) /* no break for Platinum Converting */
            {
                TimeSpan t = new TimeSpan(12, 0, 0);
                if (employeeHistory.SaturdaySummary.TotalHoursWorked >= 12)
                    calculateBreak = true;
                else
                    calculateBreak = false;
            }
            calculateBreakTime(employeeHistory.SaturdaySummary, calculateBreak, weeklyHours[5] + employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[5] += employeeHistory.SaturdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.SaturdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.SaturdaySummary.TotalHoursWorked;

            //Sunday
            /* Print Mailing Solutions */
            if (employeeHistory.SundayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            if (employeeHistory.SundaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.SundaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            /* Tangent */
            if (employeeHistory.ClientID == 258)
            {
                if (employeeHistory.SundaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.SundaySummary.WorkSummaries[0];
                    if (summary.ShiftTypeInfo.ShiftTypeId >= 4 || summary.ShiftInfo.ShiftID == 966)
                        calculateBreak = false;
                    else
                        calculateBreak = true;
                    if (summary.DepartmentInfo.DepartmentID == 377) /* receptionist */
                        employeeHistory.OverrideBreakTime = 1.0M;
                }
            }
            if (employeeHistory.ClientID == 251) /* no break for Platinum Converting */
            {
                TimeSpan t = new TimeSpan(12, 0, 0);
                if (employeeHistory.SaturdaySummary.TotalHoursWorked >= 12)
                    calculateBreak = true;
                else
                    calculateBreak = false;
            }

            calculateBreakTime(employeeHistory.SundaySummary, calculateBreak, weeklyHours[6] + employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.ClientID);
            weeklyHours[6] += employeeHistory.SundaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.SundaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.SundaySummary.TotalHoursWorked;

            //update the ot hours
            if (employeeHistory.TotalHours > 40)
            {
                employeeHistory.TotalOTHours = employeeHistory.TotalHours - 40;
                employeeHistory.TotalRegularHours = 40;
            }
            else
            {
                employeeHistory.TotalOTHours = 0;
                employeeHistory.TotalRegularHours = employeeHistory.TotalHours;
            }

            //update the weekly hours for the employee
            employeeInfo[0] = weeklyHours;
            //add the employee information for the department
            employeeInfo.Add(employeeHistory);
            if (existingEmp)
            {
                //check if the record already exists in the list
                ArrayList listCheck = (ArrayList)employeesMultiDept[strippedNumber];
                if (listCheck == null)
                {
                    //employees in the multi dept list
                    //need to be processed later for OT.
                    employeesMultiDept.Add(strippedNumber, employeeInfo);
                }
            }
            else
            {
                //never been processed so add them to the hours-hash
                employeeHours.Add(strippedNumber, employeeInfo);
            }
        }

        private void processMultiDepartments(Hashtable employeesMultiDept, HoursReport hoursReport)
        {
            //each item in the list is a different employee
            //cycle through each employee and see if the employee
            //has incurred ot.  If so determine which department(s)
            //get(s) the OT.  We need to recalculate total regular and totalot
            //if there is OT across departments.
            //employeeInfo has 0 element being total hours
            //items 1-n being each department employee worked in
            EmployeeHistory empHist = new EmployeeHistory();
            EmployeeWorkSummaryGroup workSummaryGroup = new EmployeeWorkSummaryGroup();
            ArrayList workSummaryGroups = new ArrayList();
            decimal hoursTotal = 0M;
            decimal otHours = 0M;
            decimal[] weeklyHours = { 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M };
            EmployeeHistory originalEmpHist = new EmployeeHistory();
            foreach (ArrayList employeeInfo in employeesMultiDept.Values)
            {
                /* get totalHoursAllDepts */
                //int totalHoursAllDepts = 0;
                for (int i = 1; i < employeeInfo.Count; i++)
                {
                    for (int j = 1; j < employeeInfo.Count; j++)
                    {
                        ((EmployeeHistory)employeeInfo[i]).TotalHoursAllDepts += ((EmployeeHistory)employeeInfo[j]).TotalHours;
                    }
                }
                //check if the employee has over 40 hours otherwise
                //it doesn't need to go through this logic
                weeklyHours = (decimal[])employeeInfo[0];
                if (weeklyHours[7] + weeklyHours[8] > 40)
                {
                    //order the work summaries by punch in date
                    workSummaryGroups = new ArrayList();
                    empHist = new EmployeeHistory();
                    for (int empIdx = 1; empIdx < employeeInfo.Count; empIdx++)
                    {
                        empHist = (EmployeeHistory)employeeInfo[empIdx];
                        //Monday hours
                        if (empHist.MondaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.MondaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.MondaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Tuesday hours
                        if (empHist.TuesdaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.TuesdaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.TuesdaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Wednesday hours
                        if (empHist.WednesdaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.WednesdaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.WednesdaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Thursday hours
                        if (empHist.ThursdaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.ThursdaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.ThursdaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Friday hours
                        if (empHist.FridaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.FridaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.FridaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Saturday hours
                        if (empHist.SaturdaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.SaturdaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.SaturdaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                        //Sunday hours
                        if (empHist.SundaySummary.WorkSummaries.Count > 0)
                        {
                            workSummaryGroup = new EmployeeWorkSummaryGroup();
                            workSummaryGroup.CheckInDateTime = ((EmployeeWorkSummary)empHist.SundaySummary.WorkSummaries[0]).CheckInDateTime;
                            workSummaryGroup.TotalHours = empHist.SundaySummary.TotalHoursWorked;
                            workSummaryGroup.EmployeeHistoryRefIdx = empHist.ItemIdx;
                            workSummaryGroups.Add(workSummaryGroup);
                        }
                    }
                    //sort the summary groups by check in date
                    workSummaryGroups.Sort(new EmployeeWorkSummaryGroupComparer());
                    hoursTotal = 0M;
                    otHours = 0M;
                    Hashtable processedEmps = new Hashtable();
                    object processedCheck = null;
                    foreach (EmployeeWorkSummaryGroup group in workSummaryGroups)
                    {
                        hoursTotal += group.TotalHours;

                        if (hoursTotal > 40)
                        {
                            originalEmpHist = (EmployeeHistory)hoursReport.EmployeeHistoryCollection[group.EmployeeHistoryRefIdx];
                            //check if the record needs to have the ot reset
                            processedCheck = processedEmps[group.EmployeeHistoryRefIdx];
                            if (processedCheck == null)
                            {
                                originalEmpHist.TotalRegularHours += originalEmpHist.TotalOTHours;
                                originalEmpHist.TotalOTHours = 0;
                                processedEmps.Add(group.EmployeeHistoryRefIdx, true);
                            }

                            //we are in OT so modify the original record accordingly
                            otHours = hoursTotal - 40;
                            originalEmpHist.TotalRegularHours -= otHours;
                            originalEmpHist.TotalOTHours += otHours;
                            hoursTotal = 40;
                        }
                    }
                }
            }
        }

        private void calculateTotalHours(EmployeeHistory employeeHistory)
        {
            bool calculateBreak = false;

            if (employeeHistory.ClientID != 2 && employeeHistory.ClientID != 121 && employeeHistory.ClientID != 122
                && employeeHistory.ClientID != 205 && employeeHistory.ClientID != 207 && employeeHistory.ClientID != 178
                    && employeeHistory.ClientID != 256)
            {
                calculateBreak = true;
            }

            //calculate the break time for each day
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm no break calc for creative works */
                calculateBreak = false;

            calculateBreakTime(employeeHistory.MondaySummary, calculateBreak, employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.MondaySummary.TotalHoursWorked;

            //Tuesday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm no break calc for creative works */
                calculateBreak = false;

            calculateBreakTime(employeeHistory.TuesdaySummary, calculateBreak, employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.TuesdaySummary.TotalHoursWorked;

            //Wednesday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm no break calc for creative works */
                calculateBreak = false;
            calculateBreakTime(employeeHistory.WednesdaySummary, calculateBreak, employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.WednesdaySummary.TotalHoursWorked;

            //Thursday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm no break calc for creative works */
                calculateBreak = false;
            calculateBreakTime(employeeHistory.ThursdaySummary, calculateBreak, employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.ThursdaySummary.TotalHoursWorked;

            //Friday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm no break calc for creative works */
                calculateBreak = false;
            calculateBreakTime(employeeHistory.FridaySummary, calculateBreak, employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.FridaySummary.TotalHoursWorked;

            //Saturday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 251 || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm - no break calculated on weekends for Platinum Converting or Creative Werks */
                calculateBreak = false;
            calculateBreakTime(employeeHistory.SaturdaySummary, calculateBreak, employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.SaturdaySummary.TotalHoursWorked;

            //Sunday
            if ((employeeHistory.ClientID == 258 && employeeHistory.AIdentNumber == "57630") || employeeHistory.ClientID == 251 || employeeHistory.ClientID == 178 || employeeHistory.ClientID == 256) /* jhm - no break calculated on weekends for Platinum Converting or Creative Werks */
                calculateBreak = false;
            calculateBreakTime(employeeHistory.SundaySummary, calculateBreak, employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.ClientID);
            employeeHistory.TotalHours += employeeHistory.SundaySummary.TotalHoursWorked;

            if (employeeHistory.TotalHours > 40)
            {
                employeeHistory.TotalOTHours = employeeHistory.TotalHours - 40;
                employeeHistory.TotalRegularHours = 40;
            }
            else
            {
                employeeHistory.TotalOTHours = 0;
                employeeHistory.TotalRegularHours = employeeHistory.TotalHours;
            }
        }

        private void calculateBreakTime(DailySummary summary, bool calculateBreak, decimal totalDailyHours, decimal overrideBreakHours, decimal currentDailyHours, int clientId)
        {
            double defaultBreakMin = 30;
            decimal defaultBreakHours = .5M;

            if (calculateBreak && summary.BreakApplied == false)
            {
                if (overrideBreakHours >= 0)
                {  //database default is 30 and .5 -JHM
                    defaultBreakMin = (double)overrideBreakHours * 60;
                    defaultBreakHours = overrideBreakHours;
                }

                if (summary.NumberOfPunches == 2 && totalDailyHours > _minShiftBreakHours && (totalDailyHours - currentDailyHours) <= _minShiftBreakHours)
                {
                    //if there is only a check in and check out then use the default
                    //break time
                    if (currentDailyHours >= 1)
                    {
                        summary.UseDefaultBreakTime = true;
                        summary.TotalHoursWorked -= defaultBreakHours;
                        summary.BreakApplied = true;
                    }
                }
                else if (summary.NumberOfPunches > 2)
                {
                    EmployeeWorkSummary breakInSummary = null;
                    EmployeeWorkSummary breakOutSummary = null;
                    TimeSpan breakTime;
                    double totalBreakTime = 0;
                    for (int idx = 0; idx < summary.WorkSummaries.Count; idx++)
                    {
                        if (idx < summary.WorkSummaries.Count - 1)
                        {
                            breakInSummary = (EmployeeWorkSummary)summary.WorkSummaries[idx];
                            breakOutSummary = (EmployeeWorkSummary)summary.WorkSummaries[idx + 1];

                            breakTime = breakOutSummary.RoundedCheckInDateTime - breakInSummary.RoundedCheckOutDateTime;
                            totalBreakTime += breakTime.TotalMinutes;
                        }
                    }

                    if (totalBreakTime > defaultBreakMin)
                    {
                        summary.UseDefaultBreakTime = false;
                    }
                    else if (totalBreakTime == defaultBreakMin)
                    {
                        summary.UseDefaultBreakTime = true;
                        //the break time is 30 minutes so we do not need to deduct
                        //summary.TotalHoursWorked -= .5M;
                    }
                    else
                    {
                        if (currentDailyHours >= 1)
                        {
                            //break time is less than 30 minutes so
                            //we must subtract the difference
                            summary.UseDefaultBreakTime = true;
                            summary.TotalHoursWorked -= Convert.ToDecimal((defaultBreakMin - totalBreakTime) / 60);
                            summary.TotalHoursWorked = Math.Round(summary.TotalHoursWorked, 2);
                        }
                    }
                }
            }

            string tempTotalHours = Convert.ToString(summary.TotalHoursWorked);
            string[] s = tempTotalHours.Split('.'); // the first numbers plus decimal point
            if (s.Length > 1)
            {
                if (s[1].Length >= 2)
                    s[1] = s[1].Substring(0, 2);

                int val = int.Parse(s[1]);
                if (s[1] == "00" || s[1] == "01" || s[1] == "02" || s[1] == "03" || s[1] == "04" || s[1] == "05" || s[1] == "06" || s[1] == "07" || s[1] == "08" || s[1] == "09" || s[1] == "10")
                //if( val <= 10 )
                {
                    if (UseExactTimes == false)
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0]);
                    else
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + "." + s[1]);
                }
                else if (val >= 45 && val <= 49)
                {
                    if (UseExactTimes == false)
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + ".50");
                    else
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + "." + val);

                }
                else if (val >= 92 && val <= 99)
                {
                    if (UseExactTimes == false)
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0]) + 1M;
                    else
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + "." + val);
                }
            }
        }
    }
}