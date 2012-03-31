<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebService.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam</title>
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
            width: 600px;
            margin: 0px auto;
        }
        
        .title
        {
            font-size: 32px;
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
        
        .tile
        {
            display: inline-block;
            margin-right: 16px;
            margin-bottom: 16px;
        }
        
        .tile a
        {
        }
        
        .tile a img
        {
            width: 100px;
            height: 100px;
        }
        
        .footer
        {
            line-height: 1.5em;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layout">
        <p class="title">
            Metrocam</p>
        <p class="tagline">
            Metrocam is a fun & quirky way to share your life with friends through a series
            of pictures. Snap a photo, then choose a filter to transform the look and feel of
            the shot into a memory to keep around forever.
        </p>
        <br />
        <div class="list">
            <asp:Literal ID="litDatabase" runat="server" />
        </div>
        <br />
        <div class="footer">
            Copyright &copy; 2012 Metrocam. All rights reserved.
        </div>
    </div>
    </form>
</body>
</html>
