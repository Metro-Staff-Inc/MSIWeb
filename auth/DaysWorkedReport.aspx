<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DaysWorkedReport.aspx.cs" Inherits="MSI.Web.MSINet.DaysWorkedReport" %>

<%@ Register Src="../Controls/MSINetDaysWorkedReport.ascx" TagName="MSINetDaysWorkedReport"
    TagPrefix="uc6" %>

<%@ Register Src="~/Controls/MSINetSectionHeader.ascx" TagName="MSINetSectionHeader"
    TagPrefix="uc4" %>

<%@ Register Src="~/Controls/MastHead.ascx" TagName="MastHead" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/MSINetSubHeader.ascx" TagName="MSINetSubHeader" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/MSINetMainMenu.ascx" TagName="MSINetMainMenu" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Days Worked Report</title>
    <script type="text/javascript"
        src = "https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.min.js">
    </script>
    <link href="~/includes/MSIMain.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
    
    function getScrollY() 
    {
        var scrOfY = 0;
        if( typeof( window.pageYOffset ) == 'number' ) 
        {
            //Netscape compliant
            scrOfY = window.pageYOffset;
            //scrOfX = window.pageXOffset;
        } 
        else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) 
        {
            //DOM compliant
            scrOfY = document.body.scrollTop;
            //scrOfX = document.body.scrollLeft;
        } 
        else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) 
        {
            //IE6 standards compliant mode
            scrOfY = document.documentElement.scrollTop;
            //scrOfX = document.documentElement.scrollLeft;
        }
        return scrOfY;
    }    
    
    function getCookie(c_name)
    {
        var i,x,y,ARRcookies=document.cookie.split(";");
        for (i=0;i<ARRcookies.length;i++)
        {
            x=ARRcookies[i].substr(0,ARRcookies[i].indexOf("="));
            y=ARRcookies[i].substr(ARRcookies[i].indexOf("=")+1);
            x=x.replace(/^\s+|\s+$/g,"");
            if (x==c_name)
            {
                return unescape(y);
            }
        }
    }

    function setCookie(c_name,value,exdays)
    {
        var exdate=new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value=escape(value) + ((exdays==null) ? "" : "; expires="+exdate.toUTCString());
        document.cookie=c_name + "=" + c_value;
    }

    function checkCookie()
    {
        var scrollPos=getCookie("scrPos");
        if( scrollPos != null && scrollPos != "" )
        {
            window.scrollTo(0, scrollPos);
        }
    }

    function toggleDeactivatedEmployees() {
        //alert( "toggle!");
        if( $('#btnActive').val() == "Hide Deactivated/DNR'd Employees" ){
            $('.trDWItem[active]').hide();
            $('#btnActive').val("Show Deactivated/DNR'd Employees");
        }
        else {
            $('.trDWItem[active]').show();
            $('#btnActive').val("Hide Deactivated/DNR'd Employees");
            /* in case we turned on any ineligible employees */
            if ($('#btnEligible').val() == "Show All Employees") {
                $('.trDWItem:not([hire])').hide();                
            }
        }
        var rows = $('tr.trDWItem:visible');
        rows.removeClass('trDWItemAlt');
        $('tr.trDWItem:visible:even').addClass('trDWItemAlt');
    }

    function toggleAvailableEmployees() {
        if ($('#btnEligible').val() == "View Eligible Employees Only") {
            $('.trDWItem:not([hire])').hide();
            $('#btnEligible').val("Show All Employees");
        }
        else {
            $('.trDWItem:not([hire])').show();
            $('#btnEligible').val("View Eligible Employees Only");
            /* in case any DNR'd employees became visible */
            if ($('#btnActive').val() == "Show Deactivated/DNR'd Employees") {
                $('.trDWItem[active]').hide();
            }
        }

        var rows = $('tr.trDWItem:visible');
        rows.removeClass('trDWItemAlt');
        $('tr.trDWItem:visible:even').addClass('trDWItemAlt');
    }

</script>
    
</head>
<body onload="checkCookie()" onunload="var offset = getScrollY(); 
                                    setCookie('scrPos', offset, 1);">
  
<div align="center">    
    <form id="form1" runat="server" >
    
    <div>
        <uc1:MastHead ID="ctlMastHead" runat="server" />
    </div>
        <br />
        <uc2:MSINetSubHeader ID="ctlSubHeader" runat="server" />
        <br />
        <uc3:MSINetMainMenu ID="ctlMainMenu" runat="server" SelectedSection="DaysWorkedReport" />

        <uc4:MSINetSectionHeader ID="ctlSectionHeader" runat="server" SectionHeader="Days Worked Report" />
        <asp:Panel ID="pnlDaysWorkedReport" runat="server" HorizontalAlign="Center" Width="1200px">
            <div style="text-align:right"><asp:HyperLink ID="lnkPrinterFriendly" Target="_blank" NavigateURL="~/auth/DaysWorkedReport.aspx?print=1" runat="server" Text="Printer Friendly Version"></asp:HyperLink></div>
            <uc6:MSINetDaysWorkedReport id="ctlDaysWorkedReport" runat="server">
            </uc6:MSINetDaysWorkedReport>
        </asp:Panel>
    </form>
 </div>    
</body>
</html>
