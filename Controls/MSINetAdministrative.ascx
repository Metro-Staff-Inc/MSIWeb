<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetAdministrative.ascx.cs" Inherits="MSI.Web.Controls.MSINetAdministrative" %>
           
            <br />
                <table class="ClientPreferences">
                    <tr class="Row">
                        <td>
                            Display Job Codes
                        </td>
                        <td>
                            <asp:RadioButton GroupName="djcGroup" ID="djcYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="djcGroup" ID="djcNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Pay Rates
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dprGroup" ID="dprYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dprGroup" ID="dprNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Send Email Notification / Approval
                        </td>
                        <td>
                            <asp:RadioButton GroupName="sahGroup" ID="nhrYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="sahGroup" ID="nhrNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Must Approve Hours
                        </td>
                        <td>
                            <asp:RadioButton GroupName="ahGroup" ID="ahYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="ahGroup" ID="ahNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Generate Invoices
                        </td>
                        <td>
                            <asp:RadioButton GroupName="giGroup" ID="giYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="giGroup" ID="giNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Shift Schedule in Ticket Tracker
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dssGroup" ID="dssYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dssGroup" ID="dssNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Enable Punch Reporting
                        </td>
                        <td>
                            <asp:RadioButton GroupName="eprGroup" ID="eprYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="eprGroup" ID="eprNo" runat="server" /> No
                        </td>
                    </tr>                    
                    <tr class="AltRow">
                        <td>
                            Show Exact Punch Times in Employee History
                        </td>
                        <td>
                            <asp:RadioButton GroupName="septGroup" ID="septYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="septGroup" ID="septNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Show Exact Punch Times in Ticket Tracking
                        </td>
                        <td>
                            <asp:RadioButton GroupName="ttelpGroup" ID="ttelpYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="ttelpGroup" ID="ttelpNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Weekly Reports From Sunday to Saturday
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dwrGroup" ID="dwrYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dwrGroup" ID="dwrNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Display Weekly Reports From Saturday to Friday
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dwr3Group" ID="dwrSFYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dwr3Group" ID="dwrSFNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Weekly Reports From Wednesday to Tuesday
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dwr2Group" ID="dwrWTYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dwr2Group" ID="dwrWTNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Enable Maintain Pay Rates Link
                        </td>
                        <td>
                            <asp:RadioButton GroupName="prmlGroup" ID="prmlYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="prmlGroup" ID="prmlNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Bonuses in Weekly Hours Report
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dbonGroup" ID="dbonYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dbonGroup" ID="dbonNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Display Temp Employees in Ticket Tracker
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dTmpsGroup" ID="dTempsYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dTmpsGroup" ID="dTempsNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Display Starting Date (date of first punch)
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dsdGroup" ID="dsdYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dsdGroup" ID="dsdNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Display Break Times and Minutes Late
                        </td>
                        <td>
                            <asp:RadioButton GroupName="dbtGroup" ID="dbtYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="dbtGroup" ID="dbtNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Use Exact Punch Times
                        </td>
                        <td>
                            <asp:RadioButton GroupName="deptGroup" ID="deptYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="deptGroup" ID="deptNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                        <td>
                            Roster based pay rates
                        </td>
                        <td>
                            <asp:RadioButton GroupName="rbprGroup" ID="rbprYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="rbprGroup" ID="rbprNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td>
                            Show locations in Hours Report
                        </td>
                        <td>
                            <asp:RadioButton GroupName="slhrGroup" ID="slhrYes" runat="server"/> Yes
                            <asp:RadioButton GroupName="slhrGroup" ID="slhrNo" runat="server" /> No
                        </td>
                    </tr>
                    <tr class="Row">
                    <td>
                        Display Weekly Reports From Friday to Thursday
                    </td>
                    <td>
                        <asp:RadioButton GroupName="dwr4Group" ID="dwrFTYes" runat="server"/> Yes
                        <asp:RadioButton GroupName="dwr4Group" ID="dwrFTNo" runat="server" /> No
                    </td>
                </tr>
                    <tr>
                        <td>
                            <asp:Button id="btnSubmitChanges" text="Update" tooltip="Update Client Preferences" onclick="btnGo_Click" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="lblPreferencesUpdated" EnableViewState="true" Text="Preferences Updated!" Visible="false" runat="server"/>
                        </td>
                    </tr>
               </table>
