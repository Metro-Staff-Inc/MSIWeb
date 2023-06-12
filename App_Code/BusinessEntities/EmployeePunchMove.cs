using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
	/// <summary>
	/// Summary description for Content.
	/// </summary>
	public class EmployeePunchMove
	{
        private string _movePunchList = string.Empty;
        private Department _moveToDepartment = new Department();
        private DateTime _moveDateTime = new DateTime(1, 1, 1);

        public EmployeePunchMove()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public string MovePunchList
		{
			get
			{
                return _movePunchList;
			}
			set
			{
                _movePunchList = value;
			}
		}

        public Department MoveToDepartment
        {
            get
            {
                return _moveToDepartment;
            }
            set
            {
                _moveToDepartment = value;
            }
        }

        public DateTime MoveDateTime
        {
            get
            {
                return _moveDateTime;
            }
            set
            {
                _moveDateTime = value;
            }
        }
	}
}
