using UIKit;

namespace Xam.SweetAlert.iOS
{
    public static class Extensions
    {
        public static UIColor ToUIColor(this int hexValue)
        {
            return UIColor.FromRGBA(
              (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
              (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
              (((float)(hexValue & 0xFF)) / 255.0f),
              1.0f
              );
        }

        public static UIColor FromHex(this UIColor color, int hexValue)
        {
            return UIColor.FromRGBA(
                (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(hexValue & 0xFF)) / 255.0f),
                1.0f
            );
        }
    }
}