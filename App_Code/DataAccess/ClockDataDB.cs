using Microsoft.Practices.EnterpriseLibrary.Data;
using PunchClock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClockDataDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class ClockDataDB
    {
        public ClockDataDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int InsertClockData(PunchClockResponse resp, List<Record> records)
        {
            int recordsAdded = 0;

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
            tvp.Columns.Add(new DataColumn("time"));
            tvp.Columns.Add(new DataColumn("path"));
            tvp.Columns.Add(new DataColumn("personId"));
            tvp.Columns.Add(new DataColumn("punchDt"));
            tvp.Columns.Add(new DataColumn("state"));
            tvp.Columns.Add(new DataColumn("type"));
            tvp.Columns.Add(new DataColumn("deviceKey"));
            foreach (var item in records)
            {
                DateTime dt = PunchClockData.ClockTicksToTime(item.Time);
                tvp.Rows.Add(item.Time, item.Path, item.PersonId, dt.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss.fff"), Convert.ToBoolean(item.State), item.type, resp.DeviceKey);
            }

            Database dbSvc = DatabaseFactory.CreateDatabase();
            SqlCommand cw = new SqlCommand(MSINetStoredProcs.InsertClockData);
            cw.CommandType = CommandType.StoredProcedure;

            SqlParameter tvparam = cw.Parameters.AddWithValue("@data", tvp);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if( dr.Read())
                    {
                        string deviceKey = dr.GetString(dr.GetOrdinal("deviceKey"));
                        long time = dr.GetInt64(dr.GetOrdinal("time"));

                        if( deviceKey.Equals(resp.DeviceKey) )
                        {
                            resp.Success = true;
                            resp.LastPunchDt = time;
                        }
                        else
                        {
                            resp.Success = false;
                            resp.LastPunchDt = -1;
                            resp.ErrorMsg = "DeviceKey does not match!";
                        }
                    }
                }
                catch(Exception ex)
                {
                    resp.ErrorMsg = ex.ToString();
                    //throw ex;
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
            return recordsAdded;
        }
    }
}
 