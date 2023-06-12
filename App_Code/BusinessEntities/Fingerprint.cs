using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Fingerprint
/// </summary>
namespace MSI.Web.MSINet.BusinessEntities
{
    public class FingerprintInfo
    {
        public int xmlCount { get; set; }   
        public int fptCount { get; set; }   
        public int fstCount { get; set; }   
        public List<Fingerprint> fingerprints { get; set; }
    }

    public class Fingerprint
    {
        public int info { get; set; }
        public String aident { get; set; }

        public Fingerprint()
        {
        }
        public Fingerprint(String id)
        {
            aident = id;
        }
    }
}
