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
//jhm
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.BusinessLogic;
using MSI.Web.MSINet.Common;


public partial class Controls_MSINetCalendar : System.Web.UI.UserControl
{
    private Client _clientInfo = new Client();//jhm

    private DateTime _selectedDate;
    private CalendarDisplayMode _displayMode = CalendarDisplayMode.SingleDate;
    private string _labelText = "Select a Date:";
    private string _dateChanged = "";

    public string DateChanged
    {
        get
        {
            return _dateChanged;
        }
        set
        {
            _dateChanged = value;
        }
    }
    public Client ClientInfo //jhm
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

    public enum CalendarDisplayMode
    {
        WeekEnding,
        SingleDate
    }

    public CalendarDisplayMode DisplayMode
    {
        get
        {
            return _displayMode;
        }
        set
        {
            _displayMode = value;
        }
    }

    public DateTime SelectedDate
    {
        get
        {
            return _selectedDate;
        }
        set
        {
            _selectedDate = value;
        }
    }

    public string LabelText
    {
        get
        {
            return _labelText;
        }
        set
        {
            _labelText = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        cdrPeriod.Visible = false;
        this.lblDateTime.Text = this._labelText;
        if (txtDateTime.Text.Trim() != "")
        {
            this._selectedDate = DateTime.Parse(txtDateTime.Text);
        }

        //get the client info for this user
        try
        {
            _clientInfo = (Client)Session["ClientInfo"];
        }
        catch (Exception ex)
        {
            Session["ClientInfo"] = null;
            throw ex;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (this._selectedDate != null && this._selectedDate.Date != new DateTime(1, 1, 1).Date)
        {
            this.txtDateTime.Text = this._selectedDate.ToString("MM/dd/yyyy");
        }
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtDateTime.Text.Trim() != "")
            {
                cdrPeriod.VisibleDate = DateTime.Parse(txtDateTime.Text);
                this._selectedDate = DateTime.Parse(txtDateTime.Text);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        cdrPeriod.Visible = true;
    }
    protected void cdrPeriod_SelectionChanged(object sender, EventArgs e)
    {
        this.txtDateTime.Text = cdrPeriod.SelectedDate.ToString("MM/dd/yyyy");
        this._selectedDate = cdrPeriod.SelectedDate;
        cdrPeriod.Visible = false; //hiding the calendar.

        // Write some text to output
        // Response.Write("User Control Button Click <BR/>");
    }

    protected void cdrPeriod_DayRender(object sender, DayRenderEventArgs e)
    {
        // Only allow sundays to be selected.
        if (e.Day.IsOtherMonth)
            e.Cell.ForeColor = System.Drawing.Color.DarkGray;
        else
            e.Cell.ForeColor = System.Drawing.Color.Black;

        if (this._displayMode == CalendarDisplayMode.WeekEnding)
        {
            Boolean saturdayClient = _clientInfo.ClientID == 325 || _clientInfo.ClientID == 326 || _clientInfo.ClientID == 327 || _clientInfo.ClientID == 256 || _clientInfo.ClientID == 121 || _clientInfo.ClientID == 258 || _clientInfo.ClientID == 205;
            if ((e.Day.Date.DayOfWeek == DayOfWeek.Saturday && saturdayClient)
                   || (e.Day.Date.DayOfWeek == DayOfWeek.Sunday && !saturdayClient))
                e.Day.IsSelectable = true;
            else
                e.Day.IsSelectable = false;
        }
    }

    protected void cdrPeriod_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        this.cdrPeriod.Visible = true;
    }
}
