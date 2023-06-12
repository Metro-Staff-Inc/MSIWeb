using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class TicketTracker
	{
		private int _clientId;
		private int _officeId;
        //private bool _rosterEmployeeFlag = false;
		ArrayList _employees = new ArrayList();
        private int _location;
        private Department _department = new Department();
        private ShiftType _shiftType = new ShiftType();
        private string _backgroundColor = "";
        private DateTime _periodStartDateTime = new DateTime(1,1,1);
        private DateTime _periodEndDateTime = new DateTime(1, 1, 1);
        private Enums.TrackingTypes _trackingType = Enums.TrackingTypes.Roster;
        private string _overrideRoleName = string.Empty;

        private int _clientRosterId;

		public TicketTracker()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public string OverrideRoleName
        {
            get
            {
                return _overrideRoleName;
            }
            set
            {
                _overrideRoleName = value;
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

        public Enums.TrackingTypes TrackingType
        {
            get
            {
                return _trackingType;
            }
            set
            {
                _trackingType = value;
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

        /*
        public bool RosterEmployeeFlag
        {
            get
            {
                return _rosterEmployeeFlag;
            }
            set
            {
                _rosterEmployeeFlag = value;
            }
        }
        */

        public int Location
        {
            get { return _location; }
            set { _location = value; }
        }

        [XmlArrayItem(typeof(EmployeeTracker))]
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

        public DateTime PeriodStartDateTime
        {
            get
            {
                return _periodStartDateTime;
            }
            set
            {
                _periodStartDateTime = value;
            }
        }

        public DateTime PeriodEndDateTime
        {
            get
            {
                return _periodEndDateTime;
            }
            set
            {
                _periodEndDateTime = value;
            }
        }
	}
}
