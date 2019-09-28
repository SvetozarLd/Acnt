using FluentFTP;
using ProtoBuf;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
namespace AccentBase.Utils
{
    public class VersionInfo
    {
        [ProtoContract]
        public class protoSet
        {
            [ProtoMember(1)]
            public string name { get; set; }
            [ProtoMember(2)]
            public int mainVer { get; set; }
            [ProtoMember(3)]
            public int assemblyVer { get; set; }
            [ProtoMember(4)]
            public int editionVer { get; set; }
            [ProtoMember(5)]
            public string error { get; set; }
            [ProtoMember(6)]
            public string updaterpath { get; set; }
            [ProtoMember(7)]
            public string serveraddress { get; set; }
            [ProtoMember(8)]
            public string ftplogin { get; set; }
            [ProtoMember(9)]
            public string ftppass { get; set; }
        }

        public byte[] Serialize(protoSet items)
        {
            protoSet item = new protoSet
            {
                name = items.name,
                mainVer = items.mainVer,
                assemblyVer = items.assemblyVer,
                editionVer = items.editionVer,
                error = items.error,
                updaterpath = items.updaterpath,
                serveraddress = items.serveraddress,
                ftplogin = items.ftplogin,
                ftppass = items.ftppass
            };
            byte[] result = null;
            try
            {
                using (MemoryStream stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<protoSet>(stream, item, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoSet Deserialize(byte[] message)
        {
            protoSet item = new protoSet();
            using (MemoryStream stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<protoSet>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }

        public Exception Save(string FileFullname, protoSet set)
        {
            try
            {
                if (File.Exists(FileFullname)) { File.Delete(FileFullname); }
                byte[] sett = Serialize(set);
                File.WriteAllBytes(FileFullname, sett);
                return null;
            }
            catch (Exception ex) { return ex; }
        }
        public protoSet Load()
        {
            protoSet set = new protoSet();
            
            try
            {
                string FileFullname = "C:/OABase/program" + "/VersionInfo";
                if (File.Exists(FileFullname))
                {
                    VersionInfo ps = new VersionInfo();
                    byte[] sett = File.ReadAllBytes(FileFullname);
                    set = ps.Deserialize(sett);
                }
                else
                {
                    set = new VersionInfo.protoSet
                    { name = string.Empty, mainVer = 0, assemblyVer = 0, editionVer = 0, error = "Не установлено!" };
                }
            }
            catch (Exception ex)
            {
                set = new VersionInfo.protoSet
                { name = string.Empty, mainVer = 0, assemblyVer = 0, editionVer = 0, error = ex.Message };
            }
            return set;
        }

        public string checkLocalVersion()
        {
            Settings.versionInfo = Load();
            string result = "Не определена! Возможно файлы испорчены!";
            if (Settings.versionInfo.error != string.Empty)
            {
                if (!Settings.versionInfo.error.Equals("Не установлено!")) { MessageBox.Show(Settings.versionInfo.error, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            {
                result = Settings.versionInfo.mainVer + "." + Settings.versionInfo.assemblyVer + "." + Settings.versionInfo.editionVer;
            }
            return result;
        }








        public string CheckServerVersion()
        {
            updatesProgram up = GetListing("", "", Settings.set.server_address, @"Update\Program");
            string result = "Ошибка соединения с сервером";
            if (up.error)
            {
                MessageBox.Show(up.ex, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                result = up.VersionToString();
            }
            return result;
        }






        private void iprog(FtpProgress fp)
        {
        }
        private CancellationTokenSource cancelTokenSource = null;
        private CancellationToken token = new CancellationToken();

        public async void Transmitefile(string login, string passwrd, string Server, string FileFROM, string FileTO) //string localPath, string remotePath, string fileName, bool upload, DataRow dr
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
                return version.mainVer.ToString() + "." + version.assemblyVer.ToString() + "." + version.editionVer.ToString();
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


        public void updater(string ver)
        {
            DialogResult result = AutoClosingMessageBox.Show("Доступна новая версия программы: " + ver + Environment.NewLine + "Строго рекомендуется обновление!" + Environment.NewLine + " Желаете обновить программу сейчас?", "Внимание!", 300000, MessageBoxButtons.YesNo, DialogResult.No);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string path = Settings.versionInfo.updaterpath;
                    Process process = new Process
                    {
                        StartInfo =
                    {
                        FileName = path,
                        Arguments = "/auto"
                    }
                    };
                    process.Start();

                }
                catch (Exception ex)
                {

                }
                Application.Exit();
            }
        }
        public void updaterPermanent(string ver)
        {
            DialogResult result = MessageBox.Show("Доступна новая версия программы: " + ver + Environment.NewLine + "Строго рекомендуется обновление!" + Environment.NewLine + " Желаете обновить программу сейчас?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string path = Settings.versionInfo.updaterpath;
                    Process process = new Process
                    {
                        StartInfo =
                    {
                        FileName = path,
                        Arguments = "/auto"
                    }
                    };
                    process.Start();

                }
                catch (Exception ex)
                {

                }
                Application.Exit();
            }
        }

    }
}
