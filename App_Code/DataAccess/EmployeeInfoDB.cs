using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using System.Collections;

/// <summary>
/// Summary description for ClientDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class EmployeeInfoDB
    {
        public EmployeeInfoDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();

        public int DeleteDnrRecord(string dnrRecord, string userId)
        {
            int result = 0;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDNR);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 3);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userId);
            dbSvc.AddInParameter(cw, "@clientDnrID", DbType.Int32, Convert.ToInt32(dnrRecord)); 
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 80);
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 80);
            try
            {
                result = dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return result;
        }
        public int SetDnr(string userName, string id, string client, string shift, string reason, string supervisor, string start, string loc)
        {
            int retVal = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDNR);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "action", DbType.Int32, 2);
            int clientID = Convert.ToInt32(client);
            if( clientID == 0 )
                dbSvc.AddInParameter(cw, "dnrAllClients", DbType.Boolean, "true");
            else
                dbSvc.AddInParameter(cw, "clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "userName", DbType.String, userName);
            int locID = Convert.ToInt32(loc);
            if( shift != null && shift.Length > 0 )
                dbSvc.AddInParameter(cw, "shiftID", DbType.Int32, Convert.ToInt32(shift));
            dbSvc.AddInParameter(cw, "DnrReason", DbType.String, reason);
            dbSvc.AddInParameter(cw, "aidentNumber", DbType.String, id);
            string[] dtS = start.Split('/');
            DateTime dt = new DateTime(Convert.ToInt32(dtS[2]), Convert.ToInt32(dtS[0]), Convert.ToInt32(dtS[1]));
            dbSvc.AddInParameter(cw, "date", DbType.DateTime, dt);
            dbSvc.AddInParameter(cw, "supervisor", DbType.String, supervisor);
            dbSvc.AddInParameter(cw, "locationID", DbType.String, locID);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 80);
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 80);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return retVal;
        }

        public List<ShiftData> GetClientShifts(string clientID)
        {
            List<ShiftData> list = new List<ShiftData>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDNR);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "action", DbType.Int32, 5);
            dbSvc.AddInParameter(cw, "clientID", DbType.Int32, Convert.ToInt32(clientID));
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 80);
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 80);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);                               
                try
                {
                    while (dr.Read())
                    {
                        ShiftData sd = new ShiftData();
                        sd.Desc = dr.GetString(dr.GetOrdinal("shift_desc"));
                        sd.ID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        sd.Employees = null;
                        list.Add(sd);
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
            return list;
        }

        public List<string> GetDNRReasons()
        {
            List<string> reasons = new List<string>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDNR);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 4);
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 80);
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 80);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        reasons.Add(dr.GetString(dr.GetOrdinal("dnr_reason")));
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
            return reasons;
        }

        public List<DNRInfo> GetEmployeeInfoByAident(string lookup, string clientID, string shiftID, string deptID, string start, string end, string locationID)
        {
            List<DNRInfo> retEmpList = new List<DNRInfo>();
            DNRInfo returnEmp;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDNR);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, lookup);
            if (clientID != null)
            {
                dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(clientID));
                if( locationID != null )
                    dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, Convert.ToInt32(locationID));
                if (shiftID != null)
                    dbSvc.AddInParameter(cw, "@shiftID", DbType.Int32, Convert.ToInt32(shiftID));
                if (deptID != null)
                {
                    if( deptID.IndexOf(',') >= 0 )
                    {
                        dbSvc.AddInParameter(cw, "@deptID", DbType.Int32, Convert.ToInt32(deptID.Substring(0, deptID.IndexOf(','))));
                    }
                    else
                    {
                        dbSvc.AddInParameter(cw, "@deptID", DbType.Int32, Convert.ToInt32(deptID));
                    }
                }
                dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, Convert.ToDateTime(start));
                dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, Convert.ToDateTime(end));
                dbSvc.AddInParameter(cw, "@action", DbType.Int32, 101);
            }
            else
            {
                dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1);
            }
            dbSvc.AddOutParameter(cw, "@LastName", DbType.String, 80);
            dbSvc.AddOutParameter(cw, "@FirstName", DbType.String, 80);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE Employee DNR Info
                        returnEmp = new DNRInfo();
                        returnEmp.ClientDnrID = dr.GetInt32(dr.GetOrdinal("client_dnr_id"));
                        returnEmp.AidentNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        returnEmp.Shift = dr.GetString(dr.GetOrdinal("shift_desc"));
                        returnEmp.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        returnEmp.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        returnEmp.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                        returnEmp.ClientId = dr.GetInt32(dr.GetOrdinal("client_id"));
                        returnEmp.DNRReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                        returnEmp.Supervisor = dr.GetString(dr.GetOrdinal("supervisor"));
                        returnEmp.LocationName = dr.GetString(dr.GetOrdinal("location_name"));
                        DateTime dt = dr.GetDateTime(dr.GetOrdinal("dnr_date"));
                        string dtM = "" + dt.Month;
                        if (dtM.Length < 2) dtM = "0" + dtM;
                        string dtD = "" + dt.Day;
                        if (dtD.Length < 2) dtD = "0" + dtD;
                        returnEmp.StartDate = dtM + "/" + dtD + "/" + dt.Year;
                        retEmpList.Add(returnEmp);
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
                        if (retEmpList.Count == 0)
                        {
                            returnEmp = new DNRInfo();
                            returnEmp.LastName = cw.Parameters["@LastName"].Value.ToString();
                            returnEmp.FirstName = cw.Parameters["@FirstName"].Value.ToString();
                            retEmpList.Add(returnEmp);
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

            return retEmpList;
        }

    }
}