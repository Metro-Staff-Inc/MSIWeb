<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapEmployeeID.aspx.cs" Inherits="MSI.Web.MSINet.MapEmployeeID" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetClientRoster.ascx" TagName="MSINetClientRoster" TagPrefix="uc6" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="AdminHead" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/MapEmployeeId.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="MSI / Suncast ID mapping" />
            <br />
            <asp:Panel ID="Panel1" CssClass="MapEmployee" runat="server">
                <asp:Label ID="Label1" runat="server" Text="Employee ID #: "></asp:Label>
                <asp:TextBox ID="txtboxEmpId" runat="server"></asp:TextBox>
                <input type="button" id="btnEmpId" runat="server" value="Get Suncast ID" />
            </asp:Panel>
            <asp:Panel ID="MapIdResults" CssClass="MapEmployee" runat="server">
            </asp:Panel>
    </div>
    </form>
</body>
</html>
