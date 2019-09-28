namespace AccentBase.CustomControls
{
    partial class FilesControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesControl));
            this.toolStrip_Files = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton_Upload = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItem_PhotoReport = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Docs = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Originals = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Preview = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator_2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Download = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Open = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_SetPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_6 = new System.Windows.Forms.ToolStripSeparator();
            this.fullname = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.treeListView_Files = new BrightIdeasSoftware.TreeListView();
            this.FileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FileStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FileSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FileCreate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.FullPath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Upload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Download = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_Files.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView_Files)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip_Files
            // 
            this.toolStrip_Files.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip_Files.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_Files.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip_Files.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator_1,
            this.toolStripDropDownButton_Upload,
            this.toolStripSeparator_2,
            this.toolStripButton_Download,
            this.toolStripSeparator_3,
            this.toolStripButton_Delete,
            this.toolStripSeparator_4,
            this.toolStripButton_Open,
            this.toolStripSeparator_5,
            this.toolStripButton_SetPreview,
            this.toolStripSeparator_6});
            this.toolStrip_Files.Location = new System.Drawing.Point(0, 311);
            this.toolStrip_Files.Name = "toolStrip_Files";
            this.toolStrip_Files.Size = new System.Drawing.Size(550, 25);
            this.toolStrip_Files.TabIndex = 12;
            this.toolStrip_Files.Text = "toolStrip2";
            this.toolStrip_Files.Visible = false;
            // 
            // toolStripSeparator_1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator_1";
            this.toolStripSeparator_1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton_Upload
            // 
            this.toolStripDropDownButton_Upload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton_Upload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_PhotoReport,
            this.ToolStripMenuItem_Docs,
            this.ToolStripMenuItem_Originals,
            this.ToolStripMenuItem_Preview});
            this.toolStripDropDownButton_Upload.Enabled = false;
            this.toolStripDropDownButton_Upload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_Upload.Image")));
            this.toolStripDropDownButton_Upload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton_Upload.Name = "toolStripDropDownButton_Upload";
            this.toolStripDropDownButton_Upload.Size = new System.Drawing.Size(72, 22);
            this.toolStripDropDownButton_Upload.Text = "Добавить";
            // 
            // ToolStripMenuItem_PhotoReport
            // 
            this.ToolStripMenuItem_PhotoReport.Name = "ToolStripMenuItem_PhotoReport";
            this.ToolStripMenuItem_PhotoReport.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItem_PhotoReport.Text = "Фотоотчет";
            // 
            // ToolStripMenuItem_Docs
            // 
            this.ToolStripMenuItem_Docs.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripMenuItem_Docs.Image")));
            this.ToolStripMenuItem_Docs.Name = "ToolStripMenuItem_Docs";
            this.ToolStripMenuItem_Docs.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItem_Docs.Text = "Документация";
            // 
            // ToolStripMenuItem_Originals
            // 
            this.ToolStripMenuItem_Originals.Name = "ToolStripMenuItem_Originals";
            this.ToolStripMenuItem_Originals.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItem_Originals.Text = "Оригинал-макеты";
            // 
            // ToolStripMenuItem_Preview
            // 
            this.ToolStripMenuItem_Preview.Name = "ToolStripMenuItem_Preview";
            this.ToolStripMenuItem_Preview.Size = new System.Drawing.Size(175, 22);
            this.ToolStripMenuItem_Preview.Text = "Предпросмотр";
            // 
            // toolStripSeparator_2
            // 
            this.toolStripSeparator_2.Name = "toolStripSeparator_2";
            this.toolStripSeparator_2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Download
            // 
            this.toolStripButton_Download.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Download.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Download.Image")));
            this.toolStripButton_Download.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Download.Name = "toolStripButton_Download";
            this.toolStripButton_Download.Size = new System.Drawing.Size(55, 22);
            this.toolStripButton_Download.Text = "Скачать";
            this.toolStripButton_Download.Click += new System.EventHandler(this.SaveFiles_Click);
            // 
            // toolStripSeparator_3
            // 
            this.toolStripSeparator_3.Name = "toolStripSeparator_3";
            this.toolStripSeparator_3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Delete
            // 
            this.toolStripButton_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Delete.Enabled = false;
            this.toolStripButton_Delete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Delete.Image")));
            this.toolStripButton_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Delete.Name = "toolStripButton_Delete";
            this.toolStripButton_Delete.Size = new System.Drawing.Size(55, 22);
            this.toolStripButton_Delete.Text = "Удалить";
            // 
            // toolStripSeparator_4
            // 
            this.toolStripSeparator_4.Name = "toolStripSeparator_4";
            this.toolStripSeparator_4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Open
            // 
            this.toolStripButton_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Open.Enabled = false;
            this.toolStripButton_Open.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Open.Image")));
            this.toolStripButton_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Open.Name = "toolStripButton_Open";
            this.toolStripButton_Open.Size = new System.Drawing.Size(58, 22);
            this.toolStripButton_Open.Text = "Открыть";
            // 
            // toolStripSeparator_5
            // 
            this.toolStripSeparator_5.Name = "toolStripSeparator_5";
            this.toolStripSeparator_5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_SetPreview
            // 
            this.toolStripButton_SetPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_SetPreview.Enabled = false;
            this.toolStripButton_SetPreview.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SetPreview.Image")));
            this.toolStripButton_SetPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SetPreview.Name = "toolStripButton_SetPreview";
            this.toolStripButton_SetPreview.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_SetPreview.Text = "Preview";
            // 
            // toolStripSeparator_6
            // 
            this.toolStripSeparator_6.Name = "toolStripSeparator_6";
            this.toolStripSeparator_6.Size = new System.Drawing.Size(6, 25);
            // 
            // fullname
            // 
            this.fullname.IsVisible = false;
            // 
            // treeListView_Files
            // 
            this.treeListView_Files.AllColumns.Add(this.FileName);
            this.treeListView_Files.AllColumns.Add(this.FileStatus);
            this.treeListView_Files.AllColumns.Add(this.FileSize);
            this.treeListView_Files.AllColumns.Add(this.FileCreate);
            this.treeListView_Files.AllColumns.Add(this.FullPath);
            this.treeListView_Files.AllowDrop = true;
            this.treeListView_Files.BackColor = System.Drawing.SystemColors.Window;
            this.treeListView_Files.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeListView_Files.CellEditUseWholeCell = false;
            this.treeListView_Files.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.FileStatus,
            this.FileSize,
            this.FileCreate});
            this.treeListView_Files.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListView_Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView_Files.FullRowSelect = true;
            this.treeListView_Files.IsSimpleDropSink = true;
            this.treeListView_Files.LabelWrap = false;
            this.treeListView_Files.Location = new System.Drawing.Point(0, 0);
            this.treeListView_Files.Name = "treeListView_Files";
            this.treeListView_Files.SelectedColumnTint = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.treeListView_Files.ShowGroups = false;
            this.treeListView_Files.Size = new System.Drawing.Size(550, 336);
            this.treeListView_Files.TabIndex = 13;
            this.treeListView_Files.UseCompatibleStateImageBehavior = false;
            this.treeListView_Files.UseWaitCursorWhenExpanding = false;
            this.treeListView_Files.View = System.Windows.Forms.View.Details;
            this.treeListView_Files.VirtualMode = true;
            this.treeListView_Files.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.treeListView_Files_CanDrop);
            this.treeListView_Files.SelectedIndexChanged += new System.EventHandler(this.treeListView_Files_SelectedIndexChanged);
            this.treeListView_Files.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeListView_Files_DragDrop);
            this.treeListView_Files.DragOver += new System.Windows.Forms.DragEventHandler(this.treeListView_Files_DragOver);
            this.treeListView_Files.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeListView_Files_MouseDown);
            // 
            // FileName
            // 
            this.FileName.AspectName = "FileName";
            this.FileName.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
            this.FileName.DisplayIndex = 1;
            this.FileName.Groupable = false;
            this.FileName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileName.Text = "Имя файла";
            this.FileName.Width = 300;
            // 
            // FileStatus
            // 
            this.FileStatus.AspectName = "FileStatus";
            this.FileStatus.DisplayIndex = 0;
            this.FileStatus.HeaderForeColor = System.Drawing.Color.Black;
            this.FileStatus.Text = "Статус";
            this.FileStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileStatus.Width = 100;
            // 
            // FileSize
            // 
            this.FileSize.AspectName = "FileSize";
            this.FileSize.Groupable = false;
            this.FileSize.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileSize.Text = "Размер";
            this.FileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileSize.Width = 80;
            // 
            // FileCreate
            // 
            this.FileCreate.AspectName = "FileCreate";
            this.FileCreate.Groupable = false;
            this.FileCreate.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileCreate.Text = "Дата";
            this.FileCreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileCreate.Width = 80;
            // 
            // FullPath
            // 
            this.FullPath.AspectName = "FullPath";
            this.FullPath.DisplayIndex = 3;
            this.FullPath.IsVisible = false;
            this.FullPath.Text = "FullPath";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Получение списка файлов с сервера, ожидайте..";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Upload,
            this.ToolStripMenuItem_Download,
            this.toolStripSeparator1,
            this.ToolStripMenuItem_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 98);
            // 
            // ToolStripMenuItem_Upload
            // 
            this.ToolStripMenuItem_Upload.Image = global::AccentBase.Properties.Resources.Upload_16x16;
            this.ToolStripMenuItem_Upload.Name = "ToolStripMenuItem_Upload";
            this.ToolStripMenuItem_Upload.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItem_Upload.Text = "Добавить";
            this.ToolStripMenuItem_Upload.Click += new System.EventHandler(this.ToolStripMenuItem_Upload_Click);
            // 
            // ToolStripMenuItem_Download
            // 
            this.ToolStripMenuItem_Download.Image = global::AccentBase.Properties.Resources.Download_16x16;
            this.ToolStripMenuItem_Download.Name = "ToolStripMenuItem_Download";
            this.ToolStripMenuItem_Download.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItem_Download.Text = "Скачать";
            this.ToolStripMenuItem_Download.Click += new System.EventHandler(this.SaveFiles_Click);
            // 
            // ToolStripMenuItem_Delete
            // 
            this.ToolStripMenuItem_Delete.Image = global::AccentBase.Properties.Resources.trash_alt_full;
            this.ToolStripMenuItem_Delete.Name = "ToolStripMenuItem_Delete";
            this.ToolStripMenuItem_Delete.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItem_Delete.Text = "Удалить с сервера";
            this.ToolStripMenuItem_Delete.Click += new System.EventHandler(this.ToolStripMenuItem_Delete_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::AccentBase.Properties.Resources.iLoading;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(550, 336);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // FilesControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.treeListView_Files);
            this.Controls.Add(this.toolStrip_Files);
            this.Name = "FilesControl";
            this.Size = new System.Drawing.Size(550, 336);
            this.toolStrip_Files.ResumeLayout(false);
            this.toolStrip_Files.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListView_Files)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip_Files;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_Upload;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_PhotoReport;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Docs;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Originals;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Preview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_2;
        private System.Windows.Forms.ToolStripButton toolStripButton_Download;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_3;
        private System.Windows.Forms.ToolStripButton toolStripButton_Delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_4;
        private System.Windows.Forms.ToolStripButton toolStripButton_Open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_5;
        private System.Windows.Forms.ToolStripButton toolStripButton_SetPreview;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_6;
        private BrightIdeasSoftware.OLVColumn fullname;
        private BrightIdeasSoftware.TreeListView treeListView_Files;
        private BrightIdeasSoftware.OLVColumn FileName;
        private BrightIdeasSoftware.OLVColumn FileSize;
        private BrightIdeasSoftware.OLVColumn FileCreate;
        private BrightIdeasSoftware.OLVColumn FullPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Download;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Upload;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Delete;
        private BrightIdeasSoftware.OLVColumn FileStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
