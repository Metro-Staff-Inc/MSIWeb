<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetPunchMaintenance.ascx.cs" Inherits="MSI.Web.Controls.MSINetPunchMaintenance" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>
        <script type="text/javascript">
            function confirm_delete()
            {
                if (confirm("Are you sure you want to delete this swipe record?")==true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        </script>

        <table>
            <tr>
                <td style="width:120px;">
                    <asp:Label ID="lblBadgeNumberTitle" runat="server" Text="Badge Number:" CssClass="MSINetBodyText"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblBadgeNumber" runat="server" Text="" CssClass="MSINetBodyText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:120px;">
                    <asp:Label ID="lblNameTitle" runat="server" Text="Employee Name:" CssClass="MSINetBodyText"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="" CssClass="MSINetBodyText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:120px;">
                    <asp:Label ID="lblDateTitle" runat="server" Text="Shift Date:" CssClass="MSINetBodyText"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDate" runat="server" Text="" CssClass="MSINetBodyText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:120px;">
                    <asp:Label ID="lblStatusTitle" runat="server" Text="Current Status:" CssClass="MSINetBodyText"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="MSINetBodyText"></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        <asp:Panel id="pnlConfirmation" Width="100%" CssClass="MSINetValidationText" runat="server">
            <asp:Label ID="lblConfirmationMessage" runat="server" CssClass="MSINetCheckInConfirmationText" Visible="False"></asp:Label>
            <asp:Label ID="lblValidationMessage" runat="server" CssClass="MSINetValidationText" Text="Label" Visible="False"></asp:Label>
        </asp:Panel>
        <br />    
        <asp:Repeater id="rptrEmployeePunchResults" runat="server" OnItemDataBound="rptrEmployeePunchResults_ItemDataBound" OnItemCommand="rptrEmployeePunchResults_ItemCommand">
              <HeaderTemplate>
                 <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                    <tr>
                        <td class="ResultTableHeaderRowText">Type</td>
                        <td class="ResultTableHeaderRowText">Recorded Date</td>
                        <td  class="ResultTableHeaderRowText">Recorded Time</td>
                        <!--
                        <td style="padding:2pt 2pt 2pt 2pt; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Reason for Action</td>
                        -->
                        <td id="tdActionHeader" visible="true" runat="server" class="ResultTableHeaderRowText">Action</td>
                    </tr>
              </HeaderTemplate>
                 
              <ItemTemplate>
                 <tr>
                       <td class="ResultTableRowText"><asp:Label ID="lblType" Runat="server"/></td>
                       <td class="ResultTableRowText"><asp:Label ID="lblDate" Runat="server"/></td>
                       <td class="ResultTableRowText">
                            <asp:DropDownList ID="cboHour" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblColonChar" Text=":" runat="server"></asp:Label>
                            <asp:DropDownList ID="cboMinute" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblSpaceChar" runat="server">&nbsp;</asp:Label>
                            <asp:DropDownList ID="cboAMPM" runat="server" AutoPostBack="false" >
                            </asp:DropDownList>
                       </td>
                       <!--
                       <td style="border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                            <asp:DropDownList ID="cboMaintenanceReason" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                       </td>
                       -->
                       <td id="tdAction" visible="true"  runat="server" class="ResultTableRowText"><asp:Button ID="btnSave" Text="Update Time" runat="server" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmployeePunchID")%>' /><asp:Button ID="btnDelete" Text="Delete Swipe Record" runat="server" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmployeePunchID")%>' /></td>
                </tr>                 
              </ItemTemplate>
              <AlternatingItemTemplate>
                 <tr>
                       <td class="ResultTableAlternateRowText"><asp:Label ID="lblType" Runat="server"/></td>
                       <td class="ResultTableAlternateRowText"><asp:Label ID="lblDate" Runat="server"/></td>
                       <td class="ResultTableAlternateRowText">
                            <asp:DropDownList ID="cboHour" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblColonChar" Text=":" runat="server"></asp:Label>
                            <asp:DropDownList ID="cboMinute" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblSpaceChar" runat="server">&nbsp;</asp:Label>
                            <asp:DropDownList ID="cboAMPM" runat="server" AutoPostBack="false" >
                            </asp:DropDownList>
                       </td>
                       <!--
                       <td style="background-color:#E3EAEB; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:normal;">
                            <asp:DropDownList ID="cboMaintenanceReason" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                       </td>
                       -->
                       <td id="tdAction" visible="true"  runat="server" class="ResultTableAlternateRowText"><asp:Button ID="btnSave" Text="Update Time" runat="server" CommandName="save" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmployeePunchID")%>' /><asp:Button ID="btnDelete" Text="Delete Swipe Record" runat="server" CommandName="delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmployeePunchID")%>' /></td>
                </tr>
                
              </AlternatingItemTemplate>
              
              <FooterTemplate>
                    <tr>
                        <td class="ResultTableAddNewRecordText">Record New</td>
                        <td class="ResultTableAddNewRecordText">
                            <asp:DropDownList ID="cboDate" runat="server" AutoPostBack="false" >
                            </asp:DropDownList>
                        </td>
                        <td class="ResultTableAddNewRecordText">
                            <asp:DropDownList ID="cboHour" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblColonChar" Text=":" runat="server"></asp:Label>
                            <asp:DropDownList ID="cboMinute" runat="server" AutoPostBack="false" >
                            </asp:DropDownList><asp:Label ID="lblSpaceChar" runat="server">&nbsp;</asp:Label>
                            <asp:DropDownList ID="cboAMPM" runat="server" AutoPostBack="false" >
                            </asp:DropDownList>
                        </td>
                        <!--
                        <td style="background-color:#FFFF80; border-bottom:solid 0pt #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                            <asp:DropDownList ID="cboMaintenanceReason" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                       </td>
                       -->
                        <td class="ResultTableAddNewRecordText"><asp:Button ID="btnSave" Text="Record a New Swipe" runat="server" CommandName="add" CommandArgument='0' /></td>
                        
                    </tr>
                 </table>
                 
              </FooterTemplate>
                          
           </asp:Repeater>
