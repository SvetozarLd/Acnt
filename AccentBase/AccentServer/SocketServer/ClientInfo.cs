using System.Net.Sockets;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace AccentServer.SocketServer
{
    public class ClientInfo : IDisposable
    {
        public Socket Socket { get; set; } //Сокет
        public string StrName { get; set; } // ФИО
        public int Id { get; set; } // ID в таблице Users
        public byte[] Hash { get; set; } // Hash Пароля
        public byte[] Salt { get; set; } // Генерируемая "Соль"
        public bool Autorized { get; set; } //флаг авторизации (разрешена отсылка)
        public int ByteMessageLength { get; set; }// размер сообщения
        public int ByteMessageCount { get; set; }
        //public DateTime ConnectTime { get; set; } //Время, когда подключились
        //public int idHardWare { get; set; }
        public string HardWareSerial { get; set; }
        public int PassError { get; set; }
        //public List<byte[]> toSend { get; set; }
        public byte[] bufferCache { get; set; } // Тут аккумулируются байты из буфера
        public byte[] buffer { get; set; } // буфер - Сюда поступают байты из сокета
        public byte[] ByteMessage { get; set; } // Само сообщение
        public ClientInfo(Socket socket, string strName, int id, byte[] hash, byte[] salt, int byteMessageLength, byte[] byteMessage, bool autorized, int passerror, string hardwareSerial, int byteMessageCount)
        {
            Socket = socket; StrName = strName; Id = id; Hash = hash; Salt = salt; ByteMessageLength = byteMessageLength; ByteMessage = byteMessage; Autorized = autorized; PassError = passerror; HardWareSerial = hardwareSerial; ByteMessageCount = byteMessageCount;
            //if (toSend == null) { toSend = new List<byte[]>(); }
        }
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
    }
}
