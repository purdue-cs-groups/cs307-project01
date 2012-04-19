<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchUsers.aspx.cs" Inherits="WebService.Documentation.Users.SearchUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - /v1/users/search?query={query}</title>
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
        <div class="endpoint">
            GET /v1/users/search?query={query}<br />
            <br />
            <br />
            <div class="code">
                <i>Request: </i>
                <pre></pre>
            </div>
            <div class="code">
                <i>Response: </i>
                <pre>
[
   {
      "ID":"4f5665ca5ad98505b850909c",
      "Username":"mbmccormick",
      "Name":"Matt McCormick",
      "EmailAddress":"mbmccormick@gmail.com",
      "Biography":"I am awesome at life.",
      "ProfilePicture":{
         "ID":"4f84331cd47cd409c4df24be",
         "UserID":"4f5665ca5ad98505b850909c",
         "Caption":"These are my keys. ",
         "Latitude":40.4469600040842,
         "Longitude":-86.9443232194399,
         "ViewCount":0,
         "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/993e2713-155c-4f97-a475-adfc9d223a2c_l.jpg",
         "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/993e2713-155c-4f97-a475-adfc9d223a2c_m.jpg",
         "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/993e2713-155c-4f97-a475-adfc9d223a2c_s.jpg",
         "CreatedDate":1334063901
      },
      "Location":"Purdue University",
      "CreatedDate":0
   },
   {
      "ID":"4f733cdf5ad9850a6c854ed0",
      "Username":"gustavo",
      "Name":"Gustavo Rodriguez Rivera",
      "EmailAddress":"gustavo@purdue.edu",
      "Biography":null,
      "ProfilePicture":{
         "ID":"4f774fc1d47cd408fc5c1d3a",
         "UserID":"4f5665ca5ad98505b850909c",
         "Caption":null,
         "Latitude":40.44698,
         "Longitude":-86.944189,
         "ViewCount":0,
         "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_l.jpg",
         "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_m.jpg",
         "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_s.jpg",
         "CreatedDate":1333219265
      },
      "Location":null,
      "CreatedDate":1332952288
   },
   {
      "ID":"4f7916e5d47cd408d8afc9c8",
      "Username":"Helki",
      "Name":"James Ma",
      "EmailAddress":"hekki.ss@djdjs.com",
      "Biography":null,
      "ProfilePicture":{
         "ID":"4f774fc1d47cd408fc5c1d3a",
         "UserID":"4f5665ca5ad98505b850909c",
         "Caption":null,
         "Latitude":40.44698,
         "Longitude":-86.944189,
         "ViewCount":0,
         "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_l.jpg",
         "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_m.jpg",
         "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f5685ce5ad9850e545bb48d\/4d8345fa-f701-4be7-bb7b-61950d3cdaf7_s.jpg",
         "CreatedDate":1333219265
      },
      "Location":null,
      "CreatedDate":1333335781
   }
]</pre>
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
