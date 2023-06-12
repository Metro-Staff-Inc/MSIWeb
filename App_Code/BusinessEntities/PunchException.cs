using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class PunchException
	{	
		private int _punchExceptionId;
		private string _punchExceptionMessage = "";

		public PunchException()
		{
			//
			// TODO: Add constructor logic here
			//
		}
 
		public PunchException ( int punchExceptionId, string punchExceptionMessage )
		{
			_punchExceptionId = punchExceptionId; 
			_punchExceptionMessage = punchExceptionMessage;
		}


        public int PunchExceptionID
		{
			get
			{
                return _punchExceptionId;
			}
			set
			{
                _punchExceptionId = value;
			}
		}
        /*
         * epe.badge_number, e.last_name, e.first_name, epe.created_by AS 'clock name', 
                epe.punch_dt, ISNULL(epe.client_roster_id,0) AS 'Roster Id', epe.punch_exception_id
         */
        public string BadgeNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime PunchDate { get; set; }
        public int ClientRosterId { get; set; } 
        
        public string PunchExceptionMessage
		{
			get
			{
                return _punchExceptionMessage;
			}
			set
			{
                _punchExceptionMessage = value;
			}
		}

		public override string ToString()
		{
            return this._punchExceptionMessage;
		}
	}
}
