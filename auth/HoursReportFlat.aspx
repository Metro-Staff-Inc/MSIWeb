<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HoursReportFlat.aspx.cs" Inherits="MSI.Web.MSINet.HoursReportFlat" %>
<%@ Register Src="~/Controls/MSINetHoursReportFlat.ascx" TagName="MSINetHoursReportFlat" TagPrefix="uc6" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>

    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
.zebra
{
    background-color:#DDDDFF;
}
.error
{
    color:Red;   
}
table.reference {
    background-color: #FFFFFF;
    border: 1px solid #C3C3C3;
    border-collapse: collapse;
    width: 100%;
}
table.reference th {
    background-color: #E5EECC;
    border: 1px solid #C3C3C3;
    padding: 3px;
    vertical-align: top;
}
table.reference td {
    border: 1px solid #C3C3C3;
    padding: 3px;
    vertical-align: top;
}       
 
.tblDtl
{
    font-size:12;
    border:1px solid blue;
}
.tooltip
{
    width:320;
    height:280;
    position:absolute;
    border:5px double #33A;
    background-color:#FFFFFF;
    padding: 6px 6px;
}
.bold
{
    text-decoration:underline;
    font-weight:bolder;
}
.DlyHrs
{
    background-color:#FFFFFF;
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 0pt #cccccc;
     width:40px;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}    
.highLight2
{
	 background-color: #FFEE99;
     background-image:none;
}
.highLight
{
	 background-color: #FFEE99;
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 0pt #cccccc;
     width:40px;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}

.DlyHrsAlt
{
    background-color:#E3EAEB;
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 0pt #cccccc;
     width:40px;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}    

.DlyHrsOT
{
    background-color:#FFAAAA;
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 0pt #cccccc;
     width:40px;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}

.HrsDetail td
{
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 1pt #000000;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}    
.HrsDetailAlt td
{
     mso-number-format:\#\,\#\#0\.00;
     border-bottom:solid 1pt #000000;
     padding:2pt 2pt 2pt 2pt;
     color:Black;
     background-color:#DDDDCC;
     text-align:right;
     font-family:Arial;
     font-size:10pt;
     font-weight:normal;
}    
.Detail td
{
    /*border: 2pt solid #0000FF; */
    padding:2pt 12pt 2pt 2pt; 
    height:22px; 
    color:#000000; 
    background-color:#7AFA7B; 
    text-align:right; 
    font-family:Arial; 
    font-size:11pt; 
    font-weight:bold;    
}
.img
{
    display:block;
    background-color:#FFA;
    font-size:26px;
    border:5px double blue;
    margin-left:auto;
    margin-right:auto;        
}

    </style>
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="MainMenu2" runat="server" SelectedSection="HoursReportFlat" />

        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Hours Report Flat" />
        <!--<asp:Label ID="lblTitle" runat="server" CssClass="MSINetSectionHeading" Text="Hours Report" Width="145px"></asp:Label>
        &nbsp;&nbsp;&nbsp;--><br />
        <br />
        <asp:Panel ID="pnlHoursReportFlat" runat="server" HorizontalAlign="Left" Width="1100px">
            <div style="text-align:right"><asp:HyperLink ID="lnkPrinterFriendly" Target="_blank" NavigateURL="~/auth/HoursReportFlat.aspx?print=1" runat="server" Text="Printer Friendly Version"></asp:HyperLink></div>
            <uc6:MSINetHoursReportFlat id="ctlHoursReportFlat" runat="server"></uc6:MSINetHoursReportFlat>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>
