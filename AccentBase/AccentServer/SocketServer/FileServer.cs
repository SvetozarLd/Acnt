using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace AccentServer.SocketServer
{
    public partial class FileServer
    {
        #region
        //// Incoming data from the client.
        //public static string data = null;

        //public static void StartListening()
        //{
        //    // Data buffer for incoming data.
        //    byte[] bytes = new Byte[1024];

        //    // Establish the local endpoint for the socket.
        //    // Dns.GetHostName returns the name of the 
        //    // host running the application.
        //    //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //    //IPAddress ipAddress = ipHostInfo.AddressList[0];
        //    IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);

        //    // Create a TCP/IP socket.
        //    Socket listener = new Socket(AddressFamily.InterNetwork,
        //        SocketType.Stream, ProtocolType.Tcp);

        //    // Bind the socket to the local endpoint and 
        //    // listen for incoming connections.
        //    try
        //    {
        //        listener.Bind(localEndPoint);
        //        listener.Listen(10);

        //        // Start listening for connections.
        //        while (true)
        //        {
        //            Console.WriteLine("Waiting for a connection...");
        //            // Program is suspended while waiting for an incoming connection.
        //            Socket handler = listener.Accept();
        //            data = null;

        //            // An incoming connection needs to be processed.
        //            while (true)
        //            {
        //                bytes = new byte[1024];
        //                int bytesRec = handler.Receive(bytes);
        //                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
        //                if (data.IndexOf("<EOF>") > -1)
        //                {
        //                    break;
        //                }
        //            }

        //            // Show the data on the console.
        //            Console.WriteLine("Text received : {0}", data);

        //            // Echo the data back to the client.
        //            byte[] msg = Encoding.ASCII.GetBytes(data);

        //            handler.Send(msg);
        //            handler.Shutdown(SocketShutdown.Both);
        //            handler.Close();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }

        //    Console.WriteLine("\nPress ENTER to continue...");
        //    Console.Read();

        //}
        #endregion

        #region Events
        public class ConnectEventArgs : EventArgs
        {
            public Socket Csocket { get; set; }
            public string RemoteIpPort { get; set; }
            public string Comment { get; set; }
            public bool Status { get; set; }
            public Exception Ex { get; set; }
            public SocketMessageCommand Command { get; set; }
            public ConnectEventArgs(Socket socket, string comment, bool status, SocketMessageCommand command)
            {
                Csocket = socket;
                if (socket != null)
                {
                    try
                    {
                        RemoteIpPort = socket.RemoteEndPoint.ToString();
                    }
                    catch { RemoteIpPort = string.Empty; }
                }
                else { RemoteIpPort = string.Empty; }
                Comment = comment; Status = status; Command = command;
            }
        }

        public delegate void ConnectEventHandler(object sender, ConnectEventArgs e);
        public event ConnectEventHandler Connect;
        protected virtual void OnConnect(ConnectEventArgs e) { if (Connect != null) { Connect(this, e); } }
        #endregion


        #region Начальные установки
        static internal ConcurrentDictionary<Socket, ClientInfo> clientList = new ConcurrentDictionary<Socket, ClientInfo>(); // список клиентов
        static internal Socket serverSocket;
        public bool ServerStop = true; // Флаг остановки сервера
        static int buffersize = 2048; // размер буфера
        int port = 4900; //порт
        #endregion

        #region Запуск сервера
        public void ServerStart(int Port, int BufferSize)
        {
            if (!ServerStop)
            {
                OnConnect(new ConnectEventArgs(null, "Старт файлового сервера невозможен т.к. сервер уже работает.", false, SocketMessageCommand.Error));
                return;
            }
            ServerStop = false;
            if (Port > 0) { port = Port; }
            if (BufferSize > 0) { buffersize = BufferSize; }
            try
            {

                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(100);
                serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                OnConnect(new ConnectEventArgs(null, "Старт файлого сервера. Прослушивание порта: " + port, true, SocketMessageCommand.ServerStart));
            }
            catch (Exception ex)
            {
                OnConnect(new ConnectEventArgs(null, ex.Message, false, SocketMessageCommand.Error));
            }
        }

        #endregion


        #region  Подключение клиентов
        private void OnAccept(IAsyncResult ar)
        {
            if (!ServerStop)
            {
                try
                {
                    Socket Sc = serverSocket.EndAccept(ar);
                    serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                    OnConnect(new ConnectEventArgs(Sc, "Попытка подключения", true, SocketMessageCommand.ConnectOn));
                    ClientInfo newClient = new ClientInfo(Sc, "Новое подключение", 0, null, null, 0, null, false, 0, string.Empty, 0);

                    newClient.buffer = new byte[buffersize];
                    if (!clientList.TryAdd(Sc, newClient))//;, (key, oldValue) => newClient);
                    {
                        clientLose(Sc, "Попытка подключения неудачна, такой сокет уже зарегестрирован.");
                    }
                    Sc.BeginReceive(newClient.buffer, 0, newClient.buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), Sc);

                }
                catch (Exception ex)
                {
                    OnConnect(new ConnectEventArgs(null, "Ошибка попытки подключения:" + Environment.NewLine + ex.Message, false, SocketMessageCommand.Error));
                }
            }
        }
        #endregion

        #region Убрать потерянного клиента
        private void clientLose(Socket s, string msg)
        {
            ClientInfo loseClient = null;
            if (clientList.TryGetValue(s, out loseClient))
            {
                OnConnect(new ConnectEventArgs(s, loseClient.StrName + ">>" + msg, false, SocketMessageCommand.ConnectLose));
                //loseClient.buffer. = new byte[buffersize];
                clientList.TryRemove(s, out loseClient);
            }
            if (s != null)
            {
                //if (s.Connected) { s.Disconnect(true); }
                s.Close();
                //s.Dispose();
            }
        }
        #endregion

        #region Получение данных от клиента (OnReceive)
        private void OnReceive(IAsyncResult ar)
        {
            if (!ServerStop)
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                int data = 0;
                try { data = clientSocket.EndReceive(ar); }
                catch { clientLose(clientSocket, "Потеря соединения!"); return; }
                if (data == 0) { clientLose(clientSocket, "Потеря соединения!"); return; }


                ClientInfo client;
                if (clientList.TryGetValue(clientSocket, out client))
                {

                    if (clientList[clientSocket].bufferCache == null)
                    {
                        clientList[clientSocket].bufferCache = new byte[data];
                        Buffer.BlockCopy(client.buffer, 0, clientList[clientSocket].bufferCache, 0, data);
                    }
                    else
                    {
                        byte[] tmp = new byte[clientList[clientSocket].bufferCache.Length + data];
                        Buffer.BlockCopy(clientList[clientSocket].bufferCache, 0, tmp, 0, clientList[clientSocket].bufferCache.Length);
                        Buffer.BlockCopy(client.buffer, 0, tmp, clientList[clientSocket].bufferCache.Length, data);
                        clientList[clientSocket].bufferCache = new byte[tmp.Length];
                        Buffer.BlockCopy(tmp, 0, clientList[clientSocket].bufferCache, 0, tmp.Length);
                    }
                    OnRecieveHandler(clientSocket);
                }
                else
                {
                    clientLose(clientSocket, "Клиент не зарегестрирован на сервере.");
                }
            }
        }
        #region Склейка сообщения клиента
        private void OnRecieveHandler(Socket clientSocket)
        {
            try
            {
                ClientInfo client;
                if (clientList.TryGetValue(clientSocket, out client))
                {
                    if (client.bufferCache.Length > 4)
                    {
                        switch (client.ByteMessageLength)
                        {
                            case 0:
                                client.ByteMessageLength = BitConverter.ToInt32(new byte[4] { client.bufferCache[0], client.bufferCache[1], client.bufferCache[2], client.bufferCache[3] }, 0);
                                if (client.ByteMessageLength == 0)
                                {
                                    byte[] tmp = new byte[client.bufferCache.Length - 4];
                                    Buffer.BlockCopy(client.bufferCache, 4, tmp, 0, tmp.Length);
                                    client.bufferCache = new byte[tmp.Length];
                                    Buffer.BlockCopy(tmp, 0, client.bufferCache, 0, tmp.Length);
                                    clientList[client.Socket] = client;
                                    OnRecieveHandler(client.Socket);
                                }
                                else
                                {

                                    if (client.ByteMessageLength == client.bufferCache.Length - 4)
                                    {
                                        client.ByteMessage = new byte[client.ByteMessageLength];
                                        Buffer.BlockCopy(client.bufferCache, 4, client.ByteMessage, 0, client.ByteMessageLength);
                                        MessageHandler(client);
                                        client.ByteMessageLength = 0;
                                        client.bufferCache = null;
                                        clientList[client.Socket] = client;
                                        StartRecieve(clientList[client.Socket]);
                                    }
                                    else
                                    {
                                        if (client.ByteMessageLength < client.bufferCache.Length - 4)
                                        {
                                            client.ByteMessage = new byte[client.ByteMessageLength];
                                            Buffer.BlockCopy(client.bufferCache, 4, client.ByteMessage, 0, client.ByteMessageLength);
                                            MessageHandler(client);
                                            byte[] tmp = new byte[client.bufferCache.Length - (client.ByteMessageLength + 4)];
                                            Buffer.BlockCopy(client.bufferCache, client.ByteMessageLength + 4, tmp, 0, tmp.Length);
                                            client.bufferCache = tmp;
                                            client.ByteMessageLength = 0;
                                            clientList[client.Socket] = client;
                                            OnRecieveHandler(client.Socket);
                                        }
                                        else
                                        {
                                            clientList[client.Socket] = client;
                                            StartRecieve(clientList[client.Socket]);
                                        }
                                    }
                                }
                                break;

                            default:
                                if (client.bufferCache.Length - 4 < client.ByteMessageLength)
                                {
                                    clientList[client.Socket] = client;
                                    StartRecieve(clientList[client.Socket]);
                                }
                                else
                                {
                                    if (client.bufferCache.Length - 4 == client.ByteMessageLength)
                                    {
                                        client.ByteMessage = new byte[client.ByteMessageLength];
                                        Buffer.BlockCopy(client.bufferCache, 4, client.ByteMessage, 0, client.ByteMessageLength);
                                        MessageHandler(client);
                                        client.ByteMessageLength = 0;
                                        client.bufferCache = null;
                                        clientList[client.Socket] = client;
                                        StartRecieve(clientList[client.Socket]);
                                    }
                                    else
                                    {
                                        client.ByteMessage = new byte[client.ByteMessageLength];
                                        Buffer.BlockCopy(client.bufferCache, 4, client.ByteMessage, 0, client.ByteMessageLength);
                                        MessageHandler(client);
                                        byte[] tmp = new byte[client.bufferCache.Length - (client.ByteMessageLength + 4)];
                                        Buffer.BlockCopy(client.bufferCache, client.ByteMessageLength + 4, tmp, 0, tmp.Length);
                                        client.bufferCache = tmp;
                                        client.ByteMessageLength = 0;
                                        clientList[client.Socket] = client;
                                        OnRecieveHandler(client.Socket);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        clientList[client.Socket] = client;
                        StartRecieve(client);
                    }
                }
            }
            catch { clientLose(clientSocket, " Ошибка полученных данных"); }
        }
        #endregion
        #region Запуск прослушивания клиента
        private void StartRecieve(ClientInfo client)
        {
            clientList[client.Socket].buffer = new byte[buffersize];
            //clientList[client.Socket]
            try
            {
                clientList[client.Socket].Socket.BeginReceive(clientList[client.Socket].buffer, 0, clientList[client.Socket].buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientList[client.Socket].Socket);
            }
            catch (Exception ex)
            {
                clientLose(clientList[client.Socket].Socket, "Потеря соединения!" + Environment.NewLine + ex.Message);
            }
        }
        #endregion
        #endregion

        #region Encrypt+Заголовок
        private byte[] EncryptMessage(byte[] msg, byte[] hash, byte[] salt)
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

        #region OnSend
        public void OnSend(IAsyncResult ar)
        {
            Socket clientSocket = (Socket)ar.AsyncState;
            //ar.AsyncState
            try
            {
                clientSocket.EndSend(ar);


            }
            catch (Exception ex)
            {
                OnConnect(new ConnectEventArgs(clientSocket, ex.Message, false, SocketMessageCommand.Error));
                clientLose(clientSocket, "Потеря соединения!");
            }
        }
        #endregion

        #region Проверить соединение с клиентом
        public bool ConnectTest(Socket s)
        {
            if (s != null)
            {
                try
                {
                    byte[] tmp = new byte[4] { 0, 0, 0, 0 };
                    s.BeginSend(tmp, 0, tmp.Length, SocketFlags.None, new AsyncCallback(OnSend), s);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Остановка сервера (закрытие сокетов)
        public void ServerEnd()
        {
            ServerStop = true;
            foreach (var item in clientList) { if (item.Key != null) { item.Key.Close(); } }
            clientList.Clear();
            OnConnect(new ConnectEventArgs(null, "Остановка сервера БД. Порт " + port.ToString() + " закрыт.", true, SocketMessageCommand.ServerStop));
            if (serverSocket != null) { serverSocket.Close(); serverSocket.Dispose(); serverSocket = null; }

        }
        #endregion


        #region Обработка входящих сообщений
        private void MessageHandler(ClientInfo client)
        {
            Utils.Crypto sc = new Utils.Crypto();
            byte[] decryptmessage = sc.AES_Decrypt(client.ByteMessage, null, null);
            if (decryptmessage != null)
            {
                client.ByteMessage = new byte[decryptmessage.Length];
                client.ByteMessage = decryptmessage;
                switch (Convert.ToInt32(decryptmessage[0]))
                {
                    case (int)SocketMessageCommand.ConnectOn:
                        FirstUserConnect(client);
                        break;
                    case (int)SocketMessageCommand.RowsChangeCounts:
                        //RowsChangeCounts(client);
                        break;
                    //    case (int)SocketMessageCommand.Authorization:
                    //        SetUserHash(client);
                    //        break;
                    //    case (int)SocketMessageCommand.UserAutorized:
                    //        userHWIDpass(client);
                    //        break;
                    //    case (int)SocketMessageCommand.Request_ChangingTable:
                    //        Request_ChangingTable(client);
                    //        break;
                    //    case (int)SocketMessageCommand.ChangeTable:
                    //        ChangeTable_Handler(client);
                    //        break;
                    case (int)SocketMessageCommand.None:
                        //OnConnect(new ConnectEventArgs(client.Socket, client.StrName + " тестовая отсылка", true, SocketMessageCommand.None));
                        break;
                }
            }
            else
            {
                OnConnect(new ConnectEventArgs(client.Socket, client.StrName + " >> Нарушение шифрованных данных, клиент отключен.", false, SocketMessageCommand.Error));
                clientLose(client.Socket, "Нарушение шифрования!");
            }
        }
        #endregion

        #region Первое подключение клиента - установка ника
        private void FirstUserConnect(ClientInfo client)
        {

            byte[] msg = new byte[client.ByteMessage.Length - 2];
            Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
            client.StrName = Utils.Converting.GetString(msg);
            clientList[client.Socket] = client;
            OnConnect(new ConnectEventArgs(client.Socket, client.StrName + " подключился", true, SocketMessageCommand.ConnectOn));

            byte[] header = new byte[2] { (int)SocketMessageCommand.ConnectOn, 0 };
            ProtoClasses.ProtoUsers pu = new ProtoClasses.ProtoUsers();
            Dictionary<string, string> dc = clientList.Values.Cast<ClientInfo>().ToDictionary(c => c.Socket.RemoteEndPoint.ToString(), c => c.StrName);
            List<string> ls = dc.Select(c => c.Value).ToList();
            //List<string> ls = clientList.Values.Cast<ClientInfo>().ToDictionary(c => c.Socket, c => c.StrName).SelectMany(d => d.Value).ToList();
            //Dictionary<string, string> dc = clientList.Values.Cast<ClientInfo>().ToDictionary(c => c.Socket.RemoteEndPoint.ToString(), c => c.StrName);
            byte[] body = pu.protoSerialize(ls);
            byte[] message = new byte[header.Length + body.Length];
            Buffer.BlockCopy(header, 0, message, 0, header.Length);
            Buffer.BlockCopy(body, 0, message, header.Length, body.Length);
            //Send(client, message);

            //byte[] HeaderToAll = new byte[2] {(int)SocketMessageCommand. };
            //SendToAll(message);

        }
        #endregion

        private void FileToClient(ClientInfo client)
        {
            byte[] msg = new byte[client.ByteMessage.Length - 2];
            Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);

        }


    }
}
