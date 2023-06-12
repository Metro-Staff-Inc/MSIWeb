using System;
using System.Collections;
using MSI.Web.MSINet.Common;
using System.Collections.Generic;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
    /// 
    public class Dept
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class ShiftDepartment
    {
        public String Name { get; set; }
        public int Type { get; set; }
        public List<Dept> Department { get; set; }
    }
    public class EmpInfo
    {
        public String Id;
        public String LName;
        public String FName;
        public String Dept;
        public Int32 Shift;    
        public Decimal Reg;
        public Decimal OT;
        public Decimal Pay;
    }
    public class HoursReportMin
    {
        public List<EmpInfo> employees;
    }
	public class HoursReport
	{
        private string _backgroundColor = "";
        private ArrayList _employeeHistoryCollection = new ArrayList();
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private int _clientId;
        private bool _rosterEmployeeFlag = false;
        private bool _isApproved = false;
        private string _approvalXML = string.Empty;
        private int _clientApprovalId = 0;
        private DateTime _approvalDateTime;
        private string _approvalUserName = string.Empty;
        private bool _useExactTimes = false;
        public bool MultiDepts { get; set; }
        public int DaysWorked { get; set; }
        public ClientPreferences clientPrefs { get; set; }

        public HoursReport()
		{
			//
			// TODO: Add constructor logic here
			//
            MultiDepts = false;
		}
        public bool ShowAllEmployees { get; set; }

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

        public bool UseExactTimes
        {
            get
            {
                return _useExactTimes;
            }
            set
            {
                _useExactTimes = value;
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

        public ArrayList EmployeeHistoryCollection
        {
            get
            {
                return _employeeHistoryCollection;
            }
            set
            {
                _employeeHistoryCollection = value;
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

        public string ApprovalXML
        {
            get
            {
                return _approvalXML;
            }
            set
            {
                _approvalXML = value;
            }
        }

        public bool IsApproved
        {
            get
            {
                return _isApproved;
            }
            set
            {
                _isApproved = value;
            }
        }

        public int ClientApprovalId
        {
            get
            {
                return _clientApprovalId;
            }
            set
            {
                _clientApprovalId = value;
            }
        }
        public DateTime ApprovalDateTime
        {
            get
            {
                return _approvalDateTime;
            }
            set
            {
                _approvalDateTime = value;
            }
        }
        public string ApprovalUserName
        {
            get
            {
                return _approvalUserName;
            }
            set
            {
                _approvalUserName = value;
            }
        }
	}
}
