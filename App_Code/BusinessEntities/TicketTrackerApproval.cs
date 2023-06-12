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
	public class TicketTrackerApproval
	{
        private string _approvedPunchList = string.Empty;
        private string _approvedPunchListXML = string.Empty;
        private string _approvedNoShowList = string.Empty;
        private DateTime _shiftDate = new DateTime(1,1,1);
        private DateTime _approvedDateTime = new DateTime(1, 1, 1);

        public TicketTrackerApproval()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public string ApprovedPunchList
		{
			get
			{
                return _approvedPunchList;
			}
			set
			{
                _approvedPunchList = value;
			}
		}

        public string ApprovedPunchListXML
        {
            get
            {
                return _approvedPunchListXML;
            }
            set
            {
                _approvedPunchListXML = value;
            }
        }

        public string ApprovedNoShowList
        {
            get
            {
                return _approvedNoShowList;
            }
            set
            {
                _approvedNoShowList = value;
            }
        }

        public DateTime ShiftDate
        {
            get
            {
                return _shiftDate;
            }
            set
            {
                _shiftDate = value;
            }
        }

        public DateTime ApprovedDateTime
        {
            get
            {
                return _approvedDateTime;
            }
            set
            {
                _approvedDateTime = value;
            }
        }
	}
}
