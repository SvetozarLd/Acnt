using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace AccentBase.CustomControls
{
    public partial class FilesControl : UserControl
    {
        private bool connected = false;
        public FilesControl()
        {
            InitializeComponent();
            //treeListView_Files.CanExpandGetter = delegate (object x) { return true; };
            //treeListView_Files.ChildrenGetter = delegate (object x) { return ((FileNode)x).Children; };
            //FTPFilesList = new List<FileNode> { FTPFilesPrevews, FTPFilesOriginals, FTPFilesDocuments, FTPFilesPhotoReport };
            //this.treeListView_Files.Roots = FTPFilesList;
            FTPFilesList = new List<FileNode> { FTPFilesPrevews, FTPFilesOriginals, FTPFilesDocuments, FTPFilesPhotoReport };
            treeListView_Files.CanExpandGetter = x => (x as FileNode).Children.Count > 0;
            treeListView_Files.ChildrenGetter = x => (x as FileNode).Children;
            //this.treeListView_Files.CanDrop = x => (x as FileNode).Children;
            treeListView_Files.Roots = FTPFilesList;
            treeListView_Files.ExpandAll();
            connected = SocketClient.TableClient.IsConnected;
            SocketClient.TableClient.SocketClientEvent += TableClient_SocketClientEvent;
        }

        #region Событие сервера
        internal delegate void SocketDelegate(SocketClient.TableClient.ConnectEventArgs e);
        //private 
        private void TableClient_SocketClientEvent(object sender, SocketClient.TableClient.ConnectEventArgs e)
        {
            if (e != null)
            {
                try
                {
                    Invoke(new SocketDelegate(ServerHandler), e);
                }
                catch { }
            }
        }
        private void ServerHandler(SocketClient.TableClient.ConnectEventArgs e)
        {
            if (e.Status)
            {
                connected = true;
                getFilesList();
            }
            else
            {
                connected = false;
                //getFilesList();
            }
            //if 
            //textBox1.Text += e.Comment + Environment.NewLine;
            //listBox1.Items[listBox1.Items.Count - 1] = listBox1.Items[listBox1.Items.Count - 1].ToString().Replace("Ожидайте", "Готово");
            //listBox1.Items.Add("Событие подключения:" + e.Message);
            //listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        #endregion

        private long orderID = 0;
        public long OrderID
        {
            get => orderID;
            set
            {
                orderID = value;
                if (orderID == 0)
                {
                    NewOrderFilesList();
                }
                else { getFilesList(); }
            }
        }

        public List<ProtoClasses.ProtoFiles.protoRow> GetNewFiles
        {
            get
            {
                List<ProtoClasses.ProtoFiles.protoRow> resultlist = new List<ProtoClasses.ProtoFiles.protoRow>();
                resultlist.AddRange(NewFilesPreview.Values.ToList());
                resultlist.AddRange(NewFilesOriginals.Values.ToList());
                resultlist.AddRange(NewFilesDoc.Values.ToList());
                resultlist.AddRange(NewFilesReport.Values.ToList());
                return resultlist;
            }
        }


        private void getFilesList()
        {
            pictureBox1.Image = AccentBase.Properties.Resources.iLoading;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Visible = true;
            label1.Visible = true;
            treeListView_Files.Visible = false;
            //toolStrip_Files.Enabled = false;
            toolStrip_Files.Enabled = true;
            if (connected)
            {
                byte[] msg = BitConverter.GetBytes(orderID);
                byte[] message = new byte[6] { (int)SocketClient.TableClient.SocketMessageCommand.GetFilesList, 0, msg[0], msg[1], msg[2], msg[3] };
                SocketClient.TableClient.SendToServer(message);
            }
        }

        private void NewOrderFilesList()
        {
            pictureBox1.Visible = false;
            label1.Visible = false;
            treeListView_Files.Visible = true;
            toolStrip_Files.Enabled = true;
            toolStripButton_Download.Enabled = false;
            toolStripButton_Delete.Enabled = false;
            toolStripButton_Open.Enabled = false;
            toolStripButton_SetPreview.Enabled = false;
        }

        //public List<ProtoClasses.ProtoFiles.protoRow> qqq { get; set; }

        private List<ProtoClasses.ProtoFiles.protoRow> Oldfiles = new List<ProtoClasses.ProtoFiles.protoRow>();

        internal List<ProtoClasses.ProtoFiles.protoRow> SetFiles
        {
            get => Oldfiles;
            set
            {
                Oldfiles = value;
                FillFiles();
                NewOrderFilesList();
            }
        }






        private FileNode FTPFilesPrevews = new FileNode("Предпросмотр", "", "", "", "", null);
        private FileNode FTPFilesOriginals = new FileNode("Оригинал-макеты", " ", "", "", "", null);
        private FileNode FTPFilesDocuments = new FileNode("Документация", "", "", "", "", null);
        private FileNode FTPFilesPhotoReport = new FileNode("Фотоотчет", "", "", "", "", null);
        internal List<FileNode> FTPFilesList = new List<FileNode>();




        private void FillFiles()
        {
            List<ProtoClasses.ProtoFiles.protoRow> resultlist = new List<ProtoClasses.ProtoFiles.protoRow>();
            resultlist.AddRange(NewFilesPreview.Values.ToList());// = NewFiles.Values.ToList();
            resultlist.AddRange(NewFilesOriginals.Values.ToList());
            resultlist.AddRange(NewFilesDoc.Values.ToList());
            resultlist.AddRange(NewFilesReport.Values.ToList());
            foreach (ProtoClasses.ProtoFiles.protoRow protofile in Oldfiles)
            {
                switch (protofile.type)
                {
                    case 0:
                        if (!NewFilesPreview.ContainsKey(protofile.name)) { resultlist.Add(protofile); }
                        break;
                    case 1:
                        if (!NewFilesOriginals.ContainsKey(protofile.name)) { resultlist.Add(protofile); }
                        break;
                    case 2:
                        if (!NewFilesDoc.ContainsKey(protofile.name)) { resultlist.Add(protofile); }
                        break;
                    case 3:
                        if (!NewFilesReport.ContainsKey(protofile.name)) { resultlist.Add(protofile); }
                        break;
                }
                //if (NewFiles.ContainsKey(protofile.name))
                //{
                //    if (protofile.type != NewFiles[protofile.name].type)
                //    {
                //        resultlist.Add(protofile);
                //    }
                //}
                //else
                //{
                //    resultlist.Add(protofile);
                //}
            }




            FTPFilesPrevews.Children.Clear();
            FTPFilesPrevews.Children = new List<FileNode>();
            FTPFilesOriginals.Children.Clear();
            FTPFilesDocuments.Children.Clear();
            FTPFilesPhotoReport.Children.Clear();
            FTPFilesList.Clear();
            DateTime filedate;
            foreach (ProtoClasses.ProtoFiles.protoRow protofile in resultlist)
            {
                filedate = Utils.UnixDate.Int64ToDateTime(protofile.LastWriteTime);
                FileNode filenode = new FileNode(protofile.name, protofile.status, LengthToString(protofile.Length), filedate.ToString("MM.dd.yy H:mm"), protofile.fullname, FTPFilesPrevews)
                {
                    LastCreationTime = protofile.CreationTime,
                    LastWriteTime = protofile.LastWriteTime,
                    Length = protofile.Length,
                    sourcefile = protofile.fullname
                };
                switch (protofile.type)
                {
                    case 0:
                        filenode.Parent = FTPFilesPrevews;
                        //FTPFilesPrevews.Children.Add(new FileNode(protofile.name, FileSize.ToString() + fileSize, filedate.ToString("MM.dd.yy H:mm"), protofile.fullname, FTPFilesPrevews));
                        FTPFilesPrevews.Children.Add(filenode);
                        break;
                    case 1:
                        filenode.Parent = FTPFilesOriginals;
                        //FTPFilesOriginals.Children.Add(new FileNode(protofile.name, FileSize.ToString() + fileSize, filedate.ToString("MM.dd.yy H:mm"), protofile.fullname, FTPFilesOriginals));
                        FTPFilesOriginals.Children.Add(filenode);
                        break;
                    case 2:
                        filenode.Parent = FTPFilesDocuments;
                        //FTPFilesDocuments.Children.Add(new FileNode(protofile.name, FileSize.ToString() + fileSize, filedate.ToString("MM.dd.yy H:mm"), protofile.fullname, FTPFilesDocuments));
                        FTPFilesDocuments.Children.Add(filenode);
                        break;
                    case 3:
                        filenode.Parent = FTPFilesPhotoReport;
                        //FTPFilesPhotoReport.Children.Add(new FileNode(protofile.name, FileSize.ToString() + fileSize, filedate.ToString("MM.dd.yy H:mm"), protofile.fullname, FTPFilesPhotoReport));
                        FTPFilesPhotoReport.Children.Add(filenode);
                        break;
                }

            }

            FTPFilesList = new List<FileNode> { FTPFilesPrevews, FTPFilesOriginals, FTPFilesDocuments, FTPFilesPhotoReport };
            treeListView_Files.Roots = FTPFilesList;
            treeListView_Files.ExpandAll();

        }

        private void treeListView_Files_DragOver(object sender, DragEventArgs e)
        {
            //System.Drawing.Point clientPoint = treeListView_Files.PointToClient(new System.Drawing.Point(e.X, e.Y));
            //OLVListItem targetnode = (OLVListItem)treeListView_Files.HitTest(clientPoint.X, clientPoint.Y).Item;
            //if (targetnode != null)
            //{
            //    e.Effect = DragDropEffects.Move;
            //    //if (targetnode.p)
            //    targetnode.Selected = true;

            //    FileNode fn = treeListView_Files.SelectedObject as FileNode;
            //    if (fn.Parent != null) { treeListView_Files.SelectedObject = fn.Parent; }
            //    FileNode fn1 = treeListView_Files.SelectedObject as FileNode;
            //    //Trace.WriteLine(fn1.FileName);

            //    //treeListView_Files.SelectedObject = targetnode;
            //}
            ////DragDropEffects effect = new DragDropEffects();

            ////Trace.WriteLine(treeListView_Files.SelectedItem.GetSubItem(0).ToString());
            ////tlv1.GetParent(tlv1.SelectedModel)
            ////if (treeListView_Files.SelectedObject != null)
            ////{
            ////    Object fn = treeListView_Files.SelectedObject as FileNode;
            ////}
            ////e.AllowedEffect = DragDropEffects.Copy;
            ////if (e.Data != null)
            ////{
            ////    FileNode fn = e.Data as FileNode;
            ////    Trace.WriteLine(fn.FileName);
            ////}
            ////if (e.Data.GetDataPresent(typeof(OLVDataObject))) { e.Effect = DragDropEffects.Move; }


        }

        private void treeListView_Files_CanDrop(object sender, OlvDropEventArgs e)
        {
            if (e.DropTargetItem != null) { e.Effect = DragDropEffects.Copy; } else { e.Effect = DragDropEffects.None; }
        }

        private void treeListView_Files_SelectedIndexChanged(object sender, EventArgs e)
        {
            //IList<FileNode> fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;
            System.Collections.ArrayList fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;

            if (fn != null)
            {
                //Trace.WriteLine(fn.Length);
            }
        }

        private FileNode nodeByClick = null;
        private void treeListView_Files_MouseDown(object sender, MouseEventArgs e)
        {
            ////System.Drawing.Point clientPoint = treeListView_Files.PointToClient(new System.Drawing.Point(e.X, e.Y));
            //OLVListItem targetnode = (OLVListItem)treeListView_Files.HitTest(e.X, e.Y).Item;
            //if (targetnode != null)
            //{
            //    Trace.WriteLine(targetnode.Text);

            //}
            //else { Trace.WriteLine("targetnode = null"); }
            //treeListView_Files.SelectedItem = (OLVListItem)treeListView_Files.HitTest(e.X, e.Y).Item;
            //Trace.WriteLine(e.X+":"+e.Y +"________________"+ clientPoint.X+":"+clientPoint.Y);
            //Trace.WriteLine("treeListView_Files.SelectedItem:" + treeListView_Files.SelectedItem.Text);
            if (e.Button == MouseButtons.Right)
            {
                //System.Drawing.Point clientPoint = treeListView_Files.PointToClient(new System.Drawing.Point(e.X, e.Y));
                //OLVListItem targetnode = (OLVListItem)treeListView_Files.HitTest(clientPoint.X, clientPoint.Y).Item;
                //OLVListItem targetnode = (OLVListItem)treeListView_Files.SelectedItem;
                OLVListItem targetnode = (OLVListItem)treeListView_Files.HitTest(e.X, e.Y).Item;
                if (targetnode != null)
                {
                    nodeByClick = targetnode.RowObject as FileNode;
                    ToolStripMenuItem_Upload.Enabled = true;
                    if (treeListView_Files.SelectedObjects != null && treeListView_Files.SelectedObjects.Count == 1) { treeListView_Files.SelectedItem = targetnode; }
                }
                else
                {
                    nodeByClick = null; ToolStripMenuItem_Upload.Enabled = false; ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить";
                    //if (treeListView_Files.SelectedObjects.Count == 1)
                    //{ treeListView_Files.SelectedObjects.RemoveAt(0); }
                }
                
                CheckDeleteFiles(targetnode);
                //System.Drawing.Point clientPoint = treeListView_Files.PointToClient(new System.Drawing.Point(e.X, e.Y));
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                //contextMenuStrip1.Show();
            }
            //System.Collections.ArrayList fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;

            //if (fn != null)
            //{
            //    //Trace.WriteLine(fn.Length);
            //}
        }

        #region поверка на удаление

        private List<FileNode> deleteNodes = new List<FileNode>();

        private void CheckDeleteFiles(OLVListItem clickedfileNode)
        {
            System.Collections.ArrayList fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;
            if (clickedfileNode == null && fn == null) { ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить"; return; }
            if (fn.Count == 0)
            {
                if (clickedfileNode == null) { ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить"; return; } 
                fn.Add((FileNode)clickedfileNode.RowObject);
            }
            switch (fn.Count)
            {
                case 0:
                    ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить";
                    break;
                case 1:
                    //FileNode fnode = (FileNode)fn[0];
                    FileNode fnode = fn[0] as FileNode;
                    if (fnode.FileStatus.Equals("На сервере")) { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Удалить с сервера"; }
                    else
                    {
                        if (fnode.FileStatus.Equals("Удалить с сервера"))
                        { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Отменить удаление"; }
                        else
                        {
                            if (fnode == FTPFilesPrevews || fnode == FTPFilesOriginals || fnode == FTPFilesDocuments || fnode == FTPFilesPhotoReport)
                            { ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить"; return; }
                            else
                            {
                                ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Отменить отправку";
                            }
                        }
                    }
                    break;
                default:
                    bool serverNodes = false;
                    bool localNodes = false;
                    bool serverCancelNodes = false;
                    foreach (FileNode filenode in fn)
                    {
                        if (filenode.FileStatus.Equals("На сервере")) { serverNodes = true; }
                        else
                        {
                            if (filenode.FileStatus.Equals("Отправка")) { localNodes = true; } else
                            {
                                if (filenode.FileStatus.Equals("Удалить с сервера")) { serverCancelNodes = true; }
                            }
                        }

                    }
                    if (serverCancelNodes && !serverNodes && !localNodes) { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Отменить удаление"; return; }
                    if (serverNodes && localNodes) { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Удалить"; }
                    else
                    {

                        if (serverNodes) { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Удалить с сервера"; }
                        else
                        {
                            if (localNodes) { ToolStripMenuItem_Delete.Enabled = true; ToolStripMenuItem_Delete.Text = "Отменить отправку"; } else { ToolStripMenuItem_Delete.Enabled = false; ToolStripMenuItem_Delete.Text = "Удалить"; }
                        }
                        
                    }
                    break;
            }
        }
        #endregion
        #region Скачивание всех файлов с preview
        public enum MainFolderName
        {
            Preview,
            Originals,
            Documents,
            PhotoReport
        }
        public void DownloadsFilesFromMainFolder(MainFolderName folderName)
        {
            FileNode filesparentnode = null;
            switch (folderName)
            {
                case MainFolderName.Preview:
                    filesparentnode = FTPFilesPrevews;
                    break;
                case MainFolderName.Originals:
                    filesparentnode = FTPFilesOriginals;
                    break;
                case MainFolderName.Documents:
                    filesparentnode = FTPFilesDocuments;
                    break;
                case MainFolderName.PhotoReport:
                    filesparentnode = FTPFilesPhotoReport;
                    break;
            }
            if (filesparentnode == null) { return; }
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (Utils.Settings.set.SavePath != string.Empty) { fbd.SelectedPath = Utils.Settings.set.SavePath; }
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    List<ProtoClasses.ProtoFtpSchedule.protoRow> savelist = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();
                    foreach (FileNode fnode in filesparentnode.Children)
                    {
                        if (fnode.FileStatus == "На сервере")
                        {
                            FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + fnode.FileName);
                            if (fi != null)
                            {
                                ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                {
                                    Upload = false,
                                    LastCreationTime = fnode.LastCreationTime,
                                    LastWriteTime = fnode.LastWriteTime,
                                    Length = fnode.Length,
                                    fileshortname = fi.Name,
                                    serveraddress = @"Server/" + fnode.FullPath,
                                    sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                    LengthString = fnode.FileSize,
                                    targetfile = fi.FullName
                                };
                                //savelist = CheckEqualsFileNames(savelist, pr);
                                SqlLite.FtpSchedule.Insert(pr);
                            }
                        }
                    }
                    Utils.Settings.set.SavePath = fbd.SelectedPath; Utils.Settings.Save();
                }
            }

        }

        public void DownloadsAllFilesToOneFolder()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (Utils.Settings.set.SavePath != string.Empty) { fbd.SelectedPath = Utils.Settings.set.SavePath; }
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    List<ProtoClasses.ProtoFtpSchedule.protoRow> savelist = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();
                    foreach (FileNode fnode in FTPFilesPrevews.Children)
                    {
                        if (fnode.FileStatus == "На сервере")
                        {
                            FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + fnode.FileName);
                            if (fi != null)
                            {
                                ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                {
                                    Upload = false,
                                    LastCreationTime = fnode.LastCreationTime,
                                    LastWriteTime = fnode.LastWriteTime,
                                    Length = fnode.Length,
                                    fileshortname = fi.Name,
                                    serveraddress = @"Server/" + fnode.FullPath,
                                    sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                    LengthString = fnode.FileSize,
                                    targetfile = fi.FullName
                                };
                                //savelist = CheckEqualsFileNames(savelist, pr);
                                SqlLite.FtpSchedule.Insert(pr);
                            }
                        }
                    }
                    foreach (FileNode fnode in FTPFilesOriginals.Children)
                    {
                        if (fnode.FileStatus == "На сервере")
                        {
                            FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + fnode.FileName);
                            if (fi != null)
                            {
                                ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                {
                                    Upload = false,
                                    LastCreationTime = fnode.LastCreationTime,
                                    LastWriteTime = fnode.LastWriteTime,
                                    Length = fnode.Length,
                                    fileshortname = fi.Name,
                                    serveraddress = @"Server/" + fnode.FullPath,
                                    sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                    LengthString = fnode.FileSize,
                                    targetfile = fi.FullName
                                };
                                //savelist = CheckEqualsFileNames(savelist, pr);
                                SqlLite.FtpSchedule.Insert(pr);
                            }
                        }
                    }
                    foreach (FileNode fnode in FTPFilesDocuments.Children)
                    {
                        if (fnode.FileStatus == "На сервере")
                        {
                            FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + fnode.FileName);
                            if (fi != null)
                            {
                                ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                {
                                    Upload = false,
                                    LastCreationTime = fnode.LastCreationTime,
                                    LastWriteTime = fnode.LastWriteTime,
                                    Length = fnode.Length,
                                    fileshortname = fi.Name,
                                    serveraddress = @"Server/" + fnode.FullPath,
                                    sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                    LengthString = fnode.FileSize,
                                    targetfile = fi.FullName
                                };
                                //savelist = CheckEqualsFileNames(savelist, pr);
                                SqlLite.FtpSchedule.Insert(pr);
                            }
                        }
                    }
                    foreach (FileNode fnode in FTPFilesPhotoReport.Children)
                    {
                        if (fnode.FileStatus == "На сервере")
                        {
                            FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + fnode.FileName);
                            if (fi != null)
                            {
                                ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                {
                                    Upload = false,
                                    LastCreationTime = fnode.LastCreationTime,
                                    LastWriteTime = fnode.LastWriteTime,
                                    Length = fnode.Length,
                                    fileshortname = fi.Name,
                                    serveraddress = @"Server/" + fnode.FullPath,
                                    sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                    LengthString = fnode.FileSize,
                                    targetfile = fi.FullName
                                };
                                //savelist = CheckEqualsFileNames(savelist, pr);
                                SqlLite.FtpSchedule.Insert(pr);
                            }
                        }
                    }
                    Utils.Settings.set.SavePath = fbd.SelectedPath; Utils.Settings.Save();
                }
            }

        }

        #endregion


        #region Сохранение файлов
        private void SaveFiles_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;

            if (fn != null)
            {


                switch (fn.Count)
                {
                    case 0:
                        break;
                    case 1:

                        FileNode fnode = (FileNode)fn[0];

                        if (fnode == FTPFilesPrevews) { DownloadsFilesFromMainFolder(MainFolderName.Preview); }
                        else
                        {
                            if (fnode == FTPFilesOriginals) { DownloadsFilesFromMainFolder(MainFolderName.Originals); }
                            else
                            {
                                if (fnode == FTPFilesDocuments) { DownloadsFilesFromMainFolder(MainFolderName.Documents); }
                                else
                                {
                                    if (fnode == FTPFilesPhotoReport) { DownloadsFilesFromMainFolder(MainFolderName.PhotoReport); }
                                    else
                                    {
                                        using (SaveFileDialog fbd = new SaveFileDialog())
                                        {
                                            if (Utils.Settings.set.SavePath != string.Empty) { fbd.InitialDirectory = Utils.Settings.set.SavePath; };
                                            fbd.FileName = new FileInfo(fnode.FileName).Name;
                                            DialogResult result = fbd.ShowDialog();
                                            if (result == DialogResult.OK)
                                            {
                                                if (fnode.FileStatus == "На сервере")
                                                {
                                                    FileInfo fi = new FileInfo(fbd.FileName);
                                                    if (fi != null && fbd.ValidateNames)
                                                    {
                                                        ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                                        {
                                                            Upload = false,
                                                            LastCreationTime = fnode.LastCreationTime,
                                                            LastWriteTime = fnode.LastWriteTime,
                                                            Length = fnode.Length,
                                                            fileshortname = fi.Name,
                                                            serveraddress = @"Server/" + fnode.FullPath,
                                                            sourcefile = "makets" + @"/" + fnode.FullPath.Replace(@"//", @"\"),
                                                            LengthString = fnode.FileSize,
                                                            targetfile = fbd.FileName,
                                                            order_id = orderID
                                                        };
                                                        SqlLite.FtpSchedule.Insert(pr);
                                                    }
                                                }
                                                Utils.Settings.set.SavePath = fbd.InitialDirectory; Utils.Settings.Save();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    default:

                        //MessageBox.Show("Выборочное скачивание группы файлов находится в процессе реализации" + Environment.NewLine + "На данный момент Вы можете скачать каждый файл по отдельности, либо сразу весь пакет файлов с главного окна заданий", "Извините", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                        {
                            if (Utils.Settings.set.SavePath != string.Empty) { fbd.SelectedPath = Utils.Settings.set.SavePath; }
                            DialogResult result = fbd.ShowDialog();

                            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                            {
                                //List<ProtoClasses.ProtoFtpSchedule.protoRow> savelist = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();
                                foreach (FileNode filenode in fn)
                                {
                                    if (filenode.FileStatus == "На сервере")
                                    {
                                        FileInfo fi = new FileInfo(fbd.SelectedPath + @"/" + filenode.FileName);
                                        if (fi != null)
                                        {
                                            ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                                            {
                                                Upload = false,
                                                LastCreationTime = filenode.LastCreationTime,
                                                LastWriteTime = filenode.LastWriteTime,
                                                Length = filenode.Length,
                                                fileshortname = fi.Name,
                                                serveraddress = @"Server/" + filenode.FullPath,
                                                sourcefile = "makets" + @"/" + filenode.FullPath.Replace(@"//", @"\"),
                                                LengthString = filenode.FileSize,
                                                targetfile = fi.FullName
                                            };
                                            //savelist = CheckEqualsFileNames(savelist, pr);
                                            SqlLite.FtpSchedule.Insert(pr);
                                        }
                                    }
                                }
                                Utils.Settings.set.SavePath = fbd.SelectedPath; Utils.Settings.Save();
                            }
                        }
                        break;
                }


            }
        }


        //#region Скачивание пакета файлов с проверкой на совпадение имён
        //private void CheckEqualsFileNames2(List<ProtoClasses.ProtoFtpSchedule.protoRow> items)
        //{
        //    HashSet<string> names = new HashSet<string>();
        //    string tmp;
        //    foreach (ProtoClasses.ProtoFtpSchedule.protoRow elem in items)
        //    {
        //        if (names.Contains(elem.fileshortname))
        //        {
        //            tmp = ReNamer(elem.fileshortname);
        //            //elem.targetfile

        //        }
        //        else { names.Add(elem.fileshortname); }


        //    }



        //}

        //private string ReNamer(string fn)
        //{
        //    string result = fn;
        //    using (Forms.FormFileRename renamer = new Forms.FormFileRename(fn))
        //    {
        //        renamer.StartPosition = FormStartPosition.CenterParent;
        //        renamer.ShowDialog(this);
        //        result = renamer.FileName;
        //        if (result.Equals(fn)) { result = ReNamer(fn); }
        //    }
        //    return result;
        //}

        //#endregion
        //#region Проверка при скачивании - чтобы были только файлы с сервера, без папок.



        //private List<ProtoClasses.ProtoFtpSchedule.protoRow> CheckEqualsFileNames(List<ProtoClasses.ProtoFtpSchedule.protoRow> savelist, ProtoClasses.ProtoFtpSchedule.protoRow pr)
        //{
        //    List<ProtoClasses.ProtoFtpSchedule.protoRow> newList = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();
        //    if (newList.Count > 0)
        //    {
        //        foreach (ProtoClasses.ProtoFtpSchedule.protoRow prold in savelist)
        //        {
        //            if (prold.fileshortname == pr.fileshortname)
        //            {
        //                using (Forms.FormFileRename renamer = new Forms.FormFileRename(prold.fileshortname))
        //                {
        //                    renamer.ShowDialog(this);

        //                    if (renamer.FileName != prold.fileshortname)
        //                    {
        //                        newList.Add(prold);
        //                        pr.sourcefile = pr.sourcefile + "%";
        //                        pr.sourcefile = pr.sourcefile.Replace(@"/" + pr.fileshortname + "%", @"/" + renamer.FileName);
        //                        pr.fileshortname = renamer.FileName;
        //                    }
        //                    else
        //                    {
        //                        newList.Add(pr);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                newList.Add(prold);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        newList.Add(pr);
        //    }
        //    return newList;
        //}
        //#endregion

        #endregion




        private void treeListView_Files_DragDrop(object sender, DragEventArgs e)
        {
            System.Drawing.Point clientPoint = treeListView_Files.PointToClient(new System.Drawing.Point(e.X, e.Y));
            OLVListItem targetnode = (OLVListItem)treeListView_Files.HitTest(clientPoint.X, clientPoint.Y).Item;
            if (targetnode != null)
            {

                FileNode tn = targetnode.RowObject as FileNode;


                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                DnDCreateFileList(files, GetMainNodes(tn, 0));
            }

        }

        private int GetMainNodes(FileNode tn, int n)
        {
            if (tn.FileName == "Предпросмотр") { return 0; }
            if (tn.FileName == "Оригинал-макеты") { return 1; }
            if (tn.FileName == "Документация") { return 2; }
            if (tn.FileName == "Фотоотчет") { return 3; }
            GetMainNodes(tn.Parent, 0);
            return GetMainNodes(tn.Parent, 0);
        }



        #region Добавление новых файлов
        #region преобразование списка имен добавляемых файлов в Dictionary
        private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesPreview = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesOriginals = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesDoc = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesReport = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();

        public List<ProtoClasses.ProtoFtpSchedule.protoRow> GetNewFtp
        {
            get
            {
                List<ProtoClasses.ProtoFtpSchedule.protoRow> result = new List<ProtoClasses.ProtoFtpSchedule.protoRow>();

                foreach (ProtoClasses.ProtoFiles.protoRow row in NewFilesPreview.Values)
                {
                    ProtoClasses.ProtoFtpSchedule.protoRow resultrow = new ProtoClasses.ProtoFtpSchedule.protoRow
                    {
                        sourcefile = row.fullname,
                        targetfile = "preview" + @"/" + row.name,
                        Length = row.Length,
                        LengthString = LengthToString(row.Length),
                        fileshortname = row.name,
                        LastCreationTime = row.CreationTime,
                        LastWriteTime = row.LastWriteTime,
                        Upload = true
                    };
                    result.Add(resultrow);
                }
                foreach (ProtoClasses.ProtoFiles.protoRow row in NewFilesOriginals.Values)
                {
                    ProtoClasses.ProtoFtpSchedule.protoRow resultrow = new ProtoClasses.ProtoFtpSchedule.protoRow
                    {
                        sourcefile = row.fullname,
                        targetfile = "makets" + @"/" + row.name,
                        Length = row.Length,
                        LengthString = LengthToString(row.Length),
                        fileshortname = row.name,
                        LastCreationTime = row.CreationTime,
                        LastWriteTime = row.LastWriteTime,
                        Upload = true
                    };
                    result.Add(resultrow);
                }
                foreach (ProtoClasses.ProtoFiles.protoRow row in NewFilesDoc.Values)
                {
                    ProtoClasses.ProtoFtpSchedule.protoRow resultrow = new ProtoClasses.ProtoFtpSchedule.protoRow
                    {
                        sourcefile = row.fullname,
                        targetfile = "doc" + @"/" + row.name,
                        Length = row.Length,
                        LengthString = LengthToString(row.Length),
                        fileshortname = row.name,
                        LastCreationTime = row.CreationTime,
                        LastWriteTime = row.LastWriteTime,
                        Upload = true
                    };
                    result.Add(resultrow);
                }
                foreach (ProtoClasses.ProtoFiles.protoRow row in NewFilesReport.Values)
                {
                    ProtoClasses.ProtoFtpSchedule.protoRow resultrow = new ProtoClasses.ProtoFtpSchedule.protoRow
                    {
                        sourcefile = row.fullname,
                        targetfile = "photoreport" + @"/" + row.name,
                        Length = row.Length,
                        LengthString = LengthToString(row.Length),
                        fileshortname = row.name,
                        LastCreationTime = row.CreationTime,
                        LastWriteTime = row.LastWriteTime,
                        Upload = true
                    };
                    result.Add(resultrow);
                }

                return result;
            }
        }

        private string LengthToString(double Length)
        {
            string fileSize = string.Empty; double FileSize = Length;
            if (FileSize > 0)
            {
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Kb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Mb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Gb"; }
            }
            FileSize = Math.Round(FileSize, 2);
            return FileSize.ToString() + fileSize;
        }


        private void DnDCreateFileList(string[] ob, int type)
        {
            ProtoClasses.ProtoFiles.protoRow pr = null;
            foreach (string file in ob)
            {
                FileAttributes attr = File.GetAttributes(file);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    string[] files = Directory.GetFiles(file, "*", SearchOption.AllDirectories);
                    foreach (string file2 in files)
                    {
                        FileInfo fi = new FileInfo(file2);
                        pr = StringToDic(fi, type);
                        AddNewFiles(pr);
                    }
                }
                else
                {
                    FileInfo fi = new FileInfo(file);
                    pr = StringToDic(fi, type);
                    AddNewFiles(pr);
                }
            }
            FillFiles();
            overwriteAddingFiles = false;
        }
        #endregion

        private bool overwriteAddingFiles = false; // флаг перезаписи файлов при добавлении
        private void AddNewFiles(ProtoClasses.ProtoFiles.protoRow pr)
        {
            switch (pr.type)
            {
                case 0:
                    if (NewFilesPreview.ContainsKey(pr.name) || checkAddingsFilenames(pr))
                    {
                        if (!overwriteAddingFiles)
                        {
                            using (Forms.FormFileRename renamer = new Forms.FormFileRename(pr.name, false, true))
                            {
                                renamer.StartPosition = FormStartPosition.CenterParent;
                                renamer.ShowDialog(this);
                                if (!renamer.Cancelation)
                                {
                                    if (renamer.FileName != pr.name)
                                    {
                                        pr.fullname = pr.fullname + "%";
                                        pr.fullname = pr.fullname.Replace(@"/" + pr.name + "%", @"/" + renamer.FileName);
                                        pr.name = renamer.FileName;
                                        NewFilesPreview.Add(pr.name, pr);
                                        overwriteAddingFiles = renamer.Overwrite;
                                    }
                                    else
                                    {
                                        overwriteAddingFiles = renamer.Overwrite;
                                        NewFilesPreview[pr.name] = pr;
                                    }
                                }
                            }
                        }
                        else { NewFilesPreview[pr.name] = pr; }
                    }
                    else { NewFilesPreview.Add(pr.name, pr); }
                    break;
                case 1:
                    if (NewFilesOriginals.ContainsKey(pr.name) || checkAddingsFilenames(pr))
                    {
                        if (!overwriteAddingFiles)
                        {
                            using (Forms.FormFileRename renamer = new Forms.FormFileRename(pr.name, false, true))
                            {
                                renamer.StartPosition = FormStartPosition.CenterParent;
                                renamer.ShowDialog(this);
                                if (!renamer.Cancelation)
                                {
                                    if (renamer.FileName != pr.name)
                                    {
                                        pr.fullname = pr.fullname + "%";
                                        pr.fullname = pr.fullname.Replace(@"/" + pr.name + "%", @"/" + renamer.FileName);
                                        pr.name = renamer.FileName;
                                        NewFilesOriginals.Add(pr.name, pr);
                                    }
                                    else
                                    {
                                        //FileNode fkf = new FileNode("", "", "", "", "", null);
                                        //foreach (FileNode fn in (FileNode[])treeListView_Files.)

                                        overwriteAddingFiles = renamer.Overwrite;
                                        NewFilesOriginals[pr.name] = pr;
                                    }
                                }
                            }
                        }
                        else { NewFilesOriginals[pr.name] = pr; }
                    }
                    else { NewFilesOriginals.Add(pr.name, pr); }
                    break;
                case 2:

                    if (NewFilesDoc.ContainsKey(pr.name) || checkAddingsFilenames(pr))
                    {
                        if (!overwriteAddingFiles)
                        {
                            using (Forms.FormFileRename renamer = new Forms.FormFileRename(pr.name, false, true))
                            {
                                renamer.StartPosition = FormStartPosition.CenterParent;
                                renamer.ShowDialog(this);
                                if (!renamer.Cancelation)
                                {
                                    if (renamer.FileName != pr.name)
                                    {
                                        pr.fullname = pr.fullname + "%";
                                        pr.fullname = pr.fullname.Replace(@"/" + pr.name + "%", @"/" + renamer.FileName);
                                        pr.name = renamer.FileName;
                                        NewFilesDoc.Add(pr.name, pr);
                                    }
                                    else
                                    {
                                        overwriteAddingFiles = renamer.Overwrite;
                                        NewFilesDoc[pr.name] = pr;
                                    }
                                }
                            }
                        }
                        else { NewFilesDoc[pr.name] = pr; }
                    }
                    else { NewFilesDoc.Add(pr.name, pr); }
                    break;
                case 3:

                    if (NewFilesReport.ContainsKey(pr.name) || checkAddingsFilenames(pr))
                    {
                        if (!overwriteAddingFiles)
                        {
                            using (Forms.FormFileRename renamer = new Forms.FormFileRename(pr.name, false, true))
                            {
                                renamer.StartPosition = FormStartPosition.CenterParent;
                                renamer.ShowDialog(this);
                                if (!renamer.Cancelation)
                                {
                                    if (renamer.FileName != pr.name)
                                    {
                                        pr.fullname = pr.fullname + "%";
                                        pr.fullname = pr.fullname.Replace(@"/" + pr.name + "%", @"/" + renamer.FileName);
                                        pr.name = renamer.FileName;
                                        NewFilesReport.Add(pr.name, pr);
                                    }
                                    else
                                    {
                                        overwriteAddingFiles = renamer.Overwrite;
                                        NewFilesReport[pr.name] = pr;
                                    }
                                }
                            }
                        }
                        else { NewFilesReport[pr.name] = pr; }
                    }
                    else { NewFilesReport.Add(pr.name, pr); }
                    break;
            }
        }
        #region проверка - есть ли файлы с такими именами на сервере
        private bool checkAddingsFilenames(ProtoClasses.ProtoFiles.protoRow pr)
        {
            foreach (ProtoClasses.ProtoFiles.protoRow rw in Oldfiles)
            {
                if (rw.name == pr.name && rw.type == pr.type)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region преобразование файла по имени в ProtoClasses.ProtoFiles.protoRow
        private ProtoClasses.ProtoFiles.protoRow StringToDic(FileInfo fi, int type)
        {
            ProtoClasses.ProtoFiles.protoRow pr = new ProtoClasses.ProtoFiles.protoRow
            {
                type = type,
                Length = fi.Length,
                LastWriteTime = Utils.UnixDate.DateTimeToInt64(fi.LastWriteTime),
                name = fi.Name,
                fullname = fi.FullName,
                CreationTime = Utils.UnixDate.DateTimeToInt64(fi.CreationTime),
                status = "Отправка"
            };
            return pr;
        }
        #endregion

        #endregion


        public void UploadFilesToMainFolder(MainFolderName mainFolderName)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = Utils.Settings.set.SavePath;
                openFileDialog1.Filter = "Все (*.*)| *.*";
                openFileDialog1.FilterIndex = 0;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Multiselect = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] files = openFileDialog1.FileNames;
                    switch (mainFolderName)
                    {
                        case MainFolderName.Preview: DnDCreateFileList(files, 0); break;
                        case MainFolderName.Originals: DnDCreateFileList(files, 1); break;
                        case MainFolderName.Documents: DnDCreateFileList(files, 2); break;
                        case MainFolderName.PhotoReport: DnDCreateFileList(files, 3); break;
                    }
                }
            }
        }



        private void ToolStripMenuItem_Upload_Click(object sender, EventArgs e)
        {
            if (nodeByClick != null)
            {
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                {
                    openFileDialog1.InitialDirectory = Utils.Settings.set.SavePath;
                    openFileDialog1.Filter = "Все (*.*)| *.*";
                    openFileDialog1.FilterIndex = 0;
                    openFileDialog1.RestoreDirectory = true;
                    openFileDialog1.Multiselect = true;

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string[] files = openFileDialog1.FileNames;

                        DnDCreateFileList(files, GetMainNodes(nodeByClick, 0));
                    }
                }
            }

        }


        //private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesPreview = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        //private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesOriginals = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        //private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesDoc = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();
        //private Dictionary<string, ProtoClasses.ProtoFiles.protoRow> NewFilesReport = new Dictionary<string, ProtoClasses.ProtoFiles.protoRow>();


        private void ToolStripMenuItem_Delete_Click(object sender, EventArgs e)
        {
            //GetNewFtp
            System.Collections.ArrayList fn = (System.Collections.ArrayList)treeListView_Files.SelectedObjects;

            if (fn != null)
            {


                switch (fn.Count)
                {
                    case 0:
                        break;
                    case 1:
                        FileNode fnode = (FileNode)fn[0];
                        if (fnode.FileStatus.Equals("На сервере"))
                        {
                            DeleteServerFiles(fnode);
                        }
                        else
                        {
                            if (fnode.FileStatus.Equals("Отправка"))
                            {
                                DeleteUploadFiles(fnode);
                            }
                            else
                            {
                                if (fnode.FileStatus.Equals("Удалить с сервера"))
                                {
                                    CancelDeleteServerFiles(fnode);
                                }
                            }
                        }
                        FillFiles();
                        break;
                    default:
                        foreach (FileNode filenode in fn)
                        {
                            if (filenode.FileStatus.Equals("На сервере"))
                            {
                                DeleteServerFiles(filenode);
                            }
                            else
                            {
                                if (filenode.FileStatus.Equals("Отправка"))
                                {
                                    DeleteUploadFiles(filenode);
                                }
                                else
                                {
                                    if (filenode.FileStatus.Equals("Удалить с сервера"))
                                    {
                                        CancelDeleteServerFiles(filenode);
                                    }
                                }
                            }
                        }
                        FillFiles();
                        break;
                }
            }
        }

        private void DeleteUploadFiles(FileNode filenode)
        {
            if (filenode.Parent == FTPFilesPrevews)
            {
                if (NewFilesPreview.ContainsKey(filenode.FileName)) { NewFilesPreview.Remove(filenode.FileName); }
                return;
            }
            if (filenode.Parent == FTPFilesOriginals)
            {
                if (NewFilesOriginals.ContainsKey(filenode.FileName)) { NewFilesOriginals.Remove(filenode.FileName); }
                return;
            }
            if (filenode.Parent == FTPFilesDocuments)
            {
                if (NewFilesDoc.ContainsKey(filenode.FileName)) { NewFilesDoc.Remove(filenode.FileName); }
                return;
            }
            if (filenode.Parent == FTPFilesPhotoReport)
            {
                if (NewFilesReport.ContainsKey(filenode.FileName)) { NewFilesReport.Remove(filenode.FileName); }
                return;
            }
        }

        private List<ProtoClasses.ProtoFiles.protoRow> DeleteFilesList = new List<ProtoClasses.ProtoFiles.protoRow>();
        private void DeleteServerFiles(FileNode filenode)
        {
            Oldfiles.Where(w => w.fullname == filenode.sourcefile).ToList().ForEach(s => s.status = "Удалить с сервера");
            ProtoClasses.ProtoFiles.protoRow tmp = Oldfiles.Where(w => w.fullname == filenode.sourcefile).Single();
            if (tmp != null) { DeleteFilesList.AddRange(Oldfiles.Where(w => w.fullname == filenode.sourcefile).ToList()); }
            #region _____________________
            //if (filenode.Parent == FTPFilesPrevews)
            //{
            //    if (FTPFilesPrevews.Children.Contains(filenode))
            //    {
            //        Oldfiles.Where(w => w.fullname == filenode.sourcefile).ToList().ForEach(s => s.status = "Удалить с сервера");
            //        //FTPFilesPrevews.Children.Where(w => w.FileName == filenode.FileName).ToList().ForEach(s => s.FileStatus = "Удалить с сервера");
            //        //FTPFilesPrevews.Children.Remove(filenode); filenode.FileStatus = "Удалить с сервера"; FTPFilesPrevews.a
            //    }
            //    return;
            //}
            //if (filenode.Parent == FTPFilesOriginals)
            //{
            //    if (NewFilesOriginals.ContainsKey(filenode.FileName)) { NewFilesOriginals.Remove(filenode.FileName); }
            //    return;
            //}
            //if (filenode.Parent == FTPFilesDocuments)
            //{
            //    if (NewFilesDoc.ContainsKey(filenode.FileName)) { NewFilesDoc.Remove(filenode.FileName); }
            //    return;
            //}
            //if (filenode.Parent == FTPFilesPhotoReport)
            //{
            //    if (NewFilesReport.ContainsKey(filenode.FileName)) { NewFilesReport.Remove(filenode.FileName); }
            //    return;
            //}
            #endregion
        }
        private void CancelDeleteServerFiles(FileNode filenode)
        {
            Oldfiles.Where(w => w.fullname == filenode.sourcefile).ToList().ForEach(s => s.status = "На сервере");
            ProtoClasses.ProtoFiles.protoRow tmp = Oldfiles.Where(w => w.fullname == filenode.sourcefile).Single();
            if (tmp != null) { DeleteFilesList.Remove(tmp); }
        }
        public List<ProtoClasses.ProtoFiles.protoRow> GetDeleteFilesList => DeleteFilesList;
    }
}
