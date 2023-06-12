<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRateMaintenance.aspx.cs" Inherits="MSI.Web.MSINet.PayRateMaintenance" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
</head>
<body>  <div align="center">
    <form id="form1" runat="server">
    <asp:Panel ID="pnlMSIControls" runat="server">
        <div>
            <uc1:MastHead ID="ctlMastHead" runat="server" />
        
        </div>
        <br />
            <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
            <br />
            <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="InvoiceProcessing" />
            <asp:Label ID="lblTitle" runat="server" CssClass="MSINetSectionHeading" Text="Pay Rate Maintenance" Width="165px"></asp:Label>
            &nbsp;&nbsp;&nbsp;<br />
            <br />
     </asp:Panel>
     &nbsp;&nbsp;<asp:LinkButton style="font-family:Arial; font-size:10pt; font-weight:normal;" ID="lnkGoBack" Text="Return to the Invoice" runat="server" OnClick="lnkGoBack_Click" ></asp:LinkButton>
     <br /><br />
    <div>
        <asp:Panel ID="pnlHeader" runat="server" BackColor="#E0E0E0" Width="990px">
            <asp:Table ID="tblPeriod" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblShift" runat="server"
                            CssClass="InputTableLabelText" Text="Shift:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboShift" runat="server" CssClass="InputTableInputLabelText" AutoPostBack="True" OnSelectedIndexChanged="cboShift_SelectedIndexChanged" Width="214px">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblDepartment" runat="server" CssClass="InputTableLabelText"
                            Text="Department:">
                        </asp:Label><br />
                        <asp:DropDownList ID="cboDepartment" 
                            runat="server" CssClass="InputTableInputLabelText" Width="208px">
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px" CssClass="InputTableHeader" >
                        <uc1:MSINetCalendar id="ctlWeekEnding" runat="server" DisplayMode="WeekEnding" LabelText="Week Ending:"></uc1:MSINetCalendar>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" CssClass="InputTableHeader" >
                        <asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="View Pay Rates"></asp:Button>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdnWeekEnd" runat="server" />
            <asp:HiddenField ID="hdnShowReturn" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlPayRateHeader" style="padding:10px;" runat="server" Visible="true" Width="970px" >
            <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                <tr >
                    <td style="width:150px;text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblPayRateHead" Runat="server" Text="Department Pay Rate:"/>
                    </td>
                    <td style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        $<asp:TextBox ID="txtDepartmentPayRate" Width="100px" runat="server"></asp:TextBox>
                        &nbsp;&nbsp;
                        <asp:Button ID="btnSaveDepartmentPayRate" Text="Update Pay Rate" runat="server" CommandName="departmentpayrate" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlPayRateOverrides" runat="server" Visible="true" Width="990px" style="border:solid 1px #cccccc" >
                 <asp:Repeater id="rptrOverrides" runat="server" OnItemDataBound="rptrOverrides_ItemDataBound" OnItemCommand="rptrOverrides_ItemCommand">
                      <HeaderTemplate>
                         <br />
                         <center><asp:Label ID="lblOverrideHead" Text="Employees with different pay rates are listed below" CssClass="InputTableLabelText" runat="server"></asp:Label></center>
                         <br />
                         <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                            <th style="display:table-header-group;">
                            <tr>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:70px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Aident</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Employee Name</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Pay Rate</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:150px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Effective<br />Week Ending</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:200px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Expiration</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:150px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Action</td>
                            </tr>
                            </th>
                      </HeaderTemplate>
             
                      <ItemTemplate>
                         <tr>
                               <td valign="bottom" align="left" style="width:70px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblAidentNumber" Runat="server"/>
                               </td>
                               <td valign="bottom" align="left" style="border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblLastName" Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Runat="server"/>
                               </td>
                               <td valign="bottom" align="left" style="width:100px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    $<asp:TextBox ID="txtEmpPayRate" runat="server" Width="50px"></asp:TextBox>
                               </td>
                               <td valign="bottom" align="left" style="width:150px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:TextBox ID="txtEffectiveDate" runat="server" Width="100px"></asp:TextBox>
                               </td>
                               <td valign="bottom" align="left" style="width:200px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:RadioButton ID="optIndefinite" Text="Does not expire" GroupName="radExpiration" runat="server" />
                                    <br />
                                    <asp:RadioButton ID="optWeekEnd" Text="Expires on Week Ending:" GroupName="radExpiration" runat="server" />
                                    <br />
                                    <asp:TextBox ID="txtExpirationDate" runat="server" Width="100px"></asp:TextBox>
                               </td>
                               <td valign="bottom" align="left" style="width:150px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:LinkButton ID="lnkSave" Text="Save Changes" runat="server" CommandName="save"></asp:LinkButton>
                               </td>
                        </tr>
                      </ItemTemplate>
          
                      <FooterTemplate>
                            <tr>
                                <td colspan="6" style="background-color:#cccccc;" align="center" width="100%">
                                    <table width="100%" border="0"  cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td valign="bottom" colspan="5"><br /><asp:Label ID="lblNewHeader" CssClass="InputTableLabelText" runat="server" Text="To add a pay rate for an employee fill in the fields below and click the add link"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <asp:Panel ID="pnlLookupEmployee" runat="server" DefaultButton="lnkLookup">
                                            <td valign="bottom" align="left" colspan="5">
                                                <asp:Label ID="Label1" Text="Employee Aident:" Runat="server"/>
                                                <br />
                                                <asp:TextBox ID="txtNewAidentNumber" Width="100px" runat="server"></asp:TextBox>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkLookup" Text="Lookup Employee" runat="server" CommandName="lookup"></asp:LinkButton>
                                            </td>
                                            </asp:Panel>
                                        </tr>
                                        <asp:Panel ID="pnlAddNewEmployee" runat="server" DefaultButton="lnkNewSave">
                                        <tr>
                                           <td valign="bottom" align="left" style="border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                                &nbsp;
                                                <asp:Label ID="lblNewAidentNumber" Text="" Runat="server"/>
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblNewLastName" Text="" Runat="server"/>,&nbsp;
                                                <asp:Label ID="lblNewFirstName" Text="" Runat="server"/>
                                                <asp:HiddenField ID="hdnNewEmployeeId" runat="server" />
                                           </td>
                                           <td valign="bottom" align="left" style="width:100px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                                <asp:Label ID="lblNewPayRate" Text="Pay Rate:" Runat="server"/>
                                                <br />
                                                $<asp:TextBox ID="txtNewEmpPayRate" runat="server" Width="50px"></asp:TextBox>
                                           </td>
                                           <td valign="bottom" align="left" style="width:150px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                                <asp:Label ID="lblNewEffectiveDate" Text="Week Ending Date:" Runat="server"/>
                                                <br />
                                                <asp:TextBox ID="txtNewEffectiveDate" runat="server" Width="100px"></asp:TextBox>
                                           </td>
                                           <td valign="bottom" align="left" style="width:200px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                                <asp:CheckBox ID="optFullClient" Text="ALL Departments" ToolTip="Apply to any departments not currently set" runat="server" Checked="false" />                                                
                                                <br />
                                                <asp:RadioButton ID="optNewIndefinite" Text="Does not expire" GroupName="radNewExpiration" Checked="true" runat="server" />
                                                <br />
                                                <asp:RadioButton ID="optNewWeekEnd" Text="Expires on Week Ending:" GroupName="radNewExpiration" runat="server" />
                                                <br />
                                                <asp:TextBox ID="txtExpirationDate" runat="server" Width="100px"></asp:TextBox>
                                           </td>
                                           <td valign="bottom" align="left" style="width:180px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                                <asp:LinkButton ID="lnkNewSave" Text="Add Employee's Pay Rate" runat="server" CommandName="newsave"></asp:LinkButton>
                                           </td>
                                       </tr>
                                       </asp:Panel>
                                   </td>
                                   </table>
    `                        </tr>
                         </table>
                      </FooterTemplate>
             
               </asp:Repeater>
        </asp:Panel>
        <asp:Panel Visible="false" ID="pnlPayRateCollision" runat="server">
            <asp:Label ForeColor="Red" ID="lblPayRateCollision" Text="Pay Rate Override Already Exists" runat="server"/>
        </asp:Panel>
    </div>
    </form>
 </div>    
</body>
</html>
