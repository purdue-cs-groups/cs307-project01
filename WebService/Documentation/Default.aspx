<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebService.Documentation.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Metrocam - Documentation</title>
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
        
        .endpoint
        {
            margin-top: 10px;
            margin-bottom: 10px;
        }
        
        .footer
        {
            line-height: 1.5em;
            font-size: 14px;
        }
        
        .navigation a
        {
            color: #aaaaaa; 
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
            Authentication
            <div class="endpoint">
                <a href="Authentication/Authenticate.aspx">/v1/authenticate</a>
            </div>
            <br />
            Favorites
            <div class="endpoint">
                <a href="Favorites/FetchFavoritedPicture.aspx">/v1/favorites/fetch?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="Favorites/CreateFavoritedPicture.aspx">/v1/favorites/create</a>
            </div>
            <div class="endpoint">
                <a href="Favorites/UpdateFavoritedPicture.aspx">/v1/favorites/update</a>
            </div>
            <div class="endpoint">
                <a href="Favorites/DeleteFavoritedPicture.aspx">/v1/favorites/delete?id={id}</a>
            </div>
            <br />
            Flags
            <div class="endpoint">
                <a href="Flags/FetchFlaggedPicture.aspx">/v1/flags/fetch?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="Flags/CreateFlaggedPicture.aspx">/v1/flags/create</a>
            </div>
            <div class="endpoint">
                <a href="Flags/UpdateFlaggedPicture.aspx">/v1/flags/update</a>
            </div>
            <div class="endpoint">
                <a href="Flags/DeleteFlaggedPicture.aspx">/v1/flags/delete?id={id}</a>
            </div>
            <br />
            Pictures
            <div class="endpoint">
                <a href="Pictures/FetchNewsFeed.aspx">/v1/pictures/fetch</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/FetchPopularNewsFeed.aspx">/v1/pictures/popular/fetch</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/FetchUserPictures.aspx">/v1/pictures/user/fetch?userid={userId}</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/FetchUserFavoritedPictures.aspx">/v1/pictures/user/favorites/fetch?userid={userId}</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/CreatedPicture.aspx">/v1/pictures/create</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/UploadPicture.aspx">/v1/pictures/upload</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/UpdatePicture.aspx">/v1/pictures/update</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/DeletePicture.aspx">/v1/pictures/delete?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="Pictures/FlagPicture.aspx">/v1/pictures/flag?id={id}</a>
            </div>
            <br />
            Relationships
            <div class="endpoint">
                <a href="Relationships/FetchRelationship.aspx">/v1/relationships/fetch?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="Relationships/CreateRelationship.aspx">/v1/relationships/create</a>
            </div>
            <div class="endpoint">
                <a href="Relationships/UpdateRelationship.aspx">/v1/relationships/update</a>
            </div>
            <div class="endpoint">
                <a href="Relationships/DeleteRelationship.aspx">/v1/relationships/delete?id={id}</a>
            </div>
            <br />
            Users
            <div class="endpoint">
                <a href="Users/FetchUser.aspx">/v1/users/fetch?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="Users/FetchAllUsers.aspx">/v1/users/fetch</a>
            </div>
            <div class="endpoint">
                <a href="Users/SearchUsers.aspx">/v1/users/search?query={query}</a>
            </div>
            <div class="endpoint">
                <a href="Users/CreateUser.aspx">/v1/users/create</a>
            </div>
            <div class="endpoint">
                <a href="Users/UpdateUser.aspx">/v1/users/update</a>
            </div>
            <div class="endpoint">
                <a href="Users/DeleteUser.aspx">/v1/users/delete?id={id}</a>
            </div>
            <br />
            Connected Accounts
            <div class="endpoint">
                <a href="ConnectedAccounts/FetchUserConnectedAccount.aspx">/v1/users/connections/fetch?id={id}</a>
            </div>
            <div class="endpoint">
                <a href="ConnectedAccounts/FetchUserConnectedAccountsByUserID.aspx">/v1/users/connections/fetchByUserID?userid={userId}</a>
            </div>
            <div class="endpoint">
                <a href="ConnectedAccounts/CreateUserConnectedAccount.aspx">/v1/users/connections/create</a>
            </div>
            <div class="endpoint">
                <a href="ConnectedAccounts/UpdateUserConnectedAccount.aspx">/v1/users/connections/update</a>
            </div>
            <div class="endpoint">
                <a href="ConnectedAccounts/DeleteUserConnectedAccount.aspx">/v1/users/connections/delete?id={id}</a>
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
