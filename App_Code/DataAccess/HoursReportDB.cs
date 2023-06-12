using System;
using System.Data;
using System.Collections;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using System.Xml.Linq;
using System.Collections.Generic;
using MSIToolkit.Logging;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
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

    public class HoursReportDB
    {
        public HoursReportDB()
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
            if (employeeHistory == null)
                return false;
            bool remove = false;
            string id = userId.ToUpper();
            string deptName = ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).DepartmentInfo.DepartmentName.ToUpper();
            int shiftId = ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).ShiftTypeInfo.ShiftTypeId;

            if (employeeHistory.ClientID == 258 &&
                (id.Equals("CARMONAJ") && (employeeHistory.LocationId != 354 || shiftId != 5) ) ||
                (id.Equals("LASARTEE") && (shiftId != 6)) ||
                (id.Equals("RYANJ") && (shiftId != 2)) ||
                (id.Equals("KOESTERJ") && (shiftId != 3)) ||
                (id.Equals("LIBANM") && (shiftId != 2) && (shiftId != 3)) ||
                (id.Equals("MADSENE") && (shiftId != 2) && (shiftId != 3))  ||
                (id.Equals("IBARRAM") && (employeeHistory.LocationId != 354 || shiftId != 4)  ) ||
                (id.Equals("WILSOND") && (shiftId != 3)) ||
                (id.Equals("KRETTP") && (shiftId != 6) && (shiftId != 4)) ||
                (id.Equals("VILLALPANDOA") && (employeeHistory.LocationId != 354 || shiftId != 7)) ||
                (id.Equals("CRUZD") && (employeeHistory.LocationId != 355 || shiftId != 4)) ||
                (id.Equals("FRALEYT") && (shiftId != 2)) || 
                (id.Equals("MALDONADOE") && (shiftId != 7)) ||
                (id.Equals("CORTEZS") && (shiftId != 5)) ||
                (id.Equals("HAMILTONS") && (shiftId != 6)) ||
                //(id.Equals("SALAZARJ") && (shiftId != 1)) || 
                (id.Equals("WUNDERLICHB") && (false /*shiftId != 4*/)) ||
                (id.Equals("VELEZD") && (shiftId != 5)) ||
                (id.Equals("HERNANDEZJU") && (shiftId != 7)) || 
                (id.Equals("RODRIGUEZJ") && shiftId != 4) ||
                (id.Equals("JACKMAND") && (shiftId != 6 || !deptName.Contains("100"))) ||
                //(id.Equals("NAMECHEB") && !(deptName.Contains("100-GRINDER") || deptName.Contains("100-UTILITY"))  ) ||
                (id.Equals("HERNANDEZS") && !(deptName.Contains("QC") || deptName.Contains("QA"))) ||
                (id.Equals("NIEVAG") && (shiftId != 7 || !deptName.Contains("100"))) ||
                (id.Equals("GALVANE") && (!deptName.Contains("100-SHIPPING"))) ||
                (id.Equals("HOLMESR") && (!deptName.Contains("500"))) ||
                (id.Equals("BENTONC") && (!deptName.Equals("500-Receiving"))) ||
                (id.Equals("PALOMOC") && /*(
                    (!deptName.Contains("SKID") && !deptName.Contains("OPERATOR") && !deptName.Contains("100-PROJ"))
                ) && */ (employeeHistory.LocationId != 354 || shiftId != 6)) ||
                (id.Equals("NAMECHEB") && /*(
                    (!deptName.Contains("SKID") && !deptName.Contains("OPERATOR") && !deptName.Contains("100-PROJ"))
                ) && */ (employeeHistory.LocationId != 358)) || 
                (id.Equals("AZCONAF") && (employeeHistory.LocationId != 356) && (employeeHistory.LocationId != 357)) ||
                //(id.Equals("SANTILLANESO") && (
                //    (!deptName.Equals("Line Tool Team"))
                //)) ||
                (id.Equals("CUADRADOE") && 
                 (!deptName.Equals("100-GRINDER") &&
                 !deptName.Equals("100-SKID BUILDER") &&
                 !deptName.Equals("100-SHIPPING") &&
                 !deptName.Equals("100-UTILITY") &&
                 !deptName.Equals("300-FABRICATION") &&
                 !deptName.Contains("500-SKID BUILDER") )) 
                )
                remove = true;
            return remove;
        }
        public string MovePunchDeptShift(int punchId, int departmentId, int shiftType, String userName)
        {
            DbCommand cw;

            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.MovePunchDeptShift);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@punchId", DbType.Int32, punchId);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, departmentId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);
            dbSvc.AddInParameter(cw, "@userId", DbType.String, userName);
            String ret = "";
            try
            {
                int rec = dbSvc.ExecuteNonQuery(cw);
                ret = "Punch moved";
            }
            catch (Exception ex)
            {
                
                ret = ex.Message;
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }



        public string UpdateBonuses(XElement xmlTree, int numUpdates)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            String ret;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateBonuses);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@XML_data", DbType.Xml, xmlTree.ToString());
            //dbSvc.AddOutParameter(cw, "", DbType.Xml, xmlTree);
            try
            {
                int rec = dbSvc.ExecuteNonQuery(cw);
                ret = rec + " out of " + numUpdates + " bonuses updated.";
            }
            catch (Exception ex)
            {                
                ret = ex.Message;
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }

        //int rowCounter = 0;

        public HoursReport GetHoursReport(HoursReport hoursReport, string userId, string badgeNum, bool sortByDept, PerformanceLogger log = null)
        {
            if( log != null )log.Info("HoursReportDB.GetHoursReport", "Retrieving data from DB");
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
            int locationId;
            String created_by;
            DateTime created_dt;
            String last_updated_by;
            DateTime last_updated_dt;
            bool manual_override_flag;
            int savedShift = -1;
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
            decimal curPayRate = 0;
            bool payRateSet = false;

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
            else if (hoursReport.ClientID == 92 || hoursReport.ClientID == 181 || hoursReport.ClientID == 340)
            {
                _minShiftBreakHours = 6;
            }
            /* break after 10 hours for Chicago American */
            else if (hoursReport.ClientID == 273)
            {
                _minShiftBreakHours = 10;
            }
            /* compact */
            else if (hoursReport.ClientID == 299)
            {
                _minShiftBreakHours = 11.599M;
            }
            else if( hoursReport.ClientID == 226 && hoursReport.EndDateTime > new DateTime(2016, 02, 01) )
            {
                _minShiftBreakHours = 4;
            }
            else if( hoursReport.ClientID == 62 && hoursReport.EndDateTime > new DateTime(2017, 02, 17) )
            {
                _minShiftBreakHours = 4.5M;
            }
            //cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHoursReport);
            //cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHoursReportSummary);
            if( badgeNum.Length == 0 )
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHoursReportSummaryByUser);
            else
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetIndividualsHoursReportSummaryByUser);
                dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, badgeNum);
            }
            
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@sortByDept", DbType.Boolean, sortByDept);
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, hoursReport.ClientID);
            dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, hoursReport.RosterEmployeeFlag);
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
                //int counter = 0;
                if (log != null) log.Info("HoursReportDB.GetHoursReport", "ExecuteReader - Request to DB, " + hoursReport.ClientID + " - " + hoursReport.StartDateTime.ToShortDateString());
                IDataReader dr = dbSvc.ExecuteReader(cw);
                if (log != null) log.Info("HoursReportDB.GetHoursReport", "ExecuteReader - Return from DB");

                TimeSpan dif;
                EmployeeWorkSummary prevSummary = null;
                DayOfWeek daySummaryHoursApplyTo = DayOfWeek.Sunday; //JHM from Monday (does initializing do anything?)
                TimeSpan daysWorked = new TimeSpan(hoursReport.DaysWorked, 0, 0, 0);
                try
                {
                    Boolean finished = false;  
                    while (!finished)
                    {
                        finished = !(dr.Read());
                        DateTime lastPunch = new DateTime(9999, 12, 1);
                        Boolean newLoc = false;
                        Boolean newDept = false;
                        Boolean newShift = false;
                        Boolean newPayRate = false;
                        if (hoursReport.clientPrefs.ShowLocationsHoursReport && (finished || (prevSummary != null && !prevSummary.locationName.Equals(dr.GetString(dr.GetOrdinal("location_name"))))))
                            newLoc = true;
                        if (finished || (prevSummary != null && prevSummary.DepartmentInfo.DepartmentID != dr.GetInt32(dr.GetOrdinal("punch_department_id"))))
                            newDept = true;
                        if (finished == false && prevSummary != null && prevSummary.ShiftTypeInfo.ShiftTypeId != dr.GetInt32(dr.GetOrdinal("moved_shift_type")))
                            newShift = true;
                        if (finished || (curPayRate != dr.GetDecimal(dr.GetOrdinal("pay_rate")) &&
                                            employeeHistory != null &&
                                            employeeHistory.TempNumber.Equals(dr.GetString(dr.GetOrdinal("temp_number"))) &&
                                            newDept == false && newShift == false &&
                                            processedCheckIn == false && newLoc == false))
                            newPayRate = true;
                        if (finished || (employeeHistory == null || (employeeHistory.EmployeeID != dr.GetInt32(dr.GetOrdinal("employee_id"))
                            || newDept || newShift || newPayRate || newLoc)))
                        {
                            //check if the previous employee had work summaries
                            if ((employeeHistory != null && employeeHistory.WorkSummaries.Count == 0 && hoursReport.ShowAllEmployees == false) ||
                                (hideRecord && employeeHistory != null && employeeHistory.EmployeeID == 102405) /* don't show Reda's hours */
                               || removeEmployeeHistory(employeeHistory, userId.ToUpper()) || (employeeHistory != null && (hoursReport.EndDateTime - employeeHistory.FirstPunch) > daysWorked))
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
                                DateTime punchInExact = new DateTime(1, 1, 1);
                                DateTime punchOutExact = new DateTime(1, 1, 1);
                                DateTime punchInRound = new DateTime(1, 1, 1);
                                DateTime punchOutRound = new DateTime(1, 1, 1);
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
                                        if ((employeeHistory.ClientID < 325 || employeeHistory.ClientID > 327) && employeeHistory.ClientID != 302 && employeeHistory.ClientID != 381)
                                        {
                                            punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, summary.RoundedCheckInDateTime.Second);
                                            punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, summary.RoundedCheckOutDateTime.Minute, summary.RoundedCheckOutDateTime.Second);
                                        }
                                        else if( employeeHistory.ClientID == 381 )
                                        {
                                            if (summary.CheckInDateTime > summary.SummaryShiftStartDateTime)
                                                punchIn = new DateTime(summary.CheckInDateTime.Year, summary.CheckInDateTime.Month, summary.CheckInDateTime.Day, summary.CheckInDateTime.Hour, summary.CheckInDateTime.Minute, summary.CheckInDateTime.Second);
                                            else
                                                punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, summary.RoundedCheckInDateTime.Second);

                                            int clockOutMinutes = summary.RoundedCheckOutDateTime.Minute;
                                            int clockOutSeconds = summary.RoundedCheckInDateTime.Second;
                                            if (clockOutMinutes <= 15)
                                            {
                                                clockOutMinutes = 0;
                                                clockOutSeconds = 0;
                                            }
                                            else if (clockOutMinutes > 30 && clockOutMinutes <= 45)
                                            {
                                                clockOutMinutes = 30;
                                                clockOutSeconds = 0;
                                            }
                                            punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, clockOutMinutes, clockOutSeconds);
                                        }
                                        else if( employeeHistory.ClientID == 302  && hoursReport.clientPrefs.ExactPunchInOut  )
                                        {
                                            punchIn = new DateTime(summary.CheckInDateTime.Year, summary.CheckInDateTime.Month, summary.CheckInDateTime.Day, summary.CheckInDateTime.Hour, summary.CheckInDateTime.Minute, summary.CheckInDateTime.Second);
                                            punchOut = new DateTime(summary.CheckOutDateTime.Year, summary.CheckOutDateTime.Month, summary.CheckOutDateTime.Day, summary.CheckOutDateTime.Hour, summary.CheckOutDateTime.Minute, summary.CheckOutDateTime.Second);
                                        }
                                        else
                                        {
                                            if (!summary.CheckInManualOverrideFlag)
                                                punchIn = new DateTime(summary.CheckInDateTime.Year, summary.CheckInDateTime.Month, summary.CheckInDateTime.Day, summary.CheckInDateTime.Hour, summary.CheckInDateTime.Minute, summary.CheckInDateTime.Second);
                                            else
                                                punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, summary.RoundedCheckInDateTime.Second);

                                            if (!summary.CheckOutManualOverrideFlag)
                                                punchOut = new DateTime(summary.CheckOutDateTime.Year, summary.CheckOutDateTime.Month, summary.CheckOutDateTime.Day, summary.CheckOutDateTime.Hour, summary.CheckOutDateTime.Minute, summary.CheckOutDateTime.Second);
                                            else
                                                punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, summary.RoundedCheckOutDateTime.Minute, summary.RoundedCheckOutDateTime.Second);
                                        }
                                        daySummaryHoursApplyTo = punchIn.DayOfWeek;

                                        daySummaryHoursApplyTo = summary.SummaryShiftStartDateTime.DayOfWeek;
                                        if (summary.ShiftTypeInfo.ShiftTypeId == 2 || summary.ShiftTypeInfo.ShiftTypeId == 3
                                            && punchIn.Hour < 6)
                                            daySummaryHoursApplyTo--;
                                        if (daySummaryHoursApplyTo < 0) daySummaryHoursApplyTo += 7;
                                        //summary.DaySummaryAppliesTo = daySummaryHoursApplyTo;

                                        //sum up the hours
                                        difference = punchOut - punchIn;
                                        if (punchIn.IsDaylightSavingTime() != punchOut.IsDaylightSavingTime() && employeeHistory.ClientID != 299)
                                        {
                                            if (punchOut.IsDaylightSavingTime())
                                            {
                                                TimeSpan ts = new TimeSpan(1, 0, 0);
                                                difference = difference.Subtract(ts);
                                            }
                                            else
                                            {
                                                TimeSpan ts = new TimeSpan(1, 0, 0);
                                                difference = difference.Add(ts);
                                            }
                                        }

                                        if (hoursReport.UseExactTimes == false /*&& employeeHistory.ClientID != 302*/)
                                            summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                        else
                                            summaryHours = Convert.ToDecimal(difference.TotalMinutes / 60.0);
                                        //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);

                                        switch (summary.DaySummaryAppliesTo/*daySummaryHoursApplyTo*/)
                                        {
                                            case DayOfWeek.Monday:
                                                if (employeeHistory.MondaySummary.FirstPunch.Equals(DailySummary.DATE_NOT_SET))
                                                {
                                                    employeeHistory.MondaySummary.FirstPunch = punchIn;
                                                    //employeeHistory.MondaySummary.dstFirst
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.MondaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.TuesdaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.TuesdaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.WednesdaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.WednesdaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.ThursdaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.ThursdaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.FridaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.FridaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.SaturdaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.SaturdaySummary.LastPunch);
                                                }
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
                                                {
                                                    employeeHistory.SundaySummary.FirstPunch = punchIn;
                                                    summary.MinutesLate = (/*summary.CheckInDateTime*/punchIn - summary.SummaryShiftStartDateTime);
                                                    if (summary.MinutesLate.TotalSeconds > 0)
                                                    {
                                                        employeeHistory.HasLatePunches = true;
                                                    }
                                                }
                                                else
                                                {
                                                    summary.BreakAmount = (summary.CheckInDateTime - employeeHistory.SundaySummary.LastPunch);
                                                }
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
                                payRateSet = false;
                                histIdx++;
                                employeeHistory.ClientID = hoursReport.ClientID;
                                employeeHistory.FirstName = dr.GetString(dr.GetOrdinal("first_name")).ToUpper().Trim();
                                employeeHistory.LastName = dr.GetString(dr.GetOrdinal("last_name")).ToUpper().Trim();
                                employeeHistory.Supervisor = dr.GetString(dr.GetOrdinal("supervisor")).ToUpper().Trim();
                                employeeHistory.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                employeeHistory.ClientEmployeeId = dr.GetString(dr.GetOrdinal("client_number"));
                                //log.Info("Name", employeeHistory.LastName + ", " + employeeHistory.EmployeeID);
                                employeeHistory.FirstPunch = dr.GetDateTime(dr.GetOrdinal("first_punch"));
                                employeeHistory.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                                employeeHistory.Modify = dr.GetBoolean(dr.GetOrdinal("modify"));
                                employeeHistory.CostCenter = dr.GetInt32(dr.GetOrdinal("cost_center"));
                                
                                if (employeeHistory.ClientID == 2)
                                {
                                    DateTime start = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                                    DateTime end = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                                    if (DateTime.Compare(start.AddDays(21), end) < 0)
                                    {
                                        employeeHistory.FirstName += " (REG)";
                                    }
                                }
                                else if (employeeHistory.ClientID == 302)
                                {
                                    if (employeeHistory.LastName.IndexOf('(') >= 0)
                                    {
                                        employeeHistory.LastName = employeeHistory.LastName.Substring(0, employeeHistory.LastName.IndexOf('('));
                                    }
                                    if (employeeHistory.FirstName.IndexOf('(') >= 0)
                                    {
                                        employeeHistory.FirstName = employeeHistory.FirstName.Substring(0, employeeHistory.FirstName.IndexOf('('));
                                    }
                                }

                                employeeHistory.OverrideBreakTime = dr.GetDecimal(dr.GetOrdinal("override_break_time"));

                                int bonusLoc = dr.GetOrdinal("bonus");
                                var t = dr.GetValue(bonusLoc);
                                employeeHistory.Bonus = Convert.ToDouble(t);

                                savedShift = -1;
                                //if( employeeHistory.ItemIdx % 4 == 0 )
                                //{
                                returnInfo.EmployeeHistoryCollection.Add(employeeHistory);
                                //}
                                prevSummary = null;
                            }
                        }

                        if (!finished)
                        {
                            /* this is the exact and rounded punch times */
                            punchExact = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                            punchRound = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            PunchLocation punchLocation = new PunchLocation(dr.GetDouble(dr.GetOrdinal("latitude")), dr.GetDouble(dr.GetOrdinal("longitude")));
                            locationId = dr.GetInt32(dr.GetOrdinal("location_id"));
                            employeeHistory.LocationId = locationId;
                            employeeHistory.LocationName = dr.GetString(dr.GetOrdinal("location_name"));
                            String approvedBy = dr.GetString(dr.GetOrdinal("approved_by"));
                            if (approvedBy.Length == 0)
                            {
                                approvedBy = "Unapproved";
                            }
                            created_by = dr.GetString(dr.GetOrdinal("created_by"));
                            created_dt = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                            last_updated_by = dr.GetString(dr.GetOrdinal("last_updated_by"));
                            last_updated_dt = dr.GetDateTime(dr.GetOrdinal("last_updated_dt"));
                            manual_override_flag = dr.GetBoolean(dr.GetOrdinal("manual_override_flag"));
                            bool noMissedPunches = dr.GetBoolean(dr.GetOrdinal("no_missing_punches"));

                            currentShift = dr.GetInt32(dr.GetOrdinal("moved_shift_type"));
                            currentDate = punchRound;
                            /* set cutoff date for last punch */
                            if (weekEndingDate > new DateTime(2021, 5, 17))
                            {
                                string shiftEndTemp = dr.GetString(dr.GetOrdinal("shift_end_time"));
                                string shiftStartTemp = dr.GetString(dr.GetOrdinal("shift_start_time"));
                                lastPunch = DateTime.Parse(weekEndingDate.ToString("MM/dd/yyyy " + shiftEndTemp));
                                if (shiftStartTemp.CompareTo(shiftEndTemp) > 0) lastPunch = lastPunch.AddHours(24);
                            }

                            if (currentShift == savedShift)
                            {
                                if (processedCheckIn)
                                {
                                    if (currentDate < workSummary.SummaryShiftStartDateTime.AddDays(1) 
                                        || noMissedPunches)
                                    {
                                        dif = currentDate - savedRoundedDate;

                                        if (dif.TotalHours > validShiftHours && hoursReport.ClientID != 168 && !noMissedPunches)
                                        {
                                            newWorkSummary = true;
                                        }
                                        else
                                        {
                                            workSummary.CheckOutLastUpdatedBy = last_updated_by;
                                            workSummary.CheckOutLastUpdatedDate = last_updated_dt;
                                            workSummary.CheckOutDateTime = punchExact;
                                            workSummary.CheckOutCreatedBy = created_by;
                                            workSummary.CheckOutCreatedDt = created_dt;
                                            workSummary.CheckOutLocation = punchLocation;
                                            workSummary.CheckOutManualOverrideFlag = manual_override_flag;
                                            if (hoursReport.UseExactTimes == false || (employeeHistory.ClientID == 381) || (employeeHistory.ClientID >= 325 && employeeHistory.ClientID <= 327))
                                                workSummary.RoundedCheckOutDateTime = punchRound;
                                            else
                                                workSummary.RoundedCheckOutDateTime = punchExact;
                                            if( !workSummary.approvedBy.Equals(approvedBy))
                                                workSummary.approvedBy += " / " + approvedBy;

                                            workSummary.CheckOutApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                            workSummary.CheckOutEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));

                                            //force a new summary
                                            savedShift = 0;
                                            processedCheckIn = false;
                                            newWorkSummary = false;
                                        }
                                    }
                                    else
                                    {
                                        newWorkSummary = true;
                                    }
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
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodSaturday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchRound /*currentDate*/, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift, employeeHistory.WorkSummaries.Count);
                                }
                                else if (weekEndingDate.DayOfWeek == DayOfWeek.Friday)
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodFriday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchRound /*currentDate*/, 
                                        hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift, employeeHistory.WorkSummaries.Count);
                                }
                                else if (weekEndingDate.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodTuesday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchRound /*currentDate*/, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift);
                                }
                                else
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchRound /*currentDate*/, hoursReport.StartDateTime, hoursReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, hoursReport.ClientID, currentShift);
                                }

                                sumVal = sumVal & punchRound <= lastPunch;
                                
                                if (sumVal)
                                {
                                    if (employeeHistory.GetEmployeePayRate() == 0M)
                                    {
                                        var pr = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                                        //var pr2 = dr.GetDouble(dr.GetOrdinal("pay_rate"));
                                        employeeHistory.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                                        employeeHistory.RosterPayRate = dr.GetDecimal(dr.GetOrdinal("roster_pay_rate"));
                                        if (hoursReport.clientPrefs.RosterBasedPayRates)
                                        {
                                            employeeHistory.PayRate = employeeHistory.RosterPayRate;
                                        }
                                        curPayRate = employeeHistory.PayRate;
                                        employeeHistory.DefaultPayRate = dr.GetDecimal(dr.GetOrdinal("default_pay_rate"));
                                        employeeHistory.MinimumWage = dr.GetDecimal(dr.GetOrdinal("minimum_wage"));
                                        employeeHistory.ClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                                    }
                                    if (employeeHistory.GetEmployeeJobCode() == string.Empty)
                                    {
                                        employeeHistory.DefaultJobCode = dr.GetString(dr.GetOrdinal("def_job_code"));
                                        if (employeeHistory.DefaultJobCode == "NA" && employeeHistory.ClientID == 302)
                                            employeeHistory.DefaultJobCode = " ";
                                        if (dr.GetString(dr.GetOrdinal("client_job_code")) != "0")
                                            employeeHistory.JobCode = dr.GetString(dr.GetOrdinal("client_job_code"));
                                        if (dr.GetString(dr.GetOrdinal("employee_job_code_override")) != "0")
                                            employeeHistory.JobCode = dr.GetString(dr.GetOrdinal("employee_job_code_override"));
                                        employeeHistory.ClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                                    }

                                    //new work summary
                                    workSummary = new EmployeeWorkSummary();
                                    //workSummary.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                                    workSummary.Badge = employeeHistory.TempNumber;
                                    workSummary.ShiftTypeInfo.ShiftTypeId = currentShift;
                                    workSummary.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                                    workSummary.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("punch_department_name"));
                                    workSummary.locationName = dr.GetString(dr.GetOrdinal("location_name"));
                                    //counter++;
                                    int ccIdx = dr.GetOrdinal("cost_center");
                                    workSummary.CostCenter = dr.GetInt32(ccIdx);
                                    workSummary.CheckInDateTime = punchExact; //was currentDate...
                                    workSummary.approvedBy = approvedBy;
                                    workSummary.CheckInLastUpdatedBy = last_updated_by;
                                    workSummary.CheckInLastUpdatedDate = last_updated_dt;
                                    workSummary.CheckInCreatedBy = created_by;
                                    workSummary.CheckInCreatedDt = created_dt;
                                    workSummary.CheckInManualOverrideFlag = manual_override_flag;
                                    workSummary.CheckInLocation = punchLocation;

                                    //TEMPORARY PAY RATE CHANGE!!! 
                                    if (employeeHistory.ClientID == 302)
                                    {
                                        if(!payRateSet)
                                        {
                                            _helper.adjustJBSSPayRates(employeeHistory, workSummary);
                                            payRateSet = true;
                                        }
                                    }
                                    workSummary.RoundedCheckInDateTime = punchRound;// was part of if block above...  = currentDate;
                                    workSummary.CheckInEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                    workSummary.ShiftInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));

                                    workSummary.CheckInApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                    processedCheckIn = true;
                                    workSummary.BreakTime = (double)(employeeHistory.OverrideBreakTime);
                                    employeeHistory.WorkSummaries.Add(workSummary);
                                    savedDate = punchExact;// currentDate;
                                    savedShift = currentShift;
                                    savedRoundedDate = punchRound;

                                    String stStr = dr.GetString(dr.GetOrdinal("shift_start_time"));
                                    String endStr = dr.GetString(dr.GetOrdinal("shift_end_time"));
                                    int st = Convert.ToInt32(stStr.Substring(0, stStr.IndexOf(":")));


                                    /*
                                    //TimeSpan subDayShift = new TimeSpan(0, 0, 0);
                                    //Boolean prevDayPossible = false;

                                    if ( (employeeHistory.ClientID >= 325 && employeeHistory.ClientID <= 327) 
                                        && returnInfo.StartDateTime.Date > new DateTime(2016, 07, 22) &&
                                        st == 0 && (st - savedRoundedDate.Hour) % 24 <= 2)
                                    {
                                        TimeSpan ts = new TimeSpan(24, 0, 0);
                                        DateTime newSavedRoundedDate = savedRoundedDate.Add(ts);
                                        savedRoundedDate = DateTime.Parse(newSavedRoundedDate.ToString("MM/dd/yyyy ") + stStr);
                                        if (savedRoundedDate.Hour < 12)
                                        {
                                            subDayShift = new TimeSpan(2, 30, 0);
                                            prevDayPossible = true;
                                        }
                                    }
                                    */

                                    shiftStart = DateTime.Parse(savedRoundedDate.ToString("MM/dd/yyyy ") + stStr);
                                    shiftEnd = DateTime.Parse(savedRoundedDate.ToString("MM/dd/yyyy ") + endStr);
                                    //lastPunch = DateTime.Parse(weekEndingDate.ToString("MM/dd/yyyy ") + endStr);


                                    if (shiftEnd < shiftStart)
                                    {
                                        //lastPunch = lastPunch.AddHours(24);
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
                                        if (revCount > forCount && punchRound > shiftEnd)
                                            shiftEnd = shiftEnd.AddDays(1);
                                        else
                                            shiftStart = shiftStart.AddDays(-1);
                                    }

                                    workSummary.SummaryShiftStartDateTime = shiftStart;//.Subtract(subDayShift);
                                    workSummary.DaySummaryAppliesTo = workSummary.SummaryShiftStartDateTime.DayOfWeek;// shiftStart.DayOfWeek;
                                    workSummary.SummaryShiftEndDateTime = shiftEnd;

                                    /*
                                    if (prevDayPossible)
                                    {
                                        if (workSummary.SummaryShiftStartDateTime.Hour > 18 && workSummary.RoundedCheckInDateTime.Hour > 18)
                                        {
                                            workSummary.DaySummaryAppliesTo = workSummary.SummaryShiftStartDateTime.AddDays(.5).DayOfWeek;
                                        }
                                    }
                                    */

                                    if (employeeHistory.ClientID == 275 && newWorkSummary && prevSummary != null)
                                    {
                                        if (workSummary.CheckInDateTime < prevSummary.CheckOutDateTime.AddHours(3)) //is this a check in from lunch?
                                        {
                                            if (workSummary.CheckInDateTime >= prevSummary.CheckOutDateTime.AddMinutes(40) && workSummary.CheckInDateTime.Date != new DateTime(2015, 12, 18))
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
                                            //if (workSummary.SummaryShiftStartDateTime.Day > workSummary.RoundedCheckInDateTime.Day)
                                            workSummary.SummaryShiftStartDateTime = workSummary.RoundedCheckInDateTime;
                                            //else
                                            //  workSummary.SummaryShiftStartDateTime = workSummary.RoundedCheckInDateTime.AddDays(-1);
                                        }
                                    }

                                    validShiftHours = _helper.GetValidShiftHours(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")));

                                    prevSummary = workSummary;
                                }
                            }
                        }
                    }
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
                        if (log != null) log.Info("HoursReportDB.GetHoursReport", "ReaderClosed");
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
                    if (log != null) log.Info("HoursReportDB.GetHoursReport", "ReaderDisposed - Begin MultiDepartment Split");
                    if ((hoursReport.ClientID >= 325 && hoursReport.ClientID <= 327) || hoursReport.ClientID == 299 ||
                        (hoursReport.ClientID == 113 && hoursReport.EndDateTime > new DateTime(2017, 01, 30)) ||
                        (hoursReport.ClientID == 302 && hoursReport.EndDateTime > new DateTime(2017, 02, 09)) ||
                        (hoursReport.ClientID == 324 && hoursReport.EndDateTime > new DateTime(2018, 12, 13)) ||
                        hoursReport.ClientID == 403 || hoursReport.ClientID == 412)
                    {
                        DateTime beginAryztaTime = new DateTime(2016, 2, 12);
                        foreach (EmployeeHistory eh in hoursReport.EmployeeHistoryCollection)
                        {
                            if (hoursReport.EndDateTime > beginAryztaTime)
                            {
                                setRoundedHours(eh);    // make sure rounded punch times = exact punch times, except for first punch-in and last punch-out
                                eh.MondaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.MondaySummary.LastPunch - eh.MondaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.TuesdaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.TuesdaySummary.LastPunch - eh.TuesdaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.WednesdaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.WednesdaySummary.LastPunch - eh.WednesdaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.ThursdaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.ThursdaySummary.LastPunch - eh.ThursdaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.FridaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.FridaySummary.LastPunch - eh.FridaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.SaturdaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.SaturdaySummary.LastPunch - eh.SaturdaySummary.FirstPunch).TotalMinutes) / 60.0);
                                eh.SundaySummary.TotalHoursWorked = Convert.ToDecimal(((eh.SundaySummary.LastPunch - eh.SundaySummary.FirstPunch).TotalMinutes) / 60.0);
                            }
                            ignoreBreakTime(eh.MondaySummary, eh.ClientID, hoursReport.EndDateTime);
                            ignoreBreakTime(eh.TuesdaySummary, eh.ClientID, hoursReport.EndDateTime);
                            ignoreBreakTime(eh.WednesdaySummary, eh.ClientID, hoursReport.EndDateTime);
                            ignoreBreakTime(eh.ThursdaySummary, eh.ClientID, hoursReport.EndDateTime);
                            ignoreBreakTime(eh.FridaySummary, eh.ClientID, hoursReport.EndDateTime);
                            ignoreBreakTime(eh.SaturdaySummary, eh.ClientID, hoursReport.EndDateTime);
                            //ignoreBreakTime(eh.SundaySummary, eh.ClientID, hoursReport.EndDateTime);

                            //if( eh.SaturdaySummary.dstFirst != eh.SaturdaySummary.dstLast )
                            //{
                            if (eh.SaturdaySummary.LastPunch.IsDaylightSavingTime() != eh.SaturdaySummary.FirstPunch.IsDaylightSavingTime()
                                && eh.ClientID != 299 )
                            {
                                int hoursAdd = 0;
                                foreach( Object obj in eh.SaturdaySummary.WorkSummaries )
                                {
                                    EmployeeWorkSummary ews = (EmployeeWorkSummary)obj;
                                    if (ews.CheckInDateTime.IsDaylightSavingTime() != ews.CheckOutDateTime.IsDaylightSavingTime() )
                                    {
                                        if( ews.CheckOutDateTime.IsDaylightSavingTime() )
                                        {
                                            hoursAdd = -1;
                                            break;
                                        }
                                        else
                                        {
                                            hoursAdd = 1;
                                            break;
                                        }
                                    }
                                }
                                eh.SaturdaySummary.TotalHoursWorked += hoursAdd;
                            }
                            ignoreBreakTime(eh.SundaySummary, eh.ClientID, hoursReport.EndDateTime);

                            eh.MondaySummary.TotalHoursWorked = Math.Round(eh.MondaySummary.TotalHoursWorked, 2);
                            eh.TuesdaySummary.TotalHoursWorked = Math.Round(eh.TuesdaySummary.TotalHoursWorked, 2);
                            eh.WednesdaySummary.TotalHoursWorked = Math.Round(eh.WednesdaySummary.TotalHoursWorked, 2);
                            eh.ThursdaySummary.TotalHoursWorked = Math.Round(eh.ThursdaySummary.TotalHoursWorked, 2);
                            eh.FridaySummary.TotalHoursWorked = Math.Round(eh.FridaySummary.TotalHoursWorked, 2);
                            eh.SaturdaySummary.TotalHoursWorked = Math.Round(eh.SaturdaySummary.TotalHoursWorked, 2);
                            eh.SundaySummary.TotalHoursWorked = Math.Round(eh.SundaySummary.TotalHoursWorked, 2);

                            eh.TotalHours = Convert.ToDecimal(eh.MondaySummary.TotalHoursWorked) + eh.TuesdaySummary.TotalHoursWorked +
                                eh.WednesdaySummary.TotalHoursWorked + eh.ThursdaySummary.TotalHoursWorked + eh.FridaySummary.TotalHoursWorked +
                                eh.SaturdaySummary.TotalHoursWorked + eh.SundaySummary.TotalHoursWorked;
                            if (eh.TotalHours > 40) 
                            {
                                eh.TotalRegularHours = 40;
                                eh.TotalOTHours = eh.TotalHours - 40;
                            }
                            else
                            {
                                eh.TotalRegularHours = eh.TotalHours;
                                eh.TotalOTHours = 0;
                            }
                        }
                    }

                    processMultiDepartments(employeesMultiDept, hoursReport);
                    if (employeesMultiDept.Count > 0)
                        returnInfo.MultiDepts = true;
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
            if (log != null) log.Info("HoursReportDB.GetHoursReport", "Finished GetHoursReport()");
            return returnInfo;
        }

        /* for each day's work summary, set the rounded hour = exact hour, unless it is first punch-in or last punch-out */
        public static void setRoundedHours(EmployeeHistory eh)
        {
            /* Monday */
            if (eh.MondaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.MondaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.MondaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.MondaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.MondaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.MondaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.MondaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Tuesday */
            if (eh.TuesdaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.TuesdaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.TuesdaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.TuesdaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.TuesdaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.TuesdaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.TuesdaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Wednesday */
            if( eh.WednesdaySummary.WorkSummaries.Count > 1 )
            {
                for (int i = 0; i < eh.WednesdaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.WednesdaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.WednesdaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.WednesdaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.WednesdaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.WednesdaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Thursday */
            if (eh.ThursdaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.ThursdaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.ThursdaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.ThursdaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.ThursdaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.ThursdaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.ThursdaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Friday */
            if (eh.FridaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.FridaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.FridaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.FridaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.FridaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.FridaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.FridaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Saturday */
            if (eh.SaturdaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.SaturdaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.SaturdaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.SaturdaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.SaturdaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.SaturdaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.SaturdaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
            /* Sunday */
            if (eh.SundaySummary.WorkSummaries.Count > 1)
            {
                for (int i = 0; i < eh.SundaySummary.WorkSummaries.Count; i++)
                {
                    if (i > 0) ((EmployeeWorkSummary)((DailySummary)(eh.SundaySummary)).WorkSummaries[i]).RoundedCheckInDateTime =
                                 ((EmployeeWorkSummary)((DailySummary)(eh.SundaySummary)).WorkSummaries[i]).CheckInDateTime;
                    if (i < eh.SundaySummary.WorkSummaries.Count - 1) ((EmployeeWorkSummary)((DailySummary)(eh.SundaySummary)).WorkSummaries[i]).RoundedCheckOutDateTime =
                                  ((EmployeeWorkSummary)((DailySummary)(eh.SundaySummary)).WorkSummaries[i]).CheckOutDateTime;
                }
            }
        }
        public static TimeSpan avg = new TimeSpan(0, 5, 0);
        protected static void ignoreBreakTime(DailySummary ds, int clientId, DateTime weekEnding)
        {
            if (ds.WorkSummaries.Count > 0)
            {
                ArrayList ws = ds.WorkSummaries;
                List<TimeSpan> intervalList = new List<TimeSpan>();
                TimeSpan ts = new TimeSpan(0, 0, 0);
                for (int i = 1; i < ds.WorkSummaries.Count; i++)
                {
                    TimeSpan interval = new TimeSpan();
                    DateTime punchOut;
                    DateTime punchIn;
                    if (((EmployeeWorkSummary)ws[i - 1]).CheckOutManualOverrideFlag)
                    {
                        punchOut = ((EmployeeWorkSummary)ws[i - 1]).RoundedCheckOutDateTime;
                    }
                    else
                    {
                        punchOut = ((EmployeeWorkSummary)ws[i - 1]).CheckOutDateTime;
                    }
                    if (((EmployeeWorkSummary)ws[i]).CheckInManualOverrideFlag)
                    {
                        punchIn = ((EmployeeWorkSummary)ws[i]).RoundedCheckInDateTime;
                    }
                    else
                    {
                        punchIn = ((EmployeeWorkSummary)ws[i]).CheckInDateTime;
                    }
                    interval = punchIn - punchOut;
                    if ( (clientId == 302 && ((EmployeeWorkSummary)ws[i]).SummaryShiftStartDateTime >= new DateTime(2023,2,27)) 
                        || true /*clientId != 302*/)
                    {
                        intervalList.Add(interval);
                        ts += interval;
                    }
                }
                ///* ts represents the total amount of break time
                ///* we ignore 40 minutes for 10 hours total or less, and 60 minutes for > 10 hours
                //
                TimeSpan thw;
                if( (clientId >= 325 && clientId <= 327 && weekEnding >= new DateTime(2017,2,26,3,30,0)) 
                    || (clientId >= 325 && clientId <= 327 && ds.TotalHoursWorked < 5) )
                {
                    thw = ((EmployeeWorkSummary)ws[ds.WorkSummaries.Count - 1]).CheckOutDateTime - ((EmployeeWorkSummary)ws[0]).CheckInDateTime;
                }
                else
                {
                    thw = ((EmployeeWorkSummary)ws[ds.WorkSummaries.Count - 1]).RoundedCheckOutDateTime - ((EmployeeWorkSummary)ws[0]).RoundedCheckInDateTime;
                }
                ds.TotalHoursWorked = (Decimal)(thw.TotalMinutes / 60.0);
                if( clientId == 412 && weekEnding >= new DateTime(2023,2,27) )
                {
                    ts = new TimeSpan(ts.Hours, ts.Minutes, 0);
                    if (ts.TotalMinutes < 30)
                        ts = new TimeSpan(0, 30, 0);
                    else if( ts.TotalMinutes >= 30 && ts.TotalMinutes <= 60 )
                    {
                        ts = new TimeSpan(0, 30, 0);
                    }
                    else
                    {
                        ts = ts.Add(new TimeSpan(0, -30, 0));
                    }
                }
                if (clientId == 299)
                {
                    ts = new TimeSpan(ts.Hours, ts.Minutes, 0);
                    if (ts.TotalMinutes > 45)
                        ts = ts.Add(new TimeSpan(0, -45, 0));
                    else
                        ts = new TimeSpan(0, 0, 0);
                }
                else if (clientId == 113)
                {
                    if (ts.TotalMinutes < 30 && ds.TotalHoursWorked > 5)
                    {
                        ts = new TimeSpan(0, 30, 0);
                    }
                }
                else if ((clientId == 324 || clientId == 329) &&
                    ((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 4, 1))
                {
                    if ((ds.TotalHoursWorked >= 5.6M)/*5.6M*/ ||
                        (ds.TotalHoursWorked >= 4.0M && ds.WorkSummaries.Count > 1))
                    {
                        ts = ts.Add(new TimeSpan(0, -30, 0));
                        if (ts.TotalMinutes < 30)
                        {
                            if (ds.TotalHoursWorked <= /*5.6M*/ 4.0M)
                            {
                                ts = new TimeSpan(0, 0, 0);
                            }
                            else
                            {
                                ts = new TimeSpan(0, 30, 0);
                            }
                        }
                    }
                }

                else if (clientId == 324 || clientId == 329)
                {

                    if( ((EmployeeWorkSummary)(ds.WorkSummaries[0])).DaySummaryAppliesTo == DayOfWeek.Sunday ||
                        ((EmployeeWorkSummary)(ds.WorkSummaries[0])).DaySummaryAppliesTo == DayOfWeek.Saturday ) 
                    {
                        ts = new TimeSpan(0, 0, 0);
                    }
                    else
                    {
                        if ((ds.TotalHoursWorked >= 5.6M)/*5.6M*/ ||
                            (ds.TotalHoursWorked >= 4.0M && ds.WorkSummaries.Count > 1))
                        {
                            ts = ts.Add(new TimeSpan(0, -30, 0));
                            if (ts.TotalMinutes < 30)
                            {
                                if (ds.TotalHoursWorked <= /*5.6M*/ 4.0M)
                                {
                                    ts = new TimeSpan(0, 0, 0);
                                }
                                else
                                {
                                    ts = new TimeSpan(0, 30, 0);
                                }
                            }
                        }
                    }
                }

                else if (clientId == 302  && 
                    ((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 3, 6) ) 
                {
                    /* 
                        if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 2, 27))
                        {
                            ts = ts.Add(new TimeSpan(0, -45, 0));
                        }
                        else
                        {
                            ts = ts.Add(new TimeSpan(0, -75, 0));
                        }
                    
                    if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2020, 7, 20))
                    {
                        if (ts.Minutes <= 30.5)
                        {
                            ts = new TimeSpan(0, 30, 0);
                        }
                    } */
                    //else 
                    if (ds.TotalHoursWorked >= (decimal)10.5)
                    {
                        //if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 2, 27))
                        //{
                            ts = ts.Add(new TimeSpan(0, -45, 0));
                        //}
                    }
                    else if (ds.TotalHoursWorked >= 8)
                    {
                        //if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 2, 27))
                        //{
                            ts = ts.Add(new TimeSpan(0, -30, 0));
                        //}
                    }
                    else if((ds.TotalHoursWorked > (decimal)5.5) && ts.TotalMinutes >= 0 && ts.TotalMinutes <= 30.5)
                    {
                        ts = new TimeSpan(0, 30, 0);    
                    }
                    else
                    {
                        //if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 2, 27))
                        //{
                            ts = new TimeSpan(0, 0, 0); 
                        //}
                    }
                    if ((ds.TotalHoursWorked > (decimal)5.5) /* && ts.TotalMinutes >= 0 */ && ts.TotalMinutes <= 30.5)
                    {
                        //if (((EmployeeWorkSummary)ws[0]).SummaryShiftStartDateTime < new DateTime(2023, 2, 27))
                        //{
                            ts = new TimeSpan(0, 30, 0);
                        //}
                    }
                    //else if (ts.TotalMinutes < 0)
                    //{
                    //    ts = new TimeSpan(0, 0, 0);
                    //}
                }
                
                else if (clientId == 302)
                {
                    if(ds.TotalHoursWorked >= (decimal)5 )
                    {
                        if (ds.TotalHoursWorked <= (decimal)10.25)
                        {
                            if( ws.Count >= 3 )
                            {
                                if( ts.TotalMinutes < 60 )
                                {
                                    ts = new TimeSpan(0, 60, 0);
                                }
                            }
                            else 
                            {
                                if( ts.TotalMinutes < 30 )
                                {
                                    ts = new TimeSpan(0, 30, 0);
                                }
                            }
                        }
                        else  //total hours >= 10
                        {
                            if( ts.TotalMinutes < 60 )
                            {
                                ts = new TimeSpan(0, 60, 0);
                            }
                        }
                    }
                }
                else if( clientId == 403 )
                {
                    if (ts.TotalMinutes < 30 && ds.TotalHoursWorked > 3)
                    {
                        ts = new TimeSpan(0, 30, 0);
                    }
                }
                else
                {
                    if (ds.TotalHoursWorked > 10)
                    {
                        ts = ts.Add(new TimeSpan(0, -60, 0));
                    }
                    else
                    {
                        ts = ts.Add(new TimeSpan(0, -40, 0));
                    }
                    if (ts.TotalMinutes < 30 )
                    {
                        if( ds.TotalHoursWorked < 5 )
                        {
                            ts = new TimeSpan(0, 0, 0);
                        }
                        else
                        {
                            ts = new TimeSpan(0, 30, 0);
                        }
                    }
                }
                ds.TotalHoursWorked -= (Decimal)((ts.TotalMinutes) / 60.0);
            }
        }

        public List<ShiftDepartment> GetShiftDepartments(int clientId)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            List<ShiftDepartment> sds = null;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetShiftDepartment);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                sds = new List<ShiftDepartment>();
                try
                {
                    ShiftDepartment sd = null;
                    while (dr.Read())
                    {
                        int shiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        if (sd == null || sd.Type != shiftType)
                        {
                            if( sd != null )
                                sds.Add(sd);
                            sd = new ShiftDepartment();
                            sd.Department = new List<Dept>();
                            sd.Type = shiftType;
                        }
                        //sd. = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        Dept d = new Dept();
                        d.Name = dr.GetString(dr.GetOrdinal("department_name"));
                        d.Id = dr.GetInt32(dr.GetOrdinal("department_id"));
                        sd.Department.Add(d);
                    }
                    if (sd != null)
                        sds.Add(sd);
                }
                catch (Exception drEx)
                {
                    
                    throw (drEx);
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();

                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                cw.Dispose();
            }
            return sds;
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
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, hoursReport.ClientID);
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
        public bool UnSubmitClientHours(HoursReport hoursReport, /*IPrincipal userPrincipal*/string userId)
        {
            DateTime weekEndingDate = hoursReport.EndDateTime;
            bool retVal = false;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UnSubmitClientHours);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, hoursReport.ClientID);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, weekEndingDate.ToString("MM/dd/yyyy"));
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userId);

            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                if( rows > 0 )
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

            bool fairrington7_5Hours = employeeHistory.ClientID == 297 && ((EmployeeWorkSummary)(employeeHistory.WorkSummaries[0])).SummaryShiftStartDateTime > new DateTime(2016, 08, 7, 0,0,0 );
            
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
            if (employeeHistory.ClientID != 2 /*&& employeeHistory.ClientID != 121*/ && employeeHistory.ClientID != 122
                && employeeHistory.ClientID != 205 && employeeHistory.ClientID != 207 && employeeHistory.ClientID != 1589 /*jhm*/
                && employeeHistory.ClientID != 243 && employeeHistory.ClientID != 245 && employeeHistory.ClientID != 113
                && employeeHistory.ClientID != 178 && employeeHistory.ClientID != 256 && employeeHistory.ClientID != 166 
                && employeeHistory.ClientID != 62)
            {
                calculateBreak = true;
            }
            //calculate the break time for each day
            //Monday
            var oldCalculateBreak = calculateBreak;
            if (employeeHistory.MondaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.MondaySummary.NumberOfPunches == 2 && employeeHistory.MondaySummary.TotalHoursWorked >= 5);
            /* fairrington */

            /* Print Mailing Solutions */
            if (employeeHistory.MondayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.MondayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.MondayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.MondayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Tuesday
            oldCalculateBreak = calculateBreak;
            if (employeeHistory.TuesdaySummary.WorkSummaries.Count > 0 && ((EmployeeWorkSummary)employeeHistory.TuesdaySummary.WorkSummaries[0]).ShiftInfo.ShiftID == 1144)
                calculateBreak = true;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.TuesdaySummary.NumberOfPunches == 2 && employeeHistory.TuesdaySummary.TotalHoursWorked >= 5);
            /* Print Mailing Solutions */
            if (employeeHistory.TuesdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.TuesdayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.TuesdayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.TuesdayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Wednesday
            /* fairrington */
            oldCalculateBreak = calculateBreak;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.WednesdaySummary.NumberOfPunches == 2 && employeeHistory.WednesdaySummary.TotalHoursWorked >= 5);
            /* Print Mailing Solutions */
            if (employeeHistory.WednesdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.WednesdayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.WednesdayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.WednesdayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Thursday
            oldCalculateBreak = calculateBreak;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.ThursdaySummary.NumberOfPunches == 2 && employeeHistory.ThursdaySummary.TotalHoursWorked >= 5);
            /* Print Mailing Solutions */
            if (employeeHistory.ThursdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.ThursdayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.ThursdayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.ThursdayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Friday
            calculateBreak = oldCalculateBreak;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.FridaySummary.NumberOfPunches == 2 && employeeHistory.FridaySummary.TotalHoursWorked >= 5);
            if (employeeHistory.ClientID == 347 && employeeHistory.FridaySummary.FirstPunch > new DateTime(2017, 10, 6))
                calculateBreak = false;
            /* Print Mailing Solutions */
            if (employeeHistory.FridayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.FridayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.FridayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.FridayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Saturday
            oldCalculateBreak = calculateBreak;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.SaturdaySummary.NumberOfPunches == 2 && employeeHistory.SaturdaySummary.TotalHoursWorked >= 5);
            /* World Marketing */
            if (employeeHistory.ClientID == 291 || 
                (employeeHistory.ClientID == 347 && employeeHistory.SaturdaySummary.FirstPunch > new DateTime(2017, 10, 6)) ||
                (employeeHistory.ClientID == 307 && employeeHistory.SaturdaySummary.FirstPunch > new DateTime(2017, 10, 16)))
                calculateBreak = false;
            /* Print Mailing Solutions */
            if (employeeHistory.SaturdayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.SaturdayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.SaturdayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.ClientID == 381)
                calculateBreak = false;
            else if (employeeHistory.SaturdayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //Sunday
            oldCalculateBreak = calculateBreak;
            employeeHistory.MissingBreakPunches = employeeHistory.MissingBreakPunches ||
                        (employeeHistory.SundaySummary.NumberOfPunches == 2 && employeeHistory.SundaySummary.TotalHoursWorked >= 5);
            /* World Marketing */
            if (employeeHistory.ClientID == 291)
                calculateBreak = false;
            /* Print Mailing Solutions */
            if (employeeHistory.SundayHours >= 7 && employeeHistory.ClientID == 166)
                calculateBreak = true;
            else if (employeeHistory.SundayHours >= 4.5M && employeeHistory.ClientID == 62)
                calculateBreak = true;
            else if (employeeHistory.ClientID == 166)
                calculateBreak = false;
            else if (employeeHistory.SundayHours < (Decimal)7.5 && fairrington7_5Hours)
                calculateBreak = false;
            else if (employeeHistory.ClientID == 381)
                calculateBreak = false;
            else if (employeeHistory.SundayHours <= 5.66M &&
                    (employeeHistory.ClientID == 324 ||
                        employeeHistory.ClientID == 329))
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
            calculateBreak = oldCalculateBreak;

            //update the ot hours
            int regHours = 40;
            if( employeeHistory.ClientID == 309)
            {
                DateTime endDate = new DateTime(2015, 9, 13);
                if (employeeHistory.FridaySummary.FirstPunch <= endDate && employeeHistory.FridaySummary.FirstPunch >= endDate.AddDays(-6))
                {
                    if (employeeHistory.TempNumber.Substring(2).Equals("91477") || employeeHistory.TempNumber.Substring(2).Equals("88586"))
                    {
                        regHours = 32;
                    }
                    else if (employeeHistory.TempNumber.Substring(2).Equals("102118"))
                    {
                        regHours = 16;
                    }
                    else if (employeeHistory.TempNumber.Substring(2).Equals("106607"))
                    {
                        regHours = 24;
                    }
                }
            }
            if (employeeHistory.TotalHours > regHours)
            {
                employeeHistory.TotalOTHours = employeeHistory.TotalHours - regHours;
                employeeHistory.TotalRegularHours = regHours;
            }
            else
            {
                employeeHistory.TotalOTHours = 0;
                employeeHistory.TotalRegularHours = employeeHistory.TotalHours;
            }

            if (employeeHistory.ClientID == 88)
            {

                if( ((EmployeeWorkSummary)employeeHistory.WorkSummaries[0]).DepartmentInfo.DepartmentID == 479 )
                {
                    decimal weekendHours = employeeHistory.SaturdayHours + employeeHistory.SundayHours;

                    if (weekendHours > employeeHistory.TotalOTHours)
                    {
                        decimal diff = weekendHours - employeeHistory.TotalOTHours;
                        if (diff > 0)
                        {
                            employeeHistory.TotalOTHours += diff;
                            employeeHistory.TotalRegularHours -= diff;
                        }
                    }
                }
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
            DateTime startFix = new DateTime(2018, 1, 1);
            DateTime endFix = new DateTime(2018, 3, 23);
            foreach (ArrayList employeeInfo in employeesMultiDept.Values)
            {
                /* get totalHoursAllDepts */
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
                Boolean dateBetween = hoursReport.EndDateTime >= startFix && hoursReport.EndDateTime <= endFix;
                if( ((weeklyHours[7] + weeklyHours[8] > 40) && dateBetween) ||
                    (((EmployeeHistory)employeeInfo[1]).TotalHoursAllDepts > 40 && !dateBetween) )
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
                    //if (false)
                    //{
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
                    //}
                }
            }
        }

        private void calculateTotalHours(EmployeeHistory employeeHistory)
        {
            bool calculateBreak = false;

            if (employeeHistory.ClientID != 2 /*&& employeeHistory.ClientID != 121*/ && employeeHistory.ClientID != 122
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

                if (summary.NumberOfPunches == 2 && totalDailyHours > _minShiftBreakHours && (totalDailyHours - currentDailyHours) <= _minShiftBreakHours )
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
                    if (totalDailyHours > 16 && clientId == 310)
                        defaultBreakMin *= 2;
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
                if( s[1].Length >= 2 )
                    s[1] = s[1].Substring(0, 2);

                int val = int.Parse(s[1]);
                if (s[1] == "00" || s[1] == "01" || s[1] == "02" || s[1] == "03" || s[1] == "04" || s[1] == "05" || s[1] == "06" || s[1] == "07" || s[1] == "08" || s[1] == "09" || s[1] == "10")
                {
                    if ( (UseExactTimes == false && clientId != 310) || 
                        (clientId == 310 && summary.LastPunch < new DateTime(2021, 3, 13)) )
                    {
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0]);
                    }
                    else
                    {
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + "." + s[1]);
                    }
                }
                else if (val >= 45 && val <= 49)
                {
                    if (UseExactTimes == false)
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + ".50");
                    else
                        summary.TotalHoursWorked = Convert.ToDecimal(s[0] + "." + val);

                }
                else if (val >= 92 && val <= 99 && clientId != 310)
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