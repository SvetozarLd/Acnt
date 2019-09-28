using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Security.Cryptography;

namespace AccentBase.SocketClient
{
    internal static partial class TableClient
    {

        public static void StartMassUpdate()
        {
            MassUpdateTables(SqlLite.SqlEvent.TableName.TableMaterialPrint);
        }

        public static void MassUpdateTables(SqlLite.SqlEvent.TableName tn)
        {

            SqlLite.SqlCounters.SqlCoutersArgs ar = SqlLite.SqlCounters.GetCounters(tn);
            if (ar.ex != null)
            {

            }else
            {
                    ProtoClasses.ProtoCounters pc = new ProtoClasses.ProtoCounters();
                    byte[] msg = pc.protoSerialize(ar.clist.plist);
                    byte[] header = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (byte)tn };
                    byte[] message = new byte[msg.Length + header.Length];
                    Buffer.BlockCopy(header, 0, message, 0, header.Length);
                    Buffer.BlockCopy(msg, 0, message, header.Length, msg.Length);
                    SendToServer(message);
            }
        }


    }
}
