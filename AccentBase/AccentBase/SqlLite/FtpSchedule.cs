using System;
//using System.Threading;
//using System.Collections.Concurrent;
//using System.Diagnostics;
//using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
using System.Data;
//using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace AccentBase.SqlLite
{
    internal static class FtpSchedule
    {

        static FtpSchedule()
        {

        }
        //#region For Event
        //public enum TableName
        //{
        //    WorkTypes = 0,
        //    StockFolders = 1,
        //    StockBlocks = 2,
        //    StockUnits = 3
        //}


        //public class SqliteUpdateEventArgs : EventArgs
        //{
        //    public TableName tablename { get; set; }
        //    //public bool comlete { get; set; }
        //    public Exception ex { get; set; }
        //    public SqliteUpdateEventArgs(TableName tableName, Exception Ex) { tablename = tableName; ex = Ex; }
        //}

        //public delegate void UpdateEventHandler(object sender, SqliteUpdateEventArgs e);
        //public static event UpdateEventHandler UpdateEvent;
        //static void OnSEND(SqliteUpdateEventArgs e)
        //{
        //    //e.guiDataBase = GuiDataBase;
        //    UpdateEvent?.Invoke(null, e);
        //}
        //#endregion
        public class FTPListArgs
        {
            public long id { get; set; }
            public string sourcefile { get; set; }
            public string targetfile { get; set; }
            public long Length { get; set; }
            public long LastWriteTime { get; set; }
            public long LastCreationTime { get; set; }
            public bool Upload { get; set; }
            public int filestatus { get; set; }
            public string notes { get; set; }
            public int processprogress { get; set; }
            public string fileshortname { get; set; }
            public string serveraddress { get; set; }
            public string conspeed { get; set; }
            public long order_id { get; set; }
            public string LengthString { get; set; }
            public string ETA { get; set; }
        }


        public static ConcurrentDictionary<long, FTPListArgs> FTPListDic { get; set; }
        //static internal DataTable FtpFilesList { get; set; }
        //static internal ConcurrentDictionary<Int64, ProtoClasses.ProtoFtpSchedule.protoRow> preview { get; set; }

        internal static DataTable FTPList { get; set; }

        private static readonly string path = Utils.Settings.set.data_path + @"\data";
        private static readonly string name = @"\ftpfileslist.dat";
        internal static bool breaking { get; set; }

        #region Создание базы
        private static void CreateTable()
        {
            if (FTPListDic == null) { FTPListDic = new ConcurrentDictionary<long, FTPListArgs>(); }
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            if (!File.Exists(path + name))
            {
                SQLiteConnection.CreateFile(path + name);

                using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
                {
                    connection.Open();
                    string commandstring = @"CREATE TABLE `ftplist` (
	                                        `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                                        `sourcefile`	TEXT,
	                                        `targetfile`	TEXT,
	                                        `Length`	INTEGER NOT NULL DEFAULT 0,
	                                        `LengthString`	TEXT,
	                                        `LastWriteTime`	INTEGER NOT NULL DEFAULT 0,
	                                        `LastCreationTime`	INTEGER NOT NULL DEFAULT 0,
                                        	`upload`	INTEGER NOT NULL DEFAULT 0,
                                        	`filestatus`	INTEGER NOT NULL DEFAULT 0,
	                                        `notes`	TEXT,
	                                        `processprogress`	INTEGER NOT NULL DEFAULT 0,
	                                        `fileshortname`	TEXT,
	                                        `serveraddress`	TEXT,
	                                        `conspeed`	TEXT,
	                                        `order_id`	INTEGER NOT NULL DEFAULT 0,
	                                        `ETA`	TEXT
                                            );";
                    using (SQLiteCommand command = new SQLiteCommand(commandstring, connection))
                    {
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
        }
        #endregion

        #region Select
        public static void GetTable()
        {
            if (!File.Exists(path + name)) { CreateTable(); }
            if (FTPListDic == null) { FTPListDic = new ConcurrentDictionary<long, FTPListArgs>(); }
            using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
            {
                connection.Open();
                if (FTPList == null)
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM ftplist", connection))
                    {
                        FTPList = new DataTable();
                        adapter.AcceptChangesDuringFill = false;
                        adapter.Fill(FTPList);
                    }
                    FTPList.Columns.Add("typeicon", typeof(byte[]));
                    byte[] upload = Utils.Converting.ImageToByte(Properties.Resources.Upload_32x32);
                    byte[] download = Utils.Converting.ImageToByte(Properties.Resources.Download_32x32);
                    foreach (DataRow row in FTPList.Rows)
                    {
                        if (Utils.CheckDBNull.ToBoolean(row["upload"])) { row["typeicon"] = upload; } else { row["typeicon"] = download; }

                        FTPListArgs rw = new FTPListArgs
                        {
                            id = Utils.CheckDBNull.ToLong(row["id"]),
                            sourcefile = Utils.CheckDBNull.ToString(row["sourcefile"]),
                            targetfile = Utils.CheckDBNull.ToString(row["targetfile"]),
                            Length = Utils.CheckDBNull.ToLong(row["Length"]),
                            LastWriteTime = Utils.CheckDBNull.ToLong(row["LastWriteTime"]),
                            LastCreationTime = Utils.CheckDBNull.ToLong(row["LastCreationTime"]),
                            Upload = Utils.CheckDBNull.ToBoolean(row["Upload"]),
                            filestatus = Utils.CheckDBNull.ToInt32(row["filestatus"]),
                            notes = Utils.CheckDBNull.ToString(row["notes"]),
                            processprogress = Utils.CheckDBNull.ToInt32(row["processprogress"]),
                            fileshortname = Utils.CheckDBNull.ToString(row["fileshortname"]),
                            serveraddress = Utils.CheckDBNull.ToString(row["serveraddress"]),
                            conspeed = Utils.CheckDBNull.ToString(row["conspeed"]),
                            order_id = Utils.CheckDBNull.ToLong(row["order_id"]),
                            LengthString = Utils.CheckDBNull.ToString(row["LengthString"]),
                            ETA = Utils.CheckDBNull.ToString(row["ETA"])
                        };
                        FTPListDic.TryAdd(rw.id, rw);
                    }
                }

            }
            FTPList.DefaultView.Sort = "id ASC";
            //using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))
            //{
            //    connection.Open();
            //    using (SQLiteCommand fmd = connection.CreateCommand())
            //    {
            //        fmd.CommandText = @"SELECT* FROM ftplist";
            //        fmd.CommandType = CommandType.Text;
            //        SQLiteDataReader r = fmd.ExecuteReader();
            //        while (r.Read())
            //        {
            //            ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow();
            //            pr.id = Utils.CheckDBNull.ToInt32(r["id"]);
            //            pr.sourcefile = Utils.CheckDBNull.ToString(r["sourcefile"]);
            //            pr.targetfile = Utils.CheckDBNull.ToString(r["targetfile"]);
            //            pr.Length = Utils.CheckDBNull.ToLong(r["Length"]);
            //            pr.Length = Utils.CheckDBNull.ToLong(r["LastWriteTime"]);
            //            pr.Length = Utils.CheckDBNull.ToLong(r["LastCreationTime"]);
            //            pr.Upload = Utils.CheckDBNull.ToBoolean(r["upload"]);
            //            pr.notes = Utils.CheckDBNull.ToString(r["notes"]);
            //            pr.filestatus = Utils.CheckDBNull.ToInt32(r["filestatus"]);
            //            preview.TryAdd(pr.id, pr);
            //        }
            //    }
            //}
        }
        #endregion

        #region Insert
        public static void Insert(ProtoClasses.ProtoFtpSchedule.protoRow item)
        {
            if (!File.Exists(path + name)) { CreateTable(); }
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                {
                    connection.Open();
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO ftplist (
                    sourcefile, 
                    targetfile, 
                    Length, 
                    LastWriteTime, 
                    LastCreationTime, 
                    upload, 
                    notes,
                    filestatus,
                    processprogress,
                    fileshortname,
                    serveraddress,
                    LengthString,
                    conspeed,
                    order_id
                    ) VALUES(
                    @sourcefile, 
                    @targetfile, 
                    @Length, 
                    @LastWriteTime, 
                    @LastCreationTime, 
                    @upload, 
                    @notes,
                    @filestatus,
                    @processprogress,
                    @fileshortname,
                    @serveraddress,
                    @LengthString,
                    @conspeed,
                    @order_id
                    ); ", connection))
                        {
                            DataRow dr = FTPList.NewRow();
                            FTPListArgs rw = new FTPListArgs();
                            command.Transaction = transaction;
                            command.Parameters.Add("@sourcefile", DbType.String).Value = dr["sourcefile"] = rw.sourcefile = Utils.CheckDBNull.ToString(item.sourcefile);
                            command.Parameters.Add("@targetfile", DbType.String).Value = dr["targetfile"] = rw.targetfile = Utils.CheckDBNull.ToString(item.targetfile);
                            command.Parameters.Add("@Length", DbType.Int64).Value = dr["Length"] = rw.Length = Utils.CheckDBNull.ToLong(item.Length);
                            command.Parameters.Add("@LastWriteTime", DbType.Int64).Value = dr["LastWriteTime"] = rw.LastWriteTime = Utils.CheckDBNull.ToLong(item.LastWriteTime);
                            command.Parameters.Add("@LastCreationTime", DbType.Int64).Value = dr["LastCreationTime"] = rw.LastCreationTime = Utils.CheckDBNull.ToLong(item.LastCreationTime);
                            command.Parameters.Add("@upload", DbType.Int32).Value = dr["upload"] = Utils.CheckDBNull.ToInt32(item.Upload); rw.Upload = item.Upload;
                            command.Parameters.Add("@notes", DbType.String).Value = dr["notes"] = rw.notes = Utils.CheckDBNull.ToString(item.notes);
                            command.Parameters.Add("@filestatus", DbType.Int32).Value = dr["filestatus"] = rw.filestatus = Utils.CheckDBNull.ToInt32(item.filestatus);
                            command.Parameters.Add("@processprogress", DbType.Int32).Value = dr["processprogress"] = rw.processprogress = Utils.CheckDBNull.ToInt32(item.processprogress);
                            command.Parameters.Add("@fileshortname", DbType.String).Value = dr["fileshortname"] = rw.fileshortname = Utils.CheckDBNull.ToString(item.fileshortname);
                            command.Parameters.Add("@serveraddress", DbType.String).Value = dr["serveraddress"] = rw.serveraddress = Utils.CheckDBNull.ToString(item.serveraddress);
                            command.Parameters.Add("@conspeed", DbType.String).Value = dr["conspeed"] = rw.conspeed = Utils.CheckDBNull.ToString(item.conspeed);
                            command.Parameters.Add("@order_id", DbType.Int64).Value = dr["order_id"] = rw.order_id = Utils.CheckDBNull.ToLong(item.order_id);
                            command.Parameters.Add("@LengthString", DbType.String).Value = dr["LengthString"] = rw.LengthString = Utils.CheckDBNull.ToString(item.LengthString);
                            if (item.Upload) { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Upload_32x32); }
                            else { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Download_32x32); }
                            command.ExecuteNonQuery();
                            dr["id"] = rw.id = item.id = Convert.ToInt64(connection.LastInsertRowId);
                            FTPList.Rows.Add(dr);
                            if (!FTPListDic.ContainsKey(rw.id)) { FTPListDic.TryAdd(rw.id, rw); } else { FTPListDic[rw.id] = rw; }
                        }
                        transaction.Commit();
                    }
                    FtpProgressEvent(new Ftp.FtpReciever.FTPOperationEventArgs(item.order_id, false, 100, item.id, string.Empty, string.Empty, 0, string.Empty));
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Update



        public static void Update(ProtoClasses.ProtoFtpSchedule.protoRow item)
        {
            if (!File.Exists(path + name)) { CreateTable(); }
            try
            {
                DataRow dr = FTPList.Select("id = " + item.id).FirstOrDefault();
                if (dr != null)
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {
                        connection.Open();
                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `ftplist`  SET 
`sourcefile` = @sourcefile, 
`targetfile` = @targetfile, 
`Length` = @Length, 
`LastWriteTime` = @LastWriteTime, 
`LastCreationTime` = @LastCreationTime, 
`upload` = @upload, 
`notes` = @notes, 
`filestatus` = @filestatus,
`processprogress` = @processprogress,
`fileshortname` = @fileshortname,
`serveraddress` = @serveraddress,
`conspeed` = @conspeed, 
`order_id` = @order_id, 
`LengthString` = @LengthString 
WHERE `id` = @id ; ", connection))
                            {
                                FTPListArgs rw = new FTPListArgs();
                                command.Transaction = transaction;
                                command.Parameters.Add("@id", DbType.Int64).Value = dr["id"] = rw.id = Utils.CheckDBNull.ToLong(item.id);
                                command.Parameters.Add("@sourcefile", DbType.String).Value = dr["sourcefile"] = rw.sourcefile = Utils.CheckDBNull.ToString(item.sourcefile);
                                command.Parameters.Add("@targetfile", DbType.String).Value = dr["targetfile"] = rw.targetfile = Utils.CheckDBNull.ToString(item.targetfile);
                                command.Parameters.Add("@Length", DbType.Int64).Value = dr["Length"] = rw.Length = Utils.CheckDBNull.ToLong(item.Length);
                                command.Parameters.Add("@LastWriteTime", DbType.Int64).Value = dr["LastWriteTime"] = rw.LastWriteTime = Utils.CheckDBNull.ToLong(item.LastWriteTime);
                                command.Parameters.Add("@LastCreationTime", DbType.Int64).Value = dr["LastCreationTime"] = rw.LastCreationTime = Utils.CheckDBNull.ToLong(item.LastCreationTime);
                                command.Parameters.Add("@upload", DbType.Int32).Value = dr["upload"] = Utils.CheckDBNull.ToInt32(item.Upload); rw.Upload = item.Upload;
                                command.Parameters.Add("@notes", DbType.String).Value = dr["notes"] = rw.notes = Utils.CheckDBNull.ToString(item.notes);
                                command.Parameters.Add("@filestatus", DbType.Int32).Value = dr["filestatus"] = rw.filestatus = Utils.CheckDBNull.ToInt32(item.filestatus);
                                command.Parameters.Add("@processprogress", DbType.Int32).Value = dr["processprogress"] = rw.processprogress = Utils.CheckDBNull.ToInt32(item.processprogress);
                                command.Parameters.Add("@fileshortname", DbType.String).Value = dr["fileshortname"] = rw.fileshortname = Utils.CheckDBNull.ToString(item.fileshortname);
                                command.Parameters.Add("@serveraddress", DbType.String).Value = dr["serveraddress"] = rw.serveraddress = Utils.CheckDBNull.ToString(item.serveraddress);
                                command.Parameters.Add("@conspeed", DbType.String).Value = dr["conspeed"] = rw.conspeed = Utils.CheckDBNull.ToString(item.conspeed);
                                command.Parameters.Add("@order_id", DbType.Int64).Value = dr["order_id"] = rw.order_id = Utils.CheckDBNull.ToLong(item.order_id);
                                command.Parameters.Add("@LengthString", DbType.String).Value = dr["LengthString"] = rw.LengthString = Utils.CheckDBNull.ToString(item.LengthString);
                                if (item.Upload) { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Upload_32x32); }
                                else { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Download_32x32); }
                                command.ExecuteNonQuery();
                                if (FTPListDic.ContainsKey(rw.id)) { FTPListDic[rw.id] = rw; } else { }
                            }
                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region Delete
        public static void Delete(long uid)
        {
            if (!File.Exists(path + name)) { CreateTable(); }
            try
            {
                DataRow dr = FTPList.Select("id = " + uid).FirstOrDefault();
                if (dr != null)
                {
                    long order_id = Utils.CheckDBNull.ToLong(dr["order_id"]);
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {
                        connection.Open();
                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            using (SQLiteCommand command = new SQLiteCommand("DELETE FROM ftplist WHERE id = @ID ; ", connection))
                            {
                                command.Parameters.Add("@ID", DbType.Int64).Value = uid;
                                command.Transaction = transaction;
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            dr.Delete();
                            FTPListArgs rw = new FTPListArgs();
                            if (FTPListDic.ContainsKey(uid)) { FTPListDic.TryRemove(uid, out rw); }
                            //FTPList.Rows.Remove
                            //ProtoClasses.ProtoFtpSchedule.protoRow rv = new ProtoClasses.ProtoFtpSchedule.protoRow();
                            //preview.TryRemove(uid, out rv);
                        }
                    }
                    FtpProgressEvent(new Ftp.FtpReciever.FTPOperationEventArgs(order_id, true, 0, uid, string.Empty, string.Empty, 100, string.Empty));
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region
        //static async void UpdatePreviewAsync(List<ProtoClasses.ProtoFtpSchedule.protoRow> previewlist)
        //{
        //    await Task.Run(() => UpdatePreview(List <ProtoClasses.ProtoFtpSchedule.protoRow> previewlist));
        //}

        public class PreviewlistSQL : EventArgs
        {
            public bool Ended { get; set; }
            public int ProcessCount { get; set; }
            public Exception Ex { get; set; }
            public PreviewlistSQL(bool ended, int processcount, Exception ex)
            { Ended = ended; ProcessCount = processcount; Ex = ex; }
        }
        public delegate void PreviewSqlUpdate(object sender, PreviewlistSQL e);
        public static event PreviewSqlUpdate PreviewUpdate;
        private static void PreviewSqlUpdateProcess(PreviewlistSQL e) { PreviewUpdate?.Invoke(null, e); }


        public static void UpdatePreview(List<ProtoClasses.ProtoFtpSchedule.protoRow> previewlist)
        {
            if (!File.Exists(path + name)) { CreateTable(); }
            if (previewlist != null && previewlist.Count > 0)
            {
                int i = 0;
                int max = previewlist.Count;
                int procentage = 0;
                int oldprocentage = 0;
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source =" + path + name + " ;Version=3;"))// Pooling=True; Max Pool Size=100;");
                    {
                        connection.Open();
                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            foreach (ProtoClasses.ProtoFtpSchedule.protoRow item in previewlist)
                            {
                                FileInfo fi = new FileInfo(item.sourcefile);
                                string shortname = "Предпросмотр заявки №" + fi.Directory.Name;
                                DataRow drold = FTPList.Select("sourcefile = '" + item.sourcefile + "'").FirstOrDefault();
                                if (drold != null)
                                {
                                    //                                    using (SQLiteCommand command = new SQLiteCommand(@"UPDATE `ftplist`  SET 
                                    //`sourcefile` = @sourcefile, 
                                    //`targetfile` = @targetfile, 
                                    //`Length` = @Length, 
                                    //`LastWriteTime` = @LastWriteTime, 
                                    //`LastCreationTime` = @LastCreationTime, 
                                    //`upload` = @upload, 
                                    //`notes` = @notes, 
                                    //`filestatus` = @filestatus,
                                    //`processprogress` = @processprogress,
                                    //`fileshortname` = @fileshortname,
                                    //`serveraddress` = @serveraddress,
                                    //`conspeed` = @conspeed, 
                                    //`LengthString` = @LengthString 
                                    //WHERE `id` = @id ; ", connection))
                                    //                                    {

                                    //                                        command.Transaction = transaction;
                                    //                                        command.Parameters.Add("@id", DbType.Int64).Value = Utils.CheckDBNull.ToLong(drold["id"]);
                                    //                                        command.Parameters.Add("@sourcefile", DbType.String).Value = drold["sourcefile"] = Utils.CheckDBNull.ToString(item.sourcefile);
                                    //                                        command.Parameters.Add("@targetfile", DbType.String).Value = drold["targetfile"] = Utils.CheckDBNull.ToString(item.targetfile);
                                    //                                        command.Parameters.Add("@Length", DbType.Int64).Value = drold["Length"] = Utils.CheckDBNull.ToLong(item.Length);
                                    //                                        command.Parameters.Add("@LastWriteTime", DbType.Int64).Value = drold["LastWriteTime"] = Utils.CheckDBNull.ToLong(item.LastWriteTime);
                                    //                                        command.Parameters.Add("@LastCreationTime", DbType.Int64).Value = drold["LastCreationTime"] = Utils.CheckDBNull.ToLong(item.LastCreationTime);
                                    //                                        command.Parameters.Add("@upload", DbType.Int32).Value = drold["upload"] = Utils.CheckDBNull.ToInt32(item.Upload);
                                    //                                        command.Parameters.Add("@notes", DbType.String).Value = drold["notes"] = Utils.CheckDBNull.ToString(item.notes);
                                    //                                        command.Parameters.Add("@filestatus", DbType.Int32).Value = drold["filestatus"] = Utils.CheckDBNull.ToInt32(item.filestatus);
                                    //                                        command.Parameters.Add("@processprogress", DbType.Int32).Value = drold["processprogress"] = Utils.CheckDBNull.ToInt32(item.processprogress);
                                    //                                        command.Parameters.Add("@fileshortname", DbType.String).Value = drold["fileshortname"] = shortname;//Utils.CheckDBNull.ToString(item.fileshortname);
                                    //                                        command.Parameters.Add("@serveraddress", DbType.String).Value = drold["serveraddress"] = Utils.CheckDBNull.ToString(item.serveraddress);
                                    //                                        command.Parameters.Add("@conspeed", DbType.String).Value = drold["conspeed"] = Utils.CheckDBNull.ToString(item.conspeed);
                                    //                                        command.Parameters.Add("@LengthString", DbType.String).Value = drold["LengthString"] = Utils.CheckDBNull.ToString(item.LengthString);
                                    //                                        if (item.Upload) { drold["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Upload_32x32); }
                                    //                                        else { drold["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Download_32x32); }
                                    //                                        command.ExecuteNonQuery();
                                    //                                    }
                                }
                                else
                                {

                                    using (SQLiteCommand command = new SQLiteCommand(@"INSERT INTO ftplist (
                    sourcefile, 
                    targetfile, 
                    Length, 
                    LastWriteTime, 
                    LastCreationTime, 
                    upload, 
                    notes,
                    filestatus,
                    processprogress,
                    fileshortname,
                    serveraddress,
                    LengthString,
                    conspeed, 
                    order_id
                    ) VALUES(
                    @sourcefile, 
                    @targetfile, 
                    @Length, 
                    @LastWriteTime, 
                    @LastCreationTime, 
                    @upload, 
                    @notes,
                    @filestatus,
                    @processprogress,
                    @fileshortname,
                    @serveraddress,
                    @LengthString,
                    @conspeed, 
                    @order_id 
                    ); ", connection))
                                    {
                                        DataRow dr = FTPList.NewRow();
                                        FTPListArgs rw = new FTPListArgs();
                                        command.Transaction = transaction;
                                        command.Parameters.Add("@sourcefile", DbType.String).Value = dr["sourcefile"] = rw.sourcefile = Utils.CheckDBNull.ToString(item.sourcefile);
                                        command.Parameters.Add("@targetfile", DbType.String).Value = dr["targetfile"] = rw.targetfile = Utils.CheckDBNull.ToString(item.targetfile);
                                        command.Parameters.Add("@Length", DbType.Int64).Value = dr["Length"] = rw.Length = Utils.CheckDBNull.ToLong(item.Length);
                                        command.Parameters.Add("@LastWriteTime", DbType.Int64).Value = dr["LastWriteTime"] = rw.LastWriteTime = Utils.CheckDBNull.ToLong(item.LastWriteTime);
                                        command.Parameters.Add("@LastCreationTime", DbType.Int64).Value = dr["LastCreationTime"] = rw.LastCreationTime = Utils.CheckDBNull.ToLong(item.LastCreationTime);
                                        command.Parameters.Add("@upload", DbType.Int32).Value = dr["upload"] = Utils.CheckDBNull.ToInt32(item.Upload); rw.Upload = item.Upload;
                                        command.Parameters.Add("@notes", DbType.String).Value = dr["notes"] = rw.notes = Utils.CheckDBNull.ToString(item.notes);
                                        command.Parameters.Add("@filestatus", DbType.Int32).Value = dr["filestatus"] = rw.filestatus = Utils.CheckDBNull.ToInt32(item.filestatus);
                                        command.Parameters.Add("@processprogress", DbType.Int32).Value = dr["processprogress"] = rw.processprogress = Utils.CheckDBNull.ToInt32(item.processprogress);
                                        command.Parameters.Add("@fileshortname", DbType.String).Value = dr["fileshortname"] = rw.fileshortname = shortname;//Utils.CheckDBNull.ToString(item.fileshortname);
                                        command.Parameters.Add("@serveraddress", DbType.String).Value = dr["serveraddress"] = rw.serveraddress = Utils.CheckDBNull.ToString(item.serveraddress);
                                        command.Parameters.Add("@conspeed", DbType.String).Value = dr["conspeed"] = rw.conspeed = Utils.CheckDBNull.ToString(item.conspeed);
                                        command.Parameters.Add("@LengthString", DbType.String).Value = dr["LengthString"] = rw.LengthString = Utils.CheckDBNull.ToString(item.LengthString);
                                        command.Parameters.Add("@order_id", DbType.Int64).Value = dr["order_id"] = rw.order_id = Utils.CheckDBNull.ToLong(item.order_id);
                                        if (item.Upload) { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Upload_32x32); }
                                        else { dr["typeicon"] = Utils.Converting.ImageToByte(Properties.Resources.Download_32x32); }
                                        command.ExecuteNonQuery();
                                        dr["id"] = item.id = rw.id = Convert.ToInt64(connection.LastInsertRowId);
                                        FTPList.Rows.Add(dr);
                                        if (!FTPListDic.ContainsKey(rw.id)) { FTPListDic.TryAdd(rw.id, rw); } else { FTPListDic[rw.id] = rw; }
                                    }
                                }


                                i++;
                                procentage = (i * 100) / max;
                                if (oldprocentage < procentage)
                                {
                                    oldprocentage = procentage;
                                    PreviewSqlUpdateProcess(new PreviewlistSQL(false, oldprocentage, null));
                                }


                            }

                            transaction.Commit();
                        }

                        //foreach (ProtoClasses.ProtoFtpSchedule.protoRow item in previewlist)
                        //{
                        //    FileInfo fi = new FileInfo(item.sourcefile);
                        //    string shortname = "Предпросмотр заявки №" + fi.Directory.Name;
                        //    DataRow drold = FTPList.Select("sourcefile = '" + item.sourcefile + "'").FirstOrDefault();
                        //}
                    }
                }
                catch (Exception ex)
                {
                    PreviewSqlUpdateProcess(new PreviewlistSQL(false, 0, ex));
                }
            }
            PreviewSqlUpdateProcess(new PreviewlistSQL(true, 100, null));
        }
        #endregion



        public static void SetProgress(Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            FtpProgressEvent(e);
            try
            {
                DataRow dr = FTPList.Select("id = " + e.Id).SingleOrDefault();
                if (dr != null)
                {
                    //if (dr)
                    //dr["conspeed"] = e.TransferSpeed;
                    //dr["processprogress"] = e.ProcessCount;
                    //dr["ETA"] = e.ETA;
                    //if (e.Ex != null) { dr[""] = e.Ex.Message; }
                    FtpProgressEvent(e);

                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void AddFtpEvent(Ftp.FtpReciever ft)
        {
            ft.Event_FTPOperation += Ft_Event_FTPOperation;
            //this.BeginInvoke(new MethodInvoker(() =>
            //{
            //    moveRowTo(dataGridView2, 0, 1);
            //}));
        }
        public static void RemoveFtpEvent(Ftp.FtpReciever ft)
        {
            ft.Event_FTPOperation -= Ft_Event_FTPOperation;
        }

        //public Delegate FTPOperationDelegate
        private static void Ft_Event_FTPOperation(object sender, Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            //Delegate_FTPOperation.I
            //Invoke
            if (e.Ended)
            {




                //FtpSchedule.Invoke();
                //SetProgress(e);
                Delete(e.Id);
            }
            else
            {
                SetProgress(e);
            }
            //if (e != null)
            //{

            //}
            //try
            //{
            //    if (e != null) { BeginInvoke(new SqliteDelegate(SetProgress), e); }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        public delegate void Delegate_FTPOperation(object sender, Ftp.FtpReciever.FTPOperationEventArgs e);
        public static event Delegate_FTPOperation Event_FTPOperation;
        private static void FtpProgressEvent(Ftp.FtpReciever.FTPOperationEventArgs e) { Event_FTPOperation?.Invoke(null, e); }
        //internal delegate void SqliteDelegate(Ftp.FtpReciever.FTPOperationEventArgs e);
    }
}

