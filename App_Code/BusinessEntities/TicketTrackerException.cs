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
	public class TicketTrackerException
	{
		private int _clientId;
        //private bool _rosterEmployeeFlag = false;
		ArrayList _employees = new ArrayList();
        private string _backgroundColor = "";
        private DateTime _periodStartDateTime = new DateTime(1,1,1);
        private DateTime _periodEndDateTime = new DateTime(1, 1, 1);
        private Boolean _ignoreDuplicates = false;
        
        public TicketTrackerException()
		{
			//
			// TODO: Add constructor logic here
			//
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


        [XmlArrayItem(typeof(EmployeeTracker))]
		public ArrayList Employees
		{
			get
			{
				return _employees;
			}
			set
			{
				_employees = value;
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

        public DateTime PeriodStartDateTime
        {
            get
            {
                return _periodStartDateTime;
            }
            set
            {
                _periodStartDateTime = value;
            }
        }

        public DateTime PeriodEndDateTime
        {
            get
            {
                return _periodEndDateTime;
            }
            set
            {
                _periodEndDateTime = value;
            }
        }
        public Boolean IgnoreDuplicates
        {
            get { return _ignoreDuplicates; }
            set { _ignoreDuplicates = value; }
        }
	}
}
