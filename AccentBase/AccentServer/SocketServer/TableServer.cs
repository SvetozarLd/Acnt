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
    public partial class TableServer
    {
  

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
        //static Socket clientsSocket;
        public bool ServerStop = true; // Флаг остановки сервера
        static int buffersize = 512; // размер буфера
        //byte[] buffer = new byte[buffersize]; // буфер
        int port = 4900; //порт
        #endregion

        #region Запуск сервера
        public void ServerStart(int Port, int BufferSize)
        {
            if (!ServerStop)
            {
                OnConnect(new ConnectEventArgs(null, "Старт сервера БД невозможен т.к. сервер уже работает.", false, SocketMessageCommand.Error));
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
                OnConnect(new ConnectEventArgs(null, "Старт сервера. Прослушивание порта: " + port, true, SocketMessageCommand.ServerStart));
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
                //loseClient.buffer = new byte[buffersize];
                clientList.TryRemove(s, out loseClient);
                if (loseClient != null) { loseClient.Dispose(); }
            }
            if (s != null)
            {
                if (s.Connected) { s.Disconnect(true); }
                //s.Close();
                s.Dispose();
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
                try
                {
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
                catch (Exception ex)
                {
                    OnConnect(new ConnectEventArgs(null, "Ошибка попытки подключения:" + Environment.NewLine + ex.Message, false, SocketMessageCommand.Error));
                    clientLose(clientList[clientSocket].Socket, "Потеря соединения!" + Environment.NewLine + ex.Message);
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
            catch (Exception ex)
            {
                clientLose(clientSocket," Ошибка полученных данных" + ex.Message);
            }
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



        #region Рассылка

        public void SendToAll(byte[] msg)
        {
            foreach (ClientInfo user in clientList.Values)
            {
                //if (user.Autorized)
                //{
                    try
                    {
                        byte[] encryptMessage = EncryptMessage(msg, user.Hash, user.Salt);
                        user.Socket.BeginSend(encryptMessage, 0, encryptMessage.Length, SocketFlags.None, new AsyncCallback(OnSend), user.Socket);
                    }
                    catch
                    {
                        clientLose(user.Socket, "Потеря соединения!");
                    }
                //}
            }
            //string tableName = string.Empty;
            //switch (msg[2])
            //{
            //    case (int)TableName.customers:
            //        tableName = " Таблица [Заказчики(customers)]";
            //        break;
            //    default:
            //        tableName = " Не определено";
            //        break;
            //}
            //OnConnect(new ConnectEventArgs(null, "Массовая рассылка." + tableName, true, SocketMessageCommand.log));
        }


        #endregion

    }
}
