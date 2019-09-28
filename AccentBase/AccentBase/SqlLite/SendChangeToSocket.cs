using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading;
//using System.Windows.Forms;
namespace AccentBase.SqlLite
{
    internal static class SendChangeToSocket
    {

        static SendChangeToSocket()
        {
            AccentBase.Program.OrderSendEvent += Program_OrderSendEvent;
            AccentBase.Program.OrderRecieveEvent += Program_OrderRecieveEvent;
            SocketClient.TableClient.SocketClientEvent += TableClient_SocketClientEvent;
            //AccentBase.Program.Event_OrderSatusChange += Program_Event_OrderSatusChange;
        }

        //private static void Program_Event_OrderSatusChange(object sender, Program.OrderSatusChangeEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
        #region Событие сервера
        //internal delegate void SocketDelegate(SocketClient.TableClient.ConnectEventArgs e);
        //private 
        //static bool sendingFalse = false;
        private static void TableClient_SocketClientEvent(object sender, SocketClient.TableClient.ConnectEventArgs e)
        {
            //if (e != null)
            //{
            //    if (e.Status)
            //    {
            //        //sendingFalse = false;
            //    }
            //    else
            //    {
            //        foreach (DataRow dr in TableOrdersSend.Rows)
            //        {
            //            dr["sending"] = false;
            //        }
            //        //sendingFalse = true;
            //    }
            //}
        }
        //private static void ServerHandler(SocketClient.TableClient.ConnectEventArgs e)
        //{
        //    if (e.Status)
        //    {
        //        //connected = true;
        //        //getFilesList();
        //    }
        //    else
        //    {
        //        //connected = false;
        //        //getFilesList();
        //    }
        //}
        #endregion


        private static void Program_OrderRecieveEvent(object sender, ProtoClasses.ProtoOrders.protoOrder e)
        {
            if (e != null)
            {
                if (SendChangeToSocket.IDsHashset.Contains(e.sender_row_stringid)) { DeletedRow(e); }

            }
        }

        //internal delegate void SqliteDelegate(Program.OrderSendEventArgs e);
        private static void Program_OrderSendEvent(object sender, Program.OrderSendEventArgs e)
        {
            if (e != null) { Insert(e); }
            //try
            //{
            //    if (e != null) { BeginInvoke(new SqliteDelegate(Insert), e); }
            //}
            //catch (Exception ex)
            //{

            //}
        }


        //private static readonly Thread thread;
        internal static DataTable TableOrdersSend { get; set; }
        public static HashSet<string> IDsHashset { get; set; }
        //static List<ProtoClasses.ProtoOrders.protoOrder> orders { get; set; }
        private static readonly string path = Utils.Settings.set.data_path + @"\data";
        private static readonly string name = @"\orderssend.dat";
        internal static bool breaking { get; set; }
        #region Создание базы
        public static void CreateTable()
        {


            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(path + name))
            {
                SQLiteConnection.CreateFile(path + name);

                using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
                {
                    connection.Open();
                    string commandstring = @"CREATE TABLE `orders_send` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `uin`	TEXT,
                                        	`order_binary`	BLOB,
                                        	`name`	TEXT,
                                        	`notes`	TEXT,
                                            `order_command`	INTEGER NOT NULL DEFAULT 0,
                                            `table_enum`	INTEGER NOT NULL DEFAULT 0,
                                            `date_insert`	INTEGER NOT NULL DEFAULT 0
                                            );";

                    using (SQLiteCommand command = new SQLiteCommand(commandstring, connection))
                    {
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch(Exception ex) { MessageBox.Show("SendChangeToSocket:CreateTable" + Environment.NewLine +ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error); }
                    }
                }
            }
        }
        #endregion

        //#region SQLite -> datatable
        //internal static void loadTable()
        //{
        //    breaking = false;
        //    thread = new Thread(GetTable);
        //    thread.Name = "LoadOrderHistory";
        //    thread.IsBackground = true;
        //    string[] qqq = new string[2];
        //    qqq[0] = path;
        //    qqq[1] = name;
        //    thread.Start(qqq);
        //}
        //#endregion

        #region Начальная загрузка DataTables
        public static void GetTable()
        {
            //string[] qqq = ob as string[];
            //string fname = qqq[1];
            //string fpath = qqq[0];
            if (!File.Exists(path + name)) { CreateTable(); }
            IDsHashset = new HashSet<string>();
            //orders = new List<ProtoClasses.ProtoOrders.protoOrder>();
            //SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableNoteStateChange, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
            {
                connection.Open();
                if (TableOrdersSend == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM orders_send", connection))
                    {
                        TableOrdersSend = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableOrdersSend);
                    }
                    TableOrdersSend.Columns.Add("dt_date_insert", typeof(DateTime));
                    TableOrdersSend.Columns.Add("sending", typeof(bool));
                    TableOrdersSend.Columns["sending"].DefaultValue = false;
                    foreach (DataRow row in TableOrdersSend.Rows)
                    {
                        row["dt_date_insert"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_insert"])).ToString("dd.MM.yyyy HH:mm");
                        row["sending"] = false;
                        string tmp = Utils.CheckDBNull.ToString(row["uin"]);
                        if (!IDsHashset.Contains(tmp))
                        {
                            IDsHashset.Add(tmp);
                        }


                    }
                }
            }

            //TableOrderHistory.Columns.Add("Datetime_date", typeof(DateTime));
            //foreach (DataRow row in TableOrderHistory.Rows)
            //{
            //    row["Datetime_date"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_change"])).ToString("dd.MM.yyyy HH:mm");
            //}
            //SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableNoteStateChange, true));
        }
        #endregion


        static Thread thread;

        static public void Insert(Program.OrderSendEventArgs e)
        {
                thread = new Thread(backgroundInsert);
                thread.IsBackground = true;
                thread.Name = "AccentBase FTP Daemon";
                thread.Start(e);
        }


        #region Insert
        public static void backgroundInsert(object ob)
        {
        Program.OrderSendEventArgs e = ob as Program.OrderSendEventArgs;
            if (e.Order != null)
            {
                ProtoClasses.ProtoOrders.protoOrder item = e.Order;
                SocketClient.TableClient.SocketMessageCommand socketMessageCommand = e.Command;
                SocketClient.TableClient.TableName tableName = e.TableName;
                string notes = e.Notes;
                if (!File.Exists(path + name)) { CreateTable(); }
                DateTime dateTime = DateTime.Now;
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {
                        connection.Open();

                        using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO orders_send (uin, name, notes, order_command, table_enum, date_insert)
                                                                                            VALUES(@uin, @name, @notes, @order_command, @table_enum, @date_insert); ", connection))
                        {
                            command.Parameters.Add("@uin", DbType.String).Value = Utils.CheckDBNull.ToString(item.sender_row_stringid);
                            command.Parameters.Add("@name", DbType.String).Value = Utils.CheckDBNull.ToString(item.work_name);
                            command.Parameters.Add("@notes", DbType.String).Value = Utils.CheckDBNull.ToString(notes);
                            command.Parameters.Add("@order_command", DbType.Int32).Value = (int)socketMessageCommand;
                            command.Parameters.Add("@table_enum", DbType.Int32).Value = (int)tableName;
                            command.Parameters.Add("@date_insert", DbType.Int64).Value = Utils.UnixDate.DateTimeToInt64(dateTime);
                            command.ExecuteNonQuery();
                            item.sender_row_id = Convert.ToInt64(connection.LastInsertRowId);
                        }
                        if (!IDsHashset.Contains(item.sender_row_stringid))
                        {
                            IDsHashset.Add(item.sender_row_stringid);
                        }
                        ProtoClasses.ProtoOrders protoOrders = new ProtoClasses.ProtoOrders();
                        byte[] byteOrders = protoOrders.protoSerialize(new List<ProtoClasses.ProtoOrders.protoOrder> { item });

                        using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders_send`  SET `order_binary` = @order_binary WHERE `id` = @id ; ", connection))
                        {

                            command.Parameters.Add("@id", DbType.Int64).Value = Utils.CheckDBNull.ToLong(item.sender_row_id);
                            command.Parameters.Add("@order_binary", DbType.Binary, byteOrders.Length).Value = byteOrders;
                            command.ExecuteNonQuery();
                            DataRow dr = TableOrdersSend.NewRow();
                            dr["id"] = item.sender_row_id;
                            dr["order_binary"] = byteOrders;
                            dr["uin"] = item.sender_row_stringid;
                            dr["name"] = Utils.CheckDBNull.ToString(item.work_name);
                            dr["notes"] = Utils.CheckDBNull.ToString(notes);
                            dr["order_command"] = (int)socketMessageCommand;
                            dr["table_enum"] = (int)tableName;
                            dr["date_insert"] = Utils.UnixDate.DateTimeToInt64(dateTime);
                            dr["dt_date_insert"] = dateTime;
                            if (SocketClient.TableClient.IsConnected && SocketClient.TableClient.StartedEventsEnded)
                            {
                                dr["sending"] = true;
                                TableOrdersSend.Rows.Add(dr);
                                Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(item.sender_row_id, item.work_name, notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                                byte[] head = new byte[2] { Convert.ToByte((int)socketMessageCommand), Convert.ToByte((int)tableName) };
                                byte[] message = new byte[head.Length + byteOrders.Length];
                                Buffer.BlockCopy(head, 0, message, 0, head.Length);
                                Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                                SocketClient.TableClient.SendToServer(message);
                            }
                            else
                            {
                                dr["sending"] = false;
                                TableOrdersSend.Rows.Add(dr);
                                Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(item.sender_row_id, item.work_name, notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                            }
                        }
                    }

                    //return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SendChangeToSocket:Insert"+Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (e.OrdersChangeState != null)
                {
                    ProtoClasses.ProtoOrdersChangeState.protoRowsList ordersChangeStateList = e.OrdersChangeState;
                    SocketClient.TableClient.SocketMessageCommand socketMessageCommand = e.Command;
                    SocketClient.TableClient.TableName tableName = e.TableName;
                    ordersChangeStateList.notes = e.Notes;
                    if (!File.Exists(path + name)) { CreateTable(); }
                    DateTime dateTime = DateTime.Now;
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                        {
                            connection.Open();

                            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO orders_send (uin, name, notes, order_command, table_enum, date_insert)
                                                                                            VALUES(@uin, @name, @notes, @order_command, @table_enum, @date_insert); ", connection))
                            {
                                command.Parameters.Add("@uin", DbType.String).Value = Utils.CheckDBNull.ToString(ordersChangeStateList.sender_row_stringid);
                                command.Parameters.Add("@name", DbType.String).Value = Utils.CheckDBNull.ToString(ordersChangeStateList.name);
                                command.Parameters.Add("@notes", DbType.String).Value = Utils.CheckDBNull.ToString(e.Notes);
                                command.Parameters.Add("@order_command", DbType.Int32).Value = (int)socketMessageCommand;
                                command.Parameters.Add("@table_enum", DbType.Int32).Value = (int)tableName;
                                command.Parameters.Add("@date_insert", DbType.Int64).Value = Utils.UnixDate.DateTimeToInt64(dateTime);
                                command.ExecuteNonQuery();
                                ordersChangeStateList.sender_row_id = Convert.ToInt64(connection.LastInsertRowId);
                                //ordersChangeStateList.sender_row_stringid = e.Sender_row_stringid;
                                ordersChangeStateList.notes = e.Notes;
                                ordersChangeStateList.adder = Utils.Settings.set.name;
                                //ordersChangeStateList.name = e.WorkName;
                                //    foreach (ProtoClasses.ProtoOrdersChangeState.protoRow row in ordersChangeStateRows)
                                //    {
                                //        row.sender_row_id = LastInsertRowId;
                                //        row.sender_row_stringid = Sender_row_stringid;
                                //    }

                            }
                            if (!IDsHashset.Contains(ordersChangeStateList.sender_row_stringid)) { IDsHashset.Add(ordersChangeStateList.sender_row_stringid); }
                            ProtoClasses.ProtoOrdersChangeState protoOrders = new ProtoClasses.ProtoOrdersChangeState();
                            byte[] byteOrders = protoOrders.protoSerialize(ordersChangeStateList);

                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders_send`  SET `order_binary` = @order_binary WHERE `id` = @id ; ", connection))
                            {

                                command.Parameters.Add("@id", DbType.Int64).Value = Utils.CheckDBNull.ToLong(ordersChangeStateList.sender_row_id);
                                command.Parameters.Add("@order_binary", DbType.Binary, byteOrders.Length).Value = byteOrders;
                                command.ExecuteNonQuery();
                                DataRow dr = TableOrdersSend.NewRow();
                                dr["id"] = ordersChangeStateList.sender_row_id;
                                dr["order_binary"] = byteOrders;
                                dr["uin"] = ordersChangeStateList.sender_row_stringid;
                                dr["name"] = Utils.CheckDBNull.ToString(ordersChangeStateList.name);
                                dr["notes"] = Utils.CheckDBNull.ToString(ordersChangeStateList.notes);
                                dr["order_command"] = (int)socketMessageCommand;
                                dr["table_enum"] = (int)tableName;
                                dr["date_insert"] = Utils.UnixDate.DateTimeToInt64(dateTime);
                                dr["dt_date_insert"] = dateTime;
                                if (SocketClient.TableClient.IsConnected && SocketClient.TableClient.StartedEventsEnded)
                                {
                                    dr["sending"] = true;
                                    TableOrdersSend.Rows.Add(dr);
                                    Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(ordersChangeStateList.sender_row_id, ordersChangeStateList.name, ordersChangeStateList.notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                                    byte[] head = new byte[2] { Convert.ToByte((int)socketMessageCommand), Convert.ToByte((int)tableName) };
                                    byte[] message = new byte[head.Length + byteOrders.Length];
                                    Buffer.BlockCopy(head, 0, message, 0, head.Length);
                                    Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                                    SocketClient.TableClient.SendToServer(message);
                                }
                                else
                                {
                                    dr["sending"] = false;
                                    TableOrdersSend.Rows.Add(dr);
                                    Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(ordersChangeStateList.sender_row_id, ordersChangeStateList.name, ordersChangeStateList.notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                                }
                            }
                        }

                        //return null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SendChangeToSocket:Insert:e.OrdersChangeState" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (e.DownloadOrdersFiles != null)
                    {
                        ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList downloadOrdersFiles = e.DownloadOrdersFiles;
                        SocketClient.TableClient.SocketMessageCommand socketMessageCommand = e.Command;
                        SocketClient.TableClient.TableName tableName = e.TableName;
                        if (!File.Exists(path + name)) { CreateTable(); }
                        DateTime dateTime = DateTime.Now;
                        try
                        {
                            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                            {
                                connection.Open();

                                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO orders_send (uin, name, notes, order_command, table_enum, date_insert)
                                                                                            VALUES(@uin, @name, @notes, @order_command, @table_enum, @date_insert); ", connection))
                                {
                                    command.Parameters.Add("@uin", DbType.String).Value = Utils.CheckDBNull.ToString(downloadOrdersFiles.sender_row_stringid);
                                    command.Parameters.Add("@name", DbType.String).Value = Utils.CheckDBNull.ToString(downloadOrdersFiles.name);
                                    command.Parameters.Add("@notes", DbType.String).Value = Utils.CheckDBNull.ToString(e.Notes);
                                    command.Parameters.Add("@order_command", DbType.Int32).Value = (int)socketMessageCommand;
                                    command.Parameters.Add("@table_enum", DbType.Int32).Value = (int)tableName;
                                    command.Parameters.Add("@date_insert", DbType.Int64).Value = Utils.UnixDate.DateTimeToInt64(dateTime);
                                    command.ExecuteNonQuery();
                                    downloadOrdersFiles.sender_row_id = Convert.ToInt64(connection.LastInsertRowId);
                                    //ordersChangeStateList.name = e.WorkName;
                                    //    foreach (ProtoClasses.ProtoOrdersChangeState.protoRow row in ordersChangeStateRows)
                                    //    {
                                    //        row.sender_row_id = LastInsertRowId;
                                    //        row.sender_row_stringid = Sender_row_stringid;
                                    //    }

                                }
                                if (!IDsHashset.Contains(downloadOrdersFiles.sender_row_stringid)) { IDsHashset.Add(downloadOrdersFiles.sender_row_stringid); }
                                ProtoClasses.ProtoDownloadOrdersFiles protoOrders = new ProtoClasses.ProtoDownloadOrdersFiles();
                                byte[] byteOrders = protoOrders.protoSerialize(downloadOrdersFiles);

                                using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders_send`  SET `order_binary` = @order_binary WHERE `id` = @id ; ", connection))
                                {

                                    command.Parameters.Add("@id", DbType.Int64).Value = Utils.CheckDBNull.ToLong(downloadOrdersFiles.sender_row_id);
                                    command.Parameters.Add("@order_binary", DbType.Binary, byteOrders.Length).Value = byteOrders;
                                    command.ExecuteNonQuery();
                                    DataRow dr = TableOrdersSend.NewRow();
                                    dr["id"] = downloadOrdersFiles.sender_row_id;
                                    dr["order_binary"] = byteOrders;
                                    dr["uin"] = downloadOrdersFiles.sender_row_stringid;
                                    dr["name"] = Utils.CheckDBNull.ToString(downloadOrdersFiles.name);
                                    dr["notes"] = Utils.CheckDBNull.ToString(e.Notes);
                                    dr["order_command"] = (int)socketMessageCommand;
                                    dr["table_enum"] = (int)tableName;
                                    dr["date_insert"] = Utils.UnixDate.DateTimeToInt64(dateTime);
                                    dr["dt_date_insert"] = dateTime;
                                    if (SocketClient.TableClient.IsConnected && SocketClient.TableClient.StartedEventsEnded)
                                    {
                                        dr["sending"] = true;
                                        TableOrdersSend.Rows.Add(dr);
                                        Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(downloadOrdersFiles.sender_row_id, downloadOrdersFiles.name, e.Notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                                        byte[] head = new byte[2] { Convert.ToByte((int)socketMessageCommand), Convert.ToByte((int)tableName) };
                                        byte[] message = new byte[head.Length + byteOrders.Length];
                                        Buffer.BlockCopy(head, 0, message, 0, head.Length);
                                        Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                                        SocketClient.TableClient.SendToServer(message);
                                    }
                                    else
                                    {
                                        dr["sending"] = false;
                                        TableOrdersSend.Rows.Add(dr);
                                        Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(downloadOrdersFiles.sender_row_id, downloadOrdersFiles.name, e.Notes, dateTime, (int)socketMessageCommand, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("SendChangeToSocket:Insert:e.DownloadOrdersFiles" + Environment.NewLine+ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }


                    }
                }
            }
        }
        #endregion


        #region Проверка - удаление строк
        public static void DeletedRow(ProtoClasses.ProtoOrders.protoOrder item)
        {
            DataRow dataRow = TableOrdersSend.Select("id =" + item.sender_row_id).SingleOrDefault();
            if (dataRow != null)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {
                        connection.Open();
                        using (SQLiteCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = string.Format("DELETE FROM orders_send WHERE id={0}", item.sender_row_id);

                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    TableOrdersSend.Rows.Remove(dataRow);
                    if (item.FilesUpload != null)
                    {
                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow row in item.FilesUpload)
                        {
                            if (row != null)
                            {
                                SqlLite.FtpSchedule.Insert(row);
                            }

                        }
                    }
                    Program.SendChangeToSocketEvent(new Program.ConnectEventArgs(item.sender_row_id, string.Empty, string.Empty, DateTime.Now, 0, null, SocketClient.TableClient.SocketMessageCommand.RowsDelete));

                }
                catch (Exception ex)
                { }
            }
        }

        #endregion
        #region Отправка всего, после всех процедур подключения
        public static void SendAll()
        {
            try
            {
                if (TableOrdersSend != null && TableOrdersSend.Rows != null && TableOrdersSend.Rows.Count > 0)
                {
                    foreach (DataRow item in TableOrdersSend.Rows)
                    {
                        //byte[] byteOrders = null;
                        //byte[] head = new byte[2] { Convert.ToByte((int)item["order_command"]), Convert.ToByte((int)item["table_enum"]) };


                        byte[] head = new byte[2] { Convert.ToByte(item["order_command"]), Convert.ToByte(item["table_enum"]) };
                        //try
                        //{
                        byte[] byteOrders = (byte[])item["order_binary"];
                        //}catch(Exception ex)
                        //{

                        //}
                        if (byteOrders != null && byteOrders.Length > 0)
                        {
                            byte[] message = new byte[head.Length + byteOrders.Length];
                            Buffer.BlockCopy(head, 0, message, 0, head.Length);
                            Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                            SocketClient.TableClient.SendToServer(message);
                        }
                        else
                        {
                            byteOrders = BitConverter.GetBytes((long)item["order_command"]);
                            byte[] message = new byte[head.Length + byteOrders.Length];
                            Buffer.BlockCopy(head, 0, message, 0, head.Length);
                            Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                            SocketClient.TableClient.SendToServer(message);
                        }
                        //byte[] head = new byte[2] { Convert.ToByte((int)item.command), Convert.ToByte((int)SocketClient.TableClient.TableName.TableBase) };
                        //byte[] message = new byte[head.Length + byteOrders.Length];
                        //Buffer.BlockCopy(head, 0, message, 0, head.Length);
                        //Buffer.BlockCopy(byteOrders, 0, message, head.Length, byteOrders.Length);
                        //SocketClient.TableClient.SendToServer(message);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        #endregion
        //#region Update
        //public static void UpdateOfInsert(ProtoClasses.ProtoOrders.protoOrder item)
        //{
        //    if (!File.Exists(path + name)) { CreateTable(); }
        //    try
        //    {

        //        ProtoClasses.ProtoOrders protoOrders = new ProtoClasses.ProtoOrders();
        //        List<ProtoClasses.ProtoOrders.protoOrder> poi = new List<ProtoClasses.ProtoOrders.protoOrder>
        //        {
        //            item
        //        };
        //        byte[] byteOrders = protoOrders.protoSerialize(poi);
        //        using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
        //        {
        //            connection.Open();
        //            using (SQLiteTransaction transaction = connection.BeginTransaction())
        //            {
        //                using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders_send`  SET 
        //                `order_binary` = @order_binary, 
        //                WHERE `id` = @id ; ", connection))
        //                {
        //                    DataRow dr = TableOrdersSend.NewRow();
        //                    command.Transaction = transaction;
        //                    command.Parameters.Add("@id", DbType.Int64).Value = dr["id"] = Utils.CheckDBNull.ToLong(item.sender_row_id);
        //                    command.Parameters.Add("@order_binary", DbType.Object, byteOrders.Length).Value = dr["order_binary"] = byteOrders;
        //                    dr["uin"] = item.sender_row_stringid;
        //                    dr["name"] = item.work_name;
        //                    dr["notes"] = notes;
        //                    TableOrdersSend.Rows.Add(dr);
        //                }
        //                transaction.Commit();

        //            }
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        //#endregion



    }
}
