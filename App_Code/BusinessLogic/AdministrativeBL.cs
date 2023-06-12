using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System.Security.Principal;

/// <summary>
/// Summary description for AdministrativeBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class AdministrativeBL
    {
        public AdministrativeBL()
        {
        }

        private AdministrativeDB setClientPreferencesDB = new AdministrativeDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public Boolean SetClientPreferences( int clientID, Boolean displayJobCodes, Boolean displayPayRates,
                                                    Boolean submitHours, Boolean generateInvoice, Boolean approveHours,
                                                        Boolean displaySchedule, Boolean enablePunchReports, Boolean employeeHistoryExactPunchTimes,
                                                    Boolean ticketTrackingExactLatePunches, Boolean displayWeeklyReportsSundayToSaturday,
                                                    Boolean displayBonuses, Boolean displayPayRateMaintenance, Boolean displayWeeklyReportsWednesdayToTuesday,
                                                    Boolean displayTemps, Boolean displayStartDate, Boolean displayBreakTimes, Boolean displayExactPunchTimes, Boolean rosterBasedPayRates, 
                                                    Boolean showLocationsHoursReport, Boolean displayWeeklyReportsSaturdayToFriday)
        {
            return setClientPreferencesDB.SetClientPreferences(clientID, displayJobCodes,  
                                    displayPayRates, submitHours, generateInvoice, approveHours, displaySchedule, enablePunchReports, 
                                    employeeHistoryExactPunchTimes, ticketTrackingExactLatePunches, displayWeeklyReportsSundayToSaturday,
                                    displayBonuses, displayPayRateMaintenance, displayWeeklyReportsWednesdayToTuesday, displayTemps, displayStartDate, 
                                    displayBreakTimes, displayExactPunchTimes, rosterBasedPayRates, showLocationsHoursReport, displayWeeklyReportsSaturdayToFriday);
        }
    }
}
