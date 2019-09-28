using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace AccentBase.Ftp
{
    static class FtpPreviewRecieve
    {
        public static ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> preview { get; set; }

        #region Составление списка локальных превью
        public delegate void PreviewListEventHandler(object sender, ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> e);
        public static event PreviewListEventHandler PreviewlistComeEvent;
        static private void PreviewListEvent(ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> e) { PreviewlistComeEvent?.Invoke(null, e); }

        static Thread thread;
        //static internal bool breaking { get; set; }
        static public void PreviewsScan()       
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Utils.Settings.set.data_path);
            if (!directoryInfo.Exists) { directoryInfo.Create(); }
            directoryInfo = new DirectoryInfo(Utils.Settings.set.data_path + @"\makets\");
            if (!directoryInfo.Exists) { directoryInfo.Create(); }
            string qqq = Utils.Settings.set.data_path + @"\makets\";

            //breaking = false;
            thread = new Thread(FullScan);
            thread.Name = "AccentBase ScanPreviews";
            thread.IsBackground = true;
            thread.Start(qqq);
        }



    static private void FullScan(object qqq)
        {
            string initpath = qqq as string;
            ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow> preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
            DirectoryInfo di = new DirectoryInfo(initpath);
            foreach (string datedir in Directory.GetDirectories(initpath))
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
                            pr.fullname = fileInf.FullName.Replace(di.FullName, "");
                            pr.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime);
                            pr.Length = fileInf.Length;
                            pr.order_id = i;
                            preview.TryAdd(pr.fullname, pr);
                            //Trace.WriteLine(pr.fullname + " <<>> "+ di.FullName);
                        }
                    }
                    fileInf = new FileInfo(ordirs + @"/" + "montage.doc");
                    if (fileInf.Exists)
                    {
                        int i = 0;
                        if (int.TryParse((new DirectoryInfo(ordirs).Name), out i))
                        {
                            ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow();
                            pr.fullname = fileInf.FullName.Replace(di.FullName, "");
                            pr.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime);
                            pr.Length = fileInf.Length;
                            pr.order_id = i;
                            preview.TryAdd(pr.fullname, pr);
                            //Trace.WriteLine(pr.fullname + " <<>> "+ di.FullName);
                        }
                    }
                }
            }
            PreviewListEvent(preview);
        }
        #endregion

        #region Составление ФТП-списка для скачивания превью

        static Thread threadtwo;
        static public bool working { get; set; }
        static public void Stop()
        {
            working = false;
        }

        public class PreviewlistComeEventArgs : EventArgs
        {
            public string BaseFolder { get; set; }
            public bool Ended { get; set; }
            public List<ProtoClasses.ProtoPreview.protoRow> InitialList { get; set; }
            public List<ProtoClasses.ProtoFtpSchedule.protoRow> ResultList { get; set; }
            //public List<ProtoClasses.ProtoPreview.protoRow> ResultList { get; set; }
            public int ProcessCount { get; set; }
            public Exception Ex { get; set; }
            public PreviewlistComeEventArgs(bool ended, List<ProtoClasses.ProtoPreview.protoRow> initialList, List<ProtoClasses.ProtoFtpSchedule.protoRow> resultList, int processcount, Exception ex)
            { Ended = ended; InitialList = initialList; ResultList = resultList; ProcessCount = processcount; Ex = ex; }
        }
        public delegate void comparePreviewStopped(object sender, PreviewlistComeEventArgs e);
        public static event comparePreviewStopped ComparePreviewStopped;
        static private void StopFtpDaemon(PreviewlistComeEventArgs e) { ComparePreviewStopped?.Invoke(null, e); }

        public static bool StopOperation = false;
        static public void PreviewlistComparer(List<ProtoClasses.ProtoPreview.protoRow> e)
        {
            if (e.Count > 0)
            {
                if (!working)
                {
                    StopOperation = false;
                    PreviewlistComeEventArgs ea = new PreviewlistComeEventArgs(false, e, null, 0, null);
                    ea.BaseFolder = Utils.Settings.set.data_path + @"/makets/";
                    ea.InitialList = e;
                    working = true;
                    threadtwo = new Thread(daemon);
                    threadtwo.IsBackground = true;
                    threadtwo.Name = "AccentBase PreviewUpdate Daemon";
                    threadtwo.Start(ea);

                }
                else
                {
                    StopOperation = true;
                    while (working) { }
                    StopOperation = false;
                    PreviewlistComeEventArgs ea = new PreviewlistComeEventArgs(false, e, null, 0, null);
                    ea.BaseFolder = Utils.Settings.set.data_path + @"/makets/";
                    ea.InitialList = e;
                    working = true;
                    threadtwo = new Thread(daemon);
                    threadtwo.IsBackground = true;
                    threadtwo.Name = "AccentBase PreviewUpdate Daemon";
                    threadtwo.Start(ea);
                }
            }
            else
            {
                StopFtpDaemon(new PreviewlistComeEventArgs(true, null, new List<ProtoClasses.ProtoFtpSchedule.protoRow>(), 100, null));
            }
        }

        static private void daemon(object obj)
        {
            PreviewlistComeEventArgs ea = obj as PreviewlistComeEventArgs;
            List<ProtoClasses.ProtoPreview.protoRow> prl = ea.InitialList;
            List<ProtoClasses.ProtoFtpSchedule.protoRow> newprl = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();
            int i = 0;
            int procentage = 0;
            int oldprocentage = 0;
            int max = prl.Count;
            foreach (ProtoClasses.ProtoPreview.protoRow pr in prl)
            {
                if (!StopOperation)
                {
                    if (Ftp.FtpPreviewRecieve.preview.ContainsKey(pr.fullname))
                    {
                        if (pr.Length != preview[pr.fullname].Length || pr.LastWriteTime > preview[pr.fullname].LastWriteTime)
                        {
                            newprl.Add(getProtoFtp(pr, ea.BaseFolder));
                        }

                    }
                    else
                    {
                        newprl.Add(getProtoFtp(pr, ea.BaseFolder));
                    }
                    i++;
                    procentage = (i * 100) / max;
                    if (oldprocentage < procentage)
                    {
                        oldprocentage = procentage;
                        StopFtpDaemon(new PreviewlistComeEventArgs(false, null, null, oldprocentage, null));
                    }
                }
            }
            StopFtpDaemon(new PreviewlistComeEventArgs(true, null, newprl, 100, null));
            working = false;
        }

        static private ProtoClasses.ProtoFtpSchedule.protoRow getProtoFtp(ProtoClasses.ProtoPreview.protoRow e, string tpath)
        {
            ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow();
            FileInfo fi = new FileInfo(tpath + e.fullname);
            pr.Upload = false;
            pr.LastCreationTime = e.LastWriteTime;
            pr.LastWriteTime = e.LastWriteTime;
            pr.Length = e.Length;
            pr.fileshortname = fi.Name;
            pr.serveraddress = @"Server/" + e.fullname;
            pr.sourcefile = "makets" + @"/" + e.fullname;
            pr.order_id = e.order_id;
            double FileSize = e.Length; string fileSize = string.Empty;
            if (FileSize > 0)
            {
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Kb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Mb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Gb"; }
            }
            FileSize = Math.Round(FileSize, 2);
            pr.LengthString = FileSize.ToString() + fileSize;
            pr.targetfile = fi.FullName;
            return pr;
        }
        #endregion



    }
}
