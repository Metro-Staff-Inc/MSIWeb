<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Administrative.aspx.cs" Inherits="MSI.Web.MSINet.Administrative" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetAdministrative.ascx" TagName="MSINetAdministrative" TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="AdminHead" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
    .ClientPreferences
    {
        width:500px;
        margin:auto;
        padding:10 10 10 10;
        text-align:left;
        border:1px solid Orange;
        font-family: Verdana,Arial;
	    font-size: 100%;
	    font-weight:bold;
	    color: #003776;
    }
    .Row
    {
        background-color:#FFFFFF;
    }
    .AltRow
    {
        background-color:#DDDDFF;
    }
    </style>
</head>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<body>
    <form id="form1" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Administrative" />
            <br />
            <uc6:MSINetAdministrative ID="ctlAdministrative" runat="server" />
            <br />
    </div>
    </form>
</body>
</html>
