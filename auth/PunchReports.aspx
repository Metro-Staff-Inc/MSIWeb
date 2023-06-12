<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PunchReports.aspx.cs" Inherits="MSI.Web.MSINet.PunchReports" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx"  TagName="MSINetSectionHeader"   TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx"             TagName="MastHead"              TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx"      TagName="MSINetSubHeader"       TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx"       TagName="MSINetMainMenu"        TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetPunchReports.ascx"   TagName="MSINetPunchReports"    TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="AdminHead" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript"
        src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js">
    </script>
    <style type="text/css">
    .img
    {
        background-color:#FFA;
        font-size:26px;
        border:5px double blue;
        margin-left:auto;
        margin-right:auto;        
    }
    .lightBlue
    {
	    background-color: #FFEE99;
    }
    .white
    {
        background-color: #FFFFFF;
    }
    .grey
    {
        background-color: #E9EBEF;
    }
    .lightRed 	
    {
			background-color: Silver;
			position: absolute;
			z-index: 2;
			color: black;
		}    
    </style>    


</head>

<body>
    <form id="form1" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Display punch records" />
            <br />
            <uc6:MSINetPunchReports ID="ctlPunchReports" runat="server" />
    </div>
    </form>
</body>

</html>
