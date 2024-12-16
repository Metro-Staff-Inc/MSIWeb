<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.ascx.cs" Inherits="MSI.Web.Controls.Login" %>

<div style="display: flex; justify-content: center;">
	<div style="width: 240px; background-color: #CCCCFF; border: 4px double blue; padding: 4px;">
		<asp:Panel ID="Panel2" runat="server" Width="230px" CssClass="MSINetLoginIDPanel" HorizontalAlign="Left" BorderColor="White">
			<asp:Label ID="lblUserID" runat="server" CssClass="MSINetBodyText" Text="User Name:" Width="100px"></asp:Label>
			<br />
			<asp:TextBox ID="txtUserID" runat="server" CssClass="MSINetBodyText" Width="175px"></asp:TextBox>
			<br />
			<asp:Label ID="lblPassword" runat="server" Text="Password:" Width="100px" CssClass="MSINetBodyText"></asp:Label>
			<br />
			<asp:TextBox ID="txtPassword" runat="server" CssClass="MSINetBodyText" TextMode="Password" Width="175px"></asp:TextBox>
		</asp:Panel>
		<div style="padding: 16px 4px 4px 4px;">
			<asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left" Width="230px">
				<asp:Button ID="btnLogIn" runat="server" Text="Log In" OnClick="btnLogIn_Click" />
				<asp:Label ID="lblValidationMessage" runat="server" CssClass="MSINetValidationText" Text="Label" Visible="False" Width="230px"></asp:Label>
			</asp:Panel>
		</div>
	</div>
</div>