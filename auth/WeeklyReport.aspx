<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeeklyReport.aspx.cs" Inherits="MSI.Web.MSINet.WeeklyReport" %>
<%@ Register Src="~/Controls/MSINetWeeklyReport.ascx" TagName="MSINetWeeklyReport" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MSI Web Trax</title>
    <script type="text/javascript"
        src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js">
    </script>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="~/includes/WeeklyReport.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="HoursReport2" />

        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Hours Report 2" />
        <!--<asp:Label ID="lblTitle" runat="server" CssClass="MSINetSectionHeading" Text="Hours Report" Width="145px"></asp:Label>
        &nbsp;&nbsp;&nbsp;--><br />
        <br />
        <asp:Panel ID="pnlHoursReport" runat="server" HorizontalAlign="Left" Width="1100px">
            <uc6:MSINetWeeklyReport id="ctlHoursReport" runat="server">
            </uc6:MSINetWeeklyReport>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>
