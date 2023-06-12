<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetHeadCountReport.ascx.cs" Inherits="MSI.Web.Controls.MSINetHeadCountReport" EnableViewState="false" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>

        <asp:Panel ID="pnlHeader" runat="server" BackColor="#E0E0E0" Width="905px" EnableViewState="false">
            <asp:Table ID="tblPeriod" runat="server" Width="100%">

                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px"><uc1:MSINetCalendar id="ctlWeekEnding" runat="server" DisplayMode="WeekEnding" LabelText="Week Ending:"></uc1:MSINetCalendar></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top"  ToolTip="All employees who have worked or are currently at work" HorizontalAlign="left" Width="100px"><asp:Image ID="Image2" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" /><asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="View Report"></asp:Button><br /></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" ToolTip="Shows only employees currently on the premises" HorizontalAlign="left" Width="80px"><asp:Image ID="Image3" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" /><asp:Button id="btnGoCurrent" onclick="btnGo_Click" runat="server" Text="On Site"></asp:Button><br /></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="160px">
                        <asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:HyperLink ID="lnkExport" Target="_blank" NavigateURL="~/auth/HeadCountReportExcel.aspx?date=" runat="server" Text="Export Summary to Excel"></asp:HyperLink>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                        <asp:Panel ID="pnlTotals" runat="server"  HorizontalAlign="Center" Width="260px">
                            <asp:Label ID="Label2"  Visible="false" runat="server" Text="Total Regular Hours:" Width="130px" CssClass="MSINetBodyText_Right"></asp:Label>
                            <asp:Label ID="lblRegularHrs" Visible="false"  runat="server" CssClass="MSINetBodyText_Right" Text="0.00" Width="80px"></asp:Label><br />
                            <asp:Label ID="lblTTest" Visible="false"  runat="server" Text="Total Overtime Hours:" Width="130px" CssClass="MSINetBodyText_Right"></asp:Label>
                            <asp:Label ID="lblOTHrs" Visible="false"  runat="server" CssClass="MSINetBodyText_Right" Text="0.00" Width="80px"></asp:Label>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
     </asp:Panel>
    
     <asp:Repeater id="rptrHeadCountReport" runat="server" OnItemDataBound="rptrHeadCountReport_ItemDataBound" EnableViewState="false">
          <HeaderTemplate>
             <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                <th style="display:table-header-group;">
                <tr id="trExcelTitle" runat="server">
                    <%-- original code for td element, added empty cell <td>
                    <td colspan="11" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Hours Report"/></td>
                    --%>
                    <td />
                    <td style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelTitle" Runat="server" Text="MSI WebTrax Hours Reporterer"/></td>
                </tr>
                <tr id="trExcelWeekEnding" runat="server">
                    <%-- inserted empty td immediately below --%>
                    <td />
                    <td id="tdExcelWeekEnding" runat="server" colspan="3" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekEnding" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay1" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay2" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay3" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay4" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay5" Runat="server" Text=""/></td>
                    <td style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay6" Runat="server" Text=""/></td>
                    <td colspan="3" style="mso-number-format:'m\/d'; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblExcelWeekDay7" Runat="server" Text=""/></td>
                </tr>
                <tr>
                    <td style="padding:2pt 2pt 2pt 2pt; width:40px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Seq #</td>
                    <td style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Badge #</td>
                    <td style="padding:2pt 2pt 2pt 2pt; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Employee Name</td>
                    <asp:Panel ID="pnlPrimaryJobHead" runat="server" Visible="false">
                        <td style="padding:2pt 24pt 2pt 2pt; width:96px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Primary Job</td>
                    </asp:Panel>
                    <asp:Panel ID="pnlJobCodeHead" runat="server" Visible="false">
                        <td style="padding:2pt 2pt 2pt 2pt; width:80px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Job Code</td>
                    </asp:Panel>
                    <td id="tdMonHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay1" runat="server" /></td>
                    <td id="tdTueHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay2" runat="server" /></td>
                    <td id="tdWedHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay3" runat="server" /></td>
                    <td id="tdThuHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay4" runat="server" /></td>
                    <td id="tdFriHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay5" runat="server" /></td>
                    <td id="tdSatHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay6" runat="server" /></td>
                    <td id="tdSunHeader" runat="server" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label id="lblWeekDay7" runat="server" /></td>
                </tr>
                </th>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr id="trDepartment" runat="server">
                   <%-- empty td element added and colspan reduced 12 -> 11 --%>
                   <td />
                   <td id="tdDepartmentHead" runat="server" colspan="11" style="border-bottom:solid 0pt #cccccc; padding:12pt 12pt 12pt 12pt; color:Black; text-align:left; font-family:Arial; font-size:12pt; font-weight:bold;"><asp:Label ID="lblDepartment" Runat="server"/></td>
             </tr>
             <tr>
                   <asp:Panel ID="pnlEmpCnt" runat="server" Visible="true">
                     <td style="width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                     <asp:Label ID="lblEmpCnt" Runat="server"></asp:Label>
                   </asp:Panel>
                   <td style="width:50px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/></td>
                   <td style="width:250px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblLastName" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>' Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' Runat="server"/>&nbsp;<asp:Label ID="lblUnApproved" Text='**' Visible="false" Runat="server"/></td>
                   <asp:Panel ID="pnlPrimaryJob" runat="server" Visible="false">
                        <td style="width:96px; border-bottom:solid 0pt #cccccc; padding:2pt 24pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblPrimaryJob" Runat="server"/></td>
                    </asp:Panel>
                   <asp:Panel ID="pnlJobCode" runat="server" Visible="false">
                        <td style="width:80px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblJobCode" Runat="server"/></td>
                    </asp:Panel>
                   <td id="tdMon" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay1Hours" Runat="server"/></td>
                   <td id="tdTue" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay2Hours" Runat="server"/></td>
                   <td id="tdWed" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay3Hours" Runat="server"/></td>
                   <td id="tdThu" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay4Hours" Runat="server"/></td>
                   <td id="tdFri" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay5Hours" Runat="server"/></td>
                   <td id="tdSat" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay6Hours" Runat="server"/></td>
                   <td id="tdSun" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay7Hours" Runat="server"/></td>
            </tr>

            <%-- white department totals line --%>
            <tr id="trDepartmentTotals" runat="server">
                   <td />
                   <td id="tdDepartmentTotalLabel" runat="server" colspan="2" style="border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:rigth; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDepartmentTotalLabel" Runat="server"/></td>
                   <td id="tdDeptSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                   <td id="tdDeptMonTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay1Hours" Runat="server"/></td>
                   <td id="tdDeptTueTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay2Hours" Runat="server"/></td>
                   <td id="tdDeptWedTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay3Hours" Runat="server"/></td>
                   <td id="tdDeptThuTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay4Hours" Runat="server"/></td>
                   <td id="tdDeptFriTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay5Hours" Runat="server"/></td>
                   <td id="tdDeptSatTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay6Hours" Runat="server"/></td>
                   <td id="tdDeptSunTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay7Hours" Runat="server"/></td>

             </tr>
             <%-- white shift totals --%>
            <tr id="trShiftTotals" runat="server" visible="false">
                   <td />
                   <td colspan="2" style="border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:rigth; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblShiftTotalLabel" Runat="server"/></td>
                   <td id="tdShiftSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                   <td id="tdMonTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotMonHours" Runat="server"/></td>
                   <td id="tdTueTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotTueHours" Runat="server"/></td>
                   <td id="tdWedTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotWedHours" Runat="server"/></td>
                   <td id="tdThuTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotThuHours" Runat="server"/></td>
                   <td id="tdFriTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotFriHours" Runat="server"/></td>
                   <td id="tdSatTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSatHours" Runat="server"/></td>
                   <td id="tdSunTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSunHours" Runat="server"/></td>

             </tr>
          </ItemTemplate>

          <AlternatingItemTemplate>
             <tr id="trDepartment" runat="server">
                   <%-- empty td element added and colspan reduced 12 -> 11 --%>
                   <td />
                   <td id="tdDepartmentHead" runat="server" colspan="11" style="background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:12pt 12pt 12pt 12pt; color:Black; text-align:left; font-family:Arial; font-size:12pt; font-weight:bold;"><asp:Label ID="lblDepartment" Runat="server"/></td>
             </tr>
             <tr>
                   <asp:Panel ID="pnlEmpCnt" runat="server" Visible="true">
                     <td style="width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                     <asp:Label ID="lblEmpCnt" Runat="server"></asp:Label>
                   </asp:Panel>
                   <td style="width:50px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/></td>
                   <td style="width:250px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblLastName" Text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>' Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>' Runat="server"/>&nbsp;<asp:Label ID="lblUnApproved" Text='**' Visible="false" Runat="server"/></td>
                   <asp:Panel ID="pnlPrimaryJob" runat="server" Visible="false">
                        <td style="width:96px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 24pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblPrimaryJob" Runat="server"/></td>
                    </asp:Panel>
                   <asp:Panel ID="pnlJobCode" runat="server" Visible="false">
                        <td style="width:80px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblJobCode" Runat="server"/></td>
                    </asp:Panel>
                    <asp:Panel ID="pnlPayRate" runat="server" Visible="false">
                        <td style="width:50px; background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblPayRate" Runat="server"/></td>
                    </asp:Panel>
                   <td id="tdMon" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay1Hours" Runat="server"/></td>
                   <td id="tdTue" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay2Hours" Runat="server"/></td>
                   <td id="tdWed" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay3Hours" Runat="server"/></td>
                   <td id="tdThu" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay4Hours" Runat="server"/></td>
                   <td id="tdFri" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay5Hours" Runat="server"/></td>
                   <td id="tdSat" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay6Hours" Runat="server"/></td>
                   <td id="tdSun" runat="server" style="mso-number-format:\#\,\#\#0\;width:40px; background-color:#E3EAEB; border-bottom:solid 0pt #E3EAEB; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;"><asp:Label ID="lblWeekDay7Hours" Runat="server"/></td>
            </tr>
  
            <%-- grey department totals --%>
             <tr id="trDepartmentTotals" runat="server">
                   <td />
                   <td id="tdDepartmentTotalLabel" runat="server" colspan="2" style="background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:rigth; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDepartmentTotalLabel" Runat="server"/></td>
                   <td id="tdDeptSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;background-color:#E3EAEB;"></td>
                   <td id="tdDeptMonTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay1Hours" Runat="server"/></td>
                   <td id="tdDeptTueTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay2Hours" Runat="server"/></td>
                   <td id="tdDeptWedTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay3Hours" Runat="server"/></td>
                   <td id="tdDeptThuTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay4Hours" Runat="server"/></td>
                   <td id="tdDeptFriTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay5Hours" Runat="server"/></td>
                   <td id="tdDeptSatTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay6Hours" Runat="server"/></td>
                   <td id="tdDeptSunTotal" runat="server" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblDeptTotWeekDay7Hours" Runat="server"/></td>
             </tr>
             <%-- grey shift totals --%>
            <tr id="trShiftTotals" runat="server"  visible="false">
                   <td />
                   <td colspan="2" style="background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:rigth; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblShiftTotalLabel" Runat="server"/></td>
                   <td id="tdShiftSpacer" runat="server" visible="false" style="width:80px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"></td>
                   <td id="tdMonTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotMonHour22222s" Runat="server"/></td>
                   <td id="tdTueTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotTueHours" Runat="server"/></td>
                   <td id="tdWedTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotWedHours" Runat="server"/></td>
                   <td id="tdThuTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotThuHours" Runat="server"/></td>
                   <td id="tdFriTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotFriHours" Runat="server"/></td>
                   <td id="tdSatTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSatHours" Runat="server"/></td>
                   <td id="tdSunTotal" style="mso-number-format:\#\,\#\#0\; width:40px; background-color:#E3EAEB; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblTotSunHours" Runat="server"/></td>
             </tr>
          </AlternatingItemTemplate>

          <FooterTemplate>
                <tr visible="false" id="productionTotals" runat="server">
                   <td id="td1" runat="server" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right;"></td>
                   <td id="td2" runat="server" colspan="2" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Production Totals:</td>
                   <td id="td3" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="td4" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay1Hours" Runat="server"/></td>
                   <td id="td5" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay2Hours" Runat="server"/></td>
                   <td id="td6" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay3Hours" Runat="server"/></td>
                   <td id="td7" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay4Hours" Runat="server"/></td>
                   <td id="td8" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay5Hours" Runat="server"/></td>
                   <td id="td9" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay6Hours" Runat="server"/></td>
                   <td id="td10" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblProductionWeekDay7Hours" Runat="server"/></td>
                </tr>
                <tr visible="false" id="packagingTotals" runat="server">
                   <td id="td11" runat="server" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right;"></td>
                   <td id="td12" runat="server" colspan="2" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Packaging Totals:</td>
                   <td id="td13" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="td14" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay1Hours" Runat="server"/></td>
                   <td id="td15" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay2Hours" Runat="server"/></td>
                   <td id="td16" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay3Hours" Runat="server"/></td>
                   <td id="td17" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay4Hours" Runat="server"/></td>
                   <td id="td18" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay5Hours" Runat="server"/></td>
                   <td id="td19" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay6Hours" Runat="server"/></td>
                   <td id="td20" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblPackagingWeekDay7Hours" Runat="server"/></td>
                </tr>
                <tr visible="false" id="indirectTotals" runat="server">
                   <td id="td21" runat="server" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right;"></td>
                   <td id="td22" runat="server" colspan="2" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Indirect Totals:</td>
                   <td id="td23" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="td24" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay1Hours" Runat="server"/></td>
                   <td id="td25" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay2Hours" Runat="server"/></td>
                   <td id="td26" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay3Hours" Runat="server"/></td>
                   <td id="td27" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay4Hours" Runat="server"/></td>
                   <td id="td28" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay5Hours" Runat="server"/></td>
                   <td id="td29" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay6Hours" Runat="server"/></td>
                   <td id="td30" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblIndirectWeekDay7Hours" Runat="server"/></td>
                </tr>
                <tr id="trGrandTotals" runat="server">
                   <td id="tdGrandTotalsSpc" runat="server" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right;"></td>
                   <td id="tdGrandTotalLabel" runat="server" colspan="2" style="height:17pt; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:center; font-family:Arial; font-size:10pt; font-weight:bold;">Grand Totals:</td>
                   <td id="tdGTSpacer" runat="server" visible="false" style="width:80px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">---</td>
                   <td id="tdMonGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay1Hours" Runat="server"/></td>
                   <td id="tdTueGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay2Hours" Runat="server"/></td>
                   <td id="tdWedGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay3Hours" Runat="server"/></td>
                   <td id="tdThuGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay4Hours" Runat="server"/></td>
                   <td id="tdFriGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay5Hours" Runat="server"/></td>
                   <td id="tdSatGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay6Hours" Runat="server"/></td>
                   <td id="tdSunGrandTotal" runat="server" style="mso-number-format:\#\,\#\#0\.00; width:40px; border-top:solid 1pt #000000; border-bottom:double 1pt #000000; padding:2pt 0pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;"><asp:Label ID="lblGrandWeekDay7Hours" Runat="server"/></td>
                </tr>
          </FooterTemplate>
       </asp:Repeater>
