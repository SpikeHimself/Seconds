using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Seconds
{
    public static class SystraySeconds 
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private static readonly System.Threading.Timer updateTimer;

        private static readonly int iconSize = 16;
        private static readonly NotifyIcon notifyIcon;
        private static readonly Font iconFont;

        static SystraySeconds()
        {
            notifyIcon = new NotifyIcon
            {
                Visible = true
            };
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            iconFont = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold, GraphicsUnit.Pixel);
            updateTimer = new System.Threading.Timer(updateIcon, null, Timeout.Infinite, Timeout.Infinite);
        }

        public static void Run()
        {
            updateTimer.Change(0, 200);
        }

        private static void notifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                updateTimer.Dispose();
                Application.Exit();
            }
        }

        private static void updateIcon(object? state)
        {
            string currentSecond = DateTime.Now.Second.ToString("00");

            using (var bmp = new Bitmap(iconSize, iconSize))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; 
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    
                    g.Clear(Color.BlanchedAlmond);
                    g.DrawString(currentSecond, iconFont, Brushes.SaddleBrown, new PointF(-1.5f, 0));
                }

                notifyIcon.Icon = Icon.FromHandle(bmp.GetHicon());
                DestroyIcon(notifyIcon.Icon.Handle);
            }

        }
    }
}