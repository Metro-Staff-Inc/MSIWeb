<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetDailyPunchesReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetDailyPunchesReport" %>
<!--ctlPunchReports_gvPunchRecord-->

<div style=" width:1000px;">
    <div style="float:left">
        <asp:Panel ID="Panel2" runat="server" BackColor="#E0E0E0" Width="1000px">
            <asp:Table ID="tblInput" runat="server" Width="100%" CellPadding="5" CellSpacing="0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#3399FF">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblDateTime" runat="server" CssClass="InputTableLabelText" Text="Last Creation Date:" Width="122px"></asp:Label><br />
                        <asp:TextBox ID="txtDateTime" runat="server" Width="178px" CssClass="InputTableInputLabelText"></asp:TextBox>
                        <asp:Button Height="20px" ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="..." />
                        <asp:Calendar ID="cdrEndDate" OnSelectionChanged="cdrEndDate_SelectionChanged" OnVisibleMonthChanged="cdrEndDate_VisibleMonthChanged" 
                            runat="server" DisplayMode="SingleDate" LabelText="From Date:"></asp:Calendar>
                    </asp:TableCell>
                    <asp:TableCell  VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Enabled="false" Text="View Punch Report"></asp:Button>
                    </asp:TableCell>
                    <asp:TableCell  VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <input type="button" id="btnExcel" value="Export To Excel" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <asp:GridView AutoGenerateColumns="false" ID="gvPunchRecord" EmptyDataText="No Punches Found in Range!" 
            runat="server" Width="1000px" CellPadding="4" 
            ForeColor="#333333" GridLines="None" >
            <AlternatingRowStyle CssClass="white"/>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle CssClass="grey" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
            <Columns>
              <asp:BoundField ReadOnly="true" HeaderText ="#" />
              <asp:BoundField DataField="AidentNumber"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="ID"/>
              <asp:BoundField DataField="Name"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="Name"/>
              <asp:BoundField DataField="Department"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="Department"/>
              <asp:BoundField DataField="Shift"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="Shift"/>
              <asp:BoundField DataField="PunchIn"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="Arrive"/>
              <asp:BoundField DataField="PunchOut"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                HeaderText="Leave"/>
              <asp:BoundField DataField="Hours"
                ReadOnly="true" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:F2}"
                HeaderText="Hours"/>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server">
        </asp:ObjectDataSource>
    </div>
</div>
    <script>
        $(document).ready(function () {
            var curDate = new Date();
            $("#btnExcel").click(function () {
                exportToXlsx("ctlPunchReports_gvPunchRecord");
            })
        });
    </script>
