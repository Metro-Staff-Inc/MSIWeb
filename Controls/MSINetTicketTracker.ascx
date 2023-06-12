<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetTicketTracker.ascx.cs" Inherits="MSI.Web.Controls.MSINetTicketTracker" %>
    <div visible="false">
        <input type="hidden" runat="server" id="userID"/>
        <input type="hidden" runat="server" id="webServiceLoc" />
    </div>

    <asp:Repeater id="rptrTicketTracker" OnItemDataBound="rptrTicketTracker_ItemDataBound" OnItemCommand="rptrTicketTracker_ItemCommand" runat="server">
          <HeaderTemplate>
             <table id="tt" width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc;">
                <tr class="ResultTableHeaderRowText">
                    <td id="tdIndex" runat="server"># Badge #</td>
                    <!--<td id="tdBadgeNumberHeader" runat="server" >Badge #</td>-->
                    <td id="tdNameHeader" runat="server">Employee Name</td>
                    <td id="tdScheduleHeader" runat="server">Schedule</td>
                    <td>Shift Check In</td>
                    <td id="tdLunchStartHeader" runat="server">Lunch Start</td>
                    <td id="tdLunchEndHeader" runat="server">Lunch End</td>
                    <td>Shift Check Out</td>
                    <td>Status</td>
                    <td id="tdActionHeader" runat="server">Action</td>
                    <td id="tdApproveHeader" runat="server"><asp:Button ID="btnApprove" Text="Approve" runat="server" CommandArgument='' CommandName="approve" /></td>
                </tr>
          </HeaderTemplate>
             
          <ItemTemplate>
             <tr id="trTrackingItem" runat="server" class="ResultTableRowText">
                   <td id="tdBadgeNumber" runat="server">
                       <a name="<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>"></a>
                       <asp:Label ID="lblItem" Text='' runat="server" ForeColor="#000000"></asp:Label>
                       <asp:LinkButton ID="lblBadgeNumber" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToHistory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' ></asp:LinkButton>
                       <asp:Label ID="lblBadgeNumberNoLink" CssClass="ResultTableRowText" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Visible="false" ></asp:Label>
                   </td>
                   <td id="tdName" runat="server">
                        <asp:LinkButton ID="lblName" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToHistory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Text="" ></asp:LinkButton>
                        <asp:Label ID="lblNameNoLink" CssClass="ResultTableRowText" runat="server" Text='' Visible="false" ></asp:Label>
                   </td>
                   <td id="tdSchedule" runat="server"><asp:Label ID="lblSchedule" Text='<%# FormatSchedule(DataBinder.Eval(Container.DataItem, "ShiftStartTime"), DataBinder.Eval(Container.DataItem, "ShiftEndTime")) %>' Runat="server"/></td>
                   <td id="tdCheckIn" runat="server">&nbsp;
                                <asp:HiddenField ID="hdnPunchIn" runat="server" Value='<%# GetPicNameCheckIn(Container.DataItem) %>'/>
                                <asp:Label ID="lblCheckIn" PunchId='<%# GetCheckInPunchId(Container.DataItem) %>' Exact='<%# GetExactCheckIn(Container.DataItem) %>' Round='<%# GetCheckIn(Container.DataItem) %>' ToolTip='<%# GetExactCheckIn(Container.DataItem) %>' Text='<%# GetCheckIn(Container.DataItem) %>' Runat="server"/>
                                <asp:Label ID="lblMoved" Text='' Runat="server"/>
                                <asp:LinkButton ID="lnkLinkToDepartment" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToDepartment" CommandArgument='' Text="Click here..." ></asp:LinkButton>
                   </td>
                   <td id="tdLunchStart" runat="server">&nbsp;<asp:Label ID="lblBreakIn" ToolTip='<%# GetExactBreakIn(Container.DataItem) %>' Text='<%# GetBreakIn(Container.DataItem) %>' Runat="server"/><asp:TextBox ID="txtBreakIn" Text='<%# GetBreakIn(Container.DataItem) %>' Visible="false" Runat="server"/></td>
                   <td id="tdLunchEnd" runat="server">&nbsp;<asp:Label ID="lblBreakOut" ToolTip='<%# GetExactBreakOut(Container.DataItem) %>' Text='<%# GetBreakOut(Container.DataItem) %>' Runat="server"/><asp:TextBox ID="txtBreakOut" Text='<%# GetBreakOut(Container.DataItem) %>' Visible="false" Runat="server"/></td>
                   <td id="tdCheckOut" >&nbsp;
                                <asp:HiddenField ID="hdnPunchOut" runat="server" Value='<%# GetPicNameCheckOut(Container.DataItem) %>'/>
                                <asp:Label ID="lblCheckOut" PunchId='<%# GetCheckOutPunchId(Container.DataItem) %>' Exact='<%# GetExactCheckOut(Container.DataItem) %>' Round='<%# GetCheckOut(Container.DataItem) %>' ToolTip='<%# GetExactCheckOut(Container.DataItem) %>' Text='<%# GetCheckOut(Container.DataItem) %>' Runat="server"/>
                                <asp:TextBox ID="txtCheckOut" Text='<%# GetCheckOut(Container.DataItem) %>' Visible="false" Runat="server"/>
                   </td>
                   <td id="tdStatus" runat="server" ><asp:Label ID="lblPunchStatus" Text='<%# TranslatePunchStatus(Container.DataItem) %>' Runat="server"/></td>
                   <td id="tdAction" runat="server" ><asp:ImageButton ID="btnImageEdit" Visible="true" ImageUrl="~/Images/edit.gif" runat="server" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClientRosterID")%>' /><asp:LinkButton ID="lnkChangeTimes" runat="server" CssClass="TableActionLabelText" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClientRosterID")%>' Text="Change Times" ></asp:LinkButton>
                   <asp:HiddenField ID="hdnClientID" runat="server" /><asp:HiddenField ID="hdnLocationID" runat="server" /><asp:HiddenField ID="hdnStartDate" runat="server" /><asp:HiddenField ID="hdnEndDate" runat="server" /><asp:HiddenField ID="hdnShiftType" runat="server" /><asp:HiddenField ID="hdnDepartmentID" runat="server" /><asp:HiddenField ID="hdnEmployeeName" runat="server" /><asp:HiddenField ID="hdnBadgeNumber" runat="server" />
                   </td>
                   <td id="tdApprove" runat="server" >&nbsp;<asp:Button ID="btnUnlock" Text="Unlock" runat="server" CommandArgument='' CommandName="unlock" Visible="false" /><asp:Label runat="server" ID="lblApproveStatus" Text="" Visible="false" ForeColor="#000000"></asp:Label><asp:CheckBox runat="server" ID="chkApprove" Text="Approve" ForeColor="#000000" /><asp:HiddenField ID="hdnEmployeePunchList" runat="server" /><asp:HiddenField ID="hdnShiftDate" runat="server" /></td>
            </tr>
            <asp:Repeater id="rptrPunchDetail" OnItemDataBound="rptrPunchDetail_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <tr>
                        <td colspan="10" style="border-bottom:solid 1px #cccccc;">
                            <table width="100%" cellpadding="0" cellspacing="0" style="border:none;">
                                <tr>
                                    <td style="width:200px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#ffffff; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">&nbsp;</td>
                                    <td style="width:200px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#ffffff; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">Check In</td>
                                    <td style="width:150px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#ffffff; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">Check Out</td>
                                    <td style="border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#ffffff; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">&nbsp;</td>
                                </tr>
                </HeaderTemplate>
                <ItemTemplate>
                     <tr>
                           <td style="border-bottom:solid 0px #cccccc; width:200px; background-color:#ffffff; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;">&nbsp;</td>
                           <td style="border-bottom:solid 1px #cccccc; width:200px; background-color:#ffffff; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;"><asp:Label ID="lblCheckIn" Runat="server"/><asp:Label ID="lblCheckInDetailDeptId" Runat="server" Visible="false"/></td>
                           <td style="border-bottom:solid 1px #cccccc; width:150px; background-color:#ffffff; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;"><asp:Label ID="lblCheckOut" Runat="server"/><asp:Label ID="lblCheckOutDetailDeptId" Runat="server" Visible="false"/></td>
                           <td style="border-bottom:solid 0px #cccccc; background-color:#ffffff; color:Black; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;">&nbsp;</td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                            </table>
                        </td>
                    </tr>
                </FooterTemplate> 
            </asp:Repeater>
             
          </ItemTemplate>
          <AlternatingItemTemplate>
             <tr id="trTrackingItem" class="ResultTableAlternateRowText" runat="server">
                   <td id="tdBadgeNumber" runat="server" >
                           <a name="<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>"></a>
                           <asp:Label ID="lblItem" Text='' runat="server" ForeColor="#000000"></asp:Label>
                           <asp:LinkButton ID="lblBadgeNumber" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToHistory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' ></asp:LinkButton>
                           <asp:Label ID="lblBadgeNumberNoLink" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "TempNumber") %>' ></asp:Label>
                   </td>
                   <td id="tdName" runat="server" >
                            <asp:LinkButton ID="lblName" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToHistory" CommandArgument='' Text="" ></asp:LinkButton>
                            <asp:Label ID="lblNameNoLink" runat="server" Visible="false" Text='' ></asp:Label>
                   </td>
                   <td id="tdSchedule" runat="server" ><asp:Label ID="lblSchedule" Text='<%# FormatSchedule(DataBinder.Eval(Container.DataItem, "ShiftStartTime"), DataBinder.Eval(Container.DataItem, "ShiftEndTime")) %>' Runat="server"/></td>
                   <td id="tdCheckIn" runat="server" >&nbsp;
                            <asp:HiddenField ID="hdnPunchInAlt" runat="server" Value='<%# GetPicNameCheckIn(Container.DataItem) %>'/>
                            <asp:Label ID="lblCheckIn" PunchId='<%# GetCheckInPunchId(Container.DataItem) %>' Exact='<%# GetExactCheckIn(Container.DataItem) %>' Round='<%# GetCheckIn(Container.DataItem) %>' ToolTip='<%# GetExactCheckIn(Container.DataItem) %>' Text='<%# GetCheckIn(Container.DataItem) %>' Runat="server"/>
                            <asp:Label ID="lblMoved" Text='' Runat="server"/>
                            <asp:LinkButton ID="lnkLinkToDepartment" CssClass="ResultTableLinkLabelText" runat="server" CommandName="linkToDepartment" CommandArgument='' Text="Click here..." ></asp:LinkButton>
                   </td>
                   <td id="tdLunchStart" runat="server" >&nbsp;<asp:Label ID="lblBreakIn" ToolTip='<%# GetExactBreakIn(Container.DataItem) %>' Text='<%# GetBreakIn(Container.DataItem) %>' Runat="server"/><asp:TextBox ID="txtBreakIn" Text='<%# GetBreakIn(Container.DataItem) %>' Visible="false" Runat="server"/></td>
                   <td id="tdLunchEnd" runat="server" >&nbsp;<asp:Label ID="lblBreakOut" ToolTip='<%# GetExactBreakOut(Container.DataItem) %>' Text='<%# GetBreakOut(Container.DataItem) %>' Runat="server"/><asp:TextBox ID="txtBreakOut" Text='<%# GetBreakOut(Container.DataItem) %>' Visible="false" Runat="server"/></td>
                   <td id="tdCheckOut" >&nbsp;
                            <asp:HiddenField ID="hdnPunchOutAlt" runat="server" Value='<%# GetPicNameCheckOut(Container.DataItem) %>'/>
                            <asp:Label ID="lblCheckOut" PunchId='<%# GetCheckOutPunchId(Container.DataItem) %>' Exact='<%# GetExactCheckOut(Container.DataItem) %>' Round='<%# GetCheckOut(Container.DataItem) %>' ToolTip='<%# GetExactCheckOut(Container.DataItem) %>' Text='<%# GetCheckOut(Container.DataItem) %>' Runat="server"/>
                            <asp:TextBox ID="txtCheckOut" Text='<%# GetCheckOut(Container.DataItem) %>' Visible="false" Runat="server"/></td>
                   <td id="tdStatus" runat="server" ><asp:Label ID="lblPunchStatus" Text='<%# TranslatePunchStatus(Container.DataItem) %>' Runat="server"/></td>
                   <td id="tdAction" runat="server" ><asp:ImageButton ID="btnImageEdit" Visible="true" ImageUrl="~/Images/edit.gif" runat="server" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClientRosterID")%>' /><asp:LinkButton ID="lnkChangeTimes" runat="server" CssClass="TableActionLabelText" CommandName="edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ClientRosterID")%>' Text="Change Times" ></asp:LinkButton>
                     <asp:HiddenField ID="hdnClientID" runat="server" /><asp:HiddenField ID="hdnStartDate" runat="server" /><asp:HiddenField ID="hdnEndDate" runat="server" /><asp:HiddenField ID="hdnShiftType" runat="server" /><asp:HiddenField ID="hdnDepartmentID" runat="server" /><asp:HiddenField ID="hdnEmployeeName" runat="server" /><asp:HiddenField ID="hdnLocationID" runat="server" /><asp:HiddenField ID="hdnBadgeNumber" runat="server" />
                   </td>
                   <td id="tdApprove" runat="server" >&nbsp;<asp:Button ID="btnUnlock" Text="Unlock" runat="server" CommandArgument='' CommandName="unlock" Visible="false" /><asp:Label runat="server" ID="lblApproveStatus" Text="" Visible="false" ForeColor="#000000"></asp:Label><asp:CheckBox runat="server" ID="chkApprove" Text="Approve" ForeColor="#000000" /><asp:HiddenField ID="hdnEmployeePunchList" runat="server" /><asp:HiddenField ID="hdnShiftDate" runat="server" /></td>
            </tr>
            <asp:Repeater id="rptrPunchDetail" OnItemDataBound="rptrPunchDetail_ItemDataBound" runat="server">
                <HeaderTemplate>
                    <tr>
                        <td colspan="10" style="border-bottom:solid 1px #cccccc;">
                            <table width="100%" cellpadding="0" cellspacing="0" style="border:none;">
                                <tr>
                                    <td style="width:200px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#F2F2F2; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">&nbsp;</td>
                                    <td style="width:200px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#F2F2F2; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">Check In</td>
                                    <td style="width:150px; border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#F2F2F2; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">Check Out</td>
                                    <td style="border-bottom:solid 0px #cccccc; padding:2pt 2pt 2pt 2pt; height:20px; color:#666666; background-color:#F2F2F2; text-align:left; font-family:Arial; font-size:8pt; font-weight:bold;">&nbsp;</td>
                                </tr>
                </HeaderTemplate>
                 
                <ItemTemplate>
                     <tr>
                           <td style="border-bottom:solid 0px #cccccc; width:200px; background-color:#F2F2F2; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;">&nbsp;</td>
                           <td style="border-bottom:solid 1px #cccccc; width:200px; background-color:#F2F2F2; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;"><asp:Label ID="lblCheckIn" Runat="server"/><asp:Label ID="lblCheckInDetailDeptId" Runat="server" Visible="false"/></td>
                           <td style="border-bottom:solid 1px #cccccc; width:150px; background-color:#F2F2F2; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;"><asp:Label ID="lblCheckOut" Runat="server"/><asp:Label ID="lblCheckOutDetailDeptId" Runat="server" Visible="false"/></td>
                           <td style="border-bottom:solid 0px #cccccc; background-color:#F2F2F2; color:#666666; text-align:left; font-family:Arial; font-size:8pt; font-weight:normal;">&nbsp;</td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                            </table>
                        </td>
                    </tr>
                </FooterTemplate> 
            </asp:Repeater>
          </AlternatingItemTemplate>

          <FooterTemplate>
            </table>
          </FooterTemplate>
       </asp:Repeater>
    <div runat="server" style="visibility:hidden" id="empPopupWrapper">
        <a id="close-btn" href="#">Close</a>

        <table border="1" cellpadding="0" cellspacing="0">
            <tr>
                <td>Jonathan Murfey</td>
                <td>Shift Start:</td>
                <td>09:00 AM</td>
            </tr>
            <tr>
                <td>1st Shift Warehouse</td>
                <td>Shift End:</td>
                <td>05:00 PM</td>
            </tr>
        </table>

        <table>
            <thead>
                <tr>
                    <td>Type</td>
                    <td>Recorded Date</td>
                    <td>Reorded Time</td>
                    <td>Del</td>
                    <td>Upd</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Check-In Exact</td>
                    <td>09/07/2012</td>
                    <td>07:56 AM</td>
                    <td rowspan="2">
                        <input type="button" value="DEL" />
                    </td>
                    <td rowspan="2">
                        <input type="button" value="UPD" />
                    </td>
                </tr>
                <tr>
                    <td>Check-In Round</td>
                    <td><input id="punchDate" type="text" value="09/07/2012" /></td>
                    <td><input id="punchHours" type="text" value="08" />:<input id="punchMins" type="text" value="02" />:<input id="punchAMPM" type="text" value="AM" /></td>
                </tr>
            </tbody>

        </table>

    </div>

        <link href="../Scripts/jQuery-modalPopLite/modalPopLite.css" rel="stylesheet" type="text/css" />
        <script src="../Scripts/jQuery-modalPopLite/modalPopLite.min.js" type="text/javascript"></script>
        <script id="jsTicketTracker" src="../Scripts/TicketTracker.js" type="text/javascript"></script>

