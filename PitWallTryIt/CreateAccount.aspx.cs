using PitWallTryIt.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    //sign up page By: Ayomide Oshilaja 
    //saves new member to member.xml
    public partial class CreateAccount : System.Web.UI.Page
    {
        //prevent logged in users from sign up again
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Member.aspx");
            }
        }

        //creates new member and saves to member.xml
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string user = txtUser.Text.Trim();
            string pass = txtPass.Text;

            if (!Regex.IsMatch(user, @"^[A-Za-z0-9_]{3,20}$"))
            {
                lblError.Text = "Username must be 3 to 20 letters, numbers or _.";
                return;
            }
            if (pass.Length < 6)
            {
                lblError.Text = "Password must be at least 6 characters.";
                return;
            }

            // CAPTCHA: stop here if the typed text is wrong. The control shows its own
            // error and loads a fresh image, because each CAPTCHA answer works only once.
            if (!captcha.IsValid()) return;

            if (AccountStore.MemberExists(user))
            {
                captcha.NewChallenge();   // the old CAPTCHA was already used, so load a new one
                lblError.Text = "That username is taken. Try another.";
                return;
            }

            AccountStore.AddMember(user, pass);  
            AuthHelper.SignIn(user, "Member", false); 
            Response.Redirect("~/Member.aspx");

        }


    }
}