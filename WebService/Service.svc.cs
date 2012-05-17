using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService
{
    [ServiceContract]
    public class Service
    {
        #region FavoritedPicture Methods

        [OperationContract]
        public void CreateFavoritedPicture(FavoritedPicture data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.FavoritedPictures.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public List<FavoritedPicture> GetFavoritedPictures(Guid userId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.FavoritedPictures.Where(u => u.UserID == userId).ToList();
        }

        [OperationContract]
        public void DeleteFavoritedPicture(Guid userId, Guid pictureId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            FavoritedPicture favoritedPicture = context.FavoritedPictures.Where(u => u.UserID == userId &&
                                                                                     u.PictureID == pictureId).SingleOrDefault();

            context.FavoritedPictures.DeleteOnSubmit(favoritedPicture);
            context.SubmitChanges();
        }

        #endregion

        #region FlaggedPicture Methods

        [OperationContract]
        public void CreateFlaggedPicture(FlaggedPicture data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.FlaggedPictures.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public List<FlaggedPicture> GetFlaggedPictures(Guid userId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.FlaggedPictures.Where(u => u.UserID == userId).ToList();
        }

        [OperationContract]
        public void DeleteFlaggedPicture(Guid userId, Guid pictureId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            FlaggedPicture flaggedPicture = context.FlaggedPictures.Where(u => u.UserID == userId &&
                                                                               u.PictureID == pictureId).SingleOrDefault();

            context.FlaggedPictures.DeleteOnSubmit(flaggedPicture);
            context.SubmitChanges();
        }

        #endregion

        #region Picture Methods

        [OperationContract]
        public void CreatePicture(Picture data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.Pictures.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public Picture GetPicture(Guid id)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.Pictures.Where(u => u.PictureID == id).SingleOrDefault();
        }

        [OperationContract]
        public void UpdatePicture(Picture data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            Picture picture = context.Pictures.Where(u => u.PictureID == data.PictureID).SingleOrDefault();
            picture.Caption = data.Caption;
            picture.LargeURL = data.LargeURL;
            picture.Latitude = data.Latitude;
            picture.Longitude = data.Longitude;
            picture.MediumURL = data.MediumURL;
            picture.SmallURL = data.SmallURL;
            picture.ViewCount = data.ViewCount;

            context.SubmitChanges();
        }

        [OperationContract]
        public void DeletePicture(Guid id)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            Picture picture = context.Pictures.Where(u => u.PictureID == id).SingleOrDefault();

            context.Pictures.DeleteOnSubmit(picture);
            context.SubmitChanges();
        }

        #endregion

        #region Relationship Methods

        [OperationContract]
        public void CreateRelationship(Relationship data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.Relationships.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public List<Relationship> GetRelationships(Guid userId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.Relationships.Where(u => u.UserID == userId).ToList();
        }

        [OperationContract]
        public void DeleteRelationship(Guid userId, Guid followingUserId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            Relationship relationship = context.Relationships.Where(u => u.UserID == userId &&
                                                                         u.FollowingUserID == followingUserId).SingleOrDefault();

            context.Relationships.DeleteOnSubmit(relationship);
            context.SubmitChanges();
        }

        #endregion

        #region User Methods

        [OperationContract]
        public void CreateUser(User data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.Users.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public User GetUser(Guid id)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.Users.Where(u => u.UserID == id).SingleOrDefault();
        }

        [OperationContract]
        public void UpdateUser(User data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            User user = context.Users.Where(u => u.UserID == data.UserID).SingleOrDefault();
            user.Biography = data.Biography;
            user.EmailAddress = data.EmailAddress;
            user.Location = data.Location;
            user.Name = data.Name;
            user.Password = data.Password;
            user.ProfilePictureID = data.ProfilePictureID;
            user.Username = data.Username;

            context.SubmitChanges();
        }

        [OperationContract]
        public void DeleteUser(Guid id)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            User user = context.Users.Where(u => u.UserID == id).SingleOrDefault();

            context.Users.DeleteOnSubmit(user);
            context.SubmitChanges();
        }

        #endregion

        #region UserConnectedAccount Methods

        [OperationContract]
        public void CreateUserConnectedAccount(UserConnectedAccount data)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            context.UserConnectedAccounts.InsertOnSubmit(data);
            context.SubmitChanges();
        }

        [OperationContract]
        public List<UserConnectedAccount> GetUserConnectedAccounts(Guid userId)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            return context.UserConnectedAccounts.Where(u => u.UserID == userId).ToList();
        }

        [OperationContract]
        public void DeleteUserConnectedAccount(Guid userId, string accountName)
        {
            DatabaseDataContext context = new DatabaseDataContext();

            UserConnectedAccount userConnectedAccount = context.UserConnectedAccounts.Where(u => u.UserID == userId &&
                                                                                                 u.AccountName == accountName).SingleOrDefault();

            context.UserConnectedAccounts.DeleteOnSubmit(userConnectedAccount);
            context.SubmitChanges();
        }

        #endregion
    }
}
