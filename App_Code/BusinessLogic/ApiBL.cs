using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiWebServices_EmployeeInfo;
using ApiWebServices_HoursData;
using ApiWebServices_PunchData;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using PunchClock;

/// <summary>
/// Summary description for ApiBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class ApiBL
    {
        public ApiBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /* retrieve flat punches and convert to a layered object */
        public PunchResponse RetrievePunches(PunchRequest pr)
        {
            ApiDA apida = new ApiDA();

            pr.start = Convert.ToDateTime(pr.StartDate);
            pr.end = Convert.ToDateTime(pr.EndDate);
            if ((pr.end - pr.start) > new TimeSpan(14, 0, 0, 0))
            {
                PunchResponse prs = new PunchResponse();
                prs.Msg = "Currently, timespan must be less than 14 days.  Timespan is range of employees starting shifts on StartDate through EndDate inclusive.  Note that employees may have overnight shifts ending on the day after EndDate";
                return prs;
            }
            if ((pr.end - pr.start) < new TimeSpan(0, 0, 0, 0))
            {
                PunchResponse prs = new PunchResponse();
                prs.Msg = "EndDate must be greater than Startdate.  Timespan is range of employees starting shifts on StartDate through EndDate inclusive.";
                return prs;
            }

            /* Get flat data and convert */
            PunchResponseFlat prf = apida.RetrievePunchesFlat(pr);
            PunchResponse resp = new PunchResponse();
            resp.Success = prf.Success;
            resp.Msg = prf.Msg;


            foreach( PunchData pd in prf.Data )
            {
                /* Client is top level */
                if( resp.Client == null )
                {
                    resp.Client = new Client(pd.ClientID, pd.ClientName);
                }
                Location loc; 
                if( !resp.Client.Locations.TryGetValue(pd.LocationID, out loc) )
                {
                    loc = new Location(pd.LocationID, pd.LocationName);
                    resp.Client.Locations.Add(pd.LocationID, loc);
                }
                Department dep;
                if( !loc.Departments.TryGetValue(pd.DepartmentId, out dep) ) 
                {
                    dep = new Department(pd.DepartmentId, pd.DepartmentName);
                    loc.Departments.Add(pd.DepartmentId, dep);
                }
                Shift shift;
                if(!dep.Shifts.TryGetValue(pd.ShiftType, out shift) ) 
                {
                    shift = new Shift(0, pd.ShiftType, pd.ShiftStart, pd.ShiftEnd);
                    dep.Shifts.Add(pd.ShiftType, shift);
                }
                Employee emp;
                if (!shift.Employees.TryGetValue(pd.Aident, out emp))
                {
                    emp = new Employee(pd.Aident, pd.LastName, pd.FirstName);
                    shift.Employees.Add(emp.ID, emp);
                }
                if( pd.PunchDate.Year > 2000 )
                {
                    Punch punch = new Punch(Convert.ToDateTime(pd.PunchDate), Convert.ToDateTime(pd.RoundedPunchDate));
                    emp.Punches.Add(punch);
                }                
            }
            return resp;
        }

        public EmployeeInfoResponse GetEmployeeInfo(EmployeeInfo e)
        {
            ApiDA api = new ApiDA();
            return api.GetEmployeeInfo(e);
        }

        public  HoursResponseFlat ClientHours(HoursRequest pr)
        {
            pr._weekEndDate = Convert.ToDateTime(pr.WeekEndDate);
            ApiDA apida = new ApiDA();
            return apida.RetrieveHoursFlat(pr);
        }

        internal BridgfordOut BridgfordEmployeeData(BridgfordIn brIn)
        {
            ApiDA apiDA = new ApiDA();
            return apiDA.BridgfordEmployeeData(brIn);
        }

        public EmployeeInfoResponse UpdateEmployeeInfo(EmployeeInfo ei)
        {
            string ssn = "";
            for( int i=0; i<ei.SSN.Length; i++ )
            {
                if (ei.SSN[i] < '0' || ei.SSN[i] > '9') continue;
                ssn = ssn + ei.SSN[i];
            }
            ei.SSN = ssn;
            string phone = "";
            for (int i = 0; i < ei.Phone.Length; i++)
            {
                if (ei.Phone[i] < '0' || ei.Phone[i] > '9') continue;
                phone = phone + ei.Phone[i];
            }
            ei.Phone = phone;
            ApiDA apiDA = new ApiDA();
            return apiDA.UpdateEmployeeInfo(ei);
        }

        public PunchResponseFlat RetrievePunchesFlat(PunchRequest pr)
        {
            pr.start = Convert.ToDateTime(pr.StartDate);
            pr.end = Convert.ToDateTime(pr.EndDate);
            ApiDA apida = new ApiDA();
            if ((pr.end-pr.start) > new TimeSpan(14, 0, 0, 0))
            {
                PunchResponseFlat prs = new PunchResponseFlat();
                prs.Msg = "Currently, timespan must be less than 14 days.  Timespan is range of employees starting shifts on StartDate through EndDate inclusive.  Note that employees may have overnight shifts ending on the day after EndDate";
                return prs;
            }
            if ((pr.end-pr.start) < new TimeSpan(0, 0, 0, 0))
            {
                PunchResponseFlat prs = new PunchResponseFlat();
                prs.Msg = "EndDate must be greater than Startdate.  Timespan is range of employees starting shifts on StartDate through EndDate inclusive.";
                return prs;
            }
            return apida.RetrievePunchesFlat(pr);
        }
        public MobileDataOut MobilePunch(MobileDataIn mdi)
        {
            if (mdi.PhoneDateTime == "") mdi.PhoneDateTime = "8675875578";
            mdi._phoneDateTime = DateTimeHelpers
                .MillisecondsSince1970ToDateTime(Convert.ToDouble(
                    mdi.PhoneDateTime));
            if (mdi.ClientDateTime == "") mdi.ClientDateTime = "123412344";
            mdi._clientDateTime = DateTimeHelpers
                .MillisecondsSince1970ToDateTime(Convert.ToDouble(
                    mdi.ClientDateTime));

            ApiDA apida = new ApiDA();
            MobileDataOut mdo = apida.MobilePunch(mdi);

            HelperFunctions.SavePunchClockB64AsJpg(mdi);
 
            return mdo;
        }
    }
}
 