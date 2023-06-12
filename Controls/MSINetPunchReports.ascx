<%@ Control Language="C#"  EnableViewState="true" AutoEventWireup="true" CodeFile="MSINetPunchReports.ascx.cs" Inherits="MSI.Web.Controls.MSINetPunchReports" %>
<%@ Register Src="~/Controls/MSINetCalendar.ascx"       TagName="MSINetCalendar"        TagPrefix="uc6" %>

<div style=" width:1000px;">
    <div style="float:left">
        <asp:Panel ID="Panel2" runat="server" BackColor="#E0E0E0" Width="1000px">
            <asp:Table ID="tblInput" runat="server" Width="100%" CellPadding="5" CellSpacing="0" BorderStyle="Solid" BorderWidth="1px" BorderColor="#3399FF">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <!--<uc6:MSINetCalendar id="ctlEndDate2" runat="server" DisplayMode="SingleDate" LabelText="From Date:"></uc6:MSINetCalendar>-->
                        <asp:Label ID="lblDateTime" runat="server" CssClass="InputTableLabelText" Text="Last Creation Date:" Width="122px"></asp:Label><br />
                        <asp:TextBox ID="txtDateTime" runat="server" Width="178px" CssClass="InputTableInputLabelText"></asp:TextBox>
                        <asp:Button Height="20px" ID="btnSelect" runat="server" OnClick="btnSelect_Click" Text="..." />
                        <asp:Calendar OnSelectionChanged="cdrEndDate_SelectionChanged" OnVisibleMonthChanged="cdrEndDate_VisibleMonthChanged" ID="cdrEndDate" runat="server" DisplayMode="SingleDate" LabelText="From Date:"></asp:Calendar>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="Label1" runat="server" CssClass="InputTableLabelText" Text="Search Back:"></asp:Label><br />
                        <asp:DropDownList id="ctlStartDate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LoadPunchRecordCreators">
                            <asp:ListItem Text="One Day" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Two Days" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Three Days" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Four Days" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Five Days" Value="5"></asp:ListItem>
                            <asp:ListItem Text="One Week" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Two Weeks" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Three Weeks" Value="21"></asp:ListItem>
                            <asp:ListItem Text="One Month" Value="31"></asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <asp:Label ID="lblDepartment" runat="server" CssClass="InputTableLabelText" Text="View punches made by:"></asp:Label><br />
                        <asp:DropDownList ID="cboUserList" runat="server" CssClass="InputTableInputLabelText" Width="208px"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell  VerticalAlign="Top" HorizontalAlign="center" CssClass="InputTableHeader" Width="230px">
                        <asp:Button id="btnGo" onclick="btnGo_Click" runat="server" Enabled="false" Text="View Punch Report"></asp:Button>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <asp:GridView ID="gvPunchRecord" EmptyDataText="No Punches Found in Range!" 
            runat="server" Width="1000px" CellPadding="4" 
            ForeColor="#333333" GridLines="None" >
            <AlternatingRowStyle CssClass="white"/>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle CssClass="grey" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server">
        </asp:ObjectDataSource>
    </div>
</div>
    <script type="text/javascript">
        $("#<%=gvPunchRecord.ClientID%> td").click(
	    function () {
	        var col = $(this).parent().children().index($(this));
	        //alert(col);

	        /* get child div if any */
	        var c = $("div", this);
	        //alert(c.length);
	        if (c.length >= 1 && col == 0) {
	            //alert(c.length);
	            var cpp = c.parentsUntil("tr");
	            //alert(cpp);
	            //alert(cpp.length);
	            cpp.removeClass("lightBlue");
	            cpp.removeClass("normalText");
	            c.remove();
	        }
	        else if (col == 0) {
	            var d = $('<div class="img"></div>', {});
	            var thisTr = $(this).parent();
	            d.appendTo($(this)).html(addHtml( thisTr )).addClass("lightRed");
	            //alert($(this).text());
	        }
	    });

	    function addHtml(thisTr) {

            var Months = new Array("Jan. ", "Feb. ", "Mar. ", "Apr. ", "May ", "June ", "July ", "Aug. ", "Sept. ", "Oct. ", "Nov. ", "Dec. ");
	        var name = thisTr.children().eq(0).text();
	        var id = thisTr.children().eq(2).text();
	        var dt = thisTr.children().eq(3).text();

	        var d = new Date(dt);
            
	        var day = d.getDate();
	        if (day <= 9)
	            day = '0' + day;

	        var hours;
	        var hrs = d.getHours();
            if( hrs <= 9 )
                hours = '0' + hrs;
            else
                hours = hrs;
                   
            var minutes = d.getMinutes();
            if (minutes <= 9)
                minutes = '0' + minutes;

            var seconds = d.getSeconds();
            if (seconds <= 9)
                seconds = '0' + seconds;

	        var imgDt = id + '__' + d.getFullYear() + (d.getMonth()+1) + day + '_' + hours + minutes + seconds;

	        var ret = "<table class=\"img\"><tr><td><img alt=\"Picture Not Available\"   src=\"http://msiwebtrax.com/dropbox/images/226/";
	        ret += imgDt + ".jpg\"/></td></tr><tr><td>" + name + "</td></tr><tr><td>" + Months[d.getMonth()] + day + ", " + d.getFullYear();
	        var hrs = d.getHours();
	        if (hrs > 12) {
	            hrs -= 12;
	            ret += "   " + hrs + ":" + minutes + ":" + seconds + " PM</td></tr></table>";
	        }
	        else {
	            if (hrs == 0)
	                hrs = 12;
                ret += "   " + hrs + ":" + minutes + ":" + seconds + " AM</td></tr></table>";
	        }
	        return ret;
	    }
        jQuery(document).ready(
    function () {
        $('#<%=gvPunchRecord.ClientID%> tr').hover(
   function () {
       var myClass = $(this).attr("class");
       $(this).addClass("lightBlue").removeClass(myClass);
   }, function () {
       $(this).removeClass("lightBlue");
       var row = $(this).parent().children().index($(this));
       if (row % 2 == 0)
           $(this).addClass("white");
       else
           $(this).addClass("grey");
   });
});
    </script>





