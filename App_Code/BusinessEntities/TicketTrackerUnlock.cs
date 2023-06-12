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
	public class TicketTrackerUnlock
	{
        private string _unlockPunchList = string.Empty;

        public TicketTrackerUnlock()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public string UnlockPunchList
		{
			get
			{
                return _unlockPunchList;
			}
			set
			{
                _unlockPunchList = value;
			}
		}
	}
}
