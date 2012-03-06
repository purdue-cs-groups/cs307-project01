
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace WinstagramPan
{
  public static class Settings
  {
      public static readonly Setting<bool> isLoggedIn =
          new Setting<bool>("IsLoggedIn", false);
  }
}