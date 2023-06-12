using System;
using System.Web.Security;

namespace MSI.Web.Controls
{
    public partial class Login : BaseMSINetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUserID.Focus();
            }
        }
        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            if (Membership.ValidateUser(txtUserID.Text, txtPassword.Text))
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(txtUserID.Text, false);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(txtUserID.Text, false);
                    if (_isPDA)
                    {
                        Response.Redirect("~/auth/pda/MainMenu.aspx");
                    }
                    else
                    {
                        System.Web.Security.MembershipUser user = Membership.Provider.GetUser(txtUserID.Text, true);
                        string pwd = Membership.Provider.GetPassword(txtUserID.Text, "");
                        System.TimeSpan dt = user.LastActivityDate  - user.LastPasswordChangedDate;
                        if( (user.Comment != null) && (user.Comment.Equals("Need Change Password") && dt > new System.TimeSpan(90, 0, 0, 0)))
                        {
                            //user must change password
                            Response.Redirect("~/auth/AccountManagement.aspx");
                        }
                        else
                        {
                            Response.Redirect("~/auth/MainMenu.aspx");
                        }
                    }
                }
            }
            else
            {
                lblValidationMessage.Visible = true;
                lblValidationMessage.Text = "Invalid User ID / Password.";
                txtPassword.Text = "";
                txtUserID.Focus();
            }
        }
    }
}