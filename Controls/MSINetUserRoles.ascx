<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetUserRoles.ascx.cs" Inherits="MSI.Web.Controls.MSINetUserRoles" %>

<style>
    td{
        margin-right:40px;
        padding-right:40px;
    }
    .userDivs
    {
        overflow:auto;
        height:400px;
        background-color:antiquewhite;
    }
    #update
    {
        margin-left:180px;
    }
    #rPwd
    {
        margin-left:40px;
    }
    #email
    {
        margin-left:40px;
    }
    .update
    {
        background-color:lightcoral;
    }
</style>
<div>
    <table id="userInfo">
        <tr>
            <td colspan="2">
                <div class="userDivsHeader">
                    <table id="selectUser">
                        <thead>
                            <tr>
                                <th>
                                    <span>USER NAME</span>
                                </th>
                                <th>
                                    <span>EMAIL</span>
                                    <input id="update" type="button" value="UPDATE" disabled="disabled"/>
                                    <input id="rPwd" type="button" value="RESET PASSWORD"/>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <select id="users">
                                    <option value="0">SELECT USER</option>
                                    </select>
                                    <input id="newuser" style="margin-left:80px" type="button" value="New User" />
                                </td>
                                <td>
                                    <input id="email" size="60" type="text" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="padding-right:0px;margin-right:0px;">
                <div class="userDivs">
                    <table id="userRoles">
                        <thead>
                            <tr>
                                <th>ROLE</th><th>STATUS</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </td>
            <td>
                <div class="userDivs">
                    <table id="userClients">
                        <thead>
                            <tr>
                                <th>CLIENT</th><th>ACCESSIBLE</th><th>DEFAULT</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>

    <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/UserRoles.js"></script>
    