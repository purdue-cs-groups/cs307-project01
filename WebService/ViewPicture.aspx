<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPicture.aspx.cs" Inherits="WebService.ViewPicture" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Winstagram</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 50px;
            font-family: "Segoe WPC" , "Segoe UI" , Helvetica, Arial, "Arial Unicode MS" , Sans-Serif;
        }
        
        a
        {
            color: #1BA1E2;
        }
        
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
        
        .description
        {
            color: #999999;
            line-height: 1.5em;
        }
        
        .tagline
        {
            color: #999999;
            line-height: 1.5em;
            font-size: 14px;
            font-style: italic;
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
                Winstagram is a fun & quirky way to share your life with friends through a series
                of pictures. Snap a photo, then choose a filter to transform the look and feel of
                the shot into a memory to keep around forever.
            </p>
        </div>
    </div>
    </form>
</body>
</html>
