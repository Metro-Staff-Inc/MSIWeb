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
// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class TicketTrackerDB
    {
        public TicketTrackerDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        

        private DataAccessHelper _dbHelper = new DataAccessHelper();


        public TicketTrackerException GetTicketTrackingExceptions(TicketTrackerException ticketTracker, IPrincipal userPrincipal, string aident)
        {
            TicketTrackerException returnInfo = ticketTracker;
            EmployeeTrackerException employee = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            if (aident == null || aident.Length == 0)
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTrackingExceptions);
            }
            else
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTrackingExceptionsByEmployee);
                dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, aident);
            }
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, ticketTracker.ClientID);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, ticketTracker.PeriodStartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, ticketTracker.PeriodEndDateTime);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    object employeeIdCheck = null;
                    object clientRosterIdCheck = null;
                    object employeeActiveCheck = null;
                    bool employeeActive = false;
                    while (dr.Read())
                    {
                        employeeActive = false;
                        employee = new EmployeeTrackerException();
                        employeeIdCheck = dr.GetValue(dr.GetOrdinal("employee_id"));
                        if (employeeIdCheck != DBNull.Value)
                        {
                            employeeActiveCheck = dr.GetValue(dr.GetOrdinal("employee_active"));
                            if (employeeActiveCheck != DBNull.Value)
                            {
                                employeeActive = dr.GetBoolean(dr.GetOrdinal("employee_active"));
                                if (employeeActive)
                                {
                                    employee.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                    employee.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                                    employee.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                                }
                            }
                        }

                        if (!employeeActive)
                        {
                            employee.EmployeeID = 0;
                            employee.LastName = "EMPLOYEE NOT FOUND";
                            employee.FirstName = "";
                        }

                        employee.TempNumber = dr.GetString(dr.GetOrdinal("badge_number"));
                        clientRosterIdCheck = dr.GetValue(dr.GetOrdinal("client_roster_id"));
                        if (clientRosterIdCheck != DBNull.Value)
                        {
                            employee.ClientRosterID = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                        }
                        employee.PunchExceptionInfo = new PunchException(dr.GetInt32(dr.GetOrdinal("punch_exception_id")), dr.GetString(dr.GetOrdinal("punch_exception_message")));
                        employee.PunchDateTime = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        employee.RoundedPunchDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                        returnInfo.Employees.Add(employee);
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

        //    return ttdb.AddPunch(id, crID, punchDate, deptID, userID);

        public String AddPunch(string id, string crID, DateTime punchDate, int deptID,  String userID, int shiftType)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            String retVal = "Punch Added";
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.AddPunch);
            cw.CommandTimeout = 120;    //10 minutes
            if (id[0] < '0' || id[0] > '9')
                id = id.Substring(2);
            dbSvc.AddInParameter(cw, "@id", DbType.String, id);
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.String, crID);
            dbSvc.AddInParameter(cw, "@punchDate", DbType.DateTime, punchDate);
            dbSvc.AddInParameter(cw, "@deptID", DbType.Int32, deptID);
            dbSvc.AddInParameter(cw, "@userID", DbType.String, userID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                retVal = ex.ToString();
                
            }
            finally
            {
                cw.Dispose();
            }
            return retVal;
        }


        public DailyTracker GetDailyTracking(DailyTracker dt)
        {
            DailyTracker returnInfo = dt;
            EmployeeTracker employee = null;
            DbCommand cw;
            int currentClientRosterId = 0;
            int prevClientRosterId = 0;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTrackingByUser);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, dt.ClientId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, dt.ShiftType.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, dt.Dept.DepartmentID);
            dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, true);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, dt.PeriodStart);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, dt.PeriodEnd);

            dbSvc.AddInParameter(cw, "@userName", DbType.String, "itdept");
            dbSvc.AddOutParameter(cw, "@overrideRoleName", DbType.String, 300);
            try
            {
                string overrideRoleName = string.Empty;
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {

                        currentClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));

                        if (currentClientRosterId != prevClientRosterId)
                        {
                            if (employee != null)
                            {
                                    //get the latest checkin/checkout info
                                    getLatestCheckInCheckOutTimes(employee);
                            }
                            employee = null;
                            prevClientRosterId = currentClientRosterId;
                        }


                        if (employee != null)
                        {
                            //add to the previous punch list
                            this.addEmployeePunch(dr, employee);
                        }
                        else
                        {
                            employee = new EmployeeTracker();
                            employee.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                            employee.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            employee.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            employee.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            //employee.ShiftStartTime = dr.GetString(dr.GetOrdinal("employee_start_time"));
                            //if (employee.ShiftStartTime.Length == 0)
                            //{
                            employee.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            //}
                            //employee.ShiftEndTime = dr.GetString(dr.GetOrdinal("employee_end_time"));
                            //if (employee.ShiftEndTime.Length == 0)
                            //{
                            employee.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            //}
                            employee.ClientRosterID = currentClientRosterId;
                            
                            employee.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                            employee.DefaultPayRate = dr.GetDecimal(dr.GetOrdinal("default_pay_rate"));
                            employee.MinimumWage = dr.GetDecimal(dr.GetOrdinal("minimum_wage"));

                            returnInfo.Employees.Add(employee);
                            //returnInfo.Employees.Add(
                            this.addEmployeePunch(dr, employee);
                        }
                    }
                    if (employee != null)
                    {
                        //get the latest checkin/checkout info
                        getLatestCheckInCheckOutTimes(employee);
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
                       // if (ticketTracker.ClientRosterID <= 0)
                       // {
                       //     overrideRoleName = Convert.ToString(dbSvc.GetParameterValue(cw, "@overrideRoleName"));
                       //     returnInfo.OverrideRoleName = overrideRoleName;
                       // }
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
        public TicketTracker GetTicketTracking(TicketTracker ticketTracker, IPrincipal userPrincipal)
        {
            TicketTracker returnInfo = ticketTracker;
            EmployeeTracker employee = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //int rosterEmployee = 0;
            int prevClientRosterId = 0;
            int prevEmployeeTicketId = 0;
            int currentClientRosterId = 0;
            int currentEmployeeTicketId = 0;

            if (ticketTracker.ClientRosterID > 0)
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTrackingEmployeeSummary);
            }
            else
            {
                //msinet_GetTicketTrackingByUser
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTrackingByUser);
            }
            
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, ticketTracker.ClientID);
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, ticketTracker.Location);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, ticketTracker.ShiftTypeInfo.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, ticketTracker.DepartmentInfo.DepartmentID);

            if (ticketTracker.ClientRosterID <= 0)
            {
                if (ticketTracker.TrackingType == Enums.TrackingTypes.Roster)
                {
                    dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, true);
                }
                else
                {
                    dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, false);
                }
            }

            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, ticketTracker.PeriodStartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, ticketTracker.PeriodEndDateTime);

            if (ticketTracker.ClientRosterID > 0)
            {
                dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, ticketTracker.ClientRosterID);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
                dbSvc.AddOutParameter(cw, "@overrideRoleName", DbType.String, 300);
            }

            try
            {
                int employeeTicketId = 0;
                string overrideRoleName = string.Empty;
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        if (ticketTracker.TrackingType == Enums.TrackingTypes.DayLabor)
                        {
                            currentEmployeeTicketId = dr.GetInt32(dr.GetOrdinal("employee_ticket_id"));
                        
                            if (currentEmployeeTicketId != prevEmployeeTicketId)
                            {
                                if (employee != null)
                                {
                                    //get the latest checkin/checkout info
                                    getLatestCheckInCheckOutTimes(employee);
                                }
                                employee = null;
                                prevEmployeeTicketId = currentEmployeeTicketId;
                            }
                        }
                        else
                        {
                            currentClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));

                            if (currentClientRosterId != prevClientRosterId)
                            {
                                if (employee != null)
                                {
                                    //get the latest checkin/checkout info
                                    getLatestCheckInCheckOutTimes(employee);
                                }
                                employee = null;
                                prevClientRosterId = currentClientRosterId;
                            }
                        }

                        if (employee != null)
                        {
                            //add to the previous punch list
                            this.addEmployeePunch(dr, employee);
                        }
                        else
                        {
                            employee = new EmployeeTracker();
                            employee.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                            employee.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            DateTime start = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                            DateTime end = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                            if (start.AddYears(1) > end)
                                employee.Temp = true;

                            if (ticketTracker.ClientID == 2 || ticketTracker.ClientID == 206 || ticketTracker.ClientID == 207)
                            {
                                employeeTicketId = dr.GetInt32(dr.GetOrdinal("employee_ticket_id"));
                                if (employeeTicketId > 0)
                                {
                                    employee.LastName = "* " + employee.LastName;
                                }
                                employeeTicketId = 0;
                            }
                            employee.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            employee.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            //employee.ShiftStartTime = dr.GetString(dr.GetOrdinal("employee_start_time"));
                            //if (employee.ShiftStartTime.Length == 0)
                            //{
                                employee.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            //}
                            //employee.ShiftEndTime = dr.GetString(dr.GetOrdinal("employee_end_time"));
                            //if (employee.ShiftEndTime.Length == 0)
                            //{
                                employee.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            //}
                            if (ticketTracker.TrackingType == Enums.TrackingTypes.DayLabor )
                            {
                                employee.EmployeeTicketID = currentEmployeeTicketId;
                            }
                            else
                            {
                                employee.ClientRosterID = currentClientRosterId;
                            }
                            employee.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                            employee.DefaultPayRate = dr.GetDecimal(dr.GetOrdinal("default_pay_rate"));
                            employee.MinimumWage = dr.GetDecimal(dr.GetOrdinal("minimum_wage"));

                            returnInfo.Employees.Add(employee);
                            this.addEmployeePunch(dr, employee);
                        }
                    }
                    if (employee != null)
                    {
                        //get the latest checkin/checkout info
                        getLatestCheckInCheckOutTimes(employee);
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
                        if (ticketTracker.ClientRosterID <= 0)
                        {
                            overrideRoleName = Convert.ToString(dbSvc.GetParameterValue(cw, "@overrideRoleName"));
                            returnInfo.OverrideRoleName = overrideRoleName;
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

        public TicketTracker UpdateEmployeePunchTimes(TicketTracker ticketTracker, EmployeeTracker employeeInfo, IPrincipal userPrincipal)
        {
            TicketTracker returnInfo = ticketTracker;
            EmployeeTracker employee = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //int rosterEmployee = 0;
            int prevClientRosterId = 0;
            int prevEmployeeTicketId = 0;
            int currentClientRosterId = 0;
            int currentEmployeeTicketId = 0;

            //checkin employeePunchId
            //checkIn time
            //breakIn employeePunchId
            //break time
            //breakOut employeePunchId
            //breakOut time
            //CheckOut employeePunchId
            //checkOut time
            //changed by
            //changed datetime

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTicketTracking);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, ticketTracker.ClientID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, ticketTracker.ShiftTypeInfo.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, ticketTracker.DepartmentInfo.DepartmentID);
            if (ticketTracker.TrackingType == Enums.TrackingTypes.Roster)
            {
                dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, true);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@rosterEmployeeFlag", DbType.Boolean, false);
            }

            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, ticketTracker.PeriodStartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, ticketTracker.PeriodEndDateTime);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        if (ticketTracker.TrackingType == Enums.TrackingTypes.DayLabor)
                        {
                            currentEmployeeTicketId = dr.GetInt32(dr.GetOrdinal("employee_ticket_id"));

                            if (currentEmployeeTicketId != prevEmployeeTicketId)
                            {
                                if (employee != null)
                                {
                                    //get the latest checkin/checkout info
                                    getLatestCheckInCheckOutTimes(employee);
                                }
                                employee = null;
                                prevEmployeeTicketId = currentEmployeeTicketId;
                            }
                        }
                        else
                        {
                            currentClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));

                            if (currentClientRosterId != prevClientRosterId)
                            {
                                if (employee != null)
                                {
                                    //get the latest checkin/checkout info
                                    getLatestCheckInCheckOutTimes(employee);
                                }
                                employee = null;
                                prevClientRosterId = currentClientRosterId;
                            }
                        }


                        if (employee != null)
                        {
                            //add to the previous punch list
                            this.addEmployeePunch(dr, employee);
                        }
                        else
                        {
                            employee = new EmployeeTracker();
                            employee.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                            employee.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            employee.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            employee.TempNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            employee.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            employee.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            if (ticketTracker.TrackingType == Enums.TrackingTypes.DayLabor)
                            {
                                employee.EmployeeTicketID = currentEmployeeTicketId;
                            }
                            else
                            {
                                employee.ClientRosterID = currentClientRosterId;
                            }
                            returnInfo.Employees.Add(employee);
                            this.addEmployeePunch(dr, employee);
                        }
                    }
                    if (employee != null)
                    {
                        //get the latest checkin/checkout info
                        getLatestCheckInCheckOutTimes(employee);
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

        public bool ApproveTrackingHours(TicketTrackerApproval approvalInfo, IPrincipal userPrincipal)
        {
            bool returnResult = false;
            //returnResult.EmployeePunchMaintenanceInfo = employeePunch;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ApproveDailyHours);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@rosterApproveCSV", DbType.String, approvalInfo.ApprovedPunchList);
            dbSvc.AddInParameter(cw, "@noshowApproveCSV", DbType.String, approvalInfo.ApprovedNoShowList);
            dbSvc.AddInParameter(cw, "@shiftDate", DbType.DateTime, approvalInfo.ShiftDate);
            dbSvc.AddInParameter(cw, "@approvedBy", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@approvedDateTime", DbType.DateTime, approvalInfo.ApprovedDateTime);
            dbSvc.AddInParameter(cw, "@hoursXML", DbType.String, approvalInfo.ApprovedPunchListXML);
            try
            {
                dbSvc.ExecuteNonQuery(cw);

                //record was saved
                returnResult = true;
            }
            catch (Exception ex)
            {
                
                returnResult = false;
            }
            finally
            {
                cw.Dispose();
            }

            return returnResult;
        }

        public bool UnlockTrackingHours(TicketTrackerUnlock unlockInfo, IPrincipal userPrincipal)
        {
            bool returnResult = false;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UnlockDailyHours);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@unlockList", DbType.String, unlockInfo.UnlockPunchList);

            try
            {
                dbSvc.ExecuteNonQuery(cw);

                //record was saved
                returnResult = true;
            }
            catch (Exception ex)
            {
                
                returnResult = false;
            }
            finally
            {
                cw.Dispose();
            }

            return returnResult;
        }

        private void addEmployeePunch(IDataReader dr, EmployeeTracker employee)
        {
            int employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
            bool deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
            if (employeePunchId > 0 && !deleted)
            {
                EmployeePunchSummary employeePunchSummary = new EmployeePunchSummary();
                employeePunchSummary.EmployeePunchID = employeePunchId;
                employeePunchSummary.PunchDateTime = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                employeePunchSummary.RoundedPunchDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                employeePunchSummary.BiometricResult = dr.GetInt32(dr.GetOrdinal("biometric_result"));
                employeePunchSummary.PunchPic = dr.GetDateTime(dr.GetOrdinal("punch_pic"));
                
                
                employeePunchSummary.IsApproved = dr.GetBoolean(dr.GetOrdinal("approved_flag"));
                if (employeePunchSummary.IsApproved)
                {
                    employeePunchSummary.ApprovedBy = dr.GetString(dr.GetOrdinal("approved_by"));
                    employeePunchSummary.ApprovedDateTime = dr.GetDateTime(dr.GetOrdinal("approved_dt"));
                } 
                
                employeePunchSummary.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                employeePunchSummary.TicketInfo.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
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
                //check if it were an early check in
                EmployeePunchSummary summary = (EmployeePunchSummary)employee.Punches[0];
                DateTime shiftStart = DateTime.Parse(summary.PunchDateTime.ToString("MM/dd/yyyy " + employee.ShiftStartTime));
                if (shiftStart > summary.RoundedPunchDateTime)
                {
                    employee.PunchStatus = Enums.EmployeePunchStatus.EarlyCheckIn;
                }
            }
            else
            {
                //checked in
                employee.PunchStatus = Enums.EmployeePunchStatus.CheckedIn;
                //check if it is an early check in
                EmployeePunchSummary summary = (EmployeePunchSummary)employee.Punches[0];
                DateTime shiftStart = DateTime.Parse(summary.PunchDateTime.ToString("MM/dd/yyyy " + employee.ShiftStartTime));
                if (shiftStart > summary.RoundedPunchDateTime)
                {
                    employee.PunchStatus = Enums.EmployeePunchStatus.EarlyCheckIn;
                }
            }
        }

        private void getLatestCheckInCheckOutTimes(EmployeeTracker employee)
        {
            ArrayList checkIns = new ArrayList();
            ArrayList checkOuts = new ArrayList();
            EmployeeWorkSummary workSummary = new EmployeeWorkSummary();
            EmployeePunchSummary punch = null;
            int workSummaryIdx = 0;
            for (int idx = 0; idx < employee.Punches.Count; ++idx)
            {
                punch = (EmployeePunchSummary)employee.Punches[idx];
                if ((idx + 1) % 2 == 0)
                {
                    workSummary = (EmployeeWorkSummary)employee.WorkSummaries[workSummaryIdx];
                    workSummary.CheckOutDateTime = punch.PunchDateTime;
                    workSummary.RoundedCheckOutDateTime = punch.RoundedPunchDateTime;
                    workSummary.CheckOutEmployeePunchID = punch.EmployeePunchID;
                    checkOuts.Add(employee.Punches[idx]);
                }
                else
                {
                    //create a new work summary
                    if (idx > 0)
                    {
                        workSummaryIdx++;
                    }
                    workSummary = new EmployeeWorkSummary();
                    workSummary.CheckInDateTime = punch.PunchDateTime;
                    workSummary.RoundedCheckInDateTime = punch.RoundedPunchDateTime;
                    workSummary.CheckInEmployeePunchID = punch.EmployeePunchID;
                    workSummary.DepartmentInfo = punch.TicketInfo.DepartmentInfo;
                    employee.WorkSummaries.Add(workSummary);
                    checkIns.Add(employee.Punches[idx]);
                }
            }
            employee.CheckInPunches = checkIns;
            employee.CheckOutPunches = checkOuts;
        }
    }
}