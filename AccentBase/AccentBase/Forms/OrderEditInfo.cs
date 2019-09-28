using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace AccentBase.Forms
{
    public partial class OrderEditInfo : Form
    {
        private FormBaseEdit parentform = null;







        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        //public const int HWND_TOPMOST = -1;
        //public const int HWND_NOTOPMOST = -2;
        //public const int SWP_NOMOVE = 0x0002;
        //public const int SWP_NOSIZE = 0x0001;
        //public const int SWP_NOACTIVATE = 0x0010;




        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        //private const int HWND_TOPMOST = -1;
        //private const int HWND_NOTOPMOST = -2;
        //private const int SWP_NOMOVE = 0x0002;
        //private const int SWP_NOSIZE = 0x0001;
        //private const int SWP_SHOWWINDOW = 0x0040;
        //private const int SWP_NOACTIVATE = 0x0010;

        //[DllImport("user32.dll")]
        //static extern bool SetForegroundWindow(IntPtr hWnd);
        //     public static extern bool SetWindowPos(
        //int hWnd,               // window handle
        //int hWndInsertAfter,    // placement-order handle
        //int X,                  // horizontal position
        //int Y,                  // vertical position
        //int cx,                 // width
        //int cy,                 // height
        //uint uFlags);           // window positioning flags
        //     public const int HWND_BOTTOM = 0x1;
        //     public const uint SWP_NOSIZE = 0x1;
        //     public const uint SWP_NOMOVE = 0x2;
        //     public const uint SWP_SHOWWINDOW = 0x40;
        //        [DllImport("user32.dll")]
        //        [return: MarshalAs(UnmanagedType.Bool)]
        //        private static extern bool IsIconic(IntPtr hWnd); //returns true if window is minimized

        //        private List<IntPtr> windowsHandles = new List<IntPtr>();
        ////fill list with window handles

        //for (i = 0; i<windowsHandles.Count; i++)
        //{
        //    if (windowsHandles[i] != browserHandle && windowsHandles[i] != this.Handle && !IsIconic(windowsHandles[i]))
        //    { 
        //        SetWindowPos(windowsHandles[i], HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        //    }
        //}


        public OrderEditInfo(FormBaseEdit e)
        {
            InitializeComponent();

            if (e != null)
            {

                parentform = e;
                Owner = null;
                //parentform.Owner = this;
                //BringToFront();
                parentform.Resize += Parentform_Resize;
                parentform.LocationChanged += Parentform_LocationChanged;
                parentform.FormClosing += Parentform_FormClosing;
                parentform.RTFChangeEvent += Parentform_RTFChangeEvent;
                parentform.Activated += Parentform_Activated;
                LocateToCoordinate();
            }
            else { Close(); }
        }

        private void Parentform_Activated(object sender, EventArgs e)
        {
            SetPosition();
        }

        public delegate void RecieveRTFtDelegate(string e);
        private void Parentform_RTFChangeEvent(object sender, string str)
        {
            if (str != null) { Invoke(new RecieveRTFtDelegate(RecieveRTF), str); }
        }
        private void RecieveRTF(string str)
        {
            richTextBox1.Rtf = str;
        }

        private void Parentform_FormClosing(object sender, FormClosingEventArgs e)
        {
            parentform.Resize -= Parentform_Resize;
            parentform.LocationChanged -= Parentform_LocationChanged;
            parentform.RTFChangeEvent -= Parentform_RTFChangeEvent;
            parentform.Activated -= Parentform_Activated;
            Close();
        }

        private void Parentform_LocationChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            SetPosition();
        }

        private void Parentform_Resize(object sender, EventArgs e)
        {
            SetPosition();
        }

        private void SetPosition()
        {
            switch (parentform.WindowState)
            {
                case FormWindowState.Minimized:
                    WindowState = FormWindowState.Minimized;
                    break;
                case FormWindowState.Normal:
                    WindowState = FormWindowState.Normal;
                    //if (!parentMaximize)
                    //{



                    //px = parentform.Location.X;
                    //py = parentform.Location.Y;
                    //pcx = parentform.Location.X + parentform.Width;
                    //pcy = parentform.Location.Y + parentform.Height;



                    //}
                    //if (moveend)
                    //{
                    //    Location = new Point(parentform.Location.X + parentform.Width - Width + currentX, parentform.Location.Y + SystemInformation.CaptionHeight + 10);
                    //}
                    ////LocationX = parentform.Location.X + parentform.Width;
                    //Height = parentform.Height - SystemInformation.CaptionHeight - 22 - 20;
                    parentMaximize = false;
                    LocateToCoordinate();
                    break;
                case FormWindowState.Maximized:
                    //if (parentMaximize)
                    //{
                    //    Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, px, py, pcx, pcy, Program.SWP_SHOWWINDOW);
                    //    //Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, parentform.Location.X, parentform.Location.Y, parentform.Location.X + parentform.Width - this.Height, parentform.Location.Y + parentform.Height, Program.SWP_NOMOVE | Program.SWP_NOSIZE);
                    //    parentMaximize = false;
                    //}
                    //else
                    //{
                    //parentMaximize = true;
                    //parentform.WindowState = FormWindowState.Normal;

                    //parentMaximize = true;
                    if (!timer1.Enabled)
                    {
                        Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, -8, -8, Screen.PrimaryScreen.WorkingArea.Width - this.Width+8, Screen.PrimaryScreen.WorkingArea.Height+16, Program.SWP_SHOWWINDOW);
                    }
                    parentMaximize = true;

                    //    parentform.WindowState = FormWindowState.Normal;
                    //}
                    //Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, parentform.Location.X, parentform.Location.Y, parentform.Location.X + parentform.Width - this.Height, parentform.Location.Y + parentform.Height, Program.SWP_SHOWWINDOW);

                    LocateToCoordinate();
                    //parentform.Resize -= Parentform_Resize;
                    //parentform.LocationChanged -= Parentform_LocationChanged;
                    //parentform.RTFChangeEvent -= Parentform_RTFChangeEvent;
                    //parentform.Activated -= Parentform_Activated;

                    //Close();
                    break;
            }

        }
        //int px = 0;
        //int py = 0;
        //int pcx = 0;
        //int pcy = 0;
        //private bool moveend = false;
        private bool parentMaximize = false;
        private int currentX = 0;
        private bool toclose = false;
        private void OrderEditInfo_Shown(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = false;
            //this.TopMost = true;
            //this.Focus();
            //this.BringToFront();
            //this.TopMost = false;
        }
        private void Opening()
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int baseX = parentform.Location.X + parentform.Width;

            if (toclose)
            {
                if (baseX < (Location.X + Width))
                {
                    
                    currentX = currentX - 6;
                    LocateToCoordinate();
                    //if (parentMaximize)
                    //{

                    //    Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, 0, 0, Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height, Program.SWP_SHOWWINDOW);
                    //}

                }
                else
                {
                    if (parentMaximize)
                    {
                        Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, -8, -8, Screen.PrimaryScreen.WorkingArea.Width +8, Screen.PrimaryScreen.WorkingArea.Height+16, Program.SWP_SHOWWINDOW);
                    }
                    parentform.Resize -= Parentform_Resize;
                    parentform.LocationChanged -= Parentform_LocationChanged;
                    parentform.RTFChangeEvent -= Parentform_RTFChangeEvent;
                    parentform.Activated -= Parentform_Activated;
                    Close();
                    //currentX = this.Width;
                    //parentform.Owner = null;
                    //Owner = parentform;
                    //moveend = true;
                    //timer1.Stop();
                    //parentform.orderEditInfoEnable();
                }
            }
            else
            {
                if (baseX > Location.X)
                {
                    if (parentMaximize)
                    {
                        Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, -8, -8, Screen.PrimaryScreen.WorkingArea.Width - Width +8, Screen.PrimaryScreen.WorkingArea.Height+16, Program.SWP_SHOWWINDOW);
                        baseX = parentform.Location.X + parentform.Width;
                    }
                    currentX = currentX + 6;
                    //Location = new Point(parentform.Location.X + parentform.Width - Width + currentX, parentform.Location.Y + SystemInformation.CaptionHeight + 10);
                    LocateToCoordinate();

                    //Height = parentform.Height - SystemInformation.CaptionHeight - 22 - 20;
                    //parentform.Activate();
                }
                else
                {
                    currentX = this.Width;
                    //parentform.Owner = null;
                    //Owner = parentform;
                    //moveend = true;
                    timer1.Stop();
                    if (parentMaximize)
                    {
                        Program.SetWindowPos(parentform.Handle, Program.HWND_NOTOPMOST, 0, 0, Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height, Program.SWP_SHOWWINDOW);
                    }
                    parentform.orderEditInfoEnable();
                }
            }
        }

        private void OrderEditInfo_Load(object sender, EventArgs e)
        {
            if (parentform.WindowState == FormWindowState.Maximized) { parentMaximize = true; }
            //px = parentform.Location.X;
            //py = parentform.Location.Y;
            //pcx = parentform.Location.X + parentform.Width;
            //pcy = parentform.Location.Y + parentform.Height;
            Location = new Point(parentform.Location.X + parentform.Width - Width, parentform.Location.Y + SystemInformation.CaptionHeight + 10);
            Height = parentform.Height - SystemInformation.CaptionHeight - 22 - 20;
            timer1.Start();
        }

        private void LocateToCoordinate()
        {
            int x = parentform.Location.X + parentform.Width - Width + currentX;
            int y = parentform.Location.Y + SystemInformation.CaptionHeight + 10;
            int cx = parentform.Location.X + parentform.Width - Width + currentX + this.Height;
            int cy = parentform.Height - SystemInformation.CaptionHeight - 22 - 20;
            Program.SetWindowPos(this.Handle, (int)parentform.Handle, x, y, cx, cy, Program.SWP_NOSIZE | Program.SWP_NOACTIVATE); //
            this.Height = parentform.Height - SystemInformation.CaptionHeight - 22 - 20;
        }

        public void ToClose()
        {
            //this.Owner = null;
            toclose = true;
            timer1.Start();
        }
    }
}
