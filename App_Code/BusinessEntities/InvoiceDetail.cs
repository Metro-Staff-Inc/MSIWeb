using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class InvoiceDetail
	{
        private int _invoiceDetailId = 0;
        private int _invoiceHeaderId = 0;
        private int _clientRosterId = 0;
        private string _badgeNumber = string.Empty;
        //private DateTime _invoiceDateTime;
        //private DateTime _weekEndDate;
        private decimal _totalRegularHours = 0M;
        private decimal _totalOtHours = 0M;
        private decimal _payRate = 0M;
        private decimal _regularMultiplier = 0M;
        private decimal _otMultiplier = 0M;
        private decimal _totalBilling = 0M;
        //private int _statusId = 0;
        private DateTime _createdDateTime;
        private DateTime _lastUpdatedDt;
        private string _lastUpdatedBy = string.Empty;
        private string _createdBy = string.Empty;
        private Department _departmentInfo = new Department();
        private ShiftType _shiftTypeInfo = new ShiftType();
        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _jobCode = string.Empty;
        private Shift _shiftInfo = new Shift();
        private decimal _bonus = 0M;
        private string _office = "X";
        private int _costCenter;

        public int ClientId { get; set; }

        public int CostCenter
        {
            get
            {
                return _costCenter;
            }
            set
            {
                _costCenter = value;
            }
        }

        public string Office
        {
            get
            {
                return _office;
            }
            set
            {
                _office = value;
            }
        }
        public decimal Bonus
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
        public InvoiceDetail()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public int InvoiceDetailId
        {
            get
            {
                return _invoiceDetailId;
            }
            set
            {
                _invoiceDetailId = value;
            }
        }

        public int InvoiceHeaderId
        {
            get
            {
                return _invoiceHeaderId;
            }
            set
            {
                _invoiceHeaderId = value;
            }
        }

        public string BadgeNumber
        {
            get
            {
                return _badgeNumber;
            }
            set
            {
                _badgeNumber = value;
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

        public decimal TotalRegularHours
        {
            get
            {
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
                return _totalOtHours;
            }
            set
            {
                _totalOtHours = value;
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

        public decimal RegularMultiplier
        {
            get
            {
                return _regularMultiplier;
            }
            set
            {
                _regularMultiplier = value;
            }
        }

        public decimal OTMultiplier
        {
            get
            {
                return _otMultiplier;
            }
            set
            {
                _otMultiplier = value;
            }
        }

        public decimal TotalBilling
        {
            get
            {
                return _totalBilling;
            }
            set
            {
                _totalBilling = value;
            }
        }

        public decimal CalculateTotalBilling()
        {
            decimal bill = Math.Round((_payRate * _regularMultiplier), 2, MidpointRounding.AwayFromZero);
            decimal otBill = Math.Round((_payRate * _otMultiplier * 1.5M), 2, MidpointRounding.AwayFromZero);
            _totalBilling = (bill * _totalRegularHours) + (otBill * _totalOtHours);
            _totalBilling = Math.Round(_totalBilling, 2, MidpointRounding.AwayFromZero);
            return _totalBilling;
        }

        public DateTime LastUpdatedDateTime
        {
            get
            {
                return _lastUpdatedDt;
            }
            set
            {
                _lastUpdatedDt = value;
            }
        }

        public DateTime CreatedDateTime
        {
            get
            {
                return _createdDateTime;
            }
            set
            {
                _createdDateTime = value;
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

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                _createdBy = value;
            }
        }

        public Department DepartmentInfo
        {
            get
            {
                return _departmentInfo;
            }
            set
            {
                _departmentInfo = value;
            }
        }

        public ShiftType ShiftTypeInfo
        {
            get
            {
                return _shiftTypeInfo;
            }
            set
            {
                _shiftTypeInfo = value;
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

	}
}
