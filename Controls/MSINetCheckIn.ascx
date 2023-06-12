<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetCheckIn.ascx.cs" Inherits="MSI.Web.Controls.MSINetCheckIn" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>
<asp:Panel ID="pnlValidation" runat="server" CssClass="MSINetValidationText"
    Width="230px">
    <asp:Label ID="lblValidationMessage" runat="server" CssClass="MSINetValidationText"
        Text="Label" Visible="False"></asp:Label></asp:Panel>
<br />
<asp:Panel ID="pnlInput" runat="server" BorderColor="White" HorizontalAlign="Left" Width="230px" Direction="LeftToRight">
    <asp:Label ID="lblBadgeNumber" runat="server" CssClass="InputTableLabelText" Text="Badge #:"></asp:Label><br />
    <asp:TextBox ID="txtBadgeNumber" runat="server" CssClass="InputTableInputLabelText" Text="0" OnTextChanged="txtBadgeNumber_TextChanged" Width="177px"></asp:TextBox>
    <asp:Literal ID="ltrlBR1" runat="server"><br /><br /></asp:Literal>
    <uc1:MSINetCalendar ID="ctlCheckInDate" runat="server" DisplayMode="SingleDate"
        LabelText="Check In Date:" /><asp:Literal ID="ltrlBR2" runat="server"><br /><br /></asp:Literal>
    <asp:Label ID="lblCheckInTime" runat="server" CssClass="MSINetBodyText" Text="Check In Time:"></asp:Label><asp:Literal ID="ltrlBR3" runat="server"><br /></asp:Literal>
    <asp:DropDownList ID="cboHour" runat="server" AutoPostBack="false">
    </asp:DropDownList><asp:Label ID="lblColonChar" Text=":" runat="server"></asp:Label>
    <asp:DropDownList ID="cboMinute" runat="server" AutoPostBack="false">
    </asp:DropDownList><asp:Label ID="lblSpaceChar" runat="server">&nbsp;</asp:Label>
    <asp:DropDownList ID="cboAMPM" runat="server" AutoPostBack="false">
    </asp:DropDownList>
    <br /><br />
<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
    <asp:Label runat="server" Text="Time Interval: "></asp:Label>
    <asp:Label runat="server" ID="lblTimeInterval" ></asp:Label>
    <asp:Button ID="btnCheckIn" runat="server" OnClick="btnCheckIn_Click" Text="Record Swipe" /></asp:Panel>
</asp:Panel>
<br />
<asp:Panel ID="pnlConfirmation" runat="server" CssClass="MSINetValidationText"
    Width="230px">
    <asp:Label ID="lblConfirmationMessage" runat="server" CssClass="MSINetCheckInConfirmationText"
        Visible="False"></asp:Label></asp:Panel>
