using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
    public class PayRateInput
	{
        private DateTime _weekEndDate;
        private int _clientId = 0;
        private int _departmentId = 0;
        private int _shiftTypeId = 0;

        public PayRateInput()
		{
			//
			// TODO: Add constructor logic here
			//
		}

	
        public DateTime WeekEndDate
        {
            get
            {
                return _weekEndDate;
            }
            set
            {
                _weekEndDate = value;
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

        public int ShiftTypeId
        {
            get
            {
                return _shiftTypeId;
            }
            set
            {
                _shiftTypeId = value;
            }
        }
	}
}
