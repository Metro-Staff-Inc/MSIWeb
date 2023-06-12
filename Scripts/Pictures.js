var EmployeeNameBaseURL;
var EmployeeIdBaseURL;

jQuery(document).ready(function () {

    //webServiceLoc = $('input[id$="webServiceLoc"]').val();
    //alert(webServiceLoc);
    EmployeeNameBaseURL = "../Roster/Name/Pic/";
    EmployeeDateBaseURL = "../Roster/Date/Pic/";
    EmployeeIdBaseURL = "../Roster/Id/Pic/";

    //alert("Here we go!");

    $('#txtIdNum').keypress(function (event) {
        if (event.which == 13)
            $('#btnIdNum').trigger('click');
    });

    $('#txtName').keypress(function (event) {
        if (event.which == 13)
            $('#btnName').trigger('click');
    });

    $('#btnLastWorked').click(function (event) {
        event.preventDefault();
        $('#availableEmp').addClass('Progress');
        $('#availableEmp').html("");
        //alert($('#ddlLastWorked').val());
        getAvailableEmployeesByDate($('#ddlLastWorked').val());
    });

    $("#btnIdNum").click(function (e) {
        e.preventDefault();
        $("#availableEmp").addClass("Progress");
        $("#availableEmp").html("");
        getAvailableEmployeesByID();
    });

    $("#btnName").click(function (e) {
        e.preventDefault();
        $("#availableEmp").addClass("Progress");
        $("#availableEmp").html("");
        getAvailableEmployeesByName($("#txtName").val());
    });

    function setEmployeeList(msg) {
        //alert( "in function!");
        $("#availableEmp").html(msg);
        $("#availableEmp").removeClass("Progress");
        $("#tblAvailableEmployees").fixedHeaderTable({
            width: '368',
            footer: true,
            cloneHeadToFoot: true,
            altClass: 'odd',
            themeClass: 'fancyTable',
            footer: false,
            cloneHeadToFoot: false,
            fixedColumn: false,
            autoShow: true
        });
    }

    function getAvailableEmployeesByName(name) {
        Url = EmployeeNameBaseURL + name;
        $.ajax({
            context: document.body
        });
        $.get(Url, "", setEmployeeList);
    }

    function getAvailableEmployeesByDate(days) {
        Url = EmployeeDateBaseURL + days + "?clientID=" + $('#ctlSubHeader_clientDir').val();
        //alert("days = " + days + ", url = " + Url);
        $.ajax({
            context: document.body
        });
        $.get(Url, "", setEmployeeList);
    }

    function getAvailableEmployeesByID() {
        id = $("#txtIdNum").val();
        Url = EmployeeIdBaseURL + id;
        //alert(Url);
        $.get(Url, "", setEmployeeList);
    }
});