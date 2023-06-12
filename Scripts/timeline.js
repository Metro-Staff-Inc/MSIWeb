function setTimeline(curTr) {
    trs = curTr.prevUntil("tr[id*='trDepartment_']");
    //alert(trs.length);

    google.charts.load("current", { packages: ["timeline"] });
    google.charts.setOnLoadCallback(drawChart);

    function formatDate(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
    }
    function drawChart() {
        var container = $(curTr.next('tr').find('div'))[0];
        //var container = document.getElementById('punchTimeline');
        var chart = new google.visualization.Timeline(container);
        var dataTable = new google.visualization.DataTable();
        var chartHeight = 72;
        dataTable.addColumn({ type: 'string', id: 'ID' });
        dataTable.addColumn({ type: 'string', id: 'Name' });
        dataTable.addColumn({ type: 'string', role: 'tooltip', p: { html: true } });
        dataTable.addColumn({ type: 'date', id: 'Start' });
        dataTable.addColumn({ type: 'date', id: 'End' });

        for (var i=trs.length-1; i >=0; i-=2) {
            var tr = trs.eq(i);
            var id = tr.eq(0).find("span[id*='lblBadgeNumber']").text();
            var name = tr.eq(0).find("span[id*='lblLastName']").text() + ", " + tr.eq(0).find("span[id*='lblFirstName']").text();
            var trDetail = trs.eq(i - 1); //detail row
            var trDetails = $(trDetail).find("tbody").find("tr[id*='Detail']");
            $(trDetails).each(function () {
                var checkIn = $(this).find("span[id*='lblCheckIn']").text();
                var checkOut = $(this).find("span[id*='lblCheckOut']").text();
                if (checkOut == "N/A") {
                    checkOut = checkIn;
                }
                //if (checkOut < checkIn) {
                //    alert("ERROR:  " + name + ", check in time: " + checkIn + ", check out time: " + checkOut);
                //}
                var hours = $(this).find("span[id*='lblCheckHours']").text();
                checkIn = new Date(checkIn);
                checkOut = new Date(checkOut);
                var tt = "<table>" +
                    "<tr><td>" + id + "</td></tr>" +
                    "<tr><td>" + name + "</td></tr>" +
                    "<tr><td>" + formatDate(checkIn) + "</td></tr>" + 
                    "<tr><td>" + formatDate(checkOut) + "</td></tr>" + 
                    "<tr><td>" + hours + " Hours</td></tr>" +
                "</table>";
                dataTable.addRows([
                  [id, name, tt, checkIn, checkOut]]);
            });
        }
        var options = {
            height: 76 + (trs.length / 2) * 40,
            tooltip: { isHtml: true },
            legend: 'none'
        };
        chart.draw(dataTable, options);
    }
}