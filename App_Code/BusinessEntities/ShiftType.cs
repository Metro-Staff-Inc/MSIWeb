using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ShiftType
	{	
		private int _shiftTypeId;
		private string _shiftTypeDesc = "";

		public ShiftType()
		{
			//
			// TODO: Add constructor logic here
			//
		}
 
		public ShiftType ( int shiftTypeId, string shiftTypeDesc )
		{
			_shiftTypeId = shiftTypeId; 
			_shiftTypeDesc = shiftTypeDesc;
		}

        public ShiftType(int shiftTypeId)
        {
            _shiftTypeId = shiftTypeId;
        }
		
		public int ShiftTypeId
		{
			get
			{
				return _shiftTypeId;
			}
			set
			{
				_shiftTypeId = value;
			}
		}

		public string ShiftTypeDesc
		{
			get
			{
				return this.ToString(this._shiftTypeId);
			}
			set
			{
				_shiftTypeDesc = value;
			}
		}

		public override string ToString()
		{
            return this.ToString(this._shiftTypeId);
		}

        public string ToString(int shiftType)
        {
            string retVal = "Shift Type Not Found";
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
                    /*    
                     *    for now, Tangent uses different shifts than 1, 2, or 3.
                     *    until this whole proc is a database table, this is how
                     *    it's handled.
                     */
                case 4:
                    retVal = "Shift A";
                    break;
                case 5:
                    retVal = "Shift B";
                    break;
                case 6:
                    retVal = "Shift C";
                    break;
                case 7:
                    retVal = "Shift D";
                    break;
                case 8:
                    retVal = "Shift R1";
                    break;
                case 9:
                    retVal = "Shift R2";
                    break;
                case 10:
                    retVal = "Shift R3";
                    break;
                case 11:
                    retVal = "Shift MF1";
                    break;
                case 12:
                    retVal = "Shift MF2";
                    break;
                case 13:
                    retVal = "Shift MF3";
                    break;
            }
            return retVal;
        }
	}
}
