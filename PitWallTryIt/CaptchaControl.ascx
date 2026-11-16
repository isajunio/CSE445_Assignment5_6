<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="PitWallTryIt.CaptchaControl" %>
<asp:Image ID="imgCaptcha" runat="server" AlternateText="CAPTCHA" /><br />
<asp:TextBox ID="txtCaptcha" runat="server" MaxLength="12" />
<asp:Button ID="btnRefresh" runat="server" Text="New image" CausesValidation="false" OnClick="btnRefresh_Click" />
<asp:Label ID="lblCaptchaError" runat="server" ForeColor="Red" />