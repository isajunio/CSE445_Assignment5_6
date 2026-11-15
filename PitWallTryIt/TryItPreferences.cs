// Gabriela Martinez-TryIt handlers for the cookie component
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    public partial class TryIt
    {
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
    }
}