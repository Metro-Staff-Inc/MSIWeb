<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetEmployeeHistory.ascx.cs" Inherits="MSI.Web.Controls.MSINetEmployeeHistory" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>

        <script type="text/javascript">
            function confirm_move()
            {
                if (confirm("Are you sure you want to move these swipes?")==true)
                {
                return true;
                }
                else
                {
                    return false;
                }
            }
            
            function confirm_delete()
            {
                if (confirm("Are you sure you want to delete this swipe?")==true)
                {
                return true;
                }
                else
                {
                    return false;
                }
            }
        </script>
        
<asp:Table runat="server" Width="240px" CellPadding="5" ID="tblEmployeeInput">
<asp:TableRow runat="server">
    <asp:TableCell runat="server">
        <asp:Panel ID="pnlMain" runat="server" HorizontalAlign="Left" Width="100%" Direction="LeftToRight">
            <asp:Panel ID="pnlLookup" runat="server" Width="240px" HorizontalAlign="Left">
                <asp:Label ID="Label1" runat="server" CssClass="InputTableLabelText" Text="Badge #:" Width="96px"></asp:Label><br />
                <asp:TextBox ID="txtTempNumber" runat="server" CssClass="InputTableInputLabelText"></asp:TextBox>
                <br />
                <br />
                <uc1:MSINetCalendar id="ctlWeekEnding" runat="server" DisplayMode="WeekEnding" LabelText="Week Ending Date:"></uc1:MSINetCalendar>
                <br />
                <asp:Button ID="btnLookup" runat="server" Text="Look Up" OnClick="btnLookup_Click" /><br />
                <br />
                    <asp:Label ID="lblNameHeading" runat="server" CssClass="InputTableLabelText" Text="Name:" Width="68px"></asp:Label>
                    <asp:Label ID="lblName" runat="server" CssClass="InputTableInputLabelText" Width="162px"></asp:Label>
                    <asp:Label ID="lblBadgeHeading" runat="server" CssClass="InputTableLabelText" Text="Badge #:" Width="66px"></asp:Label>
                    <asp:Label ID="lblBadge" runat="server" CssClass="InputTableInputLabelText" Width="162px"></asp:Label></asp:Panel>

            </asp:Panel>
    </asp:TableCell>
</asp:TableRow>
</asp:Table>

<asp:Table runat="server" Width="995px" CellPadding="5" ID="tblEmployeeResults">
<asp:TableRow ID="TableFooterRow1" runat="server">
    <asp:TableCell ID="TableCell1" runat="server">
         <asp:Repeater id="rptrEmployeeResults" runat="server" OnItemDataBound="rptrEmployeeResults_ItemDataBound"  OnItemCommand="rptrEmployeeResults_ItemCommand">
              <HeaderTemplate>
                 <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                    <tr>
                        <td class="ResultTableHeaderRowText">Check In</td>
                        <td class="ResultTableHeaderRowText">Check Out</td>
                        <td class="ResultTableHeaderRowText">Total Hours</td>
                        <td class="ResultTableHeaderRowText">Shift</td>
                        <td class="ResultTableHeaderRowText">Department</td>
                        <td id="tdActionHeader" visible="false" runat="server" class="ResultTableHeaderRowText">Action</td>
                    </tr>
              </HeaderTemplate>
                 
              <ItemTemplate>
                 <tr>
                       <td class="ResultTableRowText">
                            <asp:Label ID="lblCheckIn" Runat="server"/>
                            <asp:ImageButton ID="btnImageDeleteCheckIn" Visible="true" ImageUrl="~/Images/delete.gif" runat="server" CommandName="deletecheckin" CommandArgument='' />
                       </td>
                       <td class="ResultTableRowText">
                            <asp:Label ID="lblCheckOut" Runat="server"/>
                            <asp:ImageButton ID="btnImageDeleteCheckOut" Visible="true" ImageUrl="~/Images/delete.gif" runat="server" CommandName="deletecheckout" CommandArgument='' />
                       </td>
                       <td class="ResultTableRowText"><asp:Label ID="lblTotalHours" Runat="server"/></td>
                       <td class="ResultTableRowText"><asp:Label ID="lblShift" Runat="server"/></td>
                       <td class="ResultTableRowText"><asp:Label ID="lblDepartment" Runat="server"/>
                                                        <asp:DropDownList ID="cboDepartment"
                                                                    runat="server" Width="208px">
                                                                </asp:DropDownList>
                       <td id="tdAction" visible="false"  runat="server" class="ResultTableRowText">
                            <asp:ImageButton ID="btnImageMove" Visible="true" ImageUrl="~/Images/save.gif" runat="server" CommandName="move" CommandArgument='' />
                            <asp:LinkButton ID="lnkMove" runat="server" CssClass="TableActionLabelText" CommandName="move" CommandArgument='' Text="Move Swipes" ></asp:LinkButton>
                            <asp:HiddenField ID="hdnCheckInPunchID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CheckInEmployeePunchID")%>' />
                            <asp:HiddenField ID="hdnCheckOutPunchID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CheckOutEmployeePunchID")%>' />
                            <asp:HiddenField ID="hdnStartDate" runat="server" />
                            <asp:HiddenField ID="hdnTempNumber" runat="server" />
                       </td>

                </tr>                 
              </ItemTemplate>
              <AlternatingItemTemplate>
                 <tr>
                       <td class="ResultTableAlternateRowText">
                            <asp:Label ID="lblCheckIn" Runat="server"/>
                            <asp:ImageButton ID="btnImageDeleteCheckIn" Visible="true" ImageUrl="~/Images/delete.gif" runat="server" CommandName="deletecheckin" CommandArgument='' />
                       </td>
                       <td class="ResultTableAlternateRowText">
                            <asp:Label ID="lblCheckOut" Runat="server"/>
                            <asp:ImageButton ID="btnImageDeleteCheckOut" Visible="true" ImageUrl="~/Images/delete.gif" runat="server" CommandName="deletecheckout" CommandArgument='' />
                       </td>
                       <td class="ResultTableAlternateRowText"><asp:Label ID="lblTotalHours" Runat="server"/></td>
                       <td class="ResultTableAlternateRowText"><asp:Label ID="lblShift" Runat="server"/></td>
                       <td class="ResultTableAlternateRowText"><asp:Label ID="lblDepartment" Runat="server"/>
                                                                <asp:DropDownList ID="cboDepartment"
                                                                    runat="server" Width="208px">
                                                                </asp:DropDownList>
                       </td>
                       <td id="tdAction" visible="false"  runat="server" class="ResultTableAlternateRowText">
                            <asp:ImageButton ID="btnImageMove" Visible="true" ImageUrl="~/Images/save.gif" runat="server" CommandName="move" CommandArgument='' />
                            <asp:LinkButton ID="lnkMove" runat="server" CssClass="TableActionLabelText" CommandName="move" CommandArgument='' Text="Move Swipes" ></asp:LinkButton>
                            <asp:HiddenField ID="hdnCheckInPunchID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CheckInEmployeePunchID")%>' />
                            <asp:HiddenField ID="hdnCheckOutPunchID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "CheckOutEmployeePunchID")%>' />
                            <asp:HiddenField ID="hdnStartDate" runat="server" />
                            <asp:HiddenField ID="hdnTempNumber" runat="server" />
                       </td>
                </tr>
                
              </AlternatingItemTemplate>

              <FooterTemplate>
                    <tr>
                        <td class="ResultTableTotalsText">&nbsp;</td>
                        <td class="ResultTableTotalsText">&nbsp;</td>
                        <td class="ResultTableTotalsText"><asp:Label ID="lblPeriodTotal" Runat="server"/></td>
                        <td class="ResultTableTotalsText">&nbsp;</td>
                        <td class="ResultTableTotalsText">&nbsp;</td>
                        
                    </tr>
                 </table>
              </FooterTemplate>
                 
           </asp:Repeater>
       </asp:TableCell>
   </asp:TableRow>
</asp:Table>