using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Summary description for DataAccessHelper
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class DataAccessHelper
    {
        public DataAccessHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        internal ClientPreferences fillClientPreferences(IDataReader dr)
        {
            //add the calculate summary hours flag
            System.Diagnostics.Debug.WriteLine(dr.ToString());

            ClientPreferences retClientPref = new ClientPreferences(dr.GetInt32(dr.GetOrdinal("client_id")), dr.GetBoolean(dr.GetOrdinal("display_pay_rates")),
                dr.GetBoolean(dr.GetOrdinal("display_job_codes")), dr.GetBoolean(dr.GetOrdinal("display_submit_hours")),
                dr.GetBoolean(dr.GetOrdinal("display_invoice")), dr.GetBoolean(dr.GetOrdinal("approve_hours")),
                dr.GetBoolean(dr.GetOrdinal("display_schedule")), dr.GetBoolean(dr.GetOrdinal("enable_punch_reports")),
                dr.GetBoolean(dr.GetOrdinal("employee_history_exact_punch_times")),
                dr.GetBoolean(dr.GetOrdinal("ticket_tracking_exact_late_punches")),
                dr.GetBoolean(dr.GetOrdinal("display_weekly_reports_sunday_to_saturday")),
                dr.GetBoolean(dr.GetOrdinal("display_bonuses")), dr.GetBoolean(dr.GetOrdinal("display_pay_rate_maintenance")),
                dr.GetBoolean(dr.GetOrdinal("display_weekly_reports_wednesday_to_tuesday")),
                dr.GetBoolean(dr.GetOrdinal("display_temps")),
                dr.GetBoolean(dr.GetOrdinal("display_start_date")),
                dr.GetBoolean(dr.GetOrdinal("display_break_times")),
                dr.GetBoolean(dr.GetOrdinal("use_exact_times")),
                dr.GetBoolean(dr.GetOrdinal("roster_based_pay_rates")),
                dr.GetBoolean(dr.GetOrdinal("show_locations_hours_report")),
                dr.GetBoolean(dr.GetOrdinal("display_weekly_reports_saturday_to_friday")),
                dr.GetBoolean(dr.GetOrdinal("exact_punch_in_out")));
            return retClientPref;
        }

        internal Client fillClient(IDataReader dr)
        {
            //add the calculate summary hours flag
            System.Diagnostics.Debug.WriteLine(dr.ToString());
            Client retClient = new Client(dr.GetInt32(dr.GetOrdinal("client_id")), dr.GetString(dr.GetOrdinal("client_name")),
                dr.GetString(dr.GetOrdinal("emp_id_prefix")), dr.GetBoolean(dr.GetOrdinal("add_office_cd_employee")),
                dr.GetBoolean(dr.GetOrdinal("has_temp_numbers")), dr.GetBoolean(dr.GetOrdinal("maintains_employee_schedules")),
                dr.GetBoolean(dr.GetOrdinal("requires_lunch_in_out")), dr.GetBoolean(dr.GetOrdinal("calculate_summary_hours")));

            //retClient.Location.Add(dr.GetInt32(dr.GetOrdinal("location_id")), dr.GetString(dr.GetOrdinal("location_name")));

            //get the multipliers
            if (fieldExistsInReader("multiplier", dr))
            {
                decimal mult;
                bool multParse = decimal.TryParse(dr.GetValue(dr.GetOrdinal("multiplier")).ToString(), out mult);
                retClient.Multiplier = mult;
            }

            if (fieldExistsInReader("ot_multiplier", dr))
            {
                decimal otmult;
                bool multParse = decimal.TryParse(dr.GetValue(dr.GetOrdinal("ot_multiplier")).ToString(), out otmult);
                if (otmult > 0)
                {
                    retClient.OTMultiplier = otmult;
                }
                else
                {
                    retClient.OTMultiplier = retClient.Multiplier;
                }
            }
            return retClient;
        }

        internal Client fillClient(IDataReader dr, bool processPreferred)
        {
            Client retClient = fillClient(dr);

            if (processPreferred)
            {
                retClient.PreferredClient = dr.GetBoolean(dr.GetOrdinal("preferred_client"));
            }
            return retClient;
        }

        internal bool fieldExistsInReader(string fieldName, IDataReader dr)
        {
            bool retVal = false;
            for (int idx = 0; idx < dr.FieldCount; idx++)
            {
                if (dr.GetName(idx).ToUpper() == fieldName.ToUpper())
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }

        internal int getFieldOrdinalInReader(string fieldName, IDataReader dr)
        {
            int retOrdinal = -1;
            for (int idx = 0; idx < dr.FieldCount; idx++)
            {
                if (dr.GetName(idx).ToUpper() == fieldName.ToUpper())
                {
                    retOrdinal = idx;
                    break;
                }
            }
            //return dr.GetOrdinal ( fieldName );
            return retOrdinal;
        }

        internal HoursReport processHoursReport(IDataReader dr, HoursReport hoursReport)
        {
            HelperFunctions _helper = new HelperFunctions();
            HoursReport returnInfo = hoursReport;
            EmployeeHistory employeeHistory = null;
            EmployeeWorkSummary workSummary = null;
            int currentShift = 0;
            DateTime currentDate;
            int savedShift = 0;
            DateTime savedDate = new DateTime(1, 1, 1);
            DateTime shiftStart = new DateTime(1, 1, 1);
            DateTime shiftEnd = new DateTime(1, 1, 1);
            bool processedCheckIn = false;
            bool newWorkSummary = false;
            double validShiftHours = 0;

            TimeSpan dif;
            try
            {
                while (dr.Read())
                {
                    if (employeeHistory == null || employeeHistory.EmployeeID != dr.GetInt32(dr.GetOrdinal("employee_id")))
                    {
                        //check if the previous employee had work summaries
                        if (employeeHistory != null && employeeHistory.WorkSummaries.Count == 0)
                        {
                            returnInfo.EmployeeHistoryCollection.Remove(employeeHistory);
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

                                    //sum up the hours
                                    difference = punchOut - punchIn;
                                    if (punchIn.IsDaylightSavingTime() != punchOut.IsDaylightSavingTime())
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
                                    summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                    //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);

                                    switch (punchIn.DayOfWeek)
                                    {
                                        case DayOfWeek.Monday:
                                            employeeHistory.MondaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.MondaySummary.NumberOfPunches += 2;
                                            employeeHistory.MondaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Tuesday:
                                            employeeHistory.TuesdaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.TuesdaySummary.NumberOfPunches += 2;
                                            employeeHistory.TuesdaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Wednesday:
                                            employeeHistory.WednesdaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.WednesdaySummary.NumberOfPunches += 2;
                                            employeeHistory.WednesdaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Thursday:
                                            employeeHistory.ThursdaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.ThursdaySummary.NumberOfPunches += 2;
                                            employeeHistory.ThursdaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Friday:
                                            employeeHistory.FridaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.FridaySummary.NumberOfPunches += 2;
                                            employeeHistory.FridaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Saturday:
                                            employeeHistory.SaturdaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.SaturdaySummary.NumberOfPunches += 2;
                                            employeeHistory.SaturdaySummary.WorkSummaries.Add(summary);
                                            break;
                                        case DayOfWeek.Sunday:
                                            employeeHistory.SundaySummary.TotalHoursWorked += summaryHours;
                                            employeeHistory.SundaySummary.NumberOfPunches += 2;
                                            employeeHistory.SundaySummary.WorkSummaries.Add(summary);
                                            break;
                                    }
                                }
                            }
                            //get the total hours
                            calculateTotalHours(employeeHistory);
                        }

                        //a work summary = same shift same date
                        employeeHistory = new EmployeeHistory();
                        employeeHistory.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        employeeHistory.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        employeeHistory.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                        employeeHistory.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                        //employeeHistory.BillRate = dr.GetDouble(dr.GetOrdinal("bill_rate"));
                        savedShift = 0;
                        returnInfo.EmployeeHistoryCollection.Add(employeeHistory);
                    }

                    currentShift = dr.GetInt32(dr.GetOrdinal("shift_type"));
                    currentDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                    if (currentShift == savedShift)
                    {
                        if (processedCheckIn)
                        {

                            //check if the time is before the next check in for the shift
                            //if after the check in for the next day then this is a new summary
                            //if less than the check in for the next day then check out.
                            if (currentDate < shiftStart.AddDays(1))
                            {
                                dif = currentDate - savedDate;
                                if (dif.TotalHours >= validShiftHours)
                                {
                                    newWorkSummary = true;
                                }
                                else
                                {
                                    workSummary.CheckOutDateTime = currentDate;
                                    workSummary.RoundedCheckOutDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    //force a new summary
                                    savedShift = 0;
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
                        if (newSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, hoursReport.StartDateTime, hoursReport.EndDateTime))
                        {
                            //new work summary
                            workSummary = new EmployeeWorkSummary();
                            workSummary.ShiftTypeInfo.ShiftTypeId = currentShift;
                            workSummary.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                            workSummary.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("punch_department_name"));
                            workSummary.CheckInDateTime = currentDate;
                            workSummary.RoundedCheckInDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            processedCheckIn = true;
                            employeeHistory.WorkSummaries.Add(workSummary);
                            savedDate = currentDate;
                            savedShift = currentShift;
                            shiftStart = DateTime.Parse(savedDate.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_start_time")));
                            shiftEnd = DateTime.Parse(savedDate.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_end_time")));
                            if (shiftEnd < shiftStart)
                            {
                                shiftEnd = shiftEnd.AddDays(1);
                            }

                            validShiftHours = _helper.GetValidShiftHours(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")));
                        }
                    }
                }

                //check if the previous employee had work summaries
                if (employeeHistory != null && employeeHistory.WorkSummaries.Count == 0)
                {
                    returnInfo.EmployeeHistoryCollection.Remove(employeeHistory);
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

                            //sum up the hours
                            difference = punchOut - punchIn;
                            if (punchIn.IsDaylightSavingTime() != punchOut.IsDaylightSavingTime())
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

                            summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                            //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);

                            switch (punchIn.DayOfWeek)
                            {
                                case DayOfWeek.Monday:
                                    employeeHistory.MondaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.MondaySummary.NumberOfPunches += 2;
                                    employeeHistory.MondaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Tuesday:
                                    employeeHistory.TuesdaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.TuesdaySummary.NumberOfPunches += 2;
                                    employeeHistory.TuesdaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Wednesday:
                                    employeeHistory.WednesdaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.WednesdaySummary.NumberOfPunches += 2;
                                    employeeHistory.WednesdaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Thursday:
                                    employeeHistory.ThursdaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.ThursdaySummary.NumberOfPunches += 2;
                                    employeeHistory.ThursdaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Friday:
                                    employeeHistory.FridaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.FridaySummary.NumberOfPunches += 2;
                                    employeeHistory.FridaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Saturday:
                                    employeeHistory.SaturdaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.SaturdaySummary.NumberOfPunches += 2;
                                    employeeHistory.SaturdaySummary.WorkSummaries.Add(summary);
                                    break;
                                case DayOfWeek.Sunday:
                                    employeeHistory.SundaySummary.TotalHoursWorked += summaryHours;
                                    employeeHistory.SundaySummary.NumberOfPunches += 2;
                                    employeeHistory.SundaySummary.WorkSummaries.Add(summary);
                                    break;
                            }
                        }
                    }
                    //get the total hours
                    calculateTotalHours(employeeHistory);
                }

            }
            catch (Exception drEx)
            {
                throw drEx;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return returnInfo;
        }

        private bool newSummaryAppliesToBillingPeriod(string shiftStartTime, string shiftEndTime, DateTime punchDateTime, DateTime periodStartDateTime, DateTime periodEndDateTime)
        {
            bool retVal = true;

            //make sure punch date/time is not for the next period
            if (punchDateTime.Date == periodEndDateTime.AddDays(1).Date)
            {
                //should we check if the punch is missing a check in?
                retVal = false;
            }
            else
            {
                //if the punch is a monday
                //if the difference between the punch date/time and the shift start is more than
                //6 hours then it was from the previous period's sunday so ignore.
                if (punchDateTime.DayOfWeek == DayOfWeek.Monday)
                {
                    DateTime tempShiftStart = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftStartTime);
                    DateTime tempShiftEnd = DateTime.Parse(punchDateTime.ToString("MM/dd/yyyy ") + shiftEndTime);
                    if (tempShiftEnd < tempShiftStart)
                    {
                        TimeSpan shiftStartDif = tempShiftStart - punchDateTime;
                        if (shiftStartDif.TotalHours >= 6)
                        {
                            retVal = false;
                        }
                    }
                }
            }

            return retVal;
        }

        private void calculateTotalHours(EmployeeHistory employeeHistory)
        {
            //Monday
            calculateBreakTime(employeeHistory.MondaySummary);
            employeeHistory.TotalHours += employeeHistory.MondaySummary.TotalHoursWorked;
            //Tuesday
            calculateBreakTime(employeeHistory.TuesdaySummary);
            employeeHistory.TotalHours += employeeHistory.TuesdaySummary.TotalHoursWorked;
            //Wednesday
            calculateBreakTime(employeeHistory.WednesdaySummary);
            employeeHistory.TotalHours += employeeHistory.WednesdaySummary.TotalHoursWorked;
            //Thursday
            calculateBreakTime(employeeHistory.ThursdaySummary);
            employeeHistory.TotalHours += employeeHistory.ThursdaySummary.TotalHoursWorked;
            //Friday
            calculateBreakTime(employeeHistory.FridaySummary);
            employeeHistory.TotalHours += employeeHistory.FridaySummary.TotalHoursWorked;
            //Saturday
            calculateBreakTime(employeeHistory.SaturdaySummary);
            employeeHistory.TotalHours += employeeHistory.SaturdaySummary.TotalHoursWorked;
            //Sunday
            calculateBreakTime(employeeHistory.SundaySummary);
            employeeHistory.TotalHours += employeeHistory.SundaySummary.TotalHoursWorked;
        }
        private void calculateBreakTime(DailySummary summary)
        {
            if (summary.NumberOfPunches == 2 && summary.TotalHoursWorked >= 6)
            {
                //if there is only a check in and check out then use the default
                //break time
                summary.UseDefaultBreakTime = true;
                summary.TotalHoursWorked -= .5M;
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

                        breakTime = breakOutSummary.CheckInDateTime - breakInSummary.CheckOutDateTime;
                        totalBreakTime += breakTime.TotalMinutes;
                    }
                }

                if (totalBreakTime > 30)
                {
                    summary.UseDefaultBreakTime = false;
                }
                else if (totalBreakTime == 30)
                {
                    summary.UseDefaultBreakTime = true;
                    //the break time is 30 minutes so we do not need to deduct
                    //summary.TotalHoursWorked -= .5M;
                }
                else
                {
                    //break time is less than 30 minutes so
                    //we must subtract the difference
                    summary.UseDefaultBreakTime = true;
                    summary.TotalHoursWorked -= Convert.ToDecimal((30 - totalBreakTime) / 60);
                    summary.TotalHoursWorked = Math.Round(summary.TotalHoursWorked, 2);
                }
            }


            string tempTotalHours = Convert.ToString(summary.TotalHoursWorked);
            string[] s = tempTotalHours.Split('.'); // the first numbers plus decimal point
            if (s.Length > 1)
            {
                int val = int.Parse(s[1]);
                if (s[1] == "00" || s[1] == "01" || s[1] == "02" || s[1] == "03" || s[1] == "04" || s[1] == "05" || s[1] == "06" || s[1] == "07" || s[1] == "08" || s[1] == "09" || s[1] == "10")
                {
                    summary.TotalHoursWorked = Convert.ToDecimal(s[0]);
                }
                else if (val >= 95 && val <= 99)
                {
                    summary.TotalHoursWorked = Convert.ToDecimal(s[0]) + 1M;
                }
            }

        }
    }
}