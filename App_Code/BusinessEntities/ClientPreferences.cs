using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class ClientPreferences : IComparable
	{
		private int _clientId;
        private bool _displayPayRate = false;
        private bool _displayJobCode = false;
        private bool _notifyHoursReady = false;
        private bool _displayInvoice = false;
        private bool _approveHours = false;
        private bool _displaySchedule = false;
        private bool _enablePunchReporting = false;
        private bool _employeeHistoryExactPunchTimes = false;
        private bool _ticketTrackingExactLatePunches = false;
        private bool _displayWeeklyReportsSundayToSaturday = false;
        private bool _displayWeeklyReportsSaturdayToFriday = false;
        private bool _displayWeeklyReportsWednesdayToTuesday = false;        
        private bool _displayBonuses = false;
        private bool _displayPayRateMaintenance = false;
        private bool _displayTemps = true;
        private bool _displayStartDate = false;
        private bool _displayBreakTimes = false;
        private bool _useExactTimes = false;
        private bool _rosterBasedPayRates = false;
        private bool _showLocationsHoursReport = false;
        private bool _exactPunchInOut = false;
        private bool _displayWeeklyReportsFridayToThursday = false;


        public ClientPreferences()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public ClientPreferences(int clientId, bool displayPayRates, bool displayJobCodes, 
            bool notifyHoursReady, bool displayInvoice, bool approveHours, bool displaySchedule,
            bool enablePunchReporting, bool employeeHistoryExactPunchTimes, bool ticketTrackingExactLatePunches,
            bool displayWeeklyReportsSaturdayToSunday, bool displayBonuses, bool displayPayRateMaintenance, 
            bool displayWeeklyReportsWednesdayToTuesday, bool displayTemps, bool displayStartDate, bool displayBreakTimes,
            bool useExactTimes, bool rosterBasedPayRates, bool showLocationsHoursReport, bool displayWeeklyReportSaturdayToFriday, 
            bool exactPunchInOut, bool displayWeeklyReportFridayToThursday)
        {
            _clientId = clientId;
            _displayPayRate = displayPayRates;
            _displayJobCode = displayJobCodes;
            _notifyHoursReady = notifyHoursReady;
            _displayInvoice = displayInvoice;
            _approveHours = approveHours;
            _displaySchedule = displaySchedule;
            _enablePunchReporting = enablePunchReporting;
            _employeeHistoryExactPunchTimes = employeeHistoryExactPunchTimes;
            _ticketTrackingExactLatePunches = ticketTrackingExactLatePunches;
            _displayWeeklyReportsSundayToSaturday = displayWeeklyReportsSaturdayToSunday;
            _displayWeeklyReportsWednesdayToTuesday = displayWeeklyReportsWednesdayToTuesday;
            _displayBonuses = displayBonuses;
            _displayPayRateMaintenance = displayPayRateMaintenance;
            _displayTemps = displayTemps;
            _displayStartDate = displayStartDate;
            _displayBreakTimes = displayBreakTimes;
            _useExactTimes = useExactTimes;
            _rosterBasedPayRates = rosterBasedPayRates;
            _showLocationsHoursReport = showLocationsHoursReport;
            _displayWeeklyReportsSaturdayToFriday = displayWeeklyReportSaturdayToFriday;
            _exactPunchInOut = exactPunchInOut;
            _displayWeeklyReportsFridayToThursday = displayWeeklyReportFridayToThursday;
        }

		public int CompareTo ( object obj ) 
		{
			if ( obj is ClientPreferences ) 
			{
				return this._clientId.CompareTo (((ClientPreferences)obj).ClientID );
			}
			throw new ArgumentException("object is not of type ClientPreferences");
		}

        public bool TicketTrackingExactLatePunches
        {
            get
            {
                return _ticketTrackingExactLatePunches; 
            }
            set
            {
                _ticketTrackingExactLatePunches = value;
            }
        }
        public bool EmployeeHistoryExactPunchTimes
        {
            get
            {
                return _employeeHistoryExactPunchTimes;
            }
            set
            {
                _employeeHistoryExactPunchTimes = value;
            }
        }
        public bool EnablePunchReporting
        {
            get
            {
                return _enablePunchReporting;
            }
            set
            {
                _enablePunchReporting = value;
            }
        }
		public int ClientID
		{
			get
			{
				return _clientId;
			}
			set
			{
				_clientId = value;
			}
		}		
        public bool DisplayJobCode
        {
            get
            {
                return _displayJobCode;
            }
            set
            {
                _displayJobCode = value;
            }
        }
        public bool DisplayBonuses
        {
            get
            {
                return _displayBonuses;
            }
            set
            {
                _displayBonuses = value;
            }
        }
        public bool DisplayBreakTimes
        {
            get
            {
                return _displayBreakTimes;
            }
            set
            {
                _displayBreakTimes = value;
            }
        }
        public bool RosterBasedPayRates
        {
            get
            {
                return _rosterBasedPayRates;
            }
            set
            {
                _rosterBasedPayRates = value;
            }
        }
        public bool DisplayPayRate
        {
            get
            {
                return _displayPayRate;
            }
            set
            {
                _displayPayRate = value;
            }
        }
        public bool DisplaySchedule
        {
            get
            {
                return _displaySchedule;
            }
            set
            {
                _displaySchedule = value;
            }
        }
        public bool NotifyHoursReady
        {
            get
            {
                return _notifyHoursReady;
            }
            set
            {
                _notifyHoursReady = value;
            }
        }
        public bool DisplayInvoice
        {
            get
            {
                return _displayInvoice;
            }
            set
            {
                _displayInvoice = value;
            }
        }
        public bool ApproveHours
        {
            get
            {
                return _approveHours;
            }
            set
            {
                _approveHours = value;
            }
        }
        public bool DisplayWeeklyReportsSundayToSaturday
        {
            get
            {
                return _displayWeeklyReportsSundayToSaturday;
            }
            set
            {
                _displayWeeklyReportsSundayToSaturday = value;
            }
        }
        public bool DisplayWeeklyReportsSaturdayToFriday
        {
            get
            {
                return _displayWeeklyReportsSaturdayToFriday;
            }
            set
            {
                _displayWeeklyReportsSaturdayToFriday = value;
            }
        }
        public bool DisplayWeeklyReportsFridayToThursday
        {
            get
            {
                return _displayWeeklyReportsFridayToThursday;
            }
            set
            {
                _displayWeeklyReportsFridayToThursday = value;
            }
        }
        public bool DisplayWeeklyReportsWednesdayToTuesday
        {
            get
            {
                return _displayWeeklyReportsWednesdayToTuesday;
            }
            set
            {
                _displayWeeklyReportsWednesdayToTuesday = value;
            }
        }
        public bool DisplayPayRateMaintenance
        {
            get
            {
                return _displayPayRateMaintenance;
            }
            set
            {
                _displayPayRateMaintenance = value;
            }
        }
        public bool DisplayTemps
        {
            get
            {
                return _displayTemps;
            }
            set
            {
                _displayTemps = value;
            }
        }
        public bool DisplayStartDate
        {
            get
            {
                return _displayStartDate;
            }
            set
            {
                _displayStartDate = value;
            }
        }
        public bool ShowLocationsHoursReport
        {
            get { return _showLocationsHoursReport; }
            set { _showLocationsHoursReport = value; }
        }
        public bool UseExactTimes
        {
            get { return _useExactTimes; }
            set { _useExactTimes = value; }
        }
        public bool ExactPunchInOut
        {
            get { return _exactPunchInOut; }
            set { _exactPunchInOut = value; }
        }
    }
}
