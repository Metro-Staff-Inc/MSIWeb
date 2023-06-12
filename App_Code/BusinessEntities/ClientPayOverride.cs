using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ClientPayOverride
	{
	
		private int _clientPayOverrideId;
        private int _clientId = 0;
        private int _locationId = 0;
        private string _firstName = "";
        private string _LastName = "";
        private int _employeeId = 0;
        private int _shiftId = 0;
        private int _departmentId = 0;
        private int _shiftType = 0;
        private string _aidentNumber = "";
        private decimal _payRate = 0M;
        private DateTime _effectiveDate;
        private DateTime _expirationDate;
        public DateTime firstPunch { get; set; }

		public ClientPayOverride()
		{
			//
			// TODO: Add constructor logic here
			//
            firstPunch = new DateTime(1900, 1, 1);
		}

        public int ClientPayOverrideId
        {
            get
            {
                return _clientPayOverrideId;
            }
            set
            {
                _clientPayOverrideId = value;
            }
        }

        public int ClientId
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

        public int EmployeeId
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

        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
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

        public int ShiftId
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


        public DateTime EffectiveDate
        {
            get
            {
                return _effectiveDate;
            }
            set
            {
                _effectiveDate = value;
            }
        }

        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
            }
        }
        // = 0;
        //private int 
        public int DepartmentId
        {
            get
            {
                return _departmentId;
            }
            set
            {
                _departmentId = value;
            }
        }

        public int ShiftType
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
	}
}
