<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPicture.aspx.cs" Inherits="WebService.ViewPicture" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - View Picture</title>
    <link rel="Stylesheet" href="/Stylesheet.css" />
    <style type="text/css">
        .layout
        {
            width: 1000px;
            margin: 0px auto;
        }
        
        .left
        {
            width: 450px;
            float: left;
            text-align: right;
        }
        
        .right
        {
            width: 450px;
            float: right;
            text-align: left;
        }
        
        .caption
        {
            font-size: 24px;
            margin: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layout">
        <div class="left">
            <asp:Image ID="imgPicture" runat="server" Width="450" Height="450" CssClass="image" />
        </div>
        <div class="right">
            <p class="caption">
                &quot;<asp:Label ID="lblCaption" runat="server" />&quot;</p>
            <p class="description">
                This photo was taken by
                <asp:HyperLink ID="lnkUsername" runat="server" />
                on
                <asp:Label ID="lblDate" runat="server" />
                in
                <asp:Literal ID="litLocation" runat="server" />.</p>
            <asp:Literal ID="litTweetButton" runat="server" />
            <br />
            <br />
            <p class="tagline">
                Based on an open API and a powerful webservice, Metrocam boasts a beautiful user experience, powerfully integrated with your Windows Phone. It's that feeling of running into your best friend from college on 11th Avenue when she's wearing that old 'Seattle Seahawks' t-shirt. And making fun of her for it. Except anywhere. Anytime.  
            </p>
        </div>
    </div>
    </form>
</body>
</html>
