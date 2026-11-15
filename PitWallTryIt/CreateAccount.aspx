<%@ Page Title="Create account | Pit Wall" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="PitWallTryIt.CreateAccount" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- Sign up page, AccountStore.AddMember By: Ayomide Oshilaja. Captcha By: Izzy Junio. --%>
    <div class="auth-center">
        <div class="auth-card card">
            <div class="eyebrow yellow">Pit Wall · New member</div>
            <h2>Your seat is <span class="accent">waiting.</span></h2>
            <p class="muted">Create a free member account. Your password is encrypted before it is saved.</p>

            <asp:Label runat="server" AssociatedControlID="txtUser" Text="Username (3 to 20 letters, numbers or _)" />
            <asp:TextBox ID="txtUser" runat="server" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUser"
                ErrorMessage="Choose a username" Display="Dynamic" CssClass="auth-error" />

            <asp:Label runat="server" AssociatedControlID="txtPass" Text="Password (6+ characters)" />
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass"
                ErrorMessage="Choose a password" Display="Dynamic" CssClass="auth-error" />

            <asp:Label runat="server" AssociatedControlID="txtConfirm" Text="Confirm password" />
            <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" />
            <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtPass"
                ErrorMessage="Passwords don't match" Display="Dynamic" CssClass="auth-error" />

            <%-- TODO (Izzy): Captcha controls here --%>
            <div class="output" style="margin-bottom:14px;"><span class="muted">Captcha (Izzy Junio): coming soon</span></div>

            <asp:Button ID="btnCreate" runat="server" Text="Create my account" OnClick="btnCreate_Click" CssClass="btn btn-yellow" />
            <asp:Label ID="lblError" runat="server" CssClass="auth-error" />

            <p class="muted" style="margin-top:18px; margin-bottom:0;">Already a member? <a href="Login.aspx">Log in</a></p>
        </div>
    </div>
</asp:Content>