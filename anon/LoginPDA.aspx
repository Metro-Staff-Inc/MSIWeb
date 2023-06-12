<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPDA.aspx.cs" Inherits="MSI.Web.MSINet.PDA.Login" %>

<%@ Register Src="~/Controls/Login.ascx" TagName="Login" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <div>
        &nbsp; &nbsp;&nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/Images/msi_logo_mobile.jpg" /><br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="MSI Web Trax - Login"></asp:Label><br />
        <p />
        <asp:Panel ID="pnlLoginCtl" runat="server" HorizontalAlign="Left" Width="240px" Wrap="False">
            <uc3:Login ID="ctlLogin" runat="server" />
        </asp:Panel>
    </div>
    </form>
 </div>    
</body>
</html>
