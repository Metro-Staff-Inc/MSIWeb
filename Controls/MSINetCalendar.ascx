<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MSINetCalendar.ascx.cs" Inherits="Controls_MSINetCalendar" %>
<asp:Label ID="lblDateTime" runat="server" CssClass="InputTableLabelText" Text="Period Start:"
    Width="122px"></asp:Label><br />
<asp:TextBox ID="txtDateTime" runat="server" Width="178px" CssClass="InputTableInputLabelText"></asp:TextBox>
    <asp:Button Height="20px"
    ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="..." /><asp:Calendar
        ID="cdrPeriod" runat="server" 
        OnDayRender="cdrPeriod_DayRender" 
        OnSelectionChanged="cdrPeriod_SelectionChanged" 
        OnVisibleMonthChanged="cdrPeriod_VisibleMonthChanged"
        SelectedDayStyle-BackColor="Red">
        </asp:Calendar>
