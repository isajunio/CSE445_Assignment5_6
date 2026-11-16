using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitWallTryIt
{
    public partial class CaptchaControl : System.Web.UI.UserControl
    {

        private const string SessionKey = "CaptchaText";

        private static string ServiceBase
        {
            get
            {
                return ConfigurationManager.AppSettings["CaptchaServiceBase"]
                         ?? "http://localhost:5001/Captcha.svc/";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) NewChallenge();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            NewChallenge();
        }

        // Asks the service for a new string and its image.
        public void NewChallenge()
        {
            lblCaptchaError.Text = "";
            txtCaptcha.Text = "";
            try
            {
                using (var wc = new WebClient())
                {
                    string text = wc.DownloadString(ServiceBase + "GetVerifierString/5").Trim().Trim('"');
                    byte[] png = wc.DownloadData(ServiceBase + "GetImage/" + Uri.EscapeDataString(text));
                    Session[SessionKey] = text;
                    imgCaptcha.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(png);
                }
            }
            catch (WebException)
            {
                Session.Remove(SessionKey);
                lblCaptchaError.Text = "CAPTCHA service unavailable.";
            }
        }

        // True only if the typed text matches. Each answer works once.
        public bool IsValid()
        {
            lblCaptchaError.Text = "";            // drop any message left over from a previous attempt
            string expected = Session[SessionKey] as string;
            Session.Remove(SessionKey);
            bool ok = expected != null &&
                      string.Equals(txtCaptcha.Text.Trim(), expected, StringComparison.OrdinalIgnoreCase);
            if (!ok)
            {
                NewChallenge();
                lblCaptchaError.Text = "Incorrect CAPTCHA, try the new one.";
            }
            return ok;
        }
    }
}