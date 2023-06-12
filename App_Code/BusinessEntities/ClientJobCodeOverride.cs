using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// Summary description for Content.
    /// </summary>
    public class ClientJobCodeOverride
    {

        private int _clientJobCodeOverrideId;
        private int _clientId = 0;
        private int _locationId = 0;
        private string _firstName = "";
        private string _LastName = "";
        private int _employeeId = 0;
        private int _shiftId = 0;
        private int _departmentId = 0;
        private int _shiftType = 0;
        private string _aidentNumber = "";
        private string _jobCode = "";
        private DateTime _effectiveDate;
        private DateTime _expirationDate;

        public ClientJobCodeOverride()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int ClientJobCodeOverrideId
        {
            get
            {
                return _clientJobCodeOverrideId;
            }
            set
            {
                _clientJobCodeOverrideId = value;
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
