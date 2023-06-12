using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for DailyDispatchInfo
/// </summary>


namespace MSI.Web.MSINet.BusinessEntities
{
    [DataContract]
    public class DailyDispatchSettings
    {
        [DataMember]
        public string officeCd { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public int shiftType { get; set; }
        [DataMember]
        public bool weeklyReport { get; set; }
    }
    [DataContract]
    public class DailyDispatchInfoSummary
    {
        public DailyDispatchInfoSummary()
        {
            //
            // TODO: Add constructor logic here
            //
        }

    }
    [DataContract]
    public class DailyDispatchInfo
    {
        public DailyDispatchInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //client_id, dispatch_dt, office_id, shift_type, tot_sent, regs, temps_ordered, temps_sent, unfilled, extras, notes, created_by, created_dt
        [DataMember]
        public int clientId { get; set; }
        [DataMember]
        public string dispatchDt { get; set; }
        [DataMember]
        public string officeName { get; set; }
        [DataMember]
        public int officeId { get; set; }
        [DataMember]
        public string officeCd { get; set; }
        [DataMember]
        public int shiftType { get; set; }
        [DataMember]
        public int totSent { get; set; }
        [DataMember]
        public int regs { get; set; }
        [DataMember]
        public int tempsOrdered { get; set; }
        [DataMember]
        public int tempsSent { get; set; }
        [DataMember]
        public int unfilled { get; set; }
        [DataMember]
        public int extras { get; set; }
        [DataMember]
        public string notes { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public string createdDt { get; set; }
        [DataMember]
        public int transported { get; set; }
        public override String ToString()
        {
            return "" + clientId + ", " + dispatchDt + ", " + officeId + ", " + shiftType + ", " +
                totSent + ", " + regs + ", " + tempsOrdered + ", " + tempsSent + ", " + unfilled + ", " +
                extras + ", " + notes + ", " + createdBy + ", " + createdDt + ", " + transported;
        }
    }
}