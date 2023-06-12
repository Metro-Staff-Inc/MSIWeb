using System;
using System.Collections.Generic;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class Invoice
	{
        private int _invoiceHeaderId = 0;
        private int _clientId;
        private string _invoiceNumber = string.Empty;
        private DateTime _invoiceDateTime;
        private DateTime _weekEndDate;
        private decimal _totalRegularHours = 0M;
        private int _clientApprovalId = 0;
        private decimal _totalOtHours = 0M;
        private decimal _totalDollars = 0M;
        private int _statusId = 0;
        private DateTime _createdDateTime;
        private DateTime _lastUpdatedDt;
        private string _lastUpdatedBy = string.Empty;
        private string _createdBy = string.Empty;
        private List<InvoiceDetail> _detailInfo = new List<InvoiceDetail>();

        public Invoice()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public int InvoiceHeaderId
        {
            get
            {
                return _invoiceHeaderId;
            }
            set
            {
                _invoiceHeaderId = value;
            }
        }

        public string InvoiceNumber
        {
            get
            {
                return _invoiceNumber;
            }
            set
            {
                _invoiceNumber = value;
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

        public DateTime InvoiceDateTime
        {
            get
            {
                return _invoiceDateTime;
            }
            set
            {
                _invoiceDateTime = value;
            }
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

        public decimal TotalRegularHours
        {
            get
            {
                return _totalRegularHours;
            }
            set
            {
                _totalRegularHours = value;
            }
        }

        public decimal TotalOTHours
        {
            get
            {
                return _totalOtHours;
            }
            set
            {
                _totalOtHours = value;
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

        public decimal TotalDollars
        {
            get
            {
                return _totalDollars;
            }
            set
            {
                _totalDollars = value;
            }
        }

        public int StatusId
        {
            get
            {
                return _statusId;
            }
            set
            {
                _statusId = value;
            }
        }

        public DateTime LastUpdatedDateTime
        {
            get
            {
                return _lastUpdatedDt;
            }
            set
            {
                _lastUpdatedDt = value;
            }
        }

        public DateTime CreatedDateTime
        {
            get
            {
                return _createdDateTime;
            }
            set
            {
                _createdDateTime = value;
            }
        }

        public string LastUpdatedBy
        {
            get
            {
                return _lastUpdatedBy;
            }
            set
            {
                _lastUpdatedBy = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                _createdBy = value;
            }
        }

        public List<InvoiceDetail> DetailInfo
        {
            get
            {
                return _detailInfo;
            }
            set
            {
                _detailInfo = value;
            }
        }
	}
}
