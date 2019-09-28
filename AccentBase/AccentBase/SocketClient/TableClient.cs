using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Timers;
//using System.Threading;
namespace AccentBase.SocketClient
{
    internal static partial class TableClient
    {
        #region Переменные
        private static byte[] key = null; //new byte[32]; // хэш пароля 
        private static byte[] salt = null; //new byte[8]; // соль
        //internal static string username = ""; // имя пользователя
        //internal static int userid = 0;
        private static int buffersize = 4096; // размер буфера
        private static int messagecachelength = 0;
        private static byte[] buffercache = null; //сборка сообщений
        internal static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public static bool IsConnected = false;         // Состояние соединения
        internal static bool authorization = false;  // Флаг авторизации
        internal static bool AutoSendChangeCounters = false; // Автоматически отсылать последние даты изменений строк в БД

        private static Socket serverSocket = null; // сокет сервера
        private static byte[] buffer = new byte[buffersize]; // буфер сокета
        private static string address = ""; // адрес сервера
        private static int port = 4900; //
        public static bool autoconnect = false; // автозапуск вкл/выкл
        public static bool StartedEventsEnded { get; set; }//Пройдены ли все передачи при подключении.
        #endregion

        #region  Проверка соединения по таймеру.
        private static Timer aTimer;

        private static bool StartServerConnect = false;
        public static int RefreshConnect = 5000;

        #region Задержка при переподключении при потере связи
        private static Timer ReconnectTimer;
        public static int ReconnectWaiting = 1000;
        private static void SetTimerReconnect()
        {
            if (ReconnectTimer == null) { ReconnectTimer = new Timer(ReconnectWaiting); }

            ReconnectTimer.Elapsed += ReconnectWaitingEvent;
            ReconnectTimer.AutoReset = false;
            ReconnectTimer.Enabled = true;
        }
        private static void ReconnectWaitingEvent(object source, ElapsedEventArgs e)
        {
            ReconnectTimer.Elapsed -= ReconnectWaitingEvent;
            ReconnectTimer.Stop();
            ReconnectTimer.Close();
            ConnectClosing = false;// ConnectStart(); }
            if (autoconnect) { ConnectStartLock = false; ConnectStart(); }
            //ConnectClose();

            //ConnectChangeStateEventMsg(serverSocket, "Соединение прервано!", false, false);
            //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectLose, null, string.Empty, false, null, null));
            //salt = null;
            //ConnectStart();
        }
        #endregion
        private static void SetTimer()
        {
            if (aTimer == null) { aTimer = new Timer(RefreshConnect); }

            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (StartServerConnect)
            {
                if (!ConnectTest(serverSocket))
                {
                    aTimer.Elapsed -= OnTimedEvent;
                    aTimer.Stop();
                    aTimer.Close();
                    ConnectClose();
                    //ConnectChangeStateEventMsg(serverSocket, "Соединение прервано!", false, false);
                    //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectLose, null, string.Empty, false, null, null));
                    //salt = null;
                    //ConnectStart();
                }
            }
        }

        #endregion


        #region Остановить подключение
        internal static bool StopConnecting()
        {
            autoconnect = false;

            StartedEventsEnded = false;
            //ConnectClosing = false;
            ConnectClose();
            if (serverSocket == null)
            { return true; }
            else { return false; }
        }
        #endregion
        private static bool ConnectClosing = false;
        #region Прекратить соединение
        private static void ConnectClose()
        {
            if (ConnectClosing) { return; }
            SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, null, null, "Потеря соединения с сервером", false, null, null));
            ConnectClosing = true;
            MakeIconEvent(new iconStateEvent(iconClient.Offline));
            key = null;
            salt = null;
            authorization = false;
            StartServerConnect = false;
            StartedEventsEnded = false;
            //username = "";
            //userid = 0;
            messagecachelength = 0;
            IsConnected = false;
            buffer = new byte[buffersize];
            if (buffercache != null) { buffercache = null; }
            if (serverSocket != null)
            {
                if (serverSocket.Connected) { serverSocket.Shutdown(SocketShutdown.Both); }
                serverSocket.Close();
                //serverSocket.Dispose();
                serverSocket = null;
                if (aTimer != null)
                {
                    try
                    {
                        aTimer.Elapsed -= OnTimedEvent;
                        aTimer.Stop();
                        aTimer.Close();
                        aTimer = null;
                    }
                    catch { aTimer = null; }
                }

            }
            SetTimerReconnect();
        }
        ////Закрыть сокет если надо.Создать событие изменения состояние подключения.
        //private static void ConnectChangeStateEventMsg(Socket s, string msg, bool Con, bool view)
        //{
        //    if (!Con && serverSocket != null) { serverSocket.Close(); }
        //    if (IsConnected ^ Con) { SCEvent(new ConnectEventArgs(SocketMessageCommand.LineStatus, s, msg, Con, null)); } else { if (view) { SCEvent(new ConnectEventArgs(SocketMessageCommand.LineStatus, s, msg, Con, null)); } }
        //    if (!Con) { salt = null; key = null; }
        //    IsConnected = Con;
        //}


        #endregion

        #region Events
        // Аргументы к подключению
        public class ConnectEventArgs : EventArgs
        {
            public SocketMessageCommand eventStatus { get; set; }
            public Socket Csocket { get; set; }
            public Exception Ex { get; set; }
            public string Message { get; set; }
            public bool Status { get; set; }
            public byteMessage Msg { get; set; }
            public List<string> usrNames { get; set; }
            public ConnectEventArgs(SocketMessageCommand eventstatus, Socket socket, Exception ex, string message, bool status, byteMessage msg, List<string> UsrNames)
            { eventStatus = eventstatus; Csocket = socket; Message = message; Status = status; Msg = msg; usrNames = UsrNames; Ex = ex; }
        }

        public class RecieveEventArgs : EventArgs
        {
            public int AllByte { get; set; }
            public int RecieveByte { get; set; }
            public bool Complete { get; set; }
            public RecieveEventArgs(int allbyte, int recievebyte, bool complete) { AllByte = allbyte; RecieveByte = recievebyte; Complete = complete; }
        }

        // аргументы к получению данных
        public class byteMessage : EventArgs
        {
            //public сommandFromSocket Command { get; set; }
            public int Command { get; set; }
            public int Table { get; set; }
            //public int Id { get; set; }
            public byte[] Message { get; set; }
            //public int MessageLength { get; set; }
            public byteMessage(int command, int table, byte[] message) { Command = command; Table = table; Message = message; }
        }


        public class iconStateEvent : EventArgs
        {
            public iconClient ico { get; set; }
            public iconStateEvent(iconClient Ico) { ico = Ico; }
        }
        //событие подключения
        public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);
        public static event ConnectEventHandler SocketClientEvent;
        private static void SCEvent(ConnectEventArgs e) { SocketClientEvent?.Invoke(null, e); }

        // Полученные байты для progress- бара
        public delegate void RecieveEventHandler(object sender, RecieveEventArgs e);
        public static event RecieveEventHandler SocketRecieveEvent = delegate { };
        private static void RCEvent(RecieveEventArgs e) { SocketRecieveEvent?.Invoke(null, e); }


        public delegate void iconEventHandler(object sender, iconStateEvent e);
        public static event iconEventHandler IconEvent = delegate { };
        private static void MakeIconEvent(iconStateEvent e) { IconEvent?.Invoke(null, e); }

        //список файлов для заявки
        public delegate void FileListEventHandler(object sender, RecieveFilesListEventArgs e);
        public static event FileListEventHandler FilelistCome;
        private static void FileListEvent(RecieveFilesListEventArgs e) { FilelistCome?.Invoke(null, e); }
        public class RecieveFilesListEventArgs : EventArgs
        {
            public int Recieve_OrderId { get; set; }
            public List<ProtoClasses.ProtoFiles.protoRow> filelist { get; set; }
            public RecieveFilesListEventArgs(int OrderID, List<ProtoClasses.ProtoFiles.protoRow> FileList) { Recieve_OrderId = OrderID; filelist = FileList; }
        }


        // Список файлов превью
        //список файлов для заявки
        public delegate void PreviewListEventHandler(object sender, List<ProtoClasses.ProtoPreview.protoRow> e);
        public static event PreviewListEventHandler PreviewlistComeEvent;
        private static void PreviewListEvent(List<ProtoClasses.ProtoPreview.protoRow> e) { PreviewlistComeEvent?.Invoke(null, e); }
        #endregion





        public static void Connect()
        {
            if (!autoconnect)
            {
                autoconnect = true;
                ConnectClose();
            }
        }

        #region Подготовка к подключению. Создание соединения
        private static bool ConnectStartLock = false;
        private static void ConnectStart()
        {
            if (serverSocket != null) { return; }
            if (Utils.Settings.set.buffer_size > 0) { buffersize = Utils.Settings.set.buffer_size; }
            if (!Utils.Settings.set.server_address.Equals(string.Empty)) { address = Utils.Settings.set.server_address; }
            if (Utils.Settings.set.server_port > 1024) { port = Utils.Settings.set.server_port; }
            //if (lockstart) { return; }
            //lockstart = true;
            //ConnectChangeStateEventMsg(null, Environment.NewLine + "ConnectStart" + Environment.NewLine, IsConnected, true);

            if (!IsConnected && !ConnectStartLock)
            {
                ConnectStartLock = true;
                buffer = new byte[buffersize];
                try
                {
                    StartedEventsEnded = false;
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.ServerStart, null, null, "Попытка подключения к серверу.", false, null, null));
                    StartServerConnect = true;
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipAddress = IPAddress.Parse(address);
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
                    IAsyncResult result = serverSocket.BeginConnect(ipEndPoint, new AsyncCallback(Connecting), null);
                    //bool success = result.AsyncWaitHandle.WaitOne(2000, true);
                    //if (!success)
                    //{
                    //    throw new ApplicationException("Подключение не удалось.");
                    //}else
                    //{
                    //    IsConnected = true;
                    //}

                }
                catch (Exception ex)
                {
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, null, ex, "Попытка подключения к серверу.", false, null, null));
                    ConnectClose();
                }
            }
        }
        #endregion
        #region Соединение успешно. Подключение.
        private static void Connecting(IAsyncResult ar)
        {
            try
            {
                //autoconnect = true;
                serverSocket.EndConnect(ar);
                //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn, null, string.Empty, true, null));
                //ConnectChangeStateEventMsg(serverSocket, "Подключился!", true, false);
                if (!IsConnected)
                {
                    IsConnected = true;
                    SetTimer();
                    byte[] msgHeader = new byte[2] { (int)SocketMessageCommand.ConnectOn, 0 };
                    byte[] msg = Utils.Converting.GetBytes(Utils.Settings.set.name);
                    byte[] fullmasg = new byte[msgHeader.Length + msg.Length];
                    //rngCsp.GetBytes(trash);
                    Buffer.BlockCopy(msgHeader, 0, fullmasg, 0, msgHeader.Length);
                    Buffer.BlockCopy(msg, 0, fullmasg, msgHeader.Length, msg.Length);
                    //byte[] encryptMsg = EncryptMessage(fullmasg, null, null);
                    //serverSocket.BeginSend(encryptMsg, 0, encryptMsg.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                    SendToServer(fullmasg);
                    buffer = new byte[buffersize];
                    serverSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.None, null, null, "К серверу подключились.", false, null, null));
                }
                else
                {
                    ConnectClose();
                }
            }
            catch (Exception ex)
            {
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, ex, "Попытка подключения к серверу.", false, null, null));
                ConnectClose();
                //IsConnected = false;
                //serverSocket.Close();
                //serverSocket = null;
                //ConnectChangeStateEventMsg(serverSocket, ex.Message, false, true);
            }
        }
        #endregion

        #region message - Encrypt + Header
        private static byte[] EncryptMessage(byte[] msg, byte[] Key, byte[] Salt)
        {
            try
            {
                Utils.Crypto sc = new Utils.Crypto();
                byte[] encryptMessage = sc.AES_Encrypt(msg, null, null);
                int encryptMessageLength = encryptMessage.Length;
                byte[] encryptMessageHeader = new byte[4];
                encryptMessageHeader = BitConverter.GetBytes(encryptMessageLength);
                byte[] encryptMessageWithHeader = new byte[4 + encryptMessageLength];
                Buffer.BlockCopy(encryptMessageHeader, 0, encryptMessageWithHeader, 0, 4);
                Buffer.BlockCopy(encryptMessage, 0, encryptMessageWithHeader, 4, encryptMessageLength);
                return encryptMessageWithHeader;
            }
            catch { return null; }
        }
        #endregion

        #region Универсальная отправка данных
        private static bool SendingBusy = false;
        public static void SendToServer(byte[] msg)
        {
            MakeIconEvent(new iconStateEvent(iconClient.SendIcon));
            byte[] encryptMsg = EncryptMessage(msg, null, null);
            try
            {
                SendingBusy = true;
                serverSocket.BeginSend(encryptMsg, 0, encryptMsg.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

                //SCEvent(new ConnectEventArgs(SocketMessageCommand.Authorization, serverSocket, "Авторизация удачна!", true, null));
            }
            catch (Exception ex)
            {
                //ConnectChangeStateEventMsg(serverSocket, "Соединение прервано!", false, false);
                SendingBusy = false;
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, ex, "Передача данных на сервер:", false, null, null));
                ConnectClose();
            }
        }

        #endregion
        #region окончание отправки данных
        private static void OnSend(IAsyncResult ar)
        {
            SendingBusy = false;
            if (IsConnected)
            {
                try
                {
                    int data = 0;
                    data = serverSocket.EndSend(ar);
                    MakeIconEvent(new iconStateEvent(iconClient.NormalIcon));
                    //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn, null, null, string.Empty, true, null, null));

                    //if (StartedEventsEnded && SqlLite.SendChangeToSocket.TableOrdersSend.Rows.Count > 0) //
                    //{
                    //    DataRow dr = SqlLite.SendChangeToSocket.TableOrdersSend.Select("sending = false").FirstOrDefault();//SqlLite.SendChangeToSocket.TableOrdersSend.Rows[0];                            
                    //    if (dr != null)
                    //    {
                    //        try
                    //        {
                    //            dr["sending"] = true;
                    //            byte[] head = new byte[2] { Convert.ToByte(dr["order_command"]), Convert.ToByte(dr["table_enum"]) };
                    //            byte[] msg = (byte[])dr["order_binary"];
                    //            byte[] message = new byte[head.Length + msg.Length];
                    //            Buffer.BlockCopy(head, 0, message, 0, head.Length);
                    //            Buffer.BlockCopy(msg, 0, message, head.Length, msg.Length);
                    //            SendToServer(message);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            dr["sending"] = false;
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    //ConnectChangeStateEventMsg(serverSocket, ex.Message, false, false);
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, ex, "Передача данных на сервер:", false, null, null));
                    ConnectClose();
                }
            }
        }
        #endregion

        #region Получение данных (OnReceive)
        private static void OnReceive(IAsyncResult ar)
        {
            MakeIconEvent(new iconStateEvent(iconClient.RecieveStart));
            int data = 0;
            try { data = serverSocket.EndReceive(ar); }
            catch (Exception ex)
            {
                //ConnectChangeStateEventMsg(serverSocket, ex.Message, false, false);
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, ex, "Ошибка приёма данных:", false, null, null));
                ConnectClose();
                //buffer = new byte[buffersize];
                return;
            }
            if (data == 0)
            {
                //ConnectChangeStateEventMsg(serverSocket, "Соединение потеряно", false, false);
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, new ApplicationException("Соединение потеряно"), "Ошибка приёма данных:", false, null, null));
                ConnectClose();
                //buffer = new byte[buffersize];
                return;
            }


            if (buffercache == null)
            {
                buffercache = new byte[data];
                Buffer.BlockCopy(buffer, 0, buffercache, 0, data);
            }
            else
            {
                byte[] tmp = new byte[buffercache.Length + data];
                Buffer.BlockCopy(buffercache, 0, tmp, 0, buffercache.Length);
                Buffer.BlockCopy(buffer, 0, tmp, buffercache.Length, data);
                buffercache = tmp;
            }
            OnRecieveHandler();

        }


        private static void OnRecieveHandler()
        {
            try
            {
                if (buffercache.Length > 4)
                {
                    switch (messagecachelength)
                    {
                        case 0:
                            messagecachelength = BitConverter.ToInt32(new byte[4] { buffercache[0], buffercache[1], buffercache[2], buffercache[3] }, 0);
                            if (messagecachelength == 0)
                            {
                                byte[] tmp = new byte[buffercache.Length - 4];
                                Buffer.BlockCopy(buffercache, 4, tmp, 0, tmp.Length);
                                buffercache = new byte[tmp.Length];
                                Buffer.BlockCopy(tmp, 0, buffercache, 0, tmp.Length);
                                RCEvent(new RecieveEventArgs(messagecachelength, messagecachelength, true));
                                OnRecieveHandler();
                            }
                            else
                            {



                                if (messagecachelength == buffercache.Length - 4)
                                {
                                    byte[] singleBuffer = new byte[messagecachelength];
                                    Buffer.BlockCopy(buffercache, 4, singleBuffer, 0, messagecachelength);
                                    RCEvent(new RecieveEventArgs(messagecachelength, messagecachelength, true));
                                    messagecachelength = 0;
                                    buffercache = null;
                                    MessageHandler(singleBuffer);
                                    StartRecieve();
                                }
                                else
                                {
                                    if (messagecachelength < buffercache.Length - 4)
                                    {
                                        byte[] multiBuffer = new byte[messagecachelength];
                                        Buffer.BlockCopy(buffercache, 4, multiBuffer, 0, messagecachelength);
                                        RCEvent(new RecieveEventArgs(messagecachelength, messagecachelength, true));
                                        MessageHandler(multiBuffer);
                                        byte[] tmp = new byte[buffercache.Length - (messagecachelength + 4)];
                                        Buffer.BlockCopy(buffercache, messagecachelength + 4, tmp, 0, tmp.Length);
                                        buffercache = tmp;
                                        messagecachelength = 0;
                                        OnRecieveHandler();
                                    }
                                    else
                                    {
                                        RCEvent(new RecieveEventArgs(messagecachelength, buffercache.Length - 4, false));
                                        StartRecieve();
                                    }
                                }
                            }
                            break;

                        default:
                            if (buffercache.Length - 4 < messagecachelength)
                            {
                                RCEvent(new RecieveEventArgs(messagecachelength, buffercache.Length - 4, false));
                                StartRecieve();
                            }
                            else
                            {
                                if (buffercache.Length - 4 == messagecachelength)
                                {
                                    byte[] singleBuffer = new byte[messagecachelength];
                                    Buffer.BlockCopy(buffercache, 4, singleBuffer, 0, messagecachelength);
                                    RCEvent(new RecieveEventArgs(messagecachelength, messagecachelength, true));
                                    messagecachelength = 0;
                                    buffercache = null;
                                    MessageHandler(singleBuffer);
                                    StartRecieve();
                                }
                                else
                                {
                                    byte[] multiBuffer = new byte[messagecachelength];
                                    Buffer.BlockCopy(buffercache, 4, multiBuffer, 0, messagecachelength);
                                    RCEvent(new RecieveEventArgs(messagecachelength, messagecachelength, true));
                                    MessageHandler(multiBuffer);
                                    byte[] tmp = new byte[buffercache.Length - (messagecachelength + 4)];
                                    Buffer.BlockCopy(buffercache, messagecachelength + 4, tmp, 0, tmp.Length);
                                    buffercache = tmp;
                                    messagecachelength = 0;
                                    OnRecieveHandler();
                                }
                            }
                            break;
                    }
                }
                else
                {
                    StartRecieve();
                }
            }
            catch (Exception ex)
            {
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, null, "Ошибка приёма сообщения от сервера.", false, null, null));
                ConnectClose();
            }
        }

        private static void StartRecieve()
        {
            buffer = new byte[buffersize];
            try
            {
                serverSocket.BeginReceive(buffer, 0, buffersize, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (Exception ex)
            {
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, serverSocket, ex, "Ошибка начала приёма данных:", false, null, null));
                ConnectClose();
                //ConnectChangeStateEventMsg(serverSocket, ex.Message, false, false); buffer = new byte[buffersize];
            }
        }

        #endregion




        #region  Проверка состояния линии
        public static bool ConnectTest(Socket s)
        {
            //if (s != null && !SendingBusy)
            //{
            //    if (SqlLite.SendChangeToSocket.TableOrdersSend.Rows.Count > 0)
            //    {
            //        DataRow dr = SqlLite.SendChangeToSocket.TableOrdersSend.Rows[0];
            //        byte[] head = new byte[4] { Convert.ToByte(dr["order_command"]), Convert.ToByte(dr["table_enum"]),0,0};
            //        byte[] msg = (byte[])dr["order_binary"];
            //        byte[] message = new byte[head.Length + msg.Length];
            //        Buffer.BlockCopy(head, 0, message, 0, 2);
            //        Buffer.BlockCopy(msg, 0, message, 2, msg.Length);
            //        SendToServer(message);
            //        return true;
            //    }
            //    else
            //    {
            if (s != null && !SendingBusy)
            {
                try
                {
                    //SCEvent(new ConnectEventArgs(SocketMessageCommand.None, serverSocket, null, "Тестовая отсылка", false, null, null));
                    byte[] head = new byte[6] { (int)SocketMessageCommand.None, 6, 6, 6, 6, 6 };
                    SendToServer(head);
                    //byte[] body1 = BitConverter.GetBytes(percentLast);
                    //byte[] message1 = new byte[head1.Length + body1.Length];
                    //Buffer.BlockCopy(head1, 0, message1, 0, head1.Length);
                    //Buffer.BlockCopy(body1, 0, message1, head1.Length, body1.Length);
                    //SocketServer.Servers.server.Send(client, message1);
                    //return outList;

                    //byte[] tmp = new byte[4] { 0, 0, 0, 0 };



                    //s.BeginSend(tmp, 0, tmp.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                    return true;
                }
                catch (Exception ex)
                {
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.None, serverSocket, null, "Тестовая отсылка не удалась", false, null, null));
                    //Console.Write(Environment.NewLine + ex.Message + Environment.NewLine);
                    ConnectClose();
                    return false;
                }
                //}
            }
            else
            {
                return false;
            }

        }
        #endregion

    }
}
