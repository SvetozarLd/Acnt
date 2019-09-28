using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
namespace AccentServer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SocketServer.Servers.server.Connect += Server_Connect;
        }

        internal delegate void ServerDelegate(SocketServer.TableServer.ConnectEventArgs e);
        private void Server_Connect(object sender, SocketServer.TableServer.ConnectEventArgs e)
        {
            try
            {
                if (e != null)
                {

                    if (InvokeRequired) { BeginInvoke(new ServerDelegate(ServerHandler), e); }
                }
                else { ServerHandler(e); }
            }
            catch (Exception ex)
            { }
        }

        //BindingList<ServerLogs> data = new BindingList<ServerLogs>();

        private void ServerHandler(SocketServer.TableServer.ConnectEventArgs e)
        {
            string socketstr = "Нет";
            if (e.Csocket != null)
            {
                try
                {
                    IPEndPoint remoteIpEndPoint = e.Csocket.RemoteEndPoint as IPEndPoint;
                    if (remoteIpEndPoint != null)
                    {
                        // Using the RemoteEndPoint property.
                        socketstr = "Remote:" + remoteIpEndPoint.Address + ":" + remoteIpEndPoint.Port;
                    }
                }
                catch { }

            }
            try
            {
                switch (e.Command)
                {
                    case SocketServer.TableServer.SocketMessageCommand.ServerStart:
                        //textBox1.Text += e.Comment + Environment.NewLine;
                        //dataGridView1.Rows.Add(DateTime.Now, socketstr, "Сервер работает");
                        ServerLogs.Add(new MyLogs(DateTime.Now, socketstr, "Сервер запущен"));
                        break;
                    case SocketServer.TableServer.SocketMessageCommand.ServerStop:
                        //textBox1.Text += e.Comment + Environment.NewLine;
                        //dataGridView1.Rows.Add(DateTime.Now, socketstr, "Сервер остановлен");
                        ServerLogs.Add(new MyLogs(DateTime.Now, socketstr, "Сервер остановлен"));
                        break;
                    case SocketServer.TableServer.SocketMessageCommand.ConnectOn:
                        //textBox1.Text += e.Comment + Environment.NewLine;
                        //dataGridView1.Rows.Add(DateTime.Now, socketstr, e.Comment);
                        ServerLogs.Add(new MyLogs(DateTime.Now, socketstr, e.Comment));
                        break;
                    case SocketServer.TableServer.SocketMessageCommand.ConnectLose:
                        //textBox1.Text += e.Comment + Environment.NewLine;
                        string msg = e.Comment;// "Отключился";
                        if (e.Ex != null) { msg = e.Ex.Message; }
                        //dataGridView1.Rows.Add(DateTime.Now, socketstr, msg);
                        ServerLogs.Add(new MyLogs(DateTime.Now, socketstr, msg));
                        break;
                    case SocketServer.TableServer.SocketMessageCommand.None:
                        //dataGridView1.Rows.Add(DateTime.Now, socketstr, e.Comment);
                        ServerLogs.Add(new MyLogs(DateTime.Now, socketstr, e.Comment));
                        break;
                }
                if (ServerLogs.Count > 100) { ServerLogs.RemoveAt(0); }
                source.ResetBindings(false);
                listBox1.Items.Clear();

                foreach (SocketServer.ClientInfo client in SocketServer.TableServer.clientList.Values)
                {
                    listBox1.Items.Add(client.StrName);
                }
            }
            catch { }
        }

        private List<MyLogs> ServerLogs = new List<MyLogs> { new MyLogs(DateTime.Now, "", "Начало работы") };
        private BindingSource source = new BindingSource();
        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
            source.DataSource = ServerLogs;
            dataGridView1.DataSource = source;

            toolStripTextBox1.Text = Properties.Settings.Default.FilePath;
            //Settings.Parameters.FilePath = toolStripTextBox1.Text;
            ServerLogs.Add(new MyLogs(DateTime.Now, "", "Составление списка файлов превью и монтажа"));
            source.ResetBindings(false);
            Utils.ReadPreview.LoadEvent += ReadPreview_LoadEvent;
            Utils.ReadPreview.ScanFiles();
            ServerLogs.Add(new MyLogs(DateTime.Now, "", "Загрузка таблиц"));
            source.ResetBindings(false);
            MySql.Orders.OrdersLoadEvent += Orders_OrdersLoadEvent;
            MySql.Orders.loadTable();
            MySql.Material.StartReadTables();
            MySql.Equip.StartReadTables();
            MySql.OrderHistory.StartReadTables();
            //MySql.Files.StartReadTables();



            //listView1.View = View.Details;
            //listView1.Columns.Add("Время");
            //listView1.Columns.Add("Сокет");
            //listView1.Columns.Add("Сообщение");

        }
        bool tablesloaded = false;
        bool filesloaded = false;
        internal delegate void FilesLoadDelegate(bool e);
        private void ReadPreview_LoadEvent(object sender, bool e)
        {
            if (InvokeRequired) { Invoke(new FilesLoadDelegate(scanfilesEventHandler), e); } else { scanfilesEventHandler(e); }
        }
        private void scanfilesEventHandler(bool e)
        {
            if (e)
            {
                filesloaded = true; ServerLogs.Add(new MyLogs(DateTime.Now, "", "Список файлов составлен")); source.ResetBindings(false);
                if (tablesloaded) { SocketServer.Servers.server.ServerStart(0, 0); }
            }
            Utils.ReadPreview.LoadEvent -= ReadPreview_LoadEvent;
        }

        internal delegate void OrdersLoadDelegate(bool e);
        private void Orders_OrdersLoadEvent(object sender, bool e)
        {
            if (InvokeRequired) { Invoke(new OrdersLoadDelegate(OrdersLoad), e); } else { OrdersLoad(e); }
        }
        private void OrdersLoad(bool e)
        {
            if (e)
            {
                tablesloaded = true; ServerLogs.Add(new MyLogs(DateTime.Now, "", "Все данные загружены")); source.ResetBindings(false);

                if (filesloaded) { SocketServer.Servers.server.ServerStart(0, 0); }
            }
            MySql.Orders.OrdersLoadEvent -= Orders_OrdersLoadEvent;
           
        }

        internal class MyLogs
        {
            public DateTime LogTime { get; set; }
            public string Socket { get; set; }
            public string Msg { get; set; }

            public MyLogs(DateTime logTime, string socket, string msg)
            {
                LogTime = logTime;
                Socket = socket;
                Msg = msg;
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SocketServer.Servers.server.ServerEnd();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SocketServer.Servers.server.ServerStart(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (SocketServer.ClientInfo client in SocketServer.TableServer.clientList.Values)
            {
                listBox1.Items.Add(client.StrName);
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    MySql.Files.Actualize();
        //}

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FilePath = toolStripTextBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
            {
                folderDlg.ShowNewFolderButton = true;
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    toolStripTextBox1.Text = folderDlg.SelectedPath;
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SocketServer.Servers.server.ServerEnd();
            Application.ExitThread();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
