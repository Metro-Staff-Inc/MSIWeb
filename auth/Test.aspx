<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="MSI.Web.MSINet.Test" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <title>Test Page</title>

    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/jquery.fancybox.css?v=2.0.6" media="screen" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox.pack.js" type="text/javascript"></script>
    <!--<script language="javascript" src="../Scripts/Pictures.js" type="text/javascript"></script>-->
</head>

<body>
    <form id="form1" runat="server">
    <div visible="false">
        <input type="hidden" runat="server" id="clientID"/>
        <input type="hidden" runat="server" id="webServiceLoc"/>
    </div>
<div id="divBody" align="center">
        <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
        </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="TestPage" />
        <br />
        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Ajax Test Page" />
        <br />



        <script src="../Scripts/AjaxText.js" type="text/javascript"></script>
        <span>Press to call: </span><input type="button" id="twilioCall" onclick="btnGo_Click" runat="server" value="Call!" />
        <script type="text/javascript">
            using Twilio;
            var twilio = new TwilioRestClient("accountSid", "authToken");
            var call = twilio.InitiateOutboundCall("+1555456790", "+15551112222", "http://example.com/handleCall");
            var msg = twilio.SendSmsMessage("+15551112222", "+15553334444", "Can you believe it's this easy to send an SMS?!");
        </script>
</div>
</form>
</body>
</html>
