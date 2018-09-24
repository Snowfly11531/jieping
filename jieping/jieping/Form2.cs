using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Imaging;

namespace jieping
{
    public partial class Form2 : DevExpress.XtraEditors.XtraForm
    {
        Bitmap bitmap;
        Bitmap maskBitmap;
        Boolean isFirst = true;
        int formalX = 0;
        int formalY = 0;
        int downX;
        int downY;
        int upX;
        int upY;
        bool isMouseDown = false;
        bool isDrawComplete = false;
        public Form2(Bitmap bitmap)
        {
            InitializeComponent();
            this.bitmap = bitmap;
            maskBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gg = Graphics.FromImage(maskBitmap);
            gg.DrawImage(bitmap, 0, 0);
            gg.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.BackgroundImage = maskBitmap;
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            downX = e.X;
            downY = e.Y;
            isMouseDown = true;
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            isDrawComplete = true;
            upX = e.X;
            upY = e.Y;
            popupMenu1.ShowPopup(e.Location);
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawComplete)
            {
                Graphics g = this.CreateGraphics();
                g.DrawImage(maskBitmap, formalX < e.X ? formalX - 2 : e.X + 2, 0, new Rectangle(formalX < e.X ? formalX - 2 : e.X + 2, 0,
                    Math.Abs(e.X - formalX), Screen.PrimaryScreen.Bounds.Height), GraphicsUnit.Pixel);
                g.DrawImage(maskBitmap, 0, formalY < e.Y ? formalY - 2 : e.Y + 2, new Rectangle(0, formalY < e.Y ? formalY - 2 : e.Y + 2,
                    Screen.PrimaryScreen.Bounds.Width, Math.Abs(e.Y - formalY)), GraphicsUnit.Pixel);
                if (!isMouseDown)
                {
                    g.DrawLine(new Pen(Color.Green, 4), e.X, 0, e.X, Screen.PrimaryScreen.Bounds.Height);
                    g.DrawLine(new Pen(Color.Green, 4), 0, e.Y, Screen.PrimaryScreen.Bounds.Width, e.Y);
                }
                else
                {
                    g.DrawImage(bitmap, downX, downY, new Rectangle(downX, downY, e.X - downX, e.Y - downY), GraphicsUnit.Pixel);
                    g.DrawRectangle(new Pen(Color.Green, 4), new Rectangle(downX < e.X ? downX : e.X, downY < e.Y ? downY : e.Y, Math.Abs(e.X - downX), Math.Abs(e.Y - downY)));
                }
                formalX = e.X;
                formalY = e.Y;
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Bitmap downBitmap = new Bitmap(Math.Abs(downX - upX), Math.Abs(downY - upY));
            Graphics g = Graphics.FromImage(downBitmap);
            g.DrawImage(bitmap, 0, 0,
                new Rectangle(downX < upX ? downX : upX, downY < upY ? downY : upY, Math.Abs(downX - upX), Math.Abs(downY - upY)), GraphicsUnit.Pixel);
            Image icon = Image.FromFile("C:\\Users\\24513\\Desktop\\福建师范大学.png");
            g.DrawImage(icon, downBitmap.Width - 210, downBitmap.Height - 30, 210, 30);
            downBitmap.Save("E:\\123.bmp", ImageFormat.Bmp);
            this.Owner.Show();
            this.Dispose();
        }
    }
}