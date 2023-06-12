using System;
using System.Collections;
namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunch
	{
		private int _clientId;
        private int _employeeId;
        private string _employeeFirstName = "";
        private string _employeeLastName = "";
        private string _aidentNumber = "";
        private int _clientRosterId;
		private string _tempNumber = "";
        private DateTime _punchDateTime = new DateTime(1, 1, 1);
        private DateTime _roundedPunchDateTime = new DateTime(1, 1, 1);
        private int _employeePunchId;
        private bool _manualOverride = false;
        private string _lastUpdatedBy = string.Empty;
        private DateTime _lastUpdatedDateTime = new DateTime(1, 1, 1);

		public EmployeePunch()
		{
			//
			// TODO: Add constructor logic here
			//
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
	}
    public class FirstPunchDnr
    {
        public Boolean FirstPunch { get; set; }
        public String Aident { get; set; }
        public Boolean Dnr { get; set; }
    }
}
