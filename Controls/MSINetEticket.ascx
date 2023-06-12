<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetEticket.ascx.cs" Inherits="MSI.Web.Controls.MSINetEticket" %>

<br />
<div id="divHeader">
        <table id="eTicketHeader">
            <thead>
                <tr>
                    <th>Week Ending:</th>
                    <th>Location</th>
                    <th>Shift</th>
                    <th>Department</th>
                    <th>Report</th>
                    <td><input type="radio" name="temps" value="tempEmployees" />Temps</td>
                    <th><input id="btnView" type="button" value="VIEW" /></th>
                    <th><input id="btnPrn" onclick="printETicketMenu()" type="button" value="PRINT" /><input id="btnCSV" type="button" value="CSV" /></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><select id="ddlWeekEnding"></select></td>
                    <td><select id="ddlLocation"></select></td>
                    <td><select id="ddlShiftType"></select></td>
                    <td><select id="ddlDepartment"></select></td>
                    <td><span id="spanSupervisor"></span></td>
                    <td><input type="radio" value="allEmployees" name="temps" checked="checked" />All Employees</td>
                    <td id="tdSubmit"><input id="btnSubmit" disabled="disabled" type="button" value="SUBMIT" /></td>
                    <td id="tdSubmitInfo" style="display:none">
                        <div class="submitted"><span id="submitInfo">Submitted by: </span><span id="submitUser">itdept</span><br /><span>Submit Date: </span><span id="submitDate">10/21/2013</span></div>
                        <div class="notSubmitted"><span>Not Submitted</span></div>
                    </td>
                    <td><input id="btnUpdate" type="button" value="UPDATE" /></td>
                </tr>
            </tbody>
        </table>
</div>
    <table id="eTicket">
        <thead>
            <tr>
                <th>#</th><th>Name</th><th>Badge</th><th>Mon</th><th>Tue</th><th>Wed</th><th>Thr</th><th>Fri</th><th>Sat</th><th>Sun</th><th>Tot</th>
            </tr>
        </thead>
        <tbody>

        </tbody>
        <tfoot>
            <tr>
                <td colspan='3'>Totals</td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
                <td class='dayTotal'><input disabled='disabled' type='text' size='4' value='0.0'/></td>
            </tr>
        </tfoot>
    </table>

        <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
        <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
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
        <script src="../Scripts/ETicket.js" type="text/javascript"></script>
        <script src="../javascriptOOXml/TemplateDocumentB64.js" type="text/javascript"></script>
        <link href="../Includes/css/ui-lightness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .underline
            {
                text-decoration:underline;
            }
            .changed 
            {
                background-color:#FFDDDD;    
            }
            .highlight
            {
                background-color:#FFFFAA;
            }
            span
            {
                margin-left:4px;
            }
            .employeeInfo td
            {
                padding-left:40px;
            }
            #updRosters
            {
                margin-left:20px;
            }
            #addToRosterSpan
            {
                margin-left:32px;
            }
            #callTable, #clientRoster
            {
                background-color:#FFFFFF;
                width:1200px;
            }
            .odd
            {
                background-color:#DDDDFF;
            }
            #callTable th, #clientRoster th
            {
                background-color:#9999DD;
                font-size:larger;
            }
            th > input, #btnSubmit, #btnUpdate
            {
                font-size:12px;
            }
            .highLight
            {
                background-color:LightYellow;
            }
            .employeeRoot
            {
                font-size:larger;
            }
            .employeeRootBadPhoneNumber
            {
                background-color:#FFAAAA;
                font-size:larger;
            }
            .employeeRootAvailable
            {
                background-color:#AAFFAA;
                font-size:larger;
            }
            .employeeRootUnavailable
            {
                background-color:#FFAAAA;
                font-size:larger;
            }
            .browserPhoneAvailable
            {
                background-color:#367600;
                color:Black;
                font-size:larger;
            }
            .browserPhoneUnavailable
            {
                background-color:#AA2600;
                font-size:larger;
            }
            .titleBarHighlight
            {
                background-color:#5555FF;
            }
            .titleBarAlert
            {
                background-color:#FF5555;
            }
            .titleBarNormal
            {
                background-color:#003776;
            }
            .smallButton
            {
                font-size:10px;
                margin-left:8px;
            }
            .titleBarBP
            {
                color:White;
                font-size:18px;
                font-weight:bold;
                height:32px;
                margin:8px;
                width:1200px;
            }
            .titleBar
            {
                color:White;
                font-size:18px;
                font-weight:bold;
                height:32px;
                margin:8px;
                width:1200px;
            }
            .titleBar h3
            {
                float:right;
            }
            .no-close .ui-dialog-titlebar-close 
            {
                display: none;
            }
            .inpDate
            {
                background-color:#EEE;
            }
            #btnPoolClear
            {
                margin-left:64px;
            }
            .pbDnr
            {
                color:Red;
            }
        </style>

