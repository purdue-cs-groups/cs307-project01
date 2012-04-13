<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FetchUserPictures.aspx.cs" Inherits="WebService.Documentation.Pictures.FetchUserPictures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - /v1/pictures/user/fetch?userid={userId}</title>
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
            GET /v1/pictures/fetch/user/fetch?userid={userId}<br />
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
      "ID":"4f876f9bd47cd404c0b4e759",
      "User":{
         "ID":"4f876b12d47cd404c0b4e74e",
         "Username":"libbypucc",
         "Name":"Libby",
         "EmailAddress":"libby.pucc@gmail.com",
         "Biography":"Just another Metrocammer!",
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
         "Location":"Metrocam City",
         "CreatedDate":1334274834
      },
      "Caption":"What a good pup",
      "Latitude":40.4515972137451,
      "Longitude":-86.9370136260986,
      "ViewCount":0,
      "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/0469322c-0874-4ae3-b7cd-51e54c52a7cb_l.jpg",
      "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/0469322c-0874-4ae3-b7cd-51e54c52a7cb_m.jpg",
      "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/0469322c-0874-4ae3-b7cd-51e54c52a7cb_s.jpg",
      "CreatedDate":1334275996
   },
   {
      "ID":"4f876e7cd47cd404c0b4e753",
      "User":{
         "ID":"4f876b12d47cd404c0b4e74e",
         "Username":"libbypucc",
         "Name":"Libby",
         "EmailAddress":"libby.pucc@gmail.com",
         "Biography":"Just another Metrocammer!",
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
         "Location":"Metrocam City",
         "CreatedDate":1334274834
      },
      "Caption":"Sadie spam!!",
      "Latitude":40.4515972137451,
      "Longitude":-86.9370136260986,
      "ViewCount":0,
      "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/d4495f87-2f63-4667-9aed-ebb1174d7040_l.jpg",
      "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/d4495f87-2f63-4667-9aed-ebb1174d7040_m.jpg",
      "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/d4495f87-2f63-4667-9aed-ebb1174d7040_s.jpg",
      "CreatedDate":1334275708
   },
   {
      "ID":"4f876d88d47cd404c0b4e752",
      "User":{
         "ID":"4f876b12d47cd404c0b4e74e",
         "Username":"libbypucc",
         "Name":"Libby",
         "EmailAddress":"libby.pucc@gmail.com",
         "Biography":"Just another Metrocammer!",
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
         "Location":"Metrocam City",
         "CreatedDate":1334274834
      },
      "Caption":"Little baby! ",
      "Latitude":40.4515972137451,
      "Longitude":-86.9370136260986,
      "ViewCount":0,
      "LargeURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/48e22b4b-638a-4620-8e3b-149cda934942_l.jpg",
      "MediumURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/48e22b4b-638a-4620-8e3b-149cda934942_m.jpg",
      "SmallURL":"http:\/\/metrocam.blob.core.windows.net\/pictures\/4f7124f25ad9850a042a5f2d\/48e22b4b-638a-4620-8e3b-149cda934942_s.jpg",
      "CreatedDate":1334275464
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
