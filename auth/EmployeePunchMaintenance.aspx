<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeePunchMaintenance.aspx.cs" Inherits="MSI.Web.MSINet.EmployeePunchMaintenance" %>

<%@ Register Src="../Controls/MSINetPunchMaintenance.ascx" TagName="MSINetPunchMaintenance" TagPrefix="uc6" %>
<%@ Register Src="../Controls/MSINetTicketTracker.ascx" TagName="MSINetTicketTracker" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="MSINetMainMenu1" runat="server" SelectedSection="TicketTracking" />
        <uc4:MSINetSectionHeader ID="ctlSectionHead" runat="server" SectionHeader="Employee Time Maintenance" />
        <br />
        
        <div style="padding-left:10px">
            <asp:Panel ID="Panel2" runat="server" HorizontalAlign="left" Width="900px">
                <uc6:MSINetPunchMaintenance ID="ctlEmployeePunchMaintenance" runat="server" />
            </asp:Panel>
            <br />
            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="center" Width="900px">
                <asp:Button id="btnReturn" runat="server" Text="Return to Ticket Tracking" OnClick="btnReturn_Click" />
            </asp:Panel>
        </div>        
    </form>
 </div>    
</body>
</html>
