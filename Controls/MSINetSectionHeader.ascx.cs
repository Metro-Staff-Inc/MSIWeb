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
    public partial class MSINetSectionHeader : BaseMSINetControl
    {
        private string _sectionHeader = string.Empty;

        public string SectionHeader
        {
            get
            {
                return _sectionHeader;
            }
            set
            {
                _sectionHeader = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblSectionHeader.Text = this._sectionHeader;
        }


    }
}