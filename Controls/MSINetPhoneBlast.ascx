<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetPhoneBlast.ascx.cs" Inherits="MSI.Web.Controls.MSINetPhoneBlast" %>


<input type='hidden' id='capToken' runat="server"/>
<input type='hidden' id='roster2' />
<input type='hidden' id='roster3' />

    <div style="display:none" id="phoneBlastDiv" title="Auto dial all valid employees">
        <span class="pbMessage">Number of calls: </span>
        <select id="phoneBlastCount">
            <option value='1'>01</option>
            <option value='2'>02</option>
            <option value='3'>03</option>
            <option value='5'>05</option>
            <option value='10'>10</option>
            <option value='15'>15</option>
            <option value='25'>25</option>
            <option value='100'>100</option>
        </select>
    </div>

    <div style="display:none" id="callListDiv" title="Contact Employees in Search List">
        <table>
            <tbody>
                <tr>
                    <td>
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        <div id="picDiv" style="width:180px;height:240px; background-image: url('../Images/NotAvailable.jpg')"; >
                                            <img alt="" id="idBadge" src="" />
                                            <input type="hidden" id="arrayIdx" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h3 id="name"></h3>
                                        <h3 id="id"></h3>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tbody>
                                <tr>
                                    <td colspan="2">(<input id="area" type="text" size="3" />) <input id="prefix" size="3" />-<input id="last4" size="4"/></td>
                                </tr>
                                <tr>
                                    <td><span id="clientName"></span></td><td><span id="workDate"></span></td>
                                </tr>
                                <tr>
                                    <td><span id="shiftInfo"></span></td><td><span id="deptInfo"></span></td>
                                </tr>
                                <tr id="shiftStartEnd">
                                </tr>
                                <tr>
                                    <td><input id="addToRoster" type="button" value="ADD TO ROSTER" /></td>
                                </tr>
                                <tr>
                                    <td><input style="width: 110px; height: 110px;" id="call" type="button" value="CALL" disabled="disabled" /></td>
                                    <td><input style="width: 110px; height: 110px;text-align:center;" id="text" type="button" value="SEND&#13;&#10;CONFIRM&#13;&#10;TEXT" disabled="disabled" /></td>
                                </tr>
                                <tr>
                                    <td><input type="button" id="prev" value="PREV" /></td>
                                    <td><input type="button" id="next" value="NEXT" /></td>
                                </tr>
                            </tbody>
                        </table>                    
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        <table style="width:100%">
            <tbody id="employeeNotes">
            </tbody>
            <tbody>
                <tr style="width:100%;" >
                    <td rowspan="3"><textarea rows="6" cols="50"></textarea></td>
                    <td></td>
                </tr>
                <tr><td></td><td></td></tr>
                <tr>
                    <td></td>
                    <td><input type="button" value="UPDATE" disabled="disabled" id="updEmployeeNotes" /></td>
                </tr>
            </tbody>
        </table>
    </div>
    <h3 class='titleBar titleBarNormal'>Client Roster Parameters
        </h3>
        <div id="rosterParamsDiv" style="display: none;">
            <table id='rosterParams'>
                <thead>
                    <tr>
                        <th>CLIENT</th>
                        <th>LOCATION</th>
                        <th>SHIFT</th>
                        <th>DEPARTMENT</th>
                        <th>DATE</th>
                        <th>GET ROSTER</th>
                        <th>SHIFT</th>
                        <th>TRACKING</th>
                        <th>BREAK</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><select id="ddlClientRoster"></select></td>
                        <td><select id="ddlLocationRoster"></select></td>
                        <td><select id="ddlShiftRoster"></select></td>
                        <td><select id="ddlDepartmentRoster"></select></td>
                        <td><input id="dateRoster" type="text" size="8"/></td>
                        <td><input id="btnGetRoster" type="button" value="GO!" /></td>
                        <td>s: <span id="shiftStart">09:00 AM</span><br />e: <span id="shiftEnd">04:00 PM</span></td>
                        <td>s: <span id="trackingStart">07:00 AM</span><br />e: <span id="trackingEnd">07:00 PM</span></td>
                        <td><span id="break"></span></td>
                    </tr>
                </tbody>
            </table>
        </div>
    <h3 class='titleBar titleBarNormal'>Client Roster</h3>
        <div style="display: none;">
            <table id='clientRoster'>
                <thead>
                    <tr>
                        <th>#</th>
                        <th>OFFICE</th>
                        <th>ID</th>
                        <th>NAME</th>
                        <th id="subs" style="display: none;">SUBS</th>
                        <th colspan='4'><span>DAYS - SHIFT START/END</span>
                            <input id='updRosters' type='button' value='UPDATE' disabled="disabled" />
                            <input id='expRosters' type='button' value='EXPORT'/>
                            <input id='printRosters' type='button' value='PRINT' onclick="printRosterMenu()"/>
                            <input id='emailRosters' type='button' value='EMAIL' onclick="emailRosterMenu()"/></th>
                        <th>REMOVE</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    <h3 class='titleBar titleBarNormal'>Employee Search Options</h3>
        <div style="display: none;">
        <table class="container">
            <tbody>
                <tr><td>
                <table id="recruitTable">
                    <thead>
                        <tr>
                            <th>CLIENT</th>
                            <th>LOCATION</th>
                            <th>SHIFT</th>
                            <th>DEPARTMENT</th>
                            <th colspan="3">WORKED BETWEEN</th>
                            <th>SEARCH</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><select id="ddlClient"></select></td>
                            <td><select id="ddlLocation"></select></td>
                            <td><select id="ddlShift"></select></td>
                            <td><select id="ddlDepartment"></select></td>
                            <td><input id="startDate" type="text" size="8"/></td>
                            <td>AND</td>
                            <td><input id="endDate" type="text" size="8"/></td>
                            <td><input id="btnSubmit" value="GO!" type="button" /></td>
                        </tr>
                    </tbody>
            </table>
            </td></tr>
            <tr><td>
            <table id="aidentTable">
                <thead>
                    <tr>
                        <th>ID #</th>
                        <th>SEARCH ID</th>
                        <th>LAST NAME</th>
                        <th>FIRST NAME</th>
                        <th>SEARCH NAME</th>
                        <th>SKILL SET</th>
                        <th>SEARCH</th>
                        <th>CUSTOM LIST</th>
                        <th>SEARCH</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><input type="text" id="inpID" /></td>
                        <td><input type="button" value="GO!" id="btnSubmitID"/></td>
                        <td><input type="text" id="inpLastName" /></td>
                        <td><input type="text" id="inpFirstName" /></td>
                        <td><input type="button" value="GO!" id="btnSubmitName"/></td>
                        <td><select id="ddlSkillList">
                        </select></td>
                        <td><input type="button" value="GO!" id="btnSubmitSkill"/></td>
                        <td><select id="ddlCustomList">
                            <option value="0">Create New List..</option>
                        </select></td>
                        <td><input type="button" value="GO!" id="btnSubmitCustom"/></td>
                    </tr>
                </tbody>
            </table>
            </td></tr>
        </tbody>
        </table>
        </div>

        <h3 class='titleBar titleBarNormal'>Employee Search Results</h3>
        <div class='callTableDiv' style="display: none;">
        <table id="callTable">
            <thead>
                <tr>
                    <th colspan='11'>
                        <span>Phone Blast Call Size: </span>
                        <select id='maxPhoneBlastSize'>
                            <option value="1">01</option>
                            <option selected="selected" value="2">02</option>
                            <option value="03">03</option>
                            <option value="05">05</option>
                            <option value="10">10</option>
                            <option value="15">15</option>
                            <option value="20">20</option>
                            <option value="50">50</option>
                            <option value="100">100</option>
                        </select>
                    <!--<input id="mainPrint" type="button" value="PRINT" />-->
                    <input id="mainCall" type="button" value="PHONE BLAST" />
                    <input id="expCallData" type="button" value="EXPORT"/>

                    <span>Office: </span>
                    <select id='office'>
                        <option selected="selected" value='0'>- SELECT OFFICE -</option>
                    </select>

                    <span id='addToRosterSpan'>Add to Roster for: </span>
                    <select id="rosterUnits">
                        <option value='1'>DAILY</option>
                        <option value='7'>WEEKLY</option>
                        <option value='74000'>INDEFFINITE</option>
                    </select>
                    <input type="button" value="CLEAR POOL" id="btnPoolClear"/>
                    <input type="button" value="CREATE TEXT" id="btnCreateText"/>
                    </th>
                </tr>
                <tr>
                    <th><span class='index'>Index</span></th>
                    <th><span class='aident'>ID #</span></th>
                    <th><span class='name'>Name</span></th>
                    <th><input type="button" value="CALL LIST" id="callList"/></th>
                    <th><span>Days Worked</span></th>
                    <th><span>#</span></th>
                    <th><span>Call Status</span></th>
                    <th colspan='2'><span>Response / Roster</span></th>
                    <th><span>Contact</span></th>
                    <th><span>List</span></th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <h3 class='titleBarBP browserPhoneUnavailable'><span id='browserPhone'>Browser Phone Unavailable</span></h3>
    <div style="display: none;"></div>

        <style type="text/css">
            span
            {
                margin-left:4px;
            }
            .employeeInfo td
            {
                padding-left:40px;
            }
            #updRosters
            {
                margin-left:20px;
            }
            #addToRosterSpan
            {
                margin-left:32px;
            }
            #callTable, #clientRoster
            {
                background-color:#FFFFFF;
                width:1200px;
            }
            .odd
            {
                background-color:#DDDDFF;
            }
            #callTable th, #clientRoster th
            {
                background-color:#9999DD;
                font-size:larger;
            }
            th > input
            {
                font-size:10px;
            }
            .highLight
            {
                background-color:LightYellow;
            }
            .employeeRoot
            {
                font-size:larger;
            }
            .employeeRootBadPhoneNumber
            {
                background-color:#FFAAAA;
                font-size:larger;
            }
            .employeeRootAvailable
            {
                background-color:#AAFFAA;
                font-size:larger;
            }
            .employeeRootUnavailable
            {
                background-color:#FFAAAA;
                font-size:larger;
            }
            .browserPhoneAvailable
            {
                background-color:#367600;
                color:Black;
                font-size:larger;
            }
            .browserPhoneUnavailable
            {
                background-color:#AA2600;
                font-size:larger;
            }
            .titleBarHighlight
            {
                background-color:#5555FF;
            }
            .titleBarAlert
            {
                background-color:#FF5555;
            }
            .titleBarNormal
            {
                background-color:#003776;
            }
            .smallButton
            {
                font-size:10px;
                margin-left:8px;
            }
            .titleBarBP
            {
                color:White;
                font-size:18px;
                font-weight:bold;
                height:30px;
                margin:8px;
                width:1200px;
                padding-top:6px;
            }
            .titleBar
            {
                color:White;
                font-size:20px;
                font-weight:bold;
                height:28px;
                margin:8px;
                width:1200px;
                padding-top:6px;
            }
            .titleBar h3
            {
                float:right;
            }
            .no-close .ui-dialog-titlebar-close 
            {
                display: none;
            }
            .inpDate
            {
                background-color:#EEE;
            }
            #btnPoolClear
            {
                margin-left:64px;
            }
            .pbDnr
            {
                color:Red;
            }
        </style>
        <script type="text/javascript" src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js"></script>
        <script src="../Scripts/jquery.fixedheadertable.min.js" type="text/javascript"></script>
        <script src="../Scripts/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

        <script type="text/javascript" src="../javascriptOOXml/linq.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/ltxml-extensions.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-load.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-inflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/jszip-deflate.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/FileSaver.js"></script>
        <script type="text/javascript" src="../javascriptOOXml/openxml.js"></script>
        <script src="../javascriptOOXml/TemplateDocumentB64.js" type="text/javascript"></script>
		<script type="text/javascript" src="//static.twilio.com/libs/twiliojs/1.1/twilio.min.js"></script>
        <script src="../Scripts/RostersMain.js" type="text/javascript"></script>
        <script type="text/javascript" src="../Scripts/phoneBlast.js"></script>
        <script type="text/javascript" src="../scripts/phoneBlastDialer.js"></script>
