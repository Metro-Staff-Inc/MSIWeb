using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class BillType
	{
	
		private int _billTypeId;
		private string _billTypeName;
		private int _numberOfWorkDays;

		public BillType()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public BillType ( int billTypeId, string billTypeName )
		{
			_billTypeId = billTypeId;
			_billTypeName = billTypeName;
		}

		public BillType ( int billTypeId, string billTypeName, int numberOfWorkDays )
		{
			_billTypeId = billTypeId;
			_billTypeName = billTypeName;
			_numberOfWorkDays = numberOfWorkDays;
		}
		
		public int BillTypeID
		{
			get
			{
				return _billTypeId;
			}
			set
			{
				_billTypeId = value;
			}
		}

		public string BillTypeName
		{
			get
			{
				return _billTypeName;
			}
			set
			{
				_billTypeName = value;
			}
		}

		public int NumberOfWorkDays
		{
			get
			{
				return _numberOfWorkDays;
			}
			set
			{
				_numberOfWorkDays = value;
			}
		}

		public override string ToString()
		{
			return this._billTypeName;
		}
	}
}
