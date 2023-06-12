<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Unauthorized.aspx.cs" Inherits="MSI.Web.MSINet.MainMenu" %>

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
        <uc1:MastHead ID="MastHead1" runat="server" />
        &nbsp;&nbsp;<uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <asp:Panel ID="pnlMain" runat="server" Height="414px" HorizontalAlign="Center" Width="999px">
            <br />
            <asp:Label ID="lblUnauthorized" runat="server" CssClass="MSINetSubHeading" Text="You are not authorized to use this feature of MSI Web Trax.  Thank You."></asp:Label>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>
