using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// Pay Rate info for each department
    /// 
    public class PayRate
    {
        public int PayRateID { get; set; }
        public Decimal HourlyRate { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
    }

    [DataContract]
    public class DepartmentSupervisorReq
    {
        public DepartmentSupervisorReq()
        {

        }
        public DepartmentSupervisorReq(Guid uid, int clientId)
        {
            UserId = uid;
            ClientId = clientId;
        }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public int ClientId { get; set; }
    }

    [DataContract]
    public class DepartmentSupervisor
    {
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public int LocationId { get; set; }
        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        public int DepartmentId { get; set; }
        [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public bool DepartmentViewable { get; set; }
        [DataMember]
        public int ShiftType { get; set; }
        [DataMember]
        public int SupervisorId { get; set; }
    }
    /// <summary>
    /// Summary description for DepartmentInfo
    /// </summary>
    public class DepartmentInfo
    {
        //SELECT c.client_name, d.department_name, d.department_id, s.shift_desc, s.shift_type, s.shift_id, cl.location_name, 
        // csl.shift_start_time, csl.shift_end_time, 
        //    csl.shift_break_hours, csl.shift_break_hours, csl.tracking_start_time, csl.tracking_end_time, csl.void, cp.client_pay_id, cp.pay_rate, cp.effective_dt, cp.expiration_dt
        public int      ClientId { get; set; }    
        public string   ClientName { get; set; }
        public int      LocationId { get; set; }
        public string   LocationName { get; set; }
        public int      DepartmentId { get; set; }
        public string   DepartmentName { get; set; }
        public int      ShiftId { get; set; }
        public string   ShiftName { get; set; }
        public int      ShiftType { get; set; }
        public string   ShiftStart { get; set; }
        public string   ShiftEnd { get; set; }
        public decimal  ShiftBreak { get; set; }
        public string   TrackingStart { get; set; }
        public string   TrackingEnd { get; set; }
        public bool  Active { get; set; }
        public List<PayRate> PayRate { get; set; }
                
        public DepartmentInfo()
        {
            //
            // TODO: Add constructor logic here
            //
            // initiate the payrate list.  Length should rarely be more than 1, or else there is an error
            PayRate = new List<PayRate>();
        }
        public DepartmentInfo(String d) { }

        /* equals method */
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            DepartmentInfo di = obj as DepartmentInfo;
            if (di == null) return false;
            return this.ShiftId == di.ShiftId && this.LocationId == di.LocationId;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}