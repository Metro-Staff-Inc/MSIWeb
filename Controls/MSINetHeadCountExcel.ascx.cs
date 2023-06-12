using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSI.Web.MSINet.BusinessLogic;
using OpenWebServices;

namespace MSI.Web.Controls
{
    public class EmployeePunches
    {
        public int Index { get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public DateTime PunchTime { get; set; }
        public int Shift { get; set; }
        public String Department { get; set; }
        public int punchCount { get; set; }
    }

    public partial class MSINetHeadCountExcel : BaseMSINetControl
    {
        public DateTime dt { get; set; }
        public int clientId;
        public string title;

        protected void Page_Load(object sender, EventArgs e)
        {
            OpenBL obl = new OpenBL();
            //dt = dt.Add(new TimeSpan(-12, 0, 0, 0));
            List<PunchData> list = obl.PunchData(dt, dt.Add(new TimeSpan(24)), clientId);
            if (list != null && list.Count > 0)
            {
                DateTime dTime = DateTime.Now;
                headCountTitle.InnerHtml = "Head Count Report for: " + list[0].ClientName + 
                    ", " + dt.Year + "/" + dt.Month + "/" + dt.Day + " " + dTime.ToLongTimeString();
                /* parse list */
                int employeeCount = 0;
                Dictionary<String, EmployeePunches> epList = new Dictionary<String, EmployeePunches>();
                for (int i = 0; i < list.Count; i++)
                {
                    /* valid */
                    if( list[i].PunchRound >= list[i].StartDate /*&& 
                        list[i].EndDate >= list[i].PunchRound*/ )
                    {
                        if( epList.ContainsKey(list[i].Id))
                        {
                            epList[list[i].Id].punchCount++;
                            if (list[i].PunchRound > epList[list[i].Id].PunchTime)
                            {
                                epList[list[i].Id].PunchTime = list[i].PunchRound;
                            }
                        }
                        else
                        {
                            EmployeePunches ep = new EmployeePunches();
                            ep.punchCount = 1;
                            ep.Name = list[i].LastName + ", " + list[i].FirstName;
                            ep.Id = list[i].Id;
                            ep.PunchTime = list[i].PunchRound;
                            ep.Shift = list[i].ShiftType;
                            ep.Department = list[i].DepartmentName;
                            employeeCount++;
                            ep.Index = employeeCount;
                            epList.Add(list[i].Id, ep);
                        }
                    }
                }
                if (epList.Count > 0)
                {
                    List<String> rem = new List<string>();
                    foreach(KeyValuePair<String, EmployeePunches>ep in epList)
                    {
                        if (ep.Value.punchCount % 2 == 0)
                            //epList.Remove(ep.Key);
                            rem.Add(ep.Key);
                    }
                    foreach(String s in rem ) epList.Remove(s);

                    List<EmployeePunches> lep = new List<EmployeePunches>();
                    int c = 1;
                    foreach (KeyValuePair<String, EmployeePunches> ep in epList.OrderBy(key => key.Value.Department))
                    {
                        ep.Value.Index = c++;
                        lep.Add(ep.Value);
                    }

                    //foreach (KeyValuePair<String, EmployeePunches> ep in epList) ep.Value.Index = c++;
                    if( lep.Count > 0 ) // if ep.if( epList.Count > 0 )
                    {
                        grdOnPremises.DataSource = lep;
                        grdOnPremises.DataBind();
                    }
                }
                else
                {
                    headCountTitle.InnerHtml = "No Employees On Premises";
                }
            }
            else
                headCountTitle.InnerHtml = "No Employees On Premises";

        }

    }
}