using System;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Runtime.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
    [DataContract]
	public class EmployeeTracker
	{
		private int _itemIdx;
        [DataMember]
		private int _empId;
        [DataMember]
        private string _ssn = "";
        [DataMember]
        private string _aident = "";
        [DataMember]
        private string _firstName = "";
        [DataMember]
        private string _lastName = "";
        [DataMember]
        private string _middleName = "";
        [DataMember]
        private string _addressLine1 = "";
        [DataMember]
        private string _addressLine2 = "";
        [DataMember]
        private string _city = "";
        [DataMember]
        private string _state = "";
        [DataMember]
        private string _zip = "";
        [DataMember]
        private string _zipPlus4 = "";
        [DataMember]
        private string _phoneArea = "";
        [DataMember]
        private string _phonePrefix = "";
        [DataMember]
        private string _phoneLast4 = "";
		private bool _isDirty;
        [DataMember]
        private double _weeklyHours;
		private bool _employeeIsActive;

        private string _backgroundColor = "";
        private ArrayList _punches = new ArrayList();
        [DataMember]
        private string _tempNumber = "";
        private Enums.EmployeePunchStatus _punchStatus = Enums.EmployeePunchStatus.NotCheckedIn;
        private int _employeeTicketId;
        [DataMember]
        private int _clientRosterId;
        private string _shiftStartTime = "";
        private string _shiftEndTime = "";
        [DataMember]
        private ArrayList _checkInPunches = new ArrayList();
        [DataMember]
        private ArrayList _checkOutPunches = new ArrayList();
        [DataMember]
        private ArrayList _workSummaries = new ArrayList();
        [DataMember]
        private decimal _minWage = 0M;
        [DataMember]
        private decimal _defaultPayRate = 0M;
        [DataMember]
        private decimal _payRate = 0M;
        [DataMember]
        private string _defaultJobCode = string.Empty;
        [DataMember]
        private string _jobCode = string.Empty;
        private Boolean _temp;

		public EmployeeTracker()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		public int ItemIdx
		{
			get
			{
				return _itemIdx;
			}
			set
			{
				_itemIdx = value;
			}
		}
        [DataMember]
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
		
		public string SSN
		{
			get
			{
				return _ssn;
			}
			set
			{
				_ssn = value;
				this._isDirty = true;
			}
		}
        //[DataMember]
		public string AIdentNumber
		{
			get
			{
				return _aident;
			}
			set
			{
				_aident = value;
				this._isDirty = true;
			}
		}
        //[DataMember]
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				this._isDirty = true;
			}
		}

		public string MiddleName
		{
			get
			{
				return _middleName;
			}
			set
			{
				_middleName = value;
				this._isDirty = true;
			}
		}

		public string FullName
		{
			get
			{
				string fullName = this._lastName.Trim();
				
				if ( this._firstName.Trim().Length > 0 )
					fullName += " " + this._firstName;

				if ( this._middleName.Trim().Length > 0 )
					fullName += " " + this._middleName;

				return fullName;
			}
		}

        //[DataMember]
        public string AddressLine1
		{
			get
			{
				return _addressLine1;
			}
			set
			{
				_addressLine1 = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string AddressLine2
		{
			get
			{
				return _addressLine2;
			}
			set
			{
				_addressLine2 = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string City
		{
			get
			{
				return _city;
			}
			set
			{
				_city = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string Zip
		{
			get
			{
				return _zip;
			}
			set
			{
				_zip = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string Zip4
		{
			get
			{
				return _zipPlus4;
			}
			set
			{
				_zipPlus4 = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string CityStateZip
		{
			get
			{
				return this._city + " " + this._state + " " + this._zip;
			}
		}

        //[DataMember]
        public string PhoneArea
		{
			get
			{
				return _phoneArea;
			}
			set
			{
				_phoneArea = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string PhonePrefix
		{
			get
			{
				return _phonePrefix;
			}
			set
			{
				_phonePrefix = value;
				this._isDirty = true;
			}
		}

        //[DataMember]
        public string PhoneLast4
		{
			get
			{
				return _phoneLast4;
			}
			set
			{
				_phoneLast4 = value;
				this._isDirty = true;
			}
		}
        //[DataMember]
        public string PhoneNumber
		{
			get
			{
				return "(" + this.PhoneArea + ") " + this.PhonePrefix + "-" + this.PhoneLast4;
			}
		}

		public double WeeklyHours
		{
			get
			{
				return _weeklyHours;
			}
			set
			{
				_weeklyHours = value;
			}
		}

        //[DataMember]
        public bool EmployeeIsActive
		{
			get
			{
				return _employeeIsActive;
			}
			set
			{
				_employeeIsActive = value;
			}
		}


		public bool HasChanges
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
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

        //[DataMember]
        public ArrayList Punches
        {
            get
            {
                return _punches;
            }
            set
            {
                _punches = value;
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

        public Enums.EmployeePunchStatus PunchStatus
        {
            get
            {
                return _punchStatus;
            }
            set
            {
                _punchStatus = value;
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

        public string ShiftStartTime
        {
            get
            {
                return _shiftStartTime;
            }
            set
            {
                _shiftStartTime = value;
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
                _shiftEndTime = value;
            }
        }

        public bool HasCheckIn
        {
            get
            {
                if (this._checkInPunches.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasCheckOut
        {
            get
            {
                if (this._checkOutPunches.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ArrayList CheckInPunches
        {
            get
            {
                return _checkInPunches;
            }
            set
            {
                _checkInPunches = value;
            }
        }

        public ArrayList CheckOutPunches
        {
            get
            {
                return _checkOutPunches;
            }
            set
            {
                _checkOutPunches = value;
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
        public Boolean Temp
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value;
            }
        }

        private decimal GetEmployeePayRate()
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

        private string GetEmployeeJobCode()
        {
            string jobCode = _jobCode;

            if (jobCode == string.Empty)
            {
                jobCode = _defaultJobCode;
            }

            return jobCode;
        }


		public override string ToString()
		{
			return this.FullName;
		}
	}
}
