<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClientInfo.aspx.cs" Inherits="MSI.Web.MSINet.ClientInfo" %>

<%@ Register Src="~/Controls/MSINetClientInfo.ascx" TagName="MSINetClientInfo" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.fixedheadertable.min.js"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

    <meta http-equiv="refresh" content="600" />
</head>
<body><div align="center">
    <form id="form1" runat="server">
    <div>
    <uc1:MastHead ID="MastHead1" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="ClientInfo"/>
                
        <uc4:MSINetSectionHeader ID="ctlSectionHead" runat="server" SectionHeader="ClientInfo" />
        <br />
        <uc5:MSINetClientInfo ID="MSINetClientInfo" runat="server" />
    </form>
</body>
</html>
