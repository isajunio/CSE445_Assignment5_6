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

        // Gabriela Martinez - Cookie TryIt (uses PreferencesCookie in PreferenceCookies.cs)
        protected void btnSavePrefs_Click(object sender, EventArgs e)
        {
            string username = txtPrefUser.Text.Trim();
            string driver = ddlDriver.SelectedValue;
            string team = ddlTeam.SelectedValue;

            if (username.Length == 0 || username.Length > 30)
            {
                lblPrefs.Text = "Error: enter a username (1 to 30 characters).";
                return;
            }
            if (driver.Length == 0)
            {
                lblPrefs.Text = "Error: choose a favorite driver.";
                return;
            }
            if (team.Length == 0)
            {
                lblPrefs.Text = "Error: choose a favorite team.";
                return;
            }

            PreferencesCookie.Save(Response, username, driver, team);

            lblPrefs.Text = "Saved for " + HttpUtility.HtmlEncode(username)
                + ": " + HttpUtility.HtmlEncode(driver) + " / " + HttpUtility.HtmlEncode(team)
                + ". Cookie expires " + DateTime.Now.AddDays(PreferencesCookie.ExpiryDays).ToString("MMM d, yyyy") + ".";
        }
        protected void btnLoadPrefs_Click(object sender, EventArgs e)
        {
            F1Preferences prefs = PreferencesCookie.Load(Request);

            if (prefs == null || prefs.Username.Length == 0)
            {
                lblPrefs.Text = "No saved preferences found. Save some first.";
                return;
            }

            txtPrefUser.Text = prefs.Username;
            bool driverOk = SelectPreference(ddlDriver, prefs.Driver);
            bool teamOk = SelectPreference(ddlTeam, prefs.Team);

            lblPrefs.Text = "Loaded preferences for " + HttpUtility.HtmlEncode(prefs.Username) + ".";
            if (!driverOk || !teamOk)
            {
                lblPrefs.Text += "<br/>Note: a saved driver or team is not in the list anymore, so it was reset.";
            }
        }

        protected void btnClearPrefs_Click(object sender, EventArgs e)
        {
            PreferencesCookie.Clear(Response);

            txtPrefUser.Text = string.Empty;
            ddlDriver.SelectedIndex = 0;
            ddlTeam.SelectedIndex = 0;

            lblPrefs.Text = "Saved preferences were cleared.";
        }
        private bool SelectPreference(DropDownList list, string value)
        {
            list.ClearSelection();
            ListItem item = list.Items.FindByValue(value);
            if (item == null)
            {
                list.SelectedIndex = 0; 
                return false;
            }
            item.Selected = true;
            return true;
        }

        //=================== Section: TO-DO:Captcha check click handler (Izzy) ===============================



    }
}