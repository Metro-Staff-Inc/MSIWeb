using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for PhoneBlast
/// </summary>
/// 
namespace MSI.Web.MSINet.BusinessEntities
{

    public class PBEmployeeContact
    {
        public String Id { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public DateTime ContactDate { get; set; }
        public String MSIPhoneNum { get; set; }
        public String MSITextNum { get; set; }
        public String EmployeePhoneNum { get; set; }
    }

    public class PhoneBlastList
    {
        public int PhoneBlastListID { get; set; }
        public string Description { get; set; }
    }

    public class PhoneBlast
    {
        public PhoneBlast()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public int Language { get; set; }
        public int Response { get; set; }
        public DateTime ResponseDate { get; set; }

        /* class tracks lists of employees to be hired */
    }

    public class RecruitPoolItem : System.IComparable
    {
        public RecruitPoolItem()
        {
            //
            // TODO: Add constructor logic here
            //
            DaysWorked = new List<int>();
            Depts = new List<String>();
        }
        public int TotalDaysWorked { get; set; }
        public int DeptId { get; set; }
        public string FirstPunch { get; set; }
        public string BadgeNumber { get; set; }
        public string ClientNumber { get; set; }
        public string DnrReason { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> DaysWorked { get; set; }
        public List<String> Depts { get; set; }
        public string Zip { get; set; }
        public string Addr { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string FingerprintsExist { get; set; }
        public string Notes { get; set; }

        public int CompareTo(object obj)
        {
            if (!(obj is RecruitPoolItem))
                throw new InvalidCastException("This object is not of type Days Worked Item!");

            RecruitPoolItem myItem = (RecruitPoolItem)obj;

            return myItem.TotalDaysWorked.CompareTo(this.TotalDaysWorked);
        }
    }

    public class RecruitPool
    {
        public RecruitPool()
        {
            RecruitPoolCollection = new List<RecruitPoolItem>();
        }
        public List<RecruitPoolItem> RecruitPoolCollection { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ClientID { get; set; }
        public int DNRClientID { get; set; }
        public int DepartmentID { get; set; }
        public int ShiftType { get; set; }
        public int LocationID { get; set; }
        public int MinDays;
    }
}
