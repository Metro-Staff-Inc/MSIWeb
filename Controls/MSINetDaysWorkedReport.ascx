<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetDaysWorkedReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetDaysWorkedReport" EnableViewState="false" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>

<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

<style>
th {
    text-align:center;
}
td {
    text-align:right;
}

</style>



<input type="hidden" name="_employeeId" id="_employeeId" value="" />
<input type="hidden" name="_scrPos" id="_scrPos" value="" />

        <asp:Panel ID="pnlHeader" runat="server" BackColor="#E0E0E0"
            Width="1200px" HorizontalAlign="Center" EnableViewState="false" style="margin-bottom: 0px">
            <asp:Table ID="tblPeriod" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px"><uc1:MSINetCalendar id="ctlStartDate" runat="server" DisplayMode="SingleDate" LabelText="Start Date:"></uc1:MSINetCalendar></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px"><uc1:MSINetCalendar id="ctlEndDate" runat="server" DisplayMode="SingleDate" LabelText="End Date:"></uc1:MSINetCalendar></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px">
                        <asp:Label runat="server">Employees Who Have Punched In The</asp:Label>
                        <asp:DropDownList ID="ddlDaysBack" runat="server">
                            <asp:ListItem Selected="False" Value="24">Last 24 Hours</asp:ListItem>
                            <asp:ListItem Selected="False" Value="48">Last Two Days</asp:ListItem>
                            <asp:ListItem Selected="False" Value="72">Last Three Days</asp:ListItem>
                            <asp:ListItem Selected="False" Value="168">Last Week</asp:ListItem>
                            <asp:ListItem Selected="True" Value="50000">Anytime within range</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="210px">
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="View Days Worked Report"></asp:Button><br />
                        <asp:HyperLink ID="lnkExport" Target="_blank" runat="server" Text="Export to Excel"></asp:HyperLink>
                    </asp:TableCell>
                    <%--<!--<asp:TableCell >
                        <asp:Label runat="server" Text="Days Worked:"></asp:Label>
                        <asp:DropDownList ID="ddlDaysBack" runat="server" />
                    </asp:TableCell>--> --%>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="160px">
                        <input type='button' id='btnEligible' onclick='toggleAvailableEmployees()' value="View Eligible Employees Only"/><br />
                        <input type="button" id='btnActive' onclick='toggleDeactivatedEmployees()' value="Hide Deactivated/DNR'd Employees" /><br />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
     </asp:Panel>
  

     <asp:Repeater id="rptrDaysWorkedReport" runat="server" EnableViewState="false" OnItemDataBound="rptrDaysWorkedReport_ItemDataBound">
          <HeaderTemplate>
             <table width="1200px" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                <th style="display:table-header-group;">
                <tr id="trExcelTitle" runat="server">
                    <%-- original code for td element, added empty cell <td>
                    <td colspan="11" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Hours Report"/></td>
                    --%>
                    <td />
                    <td style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Days Worked Report"/></td>
                </tr>
                <tr id="trExcelDate" runat="server">
                    <%-- inserted empty td immediately below --%>
                    <td />
                    <td id="tdExcelDate" runat="server" colspan="2" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelDate" Runat="server" Text=""/></td>
                </tr>
                <tr class="trDWHead">
                    <th>Badge #</th>
                    <th>Employee Name</th>
                    <th>Start Date</th>
                    <th>Latest Punch</th>
                    <th>Department</th>
                    <th>Shift</th>
                    <th>Days Worked</th>
                    <th>Punch Count</th>
                    <th>End of Break</th>
                    <th>Status</th>
                </tr>
                </th>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr id="empRow" class="trDWItem" runat="server">
                   <td><asp:Label ID="lblBadgeNumber"  Runat="server"/><!--</asp:Panel>--><asp:Label ID="lblBadgeNumberExcel"  Runat="server"/></td>
                   <td><asp:Label ID="lblLastName"  Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Runat="server"/></td>
                   <td><asp:Label ID="lblStartDate" runat="server"/></td>
                   <td><asp:Label ID="lblEndDate" runat="server"/></td>
                   <td><asp:Label ID="lblDepartment" runat="server" /><asp:DropDownList runat="server" ID="ddlDepartment"/></td>
                   <td><asp:Label ID="lblShift" runat="server" /></td>
                   <td><asp:Label ID="lblDaysWorked" Runat="server"/></td>
                   <td><asp:Label ID="lblPunchCount" Runat="server"/></td>
                   <td><asp:Label ID="lblBreakEnd" Runat="server"/></td>
                   <td><asp:Label ID="lblDnrReason" runat="server" /><asp:HyperLink ID="lbtnDnrReason" NavigateURL="" runat="server"></asp:HyperLink></td>
            </tr>
          </ItemTemplate>
          
          <AlternatingItemTemplate>
             <tr id="empRow" class="trDWItem trDWItemAlt" runat="server">
                   <td><asp:Label ID="lblBadgeNumber"  Runat="server"/><!--</asp:Panel>--><asp:Label ID="lblBadgeNumberExcel"  Runat="server"/></td>
                   <td><asp:Label ID="lblLastName"  Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Runat="server"/></td>
                   <td><asp:Label ID="lblStartDate" runat="server"/></td>
                   <td><asp:Label ID="lblEndDate" runat="server"/></td>
                   <td><asp:Label ID="lblDepartment" runat="server" /><asp:DropDownList runat="server" ID="ddlDepartment"/></td>
                   <td><asp:Label ID="lblShift" runat="server" /></td>
                   <td><asp:Label ID="lblDaysWorked" Runat="server"/></td>
                   <td><asp:Label ID="lblPunchCount" Runat="server"/></td>
                   <td><asp:Label ID="lblBreakEnd" Runat="server"/></td>
                   <td><asp:Label ID="lblDnrReason" runat="server" /><asp:HyperLink ID="lbtnDnrReason" NavigateURL="" runat="server"></asp:HyperLink></td>
            </tr>
          </AlternatingItemTemplate>

          <FooterTemplate>
             </table>
          </FooterTemplate>
             
       </asp:Repeater>
    <script type="text/javascript">
        var ddl = document.getElementById("ddlDaysBack");
        ddl.setAttribute("onchange", function () {
            alert("Hello!");
        });
    </script>