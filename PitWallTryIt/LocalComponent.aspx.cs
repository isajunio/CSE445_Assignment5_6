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

        //=================== Section: TO-DO: cookie save/read click handlers (Gaby) ===============================

        //=================== Section: TO-DO: cookie save/read click handlers (Gaby) ===============================


        //=================== Section: TO-DO:Captcha check click handler (Izzy) ===============================



    }
}