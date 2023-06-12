<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetTicketTrackerException.ascx.cs" Inherits="MSI.Web.Controls.MSINetTicketTrackerException" %>
     
     <asp:Repeater id="rptrTicketTrackerException" OnItemDataBound="rptrTicketTrackerException_ItemDataBound" runat="server">
          <HeaderTemplate>
             <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc;">
                <tr>
                    <td id="tdBadgeNumberHeader" runat="server" class="ResultTableHeaderRowText">Badge #</td>
                    <td id="tdNameHeader" runat="server" class="ResultTableHeaderRowText">Employee Name</td>
                    <td class="ResultTableHeaderRowText">Swipe Date/Time</td>
                    <td class="ResultTableHeaderRowText">Exception Message</td>
                </tr>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr>
                   <td id="tdBadgeNumber" runat="server" class="ResultTableRowText"><asp:Label ID="lblItem" Text='' runat="server" ForeColor="#000000"></asp:Label><asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/></td>
                   <td id="tdName" runat="server" class="ResultTableRowText"><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>' Runat="server"/></td>
                   <td class="ResultTableRowText"><asp:Label ID="lblSwipeDateTime" Text='' Runat="server"/></td>
                   <td class="ResultTableRowText"><asp:Label ID="lblExceptionMessage" Text='' Runat="server"/></td>
            </tr>
             
          </ItemTemplate>
          <AlternatingItemTemplate>
             <tr>
                   <td id="tdBadgeNumber" runat="server" class="ResultTableAlternateRowText"><asp:Label ID="lblItem" Text='' runat="server" ForeColor="#000000"></asp:Label><asp:Label ID="lblBadgeNumber" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Runat="server"/></td>
                   <td id="tdName" runat="server" class="ResultTableAlternateRowText"><asp:Label ID="lblFullName" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>' Runat="server"/></td>
                   <td class="ResultTableAlternateRowText"><asp:Label ID="lblSwipeDateTime" Text='' Runat="server"/></td>
                   <td class="ResultTableAlternateRowText"><asp:Label ID="lblExceptionMessage" Text='' Runat="server"/></td>
            </tr>
          </AlternatingItemTemplate>

          <FooterTemplate>
            </table>
          </FooterTemplate>
             
       </asp:Repeater>
