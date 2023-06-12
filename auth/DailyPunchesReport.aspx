<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyPunchesReport.aspx.cs" Inherits="MSI.Web.MSINet.DailyPunchesReport" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx"  TagName="MSINetSectionHeader"   TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx"             TagName="MastHead"              TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx"      TagName="MSINetSubHeader"       TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx"       TagName="MSINetMainMenu"        TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MSINetDailyPunchesReport.ascx"   TagName="MSINetDailyPunchesReport"    TagPrefix="uc6" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" > 
<head id="AdminHead" runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript"
        src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js">
    </script>

        <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="../javascriptOOXml/linq.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml-extensions.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-load.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-inflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-deflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/FileSaver.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/openxml.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/TemplateDocumentB64.js"></script>
        <script type="text/javascript" src="../Scripts/ExcelExport.js"></script>
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
    <form id="form2" runat="server">
    <div align="center">
            <uc1:MastHead ID="ctlMastHead" runat="server" />
            <br />
            <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
            <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Daily punches" />
            <br />
            <uc6:MSINetDailyPunchesReport ID="ctlPunchReports" runat="server" />
    </div>
    </form>
</body>
</html>
