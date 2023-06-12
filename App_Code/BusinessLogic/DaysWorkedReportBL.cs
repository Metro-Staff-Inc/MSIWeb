using System;
using System.Collections;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System.Security.Principal;
using System.Xml.Linq;
using System.Collections.Generic;
using MSIToolkit.Logging;
//using MSIToolkit.Logging;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class DaysWorkedReportBL
    {
        public DaysWorkedReportBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private HelperFunctions helperFunctions = new HelperFunctions();

        public DaysWorkedItem GetDaysWorkedReportSingleUser(string clientID, string id, string start, string end)
        {
            DaysWorkedReport daysWorkedReportInput = new DaysWorkedReport();
            daysWorkedReportInput.MinDays = 1;
            daysWorkedReportInput.ClientID = Convert.ToInt32(clientID);
            daysWorkedReportInput.StartDateTime = Convert.ToDateTime(start);
            daysWorkedReportInput.EndDateTime = Convert.ToDateTime(end);

            GetDaysWorkedReport(daysWorkedReportInput, id);
            if (daysWorkedReportInput.DaysWorkedCollection.Count > 0)
                return (DaysWorkedItem)daysWorkedReportInput.DaysWorkedCollection[0];
            else return null;
        }
        public RecruitPool GetRecruitPool(string clientID, string locationID, string departmentID,
                string shiftType, string startDate, string endDate, string dnrClient )//PerformanceLogger log = null)
        {
            DaysWorkedReportDB dwdb = new DaysWorkedReportDB();
            //if (log != null) log.Info("GetRecruitPool", "Created DaysWorkedReportDB object");

            RecruitPool rp = new RecruitPool();
            rp.ClientID = Convert.ToInt32(clientID);
            rp.DepartmentID = Convert.ToInt32(departmentID);
            rp.ShiftType = Convert.ToInt32(shiftType);
            rp.LocationID = Convert.ToInt32(locationID);
            rp.StartDateTime = Convert.ToDateTime(startDate);
            rp.EndDateTime = Convert.ToDateTime(endDate);
            rp.DNRClientID = Convert.ToInt32(dnrClient);
            //if (log != null) log.Info("GetRecruitPool", "Initialized parameters");

            RecruitPool pool = dwdb.GetRecruitPool(rp, null/*JHM, log*/);
            //if (log != null) log.Info("GetRecruitPool", "Pool object returned");

            return pool;
        }
        public DaysWorkedReport GetDaysWorkedReport(DaysWorkedReport daysworkedReport, string id, PerformanceLogger log = null)
        {
            //CreateDaysWorkedReport(daysworkedReport, userPrincipal);
            DaysWorkedReportDB daysworkedReportDB = new DaysWorkedReportDB();
            if (log != null) log.Info("GetDaysWorkedReport", "daysWorkedReportDB created");
            DaysWorkedReport dwr = daysworkedReportDB.GetDaysWorkedAndDNRStatus(daysworkedReport/*, userPrincipal*/, id, log);

            return dwr;
        
        
        }

        /* get the days worked report and store the info back in the database for access by Crystal Reports */
        public Boolean CreateDaysWorkedReport(DaysWorkedReport daysworkedReport, IPrincipal userPrincipal)
        {
            DaysWorkedReportDB daysworkedReportDB = new DaysWorkedReportDB();
            DaysWorkedReport dwr = daysworkedReportDB.GetDaysWorkedAndDNRStatus(daysworkedReport/*, userPrincipal*/, null);
            if (dwr == null || dwr.DaysWorkedCollection.Count == 0)
                return false;
            else
            {
                DaysWorkedItem dwi;
                ArrayList dwc = dwr.DaysWorkedCollection;
                XElement xmlHeader =
                    new XElement("daysWorkedHeader");
                xmlHeader.Add("clientID", dwr.ClientID);
                xmlHeader.Add("startDate", dwr.StartDateTime);
                xmlHeader.Add("endDate", dwr.EndDateTime.AddDays(1));

                XElement xmlData = 
                    new XElement("daysWorkedData");
                XElement xmlDataDept = new XElement("daysWorkedDepts");

                for(int i=0; i<dwc.Count; i++ )
                {
                    dwi = (DaysWorkedItem)dwc[i];
                    for (int j = 0; j < dwi.Depts.Count; j++)
                    {
                        xmlData.Add(new XElement("row", new XAttribute("aidentNumber", dwi.BadgeNumber), new XAttribute("clientID", dwr.ClientID), 
                            new XAttribute("FirstName", dwi.FirstName), new XAttribute("LastName", dwi.LastName), new XAttribute("firstPunch", dwi.FirstPunch),
                            new XAttribute("totalDaysWorked", dwi.TotalDaysWorked), new XAttribute("DnrReason", dwi.DnrReason), 
                            new XAttribute("deptName", dwi.Depts[j]), new XAttribute("numDays", dwi.DaysWorked[j]) 
                            ));
                    }
                }
                daysworkedReportDB.GetDaysWorkedReport(dwr/*, userPrincipal*/, null);
            }
            return true;
        }
        public int ClientDNR_CheckForExistingRecords(int clientId, string badgeNum)
        {
            DaysWorkedReportDB daysworkedReportDB = new DaysWorkedReportDB();
            return daysworkedReportDB.CheckForExistingRecords(clientId, badgeNum);
        }
        public void ClientDNR_DeactivateEmployee(int clientID, string badgeNum)
        {
            DaysWorkedReportDB daysworkedReportDB = new DaysWorkedReportDB();
            daysworkedReportDB.DeactivateEmployee(clientID, badgeNum);
        }
        public void ClientDNR_ActivateEmployee(int clientID, string badgeNum)
        {
            DaysWorkedReportDB daysworkedReportDB = new DaysWorkedReportDB();
            daysworkedReportDB.ActivateEmployee(clientID, badgeNum);
        }
        /* American Litho */
        public List<RoleInfo> ALithoRoles(String st, String end)
        {
            OpenDB odb = new OpenDB();
            DateTime stDate = Convert.ToDateTime(st);
            DateTime endDate = Convert.ToDateTime(end);
            return odb.ALithoRoles(stDate, endDate);
        }
        public string ALithoUpdateRoles(String inp)
        {
            OpenDB odb = new OpenDB();
            //String[] s = inp.Split(',');
            /* id, rating, description */
            return odb.ALithoUpdateRoles(inp);
        }
    }
}