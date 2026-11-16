<%@ Page Title="Local component | Pit Wall" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocalComponent.aspx.cs" Inherits="PitWallTryIt.LocalComponent" %>
<%@ Register Src="~/CaptchaControl.ascx" TagPrefix="uc" TagName="Captcha" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- TryIt page links for every local component --%>

    <section class="hero">
        <div class="eyebrow yellow">Local component</div>
        <h1>Pop the <span class="accent">hood.</span></h1>
        <p class="lead">Every local component the team built, with a button to test it. Some of these run behind the scenes in the app, so this page makes them visible.</p>
    </section>

    <%-- ===== Encrypt: Ayomide Oshilaja (PitWallSecurity.dll) ===== --%>
    <section class="section" id="encrypt">
        <div class="card">
            <span class="badge">DLL</span>
            <h3 style="margin-top:10px;">Encrypt By: Ayomide Oshilaja</h3>
            <p class="muted">PitWallSecurity.dll. Input: string. Output: Base64 AES ciphertext. Try "Hello F1". The same text always gives the same result.</p>
            <div class="form-row">
                <div>
                    <label>Plain text</label>
                    <asp:TextBox ID="txtPlain" runat="server" placeholder="Type any text" />
                </div>
                <asp:Button ID="btnEncrypt" runat="server" Text="Run Encrypt" OnClick="btnEncrypt_Click" CssClass="btn btn-yellow" />
            </div>
            <div class="output"><asp:Label ID="lblCipher" runat="server" /></div>
        </div>
    </section>

    <%-- ===== Decrypt: Gabriela Martinez (PitWallSecurity.dll) ===== --%>
    <section class="section" id="decrypt">
        <div class="card">
            <span class="badge">DLL</span>
            <h3 style="margin-top:10px;">Decrypt By: Gabriela Martinez</h3>
            <p class="muted">PitWallSecurity.dll. Input: Base64 ciphertext string. Output: the original plain text. Run Encrypt above with "Hello F1", then paste the result here.</p>
            <div class="form-row">
                <div>
                    <label>Ciphertext (Base64)</label>
                    <asp:TextBox ID="txtCipher" runat="server" placeholder="Paste the Encrypt output" />
                </div>
                <asp:Button ID="btnDecrypt" runat="server" Text="Run Decrypt" OnClick="btnDecrypt_Click" CssClass="btn btn-yellow" />
            </div>
            <div class="output"><asp:Label ID="lblPlain" runat="server" /></div>
        </div>
    </section>

        <%-- ===== Cookies: Gabriela Martinez ===== --%>
    <section id="cookies">
        <div class="card">
            <span class="badge">Cookie</span>
            <h3 style="margin-top:10px;">My F1 Preferences By: Gabriela Martinez</h3>
            <p class="muted">Saves your username, favorite driver and favorite team in a browser cookie (RWH_F1Prefs) for 7 days.</p>

            <label>Username</label>
            <asp:TextBox ID="txtPrefUser" runat="server" MaxLength="30" />

            <div class="form-row">
                <div>
                    <label>Favorite driver</label>
                    <asp:DropDownList ID="ddlDriver" runat="server">
                        <asp:ListItem Text="-- choose a driver --" Value="" />
                        <asp:ListItem>Max Verstappen</asp:ListItem>
                        <asp:ListItem>Yuki Tsunoda</asp:ListItem>
                        <asp:ListItem>Lando Norris</asp:ListItem>
                        <asp:ListItem>Oscar Piastri</asp:ListItem>
                        <asp:ListItem>Charles Leclerc</asp:ListItem>
                        <asp:ListItem>Lewis Hamilton</asp:ListItem>
                        <asp:ListItem>George Russell</asp:ListItem>
                        <asp:ListItem>Kimi Antonelli</asp:ListItem>
                        <asp:ListItem>Fernando Alonso</asp:ListItem>
                        <asp:ListItem>Lance Stroll</asp:ListItem>
                        <asp:ListItem>Pierre Gasly</asp:ListItem>
                        <asp:ListItem>Franco Colapinto</asp:ListItem>
                        <asp:ListItem>Alexander Albon</asp:ListItem>
                        <asp:ListItem>Carlos Sainz</asp:ListItem>
                        <asp:ListItem>Isack Hadjar</asp:ListItem>
                        <asp:ListItem>Liam Lawson</asp:ListItem>
                        <asp:ListItem>Nico Hulkenberg</asp:ListItem>
                        <asp:ListItem>Gabriel Bortoleto</asp:ListItem>
                        <asp:ListItem>Esteban Ocon</asp:ListItem>
                        <asp:ListItem>Oliver Bearman</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <label>Favorite team</label>
                    <asp:DropDownList ID="ddlTeam" runat="server">
                        <asp:ListItem Text="-- choose a team --" Value="" />
                        <asp:ListItem>Red Bull Racing</asp:ListItem>
                        <asp:ListItem>McLaren</asp:ListItem>
                        <asp:ListItem>Ferrari</asp:ListItem>
                        <asp:ListItem>Mercedes</asp:ListItem>
                        <asp:ListItem>Aston Martin</asp:ListItem>
                        <asp:ListItem>Alpine</asp:ListItem>
                        <asp:ListItem>Williams</asp:ListItem>
                        <asp:ListItem>Racing Bulls</asp:ListItem>
                        <asp:ListItem>Kick Sauber</asp:ListItem>
                        <asp:ListItem>Haas</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="hero-actions" style="margin-top:0;">
                <asp:Button ID="btnSavePrefs" runat="server" Text="Save preferences" OnClick="btnSavePrefs_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnLoadPrefs" runat="server" Text="Load preferences" OnClick="btnLoadPrefs_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnClearPrefs" runat="server" Text="Clear preferences" OnClick="btnClearPrefs_Click" CssClass="btn" />
            </div>
            <div class="output"><asp:Label ID="lblPrefs" runat="server" /></div>
        </div>
    </section>

    <%-- ===== Captcha: Izzy Junio ===== --%>
    <section id="captcha">
    <div class="card">
        <span class="badge">User control</span>
        <h3 style="margin-top:10px;">Captcha By: Izzy Junio</h3>
        <p class="muted">
            CaptchaControl.ascx. Calls the Captcha WCF service (GetVerifierString and GetImage) from the
            server, keeps the answer in Session, and checks what you type. Input: the characters you read.
            Output: Correct or Incorrect. Use "New image" for another one.
        </p>
        <uc:Captcha ID="captchaTryIt" runat="server" />
        <div class="form-row">
            <asp:Button ID="btnCheckCaptcha" runat="server" Text="Check CAPTCHA" OnClick="btnCheckCaptcha_Click" CssClass="btn btn-yellow" />
        </div>
        <div class="output"><asp:Label ID="lblCaptchaResult" runat="server" /></div>
    </div>
    </section>


        <%-- ===== Session state: Ayomide Oshilaja ===== --%>
    <section id="session">
        <div class="card">
            <span class="badge">Session state</span>
            <h3 style="margin-top:10px;">Session state By: Ayomide Oshilaja</h3>
            <p class="muted">Remembers your favorite track zip code for this visit only (until you close the browser or after 20 minutes with no activity). The Track weather box on Race weekend fills it in for you. Try 33054 (Miami).</p>
            <div class="form-row">
                <div>
                    <label>Favorite track zip</label>
                    <asp:TextBox ID="txtSessionZip" runat="server" placeholder="e.g. 33054" />
                </div>
                <asp:Button ID="btnSaveSession" runat="server" Text="Save to session" OnClick="btnSaveSession_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnShowSession" runat="server" Text="Show session" OnClick="btnShowSession_Click" CssClass="btn btn-yellow" />
                <asp:Button ID="btnClearSession" runat="server" Text="Clear session" OnClick="btnClearSession_Click" CssClass="btn" />
            </div>
            <div class="output"><asp:Label ID="lblSession" runat="server" /></div>
        </div>
    </section>


    <%-- ===== Global.asax: Ayomide Oshilaja ===== --%>
    <section id="appstate">
        <div class="card">
            <span class="badge">Global.asax</span>
            <h3 style="margin-top:10px;">App events By: Ayomide Oshilaja</h3>
            <p class="muted">
                Global.asax runs behind the scenes. <strong>Application_Start</strong> sets the visitor counter
                you can see in the footer of every page. <strong>Application_Error</strong> catches any crash
                and sends the visitor to the "Something went wrong" error page instead.
            </p>
        </div>
    </section>
</asp:Content>