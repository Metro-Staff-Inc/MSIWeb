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
/// Summary description for OpenDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class TransportationDB
    {
        public TransportationDB()
        {
        }

        //PerformanceLogger log = new PerformanceLogger("AdoNetAppender");

        public Dictionary<string, Vehicle> getVehicleUseInfo(DateTime start, DateTime end)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();

            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetVehicleUseInfo);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, start);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, end);
            IDataReader dr = null;
            Dictionary<string, Vehicle> vehicles = new Dictionary<string, Vehicle>();

            List<TransportationPunch> list = new List<TransportationPunch>();
            TransportationPunch tOld = null;
            TimeSpan ts = new TimeSpan(0, 15, 0);
            try
            {
                dr = dbSvc.ExecuteReader(cw);
                //vehicle_num model   make num_passengers  office_name employee_punch_id   aident_number last_name   first_name client_name client vehicle_id  ride_date latitude    longitude version_id  punch_dt
                //employee_punch_id   aident_number last_name   first_name client_name client vehicle_id  ride_date latitude    longitude version_id  punch_dt
                while (dr.Read())
                {
                    TransportationPunch t = new TransportationPunch();
                    t.vehicleId = dr.GetString(dr.GetOrdinal("vehicle_id"));
                    //int p = dr.GetOrdinal("in_fleet");
                    //log.Info("Boolean", "" + p);
                    Boolean inFleet = Convert.ToBoolean(dr.GetString(dr.GetOrdinal("in_fleet")));

                    if (!vehicles.ContainsKey(t.vehicleId))
                    {
                        Vehicle v = new Vehicle();
                        v.inFleet = inFleet;
                        v.vehicleId = t.vehicleId;
                        v.makeModel = dr.GetString(dr.GetOrdinal("make_model"));
                        v.numPassengers = dr.GetInt32(dr.GetOrdinal("num_passengers"));
                        v.office = dr.GetString(dr.GetOrdinal("office_name"));
                        v.fleetMaticsId = dr.GetString(dr.GetOrdinal("fleet_matics_id"));
                        v.transportList = new List<TransportationPunch>();
                        vehicles[v.vehicleId] = v;
                    }

                    t.aident = dr.GetString(dr.GetOrdinal("aident_number"));
                    if( !t.aident.Equals("UNKNOWN"))
                    {
                        t.rideDate = dr.GetDateTime(dr.GetOrdinal("ride_date"));
                        //t.latitude = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("latitude")));
                        //t.longitude = Convert.ToDouble(dr.GetDecimal(dr.GetOrdinal("longitude")));
                        t.firstName = dr.GetString(dr.GetOrdinal("first_name"));
                        t.lastName = dr.GetString(dr.GetOrdinal("last_name"));
                        t.client = dr.GetString(dr.GetOrdinal("client"));
                        t.versionId = dr.GetString(dr.GetOrdinal("version_id"));
                        //t.punchDt = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        t.driverName = dr.GetString(dr.GetOrdinal("driver_name"));
                        if (tOld == null ||
                            (!t.aident.Equals(tOld.aident)) ||
                            (t.aident.Equals(tOld.aident) && ((t.rideDate - tOld.rideDate).Ticks > ts.Ticks)))
                        {
                            vehicles[t.vehicleId].transportList.Add(t);
                        }
                        tOld = t;
                    }
                }
            }
            catch (Exception e)
            {
                //log.Info("GetVehicleUseInfo", e.ToString());
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
            return vehicles;
        }

        public List<DriverData> getDailyDriverData(DateTime date)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();

            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDailyDriverData);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@rideDate", DbType.DateTime, date);
            IDataReader dr = null;
            List<DriverData> drivers = new List<DriverData>();

            //log.Info("TransportationDB", "Retrieving Driver Data");

            try
            {
                dr = dbSvc.ExecuteReader(cw);
                while (dr.Read())
                {
                    DriverData d = new DriverData();
                    d.passengerCount = dr.GetInt32(dr.GetOrdinal("passenger_count"));
                    //log.Info("PC", "" + d.passengerCount);
                    d.firstName = dr.GetString(dr.GetOrdinal("first_name"));
                    d.lastName = dr.GetString(dr.GetOrdinal("last_name"));
                    d.officeId = dr.GetInt32(dr.GetOrdinal("office_id"));
                    //log.Info("O_id", "" + d.officeId);
                    d.officeName = dr.GetString(dr.GetOrdinal("office_name"));
                    d.vehicleId = Convert.ToInt32(dr.GetString(dr.GetOrdinal("vehicle_id")));
                    //log.Info("V_id", "" + d.vehicleId);
                    d.rideDate = dr.GetDateTime(dr.GetOrdinal("ride_date"));
                    d.driverId = dr.GetString(dr.GetOrdinal("driver_id"));
                    drivers.Add(d);
                }
            }
            catch (Exception e)
            {
                //log.Info("GetDailyDriver", e.ToString());
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
            return drivers;
        }

        public string UpdateTransportInfoOld(string rides)
        {
            //Console.WriteLine(rides);

            DataTable dt = new DataTable();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //('28', '2018-05-14 05:30:15', '58854', '41.8832346', '-88.1947795', '30','00.01.000'),('29', '2018-05-14 05:30:22', '244536', '41.8832346', '-88.1947795', '30','00.01.000')
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.InsertTransportation);
            cw.CommandTimeout = 120;    //10 minutes
            int startParen = rides.IndexOf('(');
            int endParen = rides.IndexOf(')');
            if (endParen < 0 || startParen < 0) return "error in upload data";
            /* count the commas */
            int numCommas = rides.Substring(startParen, endParen + 1).Split(',').Length - 1;
            if (numCommas <= 6)
            {
                dbSvc.AddInParameter(cw, "@version", DbType.Int32, 1);
            }
            else
            {
                /* new version has additional attributes in transport table */
                dbSvc.AddInParameter(cw, "@version", DbType.Int32, 2);
            }

            dbSvc.AddInParameter(cw, "@query", DbType.String, rides);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            String ret = " rows updated";
            try
            {
                int rows = Convert.ToInt32(dbSvc.ExecuteNonQuery(cw));
                int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                ret = outputVal + ret;
            }
            catch (Exception ex)
            {
                //throw (ex);
                ret = "error: " + ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }

        public string UpdateTransportInfo(string rides)
        {
            //Console.WriteLine(rides);

            DataTable dt = new DataTable();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //('28', '2018-05-14 05:30:15', '58854', '41.8832346', '-88.1947795', '30','00.01.000'),('29', '2018-05-14 05:30:22', '244536', '41.8832346', '-88.1947795', '30','00.01.000')
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.InsertTransportation);
            cw.CommandTimeout = 120;    //10 minutes
            int startParen = rides.IndexOf('(');
            int endParen = rides.IndexOf(')');
            if (endParen < 0 || startParen < 0) return "error in upload data";
            /* count the commas */
            int numCommas = rides.Substring(startParen, endParen + 1).Split(',').Length-1;
            if( numCommas <= 5 )
            {
                dbSvc.AddInParameter(cw, "@version", DbType.Int32, 3);
            }
            else if( numCommas <= 6 )
            {
                dbSvc.AddInParameter(cw, "@version", DbType.Int32, 1);
            }
            else
            {
                /* new version has additional attributes in transport table */
                dbSvc.AddInParameter(cw, "@version", DbType.Int32, 2);
            }
            
            dbSvc.AddInParameter(cw, "@query", DbType.String, rides);
            dbSvc.AddOutParameter(cw, "@outputVal", DbType.Int32, 4);

            String ret = " rows added";
            try
            {
                int rows = Convert.ToInt32(dbSvc.ExecuteNonQuery(cw));
                int outputVal = Convert.ToInt32(dbSvc.GetParameterValue(cw, "@outputVal"));
                ret = outputVal + ret;
            }
            catch (Exception ex)
            {
                //throw (ex);
                ret = "error: " + ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }
        public List<TransportationPunch> getTransportationInfo(DateTime start, DateTime end)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();

            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetTransportationInfo);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, start);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, end);
            IDataReader dr = null;
            List<TransportationPunch> list = new List<TransportationPunch>();
            TransportationPunch tOld = null;
            TimeSpan ts = new TimeSpan(0, 15, 0);
            try
            {
                int count = 0;
                dr = dbSvc.ExecuteReader(cw);
                while (dr.Read())
                {
                    count++;
                    TransportationPunch t = new TransportationPunch();
                    t.swipeCount = 1;
                    t.transportationID = dr.GetInt32(dr.GetOrdinal("transportation_id"));
                    t.aident = dr.GetString(dr.GetOrdinal("aident_number"));
                    t.rideDate = dr.GetDateTime(dr.GetOrdinal("ride_date"));
                    t.firstName = dr.GetString(dr.GetOrdinal("first_name"));
                    t.lastName = dr.GetString(dr.GetOrdinal("last_name"));
                    t.client = dr.GetString(dr.GetOrdinal("client"));
                    t.rosterClient = dr.GetString(dr.GetOrdinal("roster_client"));
                    t.vehicleId = dr.GetString(dr.GetOrdinal("vehicle_id"));
                    t.fleetMaticsId = dr.GetString(dr.GetOrdinal("fleet_matics_id"));
                    t.versionId = dr.GetString(dr.GetOrdinal("version_id"));
                    t.dispatch = dr.GetString(dr.GetOrdinal("dispatch"));
                    t.driverName = dr.GetString(dr.GetOrdinal("driver_name"));
                    if(t.driverName.Trim().Equals(","))
                    {
                        t.driverName = "UNKNOWN";
                    }
                    if (t.firstName.Equals("UNKNOWN") || t.lastName.Equals("UNKNOWN"))
                    {
                        //log.Info("Transport", "Unknown name " + t.aident);
                        continue;
                    }
                    /* no punch - maybe roster has the client */
                    //if (t.client.Equals("UNKNOWN"))
                    //{
                    //    t.client = t.rosterClient;
                    //}
                    /* multiple client rosters, create one record */
                    if (tOld != null &&
                        tOld.aident.Equals(t.aident) &&
                        tOld.rideDate.Ticks == t.rideDate.Ticks &&
                        !tOld.rosterClient.Contains(t.rosterClient) &&
                        !t.rosterClient.Equals("UNKNOWN"))
                    {
                        tOld.rosterClient += " / " + t.rosterClient;
                        //log.Info("Transport", "Multiple clients: " + tOld.transportationID + ", " + t.transportationID);
                        continue;
                    }

                    /* add punch, but if same employee, vehicle, and day just increment swipe count */
                    if (tOld == null ||
                        (!t.aident.Equals(tOld.aident)) ||
                        (t.aident.Equals(tOld.aident) && ((t.rideDate - tOld.rideDate).Ticks > ts.Ticks || t.vehicleId != tOld.vehicleId)))
                    {
                        list.Add(t);
                        tOld = t;
                    }
                    else
                    {
                        tOld.swipeCount++;
                    }
                }
                //log.Info("Transport", "Number of records = " + count + ", final = " + list.Count);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                cw.Dispose();
                if( dr != null )
                {
                    dr.Dispose();
                }
            }

            return list;
        } 
    }
}