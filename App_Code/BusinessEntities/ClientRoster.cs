using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ClientRoster
/// </summary>
namespace ClockWebServices
{
    public class ClientRoster
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ICCardNum { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Deleted { get; set; }
        /* directory of photo info is found using ID number */

        public ClientRoster()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}