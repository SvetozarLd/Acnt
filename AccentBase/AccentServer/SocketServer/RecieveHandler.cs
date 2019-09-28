using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace AccentServer.SocketServer
{
    public partial class TableServer
    {
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
                        RowsChangeCounts(client);
                        break;
                    case (int)SocketMessageCommand.GetFilesList:
                        getFilesList(client);
                        break;
                    case (int)SocketMessageCommand.GetAllPreviewsList:
                        getAllPreviewsList(client);
                        break;
                    case (int)SocketMessageCommand.RowsInsert:
                        InsertRowHandler(client);
                        break;
                    case (int)SocketMessageCommand.RowsUpdate:
                        UpdateRowHandler(client);
                        break;
                    case (int)SocketMessageCommand.OrderChangeStates:
                        OrderChangeStates(client);
                        break;
                    case (int)SocketMessageCommand.DownloadOrderFiles:
                        DownloadOrderFiles(client);
                        break;
                    case (int)SocketMessageCommand.None:
                        //if (!ConnectTest(client.Socket)) { clientLose(client.Socket, "Потеря соединения! - ответ на тест. запрос."); }
                        break;
                    //    case (int)SocketMessageCommand.UserAutorized:
                    //        userHWIDpass(client);
                    //        break;
                    //    case (int)SocketMessageCommand.Request_ChangingTable:
                    //        Request_ChangingTable(client);
                    //        break;
                    //    case (int)SocketMessageCommand.ChangeTable:
                    //        ChangeTable_Handler(client);
                    //        break;
                    //case (int)SocketMessageCommand.None:
                    //    //OnConnect(new ConnectEventArgs(client.Socket, client.StrName + " тестовая отсылка", true, SocketMessageCommand.None));
                    //    break;
                }
            }
            else
            {
                OnConnect(new ConnectEventArgs(client.Socket, client.StrName + " >> Нарушение шифрованных данных, клиент отключен.", false, SocketMessageCommand.Error));
                clientLose(client.Socket, "Нарушение шифрования!");
            }
        }
        #endregion

        #region Клиент хочет скачать все файлы списка заявок
        private void DownloadOrderFiles(ClientInfo client)
        {
            byte[] msg = new byte[client.ByteMessage.Length - 2];
            Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
            ProtoClasses.ProtoDownloadOrdersFiles po = new ProtoClasses.ProtoDownloadOrdersFiles();
            ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList protoOrder = po.protoDeserialize(msg);
            if (protoOrder  != null && protoOrder.plist!= null)
                {
                foreach (ProtoClasses.ProtoDownloadOrdersFiles.protoRow row in protoOrder.plist)
                {                   
                    //List<AccentServer.ProtoClasses.ProtoFtpSchedule.protoRow> files = new List<AccentServer.ProtoClasses.ProtoFtpSchedule.protoRow>();
                    if (row.uid != 0)
                    {
                        //DataRow rdr = MySql.DataTables.orders.Select("id = " + row.uid).First();

                        ProtoClasses.ProtoOrders.protoOrder rdr = null;
                        if (MySql.DataTables.orders.ContainsKey(Convert.ToInt32(row.uid))) { rdr = MySql.DataTables.orders[Convert.ToInt32(row.uid)]; }
                        //DataRow rdr = MySql.DataTables.orders.Select("id = " + orderid).First();
                        if (rdr != null)
                        {
                            DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(rdr.time_recieve);

                        //    if (DateTime.TryParse(Convert.ToString(rdr["time_recieve"]), out DateTime tmpenddate))
                        //{
                            string yearpath = Properties.Settings.Default.FilePath + @"/" + tmpenddate.ToString("yyyy.MM");
                            string orderPath = tmpenddate.ToString("yyyy.MM") + "/" + row.uid.ToString("0000");
                            string FullOrderPath = Properties.Settings.Default.FilePath + @"/" + orderPath;
                            DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
                            if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                            if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }
                            #region макеты
                            if (Directory.Exists(FullOrderPath + @"/" + "makets"))
                            {
                                row.Makets = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();

                                foreach (string f in Directory.GetFiles(FullOrderPath + @"/" + "makets"))
                                {
                                    ProtoClasses.ProtoFtpSchedule.protoRow fil = new ProtoClasses.ProtoFtpSchedule.protoRow();
                                    FileInfo fi = new FileInfo(f);
                                    fil.Upload = false;
                                    fil.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                    fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                    fil.Length = fi.Length;
                                    fil.fileshortname = fi.Name;
                                    fil.targetfile = row.LocalPath + @"/" + "makets" + @"/" + fi.Name;
                                    fil.serveraddress = @"Server/" + orderPath + @"/" + "makets" + @"/" + fi.Name;
                                    fil.sourcefile = "makets" + @"/" + orderPath + @"/" + "makets" + @"/" + fi.Name;
                                    fil.LengthString = Utils.Converting.LengthToString(fi.Length);
                                    fil.order_id = row.uid;
                                    row.Makets.Add(fil);
                                }
                            }
                            else { Directory.CreateDirectory(orderPath + @"/" + "makets"); }
                            #endregion
                            #region превью
                            if (Directory.Exists(FullOrderPath + @"/" + "preview"))
                            {
                                row.Preview = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();

                                foreach (string f in Directory.GetFiles(FullOrderPath + @"/" + "preview"))
                                {
                                    ProtoClasses.ProtoFtpSchedule.protoRow fil = new ProtoClasses.ProtoFtpSchedule.protoRow();
                                    FileInfo fi = new FileInfo(f);
                                    fil.Upload = false;
                                    fil.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                    fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                    fil.Length = fi.Length;
                                    fil.fileshortname = fi.Name;
                                    fil.targetfile = row.LocalPath + @"/" + "preview" + @"/" + fi.Name;
                                    fil.serveraddress = @"Server/" + orderPath + @"/" + "preview" + @"/" + fi.Name;
                                    fil.sourcefile = "makets" + @"/" + orderPath + @"/" + "preview" + @"/" + fi.Name;
                                    fil.LengthString = Utils.Converting.LengthToString(fi.Length);
                                    fil.order_id = row.uid;
                                    row.Preview.Add(fil);
                                }
                            }
                            else { Directory.CreateDirectory(orderPath + @"/" + "preview"); }
                            #endregion
                            #region документы
                            if (Directory.Exists(FullOrderPath + @"/" + "doc"))
                            {
                                row.Documents = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();

                                foreach (string f in Directory.GetFiles(FullOrderPath + @"/" + "doc"))
                                {
                                    ProtoClasses.ProtoFtpSchedule.protoRow fil = new ProtoClasses.ProtoFtpSchedule.protoRow();
                                    FileInfo fi = new FileInfo(f);
                                    fil.Upload = false;
                                    fil.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                    fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                    fil.Length = fi.Length;
                                    fil.fileshortname = fi.Name;
                                    fil.targetfile = row.LocalPath + @"/" + "doc" + @"/" + fi.Name;
                                    fil.serveraddress = @"Server/" + orderPath + @"/" + "doc" + @"/" + fi.Name;
                                    fil.sourcefile = "makets" + @"/" + orderPath + @"/" + "doc" + @"/" + fi.Name;
                                    fil.LengthString = Utils.Converting.LengthToString(fi.Length);
                                    fil.order_id = row.uid;
                                    row.Documents.Add(fil);
                                }
                            }
                            else { Directory.CreateDirectory(orderPath + @"/" + "doc"); }
                            #endregion
                            #region фотоотчеты
                            if (Directory.Exists(FullOrderPath + @"/" + "photoreport"))
                            {
                                row.Photoreport = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();

                                foreach (string f in Directory.GetFiles(FullOrderPath + @"/" + "photoreport"))
                                {
                                    ProtoClasses.ProtoFtpSchedule.protoRow fil = new ProtoClasses.ProtoFtpSchedule.protoRow();
                                    FileInfo fi = new FileInfo(f);
                                    fil.Upload = false;
                                    fil.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                    fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                    fil.Length = fi.Length;
                                    fil.fileshortname = fi.Name;
                                    fil.targetfile = row.LocalPath + @"/" + "photoreport" + @"/" + fi.Name;
                                    fil.serveraddress = @"Server/" + orderPath + @"/" + "photoreport" + @"/" + fi.Name;
                                    fil.sourcefile = "makets" + @"/" + orderPath + @"/" + "photoreport" + @"/" + fi.Name;
                                    fil.LengthString = Utils.Converting.LengthToString(fi.Length);
                                    fil.order_id = row.uid;
                                    row.Photoreport.Add(fil);
                                }
                            }
                            else { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }
                            #endregion
                        }
                    }
                }

            }


            byte[] body = po.protoSerialize(protoOrder);
            byte[] head = new byte[2] { (int)SocketMessageCommand.DownloadOrderFiles, (int)MySql.Set.TableName.TableOrders };
            byte[] message = new byte[body.Length + head.Length];
            Buffer.BlockCopy(head, 0, message, 0, head.Length);
            Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
            Send(client, message);

        }
        #endregion

        #region


        #endregion

        #region Изменить статусы заданий
        private void OrderChangeStates(ClientInfo client)
        {
            switch (client.ByteMessage[1])
            {
                case (int)TableName.TableBase:
                    byte[] msg = new byte[client.ByteMessage.Length - 2];
                    Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
                    ProtoClasses.ProtoOrdersChangeState po = new ProtoClasses.ProtoOrdersChangeState();
                    ProtoClasses.ProtoOrdersChangeState.protoRowsList protoOrder = po.protoDeserialize(msg);
                    MySql.Orders orders = new MySql.Orders();
                    ProtoClasses.ProtoOrdersChangeState.protoRowsList result = orders.UpdateState(protoOrder, client.StrName);
                    if (result != null)
                    {
                        byte[] body = po.protoSerialize(result);
                        byte[] head = new byte[2] { (int)SocketMessageCommand.OrderChangeStates, (int)MySql.Set.TableName.TableOrders };
                        byte[] message = new byte[body.Length + head.Length];
                        Buffer.BlockCopy(head, 0, message, 0, head.Length);
                        Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                        SendToAll(message);
                    }
                    break;
            }
        }
        #endregion

        #region вставить новую строку заданий
        private void InsertRowHandler(ClientInfo client)
        {
            switch (client.ByteMessage[1])
            {
                case (int)TableName.TableBase:
                    byte[] msg = new byte[client.ByteMessage.Length - 2];
                    Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
                    ProtoClasses.ProtoOrders protoOrders = new ProtoClasses.ProtoOrders();
                    List<ProtoClasses.ProtoOrders.protoOrder> protoOrder = protoOrders.protoDeserialize(msg);
                    MySql.Orders orders = new MySql.Orders();
                    List<ProtoClasses.ProtoOrders.protoOrder> result = orders.InsertRow(protoOrder, client.StrName);
                    if (result != null)
                    {
                        byte[] body = protoOrders.protoSerialize(result);
                        byte[] head = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableOrders };
                        byte[] message = new byte[body.Length + head.Length];
                        Buffer.BlockCopy(head, 0, message, 0, head.Length);
                        Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                        SendToAll(message);
                    }
                    break;
            }

        }
        #endregion

        #region Изменить строку заданий
        private void UpdateRowHandler(ClientInfo client)
        {

            switch (client.ByteMessage[1])
            {
                case (int)TableName.TableBase:
                    byte[] msg = new byte[client.ByteMessage.Length - 2];
                    Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
                    ProtoClasses.ProtoOrders protoOrders = new ProtoClasses.ProtoOrders();
                    List<ProtoClasses.ProtoOrders.protoOrder> po = protoOrders.protoDeserialize(msg);
                    MySql.Orders orders = new MySql.Orders();
                    List<ProtoClasses.ProtoOrders.protoOrder> result = orders.UpdateRow(po, client.StrName);
                    if (result != null)
                    {
                        byte[] body = protoOrders.protoSerialize(result);
                        byte[] head = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableOrders };
                        byte[] message = new byte[body.Length + head.Length];
                        Buffer.BlockCopy(head, 0, message, 0, head.Length);
                        Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                        SendToAll(message);
                    }
                    break;
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
            SendToAll(message);

        }
        #endregion

        #region Клиент запросил список файлов превью
        private void getAllPreviewsList(ClientInfo client)
        {
            ProtoClasses.ProtoPreview po = new ProtoClasses.ProtoPreview();
            byte[] body = po.protoSerialize(MySql.DataTables.preview.Values.ToList());
            byte[] head = new byte[2] { (int)SocketMessageCommand.GetAllPreviewsList, 0 };
            byte[] message = new byte[body.Length + head.Length];
            Buffer.BlockCopy(head, 0, message, 0, head.Length);
            Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
            Send(client, message);
        }
        #endregion

        #region Клиент запросил список файлов заявки
        private void getFilesList(ClientInfo client)
        {
            //byte[] msg = new byte[client.ByteMessage.Length - 2];
            //Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);
            int orderid = BitConverter.ToInt32(client.ByteMessage, 2);
            List<AccentServer.ProtoClasses.ProtoFiles.protoRow> files = new List<AccentServer.ProtoClasses.ProtoFiles.protoRow>();
            if (orderid != 0)
            {
                //double dt1 = ((From t In itemTbl Where t.Id = < your_id > Select t).First())["Data1"];
                ProtoClasses.ProtoOrders.protoOrder rdr = null;
                if (MySql.DataTables.orders.ContainsKey(orderid)) { rdr = MySql.DataTables.orders[orderid]; }
                //DataRow rdr = MySql.DataTables.orders.Select("id = " + orderid).First();
                if (rdr != null)
                {
                        DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(rdr.time_recieve);
                        string yearpath = Properties.Settings.Default.FilePath + @"/" + tmpenddate.ToString("yyyy.MM");
                        string orderPath = Properties.Settings.Default.FilePath + @"/" + tmpenddate.ToString("yyyy.MM") + "/" + orderid.ToString("0000");
                        DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
                        if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                        if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }

                        if (Directory.Exists(orderPath + @"/" + "makets"))
                        {
                            foreach (string f in Directory.GetFiles(orderPath + @"/" + "makets"))
                            {
                                AccentServer.ProtoClasses.ProtoFiles.protoRow fil = new ProtoClasses.ProtoFiles.protoRow
                                {
                                    folder_flag = 0
                                };
                                FileInfo fi = new FileInfo(f);
                                fil.name = fi.Name;
                                fil.fullname = fi.FullName.Replace(di.FullName.ToString(), "");
                                fil.Extension = fi.Extension;
                                fil.Length = fi.Length;
                                fil.CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                fil.LastAccessTime = Utils.UnixDate.DateTimeToInt64(fi.LastAccessTime);
                                fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                //fil.folderfullname = fold.fullname;
                                //fil.foldername = fold.name;
                                fil.order_id = orderid;
                                fil.status = "На сервере";
                                fil.type = 1;
                                files.Add(fil);
                            }
                        }
                        else { Directory.CreateDirectory(orderPath + @"/" + "makets"); }

                        if (Directory.Exists(orderPath + @"/" + "preview"))
                        {
                            foreach (string f in Directory.GetFiles(orderPath + @"/" + "preview"))
                            {
                                AccentServer.ProtoClasses.ProtoFiles.protoRow fil = new ProtoClasses.ProtoFiles.protoRow
                                {
                                    folder_flag = 0
                                };
                                FileInfo fi = new FileInfo(f);
                                fil.name = fi.Name;
                                fil.fullname = fi.FullName.Replace(di.FullName.ToString(), "");
                                fil.Extension = fi.Extension;
                                fil.Length = fi.Length;
                                fil.CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                fil.LastAccessTime = Utils.UnixDate.DateTimeToInt64(fi.LastAccessTime);
                                fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                //fil.folderfullname = fold.fullname;
                                //fil.foldername = fold.name;
                                fil.order_id = orderid;
                                fil.type = 0;
                                fil.status = "На сервере";
                                files.Add(fil);
                            }
                        }
                        else { Directory.CreateDirectory(orderPath + @"/" + "preview"); }

                        if (Directory.Exists(orderPath + @"/" + "doc"))
                        {
                            foreach (string f in Directory.GetFiles(orderPath + @"/" + "doc"))
                            {
                                AccentServer.ProtoClasses.ProtoFiles.protoRow fil = new ProtoClasses.ProtoFiles.protoRow
                                {
                                    folder_flag = 0
                                };
                                FileInfo fi = new FileInfo(f);
                                fil.name = fi.Name;
                                fil.fullname = fi.FullName.Replace(di.FullName.ToString(), "");
                                fil.Extension = fi.Extension;
                                fil.Length = fi.Length;
                                fil.CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                fil.LastAccessTime = Utils.UnixDate.DateTimeToInt64(fi.LastAccessTime);
                                fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                //fil.folderfullname = fold.fullname;
                                //fil.foldername = fold.name;
                                fil.order_id = orderid;
                                fil.type = 2;
                                fil.status = "На сервере";
                                files.Add(fil);
                            }
                        }
                        else { Directory.CreateDirectory(orderPath + @"/" + "doc"); }

                        if (Directory.Exists(orderPath + @"/" + "photoreport"))
                        {
                            foreach (string f in Directory.GetFiles(orderPath + @"/" + "photoreport"))
                            {
                                AccentServer.ProtoClasses.ProtoFiles.protoRow fil = new ProtoClasses.ProtoFiles.protoRow
                                {
                                    folder_flag = 0
                                };
                                FileInfo fi = new FileInfo(f);
                                fil.name = fi.Name;
                                fil.fullname = fi.FullName.Replace(di.FullName.ToString(), "");
                                fil.Extension = fi.Extension;
                                fil.Length = fi.Length;
                                fil.CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                                fil.LastAccessTime = Utils.UnixDate.DateTimeToInt64(fi.LastAccessTime);
                                fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);
                                //fil.folderfullname = fold.fullname;
                                //fil.foldername = fold.name;
                                fil.order_id = orderid;
                                fil.type = 3;
                                fil.status = "На сервере";
                                files.Add(fil);
                            }
                        }
                        else { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }
                }
            }

            ProtoClasses.ProtoFiles po = new ProtoClasses.ProtoFiles();
            byte[] body = po.protoSerialize(files);
            byte[] oid = BitConverter.GetBytes(orderid);
            byte[] head = new byte[6] { (int)SocketMessageCommand.GetFilesList, 0, oid[0], oid[1], oid[2], oid[3] };
            byte[] message = new byte[body.Length + head.Length];
            Buffer.BlockCopy(head, 0, message, 0, head.Length);
            Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
            Send(client, message);
        }

        #endregion

        #region Клиент запросил изменения БД
        private void RowsChangeCounts(ClientInfo client)
        {
            byte[] msg = new byte[client.ByteMessage.Length - 2];
            Buffer.BlockCopy(client.ByteMessage, 2, msg, 0, msg.Length);

            switch (client.ByteMessage[1])
            {
                case (int)MySql.Set.TableName.TableOrders:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица заказов", true, SocketMessageCommand.None));
                    MySql.Orders mysql = new MySql.Orders();
                    ProtoClasses.ProtoCounters pc = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoOrders.protoOrder> ls = mysql.SelectLastChanges(pc.protoDeserialize(msg).plist, client);
                    ProtoClasses.ProtoOrders po = new ProtoClasses.ProtoOrders();
                    byte[] body = po.protoSerialize(ls);
                    byte[] head = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableOrders };
                    byte[] message = new byte[body.Length + head.Length];
                    Buffer.BlockCopy(head, 0, message, 0, head.Length);
                    Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                    Send(client, message);
                    break;

                case (int)MySql.Set.TableName.TableMaterialPrint:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица материалов печати", true, SocketMessageCommand.None));
                    MySql.Material mysqlPrint = new MySql.Material();
                    ProtoClasses.ProtoCounters pcPrint = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoMaterial.protoRow> lsPrint = mysqlPrint.SelectLastChanges(pcPrint.protoDeserialize(msg).plist, client, MySql.Set.TableName.TableMaterialPrint);
                    ProtoClasses.ProtoMaterial poPrint = new ProtoClasses.ProtoMaterial();
                    byte[] bodyPrint = poPrint.protoSerialize(lsPrint);
                    byte[] headPrint = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableMaterialPrint };
                    byte[] messagePrint = new byte[bodyPrint.Length + headPrint.Length];
                    Buffer.BlockCopy(headPrint, 0, messagePrint, 0, headPrint.Length);
                    Buffer.BlockCopy(bodyPrint, 0, messagePrint, headPrint.Length, bodyPrint.Length);
                    Send(client, messagePrint);
                    break;
                case (int)MySql.Set.TableName.TableMaterialCut:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица материалов плот.резки", true, SocketMessageCommand.None));
                    MySql.Material mysqlCut = new MySql.Material();
                    ProtoClasses.ProtoCounters pcCut = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoMaterial.protoRow> lsCut = mysqlCut.SelectLastChanges(pcCut.protoDeserialize(msg).plist, client, MySql.Set.TableName.TableMaterialCut);
                    ProtoClasses.ProtoMaterial poCut = new ProtoClasses.ProtoMaterial();
                    byte[] bodyCut = poCut.protoSerialize(lsCut);
                    byte[] headCut = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableMaterialCut };
                    byte[] messageCut = new byte[bodyCut.Length + headCut.Length];
                    Buffer.BlockCopy(headCut, 0, messageCut, 0, headCut.Length);
                    Buffer.BlockCopy(bodyCut, 0, messageCut, headCut.Length, bodyCut.Length);
                    Send(client, messageCut);
                    break;
                case (int)MySql.Set.TableName.TableMaterialCnc:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица материалов для фрезеровки", true, SocketMessageCommand.None));
                    MySql.Material mysqlCnc = new MySql.Material();
                    ProtoClasses.ProtoCounters pcCnc = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoMaterial.protoRow> lsCnc = mysqlCnc.SelectLastChanges(pcCnc.protoDeserialize(msg).plist, client, MySql.Set.TableName.TableMaterialCnc);
                    ProtoClasses.ProtoMaterial poCnc = new ProtoClasses.ProtoMaterial();
                    byte[] bodyCnc = poCnc.protoSerialize(lsCnc);
                    byte[] headCnc = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableMaterialCnc };
                    byte[] messageCnc = new byte[bodyCnc.Length + headCnc.Length];
                    Buffer.BlockCopy(headCnc, 0, messageCnc, 0, headCnc.Length);
                    Buffer.BlockCopy(bodyCnc, 0, messageCnc, headCnc.Length, bodyCnc.Length);
                    Send(client, messageCnc);
                    break;
                case (int)MySql.Set.TableName.TablePrinters:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица принтеров", true, SocketMessageCommand.None));
                    MySql.Equip mysqlPrinters = new MySql.Equip();
                    ProtoClasses.ProtoCounters pcPrinters = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoEquip.protoRow> lsPrinters = mysqlPrinters.SelectLastChanges(pcPrinters.protoDeserialize(msg).plist, client, MySql.Set.TableName.TablePrinters);
                    ProtoClasses.ProtoEquip poPrinters = new ProtoClasses.ProtoEquip();
                    byte[] bodyPrinters = poPrinters.protoSerialize(lsPrinters);
                    byte[] headPrinters = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TablePrinters };
                    byte[] messagePrinters = new byte[bodyPrinters.Length + headPrinters.Length];
                    Buffer.BlockCopy(headPrinters, 0, messagePrinters, 0, headPrinters.Length);
                    Buffer.BlockCopy(bodyPrinters, 0, messagePrinters, headPrinters.Length, bodyPrinters.Length);
                    Send(client, messagePrinters);
                    break;
                case (int)MySql.Set.TableName.TableCutters:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица каттеров", true, SocketMessageCommand.None));
                    MySql.Equip mysqlCutters = new MySql.Equip();
                    ProtoClasses.ProtoCounters pcCutters = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoEquip.protoRow> lsCutters = mysqlCutters.SelectLastChanges(pcCutters.protoDeserialize(msg).plist, client, MySql.Set.TableName.TableCutters);
                    ProtoClasses.ProtoEquip poCutters = new ProtoClasses.ProtoEquip();
                    byte[] bodyCutters = poCutters.protoSerialize(lsCutters);
                    byte[] headCutters = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableCutters };
                    byte[] messageCutters = new byte[bodyCutters.Length + headCutters.Length];
                    Buffer.BlockCopy(headCutters, 0, messageCutters, 0, headCutters.Length);
                    Buffer.BlockCopy(bodyCutters, 0, messageCutters, headCutters.Length, bodyCutters.Length);
                    Send(client, messageCutters);
                    break;
                case (int)MySql.Set.TableName.TableCncs:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица фрезеров", true, SocketMessageCommand.None));
                    MySql.Equip mysqlCncs = new MySql.Equip();
                    ProtoClasses.ProtoCounters pcCncs = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoEquip.protoRow> lsCncs = mysqlCncs.SelectLastChanges(pcCncs.protoDeserialize(msg).plist, client, MySql.Set.TableName.TableCncs);
                    ProtoClasses.ProtoEquip poCncs = new ProtoClasses.ProtoEquip();
                    byte[] bodyCncs = poCncs.protoSerialize(lsCncs);
                    byte[] headCncs = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableCncs };
                    byte[] messageCncs = new byte[bodyCncs.Length + headCncs.Length];
                    Buffer.BlockCopy(headCncs, 0, messageCncs, 0, headCncs.Length);
                    Buffer.BlockCopy(bodyCncs, 0, messageCncs, headCncs.Length, bodyCncs.Length);
                    Send(client, messageCncs);
                    break;
                case (int)MySql.Set.TableName.TableNoteStateChange:
                    OnConnect(new ConnectEventArgs(client.Socket, "Запрос последних изменений БД: Таблица истории заявок", true, SocketMessageCommand.None));
                    //MySql.OrderHistory mysqlohistory = new MySql.OrderHistory();
                    ProtoClasses.ProtoCounters pcohistory = new ProtoClasses.ProtoCounters();
                    List<ProtoClasses.ProtoOrderHistory.protoRow> lsohistory = MySql.OrderHistory.SelectLastChanges(pcohistory.protoDeserialize(msg).plist, client);
                    ProtoClasses.ProtoOrderHistory poohistory = new ProtoClasses.ProtoOrderHistory();
                    byte[] bodyohistory = poohistory.protoSerialize(lsohistory);
                    byte[] headohistory = new byte[2] { (int)SocketMessageCommand.RowsChangeCounts, (int)MySql.Set.TableName.TableNoteStateChange };
                    byte[] messageohistory = new byte[bodyohistory.Length + headohistory.Length];
                    Buffer.BlockCopy(headohistory, 0, messageohistory, 0, headohistory.Length);
                    Buffer.BlockCopy(bodyohistory, 0, messageohistory, headohistory.Length, bodyohistory.Length);
                    Send(client, messageohistory);
                    break;
            }

        }

        #endregion

        #region Послать клиенту
        internal void Send(ClientInfo client, byte[] msg)
        {
            try
            {
                byte[] encryptMessage = EncryptMessage(msg, client.Hash, client.Salt);
                client.Socket.BeginSend(encryptMessage, 0, encryptMessage.Length, SocketFlags.None, new AsyncCallback(OnSend), client.Socket);
            }
            catch
            {
                clientLose(client.Socket, "Потеря соединения!");
            }
        }

        #endregion
    }
}
