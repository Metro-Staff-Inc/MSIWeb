using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class PunchMaintenanceReason
	{	
		private int _punchMaintenanceReasonId;
        private string _punchMaintenanceReasonDesc = "";

		public PunchMaintenanceReason()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public PunchMaintenanceReason(int punchMaintenanceReasonId, string punchMaintenanceReasonDesc)
		{
            _punchMaintenanceReasonId = punchMaintenanceReasonId;
            _punchMaintenanceReasonDesc = punchMaintenanceReasonDesc;
		}


        public int PunchMaintenanceReasonId
		{
			get
			{
                return _punchMaintenanceReasonId;
			}
			set
			{
                _punchMaintenanceReasonId = value;
			}
		}

        public string PunchMaintenanceReasonDesc
		{
			get
			{
                return _punchMaintenanceReasonDesc;
			}
			set
			{
                _punchMaintenanceReasonDesc = value;
			}
		}

		public override string ToString()
		{
            return this._punchMaintenanceReasonDesc;
		}
	}
}
