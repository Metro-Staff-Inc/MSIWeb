using System;
using System.Collections;
using MSI.Web.MSINet.Common;
namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunchResult
	{
		private bool _punchSuccess = false;
        private int _punchException = 0;
        private EmployeePunchSummary _employeePunchSummary = new EmployeePunchSummary();
        private Enums.PunchTypes _punchType = Enums.PunchTypes.Invalid;
        private bool _duplicatePunch = false;
        private string _emailPunchTime = "";
        
        public EmployeePunchResult()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public string EmailPunchTime
        {
            get
            {
                return _emailPunchTime;
            }
            set
            {
                _emailPunchTime = value;
            }
        }

        public bool PunchSuccess
        {
            get
            {
                return _punchSuccess;
            }
            set
            {
                _punchSuccess = value;
            }
        }

        public bool DuplicatePunch
        {
            get
            {
                return _duplicatePunch;
            }
            set
            {
                _duplicatePunch = value;
            }
        }

        public int PunchException
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

		public EmployeePunchSummary EmployeePunchSummaryInfo
		{
			get
			{
                return _employeePunchSummary;
			}
			set
			{
                _employeePunchSummary = value;
			}
		}

        public Enums.PunchTypes PunchType
        {
            get
            {
                return _punchType;
            }
            set
            {
                _punchType = value;
            }
        }
	}
}
