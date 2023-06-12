<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceSummary.aspx.cs" Inherits="MSI.Web.MSINet.InvoiceSummary" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<%@ Register Src="~/Controls/MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:x="urn:schemas-microsoft-com:office:excel" >
<head runat="server">
    <title>MSI Web Trax</title>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
  <!--table JHM removed empty style tags...
  @page 
    {
        mso-header-data:"&C Page &P of &N";
        mso-page-orientation:portrait;
        margin:.66in .25in .25in .25in;
        mso-header-margin:.5in;
        mso-footer-margin:.5in;
	}
  -->
<!--

    <xml> 
         <x:ExcelWorkbook> 
          <x:ExcelWorksheets> 
           <x:ExcelWorksheet> 
                <x:Name>InvoiceSummary</x:Name> 
                <x:WorksheetOptions> 
                 <x:FitToPage/> 
                 <x:Print> 
                  <x:FitHeight>100</x:FitHeight>
                  <x:ValidPrinterInfo/> 
                  <x:Scale>100</x:Scale> 
                  <x:HorizontalResolution>600</x:HorizontalResolution> 
                  <x:VerticalResolution>600</x:VerticalResolution> 
                 </x:Print> 
                 <x:Selected/> 
                 <x:Panes> 
                  <x:Pane> 
                   <x:Number>3</x:Number> 
                   <x:ActiveRow>22</x:ActiveRow> 
                   <x:ActiveCol>9</x:ActiveCol> 
                  </x:Pane> 
                 </x:Panes> 
                 <x:ProtectContents>False</x:ProtectContents> 
                 <x:ProtectObjects>False</x:ProtectObjects> 
                 <x:ProtectScenarios>False</x:ProtectScenarios> 
                </x:WorksheetOptions> 
           </x:ExcelWorksheet> 
          </x:ExcelWorksheets> 
          <x:ProtectStructure>False</x:ProtectStructure> 
          <x:ProtectWindows>False</x:ProtectWindows> 
         </x:ExcelWorkbook> 
         <x:ExcelName> 
          <x:Name>Print_Titles</x:Name> 
          <x:SheetIndex>1</x:SheetIndex>
          <x:Formula>='InvoiceSummary'!$1:$7</x:Formula>
         </x:ExcelName> 
</xml>
-->
</head>
    <body>  
    <div align="center">
        <form id="form1" runat="server">
            <asp:Panel ID="pnlMSIControls" runat="server">
                <div>
                    <uc1:MastHead ID="ctlMastHead" runat="server" />
                </div>
                <br />
                <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
                <br />
                <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="InvoiceProcessing" />
                <asp:Label ID="lblTitle" runat="server" CssClass="MSINetSectionHeading" Text="Invoice Processing" Width="145px"/>
                &nbsp;&nbsp;&nbsp;
                <br />
                <br />
            </asp:Panel>
        <div>
        <asp:Panel ID="pnlHeader" runat="server" BackColor="#E0E0E0" Width="990px" >
            <asp:Table ID="tblPeriod" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="230px">
                        <uc1:MSINetCalendar id="ctlWeekEnding" runat="server" DisplayMode="WeekEnding" LabelText="Week Ending:"></uc1:MSINetCalendar></asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                        <asp:Image ID="Image1" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Text="View Invoice"></asp:Button>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                        <asp:Image ID="Image2" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:LinkButton id="lnkCreateCSV" onclick="lnkCreateCSV_Click" runat="server" Text="Export Invoice to CSV"></asp:LinkButton>
                        <asp:HiddenField ID="hdnWeekEnd" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                        <asp:Image ID="Image3" ImageUrl="../Images/clear_spacer.gif" runat="server" Height="35px" Width="1px" BorderWidth="0" />
                        <asp:HyperLink ID="lnkExportDetail" Target="_blank" NavigateURL="/auth/InvoiceSummary.aspx?date=" runat="server" Text="Export Invoice to Excel"></asp:HyperLink>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <asp:Panel ID="pnlNoInvoice" runat="server" Visible="false" Width="990px"  style="border:solid 1px #cccccc" >
            <asp:Label ID="lblCreateCopy" runat="server" Text="An invoice for the week end date selected has not been created.  Click the link below to create the invoice."></asp:Label>
            <br /><br />
            <asp:LinkButton ID="lnkCreateInvoice" runat="server" Text="Create Invoice"></asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="pnlHoursNotApproved" runat="server" Visible="false" Width="990px"  style="border:solid 1px #cccccc" >
            <asp:Label ID="Label1" runat="server" Text="Hours for this week end date have not yet been approved  Click the link below to view and/or approve the hours."></asp:Label>
            <br /><br />
            <asp:LinkButton ID="lnkApproveHours" runat="server" Text="View Hours"></asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="pnlInvoiceHeader" style="padding:10px;" runat="server" Visible="false" Width="970px" >
            <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                <tr >
                    <td colspan="6" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblClientHead" Runat="server" Text="Client Header"/>
                    </td>
                    <td style="width:30px; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceDateHead" Runat="server" Text="Date:"/>
                    </td>
                    <td style="width:130px; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceDate" Runat="server" Text="Date:"/>
                    </td>
                </tr>
                <tr >
                    <td colspan="6" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblWebTraxHead" Runat="server" Text="MSI WebTrax Invoice Report"/>
                    </td>
                    <td style="width:30px; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceNumberHead" Runat="server" Text="Inv#:"/>
                    </td>
                    <td style="width:130px; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceNumber" Runat="server" Text="Inv#:"/>
                    </td>
                </tr>
                <tr >
                    <td colspan="6" style="text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblWeekEnd" Runat="server" Text="Week Ending"/>
                    </td>
                    <td style="width:30px; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceDollarsHead" Runat="server" Text="Inv$:"/>
                    </td>
                    <td style="width:130px; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                        <asp:Label ID="lblInvoiceDollars" Runat="server" Text="Inv$:"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlInvoiceDetail" runat="server" Visible="false" Width="990px" style="border:solid 1px #cccccc" >
                 <asp:Repeater id="rptrInvoice" runat="server" OnItemDataBound="rptrInvoice_ItemDataBound">
                      <HeaderTemplate>
                         <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 0pt #cccccc">
                            <th style="display:table-header-group;">
                            <tr>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:70px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Badge #</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Employee Name</td>
                                <td align="left" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:left; font-family:Arial; font-size:10pt; font-weight:bold;">Job Code</td>
                                <td align="right" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Pay Rate</td>
                                <td align="right" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Bill Rate</td>
                                <td align="right" style="padding:2pt 2pt 2pt 2pt; width:60px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">OT Bill<br />Rate</td>
                                <td align="right" style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Reg</td>
                                <td align="right" style="padding:2pt 2pt 2pt 2pt; width:100px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">OT</td>
                                <td align="right" style="padding:2pt 5pt 2pt 2pt; width:120px; height:30px; color:#ffffff; background-color:#003776; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Total<br />Billing</td>
                            </tr>
                            </th>
                      </HeaderTemplate>
             
                      <ItemTemplate>
                         <tr id="trDepartment" runat="server">
                               <td colspan="11" style="border-bottom:solid 0pt #cccccc; padding:12pt 12pt 12pt 12pt; color:Black; text-align:left; font-family:Arial; height:40px; font-size:12pt; font-weight:bold;"><asp:Label ID="lblDepartment" Runat="server"/>&nbsp;&nbsp;<asp:LinkButton runat="server" ID="lnkChangePayRates" Text="Change Pay Rates" OnClick="lnkChangePayRates_Click"></asp:LinkButton></td>
                         </tr>
                         <tr>
                               <td align="left" style="width:70px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblBadgeNumber" Runat="server"/>
                               </td>
                               <td align="left" style="border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblLastName" Runat="server"/>,&nbsp;<asp:Label ID="lblFirstName" Runat="server"/>
                               </td>
                               <td align="left" style="width:60px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblJobCode" Runat="server"/>
                               <td align="right" style="width:60px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblPayRate" Runat="server"/>
                               </td>
                               <td align="right" style="width:60px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblBillRate" Runat="server"/>
                               </td>
                               <td align="right" style="width:60px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblOTBillRate" Runat="server"/>
                               </td>
                               <td align="right" style="width:100px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblRegHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:100px; border-bottom:solid 1px #cccccc; padding:2pt 2pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblOTHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:120px; border-bottom:solid 1px #cccccc; padding:2pt 5pt 2pt 2pt; color:Black; font-family:Arial; font-size:10pt; font-weight:normal;">
                                    <asp:Label ID="lblTotalBilling" Runat="server"/>
                               </td>
                        </tr>
                        <tr id="trDepartmentTotals" runat="server">
                               <td colspan="6" style="border-top:solid 1pt #000000; border-bottom:double 1px #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblDepartmentTotalLabel" Runat="server"/>
                               </td>
                               <td align="right" style="width:100px; border-top:solid 1px #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblDeptTotTotalHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:100px; border-top:solid 1px #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblDeptTotOTHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:120px; border-top:solid 1px #000000; border-bottom:double 1pt #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblDeptTotBilling" Runat="server"/>
                               </td>
                         </tr>
                      </ItemTemplate>
          
                      <FooterTemplate>
                            <tr id="trGrandTotals" runat="server">
                               <td colspan="6" style="border-top:solid 1pt #000000; border-bottom:double 1px #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">Grand Totals:</td>
                               <td align="right" style="width:100px; border-top:solid 1px #000000; border-bottom:double 1px #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblGrandTotalHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:100px; border-top:solid 1px #000000; border-bottom:double 1px #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblGrandOTHours" Runat="server"/>
                               </td>
                               <td align="right" style="width:120px; border-top:solid 1px #000000; border-bottom:double 1px #000000; padding:2pt 2pt 2pt 2pt; color:Black; text-align:right; font-family:Arial; font-size:10pt; font-weight:bold;">
                                    <asp:Label ID="lblGrandTotBilling" Runat="server"/>
                               </td>
                            </tr>
                            
                         </table>
                      </FooterTemplate>
             
               </asp:Repeater>
        </asp:Panel>
    </div>
    </form>
 </div>    
</body>
</html>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         