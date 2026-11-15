<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="PitWallTryIt.Error" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error | Pit Wall</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Something went wrong.</h1>
            <asp:Label ID="lblError" runat="server" />
            <p><a href="Default.aspx">Back to Home</a></p>
        </div>
    </form>
</body>
</html>