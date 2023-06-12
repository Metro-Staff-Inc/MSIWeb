using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using OpenWebServices;
using System.Data;
using System.Web.Security;
using MSI.Web.MSINet.BusinessEntities;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Configuration;
//using MSIToolkit.Logging;
using MSI.Web.MSINet.Common;

/// <summary>
/// Summary description for DailyDispatchDB
/// </summary>

namespace MSI.Web.MSINet.DataAccess
{
    public class DailyDispatchDB
    {
        public DailyDispatchDB()
        {
        }
        //PerformanceLogger log = new PerformanceLogger("AdoNetAppender");
        public string UpdateDailyDispatchData(string value)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            String ret = "Success!";
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDailyDispatch);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@values", DbType.String, value);
            try
            {
                Object o = dbSvc.ExecuteScalar(cw);
            }
            catch(Exception e)
            {
                ret = "ERROR: " + e;
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }
        public List<DailyDispatchInfo> getDailyDispatchData(DateTime dispatchDt, string officePrefix, int shiftType, bool weeklyReport)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();

            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDailyDispatch);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@dispatchDt", DbType.DateTime, dispatchDt);
            dbSvc.AddInParameter(cw, "@officePrefix", DbType.String, officePrefix);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);
            dbSvc.AddInParameter(cw, "@weeklyReport", DbType.Boolean, weeklyReport);
            IDataReader dr = null;
            List<DailyDispatchInfo> data = new List<DailyDispatchInfo>();

            //log.Info("DailyDispatchDB", "Retrieving Dispatch Daily Data");
            DailyDispatchInfo d = null;
            try
            {
                dr = dbSvc.ExecuteReader(cw);
                while (dr.Read())
                {
                    d = new DailyDispatchInfo();
                    d.clientId = dr.GetInt32(dr.GetOrdinal("client_id"));
                    d.createdBy = dr.GetString(dr.GetOrdinal("created_by"));
                    d.totSent = dr.GetInt32(dr.GetOrdinal("tot_sent"));
                    DateTime dt = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                    d.officeName = dr.GetString(dr.GetOrdinal("office_name"));
                    d.createdDt = dt.ToString("yyyy-MM-dd");
                    dt = dr.GetDateTime(dr.GetOrdinal("dispatch_dt"));
                    d.dispatchDt = dt.ToString("yyyy-MM-dd");
                    d.extras = dr.GetInt32(dr.GetOrdinal("extras"));
                    d.notes = dr.GetString(dr.GetOrdinal("notes"));
                    d.officeId = dr.GetInt32(dr.GetOrdinal("office_id"));
                    d.officeCd = dr.GetString(dr.GetOrdinal("office_cd"));
                    d.regs = dr.GetInt32(dr.GetOrdinal("regs"));
                    d.shiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                    d.tempsOrdered = dr.GetInt32(dr.GetOrdinal("temps_ordered"));
                    d.tempsSent = dr.GetInt32(dr.GetOrdinal("temps_sent"));
                    d.unfilled = dr.GetInt32(dr.GetOrdinal("unfilled"));
                    d.shiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                    d.transported = dr.GetInt32(dr.GetOrdinal("transported"));
                    data.Add(d);
                }
            }
            catch (Exception e)
            {
                //log.Info("GetDailyDispatchData", e.ToString() + "\n" + d.ToString());
                Console.WriteLine(e);
            }
            finally
            {
                cw.Dispose();
                if (dr != null)
                {
                    dr.Dispose();
                }
            }
            return data;
        }
    }
}