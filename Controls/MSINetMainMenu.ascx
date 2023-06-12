<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetMainMenu.ascx.cs" Inherits="MSI.Web.Controls.MSINetMainMenu" %>
<asp:Panel ID="pnlMain" Width="1000px" runat="server">
        
        <div>
            <asp:Menu ID="Menu1" runat="server"
                Orientation="Horizontal"  OnMenuItemDataBound="Menu1_MenuItemDataBound"
                StaticEnableDefaultPopOutImage="False">
            </asp:Menu>

            <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server"
                ShowStartingNode="False" />
        
        </div>
        
</asp:Panel>
<script type="text/javascript" src="https://assets.freshdesk.com/widget/freshwidget.js"></script>
<asp:Panel ID="pnlFreshDesk" runat="server">
    <script type="text/javascript">
        FreshWidget.init("", {"queryString": "", "url": "https://metrostaffinc.freshdesk.com",
            "buttonColor": "white", "buttonBg": "#00AAFF"});
    </script>
</asp:Panel>
