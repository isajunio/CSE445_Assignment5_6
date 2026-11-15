using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Footer visitor counter.
            // increased by Izzy's Session_Start each time a new visitor arrives.
            lblVisitors.Text = (Application["Visitors"] ?? 0).ToString();

            bool loggedIn = Request.IsAuthenticated;

            // Shows if they are Member or Staff
            if (loggedIn)
            {
                string role = Page.User.IsInRole("Staff") ? "Staff" : "Member";
                lblUser.Text = HttpUtility.HtmlEncode(Page.User.Identity.Name) + " · " + role;
            }

            lnkJoin.Visible = !loggedIn;

        }
    }
}