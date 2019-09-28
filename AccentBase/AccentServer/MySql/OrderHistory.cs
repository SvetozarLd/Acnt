using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Linq;
namespace AccentServer.MySql
{
    internal static class OrderHistory
    {
        //private static readonly Thread thread;
        private static readonly string ConnectionString = "";

        #region Начально - чтения БД в DataTable
        internal static Exception StartReadTables()
        {
            try
            {
                DataTables.old_order_history = new System.Collections.Concurrent.ConcurrentDictionary<long, ProtoClasses.ProtoOrderHistory.protoRow>();
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();                    
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM note_state_change", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            ProtoClasses.ProtoOrderHistory.protoRow row = new ProtoClasses.ProtoOrderHistory.protoRow();
                            row.id = Utils.CheckDBNull.ToLong(rdr["id"]);
                            row.work_id = Utils.CheckDBNull.ToInt32(rdr["work_id"]);
                            row.adder = Utils.CheckDBNull.ToString(rdr["adder"]);
                            row.status_task = Utils.CheckDBNull.ToInt32(rdr["status_task"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64((rdr["time_recieve"]));
                            row.date_change = Utils.UnixDate.CheckedDateTimeToInt64((rdr["date_change"]));

                            //if (DataTables.old_order_history == null)
                            //{
                            //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM note_state_change", connection))
                            //    {
                            //        DataTables.old_order_history = new DataTable();
                            //        adapter.AcceptChangesDuringFill = false;
                            //        adapter.Fill(DataTables.old_order_history);
                            //    }
                            DataTables.old_order_history.TryAdd(row.id, row);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        #endregion

        #region Получение от клиента списка последних изменений строк, отдача изменений.
        public static List<ProtoClasses.ProtoOrderHistory.protoRow> SelectLastChanges(Dictionary<int, long> pl, SocketServer.ClientInfo client)
        {
            bool breakclient = false;
            if (pl == null) { pl = new Dictionary<int, long>(); }

            List<ProtoClasses.ProtoOrderHistory.protoRow> outList = new List<ProtoClasses.ProtoOrderHistory.protoRow>();
            List<ProtoClasses.ProtoOrderHistory.protoRow> dic = DataTables.old_order_history.Values.ToList();
            //DataTable dt = DataTables.old_order_history;

            int rowsCount = dic.Count;
            int rowsCurrentIndex = 0;
            int percentCurrent = 0;
            int percentLast = 0;
            foreach (ProtoClasses.ProtoOrderHistory.protoRow rdr in dic)
            {


                //DateTime dttemp = rdr.change_count
                long servertimacount = rdr.change_count;
                int id = Convert.ToInt32(rdr.id);
                if (pl.TryGetValue(id, out long timecount))
                {
                    if (timecount < servertimacount)
                    {
                        ProtoClasses.ProtoOrderHistory.protoRow po = new ProtoClasses.ProtoOrderHistory.protoRow();
                        po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                        #region данные с БД в proto
                        po.id = rdr.id;
                        po.work_id = rdr.work_id;
                        po.adder = rdr.adder;
                        po.status_task = rdr.status_task;
                        po.note = rdr.note;
                        po.change_count = servertimacount;
                        po.time_recieve = rdr.time_recieve;
                        po.date_change = rdr.date_change;
                        #endregion
                        outList.Add(po);
                    }
                    pl.Remove(id);
                }
                else
                {
                    try
                    {
                        ProtoClasses.ProtoOrderHistory.protoRow po = new ProtoClasses.ProtoOrderHistory.protoRow();
                        po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert;
                        #region данные с БД в proto
                        po.id = rdr.id;
                        po.work_id = rdr.work_id;
                        po.adder = rdr.adder;
                        po.status_task = rdr.status_task;
                        po.note = rdr.note;
                        po.change_count = servertimacount;
                        po.time_recieve = rdr.time_recieve;
                        po.date_change = rdr.date_change;
                        #endregion
                        outList.Add(po);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                rowsCurrentIndex++;
                percentCurrent = Convert.ToInt32((rowsCurrentIndex * 100) / rowsCount);
                if (percentCurrent > percentLast)
                {
                    if (client.Socket != null)
                    {
                        if (!client.Socket.Connected)
                        {
                            breakclient = true; break;
                        }
                    }
                    else
                    {
                        breakclient = true; break;
                    }

                    percentLast = percentCurrent;
                    byte[] head = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsProcess, (int)Set.TableName.TableNoteStateChange };
                    byte[] body = BitConverter.GetBytes(percentLast);
                    byte[] message = new byte[head.Length + body.Length];
                    Buffer.BlockCopy(head, 0, message, 0, head.Length);
                    Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                    SocketServer.Servers.server.Send(client, message);
                }

            }
            if (breakclient) { return null; }
            if (pl.Count > 0)
            {
                foreach (int itmID in pl.Keys)
                {
                    ProtoClasses.ProtoOrderHistory.protoRow po = new ProtoClasses.ProtoOrderHistory.protoRow
                    {
                        id = itmID,
                        command = (int)SocketServer.TableServer.SocketMessageCommand.RowsDelete
                    };
                    outList.Add(po);
                }
            }

            byte[] head1 = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsSend, (int)Set.TableName.TableNoteStateChange };
            byte[] body1 = BitConverter.GetBytes(percentLast);
            byte[] message1 = new byte[head1.Length + body1.Length];
            Buffer.BlockCopy(head1, 0, message1, 0, head1.Length);
            Buffer.BlockCopy(body1, 0, message1, head1.Length, body1.Length);
            SocketServer.Servers.server.Send(client, message1);
            return outList;
        }

        #endregion


        #region INSERT
        public static List<ProtoClasses.ProtoOrderHistory.protoRow> InsertRow(List<ProtoClasses.ProtoOrderHistory.protoRow> uptab, long workid, string name)
        {
            if (uptab != null && uptab.Count > 0 && DataTables.old_order_history != null)
            {
                try
                {
                    DateTime dateTimeNow = DateTime.Now;
                    long change_count = Utils.UnixDate.DateTimeToInt64(DateTime.Now);
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (MySqlTransaction transaction = connection.BeginTransaction())
                        {
                            foreach (ProtoClasses.ProtoOrderHistory.protoRow item in uptab)
                            {
                                if (workid > 0) { item.work_id = workid; }
                                using (MySqlCommand command = new MySqlCommand(@"INSERT INTO note_state_change (
                            work_id, 
                            adder, 
                            status_task, 
                            note, 
                            date_change 
                            ) VALUES (
                            @work_id, 
                            @adder, 
                            @status_task, 
                            @note, 
                            @date_change 
                            )", connection))
                                {
                                    command.Transaction = transaction;
                                    command.Parameters.AddWithValue("@work_id", Utils.CheckDBNull.ToLong(item.work_id));
                                    command.Parameters.AddWithValue("@adder", Utils.CheckDBNull.ToString(name));
                                    command.Parameters.AddWithValue("@status_task", Utils.CheckDBNull.ToInt32(item.status_task));
                                    command.Parameters.AddWithValue("@note", Utils.CheckDBNull.ToString(item.note));
                                    command.Parameters.AddWithValue("@date_change", dateTimeNow);
                                    command.ExecuteNonQuery();
                                    ProtoClasses.ProtoOrderHistory.protoRow customerRow = new ProtoClasses.ProtoOrderHistory.protoRow();
                                    item.id = command.LastInsertedId;
                                    item.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert;
                                    item.change_count = change_count;
                                    item.time_recieve = change_count;
                                    item.date_change = change_count;
                                    item.adder = name;
                                    //DataRow customerRow = DataTables.old_order_history.NewRow();
                                    customerRow.id = item.id;
                                    customerRow.work_id = Utils.CheckDBNull.ToLong(item.work_id);
                                    customerRow.adder = Utils.CheckDBNull.ToString(name);
                                    customerRow.status_task = Utils.CheckDBNull.ToInt32(item.status_task);
                                    customerRow.note = Utils.CheckDBNull.ToString(item.note);
                                    customerRow.date_change = Utils.UnixDate.DateTimeToInt64(dateTimeNow);
                                    customerRow.time_recieve = Utils.UnixDate.DateTimeToInt64(dateTimeNow);
                                    if (DataTables.old_order_history.ContainsKey(customerRow.id)) { DataTables.old_order_history[customerRow.id] = customerRow; } else { DataTables.old_order_history.TryAdd(customerRow.id, customerRow); }
                                }
                            }
                            transaction.Commit();
                        }
                    }
                    return uptab;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion


    }
}
