using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Office : IComparable
	{
		private int _officeId;
		private string _officeName = "";
		private string _addressLine1 = "";
		private string _addressLine2 = "";
		private string _city = "";
		private string _state = "";
		private string _zip = "";
		private string _zipPlus4 = "";
		private string _phoneArea = "";
		private string _phonePrefix = "";
		private string _phoneLast4 = "";
		private string _officeCode = "";

		public Office()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public Office ( int officeId, string officeName, string addressLine1, string addressLine2, string city, string state, string zip, string zipPlus4, string phoneArea, string phonePrefix, string phoneLast4, string officeCode )
		{
			_officeId = officeId;
			_officeName = officeName;
			_addressLine1 = addressLine1;
			_addressLine2 = addressLine2;
			_city = city;
			_state = state;
			_zip = zip;
			_zipPlus4 = zipPlus4;
			_phoneArea = phoneArea;
			_phonePrefix = phonePrefix;
			_phoneLast4 = phoneLast4;
			_officeCode = officeCode;
		}

		public int CompareTo ( object obj ) 
		{
			if ( obj is Office ) 
			{
				Office off = (Office) obj;
				return this._officeName.CompareTo ( off._officeName );
			}
        
			throw new ArgumentException("object is not an Office");
		}

		public int CompareTo ( Office off2, ComparisonTypes comparisonMethod )
		{
			switch ( comparisonMethod )
			{
				case ComparisonTypes.OfficeID :
					return this._officeId.CompareTo ( off2._officeId );
					//break;
				default :
					return this._officeName.CompareTo ( off2._officeName );
			}
		}
		
		public int OfficeID
		{
			get
			{
				return _officeId;
			}
			set
			{
				_officeId = value;
			}
		}

		public string OfficeName
		{
			get
			{
				return _officeName;
			}
			set
			{
				_officeName = value;
			}
		}

		public string AddressLine1
		{
			get
			{
				return _addressLine1;
			}
			set
			{
				_addressLine1 = value;
			}
		}

		public string AddressLine2
		{
			get
			{
				return _addressLine2;
			}
			set
			{
				_addressLine2 = value;
			}
		}

		public string City
		{
			get
			{
				return _city;
			}
			set
			{
				_city = value;
			}
		}

		public string State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		public string Zip
		{
			get
			{
				return _zip;
			}
			set
			{
				_zip = value;
			}
		}
		public string ZipPlus4
		{
			get
			{
				return _zipPlus4;
			}
			set
			{
				_zipPlus4 = value;
			}
		}
		public string PhoneArea
		{
			get
			{
				return _phoneArea;
			}
			set
			{
				_phoneArea = value;
			}
		}
		public string PhonePrefix
		{
			get
			{
				return _phonePrefix;
			}
			set
			{
				_phonePrefix = value;
			}
		}
		public string PhoneLast4
		{
			get
			{
				return _phoneLast4;
			}
			set
			{
				_phoneLast4 = value;
			}
		}
		public string OfficeCode
		{
			get
			{
				return _officeCode;
			}
			set
			{
				_officeCode = value;
			}
		}
		public override string ToString()
		{
			return this._officeName;
		}
	}
}
