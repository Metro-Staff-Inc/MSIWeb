jQuery(document).ready(
    function () {
        $("#btnEmpId").on("click", function () {
            $("#MapIdResults").html = "";
            var id = $("#txtboxEmpId").val();
            if (id == null || id.length == 0) return;
            $.ajax({
                cache: false,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                async: true,
                dataType: "json",
                url: "../Open/SuncastId?id=" + id,
                begin: function () {
                },
                success: function (data) {
                    var table = "<table width=\"960\" border=\"1\"><thead><tr>";
                    table += "<th>Last Name</th>";
                    table += "<th>First Name</th>";
                    table += "<th>MSI Aident Number</th>";
                    table += "<th>Suncast Number</th>";
                    table += "<th>Message</th>";
                    table += "</tr></thead><tbody>";
                    if (data == null || data.length == 0) {
                        alert("Employee with ID #" + id + " not found!");
                    }
                    else {
                        for (var i = 0; i < data.length; i++) {
                            table += "<tr>";
                            table += "<td>" + data[i].LastName + "</td>";
                            table += "<td>" + data[i].FirstName + "</td>";
                            table += "<td>" + data[i].AidentNum + "</td>";
                            table += "<td id='suncastId'>" + data[i].SuncastId + "</td>";
                            if (data[i].SuncastId > 0) {
                                table += "<td>" + data[i].Msg + "</td>"
                            }
                            else {
                                table += "<td><span id='spanId'>No Suncast Number</span><input type='button' id='generateSuncastId' value='Generate'</input></td>";
                            }
                            table += "</tr>";
                        }
                        table += "</tbody></table>";
                    }
                    $("#MapIdResults").html(table);
                },
                error: function (error) {
                    alert(error);
                },
                complete: function () {
                    $("#generateSuncastId").on("click", function () {
                        $.ajax({
                            cache: false,
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            async: true,
                            dataType: "json",
                            url: "../Open/CreateSuncastId?id=" + id,
                            begin: function () {
                            },
                            success: function (data) {
                                if (data.length > 0) {
                                    $("#suncastId").html(data);
                                    $("#spanId").html("");
                                    $("#generateSuncastId").remove();
                                }
                                else {
                                    alert("Suncast ID not created.");
                                }
                            },
                            error: function (error) {
                                alert(error);
                            },
                            complete: function () {
                            }
                        });
                    });
                }
            });
        });
    }
);