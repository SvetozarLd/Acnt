using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace AccentBase.Forms
{
    public partial class FormContextOrderList : Form
    {
        public int result { get; set; }
        FormBase formBase = null;
        public FormContextOrderList(Dictionary<Int64, string> ordersIDs, FormBase e)
        {
            InitializeComponent();
            result = 0;
            var list = ordersIDs.ToList();
            dataGridView1.DataSource = list;
            formBase = e;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.RowIndex != -1 && dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value != null)
            {
                if (formBase != null) { formBase.EditOrder(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value), 7); }
                //result = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value);
                //Close();
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.RowIndex != -1 && dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value != null)
                {
                    //result = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value);
                    //Close();
                    if (formBase != null){ formBase.EditOrder(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Keys"].Value), 7); }
                }
            }
            else
            {
                if (e.KeyCode == System.Windows.Forms.Keys.Escape)
                {
                    result = 0; Close();
                }
            }
        }
    }
}
