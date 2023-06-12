using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using MSI.Web.Controls;
//using System.Web.UI.WebControls;
//namespace MSI.Web.Controls
//{
public partial class Controls_MSINetEnrollEmployee : BaseMSINetControl
    {
        string[] states = {
        "AL",
        "AK",
        "AZ", 
        "AR", 
        "CA", 
        "CO", 
        "CT",
        "DE",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY"
  };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ddlState.SelectedIndex = 12;
            ddlState.DataSource = states;
            ddlState.DataBind();
        }
        protected string GetEnteredEmployeeID()
        {
            return "JonathanIsKing";
        }
    }
//}