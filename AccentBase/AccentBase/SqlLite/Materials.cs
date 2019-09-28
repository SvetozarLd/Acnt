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
    static internal class Materials
    {
        static Thread thread;
        static internal DataTable TableMaterialPrint { get; set; }
        static internal DataTable TableMaterialCut { get; set; }
        static internal DataTable TableMaterialCnc { get; set; }
        public static ConcurrentDictionary<int, string> DicMaterialPrint{ get;set;}
        public static ConcurrentDictionary<int, string> DicMaterialCut { get; set; }
        public static ConcurrentDictionary<int, string> DicMaterialCnc { get; set; }
        static string path = Utils.Settings.set.data_path + @"\data";
        static string name = @"\stock.dat";
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
                        string commandstring = @"CREATE TABLE `material_print` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `material_name`	TEXT,
	                                        `material_price`	REAL NOT NULL DEFAULT 0,
	                                        `material_q`	REAL NOT NULL DEFAULT 0,
	                                        `material_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `material_arcticle`	TEXT,
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
                            catch{}
                        }
                    commandstring = @"CREATE TABLE `material_cut` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `material_name`	TEXT,
	                                        `material_price`	REAL NOT NULL DEFAULT 0,
	                                        `material_q`	REAL NOT NULL DEFAULT 0,
	                                        `material_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `material_arcticle`	TEXT,
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
                    commandstring = @"CREATE TABLE `material_cnc` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `material_name`	TEXT,
	                                        `material_price`	REAL NOT NULL DEFAULT 0,
	                                        `material_q`	REAL NOT NULL DEFAULT 0,
	                                        `material_position`	INTEGER NOT NULL DEFAULT 0,
	                                        `material_arcticle`	TEXT,
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
            thread.Name = "LoadMaterials";
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
            if (!File.Exists(path + name)) { CreateTable();}

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableMaterialPrint, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableMaterialPrint == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM material_print", connection))
                    {
                        TableMaterialPrint = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableMaterialPrint);
                    }
                }
            }
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableMaterialPrint, true));
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableMaterialCut, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableMaterialCut == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM material_cut", connection))
                    {
                        TableMaterialCut = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableMaterialCut);
                    }
                }
            }
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableMaterialCut, true));
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableMaterialCnc, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableMaterialCnc == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM material_cnc", connection))
                    {
                        TableMaterialCnc = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableMaterialCnc);
                    }
                }
            }            
            DicMaterialPrint = new ConcurrentDictionary<int, string>(TableMaterialPrint.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));
            DicMaterialCut = new ConcurrentDictionary<int, string>(TableMaterialCut.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));
            DicMaterialCnc = new ConcurrentDictionary<int, string>(TableMaterialCnc.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableMaterialCnc, true));
        }
        #endregion


        #region Обновление таблицы
        static internal void UpdateTable(List<ProtoClasses.ProtoMaterial.protoRow> uptab, SqlEvent.TableName tn)
        {
            string tablebaseName = string.Empty;
            DataTable dt = null;
            switch (tn)
            {
                case SqlEvent.TableName.TableMaterialPrint:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableMaterialPrint, true));
                    tablebaseName = "material_print";
                    dt = TableMaterialPrint;
                    break;
                case SqlEvent.TableName.TableMaterialCut:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableMaterialCut, true));
                    tablebaseName = "material_cut";
                    dt = TableMaterialCut;
                    break;
                case SqlEvent.TableName.TableMaterialCnc:
                    SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableMaterialCnc, true));
                    tablebaseName = "material_cnc";
                    dt = TableMaterialCnc;
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
                            foreach (ProtoClasses.ProtoMaterial.protoRow item in uptab)
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
                                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `"+ tablebaseName + @"`  SET 
`material_name` = @material_name, 
`material_arcticle` = @material_arcticle, 
`material_position` = @material_position, 
`material_price` = @material_price, 
`material_q` = @material_q, 
`note` = @note, 
`change_count` = @change_count, 
`time_recieve` = @time_recieve 
WHERE `id` = @id ; ", connection))
                                            {
                                                command.Transaction = transaction;
                                                command.Parameters.Add("@id", DbType.Int32).Value = customerRow["id"] = item.id;
                                                command.Parameters.Add("@material_name", DbType.String).Value = customerRow["material_name"] = item.material_name;
                                                command.Parameters.Add("@material_arcticle", DbType.String).Value = customerRow["material_arcticle"] = item.material_arcticle;
                                                command.Parameters.Add("@material_position", DbType.Int32).Value = customerRow["material_position"] = item.material_position;
                                                command.Parameters.Add("@material_price", DbType.Double).Value = customerRow["material_price"] = item.material_price;
                                                command.Parameters.Add("@material_q", DbType.Double).Value = customerRow["material_q"] = item.material_q;
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
                                    using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO "+ tablebaseName + @"(
id,
material_name, 
material_arcticle, 
material_position, 
material_price, 
material_q, 
note, 
change_count, 
time_recieve
) VALUES(
@id, 
@material_name, 
@material_arcticle, 
@material_position, 
@material_price, 
@material_q, 
@note, 
@change_count, 
@time_recieve
); ", connection))
                                    {
                                        command.Transaction = transaction;
                                        DataRow row = dt.NewRow();
                                        command.Parameters.Add("@id", DbType.Int32).Value = row["id"] = item.id;
                                        command.Parameters.Add("@material_name", DbType.String).Value = row["material_name"] = item.material_name;
                                        command.Parameters.Add("@material_arcticle", DbType.String).Value = row["material_arcticle"] = item.material_arcticle;
                                        command.Parameters.Add("@material_position", DbType.Int32).Value = row["material_position"] = item.material_position;
                                        command.Parameters.Add("@material_price", DbType.Double).Value = row["material_price"] = item.material_price;
                                        command.Parameters.Add("@material_q", DbType.Double).Value = row["material_q"] = item.material_q;
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
            }else
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)tn, true));
            }

            ////DataRow customerRow456 = CustomersArea.SqliteCustomers.TableCustomers.Select("id = " + 15).FirstOrDefault();
            if (!breaking)
            {
                DicMaterialPrint = new ConcurrentDictionary<int, string>(TableMaterialPrint.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));
                DicMaterialCut = new ConcurrentDictionary<int, string>(TableMaterialCut.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));
                DicMaterialCnc = new ConcurrentDictionary<int, string>(TableMaterialCnc.AsEnumerable().ToDictionary<DataRow, int, string>(row => Convert.ToInt32(row["id"]), row => Convert.ToString(row["material_name"])));
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)tn, true));
            }


        }

        #endregion

    }
}
