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
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;
using System.Text;

namespace MSI.Web.Controls
{
    public partial class MSINetTicketTrackerException : BaseMSINetControl
    {
        public enum TrackingDisplayMode
        {
            EmployeeSummary,
            ShiftSummary
        }

        private Client _clientInfo = new Client();
        private HelperFunctions _helper = new HelperFunctions();
        private DateTime _startDateTime;
        private TicketTrackerException _trackerResult = new TicketTrackerException();


        public Client ClientInfo
        {
            get
            {
                return _clientInfo;
            }
            set
            {
                _clientInfo = value;
            }
        }


        public DateTime StartDateTime
        {
            get
            {
                return _startDateTime;
            }
            set
            {
                _startDateTime = value;
            }
        }


        public void LoadTicketTrackerExceptions()
        {
            TicketTrackerException ticketTrackerInput = new TicketTrackerException();

            ticketTrackerInput.ClientID = _clientInfo.ClientID;
            ticketTrackerInput.PeriodStartDateTime = DateTime.Parse(this._startDateTime.ToString("MM/dd/yyyy 00:00:00"));
            ticketTrackerInput.PeriodEndDateTime = DateTime.Parse(this._startDateTime.ToString("MM/dd/yyyy 23:59:59"));
            TicketTrackerBL ticketTrackerBL = new TicketTrackerBL();
            //get the exception tracking
            _trackerResult = ticketTrackerBL.GetTicketTrackingExceptions(ticketTrackerInput, Context.User, null);
            this.rptrTicketTrackerException.DataSource = _trackerResult.Employees;
            this.rptrTicketTrackerException.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        protected void rptrTicketTrackerException_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {

            DateTime currentTime = _helper.GetCSTCurrentDateTime();
            // This event is raised for the header, the footer, separators, and items.
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EmployeeTrackerException employee = (EmployeeTrackerException)e.Item.DataItem;

                //set the item index
                Label lblItem = (Label)e.Item.FindControl("lblItem");
                lblItem.Text = (e.Item.ItemIndex + 1).ToString() + ". ";
                Label lblSwipeDateTime = (Label)e.Item.FindControl("lblSwipeDateTime");
                lblSwipeDateTime.Text = employee.PunchDateTime.ToString("MM/dd/yyyy hh:mm tt");
                Label lblExceptionMessage = (Label)e.Item.FindControl("lblExceptionMessage");
                lblExceptionMessage.Text = employee.PunchExceptionInfo.PunchExceptionMessage;
            }
        }
    }
}