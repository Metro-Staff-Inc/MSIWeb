using System;
using System.Collections;
using System.Text;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ClientShiftLocation
	{
		private int _clientID;
		private int _locationId;
		private int _shiftId;
		private Shift _shiftType = new Shift();
		private string _shiftName = "";
		private string _shiftStartTime = "";
		private string _shiftEndTime = "";
		private string _ticketDueTime = "";
		private Department _department = new Department();
		private Supervisor _supervisor = new Supervisor();
		private BillType _billType = new BillType();
		private double _breakHours;
		private string _emailAddress = "";
		private ShiftClass _shiftClassInfo = new ShiftClass();

		public ClientShiftLocation()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ClientShiftLocation ( int clientId, int locationId, Shift shiftType, string shiftName, string shiftStartTime, string shiftEndTime, string ticketDueTime, double breakHours )
		{
			_clientID = clientId;
			_locationId = locationId;
			_shiftId = shiftType.ShiftID;
			_shiftType = shiftType;
			_shiftName = shiftName;
			_shiftStartTime = shiftStartTime;
			_shiftEndTime = shiftEndTime;
			_ticketDueTime = ticketDueTime;
			_breakHours = breakHours;
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

		public int LocationID
		{
			get
			{
				return _locationId;
			}
			set
			{
				_locationId = value;
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

		public string ShiftName
		{
			get
			{
				if ( this._shiftName.Trim().Length.Equals ( 0 ) )
					return this._shiftType.ShiftDesc;
				else
					return _shiftName;
			}
			set
			{
				_shiftName = value;
			}
		}

		public string ShiftStartTime
		{
			get
			{
				if ( this._shiftStartTime.Trim().Length.Equals ( 0 ) )
					return this._shiftType.DefaultStartTime;
				else
					return _shiftStartTime;
			}
			set
			{
				_shiftStartTime = value;
			}
		}

		public string ShiftEndTime
		{
			get
			{
				if ( this._shiftEndTime.Trim().Length.Equals ( 0 ) )
					return this._shiftType.DefaultEndTime;
				else
					return _shiftEndTime;
			}
			set
			{
				_shiftEndTime = value;
			}
		}

		public double ShiftBreakHours
		{
			get
			{
				return _breakHours;
			}
			set
			{
				_breakHours = value;
			}
		}

		public string EmailAddress
		{
			get
			{
				return _emailAddress;
			}
			set
			{
				_emailAddress = value;
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

		public Department DepartmentInfo
		{
			get
			{
				return _department;
			}
			set
			{
				_department = value;
			}
		}

		public Supervisor SupervisorInfo
		{
			get
			{
				return _supervisor;
			}
			set
			{
				_supervisor = value;
			}
		}

		public ShiftClass ShiftClassInfo
		{
			get
			{
				return _shiftClassInfo;
			}
			set
			{
				_shiftClassInfo = value;
			}
		}

		public BillType BillTypeInfo
		{
			get
			{
				return _billType;
			}
			set
			{
				_billType = value;
			}
		}

		public string ToString ( bool showDetailInfo )
		{
			if ( showDetailInfo )
			{
				return this.ToString();
			}
			else
			{
				return this.getDefaultToStringInfo();
			}
		}

		public override string ToString ( )
		{
			StringBuilder sb = new StringBuilder();
			sb.Append ( this.getDefaultToStringInfo () );
			sb.Append ( " From " );
			sb.Append ( DateTime.Parse ( this.ShiftStartTime ).ToShortTimeString() );
			sb.Append ( " to " );
			sb.Append ( DateTime.Parse ( this.ShiftEndTime ).ToShortTimeString() );

			if ( this.SupervisorInfo.SupervisorID > 0 )
			{
				sb.Append ( " Report to " );
				sb.Append ( this.SupervisorInfo.SupervisorFirstName );
			}
			else if ( this.DepartmentInfo.DepartmentID > 0 )
			{
				sb.Append ( " Report to " );
				sb.Append ( this.DepartmentInfo.DepartmentName );
			}
			//if ( this.BillTypeInfo.BillTypeID > 0 )
			//{
			//	sb.Append ( " " );
			//	sb.Append ( this.BillTypeInfo.BillTypeName );
			//}
			
			return sb.ToString();
		}

		private string getDefaultToStringInfo ( )
		{
			return this.ShiftName;
		}
	}
}
