<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="MSI.Web.MSINet.PDA.MainMenu" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body style="width:240px;">  <div align="center">
    <form id="form1" runat="server">
        &nbsp; &nbsp;
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/msi_logo_mobile.jpg" /><br />
        <br />
        <asp:Label ID="lblSubHeader" runat="server" Text="MSI Web Trax - "></asp:Label><br />
        &nbsp;<asp:Panel ID="pnlMain" runat="server" HorizontalAlign="Left" Width="230px">
            <asp:Panel ID="pnlMainLinks" runat="server" Height="67px" Width="230px">
                <asp:LinkButton ID="lnkCheckIn" runat="server" PostBackUrl="~/auth/pda/CheckIn.aspx">Check In/Out</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="linkEmployeeHistory" runat="server" PostBackUrl="~/auth/pda/EmployeeHistory.aspx">Employee History</asp:LinkButton></asp:Panel>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>
