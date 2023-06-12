
//Twilio: YmgjZ0sYRV8Uivr0mzEFP4vXfeAFbANZUn807vOb
var ClientBaseURL = "http://localhost:52293/RestServiceWWW/Client/"; //
var RosterBaseURL = "http://localhost:52293/RestServiceWWW/Roster/"; //
ClientBaseURL = "../Client/";
RosterBaseURL = "../Roster/";

var admin = false;
var jsonArray;
var emailList = null;
var activityIndicator = true;
var HeaderStyle = 6;
var RowStyle = 3;
var RowStyleOdd = 1;
var FooterStyle = 5;
var WarningStyle = 2;
var phoneBlastOn = 'off';   /* off, on or initializing */
var exportPool = false;
var exportPoolCount = 0;
var rosters = null;
var offices = new Object;
offices['W'] = 'West Chicago';
offices['H'] = 'Wheeling';
offices['V'] = 'Villa Park';
offices['E'] = 'Elgin';
offices['A'] = 'Addison';
offices['S'] = 'Schaumburg';
offices['G'] = 'Elk Grove';
offices['B'] = 'Bartlett';
offices['R'] = 'Aurora';
offices['O'] = 'Bolingbrook';
offices['C'] = 'Chicago';

window.onbeforeunload = confirmExit;
function confirmExit() {
    if (txtWindow != null) {
        txtWindow.close();
        txtWindow = null;
    }
}

function setOfficeDDL() {
    $("#office").append("<option value='W'>West Chicago</option>" +
    "<option value='E'>Elgin</option>" +
    "<option value='V'>Villa Park</option>" +
    "<option value='H'>Wheeling</option>" +
    "<option value='A'>Addison</option>" +
    "<option value='S'>Schaumburg</option>" +
    "<option value='G'>Elk Grove</option>" +
    "<option value='B'>Bartlett</option>" +
    "<option value='R'>Aurora</option>" +
    "<option value='C'>Chicago</option>" + 
    "<option value='O'>Bolingbrook</option>");
}

function waitIndicator(on, titleBar) {
    $('#waiting').remove();
    if (on == true) {
        if (titleBar == null)
            titleBar = "loading";
        $("<div id='waiting'><h3 style='padding-left:88px'>please wait...</h3><img style='padding-left:88px' src='../Images/ajax-loader.gif'/></div>").dialog(
        { open: function (event, ui) {
            $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
        }, 
        title: titleBar
        });
    }
}

(function (root) {  // root = global
        "use strict";
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
            var fileName = '' + dt.getFullYear() + dt.getMonth() + dt.getDate() + '_';
            /* lets add some data */
            if( tableId == 'callTable' )
            {
                callTableToExcel(tableId, sheetData, worksheetXDoc);
                fileName += 'callTable';
            }
            else
            {
                rosterToExcel(tableId, sheetData, worksheetXDoc);
                fileName += 'rosterTable';
            }
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

    function rosterToExcel(tblId, sheetData, worksheetXDoc, freezePane) {
        tblId = '#' + tblId;
        /* header elements we want.. */
        setCell(1,1,sheetData, 0, null, '#', HeaderStyle, 1, 1);
        setCell(1,2,sheetData, 0, null, 'Office', HeaderStyle, 1, 1);
        setCell(1,3,sheetData, 0, null, 'ID #', HeaderStyle, 1, 1);
        setCell(1,4,sheetData, 0, null, 'Name', HeaderStyle, 1, 1);
        setCell(1,5,sheetData, 0, null, 'Start', HeaderStyle, 1, 1);
        setCell(1,6,sheetData, 0, null, 'End', HeaderStyle, 1, 1);
        var widths = new Array();
        widths[0] = 2;
        widths[1] = 12;
        widths[2] = 8;
        widths[3] = 8;
        widths[4] = 10;
        widths[5] = 6;

        /* get employees for the body */
        var tblBodyRows = $(tblId).find('tbody tr');
        for (var r = 1; r <= tblBodyRows.length; r++) {
            var rowData = $(tblBodyRows[r-1]).find('td');
                
            var style = RowStyle;
            if (r % 2 == 0)
                style = RowStyleOdd;
            /* set row number */
            setCell(r+1, 1, sheetData, r, null, null, style, 1, 1);
            var rowNum =  "" + r;
            if( widths[0] < rowNum.length )
                widths[0] = rowNum.length;

            /* get office ID */
            var officeId = $(rowData[1]).text();
            setCell(r+1, 2, sheetData, 0, null, officeId, style, 1, 1);
            if( officeId.length > widths[1] )
                widths[1] = officeId.length;

            /* get ID # */
            var id = $(rowData[2]).text();
            setCell(r+1, 3, sheetData, id, null, null, style, 1, 1);
            if( id.length > widths[2] )
                widths[2] = id.length;

            /* get name */
            var n = $(rowData[3]).text();
            setCell(r+1, 4, sheetData, 0, null, n, style, 1, 1);
            if( n.length > widths[3] )
                widths[3] = n.length; 

            /* get date */
            var startDate = $(rowData[4]).find('input').val() + ' ';
            var time = $(rowData[7]).find('select');
            startDate += $(time[0]).val() + ':' + $(time[1]).val() + ' ';
            var ampm = $(time[2]).val();
            if( ampm == 0 )
                ampm = 'AM';
            else
                ampm = 'PM';
            startDate += ampm;
            setCell(r+1, 5, sheetData, 0, null, startDate, style, 1, 1); 
            if( startDate.length > widths[4] )
                widths[4] = startDate.length;

            /* get date */
            var endDate = $(rowData[6]).find('input').val() + ' ';
            time = $(rowData[7]).find('select');
            //alert( time.length );
            endDate += $(time[3]).val() + ':' + $(time[4]).val() + ' ';
            ampm = $(time[5]).val();
            if( ampm == 0 )
                ampm = 'AM';
            else
                ampm = 'PM';
            endDate += ampm;
            setCell(r+1, 6, sheetData, 0, null, endDate, style, 1, 1); 
            if( endDate.length > widths[5] )
                widths[5] = endDate.length;
        }
        setColumnWidths(widths, worksheetXDoc);
        setFreezePane(2, worksheetXDoc);
    }

    function callTableToExcel(tblId, sheetData, worksheetXDoc, freezePane) {
        tblId = '#' + tblId;
        var eRow = 1;
        var eCol = 1;
        var widths = new Array();
        var tblHeadRows = $(tblId).find('thead tr');
        /* 2 header rows, only 2nd is needed. */
        for (var r = 1; r <= tblHeadRows.length; r++) {
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
        var tblBodyRows = $(tblId).find('tbody tr[class*="employeeRoot"]');
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
                if( val == 'Invalid #' )
                    style = WarningStyle;
                setCell(eRow, eCol, sheetData, 0, null, val, style, 1, colSpan);
                eCol += colSpan;
                if( val.length > widths[c] )
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
                if( val.length > widths[c] )
                    widths[c] = val.length;
            }
            eRow++;
        }
        setColumnWidths(widths, worksheetXDoc);
        setFreezePane(2, worksheetXDoc);
    }

    function setColumnWidths(widths, worksheetXDoc)
    {
        /* set column widths */
        var cols = worksheetXDoc.root.element(S.cols);
        cols.removeAll();
        for( var i=0; i<widths.length; i++ )
        {
            var col = new XElement(S.col);
            col.add(new XAttribute('max', (i+1)));
            col.add(new XAttribute('min', (i+1)));
            col.add(new XAttribute('width', (widths[i] * 1.3)));
            col.add(new XAttribute('customWidth', 1));
            cols.add(col);
        }
    }

    function setFreezePane( row, worksheetXDoc)
    {
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
} (this) );

















function getDate(dt) {
    return new Date(parseInt(dt.replace("/Date(", "").replace(")/", ""), 10));
}

function formatDate(dt) {
    return "" + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
}

function setHours( hours ) {
    if (hours >= 12) {
        if (hours > 12)
            hours = hours - 12;
        else if (hours.length < 2)
            hours = '0' + hours;
    }
    else if (hours == 0) {
        hours = "12";
    }
    return hours;
}

function dropDownDateTime(timeStr) {
    var hours = setHours(timeStr.substring(0, 2));
    var mins = timeStr.substring(3, 5);

    //alert(hours + ":" + mins);
    var AMPM = 'AM';
    if (timeStr.substring(0, 2) >= 12)
        AMPM = 'PM'

    var tr = "<select class='inpDate'>";
    for (var j = 1; j <= 12; j++) {
        var h = j;
        if (j <= 9)
            h = '0' + j;
        if (h != hours)
            tr += "<option value='" + h + "'>" + h + "</option>";
        else
            tr += "<option selected='selected' value='" + h + "'>" + h + "</option>";
    }
    tr += "</select><select class='inpDate'>";

    for (var j = 0; j <= 55; j += 5) {
        var m = j;
        if (j <= 9)
            m = '0' + j;
        if (m != mins)
            tr += "<option value='" + m + "'>" + m + "</option>";
        else {
            tr += "<option selected='selected' value='" + m + "'>" + m + "</option>";
        }
    }
    tr += "</select><select class='inpDate'>";
    if( AMPM == 'AM' )
        tr += "<option selected='selected' value='0'>AM</option><option value='12'>PM</option><select>";
    else
        tr += "<option value='0'>AM</option><option selected='selected' value='12'>PM</option><select>";

    return tr;
}

function getClientRosters() {
    var client = $("#ddlClientRoster").val();
    if (client == 8)
        $('#subs').show();
    else
        $('#subs').hide();
    var loc = $("#ddlLocationRoster").val();
    var shift = $("#ddlShiftRoster").val();
    var dept = $("#ddlDepartmentRoster").val();
    var stDt = $("#dateRoster").val();
    var url = ClientBaseURL + client + "/RostersWithLocation?date=" + stDt + "&loc=" + loc + "&dept=" + dept + "&shift=" + shift;
    waitIndicator(true, "Getting Rosters");
    $.ajax({
        cache: false,
        type: "GET",
        url: url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            rosters = msg.rosters;
            for (var i = 0; i < rosters.length; i++) {
                var tr = "<tr officeID='T" + rosters[i].Office + "' class='rosterRow' cr_ID='" + rosters[i].RosterID + "'>";

                tr += "<td>" + (i + 1) + ".</td>";
                tr += "<td>" + "T" + rosters[i].Office + " (" + offices[rosters[i].Office] + ")" + "</td>";
                tr += "<td><span class='aident'>" + rosters[i].ID + "</span></td>";
                tr += "<td class='name'>" + rosters[i].LastName + ", " + rosters[i].FirstName + "</td>";
                if (client == 8) {
                    tr += "<td><input type='text' id='sub_" + rosters[i].RosterID + "' value='0' size='6' />";
                    tr += "<input type='hidden' class='idAtClient' value='" + rosters[i].SubID + "' /></td>"
                }

                rosters[i].StartDate = getDate(rosters[i].StartDate);
                rosters[i].EndDate = getDate(rosters[i].EndDate);
                tr += "<td><input class='inpDate startDt' type='text' size='8' value='" + formatDate(rosters[i].StartDate) + "'/></td>";
                tr += "<td><span>thru</span></td>";
                tr += "<td><input class='inpDate endDt' type='text' size='8' value='" + formatDate(rosters[i].EndDate) + "'/></td>";
                ///get hours
                tr += "<td>S: " + dropDownDateTime(rosters[i].ShiftStart) + "<br/>E: ";
                tr += dropDownDateTime(rosters[i].ShiftEnd) + "</td>";
                tr += "<td><input class='smallButton' type='button' value='REM' /></td>";
                tr += "</tr>";
                $("#clientRoster").append(tr);
                $("#clientRoster").closest('div').show('slow');

                $("tr[cr_ID='" + rosters[i].RosterID + "']").find('.inpDate').change(function () {
                    $('#updRosters').removeAttr('disabled').click(function () {
                        updateRosters();
                    });
                    $(this).css('background-color', '#FFCCBB');
                    $(this).closest('tr').attr('update', 'true');
                });
            }
            var $inps = $("input[class*='inpDate']");
            $inps.datepicker();
            $("#clientRoster tr:odd").addClass("odd");
            $('.rosterRow').each(function (index) {
                $(this).hover(
                function () {
                    $(this).addClass("highLight");
                }, function () {
                    $(this).removeClass("highLight");
                });
            });
            waitIndicator(false);
        },
        error: function (msg) {
            waitIndicator(false);
            //$("#loading").remove();
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
}

function getDateFromString(dt) {
    var firstSlash = dt.indexOf("/");
    var secSlash = dt.indexOf("/", firstSlash + 1);
    dt = new Date(dt.substring(secSlash + 1), dt.substring(0, firstSlash) - 1, dt.substring(firstSlash + 1, secSlash));
    return dt;
}

function dateToString(dt) {
    var month;
    var year;
    var date;
    month = dt.getMonth()+1;
    if (month <= 9)
        month = '0' + month;
    year = dt.getFullYear();
    date = dt.getDate();
    if (date <= 9)
        date = '0' + date;
    return month + '/' + date + '/' + year;
};

/*
	@aident varchar(12) = '00000',
	@employeeId int = 0,
	@notes varchar(256) = '',
	@addedBy varchar(12) = 'franco'
*/

function updateEmployeeNotes($this) {
    var notes = $this.closest("div").find("textarea").val();
    var aidentNumber = $this.closest("div").find("#id").text();
    var userId = $("input[id*='userID']").val();

    var Url = RosterBaseURL + "PBUpdate?aident=" + aidentNumber + "&notes=" + notes + "&userId=" + userId;

    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (data) {
            $this.closest("div").find("textarea").css("background-color", "#FFF");
            jsonArray.RecruitPoolCollection[callListIdx].Notes = notes;
        },
        error: function (data) {
            alert("ERROR!!!" + data.responseText);
        }
    });
}

var txtWindow;
$(document).ready(function () {
    admin = false;
    var id = $('input[id*="userID"]').val();
    if (id == "lora" || id == 'imolina' || id == 'lisa' || id == 'itdept' ||
            id == 'garciac' || id == 'mchavez' || id == 'virginia' || id == 'gomezs' ||
            id == 'glaserm' || id == 'cmartinez' || id == 'badanis' || id == 'magdalenoy' ||
        id == 'ferrerm' || id == 'garciali' || id == 'quichimbol' || id == 'chavezz' || id == 'kocampo')
        admin = true;
    var dt = new Date();
    $('#endDate,#dateRoster').val(dateToString(dt));
    dt.setDate(dt.getDate() - 5);
    $("#startDate").val(dateToString(dt));
    refresh(4, $("#ddlClient").closest("tr"), true);
    refresh(4, $("#ddlClientRoster").closest("tr"), false);

    $("#callListDiv").find("#call").click(function (event) {
        alert("Here we go!");
        event.stopPropagation();
        directCall(event, $(this));
    });

    $("#startDate").datepicker();
    $("#endDate").datepicker();
    $("#dateRoster").datepicker();

    $("#btnSubmitCustom").click(function () {
        recruitCustomList();
        updateStatuses();
    });

    $("#btnSubmitSkill").click(function () {
        recruitSkillList();
        updateStatuses();
    });

    $("#btnPoolClear").click(function () {
        $("#callTable tbody tr").remove();
    });

    $("#btnCreateText").click(function () {
        if (txtWindow != null)
            alert(txtWindow.document.getElementById("respNum").value);
        else {
            txtWindow = window.open('', 'Create Text', 'height=380, width=380');
            var d = txtWindow.document.getElementById("main");
            if (d == null) {
                d = txtWindow.document;
                d.write("<body style='background-color:#FFA'>");
                d.write("<div id='main' width='300px' height='200px'>");
                d.write("<table>");
                d.write("<tr><th colspan='3'><h3>Create Text Message</h3></th>");
                d.write("<tr><td colspan='3'><h3>use '$name' to add employee's name</h3></td></tr>");
                d.write("<tr><td colspan='3'><textarea id='msg' rows='10' cols='40'></textarea></td></tr>");
                d.write("<tr><td><span>Forward Return TEXTS To:</span></td><td title='If employee sends a text in response, it will be forwarded to this number entered.' colspan='2'><input id='returnTextPhoneNum'></input></td></tr>");
                d.write("<tr><td><span>Forward Return CALLS To:</span></td><td title='If employee calls in response, it will be forwarded to this number entered.' colspan='2'><input id='returnCallPhoneNum'></input></td></tr>");
                d.write("<tr><td colspan='2'><input id='auto' type='button' value='Auto Generate Text'></input></td><td><input id='spanish' type='checkbox'>Spanish</input></td></tr>");
                d.write("</table>");
                d.write("</div>");
                d.write("</body>");
                txtWindow.onbeforeunload = function (event) {
                    txtWindow = null;
                }
            };
            txtWindow.document.getElementById('auto').onclick = function (e) {
                if (txtWindow.document.getElementById('spanish').checked) {
                    txtWindow.document.getElementById('msg').value = "Hola $name,\nHay puestos de trabajo disponibles en la actualidad.\nPor favor llame a la oficina de despacho MSI.\nGracias!";
                }
                else {
                    txtWindow.document.getElementById('msg').value = "Hello $name,\nThere are jobs available today.\nPlease call the MSI dispatch office.\nThank you!";
                }
            }
        }
    });

    $("#callTable thead tr th").click(function () {
        //sortTable($(this).find('span').attr('class'));
    });

    $("#expRosters").click(function () {
        exportToXlsx('clientRoster');
    });

    $("#expCallData").click(function () {
        exportToXlsx('callTable');
    });

    $("#btnSubmit").click(function () {
        getRecruitPool();
        updateStatuses();
    });
    $("#btnSubmitID").click(function () {
        recruitIndividual();
        updateStatuses();
    });

    $("#btnSubmitName").click(function () {
        recruitIndividualName();
        updateStatuses();
    });

    $("#mainCall").click(function () {
        phoneBlast();
    });

    $("#btnGetRoster").click(function () {
        $('#clientRoster tbody').find('tr').remove();
        getClientRosters();

        $('#clientRoster tbody').find('.smallButton').click(function () {
            deleteLine($(this).closest('tr').attr("cr_id"));
            //removeFromRoster($(this).closest('tr'));
        });
    });

    /* on change functions */
    $('#ddlDepartmentRoster').change(function () {
        $('#clientRoster').find('tbody').find('tr').remove();
        refresh(0, $(this).closest('tr'), false);
    });

    $('#ddlShift').change(function () {
        /* set dept accordingly */
        refresh(1, $(this).closest('tr'), true);
    });

    $('#ddlShiftRoster').change(function () {
        $('#clientRoster').find('tbody').find('tr').remove();
        /* set dept accordingly */
        refresh(1, $(this).closest('tr'), false);
    });

    $('#ddlLocation').change(function () {
        /* set shift accordingly */
        refresh(2, $(this).closest('tr'), true);
    });

    $('#ddlLocationRoster').change(function () {
        $('#clientRoster').find('tbody').find('tr').remove();
        /* set shift accordingly */
        refresh(2, $(this).closest('tr'), false);
    });

    $('#callList').click(function () {
        if ($("#office").val() == 0) {
            alert("Please first select a dispatch office!");
            return;
        }
        var idx = $(this).closest('tr').find('span[class="index"]').text();
        callList(0);
    });

    $('#ddlClient').change(function () {
        /* set locations accordingly */
        refresh(3, $(this).closest('tr'), true);
    });
    $('#ddlClientRoster').change(function () {
        $('#clientRoster').find('tbody').find('tr').remove();
        /* set locations accordingly */
        refresh(3, $(this).closest('tr'), false);
        var client = $('#ddlClientRoster').val();
        if (client == 8)
            $('#subs').show();
        else
            $('#subs').hide();
        getEmailList(client);
    });

    $('.titleBar').next('div').hide();
    $('.titleBar').hover(
        function () {
            $(this).addClass("titleBarHighlight");
            $(this).removeClass("titleBarNormal");
        }, function () {
            $(this).removeClass("titleBarHighlight");
            $(this).addClass("titleBarNormal");
        }
    );
    $('.titleBar').click(
        function () {
            $(this).next('div').slideToggle('slow');
        }
    );
    getCustomLists();
    getSkillLists();
    setOfficeDDL();

    /* set up TWILIO device */

    Twilio.Device.setup($("input[id*='capToken']").val(), { rtc: true, debug: true });

    //Twilio.Device.ready(function (device) {
        // The device is now ready
        $('#browserPhone').closest('h3').removeClass("browserPhoneUnavailable");
    $('#browserPhone').closest('h3').addClass("browserPhoneAvailable");
    //now ready for making connections
        $('#browserPhone').text("Browser phone is temporarily disabled");
        $('.titleBar').each(function () {
            $(this).next('div').slideDown("slow");
        });
    //});

    Twilio.Device.error(function (error) {
    
        $('#browserPhone').text(error.message);
        alert("Browser Phone disconnecting! " + error.message);
        $('#browserPhone').closest('h3').addClass("browserPhoneUnavailable");
        $('#browserPhone').closest('h3').removeClass("browserPhoneAvailable");
    
    });

    Twilio.Device.incoming(function (conn) {
        if (confirm("Incoming connection from " + conn.parameters.From + " (cancel to ignore)") == true)
        // accept the incoming connection and start two-way audio
            conn.accept();
        else
            conn.ignore();
    });

    Twilio.Device.disconnect(function (conn) {
        //$('.directCall').val("CALL").removeAttr('disabled').css("color", "black");
    });
});


function updateEmployeeInfo(tRow) {
    var index = $(tRow).find(".rowIdx").val();
    var fName = $(tRow).find(".firstName").val();
    if( fName == '' )
        fName = $(tRow).find(".firstName").text();
    var lName = $(tRow).find(".lastName").val();
    if (lName == '')
        lName = $(tRow).find(".lastName").text();
    var addr1 = $(tRow).find(".addr1").val();
    var addr2 = $(tRow).find(".addr2").val();
    var city = $(tRow).find(".city").val();
    var state = $(tRow).find(".state").val();
    var zip = $(tRow).find(".zip").val();
    var areaCode = $.trim( $(tRow).find(".areaCode").val() );
    var prefix = $.trim( $(tRow).find(".prefix").val() );
    var last4 = $.trim( $(tRow).find(".last4").val() );
    var id = $(tRow).find(".aidentNumber").val();
    var email = $(tRow).find(".email").val();
    var Url = RosterBaseURL + "EmployeeInfo/" + id + "/Set?firstName=" + fName + "&lastName=" + lName + 
                "&addr1=" + addr1 + "&addr2=" + addr2 + "&city=" + city +
                "&state=" + state + "&zip=" + zip + "&zip4=0000&phoneAreaCode=" + areaCode + 
                "&phonePrefix=" + prefix + "&phoneLast4=" + last4 + "&update=true&updatedBy=itdept" + "&email=" + email;
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (data) {
            $(tRow).find('input[update="true"]').each(function () {
                $(this).removeAttr('style');
            });
            jsonArray.RecruitPoolCollection[index].Addr = $(tRow).find(".addr1").val();
            jsonArray.RecruitPoolCollection[index].Addr2 = $(tRow).find(".addr2").val();
            jsonArray.RecruitPoolCollection[index].City = $(tRow).find(".city").val();
            jsonArray.RecruitPoolCollection[index].State = $(tRow).find(".state").val();
            jsonArray.RecruitPoolCollection[index].FirstName = fName;
            jsonArray.RecruitPoolCollection[index].LastName = lName;
            jsonArray.RecruitPoolCollection[index].PhoneNum = '(' + $.trim($(tRow).find(".areaCode").val()) + ')' + $.trim($(tRow).find(".prefix").val())
                                                                + '-' + $.trim($(tRow).find(".last4").val());
            jsonArray.RecruitPoolCollection[index].Zip = $(tRow).find(".zip").val();
            jsonArray.RecruitPoolCollection[index].Email = $(tRow).find(".email").val();
            $(tRow).closest('table').find(".btnUpdate").attr("disabled", "disabled");
            $(tRow).closest('table').find(".btnCancel").val("CLOSE");

            var tr = $(tRow).closest('tr').prev('tr');  /* get root employee row... */
            $(tr).find('.name').text(jsonArray.RecruitPoolCollection[index].LastName + ', ' + jsonArray.RecruitPoolCollection[index].FirstName);
            $(tr).find('.email').text(jsonArray.RecruitPoolCollection[index].Email);

            var t = "<span class='phoneNum'>" + jsonArray.RecruitPoolCollection[index].PhoneNum + "</span>";
            $(tr).find('.phoneNum').closest('td').html(t);

            $(tr).find('input').off();
            var idx = index;

            $(tr).find('.directCall').click(function (event) {
                event.stopPropagation();
                /* is Dispatch office selected? */
                if ($('#office').val() == 0) {
                    alert("No Dispatch Office Selected!");
                    return;
                }
                callList(idx);
            });


            if (phoneNumberIsValid(jsonArray.RecruitPoolCollection[index].PhoneNum)) {
                $(tr).find('.callStatus').text("waiting");
                //if ($(tr).find('.singlePhoneBlast').closest('td').find('.directCall').length == 0) {
                //    $(tr).find('.singlePhoneBlast').closest('td').append($("<input type='button' class='directCall smallButton' value='CALL'/>"));
                //} 
                if ($(tr).find('.singlePhoneBlast').closest('td').find('.directText').length == 0) {
                    $(tr).find('.singlePhoneBlast').closest('td').append($("<input type='button' class='directText smallButton' value='TEXT'/>"));
                }
                $(tr).removeClass("employeeRootBadPhoneNumber");
                $(tr).addClass("employeeRoot");
            }
            else {
                $(tr).removeClass("employeeRoot");
                $(tr).addClass("employeeRootBadPhoneNumber");
            }
            $(tr).find('.directCall').click(function (event) {
                event.stopPropagation();
                /* is Dispatch office selected? */
                if ($('#office').val() == 0) {
                    alert("No Dispatch Office Selected!");
                    return;
                }
                callList(idx);
            });
            $(tr).find('.directText').click(function (event) {
                event.stopPropagation();
                directText(event, $(this));
            });

        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function updateInfoField(inp) {
    $(inp).css("background-color", "#FFBBBB");
    $(inp).attr("update", "true");
    $(inp).closest('table').find(".btnUpdate").removeAttr("disabled");
}

function autoTab(field, length) {
    if ($(field).val().length == length) {
        //alert($(field).next('input').val());
        var next = $(field).next('input');
        if ($(next).length == 0)
            next = $(field).closest('tr').next('tr').find('input');
        next.focus().select();
    }
}
function employeeInfo(tRow, i) {
    /* internal information - a table embedded in a tr/td */
    if ($(tRow).next("tr").find("table").length > 0) {   /* already exists */
        $(tRow).next("tr").remove();
        return null;
    }
    else {
        var tbl = "<tr><td colspan='" + 9 + "'>";
        tbl += "<table cellpadding='6' cellspacing='0' border='1'><tbody>";
        tbl += "<tr class='employeeInfo'><td colspan='2'><span>First Name:</span></td><td colspan='4'><input class='rowIdx' type='hidden' value='" + i + 
                "'/><input class='aidentNumber' type='hidden' value='" + jsonArray.RecruitPoolCollection[i].BadgeNumber + "'/>";
        if (admin) {
            tbl += "<input class='firstName' value='" +
                        jsonArray.RecruitPoolCollection[i].FirstName + "'/></td><td colspan='2' rowspan='2'>" +
                        "<input class='btnUpdate' type='button' disabled='true' value='UPDATE'/></td><td colspan='4' rowspan='2'><span>GMP‌ Date:‌ </span><input type='text' val='2014-04-10'></td></tr>";
            tbl += "<tr class='employeeInfo'><td colspan='2'><span>Last Name:</span></td><td colspan='4'><input class='lastName' value='" +
                        jsonArray.RecruitPoolCollection[i].LastName + "'/></td></tr>";
        }
        else {
            tbl += "<span class='firstName'>" +
                        jsonArray.RecruitPoolCollection[i].FirstName + "</span></td><td colspan='2' rowspan='2'>" +
                        "<input class='btnUpdate' type='button' disabled='true' value='UPDATE'/></td><td colspan='4' rowspan='2'><span>GMP‌ Date:‌ </span><input type='text' val='2014-04-10'></td></tr>";
            tbl += "<tr class='employeeInfo'><td colspan='2'><span>Last Name:</span></td><td colspan='4'><span class='lastName'>" +
                        jsonArray.RecruitPoolCollection[i].LastName + "</span></td></tr>";
        }
        tbl += "<tr class='employeeInfo'><td colspan='2'><span>Email:</span></td><td colspan='4'><input class='email' type='text' value='" + jsonArray.RecruitPoolCollection[i].Email + "'/></td><td colspan='2' rowspan='2'>" + 
                "<input type='button' class='btnCancel' value='CANCEL'/></td></tr>";
        if (jsonArray.RecruitPoolCollection[i].PhoneNum == null) {
            tbl += "<tr class='employeeInfo'><td colspan='2'><span>Phone Num</span></td><td colspan='4'>(<input class='areaCode' type='text'/>)" +
                "<input class='prefix' size='2' type='text'/>-<input size='3' class='last4' type='text'/></td></tr>";
        }
        else {
            var p = "";
            for (var j = 0; j < jsonArray.RecruitPoolCollection[i].PhoneNum.length; j++) {
                if (jsonArray.RecruitPoolCollection[i].PhoneNum.charAt(j) >= '0' && jsonArray.RecruitPoolCollection[i].PhoneNum.charAt(j) <= '9')
                    p += jsonArray.RecruitPoolCollection[i].PhoneNum.charAt(j);
            }
            jsonArray.RecruitPoolCollection[i].PhoneNum = p;
            //alert(jsonArray.RecruitPoolCollection[i].PhoneNum);
            tbl += "<tr class='employeeInfo'><td colspan='2'><span>Phone Num</span></td><td colspan='4'>(<input onClick='this.select();' onKeyup='autoTab(this,3);' size='2' class='areaCode' size='2' value='" +
                jsonArray.RecruitPoolCollection[i].PhoneNum.substring(0, 3) + "' type='text'/>)" +
                "<input onClick='this.select();' onKeyup='autoTab(this,3);' size='2' class='prefix' value='" + jsonArray.RecruitPoolCollection[i].PhoneNum.substring(3, 6) +
                "' type='text'/>-<input onClick='this.select();' onKeyup='autoTab(this,4);' class='last4' value='" + jsonArray.RecruitPoolCollection[i].PhoneNum.substring(6) + "' size='3' type='text'/></td></tr>";
        }
        tbl += "<tr class='employeeInfo'><td colspan='2'><span>Address:</span></td><td colspan='6'><input class='addr1' type='text' value='" +
                        jsonArray.RecruitPoolCollection[i].Addr + "'/></td></tr>";
        tbl += "<tr class='employeeInfo'><td colspan='2'><span>Address:</span></td><td colspan='6'><input class='addr2' type='text' value='" +
                        jsonArray.RecruitPoolCollection[i].Addr2 + "'/></td></tr>";
        tbl += "<tr class='employeeInfo'><td colspan='2'><span>City:</span></td><td colspan='3'><input class='city' type='text' value='" + 
                        jsonArray.RecruitPoolCollection[i].City + "'/></td><td colspan='2'>" +
                            "<span>State:</span></td><td colspan='3'><input class='state' size='3' type='text' value='" +
                        jsonArray.RecruitPoolCollection[i].State + "'/></td>" + 
                        "<td><span>Zip:</span></td><td><input class='zip' size='4' type='text' value='" + jsonArray.RecruitPoolCollection[i].Zip + "'/></td></tr>";
        tbl += "</tbody></table>";
        tbl += "</td></tr>";
        return tbl;
    }
}
function addToRosterCallList($dialog) {
    //alert("Add to Roster Call List! " + $cl.length);
    var id = $dialog.find('h3[id="id"]').text();
    var startDate = $('#dateRoster').val();
    var endDate = getDateFromString(startDate);
    var days = /*$('#rosterCount').val() * */$('#rosterUnits').val();
    endDate.setDate(endDate.getDate() + Number(days));
    endDate = formatDate(endDate);
    if ($('#trackingStart').text() < $('#trackingEnd').text()) {
        days--;
    }
    if (!conflicts(id, startDate, endDate, null, $dialog)) { //removes click event for name sample
        addToRosterAction(id, startDate, endDate, null, $dialog);
        // now that we are added to roster, set the date/time //
        // find row with id..JHMJHM
        var row = $("#clientRoster").find("span:contains('" + id + "')").closest("tr");
        $(row).find("select[class='inpDate']").closest("td").html(
            $dialog.find("tr[id='shiftStartEnd']").find('td').html()
        );
        var src = $dialog.find("tr[id='shiftStartEnd']").find('td').find('select[class="inpDate"] :selected');
        var dst = $(row).find("select");
        //alert($(dst.get(0)).val() + ", " + $(src.get(0)).val());
        $(dst.get(0)).val($(src.get(0)).val());
        $(dst.get(1)).val($(src.get(1)).val());
        $(dst.get(2)).val($(src.get(2)).val());
        $(dst.get(3)).val($(src.get(3)).val());
        $(dst.get(4)).val($(src.get(4)).val());
        $(dst.get(5)).val($(src.get(5)).val());
        $(row).attr("update", "true");
        updateLine($(row));
    }
}

function addToRoster($tr) {
    /* get start and end times */
    var startDate = $('#dateRoster').val();
    
    /* calculate endDate based on radio buttons and drop down list value */
    var endDate = getDateFromString(startDate);
    var days = /*$('#rosterCount').val() * */ $('#rosterUnits').val();

    if ( $('#trackingStart').text() < $('#trackingEnd').text())
    {
        //alert( "tracking = " + $('#trackingStart').text() );
        days--;
    }
    //var d = endDate.addDays(days);
    endDate.setDate(endDate.getDate() + Number(days));
    endDate = formatDate(endDate);

    var id = $tr.find(".aident").text();

    if (!conflicts(id, startDate, endDate, $tr)) {
        addToRosterAction(id, startDate, endDate, $tr);
    }
}

function directCall2($this) {
    var $dialog = $this.closest('div');
    if ($this.val() == "CALL") {
        var num = '(' + $dialog.find('#area').val() + ') ' + $dialog.find('#prefix').val() + '-' + $dialog.find('#last4').val();
        params = { "PhoneNumber": num };
        Twilio.Device.connect(params);
        $this.val("Hang Up");
        $this.css("color", "red");
        $dialog.find("#next").attr("disabled", "disabled");
        $dialog.find("#prev").attr("disabled", "disabled");
    }
    else {
        Twilio.Device.disconnectAll();
        $this.val("CALL").removeAttr('disabled').css("color", "black");
        $dialog.find("#next").removeAttr("disabled");
        $dialog.find("#prev").removeAttr("disabled");
    }
}

function directCall(event, $this) {
    if (event == null)
        return directCall2($this);
    event.stopPropagation();
    if ($this.val() == "CALL") {
        $('.directCall').attr('disabled', 'disabled');

        $this.removeAttr('disabled');
        var num = $this.closest('tr').find('.phoneNum').text();
        params = { "PhoneNumber": num };
        Twilio.Device.connect(params);
        $this.val("Hang Up");
        $this.css("color", "red");
    }
    else {
        Twilio.Device.disconnectAll();
        $this.val("Call");
        $this.css("color", "red");
    }
}
function directTextConfirm(event, $this) {
    //alert( $this.find("#name").text());
    var body = $this.find("#name").text() + "\n";
    body += "You are scheduled for " + $this.find("#shiftInfo").text() + " " + $this.find("#deptInfo").text() + "\n";
    body += $this.find("#workDate").text() + " at \n";
    body += $this.find("#clientName").text() + "\n";
    var shift = $this.find(".inpDate :selected");
    body += "Start: " + $(shift.get(0)).text() + ":" + $(shift.get(1)).text() + " " + $(shift.get(2)).text() + "\n";
    body += "End: " + $(shift.get(3)).text() + ":" + $(shift.get(4)).text() + " " + $(shift.get(5)).text() + "\n";
    body += "Thank you!";

    var newBody = "";
    for (var i = 0; i < body.length; i++) {
        if (i + 1 < body.length && body.charAt(i) == '?' && body.charAt(i + 1) == '?') {
            i++;
        }
        else if (body.charAt(i) == '\n') {
            newBody += "%0a";
        }
        else if (body.charAt(i) == '&') {
            if (i + 1 < body.length && body.charAt(i + 1) == ' ') {
                newBody += "%26 ";
                i++;
            }
            else
                newBody += "+";
        }
        else if (body.charAt(i) == '$') {
            if (body.substr(i, 5) == "$name") {
                var name = $this.closest("tr").find(".name").text();
                alert(name);
                name = name.substr(name.indexOf(',') + 2).trim().toLowerCase();
                name = name.charAt(0).toUpperCase() + name.slice(1).toLowerCase();
                newBody += name;
                i = i + 4;
            }
        }
        else {
            newBody += body.charAt(i);
        }
    }
    var to = $this.find("#area").val() + $this.find("#prefix").val() + $this.find("#last4").val();
    var from = null;
    Url = "../Roster/PBSendSMS?to=" + to + "&msgTxt=" + newBody + "&from=" + from;
    //alert(Url);
    
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $this.find("#text").css("background-color", "#FFA");
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
    $this.find("#text").css("background-color", "#FAA");
}

function directText(event, $this, to) {
    if (txtWindow == null)
        alert("No text to send.  Create a text by clicking the Create Text button above.");
    else {
        var body = txtWindow.document.getElementById("msg").value;
        body = body.replace(/[^À-ú\$\w\s\!\.\(\)\-\'\/\:\?\,]/gi, '');
        if (typeof(to) == 'undefined') {
            to = $this.closest("tr").find(".phoneNum").text();
        }
        var fromText = txtWindow.document.getElementById("returnTextPhoneNum").value;
        if (fromText.length < 10) {
            alert("The Return Text's phone number is invalid!  Area code and number required!");
            return;
        }
        var fromCall = txtWindow.document.getElementById("returnCallPhoneNum").value;
        if (fromCall.length < 10) {
        //    alert("The Return Call's phone number is invalid!  Area code and number required!");
        //    return;
            fromCall = fromText;
        }
        Url = "../Roster/PBSendSMS?to=" + to + "&msgTxt=" + body + "&fromText=" + fromText + "&fromCall=" + fromCall;
        var newUrl = "";

        for (var i = 0; i < Url.length; i++) {
            if (i + 1 < Url.length && Url.charAt(i) == '?' && Url.charAt(i + 1) == '?') {
                i++;
            }
            else if (Url.charAt(i) == '\n') {
                newUrl += "%0a";
            }
            else if (Url.charAt(i) == '$') {
                if (Url.substr(i, 5) == "$name") {
                    var name = $this.closest("tr").find(".name").text();
                    name = name.substr(name.indexOf(',') + 2).trim().toLowerCase();
                    name = name.charAt(0).toUpperCase() + name.slice(1).toLowerCase();
                    newUrl += name;
                    i = i + 4;
                }
                else {
                    newUrl += Url.charAt(i);
                }
            }
            else {
                newUrl += Url.charAt(i);
            }
        }
        $.ajax({
            cache: false,
            type: "GET",
            url: newUrl,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $this.css("background-color", "#FFA");
            },
            error: function (msg) {
                alert("an unexpected error has occurred! " + msg.d);
            }
        });

        $this.css("background-color", "#FAA");
    }
}

function recruitIndividualName() {
    var last = $("#inpLastName").val();
    var first = $("#inpFirstName").val();
    Url = "../Roster/PBName/" + last + "?firstName=" + first;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //alert(msg.RecruitPoolCollection.length);
            setEmployees(msg);

        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
}
function recruitCustomList() {
    waitIndicator(true, "Retrieving Custom List");
    var id = $("#ddlCustomList").val();
    Url = "../Roster/PhoneBlastList/" + id;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //alert(msg.RecruitPoolCollection.length);
            setEmployees(msg);
            waitIndicator(false);
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
            waitIndicator(false);
        }
    });
}

function recruitSkillList() {
    waitIndicator(true, "Retrieving Skill List");
    var id = $("#ddlSkillList").val();
    Url = "../Roster/SkillList/" + id;
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //alert(msg.RecruitPoolCollection.length);
            setEmployees(msg, false);
            waitIndicator(false);
        },
        error: function (msg) {
            waitIndicator(false);
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
}

function recruitIndividual() {
    id = $("#inpID").val();
    Url = "../Roster/PBId/" + id;
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            setEmployees(msg);
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
        }
    });
}


function updateCustomList($div, fn) {
    var listIdx = $div.find("#customLists").val();
    var aident = $div.find("#aident").text();
    $div.find("#status").text("Updating...");
    var action = -1;
    //alert(listIdx + ", " + aident);
    if (fn == "rem")
        action = 2;
    if (fn == "add")
        action = 1;

    Url = "../Roster/PBListUpdate/" + aident + "?customListID=" + listIdx + "&action=" + action + "&userID=" + $('input[id$="userID"]').val();
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            lastCustomList = $("#customLists").val();
            $div.remove();
        },
        error: function (msg) {
            alert("an unexpected error has occurred! " + msg.d);
            $div.remove();
        }
    });    
}

var customList = "<div class='customListDialog'>" +
    "<table><tbody>" +
    "<tr><td><h2 id='aident'></h2></td><td><h2 id='empName'>Murfey, Jonathan</h2></td><td><input type='button' class='addCustom' value='ADD'/></td></tr>" +
    "<tr><td colspan='2'><select id='customLists'></select></td><td><input type='button' class='remCustom' value='REM'/></td></tr>" +
    "<tr><td id='status' colspan='2'></td><td><input type='button' class='exitCustom' value='EXIT'/></td></tr>" + 
    "</tbody></table>"                    
    "</div>";

var lastCustomList = -1;
function customListDialog($tr) {
    $(customList).dialog({
        title: "CUSTOM ROSTER",
        beforeClose: function () {
            lastCustomList = $("#customLists").val();
        },
        close: function () {
            lastCustomList = $("#customLists").val();
            $('.customListDialog').remove();
        },
        open: function () {
            $('.customListDialog').find('#empName').html($tr.find('.name').text());
            $('.customListDialog').find('#aident').html($tr.find('.aident').text());
            var $options = $('#ddlCustomList').find('option');
            $options.each(function () {
                var val = $(this).val();
                var txt = $(this).text();
                var optAdd = "<option value='" + val + "'>" + txt + "</option>";
                $("#customLists").append(optAdd);
            });
            //$("#customLists").append("<option value='100'>Hello!</option>");
            $('.customListDialog').find('.addCustom').click(function () {
                updateCustomList($(this).closest('div'), "add");
            });
            $('.customListDialog').find('.remCustom').click(function () {
                updateCustomList($(this).closest('div'), "rem");
            });
            $('.customListDialog').find('.exitCustom').click(function () {
                $('.customListDialog').dialog("close");
            });
            if (lastCustomList > 0) {
                $("#customLists").val(lastCustomList);
            }
        }
    });
}

function setCallTable() {

    $("#callTable tr:odd").addClass("odd");

    $('#callTable select').click(function (event) {
        event.stopPropagation();
    });

    $('.rosterAdd').click(function (event) {
        event.stopPropagation();
        addToRoster($(this).closest('tr'));
    });

    $('.customAdd').click(function (event) {
        event.stopPropagation();
        if ($('.customListDialog').length == 0)
            customListDialog($(this).closest('tr'));
    });

    $('.directCall').click(function (event) {
        event.stopPropagation();
        if ($("#office").val() == 0) {
            alert("No Dispatch office selected!");
            return;
        }
        var idx = $(this).closest('tr').find('span[class="index"]').text();
        idx = parseInt(idx, 10) - 1;
        callList(idx);
    });

    $('.directText').click(function (event) {
        event.stopPropagation();
        directText(event, $(this));
    });

    $('.singlePhoneBlast').click(function (event) {
        event.stopPropagation();        
        makeSingleCall($(this).closest('tr'));
    });

    $('.employeeRoot').each(function (index) {
        $(this).hover(
                function () {
                    //var myClass = $(this).attr("class");
                    $(this).addClass("highLight");
                }, function () {
                    $(this).removeClass("highLight");
                });

        if (!phoneNumberIsValid($(this).find(".phoneNum").text())) {
            $(this).removeClass("employeeRoot").addClass("employeeRootBadPhoneNumber");
            $(this).find('.callStatus').text('Invalid #');
            $(this).find('.directCall').attr('disabled', 'disabled');
            $(this).find('.directText').attr('disabled', 'disabled');
        }


        $(this).click(function () {
            var tbl = employeeInfo($(this), index);
            if (tbl != null) {
                $(tbl).insertAfter($(this));
                tbl = $(this).next('tr');
                var inps = $(this).next('tr').find('table tr td input');
                $(inps).on("change", function () {
                    updateInfoField($(this));
                });
                $(this).next('tr').find('.areaCode').focus().select();

                inps = $(this).next('tr').find('.btnCancel');
                $(inps).on("click", function () {
                    $(tbl).remove();
                });

                inps = $(this).next('tr').find('.btnUpdate');
                $(inps).on("click", function () {
                    updateEmployeeInfo(tbl);
                });

                inps = $(this).next('tr').find('.btnCall');
                $(inps).click(function () {
                    var p = '1' + $(inps).closest('table').find('.areaCode').val() +
                                            $(inps).closest('table').find('.prefix').val() +
                                                $(inps).closest('table').find('.last4').val();
                    //alert("Initiating call to: " + p);
                    params = { "PhoneNumber": p };
                    Twilio.Device.connect(params);
                });
            }
        });
    });
    $("#callTable").closest('div').show('slow');
}
function getRecruitPool() {
    var Url = RosterBaseURL + "RecruitPool?clientID=" + $("#ddlClient").val() + "&locationID=" + $("#ddlLocation").val()
        + "&departmentID=" + $("#ddlDepartment").val() + "&shiftType=" + $("#ddlShift").val() + "&startDate=" + $("#startDate").val() 
        + "&endDate=" + $("#endDate").val() + "&dnrClient=" + $("#ddlClientRoster").val()
        ;
    waitIndicator(true, "Getting Employees");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            //alert("" + jsonData.length + " records retrieved");
            setEmployees(jsonData);
            waitIndicator(false);
        },
        error: function (xhr) {
            alert("ERROR!!!" + xhr.responseText);
            waitIndicator(false);
        }
    });
}
function setEmployees(data, clear) {
    if( clear != true )
        $('#callTable tbody tr').remove();
    jsonArray = data;
    if (jsonArray.RecruitPoolCollection.length > 0) {
        /* fill out shift drop down */
        for (var i = 0; i < jsonArray.RecruitPoolCollection.length; i++) {
            var tRow = employeeRow(i, jsonArray);
            tRow = $(tRow);
            $("#callTable tbody").append(tRow);
        }
        setCallTable();
    }
    else {
        $("#callTable tbody").append($("<tr><td colspan='8'><span>No Employees Found!</span></td></tr>"));
    }
}

function employeeRow(i, data) {
    var tRow = "<tr class='employeeRoot' phoneBlast='notCalled'>";
    tRow += "<td><span class='index'>" + (i + 1) + "." + "</span></td>";
    tRow += "<td><span class='aident'>" + data.RecruitPoolCollection[i].BadgeNumber + "</span></td>";
    var dnr = "";
    tRow += "<td><span class='name";
    if (data.RecruitPoolCollection[i].DnrReason != "active") {
        dnr = " (DNR)";
        tRow += " pbDnr";
    }
    tRow += "'>" + data.RecruitPoolCollection[i].LastName + ", " + data.RecruitPoolCollection[i].FirstName +
                 dnr + "</span></td>";
                  
    tRow += "<td><span class='phoneNum'>" + data.RecruitPoolCollection[i].PhoneNum + "</span>";
    tRow += "</td>";

    var daysCount = 0;
    if (data.RecruitPoolCollection[i].DaysWorked.length > 1) {
        tRow += "<td><select>";
        for (var j = 0; j < data.RecruitPoolCollection[i].DaysWorked.length; j++) {
            tRow += "<option>" + (data.RecruitPoolCollection[i].Depts[j]) + ", " +
                                            (data.RecruitPoolCollection[i].DaysWorked[j] + 1) + "</option>";
            daysCount += (data.RecruitPoolCollection[i].DaysWorked[j] + 1);
        }
        tRow += "</select></td>";
    }
    else {
        tRow += "<td><span>" + data.RecruitPoolCollection[i].Depts[0] + "</span></td>";
        daysCount = data.RecruitPoolCollection[i].DaysWorked[0] + 1;
    }
    tRow += "<td><span>" + daysCount + "</span></td>";

    tRow += "<td><span class='callStatus'>waiting</td>";
    tRow += "<td><span class='workStatus'>No Response</span></td><td><input class='rosterAdd smallButton' type='button' value='ADD'></td>";
    tRow += "<td><input type='button' value='AUTO' class='singlePhoneBlast smallButton'/>";

    if (phoneNumberIsValid(data.RecruitPoolCollection[i].PhoneNum)) {
        tRow += "<input type='button' class='directCall smallButton' value='CALL'/>";
        tRow += "<input type='button' class='directText smallButton' value='TEXT'/>";
    }
    tRow += "</td>";
    tRow += "<td><input type='button' value='Add / Rem' class='customAdd smallButton'/></td>";
    tRow += "</tr>";
    return tRow;
}

/* refresh drop-down lists */
function refresh(ddl, $row, selectAll) {
    if (ddl >= 4) {
        getClients($row.find("select[id*='Client']"));
    }
    if (ddl >= 3) {
        getLocations($row.find("select[id*='Location']"), selectAll);
    }
    if (ddl >= 2) {
        getShifts($row.find("select[id*='Shift']"), selectAll);
    }
    if (ddl >= 1) {
        getDepartments($row.find("select[id*='Department']"), selectAll);
    }
    if (ddl >= 0 && selectAll==false) {
        getShiftTimes();
    }
}

/* initialize selectors */
function getShiftTimes() {
    var clientid = $('#ddlClientRoster').val();
    var shiftType = $('#ddlShiftRoster').val();
    var dept = $('#ddlDepartmentRoster').val();
    var loc = $('#ddlLocationRoster').val();
    Url = ClientBaseURL + clientid + "/ShiftTime?loc=" + loc + "&shiftType=" + shiftType + "&dept=" + dept;
    waitIndicator(true, "Getting Shift times");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            start = jsonData.substring(10, 15);
            end = jsonData.substring(15, 20);
            $('#shiftStart').text(jsonData.substring(0, 5));
            $('#shiftEnd').text(jsonData.substring(5, 10));
            $('#trackingStart').text(start);
            $('#trackingEnd').text(end);
            $('#break').text(jsonData.substring(20) + " hrs");
            waitIndicator(false);
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function getCustomLists() {
    var $ddlCustomList = $("#ddlCustomList");
    var Url = RosterBaseURL + "PhoneBlastLists/" + "00700";
    waitIndicator(true, "Getting Custom Phone Lists");
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            var opt = "";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].PhoneBlastListID + "\">" + jsonArray[i].Description + "</option>";
            }
            $ddlCustomList.html(opt);
            waitIndicator(false);
        },
        error: function (xhr) {
            //alert("ERROR!!!" + xhr.responseText);
            var opt = "";
            //if (jsonArray.length > 1 && selectAll)
            opt += "<option value='0'>NO AVAILABLE DEPARTMENTS</option>";
            $ddlCustomList.html(opt);
            waitIndicator(false);
        }
    });
}
function getSkillLists() {
    var $ddlSkillList = $("#ddlSkillList");
    var Url = RosterBaseURL + "SkillDescriptions/" + "00700";
    waitIndicator(true, "Getting Skill Description Lists");
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            var opt = "";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].Id + "\">" + jsonArray[i].Description + "</option>";
            }
            $ddlSkillList.html(opt);
            waitIndicator(false);
        },
        error: function (xhr) {
            //alert("ERROR!!!" + xhr.responseText);
            var opt = "";
            //if (jsonArray.length > 1 && selectAll)
            opt += "<option value='0'>NO AVAILABLE DEPARTMENTS</option>";
            $ddlCustomList.html(opt);
            waitIndicator(false);
        }
    });
}
function getDepartments($ddlDepartment, selectAll) {
    //alert("dept = " + shift);
    var $tr = $ddlDepartment.closest('tr');
    //alert($tr.length);
    var $clientID = $tr.find('select[id*="ddlClient"]').val();
    var $locationID = $tr.find('select[id*="ddlLocation"]').val();
    var $shiftType = $tr.find('select[id*="ddlShift"]').val();

    var Url = ClientBaseURL + $clientID + "/GetDepartments?locationID=" + $locationID + "&shiftType=" + $shiftType;
    waitIndicator(true, "Getting Departments");
    //alert(Url);
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            var opt = "";
            if (jsonArray.length > 1 && selectAll)
                opt += "<option value='0'>ALL DEPARTMENTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].DepartmentID + "\">" + jsonArray[i].DepartmentName + "</option>";
            }
            $ddlDepartment.html(opt);
            waitIndicator(false);
        },
        error: function (xhr) {
            //alert("ERROR!!!" + xhr.responseText);
            var opt = "";
            //if (jsonArray.length > 1 && selectAll)
            opt += "<option value='0'>NO AVAILABLE DEPARTMENTS</option>";
            $ddlDepartment.html(opt);
            waitIndicator(false);
        }
    });
}

function getShifts($ddlShift, selectAll) {
    var $clientID = $ddlShift.closest('tr').find('select[id*="ddlClient"]').val();
    var Url = ClientBaseURL + $clientID + "/ShiftTypes"; /*+
            "&locationID=" + $("#ddlLocation").children("option").filter(":selected").val()*/
    waitIndicator(true, "Getting Shifts");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonArray) {
            //jsonArray = jsonData;
            /* fill out shift drop down */
            var opt = "";
            if (jsonArray.length > 1 && selectAll)
                opt += "<option value='0'>ALL SHIFTS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].ShiftTypeId + "\">" + jsonArray[i].ShiftTypeDesc + "</option>";
            }
            $ddlShift.html(opt);
            waitIndicator(false);
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function getLocations($ddlLocation, selectAll) {
    var jsonArray = new Array();

    var $clientID = $ddlLocation.closest('tr').find('select[id*="ddlClient"]').val();
    Url = ClientBaseURL + $('input[id$="userID"]').val() + "/Locations?clientID=" + $clientID;
    waitIndicator(true, "Getting Locations");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            waitIndicator(false);
            jsonArray = jsonData;
            /* init client drop down */
            $($ddlLocation).empty();
            var opt = "";
            if (jsonArray.length > 1 && selectAll)
                opt += "<option value='0'>ALL LOCATIONS</option>";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].LocationID + "\">" + jsonArray[i].LocationName + "</option>";
            }
            $ddlLocation.html(opt);
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("ERROR!!!" + xhr.responseText);
        }
    });
}

function getEmailList($ddlClient) {
    waitIndicator(true, "Getting Email List");
    var clientID = $("#ddlClientRoster").val();
    var Url = ClientBaseURL + "GetEmail/" + clientID;
    $.ajax({
        cache: false,
        type: "GET",
        contentTypee: "application/json; charset=utf-8",
        async: false,
        url: Url,
        success: function (data) {
            emailList = data;
            waitIndicator(false);
        },
        error: function (err) {
            alert(err);
        }
    }); 
}

/* get Clients sets both client drop-down lists */
function getClients($ddlClient) {
    /* populate client ddl when page is loaded */
    var jsonArray = new Array();
    var Url = ClientBaseURL + $('input[id$="userID"]').val() + "/Active";
    waitIndicator(true, "Getting Clients");
    $.ajax({
        cache: false,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        url: Url,
        success: function (jsonData) {
            waitIndicator(false);
            jsonArray = jsonData;
            /* init client drop down */
            $ddlClient.empty();
            var opt = "";
            for (var i = 0; i < jsonArray.length; i++) {
                opt += "<option value=\"" + jsonArray[i].ClientID + "\">" + jsonArray[i].ClientName + "</option>";
            }
            $ddlClient.html(opt);
        },
        error: function (xhr) {
            waitIndicator(false);
            alert("Error coming!");
            alert(xhr.responseText);
        }
    });
}

$(function() {
    $( "#dialog" ).dialog({
        autoOpen: false
    });
});


