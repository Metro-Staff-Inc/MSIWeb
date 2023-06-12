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
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class EmployeeHistoryDB
    {
        public EmployeeHistoryDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();

        public EmployeeHistory GetEmployeeHistory(EmployeeHistory employeeHistory, IPrincipal userPrincipal)
        {
            EmployeeHistory returnInfo = employeeHistory;
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
            int employeePunchId = 0;

            DateTime weekEndingDate = employeeHistory.EndDateTime;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeHistory);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, employeeHistory.ClientID);
            dbSvc.AddInParameter(cw, "@tempNumber", DbType.String, employeeHistory.TempNumber);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, employeeHistory.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, employeeHistory.EndDateTime.AddDays(1));

            try
            {
                TimeSpan dif;
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //a work summary = same shift same date
                        if (employeeHistory.EmployeeID <= 0)
                        {
                            employeeHistory.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            employeeHistory.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            employeeHistory.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                        }
                        employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                        Boolean deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
                        if (employeePunchId > 0 && !deleted)
                        {
                            currentShift = dr.GetInt32(dr.GetOrdinal("shift_type"));
                            currentDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                            DateTime punchRound = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            if (currentShift == savedShift)
                            {
                                if (employeeHistory.ClientID == 2 || employeeHistory.ClientID == 207) //American Litho
                                {
                                    workSummary.CheckOutDateTime = currentDate;
                                    workSummary.RoundedCheckOutDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    //force a new summary
                                    savedShift = 0;
                                    newWorkSummary = false;
                                }
                                else
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
                                                workSummary.CheckOutEmployeePunchID = employeePunchId;
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
                                bool sumVal = false;
                                int clientId = employeeHistory.ClientID;
                                if (weekEndingDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodSaturday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, employeeHistory.StartDateTime, employeeHistory.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, clientId, currentShift, employeeHistory.WorkSummaries.Count);
                                }
                                else if (weekEndingDate.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriodTuesday(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), currentDate, employeeHistory.StartDateTime, employeeHistory.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, clientId, currentShift);
                                }
                                else
                                {
                                    sumVal = _helper.NewSummaryAppliesToBillingPeriod(dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), punchRound/*currentDate*/, employeeHistory.StartDateTime, employeeHistory.EndDateTime, dr.GetDateTime(dr.GetOrdinal("week_ending_date")), weekEndingDate, clientId, currentShift);
                                }

                                if (sumVal)
                                {
                                    //new work summary
                                    workSummary = new EmployeeWorkSummary();
                                    workSummary.Badge = employeeHistory.AIdentNumber;
                                    workSummary.ShiftTypeInfo.ShiftTypeId = currentShift;
                                    workSummary.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                                    workSummary.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("punch_department_name"));
                                    workSummary.CheckInDateTime = currentDate;
                                    workSummary.RoundedCheckInDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                                    workSummary.CheckInEmployeePunchID = employeePunchId;
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

        private void addEmployeePunch(IDataReader dr, EmployeeTracker employee)
        {
            int employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
            if (employeePunchId > 0)
            {
                EmployeePunchSummary employeePunchSummary = new EmployeePunchSummary();
                employeePunchSummary.EmployeePunchID = employeePunchId;
                employeePunchSummary.PunchDateTime = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                employeePunchSummary.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                employee.Punches.Add(employeePunchSummary);
            }

            determinePunchStatus(employee);
        }

        private void determinePunchStatus(EmployeeTracker employee)
        {
            if (employee.Punches.Count == 0)
            {
                //not checked in
                employee.PunchStatus = Enums.EmployeePunchStatus.NotCheckedIn;
            }
            else if (employee.Punches.Count % 2 == 0)
            {
                //checked out
                employee.PunchStatus = Enums.EmployeePunchStatus.CheckedOut;
            }
            else
            {
                //checked in
                employee.PunchStatus = Enums.EmployeePunchStatus.CheckedIn;
            }
        }

        private void getLatestCheckInCheckOutTimes(EmployeeTracker employee)
        {
            ArrayList checkIns = new ArrayList();
            ArrayList checkOuts = new ArrayList();

            for (int idx = 0; idx < employee.Punches.Count; ++idx)
            {
                if ((idx + 1) % 2 == 0)
                {
                    checkOuts.Add(employee.Punches[idx]);
                }
                else
                {
                    checkIns.Add(employee.Punches[idx]);
                }
            }
            employee.CheckInPunches = checkIns;
            employee.CheckOutPunches = checkOuts;
        }
    }
}