
var users;
var roles;
var clients;
var usersRoles;
var usersClients;
var defaultSelectUserText = "SELECT USER";

function setClients() {
    var tbl = "<tbody>";
    for (var i = 0; i < clients.length; i++) {
        tbl = tbl + "<tr><td>" + clients[i].ClientName + "</td>" +
            "<td><select id='" + clients[i].ClientID + "_Accessible'><option>False</option><option>True</option></select></td>" + 
            "<td><input type='radio' id='" + clients[i].ClientID + "_Preferred' name='preferredClient' /></td>" +
            "</tr>";
    }
    tbl = tbl + "</tbody>";
    $("#userClients").append(tbl);
}

var getClients = function () {
    $.ajax({
        url: "../Client/jmurfey/Active",
        datatype: "json",
        error: function (error) {
            alert(error);
        },
        success: function (data) {
            clients = data;
        },
        complete: function () {
            setClients();
            fht();
        }
    });
}

var getUsers = function(){
    $.ajax({
        url: "../Open/Users",
        datatype: "json",
        success: function (data) {
            users = data;
        },
        error: function (error) {
            alert( error );
        },
        complete: function () {
            var ddl = "";
            ddl = ddl + "<option email='' >" + defaultSelectUserText +"</option>";
            for (var i = 0; i < users.length; i++)
            {
                ddl = ddl + "<option email='" + users[i]['Email'] + "' >" + users[i]['UserName'] + "</option>";
            }
            $("#users").html(ddl);
            fht();
            /*$("#viewEdit").removeAttr("disabled");*/
        }
    });
}

var resetPassword = function () {
    var userName = $('#users').find(":selected").text();
    var email = $("#email").val();
    //alert("Reseting!!!" + email + ", " + userName);
    $.ajax({
        url: "../Open/ResetPassword?userName=" + userName + "&email=" + email,
        datatype: "json",
        success: function (data) {
            alert(data);
        },
        error: function (error) {
            alert("Password reset error - " + error);
        },
        complete: function () {
        }
    });
    //var url = "../Open/ResetPassword?userName=" + userName + "&email=" + email + "jmurfey@msistaff.com",
}

function fht() {
    //alert("Called fixed header table!");
    $("#userClients").fixedHeaderTable({
            width: '600',
            altClass: 'odd',
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
    });
    $("#userRoles").fixedHeaderTable({
            width: '400',
            altClass: 'odd',
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
    });
    $("#selectUser").fixedHeaderTable({
            width: '1000',
            altClass: 'odd',
            themeClass: 'fancyTable',
            fixedColumn: true,
            autoShow: true
    });
    $("#rPwd").off();
    $("#rPwd").click(resetPassword);
    //$("#update").click(function () {
    //    $(".update").each(function () {
    //        alert("value = " + $("option:selected").val());
    //    });
    var count = 0;
    var obj = {};
    obj2 = {};
    $("#update").off();
    $("#update").click(function () {
        var roles = [];
        var i = 0;
        $("#userRoles").find("select").each(function () {
            alert($(this).attr("id") + ", " + $(this).find("option:selected").val());
            if ($(this).find("option:selected").val() == true) {
                roles[i++] = $(this).attr('id');
                //alert($(this).attr('id'));
            }
        });
    });
}

var getUsersRoles = function (userName) {
    $.ajax({
        url: "../Open/UserRoles?userName=" + userName,
        datatype: "json",
        success: function (data) {
            usersRoles = data;
        },
        error: function (error) {
            alert("Error! " + error);
        },
        complete: function () {
            $("#userRoles select").val("False");
            $("#userRoles select").removeAttr("update");
            for (var i = 0; i < usersRoles.length; i++) {
                $("#" + usersRoles[i]).val("True");
            }
            fht();
        }
    });
}

var getUsersClients = function(userName){
    $.ajax({
        url: "../Client/GetAll/" + userName,
        datatype: "json",
        success: function(data){
            usersClients = data;
        },
        error: function (error) {
            alert("Error! " + error);
        },
        complete: function () {
            $("userClients select").val("False");
            for( var i=0; i<usersClients.ClientID.length; i++ ){
                $("#" + usersClients.ClientID[i] + "_Accessible").val("True");
            }
            $("#" + usersClients.Preferred + "_Preferred").attr("checked", "true");
            fht();
        }
    });
}

function setRoles()
{
    var tbl = "<tbody>";
    var r = "";
    for (var i = 0; i < roles.length; i++)
    {
        tbl = tbl + "<tr><td>" + roles[i] + "</td><td><select id='" + roles[i] +
            "'><option>False</option><option>True</option></select></td></tr>";
    }
    tbl = tbl + "</tbody>";
    $("#userRoles").append(tbl);
    $("#userRoles select").change(function () {
        $(this).parent("td").addClass("update");
        //alert($(this).val + " User Roles!");
        $("#update").removeAttr("disabled");
    });
}

var getRoles = function () {
    $.ajax({
        url: "../Open/AllRoles?",
        datatype: "json",
        success: function (data) {
            roles = data;
        },
        error: function (error) {
            alert("Get Roles error - " + error);
        },
        complete: function () {
            setRoles();
            fht();
        }
    });
}

var getClientOptions = function() {
    var sel = "";
    for( var i=0; i<clients.length; i++ )
    {
        sel = sel + "<option value='" + clients[i].ClientID + "'>" + clients[i].ClientName + "</option>";
    }
    return sel;
}

var newUserDialog = "<div id='newUser'>" +
                        "<table>" + 
                        "<tr>" +
                            "<td>USER NAME:</td>" +
                            "<td><input id='newUserName' type='text' witdth='20'/></td>" +
                            "<td><span id='userMsg'></span></td>" + 
                        "</tr>" +
                        "<tr>" +
                            "<td>DEFAULT CLIENT:</td>" +
                            "<td colspan='2'><select id='defClient'></select></td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>EMAIL:</td>" +
                            "<td colspan='2'><input id='newUserEmail' type='text' width='50'/></td>" +
                        "</tr>" +
                        "<h2>Enter UserName:</h2>" +
                    "</div>";

$(document).ready(
    function () {
    //    alert("Here we go!");
        users = getUsers();
        roles = getRoles();
        getClients();
        $("#newuser").click(function () {
            $(newUserDialog).dialog({
                close: function (event, ui) { $('#newUser').remove(); },
                resizable: false,
                height: 280,
                width: 520,
                open: function (event, ui) {
                    //alert("Opening!");
                    var clientList = getClientOptions();
                    $("#defClient").html(clientList);
                },
                title: 'Create New User',
                draggable: true,
                modal: true,
                buttons: {
                    "Create New User": function () {
                        if ($("#newUserEmail").val() == "" || $("#newUserName").val() == "")
                            alert("UserName and Email are required!");
                        else
                        {
                            var client = $("#defClient").val();
                            var userName = $("#newUserName").val();
                            var userEmail = $("#newUserEmail").val();
                            $.ajax({
                                url: "../Open/CreateUser/" + client + "?userName=" + userName + "&email=" + userEmail,
                                complete:function(data){
                                    //alert( data);
                                },
                                success:function(data){
                                    $("#userMsg").text(data);
                                },
                                error:function(data){
                                    alert( "An error has occurred!");
                                }
                            });
                        }
                    },
                    "Close": function () {
                        $('#newUser').remove();
                    }
                }
            });
            //alert("Clicked on new user!");
        });
    });
