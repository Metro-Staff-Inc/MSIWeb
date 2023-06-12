
function getDateFromString(dt) {
    var firstSlash = dt.indexOf("/");
    var secSlash = dt.indexOf("/", firstSlash + 1);
    dt = new Date(dt.substring(secSlash + 1), dt.substring(0, firstSlash) - 1, dt.substring(firstSlash + 1, secSlash));
    return dt;
}

function getDate(dt) {
    return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
}

function formatDate(dt) {
    return "" + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function formatDateTime(dt) {
    var am = "am";
    //alert(dt.getHours());
    var hour = dt.getHours() % 12;
    if (hour == 0) hour += 12;
    if (dt.getHours() >= 12) am = "pm";
    if (hour <= 9) hour = "0" + hour;
    else hour = "" + hour;
    var min = dt.getMinutes();
    if( min <= 9 ) min = "0" + min;
    else min = "" + min;
    return "" + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();// + " " + hour + ":" + min + " " + am;
}

function setMap(empLocs) {
    var marker = [];
    var uluru = {lat:0, lng:0};
    if( empLocs.length > 0 ) uluru = { lat: empLocs[0].latitude, lng: empLocs[0].longitude };
        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: 8,
            center: uluru
        });
        //alert(empLocs.length);
        //for (var i = 0; i < empLocs.length; i++) {
        new google.maps.Marker({
            position: { 'lat': empLocs[0].latitude, 'lng': empLocs[0].longitude },
            map: map,
            title: empLocs[0].lastName + ", " + empLocs[0].firstName + "\n" + empLocs[0].client// + "\n" + "http://www.msiwebtrax.com/Dropbox/ids/" + empLocs[i].aident + ".jpg"
            //icon: "http://www.msiwebtrax.com/Dropbox/ids/" + empLocs[i].aident + ".jpg"
        });
        new google.maps.Marker({
            position: { 'lat': empLocs[1].latitude, 'lng': empLocs[1].longitude },
            map: map,
            title: empLocs[1].lastName + ", " + empLocs[1].firstName + "\n" + empLocs[1].client// + "\n" + "http://www.msiwebtrax.com/Dropbox/ids/" + empLocs[i].aident + ".jpg"
            //icon: "http://www.msiwebtrax.com/Dropbox/ids/" + empLocs[i].aident + ".jpg"
        });
    //}
        createTable(empLocs);
}

function createTable(data) {
    $("#employeeTable").empty();
    var content = "<table id='employeeTable'><thead>";
    content += '<tr><th>#</th><th>ID</th><th>Last Name</th><th>First Name</th><th>Fleetmatics ID</th><th>Ride Date</th><th>Swipe Count</th><th>Punch Loc</th><th>Roster Loc</th><th>Dispatch Office</th><th>Driver</th>';
    content += "</thead>";
    content += "<tbody>"
    for (i = 0; i < data.length; i++) {
        content += '<tr><td>' + (i+1) + '</td>';
        content += '<td>' + data[i].aident + '</td>';
        content += '<td>' + data[i].lastName + '</td>';
        content += '<td>' + data[i].firstName + '</td>';
        content += '<td>' + data[i].fleetMaticsId + '</td>';
        content += '<td>' + formatDateTime(getDate(data[i].rideDate)) + '</td>';
        content += '<td>' + data[i].swipeCount + '</td>';
        content += '<td>' + data[i].client + '</td>';
        content += '<td>' + data[i].rosterClient + '</td>';
        content += '<td>' + data[i].dispatch + '</td>';
        content += '<td>' + data[i].driverName + '</td>';
        content += '</tr>';
    }
    content += "</tbody></table>"

    $('#employeeList').append(content);
    $("#employeeTable").fixedHeaderTable({
        width: '1600',
        altClass: 'odd',
        themeClass: 'fancyTable',
        fixedColumn: true,
        autoShow: true
    });
}
function retrieveData() {
    var empList;
    var sd = getDateFromString($('#inpStartDt').val());
    var ed = getDateFromString($('#inpEndDt').val());
    //sd = '01-01-2018';
    //ed = '03-01-2018';
    sd = formatDate(sd);
    ed = formatDate(ed);
    //alert(sd + ", " + ed);
    var data2 = {
        startDate: sd,
        endDate: ed
    };
    //alert(JSON.stringify(data2));
    //var Url = "../Open/EmployeeStatus";
    var data3 = JSON.stringify(data2);
    //alert(data3);
    Url = "../Open/GetTransport";
    waitIndicator(true);
    $.ajax({
        type: "POST",
        data: data3, //JSON.stringify(data2),
        url: Url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (dta) {
            //alert(dta + ", " + dta.length);
            //alert(dta.GetTransportResult.length);
            //setMap(dta.GetTransportResult);
            createTable(dta.GetTransportResult);
            $("#exportExcel").removeAttr("disabled");
            $("#exportExcel").click(function () {
                exportToXlsx("employeeTable");
            });
            waitIndicator(false);
        },
        error: function (msg) {
            waitIndicator(false);
            alert("error!" + msg);

        }
    });
}
$(document).ready(function () {
    var curDate = new Date();
    $("#inpStartDt").val(curDate.getMonth()+1 + "/01/" + curDate.getFullYear());
    $("#inpEndDt").val(formatDate(curDate));
    $("#inpStartDt").datepicker();
    $("#inpEndDt").datepicker();
    $("#btnGo").click(function () {
        retrieveData();
    })
});
function waitIndicator(on, titleBar) {
    $('#waiting').remove();
    if (on == true) {
        if (titleBar == null)
            titleBar = "loading";
        $("<div id='waiting'><h3 style='padding-left:88px'>please wait...</h3><img style='padding-left:88px' src='../Images/ajax-loader.gif'/></div>").dialog(
        {
            open: function (event, ui) {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            title: titleBar
        });
    }
    else {
        $('#waiting').remove();
    }
}


(function (root) {  // root = global
    "use strict";
    var HeaderStyle = 6;
    var RowStyle = 3;
    var RowStyleOdd = 1;
    var FooterStyle = 5;
    var WarningStyle = 2;

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

    root.exportToXlsx = function (tableId) {
        //alert(tableId);
        // open the document
        openXml.util.bucketTimer.init();
        openXml.util.bucketTimer.bucket("OpenAndSave");

        //var doc = new openXml.OpenXmlPackage(openedFileData);
        var doc = new openXml.OpenXmlPackage(excelTemplate);
        var workbook = doc.workbookPart();
        var workbookXDoc = workbook.getXDocument();
        var sheets = workbookXDoc.root.element(S.sheets);
        var sheet = null;
        var relId = null;
        var sheetName = "MySheet";
        for (var i = 0; i < sheets.nodesArray.length; i++) {
            sheet = sheets.nodesArray[i];
            if (sheet.nodeType != 'Element')
                continue;
            if (sheet.attribute('name').value == sheetName) {
                relId = sheet.attribute(R.id).value;
                break;
            }
        }
        if (relId == null)
            alert(sheetName + " not found!");
        else {
            var worksheet = workbook.getPartById(relId);
            var worksheetXDoc = worksheet.getXDocument();
            var dimensions = worksheetXDoc.root.element(S.dimension);
            var coords = dimensions.attribute('ref').value;
            var sheetData = worksheetXDoc.root.element(S.sheetData);
            sheetData.removeAll();

            /* set filename */
            var dt = new Date;
            var fileName = '' + dt.getFullYear() + (dt.getMonth()+1) + dt.getDate() + '_';
            /* lets add some data */
            tableToExcel(tableId, sheetData, worksheetXDoc);
            fileName += 'transport';
            // serialize it to a blob
            var openedFileData = doc.saveToBlob();
            //openedFileData = doc.saveToFlatOpc();

            openXml.util.bucketTimer.bucket("Done");
            var s = openXml.util.bucketTimer["OpenAndSave"];
            //alert("Document modified.  Elapsed time: " + (s.time / 1000).toString() + " seconds");

            saveAs(openedFileData, fileName + '.xlsx');
            /******************************************************************************/
        }
    }

    function tableToExcel(tblId, sheetData, worksheetXDoc) {
        tblId = '#' + tblId;
        //alert(tblId + ", " + $(tblId).find('th').length);
        var eRow = 1;
        var eCol = 1;
        var widths = new Array();
        var tblHeadRows = $(tblId).find('thead tr');
        //alert("num rows in header: " + tblHeadRows.length);
        for (var r = 0; r < tblHeadRows.length; r++) {
            var rowData = $(tblHeadRows[r]).find('th');
            eCol = 1;
            for (var c = 0; c < $(rowData).length; c++) {
                var val = $(rowData[c]).text();
                var colSpan = $(rowData[c]).attr('colspan');
                if (isNaN(colSpan))
                    colSpan = 1;
                setCell(eRow, eCol, sheetData, 0, null, val, HeaderStyle, 1, colSpan);
                eCol += colSpan;
                widths[c] = val.length * 2;
            }
            eRow++;
        }
        /* now the body */
        var tblBodyRows = $(tblId).find('tbody tr');
        //alert("Rows = " + tblBodyRows.length);
        for (var r = 0; r < tblBodyRows.length; r++) {
            var rowData = $(tblBodyRows[r]).find('td');
            eCol = 1;
            for (var c = 0; c < $(rowData).length; c++) {
                var val = $(rowData[c]).text();
                var colSpan = $(rowData[c]).attr('colspan');
                if (isNaN(colSpan))
                    colSpan = 1;
                var style = RowStyle;
                if (r % 2 == 0)
                    style = RowStyleOdd;
                if (val == 'Invalid #')
                    style = WarningStyle;
                setCell(eRow, eCol, sheetData, 0, null, val, style, 1, colSpan);
                eCol += colSpan;
                if (val.length > widths[c])
                    widths[c] = val.length;
            }
            eRow++;
        }
        /* footer */
        var tblFooterRows = $(tblId).find('tfoot tr');
        for (var r = 0; r < tblFooterRows.length; r++) {
            var rowData = $(tblFooterRows[r]).find('td');
            eCol = 1;
            for (var c = 0; c < $(rowData).length; c++) {
                var val = $(rowData[c]).text();
                var colSpan = $(rowData[c]).attr('colspan');
                if (isNaN(colSpan))
                    colSpan = 1;
                if (isNaN(val))
                    setCell(eRow, eCol, sheetData, 0, null, val, FooterStyle, 1, colSpan);
                else
                    setCell(eRow, eCol, sheetData, val, null, null, FooterStyle, 1, colSpan);
                eCol += colSpan;
                if (val.length > widths[c])
                    widths[c] = val.length;
            }
            eRow++;
        }
        setColumnWidths(widths, worksheetXDoc);
        setFreezePane(1, worksheetXDoc);
    }

    function setColumnWidths(widths, worksheetXDoc) {
        /* set column widths */
        var cols = worksheetXDoc.root.element(S.cols);
        cols.removeAll();
        for (var i = 0; i < widths.length; i++) {
            var col = new XElement(S.col);
            col.add(new XAttribute('max', (i + 1)));
            col.add(new XAttribute('min', (i + 1)));
            col.add(new XAttribute('width', (widths[i] * 1.3)));
            col.add(new XAttribute('customWidth', 1));
            cols.add(col);
        }
    }

    function setFreezePane(row, worksheetXDoc) {
        var sheetViews = worksheetXDoc.root.element(S.sheetViews);
        sheetViews.removeAll();

        var sheetView = new XElement(S.sheetView);
        sheetView.add(new XAttribute('tabSelected', '1'));
        sheetView.add(new XAttribute('workbookViewId', '0'));
        var pane = new XElement(S.pane);
        pane.add(new XAttribute('activePane', 'bottomLeft'));
        pane.add(new XAttribute('state', 'frozen'));
        pane.add(new XAttribute('topLeftCell', 'A4'));
        pane.add(new XAttribute('ySplit', row));
        sheetView.add(pane);
        sheetViews.add(sheetView);
    }
    var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    function getRef(c, r) {
        var ref = '';
        c--;
        while (c > 25) {
            ref = alpha[c % 26] + ref;
            c = (c / 26) | 0;
            c--;
        }
        ref = alpha[c] + ref + r;
        return ref;
    }

    function setCell(r, c, sheetData, val, formula, text, style, rowCount, cellCount) {
        //alert(val);
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
}(this));

