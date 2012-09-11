using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class PrettyControl
    {
        private Point margin = new Point(0, 3);
        private string Text { get; set; }
        private Font myFont { get; set; }
        public Point EndPoint { get { return new Point(controlWidth, 20); } }

        private Image[] imgs =
            {
                Image.FromFile("email_bg_left.png"),
                Image.FromFile("email_bg_middle.png"),
                Image.FromFile("email_bg_right.png"),
                Image.FromFile("email_bg_over_left.png"),
                Image.FromFile("email_bg_over_middle.png"),
                Image.FromFile("email_bg_over_right.png")
            };

        private Image drawingImage, selectedImage;
        private int controlWidth;
        private bool isSelected;

        public PrettyControl(String text, Font font)
        {
            Text = text;
            myFont = font;
            createBitmap();
            isSelected = false;
        }

        private void createBitmap()
        {
            int textwidth = TextRenderer.MeasureText(Text, myFont).Width;
            controlWidth = 9 + (textwidth/10 + 1)*10 + 17;

            drawingImage = new Bitmap(controlWidth, 20, PixelFormat.Format32bppArgb);
            selectedImage = new Bitmap(controlWidth, 20, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(drawingImage);
            Graphics gfx_selected = Graphics.FromImage(selectedImage);

            Point drawpoint = new Point(0, 0);
            gfx.DrawImage(imgs[0], drawpoint.X, drawpoint.Y);
            gfx_selected.DrawImage(imgs[3], drawpoint.X, drawpoint.Y);
            drawpoint.X += 9;
            int endImageX = drawpoint.X;

            for (int i = 0; i <= textwidth / 10; ++i)
            {
                gfx.DrawImage(imgs[1], endImageX, drawpoint.Y);
                gfx_selected.DrawImage(imgs[4], endImageX, drawpoint.Y);
                endImageX += 10;
            }

            gfx.DrawImage(imgs[2], endImageX, drawpoint.Y);
            gfx_selected.DrawImage(imgs[5], endImageX, drawpoint.Y);

            gfx.DrawString(Text, myFont, Brushes.WhiteSmoke, 9 + margin.X, margin.Y);
            gfx_selected.DrawString(Text, myFont, Brushes.WhiteSmoke, 9 + margin.X, margin.Y);

            gfx.Dispose();
            gfx_selected.Dispose();
        }

        public void Paint (Graphics gfx, Point start)
        {
            gfx.DrawImage(isSelected? selectedImage : drawingImage, start.X, start.Y);
        }

        public void OnMouseExit()
        {
            isSelected = false;
        }

        public void OnMouseEnter(Point point)
        {
            isSelected = true;
        }
    }
}
