using System;
using System.Collections;
using MSI.Web.MSINet.Common;
namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunchMaintenanceResult
	{
        private bool _resultValue = false;
        private EmployeePunchMaintenance _employeePunchMaintenance = new EmployeePunchMaintenance();
        private string _errorInfo = string.Empty;

        public EmployeePunchMaintenanceResult()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public bool ResultValue
        {
            get
            {
                return _resultValue;
            }
            set
            {
                _resultValue = value;
            }
        }

		public EmployeePunchMaintenance EmployeePunchMaintenanceInfo
		{
			get
			{
                return _employeePunchMaintenance;
			}
			set
			{
                _employeePunchMaintenance = value;
			}
		}

        public string ErrorInfo
        {
            get
            {
                return _errorInfo;
            }
            set
            {
                _errorInfo = value;
            }
        }
	}
}
