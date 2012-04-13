<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FetchPicture.aspx.cs" Inherits="WebService.Documentation.Pictures.FetchPicture" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - /v1/pictures/fetch?id={id}</title>
    <link rel="Stylesheet" href="/Stylesheet.css" />
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
        <div class="endpoint">
            GET /v1/pictures/fetch?id={id}<br />
            <br />
            <br />
            <div class="code">
                <i>Request: </i>
                <pre></pre>
            </div>
            <div class="code">
                <i>Response: </i>
                <pre>
{
  "LargeURL": "http://metrocam.blob.core.windows.net/pictures/4f7124f25ad9850a042a5f2d/993e2713-155c-4f97-a475-adfc9d223a2c_l.jpg",
  "SmallURL": "http://metrocam.blob.core.windows.net/pictures/4f7124f25ad9850a042a5f2d/993e2713-155c-4f97-a475-adfc9d223a2c_s.jpg",
  "Longitude": -86.9443232194399,
  "ViewCount": 0,
  "CreatedDate": 1334063901,
  "Latitude": 40.4469600040842,
  "ID": "4f84331cd47cd409c4df24be",
  "MediumURL": "http://metrocam.blob.core.windows.net/pictures/4f7124f25ad9850a042a5f2d/993e2713-155c-4f97-a475-adfc9d223a2c_m.jpg",
  "Caption": "These are my keys. ",
  "UserID": "4f5665ca5ad98505b850909c"
}</pre>
            </div>
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
