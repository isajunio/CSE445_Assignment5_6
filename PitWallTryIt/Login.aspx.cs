using PitWallTryIt.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    public partial class Login : System.Web.UI.Page
    {
        //login new users
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack || !User.Identity.IsAuthenticated) return;

            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                Response.Redirect(HomeFor(User));
                return;
            }

            lblInfo.Text = "You're signed in as " + HttpUtility.HtmlEncode(User.Identity.Name)
                         + ". That page needs a Staff login.";
            rblRole.SelectedValue = "Staff";



        }

        //checks the username and password 
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string user = txtUser.Text.Trim();
            string role = rblRole.SelectedValue;

            // Check the right XML file
            bool ok = role == "Staff"
                ? AccountStore.ValidateStaff(user, txtPass.Text)
                : AccountStore.ValidateMember(user, txtPass.Text);

            if (!ok)
            {
                lblError.Text = "Wrong username or password.";
                return;
            }

            AuthHelper.SignIn(user, role, chkRemember.Checked);

            // Back to the page they wanted, or their home page
            string target = string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])
                ? (role == "Staff" ? "~/Staff.aspx" : "~/Member.aspx")
                : FormsAuthentication.GetRedirectUrl(user, chkRemember.Checked);

            Response.Redirect(target);
        }

        //login users land in the member page
        private static string HomeFor(IPrincipal user)
        {
            return user.IsInRole("Staff") ? "~/Staff.aspx" : "~/Member.aspx";
        }


    }
}