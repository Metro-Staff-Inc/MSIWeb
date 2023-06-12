<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetClientRoster.ascx.cs" Inherits="MSI.Web.Controls.MSINetClientRoster" %>
    
    <div visible="false">
        <input type="hidden" runat="server" id="userID"/>
        <input type="hidden" runat="server" id="webServiceLoc" />
    </div>
    
    <div align="center"> <!-- TABLE GOES HERE -->
        <table style="width:1200px" border="1" cellpadding="4" cellspacing="0">
            <tr>
                <td class="availableEmp">
                    <fieldset>
                        <legend>Employee Pool Parameters</legend>
                        <table>
                        <tr>
                            <td>ID Number:</td>
                            <td><input id="txtIdNum" type="text" /></td>
                            <td><input type="button" value="GO" id="btnIdNum"/></td>
                            <td style="padding-left:20px" rowspan="3">
                            <fieldset style="padding-left:10px">
                            <legend>Length to add</legend>
                            <input value="day" type="radio" name="time" checked="checked"/>
                            <select id="ddlDays">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                                <option value="5">5</option>
                                <option value="6">6</option>
                            </select><span style="padding-left:10px">Day</span><br/>
                            <input value="week" type="radio" name="time" />
                            <select id="ddlWeeks">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="4">4</option>
                            </select><span style="padding-left:10px">Week</span><br />
                            <input value="perm" type="radio" name="time"/>Permanent<br />
                            </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>Last Name:</td>
                            <td><input id="txtName" type="text" /></td>
                            <td><input type="button" value="GO" id="btnName"/></td>                            
                        </tr>
                        <tr>
                            <td>Same Dept:</td>
                            <td><select id="ddlPrevious">
                                <option value="1">Yesterday</option>
                                <option value="2">2 Days Ago</option>
                                <option value="7">In the Last Week</option>
                                <option value="32">In the Last Month</option>
                            </select></td>
                            <td><input type="button" value="GO" id="btnSameDept"/></td>
                        </tr>
                        </table>
                    </fieldset>
                </td>

                <td>
                    <div class="currentEmp">
                    <fieldset>
                        <legend>Roster Parameters</legend>
                        <table border="0" cellpadding="4" cellspacing="0" style="width:780px">
                            <tr>
                                <td colspan='3' style="width:290px">
                                    <label>Client:</label><br />
                                    <select id="ddlClient"></select>
                                </td>
                                <td style="width:100px">
                                    <label>Client Location:</label><br />
                                    <select id="ddlLoc"></select>
                                </td>
                                <td style="width:100px"><label>Shift</label><br />
                                    <select id="ddlShift"></select>
                                </td>
                                <td style="width:306px"><label>Dept:</label><br />
                                    <select id="ddlDepartment"></select>
                                </td>
                            </tr>
                            <tr>
                                <td><label>Date:</label><br />
                                   <div><input type="text" id="startDate" size="10"/></div>                             
                                </td>
                                <td>
                                    <label>Dispatch Location:</label><br />
                                    <select id='ddlLocation'>
                                        <option value='-1'>Choose Location</option>
                                    </select>
                                </td>
                                <td>
                                    <input id="btnCurEmployees" type="button" title="Get Employees On Roster For Given Date" style="background-color:LightBlue" value="GO" />                            
                                </td>
                                                   
                                <td class="shiftStart">
                                        <span>Start:</span><span id="shiftStart">--:--</span>
                                        <span>End:</span><span id="shiftEnd">--:--</span>
                                        <span>Break:</span><span id="break">0.00</span>
                                        <input type='hidden' id='trackStart' />
                                        <input type='hidden' id='trackEnd' />
                                 </td>
                                 <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <table><tr><td>
                                                    <input id="btnEmail" type="button" onclick="emailRosters()" value="Email"/>   
                                                </td></tr>
                                                <tr><td>
                                                    <input id="btnPrint" type="button" onclick="printRosters()" value="Print"/>                                                   
                                                </td></tr></table>
                                            </td>
                                            <td>
                                                <table><tr><td>
                                                    <input type="radio" disabled="disabled" name="group" checked="checked" /><span>Full Client</span>
                                                </td>
                                                <td>
                                                    <span style="padding-left:14px">Temps:</span></td><td><span style="padding-left:6px" id="tempCount">0</span>
                                                </td>
                                                </tr>
                                                <tr><td>
                                                    <input type="radio" disabled="disabled" name="group" /><span>Cur. Shift All Depts.</span>
                                                </td>
                                                <td>
                                                    <span style="padding-left:14px">Direct:</span></td><td><span style="padding-left:6px" id="directCount">0</span>
                                                </td>
                                                </tr>
                                                <tr>
                                                <td>
                                                    <input id="temps" disabled="disabled" type="checkbox"checked="checked" /><span>Temps Only</span>
                                                </td>
                                                <td>
                                                <span style="padding-left:14px">Total:</span></td><td><span style="padding-left:6px" id="totalCount">0</span>
                                                </td>
                                                </tr></table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                                        <!--<asp:HyperLink NavigateUrl="~/auth/PrintRoster.aspx" runat="server" Target="_blank" ID="lnkPrint" Text="Print" ></asp:HyperLink>-->                         
                                 <!--</td>-->
                            </tr>
                        </table>
                    </fieldset>
                    </div>
                </td>
            </tr>
            <tr><td>
                <div style="width:auto; height:auto">
                    <fieldset>
                    <legend>Available Employees</legend>
                    <div id="availableEmp">
                    </div>
                     </fieldset>
                </div>
            </td>
            <td>
                <div style="width:auto; height:auto">
                    <fieldset>
                    <legend>Employees on Roster</legend>
                    <div id="currentEmp">
                    </div>
                     </fieldset>
                </div>
            </td></tr>
            </table>
    </div>
<script language="javascript" src="../Scripts/Rosters.js" type="text/javascript"></script>
