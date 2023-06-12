using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Status
	{
	
		private int _statusId;
		private string _statusDesc = "";
		private int _deliveryStatusId;

		public Status()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Status ( int statusId, string statusDesc )
		{
			_statusId = statusId;
			_statusDesc = statusDesc;
			_deliveryStatusId = 0;
		}

		public Status ( int statusId, string statusDesc, int deliveryStatusId )
		{
			_statusId = statusId;
			_statusDesc = statusDesc;
			_deliveryStatusId = deliveryStatusId;
		}
		
		public int StatusID
		{
			get
			{
				return _statusId;
			}
			set
			{
				_statusId = value;
			}
		}

		public string StatusDesc
		{
			get
			{
				return _statusDesc;
			}
			set
			{
				_statusDesc = value;
			}
		}

		public int DeliveryStatusID
		{
			get
			{
				return _deliveryStatusId;
			}
			set
			{
				_deliveryStatusId = value;
			}
		}

		public override string ToString()
		{
			return this._statusDesc;
		}
	}
}
