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
using System.Collections.Generic;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class PunchReportBL
    {
        public PunchReportBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        PunchReportDB punchReportDB = new PunchReportDB();

        public PunchReport GetPunchRecords(PunchReport punchReport, IPrincipal userPrincipal)
        {
            return punchReportDB.GetPunchRecords(punchReport, userPrincipal);
        }
        public List<ResourceGroupHours>GetResourceGroupHours(String aidentNumber, String cwAidentNumber, int resourceGroup)
        {
            PunchReportDB prDB = new PunchReportDB();
            return prDB.GetResourceGroupHours(aidentNumber, cwAidentNumber, resourceGroup);
        }
        public string UpdateResourceGroupHours(String inpList)
        {
            PunchReportDB prdb = new PunchReportDB();
            return prdb.UpdateResourceGroupHours(inpList);
        }
        public List<ResourceGroupInfo> GetResourceGroupInfo()
        {
            PunchReportDB prdb = new PunchReportDB();
            return prdb.GetResourceGroupInfo();
        }
        public PunchReport GetPunchRecordCreators(PunchReport punchReport, IPrincipal userPrincipal)
        {
            return punchReportDB.GetPunchRecordCreators(punchReport, userPrincipal);
        }
        public String PunchesExist(string clientRosterID)
        {
            RosterDB rdb = new RosterDB();

            return rdb.PunchesExist(clientRosterID);
        }

        public ArrayList GetDailyPunches(PunchReport punchReport)
        {
            punchReport = punchReportDB.GetDailyPunches(punchReport);
            ArrayList punches = new ArrayList();
            if (punchReport.PunchRecord.Count == 0) return punches;

            ArrayList pr = punchReport.PunchRecord;
            PunchRecordDisplay prd = new PunchRecordDisplay
            {
                AidentNumber = ((PunchRecord)pr[0]).AidentNumber,
                Name = ((PunchRecord)pr[0]).FullName,
                Department = ((PunchRecord)pr[0]).Department,
                Shift = ((PunchRecord)pr[0]).Shift,
                PunchIn = ((PunchRecord)pr[0]).RoundedPunchDate
            };

            for ( int i=1; i<punchReport.PunchRecord.Count; i++ )
            {
                if(((PunchRecord)pr[i]).AidentNumber != prd.AidentNumber )
                {
                    /* add current record to list */
                    punches.Add(prd);
                    prd = new PunchRecordDisplay
                    {
                        AidentNumber = ((PunchRecord)pr[i]).AidentNumber,
                        Name = ((PunchRecord)pr[i]).FullName,
                        Department = ((PunchRecord)pr[i]).Department,
                        Shift = ((PunchRecord)pr[i]).Shift,
                        PunchIn = ((PunchRecord)pr[i]).RoundedPunchDate
                    };
                }
                else
                {
                    prd.PunchOut = ((PunchRecord)pr[i]).RoundedPunchDate;
                    prd.Hours = (prd.PunchOut - prd.PunchIn).TotalSeconds / 3600;
                }
            }
            punches.Add(prd);
            return punches;
        }
    }
}