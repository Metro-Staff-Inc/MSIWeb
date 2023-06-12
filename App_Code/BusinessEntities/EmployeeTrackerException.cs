using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeeTrackerException
	{
		private int _itemIdx;
		private int _empId;
		private string _firstName = "";
		private string _lastName = "";

        private string _backgroundColor = "";
        private string _tempNumber = "";
        private int _clientRosterId;
        private DateTime _punchDateTime = new DateTime(1, 1, 1);
        private DateTime _roundedPunchDateTime = new DateTime(1, 1, 1);
        PunchException _punchException = new PunchException();

        public EmployeeTrackerException()
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

		public string FullName
		{
			get
			{
				string fullName = this._lastName.Trim();
				
				if ( this._firstName.Trim().Length > 0 )
					fullName += ", " + this._firstName;

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

		public override string ToString()
		{
			return this.FullName;
		}
	}
}
