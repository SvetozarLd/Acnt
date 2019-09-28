using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace AccentBase.SqlLite
{
    internal static class Order
    {
        private static Thread thread;
        internal static DataTable TableOrders { get; set; }
        public static HashSet<string> Users { get; set; }
        public static HashSet<string> Customers { get; set; }
        public static HashSet<string> PrinterMans { get; set; }
        public static HashSet<string> CutterMans { get; set; }
        public static HashSet<string> CncMans { get; set; }
        public static ConcurrentDictionary<long, ProtoClasses.ProtoOrders.protoOrder> OrdersDic { get; set; }
        //static string path = Application.StartupPath + @"\data";
        //static string path = "App"
        private static readonly string path = Utils.Settings.set.data_path + @"\data";
        private static readonly string name = @"\orders.dat";
        internal static bool breaking { get; set; }

        #region Создание базы
        public static void CreateTable()
        {
            #region Запрос
            string commandstring = @"CREATE TABLE `orders` (
	`id`	INTEGER NOT NULL DEFAULT 0 PRIMARY KEY AUTOINCREMENT UNIQUE, 
	`date_start`	INTEGER NOT NULL DEFAULT 0,
	`dead_line`	INTEGER NOT NULL DEFAULT 0,
	`date_ready_print`	INTEGER NOT NULL DEFAULT 0,
	`date_ready_cut`	INTEGER NOT NULL DEFAULT 0,
	`date_ready_cnc`	INTEGER NOT NULL DEFAULT 0,
	`client`	TEXT,
	`work_name`	TEXT,
	`material_print_id`	INTEGER NOT NULL DEFAULT 0,
	`material_cut_id`	INTEGER NOT NULL DEFAULT 0,
	`material_cnc_id`	INTEGER NOT NULL DEFAULT 0,
	`size_x_print`	REAL NOT NULL DEFAULT 0,
	`size_y_print`	REAL NOT NULL DEFAULT 0,
	`size_x_cut`	REAL NOT NULL DEFAULT 0,
	`size_y_cut`	REAL NOT NULL DEFAULT 0,
	`size_x_cnc`	REAL NOT NULL DEFAULT 0,
	`size_y_cnc`	REAL NOT NULL DEFAULT 0,
	`size_cut`	REAL NOT NULL DEFAULT 0,
	`line_size_cut`	REAL NOT NULL DEFAULT 0,
	`count_size_cut`	INTEGER NOT NULL DEFAULT 1,
	`size_cnc`	REAL NOT NULL DEFAULT 0,
	`line_size_cnc`	REAL NOT NULL DEFAULT 0,
	`count_size_cnc`	INTEGER NOT NULL DEFAULT 1,
	`count_print`	INTEGER NOT NULL DEFAULT 1,
	`count_cut`	INTEGER NOT NULL DEFAULT 1,
	`count_cnc`	INTEGER NOT NULL DEFAULT 1,
	`square_print`	REAL NOT NULL DEFAULT 0,
	`square_cut`	REAL NOT NULL DEFAULT 0,
	`square_cnc`	REAL NOT NULL DEFAULT 0,
	`cutting_on_print`	INTEGER NOT NULL DEFAULT 0,
    `cnc_on_print`	INTEGER NOT NULL DEFAULT 0,
	`print_on`	INTEGER NOT NULL DEFAULT 0,
	`cut_on`	INTEGER NOT NULL DEFAULT 0,
	`cnc_on`	INTEGER NOT NULL DEFAULT 0,
	`printers_id`	INTEGER NOT NULL DEFAULT 0,
	`cutters_id`	INTEGER NOT NULL DEFAULT 0,
	`cncs_id`	INTEGER NOT NULL DEFAULT 0,
	`comments`	TEXT,
	`laminat`	INTEGER NOT NULL  DEFAULT 0,
	`laminat_mat`	INTEGER NOT NULL DEFAULT 0,
	`installation`	INTEGER NOT NULL DEFAULT 0,
	`printerman`	TEXT,
	`cutterman`	TEXT,
	`cncman`	TEXT,
	`adder`	TEXT,
	`print_quality`	TEXT,
	`status`	INTEGER NOT NULL DEFAULT 0,
	`state_print`	INTEGER NOT NULL DEFAULT 0,
	`state_cut`	INTEGER NOT NULL DEFAULT 0,
	`state_cnc`	INTEGER NOT NULL DEFAULT 0,
	`state_install`	INTEGER NOT NULL DEFAULT 0,
	`date_preview`	INTEGER NOT NULL DEFAULT 0,
	`path_preview`	TEXT,
	`path_maket`	TEXT,
	`change_count`	INTEGER NOT NULL DEFAULT 0,
	`time_recieve`	INTEGER NOT NULL DEFAULT 0,
	`worktypes_list`    TEXT,
	`delivery`	INTEGER NOT NULL DEFAULT 0,
	`delivery_office`	INTEGER NOT NULL DEFAULT 0,
	`delivery_address`	TEXT,
	`baner_handling`	INTEGER NOT NULL DEFAULT 0,
	`baner_luvers`	INTEGER NOT NULL DEFAULT 0,
	`baner_handling_size`	REAL NOT NULL DEFAULT 40 
);";
            #endregion
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(path + name))
            {
                SQLiteConnection.CreateFile(path + name);
                using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(commandstring, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        #endregion

        #region SQLite -> datatable
        internal static void loadTable()
        {
            OrdersDic = new ConcurrentDictionary<long, ProtoClasses.ProtoOrders.protoOrder>();
            breaking = false;
            thread = new Thread(GetTable)
            {
                Name = "LoadOrders",
                IsBackground = true
            };
            string[] qqq = new string[2];
            qqq[0] = path;
            qqq[1] = name;
            thread.Start(qqq);
        }
        #endregion

        #region Начальная загрузка DataTables
        private static void GetTable(object ob)
        {
            string[] qqq = ob as string[];
            string fname = qqq[1];
            string fpath = qqq[0];
            if (!File.Exists(path + name)) { CreateTable(); }

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableStart, (int)SqlEvent.TableName.TableOrders, true));
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + fpath + fname + " ;Version=3;"))
            {
                connection.Open();
                if (TableOrders == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM orders", connection))
                    {
                        TableOrders = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(TableOrders);
                    }
                }
            }

            TableOrders.Columns.Add("String_id", typeof(string));
            TableOrders.Columns.Add("Datetime_date_start", typeof(DateTime));
            TableOrders.Columns.Add("Datetime_dead_line", typeof(DateTime));
            TableOrders.Columns.Add("String_date_start", typeof(string));
            TableOrders.Columns.Add("String_dead_line", typeof(string));
            TableOrders.Columns.Add("Datetime_date_ready_print", typeof(DateTime));
            TableOrders.Columns.Add("Datetime_date_ready_cut", typeof(DateTime));
            TableOrders.Columns.Add("Datetime_date_ready_cnc", typeof(DateTime));
            //TableOrders.Columns.Add("String_WorkTypes", typeof(String));
            //TableOrders.Columns.Add("Icon_WorkTypes", typeof(string));
            //TableOrders.Columns.Add("RTF_WorkTypes", typeof(String));
            TableOrders.Columns.Add("Image_WorkTypes", typeof(byte[]));
            TableOrders.Columns.Add("Order_Materials", typeof(string));
            //TableOrders.Columns.Add("be_read", typeof(bool));
            //TableOrders.Columns["be_read"].DefaultValue = true;
            //TableOrders.Columns.Add("Status_WorkTypes", typeof(String));
            StringBuilder sb = new StringBuilder();
            StringBuilder sbicon = new StringBuilder();
            StringBuilder Status_WorkTypes = new StringBuilder();
            Customers = new HashSet<string>();
            Users = new HashSet<string>();
            PrinterMans = new HashSet<string>();
            CutterMans = new HashSet<string>();
            CncMans = new HashSet<string>();
            StringBuilder sbrtf = new StringBuilder();

            string tmp;
            foreach (DataRow row in TableOrders.Rows)
            {
                #region Составление списков имён
                tmp = Utils.Converting.FirstLetterToUpper(row["client"].ToString());
                if (tmp != string.Empty && !Customers.Contains(tmp)) { Customers.Add(tmp); }
                tmp = Utils.Converting.FirstLetterToUpper(row["adder"].ToString());
                if (tmp != string.Empty && !Users.Contains(tmp)) { Users.Add(tmp); }
                tmp = Utils.Converting.FirstLetterToUpper(row["printerman"].ToString());
                if (tmp != string.Empty && !PrinterMans.Contains(tmp)) { PrinterMans.Add(tmp); }
                tmp = Utils.Converting.FirstLetterToUpper(row["cutterman"].ToString());
                if (tmp != string.Empty && !CutterMans.Contains(tmp)) { CutterMans.Add(tmp); }
                tmp = Utils.Converting.FirstLetterToUpper(row["cncman"].ToString());
                if (tmp != string.Empty && !CncMans.Contains(tmp)) { CncMans.Add(tmp); }
                #endregion
                #region Перевод с лонг в дататайм
                row["String_id"] = Convert.ToString(row["id"]);
                row["Datetime_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_start"]));
                row["Datetime_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["dead_line"]));
                row["String_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_start"])).ToString("dd.MM.yyyy HH:mm");
                row["String_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["dead_line"])).ToString("dd.MM.yyyy HH:mm");
                row["Datetime_date_ready_print"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_print"]));
                row["Datetime_date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_cut"]));
                row["Datetime_date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_cnc"]));
                #endregion
                //row["be_read"] = false;
                #region WorkTypes
                int print = 0;
                int cut = 0;
                int cnc = 0;
                int install = 0;
                int status = Convert.ToInt32(row["status"]);
                string mtmp = string.Empty;
                sb.Clear();
                if (Convert.ToBoolean(row["print_on"]))
                {
                    //var mprint = SqlLite.Materials.DicMaterialPrint.Where(item => item.Key == order.material_print_id).ToList();
                    if (Materials.DicMaterialPrint.TryGetValue(Utils.CheckDBNull.ToInt32(row["material_print_id"]), out mtmp))
                    {
                        sb.Append(mtmp);
                    }
                    if (Convert.ToBoolean(row["state_print"]) || status == 3 || status == 4) { print = 2; } else { print = 1; }
                }
                if (Convert.ToBoolean(row["cut_on"]))
                {
                    if (Materials.DicMaterialCut.TryGetValue(Utils.CheckDBNull.ToInt32(row["material_cut_id"]), out mtmp))
                    {
                        if (sb.Length > 0) { sb.Append(Environment.NewLine); }
                        sb.Append(mtmp);
                    }
                    if (Convert.ToBoolean(row["state_cut"]) || status == 3 || status == 4) { cut = 2; } else { cut = 1; }
                }
                if (Convert.ToBoolean(row["cnc_on"]))
                {
                    if (Materials.DicMaterialCnc.TryGetValue(Utils.CheckDBNull.ToInt32(row["material_cnc_id"]), out mtmp))
                    {
                        if (sb.Length > 0) { sb.Append(Environment.NewLine); }
                        sb.Append(mtmp);
                        if (Convert.ToBoolean(row["state_cnc"]) || status == 3 || status == 4) { cnc = 2; } else { cnc = 1; }
                    }
                }
                //if (Convert.ToBoolean(row["installation"])) { if (status == 3) { install = 2; } else { install = 1; } }
                if (Convert.ToBoolean(row["installation"]))
                {
                    if (Convert.ToBoolean(row["state_install"]) || status == 4) { install = 2; } else { install = 1; }
                }

                row["Image_WorkTypes"] = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);
                row["Order_Materials"] = sb.ToString();
                //if (Convert.ToBoolean(customerRow["print_on"])) { if (Convert.ToBoolean(customerRow["state_print"]) || status == 3 || status == 4) { print = 2; } else { print = 1; } }
                //if (Convert.ToBoolean(customerRow["cut_on"])) { if (Convert.ToBoolean(customerRow["state_cut"]) || status == 3 || status == 4) { cut = 2; } else { cut = 1; } }
                //if (Convert.ToBoolean(customerRow["cnc_on"])) { if (Convert.ToBoolean(customerRow["state_cnc"]) || status == 3 || status == 4) { cnc = 2; } else { cnc = 1; } }
                //if (Convert.ToBoolean(customerRow["installation"])) if (Convert.ToBoolean(customerRow["state_install"]) || status == 4) { install = 2; } else { install = 1; }
                #endregion
                OrdersDic.TryAdd(Convert.ToInt64(row["id"]), Utils.DataRowToProto.OrderToProto(row));
            }

            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.LoadTableEnd, (int)SqlEvent.TableName.TableOrders, true));
            #region мусор
            //    OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableEnd, TableName.customersFolders, false));
            //    OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableStart, TableName.customers, false));
            //    if (TableCustomers == null)
            //    {
            //        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM customers", connection))
            //        {
            //            TableCustomers = new DataTable();
            //            adapter.AcceptChangesDuringFill = false;
            //            adapter.Fill(TableCustomers);
            //        }
            //        TableCustomers.Columns.Add("unread", typeof(byte[]));
            //        foreach (DataRow row in TableCustomers.Rows)
            //        {
            //            //row["RegistrationTime"] = UtilsArea.unixdate.Int32ToDateTime(Convert.ToInt32(row["Certificate_date"]));
            //            //if (UnreadRow.Contains(Convert.ToInt32(row["id_server"]))) { row["unread"] = UtilsArea.byteConverting.ImageToByte(Properties.Resources.status_online); } else { row["Unread"] = UtilsArea.byteConverting.ImageToByte(Properties.Resources.status_offline); }
            //            if (UnreadRow.Contains(Convert.ToInt32(row["id"]))) { row["unread"] = UtilsArea.byteConverting.ImageToByte(Properties.Resources.vcard_unread); } else { row["unread"] = UtilsArea.byteConverting.ImageToByte(Properties.Resources.vcard); }

            //        }
            //    }
            //    OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableEnd, TableName.customers, false));
            //    //TableCustomersFolders = new DataTable();
            //    //MessageBox.Show(SqliteCustomers.TableCustomersFolders.Rows.Count.ToString());
            //    //    transaction.Commit();

            //    //}
            //    //var sqlAdapter2 = new SQLiteDataAdapter("SELECT * FROM folders", connection);

            //    //sqlAdapter2.AcceptChangesDuringFill = false;
            //    //sqlAdapter2.Fill(TableCustomersFolders);
            //    //connection.Close();
            //    OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableStart, TableName.customersContacts, false));
            //    if (TableCustomersContacts == null)
            //    {
            //        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM contacts", connection))
            //        {
            //            TableCustomersContacts = new DataTable();
            //            adapter.AcceptChangesDuringFill = false;
            //            adapter.Fill(TableCustomersContacts);
            //        }
            //    }
            //}
            //OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableEnd, TableName.customersContacts, false));
            ////TableCustomers.Columns.Add("RegistrationTime", typeof(DateTime));
            ////TableCustomers.Columns.Add("Unread", typeof(bool));


            ////TableCustomersFolders = new DataTable();
            ////using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + PathFolder + PathFile + " ;Version=3;"))
            ////{
            ////    connection.Open();
            ////    var sqlAdapter = new SQLiteDataAdapter("SELECT * FROM folders", connection);

            ////    sqlAdapter.AcceptChangesDuringFill = false;
            ////    sqlAdapter.Fill(TableCustomersFolders);
            ////    connection.Close();
            ////}
            //////OnSEND(new SqliteUpdateEventArgs(0, 0, 0, 0, TableName.customersFolders, true));
            //OnSEND(new SqliteUpdateEventArgs(0, 0, 0, SqLiteEvent.LoadTableEnd, TableName.customers, true));
            #endregion
        }
        #endregion

        #region Обновление таблицы
        internal static void UpdateTable(List<ProtoClasses.ProtoOrders.protoOrder> uptab)
        {
            SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsStart, (int)SqlEvent.TableName.TableOrders, true));

            //if (!File.Exists(PathFolder + PathFile)) { CREATECustomersTable(); }
            int size = 0;
            int procentBefore = 0;
            int procentCurrent = 0;
            int counter = 0;
            int counter2 = 0;

            if (uptab != null && uptab.Count > 0)
            {
                size = uptab.Count;
                //OnSEND(new SqliteUpdateEventArgs(0, ChangeList.Count, 0, SqLiteEvent.UpdateRowsStart, TableName.customers, false));
                //using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + PathFolder + PathFile + " ;Version=3; Pooling=True; Max Pool Size=100;"))
                //{
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {                                                                                                                   //try
                                                                                                                                        //{
                        connection.Open();
                        //using (SQLiteTransaction transaction = connection.BeginTransaction())
                        //{
                        SQLiteTransaction transaction = connection.BeginTransaction();
                        foreach (ProtoClasses.ProtoOrders.protoOrder item in uptab)
                        {
                            counter++;
                            if (counter2 == 100) { transaction = connection.BeginTransaction(); counter2 = 0; } //IsolationLevel.Snapshot, false
                            counter2++;
                            DataRow customerRow = TableOrders.Select("id = " + item.id).FirstOrDefault();
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
                                        try
                                        {
                                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders`  SET 
`date_start` = @date_start, 
`dead_line` = @dead_line, 
`date_ready_print` = @date_ready_print, 
`date_ready_cut` = @date_ready_cut, 
`date_ready_cnc` = @date_ready_cnc, 
`client` = @client, 
`work_name` = @work_name, 
`material_print_id` = @material_print_id, 
`material_cut_id` = @material_cut_id, 
`material_cnc_id` = @material_cnc_id, 
`size_x_print` = @size_x_print, 
`size_y_print` = @size_y_print, 
`size_x_cut` = @size_x_cut, 
`size_y_cut` = @size_y_cut, 
`size_x_cnc` = @size_x_cnc, 
`size_y_cnc` = @size_y_cnc, 
`size_cut` = @size_cut, 
`line_size_cut` = @line_size_cut, 
`count_size_cut` = @count_size_cut, 
`size_cnc` = @size_cnc, 
`line_size_cnc` = @line_size_cnc, 
`count_size_cnc` = @count_size_cnc, 
`count_print` = @count_print, 
`count_cut` = @count_cut, 
`count_cnc` = @count_cnc, 
`square_print` = @square_print, 
`square_cut` = @square_cut, 
`square_cnc` = @square_cnc, 
`cutting_on_print` = @cutting_on_print,
`cnc_on_print` = @cnc_on_print,
`print_on` = @print_on, 
`cut_on` = @cut_on, 
`cnc_on` = @cnc_on, 
`printers_id` = @printers_id, 
`cutters_id` = @cutters_id, 
`cncs_id` = @cncs_id, 
`comments` = @comments, 
`laminat` = @laminat, 
`laminat_mat` = @laminat_mat, 
`installation` = @installation, 
`printerman` = @printerman, 
`cutterman` = @cutterman, 
`cncman` = @cncman, 
`adder` = @adder, 
`print_quality` = @print_quality, 
`status` = @state, 
`state_print` = @state_print, 
`state_cut` = @state_cut, 
`state_cnc` = @state_cnc, 
`state_install` = @state_install, 
`date_preview` = @date_preview, 
`path_preview` = @path_preview, 
`path_maket` = @path_maket, 
`change_count` = @change_count, 
`time_recieve` = @time_recieve, 
`worktypes_list`= @worktypes_list, 
`delivery` = @delivery, 
`delivery_office` = @delivery_office, 
`delivery_address` = @delivery_address, 
`baner_handling` = @baner_handling, 
`baner_luvers` = @baner_luvers, 
`baner_handling_size` = @baner_handling_size 
WHERE `id` = @id ; ", connection))
                                            {
                                                command.Transaction = transaction;
                                                command.Parameters.Add("@id", DbType.Int64).Value = customerRow["id"] = Utils.CheckDBNull.ToLong(item.id);
                                                command.Parameters.Add("@date_start", DbType.Int64).Value = customerRow["date_start"] = Utils.CheckDBNull.ToLong(item.date_start);
                                                command.Parameters.Add("@dead_line", DbType.Int64).Value = customerRow["dead_line"] = Utils.CheckDBNull.ToLong(item.dead_line);
                                                command.Parameters.Add("@date_ready_print", DbType.Int64).Value = customerRow["date_ready_print"] = Utils.CheckDBNull.ToLong(item.date_ready_print);
                                                command.Parameters.Add("@date_ready_cut", DbType.Int64).Value = customerRow["date_ready_cut"] = Utils.CheckDBNull.ToLong(item.date_ready_cut);
                                                command.Parameters.Add("@date_ready_cnc", DbType.Int64).Value = customerRow["date_ready_cnc"] = Utils.CheckDBNull.ToLong(item.date_ready_cnc);
                                                command.Parameters.Add("@client", DbType.String).Value = customerRow["client"] = Utils.CheckDBNull.ToString(item.client);
                                                command.Parameters.Add("@work_name", DbType.String).Value = customerRow["work_name"] = Utils.CheckDBNull.ToString(item.work_name);
                                                command.Parameters.Add("@material_print_id", DbType.Int32).Value = customerRow["material_print_id"] = Utils.CheckDBNull.ToInt32(item.material_print_id);
                                                command.Parameters.Add("@material_cut_id", DbType.Int32).Value = customerRow["material_cut_id"] = Utils.CheckDBNull.ToInt32(item.material_cut_id);
                                                command.Parameters.Add("@material_cnc_id", DbType.Int32).Value = customerRow["material_cnc_id"] = Utils.CheckDBNull.ToInt32(item.material_cnc_id);
                                                command.Parameters.Add("@size_x_print", DbType.Double).Value = customerRow["size_x_print"] = Utils.CheckDBNull.ToDouble(item.size_x_print);
                                                command.Parameters.Add("@size_y_print", DbType.Double).Value = customerRow["size_y_print"] = Utils.CheckDBNull.ToDouble(item.size_y_print);
                                                command.Parameters.Add("@size_x_cut", DbType.Double).Value = customerRow["size_x_cut"] = Utils.CheckDBNull.ToDouble(item.size_x_cut);
                                                command.Parameters.Add("@size_y_cut", DbType.Double).Value = customerRow["size_y_cut"] = Utils.CheckDBNull.ToDouble(item.size_y_cut);
                                                command.Parameters.Add("@size_x_cnc", DbType.Double).Value = customerRow["size_x_cnc"] = Utils.CheckDBNull.ToDouble(item.size_x_cnc);
                                                command.Parameters.Add("@size_y_cnc", DbType.Double).Value = customerRow["size_y_cnc"] = Utils.CheckDBNull.ToDouble(item.size_y_cnc);
                                                command.Parameters.Add("@size_cut", DbType.Double).Value = customerRow["size_cut"] = Utils.CheckDBNull.ToDouble(item.size_cut);
                                                command.Parameters.Add("@line_size_cut", DbType.Double).Value = customerRow["line_size_cut"] = Utils.CheckDBNull.ToDouble(item.line_size_cut);
                                                command.Parameters.Add("@count_size_cut", DbType.Int32).Value = customerRow["count_size_cut"] = Utils.CheckDBNull.ToInt32(item.count_size_cut);
                                                command.Parameters.Add("@size_cnc", DbType.Double).Value = customerRow["size_cnc"] = Utils.CheckDBNull.ToDouble(item.size_cnc);
                                                command.Parameters.Add("@line_size_cnc", DbType.Double).Value = customerRow["line_size_cnc"] = Utils.CheckDBNull.ToDouble(item.line_size_cnc);
                                                command.Parameters.Add("@count_size_cnc", DbType.Int32).Value = customerRow["count_size_cnc"] = Utils.CheckDBNull.ToInt32(item.count_size_cnc);
                                                command.Parameters.Add("@count_print", DbType.Int32).Value = customerRow["count_print"] = Utils.CheckDBNull.ToInt32(item.count_print);
                                                command.Parameters.Add("@count_cut", DbType.Int32).Value = customerRow["count_cut"] = Utils.CheckDBNull.ToInt32(item.count_cut);
                                                command.Parameters.Add("@count_cnc", DbType.Int32).Value = customerRow["count_cnc"] = Utils.CheckDBNull.ToInt32(item.count_cnc);
                                                command.Parameters.Add("@square_print", DbType.Double).Value = customerRow["square_print"] = Utils.CheckDBNull.ToDouble(item.square_print);
                                                command.Parameters.Add("@square_cut", DbType.Double).Value = customerRow["square_cut"] = Utils.CheckDBNull.ToDouble(item.square_cut);
                                                command.Parameters.Add("@square_cnc", DbType.Double).Value = customerRow["square_cnc"] = Utils.CheckDBNull.ToDouble(item.square_cnc);
                                                command.Parameters.Add("@cutting_on_print", DbType.Int16).Value = customerRow["cutting_on_print"] = Utils.CheckDBNull.ToIntFromBool(item.cutting_on_print);
                                                command.Parameters.Add("@cnc_on_print", DbType.Int16).Value = customerRow["cnc_on_print"] = Utils.CheckDBNull.ToIntFromBool(item.cnc_on_print);
                                                command.Parameters.Add("@print_on", DbType.Int16).Value = customerRow["print_on"] = Utils.CheckDBNull.ToIntFromBool(item.print_on);
                                                command.Parameters.Add("@cut_on", DbType.Int16).Value = customerRow["cut_on"] = Utils.CheckDBNull.ToIntFromBool(item.cut_on);
                                                command.Parameters.Add("@cnc_on", DbType.Int16).Value = customerRow["cnc_on"] = Utils.CheckDBNull.ToIntFromBool(item.cnc_on);
                                                command.Parameters.Add("@printers_id", DbType.Int32).Value = customerRow["printers_id"] = Utils.CheckDBNull.ToInt32(item.printers_id);
                                                command.Parameters.Add("@cutters_id", DbType.Int32).Value = customerRow["cutters_id"] = Utils.CheckDBNull.ToInt32(item.cutters_id);
                                                command.Parameters.Add("@cncs_id", DbType.Int32).Value = customerRow["cncs_id"] = Utils.CheckDBNull.ToInt32(item.cncs_id);
                                                command.Parameters.Add("@comments", DbType.String).Value = customerRow["comments"] = Utils.CheckDBNull.ToString(item.comments);
                                                command.Parameters.Add("@laminat", DbType.Int16).Value = customerRow["laminat"] = Utils.CheckDBNull.ToIntFromBool(item.laminat);
                                                command.Parameters.Add("@laminat_mat", DbType.Int16).Value = customerRow["laminat_mat"] = Utils.CheckDBNull.ToIntFromBool(item.laminat_mat);
                                                command.Parameters.Add("@installation", DbType.Int16).Value = customerRow["installation"] = Utils.CheckDBNull.ToIntFromBool(item.installation);
                                                command.Parameters.Add("@printerman", DbType.String).Value = customerRow["printerman"] = Utils.CheckDBNull.ToString(item.printerman);
                                                command.Parameters.Add("@cutterman", DbType.String).Value = customerRow["cutterman"] = Utils.CheckDBNull.ToString(item.cutterman);
                                                command.Parameters.Add("@cncman", DbType.String).Value = customerRow["cncman"] = Utils.CheckDBNull.ToString(item.cncman);
                                                command.Parameters.Add("@adder", DbType.String).Value = customerRow["adder"] = Utils.CheckDBNull.ToString(item.adder);
                                                command.Parameters.Add("@print_quality", DbType.String).Value = customerRow["print_quality"] = Utils.CheckDBNull.ToString(item.print_quality);
                                                command.Parameters.Add("@state", DbType.Int32).Value = customerRow["status"] = Utils.CheckDBNull.ToInt32(item.state);
                                                command.Parameters.Add("@state_print", DbType.Int16).Value = customerRow["state_print"] = Utils.CheckDBNull.ToIntFromBool(item.state_print);
                                                command.Parameters.Add("@state_cut", DbType.Int16).Value = customerRow["state_cut"] = Utils.CheckDBNull.ToIntFromBool(item.state_cut);
                                                command.Parameters.Add("@state_cnc", DbType.Int16).Value = customerRow["state_cnc"] = Utils.CheckDBNull.ToIntFromBool(item.state_cnc);
                                                command.Parameters.Add("@state_install", DbType.Int16).Value = customerRow["state_install"] = Utils.CheckDBNull.ToIntFromBool(item.state_install);
                                                command.Parameters.Add("@date_preview", DbType.Int64).Value = customerRow["date_preview"] = Utils.CheckDBNull.ToLong(item.date_preview);
                                                command.Parameters.Add("@path_preview", DbType.String).Value = customerRow["path_preview"] = Utils.CheckDBNull.ToString(item.path_preview);
                                                command.Parameters.Add("@path_maket", DbType.String).Value = customerRow["path_maket"] = Utils.CheckDBNull.ToString(item.path_maket);
                                                command.Parameters.Add("@change_count", DbType.Int64).Value = customerRow["change_count"] = Utils.CheckDBNull.ToLong(item.change_count);
                                                command.Parameters.Add("@time_recieve", DbType.Int64).Value = customerRow["time_recieve"] = Utils.CheckDBNull.ToLong(item.time_recieve);
                                                command.Parameters.Add("@worktypes_list", DbType.String).Value = customerRow["worktypes_list"] = Utils.CheckDBNull.ToString(item.worktypes_list);

                                                command.Parameters.Add("@delivery", DbType.Int16).Value = customerRow["delivery"] = Utils.CheckDBNull.ToIntFromBool(item.delivery);
                                                command.Parameters.Add("@delivery_office", DbType.Int16).Value = customerRow["delivery_office"] = Utils.CheckDBNull.ToIntFromBool(item.delivery_office);
                                                command.Parameters.Add("@delivery_address", DbType.String).Value = customerRow["delivery_address"] = Utils.CheckDBNull.ToString(item.delivery_address);
                                                command.Parameters.Add("@baner_handling", DbType.Int16).Value = customerRow["baner_handling"] = Utils.CheckDBNull.ToIntFromBool(item.baner_handling);
                                                command.Parameters.Add("@baner_luvers", DbType.Int16).Value = customerRow["baner_luvers"] = Utils.CheckDBNull.ToIntFromBool(item.baner_luvers);
                                                command.Parameters.Add("@baner_handling_size", DbType.Double).Value = customerRow["baner_handling_size"] = Utils.CheckDBNull.ToLong(item.baner_handling_size);

                                                string yearpath = Utils.Settings.set.data_path + @"/" + "makets" + @"/" + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.time_recieve)).ToString("yyyy.MM");
                                                if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                                                string orderPath = Utils.Settings.set.data_path + @"/" + "makets" + @"/" + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.time_recieve)).ToString("yyyy.MM") + "/" + item.id.ToString("0000");
                                                if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }
                                                if (!Directory.Exists(orderPath + @"/" + "makets")) { Directory.CreateDirectory(orderPath + @"/" + "makets"); }
                                                if (!Directory.Exists(orderPath + @"/" + "preview")) { Directory.CreateDirectory(orderPath + @"/" + "preview"); }
                                                if (!Directory.Exists(orderPath + @"/" + "doc")) { Directory.CreateDirectory(orderPath + @"/" + "doc"); }
                                                if (!Directory.Exists(orderPath + @"/" + "photoreport")) { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }

                                                if (item.preview != null)
                                                {
                                                    try
                                                    {
                                                        using (FileStream fs = new FileStream(orderPath + @"/index.png", FileMode.Create, FileAccess.Write))
                                                        {
                                                            fs.Write(item.preview, 0, item.preview.Length);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                    item.preview = new byte[1] { 0 };
                                                }

                                                if (item.installation_comment != null)
                                                {
                                                    try
                                                    {
                                                        using (FileStream fs = new FileStream(orderPath + @"/montage.doc", FileMode.Create, FileAccess.Write))
                                                        {
                                                            fs.Write(item.installation_comment, 0, item.installation_comment.Length);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                    item.installation_comment = new byte[1] { 0 };
                                                }
                                                //if (item.preview != null)
                                                //{
                                                //    command.Parameters.Add("@preview", DbType.Object, item.preview.Length).Value = customerRow["preview"] = item.preview;
                                                //}
                                                //else { command.Parameters.Add("@preview", DbType.Object).Value = customerRow["preview"] = null; }
                                                //command.Parameters.Add("@preview", DbType.Object).Value = customerRow["preview"] = null;
                                                command.ExecuteNonQuery();

                                                customerRow["String_id"] = Convert.ToString(customerRow["id"]);
                                                customerRow["Datetime_date_start"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["date_start"]));
                                                customerRow["Datetime_dead_line"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["dead_line"]));
                                                customerRow["String_date_start"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["date_start"])).ToString("dd.MM.yyyy HH:mm");
                                                customerRow["String_dead_line"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["dead_line"])).ToString("dd.MM.yyyy HH:mm");
                                                customerRow["Datetime_date_ready_print"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["date_ready_print"]));
                                                customerRow["Datetime_date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["date_ready_cut"]));
                                                customerRow["Datetime_date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(customerRow["date_ready_cnc"]));
                                                //customerRow["be_read"] = true;
                                                int print = 0;
                                                int cut = 0;
                                                int cnc = 0;
                                                int install = 0;
                                                int status = Convert.ToInt32(customerRow["status"]);
                                                //if (Convert.ToBoolean(customerRow["print_on"])) { if (Convert.ToBoolean(customerRow["state_print"]) || status == 3) { print = 2; } else { print = 1; } }
                                                //if (Convert.ToBoolean(customerRow["cut_on"])) { if (Convert.ToBoolean(customerRow["state_cut"]) || status == 3) { cut = 2; } else { cut = 1; } }
                                                //if (Convert.ToBoolean(customerRow["cnc_on"])) { if (Convert.ToBoolean(customerRow["state_cnc"]) || status == 3) { cnc = 2; } else { cnc = 1; } }
                                                //if (Convert.ToBoolean(customerRow["installation"])) { if (status == 3) { install = 2; } else { install = 1; } }
                                                if (Utils.CheckDBNull.ToBoolean(customerRow["print_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_print"]) || status == 3 || status == 4) { print = 2; } else { print = 1; } }
                                                if (Utils.CheckDBNull.ToBoolean(customerRow["cut_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_cut"]) || status == 3 || status == 4) { cut = 2; } else { cut = 1; } }
                                                if (Utils.CheckDBNull.ToBoolean(customerRow["cnc_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_cnc"]) || status == 3 || status == 4) { cnc = 2; } else { cnc = 1; } }
                                                if (Utils.CheckDBNull.ToBoolean(customerRow["installation"]))
                                                {
                                                    if (Utils.CheckDBNull.ToBoolean(customerRow["state_install"]) || status == 4) { install = 2; } else { install = 1; }
                                                }

                                                customerRow["Image_WorkTypes"] = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);
                                                //if (SendChangeToSocket.IDsHashset.Contains(item.sender_row_stringid))
                                                //{ Program.RecieveOrderFromServer(item); }
                                                SqlEvent.OnSENDOrderUpdate(new SqlEvent.OrderUpdateEventArgs(item.id, customerRow.ItemArray, SocketClient.TableClient.SocketMessageCommand.RowsUpdate, item.HistoryRows));
                                                Program.RecieveOrderFromServer(item);
                                            }
                                            #endregion

                                            OrdersDic[item.id] = item;
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                }
                            }
                            else
                            {
                                #region Insert
                                using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO orders(
id,
date_start, 
dead_line, 
date_ready_print, 
date_ready_cut, 
date_ready_cnc, 
client, 
work_name, 
material_print_id, 
material_cut_id, 
material_cnc_id, 
size_x_print, 
size_y_print, 
size_x_cut, 
size_y_cut, 
size_x_cnc, 
size_y_cnc, 
size_cut, 
line_size_cut, 
count_size_cut, 
size_cnc, 
line_size_cnc, 
count_size_cnc, 
count_print, 
count_cut, 
count_cnc, 
square_print, 
square_cut, 
square_cnc, 
cutting_on_print,
cnc_on_print, 
print_on, 
cut_on,  
cnc_on, 
printers_id, 
cutters_id, 
cncs_id, 
comments, 
laminat, 
laminat_mat, 
installation,  
printerman, 
cutterman, 
cncman, 
adder, 
print_quality, 
status, 
state_print, 
state_cut, 
state_cnc, 
state_install, 
date_preview, 
path_preview, 
path_maket, 
change_count, 
time_recieve,
worktypes_list, 
delivery, 
delivery_office, 
delivery_address, 
baner_handling, 
baner_luvers, 
baner_handling_size 
) VALUES(
@id, 
@date_start, 
@dead_line, 
@date_ready_print, 
@date_ready_cut, 
@date_ready_cnc, 
@client, 
@work_name, 
@material_print_id, 
@material_cut_id, 
@material_cnc_id, 
@size_x_print, 
@size_y_print, 
@size_x_cut, 
@size_y_cut, 
@size_x_cnc, 
@size_y_cnc, 
@size_cut, 
@line_size_cut, 
@count_size_cut, 
@size_cnc, 
@line_size_cnc, 
@count_size_cnc, 
@count_print, 
@count_cut, 
@count_cnc, 
@square_print, 
@square_cut, 
@square_cnc, 
@cutting_on_print,
@cnc_on_print, 
@print_on, 
@cut_on, 
@cnc_on, 
@printers_id, 
@cutters_id, 
@cncs_id, 
@comments, 
@laminat, 
@laminat_mat, 
@installation,  
@printerman, 
@cutterman, 
@cncman, 
@adder, 
@print_quality, 
@State, 
@state_print, 
@state_cut, 
@state_cnc, 
@state_install, 
@date_preview, 
@path_preview, 
@path_maket, 
@change_count, 
@time_recieve, 
@worktypes_list, 
@delivery, 
@delivery_office, 
@delivery_address, 
@baner_handling, 
@baner_luvers, 
@baner_handling_size 
); ", connection))
                                {
                                    command.Transaction = transaction;
                                    DataRow row = TableOrders.NewRow();
                                    command.Parameters.Add("@id", DbType.Int64).Value = row["id"] = Utils.CheckDBNull.ToLong(item.id);
                                    command.Parameters.Add("@date_start", DbType.Int64).Value = row["date_start"] = Utils.CheckDBNull.ToLong(item.date_start);
                                    command.Parameters.Add("@dead_line", DbType.Int64).Value = row["dead_line"] = Utils.CheckDBNull.ToLong(item.dead_line);
                                    command.Parameters.Add("@date_ready_print", DbType.Int64).Value = row["date_ready_print"] = Utils.CheckDBNull.ToLong(item.date_ready_print);
                                    command.Parameters.Add("@date_ready_cut", DbType.Int64).Value = row["date_ready_cut"] = Utils.CheckDBNull.ToLong(item.date_ready_cut);
                                    command.Parameters.Add("@date_ready_cnc", DbType.Int64).Value = row["date_ready_cnc"] = Utils.CheckDBNull.ToLong(item.date_ready_cnc);
                                    command.Parameters.Add("@client", DbType.String).Value = row["client"] = Utils.CheckDBNull.ToString(item.client);
                                    command.Parameters.Add("@work_name", DbType.String).Value = row["work_name"] = Utils.CheckDBNull.ToString(item.work_name);
                                    command.Parameters.Add("@material_print_id", DbType.Int32).Value = row["material_print_id"] = Utils.CheckDBNull.ToInt32(item.material_print_id);
                                    command.Parameters.Add("@material_cut_id", DbType.Int32).Value = row["material_cut_id"] = Utils.CheckDBNull.ToInt32(item.material_cut_id);
                                    command.Parameters.Add("@material_cnc_id", DbType.Int32).Value = row["material_cnc_id"] = Utils.CheckDBNull.ToInt32(item.material_cnc_id);
                                    command.Parameters.Add("@size_x_print", DbType.Double).Value = row["size_x_print"] = Utils.CheckDBNull.ToDouble(item.size_x_print);
                                    command.Parameters.Add("@size_y_print", DbType.Double).Value = row["size_y_print"] = Utils.CheckDBNull.ToDouble(item.size_y_print);
                                    command.Parameters.Add("@size_x_cut", DbType.Double).Value = row["size_x_cut"] = Utils.CheckDBNull.ToDouble(item.size_x_cut);
                                    command.Parameters.Add("@size_y_cut", DbType.Double).Value = row["size_y_cut"] = Utils.CheckDBNull.ToDouble(item.size_y_cut);
                                    command.Parameters.Add("@size_x_cnc", DbType.Double).Value = row["size_x_cnc"] = Utils.CheckDBNull.ToDouble(item.size_x_cnc);
                                    command.Parameters.Add("@size_y_cnc", DbType.Double).Value = row["size_y_cnc"] = Utils.CheckDBNull.ToDouble(item.size_y_cnc);
                                    command.Parameters.Add("@size_cut", DbType.Double).Value = row["size_cut"] = Utils.CheckDBNull.ToDouble(item.size_cut);
                                    command.Parameters.Add("@line_size_cut", DbType.Double).Value = row["line_size_cut"] = Utils.CheckDBNull.ToDouble(item.line_size_cut);
                                    command.Parameters.Add("@count_size_cut", DbType.Int32).Value = row["count_size_cut"] = Utils.CheckDBNull.ToInt32(item.count_size_cut);
                                    command.Parameters.Add("@size_cnc", DbType.Double).Value = row["size_cnc"] = Utils.CheckDBNull.ToDouble(item.size_cnc);
                                    command.Parameters.Add("@line_size_cnc", DbType.Double).Value = row["line_size_cnc"] = Utils.CheckDBNull.ToDouble(item.line_size_cnc);
                                    command.Parameters.Add("@count_size_cnc", DbType.Int32).Value = row["count_size_cnc"] = Utils.CheckDBNull.ToInt32(item.count_size_cnc);
                                    command.Parameters.Add("@count_print", DbType.Int32).Value = row["count_print"] = Utils.CheckDBNull.ToInt32(item.count_print);
                                    command.Parameters.Add("@count_cut", DbType.Int32).Value = row["count_cut"] = Utils.CheckDBNull.ToInt32(item.count_cut);
                                    command.Parameters.Add("@count_cnc", DbType.Int32).Value = row["count_cnc"] = Utils.CheckDBNull.ToInt32(item.count_cnc);
                                    command.Parameters.Add("@square_print", DbType.Double).Value = row["square_print"] = Utils.CheckDBNull.ToDouble(item.square_print);
                                    command.Parameters.Add("@square_cut", DbType.Double).Value = row["square_cut"] = Utils.CheckDBNull.ToDouble(item.square_cut);
                                    command.Parameters.Add("@square_cnc", DbType.Double).Value = row["square_cnc"] = Utils.CheckDBNull.ToDouble(item.square_cnc);
                                    command.Parameters.Add("@cutting_on_print", DbType.Int16).Value = row["cutting_on_print"] = Utils.CheckDBNull.ToIntFromBool(item.cutting_on_print);
                                    command.Parameters.Add("@cnc_on_print", DbType.Int16).Value = row["cnc_on_print"] = Utils.CheckDBNull.ToIntFromBool(item.cnc_on_print);
                                    command.Parameters.Add("@print_on", DbType.Int16).Value = row["print_on"] = Utils.CheckDBNull.ToIntFromBool(item.print_on);
                                    command.Parameters.Add("@cut_on", DbType.Int16).Value = row["cut_on"] = Utils.CheckDBNull.ToIntFromBool(item.cut_on);
                                    command.Parameters.Add("@cnc_on", DbType.Int16).Value = row["cnc_on"] = Utils.CheckDBNull.ToIntFromBool(item.cnc_on);
                                    command.Parameters.Add("@printers_id", DbType.Int32).Value = row["printers_id"] = Utils.CheckDBNull.ToInt32(item.printers_id);
                                    command.Parameters.Add("@cutters_id", DbType.Int32).Value = row["cutters_id"] = Utils.CheckDBNull.ToInt32(item.cutters_id);
                                    command.Parameters.Add("@cncs_id", DbType.Int32).Value = row["cncs_id"] = Utils.CheckDBNull.ToInt32(item.cncs_id);
                                    command.Parameters.Add("@comments", DbType.String).Value = row["comments"] = Utils.CheckDBNull.ToString(item.comments);
                                    command.Parameters.Add("@laminat", DbType.Int16).Value = row["laminat"] = Utils.CheckDBNull.ToIntFromBool(item.laminat);
                                    command.Parameters.Add("@laminat_mat", DbType.Int16).Value = row["laminat_mat"] = Utils.CheckDBNull.ToIntFromBool(item.laminat_mat);
                                    command.Parameters.Add("@installation", DbType.Int16).Value = row["installation"] = Utils.CheckDBNull.ToIntFromBool(item.installation);
                                    command.Parameters.Add("@printerman", DbType.String).Value = row["printerman"] = Utils.CheckDBNull.ToString(item.printerman);
                                    command.Parameters.Add("@cutterman", DbType.String).Value = row["cutterman"] = Utils.CheckDBNull.ToString(item.cutterman);
                                    command.Parameters.Add("@cncman", DbType.String).Value = row["cncman"] = Utils.CheckDBNull.ToString(item.cncman);
                                    command.Parameters.Add("@adder", DbType.String).Value = row["adder"] = Utils.CheckDBNull.ToString(item.adder);
                                    command.Parameters.Add("@print_quality", DbType.String).Value = row["print_quality"] = Utils.CheckDBNull.ToString(item.print_quality);
                                    command.Parameters.Add("@state", DbType.Int32).Value = row["status"] = Utils.CheckDBNull.ToInt32(item.state);
                                    command.Parameters.Add("@state_print", DbType.Int16).Value = row["state_print"] = Utils.CheckDBNull.ToIntFromBool(item.state_print);
                                    command.Parameters.Add("@state_cut", DbType.Int16).Value = row["state_cut"] = Utils.CheckDBNull.ToIntFromBool(item.state_cut);
                                    command.Parameters.Add("@state_cnc", DbType.Int16).Value = row["state_cnc"] = Utils.CheckDBNull.ToIntFromBool(item.state_cnc);
                                    command.Parameters.Add("@state_install", DbType.Int16).Value = row["state_install"] = Utils.CheckDBNull.ToIntFromBool(item.state_install);
                                    command.Parameters.Add("@date_preview", DbType.Int64).Value = row["date_preview"] = Utils.CheckDBNull.ToLong(item.date_preview);
                                    command.Parameters.Add("@path_preview", DbType.String).Value = row["path_preview"] = Utils.CheckDBNull.ToString(item.path_preview);
                                    command.Parameters.Add("@path_maket", DbType.String).Value = row["path_maket"] = Utils.CheckDBNull.ToString(item.path_maket);
                                    command.Parameters.Add("@change_count", DbType.Int64).Value = row["change_count"] = Utils.CheckDBNull.ToLong(item.change_count);
                                    command.Parameters.Add("@time_recieve", DbType.Int64).Value = row["time_recieve"] = Utils.CheckDBNull.ToLong(item.time_recieve);
                                    command.Parameters.Add("@worktypes_list", DbType.String).Value = row["worktypes_list"] = Utils.CheckDBNull.ToString(item.worktypes_list);

                                    command.Parameters.Add("@delivery", DbType.Int16).Value = row["delivery"] = Utils.CheckDBNull.ToIntFromBool(item.delivery);
                                    command.Parameters.Add("@delivery_office", DbType.Int16).Value = row["delivery_office"] = Utils.CheckDBNull.ToIntFromBool(item.delivery_office);
                                    command.Parameters.Add("@delivery_address", DbType.String).Value = row["delivery_address"] = Utils.CheckDBNull.ToString(item.delivery_address);
                                    command.Parameters.Add("@baner_handling", DbType.Int16).Value = row["baner_handling"] = Utils.CheckDBNull.ToIntFromBool(item.baner_handling);
                                    command.Parameters.Add("@baner_luvers", DbType.Int16).Value = row["baner_luvers"] = Utils.CheckDBNull.ToIntFromBool(item.baner_luvers);
                                    command.Parameters.Add("@baner_handling_size", DbType.Double).Value = row["baner_handling_size"] = Utils.CheckDBNull.ToLong(item.baner_handling_size);

                                    string yearpath = Utils.Settings.set.data_path + @"/" + "makets" + @"/" + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.time_recieve)).ToString("yyyy.MM");
                                    if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                                    string orderPath = Utils.Settings.set.data_path + @"/" + "makets" + @"/" + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(item.time_recieve)).ToString("yyyy.MM") + "/" + item.id.ToString("0000");
                                    if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }
                                    if (!Directory.Exists(orderPath + @"/" + "makets")) { Directory.CreateDirectory(orderPath + @"/" + "makets"); }
                                    if (!Directory.Exists(orderPath + @"/" + "preview")) { Directory.CreateDirectory(orderPath + @"/" + "preview"); }
                                    if (!Directory.Exists(orderPath + @"/" + "doc")) { Directory.CreateDirectory(orderPath + @"/" + "doc"); }
                                    if (!Directory.Exists(orderPath + @"/" + "photoreport")) { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }
                                    //row["be_read"] = true;
                                    if (item.preview != null)
                                    {
                                        try
                                        {
                                            using (FileStream fs = new FileStream(orderPath + @"/index.png", FileMode.Create, FileAccess.Write))
                                            {
                                                fs.Write(item.preview, 0, item.preview.Length);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        item.preview = new byte[1] { 0 };
                                    }
                                    if (item.installation_comment != null)
                                    {
                                        try
                                        {
                                            using (FileStream fs = new FileStream(orderPath + @"/montage.doc", FileMode.Create, FileAccess.Write))
                                            {
                                                fs.Write(item.installation_comment, 0, item.installation_comment.Length);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        item.installation_comment = new byte[1] { 0 };
                                    }
                                    //if (item.preview != null)
                                    //{
                                    //    command.Parameters.Add("@preview", DbType.Object, item.preview.Length).Value = row["preview"] = item.preview;
                                    //}else { command.Parameters.Add("@preview", DbType.Object).Value = row["preview"] = null; }
                                    //command.Parameters.Add("@preview", DbType.Object).Value = row["preview"] = null;
                                    command.ExecuteNonQuery();

                                    row["String_id"] = Convert.ToString(row["id"]);
                                    row["Datetime_date_start"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["date_start"]));
                                    row["Datetime_dead_line"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["dead_line"]));
                                    row["String_date_start"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["date_start"])).ToString("dd.MM.yyyy HH:mm");
                                    row["String_dead_line"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["dead_line"])).ToString("dd.MM.yyyy HH:mm");
                                    row["Datetime_date_ready_print"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["date_ready_print"]));
                                    row["Datetime_date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["date_ready_cut"]));
                                    row["Datetime_date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(row["date_ready_cnc"]));
                                    //StringBuilder sb = new StringBuilder();
                                    //if (Convert.ToBoolean(row["print_on"])) { sb.Append("• Печать"); }
                                    //if (Convert.ToBoolean(row["cut_on"]))
                                    //{
                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
                                    //    sb.Append("• Плот.резка");
                                    //}
                                    //if (Convert.ToBoolean(row["cnc_on"]))
                                    //{
                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
                                    //    sb.Append("• Фрезеровка");
                                    //}
                                    //if (Convert.ToBoolean(row["installation"]))
                                    //{
                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
                                    //    sb.Append("• Монтаж");
                                    //}
                                    //row["String_WorkTypes"] = sb.ToString();
                                    int print = 0;
                                    int cut = 0;
                                    int cnc = 0;
                                    int install = 0;
                                    int status = Convert.ToInt32(row["status"]);
                                    //if (Convert.ToBoolean(row["print_on"])) { if (Convert.ToBoolean(row["state_print"]) || status == 3) { print = 2; } else { print = 1; } }
                                    //if (Convert.ToBoolean(row["cut_on"])) { if (Convert.ToBoolean(row["state_cut"]) || status == 3) { cut = 2; } else { cut = 1; } }
                                    //if (Convert.ToBoolean(row["cnc_on"])) { if (Convert.ToBoolean(row["state_cnc"]) || status == 3) { cnc = 2; } else { cnc = 1; } }
                                    //if (Convert.ToBoolean(row["installation"])) { if (status == 3) { install = 2; } else { install = 1; } }
                                    if (Utils.CheckDBNull.ToBoolean(row["print_on"])) { if (Utils.CheckDBNull.ToBoolean(row["state_print"]) || status == 3 || status == 4) { print = 2; } else { print = 1; } }
                                    if (Utils.CheckDBNull.ToBoolean(row["cut_on"])) { if (Utils.CheckDBNull.ToBoolean(row["state_cut"]) || status == 3 || status == 4) { cut = 2; } else { cut = 1; } }
                                    if (Utils.CheckDBNull.ToBoolean(row["cnc_on"])) { if (Utils.CheckDBNull.ToBoolean(row["state_cnc"]) || status == 3 || status == 4) { cnc = 2; } else { cnc = 1; } }
                                    if (Utils.CheckDBNull.ToBoolean(row["installation"]))
                                    {
                                        if (Convert.ToBoolean(row["state_install"]) || status == 4) { install = 2; } else { install = 1; }
                                    }

                                    row["Image_WorkTypes"] = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);
                                    TableOrders.Rows.Add(row);
                                    OrdersDic.TryAdd(item.id, item);
                                    //if (SendChangeToSocket.IDsHashset.Contains(item.sender_row_stringid)) { Program.RecieveOrderFromServer(item); }
                                    SqlEvent.OnSENDOrderUpdate(new SqlEvent.OrderUpdateEventArgs(item.id, row.ItemArray, SocketClient.TableClient.SocketMessageCommand.RowsInsert, item.HistoryRows));
                                    Program.RecieveOrderFromServer(item);

                                }
                                #endregion
                            }


                            #region История
                            if (item.HistoryRows != null && item.HistoryRows.Count > 0) { OrderHistory.UpdateTable(item.HistoryRows); }
                            #endregion

                            if (counter2 == 100) { transaction.Commit(); }

                            if (breaking) { break; }
                            procentCurrent = Convert.ToInt32((counter * 100) / size);
                            if (procentCurrent > procentBefore)
                            {
                                procentBefore = procentCurrent; if (procentBefore > 100) { procentBefore = 100; }
                                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(procentBefore, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsProcess, (int)SqlEvent.TableName.TableOrders, true));
                                //OnSEND(new SqliteUpdateEventArgs(procentBefore, size, counter, SqLiteEvent.UpdateRowsProcess, TableName.customers, false));
                            }



                            //if (SendChangeToSocket.IDsHashset.Contains(item.sender_row_stringid)) { SendChangeToSocket.DeletedRow(item.sender_row_id); }


                        }
                        if (counter2 != 100) { transaction.Commit(); }

                        //}
                        //catch (Exception)
                        //{

                        //}
                        //finally
                        //{
                        //    if (connection != null) { connection.Close(); }
                        //}

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SqlLite.Order.UpdateTable", ex.Message);
                }
            }


            ////DataRow customerRow456 = CustomersArea.SqliteCustomers.TableCustomers.Select("id = " + 15).FirstOrDefault();
            if (!breaking)
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.UpdateRowsEnd, (int)SqlEvent.TableName.TableOrders, true));
            }


        }

        #endregion
        #region Изменение статусов заявок
        internal static void UpdateState(ProtoClasses.ProtoOrdersChangeState.protoRowsList uptab)
        {

            if (uptab != null && uptab.plist != null && uptab.plist.Count > 0)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {                                                                                                                   //try
                                                                                                                                        //{
                        connection.Open();
                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            foreach (ProtoClasses.ProtoOrdersChangeState.protoRow item in uptab.plist)
                            {
                                DataRow customerRow = TableOrders.Select("id = " + item.id).FirstOrDefault();
                                if (customerRow != null)
                                {
                                    #region Update
                                    try
                                    {
                                        using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `orders`  SET 
`status` = @state, 
`state_print` = @state_print, 
`state_cut` = @state_cut, 
`state_cnc` = @state_cnc, 
`state_install` = @state_install, 
`change_count` = @change_count, 
`printerman` = @printerman, 
`cutterman` = @cutterman, 
`cncman` = @cncman, 
`date_ready_print` = @date_ready_print, 
`date_ready_cut` = @date_ready_cut, 
`date_ready_cnc` = @date_ready_cnc 
WHERE `id` = @id ; ", connection))
                                        {
                                            command.Transaction = transaction;
                                            command.Parameters.Add("@id", DbType.Int64).Value = customerRow["id"] = Utils.CheckDBNull.ToLong(item.id);
                                            command.Parameters.Add("@state", DbType.Int32).Value = customerRow["status"] = Utils.CheckDBNull.ToInt(item.state);
                                            command.Parameters.Add("@state_print", DbType.Int16).Value = customerRow["state_print"] = Utils.CheckDBNull.ToIntFromBool(item.state_print);
                                            command.Parameters.Add("@state_cut", DbType.Int16).Value = customerRow["state_cut"] = Utils.CheckDBNull.ToIntFromBool(item.state_cut);
                                            command.Parameters.Add("@state_cnc", DbType.Int16).Value = customerRow["state_cnc"] = Utils.CheckDBNull.ToIntFromBool(item.state_cnc);
                                            command.Parameters.Add("@state_install", DbType.Int16).Value = customerRow["state_install"] = Utils.CheckDBNull.ToIntFromBool(item.state_install);
                                            command.Parameters.Add("@change_count", DbType.Int64).Value = customerRow["change_count"] = Utils.CheckDBNull.ToLong(item.change_count);
                                            command.Parameters.Add("@printerman", DbType.String).Value = customerRow["printerman"] = Utils.CheckDBNull.ToString(item.printerman);
                                            command.Parameters.Add("@cutterman", DbType.String).Value = customerRow["cutterman"] = Utils.CheckDBNull.ToString(item.cutterman);
                                            command.Parameters.Add("@cncman", DbType.String).Value = customerRow["cncman"] = Utils.CheckDBNull.ToString(item.cncman);
                                            command.Parameters.Add("@date_ready_print", DbType.Int64).Value = customerRow["date_ready_print"] = Utils.CheckDBNull.ToLong(item.date_ready_print);
                                            command.Parameters.Add("@date_ready_cut", DbType.Int64).Value = customerRow["date_ready_cut"] = Utils.CheckDBNull.ToLong(item.date_ready_cut);
                                            command.Parameters.Add("@date_ready_cnc", DbType.Int64).Value = customerRow["date_ready_cnc"] = Utils.CheckDBNull.ToLong(item.date_ready_cnc);
                                            command.ExecuteNonQuery();

                                            int print = 0;
                                            int cut = 0;
                                            int cnc = 0;
                                            int install = 0;
                                            int status = Convert.ToInt32(customerRow["status"]);
                                            if (Utils.CheckDBNull.ToBoolean(customerRow["print_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_print"]) || status == 3 || status == 4) { print = 2; } else { print = 1; } }
                                            if (Utils.CheckDBNull.ToBoolean(customerRow["cut_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_cut"]) || status == 3 || status == 4) { cut = 2; } else { cut = 1; } }
                                            if (Utils.CheckDBNull.ToBoolean(customerRow["cnc_on"])) { if (Utils.CheckDBNull.ToBoolean(customerRow["state_cnc"]) || status == 3 || status == 4) { cnc = 2; } else { cnc = 1; } }
                                            if (Utils.CheckDBNull.ToBoolean(customerRow["installation"]))
                                            {
                                                if (Convert.ToBoolean(customerRow["state_install"]) || status == 4) { install = 2; } else { install = 1; }
                                            }

                                            item.Image_WorkTypes = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);
                                            customerRow["Image_WorkTypes"] = item.Image_WorkTypes;
                                            //if (!Deletesendingrow && SendChangeToSocket.IDsHashset.Contains(item.sender_row_stringid)) { Deletesendingrow = true;  Sender_row_stringid = item.sender_row_stringid; Sender_row_id = item.sender_row_id; }
                                            //{ Program.RecieveOrderFromServer(item); }
                                            //SqlEvent.OnSENDOrderUpdate(new SqlEvent.OrderUpdateEventArgs(item.id, customerRow.ItemArray, SocketClient.TableClient.SocketMessageCommand.RowsUpdate, item.HistoryRows));
                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                    #endregion
                                    OrdersDic[item.id].state = Utils.CheckDBNull.ToInt32(item.state);
                                    OrdersDic[item.id].state_print = Utils.CheckDBNull.ToBoolean(item.state_print);
                                    OrdersDic[item.id].state_cut = Utils.CheckDBNull.ToBoolean(item.state_cut);
                                    OrdersDic[item.id].state_cnc = Utils.CheckDBNull.ToBoolean(item.state_cnc);
                                    OrdersDic[item.id].state_install = Utils.CheckDBNull.ToBoolean(item.state_install);
                                    OrdersDic[item.id].printerman = Utils.CheckDBNull.ToString(item.printerman);
                                    OrdersDic[item.id].cutterman = Utils.CheckDBNull.ToString(item.cutterman);
                                    OrdersDic[item.id].cncman = Utils.CheckDBNull.ToString(item.cncman);
                                    OrdersDic[item.id].date_ready_print = Utils.CheckDBNull.ToLong(item.date_ready_print);
                                    OrdersDic[item.id].date_ready_cut = Utils.CheckDBNull.ToLong(item.date_ready_cut);
                                    OrdersDic[item.id].date_ready_cnc = Utils.CheckDBNull.ToLong(item.date_ready_cnc);
                                }
                            }
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SqlLite.Order.UpdateTable", ex.Message);
                }
                //if (Deletesendingrow) { Program.RecieveOrderFromServer(new ProtoClasses.ProtoOrders.protoOrder() { sender_row_id = Sender_row_id, sender_row_stringid = Sender_row_stringid, adder =  }); }
                #region История
                if (uptab.HistoryRows != null && uptab.HistoryRows.Count > 0) { OrderHistory.UpdateTable(uptab.HistoryRows); }
                #endregion
                //Program.SendOrderStatusChange(new Program.OrderSatusChangeEventArgs());
                //if (SendChangeToSocket.IDsHashset.Contains(uptab.sender_row_stringid))
                //{
                //    Program.RecieveOrderFromServer(new ProtoClasses.ProtoOrders.protoOrder()
                //    {
                //        sender_row_id = uptab.sender_row_id,
                //        sender_row_stringid = uptab.sender_row_stringid,
                //        adder = uptab.adder,
                //        command = (int)SocketClient.TableClient.SocketMessageCommand.OrderChangeStates,
                //        work_name = uptab.name,
                //        comments = uptab.notes
                //    });
                //}

                SqlEvent.OnSENDOrderChangeStates(uptab);
                Program.RecieveOrderFromServer(new ProtoClasses.ProtoOrders.protoOrder()
                {
                    sender_row_id = uptab.sender_row_id,
                    sender_row_stringid = uptab.sender_row_stringid,
                    adder = uptab.adder,
                    command = (int)SocketClient.TableClient.SocketMessageCommand.OrderChangeStates,
                    work_name = uptab.name,
                    comments = uptab.notes
                });
            }
        }
        #endregion

    }
}
