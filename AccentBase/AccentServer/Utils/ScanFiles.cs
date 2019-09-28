using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentServer.Utils
{
    class ScanFiles
    {
        #region Рекурсивное сканирование файлов и каталогов
        Dictionary<string, AccentServer.ProtoClasses.ProtoFiles.protoRow> files = new Dictionary<string, AccentServer.ProtoClasses.ProtoFiles.protoRow>();

        public Dictionary<string, AccentServer.ProtoClasses.ProtoFiles.protoRow> FullScanFiles(DirectoryInfo maindir)
        {
            if (files.Count > 0) { files.Clear(); }
            files = new Dictionary<string, AccentServer.ProtoClasses.ProtoFiles.protoRow>();
            recurseScan(maindir.FullName.ToString(), 0);
            return files;
        }

//        int i = 0;
        DirectoryInfo maind = new DirectoryInfo(Properties.Settings.Default.FilePath);

        private void recurseScan(string sDir, int order_id)
        {

            //string[] streeee = Directory.GetDirectories(sDir);

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    AccentServer.ProtoClasses.ProtoFiles.protoRow fold = new ProtoClasses.ProtoFiles.protoRow();
                    fold.folder_flag = 1;
                    DirectoryInfo di = new DirectoryInfo(d);
                    fold.name = di.Name;
                    fold.fullname = di.FullName;
                    fold.CreationTime = Utils.UnixDate.DateTimeToInt64(di.CreationTime);
                    fold.LastAccessTime = Utils.UnixDate.DateTimeToInt64(di.LastAccessTime);
                    fold.LastWriteTime = Utils.UnixDate.DateTimeToInt64(di.LastWriteTime);
                    fold.folderfullname = di.Parent.FullName;
                    fold.foldername = di.Parent.Name;                    
                    if (di.Parent != null)
                    {
                        if (di.Parent.Parent != null && maind.FullName == di.Parent.Parent.FullName)
                        {
                            int i = 0;
                            if (int.TryParse(fold.name, out i))
                            {
                                order_id = i;
                            }
                        }
                    }
                    fold.order_id = order_id;
                    files.Add(fold.fullname, fold);

                    foreach (string f in Directory.GetFiles(d))
                    {
                        AccentServer.ProtoClasses.ProtoFiles.protoRow fil = new ProtoClasses.ProtoFiles.protoRow();
                        fil.folder_flag = 0;
                        FileInfo fi = new FileInfo(f);
                        fil.name = fi.Name;
                        fil.fullname = fi.FullName;
                        fil.Extension = fi.Extension;
                        fil.Length = fi.Length;

                        fil.CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime);
                        fil.LastAccessTime = Utils.UnixDate.DateTimeToInt64(fi.LastAccessTime);
                        fil.LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime);

                        fil.folderfullname = fold.fullname;
                        fil.foldername = fold.name;
                        fil.order_id = order_id;

                        files.Add(fil.fullname, fil);


                    }



                    recurseScan(d, order_id);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        #endregion
    }
}
