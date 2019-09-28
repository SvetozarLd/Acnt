//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
using FluentFTP;
using System;
//using System.Windows.Forms;
using System.Collections.Generic;
//using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net;
//using System.Windows.Forms;
using System.Threading;
namespace AccentBase.Ftp
{
    public class FtpReciever
    {

        public class FTPOperationEventArgs : EventArgs
        {
            public bool Ended { get; set; }
            public int Status { get; set; }
            public double ProcessCount { get; set; }
            public string Ex { get; set; }
            public long Id { get; set; }
            public string TransferSpeed { get; set; }
            public string ETA { get; set; }
            public long OrderId { get; set; }
            public FTPOperationEventArgs(long orderId, bool ended, int status, long id, string eta, string transferSpeed, double processcount, string ex)
            { OrderId = orderId; Ended = ended; Status = status; Id = id; ETA = eta; TransferSpeed = transferSpeed; ProcessCount = processcount; Ex = ex; }
        }
        public delegate void Delegate_FTPOperation(object sender, FTPOperationEventArgs e);
        public event Delegate_FTPOperation Event_FTPOperation;
        private void FtpProgressEvent(FTPOperationEventArgs e) { Event_FTPOperation?.Invoke(null, e); }

        private System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
        private bool ShowStats { get; set; }
        public FtpReciever()
        {
            ShowStats = true;
            tmr.Interval = 500;
            tmr.Start();
            tmr.Tick += Tmr_Tick;
        }


        private void Tmr_Tick(object sender, EventArgs e)
        {
            ShowStats = true;
        }







        public bool ToWork { get; set; }

        private void Iprog(FtpProgress fp, long uid, int status, long order_id)
        {

            if (ToWork)
            {
                if (fp.Progress >= 100)
                {
                    FtpProgressEvent(new FTPOperationEventArgs(order_id, true, status, uid, "", "", 100f, string.Empty));
                }
                else
                {
                    if (ShowStats)
                    {
                        FtpProgressEvent(new FTPOperationEventArgs(order_id, false, status, uid, $"{ fp.ETA:hh\\:mm\\:ss}", fp.TransferSpeedToString(), fp.Progress, string.Empty));
                        //SqlLite.FtpSchedule.SetProgress(uid, fp.Progress, fp.TransferSpeedToString());
                        ShowStats = false;
                    }
                }
            }
            else
            {
                try
                {
                    cancelTokenSource.Cancel();
                }
                catch (Exception ex)
                {
                    //StopFtpDaemon(new FTPOperationEventArgs(false, filename));
                }
                finally
                {
                    cancelTokenSource.Dispose();
                }
            }

            //SqlLite.FtpSchedule.SetProgress(uid, fp.Progress, fp.TransferSpeedToString());
            //DataRow dr = SqlLite.FtpSchedule.FTPList.Select("id =" + uid).SingleOrDefault();
            //if (dr != null)
            //{
            //    dr["conspeed"] = fp.TransferSpeedToString();
            //    dr["processprogress"] = Utils.CheckDBNull.ToInt32(fp.Progress);
            //}
            //StopFtpDaemon(new FTPOperationEventArgs(false, filename, fp.TransferSpeedToString(), fp.Progress, null));
            //Trace.WriteLine(filename +" : "+ fp.Progress.ToString() + "%");
            //if (fp.Progress >= 50 && token.CanBeCanceled)
            //{
            //    try
            //    {
            //        cancelTokenSource.Cancel();
            //    } catch(Exception ex)
            //    {
            //        StopFtpDaemon(new FTPOperationEventArgs(false, filename));
            //    }
            //    finally
            //    {
            //        cancelTokenSource.Dispose();
            //    }
            //    }
            //if (fp.Progress == 100) { Trace.WriteLine(filename+" : ENDED"); }

        }

        private CancellationTokenSource cancelTokenSource = null;
        private CancellationToken token = new CancellationToken();

        public async void Transmitefile() //string localPath, string remotePath, string fileName, bool upload, DataRow dr
        {

            ToWork = true;
            //try
            //{
            while (ToWork)
            {
                ////DataTable dt = SqlLite.FtpSchedule.FTPList.Copy();

                ////DataRow row = dt.Rows[0];
                ////DataView view = new DataView(SqlLite.FtpSchedule.FTPList);
                ////dt.DefaultView = 
                //DataRowCollection rowCollection = dt.Rows;
                ////DataRowCollection rowCollection = view.Rows;
                ////view.Sort = "id ASC";
                ////DataTable dt = view.DefaultView;
                ////SqlLite.FtpSchedule.FTPList.Rows.CopyTo
                ////SqlLite.FtpSchedule.FTPList.DefaultView.Sort = "id ASC";
                ////DataTable dt = SqlLite.FtpSchedule.FTPList.Copy();
                ////dt.DefaultView.Sort = "id ASC";
                ////DataView view = dt.DefaultView;
                ///


                //foreach (SqlLite.FtpSchedule.FTPListArgs row in SqlLite.FtpSchedule)
                //{
                if (SqlLite.FtpSchedule.FTPListDic.Count > 0)
                {
                    //SqlLite.FtpSchedule.FTPListArgs rw = SqlLite.FtpSchedule.FTPListDic[0];
                    //KeyValuePair<long, SqlLite.FtpSchedule.FTPListArgs>
                    //var first = SqlLite.FtpSchedule.FTPListDic.OrderBy(kvp => kvp.Key).First();
                    KeyValuePair<long, SqlLite.FtpSchedule.FTPListArgs> firstPairOfDictionary = SqlLite.FtpSchedule.FTPListDic.FirstOrDefault();
                    if (firstPairOfDictionary.Value.sourcefile != null && firstPairOfDictionary.Value.targetfile != null)
                    {
                        //if (!ToWork) { break; }
                        long uid = firstPairOfDictionary.Value.id;//Utils.CheckDBNull.ToLong(row["id"]);
                        bool upload = firstPairOfDictionary.Value.Upload;// Utils.CheckDBNull.ToBoolean(row["upload"]);
                        string FileFROM = firstPairOfDictionary.Value.sourcefile;// Utils.CheckDBNull.ToString(row["sourcefile"]);
                        string FileTO = firstPairOfDictionary.Value.targetfile;// Utils.CheckDBNull.ToString(row["targetfile"]);
                        long order_id = firstPairOfDictionary.Value.order_id;// Utils.CheckDBNull.ToLong(row["order_id"]);
                        FileTO = FileTO.Replace(".oabzip", ".zip");
                        string[] tmp = FileFROM.Split('\\');
                        if (tmp.Length == 4 && (tmp[3].Equals("makets") || tmp[3].Equals("preview") || tmp[3].Equals("doc") || tmp[3].Equals("photoreport")))
                        {
                            FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 2, uid, "", "", 100f, string.Empty));
                        }
                        else
                        {
                            FileInfo fi = new FileInfo(FileTO);
                            if (fi.Exists)
                            {
                                RenamerResult res = Renamer(new RenamerResult(fi.FullName, false, false));
                                if (res.Cancelation)
                                {
                                    FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 2, uid, "", "", 100f, string.Empty));
                                }
                                else
                                {
                                    FileTO = res.FileName;
                                    try
                                    {
                                        using (FtpClient client = new FtpClient(Utils.Settings.set.server_address))
                                        {
                                            cancelTokenSource = new CancellationTokenSource();
                                            token = cancelTokenSource.Token;
                                            client.Credentials = new NetworkCredential(Utils.Settings.set.FtpLogin, Utils.Settings.set.FtpPass);
                                            client.Connect();
                                            FtpProgress prog = new FtpProgress(0, 0, new TimeSpan());
                                            bool xxx = false;
                                            try
                                            {
                                                if (upload)
                                                {
                                                    IProgress<FtpProgress> progress = new Progress<FtpProgress>((p) => Iprog(p, uid, 1, order_id));
                                                    //FtpProgressEvent(new FTPOperationEventArgs(false, 1, uid, "", "", 0f, string.Empty));
                                                    xxx = await client.UploadFileAsync(FileFROM, FileTO, FtpExists.Overwrite, true, FtpVerify.OnlyChecksum, progress, token);
                                                    //await Task.Delay(500);

                                                    FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 1, uid, "", "", 100f, string.Empty));
                                                }
                                                else
                                                {
                                                    //client.DownloadFile(FileTO, FileFROM, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress);
                                                    //FtpProgressEvent(new FTPOperationEventArgs(false, 2, uid, "", "", 100f, string.Empty));
                                                    IProgress<FtpProgress> progress = new Progress<FtpProgress>((p) => Iprog(p, uid, 2, order_id));
                                                    xxx = await client.DownloadFileAsync(FileTO, FileFROM, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress, token);
                                                    //await Task.Delay(500);
                                                    FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 2, uid, "", "", 100f, string.Empty));
                                                }
                                                // FtpProgressEvent(new FTPOperationEventArgs(true, 2, uid, "", "", 100f, string.Empty));
                                            }
                                            catch (Exception ex)
                                            {
                                                //await Task.Delay(500);
                                                FtpProgressEvent(new FTPOperationEventArgs(order_id, false, 0, uid, "", "", 0, ex.Message));
                                            }
                                            #region ______________________
                                            //    if (xxx)
                                            //{
                                            //    //Trace.WriteLine("------------" + FileTO + "------------------- Удачно");
                                            //    row["notes"] = "успех";
                                            //}else
                                            //{
                                            //    row["notes"] = "неудача";
                                            //}
                                            //client.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress);
                                            //if (token.IsCancellationRequested) { Trace.WriteLine("token.IsCancellationRequested"); }
                                            //if (xxx) { }
                                            //Trace.WriteLine("------------" + remotePath + "-------------------");
                                            //}
                                            //else
                                            //{

                                            //}

                                            //bool xxx = await client.DownloadFileAsync(localPath, remotePath, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress);

                                            //Progress<double> progress = new Progress<double>(x => {
                                            //    // When progress in unknown, -1 will be sent
                                            //    if (x < 0)
                                            //    {
                                            //        progressBar.IsIndeterminate = true;
                                            //    }
                                            //    else
                                            //    {
                                            //        progressBar.IsIndeterminate = false;
                                            //        progressBar.Value = x;
                                            //    }
                                            //});


                                            /* IProgress<FtpProgress> qqq = new IProgress<FtpProgress>()*/
                                            //;
                                            //// download the file again
                                            //await client.DownloadFileAsync(@"C:\MyVideo_2.mp4", "/htdocs/MyVideo_2.mp4", FtpLocalExists.Overwrite, FtpVerify.OnlyChecksum, progress);




                                            //// get a list of files and directories in the "/htdocs" folder
                                            //foreach (FtpListItem item in client.GetListing("/htdocs"))
                                            //{

                                            //    // if this is a file
                                            //    if (item.Type == FtpFileSystemObjectType.File)
                                            //    {

                                            //        // get the file size
                                            //        long size = client.GetFileSize(item.FullName);

                                            //    }

                                            //    // get modified date/time of the file or folder
                                            //    DateTime time = client.GetModifiedTime(item.FullName);

                                            //    // calculate a hash for the file on the server side (default algorithm)
                                            //    FtpHash hash = client.GetHash(item.FullName);

                                            //}

                                            //// upload a file
                                            //client.UploadFile(@"C:\MyVideo.mp4", "/htdocs/MyVideo.mp4");

                                            //// rename the uploaded file
                                            //client.Rename("/htdocs/MyVideo.mp4", "/htdocs/MyVideo_2.mp4");

                                            //// download the file again
                                            //client.DownloadFile(@"C:\MyVideo_2.mp4", "/htdocs/MyVideo_2.mp4");

                                            //// delete the file
                                            //client.DeleteFile("/htdocs/MyVideo_2.mp4");

                                            //// delete a folder recursively
                                            //client.DeleteDirectory("/htdocs/extras/");

                                            //// check if a file exists
                                            //if (client.FileExists("/htdocs/big2.txt")) { }

                                            //// check if a folder exists
                                            //if (client.DirectoryExists("/htdocs/extras/")) { }

                                            //// upload a file and retry 3 times before giving up
                                            //client.RetryAttempts = 3;
                                            //client.UploadFile(@"C:\MyVideo.mp4", "/htdocs/big.txt", FtpExists.Overwrite, false, FtpVerify.Retry);

                                            //// disconnect! good bye!
                                            #endregion
                                            client.Disconnect();
                                            //Trace.WriteLine(uid + ":"+FileFROM +" : ENDED!");
                                            //SqlLite.FtpSchedule.Delete(uid);
                                            //SqlLite.FtpSchedule.SetProgress(uid, 100, string.Empty);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FtpProgressEvent(new FTPOperationEventArgs(order_id, false, 0, uid, "", "", 0f, ex.Message));
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    using (FtpClient client = new FtpClient(Utils.Settings.set.server_address))
                                    {
                                        cancelTokenSource = new CancellationTokenSource();
                                        token = cancelTokenSource.Token;
                                        client.Credentials = new NetworkCredential(Utils.Settings.set.FtpLogin, Utils.Settings.set.FtpPass);
                                        client.Connect();
                                        FtpProgress prog = new FtpProgress(0, 0, new TimeSpan());
                                        bool xxx = false;
                                        try
                                        {
                                            if (upload)
                                            {
                                                IProgress<FtpProgress> progress = new Progress<FtpProgress>((p) => Iprog(p, uid, 1, order_id));
                                                xxx = await client.UploadFileAsync(FileFROM, FileTO, FtpExists.Overwrite, true, FtpVerify.OnlyChecksum, progress, token);
                                                FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 1, uid, "", "", 100f, string.Empty));
                                            }
                                            else
                                            {
                                                IProgress<FtpProgress> progress = new Progress<FtpProgress>((p) => Iprog(p, uid, 2, order_id));
                                                xxx = await client.DownloadFileAsync(FileTO, FileFROM, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress, token);
                                                FtpProgressEvent(new FTPOperationEventArgs(order_id, true, 2, uid, "", "", 100f, string.Empty));
                                            };
                                        }
                                        catch (Exception ex)
                                        {
                                            FtpProgressEvent(new FTPOperationEventArgs(order_id, false, 0, uid, "", "", 0, ex.Message));
                                        }
                                        client.Disconnect();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    FtpProgressEvent(new FTPOperationEventArgs(order_id, false, 0, uid, "", "", 0f, ex.Message));
                                }
                            }

                        }
                    }
                }
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    //DataRow dr = SqlLite.FtpSchedule.FTPList.Select("id =" + uid).SingleOrDefault();
        //    //row["notes"] = ex.Message.ToString;
        //}


        private class RenamerResult
        {
            public string FileName { get; set; }
            public bool Cancelation { get; set; }
            public bool FileBlocked { get; set; }
            public RenamerResult(string fname, bool canc, bool fileblock) { FileName = fname; Cancelation = canc; FileBlocked = fileblock; }
        }

        private RenamerResult Renamer(RenamerResult e)
        {
            if (e.Cancelation) { return e; }
            if (Utils.FileUtils.TryDelete(e.FileName))
            {
                e.FileBlocked = false;
                return e;
            }
            else
            {
                e.FileBlocked = true;
                FileInfo info = new FileInfo(e.FileName);
                using (Forms.FormFileRename renamer = new Forms.FormFileRename(info.Name, e.FileBlocked, false))
                {
                    renamer.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    renamer.ShowDialog();
                    info = new FileInfo(info.DirectoryName + @"\" + renamer.FileName);
                    e.FileName = info.FullName;
                    e.Cancelation = renamer.Cancelation;
                    e = Renamer(e);
                }
            }
            return e;
        }
    }
}
