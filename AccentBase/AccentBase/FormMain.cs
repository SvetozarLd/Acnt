using Microsoft.WindowsAPICodePack.Taskbar;
using System;
//using System.Windows.Shell;
//using TaskbarHook;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
namespace AccentBase
{


    public partial class FormMain : Form
    {


        //public System.Windows.Shell.TaskbarItemProgressState ProgressState { get; set; }
        //TaskbarItemInfo tii = new System.Windows.Shell.TaskbarItemInfo() { ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal };

        public FormMain()
        {
            InitializeComponent();
        }

        private long TryOfConnectToServer = 0;
        private bool ApplicationFirstStart = true;

        internal delegate void SocketRecieveDelegate(SocketClient.TableClient.RecieveEventArgs e);
        private void TableClient_SocketRecieveEvent(object sender, SocketClient.TableClient.RecieveEventArgs e)
        {
            if (e != null) { if (InvokeRequired) { Invoke(new SocketRecieveDelegate(ServerRecieveHandler), e); } else { ServerRecieveHandler(e); } }
        }

        //int iconViewDelay = 0;
        private bool showRecievedBytes = false;
        private void ServerRecieveHandler(SocketClient.TableClient.RecieveEventArgs e)
        {
            if (showRecievedBytes && e.AllByte > 0)
            {
                int i = (e.RecieveByte * 100) / e.AllByte;
                if (i >= 100) { i = 100; showRecievedBytes = false; }
                if (i > progressBar1.Value)
                {
                    progressBar1.Value = i;
                    TaskbarManager.Instance.SetProgressValue(i, 100, Handle);
                }


            }
            //iconViewDelay++;
            //if (iconViewDelay == 1)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
            //}
            //if (iconViewDelay == 2)
            //{
            //    iconViewDelay = 0;
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online2;
            //}
            //if (iconViewDelay >= 2)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online2;
            //}
            //else
            //{                
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;                
            //}
            //if (iconViewDelay >=4){ iconViewDelay = 0; }
            //if (iconViewDelay >= 50)
            //{
            //    iconViewDelay = 0;
            //if (notifyIcon1.Icon == Properties.Resources.icon_layout_online)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online2;
            //}
            //else
            //{
            //    if (notifyIcon1.Icon == Properties.Resources.icon_layout_online2)
            //    {
            //        notifyIcon1.Icon = Properties.Resources.icon_layout_online;
            //    }
            //}
            //}
            //if (SocketClient.TableClient.IsConnected)
            //{
            //    iconViewDelay++;
            //    if (iconViewDelay >= 100)
            //    {
            //        iconViewDelay = 0;
            //        if (notifyIcon1.Icon == Properties.Resources.icon_layout_online) { notifyIcon1.Icon = Properties.Resources.icon_layout_online2; }
            //        else
            //        {TableClient_SocketClientEvent
            //            if (notifyIcon1.Icon == Properties.Resources.icon_layout_online2) { notifyIcon1.Icon = Properties.Resources.icon_layout_online; }
            //        }
            //    }
            //}
            //Trace.WriteLine(e.AllByte.ToString() +":"+ e.RecieveByte.ToString()+":"+e.Complete.ToString());
            //if (e.Status)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
            //}
            //else
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_offline;
            //}
            ////textBox1.Text += e.Comment + Environment.NewLine;
        }

        private bool CheckUpdateNotStarted = true;
        private void CheckUpdate()
        {
            Utils.VersionInfo version = new Utils.VersionInfo();
            string localversion = version.checkLocalVersion();
            string serverversion = version.CheckServerVersion();
            if (!localversion.Equals(serverversion))
            {
                if (!serverversion.Equals("Ошибка соединения с сервером"))
                {
                    if (mainForm == null)
                    {
                        version.updater(serverversion);
                    }
                    else { version.updaterPermanent(serverversion); }
                }
            }
            CheckUpdateNotStarted = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CheckUpdateNotStarted && SocketClient.TableClient.StartedEventsEnded) { CheckUpdateNotStarted = false; CheckUpdate(); }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            timer1.Interval = 60 * 60 * 1000 * 3;
            //timer1.Interval = 10000;
            Utils.VersionInfo version = new Utils.VersionInfo();
            string localversion = version.checkLocalVersion();
            Utils.Settings.Load();
            Utils.Settings.UniqueId = Utils.UniqueId.ThumbPrint.Value();
            string serverversion = version.CheckServerVersion();
            if (localversion.Equals(serverversion))
            {
                label4.Text = "Ver.:" + localversion + ", обновления отсутствуют.";
            }
            else
            {
                if (serverversion.Equals("Ошибка соединения с сервером")) { label4.Text = "Ver.:" + localversion + ", ошибка соединения с сервером обновлений"; }
                else
                {
                    label4.Text = "Ver.:" + localversion + ", доступно обновление: " + serverversion;
                    version.updater(serverversion);
                }
            }

            //label4.Text = "Ver.:" + version.checkLocalVersion();

            //label4.Text = "Ver.:" + version.checkLocalVersion();




            SocketClient.TableClient.StartedEventsEnded = false;
            //MessageBox.Show(Utils.UniqueId.ThumbPrint.Value());
            //System.Windows.Window w = new System.Windows.Window();
            //w.TaskbarItemInfo = new System.Windows.Shell.TaskbarItemInfo() { ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal };
            //w.Loaded += delegate {
            //    Action<Object> callUpdateProgress = (o) => {
            //        w.TaskbarItemInfo.ProgressValue = (double)o;
            //    };

            //    Thread t = new Thread(() => {
            //        for (int i = 1; i <= 10; i++)
            //        {
            //            w.Dispatcher.BeginInvoke(callUpdateProgress, 1.0 * i / 10);
            //            Thread.Sleep(1000);
            //        }
            //    });
            //    t.Start();
            //};

            //System.Windows.Application app = new System.Windows.Application();
            //app.Run(w);











            //tii = new TaskbarItemInfo();
            //tii.ProgressState = TaskbarItemProgressState.Normal;
            //TaskbarItemInfo = new TaskbarItemInfo();
            //TaskbarItemInfo.ProgressState = new TaskbarItemProgressState();
            //RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            //try
            //{
            //    System.Windows.Application.Current.MainWindow.TaskbarItemInfo = new TaskbarItemInfo() ;
            //    System.Windows.Application.Current.MainWindow.TaskbarItemInfo = tii;
            //}
            //catch (Exception ex)
            //{

            //}
            //this.mainForm.

            //TaskbarManager.Instance.SetProgressValue(progressBar_Total.Value, progressBar_Total.Maximum, this.Handle);

            Program.OrderSendEvent += Program_OrderSendEvent;
            SocketClient.TableClient.SocketClientEvent += TableClient_SocketClientEvent;
            SocketClient.TableClient.SocketRecieveEvent += TableClient_SocketRecieveEvent;
            SocketClient.TableClient.IconEvent += TableClient_IconEvent;
            SqlLite.SqlEvent.UpdateEvent += SqlEvent_UpdateEvent;
            SocketClient.TableClient.PreviewlistComeEvent += TableClient_PreviewlistComeEvent;
            Ftp.FtpPreviewRecieve.PreviewlistComeEvent += FtpPreviewRecieve_PreviewlistComeEvent;
            Ftp.FtpPreviewRecieve.ComparePreviewStopped += FtpPreviewRecieve_ComparePreviewStopped;
            SqlLite.FtpSchedule.PreviewUpdate += FtpSchedule_PreviewUpdate;
            //StopFtpDaemon(null);

            //label4.Text = "Ver.: " + CurrentVersion;
            listBox1.Items.Add("Загрузка настроек... Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            Exception result = null;
            do
            {
                result = Utils.Settings.Load();
                if (result != null)
                {
                    using (Forms.Settings.FormSettings fs = new Forms.Settings.FormSettings())
                    {
                        fs.ShowInTaskbar = true;
                        fs.StartPosition = FormStartPosition.CenterScreen;
                        fs.ShowDialog();
                    }
                }
            } while (result != null);
            listBox1.Items[listBox1.Items.Count - 1] = "Загрузка настроек... Готово";
            listBox1.Items.Add("Открываем список не отправленных заявок.. Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            SqlLite.SendChangeToSocket.GetTable();
            listBox1.Items[listBox1.Items.Count - 1] = "Открываем список не отправленных заявок.. Готово";
            listBox1.Items.Add("Открываем заданий для FTP... Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            SqlLite.FtpSchedule.GetTable();
            listBox1.Items[listBox1.Items.Count - 1] = "Открываем заданий для FTP... Готово";
            listBox1.TopIndex = listBox1.Items.Count - 1;
            listBox1.Items.Add("Составляем список всех изображений предпросмотра... Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            Ftp.FtpPreviewRecieve.PreviewsScan();
            //listBox1.Items.Add("Открываем таблицы данных... Ожидайте");
            //listBox1.TopIndex = listBox1.Items.Count - 1;

            ////listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");

            ////listBox1.TopIndex = listBox1.Items.Count - 1;
            ////
            //SqlLite.Materials.loadTable();
            timer1.Start();
        }
        #region baloon tooltips
        #region Приход заявки с сервера
        internal delegate void OrderRecieveEventDelegate(ProtoClasses.ProtoOrders.protoOrder e);
        private void Program_OrderRecieveEvent(object sender, ProtoClasses.ProtoOrders.protoOrder e)
        {
            if (e != null) { if (InvokeRequired) { Invoke(new OrderRecieveEventDelegate(OrderRecieve), e); } else { OrderRecieve(e); } }
        }
        private void OrderRecieve(ProtoClasses.ProtoOrders.protoOrder e)
        {
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            if (e.work_name == string.Empty) { e.work_name = "Без названия"; }
            switch (e.command)
            {
                case (int)SocketClient.TableClient.SocketMessageCommand.RowsInsert:
                    notifyIcon1.BalloonTipTitle = e.adder + ": добавление заявки №" + e.id;
                    notifyIcon1.BalloonTipText = e.work_name;
                    notifyIcon1.ShowBalloonTip(3000);
                    notifyIcon1.Tag = e.id;
                    break;
                case (int)SocketClient.TableClient.SocketMessageCommand.RowsUpdate:
                    notifyIcon1.BalloonTipTitle = e.adder + ": изменение заявки №" + e.id;
                    notifyIcon1.BalloonTipText = e.work_name;
                    notifyIcon1.ShowBalloonTip(3000);
                    notifyIcon1.Tag = e.id;
                    break;
                case (int)SocketClient.TableClient.SocketMessageCommand.RowsDelete:

                    break;
                case (int)SocketClient.TableClient.SocketMessageCommand.OrderChangeStates:
                    notifyIcon1.BalloonTipTitle = e.adder + ": изменение статуса";
                    notifyIcon1.BalloonTipText = e.work_name;
                    notifyIcon1.ShowBalloonTip(3000);
                    notifyIcon1.Tag = e.id;
                    break;
                case (int)SocketClient.TableClient.SocketMessageCommand.DownloadOrderFiles:
                    notifyIcon1.BalloonTipTitle = "Пакетное скачивание файлов по FTP";
                    notifyIcon1.BalloonTipText = e.work_name;
                    notifyIcon1.ShowBalloonTip(3000);
                    notifyIcon1.Tag = e.id;
                    break;
            }

        }
        #endregion
        #region Отправка заявки на сервер
        internal delegate void OrderSendEventDelegate(Program.OrderSendEventArgs e);
        private void Program_OrderSendEvent(object sender, Program.OrderSendEventArgs e)
        {
            if (e != null)
            { if (InvokeRequired) { Invoke(new OrderSendEventDelegate(OrderToSend), e); } else { OrderToSend(e); }; }
        }
        private void OrderToSend(Program.OrderSendEventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Отправка на сервер: " + e.Notes;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            //if (e.Order.id > 0)
            //{
            if (e.Order == null)
            {
                if (e.OrdersChangeState != null) { notifyIcon1.BalloonTipText = e.OrdersChangeState.name; }
                else { notifyIcon1.BalloonTipText = e.Notes; }
            }
            else
            {
                if (e.Order.work_name != string.Empty)
                {
                    notifyIcon1.BalloonTipText = e.Order.work_name;
                }
                else
                {
                    notifyIcon1.BalloonTipText = "Без названия";
                }
            }

            //}
            //else
            //{
            //    notifyIcon1.BalloonTipText = e.Notes;
            //}


            notifyIcon1.ShowBalloonTip(3000);
            notifyIcon1.Tag = string.Empty;
        }
        #endregion
        #region кликнули по BalloonTip
        private void NotifyIcon1_BalloonTip_Clicked(object sender, EventArgs e)
        {
            string uidstr = notifyIcon1.Tag.ToString();
            long uidtmp = 0;
            if (notifyIcon1.Tag.ToString() != string.Empty)
            {

                if (long.TryParse(uidstr, out uidtmp))
                {
                    if (OrdersNotOpened) { ShowMainWindow(); }
                    Program.BalloonTipClicked(new Program.BalloonTipClickedEventArgs(uidtmp, SocketClient.TableClient.TableName.TableBase));
                }
            }
        }
        #endregion
        #endregion

        #region Работа с превью
        #region Составление списка превью
        internal delegate void PreviewScanDelegate(ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> e);
        private void FtpPreviewRecieve_PreviewlistComeEvent(object sender, ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> e)
        {
            if (e != null) { if (InvokeRequired) { Invoke(new PreviewScanDelegate(PreviewScanHandler), e); } else { PreviewScanHandler(e); } }
        }
        private void PreviewScanHandler(ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> e)
        {
            if (Ftp.FtpPreviewRecieve.preview != null) { Ftp.FtpPreviewRecieve.preview.Clear(); }
            Ftp.FtpPreviewRecieve.preview = e;
            ListLogDeleteRow();
            listBox1.Items[listBox1.Items.Count - 1] = "Составляем список всех изображений предпросмотра... Готово";
            listBox1.Items.Add("Открываем таблицы данных... Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            SqlLite.Materials.loadTable();
        }
        #endregion
        #region Событие - пришёл список превью
        internal delegate void PreviewlistComeDelegate(List<ProtoClasses.ProtoPreview.protoRow> e);
        private void TableClient_PreviewlistComeEvent(object sender, List<ProtoClasses.ProtoPreview.protoRow> e)
        {
            if (e != null)
            { if (InvokeRequired) { Invoke(new PreviewlistComeDelegate(PreviewlistHandler), e); } else { PreviewlistHandler(e); } }
        }
        private void PreviewlistHandler(List<ProtoClasses.ProtoPreview.protoRow> e)
        {
            ListLogDeleteRow();
            listBox1.Items.Add("Сравниваем коллекции изображений предпросмотра ... Ожидайте");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            Ftp.FtpPreviewRecieve.PreviewlistComparer(e);
        }
        #endregion
        #region Список превью сравнили, получен список ftp
        internal delegate void FtpScheduleDelegate(Ftp.FtpPreviewRecieve.PreviewlistComeEventArgs e);
        private void FtpPreviewRecieve_ComparePreviewStopped(object sender, Ftp.FtpPreviewRecieve.PreviewlistComeEventArgs e)
        {
            if (e != null)
            { if (InvokeRequired) { Invoke(new FtpScheduleDelegate(Recieveftppreviewlist), e); } else { Recieveftppreviewlist(e); } }
        }
        private void Recieveftppreviewlist(Ftp.FtpPreviewRecieve.PreviewlistComeEventArgs e)
        {
            if (e.Ended && !SocketClient.TableClient.StartedEventsEnded)
            {
                listBox1.Items[listBox1.Items.Count - 1] = "Сравниваем коллекции изображений предпросмотра... Готово";
                listBox1.TopIndex = listBox1.Items.Count - 1;
                progressBar1.Value = 100;
                TaskbarManager.Instance.SetProgressValue(100, 100, Handle);
                ListLogDeleteRow();
                listBox1.Items.Add("Добавляем отсутствующие файлы предпросмотра в список загрузки... Ожидайте");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                //progressBar1.Style = ProgressBarStyle.;
                //TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                progressBar1.Value = 0;
                TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                //List<ProtoClasses.ProtoFtpSchedule.protoRow> result = e.ResultList;
                //SqlLite.FtpSchedule.UpdatePreview(e.ResultList);
                backgroundWorker1.RunWorkerAsync(e.ResultList);

            }
            else
            {
                progressBar1.Value = e.ProcessCount;
                TaskbarManager.Instance.SetProgressValue(e.ProcessCount, 100, Handle);
            }
        }
        #endregion
        #region Вставка и её окончание в DataTable FTP и SQLITE FTP.
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<ProtoClasses.ProtoFtpSchedule.protoRow> reslt = e.Argument as List<ProtoClasses.ProtoFtpSchedule.protoRow>;
            SqlLite.FtpSchedule.UpdatePreview(reslt);
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Ftp.FTPClient.working && Ftp.FTPClient.DaemonMustWork) { Ftp.FTPClient.Start(); }
            progressBar1.Value = 0;
            TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            Context_Ftp.Enabled = true;
            Context_SocketSend.Enabled = true;
            Context_Orders.Enabled = true;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
            //showRecievedBytes = false;
            SocketClient.TableClient.StartedEventsEnded = true;
            listBox1.Items[listBox1.Items.Count - 1] = "Добавляем отсутствующие файлы предпросмотра в список загрузки... Готово";
            listBox1.TopIndex = listBox1.Items.Count - 1;
            ListLogDeleteRow();
            listBox1.Items.Add("<----------------Work Started------------------->");
            listBox1.TopIndex = listBox1.Items.Count - 1;

            if (ApplicationFirstStart) { ApplicationFirstStart = false; Program.OrderRecieveEvent += Program_OrderRecieveEvent; ShowMainWindow(); }
            SqlLite.SendChangeToSocket.SendAll();
        }
        #endregion
        #region Событие вставки/обновление превью в списке файлов для ftp
        internal delegate void PreviewFtpListUpdateDelegate(SqlLite.FtpSchedule.PreviewlistSQL e);
        private void FtpSchedule_PreviewUpdate(object sender, SqlLite.FtpSchedule.PreviewlistSQL e)
        {
            if (e != null)
            { if (InvokeRequired) { Invoke(new PreviewFtpListUpdateDelegate(PreviewFtpListUpdate), e); } else { PreviewFtpListUpdate(e); } }
        }
        private void PreviewFtpListUpdate(SqlLite.FtpSchedule.PreviewlistSQL e)
        {
            if (e.Ex != null)
            {

            }
            else
            {
                progressBar1.Value = e.ProcessCount;
                TaskbarManager.Instance.SetProgressValue(e.ProcessCount, 100, Handle);
            }
        }
        #endregion
        #region Команда серверу - прислать его превьюхи
        private void GetPreview()
        {
            string tmp = "Актуализация файлов превью";
            progressBar1.Value = 0;
            TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
            listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
            ListLogDeleteRow();
            listBox1.Items.Add(tmp);
            listBox1.TopIndex = listBox1.Items.Count - 1;
            showRecievedBytes = true;
            SocketClient.TableClient.SendToServer(new byte[4] { (int)SocketClient.TableClient.SocketMessageCommand.GetAllPreviewsList, 0, 0, 0 });
            //Ftp.FTPClient.Start();
        }

        #endregion
        #endregion

        //#region Получение версии сборки
        //public string CurrentVersion => ApplicationDeployment.IsNetworkDeployed
        //               ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
        //               : Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //#endregion

        #region Событие сервера
        internal delegate void SocketDelegate(SocketClient.TableClient.ConnectEventArgs e);
        private void TableClient_SocketClientEvent(object sender, SocketClient.TableClient.ConnectEventArgs e)
        {
            try
            {
                if (e != null) { if (InvokeRequired) { Invoke(new SocketDelegate(ServerHandler), e); } else { ServerHandler(e); } }
            }
            catch { }
        }
        private void ServerHandler(SocketClient.TableClient.ConnectEventArgs e)
        {
            //if (e.Status)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
            //}
            //else
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_offline;
            //}
            //if 
            //textBox1.Text += e.Comment + Environment.NewLine;
            //listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
            //if (listBox1.Items.Count > 2)
            //{
            //    string strtmp = listBox1.Items[listBox1.Items.Count - 1].ToString();
            //    string strtmp2 = listBox1.Items[listBox1.Items.Count - 2].ToString();
            //    string strtmp3 = listBox1.Items[listBox1.Items.Count - 3].ToString();
            //    if (strtmp.Equals("Событие подключения: Попытка подключения к серверу:") && e.Message.Equals("Потеря соединения с сервером") && strtmp.Equals("Событие подключения: Попытка подключения к серверу."))
            //    {


            //    }else
            //    {




            //    if (strtmp2.Equals("Событие подключения: Потеря соединения с сервером") && e.Message.Equals(""))
            //    {


            //    }
            //        //if (.Substring(0,50).Equals("Событие подключения: Попытка подключения к серверу") && listBox1.Items[listBox1.Items.Count - 2].Equals("Событие подключения: Потеря соединения с сервером"))
            //        //{

            //        //}else
            //        //{
            //        listBox1.Items.Add("Событие подключения: " + e.Message);
            //    listBox1.TopIndex = listBox1.Items.Count - 1;
            //        //}
            //    }
            //}
            //else
            //{
            //    listBox1.Items.Add("Событие подключения: " + e.Message);
            //    listBox1.TopIndex = listBox1.Items.Count - 1;
            //}
            if (e.Message == "") { return; }
            if (!e.Message.Equals("Потеря соединения с сервером"))
            {
                if (e.Message.Equals("К серверу подключились."))
                {
                    TryOfConnectToServer = 0;
                    ListLogDeleteRow();
                    listBox1.Items.Add("Событие подключения: " + e.Message);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    if (listBox1.Items.Count > 1)
                    {
                        string strtmp = listBox1.Items[listBox1.Items.Count - 1].ToString();
                        if (strtmp.Length > 50) { strtmp = strtmp.Substring(0, 50); }
                        if (strtmp.Equals("Событие подключения: Попытка подключения к серверу") && e.Message.Equals("Попытка подключения к серверу."))
                        {
                            listBox1.Items[listBox1.Items.Count - 1] = "Событие подключения: Попытка подключения к серверу..." + TryOfConnectToServer.ToString();
                            listBox1.TopIndex = listBox1.Items.Count - 1;

                            progressBar1.Style = ProgressBarStyle.Marquee;
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);




                        }
                        else
                        {
                            ListLogDeleteRow();
                            listBox1.Items.Add("Событие подключения: " + e.Message);
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                        }
                    }
                }
            }
            else
            {
                TryOfConnectToServer++;
            }
        }
        #endregion

        #region Событие сервера - иконки
        internal delegate void SocketIconDelegate(SocketClient.TableClient.iconStateEvent e);
        private void TableClient_IconEvent(object sender, SocketClient.TableClient.iconStateEvent e)
        {
            try
            {
                if (e != null) { if (InvokeRequired) { Invoke(new SocketIconDelegate(ServerIconHandler), e); } else { ServerIconHandler(e); } }
            }
            catch { }
        }
        private void ServerIconHandler(SocketClient.TableClient.iconStateEvent e)
        {
            switch (e.ico)
            {
                case SocketClient.TableClient.iconClient.Offline:
                    notifyIcon1.Icon = Properties.Resources.icon_layout_offline;
                    //if (SocketClient.TableClient. == false)( ConnectStart()
                    break;
                case SocketClient.TableClient.iconClient.NormalIcon:
                    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
                    break;
                case SocketClient.TableClient.iconClient.SendIcon:
                    notifyIcon1.Icon = Properties.Resources.icon_layout_online3;
                    break;
                case SocketClient.TableClient.iconClient.RecieveStart:
                    notifyIcon1.Icon = Properties.Resources.icon_layout_online2;
                    break;
                    //default:
                    //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
                    //    break;
            }
            //if (e.Status)
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_online;
            //}
            //else
            //{
            //    notifyIcon1.Icon = Properties.Resources.icon_layout_offline;
            //}
            //if 
            //textBox1.Text += e.Comment + Environment.NewLine;
        }
        #endregion

        #region событие sql
        internal delegate void SqliteDelegate(SqlLite.SqlEvent.SqliteUpdateEventArgs e);
        private void SqlEvent_UpdateEvent(object sender, SqlLite.SqlEvent.SqliteUpdateEventArgs e)
        {
            try
            {
                if (e != null) { if (InvokeRequired) { Invoke(new SqliteDelegate(SqliterHandler), e); } else { SqliterHandler(e); } }
            }
            catch (Exception ex)
            {

            }
        }
        private void SqliterHandler(SqlLite.SqlEvent.SqliteUpdateEventArgs e)
        {
            if (!SocketClient.TableClient.StartedEventsEnded)
            {
                string tmp = string.Empty;
                switch (e.sqliteEvent)
                {
                    case SqlLite.SqlEvent.SqLiteEvent.LoadTableStart:
                        //listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        ListLogDeleteRow();
                        listBox1.Items.Add("Загрузка: " + SqlLite.SqlEvent.EnumToName(e.tablename) + "... Ожидайте");
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                        progressBar1.Style = ProgressBarStyle.Marquee;
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.LoadTableEnd:
                        int i = listBox1.FindString("Загрузка: " + SqlLite.SqlEvent.EnumToName(e.tablename) + "... Ожидайте");
                        if (i >= 0)
                        {
                            listBox1.Items[i] = "Загрузка: " + SqlLite.SqlEvent.EnumToName(e.tablename) + "... Готово";
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                        }
                        switch (e.tablename)
                        {
                            case (int)SqlLite.SqlEvent.TableName.TableOrders:
                                int i1 = listBox1.FindString("Открываем таблицы данных... Ожидайте");
                                if (i >= 0)
                                {
                                    listBox1.Items[i1] = "Открываем таблицы данных... Готово";
                                }
                                ListLogDeleteRow();
                                listBox1.Items.Add("Соединение с сервером... Ожидайте");
                                listBox1.TopIndex = listBox1.Items.Count - 1;
                                SocketClient.TableClient.Connect();
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableMaterialCnc:
                                SqlLite.Equip.loadTable();
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableCncs:
                                SqlLite.OrderHistory.loadTable();
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableNoteStateChange:
                                SqlLite.Order.loadTable();
                                break;
                        }

                        //listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        //listBox1.Items.Add("Соединение с сервером... Ожидайте");
                        //listBox1.TopIndex = listBox1.Items.Count - 1;
                        //SocketClient.TableClient.Connect();
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.GetRowTimeCountStart:
                        //if (listBox1.Items[listBox1.Items.Count - 1].ToString() != label3.Text) { listBox1.Items.Add(label3.Text); listBox1.TopIndex = listBox1.Items.Count - 1; }

                        listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        ListLogDeleteRow();
                        listBox1.Items.Add(SqlLite.SqlEvent.EnumToName(e.tablename) + " - синхронизация. Формирую список для отправки на сервер... Ожидайте");
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                        //label3.Text = SqlLite.SqlEvent.EnumToName(e.tablename) + " - синхронизация. Формирую список для отправки на сервер";
                        //if (listBox1.Items[listBox1.Items.Count - 1].ToString() != label3.Text) { listBox1.Items.Add(label3.Text); listBox1.TopIndex = listBox1.Items.Count - 1; }
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Value = 0;
                        TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                        break;

                    case SqlLite.SqlEvent.SqLiteEvent.ServerWaitProcess:
                        //label3.Text = SqlLite.SqlEvent.EnumToName(e.tablename) + " - синхронизация. Сервер формирует список обновления... Ожидайте";
                        tmp = SqlLite.SqlEvent.EnumToName(e.tablename) + " - синхронизация. Сервер формирует список обновления... Ожидайте";
                        if (listBox1.Items[listBox1.Items.Count - 1].ToString() != tmp)
                        {
                            listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                            ListLogDeleteRow();
                            listBox1.Items.Add(tmp);
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                        }
                        if (e.procent >= 95)
                        {
                            e.procent = 100;
                        }
                        progressBar1.Value = e.procent;
                        //TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(e.procent, 100, Handle);
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.ServerEndProcess:
                        tmp = SqlLite.SqlEvent.EnumToName(e.tablename) + " - синхронизация. Получение данных от сервера... Ожидайте";
                        progressBar1.Value = 0;
                        TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                        listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        ListLogDeleteRow();
                        listBox1.Items.Add(tmp);
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                        showRecievedBytes = true;
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.UpdateRowsStart:
                        showRecievedBytes = false;
                        tmp = SqlLite.SqlEvent.EnumToName(e.tablename) + " - пакетное обновление записей... Ожидайте";
                        progressBar1.Value = 0;
                        listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        ListLogDeleteRow();
                        listBox1.Items.Add(tmp);
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                        progressBar1.Value = 0;
                        //TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.UpdateRowsProcess:
                        if (e.procent >= 100) { e.procent = 100; }
                        progressBar1.Value = e.procent;
                        //TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(e.procent, 100, Handle);
                        break;
                    case SqlLite.SqlEvent.SqLiteEvent.UpdateRowsEnd:
                        listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
                        progressBar1.Value = 0;
                        //TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                        TaskbarManager.Instance.SetProgressValue(0, 100, Handle);
                        switch (e.tablename)
                        {
                            case (int)SqlLite.SqlEvent.TableName.TableOrders:
                                GetPreview();
                                //this.WindowState = FormWindowState.Minimized;
                                //this.ShowInTaskbar = false;
                                //this.Hide();
                                //ShowMainWindow();

                                //if (mainForm == null && formsNotOpened)
                                //{
                                //    this.WindowState = FormWindowState.Minimized;
                                //    this.ShowInTaskbar = false;
                                //    this.Hide();
                                //    ShowMainWindow();
                                //}
                                //SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableMaterialPrint);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableMaterialPrint:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableMaterialCut);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableMaterialCut:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableMaterialCnc);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableMaterialCnc:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TablePrinters);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TablePrinters:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableCutters);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableCutters:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableCncs);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableCncs:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableNoteStateChange);
                                break;
                            case (int)SqlLite.SqlEvent.TableName.TableNoteStateChange:
                                SocketClient.TableClient.MassUpdateTables(SqlLite.SqlEvent.TableName.TableOrders);
                                break;
                        }



                        break;
                }
                //label3.Text = "Загрузка таблицы заявок... Ожидайте";
                //SqlLite.order.loadTable();
                //label3.Text = "Соединение с сервером... Ожидайте";
                //SocketClient.TableClient.Connect();
            }
        }
        #endregion



        #region Показ форм
        #region Показ настроек
        private void SettingsShow(object sender, EventArgs e)
        {
            SocketClient.TableClient.StopConnecting();
            using (Forms.Settings.FormSettings fs = new Forms.Settings.FormSettings())
            {
                fs.ShowInTaskbar = true;
                fs.StartPosition = FormStartPosition.CenterScreen;
                fs.ShowDialog();
                //if (SocketClient.TableClient.IsConnected)
                //{if (SocketClient.TableClient.StopConnecting()){ SocketClient.TableClient.Connect(); }}
                SocketClient.TableClient.Connect();
            }
        }
        #endregion

        #endregion

        #region Показать главное окно (заявки)
        private Forms.FormBase mainForm = null;
        private bool OrdersNotOpened = true;
        private void ShowMainWindow()
        {
            try
            {
                OrdersNotOpened = false;
                Context_Orders.Checked = true;
                if (mainForm == null)
                {
                    mainForm = new Forms.FormBase
                    {
                        ShowInTaskbar = true,
                        StartPosition = FormStartPosition.CenterScreen,
                        Owner = this
                    };
                    mainForm.FormClosed += MainForm_FormClosed;
                    //mainForm.BringToFront();
                    //mainForm.TopMost = true;
                    mainForm.Show(this);
                    //mainForm.TopMost = false;
                    //mainForm.BringToFront();
                    //mainForm.TopMost = true;
                    //mainForm.ShowAllEditOrders();
                }
                else
                {
                    //mainForm.TopMost = true;
                    mainForm.Activate();
                    mainForm.ShowAllEditOrders();
                }
            }
            catch { }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.FormClosed -= MainForm_FormClosed; mainForm = null; Context_Orders.Checked = false; OrdersNotOpened = true;
        }
        #endregion

        #region Показать FTP
        private Forms.FormUpDownload FTPform = null;
        private bool FTPformNotOpened = true;
        private void ShowFTPWindow()
        {
            FTPformNotOpened = false;
            Context_Ftp.Checked = true;
            if (FTPform == null)
            {
                FTPform = new Forms.FormUpDownload
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                FTPform.FormClosed += FTPForm_FormClosed;
                FTPform.Show(this);
            }
            else
            {
                FTPform.Activate();
            }
        }

        private void FTPForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FTPform.FormClosed -= FTPForm_FormClosed; FTPform = null; Context_Ftp.Checked = false; FTPformNotOpened = true;
        }
        #endregion

        #region Показать Messenger
        private Forms.FormMessenger Messengerform = null;
        private bool MessengerformNotOpened = true;
        private void ShowMessengerWindow()
        {
            MessengerformNotOpened = false;
            Context_Messenger.Checked = true;
            if (Messengerform == null)
            {
                Messengerform = new Forms.FormMessenger
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                Messengerform.FormClosed += MessengerForm_FormClosed;
                Messengerform.Show(this);
            }
            else
            {
                Messengerform.Activate();
            }
        }

        private void MessengerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Messengerform.FormClosed -= MessengerForm_FormClosed; Messengerform = null; Context_Messenger.Checked = false; MessengerformNotOpened = true;
        }
        #endregion

        #region Показать старый склад

        #region Форма 
        private Forms.Stock.FormOldStock OldStock = null;
        #endregion


        private void ShowOldStockWindow()
        {
            if (OldStock == null)
            {
                OldStock = new Forms.Stock.FormOldStock
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                OldStock.FormClosed += OldStock_FormClosed;
                OldStock.Show(this);
            }
            else
            {
                //if (Form.ActiveForm == OldStock)
                //{
                //    OldStock.FormClosed -= OldStock_FormClosed;
                //    OldStock.Close();
                //    OldStock = null;
                //}else
                //{
                OldStock.Activate();
                //}
            }
        }

        private void OldStock_FormClosed(object sender, FormClosedEventArgs e)
        {
            OldStock.FormClosed -= OldStock_FormClosed; OldStock = null;
        }
        #endregion

        #region Показать новый  склад

        #region Форма
        private Forms.Stock.FormStock Stock = null;
        #endregion


        private void ShowStockWindow()
        {
            if (Stock == null)
            {
                Stock = new Forms.Stock.FormStock
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                Stock.FormClosed += Stock_FormClosed;
                Stock.Show(this);
            }
            else
            {
                //if (Form.ActiveForm == OldStock)
                //{
                //    OldStock.FormClosed -= OldStock_FormClosed;
                //    OldStock.Close();
                //    OldStock = null;
                //}else
                //{
                Stock.Activate();
                //}
            }
        }

        private void Stock_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stock.FormClosed -= Stock_FormClosed; Stock = null;
        }
        #endregion

        #region Показать Google - таблицы
        private Forms.Stock.FormGoogleStock formGoogleStock = null;
        private bool GoogleStockformNotOpened = true;
        private void ShowformGoogleStock()
        {
            GoogleStockformNotOpened = false;
            Context_GoogleTables.Checked = true;
            if (formGoogleStock == null)
            {
                formGoogleStock = new Forms.Stock.FormGoogleStock
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                formGoogleStock.FormClosed += formGoogleStock_FormClosed;
                formGoogleStock.Show(this);
            }
            else
            {
                FTPform.Activate();
            }
        }

        private void formGoogleStock_FormClosed(object sender, FormClosedEventArgs e)
        {
            formGoogleStock.FormClosed -= formGoogleStock_FormClosed; formGoogleStock = null; Context_GoogleTables.Checked = false; GoogleStockformNotOpened = true;
        }
        #endregion
        #region Показать Список отправки на сокет
        private Forms.FormSendToServer formSendToServer = null;
        private bool formSendToServerNotOpened = true;
        private void ShowformGSocketSend()
        {
            formSendToServerNotOpened = false;
            Context_SocketSend.Checked = true;
            if (formSendToServer == null)
            {
                formSendToServer = new Forms.FormSendToServer
                {
                    ShowInTaskbar = true,
                    StartPosition = FormStartPosition.CenterScreen,
                    Owner = this
                };
                formSendToServer.FormClosed += formSendToServer_FormClosed;
                formSendToServer.Show(this);
            }
            else
            {
                formSendToServer.Activate();
            }
        }

        private void formSendToServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            formSendToServer.FormClosed -= formSendToServer_FormClosed; formSendToServer = null; Context_SocketSend.Checked = false; formSendToServerNotOpened = true;
        }
        #endregion

        #region Кнопки контекстного меню
        #region Показать заявки
        private void ShowOrders_Click(object sender, EventArgs e)
        {
            if (OrdersNotOpened) { ShowMainWindow(); } else { if (mainForm != null) { mainForm.Close(); } }
        }
        #endregion
        #region Показать/скрыть список отправки на сервер
        private void Context_SocketSend_Click(object sender, EventArgs e)
        {
            if (formSendToServerNotOpened) { ShowformGSocketSend(); } else { if (formSendToServer != null) { formSendToServer.Close(); } }
        }
        #endregion
        #region Показать мессенджер
        private void Context_Messenger_Click(object sender, EventArgs e)
        {
            if (MessengerformNotOpened) { ShowMessengerWindow(); } else { if (Messengerform != null) { Messengerform.Close(); } }
        }
        #endregion
        #region Показать фтп-клиент
        private void Context_Ftp_Click(object sender, EventArgs e)
        {
            if (FTPformNotOpened) { ShowFTPWindow(); } else { if (FTPform != null) { FTPform.Close(); } }
        }
        #endregion
        #region Показать/скрыть гугл таблицы
        private void contextGoogleTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GoogleStockformNotOpened) { ShowformGoogleStock(); } else { if (formGoogleStock != null) { formGoogleStock.Close(); } }
        }
        #endregion
        #region Выход из программы
        private void ApplicationExit_Click(object sender, EventArgs e)
        {
            if (mainForm != null && mainForm.OpenedOrders != null && mainForm.OpenedOrders.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "При выходе из приложения все изменения будут потеряны." + Environment.NewLine + "Вы действительно хотите выйти?",
                    "Внимание, остались открытые заявки.",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    Ftp.FTPClient.Stop();
                    SocketClient.TableClient.StopConnecting();
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    mainForm.ShowAllEditOrders();
                }
            }
            else
            {
                Ftp.FTPClient.Stop();
                SocketClient.TableClient.StopConnecting();
                System.Windows.Forms.Application.Exit();
            }

        }
        #endregion

        #region Показать старый склад
        private void ShowOldStock_Click(object sender, EventArgs e)
        {
            ShowOldStockWindow();
        }
        #endregion
        #region Показать новый склад
        private void ShowStock_Click(object sender, EventArgs e)
        {
            ShowStockWindow();
        }
        #endregion

        #region About
        private void Context_About_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region Удаление первой строчки списка лога если счётчик 100
        private void ListLogDeleteRow()
        {
            if (listBox1.Items.Count > 100)
            {
                listBox1.Items.RemoveAt(0);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }




        #endregion
    }
}
