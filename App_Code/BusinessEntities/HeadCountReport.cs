using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class HeadCountReport
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

        public HeadCountReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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
