using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using RGB.NET.Brushes;
using RGB.NET.Core;
using RGB.NET.Devices.Asus;
using RGB.NET.Devices.Corsair;
using RGB.NET.Devices.CorsairLink;
using Color = RGB.NET.Core.Color;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace AverageScreenColor.Utility
{
    public class Interaction
    {
        public bool AllScreens;

        public Interaction()
        {
            Initialize();
        }

        public void Initialize()
        {
            try
            {
                var surface = RGBSurface.Instance;
                surface.Exception += SurfaceOnException;

                //surface.LoadDevices(AsusDeviceProvider.Instance); // This one can cause some trouble right now
                surface.LoadDevices(CorsairDeviceProvider.Instance);
                surface.LoadDevices(CorsairLinkDeviceProvider.Instance);

                surface.AlignDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}! ErrorCode: {ex.InnerException}");
            }
        }

        private void SurfaceOnException(ExceptionEventArgs exceptionEventArgs)
        {
            Debug.WriteLine(exceptionEventArgs.Exception.Message);
        }

        public SolidColorBrush LoadAverageColor(CaptureMode captureMode, ScreenDisplayItem screen = null)
        {
            var img = LoadBitmap(captureMode, screen);
            var background = GetDominantColor(img);
            Debug.Print(background.ToString());
            var brush = new SolidColorBrush(background) {Brightness = 1f};
            return brush;
        }

        public BitmapImage LoadScreen(CaptureMode captureMode, ScreenDisplayItem screen = null)
        {
            var img = LoadBitmap(captureMode, screen);
            return ToBitmapImage(img);
        }

        private static Bitmap LoadBitmap(CaptureMode captureMode, ScreenDisplayItem screen = null)
        {
            var capture = new ScreenTools();
            var bounds = new Rectangle();
            var img = capture.Screen(ref bounds, captureMode, screen);
            img = new Bitmap(img, new Size(img.Width / 20, img.Height / 20));
            return img;
        }

        public Color GetDominantColor(Bitmap bmp)
        {
            var r = 0;
            var g = 0;
            var b = 0;

            var total = 0;

            for (var x = 0; x < bmp.Width; x++)
            {
                for (var y = 0; y < bmp.Height; y++)
                {
                    var clr = bmp.GetPixel(x, y);
                    r += clr.R;
                    g += clr.G;
                    b += clr.B;
                    total++;
                }
            }

            r /= total;
            g /= total;
            b /= total;

            return new Color(r, g, b);
        }

        public static BitmapImage ToBitmapImage(Image bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);

                memoryStream.Position = 0;
                var result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = memoryStream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public List<ScreenDisplayItem> LoadScreens()
        {
            return CaptureScreen.LoadScreens();
        }
    }
}