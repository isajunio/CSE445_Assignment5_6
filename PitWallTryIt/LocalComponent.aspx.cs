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