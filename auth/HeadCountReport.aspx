<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HeadCountReport.aspx.cs" Inherits="MSI.Web.MSINet.HeadCountReport" %>

<%@ Register Src="../Controls/MSINetHeadCountReport.ascx" TagName="MSINetHeadCountReport"
    TagPrefix="uc6" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="HoursReport" />

        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Head Count Report" />
        <br />
        <asp:Panel ID="pnlHeadCountReport" runat="server" HorizontalAlign="Left" Width="905px">
            <div style="text-align:right"><asp:HyperLink ID="lnkPrinterFriendly" Target="_blank" NavigateURL="~/auth/HeadCountReport.aspx?print=1" runat="server" Text="Printer Friendly Version"></asp:HyperLink></div>
            <uc6:MSINetHeadCountReport id="ctlHeadCountReport" runat="server">
            </uc6:MSINetHeadCountReport>
        </asp:Panel>

    </form>
 </div>    
</body>
</html>
