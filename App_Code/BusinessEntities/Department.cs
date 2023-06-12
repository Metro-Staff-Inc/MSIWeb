using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Department
	{
	
		private int _departmentId;
		private string _departmentName = "";
        private string _payCode = "";
        private string _emailAddress = string.Empty;
        private int _location;

		public Department()
		{
		}

		public Department ( int departmentId, string departmentName )
		{
			_departmentId = departmentId;
			_departmentName = departmentName;
		}

        public Department(int departmentId, string departmentName, string emailAddress)
        {
            _departmentId = departmentId;
            _departmentName = departmentName;
            _emailAddress = emailAddress;
        }

        public Department(int departmentId, string departmentName, string payCode, string emailAddress)
        {
            _departmentId = departmentId;
            _departmentName = departmentName;
            _payCode = payCode;
            _emailAddress = emailAddress;
        }

        public int Location
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
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value;
            }
        }

		public int DepartmentID
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

		public string DepartmentName
		{
			get
			{
				return _departmentName;
			}
			set
			{
				_departmentName = value;
			}
		}

        public string PayCode
        {
            get
            {
                return _payCode;
            }
            set
            {
                _payCode = value;
            }
        }

		public override string ToString()
		{
			return this._departmentName;
		}
	}
}
