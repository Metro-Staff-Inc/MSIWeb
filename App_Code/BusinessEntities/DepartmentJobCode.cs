using System;
using System.Collections;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
    /// <summary>
    /// Summary description for Content.
    /// </summary>
    public class DepartmentJobCode
    {

        private Department _departmentInfo;
        private int _clientId;
        private int _locationId = 0;
        private int _shiftId = 0;
        private int _shiftType = 0;
        private DateTime _effectiveDate;
        private DateTime _expirationDate;
        private int _clientJobCodeId = 0;
        private string _jobCode = "";
        private List<ClientJobCodeOverride> _jobCodeOverrides = new List<ClientJobCodeOverride>();

        public DepartmentJobCode()
        {
            //
            // TODO: Add constructor logic here
            //
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

        public int ClientJobCodeId
        {
            get
            {
                return _clientJobCodeId;
            }
            set
            {
                _clientJobCodeId = value;
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

        public List<ClientJobCodeOverride> JobCodeOverrides
        {
            get
            {
                return _jobCodeOverrides;
            }
            set
            {
                _jobCodeOverrides = value;
            }
        }
    }
}
