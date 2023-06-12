using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Supervisor
	{
	
		private int _supervisorId;
		private string _supervisorFirstName = "";
		private string _supervisorLastName = "";

		public Supervisor()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Supervisor ( int supervisorId, string supervisorLastName, string supervisorFirstName )
		{
			_supervisorId = supervisorId;
			_supervisorLastName = supervisorLastName;
			_supervisorFirstName = supervisorFirstName;
		}
		
		public int SupervisorID
		{
			get
			{
				return _supervisorId;
			}
			set
			{
				_supervisorId = value;
			}
		}

		public string SupervisorLastName
		{
			get
			{
				return _supervisorLastName;
			}
			set
			{
				_supervisorLastName = value;
			}
		}

		public string SupervisorFirstName
		{
			get
			{
				return _supervisorFirstName;
			}
			set
			{
				_supervisorFirstName = value;
			}
		}
		public override string ToString()
		{
			if ( this._supervisorLastName.Trim().Length.Equals ( 0 ) )
				return this._supervisorFirstName;
			else
				return this._supervisorLastName + ", " + this._supervisorFirstName;
		}
	}
}
