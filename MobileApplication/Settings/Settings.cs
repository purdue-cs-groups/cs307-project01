
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace MetrocamPan
{
  public static class Settings
  {
      public static readonly Setting<bool> isLoggedIn =
          new Setting<bool>("IsLoggedIn", false);

      public static readonly Setting<string> username =
          new Setting<string>("Username", "");

      public static readonly Setting<string> password =
          new Setting<string>("Password", "");

      public static readonly Setting<int> userid =
          new Setting<int>("Userid", -1);

      // Setting for save original photo into media library
      public static readonly Setting<bool> saveOriginal =
          new Setting<bool>("SaveOriginal", false);

      // Setting for save edited photo into media library
      public static readonly Setting<bool> saveEdited =
          new Setting<bool>("SaveEdited", false);

      public static readonly Setting<bool> locationService =
          new Setting<bool>("LocationService", false);

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