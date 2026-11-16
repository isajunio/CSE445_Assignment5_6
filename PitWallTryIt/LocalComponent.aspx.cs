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

        // Gabriela Martinez - Decrypt TryIt: turns Encrypt's output back into the original text
        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            string cipher = txtCipher.Text.Trim();
            if (cipher.Length == 0)
            {
                lblPlain.Text = "Paste a ciphertext first.";
                return;
            }

            try
            {
                lblPlain.Text = HttpUtility.HtmlEncode(PitWallCrypto.Decrypt(cipher));
            }
            catch (FormatException)
            {
                lblPlain.Text = "That is not valid Base64 text.";
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                lblPlain.Text = "Could not decrypt: this text was not made by Encrypt.";
            }
        }

        //=================== Section: TO-DO: cookie save/read click handlers (Gaby) ===============================

        //=================== Section: TO-DO: cookie save/read click handlers (Gaby) ===============================


        //=================== Section: TO-DO:Captcha check click handler (Izzy) ===============================
        protected void btnCheckCaptcha_Click(object sender, EventArgs e)
        {
            bool ok = captchaTryIt.IsValid();
            lblCaptchaResult.Text = ok
                ? "Correct. The CAPTCHA was solved."
                : "Incorrect. A new image was loaded.";
            if (ok) captchaTryIt.NewChallenge();   // answer is used up, so show a new image
        }


    }
}