<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DaysWorkedReportExcel.aspx.cs" Inherits="MSI.Web.MSINet.DaysWorkedReportExcel" EnableViewState="false" %>

<%@ Register Src="../Controls/MSINetDaysWorkedReport.ascx" TagName="MSINetDaysWorkedReport"
    TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:x="urn:schemas-microsoft-com:office:excel">

<head runat="server">
    <title>MSI Web Trax</title>
    
<style>
  <!--table
  @page 
    {
        mso-header-data:"&C Page &P of &N";
        mso-page-orientation:portrait;
	}
  -->
</style>

    <xml> 
         <x:ExcelWorkbook> 
          <x:ExcelWorksheets> 
           <x:ExcelWorksheet> 
                <x:Name>DaysWorkedReport</x:Name> 
                <x:WorksheetOptions> 
                 <x:FitToPage/> 
                 <x:Print> 
                  <x:FitHeight>99</x:FitHeight>
                  <x:ValidPrinterInfo/> 
                  <x:Scale>86</x:Scale> 
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
          <x:Formula>='HoursReport'!$1:$5</x:Formula>
         </x:ExcelName> 
</xml>



</head>
<body>  <div align="center">
        <asp:Panel ID="Panel1" runat="server" Height="159px" HorizontalAlign="Left" Width="990px" EnableViewState="false">
            <uc6:MSINetDaysWorkedReport id="ctlDaysWorkedReport" runat="server" ExportToExcel="true" EnableViewState="false">
            </uc6:MSINetDaysWorkedReport>
        </asp:Panel>
 </div>    
</body>
</html>
