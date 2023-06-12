<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Roster.aspx.cs" Inherits="Reports_Roster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2><span runat="server" id="lblClient"></span>, <span runat="server" id="lblDate"></span></h2>
    </div>
        <asp:Repeater ID="rptrRoster" OnItemDataBound="rptrRoster_DataBinding" runat="server">
            <HeaderTemplate>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <th colspan="2" runat="server" id="tdShiftType"></th>
                        <th colspan="3" runat="server" id="tdDeptName"></th>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="0" border="1">
            </HeaderTemplate>
            <ItemTemplate>
                    <tr runat="server" id="trHeader" visible="false">
                        <th runat="server" id="thEmpCnt"><p>#</p></th>
                        <th runat="server" id="thMSIId"><p>MSI ID#</p></th>
                        <th runat="server" id="tdSuncastHeader"><p>Suncast ID#</p></th>
                        <th runat="server" id="thLastName"><p>Last Name</p></th>
                        <th runat="server" id="thFirstName"><p>First Name</p></th>
                        <th runat="server" id="thHoursWorked"><p>Hours Worked</p></th>
                    </tr>
                    <tr runat="server">
                        <td runat="server" id="tdEmpCnt"></td>
                        <td runat="server" id="tdMsiId"></td>
                        <td runat="server" id="tdSuncastId"></td>
                        <td runat="server" id="tdLastName"></td>
                        <td runat="server" id="tdFirstName"></td>
                        <td></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
               </table>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
