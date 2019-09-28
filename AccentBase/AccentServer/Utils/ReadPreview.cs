using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
namespace AccentServer.Utils
{
    static class ReadPreview
    {


        #region SQLite -> datatable
        internal static void ScanFiles()
        {
            Thread thread = new Thread(FullScan)
            {
                Name = "FullScan",
                IsBackground = true
            };
            thread.Start();
        }
        #endregion
        public delegate void LoadHandler(object sender, bool e);
        public static event LoadHandler LoadEvent;
        static public void OnSEND(bool e)
        {
            LoadEvent?.Invoke(null, e);
        }

        static private void FullScan()
        {
            MySql.DataTables.preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
            DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
            foreach (string datedir in Directory.GetDirectories(Properties.Settings.Default.FilePath))
            {
                foreach (string ordirs in Directory.GetDirectories(datedir))
                {
                    FileInfo fileInf = new FileInfo(ordirs + @"/" + "index.png");
                    if (fileInf.Exists)
                    {
                        int i = 0;
                        if (int.TryParse((new DirectoryInfo(ordirs).Name), out i))
                        {
                            ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow();
                            pr.fullname = fileInf.FullName.Replace(di.FullName+@"\", "");
                            pr.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime);
                            pr.Length = fileInf.Length;
                            pr.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime);
                            pr.order_id = i;
                            MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                        }
                    }
                    fileInf = new FileInfo(ordirs + @"/" + "montage.doc");
                    if (fileInf.Exists)
                    {
                        int i = 0;
                        if (int.TryParse((new DirectoryInfo(ordirs).Name), out i))
                        {
                            ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow();
                            pr.fullname = fileInf.FullName.Replace(di.FullName + @"\", "");
                            pr.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime);
                            pr.Length = fileInf.Length;
                            pr.LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime);
                            pr.order_id = i;
                            MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                        }
                    }
                }
            }
            OnSEND(true);
        }
    }
}
