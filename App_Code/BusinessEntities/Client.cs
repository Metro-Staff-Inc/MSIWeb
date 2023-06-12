using System;
using System.Collections;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
    /* some desc/value info for drop down lists */
    public class ClientDDLMobile
    {
        public List<int> ClientID { set; get; }
        public List<string> ClientName { set; get; }
        public int Preferred { get; set; }
    }

    /* some desc/value info for drop down lists */
    public class ClientDDL
    {
        public int ClientID { set; get; }
        public string ClientName { set; get; }
    }

    /* some desc/value info for drop down lists */
    public class LocationDDL
    {
        public int LocationID { set; get; }
        public string LocationName { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Zip { get; set; }
        public string PhonePrefix { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneLast4 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }


    public class SupervisorParams
    {
        public int DepartmentId { get; set; }
        public Guid UserId { get; set; }
        public int ClientId { get; set; }
        public int ShiftType { get; set; }

    }
    public class UserDepartment
    {
        public int ClientMembershipId { get; set; }
        public int DepartmentId { get; set; }
        public bool Hide { get; set; }
    }

    /// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Client : IComparable
	{
		private int _clientId;
		private string _clientName = "";
        private string _employeeIdPrefix = "";
        private bool _addOfficeCodeToEmpNum = true;
		private ArrayList _shifts = new ArrayList();
		private bool _hasTempNumbers = false;
        private ArrayList _shiftTypes = new ArrayList();
        private ArrayList _departments = new ArrayList();
        private bool _maintainsEmployeeSchedules = false;
        private bool _preferredClient = false;
        private bool _requiresLunchInOut = false;
        private bool _calculateSummaryHours = false;
        private decimal _multiplier = 0;
        private decimal _otMultiplier = 0;
        private List<int> _ignoreShiftList;
        public int daysWorked { get; set; }
        private Dictionary<int, String> _location { get; set; }

		public Client()
		{
			//
			// TODO: Add constructor logic here
			//
            //System.Diagnostics.Debug.Write("Here we are in the Client Constructor!");
		}
        public Client(int clientId, string clientName, string employeeIdPrefix, bool addOfficeCodeToEmpNum,
            bool hasTempNumbers, bool maintainsEmployeeSchedules, bool requiresLunchInOut,
                bool calculateSummaryHours)
        {
            _clientId = clientId;
            _clientName = clientName;
            _employeeIdPrefix = employeeIdPrefix;
            _addOfficeCodeToEmpNum = addOfficeCodeToEmpNum;
            _hasTempNumbers = hasTempNumbers;
            _maintainsEmployeeSchedules = maintainsEmployeeSchedules;
            _requiresLunchInOut = requiresLunchInOut;
            _calculateSummaryHours = calculateSummaryHours;
        }

		public int CompareTo ( object obj ) 
		{
			if ( obj is Client ) 
			{
				Client cl = (Client) obj;
				return this._clientId.CompareTo ( cl._clientId );
			}
       
			throw new ArgumentException("object is not an Client");
		}
        public Dictionary<int, String> Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
		public int CompareTo ( Client client2, ComparisonTypes comparisonMethod )
		{
			switch ( comparisonMethod )
			{
				case ComparisonTypes.ClientName :
					return this._clientName.CompareTo ( client2._clientName );
				default :
					return this._clientId.CompareTo ( client2._clientId );
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
		
		public string ClientName
		{
			get
			{
				return _clientName;
			}
			set
			{
				_clientName = value;
			}
		}

        public string EmployeeIdPrefix
        {
            get
            {
                return _employeeIdPrefix;
            }
            set
            {
                _employeeIdPrefix = value;
            }
        }

        public bool AddOfficeCodeToEmpNum
        {
            get
            {
                return _addOfficeCodeToEmpNum;
            }
            set
            {
                _addOfficeCodeToEmpNum = value;
            }
        }
		public bool HasTempNumbers
		{
			get
			{
				return _hasTempNumbers;
			}
			set
			{
				_hasTempNumbers = value;
			}
		}

        public bool MaintainsEmployeeSchedules
        {
            get
            {
                return _maintainsEmployeeSchedules;
            }
            set
            {
                _maintainsEmployeeSchedules = value;
            }
        }

        public bool PreferredClient
        {
            get
            {
                return _preferredClient;
            }
            set
            {
                _preferredClient = value;
            }
        }

		public ArrayList Shifts
		{
			get
			{
				return _shifts;
			}
			set
			{
				_shifts = value;
			}
		}

        public ArrayList ShiftTypes
        {
            get
            {
                return _shiftTypes;
            }
            set
            {
                _shiftTypes = value;
            }
        }

        public ArrayList Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                _departments = value;
            }
        }

        public bool RequiresLunchInOut
        {
            get
            {
                return _requiresLunchInOut;
            }
            set
            {
                _requiresLunchInOut = value;
            }
        }

        public bool CalculateSummaryHours
        {
            get
            {
                return _calculateSummaryHours;
            }
            set
            {
                _calculateSummaryHours = value;
            }
        }

        public decimal Multiplier
        {
            get
            {
                return _multiplier;
            }
            set
            {
                _multiplier = value;
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
        public List<int> IgnoreShiftList
        {
            get { return _ignoreShiftList; }
            set { _ignoreShiftList = value; }
        }

		public override string ToString()
		{
			return this._clientName;
		}
	}
}
