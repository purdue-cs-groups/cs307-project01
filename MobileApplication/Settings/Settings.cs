
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System;

namespace MetrocamPan
{
    public static class Settings
    {
        public static readonly Setting<bool> isLoggedIn = new Setting<bool>("isLoggedIn", false);
        public static readonly Setting<string> username = new Setting<string>("Username", "");
        public static readonly Setting<string> password = new Setting<string>("Password", "");

        public static Setting<bool> saveOriginal;
        public static Setting<bool> saveEdited;
        public static Setting<bool> locationService;

        public static Setting<bool> twitterAuth;

        public static void getUserSpecificSettings(String currentUser)
        {
            var iso = IsolatedStorageSettings.ApplicationSettings;

            // check to see if this user exists
            if (!iso.Contains(currentUser + "SaveOriginal"))
            {
                saveOriginal = new Setting<bool>(currentUser + "SaveOriginal", false);
                saveEdited = new Setting<bool>(currentUser + "SaveEdited", false);
                locationService = new Setting<bool>(currentUser + "LocationService", true);
                
                twitterAuth = new Setting<bool>(currentUser + "TwitterAuth", false);
            }
            else
            {
                saveOriginal = new Setting<bool>(currentUser + 
                    "SaveOriginal", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "SaveOriginal"]));
                saveEdited = new Setting<bool>(currentUser + 
                    "SaveEdited", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "SaveEdited"]));
                locationService = new Setting<bool>(currentUser + 
                    "LocationService", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "LocationService"]));
                
                twitterAuth = new Setting<bool>(currentUser +
                    "TwitterAuth", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "TwitterAuth"]));
            }
        }

        // Logout user and reset specific settings to default
        public static void logoutUser()
        {
            // Change user specific settings to default
            resetToDefault();

            // Clear views
            App.PopularPictures.Clear();
            App.RecentPictures.Clear();
            App.ContinuedRecentPictures.Clear();
        }

        // Reset all non user specific Setting objects to default values
        public static void resetToDefault()
        {
            isLoggedIn.Value = false;
            username.Value = "";
            password.Value = "";
        }
    }
}