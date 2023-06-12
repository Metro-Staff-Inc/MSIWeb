using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
    public class DailySummary
    {
        public static DateTime DATE_NOT_SET = new DateTime(1, 1, 1);
        private DayOfWeek _workDay;
        private decimal _totalHoursWorked = 0M;
        private int _numberOfPunches = 0;
        private DateTime _firstPunch = DATE_NOT_SET;
        public bool dstFirst { get; set; }
        private DateTime _lastPunch = DATE_NOT_SET;
        public bool dstLast { get; set; }
        private ArrayList _workSummaries = new ArrayList();
        private bool _useDefaultBreakTime = false;
        private DateTime _checkInDateTime = new DateTime(1900, 1, 1);
        private bool _breakApplied = false;

        public DailySummary()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DateTime FirstPunch
        {
            get
            {
                return _firstPunch;
            }
            set
            {
                _firstPunch = value;
            }
        }
        public DateTime LastPunch
        {
            get
            {
                return _lastPunch;
            }
            set
            {
                _lastPunch = value;
            }
        }

        public DailySummary(DayOfWeek workDay)
        {
            _workDay = workDay;
        }

        public bool BreakApplied
        {
            get
            {
                return _breakApplied;
            }
            set
            {
                _breakApplied = value;
            }
        }

        public DateTime CheckInDateTime
        {
            get
            {
                return _checkInDateTime;
            }
            set
            {
                _checkInDateTime = value;
            }
        }

        public DayOfWeek WorkDay
        {
            get
            {
                return _workDay;
            }
            set
            {
                _workDay = value;
            }
        }

        public decimal TotalHoursWorked
        {
            get
            {
                return _totalHoursWorked;
            }
            set
            {
                _totalHoursWorked = value;
            }
        }

        public int NumberOfPunches
        {
            get
            {
                return _numberOfPunches;
            }
            set
            {
                _numberOfPunches = value;
            }
        }

        public ArrayList WorkSummaries
        {
            get
            {
                return _workSummaries;
            }
            set
            {
                _workSummaries = value;
            }
        }

        public bool UseDefaultBreakTime
        {
            get
            {
                return _useDefaultBreakTime;
            }
            set
            {
                _useDefaultBreakTime = value;
            }
        }

    }
}