using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{

/*    aident_number last_name   first_name client_name shift_name department_name Work Day
        96403	Kelty Suzanne ARYZTA CHICAGO  1st Shift   PACKING 30B 6am	2018-04-02 
*/

    public class PunchLocation
    {
        public PunchLocation(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        override public string ToString()
        {

            return "" + Latitude + "," + Longitude;
        }
    }
    public class EmployeeStatus
    {
        public string EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ShiftName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime Day { get; set; }
        public string ClientName { get; set; }

        override public String ToString()
        {
            string ret = "";
            ret = "\"" + EmployeeID + "\",";
            ret += "\"" + LastName + "\",";
            ret += "\"" + FirstName + "\",";
            ret += "\"" + ShiftName + "\",";
            ret += "\"" + DepartmentName + "\",";
            ret += "\"" + Day.ToShortDateString() + "\",";
            ret += "\"" + ClientName + "\",";
            return ret;
        }
    }

    public class EmployeeWorkSummary
    {
        private DateTime _workDate;
        private int _checkInEmployeePunchId = 0;
        private int _checkOutEmployeePunchId = 0;
        private DateTime _checkInDateTime;
        private DateTime _checkOutDateTime;
        private DateTime _roundedCheckInDateTime;
        private DateTime _roundedCheckOutDateTime;
        private ShiftType _shiftType = new ShiftType();
        private Department _department = new Department();
        private ArrayList _punchTimes = new ArrayList();
        private double _hoursWorked = 0;
        private DateTime _summaryShiftStartDateTime;
        private DateTime _summaryShiftEndDateTime;
        private bool _checkInApproved = false;
        private bool _checkOutApproved = false;
        private String _checkInCreatedBy = "";
        private DateTime _checkOutCreatedDt;
        private String _checkOutCreatedBy = "";
        private DateTime _checkInCreatedDt;
        private Shift _shiftInfo = new Shift();
        private int _clientID;
        private String _badge;
        private int _costCenter;
        public String approvedBy;
        public String locationName;
        public DayOfWeek DaySummaryAppliesTo { get; set; }
        public Boolean CheckInManualOverrideFlag { get; set; }
        public Boolean CheckOutManualOverrideFlag { get; set; }
        public DateTime CheckInLastUpdatedDate { get; set; }
        public DateTime CheckOutLastUpdatedDate { get; set; }
        public string CheckInLastUpdatedBy { get; set; }
        public string CheckOutLastUpdatedBy { get; set; }
        public double BreakTime { get; set; }
        public int CostCenter
        {
            get { return _costCenter; }
            set { _costCenter = value; }
        }

        public TimeSpan MinutesLate { get; set; }

        public TimeSpan BreakAmount { get; set; }

        public EmployeeWorkSummary()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public String CheckInCreatedBy
        {
            get
            {
                return _checkInCreatedBy;
            }
            set
            {
                _checkInCreatedBy = value;
            }
        }
        public DateTime CheckInCreatedDt
        {
            get
            {
                return _checkInCreatedDt;
            }
            set
            {
                _checkInCreatedDt = value;
            }
        }
        public String CheckOutCreatedBy
        {
            get
            {
                return _checkOutCreatedBy;
            }
            set
            {
                _checkOutCreatedBy = value;
            }
        }
        public DateTime CheckOutCreatedDt
        {
            get
            {
                return _checkOutCreatedDt;
            }
            set
            {
                _checkOutCreatedDt = value;
            }
        }

        public String Badge
        {
            get
            {
                return _badge;
            }
            set
            {
                _badge = value;
            }
        }
        public int CheckInEmployeePunchID
        {
            get
            {
                return _checkInEmployeePunchId;
            }
            set
            {
                _checkInEmployeePunchId = value;
            }
        }
        public int ClientID
        {
            get
            {
                return _clientID;
            }
            set
            {
                _clientID = value;
            }
        }

        public int CheckOutEmployeePunchID
        {
            get
            {
                return _checkOutEmployeePunchId;
            }
            set
            {
                _checkOutEmployeePunchId = value;
            }
        }

        public DateTime WorkDate
        {
            get
            {
                return _workDate;
            }
            set
            {
                _workDate = value;
            }
        }

        public DateTime CheckInDateTime
        {
            get
            {
                return _checkInDateTime;
            }
            set
            {
                _checkInDateTime = value;
            }
        }

        public DateTime CheckOutDateTime
        {
            get
            {
                return _checkOutDateTime;
            }
            set
            {
                _checkOutDateTime = value;
            }
        }

        public DateTime RoundedCheckInDateTime
        {
            get
            {
                return _roundedCheckInDateTime;
            }
            set
            {
                _roundedCheckInDateTime = value;
            }
        }

        public DateTime RoundedCheckOutDateTime
        {
            get
            {
                return _roundedCheckOutDateTime;
            }
            set
            {
                _roundedCheckOutDateTime = value;
            }
        }

        public ShiftType ShiftTypeInfo
        {
            get
            {
                return _shiftType;
            }
            set
            {
                _shiftType = value;
            }
        }

        public Department DepartmentInfo
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        public double HoursWorked
        {
            get
            {
                return _hoursWorked;
            }
            set
            {
                _hoursWorked = value;
            }
        }

        public ArrayList PunchTimes
        {
            get
            {
                return _punchTimes;
            }
            set
            {
                _punchTimes = value;
            }
        }

        public DateTime SummaryShiftStartDateTime
        {
            get
            {
                return _summaryShiftStartDateTime;
            }
            set
            {
                _summaryShiftStartDateTime = value;
            }
        }

        public DateTime SummaryShiftEndDateTime
        {
            get
            {
                return _summaryShiftEndDateTime;
            }
            set
            {
                _summaryShiftEndDateTime = value;
            }
        }

        public bool CheckInApproved
        {
            get
            {
                return _checkInApproved;
            }
            set
            {
                _checkInApproved = value;
            }
        }
        public bool CheckOutApproved
        {
            get
            {
                return _checkOutApproved;
            }
            set
            {
                _checkOutApproved = value;
            }
        }

        public Shift ShiftInfo
        {
            get
            {
                return _shiftInfo;
            }
            set
            {
                _shiftInfo = value;
            }
        }
        public PunchLocation CheckInLocation { get; set; }
        public PunchLocation CheckOutLocation { get; set; }

    }
}