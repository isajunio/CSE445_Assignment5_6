<%@ Page Title="Local component | Pit Wall" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocalComponent.aspx.cs" Inherits="PitWallTryIt.LocalComponent" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- TryIt page for every local component --%>

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

    <%-- ===== Decrypt: Gabriela Martinez ===== --%>
    <section id="decrypt">
        <div class="card">
            <%-- TODO (Gaby): Decrypt TryIt. Copy the Encrypt card above and use:
                 TextBox txtCipher, Button btnDecrypt (OnClick="btnDecrypt_Click"), Label lblPlain. --%>
            <span class="badge">DLL</span>
            <h3 style="margin-top:10px;">Decrypt By: Gabriela Martinez</h3>
            <p class="muted">Coming soon. Paste the Encrypt output above to get the original text back.</p>
        </div>
    </section>

    <%-- ===== Cookies: Gabriela Martinez ===== --%>
    <section id="cookies">
        <div class="card">
            <%-- TODO (Gaby): Cookies TryIt. Save + read username, favorite driver, favorite team (7-day expiry). --%>
            <span class="badge">Cookie</span>
            <h3 style="margin-top:10px;">Cookies By: Gabriela Martinez</h3>
            <p class="muted">Coming soon.</p>
        </div>
    </section>

    <%-- ===== Captcha: Izzy Junio ===== --%>
    <section id="captcha">
        <div class="card">
            <%-- TODO (Izzy): Captcha user control TryIt. Register Captcha.ascx at the top of this page, then place the control here. --%>
            <span class="badge">User control</span>
            <h3 style="margin-top:10px;">Captcha By: Izzy Junio</h3>
            <p class="muted">Coming soon.</p>
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