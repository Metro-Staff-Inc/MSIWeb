//var RosterBaseURL = 'http://localhost:52293/RestServiceWWW/Roster';
//var ClientBaseURL = 'http://localhost:52293/RestServiceWWW/Client';
var RosterBaseURL = "../Roster";
var ClientBaseURL = "../Client";
var OpenBaseURL = "../Open";

$(function () {
    $('#popup-wrapper').modalPopLite({ openButton: 'tzz[id*="ZAction"]', closeButton: '#close-btn' });
});

function createPunch() {
    var $time = $('#adder > p > select option:selected');
    var $date = $('.ttDateHidden').val() + ' ' +
                            $time.eq(0).text() + ':' + //HOURS
                            $time.eq(1).text() + ' ' + //MINS
                            $time.eq(2).text(); //AM-PM
    var dtRnd = new Date($date);

    var start = new Date($('input[id*="hdnStartDate"]').val());
    var end = new Date($('input[id*="hdnEndDate"]').val());
    var warning = "";

    if (dtRnd < start || dtRnd > end) {
        warning += "<br/><hr/><b>Warning!</b> Date is out of page range.  It WON'T be visible on this page!" +
                                        "<br/>The only punches visible on this page are between: <br/><b>" + start.toLocaleString() +
                                        "</b><br/> and <br/><b>" + end.toLocaleString() + "</b>";
    }
    if (warning.length > 0) /* warning dialog */{
        $('<div>' + warning + '</div>').dialog({
            title: "<b>Add Punch Warning!</b>",
            width: "460px",

            draggable: true,
            buttons: {
                "Add Anyway": function () {
                    createPunchOK(dtRnd);
                    $(this).dialog("close");
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                //nothing happens on close...
            }
        });
    }
    else {
        createPunchOK(dtRnd);
    }

}

function createPunchOK($dtRnd) {
    //alert($('#adder').attr('td'));
    var m = $dtRnd.getMonth() + 1;
    if (m < 10) m = '0' + m;
    var d = $dtRnd.getDate();
    if (d < 10) d = '0' + d;
    var amPm = 'AM';
    var h = $dtRnd.getHours();
    if( h > 12 )
    {
        h -= 12;
        amPm = 'PM';
    }
    if( h == 12 )
        amPm = 'PM';
    var mins = $dtRnd.getMinutes();
    if (mins < 10)
        mins = '0' + mins;
    $date = m + "/" + d + "/" + $dtRnd.getFullYear() + " " + h + ":" + mins + ' ' + amPm;
    //alert($date);
    var deptID = $("#cboDepartment > option:selected").val();
    var shiftId = $("#cboShift > option:selected").val();
    var badge = $("span[id='apBadge']").text();
    var userID = $("input[id*='userID']").val();
    var crID = $("#adder").attr("cr_id");
    Url = ClientBaseURL + '/' + badge + '/AddPunch?crID=' + crID + '&punchDate=' + $date + '&deptID=' + deptID + '&userID=' + userID + "&shiftType=" + shiftId;
    $.ajax({
        cache: false,
        type: "GET",
        url: Url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (d) {
            var tdID = '#' + $('#adder').attr('td');
            $(tdID).html("<span>" + $date + "</span>");
            $('#addPunch').remove();
            $(tdID).parent().find("span[id*='lblPunchStatus']").text("*MODIFIED*");
            $(tdID).off('click');
        },
        error: function (msg) {
            msg = eval(msg);
            alert(msg);
        }
    });
}


function addPunch(e, td) {
    var dtRnd = new Date($('input[id*="hdnStartDate"]').val());
    var crID = td.parent().attr("cr_id");
    var sY = $(window).scrollTop() + $(window).height();
    var d = $("<div id='addPunch'>" +
        "<div id='adder' cr_id='" + crID + "' td='" + td.attr("id") + "'>Add punch for:<span style='float:right; padding-right:8px' onclick='closePopUps()'>close</span><hr/>" + 
        "<span style='font-size:16px;'>" + td.parent().find("td > a[id*='lblName']").text() + '</span><br/>' +
        "ID# <span id='apBadge' style='font-size:16px;'>" + td.parent().find("td > a[id*='BadgeNumber']").text() + "</span>" + 
        "<p>" + htmlDate(dtRnd, true) + "</p><hr/>" +
        "<input type='button' style='float:right; padding-right:8px;' onclick='createPunch()' value='ADD'/>" + 
        "</div>" + 
        "</div>");
        d.appendTo('body').addClass("tooltip").css('top', (e.pageY - 80) + 'px').css('left', (e.pageX + 10) + 'px');
        setDatePicker();
}


function setDatePicker() {
    var d = new Date($(".ttDateHidden").val());
    $('.ttDate').datepicker({
        altFormat: "mm dd yy",
        altField: ".ttDateHidden",
        dateFormat: "M dd, yy",
        defaultDate: d,
//        changeMonth: true,
//        changeYear: true,
        monthNamesShort: ["Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. "]
    });
    //alert("set!");
}
/* only set click event if update times action event exists */
var tdAction = $("td[id*='tdAction_0']");
if (tdAction.length > 0) {
    $('input[id*="hdnPunch"]').parent().click(
        function (e) {
            alert("Click!");
	        var td = $(this);
	        var dw = $(document).width();
	        var dh = $(document).height();

	        var c = $('.img');
	        var c2 = $("#addPunch");

	        if (c.length > 0) {
	            c.remove();
	        }
	        else if (c2.length > 0) {
	            c2.remove();
	        }
	        else {
	            var punchID = $(this).find('span').attr("punchID");
	            if (punchID == null || punchID.length == 0) {
	                var id = $(this).parents('tr').find('td:first').find('a').text();
	                addPunch(e, td);
	            }
	            else {
	                var d = $('<div class="img" style="top:140px"></div>', {});
	                var sY = $(window).scrollTop() + $(window).height();
	                var im = $(this).find('input[id*="hdnPunch"]');
	                var nm = $(this).parent().find('td > a[id*="lblName"]');
	                var htm = addHtml(im.val(), nm.text(), $(this));
	                if ((e.pageY + 400) < sY)
	                    d.appendTo('body').html(htm).addClass("tooltip").css('top', (e.pageY - 80) + 'px').css('left', (e.pageX + 10) + 'px');
	                else
	                    d.appendTo('body').html(htm).addClass("tooltip").css('top', (e.pageY - 400) + 'px').css('left', (e.pageX + 10) + 'px');
	                /*$('.ttDate').datepicker({
	                altFormat: "mm dd yy",
	                altField: ".ttDateHidden",
	                dateFormat: "M dd, yy",
	                changeMonth: true,
	                changeYear: true,
	                monthNamesShort: ["Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. "]
	                });*/
	                setDatePicker();
	                $('#punchTime > input').change(enableUpdate);
	                $('#punchTime > select').change(enableUpdate);

	                document.getElementById("ttDel").onclick = function () {
	                    var userID = $('input[id$="userID"]').val();
	                    //alert("punch id = " + punchID);
	                    $("<div id='delPrompt'><p>Are you sure you want to delete this punch?</p></div>").dialog({
	                        height: 200,
	                        width: 350,
	                        title: "Delete punch for " + nm.text() + "<br/>Actual: " + td.find('span').attr("exact") +
                                        "<br/>Rounded: " + td.find('span').attr("round"),
	                        modal: true,
	                        buttons: {

	                            "Delete": function () {
	                                Url = RosterBaseURL + '/TTDelete?id=' + userID + '&punchID=' + punchID;
	                                $.ajax({
	                                    cache: false,
	                                    type: "GET",
	                                    url: Url,
	                                    async: false,
	                                    contentType: "application/json; charset=utf-8",
	                                    dataType: "json",
	                                    success: function (d) {
	                                        if (d == false) {
	                                            //alert($('#delPrompt').text());
	                                            $('#delPrompt').append("<h2>Error - Punch NOT Deleted!</h2>");
	                                        }
	                                        else {
	                                            //alert($('#delPrompt').text());
	                                            $('#delPrompt').append("<h2>Punch Deleted!</h2>");
	                                            td.text("");
	                                            td.parent().find('td > span[id*="lblPunchStatus"]').text("*MODIFIED*");
	                                            var span = td.parent().find('td > span[id*="lblPunchStatus"]');
	                                            $(span).attr("title", "Refresh to see status");
	                                        }
	                                        $('#delPrompt').dialog("option",
                                                "buttons", [
                                                    {
                                                        text: "Ok", click: function () {
                                                            $(this).dialog("close");
                                                            element = document.getElementById("delPrompt");
                                                            element.parentNode.removeChild(element);
                                                        }
                                                    }
                                                ]);
	                                    },
	                                    error: function (msg) {
	                                        msg = eval(msg);
	                                        alert(msg);
	                                    }
	                                });
	                            },
	                            Cancel: function () {
	                                $(this).dialog("close");
	                                element = document.getElementById("delPrompt");
	                                element.parentNode.removeChild(element);
	                            }
	                        },
	                        close: function () {
	                            var c = $('.img');
	                            if (c.length >= 1) {
	                                c.remove();
	                            }
	                        }
	                    });
	                };
	                document.getElementById("ttUpd").onclick = function () {
	                    //alert($('#punchTime > select').first().length);
                        alert("Click!");
	                    var $time = $('#punchTime > select option:selected');
	                    var $date = $('.ttDateHidden').val() + ' ' +
                            $time.eq(0).text() + ':' + //HOURS
                            $time.eq(1).text() + ' ' + //MINS
                            $time.eq(2).text(); //AM-PM
	                    var dtRnd = new Date($date);
	                    //alert(dtRnd);

	                    $date = $('.img tbody tr').eq(1).find('td').text();
	                    var dtEx = new Date($date);
	                    //alert(dtEx);

	                    var timeDiff = Math.abs(dtEx.getTime() - dtRnd.getTime());
	                    //var diffDays = Math.ceil( /*parseInt(timeDiff,10) / (1000 * 3600 * 24)*/ 24.545 );
	                    var diffMins = Math.ceil(timeDiff / (1000 * 60));
	                    //alert(timeDiff);
	                    var count = 1;
	                    var warning = "";
	                    if (diffMins > 40) {
	                        warning = "(" + count + ") <b>Warning!</b> There are " + diffMins + " minutes difference between the actual punch time and the rounded punch time.";
	                        warning += "<br/>(Typically the maximum difference is less than 30 minutes)";
	                        count++;
	                    }
	                    var start = new Date($('input[id*="hdnStartDate"]').val());
	                    var end = new Date($('input[id*="hdnEndDate"]').val());

	                    if (dtRnd < start || dtRnd > end) {
	                        warning += "<br/><hr/>(" + count + ") <b>Warning!</b> Date is out of page range.  It won't be visible on this page!" +
                                        "<br/>The only punches visible on this page are between: <br/><b>" + start.toLocaleString() +
                                        "</b><br/> and <br/><b>" + end.toLocaleString() + "</b>";
	                    }
	                    if (warning.length > 0) /* warning dialog */{
	                        $('<div>' + warning + '</div>').dialog({
	                            title: "<b>Punch adjust Warning!</b>",
	                            width: "460px",

	                            draggable: true,
	                            buttons: {
	                                "Adjust Anyway": function () {
	                                    updateTime(punchID, dtRnd, td);
	                                    $(this).dialog("close");
	                                },
	                                Cancel: function () {
	                                    $(this).dialog("close");
	                                }
	                            },
	                            close: function () {
	                                var c = $('.img');
	                                if (c.length >= 1) {
	                                    c.remove();
	                                }
	                            }
	                        });
	                    }
	                    else {
	                        updateTime(punchID, dtRnd, td);
	                        var c = $('.img');
	                        if (c.length >= 1) {
	                            c.remove();
	                        }
	                    }
	                };
	            }
	        }
	    });
    }
        function updateTime(punchID, dtRnd, td){
            Url = RosterBaseURL + "/" + punchID + "/UpdatePunch?userID=" + $('input[id$="userID"]').val() + "&month=" + (1 + parseInt(dtRnd.getMonth())) +
                                            "&day=" + dtRnd.getDate() + "&year=" + dtRnd.getFullYear() + "&hour=" + dtRnd.getHours() + "&min=" + dtRnd.getMinutes();
            //alert(Url);
            $.ajax({
                cache: false,
                type: "GET",
                url: Url,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (d) {
                    td.text(d);
                    td.parent().find('td > span[id*="lblPunchStatus"]').text("*MODIFIED*");
                    var span = td.parent().find('td > span[id*="lblPunchStatus"]');
                    $(span).attr("title", "Refresh to see status");
                    td.off('click');
                },
                error: function () {
                    alert("error!!!!");
                }
            });
        }
        function closePopUps() {
            $(".img").remove();
            $("#addPunch").remove();
        }

	    function addHtml(im, nm, td) {
	        var dtEx = td.find('span').attr("exact");
	        var dtRnd = td.find('span').attr("round");

	        var dir = $('input[id*="_clientDir"]').val();
	        var ret = "<table class=\"img\">";
	        ret += "<tr><td colspan='2'>" + nm + "</td></tr>"; /* name */
	        ret += "<tr><td colspan='2'>" + htmlDate(dtEx, false) + "</td></tr>";
	        ret += "<tr><td colspan='2' width=\"320\" height=\"246\">" +
                "<img alt=\"Picture Not Available\" src=\"http://msiwebtrax.com/dropbox/images/" + dir + "/" +
                im + "\"/></td></tr>";
	        ret += "<tr><td colspan='2' class='ttBtn'><hr/>Update:<span style='float:right; padding-right:8px' onclick='closePopUps()'>close</span></td></tr>";
	        ret += "<tr><td id='punchTime' colspan='2'>" + htmlDate(dtRnd, true) + "</td></tr>";
	        ret += "<tr><td><input id='ttUpd' class='ttBtn' type='button' disabled='true' value='Update Punch Record'/></td>";
	        ret += "<td><input id='ttDel' class='ttBtn' type='button' value='Delete Punch Record'/></td></tr>";

	        ret += "</table>";
            return ret;
        }

        function enableUpdate() {
            document.getElementById('ttUpd').disabled = false;
            $('#punchTime > input').css({
                "background-color": "#FFDDDD"
                /*"font-weight": "bolder"*/
            })
            $('#punchTime > select').css({
                "background-color": "#FFDDDD"
                /*"font-weight": "bolder"*/
            })
        }

        function htmlDate(dt, inp) {
            dt = new Date(dt);
            //alert(dt);
            var Days = new Array("Sun", "Mon", "Tues", "Wed", "Thu", "Fri", "Sat", "Sun");
            var Months = new Array("Trouble ", "Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. ");

            //var s = dt.indexOf('/');
            var mnth = parseInt(dt.getMonth(),10) + 1;//.slice(0, s);
            //dt = dt.slice(s + 1);

            //s = dt.indexOf('/');
            var day = dt.getDate(); // .slice(0, s);
            var weekDay = dt.getDay();

            //dt = dt.slice(s + 1);

            //s = dt.indexOf(' ');
            var yr = dt.getFullYear();// .slice(0, s);
            //dt = dt.slice(s + 1);

            var amPm = 'AM';

            //s = dt.indexOf(':');
            var hrs = dt.getHours(); // .slice(0, s);
            if (hrs >= 12) {
                hrs -= 12;
                amPm = 'PM';
            }
            if (hrs == 0)
                hrs += 12;
            if (hrs <= 9)
                hrs = '0' + hrs;
            //dt = dt.slice(s + 1);

            //s = dt.indexOf(' ');
            var mins = dt.getMinutes(); //dt.slice(0, s);
            if (mins <= 9)
                mins = '0' + mins;

            if (inp == false) {
                return Days[weekDay] + ", " + Months[parseInt(mnth, 10)] + day + ", " + yr +
                    "  " + hrs + ":" + mins + " " + amPm;
            }
            else {
                var ret = "<input class='ttDate' type='text' size='14' value='" + Months[parseInt(mnth, 10)] + day + ", " + yr +
                    "'></input><input class='ttDateHidden' type='hidden' value='" +  mnth + " " + day + " " + yr +"'></input>"; 
                    
                ret += "<select name='hours'>"; 
                for( var i=1; i<= 12; i++ ){
                    ret += "<option value='" + i + "'";
                    if (i != hrs)
                        ret += ">" + i + "</option>";
                    else
                        ret += " selected>" + i + "</option>";
                }
                ret += "</select>" + ":";
                ret += "<select name='mins'>";
                for (var i = 0; i < 60; i += 5) {
                    var v = i;
                    if (i < 10)
                        v = '0' + i;
                    ret += "<option value='" + v + "'";
                    if (i != mins)
                        ret += ">" + v + "</option>";
                    else
                        ret += "selected>" + v + "</option>";
                }
                ret += "</select>" + " ";

                ret += "<select name='amPm'>";
                if (amPm == "AM")
                    ret += "<option value='AM' selected>AM</option><option value='PM'>PM</option>";
                else
                    ret += "<option value='PM' selected>PM</option><option value='AM'>AM</option>";
                ret += "</select>";
                return ret;
            }
        } 

        jQuery(document).ready( function () {
            $('tl').hover(
            function () {
                var myClass = $(this).attr("class");
                $(this).addClass("lightBlue").removeClass(myClass);
            }, function () {
                $(this).removeClass("lightBlue");
                var row = $(this).parent().children().index($(this));
                if (row % 2 == 0)
                    $(this).addClass("white");
                else
                    $(this).addClass("grey");
            });
            $("a[id*='lblName']").click(function (e) {
                e.stopPropagation();
            });
            if ($("td[id*='tdAction_0']").length > 0) {

                $("td[id*='tdName']").click(function () {
                    var tr = $(this).closest("tr");
                    var name = tr.find("td[id*='Name']").text();
                    var badge = tr.find("a[id*='lblBadgeNum']").text();
                    var id = tr["0"].attributes["cr_id"].value;
                    $("#dialogRosterRemove").append();
                    $("<div id='dialogRosterRemove'>Remove " + name + ", " + badge + " from roster?</div>").dialog({
                        title: "<b>Remove Employee From Roster</b>",
                        width: "460px",
                        draggable: true,
                        buttons: {
                            "Remove": function () {
                                removeFromRoster(id, tr, this);
                            },
                            Cancel: function () {
                                $(this).dialog("close");
                            }
                        },
                        close: function () {
                            //nothing happens on close...
                        }
                    })
                }).hover(function () {
                    var myClass = $(this).attr("class");
                    $(this).addClass("yellow").removeClass(myClass);
                    $(this).attr("store", myClass);
                },
                function () {
                    var myClass = $(this).attr("store");
                    $(this).removeClass("yellow").addClass(myClass);
                    $(this).attr("class", myClass);
                });
            }
        });

            function removeFromRoster(clientRosterId, tr, dl) {
                /* get row the employee is on */
                $(tr).hide('fast', function () {
                    /* first remove from database */
                    //    alert("Start: " + start);
                    //    alert("End: " + end);

                    Url = OpenBaseURL + "/ClearClientRoster";
                    //alert(Url);
                    $.ajax({
                        cache: false,
                        type: "POST",
                        url: Url,
                        data: JSON.stringify(clientRosterId),
                        async: true,
                        contentType: "application/json",
                        dataType: "json",
                        success: function (msg) {
                            //$(dl).
                            $(dl).dialog("close");
                        },
                        error: function (msg) {
                            alert(msg);
                            $(dl).dialog("close");
                        }
                    });
                });
            }