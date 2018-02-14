using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace AverageScreenColor.Utility
{
    public class ScreenDisplayItem
    {
        public bool IsChecked { get; set; }
        public BitmapImage Image { get; set; }
        public Screen Screen { get; set; }
    }
}