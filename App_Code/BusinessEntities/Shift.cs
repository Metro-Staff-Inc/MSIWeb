using System;
using System.Collections;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Shift
	{
	
		private int _shiftId;
		private string _shiftDesc = "";
		private string _defaultStartTime = "";
		private string _defaultEndTime = "";
        private string _trackingStartTime = "";
        private string _trackingEndTime = "";
        private bool _trackingMultiDay = false;
		private int _shiftType;
        private int _tempWorksMappingId = 0;
        //private double _brk = 0.0;

		public Shift()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public Shift(int shiftId, string shiftDesc, string defaultStartTime, string defaultEndTime, string trackingStartTime, string trackingEndTime, bool trackingMultiDay)
        {
            _shiftId = shiftId;
            _shiftDesc = shiftDesc;
            _defaultStartTime = defaultStartTime;
            _defaultEndTime = defaultEndTime;
            _trackingStartTime = trackingStartTime;
            _trackingEndTime = trackingEndTime;
            _trackingMultiDay = trackingMultiDay;
        }

        public Shift(int shiftId, string shiftDesc, string defaultStartTime, string defaultEndTime, int shiftType)
        {
            _shiftId = shiftId;
            _shiftDesc = shiftDesc;
            _defaultStartTime = defaultStartTime;
            _defaultEndTime = defaultEndTime;
            _shiftType = shiftType;
        }
		
		public int ShiftID
		{
			get
			{
				return _shiftId;
			}
			set
			{
				_shiftId = value;
			}
		}

		public string ShiftDesc
		{
			get
			{
				return _shiftDesc;
			}
			set
			{
				_shiftDesc = value;
			}
		}

		public string DefaultStartTime
		{
			get
			{
				return _defaultStartTime;
			}
			set
			{
				_defaultStartTime = value;
			}
		}

		public string DefaultEndTime
		{
			get
			{
				return _defaultEndTime;
			}
			set
			{
				_defaultEndTime = value;
			}
		}

		public int ShiftType
		{
			get
			{
				return _shiftType;
			}
			set
			{
				_shiftType = value;
			}
		}

        public string TrackingStartTime
        {
            get
            {
                return _trackingStartTime;
            }
            set
            {
                _trackingStartTime = value;
            }
        }

        public string TrackingEndTime
        {
            get
            {
                return _trackingEndTime;
            }
            set
            {
                _trackingEndTime = value;
            }
        }

        public bool TrackingMultiDay
        {
            get
            {
                return _trackingMultiDay;
            }
            set
            {
                _trackingMultiDay = value;
            }
        }

        public int TempWorksMappingId
        {
            get
            {
                return _tempWorksMappingId;
            }
            set
            {
                _tempWorksMappingId = value;
            }
        }


		public override string ToString()
		{
			return this._shiftDesc;
		}
	}
}
