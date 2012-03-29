<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebService.Default" %>

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
            width: 600px;
            margin: 0px auto;
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
        <p class="caption">
            Winstagram</p>
        <p class="tagline">
            Winstagram is a fun & quirky way to share your life with friends through a series
            of pictures. Snap a photo, then choose a filter to transform the look and feel of
            the shot into a memory to keep around forever.
        </p>
        <br />
        <asp:Literal ID="litDatabase" runat="server" />
    </div>
    </div>
    </form>
</body>
</html>
