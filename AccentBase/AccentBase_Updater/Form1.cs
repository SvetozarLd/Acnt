using FluentFTP;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using IWshRuntimeLibrary;
namespace AccentBase_Updater
{
    public partial class Form_Main : Form
    {
        private ProtoClasses.VersionInfo versionInfo = new ProtoClasses.VersionInfo();
        private ProtoClasses.VersionInfo.protoSet protoSet = new ProtoClasses.VersionInfo.protoSet();
        private FtpReciever.updatesProgram up = new FtpReciever.updatesProgram();
        private bool autoupdate = false;
        public Form_Main(bool auto)
        {
            InitializeComponent();
            autoupdate = auto;
        }

        private void SelectLocalPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
            {
                folderDlg.ShowNewFolderButton = true;
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    LocalPath.Text = folderDlg.SelectedPath;
                }
            }
            checkLocalVersion();
        }

        private void checkLocalVersion()
        {
            protoSet = versionInfo.Load(LocalPath.Text + @"\program\VersionInfo");
            LocalVersion.Text = "Не установлено!";
            if (protoSet.error != string.Empty)
            {
                if (!protoSet.error.Equals("Не установлено!")) { MessageBox.Show(protoSet.error, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            {
                LocalVersion.Text = protoSet.mainVer + "." + protoSet.assemblyVer + "." + protoSet.editionVer;
            }
            if (LocalVersion.Text.Equals("Не установлено!"))
            {
                Update.Text = "Установить";
            }
            else { Update.Text = "Обновить"; }
            if (LocalVersion.Text.Equals("Не установлено!")) { LocalPath.Enabled = true; button_LocalPath.Enabled = true; buttonDelete.Enabled = false; } else { LocalPath.Enabled = false; button_LocalPath.Enabled = false; buttonDelete.Enabled = true; }
            if (ServerVersion.Text.Equals("Нет подключения"))
            {
                Update.Enabled = false;
                FtpAddress.Enabled = FtpLogin.Enabled = FtpPass.Enabled = FtpProgram.Enabled = FtpData.Enabled = button_Save.Enabled = true;
            }
            else
            {
                Update.Enabled = true;
                FtpAddress.Enabled = FtpLogin.Enabled = FtpPass.Enabled = FtpProgram.Enabled = FtpData.Enabled = button_Save.Enabled = false;
            }
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            LocalPath.Text = Properties.Settings.Default.LocalPath;
            FtpAddress.Text = Properties.Settings.Default.FtpServer;
            FtpLogin.Text = Properties.Settings.Default.FtpLogin;
            FtpPass.Text = Properties.Settings.Default.FtpPass;
            FtpProgram.Text = Properties.Settings.Default.FtpProgram;
            FtpData.Text = Properties.Settings.Default.FtpData;
            CheckServerVersion(FtpLogin.Text, FtpPass.Text, FtpAddress.Text, FtpProgram.Text);
            checkLocalVersion();
            if (autoupdate) { Update_Click(null, null); }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocalPath = LocalPath.Text;
            Properties.Settings.Default.FtpServer = FtpAddress.Text;
            Properties.Settings.Default.FtpLogin = FtpLogin.Text;
            Properties.Settings.Default.FtpPass = FtpPass.Text;
            Properties.Settings.Default.FtpProgram = FtpProgram.Text;
            Properties.Settings.Default.FtpData = FtpData.Text;
            Properties.Settings.Default.Save();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UnitsProgress.Text = "Проверка последней версии на сервере";
            CheckServerVersion(FtpLogin.Text, FtpPass.Text, FtpAddress.Text, FtpProgram.Text);
            checkLocalVersion();
        }


        private void CheckServerVersion(string login, string passwrd, string server, string folder)
        {
            FtpReciever ftpReciever = new FtpReciever();
            //FtpReciever.updatesProgram up = new FtpReciever.updatesProgram();
            up = ftpReciever.GetListing(login, passwrd, server, folder);
            if (up.error)
            {
                MessageBox.Show(up.ex, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServerVersion.Text = "Нет подключения";
                Update.Enabled = false;
                UnitsProgress.Text = "Ошибка соединения с сервером";
            }
            else
            {
                ServerVersion.Text = up.VersionToString();
                Update.Enabled = true;
                UnitsProgress.Text = "Версия получена";
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            string processName = "AccentBase";
            bool processExists = Process.GetProcesses().Any(p => p.ProcessName == processName);
            if (processExists)
            {
                if (autoupdate)
                {
                    Process[] workers = Process.GetProcessesByName("AccentBase");
                    foreach (Process worker in workers)
                    {
                        worker.Kill();
                        worker.WaitForExit();
                        worker.Dispose();
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("База запущена!" + Environment.NewLine + "Вы хотите закрыть базу и продолжить?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Process[] workers = Process.GetProcessesByName("AccentBase");
                        foreach (Process worker in workers)
                        {
                            worker.Kill();
                            worker.WaitForExit();
                            worker.Dispose();
                        }
                    }
                    else { return; }
                }
            }
            UnitsProgress.Enabled = true; Update.Enabled = false; ServerCheck.Enabled = false;
            LocalPath.Enabled = button_LocalPath.Enabled = FtpAddress.Enabled = FtpLogin.Enabled = FtpPass.Enabled = FtpProgram.Enabled = FtpData.Enabled = buttonDelete.Enabled = button_Save.Enabled = false;
            if (!Directory.Exists(LocalPath.Text)) { Directory.CreateDirectory(LocalPath.Text); }
            if (!Directory.Exists(LocalPath.Text + @"\update")) { Directory.CreateDirectory(LocalPath.Text + @"\update"); }
            if (!Directory.Exists(LocalPath.Text + @"\data")) { Directory.CreateDirectory(LocalPath.Text + @"\data"); }
            if (!Directory.Exists(LocalPath.Text + @"\program")) { Directory.CreateDirectory(LocalPath.Text + @"\program"); }
            UnitsProgressBar.Value = 0;
            FileFromFtp(FtpLogin.Text, FtpPass.Text, FtpAddress.Text, FtpProgram.Text + @"\" + up.filename, LocalPath.Text + @"\update\" + up.filename, 0, "Скачивание базы");
        }









        private CancellationTokenSource cancelTokenSource = null;
        private CancellationToken token = new CancellationToken();

        public async void FileFromFtp(string login, string passwrd, string Server, string FileFROM, string FileTO, int steps, string notes) //string localPath, string remotePath, string fileName, bool upload, DataRow dr
        {
            bool noerr = true;
            UnitsProgress.Text = notes;
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
                noerr = false;
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (noerr)
            {
                switch (steps)
                {
                    case 0:
                        backgroundWorker1.RunWorkerAsync();
                        break;
                    case 1:
                        if (DataBases != null && DataBases.Count > 1)
                        {
                            DataBases.RemoveAt(0);
                            UnitsProgressBar.Value = 0;
                            FileFromFtp(FtpLogin.Text, FtpPass.Text, FtpAddress.Text, FtpData.Text + @"\" + DataBases[0], LocalPath.Text + @"\data\" + DataBases[0], 1, "Скачивание :" + DataBases[0]);
                        }
                        else
                        {
                            Ended();
                        }
                        break;
                }
            }
        }

        private void iprog(FtpProgress fp)
        {
            int i = Convert.ToInt32(fp.Progress);
            if (i >= 100)
            {
                UnitsProgressBar.Value = 100;
            }
            else
            {
                UnitsProgressBar.Value = i;
            }
        }

        public class backgroundWorkerReport
        {
            public int maximum { get; set; }
            public string name { get; set; }
            public int counter { get; set; }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ReadOptions options = new ReadOptions
                {
                    Encoding = System.Text.Encoding.GetEncoding("cp866")
                };
                using (ZipFile zip = ZipFile.Read(LocalPath.Text + @"\update\" + up.filename, options))
                {
                    int maxcount = zip.Count + 1;
                    int count = 0;
                    string filename = string.Empty;
                    foreach (ZipEntry ent in zip)
                    {
                        if (!backgroundWorker1.CancellationPending)
                        {
                            filename = "Установка: " + ent.FileName;
                            ent.Extract(LocalPath.Text + @"\program\", ExtractExistingFileAction.OverwriteSilently);
                            count++;
                            backgroundWorker1.ReportProgress(0, new backgroundWorkerReport() { maximum = maxcount, counter = count, name = filename });
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при разархивировании!" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker1_ProgressChanged_1(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            backgroundWorkerReport report = e.UserState as backgroundWorkerReport;
            UnitsProgress.Text = report.name;
            UnitsProgressBar.Maximum = report.maximum;
            UnitsProgressBar.Value = report.counter;
        }

        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (System.IO.File.Exists(LocalPath.Text + @"\update\" + up.filename)) { System.IO.File.Delete(LocalPath.Text + @"\update\" + up.filename); }
            UnitsProgressBar.Maximum = 100;
            UnitsProgressBar.Value = 100;
            protoSet.name = up.filename;
            protoSet.mainVer = up.version.mainVer;
            protoSet.assemblyVer = up.version.assemblyVer;
            protoSet.editionVer = up.version.editionVer;
            protoSet.error = string.Empty;
            protoSet.updaterpath = Application.StartupPath+ @"\AccentBase_Updater.exe";
            protoSet.serveraddress = FtpAddress.Text;
            protoSet.ftplogin = FtpLogin.Text;
            protoSet.ftppass = FtpPass.Text;

            versionInfo.Save(LocalPath.Text + @"\program\VersionInfo", protoSet);
            if (LocalVersion.Text.Equals("Не установлено!"))
            {
                downloadData();
            }
            else
            {
                if (autoupdate) { Ended(); }
                else
                {
                    DialogResult result = MessageBox.Show("Вы хотите скачать с сервера базы данных?" + Environment.NewLine + "Синхронизация большого объема архивных данных в самой базе может занять много времени.", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        downloadData();
                    }
                    else { Ended(); }
                }
            }
        }

        private void Ended()
        {
            if (Update.Text.Equals("Установить"))
            {
                CorelDrawDllRegistration();
                Update.Text = "Установка прошла успешно.";
            }
            else
            {
                Update.Text = "Обновление прошло успешно.";
            }
            CreateStartMenuShortcut();
            CreateDesktopShortcut();
            checkLocalVersion();
            if (autoupdate)
            {
                Process.Start(LocalPath.Text + @"\program\AccentBase.exe");
                Application.Exit();
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы хотите запустить базу?", Update.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    Process.Start(LocalPath.Text + @"\program\AccentBase.exe");
                    Application.Exit();
                }
            }
            //buttonDelete.Enabled = true; ServerCheck.Enabled = true;
            //UnitsProgress.Enabled = true; Update.Enabled = true; ServerCheck.Enabled = true;
            //LocalPath.Enabled = button_LocalPath.Enabled = FtpAddress.Enabled = FtpLogin.Enabled = FtpPass.Enabled = FtpProgram.Enabled = FtpData.Enabled = buttonDelete.Enabled = button_Save.Enabled = true;
            checkLocalVersion();
        }

        #region Скачивание баз
        private List<string> DataBases = new List<string>();
        public void downloadData()
        {
            DataBases = GetDataList();
            if (DataBases != null && DataBases.Count > 0)
            {
                UnitsProgress.Enabled = true; Update.Enabled = false; ServerCheck.Enabled = false;
                LocalPath.Enabled = button_LocalPath.Enabled = FtpAddress.Enabled = FtpLogin.Enabled = FtpPass.Enabled = FtpProgram.Enabled = FtpData.Enabled = buttonDelete.Enabled = button_Save.Enabled = false;
                UnitsProgressBar.Value = 0;
                FileFromFtp(FtpLogin.Text, FtpPass.Text, FtpAddress.Text, FtpData.Text + @"\" + DataBases[0], LocalPath.Text + @"\data\" + DataBases[0], 1, "Скачивание :" + DataBases[0]);
            }

        }



        public List<string> GetDataList()
        {
            List<string> result = new List<string>();
            try
            {
                using (FtpClient conn = new FtpClient())
                {
                    conn.Host = FtpAddress.Text;
                    conn.Credentials = new NetworkCredential(FtpLogin.Text, FtpPass.Text);
                    conn.SetWorkingDirectory("/");
                    foreach (FtpListItem item in conn.GetListing(conn.GetWorkingDirectory() + FtpData.Text, FtpListOption.AllFiles | FtpListOption.Size))
                    {
                        if (item.Type == FtpFileSystemObjectType.File)
                        {
                            result.Add(item.Name);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return result;
        }

        #endregion

        #region Удаление
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить базу и все данные?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (Directory.Exists(LocalPath.Text))
                {
                    DeleteShortcuts();
                    UnitsProgress.Enabled = true;
                    UnitsProgress.Text = "Подсчёт количества удаляемых файлов";
                    UnitsProgressBar.Style = ProgressBarStyle.Marquee;
                    backgroundWorker2.RunWorkerAsync();
                }
            }
        }

        private void DeleteShortcuts()
        {
            try
            {
                string inkpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AccentBase.lnk";
                if (System.IO.File.Exists(inkpath)) { System.IO.File.Delete(inkpath); }
                inkpath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\Programs\Svetozar\AccentBase.lnk";
                if (System.IO.File.Exists(inkpath)) { System.IO.File.Delete(inkpath); }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LocalPath_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(LocalPath.Text)) { buttonDelete.Enabled = true; } else { buttonDelete.Enabled = false; }
        }



        private void RemoveProgram()
        {
            int files = Directory.GetFiles(LocalPath.Text, "*.*", SearchOption.AllDirectories).Length;
            MessageBox.Show(files.ToString());
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            int files = Directory.GetFiles(LocalPath.Text, "*.*", SearchOption.AllDirectories).Length;
            e.Result = files;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnitsProgressBar.Style = ProgressBarStyle.Blocks;
            UnitsProgressBar.Maximum = (int)e.Result + 1;
            UnitsProgressBar.Value = 0;
            backgroundWorker3.RunWorkerAsync();
        }

        private int getFilesRecursive(string sDir, int count)
        {
            try
            {
                foreach (string file in Directory.GetFiles(sDir))
                {
                    if (backgroundWorker3.CancellationPending) { return 0; }
                    else
                    {
                        count++;
                        backgroundWorker3.ReportProgress(count, file);
                        System.IO.File.Delete(file);
                    }
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    if (backgroundWorker3.CancellationPending) { return 0; }
                    else
                    {
                        count = getFilesRecursive(d, count);
                    }
                    Directory.Delete(d);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return count;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            int result = getFilesRecursive(LocalPath.Text, 0);
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            UnitsProgress.Text = "Удаление: " + e.UserState as string;
            UnitsProgressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UnitsProgress.Text = "База удалена";
            UnitsProgressBar.Maximum = 100;
            UnitsProgressBar.Value = 100;
            Directory.Delete(LocalPath.Text);
            checkLocalVersion();
        }
        #endregion

        private void CreateStartMenuShortcut()
        {
            try
            {
            IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\Programs\Svetozar\AccentBase.lnk") as IWshRuntimeLibrary.IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = LocalPath.Text+ @"\program\AccentBase.exe";
            shortcut.WindowStyle = 1;
            shortcut.Description = "База Акцент";
            shortcut.WorkingDirectory = LocalPath.Text + @"\program\";
            shortcut.Save();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CorelDrawDllRegistration()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = LocalPath.Text + @"\program\setup.bat";
            process.StartInfo = startInfo;
            process.Start();
        }


        private void CreateDesktopShortcut()
        {
            try
            {
                IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\AccentBase.lnk") as IWshRuntimeLibrary.IWshShortcut;
                shortcut.Arguments = "";
                shortcut.TargetPath = LocalPath.Text + @"\program\AccentBase.exe";
                shortcut.WindowStyle = 1;
                shortcut.Description = "База Акцент";
                shortcut.WorkingDirectory = LocalPath.Text + @"\program\";
                shortcut.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
