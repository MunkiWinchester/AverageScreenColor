using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace AverageScreenColor.Utility
{
    public class CaptureScreen
    {
        public static Bitmap CaptureDesktopWithCursor()
        {
            var screen = DetermineScreen(Win32Stuff.GetCursorPosition(), Screen.AllScreens);

            if (screen != null)
            {
                var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                // Draw the screenshot into our bitmap.
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screen.Bounds.Left, screen.Bounds.Top, 0, 0, bmp.Size);
                }

                return bmp;
            }

            return null;
        }

        private static Screen DetermineScreen(Point p, IEnumerable<Screen> screens)
        {
            foreach (var screen in screens)
            {
                if(screen.Bounds.X >= p.X)
                    continue;
                if(screen.Bounds.X + screen.Bounds.Width <= p.X)
                    continue;
                if(screen.Bounds.Y >= p.Y)
                    continue;
                if (screen.Bounds.Y - screen.Bounds.Height <= p.Y)
                    return screen;
            }

            return null;
        }

        public static Bitmap CaptureAllScreens()
        {
            var screenLeft = SystemInformation.VirtualScreen.Left;
            var screenTop = SystemInformation.VirtualScreen.Top;
            var screenWidth = SystemInformation.VirtualScreen.Width;
            var screenHeight = SystemInformation.VirtualScreen.Height;

            var endBmp = new Bitmap(screenWidth, screenHeight);
            foreach (var screen in  Screen.AllScreens)
            {
                var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                // Draw the screenshot into our bitmap.
                using (var g = Graphics.FromImage(endBmp))
                {
                    var left = screen.Bounds.Left;
                    if (screenLeft < 0)
                        left = screen.Bounds.Left + Math.Abs(screenLeft);
                    var top = screen.Bounds.Top;
                    if (screenTop < 0)
                        top = screen.Bounds.Top + Math.Abs(screenTop);

                    g.CopyFromScreen(screen.Bounds.Left, screen.Bounds.Top, left, top, bmp.Size);
                }
            }

            return endBmp;
        }

        public static List<ScreenDisplayItem> LoadScreens()
        {
            var list = new List<ScreenDisplayItem>();
            foreach (var screen in Screen.AllScreens.OrderBy(s => s.Bounds.Left))
            {
                var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screen.Bounds.Left, screen.Bounds.Top, 0, 0, bmp.Size);
                }

                list.Add(new ScreenDisplayItem
                {
                    IsChecked = screen.Primary,
                    Screen = screen,
                    Image =
                        ResizeImage(bmp, new Size(screen.Bounds.Width /10, screen.Bounds.Height /10))
                });
            }

            return list;
        }
        public static BitmapImage ResizeImage(Bitmap imgToResize, Size size)
        {
            return Interaction.BitmapToImageSource(new Bitmap(imgToResize, size));
        }
    }
}
