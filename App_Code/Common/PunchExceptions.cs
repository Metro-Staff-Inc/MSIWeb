using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for PunchExceptions
/// </summary>
namespace MSI.Web.MSINet.Common
{
    public struct PunchExceptions
    {
        public const int BeforeShiftStart = 1;
        public const int EmployeeNotAuthorized = 2;
        public const int CheckInAttemptAfterShiftEnd = 3;
    }
}