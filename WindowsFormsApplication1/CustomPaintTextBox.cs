using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication1
{
    public class CustomPaintTextBox : NativeWindow
    {
        private const int WM_PAINT = 0xF;

        private const int WM_SETFOCUS = 0x7;
        private const int WM_KILLFOCUS = 0x8;

        private const int WM_SETFONT = 0x30;

        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_CHAR = 0x0102;


        private TextBox parentTextBox;
        private Bitmap bitmap;
        private Graphics bufferGraphics;
        private List<PrettyControl> pretiControlList;
        private PrettyControl controlMouseOver;

        private int fps = 0;
        private long lastUpdate = 0;


        // this is where we intercept the Paint event for the TextBox at the OS level  
        protected override void WndProc(ref Message m)
        {
            //Console.WriteLine("message: " + DateTime.Now.Millisecond);
            fps++;
            TimeSpan espam = new TimeSpan(DateTime.Now.Ticks - lastUpdate);
            if (espam.TotalMilliseconds > 1000)
            {
                Console.WriteLine("FPS: " + fps);
                fps = 0;
                lastUpdate = DateTime.Now.Ticks;
            }
            if (this.parentTextBox != null)
            {
                switch (m.Msg)
                {
                    case WM_PAINT: // this is the WM_PAINT message  
                        // invalidate the TextBox so that it gets refreshed properly  
                        //parentTextBox.Invalidate();
                        // call the default win32 Paint method for the TextBox first  
                        //base.WndProc(ref m);
                        // now use our code to draw extra stuff over the TextBox
                        //if (espam.TotalMilliseconds > 1000)
                            RePaint();
                        break;
                    case WM_SETFONT:
                        SetMargin();
                        break;
                    case WM_SETFOCUS:
                    case WM_KILLFOCUS:
                        RePaint();
                        break;
                    case WM_LBUTTONDOWN:
                    case WM_RBUTTONDOWN:
                    case WM_MBUTTONDOWN:
                        RePaint();
                        break;
                    case WM_LBUTTONUP:
                    case WM_RBUTTONUP:
                    case WM_MBUTTONUP:
                        RePaint();
                        break;
                    case WM_LBUTTONDBLCLK:
                    case WM_RBUTTONDBLCLK:
                    case WM_MBUTTONDBLCLK:
                        RePaint();
                        break;
                    case WM_KEYDOWN:
                    case WM_CHAR:
                    case WM_KEYUP:
                        RePaint();
                        break;
                    case WM_MOUSEMOVE:
                        Point pt = new Point();
                        pt.X = (int)m.LParam & 0x0000ffff;  // LOWORD = x
                        pt.Y = (int)m.LParam >> 16;         // HIWORD = y
                        OnMouseOverEvent(pt, !m.WParam.Equals(IntPtr.Zero));
                        RePaint();
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            else
            {
                switch (m.Msg)
                {
                    case WM_PAINT:
                        MoveControl();
                        break;
                }
            }
        }

        private void OnMouseOverEvent(Point mousePos, bool mouseClick)
        {
            Point start = new Point(0, 0);
            bool found = false;
            foreach (var prettyControl in pretiControlList)
            {
                Rectangle rc = new Rectangle(start.X, start.Y, prettyControl.EndPoint.X, prettyControl.EndPoint.Y);
                if (rc.Contains(mousePos))
                {
                    if (controlMouseOver != prettyControl)
                    {
                        if (controlMouseOver != null) controlMouseOver.OnMouseExit();
                        prettyControl.OnMouseEnter(new Point(mousePos.X - start.X, mousePos.Y - start.Y));
                        controlMouseOver = prettyControl;
                    }
                    found = true;
                }
                start.X += prettyControl.EndPoint.X + 5;
            }
            if (!found && controlMouseOver != null)
            {
                controlMouseOver.OnMouseExit();
                controlMouseOver = null;
            }
        }

        private void SetMargin()
        {
            //throw new NotImplementedException();
        }

        private void MoveControl()
        {
            //throw new NotImplementedException();
        }

        private void RePaint()
        {
            Graphics gfx = Graphics.FromHwnd(parentTextBox.Handle);
            Point start = new Point(0, 0);
            foreach (var prettyControl in pretiControlList)
            {
                prettyControl.Paint(gfx, start);
                start.X += prettyControl.EndPoint.X + 5;
            }
            gfx.Dispose();
        }
        
        public CustomPaintTextBox(TextBox textBox)
        {
            this.parentTextBox = textBox;
            this.bitmap = new Bitmap(textBox.Width, textBox.Height);
            this.bufferGraphics = Graphics.FromImage(this.bitmap);
            this.bufferGraphics.Clip = new Region(textBox.ClientRectangle);
            // Start receiving messages (make sure you call ReleaseHandle on Dispose):  
            this.AssignHandle(textBox.Handle);

            pretiControlList = new List<PrettyControl>
                    {
                        new PrettyControl("dani@nec.com", parentTextBox.Font),
                        new PrettyControl("otro@nec.com", parentTextBox.Font)
                    };

            lastUpdate = DateTime.Now.Ticks;
        }

        ~CustomPaintTextBox()
        {
            this.ReleaseHandle();
        }

    }
}