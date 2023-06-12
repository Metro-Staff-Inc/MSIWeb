<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetClientInfo.ascx.cs" Inherits="MSI.Web.Controls.MSINetClientInfo" %>

<table>
    <tr>
        <td>
<table>
    <thead>
        <tr>
            <th><span>SELECT CLIENT:</span></th>
            <th><select id="selClient"><option value="2">American Litho</option><option value="3">Weber Palatine</option><option value="4">John B. San Fillipo</option><option value="5">Print & Mailing Solutions</option></select></th>
            <th><input type="button" value="GO" /></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><span>Active:</span></td>
            <td><select><option value="true">ACTIVE</option><option value="false">NOT ACTIVATED</option></select></td>
        </tr>
        <tr>
            <td><span>Work Week Begins On:</span></td>
            <td><select><option>MONDAY</option><option>TUESDAY</option><option>WEDNESDAY</option><option>THURSDAY</option><option>FRIDAY</option><option>SATURDAY</option><option>SUNDAY</option></select></td>
        </tr>
        <tr>
            <td>Address:</td>
        </tr>
        <tr>
            <td>Address Line 2:</td>
        </tr>
        <tr>
            <td>City:</td><td><input type="text" /></td>
            <td>State:</td><td><input type="text" /></td>
            <td>Zip:</td><td><input type="text" /></td>
        </tr>
        <tr>
            <td>Phone:</td><td><span>(</span><input type="text" /><span>)&nbsp</span><input type="text" /><span>&nbsp-&nbsp</span><input type="text" /></td>
        </tr>
    </tbody>
</table>
        </td>
        <td>
            <table>
                <tr>
                    <td>Shift:<br/><select><option value="1">1ST SHIFT</option><option value="2">2ND SHIFT</option><option value="3">3RD SHIFT</option><option value="A">SHIFT A</option><option value="5">SHIFT B</option><option value="6">SHIFT C</option><option value="7">SHIFT D</option></select></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
