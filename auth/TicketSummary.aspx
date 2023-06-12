<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketSummary.aspx.cs" Inherits="MSI.Web.MSINet.TicketSummary" %>
<%@ Register Src="~/Controls/MSINetTicketTracker.ascx" TagName="MSINetTicketTracker" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetTicketTrackerException.ascx" TagName="MSINetTicketTrackerException"
    TagPrefix="uc7" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" > 
<head runat="server">
    <title>MSI Web Trax</title> 
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox.pack.js" type="text/javascript"></script>
    <style type="text/css">
    .ttBtn
    {
        font-size:13px;
    }
    .img
    {
        display:block;
        background-color:#FFA;
        font-size:26px;
        border:5px double blue;
        margin-left:auto;
        margin-right:auto;        
    }
    .ttDate
    {
        font-size:13px;
    }
    .lightBlue
    {
	    background-color: #CCCCFF;
    }
    .underline{
        /*font-weight: bolder;*/
        text-decoration:underline;
    }
    .white
    {
        background-color: #FFFFFF;
    }
    .grey
    {
        background-color: #E9EBEF;
    }
    .yellow
    {
        background-color: #D0D066;
    	border-bottom:solid 0pt #cccccc; 
	    padding:2pt 2pt 2pt 2pt; 
	    color:#333333; 
	    text-align:left; 
	    font-family:verdana,arial; 
	    font-size:85%; 
	    font-weight:normal;
    }
    .tooltip 
    {
        position: absolute;
        border: 4px solid #333;
        z-index: 2;
        background-color: #ffed8a;
        padding: 2px 6px;
    }		
    </style>    

</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <div>
        <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="MSINetMainMenu1" runat="server" SelectedSection="TicketTracking" />
        <uc4:MSINetSectionHeader ID="MSINetSctHdr" runat="server" SectionHeader="Ticket Tracking" />
        <%--<asp:Label ID="Label1" runat="server" CssClass="MSINetSectionHeading" Height="12px"
            Text="Ticket Tracking"></asp:Label>
        &nbsp;&nbsp;--%>
        <br />
        <br />

        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" Width="990" >
        <asp:Menu ID="mnuTrackingType" runat="server" BackColor="#E3EAEB" DynamicHorizontalOffset="2"
            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#666666" Orientation="Horizontal"
            StaticSubMenuIndent="10px">
            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <DynamicHoverStyle BackColor="#666666" ForeColor="White" />
            <DynamicMenuStyle BackColor="#E3EAEB" />
            <StaticSelectedStyle BackColor="Blue" ForeColor="White" />
            <DynamicSelectedStyle BackColor="Blue" BorderColor="White" />
            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
            <Items>
                <asp:MenuItem Selected="True" Text="Roster Tracking" Value="Roster"></asp:MenuItem>
                <asp:MenuItem Text="Exception Tracking" Value="Exceptions"></asp:MenuItem>
            </Items>
            <StaticHoverStyle BackColor="#666666" ForeColor="White" />
       </asp:Menu>
        <asp:Panel ID="Panel2" runat="server" BackColor="#E0E0E0" Width="990px">
            <asp:Table ID="tblInput" runat="server" Width="100%" CellPadding="5" CellSpacing="0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#3399FF">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblLocation" runat="server"
                            CssClass="InputTableLabelText" Text="Location:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboLocation" runat="server" CssClass="InputTableInputLabelText" AutoPostBack="True" OnSelectedIndexChanged="cboLocation_SelectedIndexChanged" Width="214px">
                        </asp:DropDownList>
                        <asp:Label ID="lblShift" runat="server"
                            CssClass="InputTableLabelText" Text="Shift:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboShift" runat="server" CssClass="InputTableInputLabelText" AutoPostBack="True" OnSelectedIndexChanged="cboShift_SelectedIndexChanged" Width="214px">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblDepartment" runat="server" CssClass="InputTableLabelText"
                            Text="Department:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboDepartment"
                            runat="server" CssClass="InputTableInputLabelText" Width="208px">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" Width="230px">
                        <%--<label>Date:</label>--%>
                        <%--<input type="text" runat="server" id="ctlPeriodStart" />--%>
                        <uc6:MSINetCalendar id="ctlPeriodStart" runat="server" DisplayMode="SingleDate" LabelText="Date:"></uc6:MSINetCalendar>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader">
                        <asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                        <asp:Button ID="btnSwap" runat="server" Text="Exact Time" />
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader"><asp:CheckBox ID="chkClearEmptyRows" runat="server" Text="Don't Display Empty Rows" /></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader">
                        <asp:Image ID="Image2" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:Button ID="btnSendEmail" runat="server" Text="Email Roster" OnClick="btnSendEmail_Click" Visible="true" />
                        <!--<%--<asp:HyperLink ID="lnkExport" Target="_blank" NavigateURL="~/auth/TicketTrackerExcel.aspx?date=" runat="server" Text="Export to Excel"></asp:HyperLink>--%>-->
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <asp:Panel ID="pnlTicketTracker" runat="server"  HorizontalAlign="Center" Width="990px">
            <uc5:MSINetTicketTracker ID="ctlTicketTracker" runat="server" />
            <uc7:MSINetTicketTrackerException ID="ctlTicketTrackerException" runat="server" />
        </asp:Panel>
        </asp:Panel>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $('input[id*="btnSelect"]').hide();
    $('input[id*="txtDateTime"]').datepicker();
    $('#<%=btnSwap.ClientID%>').click(
            function () {
                if ($(this).val() == "Exact Time") {
                    $(this).val("Rounded Time");
                }
                else {
                    $(this).val("Exact Time");
                }
                $("#tt").children().find("tr").each(function (i) {
                    if ($(this).children().length >= 5) {
                        $(this).children().each(function (j) {
                            var sp = $(this).find('span:contains("/")');
                            var spText = sp.text();
                            var titleText = sp.attr("title");
                            sp.text(titleText);
                            sp.attr("title", spText);
                        })
                    }
                });
            });
</script>
