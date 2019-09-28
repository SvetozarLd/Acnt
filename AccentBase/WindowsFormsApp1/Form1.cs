using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccentBase;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AccentBase.CustomControls.OrdersDataGridView ordersDataGridView = new AccentBase.CustomControls.OrdersDataGridView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
