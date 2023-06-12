using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClockTask
/// </summary>
namespace ClockWebServices
{
    public class ClockTask
    {
        public string TaskName { get; set; }
        public string Time { get; set; }
        public bool Result { get; set; }
        public ClockTask()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
    public class ClientRosterLastUpdate
    {
        public string LastUpdate { get; set; }
        public bool Success { get; set; }
        public string Msg { get; set; }
        public List<ClientRoster> Roster { get; set; }
    }
}