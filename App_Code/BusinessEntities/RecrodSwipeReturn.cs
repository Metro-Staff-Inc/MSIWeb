using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class RecordSwipeReturn
	{
        private bool _punchSuccess;
        private int _punchException = 0;
        private string _punchType = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _systemErrorCode = String.Empty;

        public RecordSwipeReturn()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public bool PunchSuccess
        {
            get
            {
                return _punchSuccess;
            }
            set
            {
                _punchSuccess = value;
            }
        }

        public int PunchException
        {
            get
            {
                return _punchException;
            }
            set
            {
                _punchException = value;
            }
        }

        public string PunchType
        {
            get
            {
                return _punchType;
            }
            set
            {
                _punchType = value;
            }
        }

        public string FirstName
		{
			get
			{
                return _firstName;
			}
			set
			{
                _firstName = value;
			}
		}

        public string LastName
		{
			get
			{
                return _lastName;
			}
			set
			{
                _lastName = value;
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
	}
}
