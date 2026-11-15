<%@ Page Title="Log in | Pit Wall" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PitWallTryIt.Login" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <%-- By: Ayomide Oshilaja. Login page for Members and Staff. --%>
    <div class="auth-center">
        <div class="auth-card card">
            <div class="eyebrow yellow">Pit Wall · Member access</div>
            <h2>Log in</h2>
            <asp:Label ID="lblInfo" runat="server" CssClass="auth-info" />

            <%-- Member / Staff toggle login: specify which XML file is checked --%>
            <asp:RadioButtonList ID="rblRole" runat="server" RepeatDirection="Horizontal" CssClass="role-toggle">
                <asp:ListItem Text="Member" Value="Member" Selected="True" />
                <asp:ListItem Text="Staff" Value="Staff" />
            </asp:RadioButtonList>
            <br />

            <asp:Label runat="server" AssociatedControlID="txtUser" Text="Username" />
            <asp:TextBox ID="txtUser" runat="server" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUser"
                ErrorMessage="Enter your username" Display="Dynamic" CssClass="auth-error" />

            <asp:Label runat="server" AssociatedControlID="txtPass" Text="Password" />
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass"
                ErrorMessage="Enter your password" Display="Dynamic" CssClass="auth-error" />

            <asp:CheckBox ID="chkRemember" runat="server" Text=" Keep me signed in" />
            <br /><br />
            <asp:Button ID="btnLogin" runat="server" Text="Log in to Pit Wall" OnClick="btnLogin_Click" CssClass="btn btn-yellow" />
            <asp:Label ID="lblError" runat="server" CssClass="auth-error" />

            <p class="muted" style="margin-top:18px;">New here? <a href="CreateAccount.aspx">Create an account</a></p>
            <p class="muted" style="margin-bottom:0;">Staff test login: <span class="chip">TA</span> <span class="chip">Cse445!</span> (choose Staff)</p>
        </div>
    </div>
</asp:Content>