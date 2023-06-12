using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunchMaintenance
	{
		private int _empId;
		private string _aident = "";
		private string _firstName = "";
		private string _lastName = "";
		private string _middleName = "";
        private int _clientRosterId;
        private ArrayList _employeePunches = new ArrayList();
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private int _clientId;
        private string _tempNumber = "";
        private PunchMaintenanceReason _punchMaintenanceReason = new PunchMaintenanceReason();

        public EmployeePunchMaintenance()
		{
			//
			// TODO: Add constructor logic here
			//
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
				return _lastName;
			}
			set
			{
				_lastName = value;
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

        public ArrayList EmployeePunches
        {
            get
            {
                return _employeePunches;
            }
            set
            {
                _employeePunches = value;
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

        public PunchMaintenanceReason PunchMaintenanceReasonInfo
        {
            get
            {
                return _punchMaintenanceReason;
            }
            set
            {
                _punchMaintenanceReason = value;
            }
        }

		public override string ToString()
		{
			return this.FullName;
		}
	}
}
