
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

      public static readonly Setting<bool> saveOriginal =
          new Setting<bool>("SaveOriginal", false);

      public static readonly Setting<bool> saveEdited =
          new Setting<bool>("SaveEdited", false);
  }
}