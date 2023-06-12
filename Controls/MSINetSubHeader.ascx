<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetSubHeader.ascx.cs" Inherits="MSI.Web.Controls.MSINetSubHeader" %>


<asp:Table ID="tblSubHeader" runat="server" Width="1000px">
<asp:TableRow>
    <asp:TableCell ID="tdLabel" runat="server" HorizontalAlign="Left">
        <asp:Label ID="lblSubHeader" runat="server" CssClass="MSINetSubHeading" Text="Label"></asp:Label>
    </asp:TableCell>
    <asp:TableCell ID="tdChangeClient" runat="server" HorizontalAlign="Right" ColumnSpan="2">
        <asp:Label ID="lblChangeClient" runat="server"
            CssClass="MSINetMastHeadLogoff" Text="Change Client:" Width="122px">
        </asp:Label>
        <asp:DropDownList
            ID="cboClient" runat="server" CssClass="MSINetBodyText" Width="214px" AutoPostBack="False">
        </asp:DropDownList>
        <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
    </asp:TableCell>
    <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="Right" Width="50px">
        &nbsp;
    </asp:TableCell>
</asp:TableRow>
</asp:Table>
<asp:Panel ID="pnlPDASubHeader" runat="server" Width="240px" HorizontalAlign="left">
<asp:Label ID="lblPDASubHeader" runat="server" Text="Label"></asp:Label>
<br />
<asp:LinkButton ID="lnkPDAlnkPDALogoff" runat="server" PostBackUrl="~/auth/Logoff.aspx">Log Off</asp:LinkButton></asp:Panel>