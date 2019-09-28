using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace AccentBase
{
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            checkRunningProgram();
            Ghostscript ghostscript = new Ghostscript();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #region Загрузка картинок в byte
            PrinterPic = Utils.Converting.ImageToByte(AccentBase.Properties.Resources.printer_epson_big2);
            CutterPic = Utils.Converting.ImageToByte(AccentBase.Properties.Resources.cutter_big2);
            CncPic = Utils.Converting.ImageToByte(AccentBase.Properties.Resources.cnc_big);
            #endregion
            Application.Run(new FormMain());
        }

        public static byte[] PrinterPic { get; set; }
        public static byte[] CutterPic { get; set; }
        public static byte[] CncPic { get; set; }

        private static void checkRunningProgram()
        {
            //string processName = "AccentBase";
            //bool processExists = Process.GetProcesses().Any(p => p.ProcessName == processName);
            Process[] workers = Process.GetProcessesByName("AccentBase");
            if (workers.Length > 1)
            {
                DialogResult result = MessageBox.Show("База уже запущена!" + Environment.NewLine + "Вы хотите закрыть уже открытую базу и продолжить?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    foreach (Process worker in workers)
                    {
                        if (worker.Id != currentProcess.Id)
                        {
                            worker.Kill();
                            worker.WaitForExit();
                            worker.Dispose();
                        }
                    }
                }
                else
                {
                    System.Environment.Exit(1);
                }
            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_SHOWWINDOW = 0x0040;


        public static bool FTPWorking { get; set; }

        #region События для работы со списком отправки на сокет-сервер.
        public class OrderSendEventArgs : EventArgs
        {
            public ProtoClasses.ProtoOrders.protoOrder Order { get; set; }
            public SocketClient.TableClient.SocketMessageCommand Command { get; set; }
            public SocketClient.TableClient.TableName TableName{ get; set; }
            public ProtoClasses.ProtoOrdersChangeState.protoRowsList OrdersChangeState { get; set; }
            public ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList DownloadOrdersFiles { get; set; }
            public string Notes { get; set; }
            public OrderSendEventArgs(ProtoClasses.ProtoOrders.protoOrder order, ProtoClasses.ProtoOrdersChangeState.protoRowsList ordersChangeState, ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList downloadOrdersFiles, SocketClient.TableClient.SocketMessageCommand command, SocketClient.TableClient.TableName tableName, string notes)
            { Order = order; Command = command; OrdersChangeState = ordersChangeState; DownloadOrdersFiles = downloadOrdersFiles;  TableName = tableName; Notes = notes;}
        }

        public delegate void OrderSendDelegate(object sender, OrderSendEventArgs e);
        public static event OrderSendDelegate OrderSendEvent;
        static public void SendOrderToServer(OrderSendEventArgs e) { OrderSendEvent?.Invoke(null, e); }
        #endregion

        #region События для работы со списком получения от сокет-сервера.
        public delegate void OrderRecieveDelegate(object sender, ProtoClasses.ProtoOrders.protoOrder e);
        public static event OrderRecieveDelegate OrderRecieveEvent;
        static public void RecieveOrderFromServer(ProtoClasses.ProtoOrders.protoOrder e)
        {
            try
            {
                OrderRecieveEvent?.Invoke(null, e);
            }
            catch(Exception ex)
            {

            }
        }
        #endregion

        #region кликнули по BalloonTip
        public class BalloonTipClickedEventArgs : EventArgs
        {
            public Int64 Uid { get; set; }
            public SocketClient.TableClient.TableName TableName { get; set; }
            public BalloonTipClickedEventArgs (Int64 uid, SocketClient.TableClient.TableName tableName)
            { Uid = uid; TableName = tableName;}
        }
        public delegate void BalloonTipClickedDelegate(object sender, BalloonTipClickedEventArgs e);
        public static event BalloonTipClickedDelegate BalloonTipClickedEvent;
        static public void BalloonTipClicked(BalloonTipClickedEventArgs e) { BalloonTipClickedEvent?.Invoke(null, e); }
        #endregion

        #region Изменение в списке заявок, что необходимо отправить
        public class ConnectEventArgs : EventArgs
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
            public int OrderCommand { get; set; }
            public DateTime DateInit { get; set; }
            public Exception Ex { get; set; }
            public SocketClient.TableClient.SocketMessageCommand MessageCommand { get; set; }
            public ConnectEventArgs(long id, string name, string notes, DateTime dateInit, int orderCommand, Exception ex, SocketClient.TableClient.SocketMessageCommand messageCommand)
            { Id = id; Name = name; Notes = notes; Ex = ex; MessageCommand = messageCommand; DateInit = dateInit; OrderCommand = orderCommand; }
        }


        public delegate void Delegate_SendChangeToSocket(object sender, ConnectEventArgs e);
        public static event Delegate_SendChangeToSocket Event_SendChangeToSocket;
        public static void SendChangeToSocketEvent(ConnectEventArgs e){Event_SendChangeToSocket?.Invoke(null, e);}
        #endregion

        //#region Изменение статуса заявок
        //public class OrderSatusChangeEventArgs : EventArgs
        //{
        //    public long Id { get; set; }
        //    public string Name { get; set; }
        //    public string Notes { get; set; }
        //    public int OrderCommand { get; set; }
        //    public DateTime DateInit { get; set; }
        //    public Exception Ex { get; set; }
        //    public SocketClient.TableClient.SocketMessageCommand MessageCommand { get; set; }
        //    public OrderSatusChangeEventArgs(long id, string name, string notes, DateTime dateInit, int orderCommand, Exception ex, SocketClient.TableClient.SocketMessageCommand messageCommand)
        //    { Id = id; Name = name; Notes = notes; Ex = ex; MessageCommand = messageCommand; DateInit = dateInit; OrderCommand = orderCommand; }
        //}


        //public delegate void Delegate_OrderSatusChange(object sender, OrderSatusChangeEventArgs e);
        //public static event Delegate_OrderSatusChange Event_OrderSatusChange;
        //public static void SendOrderStatusChange(OrderSatusChangeEventArgs e) { Event_OrderSatusChange?.Invoke(null, e); }
        //#endregion
    }
}
