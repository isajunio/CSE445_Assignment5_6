using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Shows the last error message saved by Application_Error
            lblError.Text = HttpUtility.HtmlEncode(Application["LastError"] as string ?? "");
        }
    }
}