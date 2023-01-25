using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Seconds
{
    public partial class SystraySeconds 
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private System.Windows.Forms.Timer timer1;

        private int iconSize = 16;
        private NotifyIcon notifyIcon;
        private Icon? ico;
        private Bitmap? bmp;
        private Font iconFont;

        public SystraySeconds()
        {
            //InitializeComponent();
            notifyIcon = new NotifyIcon();
            notifyIcon.MouseClick += notifyIcon_MouseClick;

            iconFont = new Font("Consolas", 11, FontStyle.Bold, GraphicsUnit.Pixel);

            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
        }

        private void notifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object? sender, EventArgs e)
        {
            if(notifyIcon != null)
            {
                updateIcon();

                if (!notifyIcon.Visible)
                {
                    notifyIcon.Visible = true;
                }
            }
        }

        private void updateIcon()
        {
            if (bmp == null)
            {
                bmp = new Bitmap(iconSize, iconSize);
            }
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, iconSize, iconSize);
            g.DrawString(DateTime.Now.Second.ToString("00"), iconFont, Brushes.Black, new PointF(0, 1));

            ico = Icon.FromHandle(bmp.GetHicon());
            notifyIcon.Icon = ico;

            DestroyIcon(ico.Handle);
        }
    }
}