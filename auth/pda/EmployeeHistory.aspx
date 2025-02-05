<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeHistory.aspx.cs" Inherits="MSI.Web.MSINet.PDA.EmployeeHistory" %>

<%@ Register Src="~/Controls/MSINetEmployeeHistory.ascx" TagName="MSINetEmployeeHistory" TagPrefix="uc5" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body style="width:240px;">  <div align="center">
    <form id="form1" runat="server">
        &nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/msi_logo_mobile.jpg" /><br />
        <asp:Table ID="tblMain" runat="server" style="width:240px;">
            <asp:TableRow ID="TableRow3" runat="server">
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="Center" >
                    <asp:Panel ID="pnlMain" runat="server" HorizontalAlign="Left">
                        <uc5:MSINetEmployeeHistory id="ctlEmployeeHistory" runat="server">
                        </uc5:MSINetEmployeeHistory>
                    </asp:Panel>
                    </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="TableRow1" runat="server">
                <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Center" >
                        <asp:Table ID="tblPDAMainMenu" runat="server" HorizontalAlign="Left" Width="240px" Style="border-top-color:#000000; border-top-style:solid; border-top-width:1px;">
                            <asp:TableRow ID="TableRow2" runat="server">
                            
                        <asp:TableCell ID="TableCell6" runat="server" HorizontalAlign="Center" >
                            <asp:LinkButton ID="lnkPDAMain" runat="server" Text="Main Menu" PostBackUrl="~/auth/pda/MainMenu.aspx"></asp:LinkButton>&nbsp;�&nbsp;
                            <asp:LinkButton ID="lnkPDACheckInOut" runat="server" Text="Check In/Out" PostBackUrl="~/auth/pda/CheckIn.aspx"></asp:LinkButton>
                            &nbsp;�&nbsp;
                            <br />
                            <asp:Label ID="Label1" runat="server" Text="Employee History"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </form>
 </div>    
</body>
</html>