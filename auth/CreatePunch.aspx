<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreatePunch.aspx.cs" Inherits="auth_CreatePunch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server">Badge Number: </asp:Label><asp:TextBox ID="txtBadge" runat="server">00000</asp:TextBox>
        <br />
        <asp:Label ID="lblDate" runat="server" Text="Punch DateTime: "></asp:Label><asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
    </div>
    <p>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Punch Clock" />
    </p>
    </form>
</body>
</html>
