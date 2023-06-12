using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Ticket
	{
        private int _ticketId;
		private int _clientId;
		private int _locationId;
		private int _shiftId;
		private int _officeId;
		private int _ticketInstance;
		private DateTime _ticketDate;
		private string _ticketNumber = "";
		private Status _ticketStatus = new Status();
		private Status _previousTicketStatus = new Status();
		ArrayList _employees = new ArrayList();
		DateTime _ticketEffectiveDate;
		DateTime _ticketExpirationDate;
        DateTime _payPeriodEndDate;
		private string _clientSupervisorFirstName = "";
		private string _clientSupervisorLastName = "";
		private string _shiftStartTime = "";
		private string _shiftEndTime = "";
        private string _alternateShiftStartTime = "";
		private string _eTicketNumber = "";
        private Department _department = new Department();

        private bool _hasOvertimeEmployees = false;

        private string _backgroundColor = "";

		public Ticket()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public Ticket(int clientId, int locationId, ClientShiftLocation shiftInfo, Office officeInfo, int ticketInstance, DateTime ticketDate, string ticketNumber, string shiftStartTime, string shiftEndTime, string clientSupervisorFirstName, string clientSupervisorLastName, string eTicketNumber, Status ticketStatus, bool readOnly, DateTime createdDt, string createdBy, DateTime lastUpdatedDt, string lastUpdatedBy, DateTime ticketEffectiveDate, DateTime ticketExpirationDate, DateTime payPeriodEndDate)
		{
			_clientId = clientId;
			_locationId = locationId;
			_shiftId = shiftInfo.ShiftID;
			_officeId = officeInfo.OfficeID;
			_ticketInstance = ticketInstance;
			_ticketDate = ticketDate;
			_ticketNumber = ticketNumber;
			_clientSupervisorFirstName = clientSupervisorFirstName;
			_clientSupervisorLastName = clientSupervisorLastName;
			_shiftStartTime = shiftStartTime;
			_shiftEndTime = shiftEndTime;
			_eTicketNumber = eTicketNumber;
			_ticketStatus = ticketStatus;
			_ticketEffectiveDate = ticketEffectiveDate;
			_ticketExpirationDate = ticketExpirationDate;
            _payPeriodEndDate = payPeriodEndDate;
		}

		public Ticket ( int clientId, int locationId, int shiftId, int officeId )
		{
			_clientId = clientId;
			_locationId = locationId;
			_shiftId = shiftId;
			_officeId = officeId;
		}

		public Ticket ( int clientId, int locationId, int shiftId, int officeId, DateTime ticketDate )
		{
			_clientId = clientId;
			_locationId = locationId;
			_shiftId = shiftId;
			_officeId = officeId;
			_ticketDate = ticketDate;
		}

        public int TicketID
        {
            get
            {
                return _ticketId;
            }
            set
            {
                _ticketId = value;
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

		public int LocationID
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

		public int ShiftID
		{
			get
			{
				return _shiftId;
			}
			set
			{
				_shiftId = value;
			}
		}

		public int OfficeID
		{
			get
			{
				return _officeId;
			}
			set
			{
				_officeId = value;
			}
		}

		public int TicketInstance
		{
			get
			{
				return _ticketInstance;
			}
			set
			{
				_ticketInstance = value;
			}
		}

		public DateTime TicketDate
		{
			get
			{
				return _ticketDate;
			}
			set
			{
				_ticketDate = value;
			}
		}

		public string FormattedTicketDate
		{
			get
			{
				return _ticketDate.ToString ( "MM/dd/yyyy" );
			}
			set
			{
				_ticketDate = DateTime.Parse ( value );
			}
		}

		public string TicketNumber
		{
			get
			{
				return _ticketNumber;
			}
			set
			{
				_ticketNumber = value;
			}
		}

		public Status TicketStatus
		{
			get
			{
				return _ticketStatus;
			}
			set
			{
				_ticketStatus = value;
			}
		}

		public Status PreviousTicketStatus
		{
			get
			{
				return _previousTicketStatus;
			}
			set
			{
				_previousTicketStatus = value;
			}
		}

		public DateTime TicketEffectiveDate
		{
			get
			{
				return _ticketEffectiveDate;
			}
			set
			{
				_ticketEffectiveDate = value;
			}
		}

		public string FormattedTicketEffectiveDate
		{
			get
			{
				return _ticketEffectiveDate.ToString ( "MM/dd/yyyy" );
			}
			set
			{
				_ticketEffectiveDate = DateTime.Parse ( value );
			}
		}

		public DateTime TicketExpirationDate
		{
			get
			{
				return _ticketExpirationDate;
			}
			set
			{
				_ticketExpirationDate = value;
			}
		}
		
		public string FormattedTicketExpirationDate
		{
			get
			{
				return _ticketExpirationDate.ToString ( "MM/dd/yyyy" );
			}
			set
			{
				_ticketExpirationDate = DateTime.Parse ( value );
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

        public string FormattedPayPeriodEndDate
        {
            get
            {
                return _payPeriodEndDate.ToString("MM/dd/yyyy");
            }
            set
            {
                _payPeriodEndDate = DateTime.Parse(value);
            }
        }

		public ArrayList Employees
		{
			get
			{
				return _employees;
			}
			set
			{
				_employees = value;
			}
		}

		public string ClientSupervisorFirstName
		{
			get
			{
				return _clientSupervisorFirstName;
			}
			set
			{
				_clientSupervisorFirstName = value;
			}
		}

		public string ClientSupervisorLastName
		{
			get
			{
				return _clientSupervisorLastName;
			}
			set
			{
				_clientSupervisorLastName = value;
			}
		}

		public string ShiftStartTime
		{
			get
			{
				return _shiftStartTime;
			}
			set
			{
				_shiftStartTime = this.formatTime ( value );
			}
		}

		public string FormattedShiftStartTime
		{
			get
			{
				return formatTimeAMPM ( _shiftStartTime );
			}
			set
			{
				_shiftStartTime = this.formatTime ( value );
			}
		}

        public string AlternateShiftStartTime
        {
            get
            {
                return _alternateShiftStartTime;
            }
            set
            {
                _alternateShiftStartTime = value;
                if (_alternateShiftStartTime.Length > 0)
                {
                    _alternateShiftStartTime = this.formatTime(_alternateShiftStartTime);
                }
            }
        }

        public string FormattedAlternateShiftStartTime
        {
            get
            {
                if (_alternateShiftStartTime.Length > 0)
                {
                    return formatTimeAMPM(_alternateShiftStartTime);
                }
                else
                {
                    return _alternateShiftStartTime;
                }
            }
            set
            {
                _alternateShiftStartTime = value;
                if (_alternateShiftStartTime.Length > 0)
                {
                    _alternateShiftStartTime = this.formatTime(_alternateShiftStartTime);
                }
            }
        }

		public string ShiftEndTime
		{
			get
			{
				return _shiftEndTime;
			}
			set
			{
				_shiftEndTime = this.formatTime ( value );
			}
		}

		public string FormattedShiftEndTime
		{
			get
			{
				return formatTimeAMPM ( _shiftEndTime );
			}
			set
			{
				_shiftEndTime = this.formatTime ( value );
			}
		}

		public string ETicketNumber
		{
			get
			{
				return _eTicketNumber;
			}
			set
			{
				_eTicketNumber = value;
			}
		}

		public bool HasOvertimeEmployees
		{
			get
			{
				return _hasOvertimeEmployees;
			}
			set
			{
				_hasOvertimeEmployees = value;
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

		private string formatTime ( string valueToFormat )
		{
			return DateTime.Parse ( valueToFormat ).ToString ( "HH:mm" );
		}

		private string formatTimeAMPM ( string valueToFormat )
		{
			return DateTime.Parse ( valueToFormat ).ToString ( "h:mm tt" );
		}
	}
}
