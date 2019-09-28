using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccentBase.Forms
{
    public partial class FormSendToServer : Form
    {
        public FormSendToServer()
        {
            InitializeComponent();
        }
        DataTable www = new DataTable();
        private void FormSendOrder_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.N0091;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //dataGridView1.RowTemplate.Height = 50;
            //www = SqlLite.SendChangeToSocket.TableOrdersSend.Copy();
            //dataGridView1.DataSource = www;
            //Program.Event_SendChangeToSocket += SendChangeToSocket_Event_SendChangeToSocket;
            databaseConnect();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            databaseConnect();
        }
        bool Event_FTPOperationNotSubs = true;
        private void databaseConnect()
        {
            try
            {
                timer1.Enabled = false;
                www = SqlLite.SendChangeToSocket.TableOrdersSend.Copy();
                dataGridView1.DataSource = www;
                if (Event_FTPOperationNotSubs)
                {
                    Program.Event_SendChangeToSocket += SendChangeToSocket_Event_SendChangeToSocket;
                    Event_FTPOperationNotSubs = false;
                }

            }
            catch
            {
                timer1.Enabled = true;
            }

        }

        internal delegate void SendChangeToSocketDelegate(Program.ConnectEventArgs e);
        private void SendChangeToSocket_Event_SendChangeToSocket(object sender, Program.ConnectEventArgs e)
        {
            if (e != null) { if (InvokeRequired) { Invoke(new SendChangeToSocketDelegate(TableEventHandler), e); } else { TableEventHandler(e); } }
        }
        private void TableEventHandler(Program.ConnectEventArgs e)
        {
                switch (e.MessageCommand)
                {
                    case SocketClient.TableClient.SocketMessageCommand.RowsInsert:
                        DataRow dr = www.NewRow();
                        dr["id"] = e.Id;
                        dr["name"] = e.Name;
                        dr["notes"] = e.Notes;
                        dr["dt_date_insert"] = e.DateInit;
                        dr["order_command"] = e.OrderCommand;
                        www.Rows.Add(dr);
                        break;
                case SocketClient.TableClient.SocketMessageCommand.RowsDelete:
                    DataRow row = www.Select("id =" + e.Id).SingleOrDefault();
                    if (row != null) { www.Rows.Remove(row); }
                    break;
            }

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenOrders();
                e.Handled = true;
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenOrders();
        }
        private void OpenOrders()
        {
            if (dataGridView1.SelectedRows != null && dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.Cells["id"] != null && row.Cells["id"].Value != null)
                    {
                        EditOrder(Convert.ToInt32(row.Cells["id"].Value));

                    }
                }
            }
        }


        private void EditOrder(int order_id)
        {
            //if (OpenedOrders.ContainsKey(order_id))
            //{
            //    OpenedOrders[order_id].WindowState = FormWindowState.Normal;
            //    OpenedOrders[order_id].Activate();
            //}
            //else
            //{

            //    ProtoClasses.ProtoOrders.protoOrder po = null;
            //    bool neworder = true;
            //    if (order_id > 0)
            //    {
            //        DataRow row = customDataGridView_Orders.SourceTable.Select("id = " + order_id).FirstOrDefault();
            //        if (row != null)
            //        {
            //            neworder = false;
            //            po = Utils.DataRowToProto.OrderToProto(row);
            //        }
            //    }
            //    if (TableOrdersHistory != null)
            //    {
            //        TableOrdersHistory.DefaultView.RowFilter = "work_id = " + order_id;
            //        TableOrdersHistory.DefaultView.Sort = "Datetime_date ASC";
            //        Forms.FormBaseEdit frm = new Forms.FormBaseEdit(po, TableOrdersHistory.DefaultView.ToTable(), neworder);//Convert.ToInt32(customDataGrid_StockBlock.Rows[e.RowIndex].Cells["stockblock_id"].Value), string.Empty, string.Empty);
            //        //frm.ShowInTaskbar = false;
            //        frm.Owner = this;
            //        frm.StartPosition = FormStartPosition.CenterParent;
            //        OpenedOrders.Add(order_id, frm);
            //        //frm.FormClosing += Frm_FormClosing;
            //        frm.FormClosed += Frm_FormClosed;
            //        frm.Show(this);
            //    }
            //}
        }


    }
}
