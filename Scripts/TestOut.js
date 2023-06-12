var S = openXml.S;
var R = openXml.R;


jQuery(document).ready(
     (function (root) {  // root = global
        /*"use strict";*/
        //alert("Hello!");
        root.exportToXlsx2 = function (tableId) {

            // open the document
            openXml.util.bucketTimer.init();
            openXml.util.bucketTimer.bucket("OpenAndSave");

            var doc = new openXml.OpenXmlPackage(excelTemplate);

            var workbook = doc.workbookPart();
            var workbookXDoc = workbook.getXDocument();

            var z = workbookXDoc.toString(true);
            var sheets = workbookXDoc.root.element(S.sheets);
            var z2 = sheets.toString(true);




            var sheet = null;
            var relId = null;
            var sheetName = "MySheet";
            
            for (var i = 0; i < sheets.nodesArray.length; i++) {
                sheet = sheets.nodesArray[i];
                if (sheet.nodeType != 'Element')
                    continue;
                if (sheet.attribute('name').value == sheetName) {
                    relId = sheet.attribute(R.id).value;
                    sheet.attribute('name').value = 'HeadCount';
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
                var fileName = '' + dt.getFullYear() + (dt.getMonth() + 1) + dt.getDate() + '_';
                fileName += "HeadCount";



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
    }(this)));