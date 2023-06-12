using System;
using System.Data;
using System.Data.Common;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using log4net;
using MSIToolkit.Logging;
//using MSIToolkit.Logging;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class DaysWorkedReportDB
    {
        public DaysWorkedReportDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
         
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void CreateDaysWorkedReport(DaysWorkedReport dwr, IPrincipal userPrincipal)
        {
        }

        public RecruitPool GetRecruitPool(RecruitPool recruitPool, string id)
        {
            //if (log != null) log.Info("GetRecruitPool", "Start of function");
            RecruitPool returnInfo = recruitPool;
            DbCommand cw;
            //if (log != null) log.Info("GetRecruitPool", "Initialized RecruitPool");

            Database dbSvc = DatabaseFactory.CreateDatabase();
            //if (log != null) log.Info("GetRecruitPool", "Created database object");

            RecruitPoolItem savedItem = null;
            DateTime shiftStart = new DateTime(1, 1, 1);
            DateTime currentPunch = new DateTime(1, 1, 1);
            bool processedCheckIn = false;
            int oldDept = -1;
            int deptIdx = 0;
            //if (log != null) log.Info("GetRecruitPool", "Created various variables");
            
            if (recruitPool.ClientID == 8 /*|| true*/)
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetRecruitPoolFromRosters);
                //if (log != null) log.Info("GetRecruitPool", "Selected GetRecruitPoolFromRosters SP");
            }
            else
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetRecruitPool);
                //if (log != null) log.Info("GetRecruitPool", "Selected GetRecruitPool SP");
            }
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, recruitPool.ClientID);
            TimeSpan ts = new TimeSpan(05, 0, 0, 0);
            if (recruitPool.EndDateTime.Subtract(recruitPool.StartDateTime) > ts)
                recruitPool.StartDateTime = recruitPool.EndDateTime - ts;
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, recruitPool.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, recruitPool.EndDateTime);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, recruitPool.DepartmentID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, recruitPool.ShiftType);
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, recruitPool.LocationID);
            dbSvc.AddInParameter(cw, "@dnrClientID", DbType.Int32, recruitPool.DNRClientID);

            cw.CommandTimeout = 120;
            //if (log != null) log.Info("GetRecruitPool", "Initialized parameters for SP");

            //Random r = new Random();

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                //if (log != null) log.Info("GetRecruitPool", "Data reader executed");

                try
                {
                    while (dr.Read())
                    {
                        currentPunch = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        shiftStart = DateTime.Parse(currentPunch.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_start_time")));
                        int deptId = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                        string dept = dr.GetString(dr.GetOrdinal("punch_department_name"));
                        if (savedItem == null || savedItem.BadgeNumber != dr.GetString(dr.GetOrdinal("temp_number")))
                        {
                            if (savedItem != null)
                            {
                                if (savedItem.TotalDaysWorked < recruitPool.MinDays)
                                    recruitPool.RecruitPoolCollection.Remove(savedItem);
                            }
                            RecruitPoolItem myItem = new RecruitPoolItem();
                            myItem.BadgeNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            myItem.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            myItem.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            myItem.BadgeNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            myItem.DnrReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                            myItem.PhoneNum = dr.GetString(dr.GetOrdinal("phone_num"));
                            myItem.Addr = dr.GetString(dr.GetOrdinal("employee_address_1"));
                            myItem.Addr2 = dr.GetString(dr.GetOrdinal("employee_address_2"));
                            myItem.City = dr.GetString(dr.GetOrdinal("employee_city"));
                            myItem.State = dr.GetString(dr.GetOrdinal("employee_state"));
                            myItem.Zip = dr.GetString(dr.GetOrdinal("employee_zip"));
                            myItem.Email = dr.GetString(dr.GetOrdinal("email"));
                            myItem.Notes = dr.GetString(dr.GetOrdinal("data"));
                            //int shiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                            //RosterWS rws = new RosterWS();
                            //myItem.FingerprintsExist = rws.FingerprintsExist(myItem.BadgeNumber);
                            myItem.FingerprintsExist = "0";
                            if (myItem.Email.Equals("-1"))
                                myItem.Email = "(not available)";
                            /* start with a fresh department */
                            deptIdx = 0;
                            myItem.DeptId = deptId;
                            oldDept = deptId;
                            myItem.Depts.Add(dept);
                            myItem.DaysWorked.Add(0);
                            myItem.FirstPunch = currentPunch.ToString("MM/dd/yyyy");
                            processedCheckIn = true;
                            recruitPool.RecruitPoolCollection.Add(myItem);
                            savedItem = myItem;
                        }
                        else
                        {
                            if (!processedCheckIn)
                            {
                                savedItem.DaysWorked[deptIdx]++;
                                savedItem.TotalDaysWorked++;
                                processedCheckIn = true;
                            }
                            else
                            {
                                if (currentPunch > shiftStart.AddDays(1))
                                {
                                    savedItem.TotalDaysWorked++;
                                    savedItem.DaysWorked[deptIdx]++;
                                    processedCheckIn = true;
                                }
                                else
                                {
                                    processedCheckIn = false;
                                }
                            }
                        }
                    }
                    if (savedItem != null)
                    {
                        if (savedItem.TotalDaysWorked < recruitPool.MinDays)
                            recruitPool.RecruitPoolCollection.Remove(savedItem);
                    }
                    //if (log != null) log.Info("GetRecruitPool", "Loaded items into recruit pool");
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
            //if (log != null) log.Info("GetRecruitPool", "End of function");
            return returnInfo;
        }

        public DaysWorkedReport GetDaysWorkedAndDNRStatus(DaysWorkedReport daysworkedReport, string id, PerformanceLogger log = null)
        {
            if (log != null) log.Info("GetDaysWorkedAndDNRStatus", "Start of data access");
            DaysWorkedReport returnInfo = daysworkedReport;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            if (log != null) log.Info("GetDaysWorkedAndDNRStatus", "Database created");
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDaysWorkedFirstPunch);
            if ( id != null )
            {
                dbSvc.AddInParameter(cw, "@id", DbType.String, id);
            }
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, daysworkedReport.ClientID);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, daysworkedReport.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, daysworkedReport.EndDateTime.AddDays(1));
            cw.CommandTimeout = 120;
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                if (log != null) log.Info("GetDaysWorkedAndDNRStatus", "Query executed");
                try
                {
                    while (dr.Read())
                    {
                        DaysWorkedItem dwItem = new DaysWorkedItem();
                        dwItem.BadgeNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        dwItem.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        dwItem.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        dwItem.DnrReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                        dwItem.TotalDaysWorked = dr.GetInt32(dr.GetOrdinal("days_worked"));
                        dwItem.DaysWorked.Add(dr.GetInt32(dr.GetOrdinal("days_worked")));
                        dwItem.FirstPunch = dr.GetDateTime(dr.GetOrdinal("first_punch")).ToString("MM/dd/yyyy");
                        DateTime lastPunchDt = dr.GetDateTime(dr.GetOrdinal("last_punch"));
                        dwItem.LastPunch = lastPunchDt.ToString("MM/dd/yyyy");
                        dwItem.Depts.Add(dr.GetString(dr.GetOrdinal("department_name")));
                        dwItem.PunchCount = dr.GetInt32(dr.GetOrdinal("punch_count"));
                        //dwItem.EndOfBreak = daysworkedReport.StartDateTime.ToString("MM/dd/yyyy");
                        dwItem.EndOfBreak = dr.GetDateTime(dr.GetOrdinal("break_end")).ToString("MM/dd/yyyy");
                        if ( dwItem.TotalDaysWorked > dwItem.PunchCount / 2 && dwItem.PunchCount % 2 == 0 )
                        {
                            dwItem.TotalDaysWorked = dwItem.PunchCount / 2;
                        }
                        int shift = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        if (shift == 1)
                            dwItem.Shift = "1st Shift";
                        else if (shift == 2)
                            dwItem.Shift = "2nd Shift";
                        else if (shift == 3)
                            dwItem.Shift = "3rd Shift";
                        else
                            dwItem.Shift = "Unknown Shift";
                        if( lastPunchDt >= daysworkedReport.LastDayWorked )
                        {
                            returnInfo.DaysWorkedCollection.Add(dwItem);
                        }
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    dr.Dispose();
                    if (log != null) log.Info("GetDaysWorkedAndDNRStatus", "DaysWorkedReport DataReader disposed");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
                if (log != null) log.Info("GetDaysWorkedAndDNRStatus", "DaysWorkedReport DbCommand disposed");
            }
            return returnInfo;
        }
        public DaysWorkedReport GetDaysWorkedReport(DaysWorkedReport daysworkedReport, /*IPrincipal userPrincipal,*/ string id)
        {
            DaysWorkedReport returnInfo = daysworkedReport;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            DaysWorkedItem savedItem = null;
            DateTime shiftStart = new DateTime(1, 1, 1);
            DateTime currentPunch = new DateTime(1, 1, 1);
            bool processedCheckIn = false;
            int oldDept = -1;
            int deptIdx = 0;

            if (id == null)
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDaysWorkedReport);
            else
            {
                cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDaysWorkedReportSingleUser);
                dbSvc.AddInParameter(cw, "@id", DbType.String, id);
            }
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, daysworkedReport.ClientID);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, daysworkedReport.StartDateTime);
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, daysworkedReport.EndDateTime);
            cw.CommandTimeout = 120;
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        currentPunch = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        shiftStart = DateTime.Parse(currentPunch.ToString("MM/dd/yyyy ") + dr.GetString(dr.GetOrdinal("shift_start_time")));
                        int deptId = dr.GetInt32(dr.GetOrdinal("punch_department_id"));
                        string dept = dr.GetString(dr.GetOrdinal("punch_department_name"));
                        if (savedItem == null || savedItem.BadgeNumber != dr.GetString(dr.GetOrdinal("temp_number")))
                        {
                            DaysWorkedItem myItem = new DaysWorkedItem();
                            myItem.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            myItem.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            myItem.BadgeNumber = dr.GetString(dr.GetOrdinal("temp_number"));
                            myItem.DnrReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                            /* start with a fresh department */
                            deptIdx = 0;
                            myItem.DeptId = deptId;
                            oldDept = deptId;
                            myItem.Depts.Add(dept);
                            myItem.DaysWorked.Add(1);
                            myItem.FirstPunch = currentPunch.ToString("MM/dd/yyyy");
                            myItem.TotalDaysWorked = 1;
                            processedCheckIn = true;
                            daysworkedReport.DaysWorkedCollection.Add(myItem);
                            savedItem = myItem;
                        }
                        else
                        {
                            if (!processedCheckIn)
                            {
                                savedItem.DaysWorked[deptIdx]++;
                                savedItem.TotalDaysWorked++;
                                processedCheckIn = true;
                            }
                            else
                            {
                                if (currentPunch > shiftStart.AddDays(1))
                                {
                                    savedItem.TotalDaysWorked++;
                                    savedItem.DaysWorked[deptIdx]++;
                                    processedCheckIn = true;
                                }
                                else
                                {
                                    processedCheckIn = false;
                                }
                            }
                            if (deptId != oldDept)
                            {
                                oldDept = deptId;
                                String newDept = dr.GetString(dr.GetOrdinal("punch_department_name"));
                                Boolean found = false;
                                int i = 0;
                                for (i = 0; i < savedItem.Depts.Count; i++)
                                {
                                    if (newDept.Equals(savedItem.Depts[i]))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    savedItem.Depts.Add(dr.GetString(dr.GetOrdinal("punch_department_name")));
                                    savedItem.DaysWorked.Add(0);
                                }
                                deptIdx = i;
                            }
                        }
                    }
                    if (savedItem != null)
                    {
                        if (savedItem.TotalDaysWorked < daysworkedReport.MinDays)
                            daysworkedReport.DaysWorkedCollection.Remove(savedItem);
                    }
                    daysworkedReport.DaysWorkedCollection.Sort();
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

        public void ActivateEmployee( int clientID, string badgeNum )
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientDNR_ActivateEmployee);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            //badgeNum = badgeNum.Substring(2);
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, badgeNum);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
            }
            catch (Exception drEx)
            {
                throw drEx;
            }
            finally
            {
                cw.Dispose();
            }
        }

        public void DeactivateEmployee( int clientID, string badgeNum )
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientDNR_DeactivateEmployee);
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            cw.CommandTimeout = 120;    //10 minutes
            //badgeNum = badgeNum.Substring(2);
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, badgeNum);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
            }
            catch (Exception drEx)
            {
                throw drEx;
            }
            finally
            {
                cw.Dispose();
            }
        }

        /* necessary? */
        public int CheckForExistingRecords( int clientID, string badgeNum)
        {
            int returnInfo = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientDNR_CheckForExistingRecords);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            //badgeNum = badgeNum.Substring(2);
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, badgeNum);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        returnInfo++;   //just count up the records.  should only be 0 or 1
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
    }
}