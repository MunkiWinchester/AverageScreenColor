using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AverageScreenColor.Utility
{
    public class CaptureScreen
    {
        //This structure shall be used to keep the size of the screen.
        public struct Size
        {
            public int Cx;
            public int Cy;
        }

        public static Bitmap CaptureDesktop()
        {
            Size size;
            var hDc = Win32Stuff.GetDC(Win32Stuff.GetDesktopWindow());
            var hMemDc = GdiStuff.CreateCompatibleDC(hDc);

            size.Cx = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SmCxscreen);

            size.Cy = Win32Stuff.GetSystemMetrics
                      (Win32Stuff.SmCyscreen);

            var hBitmap = GdiStuff.CreateCompatibleBitmap(hDc, size.Cx, size.Cy);

            if (hBitmap != IntPtr.Zero)
            {
                var hOld = GdiStuff.SelectObject
                    (hMemDc, hBitmap);

                GdiStuff.BitBlt(hMemDc, 0, 0, size.Cx, size.Cy, hDc,
                                               0, 0, GdiStuff.Srccopy);

                GdiStuff.SelectObject(hMemDc, hOld);
                GdiStuff.DeleteDC(hMemDc);
                Win32Stuff.ReleaseDC(Win32Stuff.GetDesktopWindow(), hDc);
                var bmp = Image.FromHbitmap(hBitmap);
                GdiStuff.DeleteObject(hBitmap);
                GC.Collect();
                return bmp;
            }
            return null;

        }


        public static Bitmap CaptureCursor(ref int x, ref int y)
        {
            var ci = new Win32Stuff.Cursorinfo();
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CursorShowing)
                {
                    var hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out var icInfo))
                    {
                        x = ci.ptScreenPos.x - icInfo.xHotspot;
                        y = ci.ptScreenPos.y - icInfo.yHotspot;

                        var ic = Icon.FromHandle(hicon);
                        var bmp = ic.ToBitmap();
                        return bmp;
                    }
                }
            }

            return null;
        }

        public static Bitmap CaptureDesktopWithCursor()
        {
            var cursorX = 0;
            var cursorY = 0;

            var desktopBmp = CaptureDesktop();
            var cursorBmp = CaptureCursor(ref cursorX, ref cursorY);
            if (desktopBmp != null)
            {
                if (cursorBmp != null)
                {
                    var r = new Rectangle(cursorX, cursorY, cursorBmp.Width, cursorBmp.Height);
                    var g = Graphics.FromImage(desktopBmp);
                    g.DrawImage(cursorBmp, r);
                    g.Flush();

                    return desktopBmp;
                }
                else
                    return desktopBmp;
            }

            return null;

        }


    }
}
