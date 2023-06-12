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
    public class EmployeePunchDB
    {
        public EmployeePunchDB()
        {
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();

        public string ClearClientRoster(int clientRosterId)
        {
            string retMsg = "Cleared!";
            DbCommand cw = null;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            try
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClearClientRoster);
                cw.CommandTimeout = 120;    //10 minutes
                dbSvc.AddInParameter(cw, "@crId", DbType.Int32, clientRosterId);
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception e)
            {
                retMsg = "An error occurred: " + e;
            }
            finally
            {
                if( cw != null )
                {
                    cw.Dispose();
                }
            }
            return retMsg;
        }
        public bool DeletePunch(string id, string punchID)
        {
            int retRosterID = 0;

            DbCommand cw = null;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            try
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.DeleteEmployeePunch);
                cw.CommandTimeout = 120;    //10 minutes
                dbSvc.AddInParameter(cw, "@employeePunchID", DbType.Int32, Convert.ToInt32(punchID));
                dbSvc.AddInParameter(cw, "@userID", DbType.String, id);
                dbSvc.AddOutParameter(cw, "@returnID", DbType.Int32, 8);

                dbSvc.ExecuteNonQuery(cw);
                var obj = dbSvc.GetParameterValue(cw, "@returnID"); 
                if( obj.ToString().Length > 0 )
                    retRosterID = (int)dbSvc.GetParameterValue(cw, "@returnID");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                retRosterID = 0;
                //throw (ex);
            }
            finally
            {
                if( cw != null ) 
                    cw.Dispose();
            }
            if (retRosterID != 0)
            {
                return true;
            }
            else
                return false;
        }
        public string LineApprove(string userName, string punchIds)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.LineApprove);
            cw.CommandTimeout = 120; //10 minutes
            dbSvc.AddInParameter(cw, "@input", DbType.String, punchIds);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userName);
            dbSvc.AddOutParameter(cw, "@approvedCount", DbType.Int32, 4);
            int approved = 0;
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                approved = (int)(dbSvc.GetParameterValue(cw, "@approvedCount"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return "" + approved;
        }

        public String UpdatePunch(string userID, int punchId, DateTime dt)
        {
            String retDate = "01/01/1900 00:00 000";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateEmployeePunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@punchID", DbType.Int32, punchId);
            dbSvc.AddInParameter(cw, "@userID", DbType.String, userID);
            dbSvc.AddInParameter(cw, "@punchTime", DbType.DateTime, dt);
            dbSvc.AddOutParameter(cw, "@returnDate", DbType.DateTime, 8);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                retDate = ((DateTime) (dbSvc.GetParameterValue(cw, "@returnDate"))).ToString("MM/dd/yyyy hh:mm tt");   //dr.GetDateTime(dr.GetOrdinal("returnDate"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }

            return retDate;
        }

        public ArrayList GetEmployeeTicketForPunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            ArrayList returnList = new ArrayList();
            EmployeePunchResult punchResult = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            int rosterEmployee = 0;
            int prevClientRosterId = 0;
            int prevEmployeeTicketId = 0;
            int currentClientRosterId = 0;
            int currentEmployeeTicketId = 0;

            //Get the previous day
            DateTime previousDate = employeePunchSummary.PunchDateTime.AddDays(-1);
            
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeTicketForPunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, employeePunchSummary.ClientID);
            dbSvc.AddInParameter(cw, "@tempNumber", DbType.String, employeePunchSummary.TempNumber);
            dbSvc.AddInParameter(cw, "@ticketEffectiveDateStart", DbType.DateTime, previousDate.ToString("MM/dd/yyyy 00:00:00"));
            dbSvc.AddInParameter(cw, "@ticketEffectiveDateEnd", DbType.DateTime, employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy 23:59:59.000"));
            dbSvc.AddInParameter(cw, "@clientRosterId", DbType.Int32, employeePunchSummary.ClientRosterID);
            dbSvc.AddInParameter(cw, "@punchTime", DbType.DateTime, employeePunchSummary.PunchDateTime);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        rosterEmployee = dr.GetInt32(dr.GetOrdinal("roster_employee"));
                        currentEmployeeTicketId = 0;// dr.GetInt32(dr.GetOrdinal("employee_ticket_id"));
                        currentClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                        
                        if (rosterEmployee == 0)
                        {
                            if (currentEmployeeTicketId != prevEmployeeTicketId)
                            {
                                punchResult = null;
                                prevEmployeeTicketId = currentEmployeeTicketId;
                            }
                        }
                        else
                        {
                            if (currentClientRosterId != prevClientRosterId)
                            {
                                punchResult = null;
                                prevClientRosterId = currentClientRosterId;
                            }
                        }


                        if (punchResult != null)
                        {
                            //add to the previous punch list
                            if (this.isPunchValid(employeePunchSummary, dr))
                            {
                                this.addPreviousPunch(dr, punchResult.EmployeePunchSummaryInfo);
                            }
                        }
                        else
                        {
                            punchResult = new EmployeePunchResult();
                            punchResult.EmployeePunchSummaryInfo.BiometricResult = employeePunchSummary.BiometricResult;
                            punchResult.EmployeePunchSummaryInfo.ClientID = employeePunchSummary.ClientID;
                            punchResult.EmployeePunchSummaryInfo.UseExactTimes = employeePunchSummary.UseExactTimes;
                            punchResult.EmployeePunchSummaryInfo.CalculateWeeklyHours = employeePunchSummary.CalculateWeeklyHours;
                            punchResult.EmployeePunchSummaryInfo.PunchDateTime = employeePunchSummary.PunchDateTime;
                            punchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = employeePunchSummary.RoundedPunchDateTime;
                            punchResult.EmployeePunchSummaryInfo.TempNumber = employeePunchSummary.TempNumber;
                            punchResult.EmployeePunchSummaryInfo.ManualOverride = employeePunchSummary.ManualOverride;
                            punchResult.EmployeePunchSummaryInfo.DeptOverride = employeePunchSummary.DeptOverride;
                            punchResult.EmployeePunchSummaryInfo.ShiftOverride = employeePunchSummary.ShiftOverride;
                            if (rosterEmployee == 0)
                            {
                                punchResult.EmployeePunchSummaryInfo.IsRosterEmployee = false;
                                punchResult.EmployeePunchSummaryInfo.EmployeeTicketID = currentEmployeeTicketId;
                                //LOAD the first ticket that is within the valid time frame one or more tickets
                                punchResult.EmployeePunchSummaryInfo.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                punchResult.EmployeePunchSummaryInfo.ClientRosterID = -1;
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ClientID = employeePunchSummary.ClientID;
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.LocationID = dr.GetInt32(dr.GetOrdinal("location_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.OfficeID = dr.GetInt32(dr.GetOrdinal("office_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.TicketDate = dr.GetDateTime(dr.GetOrdinal("ticket_date"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.TicketInstance = dr.GetInt32(dr.GetOrdinal("ticket_instance"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.TicketID = dr.GetInt32(dr.GetOrdinal("ticket_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            }
                            else
                            {
                                punchResult.EmployeePunchSummaryInfo.IsRosterEmployee = true;
                                punchResult.EmployeePunchSummaryInfo.ClientRosterID = currentClientRosterId;
                                punchResult.EmployeePunchSummaryInfo.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                                punchResult.EmployeePunchSummaryInfo.EmployeeTicketID = -1;
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ClientID = employeePunchSummary.ClientID;
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.LocationID = dr.GetInt32(dr.GetOrdinal("location_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                                int t = dr.GetOrdinal("alternate_start_time");
                                if( t>=0 )
                                    punchResult.EmployeePunchSummaryInfo.TicketInfo.AlternateShiftStartTime = dr.GetString(t);//JHM2011-09-09 see duplicate code 30 lines down
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                                punchResult.EmployeePunchSummaryInfo.EffectiveDateTime = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                                punchResult.EmployeePunchSummaryInfo.ExpirationDateTime = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                            }
                            returnList.Add(punchResult);

                            if (this.isPunchValid(employeePunchSummary, dr))
                            {
                                this.addPreviousPunch(dr, punchResult.EmployeePunchSummaryInfo);
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

            return returnList;
        }

        public ArrayList GetEmployeeRosterScheduleForPunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            ArrayList returnList = new ArrayList();
            EmployeePunchResult punchResult = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            int prevClientRosterId = 0;
            int currentClientRosterId = 0;

            //Get the previous day
            DateTime previousDate = employeePunchSummary.PunchDateTime.AddDays(-1);

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeTicketForPunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, employeePunchSummary.ClientID);
            dbSvc.AddInParameter(cw, "@tempNumber", DbType.String, employeePunchSummary.TempNumber);
            dbSvc.AddInParameter(cw, "@ticketEffectiveDateStart", DbType.DateTime, previousDate.ToString("MM/dd/yyyy 00:00:00"));
            dbSvc.AddInParameter(cw, "@ticketEffectiveDateEnd", DbType.DateTime, employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy 23:59:59.000"));
            dbSvc.AddInParameter(cw, "@clientRosterId", DbType.Int32, employeePunchSummary.ClientRosterID);
            dbSvc.AddInParameter(cw, "@punchTime", DbType.DateTime, employeePunchSummary.PunchDateTime);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        currentClientRosterId = dr.GetInt32(dr.GetOrdinal("client_roster_id"));

                        if (currentClientRosterId != prevClientRosterId)
                        {
                            punchResult = null;
                            prevClientRosterId = currentClientRosterId;
                        }


                        if (punchResult != null)
                        {
                            //add roster info to the previous punch list
                            if (this.isPunchValidNew(employeePunchSummary, dr))
                            {
                                this.addPreviousPunch(dr, punchResult.EmployeePunchSummaryInfo);
                            }
                        }
                        else
                        {
                            punchResult = new EmployeePunchResult();
                            punchResult.EmployeePunchSummaryInfo.BiometricResult = employeePunchSummary.BiometricResult;
                            punchResult.EmployeePunchSummaryInfo.ClientID = employeePunchSummary.ClientID;
                            punchResult.EmployeePunchSummaryInfo.UseExactTimes = employeePunchSummary.UseExactTimes;
                            punchResult.EmployeePunchSummaryInfo.CalculateWeeklyHours = employeePunchSummary.CalculateWeeklyHours;
                            punchResult.EmployeePunchSummaryInfo.PunchDateTime = employeePunchSummary.PunchDateTime;
                            punchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = employeePunchSummary.RoundedPunchDateTime;
                            punchResult.EmployeePunchSummaryInfo.TempNumber = employeePunchSummary.TempNumber;
                            punchResult.EmployeePunchSummaryInfo.ManualOverride = employeePunchSummary.ManualOverride;
                            punchResult.EmployeePunchSummaryInfo.DeptOverride = employeePunchSummary.DeptOverride;
                            punchResult.EmployeePunchSummaryInfo.ShiftOverride = employeePunchSummary.ShiftOverride;

                            punchResult.EmployeePunchSummaryInfo.IsRosterEmployee = true;
                            punchResult.EmployeePunchSummaryInfo.ClientRosterID = currentClientRosterId;
                            punchResult.EmployeePunchSummaryInfo.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                            punchResult.EmployeePunchSummaryInfo.EmployeeTicketID = -1;
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.ClientID = employeePunchSummary.ClientID;
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.LocationID = dr.GetInt32(dr.GetOrdinal("location_id"));
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            int t = dr.GetOrdinal("alternate_start_time");
                            if (t >= 0)
                                punchResult.EmployeePunchSummaryInfo.TicketInfo.AlternateShiftStartTime = dr.GetString(t);//JHM2011-09-09 see duplicate code 30 lines down
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                            punchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            punchResult.EmployeePunchSummaryInfo.EffectiveDateTime = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                            punchResult.EmployeePunchSummaryInfo.ExpirationDateTime = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                            returnList.Add(punchResult);

                            if (this.isPunchValidNew(employeePunchSummary, dr))
                            {
                                this.addPreviousPunch(dr, punchResult.EmployeePunchSummaryInfo);
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

            return returnList;
        }

        public EmployeePunchResult RecordEmployeeBiometricPunch(EmployeePunchResult employeePunchResult, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = employeePunchResult;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //IDataReader dr = null;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RecordEmployeePunchBiometricUpdateRoster);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, employeePunchResult.EmployeePunchSummaryInfo.TempNumber);
            dbSvc.AddInParameter(cw, "@employeeTicketID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID);
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@punchExceptionID", DbType.Int32, employeePunchResult.PunchException);
            dbSvc.AddInParameter(cw, "@punchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime);
            dbSvc.AddInParameter(cw, "@roundedPunchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime);
            dbSvc.AddInParameter(cw, "@createdBY", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@createdDateTime", DbType.DateTime, _helper.GetCSTCurrentDateTime());
            dbSvc.AddInParameter(cw, "@manualOverrideFlag", DbType.Boolean, employeePunchResult.EmployeePunchSummaryInfo.ManualOverride);
            dbSvc.AddInParameter(cw, "@biometricResult", DbType.Boolean, employeePunchResult.EmployeePunchSummaryInfo.BiometricResult);


            if (employeePunchResult.PunchException == 0 && employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime > employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }

            else if (employeePunchResult.PunchException == 0 && employeePunchResult.PunchType == Enums.PunchTypes.CheckIn && employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.Date > employeePunchResult.EmployeePunchSummaryInfo.EffectiveDateTime.Date && employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime >= employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, false);
            }

            if (employeePunchResult.PunchException == PunchExceptions.EmployeeNotAuthorized)
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, DBNull.Value);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, returnResult.EmployeePunchSummaryInfo.ExpirationDateTime);
            }

            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 100);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 150);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);

                if (employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID > 0 || employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID > 0)
                {
                    //get first Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeFirstName = (string)dbSvc.GetParameterValue(cw, "@FirstName");
                    //get Last Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeLastName = (string)dbSvc.GetParameterValue(cw, "@LastName");
                    int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                    if (outputVal == 1)
                    {
                        returnResult.DuplicatePunch = true;
                    }
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

            return returnResult;
        }

        public EmployeePunchResult RecordEmployeePunch(EmployeePunchResult employeePunchResult, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = employeePunchResult;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //IDataReader dr = null;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RecordEmployeePunchUpdateRoster);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, employeePunchResult.EmployeePunchSummaryInfo.TempNumber);
            dbSvc.AddInParameter(cw, "@employeeTicketID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID);
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@punchExceptionID", DbType.Int32, employeePunchResult.PunchException);
            dbSvc.AddInParameter(cw, "@punchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime);
            dbSvc.AddInParameter(cw, "@roundedPunchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime);
            dbSvc.AddInParameter(cw, "@createdBY", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@createdDateTime", DbType.DateTime, _helper.GetCSTCurrentDateTime());
            dbSvc.AddInParameter(cw, "@manualOverrideFlag", DbType.Boolean, employeePunchResult.EmployeePunchSummaryInfo.ManualOverride);
            dbSvc.AddInParameter(cw, "@biometricResult", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.BiometricResult);
            dbSvc.AddInParameter(cw, "@departmentOverride", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.DeptOverride);
            dbSvc.AddInParameter(cw, "@shiftOverride", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ShiftOverride);
            
            if (employeePunchResult.PunchException == 0 && employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime > employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }
            
            else if (employeePunchResult.PunchException == 0 && employeePunchResult.PunchType == Enums.PunchTypes.CheckIn && 
                employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.Date > employeePunchResult.EmployeePunchSummaryInfo.EffectiveDateTime.Date && 
                employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime >= employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, false);
            }

            if (employeePunchResult.PunchException == PunchExceptions.EmployeeNotAuthorized)
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, DBNull.Value);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, returnResult.EmployeePunchSummaryInfo.ExpirationDateTime);
            }

            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 100);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 150);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);

                if (employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID > 0 || employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID > 0)
                {
                    //get first Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeFirstName = (string)dbSvc.GetParameterValue(cw, "@FirstName");
                    //get Last Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeLastName = (string)dbSvc.GetParameterValue(cw, "@LastName");
                    int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                    if (outputVal == 1)
                    {
                        returnResult.DuplicatePunch = true;
                    }
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

            return returnResult;
        }

        public EmployeePunchResult RecordEmployeePunchSummary(EmployeePunchResult employeePunchResult, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = employeePunchResult;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            IDataReader dr = null;
            DateTime startDateTime = new DateTime(1, 1, 1);
            DateTime endDateTime = new DateTime(1, 1, 1);

            //calculate the start and end date
            endDateTime = _helper.GetShiftWeekEndingDate(employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime, employeePunchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime, employeePunchResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime);
            TimeSpan ts = new TimeSpan(6, 0, 0, 0);
            startDateTime = endDateTime.Subtract(ts);
            startDateTime = DateTime.Parse(startDateTime.ToString("MM/dd/yyyy") + " 00:00:00");
            endDateTime = DateTime.Parse(endDateTime.ToString("MM/dd/yyyy") + " 23:59:59").AddDays(1);

            //set up the params
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RecordEmployeePunchReturnSummary);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, startDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, endDateTime);
            dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, employeePunchResult.EmployeePunchSummaryInfo.TempNumber);
            dbSvc.AddInParameter(cw, "@employeeTicketID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID);
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@punchExceptionID", DbType.Int32, employeePunchResult.PunchException);
            dbSvc.AddInParameter(cw, "@punchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime);
            dbSvc.AddInParameter(cw, "@roundedPunchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime);
            dbSvc.AddInParameter(cw, "@createdBY", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@createdDateTime", DbType.DateTime, _helper.GetCSTCurrentDateTime());
            dbSvc.AddInParameter(cw, "@manualOverrideFlag", DbType.Boolean, employeePunchResult.EmployeePunchSummaryInfo.ManualOverride);
            dbSvc.AddInParameter(cw, "@departmentOverride", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.DeptOverride);
            dbSvc.AddInParameter(cw, "@shiftOverride", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ShiftOverride);

            if (employeePunchResult.PunchException == 0 && employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime > employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }
            
            
            if (employeePunchResult.PunchException == 0 && employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.Date > employeePunchResult.EmployeePunchSummaryInfo.EffectiveDateTime.Date && employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime.Date == employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.Date)
            {
                returnResult.EmployeePunchSummaryInfo.ExpirationDateTime = employeePunchResult.EmployeePunchSummaryInfo.ExpirationDateTime.AddDays(1);
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, true);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@updateExpirationDateTime", DbType.Boolean, false);
            }

            if (employeePunchResult.PunchException == PunchExceptions.EmployeeNotAuthorized)
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, DBNull.Value);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@expirationDateTime", DbType.DateTime, returnResult.EmployeePunchSummaryInfo.ExpirationDateTime);
            }


            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 100);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 150);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            try
            {
                dr = dbSvc.ExecuteReader(cw);
                
                if (employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID > 0 || employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID > 0)
                {
                    //determine the weekly summary
                    
                    //create the hours report
                    HoursReport hoursReport = new HoursReport();
                    hoursReport.EndDateTime = endDateTime;
                    hoursReport.StartDateTime = startDateTime;
                    hoursReport = _dbHelper.processHoursReport(dr, hoursReport);
                    if (hoursReport.EmployeeHistoryCollection.Count == 1)
                    {
                        EmployeeHistory history = (EmployeeHistory)hoursReport.EmployeeHistoryCollection[0];
                        returnResult.EmployeePunchSummaryInfo.CurrentWeeklyHours = history.TotalHours;
                    }
                    else
                    {
                        returnResult.EmployeePunchSummaryInfo.CurrentWeeklyHours = 0M;
                    }
                    //get first Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeFirstName = (string)dbSvc.GetParameterValue(cw, "@FirstName");
                    //get Last Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeLastName = (string)dbSvc.GetParameterValue(cw, "@LastName");
                    int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                    if (outputVal == 1)
                    {
                        returnResult.DuplicatePunch = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();
                dr.Dispose();

                cw.Dispose();
            }

            return returnResult;
        }

        public EmployeeDepartmentPunchResult RecordEmployeeDepartmentPunch(Department department, EmployeeDepartmentPunchResult employeePunchResult, IPrincipal userPrincipal)
        {
            EmployeeDepartmentPunchResult returnResult = employeePunchResult;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //IDataReader dr = null;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RecordEmployeeDepartmentPunch);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, employeePunchResult.EmployeePunchSummaryInfo.TempNumber);
            dbSvc.AddInParameter(cw, "@employeeTicketID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID);
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, department.DepartmentID);
            dbSvc.AddInParameter(cw, "@punchExceptionID", DbType.Int32, employeePunchResult.PunchException);
            dbSvc.AddInParameter(cw, "@punchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.PunchDateTime);
            dbSvc.AddInParameter(cw, "@roundedPunchDateTime", DbType.DateTime, employeePunchResult.EmployeePunchSummaryInfo.RoundedPunchDateTime);
            dbSvc.AddInParameter(cw, "@createdBY", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@createdDateTime", DbType.DateTime, _helper.GetCSTCurrentDateTime());
            dbSvc.AddInParameter(cw, "@manualOverrideFlag", DbType.Boolean, employeePunchResult.EmployeePunchSummaryInfo.ManualOverride);
            dbSvc.AddInParameter(cw, "@currentDepartmentID", DbType.Int32, employeePunchResult.CurrentDepartmentId);
            if (employeePunchResult.PunchType == Enums.PunchTypes.NoGeneralPunch)
            {
                dbSvc.AddInParameter(cw, "@generalPunchExists", DbType.Boolean, false);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@generalPunchExists", DbType.Boolean, true);
            }
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 100);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 150);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);

                if (employeePunchResult.EmployeePunchSummaryInfo.EmployeeTicketID > 0 || employeePunchResult.EmployeePunchSummaryInfo.ClientRosterID > 0)
                {
                    //get first Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeFirstName = (string)dbSvc.GetParameterValue(cw, "@FirstName");
                    //get Last Name
                    returnResult.EmployeePunchSummaryInfo.EmployeeLastName = (string)dbSvc.GetParameterValue(cw, "@LastName");
                    int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                    if (outputVal == 1)
                    {
                        returnResult.DuplicatePunch = true;
                    }
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

            return returnResult;
        }

        private void addPreviousPunch(IDataReader dr, EmployeePunchSummary employeePunchSummary)
        {
            int employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
            if ( employeePunchId > 0 )
            {
                EmployeePunchSummary previousPunch = new EmployeePunchSummary();
                previousPunch.EmployeePunchID = employeePunchId;
                previousPunch.PunchDateTime = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                if ( _dbHelper.fieldExistsInReader("rounded_punch_dt", dr ) )
                {
                    previousPunch.RoundedPunchDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                }
                previousPunch.TicketInfo.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                employeePunchSummary.PreviousPunches.Add(previousPunch);
            }
        }

        private bool isPunchValid(EmployeePunchSummary employeePunchSummary, IDataReader dr)
        {
            //check if the start/end time spans multiple days
            DateTime shiftStart = DateTime.Parse(employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy " + dr.GetString(dr.GetOrdinal("shift_start_time"))));
            DateTime shiftEnd = DateTime.Parse(employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy " + dr.GetString(dr.GetOrdinal("shift_end_time"))));
            bool validPunch = false;
            int employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
            if (employeePunchId > 0)
            {
                DateTime punchDT = dr.GetDateTime(dr.GetOrdinal("punch_dt"));

                //the valid shift start from a punch perspective
                //is 15 minutes prior to actual shift start
                //shiftStart = shiftStart.AddMinutes(-15);
                shiftStart = shiftStart.AddHours(-5);

                if (shiftEnd < shiftStart)
                {
                    //shift spans multiple days
                    //When current date + shift start time > current date/time
                    if (shiftStart > employeePunchSummary.PunchDateTime)
                    {
                        //make shift start date = current date - 1
                        shiftStart = shiftStart.AddDays(-1);
                        //leave the shift end date = current date
                    }
                    else
                    {
                        //when current date + shift start time < current date/time
                        //leave the shift start date = current date
                        //make shift end date = current date + 1
                        shiftEnd = shiftEnd.AddDays(1);
                    }
                }

                //shift does not span multiple days
                //so valid punches are punches recorded for the same day as punch date
                if (employeePunchId > 0)
                {
                    //validate that the punch is for the current day
                    if (punchDT >= shiftStart)
                    {
                        //TODO: May need to look at adding this rule.
                        //only valid if greater than or equal to 15 min before shift start
                        validPunch = true;
                    }
                }
            }
            return validPunch;
        }

        /* check whether punch falls between the start and end times for current shift employee is
         * punching in for.  If it does, this punch is for a break, or punching out */
        private bool isPunchValidNew(EmployeePunchSummary employeePunchSummary, IDataReader dr)
        {
            //check if the start/end time spans multiple days
            DateTime shiftStart = DateTime.Parse(employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy " + dr.GetString(dr.GetOrdinal("shift_start_time"))));
            DateTime shiftEnd = DateTime.Parse(employeePunchSummary.PunchDateTime.ToString("MM/dd/yyyy " + dr.GetString(dr.GetOrdinal("shift_end_time"))));

            bool validPunch = false;
            int employeePunchId = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
            if (employeePunchId > 0)
            {
                DateTime punchDT = dr.GetDateTime(dr.GetOrdinal("punch_dt"));

                //TODO: get time prior to shift start from client preferences
                //the valid shift start from a punch perspective
                //is 15 minutes prior to actual shift start
                shiftStart = shiftStart.AddMinutes(-46);

                if (shiftEnd < shiftStart)
                {
                    //shift spans multiple days
                    //When current date + shift start time > current date/time
                    if (shiftStart > employeePunchSummary.PunchDateTime)
                    {
                        //make shift start date = current date - 1
                        shiftStart = shiftStart.AddDays(-1);
                        //leave the shift end date = current date
                    }
                    else
                    {
                        //when current date + shift start time < current date/time
                        //leave the shift start date = current date
                        //make shift end date = current date + 1
                        shiftEnd = shiftEnd.AddDays(1);
                    }
                }

                //shift does not span multiple days
                //so valid punches are punches recorded for the same day as punch date
                if (employeePunchId > 0)
                {
                    //validate that the punch is for the current day
                    if (punchDT >= shiftStart)
                    {
                        //TODO: May need to look at adding this rule.
                        //only valid if greater than or equal to 15 min before shift start
                        validPunch = true;
                    }
                }
            }
            return validPunch;
        }
    }
}