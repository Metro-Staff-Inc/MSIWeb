<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetDispatchData.ascx.cs" Inherits="MSI.Web.Controls.MSINetDispatchData" EnableViewState="false" %>


    <div align="center">
        <table id="clientParams">
            <thead>
                <tr>
                    <th>Dispatch Office:
                    </th>
                    <th>Date:
                    </th>
                    <th class="weekly">Weekly Report:
                    </th>
                    <th>Shift:
                    </th>
                    <th>Export:
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>
                        <select id="selDispatch">
                        </select>
                    </td>
                    <td>
                        <input type="date" id="selDate" disabled="disabled" />
                    </td>
                    <td class="weekly">
                        <input type="checkbox"/>
                    </td>
                    <td>
                        <select id="selShift" disabled="disabled"/>
                    </td>
                    <td>
                        <input type="button" id="btnExcel" value="Export to Excel" disabled="disabled"/>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <table id="clientTable" visible="false">
        <thead>
            <tr>
                <!-- 
        public int clientId { get; set; }
        public int totSent { get; set; }
        public int regs { get; set; }
        public int tempsOrdered { get; set; }
        public int tempsSent { get; set; }
        public int unfilled { get; set; }
        public int extras { get; set; }
        public string notes { get; set; }
        public string createdBy { get; set; }
        public DateTime createdDt { get; set; }
        public override String ToString()
                    -->
                <th>Client</th>
                <th>TOTAL SHIFT HEAD COUNT</th>
                <th>DIRECT REPORTS / REGULARS</th>
                <th>DAILY DISPATCH</th>
                <th>DAILY SWING ORDER</th>
                <th>OVER / UNDER</th>
                <th>MSI TRANSPORTED</th>
                <th>NOTES</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td colspan="8"></td>
<!--            <td><select class="clientNames" disabled="disabled"></select></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><input type="number" min="0" max="999" value="0" disabled="disabled" /></td>
                <td><textarea cols="40" rows="1" placeholder="Enter any notes here" disabled="disabled"></textarea></td>
-->            </tr>
        </tbody>
    </table>
<script type="text/javascript" src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
<script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
<script src="../Scripts/DispatchData.js"></script>
<script type="text/javascript" src="../javascriptOOXml/linq.js"></script>
<script type="text/javascript" src="../javascriptOOXml/ltxml.js"></script>
<script type="text/javascript" src="../javascriptOOXml/ltxml-extensions.js"></script>
<script type="text/javascript" src="../javascriptOOXml/jszip.js"></script>
<script type="text/javascript" src="../javascriptOOXml/jszip-load.js"></script>
<script type="text/javascript" src="../javascriptOOXml/jszip-inflate.js"></script>
<script type="text/javascript" src="../javascriptOOXml/jszip-deflate.js"></script>
<script type="text/javascript" src="../javascriptOOXml/FileSaver.js"></script>
<script type="text/javascript" src="../javascriptOOXml/openxml.js"></script>
<script type="text/javascript" src="../javascriptOOXml/TemplateDocumentB64.js"></script>
<script src="../Scripts/Excel.js"></script>

<style>
    .negVal {
        color:red;
    }
</style>

