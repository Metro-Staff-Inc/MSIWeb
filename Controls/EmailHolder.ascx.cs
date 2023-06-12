using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MSI.Web.Controls
{
    public partial class EmailHolder : BaseMSINetControl
    {
        private string _title = string.Empty;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                this.lblHeader.Text = _title;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}