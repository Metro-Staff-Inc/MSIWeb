<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetHeadCount.ascx.cs" Inherits="MSI.Web.Controls.MSINetHeadCount" EnableViewState="false" %>
<%@ Register Src="MSINetCalendar.ascx" TagName="MSINetCalendar" TagPrefix="uc1" %>
<style>
    .employeeTable tbody{
        height: 200px;
        overflow-y:auto;
        background-color:lightblue;
    }
    .employeeTable, .employeeTable th, .employeeTable td {
        border: 1px solid black;
        border-collapse: collapse;
        font-size:large;
    }
    .employeeTable th, .employeeTable td {
        padding: 5px;
    }
    .employeeTable th {
        text-align: left;
    }
    .deptRow{
        background-color: lightgray;
    }
</style>
        <div runat="server" id="headCountControlPanel" style="width:1000px">
            <table>
                <tr>
                    <td>
                        <span>Date: </span>
                    </td>
                    <td>
                        <select runat="server" id="hcDate" >
                        </select>
                    </td>
                    <td> 
                        <input id="hcGo" type="button" value="GO" />
                    </td>
                    <td>
                        <asp:HyperLink ID="lnkExportExcel" Target="_blank" runat="server" Text="Export to Excel"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:HyperLink ID="lnkExportWord" Target="_blank" runat="server" Text="Export to Word"></asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <link href="../includes/MSIMain.css" type="text/css" rel="stylesheet" />
    <link href="../Includes/myTheme.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Includes/css/defaultTheme.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <link href="../Includes/flexigrid.pack.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/flexigrid.pack.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox.pack.js" type="text/javascript"></script>

    <link href="../Scripts/jQuery-modalPopLite/modalPopLite.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery-modalPopLite/modalPopLite.min.js" type="text/javascript"></script>

    <script src="../Scripts/DailyPunches.js"></script>
    
