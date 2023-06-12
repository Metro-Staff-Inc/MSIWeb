using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using OpenWebServices;
using System.Data;
using System.Web.Security;
using ClientWebServices;
using System.Data.SqlClient;
//using MSI.Web.MSINet.BusinessEntities;
//using MSI.Web.MSINet.BusinessEntities;

/// <summary>
/// Summary description for OpenDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class OpenDB
    {
        public OpenDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string CreateSuncastId(string id)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.CreateSuncastId);
            cw.CommandTimeout = 10;    //10 seconds
            dbSvc.AddInParameter(cw, "@aidentID", DbType.String, id);
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, 8);
            dbSvc.AddInParameter(cw, "@lastUpdatedBy", DbType.String, "Webtrax");
            dbSvc.AddOutParameter(cw, "@clientTempNumber", DbType.String, 80);
/*
 * 
 * 		@clientID int,
 *		@aidentID varchar(24),
 *		@lastUpdatedBy	varchar(50)
 *
 */
            string retStr = "";
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                retStr = (String)dbSvc.GetParameterValue(cw, "@clientTempNumber");
            }
            catch (Exception ex)
            {
                retStr = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }

        public List<SuncastInfo> SuncastId(string id)
        {
            List<SuncastInfo> list = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetSuncastId);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@aident", DbType.String, id);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        if (list == null)
                            list = new List<SuncastInfo>();
                        SuncastInfo s = new SuncastInfo();
                        s.AidentNum = dr.GetString(dr.GetOrdinal("aident_number"));
                        s.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        s.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        s.SuncastId = Convert.ToString(dr.GetDecimal(dr.GetOrdinal("client_temp_number")));
                        s.Msg = "";
                        list.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    SuncastInfo s = new SuncastInfo();
                    s.Msg = ex.ToString();
                    list.Add(s);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();
                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                SuncastInfo s = new SuncastInfo();
                s.Msg = ex.ToString();
                list.Add(s);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }

        public string SetSupervisor(BusinessEntities.SupervisorParams info) {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.SetSupervisor);
            cw.CommandTimeout = 10;
            dbSvc.AddInParameter(cw, "@userId", DbType.Guid, info.UserId);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, info.DepartmentId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, info.ShiftType);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, info.ClientId);

            int lineCount = -1;
            try
            {
                lineCount = dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            if (lineCount >= 0)
            {
                return "Success";
            }
            return "Error";
        }
        public string DepartmentViewHide(BusinessEntities.UserDepartment info)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.DepartmentViewHide);

            cw.CommandTimeout = 10;    //10 seconds
            dbSvc.AddInParameter(cw, "@clientMembershipId", DbType.Int32, info.ClientMembershipId);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, info.DepartmentId);
            dbSvc.AddInParameter(cw, "@hide", DbType.Boolean, info.Hide);
            int lineCount = -1;
            try
            {
                lineCount = dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.ToString();
                     
            }
            finally
            {
                cw.Dispose();
            }
            if( lineCount >= 0 )
            {
                return "Success";
            }
            return "Error";
        }

        public List<MSI.Web.MSINet.BusinessEntities.User> GetAllUsers()
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            List<MSI.Web.MSINet.BusinessEntities.User> retUsers = new List<MSI.Web.MSINet.BusinessEntities.User>();
            foreach (MembershipUser user in users)
            {
                MSI.Web.MSINet.BusinessEntities.User u = new MSI.Web.MSINet.BusinessEntities.User();
                u.Email = user.Email;
                u.UserName = user.UserName;
                u.LastActive = user.LastActivityDate.ToString("yyyy/MM/dd hh:mm:ss");
                retUsers.Add(u);
            }
            return retUsers;
        }
        public List<MSI.Web.MSINet.BusinessEntities.User> GetUsersByClient(int clientId)
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            List<MSI.Web.MSINet.BusinessEntities.User> retUsers = new List<MSI.Web.MSINet.BusinessEntities.User>();

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.GetUsersByClient);
            cw.CommandTimeout = 10;    //10 seconds
            dbSvc.AddInParameter(cw, "clientId", DbType.Int32, clientId);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        MSI.Web.MSINet.BusinessEntities.User user = new MSI.Web.MSINet.BusinessEntities.User();
                        user.UserID = dr.GetGuid(dr.GetOrdinal("UserId"));
                        user.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                        user.ClientMembershipId = dr.GetInt32(dr.GetOrdinal("client_membership_id"));
                        retUsers.Add(user);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return retUsers;
        }
        public bool SetDefaultClient(String userName, int clientId)
        {
            Boolean success = true;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetDefaultClient);
            cw.CommandTimeout = 10;    //10 minutes
            dbSvc.AddInParameter(cw, "clientId", DbType.Int32, clientId);
            dbSvc.AddInParameter(cw, "userName", DbType.String, userName);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch(Exception ex)
            {
                success = false;
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return success;
        }
        public List<String> GetUsersRoles(String userName)
        {
            List<String> roles = new List<string>(Roles.GetRolesForUser(userName));
            return roles;
        }
        public List<String> GetAllRoles()
        {
            List<String> roles = new List<String>(Roles.GetAllRoles());
            return roles;
        }
        public String ALithoUpdateRoles(String strinp)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateEmployeeRole);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@inpStr", DbType.String, strinp);
            int rows = 0;
            try
            {
                rows = dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return "Number of updated records: " + rows;
        }
        public List<RoleInfo> ALithoRoles(DateTime start, DateTime end)
        {
            List<RoleInfo> list = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetALithoInfo);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, start);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, end);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        if (list == null)
                            list = new List<RoleInfo>();
                        RoleInfo r = new RoleInfo();
                        r.Id = dr.GetString(dr.GetOrdinal("aident_number"));
                        r.DayOff = dr.GetInt32(dr.GetOrdinal("day_off"));
                        r.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        r.DnrReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                        if (r.DnrReason != null && r.DnrReason.Length > 0)
                            r.Dnr = true;
                        else
                            r.Dnr = false;
                        r.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        r.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        r.Notes = dr.GetString(dr.GetOrdinal("notes"));
                        r.Rating = dr.GetInt32(dr.GetOrdinal("rating"));
                        r.Role = dr.GetString(dr.GetOrdinal("role"));
                        r.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        r.Vip = dr.GetBoolean(dr.GetOrdinal("vip"));
                        r.CrossTrained = dr.GetString(dr.GetOrdinal("cross_trained"));
                        string temp = dr.GetString(dr.GetOrdinal("reg"));
                        if (temp.Equals("false"))
                        {
                            r.Vip = false;
                        }
                        else
                        {
                            r.Vip = true;
                        }
                        list.Add(r);
                    }
                }
                catch (Exception ex)
                {
                    
                    throw (ex);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }

        public List<PunchData> Punches(DateTime startDate, DateTime endDate, int clientId)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPunches);
            cw.CommandTimeout = 120;
            List<PunchData> list = new List<PunchData>();

            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDate);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        DateTime dt = DateTime.Now;
                        if (list == null)
                            list = new List<PunchData>();

                        PunchData pd = new PunchData();
                        pd.ReportDate = dt;
                        pd.Id = dr.GetString(dr.GetOrdinal("aident_number"));
                        pd.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        pd.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        pd.PunchExact = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        pd.PunchRound = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                        pd.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        pd.DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id"));
                        pd.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                        pd.ClientId = dr.GetInt32(dr.GetOrdinal("client_id"));
                        pd.ShiftName = dr.GetString(dr.GetOrdinal("shift_desc"));
                        pd.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        pd.CreatedBy = dr.GetString(dr.GetOrdinal("created_by"));
                        pd.CreatedDate = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        pd.Overnight = Convert.ToBoolean(dr.GetInt32(dr.GetOrdinal("tracking_multi_day")));
                        pd.PayRate = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("pay_rate")));
                        pd.BreakTime = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("break_time")));
                        pd.StartDate = dr.GetDateTime(dr.GetOrdinal("start_dt"));
                        pd.EndDate = dr.GetDateTime(dr.GetOrdinal("end_dt"));
                        pd.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start"));
                        pd.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end"));
                        pd.MarkUp = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("multiplier")));
                        list.Add(pd);
                    }
                }
                catch (Exception ex)
                {
                    
                    throw (ex);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }

        public List<MSI.Web.MSINet.BusinessEntities.FirstPunchDnr> getFirstPunchDnr(int clientId, DateTime endDate)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDnrFirstPunch);
            cw.CommandTimeout = 120;
            List<MSI.Web.MSINet.BusinessEntities.FirstPunchDnr> list = null;

            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            DateTime startDt = endDate.AddDays(-7);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDt);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        if (list == null)
                            list = new List<MSI.Web.MSINet.BusinessEntities.FirstPunchDnr>();
                        MSI.Web.MSINet.BusinessEntities.FirstPunchDnr fpd = new MSI.Web.MSINet.BusinessEntities.FirstPunchDnr();
                        fpd.FirstPunch = dr.GetBoolean(dr.GetOrdinal("first_punch"));
                        fpd.Aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        fpd.Dnr = dr.GetDateTime(dr.GetOrdinal("dnr_date")) > startDt;
                        list.Add(fpd);
                    }
                }
                catch (Exception ex)
                {
                    
                    throw (ex);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }
        public List<MSI.Web.MSINet.BusinessEntities.PunchException>
                RetrievePunchExceptions(DateTime startDate, DateTime endDate)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RetrievePunchExceptions);
            cw.CommandTimeout = 60;
            List<MSI.Web.MSINet.BusinessEntities.PunchException> list = 
                            new List<MSI.Web.MSINet.BusinessEntities.PunchException>();

            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDate);
            /* for now, client is collection of Weber clients */
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        /*
                         * 	SELECT  epe.badge_number, e.last_name, e.first_name, epe.created_by AS 'clock name', 
		                    epe.punch_dt, ISNULL(epe.client_roster_id,0) AS 'Roster Id', epe.punch_exception_id
                        */
                        MSI.Web.MSINet.BusinessEntities.PunchException pe =
                            new MSI.Web.MSINet.BusinessEntities.PunchException();
                        pe.BadgeNumber = dr.GetString(dr.GetOrdinal("badge_number"));
                        pe.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        pe.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        pe.CreatedBy = dr.GetString(dr.GetOrdinal("clock name"));
                        pe.PunchDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        pe.ClientRosterId = dr.GetInt32(dr.GetOrdinal("Roster Id"));
                        pe.PunchExceptionID = dr.GetInt32(dr.GetOrdinal("punch_exception_id"));
                        list.Add(pe);
                    }
                }
                catch (Exception ex)
                {

                    throw (ex);
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }
        public List<PunchData> RetrievePunches(DateTime date)
        {

            DataTable tvp = new DataTable();
            //d.deviceKey, d.personId, d.time, d.state, d.type, d.punchDt
            /*
	[time] [bigint] NOT NULL,
	[path] [varchar](max) NOT NULL,
	[personId] [varchar](max) NOT NULL,
	[punchDt] [datetime] NOT NULL,
	[state] [bit] NOT NULL,
	[type] [int] NOT NULL,
	[deviceKey] [varchar](max) NOT NULL
            */
            tvp.Columns.Add(new DataColumn("ClientId"));
            tvp.Rows.Add(181);
            tvp.Rows.Add(92);
            tvp.Rows.Add(384);
            tvp.Rows.Add(340);

            Database dbSvc = DatabaseFactory.CreateDatabase();
            SqlCommand cw = new SqlCommand(MSINetStoredProcs.RetrievePunches);
            cw.CommandType = CommandType.StoredProcedure;
            SqlParameter tvparam = cw.Parameters.AddWithValue("@clientList", tvp);
            tvparam = cw.Parameters.AddWithValue("@startDate", date);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        PunchData pd = new PunchData();
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
            return null;
        }
    }
}