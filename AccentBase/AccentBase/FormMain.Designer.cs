namespace AccentBase
{
    partial class FormMain
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Context_Orders = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Messenger = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Stock = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_Ftp = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_SocketSend = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_GoogleTables = new System.Windows.Forms.ToolStripMenuItem();
            this.Context_OldStock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Context_About = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "База данных компании \"Акцент\"";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.NotifyIcon1_BalloonTip_Clicked);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Orders,
            this.Context_Messenger,
            this.Context_Stock,
            this.Context_Ftp,
            this.Context_SocketSend,
            this.Context_GoogleTables,
            this.Context_OldStock,
            this.toolStripSeparator1,
            this.настройкиToolStripMenuItem,
            this.exitToolStripMenuItem,
            this.toolStripSeparator4,
            this.Context_About});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(242, 338);
            // 
            // Context_Orders
            // 
            this.Context_Orders.CheckOnClick = true;
            this.Context_Orders.Enabled = false;
            this.Context_Orders.Image = global::AccentBase.Properties.Resources.Text_Document_24x24;
            this.Context_Orders.Name = "Context_Orders";
            this.Context_Orders.Size = new System.Drawing.Size(241, 30);
            this.Context_Orders.Text = "Заявки";
            this.Context_Orders.Click += new System.EventHandler(this.ShowOrders_Click);
            // 
            // Context_Messenger
            // 
            this.Context_Messenger.CheckOnClick = true;
            this.Context_Messenger.Enabled = false;
            this.Context_Messenger.Image = global::AccentBase.Properties.Resources.Send_24x24;
            this.Context_Messenger.Name = "Context_Messenger";
            this.Context_Messenger.Size = new System.Drawing.Size(241, 30);
            this.Context_Messenger.Text = "Мессенджер";
            this.Context_Messenger.Click += new System.EventHandler(this.Context_Messenger_Click);
            // 
            // Context_Stock
            // 
            this.Context_Stock.CheckOnClick = true;
            this.Context_Stock.Enabled = false;
            this.Context_Stock.Image = global::AccentBase.Properties.Resources.Archive_24x24;
            this.Context_Stock.Name = "Context_Stock";
            this.Context_Stock.Size = new System.Drawing.Size(241, 30);
            this.Context_Stock.Text = "Склад";
            this.Context_Stock.Click += new System.EventHandler(this.ShowStock_Click);
            // 
            // Context_Ftp
            // 
            this.Context_Ftp.CheckOnClick = true;
            this.Context_Ftp.Enabled = false;
            this.Context_Ftp.Image = global::AccentBase.Properties.Resources.Synchronize_24x24;
            this.Context_Ftp.Name = "Context_Ftp";
            this.Context_Ftp.Size = new System.Drawing.Size(241, 30);
            this.Context_Ftp.Text = "Передача файлов";
            this.Context_Ftp.Click += new System.EventHandler(this.Context_Ftp_Click);
            // 
            // Context_SocketSend
            // 
            this.Context_SocketSend.Enabled = false;
            this.Context_SocketSend.Image = global::AccentBase.Properties.Resources.N009;
            this.Context_SocketSend.Name = "Context_SocketSend";
            this.Context_SocketSend.Size = new System.Drawing.Size(241, 30);
            this.Context_SocketSend.Text = "Список отправки изменений";
            this.Context_SocketSend.Click += new System.EventHandler(this.Context_SocketSend_Click);
            // 
            // Context_GoogleTables
            // 
            this.Context_GoogleTables.CheckOnClick = true;
            this.Context_GoogleTables.Image = global::AccentBase.Properties.Resources.googleimg;
            this.Context_GoogleTables.Name = "Context_GoogleTables";
            this.Context_GoogleTables.Size = new System.Drawing.Size(241, 30);
            this.Context_GoogleTables.Text = "Google таблицы - склад";
            this.Context_GoogleTables.Click += new System.EventHandler(this.contextGoogleTablesToolStripMenuItem_Click);
            // 
            // Context_OldStock
            // 
            this.Context_OldStock.Enabled = false;
            this.Context_OldStock.Name = "Context_OldStock";
            this.Context_OldStock.Size = new System.Drawing.Size(241, 30);
            this.Context_OldStock.Text = "Склад старый";
            this.Context_OldStock.Visible = false;
            this.Context_OldStock.Click += new System.EventHandler(this.ShowOldStock_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(238, 6);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Image = global::AccentBase.Properties.Resources.Settings_24x241;
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(241, 30);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.SettingsShow);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::AccentBase.Properties.Resources.Log_Out_24x24;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(241, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ApplicationExit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(238, 6);
            // 
            // Context_About
            // 
            this.Context_About.Enabled = false;
            this.Context_About.Image = global::AccentBase.Properties.Resources.Information_24x24;
            this.Context_About.Name = "Context_About";
            this.Context_About.Size = new System.Drawing.Size(241, 30);
            this.Context_About.Text = "О программе";
            this.Context_About.Click += new System.EventHandler(this.Context_About_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(179, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "База данных компании \"Акцент\"";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 321);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ver.:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 296);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(680, 16);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(617, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Настройки";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SettingsShow);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 36);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(680, 251);
            this.listBox1.TabIndex = 7;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AccentBase.Properties.Resources.back2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(704, 350);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccentBase";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem Context_Orders;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Context_Stock;
        private System.Windows.Forms.ToolStripMenuItem Context_OldStock;
        private System.Windows.Forms.ToolStripMenuItem Context_About;
        private System.Windows.Forms.ToolStripMenuItem Context_Messenger;
        private System.Windows.Forms.ToolStripMenuItem Context_Ftp;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem Context_GoogleTables;
        private System.Windows.Forms.ToolStripMenuItem Context_SocketSend;
        private System.Windows.Forms.Timer timer1;
    }
}

