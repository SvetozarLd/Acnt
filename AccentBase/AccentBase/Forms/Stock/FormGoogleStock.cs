using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace AccentBase.Forms.Stock
{
    public partial class FormGoogleStock : Form
    {
        ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser("https://docs.google.com/......");
        public FormGoogleStock()
        {
            InitializeComponent();
        }

        private void FormGoogleStock_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Controls.Add(chromiumWebBrowser);
            chromiumWebBrowser.Dock = DockStyle.Fill;
            chromiumWebBrowser.SendToBack();
            statusStrip1.SendToBack();
        }
    }
}
