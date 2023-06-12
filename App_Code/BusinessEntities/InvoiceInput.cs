using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
    public class InvoiceInput
	{
        private DateTime _weekEndDate;
        private int _clientId = 0;
        private int _clientApprovalId = 0;

        public InvoiceInput()
		{
			//
			// TODO: Add constructor logic here
			//
		}

	
        public DateTime WeekEndDate
        {
            get
            {
                return _weekEndDate;
            }
            set
            {
                _weekEndDate = value;
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

        public int ClientApprovalId
        {
            get
            {
                return _clientApprovalId;
            }
            set
            {
                _clientApprovalId = value;
            }
        }
	}
}
