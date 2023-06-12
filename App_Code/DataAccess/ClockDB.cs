using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ClockWebServices
{
    public class ClockDB
    {
        public string SetICCardAident(string aidentNum, string icCardNum)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetAidentICCardNum);

            dbSvc.AddInParameter(cw, "@aidentNum", DbType.String, aidentNum);
            dbSvc.AddInParameter(cw, "@icCardNum", DbType.String, icCardNum);

            string msg = "";
            try
            {
                msg = Convert.ToString(dbSvc.ExecuteScalar(cw));
            }
            catch(Exception ex)
            {
                msg = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return msg;
        }
        public ClientRosterLastUpdate EmployeeList(int clientId, DateTime createdDate)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.EmployeeList);
            cw.CommandTimeout = 10;    //10 seconds
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);
            dbSvc.AddInParameter(cw, "@createdDate", DbType.DateTime, createdDate);
            ClientRosterLastUpdate crlu = new ClientRosterLastUpdate();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    crlu.Roster = new List<ClientRoster>();
                    while(dr.Read())
                    {
                        ClientRoster cr = new ClientRoster();
                        cr.Name = dr.GetString(dr.GetOrdinal("name"));
                        cr.Id = dr.GetString(dr.GetOrdinal("aident_number"));
                        cr.ICCardNum = dr.GetString(dr.GetOrdinal("iccard_num"));
                        cr.CreatedDate = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        cr.ExpirationDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        cr.Deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
                        if( cr.Deleted )
                        {
                            /* expiration date may be set for midnight, or last punch date */
                            cr.ExpirationDate = DateTime.Now.AddDays(-2);  
                        }

                        if ( (crlu.Roster.Where(item => item.Id == cr.Id)).Count() == 0)
                        {
                            crlu.Roster.Add(cr);
                        } 
                    }
                }
                catch (Exception ex)
                {
                    crlu.Success = false;
                    crlu.Msg = ex.ToString();
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch(Exception ex)
            {
                crlu.Msg = ex.ToString();
                crlu.Success = false;
            }
            finally
            {
                cw.Dispose();
            }
            return crlu;
        }

        public ClientRosterLastUpdate ClientRosterLastUpdate(int clientId, int locationId)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientRosterLastUpdate);
            cw.CommandTimeout = 10;    //10 seconds
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, locationId);

            ClientRosterLastUpdate crlu = new ClientRosterLastUpdate();
            try
            {
                crlu.LastUpdate = Convert.ToString(dbSvc.ExecuteScalar(cw));
                crlu.Success = true;
            }
            catch(Exception e)
            {
                crlu.Success = false;
                crlu.Msg = e.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return crlu;
        }
    }
}