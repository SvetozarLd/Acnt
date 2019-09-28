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
    static internal class Equip
    {
        static Thread thread;
        static internal DataTable TablePrinters { get; set; }
        static internal DataTable TableCutters { get; set; }
        static internal DataTable TableCncs { get; set; }
        public static ConcurrentDictionary<int, string> DicPrinters { get; set; }
        public static ConcurrentDictionary<int, string> DicCuters { get; set; }
        public static ConcurrentDictionary<int, string> DicCncs { get; set; }
        static string path = Utils.Settings.set.data_path + @"\data";
        static string name = @"\equip.dat";
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
                    string commandstring = @"CREATE TABLE `printers` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `name`	TEXT,
	                                        `price`	INTEGER NOT NULL DEFAULT 0,
	                                        `t_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `note`	TEXT,
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
                    commandstring = @"CREATE TABLE `cutters` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `name`	TEXT,
	                                        `price`	INTEGER NOT NULL DEFAULT 0,
	                                        `t_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `note`	TEXT,
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
                    commandstring = @"CREATE TABLE `cncs` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `name`	TEXT,
	                                        `price`	INTEGER NOT NULL DEFAULT 0,
	                                        `t_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `note`	TEXT,
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
            thread.Name = "LoadEquip";
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

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TablePrinters, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TablePrinters == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM printers", connection))
                    {
                        TablePrinters = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TablePrinters);
                    }
                }
            }
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TablePrinters, true));
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableCutters, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableCutters == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM cutters", connection))
                    {
                        TableCutters = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableCutters);
                    }
                }
            }
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableCutters, true));
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableCncs, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableCncs == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM cncs", connection))
                    {
                        TableCncs = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableCncs);
                    }
                }
            }
            DicPrinters = new ConcurrentDictionary<int, string>(TablePrinters.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));
            DicCuters = new ConcurrentDictionary<int, string>(TableCutters.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));
            DicCncs = new ConcurrentDictionary<int, string>(TableCncs.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableCncs, true));
        }
        #endregion


        #region Обновление таблицы
        static internal void UpdateTable(List<ProtoClasses.ProtoEquip.protoRow> uptab, SqlEvent.TableName tn)
        {
            string tablebaseName = string.Empty;
            DataTable dt = null;
            switch (tn)
            {
                case SqlEvent.TableName.TablePrinters:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TablePrinters, true));
                    tablebaseName = "printers";
                    dt = TablePrinters;
                    break;
                case SqlEvent.TableName.TableCutters:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableMaterialCut, true));
                    tablebaseName = "cutters";
                    dt = TableCutters;
                    break;
                case SqlEvent.TableName.TableCncs:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableMaterialCnc, true));
                    tablebaseName = "cncs";
                    dt = TableCncs;
                    break;
            }
            if (dt != null && !tablebaseName.Equals(string.Empty))
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
                            foreach (ProtoClasses.ProtoEquip.protoRow item in uptab)
                            {
                                counter++;
                                if (counter2 == 100) { transaction = connection.BeginTransaction(); counter2 = 0; } //IsolationLevel.Snapshot, false
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
                                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `" + tablebaseName + @"`  SET 
`name` = @name, 
`price` = @price, 
`t_position` = @t_position, 
`note` = @note, 
`change_count` = @change_count, 
`time_recieve` = @time_recieve 
WHERE `id` = @id ; ", connection))
                                            {
                                                command.Transaction = transaction;
                                                command.Parameters.Add("@id", DbType.Int32).Value = customerRow["id"] = item.id;
                                                command.Parameters.Add("@name", DbType.String).Value = customerRow["name"] = item.equip_name;
                                                command.Parameters.Add("@price", DbType.Int32).Value = customerRow["price"] = item.price;
                                                command.Parameters.Add("@t_position", DbType.Int32).Value = customerRow["t_position"] = item.t_position;
                                                command.Parameters.Add("@note", DbType.String).Value = customerRow["note"] = item.note;
                                                command.Parameters.Add("@change_count", DbType.Int64).Value = customerRow["change_count"] = item.change_count;
                                                command.Parameters.Add("@time_recieve", DbType.Int64).Value = customerRow["time_recieve"] = item.time_recieve;
                                                command.ExecuteNonQuery();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    #region Insert
                                    using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO " + tablebaseName + @"(
id,
name, 
price, 
t_position, 
note, 
change_count, 
time_recieve
) VALUES(
@id, 
@name, 
@price, 
@t_position, 
@note, 
@change_count, 
@time_recieve
); ", connection))
                                    {
                                        command.Transaction = transaction;
                                        DataRow row = dt.NewRow();
                                        command.Parameters.Add("@id", DbType.Int32).Value = row["id"] = item.id;
                                        command.Parameters.Add("@name", DbType.String).Value = row["name"] = item.equip_name;
                                        command.Parameters.Add("@price", DbType.Int32).Value = row["price"] = item.price;
                                        command.Parameters.Add("@t_position", DbType.Int32).Value = row["t_position"] = item.t_position;
                                        command.Parameters.Add("@note", DbType.String).Value = row["note"] = item.note;
                                        command.Parameters.Add("@change_count", DbType.Int64).Value = row["change_count"] = item.change_count;
                                        command.Parameters.Add("@time_recieve", DbType.Int64).Value = row["time_recieve"] = item.time_recieve;
                                        command.ExecuteNonQuery();
                                        dt.Rows.Add(row);
                                    }
                                    #endregion
                                }
                                if (counter2 == 100) { transaction.Commit(); }

                                if (breaking) { break; }
                                procentCurrent = Convert.ToInt32((counter * 100) / size);
                                if (procentCurrent > procentBefore)
                                {
                                    procentBefore = procentCurrent; if (procentBefore > 100) { procentBefore = 100; }
                                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(procentBefore, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsProcess, (int)tn, true));
                                    //OnSEND(new SqliteUpdateEventArgs(procentBefore, size, counter, SqLiteEvent.UpdateRowsProcess, TableName.customers, false));
                                }
                            }
                            if (counter2 != 100) { transaction.Commit(); }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)tn, true));
            }

            ////DataRow customerRow456 = CustomersArea.SqliteCustomers.TableCustomers.Select("id = " + 15).FirstOrDefault();
            if (!breaking)
            {
                DicPrinters = new ConcurrentDictionary<int, string>(TablePrinters.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));
                DicCuters = new ConcurrentDictionary<int, string>(TableCutters.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));
                DicCncs = new ConcurrentDictionary<int, string>(TableCncs.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["name"])));
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)tn, true));
            }


        }

        #endregion
    }
}
