
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System;

namespace MetrocamPan
{
  public static class Settings
  {
      public static Setting<bool> isLoggedIn = new Setting<bool>("isLoggedIn", false);

      public static Setting<string> username;
      public static Setting<string> password;
      public static Setting<int>    userid;
      public static Setting<bool>   saveOriginal;
      public static Setting<bool>   saveEdited;
      public static Setting<bool>   locationService;

      public static void getSettings(String currentUser)
      {
          var iso = IsolatedStorageSettings.ApplicationSettings;

          // check to see if this user exists
          if (!iso.Contains(currentUser + "Username"))
          {
              username        = new Setting<string>(currentUser + "Username", "");
              password        = new Setting<string>(currentUser + "Password", "");
              userid          = new Setting<int>(currentUser + "Userid", -1);
              saveOriginal    = new Setting<bool>(currentUser + "SaveOriginal", false);
              saveEdited      = new Setting<bool>(currentUser + "SaveEdited", false);
              locationService = new Setting<bool>(currentUser + "LocationService", false);
          }
          else
          {
              username        = new Setting<string>(currentUser + "Username", (IsolatedStorageSettings.ApplicationSettings[currentUser + "Username"]).ToString());
              password        = new Setting<string>(currentUser + "Username", (IsolatedStorageSettings.ApplicationSettings[currentUser + "Password"]).ToString());
              userid          = new Setting<int>(currentUser + "Userid", Convert.ToInt32(IsolatedStorageSettings.ApplicationSettings[currentUser + "Userid"]));
              saveOriginal    = new Setting<bool>(currentUser + "SaveOriginal", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "SaveOriginal"]));
              saveEdited      = new Setting<bool>(currentUser + "SaveEdited", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "SaveEdited"]));
              locationService = new Setting<bool>(currentUser + "LocationService", Convert.ToBoolean(IsolatedStorageSettings.ApplicationSettings[currentUser + "LocationService"]));
          }
      }

      public static void logoutUser()
      {
          isLoggedIn.Value = false;
      }

      // Reset all Setting objects to default values
      public static void resetToDefault()
      {
          isLoggedIn.Value = false;
          username.Value = "";
          password.Value = "";
          userid.Value = -1;
          saveOriginal.Value = false;
          saveEdited.Value = false;
          locationService.Value = false;
      }
  }
}