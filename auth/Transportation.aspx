<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Transportation.aspx.cs" Inherits="MSI.Web.MSINet.Transportation" %>
<%@ Register Src="~/Controls/MSINetTransport.ascx" TagName="MSINetTransport" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transportation Info</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
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

    <script src="../Scripts/Transport.js"></script>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <style>
       #map {
        height: 400px;
        width: 1000px;
       }
       #employeeList {
        /*height: 400px;*/
        width: 1200px;
        padding-bottom: 20px;
       }
       #dateSelect {
        height: 60px;
        width: 1000px;
       }
    </style>
</head>
<body>
    <div id="main" runat="server" align="center">
    <form id="Form2" runat="server">
        <div><uc1:MastHead ID="ctlMastHead" runat="server" /></div><br />
        <div><uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" /></div><br />
        <div><uc3:MSINetMainMenu ID="MainMenu2" runat="server" SelectedSection="HoursReport" /></div>
        <div><uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Transportation" /></div>
        <uc6:MSINetTransport id="ctlTransport" runat="server"></uc6:MSINetTransport>

        <script async defer
            src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCV9GlpoLiirJUVrpiqz0xjrAt3sdBP5Ac">
        </script>
    </form>
    </div>
</body>
</html>
