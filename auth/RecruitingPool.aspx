<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecruitingPool.aspx.cs" Inherits="MSI.Web.MSINet.RecruitingPool" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetPhoneBlast.ascx" TagName="MSINetPhoneBlast" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Recruiting / PhoneBlast</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />
</head>

<body>
    <form id="form1" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Recruit Search / Phone Blast" />
            <br />
            <uc6:MSINetPhoneBlast ID="ctlPhoneBlast" runat="server" />
            <br />
    </div>
    </form>
</body>
</html>
