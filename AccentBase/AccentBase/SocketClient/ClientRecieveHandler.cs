using System;
using System.Data;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Security.Cryptography;

namespace AccentBase.SocketClient
{
    internal static partial class TableClient
    {
        #region Обработка комманд сервера
        private static void MessageHandler(byte[] msg)
        {
            if (msg.Length > 0)
            {
                Utils.Crypto sc = new Utils.Crypto();
                byte[] decryptmessage = sc.AES_Decrypt(msg, key, salt);
                if (decryptmessage != null)
                {
                    MakeIconEvent(new iconStateEvent(iconClient.NormalIcon));
                    switch (Convert.ToInt32(decryptmessage[0]))
                    {
                        case (int)SocketMessageCommand.ConnectOn:
                            HandlerConnectOn(decryptmessage);
                            break;
                        case (int)SocketMessageCommand.RowsChangeCounts:
                            HandlerRowsChangeCounts(decryptmessage);
                            break;
                        case (int)SocketMessageCommand.RowsChangeCountsProcess:
                            RowsChangeCountsProcess(decryptmessage);
                            break;
                        case (int)SocketMessageCommand.RowsChangeCountsSend:
                            RowsChangeCountsSend(decryptmessage);
                            break;
                        case (int)SocketMessageCommand.RowsUpdate:
                            HandlerRowsChangeCounts(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                        case (int)SocketMessageCommand.GetFilesList:
                            GetFilesList(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                        case (int)SocketMessageCommand.GetAllPreviewsList:
                            getAllPreviewsList(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                        case (int)SocketMessageCommand.OrderChangeStates:
                            OrderChangeStates(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                        case (int)SocketMessageCommand.DownloadOrderFiles:
                            DownloadOrdersFiles(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                        case (int)SocketMessageCommand.CopyOrder:
                            //(decryptmessage);
                            //SCEvent(new ConnectEventArgs(SocketMessageCommand.HWNotFound, serverSocket, "Серийный номер не найден в базе данных сервера!", true, null));
                            break;
                            //        case (int)SocketMessageCommand.HWRegistered:
                            //            HardWareKey_SetUp(decryptmessage);
                            //            break;
                            //        case (int)SocketMessageCommand.HWNewRegistered:
                            //            NewHardWare_SetUp(decryptmessage);
                            //            break;
                            //        case (int)SocketMessageCommand.Request_ChangingTable:
                            //            LastUpdateHandler(decryptmessage);
                            //            break;
                            //        case (int)SocketMessageCommand.ChangeTableOK:
                            //            LogsArea.SqliteSendingArgs args = new LogsArea.SqliteSendingArgs();
                            //            args.command = LogsArea.SqliteSending.SendingCommand.sendOk;
                            //            args.item = new LogsArea.protolog();
                            //            args.item.id = BitConverter.ToInt32(decryptmessage, 1);
                            //            LogsArea.SqliteSending.UpdateStatus(args);
                            //            break;
                            //        case (int)SocketMessageCommand.AccountMultiWork:
                            //            AccountMultiWork();
                            //            break;
                    }
            }
                else
                {
                    //SCEvent(new ConnectEventArgs(SocketMessageCommand.SecurityError, serverSocket, "Ошибка криптографии." + Environment.NewLine + "Необходима повторная авторизация", false, null));
                }
            }
        }
        #endregion

        //#region Копирование заявки

        //private














        //#endregion


        #region Получен от сервера список файлов, которые надо добавить в список закачек фтп
        private static void DownloadOrdersFiles(byte[] msg)
        {
            byte[] message = new byte[msg.Length - 2];
            Buffer.BlockCopy(msg, 2, message, 0, message.Length);
            ProtoClasses.ProtoDownloadOrdersFiles po = new ProtoClasses.ProtoDownloadOrdersFiles();
            ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList protoOrder = po.protoDeserialize(message);
            if (protoOrder != null && protoOrder.plist != null)
            {
                foreach (ProtoClasses.ProtoDownloadOrdersFiles.protoRow row in protoOrder.plist)
                {
                    if (row.Makets != null && row.Makets.Count > 0) {
                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in row.Makets)
                        { SqlLite.FtpSchedule.Insert(pr); } }
                    if (row.Documents != null && row.Documents.Count > 0)
                    {
                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in row.Documents)
                        { SqlLite.FtpSchedule.Insert(pr); }
                    }
                    if (row.Preview != null && row.Preview.Count > 0)
                    {
                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in row.Preview)
                        { SqlLite.FtpSchedule.Insert(pr); }
                    }
                    if (row.Photoreport != null && row.Photoreport.Count > 0)
                    {
                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in row.Photoreport)
                        { SqlLite.FtpSchedule.Insert(pr); }
                    }
                }

                ProtoClasses.ProtoOrders.protoOrder pro = new ProtoClasses.ProtoOrders.protoOrder();
                pro.id = 0;
                pro.sender_row_id = protoOrder.sender_row_id;
                pro.sender_row_stringid = protoOrder.sender_row_stringid;
                pro.command = (int)SocketClient.TableClient.SocketMessageCommand.DownloadOrderFiles;
                pro.work_name = protoOrder.name;
               Program.RecieveOrderFromServer(pro);
            }





        }
        #endregion
        #region Пришёл список превью
        private static void getAllPreviewsList(byte[] msg)
        {
            int orderid = BitConverter.ToInt32(msg, 2);
            byte[] message = new byte[msg.Length - 2];
            Buffer.BlockCopy(msg, 2, message, 0, message.Length);
            ProtoClasses.ProtoPreview pf = new ProtoClasses.ProtoPreview();
            List<ProtoClasses.ProtoPreview.protoRow> filelist = pf.protoDeserialize(message);
            PreviewListEvent(filelist);
        }
        #endregion
        #region Берем список файлов для показа заявки
        private static void GetFilesList(byte[] msg)
        {
            int orderid = BitConverter.ToInt32(msg, 2);
            byte[] message = new byte[msg.Length - 6];
            Buffer.BlockCopy(msg, 6, message, 0, message.Length);
            ProtoClasses.ProtoFiles pf = new ProtoClasses.ProtoFiles();
            List<ProtoClasses.ProtoFiles.protoRow> filelist = pf.protoDeserialize(message);
            FileListEvent(new RecieveFilesListEventArgs(orderid, filelist));
        }
        #endregion
        #region Начальное соединение произошло
        private static void HandlerConnectOn(byte[] msg)
        {
            try
            {
                byte[] message = new byte[msg.Length - 2];
                Buffer.BlockCopy(msg, 2, message, 0, message.Length);
                ProtoClasses.ProtoUsers pu = new ProtoClasses.ProtoUsers();

                List<string> ls = pu.protoDeserialize(message);
                SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn,null, null, string.Empty, true, null, ls));
                StartMassUpdate();
            }
            catch(Exception ex)
            {
                SCEvent(new ConnectEventArgs(SocketMessageCommand.Error, null, ex, "Ошибка подключения:", false, null, null));
                ConnectClose();
            }

        }
        #endregion
        #region Приход пакетного, стартового обновления БД
        private static void HandlerRowsChangeCounts(byte[] msg)
        {
            byte[] message = new byte[msg.Length - 2];
            Buffer.BlockCopy(msg, 2, message, 0, message.Length);
            switch ((int)msg[1])
            {
                case (int)SqlLite.SqlEvent.TableName.TableOrders:
                    ProtoClasses.ProtoOrders po = new ProtoClasses.ProtoOrders();
                    List<ProtoClasses.ProtoOrders.protoOrder> ls = po.protoDeserialize(message);
                    SqlLite.Order.UpdateTable(ls);
                    SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn, null, null, string.Empty, true, null, null));
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableMaterialPrint:
                    ProtoClasses.ProtoMaterial poprint = new ProtoClasses.ProtoMaterial();
                    List<ProtoClasses.ProtoMaterial.protoRow> lsprint = poprint.protoDeserialize(message);
                    SqlLite.Materials.UpdateTable(lsprint, SqlLite.SqlEvent.TableName.TableMaterialPrint);
                    //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn, null, null, string.Empty, true, null, null));
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableMaterialCut:
                    ProtoClasses.ProtoMaterial pocut = new ProtoClasses.ProtoMaterial();
                    List<ProtoClasses.ProtoMaterial.protoRow> lscut = pocut.protoDeserialize(message);
                    SqlLite.Materials.UpdateTable(lscut, SqlLite.SqlEvent.TableName.TableMaterialCut);
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableMaterialCnc:
                    ProtoClasses.ProtoMaterial pocnc = new ProtoClasses.ProtoMaterial();
                    List<ProtoClasses.ProtoMaterial.protoRow> lscnc = pocnc.protoDeserialize(message);
                    SqlLite.Materials.UpdateTable(lscnc, SqlLite.SqlEvent.TableName.TableMaterialCnc);
                    break;
                case (int)SqlLite.SqlEvent.TableName.TablePrinters:
                    ProtoClasses.ProtoEquip poprinters = new ProtoClasses.ProtoEquip();
                    List<ProtoClasses.ProtoEquip.protoRow> lsprinters = poprinters.protoDeserialize(message);
                    SqlLite.Equip.UpdateTable(lsprinters, SqlLite.SqlEvent.TableName.TablePrinters);
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableCutters:
                    ProtoClasses.ProtoEquip pocutters = new ProtoClasses.ProtoEquip();
                    List<ProtoClasses.ProtoEquip.protoRow> lscutters = pocutters.protoDeserialize(message);
                    SqlLite.Equip.UpdateTable(lscutters, SqlLite.SqlEvent.TableName.TableCutters);
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableCncs:
                    ProtoClasses.ProtoEquip pocncs = new ProtoClasses.ProtoEquip();
                    List<ProtoClasses.ProtoEquip.protoRow> lscncs = pocncs.protoDeserialize(message);
                    SqlLite.Equip.UpdateTable(lscncs, SqlLite.SqlEvent.TableName.TableCncs);
                    break;
                case (int)SqlLite.SqlEvent.TableName.TableNoteStateChange:
                    ProtoClasses.ProtoOrderHistory poorderhistory = new ProtoClasses.ProtoOrderHistory();
                    List<ProtoClasses.ProtoOrderHistory.protoRow> lsorderhistory = poorderhistory.protoDeserialize(message);
                    SqlLite.OrderHistory.UpdateTable(lsorderhistory);
                    break;
            }



        }
        #endregion
        #region Формирование списка на сервере
        private static void RowsChangeCountsProcess(byte[] msg)
        {
            int i = BitConverter.ToInt32(msg, 2);
            SqlLite.SqlEvent.OnSEND(new SqlLite.SqlEvent.SqliteUpdateEventArgs(i, 0, 0, SqlLite.SqlEvent.SqLiteEvent.ServerWaitProcess, (int)msg[1], false));
        }

        #endregion
        #region Сервер начал посылать список
        private static void RowsChangeCountsSend(byte[] msg)
        {
            int i = BitConverter.ToInt32(msg, 2);
            SqlLite.SqlEvent.OnSEND(new SqlLite.SqlEvent.SqliteUpdateEventArgs(i, 0, 0, SqlLite.SqlEvent.SqLiteEvent.ServerEndProcess, (int)msg[1], false));
            //SCEvent(new ConnectEventArgs(SocketMessageCommand.ConnectOn, null, string.Empty, true, null, ls));));
        }

        #endregion
        #region Изменение статуса заявок
        private static void OrderChangeStates(byte[] msg)
        {
            byte[] message = new byte[msg.Length - 2];
            Buffer.BlockCopy(msg, 2, message, 0, message.Length);
            switch ((int)msg[1])
            {
                case (int)SqlLite.SqlEvent.TableName.TableOrders:
                    ProtoClasses.ProtoOrdersChangeState po = new ProtoClasses.ProtoOrdersChangeState();
                    ProtoClasses.ProtoOrdersChangeState.protoRowsList lscutters = po.protoDeserialize(message);
                    SqlLite.Order.UpdateState(lscutters);
                    break;
            }

        }
        #endregion
    }
}
