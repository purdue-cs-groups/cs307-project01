<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authenticate.aspx.cs" Inherits="WebService.Documentation.Authentication.Authenticate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - /v1/authenticate</title>
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
        
        pre
        {
            padding: 10px;
            background-color: #eeeeee;
            overflow: hidden;
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
        <div class="endpoint">
            POST /v1/authenticate<br />
            <br />
            <br />
            <div class="code">
                <i>Request: </i>
                <pre>
{
   "Username":"mbmccormick",
   "Password":"5f4dcc3b5aa765d61d8327deb882cf99"
}</pre>
            </div>
            <div class="code">
                <i>Response: </i>
                <pre>
{
  "User": {
    "Name": "Matt McCormick",
    "Username": "mbmccormick",
    "Location": "Purdue University",
    "CreatedDate": 0,
    "EmailAddress": "mbmccormick@gmail.com",
    "ID": "4f5665ca5ad98505b850909c",
    "Biography": "I am awesome at life.",
    "ProfilePicture": {
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
    }
  },
  "Token": "a21f8a253abf45a08f68ad0292878710"
}</pre>
            </div>
        </div>
        <br />
        <br />
        <div class="footer">
            Copyright &copy; 2012 Metrocam. All rights reserved.
        </div>
    </div>
    </form>
</body>
</html>
