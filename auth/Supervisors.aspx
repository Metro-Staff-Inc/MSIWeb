<%@ Page Language="C#" EnableEventValidation="false" EnableViewState="false" EnableViewStateMac="false" 
        AutoEventWireup="true" CodeFile="Supervisors.aspx.cs" Inherits="MSI.Web.MSINet.Supervisors" %>
<%@ Register Src="~/Controls/MSINetUserRoles.ascx" TagName="MSINetUserRoles" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetSupervisors.ascx" TagName="MSINetSupervisors" TagPrefix="uc6" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="Supervisors" />

        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Set Supervisors / Depts" />
        <br />
        <uc6:MSINetSupervisors ID="ctlSupervisors" runat="server"></uc6:MSINetSupervisors>
    </form>
 </div>    
</body>
</html>
