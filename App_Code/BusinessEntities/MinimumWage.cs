using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class MinimumWage
	{	
		private int _minimumWageId;
		private double _minimumWageAmount = 0;
        private DateTime _effectiveDate = new DateTime(1900, 1, 1);
        private DateTime _expirationDate = new DateTime(1900, 1, 1);

		public MinimumWage()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public MinimumWage(int minimumWageId, double minimumWageAmount, DateTime effectiveDate, DateTime expirationDate)
		{
			_minimumWageId = minimumWageId; 
			_minimumWageAmount = minimumWageAmount;
            _effectiveDate = effectiveDate;
            _expirationDate = expirationDate;
		}


        public int MinimumWageId
		{
			get
			{
                return _minimumWageId;
			}
			set
			{
                _minimumWageId = value;
			}
		}

        public double MinimumWageAmount
		{
			get
			{
                return _minimumWageAmount;
			}
			set
			{
                _minimumWageAmount = value;
			}
		}

        public DateTime EffectiveDate
        {
            get
            {
                return _effectiveDate;
            }
            set
            {
                _effectiveDate = value;
            }
        }

        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
            }
        }

	}
}
