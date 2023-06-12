using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using System.Collections;
using System.Xml.Linq;

/// <summary>
/// Summary description for ClientDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class ClientDB
    {
        public ClientDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private DataAccessHelper _dbHelper = new DataAccessHelper();

        public List<DepartmentSupervisor> GetDepartmentSupervisors(int clientId, Guid userId)
        {
            List<DepartmentSupervisor> depts = new List<DepartmentSupervisor>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDepartmentSupervisors);
            cw.CommandTimeout = 120;    //2 minutes
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);
            dbSvc.AddInParameter(cw, "@userId", DbType.Guid, userId );
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        DepartmentSupervisor ds = new DepartmentSupervisor();
                        ds.DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id"));
                        ds.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        ds.DepartmentViewable = dr.GetInt32(dr.GetOrdinal("department_access")) == 0?false:true;
                        ds.DepartmentViewable |= dr.GetBoolean(dr.GetOrdinal("all_access"));
                        ds.SupervisorId = dr.GetInt32(dr.GetOrdinal("supervisor_id"));
                        ds.LocationId = dr.GetInt32(dr.GetOrdinal("location_id"));
                        ds.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        depts.Add(ds);
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
            return depts;
        }
        public List<DepartmentInfo> GetDepartmentInfo(int clientId)
        {
            List<DepartmentInfo> depts = new List<DepartmentInfo>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDepartmentInfo);
            cw.CommandTimeout = 120;    //2 minutes
            dbSvc.AddInParameter(cw, "@CLIENT_ID", DbType.Int32, clientId);
            dbSvc.AddInParameter(cw, "@TODAY", DbType.DateTime, DateTime.Now);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        DepartmentInfo di = new DepartmentInfo();
                        PayRate pr = new PayRate();
                        di.DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id"));
                        di.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        di.Active = !dr.GetBoolean(dr.GetOrdinal("void"));
                        di.LocationName = dr.GetString(dr.GetOrdinal("location_name"));
                        di.LocationId = dr.GetInt32(dr.GetOrdinal("location_id"));
                        di.ShiftId = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        di.ShiftName = dr.GetString(dr.GetOrdinal("shift_desc"));
                        di.ShiftBreak = dr.GetDecimal(dr.GetOrdinal("shift_break_hours"));
                        di.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        di.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        di.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        di.TrackingStart = dr.GetString(dr.GetOrdinal("tracking_start_time"));
                        di.TrackingEnd = dr.GetString(dr.GetOrdinal("tracking_end_time"));
                        pr.PayRateID = dr.GetInt32(dr.GetOrdinal("client_pay_id"));
                        pr.HourlyRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                        pr.StartDate = dr.GetDateTime(dr.GetOrdinal("effective_dt")).ToString("yyyy-MM-dd");
                        pr.EndDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt")).ToString("yyyy-MM-dd");
                        int idx = depts.IndexOf(di);
                        if (idx < 0)
                        {
                            depts.Add(di);
                            idx = depts.IndexOf(di);
                        }
                        depts[idx].PayRate.Add(pr);
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
            return depts;
        }
        public List<Department> GetDepartments(string clientID, string locationID, string shiftType)
        {
            List<Department> depts = new List<Department>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDepartments);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(clientID));
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, Convert.ToInt32(locationID));
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, Convert.ToInt32(shiftType));
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        Department d = new Department();
                        d.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                        d.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        d.Location = Convert.ToInt32(locationID);
                        if (d.DepartmentID == 100 && Convert.ToInt32(clientID) == 92)
                            continue;
                        depts.Add(d);
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
            return depts;
        }

        public List<Department> GetDepartments(int clientID)
        {
            List<Department> depts = new List<Department>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientDepartments);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(clientID));
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        Department d = new Department();
                        d.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                        d.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        d.DepartmentName = d.DepartmentName.ToUpper();
                        Boolean contains = false;
                        for (int i = 0; i < depts.Count; i++)
                        {
                            if (depts[i].DepartmentName.Equals(d.DepartmentName)) contains = true;
                        }
                        if (!contains)
                            depts.Add(d);
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
            return depts;
        }

        public string SetClientMultiplier(string clientID, string multiplier, string multiplier2, string otMultiplier, string otMultiplier2,
                                        string bonusMultiplier, string otherMultiplier, string passThruMultiplier, string vacationMultiplier)
        {
            string retStr = "";
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetClientMultiplier);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(clientID));
            dbSvc.AddInParameter(cw, "@multiplier", DbType.Decimal, Convert.ToDecimal(multiplier));
            dbSvc.AddInParameter(cw, "@multiplier2", DbType.Decimal, Convert.ToDecimal(multiplier2));
            dbSvc.AddInParameter(cw, "@otMultiplier", DbType.Decimal, Convert.ToDecimal(otMultiplier));
            dbSvc.AddInParameter(cw, "@otMultiplier2", DbType.Decimal, Convert.ToDecimal(otMultiplier2));

            dbSvc.AddInParameter(cw, "@bonusMultiplier", DbType.Decimal, Convert.ToDecimal(bonusMultiplier));
            dbSvc.AddInParameter(cw, "@otherMultiplier", DbType.Decimal, Convert.ToDecimal(otherMultiplier));
            dbSvc.AddInParameter(cw, "@passThruMultiplier", DbType.Decimal, Convert.ToDecimal(passThruMultiplier));
            dbSvc.AddInParameter(cw, "@vacationMultiplier", DbType.Decimal, Convert.ToDecimal(vacationMultiplier));

            try
            {
                int rec = dbSvc.ExecuteNonQuery(cw);
                retStr = "Client multipliers updated.";
            }
            catch (Exception ex)
            {

                retStr = ex.Message;
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }



        public DepartmentMapping GetDepartmentMapping(int clientID)
        {
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetShiftMapping);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            DepartmentMapping dm = new DepartmentMapping();
            dm.ClientID = clientID;

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    bool rateNotSet = true;
                    while (dr.Read())
                    {
                        if (rateNotSet)
                        {
                            int p = dr.GetOrdinal("reg_rate");
                            var v = dr.GetValue(p);
                            dm.RegRate = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("ot_rate"));
                            dm.OTRate = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("ot_rate2"));
                            dm.OTRate2 = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("reg_rate2"));
                            dm.RegRate2 = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("bonus_rate"));
                            dm.BonusRate = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("vac_rate"));
                            dm.VacRate = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("passThru_rate"));
                            dm.PassThruRate = Convert.ToDouble(v);
                            v = dr.GetValue(dr.GetOrdinal("other_rate"));
                            dm.OtherRate = Convert.ToDouble(v);
                            dm.ShiftMappingID = dr.GetInt32(dr.GetOrdinal("client_multiplier_id"));
                            rateNotSet = false;
                        }
                        MapInfo inf = new MapInfo();
                        inf.Desc = dr.GetString(dr.GetOrdinal("department_name"));
                        inf.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        inf.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));

                        inf.MSIDeptID = dr.GetInt32(dr.GetOrdinal("department_id"));
                        inf.TWMapID = dr.GetInt32(dr.GetOrdinal("temp_works_id"));
                        inf.ShiftMappingDBIdx = dr.GetInt32(dr.GetOrdinal("shift_mapping_id"));
                        dm.MapList.Add(inf);
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

            return dm;
        }

        public string UpdateMapping(XElement xmlTree, int numUpdates)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            String ret;
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateShiftMappings);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@XML_data", DbType.Xml, xmlTree.ToString());
            //dbSvc.AddOutParameter(cw, "", DbType.Xml, xmlTree);
            try
            {
                int rec = dbSvc.ExecuteNonQuery(cw);
                ret = rec + " mappings updated.";
            }
            catch (Exception ex)
            {

                ret = ex.Message;
            }
            finally
            {
                cw.Dispose();
            }
            return ret;
        }

        public DateTime CheckIPClock()
        {
            DateTime retInfo = new DateTime();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.CheckIPClock);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                        retInfo = dr.GetDateTime(dr.GetOrdinal("last_tick"));
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
            return retInfo;
        }

        public Client GetClientByUserName(string userName)
        {
            Client returnClient = new Client();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientByUserName);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userName);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE CLIENT
                        returnClient = _dbHelper.fillClient(dr);
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

            if (returnClient.ClientID > 0)
            {
                return returnClient;
            }
            else
            {
                return null;
            }
        }

        public List<String> GetEmail(string id)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmail);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(id));
            List<String> emailList = new List<String>();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        emailList.Add(dr.GetString(dr.GetOrdinal("email_addr")));
                    }
                }
                catch (Exception ex)
                {

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

            }
            finally
            {
                cw.Dispose();
            }
            return emailList;
        }

        public List<String> SetEmail(string id, string email, string name)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetEmail);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(id));
            dbSvc.AddInParameter(cw, "@emailAddress", DbType.String, email);
            dbSvc.AddInParameter(cw, "@name", DbType.String, name);
            List<String> emailList = new List<String>();
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cw.Dispose();
            }

            return emailList;
        }

        public string GetShiftTimes(string id, string loc, string dept, string shiftType)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 21);
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, Convert.ToInt32(id));
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, Convert.ToInt32(dept));
            dbSvc.AddInParameter(cw, "@locationid", DbType.Int32, Convert.ToInt32(loc));
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, Convert.ToInt32(shiftType));
            string retStr = "";
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        retStr = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        retStr += dr.GetString(dr.GetOrdinal("shift_end_time"));
                        retStr += dr.GetString(dr.GetOrdinal("tracking_start_time"));
                        retStr += dr.GetString(dr.GetOrdinal("tracking_end_time"));
                        retStr += dr.GetDecimal(dr.GetOrdinal("shift_break_hours")).ToString();
                    }
                }
                catch (Exception ex)
                {
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

            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }

        public ClientDDLMobile MobileGetClientsByUserName(string userName)
        {
            ClientDDLMobile returnClients = new ClientDDLMobile();
            returnClients.ClientID = new List<int>();
            returnClients.ClientName = new List<String>();

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientsByUserName);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userName);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE CLIENT
                        returnClients.ClientID.Add(dr.GetInt32(dr.GetOrdinal("client_id")));
                        returnClients.ClientName.Add(dr.GetString(dr.GetOrdinal("client_name")));
                        bool pref = dr.GetBoolean(dr.GetOrdinal("preferred_client"));
                        if (pref)
                        {
                            returnClients.Preferred = dr.GetInt32(dr.GetOrdinal("client_id"));
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

            return returnClients;
        }
        public ArrayList GetClientsByUserName(string userName)
        {
            Client returnClient = new Client();
            ArrayList returnClients = new ArrayList();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientsByUserName);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userName);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                int c;
                try
                {
                    while (dr.Read())
                    {
                        //if (dr.GetBoolean(dr.GetOrdinal("void")) == true)
                        //    continue; 
                        //LOAD THE CLIENT
                        Client client = _dbHelper.fillClient(dr, true);
                        int i = 0;
                        for (i = 0; i < returnClients.Count; i++)
                        {
                            if (((Client)(returnClients[i])).ClientID == client.ClientID)
                                break;
                        }
                        if (i >= returnClients.Count)
                        {
                            client.Location = new Dictionary<int, String>();
                            client.Location.Add(dr.GetInt32(dr.GetOrdinal("location_id")),
                                dr.GetString(dr.GetOrdinal("location_name")));
                            returnClients.Add(client);
                        }
                        else
                        {
                            c = dr.GetInt32(dr.GetOrdinal("location_id"));
                            ((Client)returnClients[i]).Location.Add(dr.GetInt32(dr.GetOrdinal("location_id")),
                                dr.GetString(dr.GetOrdinal("location_name")));
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

            if (returnClients.Count > 0)
            {
                return returnClients;
            }
            else
            {
                return null;
            }
        }
        public ClientPreferences GetClientPreferencesByID(int clientID)
        {
            ClientPreferences returnClientPrefs = new ClientPreferences();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientPreferencesByID);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                        //LOAD THE CLIENT Preferences
                        returnClientPrefs = _dbHelper.fillClientPreferences(dr);
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
            return returnClientPrefs;
        }

        public List<ClientDDL> GetActiveClients(string userID)
        {
            List<ClientDDL> list = new List<ClientDDL>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetActiveClients);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        ClientDDL c = new ClientDDL();
                        c.ClientID = dr.GetInt32(dr.GetOrdinal("client_id"));
                        c.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                        if (userID.ToUpper().Equals("SHIRED") && c.ClientID != 258)
                            continue;
                        else
                            list.Add(c);
                    }
                }
                catch (Exception drEx)
                {
                    //throw ex;

                    ClientDDL c = new ClientDDL();
                    c.ClientID = -1;
                    c.ClientName = drEx.ToString();
                    list.Add(c);

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

                ClientDDL c = new ClientDDL();
                c.ClientID = -2;
                c.ClientName = ex.ToString();
                list.Add(c);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }
        public int RemoveRoster(int rosterID)
        {
            int count = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "action", DbType.Int32, 92);
            dbSvc.AddInParameter(cw, "client_roster_id", DbType.Int32, rosterID);
            try
            {
                IDataReader dr = null;
                try
                {
                    dr = dbSvc.ExecuteReader(cw);
                }
                catch (Exception ex)
                {

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

            }
            finally
            {
                cw.Dispose();
            }
            return count;
        }
        public int UpdateRoster(int rosterID, DateTime start, DateTime end, string startTime, string endTime, string trackStart, string trackEnd, string subs)
        {
            int count = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "action", DbType.Int32, 9);
            dbSvc.AddInParameter(cw, "client_roster_id", DbType.Int32, rosterID);
            dbSvc.AddInParameter(cw, "startdate", DbType.DateTime, start);
            dbSvc.AddInParameter(cw, "enddate", DbType.DateTime, end);
            dbSvc.AddInParameter(cw, "startTime", DbType.String, startTime);
            dbSvc.AddInParameter(cw, "endTime", DbType.String, endTime);
            dbSvc.AddInParameter(cw, "track_start", DbType.String, trackStart);
            dbSvc.AddInParameter(cw, "track_end", DbType.String, trackEnd);
            if (subs == null || subs.Length > 1)
                dbSvc.AddInParameter(cw, "subs", DbType.String, subs);
            try
            {
                IDataReader dr = null;
                try
                {
                    dr = dbSvc.ExecuteReader(cw);
                }
                catch (Exception ex)
                {

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

            }
            finally
            {
                cw.Dispose();
            }
            return count;
        }

        public Client GetClientShiftTypes(Client clientInfo, int loc)
        {
            Client returnClient = clientInfo;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientShiftTypes);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "clientID", DbType.Int32, clientInfo.ClientID);
            dbSvc.AddInParameter(cw, "locID", DbType.Int32, loc);
            try
            {
                returnClient.ShiftTypes.Clear();
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE CLIENT SHIFT TYPES
                        returnClient.ShiftTypes.Add(new ShiftType(dr.GetInt32(dr.GetOrdinal("shift_type"))));
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

            return returnClient;
        }
        public int UpdateClockTick(string dt)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateClockTick);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "tickTime", DbType.String, dt);
            int t = 0;
            try
            {
                t = dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return t;
        }
        public ArrayList GetClientDepartmentsByShiftType(Client clientInfo, ShiftType shiftType, int loc=0)
        {
            ArrayList departments = new ArrayList();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientDepartmentsByShiftType);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientInfo.ClientID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType.ShiftTypeId);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE CLIENT SHIFT TYPES
                        Department dpt = new Department(dr.GetInt32(dr.GetOrdinal("department_id")), dr.GetString(dr.GetOrdinal("department_name")));
                        dpt.Location = dr.GetInt32(dr.GetOrdinal("location_id"));
                        if (dpt.DepartmentID == 100 && clientInfo.ClientID == 92) continue;
                        departments.Add(dpt);
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

            return departments;
        }

        public ArrayList GetClientDepartmentsByShiftType(Client clientInfo, ShiftType shiftType, string UserName, int loc=0)
        {
            ArrayList departments = new ArrayList();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientDepartmentsByShiftTypeByUser);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientInfo.ClientID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, UserName);
            dbSvc.AddInParameter(cw, "@locID", DbType.Int32, loc);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE CLIENT SHIFT TYPES
                        Department dept = new Department();
                        dept.EmailAddress = dr.GetString(dr.GetOrdinal("email_address"));

                        departments.Add(new Department(dr.GetInt32(dr.GetOrdinal("department_id")), dr.GetString(dr.GetOrdinal("department_name")), dr.GetString(dr.GetOrdinal("email_address"))));
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

            return departments;
        }


        public Client GetClientShifts(Client client, int locationId = 0)
        {
            Client returnClient = client;
            ShiftType shiftType = (ShiftType)client.ShiftTypes[0];
            Department department = (Department)client.Departments[0];
            Shift shift = new Shift();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientShifts);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, client.ClientID);
            if (shiftType.ShiftTypeId > 0)
                dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType.ShiftTypeId);
            if (department.DepartmentID != 0)
                dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, department.DepartmentID);
            if (locationId > 0)
                dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, locationId);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE shifts
                        returnClient.Shifts.Add(new Shift(dr.GetInt32(dr.GetOrdinal("shift_id")), "", dr.GetString(dr.GetOrdinal("shift_start_time")), dr.GetString(dr.GetOrdinal("shift_end_time")), dr.GetString(dr.GetOrdinal("tracking_start_time")), dr.GetString(dr.GetOrdinal("tracking_end_time")), dr.GetBoolean(dr.GetOrdinal("tracking_multi_day"))));
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

            return returnClient;
        }

        public ArrayList GetSwipeDepartments(Client client)
        {
            ArrayList returnVal = new ArrayList();
            Department department = new Department();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientSwipeDepartments);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, client.ClientID);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE departments
                        returnVal.Add(new Department(dr.GetInt32(dr.GetOrdinal("department_id")), dr.GetString(dr.GetOrdinal("department_name"))));
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

            return returnVal;
        }

        public DepartmentPayRate GetDepartmentPayRates(PayRateInput inputInfo, string UserName)
        {
            DepartmentPayRate payOverride = new DepartmentPayRate();
            ClientPayOverride emp = new ClientPayOverride();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDepartmentPayRates);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, inputInfo.ClientID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, inputInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, inputInfo.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, inputInfo.WeekEndDate);
            //get the start of the week
            dbSvc.AddInParameter(cw, "@weekEndStart", DbType.DateTime, inputInfo.WeekEndDate.AddDays(-6));
            dbSvc.AddOutParameter(cw, "@clientPayID", DbType.Int32, 4);
            dbSvc.AddOutParameter(cw, "@departmentPayRate", DbType.Currency, 8);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        emp = new ClientPayOverride();
                        //LOAD THE overrides
                        emp.AidentNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        emp.ClientPayOverrideId = dr.GetInt32(dr.GetOrdinal("client_pay_override_id"));
                        emp.EmployeeId = dr.GetInt32(dr.GetOrdinal("employee_id"));
                        emp.EffectiveDate = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        emp.ExpirationDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        emp.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        emp.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        emp.ShiftId = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        emp.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                        emp.ClientId = dr.GetInt32(dr.GetOrdinal("client_id"));
                        emp.LocationId = dr.GetInt32(dr.GetOrdinal("location_id"));


                        payOverride.PayRateOverrides.Add(emp);
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
                        //get the department payrate
                        payOverride.PayRate = decimal.Parse(cw.Parameters["@departmentPayRate"].Value.ToString());
                        payOverride.ClientPayId = int.Parse(cw.Parameters["@clientPayId"].Value.ToString());
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

            return payOverride;
        }

        public DepartmentJobCode GetDepartmentJobCodes(JobCodeInput inputInfo, string UserName)
        {
            DepartmentJobCode jobcodeOverride = new DepartmentJobCode();
            ClientJobCodeOverride emp = new ClientJobCodeOverride();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDepartmentJobCodes);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, inputInfo.ClientID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, inputInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, inputInfo.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, inputInfo.WeekEndDate);
            //get the start of the week
            dbSvc.AddInParameter(cw, "@weekEndStart", DbType.DateTime, inputInfo.WeekEndDate.AddDays(-6));
            dbSvc.AddOutParameter(cw, "@clientJobCodeID", DbType.Int32, 4);
            dbSvc.AddOutParameter(cw, "@departmentJobCode", DbType.String, 8);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        emp = new ClientJobCodeOverride();
                        //LOAD THE overrides
                        emp.AidentNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        emp.ClientJobCodeOverrideId = dr.GetInt32(dr.GetOrdinal("client_pay_override_id"));
                        emp.EmployeeId = dr.GetInt32(dr.GetOrdinal("employee_id"));
                        emp.EffectiveDate = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        emp.ExpirationDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        emp.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        emp.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        emp.ShiftId = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        emp.JobCode = dr.GetString(dr.GetOrdinal("job_code"));
                        emp.ClientId = dr.GetInt32(dr.GetOrdinal("client_id"));
                        emp.LocationId = dr.GetInt32(dr.GetOrdinal("location_id"));


                        jobcodeOverride.JobCodeOverrides.Add(emp);
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
                        //get the department jobcode
                        jobcodeOverride.JobCode = cw.Parameters["@departmentJobCode"].Value.ToString();
                        jobcodeOverride.ClientJobCodeId = int.Parse(cw.Parameters["@clientJobCodeId"].Value.ToString());
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

            return jobcodeOverride;
        }

        public string GetIDFromSuncastNum(string suncastNum)
        {
            string retVal = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetIDFromSuncastNum);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@suncastNum", DbType.Int32, Convert.ToInt32(suncastNum));
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                    {
                        retVal = "" + dr.GetInt32(dr.GetOrdinal("employee_id"));
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
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

                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            if (retVal.Length == 0)
                retVal = "NO ID!";
            return retVal;
        }
    }
}
