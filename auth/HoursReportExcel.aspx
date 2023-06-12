<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HoursReportExcel.aspx.cs" Inherits="MSI.Web.MSINet.HoursReportExcel" EnableViewState="false" %>

<%@ Register Src="../Controls/MSINetHoursReport.ascx" TagName="MSINetHoursReport"
    TagPrefix="uc6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns:x="urn:schemas-microsoft-com:office:excel">

<head id="Head1" runat="server">
    <title>MSI Web Trax</title>
    <style type="text/css">
    @page 
    {
        mso-header-data:"&C Page &P of &N";        
        mso-page-orientation:portrait;
        margin:.5in .25in .25in .25in;
        mso-header-margin:.3in;
        mso-footer-margin:.3in;
	}
    </style>
    <xml> 
         <x:ExcelWorkbook> 
          <x:ExcelWorksheets> 
           <x:ExcelWorksheet> 
                <x:Name>HoursReport</x:Name> 
                <x:WorksheetOptions>
    			<x:Selected/>
	    		<x:FreezePanes/>
		    	<x:FrozenNoSplit/>
			    <!--- Bottom row number of top pane. --->
			    <x:SplitHorizontal>4</x:SplitHorizontal>
			    <x:TopRowBottomPane>4</x:TopRowBottomPane>

                 <x:FitToPage/> 
                 <x:Print> 
                  <x:FitHeight>98</x:FitHeight>
                  <x:ValidPrinterInfo/> 
                  <x:Scale>100</x:Scale> 
                  <HorizontalResolution>600</HorizontalResolution> 
                  <VerticalResolution>600</VerticalResolution> 
                 </x:Print> 
                 <x:Selected/> 
                 <x:ActivePane>2</x:ActivePane>
                 <x:ProtectContents>False</x:ProtectContents> 
                 <x:ProtectObjects>False</x:ProtectObjects> 
                 <x:ProtectScenarios>False</x:ProtectScenarios>
                </x:WorksheetOptions> 
			    <x:PageMargins x:Left="0.25" x:Right="0.25" x:Top="0.5" x:Bottom="0.25" x:Header="0.3" x:Footer="0.3" />           
           </x:ExcelWorksheet> 
          </x:ExcelWorksheets> 
          <x:ProtectStructure>False</x:ProtectStructure> 
          <x:ProtectWindows>False</x:ProtectWindows> 
         </x:ExcelWorkbook> 
         <x:ExcelName> 
          <x:Name>Print_Titles</x:Name> 
          <x:SheetIndex>1</x:SheetIndex>
          <x:Formula>='HoursReport'!$1:$4</x:Formula>
         </x:ExcelName> 
</xml>

</head>
<body >  <div align="center">
        <asp:Panel ID="Panel1" runat="server" Height="159px" HorizontalAlign="Left" Width="1100px" EnableViewState="false">
    <form id="Form1" runat="server" enableviewstate="false">
            <uc6:MSINetHoursReport id="ctlHoursReport" runat="server" ExportToExcel="true" EnableViewState="false">
            </uc6:MSINetHoursReport> 
    </form>
        </asp:Panel>
 </div>    
</body>
</html>