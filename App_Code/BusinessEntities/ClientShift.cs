using System;
using System.Collections;
using System.Text;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ClientShift
	{
		private int _clientID;
		private int _shiftId;
		private Shift _shiftType = new Shift();
		private string _ticketDueTime = "";

		public ClientShift()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ClientShift ( int clientId, Shift shiftType, string ticketDueTime )
		{
			_clientID = clientId;
			_shiftId = shiftType.ShiftID;
			_shiftType = shiftType;
			_ticketDueTime = ticketDueTime;
		}
		
		public int ClientID
		{
			get
			{
				return _clientID;
			}
			set
			{
				_clientID = value;
			}
		}

		public int ShiftID
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

		public Shift ShiftType
		{
			get
			{
				return _shiftType;
			}
			set
			{
				_shiftType = value;
				if ( _shiftType.ShiftID > 0 )
					_shiftId = _shiftType.ShiftID;
			}
		}

		public string TicketDueTime
		{
			get
			{
				return _ticketDueTime;
			}
			set
			{
				_ticketDueTime = value;
			}
		}

		public override string ToString ( )
		{
			return this.ShiftType.ShiftDesc;
		}

        public string ToString(int shiftType)
        {
            string retVal = "";
            switch (shiftType)
            {
                case 1:
                    retVal = "1st Shift";
                    break;
                case 2:
                    retVal = "2nd Shift";
                    break;
                case 3:
                    retVal = "3rd Shift";
                    break;
            }
            return retVal;
        }
	}
}
