<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.master" AutoEventWireup="true" CodeFile="HoursReport_Bootstrap.aspx.cs" Inherits="MSI.Web.MSINet.HoursReport" %>

<%-- Add content controls here --%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<div class="container-fluid">
  <div class="well-fluid">
  <div class="row">
    <div class="col-sm-2">
        <div class="btn-group">
            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                Week Ending: <span class="selection" style="margin-right:5px"></span><span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu">
                <li><a tabindex="-1" href="#">Item I</a></li>
                <li><a tabindex="-1" href="#">Item II</a></li>
                <li><a tabindex="-1" href="#">Item III</a></li>
                <li class="divider"></li>
                <li><a tabindex="-1" href="#">Other</a></li>
            </ul>
        </div>
    </div>
    <div class="col-sm-2">
        <label class="checkbox-inline"><input type="checkbox" value=""/>Show all employees scheduled to work</label>
    </div>
    <div class="col-sm-2">
        <label class="checkbox-inline"><input type="checkbox" value=""/>Show all punches (including exceptions)</label>
    </div>
    <div class="col-sm-3">
        <label class="radio-inline">
            <input type="radio" name="optradio"/>Sort by Department
        </label>
        <label class="radio-inline">
            <input type="radio" name="optradio"/>Sort by Shift
        </label>
    </div>
    <div class="col-sm-3">
        <button type="button" class="btn btn-primary btn-md">View Hours Report</button>
        <button type="button" class="btn btn-primary btn-md">Submit Hours</button>
    </div>
  </div>
  </div>
</div>
    <script type="text/javascript" id="\">
        jQuery(document).ready(
        function () {
        //alert("Hello!");
        $(".dropdown-menu li a").click(function () {

            $(this).parents(".btn-group").find('.selection').text($(this).text());
            $(this).parents(".btn-group").find('.selection').val($(this).text());

        });
    });
    </script>
</asp:Content>
