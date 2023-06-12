<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportExcel.aspx.cs" Inherits="MSI.Web.MSINet.TransportExcel" %>

<!DOCTYPE html>
<html xmlns:x="urn:schemas-microsoft-com:office:word">
<head runat="server">
    <title></title>
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
    <xml runat="server" id="excelId"> 
         <x:ExcelWorkbook> 
          <x:ExcelWorksheets> 
           <x:ExcelWorksheet> 
                <x:Name>HeadCountReport</x:Name> 
                <x:WorksheetOptions>
    			<x:Selected/>
	    		<x:FreezePanes/>
		    	<x:FrozenNoSplit/>
			    <!--- Bottom row number of top pane. --->
			    <x:SplitHorizontal>3</x:SplitHorizontal>
			    <x:TopRowBottomPane>3</x:TopRowBottomPane>

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
          <x:Formula>='HeadCountReport'!$1:$8</x:Formula>
         </x:ExcelName> 
</xml>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
