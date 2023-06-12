<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketTrackerExcel.aspx.cs" Inherits="MSI.Web.MSINet.TicketSummaryExcel" %>

<%@ Register Src="~/Controls/MSINetTicketTracker.ascx" TagName="MSINetTicketTracker" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetTicketTrackerException.ascx" TagName="MSINetTicketTrackerException"
    TagPrefix="uc7" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<%@ Register Src="~/Controls/MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns:x="urn:schemas-microsoft-com:office:excel">

<head runat="server">
    <title>MSI Web Trax</title>
    <style type="text/css">
    @page 
    {
        mso-header-data:"&C Page &P of &N";        
        mso-page-orientation:portrait;
        margin:.5in .25in .25in .25in;
        mso-header-margin:.3in;
        mso-footer-margin:.3in;
	}
    </style>
    <xml> 
         <x:ExcelWorkbook> 
          <x:ExcelWorksheets> 
           <x:ExcelWorksheet> 
                <x:Name>HoursReport</x:Name> 
                <x:WorksheetOptions>
    			<x:Selected/>
	    		<x:FreezePanes/>
		    	<x:FrozenNoSplit/>
			    <!--- Bottom row number of top pane. --->
			    <x:SplitHorizontal>4</x:SplitHorizontal>
			    <x:TopRowBottomPane>4</x:TopRowBottomPane>

                 <x:FitToPage/> 
                 <x:Print> 
                  <x:FitHeight>98</x:FitHeight>
                  <x:ValidPrinterInfo/> 
                  <x:Scale>100</x:Scale> 
                  <x:HorizontalResolution>600</x:HorizontalResolution> 
                  <x:VerticalResolution>600</x:VerticalResolution> 
                 </x:Print> 
                 <x:Selected/> 
                 <x:ActivePane>2</x:ActivePane>
                 <x:ProtectContents>False</x:ProtectContents> 
                 <x:ProtectObjects>False</x:ProtectObjects> 
                 <x:ProtectScenarios>False</x:ProtectScenarios>
                </x:WorksheetOptions> 
			    <x:PageMargins x:Left="0.25" x:Right="0.25" x:Top="0.5" x:Bottom="0.25" x:Header="0.3" x:Footer="0.3" />           
           </x:ExcelWorksheet> 
          </x:ExcelWorksheets> 
          <x:ProtectStructure>False</x:ProtectStructure> 
          <x:ProtectWindows>False</x:ProtectWindows> 
         </x:ExcelWorkbook> 
         <x:ExcelName> 
          <x:Name>Print_Titles</x:Name> 
          <x:SheetIndex>1</x:SheetIndex>
          <x:Formula>='TicketTracker'!$1:$4</x:Formula>
         </x:ExcelName> 
    </xml>

</head>
<body>  
<%--<!--    <div align="center">
        <asp:Panel ID="Panel1" runat="server" Height="159px" HorizontalAlign="Left" Width="1100px" EnableViewState="false">
            <form id="Form1" runat="server" enableviewstate="false">
                <uc6:MSINetTicketTracker id="ctlTicketTracker" runat="server" ExportToExcel="true" EnableViewState="false">
                </uc6:MSINetTicketTracker> 
            </form>
        </asp:Panel>
    </div> -->--%>  
    <form id="form1" runat="server">
    <div align="center">
    <div>
        <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
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
                        <asp:Label ID="lblShift" runat="server"
                            CssClass="InputTableLabelText" Text="Shift:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboShift" runat="server" CssClass="InputTableInputLabelText"  Width="214px">
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
                        <asp:Button ID="btnSwap" runat="server" Text="Exact Time" />
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader"><asp:CheckBox ID="chkClearEmptyRows" runat="server" Text="Don't Display Empty Rows" /></asp:TableCell>
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
