<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="MSI.Web.MSINet.Login" %>

<%@ Register Src="~/Controls/Login.ascx" TagName="Login" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <!-- load jquery -->
    <script type="text/javascript"
        src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js">
    </script>
</head>
<body>
  <div align="center">
    <form id="form1"  runat="server">
    <div align="center">
        <uc1:MastHead ID="ctlMastHead" runat="server" />    
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" SectionHeader="Login" ShowLogoffLink="false"  />
        <br />
        <asp:Panel ID="pnlLoginCtl" runat="server" Height="50px" HorizontalAlign="Center" Width="996px" Wrap="False">
            <uc3:Login ID="ctlLogin" runat="server" />
        </asp:Panel>
    </div>
   </form>
 </div>    
</body>
</html>
