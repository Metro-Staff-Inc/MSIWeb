using System;
using System.Collections.Generic;
using MSI.Web.MSINet.DataAccess;
using MSI.Web.MSINet.BusinessEntities;

namespace MSI.Web.MSINet
{
    public partial class Transportation : BaseMSINetPage
    {
        List<TransportationPunch> tpList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            TransportationDB tdb = new TransportationDB();
            tpList = tdb.getTransportationInfo(new DateTime(2018, 02,01), new DateTime(2018, 03, 29));
        }

        protected override bool IsAuthorizedAccess()
        {
            _isAuthorized = true;
            if (!Context.User.Identity.Name.ToLower().Equals("elgin") &&
                !Context.User.Identity.Name.ToLower().Equals("riznerd") && 
                !Context.User.Identity.Name.ToLower().Equals("maria") &&
                !Context.User.IsInRole("Transportation"))
                _isAuthorized = true;
            return base.IsAuthorizedAccess();
        }
    }
}