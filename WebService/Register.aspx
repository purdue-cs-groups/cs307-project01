<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebService.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b>Registration</b>
        <p>
            Use the form below to register your application for an API key. The API key will
            be sent to the email address that you provide.</p>
        <asp:TextBox ID="txtEmailAddress" runat="server" />
        <asp:Button ID="btnSubmit" runat="server" UseSubmitBehavior="true" Text="Register"
            OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
