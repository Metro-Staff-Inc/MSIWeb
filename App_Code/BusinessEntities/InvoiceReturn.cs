using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class InvoiceReturn
	{
        private bool _isSuccess;
        private Invoice _invoiceInfo = new Invoice();
        private Exception _exceptionInfo = null;
        private string _systemErrorCode = String.Empty;

        public InvoiceReturn()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
            }
        }

        public Invoice InvoiceInfo
        {
            get
            {
                return _invoiceInfo;
            }
            set
            {
                _invoiceInfo = value;
            }
        }

        public string SystemErrorCode
		{
			get
			{
                return _systemErrorCode;
			}
			set
			{
                _systemErrorCode = value;
			}
		}

        public Exception ExceptionInfo
        {
            get
            {
                return _exceptionInfo;
            }
            set
            {
                _exceptionInfo = value;
            }
        }

    }
}
