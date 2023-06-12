<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetTransport.ascx.cs" Inherits="MSI.Web.Controls.MSINetTransport" %>

    <div id="dateSelect">
        <table id="transportHeader">
            <thead>
                <tr>
                    <th>Start Date:</th>
                    <th>End Date:</th>
                    <th>Select Client:</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><input type="text" id="inpStartDt"/></td>
                    <td><input type="text" id="inpEndDt"/></td>
                    <td><select id="clientID">
                        <option value="0">ALL CLIENTS</option>
                        </select>
                    </td>
                    <td><input type="button" id="btnGo" value="GO" /></td>
                    <td></td>
                    <td><input disabled="disabled" type="button" id="exportExcel" value="Export To Excel" /></td>
                </tr>
            </tbody>
        </table>
    </div>
    <div style="width:1600px" id="employeeList"></div>
    <div id="map"></div>
