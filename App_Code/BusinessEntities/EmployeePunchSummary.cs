using System;
using System.Collections;
namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunchSummary
	{
		private int _clientId;
        private bool _useExactTimes;
        private bool _clientMaintainsEmployeeSchedules = false;
        private int _employeeId;
        private string _employeeFirstName = "";
        private string _employeeLastName = "";
        private string _aidentNumber = "";
		private ShiftType _shiftType = new ShiftType();
        private int _employeeTicketId;
        private int _clientRosterId;
		private string _tempNumber = "";
        private DateTime _ticketEffectiveDateStart = new DateTime(1,1,1);
        private DateTime _ticketEffectiveDateEnd = new DateTime(1, 1, 1);
		private decimal _clientTempNumber = 0M;
        private DateTime _payPeriodEndDate;
        private string _formattedTicketEffectiveDate = "";
        private Ticket _ticket = new Ticket();
        private bool _isRosterEmployee;
        private DateTime _punchDateTime = new DateTime(1, 1, 1);
        private DateTime _roundedPunchDateTime = new DateTime(1, 1, 1);
        private ArrayList _previousPunches = new ArrayList();
        private int _employeePunchId;
        private PunchException _punchException = new PunchException();
        private bool _manualOverride = false;
        private string _lastUpdatedBy = string.Empty;
        private DateTime _lastUpdatedDateTime = new DateTime(1, 1, 1);
        private bool _isApproved = false;
        private string _approvedBy = string.Empty;
        private DateTime _approvedDateTime = new DateTime(1, 1, 1);
        private DateTime _effectiveDateTime = new DateTime(1, 1, 1);
        private DateTime _expirationDateTime = new DateTime(1, 1, 1);
        private bool _calculateWeeklyHours = false;
        private decimal _currentWeeklyHours = 0M;
        private int _deptOverride = 0;
        private int _shiftOverride = 0;

        public EmployeePunchSummary()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public int Location { get; set; }
        public DateTime PunchPic { get; set; }
        public int BiometricResult { get; set; }
        public int DeptOverride { get { return _deptOverride; } set { _deptOverride = value; } }
        public int ShiftOverride { get { return _shiftOverride; } set { _shiftOverride = value; } }

        public bool UseExactTimes
        {
            get { return _useExactTimes; }
            set { _useExactTimes = value; }
        }
        public int EmployeePunchID
        {
            get
            {
                return _employeePunchId;
            }
            set
            {
                _employeePunchId = value;
            }
        }

        public string EmployeeFirstName
        {
            get
            {
                return _employeeFirstName;
            }
            set
            {
                _employeeFirstName = value;
            }
        }

        public string EmployeeLastName
        {
            get
            {
                return _employeeLastName;
            }
            set
            {
                _employeeLastName = value;
            }
        }

        public bool ManualOverride
        {
            get
            {
                return _manualOverride;
            }
            set
            {
                _manualOverride = value;
            }
        }

        public int EmployeeID
        {
            get
            {
                return _employeeId;
            }
            set
            {
                _employeeId = value;
            }
        }

        public string AidentNumber
        {
            get
            {
                return _aidentNumber;
            }
            set
            {
                _aidentNumber = value;
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

        public int EmployeeTicketID
        {
            get
            {
                return _employeeTicketId;
            }
            set
            {
                _employeeTicketId = value;
            }
        }

        public int ClientRosterID
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

        public DateTime TicketEffectiveDateStart
		{
			get
			{
                return _ticketEffectiveDateStart;
			}
			set
			{
                _ticketEffectiveDateStart = value;
			}
		}

        public DateTime TicketEffectiveDateEnd
        {
            get
            {
                return _ticketEffectiveDateEnd;
            }
            set
            {
                _ticketEffectiveDateEnd = value;
            }
        }

        public DateTime PunchDateTime
        {
            get
            {
                return _punchDateTime;
            }
            set
            {
                _punchDateTime = value;
            }
        }

        public DateTime RoundedPunchDateTime
        {
            get
            {
                return _roundedPunchDateTime;
            }
            set
            {
                _roundedPunchDateTime = value;
            }
        }

		public decimal ClientTempNumber
		{
			get
			{
				return _clientTempNumber;
			}
			set
			{
				_clientTempNumber = value;
			}
		}

        public DateTime PayPeriodEndDate
        {
            get
            {
                return _payPeriodEndDate;
            }
            set
            {
                _payPeriodEndDate = value;
            }
        }
        public string FormattedTicketEffectiveDate
        {
            get
            {
                return _formattedTicketEffectiveDate;
            }
            set
            {
                _formattedTicketEffectiveDate = value;
            }
        }

        public Ticket TicketInfo
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }

        public bool IsRosterEmployee
        {
            get
            {
                return _isRosterEmployee;
            }
            set
            {
                _isRosterEmployee = value;
            }
        }

        public ArrayList PreviousPunches
        {
            get
            {
                return _previousPunches;
            }
            set
            {
                _previousPunches = value;
            }
        }

        public PunchException PunchExceptionInfo
        {
            get
            {
                return _punchException;
            }
            set
            {
                _punchException = value;
            }
        }

        public string LastUpdatedBy
        {
            get
            {
                return _lastUpdatedBy;
            }
            set
            {
                _lastUpdatedBy = value;
            }
        }

        public DateTime LastUpdatedDateTime
        {
            get
            {
                return _lastUpdatedDateTime;
            }
            set
            {
                _lastUpdatedDateTime = value;
            }
        }

        public bool ClientMaintainsEmployeeSchedules
        {
            get
            {
                return _clientMaintainsEmployeeSchedules;
            }
            set
            {
                _clientMaintainsEmployeeSchedules = value;
            }
        }

        public bool IsApproved
        {
            get
            {
                return _isApproved;
            }
            set
            {
                _isApproved = value;
            }
        }

        public string ApprovedBy
        {
            get
            {
                return _approvedBy;
            }
            set
            {
                _approvedBy = value;
            }
        }

        public DateTime ApprovedDateTime
        {
            get
            {
                return _approvedDateTime;
            }
            set
            {
                _approvedDateTime = value;
            }
        }

        public DateTime EffectiveDateTime
        {
            get
            {
                return _effectiveDateTime;
            }
            set
            {
                _effectiveDateTime = value;
            }
        }

        public DateTime ExpirationDateTime
        {
            get
            {
                return _expirationDateTime;
            }
            set
            {
                _expirationDateTime = value;
            }
        }

        public decimal CurrentWeeklyHours
        {
            get
            {
                return _currentWeeklyHours;
            }
            set
            {
                _currentWeeklyHours = value;
            }
        }

        public bool CalculateWeeklyHours
        {
            get
            {
                return _calculateWeeklyHours;
            }
            set
            {
                _calculateWeeklyHours = value;
            }
        }
	}
}
