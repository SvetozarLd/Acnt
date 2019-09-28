using FluentFTP;
using System;
using System.Net;
using System.Threading;
namespace AccentBase_Updater
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

        private void iprog(FtpProgress fp)
        {

            //if (ToWork)
            //{
            //    if (fp.Progress >= 100)
            //    {
            //        //FtpProgressEvent(new FTPOperationEventArgs(order_id, true, status, uid, "", "", 100f, string.Empty));
            //    }
            //    else
            //    {
            //        if (ShowStats)
            //        {
            //            //FtpProgressEvent(new FTPOperationEventArgs(order_id, false, status, uid, $"{ fp.ETA:hh\\:mm\\:ss}", fp.TransferSpeedToString(), fp.Progress, string.Empty));
            //            ShowStats = false;
            //        }
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        cancelTokenSource.Cancel();
            //    }
            //    catch (Exception ex)
            //    {
            //        //StopFtpDaemon(new FTPOperationEventArgs(false, filename));
            //    }
            //    finally
            //    {
            //        cancelTokenSource.Dispose();
            //    }
            //}

        }

        private CancellationTokenSource cancelTokenSource = null;
        private CancellationToken token = new CancellationToken();

        async public void Transmitefile(string login, string passwrd, string Server, string FileFROM, string FileTO) //string localPath, string remotePath, string fileName, bool upload, DataRow dr
        {

            try
            {
                using (FtpClient client = new FtpClient(Server))
                {
                    cancelTokenSource = new CancellationTokenSource();
                    token = cancelTokenSource.Token;
                    client.Credentials = new NetworkCredential(login, passwrd);
                    client.Connect();
                    FtpProgress prog = new FtpProgress(0, 0, new TimeSpan());
                    bool xxx = false;
                    try
                    {
                        IProgress<FtpProgress> progress = new Progress<FtpProgress>((p) => iprog(p));
                        xxx = await client.DownloadFileAsync(FileTO, FileFROM, FtpLocalExists.Overwrite, FluentFTP.FtpVerify.OnlyChecksum, progress, token);
                    }
                    catch (Exception ex)
                    {

                    }
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {

            }
        }


        public class program_version
        {
            public int mainVer { get; set; }
            public int assemblyVer { get; set; }
            public int editionVer { get; set; }
            public program_version(int MainVer, int AassemblyVer, int EditionVer)
            { mainVer = MainVer; assemblyVer = AassemblyVer; editionVer = EditionVer; }
        }


        public class updatesProgram
        {
            public string filename { get; set; }
            public program_version version { get; set; }
            public long modifydate { get; set; }
            public long length { get; set; }
            public bool changed { get; set; }
            public string ex { get; set; }
            public bool error { get; set; }

            public string VersionToString()
            {
                return version.mainVer.ToString()+"." +version.assemblyVer.ToString()+"."+ version.editionVer.ToString();
            }


            public void UpdateByNewestVersion(string FileName)
            {
                program_version ver = new program_version(0, 0, 0);
                bool noerr = true;
                string[] newname = FileName.Split('.');
                if (int.TryParse(newname[0], out int i))
                {
                    ver.mainVer = i;
                    if (int.TryParse(newname[1], out i))
                    {
                        ver.assemblyVer = i;
                        if (int.TryParse(newname[2], out i)) { ver.editionVer = i; }
                        else { noerr = false; }
                    }
                    else { noerr = false; }
                }
                else { noerr = false; }

                if (noerr)
                {
                    if (version.mainVer < ver.mainVer) { version.mainVer = ver.mainVer; version.assemblyVer = ver.assemblyVer; version.editionVer = ver.editionVer; changed = true; filename = FileName; }
                    else
                    {
                        if (version.mainVer == ver.mainVer)
                        {
                            if (version.assemblyVer < ver.assemblyVer) { version.assemblyVer = ver.assemblyVer; version.editionVer = ver.editionVer; changed = true; filename = FileName; }
                            else
                            {
                                if (version.assemblyVer == ver.assemblyVer) { if (version.editionVer < ver.editionVer) { version.editionVer = ver.editionVer; changed = true; filename = FileName; } }
                            }
                        }
                    }
                }
            }
        }



        public updatesProgram GetListing(string login, string passwrd, string Server, string folder)
        {
            updatesProgram up = new updatesProgram()
            {
                filename = string.Empty,
                version = new program_version(0, 0, 0),
                modifydate = 0,
                length = 0,
                changed = false,
                ex = "У вас установлена последняя версия",
                error = false
            };
            try
            {
                using (FtpClient conn = new FtpClient())
                {
                    conn.Host = Server;
                    conn.Credentials = new NetworkCredential(login, passwrd);
                    conn.SetWorkingDirectory("/");
                    foreach (FtpListItem item in conn.GetListing(conn.GetWorkingDirectory() + folder, FtpListOption.AllFiles | FtpListOption.Size))
                    {
                        if (item.Type == FtpFileSystemObjectType.File)
                        {
                            up.UpdateByNewestVersion(item.Name);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                up.error = true; up.ex = ex.Message;
            }

            return up;
        }



    }
}
