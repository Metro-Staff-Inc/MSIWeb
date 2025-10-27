<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MastHead.ascx.cs" Inherits="MSI.Web.Controls.MastHead" %>

<asp:HiddenField runat="server" ID="clientDir" Value="259-272"/>        
<asp:HiddenField runat="server" ID="clientID" Value="259"/>        
<asp:HiddenField runat="server" ID="userID" Value="itdept" />
<asp:HiddenField runat="server" ID="weekEnd" Value="7" />
<asp:HiddenField runat="server" ID="multiplier" Value="1.26" />

<%--
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
--%>
    <asp:Panel ID="pnlMastHead" runat="server" CssClass="MastHead">
        &nbsp;
        <asp:Panel ID="Panel2" runat="server" CssClass="MastHead_Logo">
            <img src="../images/MSILogo.png" border="0" alt="Metro Staff, Inc." id="IMG1" />
        </asp:Panel>
        &nbsp;
        <asp:Panel ID="Panel3" runat="server" CssClass="MastHead_Heading" >
            <img src="../images/MetroStaffHeaderText.png" border="0" alt="Metro Staff, Inc." id="IMG2" />
        </asp:Panel>
    </asp:Panel>
<asp:Panel ID="pnlMastHeadMobile" runat="server" Height="50px" HorizontalAlign="Left"
    Width="240px">
    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/msi_logo_mobile.jpg" />
</asp:Panel>

