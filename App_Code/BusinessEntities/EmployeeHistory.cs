using System;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Runtime.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// Summary description for Content.
    /// </summary>
    /// 
    [DataContract]
    public class EmployeeHistoryWeekSupervisor
    {
        [DataMember]
        public string Aident { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public decimal[] Hours { get; set; }
        [DataMember]
        public decimal PayRate { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Shift { get; set; }
        [DataMember]
        public int CostCenter { get; set; }
        [DataMember]
        public string Supervisor { get; set; }

        public EmployeeHistoryWeekSupervisor()
        {
            Hours = new decimal[7];
        }
    }

    [DataContract]
    public class EmployeeHistory
    {
        private int _idx;
        private int _empId;
        private string _ssn = "";
        private string _aident = "";
        private string _firstName = "";
        private string _lastName = "";
        private string _middleName = "";
        private string _addr1 = "";
        private string _addr2 = "";
        private string _city = "";
        private string _zip = "";
        private string _zip4 = "";
        private string _state = "";
        private string _email = "";
        private DateTime _created;
        private DateTime _updated;
        private string _updatedBy;
        private string _phonePrefix = "";
        private string _phoneAreaCode = "";
        private string _phoneLast4 = "";
        private int _locationId = 0;
        private string _backgroundColor = "";
        private ArrayList _workSummaries = new ArrayList();
        private bool _hasInvalidWorkSummaries = false;
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private int _clientId;
        private decimal _totalRegularHours = 0M;
        private decimal _totalOTHours = 0M;
        private decimal _totalHours = 0M;
        private decimal _totalHoursAllDepts = 0M;
        private double _bonus = 0.0f;
        public bool Update { get; set; }
        public bool Modify { get; set; }
        private string _tempNumber = "";
        private bool _hasUnapprovedHours = false;
        private decimal _overrideBreakTime = 0;
        public double BillRate { get; set; }
        public string PrimaryRole { get; set; }
        public decimal RosterPayRate { get; set; }
        public DateTime FirstPunch { get; set; }
        public Boolean HasLatePunches { get; set; }
        public Boolean MissingBreakPunches { get; set; }
        public string ClientEmployeeId { get; set; }

        private DailySummary _mondaySummary = new DailySummary();
        private DailySummary _tuesdaySummary = new DailySummary();
        private DailySummary _wednesdaySummary = new DailySummary();
        private DailySummary _thursdaySummary = new DailySummary();
        private DailySummary _fridaySummary = new DailySummary();
        private DailySummary _saturdaySummary = new DailySummary();
        private DailySummary _sundaySummary = new DailySummary();

        private ArrayList _dailySummaries = new ArrayList();
        private decimal _minWage = 0M;
        private decimal _defaultPayRate = 0M;
        private decimal _payRate = 0M;
        private string _defaultJobCode = string.Empty;
        private string _jobCode = string.Empty;
        private int _clientRosterId = 0;

        public string Supervisor { get; set; }
        public int CostCenter { get; set; }

        public EmployeeHistory()
        {
            //
            // TODO: Add constructor logic here
            //
            _dailySummaries.Add(new DailySummary(DayOfWeek.Monday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Tuesday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Wednesday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Thursday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Friday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Saturday));
            _dailySummaries.Add(new DailySummary(DayOfWeek.Sunday));
        }

        public decimal OverrideBreakTime
        {
            get
            {
                return _overrideBreakTime;
            }
            set
            {
                _overrideBreakTime = value;
            }
        }

        public double Bonus
        {
            get
            {
                return _bonus;
            }
            set
            {
                _bonus = value;
            }
        }

        public ArrayList DailySummaries
        {
            get
            {
                return _dailySummaries;
            }
            set
            {
                _dailySummaries = value;
            }
        }

        public int ItemIdx
        {
            get
            {
                return _idx;
            }
            set
            {
                _idx = value;
            }
        }

        public int EmployeeID
        {
            get
            {
                return _empId;
            }
            set
            {
                _empId = value;
            }
        }
        [DataMember]
        public string SSN
        {
            get
            {
                return _ssn;
            }
            set
            {
                _ssn = value;
            }
        }

        [DataMember]
        public string AIdentNumber
        {
            get
            {
                return _aident;
            }
            set
            {
                _aident = value;
            }
        }
        [DataMember]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }
        [DataMember]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }
        public string LocationName { get; set; }

        [DataMember]
        public string MiddleName
        {
            get
            {
                return _middleName;
            }
            set
            {
                _middleName = value;
            }
        }
        [DataMember]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }

        }
        public string FullName
        {
            get
            {
                string fullName = this._lastName.Trim();

                if (this._firstName.Trim().Length > 0)
                    fullName += " " + this._firstName;

                if (this._middleName.Trim().Length > 0)
                    fullName += " " + this._middleName;

                return fullName;
            }
        }

        public string BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
            }
        }

        public bool HasInvalidWorkSummaries
        {
            get
            {
                return _hasInvalidWorkSummaries;
            }
            set
            {
                _hasInvalidWorkSummaries = value;
            }
        }

        public ArrayList WorkSummaries
        {
            get
            {
                return _workSummaries;
            }
            set
            {
                _workSummaries = value;
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return _startDateTime;
            }
            set
            {
                _startDateTime = value;
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return _endDateTime;
            }
            set
            {
                _endDateTime = value;
            }
        }

        public int ClientID
        {
            get
            {
                return _clientId;
            }
            set
            {
                _clientId = value;
            }
        }

        public string TempNumber
        {
            get
            {
                return _tempNumber;
            }
            set
            {
                _tempNumber = value;
            }
        }

        public decimal TotalHours
        {
            get
            {
                return _totalHours;
            }
            set
            {
                _totalHours = value;
            }
        }
        public decimal TotalHoursAllDepts
        {
            get
            {
                return _totalHoursAllDepts;
            }
            set
            {
                _totalHoursAllDepts = value;
            }
        }

        public decimal TotalRegularHours
        {
            get
            {
                //if (_totalHours > 40)
                //    return 40;
                //else
                //    return _totalHours;
                return _totalRegularHours;
            }
            set
            {
                _totalRegularHours = value;
            }
        }

        public decimal TotalOTHours
        {
            get
            {
                return _totalOTHours;
                //if (_totalHours > 40)
                //    return _totalHours - 40;
                //else
                //    return 0M;
            }
            set
            {
                _totalOTHours = value;
            }
        }

        public decimal MondayHours
        {
            get
            {
                return _mondaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _monHours = value;
            //}
        }

        public decimal TuesdayHours
        {
            get
            {
                return _tuesdaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _tueHours = value;
            //}
        }

        public decimal WednesdayHours
        {
            get
            {
                return _wednesdaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _wedHours = value;
            //}
        }

        public decimal ThursdayHours
        {
            get
            {
                return _thursdaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _thuHours = value;
            //}
        }

        public decimal FridayHours
        {
            get
            {
                return _fridaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _friHours = value;
            //}
        }

        public decimal SaturdayHours
        {
            get
            {
                return _saturdaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _satHours = value;
            //}
        }

        public decimal SundayHours
        {
            get
            {
                return _sundaySummary.TotalHoursWorked;
            }
            //set
            //{
            //    _sunHours = value;
            //}
        }

        public DailySummary MondaySummary
        {
            get
            {
                return _mondaySummary;
            }
            set
            {
                _mondaySummary = value;
            }
        }

        public DailySummary TuesdaySummary
        {
            get
            {
                return _tuesdaySummary;
            }
            set
            {
                _tuesdaySummary = value;
            }
        }

        public DailySummary WednesdaySummary
        {
            get
            {
                return _wednesdaySummary;
            }
            set
            {
                _wednesdaySummary = value;
            }
        }

        public DailySummary ThursdaySummary
        {
            get
            {
                return _thursdaySummary;
            }
            set
            {
                _thursdaySummary = value;
            }
        }

        public DailySummary FridaySummary
        {
            get
            {
                return _fridaySummary;
            }
            set
            {
                _fridaySummary = value;
            }
        }

        public DailySummary SaturdaySummary
        {
            get
            {
                return _saturdaySummary;
            }
            set
            {
                _saturdaySummary = value;
            }
        }

        public DailySummary SundaySummary
        {
            get
            {
                return _sundaySummary;
            }
            set
            {
                _sundaySummary = value;
            }
        }

        public bool HasUnapprovedHours
        {
            get
            {
                return _hasUnapprovedHours;
            }
            set
            {
                _hasUnapprovedHours = value;
            }
        }

        public decimal MinimumWage
        {
            get
            {
                return _minWage;
            }
            set
            {
                _minWage = value;
            }
        }

        public decimal DefaultPayRate
        {
            get
            {
                return _defaultPayRate;
            }
            set
            {
                _defaultPayRate = value;
            }
        }

        public decimal PayRate
        {
            get
            {
                return _payRate;
            }
            set
            {
                _payRate = value;
            }
        }

        public string DefaultJobCode
        {
            get
            {
                return _defaultJobCode;
            }
            set
            {
                _defaultJobCode = value;
            }
        }

        public string JobCode
        {
            get
            {
                return _jobCode;
            }
            set
            {
                _jobCode = value;
            }
        }

        public decimal GetEmployeePayRate()
        {
            decimal payRate = _payRate;

            if (payRate == 0M)
            {
                payRate = _defaultPayRate;
            }

            if (payRate == 0M)
            {
                payRate = _minWage;
            }

            return payRate;
        }

        public string GetEmployeeJobCode()
        {
            string jobCode = _jobCode;

            if (jobCode == string.Empty)
            {
                jobCode = _defaultJobCode;
            }

            return jobCode;
        }

        public int ClientRosterId
        {
            get
            {
                return _clientRosterId;
            }
            set
            {
                _clientRosterId = value;
            }
        }

        [DataMember]
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        [DataMember]
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public int LocationId
        {
            get
            {
                return _locationId;
            }
            set
            {
                _locationId = value;
            }
        }
        [DataMember]
        public string PhonePrefix
        {
            get
            {
                return _phonePrefix;
            }
            set
            {
                _phonePrefix = value;
            }
        }
        [DataMember]
        public string PhoneAreaCode
        {
            get
            {
                return _phoneAreaCode;
            }
            set
            {
                _phoneAreaCode = value;
            }
        }
        [DataMember]
        public string PhoneLast4
        {
            get
            {
                return _phoneLast4;
            }
            set
            {
                _phoneLast4 = value;
            }
        }
        [DataMember]
        public string Addr1
        {
            get { return _addr1; }
            set { _addr1 = value; }
        }
        [DataMember]
        public string Addr2
        {
            get { return _addr2; }
            set { _addr2 = value; }
        }
        [DataMember]
        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }
        [DataMember]
        public string Zip4
        {
            get { return _zip4; }
            set { _zip4 = value; }
        }
        [DataMember]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }
        public DateTime Updated
        {
            get { return _updated; }
            set { _updated = value; }
        }
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; }
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
