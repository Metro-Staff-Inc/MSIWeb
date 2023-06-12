var empInfo = {};
var currentTab = 0; // Current tab is set to be the first tab (0)
var tabs = document.getElementsByClassName("tab");  // the 5 (so far) tabs
var $divs;
var $activeDiv; 
var currentDivId = "none";
var nextDivId = "none";

/*
function showTab(n) {
    // This function will display the specified tab of the form ...
    var x = document.getElementsByClassName("tab");
    x[n].style.display = "block";
    // ... and fix the Previous/Next buttons:
    if (n == 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n == (x.length - 1)) {
        document.getElementById("nextBtn").innerHTML = "Submit";
    } else {
        document.getElementById("nextBtn").innerHTML = "Next";
    }
    // ... and run a function that displays the correct step indicator:
    fixStepIndicator(n)
}
*/
    //divs
    // 0-div_main
    // 1-div_login
    // 2-div_employee_info
    // 3-div_coverage
    // 4-div_coverage_options
    // 5-div_family

function next() {
    // This function will figure out which tab to display

/* what tab are we currently on? */
    currentDivId = "";
    nextDivId = "none";
    $divs.each(function () {
        if ($(this).is(":visible") == true) {
            $activeDiv = $(this);
            currentDivId = $(this).attr("id");
        }
        else if (nextDivId == "none") {
            nextDivId = $(this).attr("id");
        }
    });
    alert(currentDivId + ", " + nextDivId);

    if (currentDivId == "div_login") {
        if (validateLogin()) {
            getEmployeeInfo(document.getElementById("metro_staff_id").value,
                document.getElementById("ssn_last_four").value);
        }
    }
    else if (currentDivId == "div_employee_info") {
        advance();
    }
}

function advance() {

    $activeDiv.attr("disabled", "disabled");
    $activeDiv = $("#" + nextDivId);

    $activeDiv.css("display", "block");
    $activeDiv.removeClass("collapse");
    currentDivId = $activeDiv.attr("id");


    if (currentDivId == "div_employee_info") {
        $("#prevBtn").show().text("Exit").removeAttr("disabled");
        $("#nextBtn").show().text("Next").removeAttr("disabled");
    }

    if (currentDivId == "div_plan_selection") {
        var ele = document.getElementsByName("coverage");
        for (var i = 0; i < ele.length; i++)
            ele[i].checked = false;
    }
    /*
    if (currentTab >= tabs.length) {
        //...the form gets submitted:
        document.getElementById("regForm").submit();
    }
    */
    // Otherwise, display the correct tab:
    //showTab(currentTab);
}

function prev() {
    if (currentTab < 0) {
        currentTab = 0;
        return;
    }
    currentTab--;
    // This function will figure out which tab to display
    var x = document.getElementsByClassName("tab");  // the 5 (so far) tabs

    if (x[currentTab].id == "div_login") {
        document.getElementById("employee_info").classList.add("collapse");
        document.getElementById("metro_staff_id").value = "";
        document.getElementById("last_four").value = "";
        document.getElementById("plan_selection").style.display = "none";
    }
    showTab(currentTab);
}

function validatePlanSelection() {
    var x;
    var valid = false;
    x = document.getElementsByName("coverage");
    for (i = 0; i < x.length; i++) {
        if (x[i].checked) {
            valid = true;
            var ele = document.getElementById("plan_selection");
            if (x[i].value == "change_in_coverage") {
                ele.textContent = "Change to an existing plan.";
            }
            else {
                ele.textContent = "Create a new plan";
            }
        }
    }
    return valid;
}

function validateLogin() {

    // This function deals with validation of the form fields
    var x, y, i, valid = true;
    y = $activeDiv.find("input");
    //alert(y.length);
    for (i = 0; i < y.length; i++) {
        // If a field is empty...
        if (y[i].value == "") {
            // add an "invalid" class to the field:
            y[i].className += " invalid";
            // and set the current valid status to false:
            valid = false;
        }
    }
    return valid; // return the valid status
}

function fixStepIndicator(n) {
    // This function removes the "active" class of all steps...
    var i, x = document.getElementsByClassName("step");
    for (i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(" active", "");
    }
    //... and adds the "active" class to the current step:
    x[n].className += " active";
}

function fillEmployeeData() {
    document.getElementById("ei_last_name").textContent = empInfo.LastName;
    document.getElementById("ei_first_name").textContent = empInfo.FirstName;
    document.getElementById("ei_middle_initial").textContent = empInfo.MiddleInitial;
    document.getElementById("ei_ssn").textContent = "***-**-" + empInfo.SSN;

    document.getElementById("ei_address").value = empInfo.Address;
    document.getElementById("ei_city").value = empInfo.City;
    document.getElementById("ei_state").value = empInfo.State;
    document.getElementById("ei_zip").value = empInfo.Zip;
    document.getElementById("ei_email").value = empInfo.Email;
    document.getElementById("ei_phone").value = empInfo.Phone;
    document.getElementById("ei_date_of_birth").value = empInfo.Birthdate;
}

function getEmployeeInfo(id, ssn) {
    var url = "../Api/GetEmployee";
    var reqInfo = {};
    reqInfo.Aident = id;
    reqInfo.SSN = ssn;

    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        data: JSON.stringify(reqInfo),  //DATA NEEDS TO BE STRINGIFIED!
        url: url,
        success: function (data) {
            if (data.Success && data.Msg == "Query Made" ) {
                empInfo = data.Data;
                advance();
                fillEmployeeData();
            }
            else {
                alert(data.Msg);
            }
        },
        error: function (data) {
            alert(data.getAllResponseHeaders.toString());
        }
    });

    //return data.Data;
}

function updateEmployeeInfo() {
    var url = "";
    url = "../Api/UpdateEmployee";

    $.ajax({
        cache: false,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        data: JSON.stringify(empInfo),  //DATA NEEDS TO BE STRINGIFIED!
        url: url,
        success: function (data) {
            if (data.Success) {
                alert("Success! " + data.Msg);
            }
            else {
                alert("Request was not successful. " + data.Msg);
            }
        },
        error: function (data) {
            alert(data.getAllResponseHeaders.toString());
        }
    });
}

function addDependentRows() {
    /* calculate number of dependents - spouse and children */
    var linesNeeded = 0;
    if (document.getElementById("spouse_yes").checked) linesNeeded++;
    linesNeeded += parseInt(document.getElementById("num_children").value);

    var $container = $("#dependent_info_container");
    var idIdx = 0;
    var idLead = "dependent_info_";
    var genderLead = "dependent_gender_";
    var spouseLead = "dependent_spouse_";

    var $dependents = $container.find("div[id^='" + idLead + "']");
    var diff = linesNeeded - $dependents.length;

    if (diff >= 0) {
        /* find max to create unique id vals */
        $dependents.each(function () {
            var id = $(this).attr("id");
            var idVal = $(this).attr("id").substring(id.lastIndexOf("_") + 1);
            idVal = parseInt(idVal);
            if (idVal > idIdx) idIdx = idVal;
        });

        var $template = $("#dependent_info_");
        for (i = 0; i < diff; i++) {
            var $row = $template.clone();
            //$row.removeAttr('style');   /* this kept the template hidden */
            $row.css({ "display": "" });
            if (idIdx % 2 == 0) {
                $row.css("background-color", "#E0E0E0");
            }

            idIdx = idIdx + 1;
            $row.attr("id", idLead + idIdx);
            $row.find(".dependent_spouse").attr("name", spouseLead + idIdx);
            $row.find(".dependent_gender").attr("name", genderLead + idIdx);

            $container.append($row);

        }
    }
    else {
        var needToKeep = $dependents.length + diff;
        /* need to remove some of the dependent lines */
        $dependents.each(function () {
            if (needToKeep <= 0) $(this).remove();
            needToKeep = needToKeep - 1;
        });
    }
    if ($container.find("div[id^='" + idLead + "']").length == 0) {
        $container.css('display', 'none');
    }
    else {
        $container.css('display', 'block');
    }
}

function activateUpdateButton() {
    document.getElementById("btnUpdateEmployeeInfo").removeAttribute("disabled");
    empInfo.LastName = document.getElementById("ei_last_name").textContent;
    empInfo.FirstName = document.getElementById("ei_first_name").textContent;
    empInfo.MiddleInitial = document.getElementById("ei_middle_initial").textContent;
    empInfo.SSN = document.getElementById("ei_ssn").textContent;

    empInfo.Address = document.getElementById("ei_address").value;
    empInfo.City = document.getElementById("ei_city").value;
    empInfo.State = document.getElementById("ei_state").value;
    empInfo.Zip = document.getElementById("ei_zip").value;
    empInfo.Email = document.getElementById("ei_email").value;
    empInfo.Phone = document.getElementById("ei_phone").value;
    empInfo.Birthdate = document.getElementById("ei_date_of_birth").value;
}

function acaInit() {
    $("#prevBtn").hide();
    $("#nextBtn").show().text("Submit").removeAttr("disabled");
    $divs.each(function () {
        $(this).hide();
        if ($(this).attr("id") == "div_main" || $(this).attr("id") == "div_login") {
            $(this).show();
        }
    });
}

jQuery(document).ready(function () {
    /* debug */
    $divs = $("div[id^='div_']");
    //alert($divs.length);
    $divs.each(function () {
        //alert($(this).attr("id"));
    })

    /* initialize login div */
    acaInit();


    var signaturePad = new SignaturePad(document.getElementById('signature-pad'), {
        backgroundColor: 'rgb(255, 255, 255)',
        penColor: 'rgb(0, 0, 0)', 
        backgroundImage: '../Images/signatureLine.jpg'
    });
    var canvas = document.getElementById("signature-pad");
    const context = canvas.getContext("2d");

    var clearButton = document.getElementById('clear');
    /*
    saveButton.addEventListener('click', function (event) {
        var data = signaturePad.toDataURL('image/png');
        alert(data);
        // Send data to server instead...
        window.open(data);
    });
    */
    clearButton.addEventListener('click', function (event) {
        signaturePad.clear();
    });
});