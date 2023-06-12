using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data.Common;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;

/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class EmployeePunchMaintenanceDB
    {
        public EmployeePunchMaintenanceDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();

        public EmployeePunchMaintenance GetEmployeePunchMaintenance(EmployeePunchMaintenance employeePunchMaintenance, IPrincipal userPrincipal)
        {
            EmployeePunchMaintenance returnResult = new EmployeePunchMaintenance();
            EmployeePunch employeePunch = null;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeePunchMaintenance);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientRosterID", DbType.Int32, employeePunchMaintenance.ClientRosterID);
            dbSvc.AddInParameter(cw, "@startDateTime", DbType.DateTime, employeePunchMaintenance.StartDateTime.ToString("MM/dd/yyyy HH:mm"));
            dbSvc.AddInParameter(cw, "@endDateTime", DbType.DateTime, employeePunchMaintenance.EndDateTime.ToString("MM/dd/yyyy HH:mm"));

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        Boolean deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
                        if( !deleted )
                        {
                            employeePunch = new EmployeePunch();
                            employeePunch.EmployeePunchID = dr.GetInt32(dr.GetOrdinal("employee_punch_id"));
                            employeePunch.RoundedPunchDateTime = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            returnResult.EmployeePunches.Add(employeePunch);
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

            return returnResult;
        }

        public EmployeePunchMaintenanceResult MoveEmployeePunch(EmployeePunchMove employeePunchMove, IPrincipal userPrincipal)
        {
            EmployeePunchMaintenanceResult returnResult = new EmployeePunchMaintenanceResult();
            //returnResult.EmployeePunchMaintenanceInfo = employeePunch;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.MoveEmployeePunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@movePunchList", DbType.String, employeePunchMove.MovePunchList);
            dbSvc.AddInParameter(cw, "@moveToDepartment", DbType.Int32, employeePunchMove.MoveToDepartment.DepartmentID);
            dbSvc.AddInParameter(cw, "@movedBy", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@movedDateTime", DbType.DateTime, employeePunchMove.MoveDateTime);

            try
            {
                dbSvc.ExecuteNonQuery(cw);

                //record was saved
                returnResult.ResultValue = true;
                returnResult.ErrorInfo = string.Empty;
            }
            catch (Exception ex)
            {
                returnResult.ResultValue = false;
                returnResult.ErrorInfo = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }

            return returnResult;
        }

        public EmployeePunchMaintenanceResult SaveEmployeePunch(EmployeePunch employeePunch, IPrincipal userPrincipal)
        {
            EmployeePunchMaintenanceResult returnResult = new EmployeePunchMaintenanceResult();
            //returnResult.EmployeePunchMaintenanceInfo = employeePunch;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SaveEmployeePunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@employeePunchID", DbType.Int32, employeePunch.EmployeePunchID);
            dbSvc.AddInParameter(cw, "@roundedPunchDateTime", DbType.DateTime, employeePunch.RoundedPunchDateTime);
            dbSvc.AddInParameter(cw, "@lastUpdatedBy", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@lastUpdatedDateTime", DbType.DateTime, employeePunch.LastUpdatedDateTime);
            dbSvc.AddInParameter(cw, "@manualOverrideFlag", DbType.Boolean, employeePunch.ManualOverride);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
                
                //record was saved
                returnResult.ResultValue = true;
                returnResult.ErrorInfo = string.Empty;
            }
            catch (Exception ex)
            {
                returnResult.ResultValue = false;
                returnResult.ErrorInfo = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }

            return returnResult;
        }

        public EmployeePunchMaintenanceResult DeleteEmployeePunch(EmployeePunch employeePunch, IPrincipal userPrincipal)
        {
            EmployeePunchMaintenanceResult returnResult = new EmployeePunchMaintenanceResult();
            //returnResult.EmployeePunchMaintenanceInfo = employeePunch;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.DeleteEmployeePunch);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@employeePunchID", DbType.Int32, employeePunch.EmployeePunchID);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
                //record was deleted
                returnResult.ResultValue = true;
                returnResult.ErrorInfo = string.Empty;
            }
            catch (Exception ex)
            {
                returnResult.ResultValue = false;
                returnResult.ErrorInfo = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }

            return returnResult;
        }

    }
}