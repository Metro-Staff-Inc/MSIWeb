<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logoff.aspx.cs" Inherits="MSI.Web.MSINet.Logoff" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
        <uc1:MastHead ID="MastHead1" runat="server" />
        &nbsp;&nbsp;<uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" SectionHeader="Log Off" ShowLogoffLink="false" />
        <br />
        <asp:Panel ID="pnlMain" runat="server" Height="414px" HorizontalAlign="Center" Width="999px">
            <br />
            <asp:Label ID="lblLogoff" runat="server" CssClass="MSINetSubHeading" Text="Thank You for Visiting MSI Web Trax.  Come Back Soon."></asp:Label>
            <br />
            <br />
            <asp:HyperLink ID="hlnkReturn" runat="server" NavigateUrl="~/anon/Login.aspx">Return to MSI Web Trax</asp:HyperLink></asp:Panel>
    </form>
 </div>    
</body>
</html>
