<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="HeadCountFullRoster.aspx.cs" Inherits="MSI.Web.MSINet.HeadCountFullRoster" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
	<title>Headcount Report</title>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
        <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />
        <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
        
        <script type="text/javascript" src="../javascriptOOXml/linq.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml-extensions.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-load.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-inflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-deflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/FileSaver.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/openxml.js"></script>
        <script src="../javascriptOOXml/TemplateDocumentB64.js" type="text/javascript"></script>
        <!--<script src="../Scripts/ExcelExport.js"></script>-->
               
        <!--<script src= "https://appsforoffice.microsoft.com/lib/1.0/hosted/office.js"></script>-->
        <script src="../Scripts/TestOut.js"></script>
    <style>
        #employeeTable, #employeeTable tr, #employeeTable td {
        border: 1px solid black;
        border-collapse: collapse;
        padding:4px 7px 2px 8px;
        }
        #employeeTable tr {
            margin-left:12px;
            border-left:12px;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <uc1:MastHead ID="ctlMastHead" runat="server" />
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" />
        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="HeadCount Full Rosters" />
        
        <asp:Panel Width="1000px" HorizontalAlign="Center" BackColor="#ffffee" runat="server">
        <asp:Repeater ID="rptrHeadCount" runat="server" OnItemDataBound="rptrHeadCount_ItemDataBound">
            <HeaderTemplate>
                <table id="employeeTable" style="text-align:left; width:100%">
                    <thead>
                        <tr style="height:64px">
                            <th colspan="4">
                                <asp:Button ID="btnWord" runat="server" OnClick="btnWord_Click" Text="Export to Word" />
                                <input style="margin-left:12px; margin-bottom:12px; margin-top:32px" 
                                    onclick="exportToXlsx2('employeeTable');" type="button" runat="server" value="Export to Excel" />
                            </th>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>    
                <tr runat="server" id="trDepartmentSpace" visible="false" style="border:0px">
                    <td colspan="4" style="border:0px"> </td>
                </tr>
                <tr runat="server" style="height:16px; margin-right:12px; margin-left:12px" id="trDepartmentInfo">
                    <td colspan="2" runat="server" style="background-color:aquamarine; font-size:24px" id="tdDepartmentShift">
                    </td>
                    <td colspan="2" runat="server" style="background-color:aquamarine; text-align:right; font-size:24px" id="tdReportTime">
                    </td>
                </tr>
                <tr runat="server" style="font-size:16px; background-color:lightblue" id="trEmployeeHeader">
                    <td runat="server" id="thId">ID #</td>
                    <td runat="server" id="thName">Name</td>
                    <td runat="server" id="thStatus">Status</td>
                    <td runat="server" id="thPunchTime">Most Recent Punch</td>
                </tr>
                <tr style="text-align:left">
                    <td runat="server" id="tdId"></td>
                    <td runat="server" id="tdName"></td>
                    <td runat="server" id="tdStatus"></td>
                    <td runat="server" id="tdPunchTime"></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </asp:Panel>
        <br />
    </div>
    </form>
</body>
</html>
