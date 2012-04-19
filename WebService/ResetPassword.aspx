<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="WebService.ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - Reset Password</title>
    <link rel="Stylesheet" href="/Stylesheet.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layout">
        <p class="title">
            Metrocam</p>
        <p class="tagline">
            Based on an open API and a powerful webservice, Metrocam boasts a beautiful user experience, powerfully integrated with your Windows Phone. It's that feeling of running into your best friend from college on 11th Avenue when she's wearing that old 'Seattle Seahawks' t-shirt. And making fun of her for it. Except anywhere. Anytime.  
        </p>
        <br />
        <div>
            <p>
                Enter your email address below and a new password will be emailed to you.</p>
            <br />
            <table>
                <tr>
                    <td>Email Address: </td>
                    <td><asp:TextBox ID="txtEmailAddress" runat="server" /></td>
                </tr>
            </table>
            <br />
            <asp:Button ID="btnSubmit" runat="server" UseSubmitBehavior="true" Text="Reset"
                OnClick="btnSubmit_Click" />
        </div>
        <br />
        <br />
        <div class="footer">
            <div class="navigation">
                <a href="/default.aspx">Home</a> | <a href="/documentation/default.aspx">API Documentation</a>
                | <a href="/register.aspx">API Key Registration</a> | <a href="/resetpassword.aspx">
                    Forgot Password</a>
            </div>
            Copyright &copy; 2012 Metrocam. All rights reserved.
        </div>
    </div>
    </form>
</body>
</html>
