<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="MSI.Web.MSINet.MainMenu" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <!-- load jQuery, should already be loaded -->
    <!--
    <script type="text/javascript"
        src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js">
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    </script>
    <script type="text/javascript">
        if (typeof jQuery == 'undefined')
            alert("jQuery not loading!");
        $(document).ready(function () {
            var divs = $('div');
            alert("number of divs = " + divs.length);
            divs.each(function () {
                alert($(this).html);
            })
        });
    </script>
    -->
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
        <uc1:MastHead ID="MastHead1" runat="server" />
        &nbsp;&nbsp;<uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="MainMenu2" runat="server" />
        <%--
        <!--<asp:Panel ID="pnlMain" runat="server" HorizontalAlign="center" style="text-align:left;" Width="1000px">-->
        --%>    
            <br />
            <%-- 
            <asp:Panel ID="pnlMainLinks" runat="server" Width="389px" style="text-align:left;">
                <asp:LinkButton ID="lnkHeadCountReport" CssClass="MainMenuLinkText" runat="server" PostBackUrl="~/auth/HeadCountReport.aspx">Head Count Report</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="lnkDaysWorkedReport" CssClass="MainMenuLinkText" runat="server" PostBackUrl="~/auth/DaysWorkedReport.aspx">Days Worked Report</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="lnkInvoiceProcessing" CssClass="MainMenuLinkText" runat="server" PostBackUrl="~/auth/InvoiceSummary.aspx">Invoice Processing</asp:LinkButton><br />
                <br />
                <asp:LinkButton ID="lnkAdministrative" Visible="false" CssClass="MainMenuLinkText" runat="server" PostBackUrl="~/auth/Administrative.aspx">Administrative Settings</asp:LinkButton><br />
            </asp:Panel>
            
        </asp:Panel>
            --%>
    </form>
 </div>    
</body>
</html>
