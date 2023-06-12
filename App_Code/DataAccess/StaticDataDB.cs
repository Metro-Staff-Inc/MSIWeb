using System;
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
    public class StaticDataDB
    {
        public StaticDataDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
         

        private DataAccessHelper _dbHelper = new DataAccessHelper();

        public Hashtable GetPunchExceptions()
        {
            Hashtable punchExceptions = new Hashtable();
            PunchException punchException = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPunchExceptions);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE Exception
                        punchException = new PunchException(dr.GetInt32(dr.GetOrdinal("punch_exception_id")), dr.GetString(dr.GetOrdinal("punch_exception_message")));
                        punchExceptions.Add(punchException.PunchExceptionID, punchException);
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

            return punchExceptions;
        }

        public ArrayList GetPunchMaintenanceReasons()
        {
            ArrayList punchMaintenanceReasons = new ArrayList();
            PunchMaintenanceReason punchMaintenanceReason = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPunchMaintenanceReasons);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE Reason
                        punchMaintenanceReason = new PunchMaintenanceReason(dr.GetInt32(dr.GetOrdinal("punch_maintenance_reason_id")), dr.GetString(dr.GetOrdinal("punch_maintenance_reason_desc")));
                        punchMaintenanceReasons.Add(punchMaintenanceReason);
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

            return punchMaintenanceReasons;
        }

        public ArrayList GetMinimumWageHistory()
        {
            ArrayList minWageHistory = new ArrayList();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetMinimumWageHistory);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE min wage history
                        minWageHistory.Add ( new MinimumWage(dr.GetInt32(dr.GetOrdinal("minimum_wage_id")), dr.GetDouble(dr.GetOrdinal("minimum_wage_amount")), dr.GetDateTime(dr.GetOrdinal("effective_dt")), dr.GetDateTime(dr.GetOrdinal("expiration_dt"))));
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

            return minWageHistory;
        }

    }
}