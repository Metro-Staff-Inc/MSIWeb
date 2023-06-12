using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ShiftClass
	{
	
		private int _shiftClassId;
		private string _shiftClassDesc = "";

		public ShiftClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public ShiftClass ( int shiftClassId, string shiftClassDesc )
		{
			_shiftClassId = shiftClassId;
			_shiftClassDesc = shiftClassDesc;
		}
		
		public int ShiftClassId
		{
			get
			{
				return _shiftClassId;
			}
			set
			{
				_shiftClassId = value;
			}
		}

		public string ShiftClassDesc
		{
			get
			{
				return _shiftClassDesc;
			}
			set
			{
				_shiftClassDesc = value;
			}
		}


		public override string ToString()
		{
			return this._shiftClassDesc;
		}
	}
}
