using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PitWallSecurity;


namespace PitWallTryIt
{
    public partial class LocalComponent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //=================== Section: Ayomide Oshilaja ===============================
            protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            lblCipher.Text = string.IsNullOrEmpty(txtPlain.Text)
                ? "Type something first."
                : HttpUtility.HtmlEncode(PitWallCrypto.Encrypt(txtPlain.Text));
        }

        // ======================= Session state TryIt ==================================
        // saves the favorite track zip for this visitor's session only
        protected void btnSaveSession_Click(object sender, EventArgs e)
        {
            string zip = txtSessionZip.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(zip, @"^\d{5}$"))
            {
                lblSession.Text = "Enter a 5-digit zip code.";
                return;
            }
            Session["TrackZip"] = zip;
            lblSession.Text = "Saved " + zip + " to your session. Open Race weekend: the zip box is already filled in.";
        }

        // Shows what this visitor's session holds right now
        protected void btnShowSession_Click(object sender, EventArgs e)
        {
            string zip = Session["TrackZip"] as string;
            lblSession.Text = "Session ID: " + HttpUtility.HtmlEncode(Session.SessionID) + "<br/>"
                + "Favorite track zip: " + (string.IsNullOrEmpty(zip) ? "(nothing saved)" : HttpUtility.HtmlEncode(zip)) + "<br/>"
                + "Session timeout: " + Session.Timeout + " minutes";
        }
 
        // Removes the saved zip from this visitor's session
        protected void btnClearSession_Click(object sender, EventArgs e)
        {
            Session.Remove("TrackZip");
            lblSession.Text = "Session cleared. Race weekend goes back to the default zip.";
        }


        //=================== Section: TO-DO: Decrypt (Gaby) ===============================


        //=================== Section: TO-DO: cookie save/read click handlers (Gaby) ===============================


        //=================== Section: TO-DO:Captcha check click handler (Izzy) ===============================



    }
}