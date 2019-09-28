using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace AccentBase.SqlLite
{
    class OrderHistory
    {
        static Thread thread;
        static internal DataTable TableOrderHistory { get; set; }
        static string path = Utils.Settings.set.data_path + @"\data";
        static string name = @"\history.dat";
        static internal bool breaking { get; set; }

        #region Создание базы
        static public void CreateTable()
        {


            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(path + name))
            {
                SQLiteConnection.CreateFile(path + name);

                using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
                {
                    connection.Open();
                    string commandstring = @"CREATE TABLE `order_history` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `adder`	TEXT,
	                                        `work_id`	INTEGER NOT NULL DEFAULT 0,
	                                        `status_task`	INTEGER NOT NULL DEFAULT 0,
	                                        `note`	TEXT,
                                        	`date_change`	INTEGER NOT NULL DEFAULT 0,
                                        	`change_count`	INTEGER NOT NULL DEFAULT 0,
	                                        `time_recieve`	INTEGER NOT NULL DEFAULT 0
                                            );";

                    using (SQLiteCommand command = new SQLiteCommand(commandstring, connection))
                    {
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch { }
                    }
                }
            }
        }
        #endregion

        #region SQLite -> datatable
        internal static void loadTable()
        {
            breaking = false;
            thread = new Thread(GetTable);
            thread.Name = "LoadOrderHistory";
            thread.IsBackground = true;
            string[] qqq = new string[2];
            qqq[0] = path;
            qqq[1] = name;
            thread.Start(qqq);
        }
        #endregion

        #region Начальная загрузка DataTables
        static private void GetTable(object ob)
        {
            string[] qqq = ob as string[];
            string fname = qqq[1];
            string fpath = qqq[0];
            if (!File.Exists(path + name)) { CreateTable(); }

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableNoteStateChange, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableOrderHistory == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM order_history", connection))
                    {
                        TableOrderHistory = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableOrderHistory);
                    }
                }
            }

            TableOrderHistory.Columns.Add("Datetime_date", typeof(DateTime));
            foreach (DataRow row in TableOrderHistory.Rows)
            {
                row["Datetime_date"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_change"])).ToString("dd.MM.yyyy HH:mm");
            }
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableNoteStateChange, true));
        }
        #endregion


        #region Обновление таблицы
        static internal void UpdateTable(List<ProtoClasses.ProtoOrderHistory.protoRow> uptab)
        {
            DataTable dt = TableOrderHistory;
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableNoteStateChange, true));
            if (dt != null)
            {
                int size = 0;
                int procentBefore = 0;
                int procentCurrent = 0;
                int counter = 0;
                int counter2 = 0;

                if (uptab != null && uptab.Count > 0)
                {
                    size = uptab.Count;
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                        {                                                                                                                   //try
                                                                                                                                            //{
                            connection.Open();
                            SQLiteTransaction transaction = connection.BeginTransaction();
                            foreach (ProtoClasses.ProtoOrderHistory.protoRow item in uptab)
                            {
                                counter++;
                                if (counter2 == 200) { transaction = connection.BeginTransaction(); counter2 = 0; } //IsolationLevel.Snapshot, false
                                counter2++;
                                DataRow customerRow = dt.Select("id = " + item.id).FirstOrDefault();
                                if (customerRow != null)
                                {
                                    if (item.command == (int)SocketClient.TableClient.SocketMessageCommand.RowsChangeState)
                                    {
                                        //using (SQLiteCommand command = new SQLiteCommand("UPDATE `customers`  SET `folder`= " + item.folder + ", `change_date` = @Change_date  WHERE `id` = " + item.id + " ;", connection))
                                        //{
                                        //    command.Transaction = transaction;
                                        //    command.Parameters.Add("@Change_date", DbType.Int64).Value = item.change_count;
                                        //    command.ExecuteNonQuery();
                                        //    customerRow["folder"] = item.folder;
                                        //    customerRow["dlt"] = item.delete;
                                        //}
                                    }
                                    else
                                    {
                                        if (item.command == (int)SocketClient.TableClient.SocketMessageCommand.RowsDelete)
                                        {
                                            //using (SQLiteCommand command = new SQLiteCommand("UPDATE `customers`  SET `dlt`= 1, `change_date` = @Change_date, `folder`= 2 WHERE `id` = " + item.id + " ;", connection))
                                            //{
                                            //    command.Transaction = transaction;
                                            //    command.Parameters.Add("@Change_date", DbType.Int64).Value = item.change_count;
                                            //    //command.Parameters.Add("@Folder", DbType.Int32).Value = item.folder;
                                            //    command.ExecuteNonQuery();
                                            //    customerRow["dlt"] = 1;
                                            //    customerRow["folder"] = 2;
                                            //}
                                        }
                                        else
                                        {

                                            #region Update
                                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `order_history`  SET 
`work_id` = @work_id, 
`status_task` = @status_task, 
`adder` = @adder, 
`date_change` = @date_change, 
`note` = @note, 
`change_count` = @change_count, 
`time_recieve` = @time_recieve 
WHERE `id` = @id ; ", connection))
                                            {
                                                command.Transaction = transaction;
                                                command.Parameters.Add("@id", DbType.Int64).Value = customerRow["id"] = Utils.CheckDBNull.ToLong(item.id);
                                                command.Parameters.Add("@work_id", DbType.Int64).Value = customerRow["work_id"] = Utils.CheckDBNull.ToLong(item.work_id);
                                                command.Parameters.Add("@status_task", DbType.Int16).Value = customerRow["status_task"] = Utils.CheckDBNull.ToInt(item.status_task);
                                                command.Parameters.Add("@adder", DbType.String).Value = customerRow["adder"] = Utils.CheckDBNull.ToString(item.adder);
                                                command.Parameters.Add("@date_change", DbType.Int64).Value = customerRow["date_change"] = Utils.CheckDBNull.ToLong(item.date_change);
                                                command.Parameters.Add("@note", DbType.String).Value = customerRow["note"] = Utils.CheckDBNull.ToString(item.note);
                                                command.Parameters.Add("@change_count", DbType.Int64).Value = customerRow["change_count"] = Utils.CheckDBNull.ToLong(item.change_count);
                                                command.Parameters.Add("@time_recieve", DbType.Int64).Value = customerRow["time_recieve"] = Utils.CheckDBNull.ToLong(item.time_recieve);
                                                command.ExecuteNonQuery();
                                                customerRow["Datetime_date"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.date_change)).ToString("dd.MM.yyyy HH:mm");
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        #region Insert
                                            using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO order_history (
id,
work_id, 
status_task, 
adder, 
date_change, 
note, 
change_count, 
time_recieve
) VALUES(
@id, 
@work_id, 
@status_task, 
@adder, 
@date_change, 
@note, 
@change_count, 
@time_recieve
); ", connection))
                                            {
                                                command.Transaction = transaction;
                                                DataRow row = dt.NewRow();
                                                command.Parameters.Add("@id", DbType.Int64).Value = row["id"] = Utils.CheckDBNull.ToLong(item.id);
                                                command.Parameters.Add("@work_id", DbType.Int64).Value = row["work_id"] = Utils.CheckDBNull.ToLong(item.work_id);
                                                command.Parameters.Add("@status_task", DbType.Int16).Value = row["status_task"] = Utils.CheckDBNull.ToInt(item.status_task);
                                                command.Parameters.Add("@adder", DbType.String).Value = row["adder"] = Utils.CheckDBNull.ToString(item.adder);
                                                command.Parameters.Add("@date_change", DbType.Int64).Value = row["date_change"] = Utils.CheckDBNull.ToLong(item.date_change);
                                                command.Parameters.Add("@note", DbType.String).Value = row["note"] = Utils.CheckDBNull.ToString(item.note);
                                                command.Parameters.Add("@change_count", DbType.Int64).Value = row["change_count"] = Utils.CheckDBNull.ToLong(item.change_count);
                                                command.Parameters.Add("@time_recieve", DbType.Int64).Value = row["time_recieve"] = Utils.CheckDBNull.ToLong(item.time_recieve);
                                                command.ExecuteNonQuery();
                                                row["Datetime_date"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.date_change)).ToString("dd.MM.yyyy HH:mm");
                                                dt.Rows.Add(row);

                                            }
                                        #endregion
                                    }catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, "SqlLite.OrderHistory.UpdateTable.Insert");
                                    }
                                }
                                if (counter2 == 200) {
                                    transaction.Commit();
                                }

                                if (breaking) { break; }
                                procentCurrent = Convert.ToInt32((counter * 100) / size);
                                if (procentCurrent > procentBefore)
                                {
                                    procentBefore = procentCurrent; if (procentBefore > 100) { procentBefore = 100; }
                                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(procentBefore, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsProcess, (int)SqlEvent.TableName.TableNoteStateChange, true));
                                }
                            }
                            if (counter2 != 200) { transaction.Commit(); }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "SqlLite.OrderHistory.UpdateTable");
                    }
                }
            }
            else
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)SqlEvent.TableName.TableNoteStateChange, true));
            }

            ////DataRow customerRow456 = CustomersArea.SqliteCustomers.TableCustomers.Select("id = " + 15).FirstOrDefault();
            if (!breaking)
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)SqlEvent.TableName.TableNoteStateChange, true));
            }


        }

        #endregion


    }
}
