var XAttribute = Ltxml.XAttribute;
var XCData = Ltxml.XCData;
var XComment = Ltxml.XComment;
var XContainer = Ltxml.XContainer;
var XDeclaration = Ltxml.XDeclaration;
var XDocument = Ltxml.XDocument;
var XElement = Ltxml.XElement;
var XName = Ltxml.XName;
var XNamespace = Ltxml.XNamespace;
var XNode = Ltxml.XNode;
var XObject = Ltxml.XObject;
var XProcessingInstruction = Ltxml.XProcessingInstruction;
var XText = Ltxml.XText;
var XEntity = Ltxml.XEntity;
var cast = Ltxml.cast;
var castInt = Ltxml.castInt;

var W = openXml.W;
var NN = openXml.NoNamespace;
var wNs = openXml.wNs;

var S = openXml.S;
var sNs = openXml.sNs;

var R = openXml.R;


function hoursReportToExcel(tblId, sheetData, worksheetXDoc) {
    //get client name
    var clientName = $("span[id*='lblSubHeader']").text();
    clientName = clientName.substring(clientName.indexOf(" - ") + 3);
    setCell(1, 1, sheetData, 0, null, clientName, 3, 1, 2);
    setCell(2, 1, sheetData, 0, null, "MSI Webtrax Hours Report", 3, 1, 2);
    //get date
    var weDate = $('select[id*="txtCalendar"] option:selected').text();

    setCell(3, 1, sheetData, 0, null, "Week Ending Date: " + weDate, 3, 1, 2);

    weDate = new Date(weDate);
    //alert(weDate);

    tblId = '#' + tblId;
    var eRow = 4;
    var eCol = 1;
    var widths = new Array();
    /* now the body */
    var tblBodyRows = $(tblId).find('tbody tr:visible');
    var daysRef = ["Mon", "Tue", "Wed", "Thr", "Fri", "Sat", "Sun"];
    var days = { Mon: 0, Tue: 1, Wed: 2, Thr: 3, Fri: 4, Sat: 5, Sun: 6 };
    var dayOffset = 0;
    var val = 0;
    var rowData = $(tblBodyRows[1]).find('td:visible');
    for (var c = 0; c < $(rowData).length; c++) {
        val = $(rowData[c]).text();
        if (daysRef.indexOf(val) >= 0) {
            var curDay = new Date(weDate);
            curDay.setDate(weDate.getDate() - 6 + dayOffset);
            dayOffset++;
            //alert(val + ", " + curDay);
            var m = "" + (curDay.getMonth() + 1);
            if (m.length == 1) m = "0" + m;
            var d = "" + curDay.getDate();
            if (d.length == 1) d = "0" + d;
            //alert(m + "/" + d);
            setCell(eRow, eCol, sheetData, 0, null, m + "/" + d, 3, 1, 1);
        }
        eCol++;
    }
    for (var r = 0; r < tblBodyRows.length; r++) {
        var rowData = $(tblBodyRows[r]).find('td:visible');
        eCol = 1;
        for (var c = 0; c < $(rowData).length; c++) {
            if ($(rowData[c]).find("input").length == 1) {
                //alert($(rowData).length);
                eCol++;
            }
            else {
                val = $(rowData[c]).text();
                val = val.trim();
                val = String(val);
                var colSpan = $(rowData[c]).attr('colspan');
                if (isNaN(colSpan))
                    colSpan = 1;
                colSpan = parseInt(colSpan);
                var style = RowStyle;
                setCell(eRow, eCol, sheetData, 0, null, val, style, 1, 1);
                eCol++;
                while (colSpan > 1) {
                    setCell(eRow, eCol, sheetData, 0, null, "", style, 1, 1);
                    eCol++;
                    colSpan--;
                }
                if (widths[c] == null || val.length > widths[c])
                    widths[c] = val.length;
            }
        }
        eRow++;
    }
    setColumnWidths(widths, worksheetXDoc);
    setFreezePane(3, worksheetXDoc);
}

function setCell(r, c, sheetData, val, formula, text, style, rowCount, cellCount) {
    for (var cIdx = 0; cIdx < cellCount; cIdx++) {
        /* CREATE CELL */
        var cell = new XElement(S.c);
        if (cIdx > 0) {
            val = cIdx;
        }
        else if (formula != null && c == 0) {
            var f = new XElement(S.f);
            f.value = formula;
            cell.add(f);
        }
        else if (text != null) {
            cell.add(new XAttribute('t', 'inlineStr'));
            var is = new XElement(S._is);
            var t = new XElement(S.t);
            t.value = text;
            is.add(t);
            cell.add(is);
        }
        else {
            var value = new XElement(S.v);
            value.value = "" + val;
            cell.add(value);
        }

        var attribute = new XAttribute('r', getRef(c + cIdx, r));
        cell.add(attribute);
        if (style != null)
            cell.add(new XAttribute('s', style));

        var row = new XElement(S.row, cell);
        attribute = new XAttribute('r', r);
        row.add(attribute);
        sheetData.add(row);
    }
}

