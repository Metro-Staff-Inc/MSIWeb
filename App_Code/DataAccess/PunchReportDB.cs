using System;
using System.Data;
using System.Data.Common;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using System.Collections.Generic;

/// <summary>
/// Summary description for PunchReport
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class PunchReportDB
    {
        public PunchReportDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();
        
        public List<ResourceGroupInfo> GetResourceGroupInfo()
        {
            List<ResourceGroupInfo> list = new List<ResourceGroupInfo>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetResourceGroupInfo);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        ResourceGroupInfo rg = new ResourceGroupInfo();
                        rg.Name = dr.GetString(dr.GetOrdinal("name"));
                        rg.Description = dr.GetString(dr.GetOrdinal("description"));
                        rg.ResourceGroupId = dr.GetInt32(dr.GetOrdinal("cw_resource_group_id"));
                        list.Add(rg);
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
            return list;
        }
        
        internal PunchReport GetDailyPunches(PunchReport punchReport)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDaysPunches);
            cw.CommandTimeout = 120;    //2 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, punchReport.ClientID);
            dbSvc.AddInParameter(cw, "startDate", DbType.DateTime, DateTime.Parse(punchReport.StartDate.Date.ToString()));

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        PunchRecord pr = new PunchRecord();
                        pr.AidentNumber = dr.GetString(dr.GetOrdinal("badge_number"));
                        pr.Department = dr.GetString(dr.GetOrdinal("department_name"));
                        pr.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        pr.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        pr.CreatedDate = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        pr.PunchDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        pr.RoundedPunchDate = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                        pr.Shift = dr.GetInt32(dr.GetOrdinal("shift"));
                        pr.FullName = pr.LastName + ", " + pr.FirstName;
                        punchReport.PunchRecord.Add(pr);
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
            return punchReport;
        }
        
        public string UpdateResourceGroupHours(String inpList)
        {
            String retStr = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateResourceGroup);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@inpStr", DbType.String, inpList);
            try
            {
                int lines = dbSvc.ExecuteNonQuery(cw);
                retStr = lines + " records updated / added.";
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                cw.Dispose();
            }

            return retStr;
        }


        public List<ResourceGroupHours> GetResourceGroupHours(String aidentNumber, String cwAidentNumber, int resourceGroup)
        {
            List<ResourceGroupHours> list = new List<ResourceGroupHours>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetResourceGroupHours);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "cwAidentNumber", DbType.String, cwAidentNumber);
            dbSvc.AddInParameter(cw, "aidentNumber", DbType.String, aidentNumber);
            dbSvc.AddInParameter(cw, "resourceGroupId", DbType.Int32, resourceGroup);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        ResourceGroupHours rg = new ResourceGroupHours();
                        rg.AidentNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        rg.CWAidentNumber = dr.GetString(dr.GetOrdinal("cw_aident_number"));
                        rg.ResourceGroupId = dr.GetInt32(dr.GetOrdinal("cw_resource_group_id"));
                        rg.StartDt = dr.GetDateTime(dr.GetOrdinal("start_dt"));
                        rg.EndDt = dr.GetDateTime(dr.GetOrdinal("end_dt"));
                        rg.Hours = (double)dr.GetDecimal(dr.GetOrdinal("hours"));
                        rg.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        rg.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        list.Add(rg);
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
            return list;
        }

        public PunchReport GetPunchRecordCreators(PunchReport punchReport, IPrincipal userPrincipal)
        {
            PunchReport returnInfo = new PunchReport();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPunchRecordCreators);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "startingDate", DbType.DateTime, DateTime.Parse(punchReport.StartDate.Date.ToString()) );
            dbSvc.AddInParameter(cw, "endingDate", DbType.DateTime, punchReport.EndDate.Date.AddDays(1));
            dbSvc.AddInParameter(cw, "clientID", DbType.Int32, punchReport.ClientID);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string userID = dr.GetString(dr.GetOrdinal("created_by"));
                        if( !userID.Equals("itdept") )
                            returnInfo.UserIdList.Add(userID);
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
            return returnInfo;
        }

        public PunchReport GetPunchRecords(PunchReport punchReport, IPrincipal userPrincipal)
        {
            PunchReport returnInfo = new PunchReport();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPunchRecords);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, punchReport.ClientID);
            dbSvc.AddInParameter(cw, "startingDate", DbType.DateTime, DateTime.Parse(punchReport.StartDate.Date.ToString()));
            dbSvc.AddInParameter(cw, "endingDate", DbType.DateTime, punchReport.EndDate.Date.AddDays(1));
            dbSvc.AddInParameter(cw, "@userID", DbType.String, punchReport.UserID.ToString());

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        PunchRecord pr = new PunchRecord();
                        pr.AidentNumber = dr.GetString(dr.GetOrdinal("badge_number"));
                        pr.Department = dr.GetString(dr.GetOrdinal("department_name"));
                        string lastName = dr.GetString(dr.GetOrdinal("last_name"));
                        string firstName = dr.GetString(dr.GetOrdinal("first_name"));
                        pr.CreatedDate = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        pr.PunchDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                        pr.FullName = lastName + ", " + firstName;
                        returnInfo.PunchRecord.Add(pr);
                    }
                    //returnInfo.PunchRecord.Sort();
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
            return returnInfo;
        }
    }
}