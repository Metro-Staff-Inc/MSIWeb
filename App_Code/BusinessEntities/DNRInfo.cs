using System;
using System.Collections;
using MSI.Web.MSINet.Common;

namespace MSI.Web.MSINet.BusinessEntities
{
    public class DNRInfo
    {
        private int         _clientDnrID;
        private int         _clientId;
        private string      _clientName;
        private Boolean     _allClients;
        private string      _dnrReason;
        private string      _firstName;
        private string      _lastName;
        private string      _aidentNumber;
        private string      _startDate;
        private string      _shift;
        private string      _supervisor;
        public string LocationName { get; set; }


        public int ClientDnrID
        {
            get { return _clientDnrID; }
            set { _clientDnrID = value; }
        }
        public string Shift
        {
            get
            {
                return _shift;
            }
            set
            {
                _shift = value;
            }
        }
        public string StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }
        public string Supervisor
        {
            get
            {
                return _supervisor;
            }
            set
            {
                _supervisor = value;
            }
        }
        public int ClientId
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
        public string ClientName
        {
            get
            {
                return _clientName;
            }
            set
            {
                _clientName = value;
            }
        }
        public Boolean AllClients
        {
            get
            {
                return _allClients;
            }
            set
            {
                _allClients = value;
            }
        }
        public string DNRReason
        {
            get
            {
                return _dnrReason;
            }
            set
            {
                _dnrReason = value;
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
        public string AidentNumber
        {
            get
            {
                return _aidentNumber;
            }
            set
            {
                _aidentNumber = value;
            }
        }
    }
}