using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class RecordSwipeReturnSummary
	{
        private RecordSwipeReturn _recordSwipeReturn = new RecordSwipeReturn();
        private decimal _currentWeeklyHours = 0M;
        private bool _calculateWeeklyHours = false;

        public RecordSwipeReturnSummary()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public RecordSwipeReturn RecordSwipeReturnInfo
        {
            get
            {
                return _recordSwipeReturn;
            }
            set
            {
                _recordSwipeReturn = value;
            }
        }

        public decimal CurrentWeeklyHours
        {
            get
            {
                return _currentWeeklyHours;
            }
            set
            {
                _currentWeeklyHours = value;
            }
        }

        public bool CalculateWeeklyHours
        {
            get
            {
                return _calculateWeeklyHours;
            }
            set
            {
                _calculateWeeklyHours = value;
            }
        }

	}
}
