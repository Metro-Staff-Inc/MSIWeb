using System;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class DailyPunchDepartmentShiftInfo
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public string departmentName { get; set; }
        public int shiftType { get; set; } /* first, second, third etc */
        //public List<DailyPunchEmployeeInfo> employees { get; set; }
        public string aidentNumber { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public DateTime requestDate { get; set; }
        public List<DailyPunchInfo> punches { get; set; }
    }

    public class DailyPunchInfo
    {
        public DateTime punchExact { get; set; }
        public DateTime punchRound { get; set; }
        public DateTime earliestDate { get; set; }
    }
    public class DailyPunchEmployeeInfo
    {
        public string aidentNumber { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public List<DailyPunchInfo> punches { get; set; }

        /* 
         * CREATE TABLE #punches
        (
         client_id int,
         client_name varchar(64),
         aident_number varchar(12),
         employee_id int,
         last_name varchar(64),
         first_name varchar(64),
         department_id int,
         department_name varchar(64),
         punch_dt	DateTime,
         rounded_punch_dt DateTime,
         created_by varchar(64),
         created_dt DateTime,
         shift_desc varchar(64),
         shift_type int,
         shift_id	int, 
         earliestDate DateTime,
         latestDate DateTime, 
         deletedDate DateTime, 
         location_id	int, 
         client_roster_id		int,
         break_time		decimal(8,2),
         tracking_start_time	varchar(50),
         tracking_end_time		varchar(50), 
         shift_start	varchar(50),
         shift_end		varchar(50)
        )
        *
        */
    }

    public class HeadCountReportDB
    {

        public int clientId { get; set; }
        public string clientName { get; set; }
        public string departmentName { get; set; }
        public int shiftType { get; set; } /* first, second, third etc */
        //public List<DailyPunchEmployeeInfo> employees { get; set; }
        public string aidentNumber { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public DateTime punchExact { get; set; }
        public DateTime punchRound { get; set; }
        public DateTime earliestDate { get; set; }

        public HeadCountReportDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();
        private decimal _minShiftBreakHours = 5;

        public HeadCountReport GetHeadCountReport(HeadCountReport headCountReport, IPrincipal userPrincipal)
        {
            HeadCountReport returnInfo = headCountReport;
            EmployeeHistory employeeHistory = null;
            EmployeeWorkSummary workSummary = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            int currentShift = 0;
            DateTime currentDate;
            int savedShift = 0;
            DateTime savedDate = new DateTime(1, 1, 1);
            DateTime shiftStart = new DateTime(1, 1, 1);
            DateTime shiftEnd = new DateTime(1, 1, 1);
            bool processedCheckIn = false;
            bool newWorkSummary = false;
            double validShiftHours = 0;
            Hashtable employeeTotalHours = new Hashtable();
            Hashtable employeesMultiDept = new Hashtable();
            DateTime weekEndingDate = headCountReport.EndDateTime;
            int histIdx = 0;

            //tutco provides paid 20 min break when hours 
            //worked are less than 6 but greater than 5.
            if (headCountReport.ClientID == 186)
            {
                _minShiftBreakHours = 6;
            }

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetHeadCountReportSummary);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, headCountReport.ClientID);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, headCountReport.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, headCountReport.EndDateTime.AddDays(1));
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, weekEndingDate.ToString("MM/dd/yyyy"));

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                TimeSpan dif;
                EmployeeWorkSummary prevSummary = null;
                DayOfWeek daySummaryHoursApplyTo = DayOfWeek.Monday;
                try
                {
                    Boolean finished = false;
                    while (!finished)
                    {
                        finished = !dr.Read();
                        if (finished || employeeHistory == null || employeeHistory.EmployeeID != dr.GetInt32(dr.GetOrdinal("employee_id")))
                        {
                            //check if the previous employee had work summaries
                            if (employeeHistory != null && employeeHistory.WorkSummaries.Count == 0)
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

                                    if (summary.CheckOutDateTime == null || summary.CheckOutDateTime.Date == new DateTime(1, 1, 1).Date)
                                    {

                                        //indicate that there is a missing punch
                                        //employeeHistory.HasInvalidWorkSummaries = true;
                                        punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, 0);

                                        daySummaryHoursApplyTo = punchIn.DayOfWeek;

                                        switch (daySummaryHoursApplyTo)
                                        {
                                            case DayOfWeek.Monday:

                                                employeeHistory.MondaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Tuesday:

                                                employeeHistory.TuesdaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Wednesday:

                                                employeeHistory.WednesdaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Thursday:

                                                employeeHistory.ThursdaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Friday:

                                                employeeHistory.FridaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Saturday:

                                                employeeHistory.SaturdaySummary.NumberOfPunches += 1;

                                                break;
                                            case DayOfWeek.Sunday:

                                                employeeHistory.SundaySummary.NumberOfPunches += 1;

                                                break;
                                        }

                                    }
                                    else
                                    {


                                        punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, 0);
                                        punchOut = new DateTime(summary.RoundedCheckOutDateTime.Year, summary.RoundedCheckOutDateTime.Month, summary.RoundedCheckOutDateTime.Day, summary.RoundedCheckOutDateTime.Hour, summary.RoundedCheckOutDateTime.Minute, 0);

                                        daySummaryHoursApplyTo = punchIn.DayOfWeek;

                                        if (employeeHistory.ClientID == 158 || employeeHistory.ClientID == 166)
                                        {
                                            daySummaryHoursApplyTo = summary.SummaryShiftStartDateTime.DayOfWeek;
                                        }

                                        //sum up the hours
                                        difference = punchOut - punchIn;
                                        summaryHours = Math.Round(Convert.ToDecimal(difference.TotalMinutes / 60), 2);
                                        //summaryHours = Math.Round( Convert.ToDecimal(difference.TotalMinutes / 60) * 4, 0) / 4; // * 4 / 4, 2);

                                        switch (daySummaryHoursApplyTo)
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
                                employeeHistory.ClientID = headCountReport.ClientID;
                                employeeHistory.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                                employeeHistory.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                                employeeHistory.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                employeeHistory.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                                employeeHistory.OverrideBreakTime = dr.GetDecimal(dr.GetOrdinal("override_break_time"));
                                employeeHistory.PrimaryRole = dr.GetString(dr.GetOrdinal("primary_role"));

                                savedShift = 0;
                                returnInfo.EmployeeHistoryCollection.Add(employeeHistory);
                                prevSummary = null;
                            }
                        }
                        if (finished)
                            continue;

                        if (employeeHistory.TempNumber.Substring(2) == "47897")
                            System.Diagnostics.Debug.Write("");

                        currentShift = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        currentDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        if (currentShift == savedShift)
                        {
                            if (processedCheckIn)
                            {
                                if (headCountReport.ClientID == 2 || headCountReport.ClientID == 207)
                                {
                                    workSummary.CheckOutDateTime = currentDate;
                                    workSummary.RoundedCheckOutDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    workSummary.CheckOutEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                    workSummary.CheckOutApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                    //force a new summary
                                    savedShift = 0;
                                    newWorkSummary = false;
                                }
                                else
                                {
                                    //check if the time is before the next check in for the shift
                                    //if after the check in for the next day then this is a new summary
                                    //if less than the check in for the next day then check out.
                                    if (currentDate < shiftStart.AddDays(1))
                                    {
                                        dif = currentDate - savedDate;

                                        if (dif.TotalHours > validShiftHours && headCountReport.ClientID != 168)
                                        {
                                            newWorkSummary = true;
                                        }
                                        else
                                        {
                                            workSummary.CheckOutDateTime = currentDate;
                                            workSummary.RoundedCheckOutDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                            workSummary.CheckOutApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                                            workSummary.CheckOutEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                            if (headCountReport.ClientID == 113)
                                            {
                                                //round the time
                                                //round the time
                                                workSummary.RoundedCheckOutDateTime = _helper.GetRoundedPunchTime(workSummary.CheckOutDateTime);
                                                workSummary.CheckOutDateTime = workSummary.RoundedCheckOutDateTime;
                                            }
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
                                sumVal = _helper.NewSummaryAppliesToBillingPeriodSaturday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, headCountReport.StartDateTime, headCountReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, headCountReport.ClientID, currentShift, employeeHistory.WorkSummaries.Count);
                            }
                            else
                            {
                                sumVal = _helper.NewSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, headCountReport.StartDateTime, headCountReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, headCountReport.ClientID, currentShift);
                            }

                            //if (_helper.NewSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, headCountReport.StartDateTime, headCountReport.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate))
                            if (sumVal)
                            {
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
                                workSummary.CheckInDateTime = currentDate;
                                workSummary.RoundedCheckInDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                workSummary.CheckInEmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                                workSummary.ShiftInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                                if (headCountReport.ClientID == 113)
                                {
                                    //round the time
                                    workSummary.RoundedCheckInDateTime = _helper.GetRoundedPunchTime(workSummary.CheckInDateTime);
                                    workSummary.CheckInDateTime = workSummary.RoundedCheckInDateTime;
                                }
                                workSummary.CheckInApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
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
                                workSummary.SummaryShiftStartDateTime = shiftStart;
                                workSummary.SummaryShiftEndDateTime = shiftEnd;

                                if ((employeeHistory.ClientID == 158 || employeeHistory.ClientID == 166) && prevSummary != null)
                                {
                                    if (workSummary.CheckInDateTime < prevSummary.SummaryShiftEndDateTime)
                                    {
                                        workSummary.SummaryShiftStartDateTime = prevSummary.SummaryShiftStartDateTime;
                                        workSummary.SummaryShiftEndDateTime = prevSummary.SummaryShiftEndDateTime;
                                    }
                                }

                                validShiftHours = _helper.GetValidShiftHours(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")));

                                prevSummary = workSummary;
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
                        //TimeSpan difference;
                        foreach (EmployeeWorkSummary summary in employeeHistory.WorkSummaries)
                        {
                            if (!summary.CheckInApproved || !summary.CheckOutApproved)
                            {
                                employeeHistory.HasUnapprovedHours = true;
                            }

                            if (summary.CheckOutDateTime == null || summary.CheckOutDateTime.Date == new DateTime(1, 1, 1).Date)
                            {
                                //indicate that there is a missing punch
                                //employeeHistory.HasInvalidWorkSummaries = true;
                                punchIn = new DateTime(summary.RoundedCheckInDateTime.Year, summary.RoundedCheckInDateTime.Month, summary.RoundedCheckInDateTime.Day, summary.RoundedCheckInDateTime.Hour, summary.RoundedCheckInDateTime.Minute, 0);

                                daySummaryHoursApplyTo = punchIn.DayOfWeek;

                                switch (daySummaryHoursApplyTo)
                                {
                                    case DayOfWeek.Monday:

                                        employeeHistory.MondaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Tuesday:

                                        employeeHistory.TuesdaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Wednesday:

                                        employeeHistory.WednesdaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Thursday:

                                        employeeHistory.ThursdaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Friday:

                                        employeeHistory.FridaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Saturday:

                                        employeeHistory.SaturdaySummary.NumberOfPunches += 1;

                                        break;
                                    case DayOfWeek.Sunday:

                                        employeeHistory.SundaySummary.NumberOfPunches += 1;

                                        break;

                                }


                            }
                        }
                        //get the total hours
                        calculateTotalHours(employeeHistory, employeeTotalHours, employeesMultiDept);

                        //process the employees accross departments
                        processMultiDepartments(employeesMultiDept, headCountReport);
                    }

                }
                catch (Exception drEx)
                {

                    throw drEx;
                }
                finally
                {

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
        public DataSet GetRosterAndHeadCountData(int clientId)
        {
            DataSet ds = new DataSet("PunchInfo");
            using (SqlConnection conn = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServices_LOCAL"].ConnectionString))
            {
                SqlCommand sqlComm = new SqlCommand(MSINetStoredProcs.GetRosterAndHeadCountReport, conn);
                //sqlComm = new SqlCommand("SELECT * FROM office WHERE office_city = 'ELGIN'", conn);

                sqlComm.Parameters.AddWithValue("@clientId", clientId);

                sqlComm.CommandType = CommandType.StoredProcedure;
                //sqlComm.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                //da.E

                da.Fill(ds);
            }

            /*
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetRosterAndHeadCountReport);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientId);
            DataSet data = new DataSet("PunchInfo");
            
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {

                }
                catch(Exception ex)
                {

                }
            }
            catch(Exception ex)
            {

            }
            */
            return ds;
        }
        
        public List<DailyPunchDepartmentShiftInfo> SelectListRosterAndHeadCountReport(int clientId)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetRosterAndHeadCountReport);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientId);
            List<DailyPunchDepartmentShiftInfo> list = new List<DailyPunchDepartmentShiftInfo>();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    Boolean finished = false;
                    DailyPunchDepartmentShiftInfo dsi = null;
                    while (!finished)
                    {
                        finished = !dr.Read();
                        if ( !finished )
                        {
                            string departmentName = dr.GetString(dr.GetOrdinal("department_name"));
                            int shiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                            string aidentNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                            /* do we need a new record? */
                            int n = list.Count;
                            if( n == 0 || !aidentNumber.Equals(list[n-1].aidentNumber) 
                                || !departmentName.Equals(list[n-1].departmentName) 
                                || !shiftType.Equals(list[n-1].shiftType) )
                            {
                                if( dsi != null )
                                {
                                    list.Add(dsi);
                                }
                                dsi = new DailyPunchDepartmentShiftInfo();
                                dsi.departmentName = departmentName;
                                dsi.shiftType = shiftType;
                                dsi.clientName = dr.GetString(dr.GetOrdinal("client_name"));
                                dsi.clientId = dr.GetInt32(dr.GetOrdinal("client_id"));

                                dsi.aidentNumber = aidentNumber;
                                dsi.lastName = dr.GetString(dr.GetOrdinal("last_name"));
                                dsi.firstName = dr.GetString(dr.GetOrdinal("first_name"));
                                dsi.requestDate = dr.GetDateTime(dr.GetOrdinal("request_date"));
                                dsi.punches = new List<DailyPunchInfo>();
                            }
                            DailyPunchInfo dpi = new DailyPunchInfo();
                            dpi.earliestDate = dr.GetDateTime(dr.GetOrdinal("earliest_date"));
                            dpi.punchExact = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                            dpi.punchRound = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            if( dpi.punchExact.CompareTo(dpi.earliestDate) >= 0 )
                            {
                                dsi.punches.Add(dpi);
                            }
                        }
                    }
                    if (dsi != null) // store remaining info
                    {
                        list.Add(dsi);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
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
            return list;
        }

        private void calculateTotalHours(EmployeeHistory employeeHistory, Hashtable employeeHours, Hashtable employeesMultiDept)
        {
            employeeHistory.WorkSummaries.Sort(new EmployeeWorkSummarySortByCheckIn());

            EmployeeWorkSummary summary = null;

            //only process multi-department logic for authorized clients
            //if (employeeHistory.ClientID != 186)
            //{
            //do not process as multi-department
            //    calculateTotalHours(employeeHistory);
            //}
            //else
            // {
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

            if (employeeHistory.ClientID != 2 && employeeHistory.ClientID != 121 && employeeHistory.ClientID != 122 && employeeHistory.ClientID != 205 && employeeHistory.ClientID != 207 && employeeHistory.ClientID != 158 && employeeHistory.ClientID != 243 && employeeHistory.ClientID != 245)
            {
                calculateBreak = true;
            }

            if (employeeHistory.TempNumber == "TR38398")
            {
                System.Diagnostics.Debug.Write("testy");
            }
            //calculate the break time for each day
            //Monday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.MondaySummary, calculateBreak, weeklyHours[0] + employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.MondaySummary.TotalHoursWorked);
            weeklyHours[0] += employeeHistory.MondaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.MondaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.MondaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }

            //Tuesday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.TuesdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.TuesdaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.TuesdaySummary, calculateBreak, weeklyHours[1] + employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.TuesdaySummary.TotalHoursWorked);
            weeklyHours[1] += employeeHistory.TuesdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.TuesdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.TuesdaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }
            //Wednesday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.WednesdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.WednesdaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.WednesdaySummary, calculateBreak, weeklyHours[2] + employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.WednesdaySummary.TotalHoursWorked);
            weeklyHours[2] += employeeHistory.WednesdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.WednesdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.WednesdaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }
            //Thursday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.ThursdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.ThursdaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.ThursdaySummary, calculateBreak, weeklyHours[3] + employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.ThursdaySummary.TotalHoursWorked);
            weeklyHours[3] += employeeHistory.ThursdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.ThursdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.ThursdaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }
            //Friday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.FridaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.FridaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.FridaySummary, calculateBreak, weeklyHours[4] + employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.FridaySummary.TotalHoursWorked);
            weeklyHours[4] += employeeHistory.FridaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.FridaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.FridaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }
            //Saturday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.SaturdaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.SaturdaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.SaturdaySummary, calculateBreak, weeklyHours[5] + employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SaturdaySummary.TotalHoursWorked);
            weeklyHours[5] += employeeHistory.SaturdaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.SaturdaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.SaturdaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }
            //Sunday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.SundaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.SundaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.SundaySummary, calculateBreak, weeklyHours[6] + employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SundaySummary.TotalHoursWorked);
            weeklyHours[6] += employeeHistory.SundaySummary.TotalHoursWorked;
            weeklyHours[7] += employeeHistory.SundaySummary.TotalHoursWorked;
            employeeHistory.TotalHours += employeeHistory.SundaySummary.TotalHoursWorked;

            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = true;
            }

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
            //}
        }

        private void processMultiDepartments(Hashtable employeesMultiDept, HeadCountReport headCountReport)
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
                            originalEmpHist = (EmployeeHistory)headCountReport.EmployeeHistoryCollection[group.EmployeeHistoryRefIdx];
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
            EmployeeWorkSummary summary = null;

            if (employeeHistory.ClientID != 2 && employeeHistory.ClientID != 121 && employeeHistory.ClientID != 122 && employeeHistory.ClientID != 205 && employeeHistory.ClientID != 207)
            {
                calculateBreak = true;
            }

            //calculate the break time for each day
            //Monday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }

            calculateBreakTime(employeeHistory.MondaySummary, calculateBreak, employeeHistory.MondaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.MondaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.MondaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Tuesday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.TuesdaySummary, calculateBreak, employeeHistory.TuesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.TuesdaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.TuesdaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Wednesday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.WednesdaySummary, calculateBreak, employeeHistory.WednesdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.WednesdaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.WednesdaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Thursday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.ThursdaySummary, calculateBreak, employeeHistory.ThursdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.ThursdaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.ThursdaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Friday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.FridaySummary, calculateBreak, employeeHistory.FridaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.FridaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.FridaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Saturday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.SaturdaySummary, calculateBreak, employeeHistory.SaturdaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SaturdaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.SaturdaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

            //Sunday
            if (employeeHistory.ClientID == 166)
            {
                if (employeeHistory.MondaySummary.WorkSummaries.Count > 0)
                {
                    summary = (EmployeeWorkSummary)employeeHistory.MondaySummary.WorkSummaries[0];
                    if (summary.ShiftInfo.ShiftID == 1 || summary.ShiftInfo.ShiftID == 3)
                    {
                        calculateBreak = false;
                    }
                    else
                    {
                        calculateBreak = true;
                    }
                }
            }
            calculateBreakTime(employeeHistory.SundaySummary, calculateBreak, employeeHistory.SundaySummary.TotalHoursWorked, employeeHistory.OverrideBreakTime, employeeHistory.SundaySummary.TotalHoursWorked);
            employeeHistory.TotalHours += employeeHistory.SundaySummary.TotalHoursWorked;
            if (employeeHistory.ClientID == 166)
            {
                calculateBreak = false;
            }

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

        private void calculateBreakTime(DailySummary summary, bool calculateBreak, decimal totalDailyHours, decimal overrideBreakHours, decimal currentDailyHours)
        {
            double defaultBreakMin = 30;
            decimal defaultBreakHours = .5M;

            if (calculateBreak)
            {
                if (overrideBreakHours > 0)
                {
                    defaultBreakMin = (double)overrideBreakHours * 60;
                    defaultBreakHours = overrideBreakHours;
                }

                if (summary.NumberOfPunches == 2 && totalDailyHours > _minShiftBreakHours)
                {
                    //if there is only a check in and check out then use the default
                    //break time
                    if (currentDailyHours >= 1)
                    {
                        summary.UseDefaultBreakTime = true;
                        summary.TotalHoursWorked -= defaultBreakHours;
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

                            breakTime = breakOutSummary.CheckInDateTime - breakInSummary.CheckOutDateTime;
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
                int val = int.Parse(s[1]);
                if (s[1] == "00" || s[1] == "01" || s[1] == "02" || s[1] == "03" || s[1] == "04" || s[1] == "05" || s[1] == "06" || s[1] == "07" || s[1] == "08" || s[1] == "09" || s[1] == "10")
                {
                    summary.TotalHoursWorked = Convert.ToDecimal(s[0]);
                }
                else if (val >= 45 && val <= 49)
                {
                    summary.TotalHoursWorked = Convert.ToDecimal(s[0] + ".50");
                }
                else if (val >= 92 && val <= 99)
                {
                    summary.TotalHoursWorked = Convert.ToDecimal(s[0]) + 1M;
                }
            }
        }
    }
}