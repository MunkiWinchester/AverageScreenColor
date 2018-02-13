using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Exceptions;

namespace AverageScreenColor.Utility
{
    public class Interaction
    {
        private int _lightningMode;
        private bool _active;
        private Timer _timer;

        public bool AllScreens;
        //CorsairKeyboard keyboard;

        public Interaction()
        {
            Initialize();
            //keyboard = CueSDK.KeyboardSDK;
        }

        public void Initialize()
        {
            try
            {
                //CueSDK.Initialize();
                //Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");

                //CorsairKeyboard keyboard = CueSDK.KeyboardSDK;
                //if (keyboard == null)
                //    throw new WrapperException("No keyboard found");
            }
            catch (CUEException ex)
            {
                Debug.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
            }
            catch (WrapperException ex)
            {
                Debug.WriteLine("Wrapper Exception! Message:" + ex.Message);
            }
            _timer = new Timer();
            _timer.Tick += Refresh;
            _timer.Interval = 100; // in miliseconds
            _timer.Start();
        }

        private void Refresh(object sender, EventArgs e)
        {
            /*switch (lightningMode)
            {
                case 0:
                    AverageColor();
                    break;
                case 1:
                    AmbientColor();
                    break;
                case 2:
                    BottomColor();
                    break;
            }*/
            if (_active)
            {
                AverageColor();
            }
            else
            {
                //keyboard.Brush = null;
                //keyboard.Update();
            }
        }

        public void AverageColor()
        {
            var capture = new ScreenTools();
            var bounds = new Rectangle();
            var img = capture.Screen(ref bounds, AllScreens);
            img = new Bitmap(img, new Size(img.Width / 100, img.Height / 100));
            var background = GetDominantColor(img);
            Debug.Print(background.ToString());
            var brush = new SolidColorBrush(background) {Brightness = 1f};
            //keyboard.Brush = brush;
            //keyboard.Update();
        }

        public SolidColorBrush LoadAverageColor()
        {
            var capture = new ScreenTools();
            var bounds = new Rectangle();
            var img = capture.Screen(ref bounds, AllScreens);
            img = new Bitmap(img, new Size(img.Width / 100, img.Height / 100));
            var background = GetDominantColor(img);
            Debug.Print(background.ToString());
            return new SolidColorBrush(background);
        }

        public BitmapImage LoadSceen()
        {
            var capture = new ScreenTools();
            var bounds = new Rectangle();
            var img = capture.Screen(ref bounds, AllScreens);
            img = new Bitmap(img, new Size(img.Width / 10, img.Height / 10));
            return BitmapToImageSource(img);
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

            return Color.FromArgb(r, g, b);
        }

        /*public void AmbientColor()
        {
            ScreenTools capture = new ScreenTools();
            Rectangle bounds = new Rectangle();
            Bitmap img = capture.Screen(ref bounds);
            img = new Bitmap(img, new Size(img.Width / 90, img.Height / 90));
            Color background = getDominantColor(img);
            Debug.Print(background.ToString());
            IBrush brush = new SolidColorBrush(background);
            brush.Brightness = 1f;
            keyboard.Brush = brush;
            keyboard.Update();
        }

        private RectangleKeyGroup[,] GenerateAmbientKeyGroup(RectangleKeyGroup[,] rectGroup)
        {
            RectangleF rect = new RectangleF();
            int xOffset = 20;
            int yOffset = 20;
            //423 maxWidth
            //137 maxHeight
            //20 horizontal Step
            //20 vertical Step
            rect.Width = 25;
            rect.Height = 25;
            for (int y = 0; y < rectGroup.GetLength(1); y++)
            {
                for (int x = 0; x < rectGroup.GetLength(0); x++)
                {
                    rect.X += xOffset;
                    rectGroup[x, y] = new RectangleKeyGroup(keyboard, rect, 0.5f, true);
                    //rectGroup[x, y].Brush = new RandomColorBrush();
                }
                rect.X = 0;
                rect.Y += yOffset;
            }

            return rectGroup;
        }

        private void AverageOn()
        {
            _lightningMode = 0;
            _active = true;
        }

        private void AverageOff()
        {
            _active = false;
        }

        private void Ambient()
        {
            _lightningMode = 1;
        }

        private void Bottom()
        {
            _lightningMode = 2;
        }
        */

        private static BitmapImage BitmapToImageSource(Image bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}