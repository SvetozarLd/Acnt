using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System;
using System.Collections.Generic;
//using org.pdfclown;
//using org.pdfclown.bytes;
//using org.pdfclown.documents;
//using org.pdfclown.files;
//using org.pdfclown.tools;
//using org.pdfclown.documents.contents.composition;
//using org.pdfclown.documents.contents.fonts;
//using ImageProcessor;
//using ImageProcessor.Processors;
//using ImageProcessor.Plugins;
//using ImageProcessor.Common;
//using ImageProcessor.Configuration;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
namespace AccentBase.Forms
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
            OpenedOrders = new Dictionary<long, FormBaseEdit>();
            //this.Owner = null;
        }

        #region DataTables
        //DataTable TableOrders = null;
        //DataTable TableViewOrders = null;
        private DataTable TableOrdersHistory = null;
        private DataTable dataTable_SocketSend = null;
        #endregion

        #region Событие сервера
        internal delegate void SocketDelegate(SocketClient.TableClient.ConnectEventArgs e);
        //private 
        private void TableClient_SocketClientEvent(object sender, SocketClient.TableClient.ConnectEventArgs e)
        {
            if (e != null)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new SocketDelegate(ServerHandler), e);
                    }
                    else { ServerHandler(e); }

                }
                catch { }
            }
        }
        private void ServerHandler(SocketClient.TableClient.ConnectEventArgs e)
        {
            if (e.Status)
            {
                toolStripStatusLabel1.Image = Properties.Resources.taskplay;
                toolStripStatusLabel1.Text = "Подключение к серверу: ок";
            }
            else
            {
                if (SocketClient.TableClient.autoconnect)
                {
                    toolStripStatusLabel1.Image = Properties.Resources.wait;
                    toolStripStatusLabel1.Text = "Подключение к серверу: попытка соединения";
                }
                else
                {
                    toolStripStatusLabel1.Image = Properties.Resources.taskstop;
                    toolStripStatusLabel1.Text = "Вы остановили соединение с сервером";
                }
            }
            //if 
            //textBox1.Text += e.Comment + Environment.NewLine;
            //listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
            //listBox1.Items.Add("Событие подключения:" + e.Message);
            //listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        #endregion


        CheckBox cb = new CheckBox();

        private void FormBase_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon_Text_Document;
            tabControl1.ItemSize = new Size(0, 1);


            cb.Text = "Игнорировать статус";
            cb.Checked = true;
            cb.CheckStateChanged += cb_CheckedChanged;
            cb.BackColor = Color.Transparent;
            ToolStripControlHost host = new ToolStripControlHost(cb);
            toolStrip_Main.Items.Insert(19, host);

            customDataGridView_Orders.AutoGenerateColumns = false;
            dataGridView_History.AutoGenerateColumns = false;
            SetDoubleBuffered(customDataGridView_Orders);



            //TableOrders = new DataTable();
            //TableOrders = SqlLite.order.TableOrders.Copy();
            //TableViewOrders = new DataTable();
            //TableOrders.DefaultView.RowFilter = string.Empty;
            //TableViewOrders = TableOrders.DefaultView.ToTable();
            //customDataGridView1.DataSource = null;
            //customDataGridView1.DataSource = TableViewOrders;
            //TableOrders = SqlLite.Order.TableOrders.Copy();
            customDataGridView_Orders.SourceTable = SqlLite.Order.TableOrders;
            TableOrdersHistory = SqlLite.OrderHistory.TableOrderHistory.Copy();
            customDataGridView_Orders.SortingOrder = "id DESC";
            SocketClient.TableClient.SocketClientEvent += TableClient_SocketClientEvent;
            SqlLite.SqlEvent.OrderUpdate += SqlEvent_OrderUpdate;
            SqlLite.SqlEvent.OrderChangeStates += SqlEvent_OrderChangeStates;
            Program.BalloonTipClickedEvent += Program_BalloonTipClickedEvent;

            dataGridView_SocketSend.AutoGenerateColumns = false;
            dataGridView_SocketSend.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //dataGridView1.RowTemplate.Height = 50;

            dataTable_SocketSend = SqlLite.SendChangeToSocket.TableOrdersSend.Copy();
            dataGridView_SocketSend.DataSource = dataTable_SocketSend;
            Program.Event_SendChangeToSocket += SendChangeToSocket_Event_SendChangeToSocket;
            SqlLite.FtpSchedule.Event_FTPOperation += FtpSchedule_Event_FTPOperation;
            GUI_ShowPanels();
            //orderMenuTreeView1.LostFocus += OrderMenuTreeView1_LostFocus;
            //orderMenuTreeView1.GotFocus += OrderMenuTreeView1_GotFocus;
            orderMenuTreeView1.ExpandAll();
            orderMenuTreeView1.SelectedNode_Name = "nodeAwait";
            //Program.SetWindowPos(this.Handle, Program.HWND_NOTOPMOST, 0, 0, 0, 0, Program.SWP_NOMOVE | Program.SWP_NOSIZE);
        }

        public static void SetDoubleBuffered(Control control)
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        #region по фтп пришли какие-то файлы
        public delegate void Delegate_FTPOperation(Ftp.FtpReciever.FTPOperationEventArgs e);
        private void FtpSchedule_Event_FTPOperation(object sender, Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            if (e != null)
            {
                if (InvokeRequired)
                {
                    Invoke(new Delegate_FTPOperation(FileRecieverHandler), e);
                }
                else
                {
                    FileRecieverHandler(e);
                }
            }
        }

        private void FileRecieverHandler(Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null)
            {
                long i = Convert.ToInt64(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value);
                if (i == e.OrderId)
                {
                    if (e.Ended)
                    {
                        if (Utils.CheckDBNull.ToLong(toolStripButton1.Tag) == e.OrderId) { toolStripButton1.Tag = 0; }
                        ShowInfoSelectedOrder();
                    }
                }
            }
        }
        #endregion
        //public TreeNode previousSelectedNode = null;
        //private void orderMenuTreeView1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    orderMenuTreeView1.SelectedNode.BackColor = SystemColors.Highlight;
        //    orderMenuTreeView1.SelectedNode.ForeColor = SystemColors.WindowText;
        //    previousSelectedNode = orderMenuTreeView1.SelectedNode;
        //}
        //private void OrderMenuTreeView1_GotFocus(object sender, EventArgs e)
        //{
        //    orderMenuTreeView1.SelectedNode.BackColor = SystemColors.Window;
        //}

        //private void OrderMenuTreeView1_LostFocus(object sender, EventArgs e)
        //{
        //    orderMenuTreeView1.SelectedNode.BackColor = SystemColors.InactiveCaption;
        //}

        #region изменение статуса заявок
        internal delegate void OrderChangeStatesDelegate(ProtoClasses.ProtoOrdersChangeState.protoRowsList e);
        private void SqlEvent_OrderChangeStates(object sender, ProtoClasses.ProtoOrdersChangeState.protoRowsList e)
        {
            if (e != null)
            {
                if (InvokeRequired) { Invoke(new OrderChangeStatesDelegate(OrderChangeStates), e); }
                else { OrderChangeStates(e); }
            }
        }
        private void OrderChangeStates(ProtoClasses.ProtoOrdersChangeState.protoRowsList e)
        {
            if (e != null && e.plist != null && e.plist.Count > 0)
            {
                foreach (ProtoClasses.ProtoOrdersChangeState.protoRow row in e.plist)
                {
                    if (e.HistoryRows != null && e.HistoryRows.Count > 0) { AddNewHistoryRows(e.HistoryRows); }
                    DataRow dr = customDataGridView_Orders.SourceTable.Select("id =" + row.id).Single();
                    if (dr != null)
                    {
                        dr["status"] = row.state;
                        dr["state_print"] = row.state_print;
                        dr["state_cut"] = row.state_cut;
                        dr["state_cnc"] = row.state_cnc;
                        dr["state_install"] = row.state_install;
                        dr["printerman"] = row.printerman;
                        dr["cutterman"] = row.cutterman;
                        dr["cncman"] = row.cncman;
                        dr["date_ready_print"] = row.date_ready_print;
                        dr["date_ready_cut"] = row.date_ready_cut;
                        dr["date_ready_cnc"] = row.date_ready_cnc;
                        dr["Image_WorkTypes"] = row.Image_WorkTypes;
                    }
                    //if (e.HistoryRows != null && e.HistoryRows.Count > 0) { AddNewHistoryRows(e.HistoryRows); }
                }
                customDataGridView_Orders.ReFresh();
                //ShowInfoSelectedOrder();
            }
        }
        #endregion

        #region открыть заявку по клику из таскбара
        internal delegate void BalloonTipClickedDelegate(Program.BalloonTipClickedEventArgs e);
        private void Program_BalloonTipClickedEvent(object sender, Program.BalloonTipClickedEventArgs e)
        {
            if (e != null) { if (InvokeRequired) { Invoke(new BalloonTipClickedDelegate(BalloonTipClickedEvent), e); } else { BalloonTipClickedEvent(e); } }
        }
        private void BalloonTipClickedEvent(Program.BalloonTipClickedEventArgs e)
        {
            if (e.TableName == SocketClient.TableClient.TableName.TableBase)
            {
                EditOrder(e.Uid, 7);
            }
        }
        #endregion
        #region Прислали новую заявку или изменили старую
        private void SqlEvent_OrderUpdate(object sender, SqlLite.SqlEvent.OrderUpdateEventArgs e)
        {
            if (e != null)
            { if (InvokeRequired) { BeginInvoke(new OrderUpdateDelegate(UpdateOrdersTable), e); } else { UpdateOrdersTable(e); } }
        }
        internal delegate void OrderUpdateDelegate(SqlLite.SqlEvent.OrderUpdateEventArgs e);
        private void UpdateOrdersTable(SqlLite.SqlEvent.OrderUpdateEventArgs e)
        {
            if (e.Row != null)
            {
                switch (e.Command)
                {
                    case SocketClient.TableClient.SocketMessageCommand.RowsUpdate:
                        DataRow dr = customDataGridView_Orders.SourceTable.Select("id =" + e.Id).Single();
                        try
                        {
                            if (dr != null)
                            {
                                AddNewHistoryRows(e.HistoryList);
                                dr.ItemArray = e.Row;
                                customDataGridView_Orders.ReFresh();
                                //ShowInfoSelectedOrder();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка!", ex.Message);
                        }
                        break;
                    case SocketClient.TableClient.SocketMessageCommand.RowsInsert:
                        try
                        {
                            AddNewHistoryRows(e.HistoryList);
                            DataRow dri = customDataGridView_Orders.SourceTable.NewRow();
                            dri.ItemArray = e.Row;
                            customDataGridView_Orders.SourceTable.Rows.Add(dri);
                            customDataGridView_Orders.ReFresh();
                            //ShowInfoSelectedOrder();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка!", ex.Message);
                        }
                        break;
                }
            }
        }

        private void AddNewHistoryRows(List<ProtoClasses.ProtoOrderHistory.protoRow> e)
        {
            if (e != null && e.Count > 0)
            {
                foreach (ProtoClasses.ProtoOrderHistory.protoRow row in e)
                {
                    DataRow dr = TableOrdersHistory.NewRow();
                    dr["id"] = row.id;
                    dr["adder"] = row.adder;
                    dr["work_id"] = row.work_id;
                    dr["status_task"] = row.status_task;
                    dr["note"] = row.note;
                    dr["date_change"] = row.date_change;
                    dr["change_count"] = row.change_count;
                    dr["time_recieve"] = row.time_recieve;
                    dr["Datetime_date"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row.date_change)).ToString("dd.MM.yyyy HH:mm");
                    TableOrdersHistory.Rows.Add(dr);
                }
            }
        }
        #endregion



        private void customDataGridView1_ErrorEvent(object sender, CustomControls.OrdersDataGridView.MyDataGridEventArgs e)
        {
            if (e.Ex != null)
            {
                Trace.WriteLine(e.Ex.Message);
            }
        }








        #region Блок - отсылаемые на сервер изменения
        private bool SocketSendShow = false;
        private void show_SocketSend()
        {
            if (SocketSendShow)
            {
                if (dataTable_SocketSend != null && dataTable_SocketSend.Rows != null && dataTable_SocketSend.Rows.Count > 0)
                {
                    dataGridView_SocketSend.DataSource = null;
                    dataGridView_SocketSend.DataSource = dataTable_SocketSend;
                    int i = dataTable_SocketSend.Rows.Count * 22;
                    if (i < dataGridView_SocketSendMaxHeight) { dataGridView_SocketSend.Height = i; } else { dataGridView_SocketSend.Height = dataGridView_SocketSendMaxHeight; }
                    dataGridView_SocketSend.Visible = true; splitter_SocketSend.Visible = true;
                }
                else { dataGridView_SocketSend.Visible = false; splitter_SocketSend.Visible = false; }

            }
            else
            {
                dataGridView_SocketSend.Visible = false; splitter_SocketSend.Visible = false;
            }
        }


        internal delegate void SendChangeToSocketDelegate(Program.ConnectEventArgs e);
        private void SendChangeToSocket_Event_SendChangeToSocket(object sender, Program.ConnectEventArgs e)
        {
            if (e != null) { if (InvokeRequired) { BeginInvoke(new SendChangeToSocketDelegate(TableEventHandler), e); } else { TableEventHandler(e); } }
        }
        private void TableEventHandler(Program.ConnectEventArgs e)
        {
            switch (e.MessageCommand)
            {
                case SocketClient.TableClient.SocketMessageCommand.RowsInsert:
                    DataRow dr = dataTable_SocketSend.NewRow();
                    dr["id"] = e.Id;
                    dr["name"] = e.Name;
                    dr["notes"] = e.Notes;
                    dr["dt_date_insert"] = e.DateInit;
                    dr["order_command"] = e.OrderCommand;
                    dataTable_SocketSend.Rows.Add(dr);
                    show_SocketSend();
                    break;
                case SocketClient.TableClient.SocketMessageCommand.RowsDelete:
                    DataRow row = dataTable_SocketSend.Select("id =" + e.Id).SingleOrDefault();
                    if (row != null) { dataTable_SocketSend.Rows.Remove(row); }
                    show_SocketSend();
                    break;
            }

        }
        #endregion







        #region открыть заявку
        public Dictionary<long, FormBaseEdit> OpenedOrders { get; set; }
        public void EditOrder(long order_id, int orderstate)
        {
            if (OpenedOrders.ContainsKey(order_id))
            {
                OpenedOrders[order_id].WindowState = FormWindowState.Normal;
                OpenedOrders[order_id].Activate();
            }
            else
            {

                ProtoClasses.ProtoOrders.protoOrder po = null;
                bool neworder = true;
                if (order_id > 0)
                {
                    DataRow row = customDataGridView_Orders.SourceTable.Select("id = " + order_id).FirstOrDefault();
                    if (row != null)
                    {
                        neworder = false;
                        po = Utils.DataRowToProto.OrderToProto(row);
                    }
                }
                if (TableOrdersHistory != null)
                {
                    TableOrdersHistory.DefaultView.RowFilter = "work_id = " + order_id;
                    TableOrdersHistory.DefaultView.Sort = "Datetime_date ASC";
                    Forms.FormBaseEdit frm = new Forms.FormBaseEdit(this, po, TableOrdersHistory.DefaultView.ToTable(), neworder, orderstate);
                    //{
                    //    //frm.ShowInTaskbar = false;
                    //    //Owner = this,
                    //    //StartPosition = FormStartPosition.CenterParent
                    //    //StartPosition = FormStartPosition.CenterScreen
                    //};//Convert.ToInt32(customDataGrid_StockBlock.Rows[e.RowIndex].Cells["stockblock_id"].Value), string.Empty, string.Empty);
                    OpenedOrders.Add(order_id, frm);
                    //frm.FormClosing += Frm_FormClosing;
                    frm.FormClosed += Frm_FormClosed;
                    frm.Show(this);
                }
            }
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormBaseEdit frm = sender as FormBaseEdit;
            if (frm.order != null)
            {
                KeyValuePair<long, FormBaseEdit> item = OpenedOrders.First(kvp => kvp.Value == frm);
                OpenedOrders.Remove(item.Key);
                frm.FormClosed -= Frm_FormClosed;
            }
        }

        //private void Frm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    FormBaseEdit frm = sender as FormBaseEdit;
        //    if (frm.order != null)
        //    {
        //        var item = OpenedOrders.First(kvp => kvp.Value == frm);
        //        OpenedOrders.Remove(item.Key);
        //        frm.FormClosing -= Frm_FormClosing;
        //    }
        //}
        #endregion

        private void customDataGridView_Orders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null)
                //{
                //    EditOrder(Convert.ToInt32(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value));
                //}
                OpenOrders();
                e.Handled = true;
            }
        }

        private void customDataGridView_Orders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenOrders();
            //if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            //{

            //}

            //foreach (customDataGridView_Orders)
            //if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null)
            //{
            //    EditOrder(Convert.ToInt32(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value));
            //}
        }

        private void OpenOrders()
        {
            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            {
                if (customDataGridView_Orders.SelectedRows.Count == 1)
                {
                    EditOrder(Convert.ToInt32(customDataGridView_Orders.SelectedRows[0].Cells["id"].Value), Convert.ToInt32(customDataGridView_Orders.SelectedRows[0].Cells["OrderState"].Value));
                }
                else
                {
                    Dictionary<long, string> dic = new Dictionary<long, string>();

                    foreach (DataGridViewRow row in customDataGridView_Orders.SelectedRows)
                    {
                        if (row.Cells["id"] != null && row.Cells["id"].Value != null)
                        {
                            dic.Add(Utils.CheckDBNull.ToLong(row.Cells["id"].Value), Utils.CheckDBNull.ToString(row.Cells["work_name"].Value));
                        }
                    }
                    if (dic.Count > 0)
                    {
                        FormContextOrderList frm = new FormContextOrderList(dic, this);
                        frm.Show();
                    }
                }

            }
        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //textBox2.Text = string.Empty;
        //    listView1.Items.Clear();
        //    for (int i = 1; i < 300; i++)
        //    {
        //        listView1.Items.Add(Convert.ToString((char)i));
        //        listView1.Items[listView1.Items.Count-1].Tag = i.ToString();
        //        //textBox2.Text += (char)i + Environment.NewLine;
        //        //textBox3.Text += i.ToString() + Environment.NewLine;
        //    }  
        //}

        //private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (listView1.SelectedItems.Count > 0)
        //    {
        //        toolStripStatusLabel2.Text = listView1.SelectedItems[0].Tag.ToString();
        //    }
        //    else { toolStripStatusLabel2.Text = string.Empty; }
        //}
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_QuickSearch.Text.Trim().Equals(string.Empty)) { return; }
            else
            {
                textBox_QuickSearch_TextChanged(textBox_QuickSearch, null);
            }
        }
        private void textBox_QuickSearch_TextChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterOfAllOrders = checkBox1.Checked;
            customDataGridView_Orders.Filter = textBox_QuickSearch.Text.Trim();
        }




        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text.Trim().Equals(string.Empty)) { return; }
            else
            {
                toolStripTextBox1_TextChanged(textBox_QuickSearch, null);
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterOfAllOrders = cb.Checked;
            customDataGridView_Orders.Filter = toolStripTextBox1.Text.Trim();
        }








        private void panel_Right_SizeChanged(object sender, EventArgs e)
        {
            panel_QuickSearch.Location = new System.Drawing.Point(panel_Right.Location.X, panel_QuickSearch.Location.Y);
            panel_QuickSearch.Size = new Size(panel_Right.Size.Width, panel_QuickSearch.Size.Height);
        }

        #region Фильтрация
        #region Фильтрация заявок в зависимости от вида работ
        private void Button_OrderViewAll_CheckedChanged(object sender, EventArgs e)
        {
            if (Button_OrderViewAll.Checked)
            {
                Button_OrderViewPrint.Enabled = false;
                Button_OrderViewCut.Enabled = false;
                Button_OrderViewCnc.Enabled = false;
                Button_OrderViewInstall.Enabled = false;
            }
            else
            {
                Button_OrderViewPrint.Enabled = true;
                Button_OrderViewCut.Enabled = true;
                Button_OrderViewCnc.Enabled = true;
                Button_OrderViewInstall.Enabled = true;
            }
            customDataGridView_Orders.FilterAll = Button_OrderViewAll.Checked;
        }
        private void Button_OrderViewPrint_CheckedChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterPrint = Button_OrderViewPrint.Checked;
        }
        private void Button_OrderViewCut_CheckedChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterCut = Button_OrderViewCut.Checked;
        }

        private void Button_OrderViewCnc_CheckedChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterCnc = Button_OrderViewCnc.Checked;
        }

        private void Button_OrderViewInstall_CheckedChanged(object sender, EventArgs e)
        {
            customDataGridView_Orders.FilterInstall = Button_OrderViewInstall.Checked;
        }

        #endregion
        #region Фильтрация по категориям





        public TreeNode previousSelectedNode = null;
        //private void orderMenuTreeView1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    orderMenuTreeView1.SelectedNode.BackColor = SystemColors.Highlight;
        //    orderMenuTreeView1.SelectedNode.ForeColor = SystemColors.WindowText;
        //    previousSelectedNode = orderMenuTreeView1.SelectedNode;
        //}




        private void orderMenuTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (previousSelectedNode != null)
            {
                previousSelectedNode.BackColor = orderMenuTreeView1.BackColor;
                previousSelectedNode.ForeColor = orderMenuTreeView1.ForeColor;
            }
            switch (e.Node.Name)
            {
                case "nodeAll":
                    customDataGridView_Orders.FilterStatus = string.Empty;
                    break;
                case "nodeOpen":
                    customDataGridView_Orders.FilterStatus = "status = 7 OR status = 0 OR status = 1 OR status = 2 OR status = 3 OR status = 5";
                    break;
                case "nodeDraft":// черновик
                    customDataGridView_Orders.FilterStatus = "status = 7";
                    break;
                case "nodeAwait":// Ожидают
                    customDataGridView_Orders.FilterStatus = "status = 0";
                    break;
                case "nodeInWork": // в работе
                    customDataGridView_Orders.FilterStatus = "status = 1";
                    break;
                case "nodePostProc": //постобработка
                    customDataGridView_Orders.FilterStatus = "status = 2";
                    break;
                case "nodeStock": // сделаны
                    customDataGridView_Orders.FilterStatus = "status = 3";
                    break;
                case "nodeStopped": // Остановлены
                    customDataGridView_Orders.FilterStatus = "status = 5";
                    break;
                case "nodeArchive": // Отданы клиенту - закрыты
                    customDataGridView_Orders.FilterStatus = "status = 4";
                    break;
                case "nodeBasket": // Корзина
                    customDataGridView_Orders.FilterStatus = "status = 6";
                    break;
            }
            previousSelectedNode = orderMenuTreeView1.SelectedNode;
            orderMenuTreeView1.SelectedNode.BackColor = SystemColors.Highlight;
            orderMenuTreeView1.SelectedNode.ForeColor = Color.White;
            textBox_QuickSearch.Text = string.Empty;
            toolStripTextBox1.Text = string.Empty;
            //if (previousSelectedNode != null)
            //{
            //    previousSelectedNode.BackColor = orderMenuTreeView1.BackColor;
            //    previousSelectedNode.ForeColor = orderMenuTreeView1.ForeColor;
            //}
            //orderMenuTreeView1.DrawMode = TreeViewDrawMode.Normal;
            //ShowInfoSelectedOrder();
            //if (customDataGridView_Orders)
        }
        #endregion
        #endregion

        #region События при закрытии формы
        private void FormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Проверяем, есть ли открытые заявки
            if (OpenedOrders != null && OpenedOrders.Count > 0)
            {
                DialogResult result = MessageBox.Show(
     "Все изменения будут потеряны." + Environment.NewLine + "Вы действительно хотите выйти?",
     "Внимание, остались открытые заявки.",
     MessageBoxButtons.YesNo,
     MessageBoxIcon.Information,
     MessageBoxDefaultButton.Button1,
     MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes) { SocketClient.TableClient.SocketClientEvent -= TableClient_SocketClientEvent; SqlLite.SqlEvent.OrderUpdate -= SqlEvent_OrderUpdate; GUI_SavePanels(); }
                else
                {
                    e.Cancel = true;
                    ShowAllEditOrders();
                }
            }
            else
            {
                SocketClient.TableClient.SocketClientEvent -= TableClient_SocketClientEvent;
                SqlLite.SqlEvent.OrderUpdate -= SqlEvent_OrderUpdate;
                GUI_SavePanels();
            }

        }
        #endregion

        private void customDataGridView_Orders_SelectionChanged(object sender, EventArgs e)
        {
            ShowInfoSelectedOrder();
            SelectedOrdersGuiEvent();
            //if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null && Convert.ToBoolean(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["be_read"].Value))
            //{
            //    Timer timer = new Timer();
            //    timer.Interval = 2000;
            //    timer.Tick += (obj, ea) => MyElapsedMethod(timer, Convert.ToInt64(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value));
            //    timer.Start();
            //}


            //worker.DoWork += (obj, er) => WorkerDoWork(ordersIDs.Keys.ToList(), myPrintDialog);
        }

        //private void MyElapsedMethod(object sender, Int64 ids)
        //{
        //    ((Timer)sender).Stop();
        //    if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null && Convert.ToInt64(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value) == ids)
        //    {
        //        customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["be_read"].Value = false;
        //        customDataGridView_Orders.ReFresh();
        //    }

        //}


        #region Изменение кнопок и меню при выборе строк в datagridview завок
        private void SelectedOrdersGuiEvent()
        {
            if (customDataGridView_Orders.SelectedRows != null)
            {
                switch (customDataGridView_Orders.SelectedRows.Count)
                {
                    case 0:
                        Button_OrderCopy.Enabled = false;
                        Button_OrderEdit.Enabled = false;
                        Button_OrderDelete.Enabled = false;
                        Button_OrderPrint.Enabled = false;
                        Button_OrderPdfSave.Enabled = false;
                        Button_OrderFilesDownload.Enabled = false;
                        Menu_OrderCopy.Enabled = false;
                        Menu_OrderEdit.Enabled = false;
                        Menu_OrderDelete.Enabled = false;
                        Menu_OrderPrint.Enabled = false;
                        Menu_OrderPdfSave.Enabled = false;
                        Menu_OrderFilesDownload.Enabled = false;
                        break;
                    case 1:
                        Button_OrderCopy.Enabled = true;
                        Button_OrderEdit.Enabled = true;
                        Button_OrderDelete.Enabled = true;
                        Button_OrderPrint.Enabled = true;
                        Button_OrderPdfSave.Enabled = true;
                        Button_OrderFilesDownload.Enabled = true;
                        Menu_OrderCopy.Enabled = true;
                        Menu_OrderEdit.Enabled = true;
                        Menu_OrderDelete.Enabled = true;
                        Menu_OrderPrint.Enabled = true;
                        Menu_OrderPdfSave.Enabled = true;
                        Menu_OrderFilesDownload.Enabled = true;
                        Button_OrderCopy.Text = Menu_OrderCopy.Text = "Открыть копию задания";
                        Button_OrderEdit.Text = Menu_OrderEdit.Text = "Открыть задание";
                        if (orderMenuTreeView1.SelectedNode != null && orderMenuTreeView1.SelectedNode.Text.Equals("Корзина"))
                        {
                            Button_OrderDelete.Text = Menu_OrderDelete.Text = "Удалить задание";
                        }
                        else
                        {
                            Button_OrderDelete.Text = Menu_OrderDelete.Text = "Поместить задание в корзину";
                        }

                        Button_OrderPrint.Text = Menu_OrderPrint.Text = "Печать заявки задания";
                        Button_OrderPdfSave.Text = Menu_OrderPdfSave.Text = "Сохранить заявку задания в PDF";
                        Button_OrderFilesDownload.Text = Button_OrderFilesDownload.Text = "Сохранить все файлы задания";
                        break;
                    default:
                        Button_OrderCopy.Enabled = true;
                        Button_OrderEdit.Enabled = true;
                        Button_OrderDelete.Enabled = true;
                        Button_OrderPrint.Enabled = true;
                        Button_OrderPdfSave.Enabled = true;
                        Button_OrderFilesDownload.Enabled = true;
                        Menu_OrderCopy.Enabled = true;
                        Menu_OrderEdit.Enabled = true;
                        Menu_OrderDelete.Enabled = true;
                        Menu_OrderPrint.Enabled = true;
                        Menu_OrderPdfSave.Enabled = true;
                        Menu_OrderFilesDownload.Enabled = true;
                        Button_OrderCopy.Text = Menu_OrderCopy.Text = "Открыть копии заданий";
                        Button_OrderEdit.Text = Menu_OrderEdit.Text = "Открыть задания";
                        if (orderMenuTreeView1.SelectedNode != null && orderMenuTreeView1.SelectedNode.Text.Equals("Корзина"))
                        {
                            Button_OrderDelete.Text = Menu_OrderDelete.Text = "Удалить задания";
                        }
                        else
                        {
                            Button_OrderDelete.Text = Menu_OrderDelete.Text = "Поместить задания в корзину";
                        }
                        Button_OrderPrint.Text = Menu_OrderPrint.Text = "Печать заявок заданий";
                        Button_OrderPdfSave.Text = Menu_OrderPdfSave.Text = "Сохранить заявки заданий в PDF";
                        Button_OrderFilesDownload.Text = Button_OrderFilesDownload.Text = "Сохранить все файлы заявок";
                        break;
                }
            }
        }
        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripButton1.Tag.ToString() != string.Empty && int.TryParse(toolStripButton1.Tag.ToString(), out int i))
            {
                EditOrder(i, 7);
            }
        }

        private void customDataGridView_Orders_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            toolStripLabel3.Text = customDataGridView_Orders.Rows.Count.ToString();
        }


        #region Показать инфу выбранной заявки
        private void ShowInfoSelectedOrder()
        {
            if (customDataGridView_Orders.CurrentCell != null && customDataGridView_Orders.CurrentCell.RowIndex != -1 && customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value != null)
            {
                long i = Convert.ToInt32(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value);
                if (Convert.ToInt32(toolStripButton1.Tag) != i)
                {
                    toolStripButton1.Text = "№" + i + " : " + customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["work_name"].Value;
                    toolStripButton1.Tag = i;//Convert.ToInt32(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["id"].Value);

                    #region Установка превью
                    Image oldimg = pictureBoxPreview.Image;
                    //pictureBoxPreview.Image = null;
                    //pictureBoxPreview.Image.Dispose();

                    DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["time_recieve"].Value));
                    if (tmpenddate != null)
                    {
                        string OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString() + @"\index.png";
                        //if (System.IO.File.Exists(OrderMainPath))
                        //{
                        //    using (var bmpTemp = new Bitmap(OrderMainPath))
                        //    {
                        //        pictureBoxPreview.Image = new Bitmap(bmpTemp);
                        //    }
                        //}
                        //else { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
                        if (System.IO.File.Exists(OrderMainPath))
                        {
                            byte[] img = System.IO.File.ReadAllBytes(OrderMainPath);
                            if (img != null && img.Length > 0)
                            {
                                try { pictureBoxPreview.Image = Utils.Converting.ByteToImage(img); }
                                catch { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
                            }
                        }
                        else { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
                    }
                    else { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
                    if (oldimg != null) { oldimg.Dispose(); }
                    #endregion
                    #region  Инфа в RTFBox
                    string TextOut = string.Empty;
                    string EquipmentName = string.Empty;
                    string MaterialName = string.Empty;
                    DataRow row = customDataGridView_Orders.SourceTable.Select("id = " + i).FirstOrDefault();
                    if (row != null)
                    {
                        string order_status = string.Empty;
                        if (SqlLite.Order.OrdersDic.ContainsKey(i))
                        {
                            switch (SqlLite.Order.OrdersDic[i].state)
                            {
                                case 0:
                                    order_status = "Ожидание";
                                    break;
                                case 1:
                                    order_status = "В работе";
                                    break;
                                case 2:
                                    order_status = "Постобработка";
                                    break;
                                case 3:
                                    order_status = "Готово";
                                    break;
                                case 4:
                                    order_status = "Закрыто";
                                    break;
                                case 5:
                                    order_status = "Остановлено";
                                    break;
                                case 6:
                                    order_status = "Корзина";
                                    break;
                                case 7:
                                    order_status = "Черновики";
                                    break;
                            }

                        }


                        groupBox_OrderInfo.Text = "Задание № " + Convert.ToString(row["id"]); // +"/" + Convert.ToString(row["N2"]);
                                                                                              //string MaterialName;
                                                                                              //string EquipmentName;
                                                                                              //string TextOut;
                        if (!order_status.Equals(string.Empty)) { groupBox_OrderInfo.Text = groupBox_OrderInfo.Text + " Статус: " + order_status; }
                        TextOut = @"{\rtf1 {\qc\loch\f6\b " + Convert.ToString(row["work_name"]) + @" \b0 от \b " + Convert.ToString(row["client"] + @" \b0 ");
                        TextOut += @"\par";

                        if (string.IsNullOrEmpty(Convert.ToString(row["comments"])))
                        {
                        }
                        else
                        {
                            TextOut += @"\par\ql " + Convert.ToString(row["comments"]).Replace("\r\n", @" \par ") + @" \par";
                        }


                        if (Convert.ToBoolean(row["print_on"]))
                        {
                            TextOut += @"\par\ql • Печать на \b ";
                            if (string.IsNullOrEmpty(Convert.ToString(row["printers_id"]))) { EquipmentName = "Удален из базы!"; } else { EquipmentName = GetEquipmentName(Convert.ToInt32(row["printers_id"]), "print"); }
                            if (string.IsNullOrEmpty(Convert.ToString(row["material_print_id"]))) { MaterialName = "Удален из базы!"; } else { MaterialName = GetMaterialName(Convert.ToInt32(row["material_print_id"]), "print"); }
                            TextOut += EquipmentName + @"\b0\par\tab материал: \b " + MaterialName + @"\b0 ";
                            TextOut += @"\par\tab Размер: \b " + Convert.ToString(row["size_x_print"]) + @"\b0 {  x }\b " + Convert.ToString(row["size_y_print"]) + @"\b0 { м.}";
                            TextOut += @"\par\tab Количество: \b " + Convert.ToString(row["count_print"]) + @" \b0 { шт.}";
                            TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(Convert.ToDouble(row["square_print"]), 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                            //if (Convert.ToBoolean(row["laminat"]))
                            //{
                            //    TextOut += @"\par\tab\b Ламинация \b0";
                            //}

                            TextOut += @"\par";

                        }

                        if (Convert.ToBoolean(row["cut_on"]))
                        {
                            TextOut += @"\par\ql • Резка на \b ";
                            if (string.IsNullOrEmpty(Convert.ToString(row["cutters_id"]))) { EquipmentName = "Удален из базы!"; } else { EquipmentName = GetEquipmentName(Convert.ToInt32(row["cutters_id"]), "cut"); }
                            if (string.IsNullOrEmpty(Convert.ToString(row["material_cut_id"]))) { MaterialName = "Удален из базы!"; } else { MaterialName = GetMaterialName(Convert.ToInt32(row["material_cut_id"]), "cut"); }
                            if (Convert.ToBoolean(row["cutting_on_print"])) { MaterialName = "Резка по меткам"; }
                            TextOut += EquipmentName + @"\b0\par\tab материал: \b " + MaterialName + @"\b0 ";
                            TextOut += @"\par\tab Размер: \b " + Convert.ToString(row["size_x_cut"]) + @"\b0 {  x }\b " + Convert.ToString(row["size_y_cut"]) + @"\b0 { м.}";
                            TextOut += @"\par\tab Количество: \b " + Convert.ToString(row["count_cut"]) + @" \b0 { шт.}";
                            TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(Convert.ToDouble(row["square_cut"]), 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                            //if (Convert.ToBoolean(row["laminat"]))
                            //{
                            //    TextOut += @"\par\tab\b Ламинация \b0";
                            //}

                            TextOut += @"\par";

                        }


                        if (Convert.ToBoolean(row["cnc_on"]))
                        {
                            TextOut += @"\par\ql • Фрезеровка на \b ";
                            if (string.IsNullOrEmpty(Convert.ToString(row["cncs_id"]))) { EquipmentName = "Удален из базы!"; } else { EquipmentName = GetEquipmentName(Convert.ToInt32(row["cncs_id"]), "cnc"); }
                            if (string.IsNullOrEmpty(Convert.ToString(row["material_cnc_id"]))) { MaterialName = "Удален из базы!"; } else { MaterialName = GetMaterialName(Convert.ToInt32(row["material_cnc_id"]), "cnc"); }
                            TextOut += EquipmentName + @"\b0\par\tab материал: \b " + MaterialName + @"\b0 ";
                            TextOut += @"\par\tab Размер: \b " + Convert.ToString(row["size_x_cnc"]) + @"\b0 {  x }\b " + Convert.ToString(row["size_y_cnc"]) + @"\b0 { м.}";
                            TextOut += @"\par\tab Количество: \b " + Convert.ToString(row["count_cnc"]) + @" \b0 { шт.}";
                            TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(Convert.ToDouble(row["square_cnc"]), 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                            //if (Convert.ToBoolean(row["laminat"]))
                            //{
                            //    TextOut += @"\par\tab\b Ламинация \b0";
                            //}

                            TextOut += @"\par";

                        }

                        string extrawork = string.Empty;
                        //
                        if (Convert.ToBoolean(row["baner_handling"]))
                        {
                            if (Convert.ToBoolean(row["baner_luvers"])) { extrawork += @"\par • Люверсы через " + Convert.ToDouble(row["baner_handling_size"]) + " см."; } else { extrawork += @"\par • Карманы " + Convert.ToDouble(row["baner_handling_size"]) + " см."; }
                        }

                        if (Convert.ToBoolean(row["installation"])) { extrawork += @"\par • " + "Монтажные работы"; }

                        string tmp = Convert.ToString(row["worktypes_list"]);
                        if (tmp != string.Empty && !CheckStandartWorkTypes(tmp))
                        {
                            string[] worktypes_list = tmp.Split((char)219);
                            string tmp2 = string.Empty;
                            foreach (string worktype in worktypes_list)
                            {
                                tmp2 = worktype.Replace(Convert.ToString((char)(219)), string.Empty);
                                if (tmp2 != string.Empty) { extrawork += @"\par • " + tmp2; }
                            }
                        }

                        if (Convert.ToBoolean(row["delivery"]))
                        {
                            if (Convert.ToBoolean(row["delivery_office"]))
                            { extrawork += @"\par • Доставка в офис"; }
                            else
                            {
                                if (Convert.ToString(row["delivery_address"]).Trim() == string.Empty) { extrawork += @"\par • Доставка"; } else { extrawork += @"\par • Доставка по адресу: " + Convert.ToString(row["delivery_address"]); }
                            }

                        }
                        if (extrawork != string.Empty) { TextOut += @"\par\tab\b Дополнительные работы: \b0" + extrawork; }
                        TextOut += @"\par}}";
                        richTextBoxEx1.Rtf = TextOut;
                    }
                    #endregion

                    #region история заявки
                    dataGridView_History.DataSource = null;

                    if (TableOrdersHistory != null)
                    {

                        TableOrdersHistory.DefaultView.RowFilter = "work_id = " + i;
                        TableOrdersHistory.DefaultView.Sort = "id DESC";
                        dataGridView_History.DataSource = TableOrdersHistory.DefaultView.ToTable();
                    }
                    #endregion

                }
                if (customDataGridView_Orders.SelectedRows != null)
                {
                    toolStripLabel7.Text = customDataGridView_Orders.SelectedRows.Count.ToString();
                }
            }
            else
            {
                Image oldimg = pictureBoxPreview.Image;
                pictureBoxPreview.Image = Properties.Resources.New_256x256;
                if (oldimg != null) { oldimg.Dispose(); }
                richTextBoxEx1.Text = string.Empty;
                groupBox_OrderInfo.Text = string.Empty;
                dataGridView_History.DataSource = null;
            }
        }

        private bool CheckStandartWorkTypes(string e)
        {
            switch (e)
            {
                case "Широкоформатная печать": return true;
                case "Плоттерная резка": return true;
                case "Плоттерная резка по меткам": return true;
                case "Фрезеровка": return true;
                case "Фрезеровка по меткам": return true;
            }
            return false;
        }

        #endregion
        #region Взять имя материала по id
        private string GetEquipmentName(int EquipmentId, string change)
        {
            string result = "Не найден";
            switch (change)
            {
                case "print":
                    SqlLite.Equip.DicPrinters.TryGetValue(EquipmentId, out result);
                    break;
                case "cut":
                    SqlLite.Equip.DicCuters.TryGetValue(EquipmentId, out result);
                    break;
                case "cnc":
                    SqlLite.Equip.DicCncs.TryGetValue(EquipmentId, out result);
                    break;
            }
            if (result.Equals(string.Empty)) { result = "Не найден"; }
            return result;
        }
        #endregion
        #region Взять имя материала по id
        private string GetMaterialName(int MaterialId, string change)
        {
            string result = "Не найден";
            switch (change)
            {
                case "print":
                    SqlLite.Materials.DicMaterialPrint.TryGetValue(MaterialId, out result);
                    break;
                case "cut":
                    SqlLite.Materials.DicMaterialCut.TryGetValue(MaterialId, out result);
                    break;
                case "cnc":
                    SqlLite.Materials.DicMaterialCnc.TryGetValue(MaterialId, out result);
                    break;
            }
            return result;
        }
        #endregion
        #region ДрагНДропы
        private Rectangle dragBoxFromMouseDown;
        private bool CopyDragNDropFlag = false;
        public Dictionary<long, string> ordersIDs = new Dictionary<long, string>();


        private void customDataGridView_Orders_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            int rowIndexFromMouseDown = dgv.HitTest(e.X, e.Y).RowIndex;

            if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control) { dgv.ClearSelection(); }
            if (rowIndexFromMouseDown != -1 && e.Button == MouseButtons.Right) { dgv.Rows[rowIndexFromMouseDown].Selected = true; }

            if (e.Button == MouseButtons.Right)
            {
                ordersIDs = new Dictionary<long, string>();
                if (dgv.SelectedRows.Count > 0)
                {
                    long i;
                    foreach (DataGridViewRow row in dgv.SelectedRows)
                    {
                        i = 0;
                        if (long.TryParse(row.Cells["id"].Value.ToString(), out i))
                        {
                            ordersIDs.Add(i, row.Cells["work_name"].Value.ToString());
                        }
                    }
                    context_Main_Order_Show(dgv, e.Location);
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {

                    if (rowIndexFromMouseDown != -1)
                    {
                        Size dragSize = SystemInformation.DragSize;
                        dragBoxFromMouseDown = new Rectangle(new System.Drawing.Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                    }
                    else
                    {
                        dragBoxFromMouseDown = Rectangle.Empty;
                    }
                    CopyDragNDropFlag = false;
                }
            }
        }


        #region Формирование и показ контекстного меню

        private void context_Main_Order_Show(Control ctrl, System.Drawing.Point loc)
        {
            ToolStripMenuItem_PrintON.Image = null;
            ToolStripMenuItem_CutON.Image = null;
            ToolStripMenuItem_CncON.Image = null;
            ToolStripMenuItem_InstallON.Image = null;
            switch (ordersIDs.Count)
            {
                case 0:
                    break;
                case 1:
                    contextMenuStrip_Open.Text = "Заявка № " + ordersIDs.ElementAt(0).Key.ToString();
                    contextMenuStrip_Open.ToolTipText = ordersIDs.ElementAt(0).Value;
                    contextMenuStrip_Open.Image = Properties.Resources.table;
                    ToolStripMenuItem_OrderSave.Text = "Сохранить все данные заявки";
                    //if (напечатаноToolStripMenuItem.Image != null)
                    //{
                    //    Image tmp = напечатаноToolStripMenuItem.Image;
                    //    напечатаноToolStripMenuItem.Image = null;
                    //    if (tmp!= null) { tmp.Dispose(); }
                    //}
                    //DataRow dr = customDataGridView_Orders.SourceTable.Select("id =" + ordersIDs.ElementAt(0).Key).SingleOrDefault();
                    int i = 0;
                    if (SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].print_on)
                    {
                        ToolStripMenuItem_PrintON.Visible = true;
                        ToolStripMenuItem_PrintON.Checked = SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].state_print;
                        if (ToolStripMenuItem_PrintON.Checked) { ToolStripMenuItem_PrintON.Text = "Снять статус готовности печати"; } else { ToolStripMenuItem_PrintON.Text = "Напечатано"; }
                    }
                    else { ToolStripMenuItem_PrintON.Visible = false; i++; }
                    if (SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].cut_on)
                    {
                        ToolStripMenuItem_CutON.Visible = true;
                        ToolStripMenuItem_CutON.Checked = SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].state_cut;
                        if (ToolStripMenuItem_CutON.Checked) { ToolStripMenuItem_CutON.Text = "Снять статус готовности резки"; } else { ToolStripMenuItem_CutON.Text = "Порезано"; }
                    }
                    else { ToolStripMenuItem_CutON.Visible = false; i++; }
                    if (SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].cnc_on)
                    {
                        ToolStripMenuItem_CncON.Visible = true;
                        ToolStripMenuItem_CncON.Checked = SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].state_cnc;
                        if (ToolStripMenuItem_CncON.Checked) { ToolStripMenuItem_CncON.Text = "Снять статус готовности фрезеровки"; } else { ToolStripMenuItem_CncON.Text = "Отфрезеровано"; }
                    }
                    else { ToolStripMenuItem_CncON.Visible = false; i++; }
                    if (SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].installation)
                    {
                        ToolStripMenuItem_InstallON.Visible = true;
                        ToolStripMenuItem_InstallON.Checked = SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].state_install;
                        if (ToolStripMenuItem_InstallON.Checked) { ToolStripMenuItem_InstallON.Text = "Снять статус готовности монтажа"; } else { ToolStripMenuItem_InstallON.Text = "Монтаж осуществлен"; }
                    }
                    else { ToolStripMenuItem_InstallON.Visible = false; i++; }

                    if (i == 4) { toolStripSeparator8.Visible = false; } else { toolStripSeparator8.Visible = true; }

                    DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(SqlLite.Order.OrdersDic[ordersIDs.ElementAt(0).Key].time_recieve);
                    if (tmpenddate != null)
                    {
                        string montage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + ordersIDs.ElementAt(0).Key.ToString("0000") + @"\montage.doc";
                        if (File.Exists(montage)) { ToolStripMenuItem_InstallPrint.Visible = true; } else { ToolStripMenuItem_InstallPrint.Visible = false; }
                    }
                    break;
                default:

                    contextMenuStrip_Open.Text = "Список заявок";
                    contextMenuStrip_Open.ToolTipText = "Список заявок";
                    contextMenuStrip_Open.Image = Properties.Resources.table_multiple;
                    ToolStripMenuItem_OrderSave.Text = "Сохранить все данные заявок";
                    //bool printed = false; 
                    List<bool> printed = new List<bool>();
                    List<bool> cutted = new List<bool>();
                    List<bool> cnced = new List<bool>();
                    List<bool> installed = new List<bool>();
                    bool montageDoc = false;
                    foreach (long ids in ordersIDs.Keys)
                    {
                        if (SqlLite.Order.OrdersDic[ids].print_on) { printed.Add(SqlLite.Order.OrdersDic[ids].state_print); }
                        if (SqlLite.Order.OrdersDic[ids].cut_on) { cutted.Add(SqlLite.Order.OrdersDic[ids].state_cut); }
                        if (SqlLite.Order.OrdersDic[ids].cnc_on) { cnced.Add(SqlLite.Order.OrdersDic[ids].state_cnc); }
                        if (SqlLite.Order.OrdersDic[ids].installation) { installed.Add(SqlLite.Order.OrdersDic[ids].state_install); }
                        if (!montageDoc)
                        {
                            DateTime tmpenddate2 = Utils.UnixDate.Int64ToDateTime(SqlLite.Order.OrdersDic[ids].time_recieve);
                            if (tmpenddate2 != null)
                            {
                                string montage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate2.ToString("yyyy.MM") + @"\" + ids.ToString("0000") + @"\montage.doc";
                                if (File.Exists(montage)) { ToolStripMenuItem_InstallPrint.Visible = true; montageDoc = true; } else { ToolStripMenuItem_InstallPrint.Visible = false; }
                            }
                        }
                    }

                    //int tmp1 = 0; int tmp2 = 0;
                    if (printed.Count > 0)
                    {
                        ToolStripMenuItem_PrintON.Visible = true;
                        if (printed.Count(v => v == true) == 0) { ToolStripMenuItem_PrintON.Text = "Напечатано"; ToolStripMenuItem_PrintON.Checked = false; }
                        else
                        {
                            if (printed.Count(v => v == false) == 0) { ToolStripMenuItem_PrintON.Text = "Снять статус готовности печати"; ToolStripMenuItem_PrintON.Checked = true; }
                            else
                            {
                                ToolStripMenuItem_PrintON.Image = Properties.Resources.buttonask;
                                ToolStripMenuItem_PrintON.Text = "Установить статус готовности печати";
                                ToolStripMenuItem_PrintON.Checked = false;
                            }
                        }
                    }
                    else { ToolStripMenuItem_PrintON.Visible = false; }
                    if (cutted.Count > 0)
                    {
                        ToolStripMenuItem_CutON.Visible = true;
                        if (cutted.Count(v => v == true) == 0) { ToolStripMenuItem_CutON.Text = "Порезано"; ToolStripMenuItem_CutON.Checked = false; }
                        else
                        {
                            if (cutted.Count(v => v == false) == 0) { ToolStripMenuItem_CutON.Text = "Снять статус готовности плот. резки"; ToolStripMenuItem_CutON.Checked = true; }
                            else
                            {
                                ToolStripMenuItem_CutON.Image = Properties.Resources.buttonask;
                                ToolStripMenuItem_CutON.Text = "Установить статус готовности плот. резки";
                                ToolStripMenuItem_CutON.Checked = false;
                            }
                        }
                    }
                    else { ToolStripMenuItem_CutON.Visible = false; }
                    if (cnced.Count > 0)
                    {
                        ToolStripMenuItem_CncON.Visible = true;
                        if (cnced.Count(v => v == true) == 0) { ToolStripMenuItem_CncON.Text = "Отфрезеровано"; ToolStripMenuItem_CncON.Checked = false; }
                        else
                        {
                            if (cnced.Count(v => v == false) == 0) { ToolStripMenuItem_CncON.Text = "Снять статус готовности фрезеровки"; ToolStripMenuItem_CncON.Checked = true; }
                            else
                            {
                                ToolStripMenuItem_CncON.Image = Properties.Resources.buttonask;
                                ToolStripMenuItem_CncON.Text = "Установить статус готовности фрезеровки";
                                ToolStripMenuItem_CncON.Checked = false;
                            }
                        }
                    }
                    else { ToolStripMenuItem_CncON.Visible = false; }
                    if (installed.Count > 0)
                    {
                        ToolStripMenuItem_InstallON.Visible = true;
                        if (installed.Count(v => v == true) == 0) { ToolStripMenuItem_InstallON.Text = "Монтаж произведен"; ToolStripMenuItem_InstallON.Checked = false; }
                        else
                        {
                            if (installed.Count(v => v == false) == 0) { ToolStripMenuItem_InstallON.Text = "Снять статус готовности монтажа"; ToolStripMenuItem_InstallON.Checked = true; }
                            else
                            {
                                ToolStripMenuItem_InstallON.Image = Properties.Resources.buttonask;
                                ToolStripMenuItem_InstallON.Text = "Установить статус готовности монтажа";
                                ToolStripMenuItem_InstallON.Checked = false;
                            }
                        }
                    }
                    else { ToolStripMenuItem_InstallON.Visible = false; }

                    break;
            }
            context_Main_Order_Button_Visible();
            //contextMenuStrip_Orders.Show(ctrl, loc);

            //switch (ctrl.Name)
            //{
            //    case "dataGridView_Customers":
            //        context_Main_treeview_Create.Text = "Создать нового заказчика";
            //        context_Main_treeview_Create.Enabled = true;
            //        context_Main_treeview_Create.Visible = true;
            //        context_Main_treeview_Create.Image = borg.Properties.Resources.vcard_add;
            //        context_Main_treeview_Edit_Row.Text = "Изменить заказчика";
            //        context_Main_treeview_Edit_Row.Enabled = true;
            //        context_Main_treeview_Edit_Row.Visible = true;
            //        context_Main_treeview_Edit_Row.Image = borg.Properties.Resources.vcard_edit;
            //        //context_Main_treeview_Copy_RowChild1.Text = "";
            //        //context_Main_treeview_Copy_RowChild1.Enabled = true;
            //        //context_Main_treeview_Copy_RowChild1.Image = borg.Properties.Resources.vcard_add;
            //        //context_Main_treeview_Cut_Row.Text = "";
            //        //context_Main_treeview_Cut_Row.Enabled = true;
            //        //context_Main_treeview_Cut_Row.Image = borg.Properties.Resources.vcard_add;
            //        //context_Main_treeview_Cut_Row.Text = "";
            //        //context_Main_treeview_Cut_Row.Enabled = true;
            //        //context_Main_treeview_Cut_Row.Image = borg.Properties.Resources.vcard_add;
            //        //context_Main_treeview_Cut_Row.Text = "";
            //        //context_Main_treeview_Cut_Row.Enabled = true;
            //        //context_Main_treeview_Cut_Row.Image = borg.Properties.Resources.vcard_add;

            //        context_Main_treeview.Show(ctrl, loc);
            //        break;
            //    case "customDataGridView_Person":
            //        context_Main_treeview_Create.Image = borg.Properties.Resources.user_add;
            //        context_Main_treeview_Create.Text = "Создать новое контактное лицо";
            //        context_Main_treeview_Create.Enabled = true;
            //        context_Main_treeview_Create.Visible = true;

            //        context_Main_treeview_Edit_Row.Image = borg.Properties.Resources.user_edit;
            //        context_Main_treeview_Edit_Row.Text = "Изменить контактное лицо";
            //        context_Main_treeview_Edit_Row.Enabled = true;
            //        context_Main_treeview_Edit_Row.Visible = true;

            //        context_Main_treeview.Show((CustomControls.customDataGridView)ctrl, loc);
            //        break;
            //    case "treeView_Customers":
            //        context_Main_treeview_Create.Image = borg.Properties.Resources.folder_add;
            //        context_Main_treeview_Create.Text = "Создать новый раздел";
            //        context_Main_treeview_Create.Enabled = true;
            //        context_Main_treeview_Create.Visible = true;

            //        context_Main_treeview_Edit_Row.Image = borg.Properties.Resources.folder_edit;
            //        context_Main_treeview_Edit_Row.Text = "Изменить раздел";
            //        context_Main_treeview_Edit_Row.Enabled = true;
            //        context_Main_treeview_Edit_Row.Visible = true;

            //        context_Main_treeview.Show((TreeView)ctrl, loc);
            //        break;
            //}
        }

        private void context_Main_Order_Button_Visible()
        {
            switch (orderMenuTreeView1.SelectedNode.Name)
            {
                case "nodeDraft":
                    ToolStripMenuItem_Await.Visible = true;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;


                    ToolStripMenuItem_DraftDouble.Visible = false;
                    ToolStripMenuItem_AwaiteDouble.Visible = false;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodeAwait":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = true;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = false;
                    ToolStripMenuItem_InworkDouble.Visible = false;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodeInWork":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = true;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = true;
                    ToolStripMenuItem_InworkDouble.Visible = false;
                    ToolStripMenuItem_PostProcDouble.Visible = false;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodePostProc":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = true;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = true;
                    ToolStripMenuItem_InworkDouble.Visible = false;
                    ToolStripMenuItem_StockDouble.Visible = false;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodeStock":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = true;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = true;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = false;
                    ToolStripMenuItem_ClosedDouble.Visible = false;
                    break;
                //------------------------
                case "nodeStopped":
                    ToolStripMenuItem_Await.Visible = true;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = false;
                    ToolStripMenuItem_Stoped.Visible = false;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = false;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                //-------------------------
                case "nodeBasket":
                    ToolStripMenuItem_Await.Visible = true;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = false;
                    ToolStripMenuItem_Stoped.Visible = false;
                    ToolStripMenuItem_Basket.Visible = false;
                    ToolStripMenuItem_Remove.Visible = true;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = false;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodeOpen":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = true;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;
                case "nodeAll":
                    ToolStripMenuItem_Await.Visible = false;
                    ToolStripMenuItem_InWork.Visible = false;
                    ToolStripMenuItem_PostProc.Visible = false;
                    ToolStripMenuItem_Stock.Visible = false;
                    ToolStripMenuItem_Closed.Visible = false;

                    toolStripSeparator_Stop.Visible = true;
                    ToolStripMenuItem_Stoped.Visible = true;
                    ToolStripMenuItem_Basket.Visible = true;
                    ToolStripMenuItem_Remove.Visible = false;

                    ToolStripMenuItem_DraftDouble.Visible = true;
                    ToolStripMenuItem_AwaiteDouble.Visible = true;
                    ToolStripMenuItem_InworkDouble.Visible = true;
                    ToolStripMenuItem_PostProcDouble.Visible = true;
                    ToolStripMenuItem_StockDouble.Visible = true;
                    ToolStripMenuItem_ClosedDouble.Visible = true;
                    break;

            }
        }


        #endregion

        #region ДрагНДроп
        private void orderMenuTreeView1_DragOver(object sender, DragEventArgs e)
        {
            System.Drawing.Point clientPoint = orderMenuTreeView1.PointToClient(new System.Drawing.Point(e.X, e.Y));
            //if (scrollingTimer)
            //{
            //    // See where the cursor is
            //    //Point pt = treeView_Customers.PointToClient(Cursor.Position);

            //    // See if we need to scroll up or down
            //    if ((clientPoint.Y + scrollRegion) > (treeView_Customers.Height - 5))
            //    {
            //        // Call the API to scroll down
            //        SendMessage(treeView_Customers.Handle, (int)277, (int)1, 0);
            //        scrollingTimer = false;
            //        timer_ForScrolling.Start();
            //    }
            //    else if ((clientPoint.Y + 10) < (treeView_Customers.Top + scrollRegion))
            //    {
            //        // Call thje API to scroll up
            //        SendMessage(treeView_Customers.Handle, (int)277, (int)0, 0);
            //        scrollingTimer = false;
            //        timer_ForScrolling.Start();
            //    }
            //}


            DragDropEffects effect = new DragDropEffects();
            if (CopyDragNDropFlag) { effect = DragDropEffects.Copy; } else { effect = DragDropEffects.Move; }

            TreeNode targetnode = orderMenuTreeView1.HitTest(clientPoint.X, clientPoint.Y).Node;
            if (targetnode != null) { orderMenuTreeView1.SelectedNode = targetnode; }
            int[] val = e.Data.GetData(typeof(int[])) as int[];
            if (targetnode != null && val.Length > 0 && orderMenuTreeView1.SelectedNode.Name != "nodeOpen" && orderMenuTreeView1.SelectedNode.Name != "nodeAll")
            {
                e.Effect = effect;
                //switch (val.type)
                //{
                //    case TableName.customersFolders:
                //        if (val.value.ContainsKey(Convert.ToInt32(treeView_Customers.SelectedNode.Name))) { e.Effect = DragDropEffects.None; }
                //        else
                //        {
                //            e.Effect = effect;
                //            if (CopyMoveFlag && targetnode.Name == "2")
                //            {

                //                e.Effect = DragDropEffects.None;
                //            }
                //            else
                //            {
                //                e.Effect = effect;
                //            }
                //        }

                //        break;
                //    case TableName.customers:
                //        e.Effect = effect;
                //        if (CopyMoveFlag && targetnode.Name == "2")
                //        {

                //            e.Effect = DragDropEffects.None;
                //        }
                //        else
                //        {
                //            e.Effect = effect;
                //        }
                //        break;
                //    default:
                //        e.Effect = DragDropEffects.None;
                //        break;
                //}
                ////blockTreeviewFilter = true;
                //if (val.type == TableName.customersFolders)
                //{
                //    if (val.value.ContainsKey(Convert.ToInt32(treeView_Customers.SelectedNode.Name))) { e.Effect = DragDropEffects.None; }
                //    else
                //    {
                //        if (CopyMoveFlag && targetnode.Name != "2")
                //        {

                //            e.Effect = DragDropEffects.Copy;
                //        }
                //        else { e.Effect = DragDropEffects.Move; }
                //    }
                //}
                //else { if (CopyMoveFlag && targetnode.Name != "2") { e.Effect = DragDropEffects.Copy; } else { e.Effect = DragDropEffects.Move; } }

            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
            //if (e.)
        }

        private void customDataGridView_Orders_DragOver(object sender, DragEventArgs e)
        {
            int[] ids = e.Data.GetData(typeof(int[])) as int[];
            if (ids.Length > 0 && orderMenuTreeView1.SelectedNode.Name != "nodeOpen" && orderMenuTreeView1.SelectedNode.Name != "nodeAll")
            {
                if (CopyDragNDropFlag)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
                //switch (val.type)
                //{
                //    //case TableName.customers:
                //    //    TreeNode targetnode = treeView_Customers.SelectedNode;
                //    //    if (targetnode != null) { e.Effect = DragDropEffects.Move; treeView_Customers.SelectedNode = targetnode; }
                //    //    else { e.Effect = DragDropEffects.None; }
                //    //    break;
                //    case TableName.customersContacts:
                //        Point pt = dgv.PointToClient(new Point(e.X, e.Y));
                //        DataGridView.HitTestInfo hit = dgv.HitTest(pt.X, pt.Y);
                //        if (hit.Type == DataGridViewHitTestType.Cell)
                //        {
                //            dgv.ClearSelection();
                //            dgv.Rows[hit.RowIndex].Selected = true;
                //            if (CopyMoveFlag) { e.Effect = DragDropEffects.Copy; } else { e.Effect = DragDropEffects.Move; }

                //        }
                //        else
                //        {
                //            e.Effect = DragDropEffects.None;
                //        }

                //        if (scrollingTimer)
                //        {
                //            if ((pt.Y + scrollRegion) > dgv.Height)
                //            {
                //                dgv.FirstDisplayedScrollingRowIndex = dgv.FirstDisplayedScrollingRowIndex + 1;
                //            }
                //            else if (pt.Y < (dgv.Top + scrollRegion))
                //            {
                //                dgv.FirstDisplayedScrollingRowIndex = dgv.FirstDisplayedScrollingRowIndex - 1;
                //            }
                //        }
                //        break;
                //    default:

                //break;
                //}


            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void customDataGridView_Orders_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
                    {
                        int[] DnDids = new int[customDataGridView_Orders.SelectedRows.Count];
                        int i = 0;
                        foreach (DataGridViewRow dr in customDataGridView_Orders.SelectedRows)
                        {
                            DnDids[i] = Convert.ToInt32(dr.Cells["id"].Value);
                            i++;
                        }
                        DragDropEffects dropEffect = customDataGridView_Orders.DoDragDrop(DnDids, DragDropEffects.All);
                    }
                }
            }
        }

        private void orderMenuTreeView1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.KeyState == 2 || e.KeyState == 3)
            {
                DragNDrop_MouseButtonUP = true;
            }
            else
            {
                if (DragNDrop_MouseButtonUP) { CopyDragNDropFlag = !CopyDragNDropFlag; DragNDrop_MouseButtonUP = false; }
            }
        }

        private bool DragNDrop_MouseButtonUP = false;
        private void customDataGridView_Orders_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.KeyState == 2 || e.KeyState == 3)
            {
                DragNDrop_MouseButtonUP = true;
            }
            else
            {
                if (DragNDrop_MouseButtonUP) { CopyDragNDropFlag = !CopyDragNDropFlag; DragNDrop_MouseButtonUP = false; }
            }
        }
        #endregion
        //TXTextControl.TextControl ctrltx = new TXTextControl.TextControl();

        public static GhostscriptVersionInfo _lastInstalledVersion = new GhostscriptVersionInfo(new System.Version(0, 0, 0), "gsdll32.dll", string.Empty, GhostscriptLicense.GPL);
        //public GhostscriptVersionInfo _lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion();
        private void button1_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения | *.pdf; |Все файлы (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(ofd.FileName))
                    {

                        byte[] openPDF = System.IO.File.ReadAllBytes(ofd.FileName);
                        int eofPos = -1;
                        for (int i = openPDF.Length - 1; i >= 0; i--)
                        {
                            if (openPDF[i] != 0)
                            {
                                eofPos = i;
                                break;
                            }
                        }
                        byte[] resultPDF = new byte[eofPos + 1];
                        Array.Copy(openPDF, resultPDF, eofPos + 1);

                        if (resultPDF != null && resultPDF.Length > 0)
                        {
                            try
                            {
                                GhostscriptRasterizer _rasterizer = new GhostscriptRasterizer();
                                _rasterizer.Open(new System.IO.MemoryStream(resultPDF.ToArray()), _lastInstalledVersion, true);
                                for (int pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
                                {
                                    Image img = _rasterizer.GetPage(360, 360, pageNumber);
                                    pictureBoxPreview.Image = img;
                                    MessageBox.Show("страница растеризована");
                                    //PrintingDocument.PrintPage += (obj, e) => { e.Graphics.DrawImage(bmp, rect); };
                                    //PrintingDocument.Print();
                                }

                                _rasterizer.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка растеризации заявки" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }












                        //using (org.pdfclown.files.File file = new org.pdfclown.files.File(ofd.FileName))
                        //{
                        //    // 2. Printing the document...
                        //    Renderer renderer = new Renderer();
                        //    bool silent = false;
                        //    if (renderer.Print(file.Document, silent))
                        //    { Console.WriteLine("Print fulfilled."); }
                        //    else
                        //    { Console.WriteLine("Print discarded."); }
                        //}

                        //byte[] bytes = System.IO.File.ReadAllBytes(ofd.FileName);
                        //int eofPos = -1;
                        //for (int i = bytes.Length - 1; i >= 0; i--)
                        //{
                        //    if (bytes[i] != 0)
                        //    {
                        //        eofPos = i;
                        //        break;
                        //    }
                        //}

                        //byte[] newBytes = new byte[eofPos + 1];
                        //Array.Copy(bytes, newBytes, eofPos + 1);

                        //MemoryStream ms = new MemoryStream(newBytes);
                        //////radPdfViewer1.LoadDocument(ms);

                        //using (var file = new org.pdfclown.bytes.Stream() .files.File(ms))
                        //{
                        //}

                        //using (org.pdfclown.files.File file = new org.pdfclown.files.File(newBytes))
                        //{
                        //    using (org.pdfclown.files.File file = new org.pdfclown.files.File(org.pdfclown.bytes.IInputStream ms))
                        //    {
                        //        org.pdfclown.documents.Document document = file.Document;
                        //    org.pdfclown.documents.Pages pages = document.Pages;

                        //    // 2. Page rasterization.
                        //    int pageIndex = 0;
                        //    org.pdfclown.documents.Page page = pages[pageIndex];
                        //    SizeF imageSize = page.Size;
                        //    Renderer renderer = new Renderer();
                        //    Image image = renderer.Render(page, imageSize);
                        //    pictureBoxPreview.Image = image;
                        //    // 3. Save the page image!
                        //    //image.Save(org.pdfclown.util.GetOutputPath("C:\\ContentRenderingSample.jpg"), ImageFormat.Jpeg);
                        //}

                        //textControl1.Load(TXTextControl.StreamType.AdobePDF);
                        //textControl1.Print(ofd.FileName);
                        //PdfSharp.Pdf.PdfDocument PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(ofd.FileName, PdfDocumentOpenMode.Import);
                        //PdfSharp.Pdf.PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();

                        //DocumentRenderer renderer = 


                        //PdfDocument doc =  new PdfDocument();
                        //doc.Pages.Add(new PdfPage());
                        //XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                        //XImage img = XImage.FromFile(source);
                        //xgr.DrawImage(img, 0, 0);
                        //doc.Save(destinaton); doc.Close();


                        //                        int page = this.pagePreview.Page;

                        //05.
                        //// Reuse the renderer from the preview
                        //06.
                        //DocumentRenderer renderer = this.pagePreview.Renderer;
                        //                        07.
                        //                        PageInfo info = renderer.FormattedDocument.GetPageInfo(page);
                        //                        08.


                        //09.
                        //// Create an image
                        //10.
                        //int dpi = 150;
                        //                        11.
                        //int dx, dy;
                        //                        12.
                        //if (info.Orientation == PdfSharp.PageOrientation.Portrait)
                        //                            13.
                        //{
                        //                            14.
                        //                            dx = (int)(info.Width.Inch * dpi);
                        //                            15.
                        //                            dy = (int)(info.Height.Inch * dpi);
                        //                            16.
                        //}
                        //                        17.
                        //else
                        //18.
                        //{
                        //                            19.
                        //                            dx = (int)(info.Height.Inch * dpi);
                        //                            20.
                        //                            dy = (int)(info.Width.Inch * dpi);
                        //                            21.
                        //}
                        //                        22.


                        //23.
                        //Image image = new Bitmap(dx, dy, PixelFormat.Format32bppRgb);
                        //                        24.


                        //25.
                        //// Create a Graphics object for the image and scale it for drawing with 72 dpi
                        //26.
                        //Graphics graphics = Graphics.FromImage(image);
                        //                        27.
                        //                        graphics.Clear(System.Drawing.Color.White);
                        //                        28.
                        //float scale = dpi / 72f;
                        //                        29.
                        //                        graphics.ScaleTransform(scale, scale);
                        //                        30.


                        //31.
                        //// Create an XGraphics object and render the page
                        //32.
                        //XGraphics gfx = XGraphics.FromGraphics(graphics, new XSize(info.Width.Point, info.Height.Point));
                        //                        33.
                        //                        renderer.RenderPage(gfx, page);
                        //                        34.
                        //                        gfx.Dispose();
                        //                        35.
                        //                        image.Save("test.png", ImageFormat.Png);

                        //                        MigraDoc.DocumentObjectModel.Document document = MigraDoc.DocumentObjectModel.IO.DdlReader.DocumentFromFile(ofd.FileName);
                        //                        string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                        //MigraDoc. pagePreview.Ddl = ddl;

                        //PdfDocument document = new PdfDocument();
                        //document.
                        //XGraphics gfx = XGraphics.f(page);

                        //PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding)

                        //                        XRect rect;
                        //                        XPen pen;
                        //double x = 50, y = 100;
                        //                        XFont fontH1 = new XFont("Times", 18, XFontStyle.Bold);
                        //                        XFont font = new XFont("Times", 12);
                        //                        XFont fontItalic = new XFont("Times", 12, XFontStyle.BoldItalic);
                        //                        double ls = font.GetHeight(gfx);
                        //// Draw some text
                        //                        gfx.DrawString("Create PDF on the fly with PDFsharp",fontH1, XBrushes.Black, x, x);
                        //                        gfx.DrawString("With PDFsharp you can use the same code to draw graphic, " +"text and images on different targets.", font, XBrushes.Black, x, y);
                        //                        y += ls;
                        //                        gfx.DrawString("The object used for drawing is the XGraphics object.",font, XBrushes.Black, x, y);
                        //                        y += 2 * ls;
                        //// Draw an arc
                        //                        pen = new XPen(XColors.Red, 4);
                        //                        pen.DashStyle = XDashStyle.Dash;
                        //                        gfx.DrawArc(pen, x + 20, y, 100, 60, 150, 120);
                        //// Draw a star
                        //                        XGraphicsState gs = gfx.Save();
                        //                        gfx.TranslateTransform(x + 140, y + 30);
                        //                        for (int idx = 0; idx < 360; idx += 10)
                        //{
                        //                            gfx.RotateTransform(10);
                        //                            gfx.DrawLine(XPens.DarkGreen, 0, 0, 30, 0);
                        //}
                        //                        gfx.Restore(gs);
                        //// Draw a rounded rectangle
                        //rect = new XRect(x + 230, y, 100, 60);
                        //                        pen = new XPen(XColors.DarkBlue, 2.5);
                        //                        XColor color1 = XColor.FromKnownColor(KnownColor.DarkBlue);
                        //                        XColor color2 = XColors.Red;
                        //                        XLinearGradientBrush lbrush = new XLinearGradientBrush(rect, color1, color2,
                        //                        XLinearGradientMode.Vertical);
                        //                        gfx.DrawRoundedRectangle(pen, lbrush, rect, new XSize(10, 10));
                        //// Draw a pie
                        //pen = new XPen(XColors.DarkOrange, 1.5);
                        //                        pen.DashStyle = XDashStyle.Dot;
                        //                        gfx.DrawPie(pen, XBrushes.Blue, x + 360, y, 100, 60, -130, 135);
                        //// Draw some more text
                        //y += 60 + 2 * ls;
                        //                        gfx.DrawString("With XGraphics you can draw on a PDF page as well as " +"on any System.Drawing.Graphics object.", font, XBrushes.Black, x, y);
                        //                        y += ls * 1.1;
                        //                        gfx.DrawString("Use the same code to", font, XBrushes.Black, x, y);
                        //                        x += 10;
                        //                        y += ls * 1.1;
                        //                        gfx.DrawString("• draw on a newly created PDF page", font, XBrushes.Black, x, y);
                        //                        y += ls;
                        //                        gfx.DrawString("• draw above or beneath of the content of an existing PDF page",
                        //                        font, XBrushes.Black, x, y);
                        //                        y += ls;
                        //                        gfx.DrawString("• draw in a window", font, XBrushes.Black, x, y);
                        //                        y += ls;
                        //                        gfx.DrawString("• draw on a printer", font, XBrushes.Black, x, y);
                        //                        y += ls;
                        //                        gfx.DrawString("• draw in a bitmap image", font, XBrushes.Black, x, y);
                        //                        x -= 10;
                        //                        y += ls * 1.1;
                        //                        gfx.DrawString("You can also import an existing PDF page and use it like " +"an image, e.g. draw it on another PDF page.", font, XBrushes.Black, x, y);
                        //                        y += ls * 1.1 * 2;
                        //                        gfx.DrawString("Imported PDF pages are neither drawn nor printed; create a " +"PDF file to see or print them!", fontItalic, XBrushes.Firebrick, x, y);
                        //                        y += ls * 1.1;
                        //                        gfx.DrawString("Below this text is a PDF form that will be visible when " +"viewed or printed with a PDF viewer.", fontItalic, XBrushes.Firebrick, x, y);
                        //                        y += ls * 1.1;
                        //                        XGraphicsState state = gfx.Save();
                        //                        XRect rcImage = new XRect(100, y, 100, 100 * Math.Sqrt(2));
                        //                        gfx.DrawRectangle(XBrushes.Snow, rcImage);
                        //                        gfx.DrawImage(XPdfForm.FromFile("../../../../../PDFs/SomeLayout.pdf"), rcImage);
                        //                        gfx.Restore(state);





                        //gfx.DrawImage(XPdfForm.FromFile("../../../../../PDFs/SomeLayout.pdf"), rcImage);
                        //string pdf_filename = ofd.FileName;
                        //string png_filename = "C:\\converted.png";
                        //string errrrr = string.Empty;
                        //List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(pdf_filename, png_filename);
                        //foreach(string str in errors)
                        //{
                        //    errrrr += str + Environment.NewLine;

                        //}
                        //MessageBox.Show(errrrr);




                        //Utils.TiffImage myTiff = new Utils.TiffImage(ofd.FileName);
                        //imageBox is a PictureBox control, and the [] operators pass back
                        //the Bitmap stored at that position in the myImages ArrayList in the TiffImage
                        //if (myTiff.myImages.Count > 0) { this.pictureBoxPreview.Image = (Bitmap)myTiff.myImages[0]; }
                        //try
                        //{
                        //    byte[] bytes = System.IO.File.ReadAllBytes(ofd.FileName);
                        //    int eofPos = -1;
                        //    for (int i = bytes.Length - 1; i >= 0; i--)
                        //    {
                        //        if (bytes[i] != 0)
                        //        {
                        //            eofPos = i;
                        //            break;
                        //        }
                        //    }

                        //    byte[] newBytes = new byte[eofPos + 1];
                        //    Array.Copy(bytes, newBytes, eofPos + 1);

                        //    MemoryStream ms = new MemoryStream(newBytes);
                        //    //File.WriteAllBytes(ofd.FileName + "2.pdf", newBytes);
                        //    ////radPdfViewer1.LoadDocument(ms);

                        //    using (org.pdfclown.bytes.IInputStream iis = new org.pdfclown.bytes.Stream(ms))//ofd.FileName + "2.pdf"))
                        //    {
                        //        using (org.pdfclown.files.File file = new org.pdfclown.files.File(iis))
                        //        {




                        //            org.pdfclown.documents.Document document = file.Document;
                        //            org.pdfclown.documents.Pages pages = document.Pages;

                        //            // 2. Page rasterization.
                        //            int pageIndex = 0;// PromptPageChoice("Select the page to render", pages.Count);
                        //            org.pdfclown.documents.Page page = pages[pageIndex];
                        //            SizeF imageSize = page.Size;
                        //            org.pdfclown.tools.Renderer renderer = new org.pdfclown.tools.Renderer();
                        //            Image image = renderer.Render(page, imageSize);
                        //            this.pictureBoxPreview.Image = image;
                        //            //image.Dispose();
                        //            // 3. Save the page image!
                        //            //image.Save(GetOutputPath("ContentRenderingSample.jpg"), ImageFormat.Jpeg);
                        //            //image.Save(path + ".jpg", ImageFormat.Jpeg);
                        //        }
                        //    }
                        //}
                        //catch (Exception ex){ MessageBox.Show(ex.Message); }
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {


            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0 && customDataGridView_Orders.SelectedRows[0].Cells["id"] != null && customDataGridView_Orders.SelectedRows[0].Cells["id"].Value != null)
            {
                long uid = Convert.ToInt64(customDataGridView_Orders.SelectedRows[0].Cells["id"].Value.ToString());
                List<byte[]> result = Utils.OrderRasterize.CreateImage_FromBase(uid);
                if (result != null && result.Count > 0)
                {
                    foreach (byte[] b in result)
                    {
                        pictureBoxPreview.Image = Utils.Converting.ByteToImage(b);
                        MessageBox.Show("Страница растеризована");
                    }
                }
            }






            //org.pdfclown.files.File file = new org.pdfclown.files.File();

            //Document document = file.Document;
            //document.PageSize = PageFormat.GetSize(PageFormat.SizeEnum.A4, PageFormat.OrientationEnum.Portrait);

            //Page page = new Page(document);
            //document.Pages.Add(page);

            //PrimitiveComposer composer = new PrimitiveComposer(page);


            ////draw a rectangle
            //composer.SetFillColor(org.pdfclown.documents.contents.colorSpaces.DeviceRGBColor.Get(System.Drawing.Color.LightSalmon));
            //composer.DrawRectangle(new RectangleF(30, 42, 300, 32));
            //composer.Fill();


            ////draw some text
            //composer.SetFillColor(org.pdfclown.documents.contents.colorSpaces.DeviceRGBColor.Get(System.Drawing.Color.Black));
            //composer.SetFont(new StandardType1Font(document, StandardType1Font.FamilyEnum.Helvetica, true, false), 32);
            //composer.ShowText("Hello World!", new PointF(32, 48));
            //composer.Flush();


            ////save the file
            ////file.Save(@"..\document.pdf", SerializationModeEnum.Standard);


            ////and print it
            //Renderer renderer = new Renderer();
            //// 2. Page rasterization.
            ////int pageIndex = 0;// PromptPageChoice("Select the page to render", pages.Count);
            ////org.pdfclown.documents.Page page = pages[pageIndex];
            //SizeF imageSize = page.Size;
            ////org.pdfclown.tools.Renderer renderer = new org.pdfclown.tools.Renderer();
            //Image image = renderer.Render(page, imageSize);
            //this.pictureBoxPreview.Image = image;
            ////renderer.Print(file.Document, false);
        }

        #endregion
        #region Обработка контекстного меню
        private void contextMenuStrip_Open_Click(object sender, EventArgs e)
        {
            switch (ordersIDs.Count)
            {
                case 0:
                    break;
                case 1:
                    EditOrder(ordersIDs.ElementAt(0).Key, 7);
                    break;
                default:
                    //using (FormContextOrderList frm = new FormContextOrderList(ordersIDs))
                    //{
                    //    frm.ShowDialog();
                    //    if (frm.result > 0) { EditOrder(frm.result, 7); }
                    //}
                    FormContextOrderList frm = new FormContextOrderList(ordersIDs, this);
                    frm.Show();
                    break;
            }

        }
        #endregion

        #region Новая заявка
        public void NewOrder_Click(object sender, EventArgs e)
        {
            DataTable historyOrders = TableOrdersHistory.Clone();
            Forms.FormBaseEdit frm = new Forms.FormBaseEdit(this, null, historyOrders, true, 7);
            //{
            //    ShowInTaskbar = false,
            //    //StartPosition = FormStartPosition.CenterParent,
            //    //Owner = this
            //};
            frm.Show(this);
        }
        #endregion

        #region Показать себя и все открытые заявки 
        private FormWindowState thisWindowsState = FormWindowState.Normal;
        public void ShowAllEditOrders()
        {
            //WindowState = thisWindowsState;
            ////this.TopMost = true;
            ////this.BringToFront();
            //Activate();

            ////this.TopMost = false;
            //foreach (FormBaseEdit frm in OpenedOrders.Values)
            //{
            //    frm.WindowState = FormWindowState.Normal;
            //    //frm.TopMost = true;
            //    //frm.BringToFront();
            //    frm.Activate();

            //    //frm.TopMost = false;
            //}
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112) // WM_SYSCOMMAND
            {
                //thisWindowsState = FormWindowState.Normal;
                // Check your window state here
                if (m.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
                {
                    thisWindowsState = FormWindowState.Maximized;
                }
                if (m.WParam == new IntPtr(0xF120))
                {
                    thisWindowsState = FormWindowState.Normal;
                }
                //if (m.WParam == new IntPtr(0XF020))
                //{
                //    thisWindowsState = FormWindowState.Normal;
                //}
                //Это должно обрабатывать событие в любом окне.SC_RESTORE - 0xF120, а SC_MINIMIZE -0XF020, если вам нужны эти константы.
            }
            base.WndProc(ref m);
        }

        #endregion



        private void FormBase_Activated(object sender, EventArgs e)
        {
            Program.SetWindowPos(Handle, Program.HWND_NOTOPMOST, 0, 0, 0, 0, Program.SWP_NOMOVE | Program.SWP_NOSIZE);
            //BringToFront();
            //this.TopMost = true;
            //this.Focus();
            //this.BringToFront();
            //this.TopMost = false;
        }


        #region GUI - Кнопки, меню и т.д.

        private void GUI_ShowPanels()
        {
            MenuView_ToolStrip.Checked = Utils.Settings.set.View_ToolStrip; ViewItem_CheckedChanged(MenuView_ToolStrip, new EventArgs()); MenuView_ToolStrip.CheckedChanged += ViewItem_CheckedChanged;
            //MenuView_QuickSearch.Checked = Utils.Settings.set.View_QuickSearch; ViewItem_CheckedChanged(MenuView_QuickSearch, new EventArgs()); MenuView_QuickSearch.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderListInfo.Checked = Utils.Settings.set.View_OrderListInfo; ViewItem_CheckedChanged(MenuView_OrderListInfo, new EventArgs()); MenuView_OrderListInfo.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderTimeStart.Checked = Utils.Settings.set.View_OrderTimeStart; ViewItem_CheckedChanged(MenuView_OrderTimeStart, new EventArgs()); MenuView_OrderTimeStart.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderTimeEnd.Checked = Utils.Settings.set.View_OrderTimeEnd; ViewItem_CheckedChanged(MenuView_OrderTimeEnd, new EventArgs()); MenuView_OrderTimeEnd.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderWorkTypes.Checked = Utils.Settings.set.View_OrderWorkTypes; ViewItem_CheckedChanged(MenuView_OrderWorkTypes, new EventArgs()); MenuView_OrderWorkTypes.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderMaterial.Checked = Utils.Settings.set.View_OrderMaterial; ViewItem_CheckedChanged(MenuView_OrderMaterial, new EventArgs()); MenuView_OrderMaterial.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderManager.Checked = Utils.Settings.set.View_OrderManager; ViewItem_CheckedChanged(MenuView_OrderManager, new EventArgs()); MenuView_OrderManager.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderCustomer.Checked = Utils.Settings.set.View_OrderCustomer; ViewItem_CheckedChanged(MenuView_OrderCustomer, new EventArgs()); MenuView_OrderCustomer.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_Worktypes.Checked = Utils.Settings.set.View_Worktypes; ViewItem_CheckedChanged(MenuView_Worktypes, new EventArgs()); MenuView_Worktypes.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderHistory.Checked = Utils.Settings.set.View_OrderHistory; ViewItem_CheckedChanged(MenuView_OrderHistory, new EventArgs()); MenuView_OrderHistory.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderPreview.Checked = Utils.Settings.set.View_OrderPreview; ViewItem_CheckedChanged(MenuView_OrderPreview, new EventArgs()); MenuView_OrderPreview.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_OrderNotes.Checked = Utils.Settings.set.View_OrderNotes; ViewItem_CheckedChanged(MenuView_OrderNotes, new EventArgs()); MenuView_OrderNotes.CheckedChanged += ViewItem_CheckedChanged;
            MenuView_SocketSend.Checked = Utils.Settings.set.View_SocketSend; ViewItem_CheckedChanged(MenuView_SocketSend, new EventArgs()); MenuView_SocketSend.CheckedChanged += ViewItem_CheckedChanged;
            Button_OrderViewAll.Checked = Utils.Settings.set.View_OrdersAll;
            Button_OrderViewPrint.Checked = Utils.Settings.set.View_OrderCut;
            Button_OrderViewCut.Checked = Utils.Settings.set.View_OrderCut;
            Button_OrderViewCnc.Checked = Utils.Settings.set.View_OrdersCnc;
            Button_OrderViewInstall.Checked = Utils.Settings.set.View_OrdersInstall;

            if (Utils.Settings.set.View_Maximize) { WindowState = FormWindowState.Maximized; } else { WindowState = FormWindowState.Normal; }

        }
        private void GUI_SavePanels()
        {
            Utils.Settings.set.View_ToolStrip = MenuView_ToolStrip.Checked;
            Utils.Settings.set.View_QuickSearch = MenuView_QuickSearch.Checked;
            Utils.Settings.set.View_OrderListInfo = MenuView_OrderListInfo.Checked;
            Utils.Settings.set.View_OrderTimeStart = MenuView_OrderTimeStart.Checked;
            Utils.Settings.set.View_OrderTimeEnd = MenuView_OrderTimeEnd.Checked;
            Utils.Settings.set.View_OrderWorkTypes = MenuView_OrderWorkTypes.Checked;
            Utils.Settings.set.View_OrderMaterial = MenuView_OrderMaterial.Checked;
            Utils.Settings.set.View_OrderManager = MenuView_OrderManager.Checked;
            Utils.Settings.set.View_OrderCustomer = MenuView_OrderCustomer.Checked;
            Utils.Settings.set.View_Worktypes = MenuView_Worktypes.Checked;
            Utils.Settings.set.View_OrderHistory = MenuView_OrderHistory.Checked;
            Utils.Settings.set.View_OrderPreview = MenuView_OrderPreview.Checked;
            Utils.Settings.set.View_OrderNotes = MenuView_OrderNotes.Checked;
            Utils.Settings.set.View_SocketSend = MenuView_SocketSend.Checked;
            Utils.Settings.set.View_OrdersAll = Button_OrderViewAll.Checked;
            Utils.Settings.set.View_OrderCut = Button_OrderViewPrint.Checked;
            Utils.Settings.set.View_OrderCut = Button_OrderViewCut.Checked;
            Utils.Settings.set.View_OrdersCnc = Button_OrderViewCnc.Checked;
            Utils.Settings.set.View_OrdersInstall = Button_OrderViewInstall.Checked;
            if (WindowState == FormWindowState.Maximized) { Utils.Settings.set.View_Maximize = true; } else { Utils.Settings.set.View_Maximize = false; }
            Utils.Settings.Save();
        }

        #region Показ/сокрытие - обработка меню View
        private void ViewItem_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem ctrl = sender as ToolStripMenuItem;
            switch (ctrl.Name)
            {
                case "MenuView_ToolStrip":
                    if (ctrl.Checked) { toolStrip_Main.Visible = true; } else { toolStrip_Main.Visible = false; }
                    break;
                case "MenuView_QuickSearch":
                    if (ctrl.Checked) { panel_QuickSearch.Visible = true; } else { panel_QuickSearch.Visible = false; } //textBox_QuickSearch.Visible = pictureBox_QuickSearch_Customers.Visible = 
                    break;
                case "MenuView_OrderListInfo":
                    if (ctrl.Checked) { toolStrip_OrderDatagridview.Visible = true; } else { toolStrip_OrderDatagridview.Visible = false; }
                    break;
                case "MenuView_OrderTimeStart":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["String_date_start"].Visible = true; } else { customDataGridView_Orders.Columns["String_date_start"].Visible = false; }
                    break;
                case "MenuView_OrderTimeEnd":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["Datetime_dead_line"].Visible = true; } else { customDataGridView_Orders.Columns["Datetime_dead_line"].Visible = false; }
                    break;
                case "MenuView_OrderWorkTypes":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["Image_WorkTypes"].Visible = true; } else { customDataGridView_Orders.Columns["Image_WorkTypes"].Visible = false; }
                    break;
                case "MenuView_OrderMaterial":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["Materials"].Visible = true; } else { customDataGridView_Orders.Columns["Materials"].Visible = false; }
                    break;
                case "MenuView_OrderManager":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["manager"].Visible = true; } else { customDataGridView_Orders.Columns["manager"].Visible = false; }
                    break;
                case "MenuView_OrderCustomer":
                    if (ctrl.Checked) { customDataGridView_Orders.Columns["client"].Visible = true; } else { customDataGridView_Orders.Columns["client"].Visible = false; }
                    break;
                case "MenuView_Worktypes":
                    if (ctrl.Checked) { toolStrip_Worktypes.Visible = true; } else { toolStrip_Worktypes.Visible = false; }
                    break;
                case "MenuView_OrderHistory":
                    if (ctrl.Checked)
                    {
                        panel_Treeview.Dock = DockStyle.Top;
                        panel_History.Visible = splitter_History.Visible = true;

                    }
                    else
                    {

                        panel_History.Visible = splitter_History.Visible = false;
                        panel_Treeview.Dock = DockStyle.Fill;
                    }
                    break;
                case "MenuView_OrderPreview":
                    if (ctrl.Checked)
                    {
                        panel_Right.Visible = splitter_Right.Visible = true;
                        panel_Preview.Visible = true;
                        if (MenuView_OrderNotes.Checked) { splitter_Preview.Visible = true; }

                    }
                    else
                    {
                        panel_Preview.Visible = splitter_Preview.Visible = false;
                        if (!MenuView_OrderNotes.Checked) { panel_Right.Visible = splitter_Right.Visible = false; }
                    }

                    break;
                case "MenuView_OrderNotes":
                    if (ctrl.Checked)
                    {
                        panel_Right.Visible = splitter_Right.Visible = true;
                        panel_Preview.Dock = DockStyle.Top;
                        panel_OrderAbout.Visible = true; ;
                        if (MenuView_OrderPreview.Checked) { splitter_Preview.Visible = true; }

                    }
                    else
                    {
                        panel_Preview.Dock = DockStyle.Fill;
                        panel_OrderAbout.Visible = splitter_Preview.Visible = false;
                        if (!MenuView_OrderPreview.Checked) { panel_Right.Visible = splitter_Right.Visible = false; }
                    }

                    break;
                case "MenuView_TreeViewDraft":
                    //if (ctrl.Checked)
                    //{ orderMenuTreeView1.Nodes[].Nodes["nodeDraft"].vi }
                    //else { }
                    break;
                case "MenuView_SocketSend":
                    if (ctrl.Checked)
                    { SocketSendShow = true; }
                    else { SocketSendShow = false; }
                    show_SocketSend();
                    break;



            }

        }
        #endregion



        #region меню - Сервис
        private void MenuService_DropDownOpening(object sender, EventArgs e)
        {
            if (SocketClient.TableClient.IsConnected) { MenuService_SocketServerTransmite.Text = "Прервать связь с сервером"; MenuService_SocketServerTransmite.Image = Properties.Resources.server_delete; } else { MenuService_SocketServerTransmite.Text = "Подключиться к серверу"; MenuService_SocketServerTransmite.Image = Properties.Resources.server_go; }
            if (Ftp.FTPClient.working) { MenuService_FtpTransmite.Text = "Остановить обмен файлов"; MenuService_FtpTransmite.Image = Properties.Resources.drive_delete; } else { MenuService_FtpTransmite.Text = "Возобновить обмен файлов"; MenuService_FtpTransmite.Image = Properties.Resources.drive_go; }
        }

        private void MenuService_SocketServerTransmite_Click(object sender, EventArgs e)
        {
            if (MenuService_SocketServerTransmite.Text.Equals("Прервать связь с сервером")) { SocketClient.TableClient.StopConnecting(); } else { SocketClient.TableClient.Connect(); }
        }

        private void MenuService_FtpTransmite_Click(object sender, EventArgs e)
        {
            if (MenuService_FtpTransmite.Text.Equals("Остановить обмен файлов")) { Ftp.FTPClient.Stop(); } else { Ftp.FTPClient.Start(); }
        }
        private void MenuService__Settings_Click(object sender, EventArgs e)
        {
            SocketClient.TableClient.StopConnecting();
            using (Forms.Settings.FormSettings fs = new Forms.Settings.FormSettings())
            {
                fs.ShowInTaskbar = true;
                fs.StartPosition = FormStartPosition.CenterScreen;
                fs.ShowDialog();
                SocketClient.TableClient.Connect();
            }
        }
        #endregion


        #region контекстное меню Treeview
        private int SelectedNodeOrderState = 7;
        private void orderMenuTreeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode treeNode = orderMenuTreeView1.GetNodeAt(e.X, e.Y);
            if (treeNode != null)
            {
                orderMenuTreeView1.SelectedNode = treeNode;
                if (e.Button == MouseButtons.Right)
                {
                    switch (treeNode.Name)
                    {
                        case "nodeDraft":
                            SelectedNodeOrderState = 7;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodeAwait":
                            SelectedNodeOrderState = 0;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodeInWork":
                            SelectedNodeOrderState = 1;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodePostProc":
                            SelectedNodeOrderState = 2;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodeStock":
                            SelectedNodeOrderState = 3;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodeStopped":
                            SelectedNodeOrderState = 5;
                            ToolStripMenuItem_ClearBasket.Visible = false;
                            Treeview_newOrder.Visible = true;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                        case "nodeBasket":
                            ToolStripMenuItem_ClearBasket.Visible = true;
                            Treeview_newOrder.Visible = false;
                            SelectedNodeOrderState = 6;
                            contextMenuStrip_treeview.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                    }

                }
            }

        }

        private void Treeview_newOrder_Click(object sender, EventArgs e)
        {
            EditOrder(0, SelectedNodeOrderState);
        }
        #endregion

        #endregion

        #region Печать заявок
        private void Button_OrderPrint_Click(object sender, EventArgs e)
        {

            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            {
                printDialog1.AllowSomePages = true;
                printDialog1.ShowHelp = true;
                printDialog1.Document = printDocument1;
                TopLevelControl.Focus();
                DialogResult result = printDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (DataGridViewRow dr in customDataGridView_Orders.SelectedRows)
                    {

                        if (dr.Cells["id"] != null && dr.Cells["id"].Value != null)
                        {
                            long uid = Convert.ToInt64(dr.Cells["id"].Value.ToString());
                            List<byte[]> imgs = Utils.OrderRasterize.CreateImage_FromBase(uid);
                            if (imgs != null && imgs.Count > 0)
                            {
                                foreach (byte[] b in imgs)
                                {
                                    printDocument1.PrintPage += (sendr, args) =>
                                    {
                                        Image i = Utils.Converting.ByteToImage(b);
                                        Rectangle m = args.PageBounds;

                                        if (i.Width / (double)i.Height > m.Width / (double)m.Height) // image is wider
                                        {
                                            m.Height = (int)(i.Height / (double)i.Width * m.Width);
                                        }
                                        else
                                        {
                                            m.Width = (int)(i.Width / (double)i.Height * m.Height);
                                        }
                                        //args.Graphics.DrawImage(i, m);
                                        ////
                                        //float newWidth = i.Width * 100 / 360;
                                        //float newHeight = i.Height * 100 / 360;

                                        float newWidth = (i.Width * 100) / 360;
                                        float newHeight = (i.Height * 100) / 360;

                                        float widthFactor = newWidth / args.PageBounds.Width; //(e.MarginBounds.Width+e.MarginBounds.X + e.MarginBounds.);
                                        float heightFactor = newHeight / args.PageBounds.Height; //(e.MarginBounds.Height+e.MarginBounds.Y);

                                        if (widthFactor > 1 | heightFactor > 1)
                                        {
                                            if (widthFactor > heightFactor)
                                            {
                                                newWidth = newWidth / widthFactor;
                                                newHeight = newHeight / widthFactor;
                                            }
                                            else
                                            {
                                                newWidth = newWidth / heightFactor;
                                                newHeight = newHeight / heightFactor;
                                            }
                                        }
                                        args.Graphics.DrawImage(i, 0, 0, (int)newWidth, (int)newHeight);
                                        i.Dispose();
                                    };
                                    printDocument1.Print();

                                }
                            }
                        }

                    }
                }
                //printDocument1.
                //    && customDataGridView_Orders.SelectedRows[0].Cells["id"] != null && customDataGridView_Orders.SelectedRows[0].Cells["id"].Value != null
                //long uid = Convert.ToInt64(customDataGridView_Orders.SelectedRows[0].Cells["id"].Value.ToString());
                //List<byte[]> result = Utils.OrderRasterize.CreateImage(uid);
                //if (result != null && result.Count > 0)
                //{
                //    foreach (byte[] b in result)
                //    {
                //        pictureBoxPreview.Image = Utils.Converting.ByteToImage(b);
                //        MessageBox.Show("Страница растеризована");
                //    }
                //}
            }
            //printDialog1.AllowSomePages = true;
            //printDialog1.ShowHelp = true;
            //printDialog1.Document = printDocument1;
            //DialogResult result = printDialog1.ShowDialog();
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    printDocument1.Print();
            //}
        }

        private void Button_OrderPdfSave_Click(object sender, EventArgs e)
        {
            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            {
                //printDialog1.AllowSomePages = true;
                //printDialog1.ShowHelp = true;
                //printDialog1.Document = printDocument1;
                //DialogResult result = printDialog1.ShowDialog();
                //if (result == System.Windows.Forms.DialogResult.OK)
                //{
                foreach (DataGridViewRow dr in customDataGridView_Orders.SelectedRows)
                {

                    if (dr.Cells["id"] != null && dr.Cells["id"].Value != null)
                    {
                        long uid = Convert.ToInt64(dr.Cells["id"].Value.ToString());
                        byte[] imgs = Utils.OrderRasterize.CreatePDF_FromBase(uid);
                        if (imgs != null && imgs.Length > 0)
                        {
                            SaveFileDialog sfdlg = new SaveFileDialog
                            {
                                FileName = "Заявка N" + uid.ToString() + "_" + Utils.FileNamesValidation.GetValidPath(dr.Cells["work_name"].Value.ToString()) + ".pdf",
                                AddExtension = true,
                                DefaultExt = "pdf",
                                Filter = "PDF-документ(*.pdf)|*.*",
                                RestoreDirectory = true
                            };
                            if (sfdlg.ShowDialog() == DialogResult.OK)
                            {
                                System.IO.File.WriteAllBytes(sfdlg.FileName, imgs.ToArray());
                            }
                            //    try
                            //    {
                            //        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                            //        {
                            //            fs.Write(imgs, 0, imgs.Length);
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {

                            //    }
                        }
                    }

                }
                //}
            }
        }

        private void dataGridView_SocketSend_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_SocketSend.ClearSelection();
        }

        private int dataGridView_SocketSendMaxHeight = 110;
        private void splitter_SocketSend_SplitterMoved(object sender, SplitterEventArgs e)
        {
            dataGridView_SocketSendMaxHeight = dataGridView_SocketSend.Height;
            Trace.WriteLine(dataGridView_SocketSend.Height.ToString());
        }


        #endregion
        private void MenuService_WinCalc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void Menu_OrderExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Menu_AppExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormBase_Shown(object sender, EventArgs e)
        {
            //BringToFront();
        }



        #region Изменение статусов заявок
        private void ToolStripMenuItem_StatesChange_Click(object sender, EventArgs e)
        {
            if (ordersIDs != null && ordersIDs.Count > 0)
            {
                ProtoClasses.ProtoOrdersChangeState.protoRowsList orderchangedlist = new ProtoClasses.ProtoOrdersChangeState.protoRowsList
                {
                    plist = new List<ProtoClasses.ProtoOrdersChangeState.protoRow>(),
                    adder = Utils.Settings.set.name,
                    sender_row_stringid = Utils.Settings.UniqueId
                };

                StringBuilder stringBuilder = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                if (ordersIDs.Count == 1)
                {
                    stringBuilder.Append("Изменения статуса заявки: ");
                }
                else { stringBuilder.Append("Изменения статуса заявок: "); }



                foreach (long i in ordersIDs.Keys)
                {
                    ProtoClasses.ProtoOrdersChangeState.protoRow row = new ProtoClasses.ProtoOrdersChangeState.protoRow
                    {
                        id = i,
                        state = SqlLite.Order.OrdersDic[i].state
                    };
                    if (SqlLite.Order.OrdersDic[i].print_on) { row.state_print = ToolStripMenuItem_PrintON.Checked; } else { row.state_print = SqlLite.Order.OrdersDic[i].state_print; }
                    if (SqlLite.Order.OrdersDic[i].cut_on) { row.state_cut = ToolStripMenuItem_CutON.Checked; } else { row.state_cut = SqlLite.Order.OrdersDic[i].state_cut; }
                    if (SqlLite.Order.OrdersDic[i].cnc_on) { row.state_cnc = ToolStripMenuItem_CncON.Checked; } else { row.state_cnc = SqlLite.Order.OrdersDic[i].state_cnc; }
                    if (SqlLite.Order.OrdersDic[i].installation) { row.state_install = ToolStripMenuItem_InstallON.Checked; } else { row.state_install = SqlLite.Order.OrdersDic[i].state_install; }
                    orderchangedlist.plist.Add(row);
                    stringBuilder.Append(i.ToString() + ", ");
                    if (stringBuilder2.Length > 0) { stringBuilder2.Append(Environment.NewLine); }
                    stringBuilder2.Append(SqlLite.Order.OrdersDic[i].work_name);
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
                orderchangedlist.name = stringBuilder2.ToString();

                Program.SendOrderToServer(new Program.OrderSendEventArgs(null, orderchangedlist, null, SocketClient.TableClient.SocketMessageCommand.OrderChangeStates, SocketClient.TableClient.TableName.TableBase, stringBuilder.ToString()));
            }
        }

        private void orderMenuTreeView1_DragDrop(object sender, DragEventArgs e)
        {
            //TreeNode targetnode = orderMenuTreeView1.HitTest(clientPoint.X, clientPoint.Y).Node;
            //if (targetnode != null) { orderMenuTreeView1.SelectedNode = targetnode; }
            //var r = orderMenuTreeView1.SelectedNode.;
            if (e.Effect == DragDropEffects.Move)
            {
                int[] idsOfOrder = e.Data.GetData(typeof(int[])) as int[];
                int targetStatus = 100;
                switch (orderMenuTreeView1.SelectedNode.Name)
                {
                    case "nodeDraft":
                        targetStatus = 7;
                        break;
                    case "nodeAwait":
                        targetStatus = 0;
                        break;
                    case "nodeInWork":
                        targetStatus = 1;
                        break;
                    case "nodePostProc":
                        targetStatus = 2;
                        break;
                    case "nodeStock":
                        targetStatus = 3;
                        break;
                    case "nodeStopped":
                        targetStatus = 5;
                        break;
                    case "nodeArchive":
                        targetStatus = 4;
                        break;
                    case "nodeBasket":
                        targetStatus = 6;
                        break;

                }
                if (targetStatus != 100)
                {

                    StringBuilder stringBuilder = new StringBuilder();
                    StringBuilder stringBuilder2 = new StringBuilder();
                    if (idsOfOrder.Length == 1)
                    {
                        stringBuilder.Append("Изменения статуса заявки: ");
                    }
                    else { stringBuilder.Append("Изменения статуса заявок: "); }


                    ProtoClasses.ProtoOrdersChangeState.protoRowsList orderchangedlist = new ProtoClasses.ProtoOrdersChangeState.protoRowsList
                    {
                        plist = new List<ProtoClasses.ProtoOrdersChangeState.protoRow>(),
                        adder = Utils.Settings.set.name,
                        sender_row_stringid = Utils.Settings.UniqueId
                    };
                    foreach (int i in idsOfOrder)
                    {
                        ProtoClasses.ProtoOrdersChangeState.protoRow row = new ProtoClasses.ProtoOrdersChangeState.protoRow
                        {
                            id = i,
                            state = targetStatus,
                            state_print = SqlLite.Order.OrdersDic[i].state_print,
                            state_cut = SqlLite.Order.OrdersDic[i].state_cut,
                            state_cnc = SqlLite.Order.OrdersDic[i].state_cnc,
                            state_install = SqlLite.Order.OrdersDic[i].state_install
                        };
                        orderchangedlist.plist.Add(row);
                        stringBuilder.Append(i.ToString() + ", ");
                        if (stringBuilder2.Length > 0) { stringBuilder2.Append(Environment.NewLine); }
                        stringBuilder2.Append(SqlLite.Order.OrdersDic[i].work_name);
                    }
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    orderchangedlist.name = stringBuilder2.ToString();

                    Program.SendOrderToServer(new Program.OrderSendEventArgs(null, orderchangedlist, null, SocketClient.TableClient.SocketMessageCommand.OrderChangeStates, SocketClient.TableClient.TableName.TableBase, stringBuilder.ToString()));
                }

            }
            //}
        }

        private void ToolStripMenuItem_Await_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem snd = sender as ToolStripMenuItem;
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();

            if (ordersIDs.Count == 1)
            {
                stringBuilder.Append("Изменения статуса заявки: ");
            }
            else { stringBuilder.Append("Изменения статуса заявок: "); }


            ProtoClasses.ProtoOrdersChangeState.protoRowsList orderchangedlist = new ProtoClasses.ProtoOrdersChangeState.protoRowsList
            {
                plist = new List<ProtoClasses.ProtoOrdersChangeState.protoRow>(),
                adder = Utils.Settings.set.name,
                sender_row_stringid = Utils.Settings.UniqueId
            };
            foreach (int i in ordersIDs.Keys)
            {
                ProtoClasses.ProtoOrdersChangeState.protoRow row = new ProtoClasses.ProtoOrdersChangeState.protoRow
                {
                    id = i,
                    state = Convert.ToInt16(snd.Tag),
                    state_print = SqlLite.Order.OrdersDic[i].state_print,
                    state_cut = SqlLite.Order.OrdersDic[i].state_cut,
                    state_cnc = SqlLite.Order.OrdersDic[i].state_cnc,
                    state_install = SqlLite.Order.OrdersDic[i].state_install
                };
                orderchangedlist.plist.Add(row);
                stringBuilder.Append(i.ToString() + ", ");
                if (stringBuilder2.Length > 0) { stringBuilder2.Append(Environment.NewLine); }
                stringBuilder2.Append(SqlLite.Order.OrdersDic[i].work_name);
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            orderchangedlist.name = stringBuilder2.ToString();

            Program.SendOrderToServer(new Program.OrderSendEventArgs(null, orderchangedlist, null, SocketClient.TableClient.SocketMessageCommand.OrderChangeStates, SocketClient.TableClient.TableName.TableBase, stringBuilder.ToString()));
        }
        #endregion
        //public class printMontageDoc
        //{
        //    public List<Int64> Ids { get; set; }
        //    public PrintDialog printDialog {get;set;}
        //}
        private void ToolStripMenuItem_InstallPrint_Click(object sender, EventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == DialogResult.OK)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (obj, er) => WorkerDoWork(ordersIDs.Keys.ToList(), myPrintDialog);
                worker.RunWorkerAsync();
            }
            //printMontageDoc montageDoc = new printMontageDoc();
            //montageDoc.Ids = ordersIDs.Keys.ToList();
            //montageDoc.printDialog = myPrintDialog;
            //System.ComponentModel.DoWorkEventArgs args = new System.ComponentModel.DoWorkEventArgs(new printMontageDoc(ordersIDs.Keys.ToList(), myPrintDialog));


        }

        private void WorkerDoWork(List<long> Ids, PrintDialog printDialog)
        {
            foreach (long i in ordersIDs.Keys)
            {
                DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(SqlLite.Order.OrdersDic[i].time_recieve);
                if (tmpenddate != null)
                {
                    string montage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString("0000") + @"\montage.doc";
                    if (File.Exists(montage))
                    {
                        try
                        {
                            using (TXTextControl.ServerTextControl control = new TXTextControl.ServerTextControl())
                            {
                                control.Create();
                                TXTextControl.LoadSettings ls = new TXTextControl.LoadSettings { ApplicationFieldFormat = TXTextControl.ApplicationFieldFormat.MSWord };
                                control.Load(montage, TXTextControl.StreamType.MSWord, ls);
                                PrintDocument myPrintDocument = new PrintDocument();
                                printDialog.Document = myPrintDocument;
                                printDialog.AllowSomePages = false;
                                printDialog.AllowPrintToFile = false;
                                printDialog.PrinterSettings.FromPage = 0;
                                printDialog.PrinterSettings.ToPage = control.Pages;
                                printDialog.UseEXDialog = true;
                                control.Print(myPrintDocument);
                            }
                        }
                        catch (Exception ex)
                        { MessageBox.Show("FormBase:WorkerDoWork" + Environment.NewLine + ex.Message, "Ошибка печати или открытия *.doc"); }
                    }
                }
            }
        }

        private void customDataGridView_Orders_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //if (Convert.ToBoolean(customDataGridView_Orders.Rows[e.RowIndex].Cells["be_read"].Value))
            //{
            //    customDataGridView_Orders.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.MidnightBlue;
            //    customDataGridView_Orders.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(customDataGridView_Orders.DefaultCellStyle.Font, FontStyle.Bold);
            //}
            //else
            //{
            //    customDataGridView_Orders.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            //    customDataGridView_Orders.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(customDataGridView_Orders.DefaultCellStyle.Font, FontStyle.Regular);
            //}
        }

        private void Button_OrderFilesDownload_Click(object sender, EventArgs e)
        {
            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (Utils.Settings.set.SavePath != string.Empty) { fbd.SelectedPath = Utils.Settings.set.SavePath; }
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Utils.Settings.set.SavePath = fbd.SelectedPath;
                        Utils.Settings.Save();
                        ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList sendingList = new ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList
                        {
                            sender_row_stringid = Utils.Settings.UniqueId,
                            plist = new List<ProtoClasses.ProtoDownloadOrdersFiles.protoRow>()
                        };

                        foreach (DataGridViewRow dr in customDataGridView_Orders.SelectedRows)
                        {
                            if (dr.Cells["id"] != null && dr.Cells["id"].Value != null)
                            {
                                long i = Convert.ToInt64(dr.Cells["id"].Value);
                                DirectoryInfo dirname = new DirectoryInfo(fbd.SelectedPath + @"\Заявка N" + i.ToString("000000") + "_" + Utils.FileNamesValidation.GetValidPath(SqlLite.Order.OrdersDic[i].work_name));
                                try
                                {
                                    if (!Directory.Exists(dirname.FullName)) { Directory.CreateDirectory(dirname.FullName); }
                                    byte[] imgs = Utils.OrderRasterize.CreatePDF_FromBase(i); if (imgs != null && imgs.Length > 0) { System.IO.File.WriteAllBytes(dirname.FullName + @"\" + dirname.Name + ".pdf", imgs.ToArray()); }
                                    DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customDataGridView_Orders.Rows[customDataGridView_Orders.CurrentCell.RowIndex].Cells["time_recieve"].Value));
                                    if (tmpenddate != null)
                                    {
                                        string sourcePreview = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString() + @"\index.png";
                                        if (System.IO.File.Exists(sourcePreview)) { File.Copy(sourcePreview, dirname + @"\preview.png", true); }
                                        string sourceMontage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString() + @"\montage.doc";
                                        if (System.IO.File.Exists(sourceMontage)) { File.Copy(sourceMontage, dirname + @"\montage.doc", true); }
                                    }
                                    ProtoClasses.ProtoDownloadOrdersFiles.protoRow pr = new ProtoClasses.ProtoDownloadOrdersFiles.protoRow
                                    {
                                        uid = i,
                                        LocalPath = dirname.FullName,
                                        Preview = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                                        Makets = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                                        Photoreport = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                                        Documents = new List<ProtoClasses.ProtoFtpSchedule.protoRow>()
                                    };
                                    sendingList.plist.Add(pr);

                                }
                                catch (Exception ex) { MessageBox.Show("FormBase:Button_OrderFilesDownload_Click" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                            }
                        }

                        //sendingList.sender_row_stringid = Utils.Settings.UniqueId;
                        switch (sendingList.plist.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                Program.SendOrderToServer(new Program.OrderSendEventArgs(null, null, sendingList, SocketClient.TableClient.SocketMessageCommand.DownloadOrderFiles, SocketClient.TableClient.TableName.TableBase, "Запрос на скачивание файлов заявки № " + sendingList.plist[0].uid.ToString()));
                                break;
                            default:
                                Program.SendOrderToServer(new Program.OrderSendEventArgs(null, null, sendingList, SocketClient.TableClient.SocketMessageCommand.DownloadOrderFiles, SocketClient.TableClient.TableName.TableBase, "Запрос на скачивание файлов заявок"));
                                break;
                        }
                    }
                }
            }
        }

        private void Menu_CheckUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Utils.Settings.versionInfo.updaterpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button_OrderEdit_Click(object sender, EventArgs e)
        {
            OpenOrders();
        }

        private void Button_OrderDelete_Click(object sender, EventArgs e)
        {
            if (customDataGridView_Orders.SelectedRows != null && customDataGridView_Orders.SelectedRows.Count > 0)
            {
                ProtoClasses.ProtoOrdersChangeState.protoRowsList orderchangedlist = new ProtoClasses.ProtoOrdersChangeState.protoRowsList
                {
                    plist = new List<ProtoClasses.ProtoOrdersChangeState.protoRow>(),
                    adder = Utils.Settings.set.name,
                    sender_row_stringid = Utils.Settings.UniqueId
                };

                switch (orderMenuTreeView1.SelectedNode.Name)
                {
                    case "nodeBasket":
                        DeleteOrders();
                        break;
                    default:
                        StringBuilder stringBuilder = new StringBuilder();
                        StringBuilder stringBuilder2 = new StringBuilder();

                        if (customDataGridView_Orders.SelectedRows.Count == 1)
                        {
                            stringBuilder.Append("Изменения статуса заявки: ");
                        }
                        else { stringBuilder.Append("Изменения статуса заявок: "); }

                        foreach (DataGridViewRow row in customDataGridView_Orders.SelectedRows)
                        {
                            if (row.Cells["id"] != null && row.Cells["id"].Value != null)
                            {
                                int i = Utils.CheckDBNull.ToInt32(row.Cells["id"].Value);
                                ProtoClasses.ProtoOrdersChangeState.protoRow prow = new ProtoClasses.ProtoOrdersChangeState.protoRow
                                {
                                    id = i,
                                    state = 6,
                                    state_print = SqlLite.Order.OrdersDic[i].state_print,
                                    state_cut = SqlLite.Order.OrdersDic[i].state_cut,
                                    state_cnc = SqlLite.Order.OrdersDic[i].state_cnc,
                                    state_install = SqlLite.Order.OrdersDic[i].state_install
                                };
                                orderchangedlist.plist.Add(prow);
                                stringBuilder.Append(i.ToString() + ", ");
                                if (stringBuilder2.Length > 0) { stringBuilder2.Append(Environment.NewLine); }
                                stringBuilder2.Append(SqlLite.Order.OrdersDic[i].work_name);
                            }
                        }
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                        orderchangedlist.name = stringBuilder2.ToString();

                        Program.SendOrderToServer(new Program.OrderSendEventArgs(null, orderchangedlist, null, SocketClient.TableClient.SocketMessageCommand.OrderChangeStates, SocketClient.TableClient.TableName.TableBase, stringBuilder.ToString()));
                        break;
                }

            }
        }

        private void RemoveOrders_Click(object sender, EventArgs e)
        {
            DeleteOrders();
        }

        private void DeleteOrders()
        {
            MessageBox.Show("Окончательное удаление заявок из корзины, навсегда, пока находится под вопросом", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteOrders();
        }

        private void QuickSearch_Click(object sender, EventArgs e)
        {
            textBox_QuickSearch.Focus();
        }


    }
}
