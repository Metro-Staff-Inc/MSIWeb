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
/// Summary description for AdministrativeDB
/// </summary>
/// 
namespace MSI.Web.MSINet.DataAccess
{
    public class AdministrativeDB
    {
        public AdministrativeDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();

        /* set preferences for current client */
        public Boolean SetClientPreferences( int clientID, Boolean displayJobCodes, Boolean displayPayRates, 
                                                        Boolean submitHours, Boolean generateInvoice, Boolean approveHours,
                                                Boolean displaySchedule, Boolean enablePunchReports, 
                                Boolean employeeHistoryExactPunchTimes, Boolean ticketTrackingExactLatePunches, 
                                Boolean displayWeeklyReportsSundayToSaturday, Boolean displayBonuses, Boolean displayPayRateMaintenance, 
                                Boolean displayWeeklyReportsWednesdayToTuesday, Boolean displayTemps, Boolean displayStartDate, 
                                Boolean displayBreakTimes, Boolean displayExactPunchTimes, Boolean rosterBasedPayRates, 
                                Boolean showLocationsHoursReport, Boolean displayWeeklyReportsSaturdayToFriday, Boolean displayWeeklyReportsFridayToThursday)
        {
            Boolean success = false;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetClientPreferences);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@displayJobCodes", DbType.Boolean, displayJobCodes);
            dbSvc.AddInParameter(cw, "@displayPayRates", DbType.Boolean, displayPayRates);
            dbSvc.AddInParameter(cw, "@displayInvoice", DbType.Boolean, generateInvoice);
            dbSvc.AddInParameter(cw, "@displaySubmitHours", DbType.Boolean, submitHours);
            dbSvc.AddInParameter(cw, "@approveHours", DbType.Boolean, approveHours);
            dbSvc.AddInParameter(cw, "@displaySchedule", DbType.Boolean, displaySchedule);
            dbSvc.AddInParameter(cw, "@enablePunchReports", DbType.Boolean, enablePunchReports);
            dbSvc.AddInParameter(cw, "@employeeHistoryExactPunchTimes", DbType.Boolean, employeeHistoryExactPunchTimes);
            dbSvc.AddInParameter(cw, "@ticketTrackingExactLatePunches", DbType.Boolean, ticketTrackingExactLatePunches);
            dbSvc.AddInParameter(cw, "@displayWeeklyReportsSundayToSaturday", DbType.Boolean, displayWeeklyReportsSundayToSaturday);
            dbSvc.AddInParameter(cw, "@displayWeeklyReportsWednesdayToTuesday", DbType.Boolean, displayWeeklyReportsWednesdayToTuesday);
            dbSvc.AddInParameter(cw, "@displayBonuses", DbType.Boolean, displayBonuses);
            dbSvc.AddInParameter(cw, "@displayPayRateMaintenance", DbType.Boolean, displayPayRateMaintenance);
            dbSvc.AddInParameter(cw, "@displayTemps", DbType.Boolean, displayTemps);
            dbSvc.AddInParameter(cw, "@displayStartDate", DbType.Boolean, displayStartDate);
            dbSvc.AddInParameter(cw, "@displayBreakTimes", DbType.Boolean, displayBreakTimes);
            dbSvc.AddInParameter(cw, "@displayExactPunchTimes", DbType.Boolean, displayExactPunchTimes);
            dbSvc.AddInParameter(cw, "@rosterBasedPayRates", DbType.Boolean, rosterBasedPayRates);
            dbSvc.AddInParameter(cw, "@showLocationsHoursReport", DbType.Boolean, showLocationsHoursReport);
            dbSvc.AddInParameter(cw, "@displayWeeklyReportsSaturdayToFriday", DbType.Boolean, displayWeeklyReportsSaturdayToFriday);
            dbSvc.AddInParameter(cw, "@displayWeeklyReportsFridayToThursday", DbType.Boolean, displayWeeklyReportsFridayToThursday);
            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                if( rows == 1 )
                    success = true;  /* check results, then set... */
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return success;
        }
    }
}