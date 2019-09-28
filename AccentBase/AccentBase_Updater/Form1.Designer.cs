namespace AccentBase_Updater
{
    partial class Form_Main
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.UnitsProgress = new System.Windows.Forms.Label();
            this.UnitsProgressBar = new System.Windows.Forms.ProgressBar();
            this.ServerCheck = new System.Windows.Forms.Button();
            this.ServerVersion = new System.Windows.Forms.TextBox();
            this.Update = new System.Windows.Forms.Button();
            this.LocalVersion = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button_LocalPath = new System.Windows.Forms.Button();
            this.LocalPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Save = new System.Windows.Forms.Button();
            this.FtpData = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.FtpProgram = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.FtpPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.FtpAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FtpLogin = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(378, 221);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.buttonDelete);
            this.tabPage2.Controls.Add(this.UnitsProgress);
            this.tabPage2.Controls.Add(this.UnitsProgressBar);
            this.tabPage2.Controls.Add(this.ServerCheck);
            this.tabPage2.Controls.Add(this.ServerVersion);
            this.tabPage2.Controls.Add(this.Update);
            this.tabPage2.Controls.Add(this.LocalVersion);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(370, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Настройки установки";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(109, 169);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(136, 23);
            this.buttonDelete.TabIndex = 35;
            this.buttonDelete.Text = "Удалить базу";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.button1_Click);
            // 
            // UnitsProgress
            // 
            this.UnitsProgress.AutoSize = true;
            this.UnitsProgress.Location = new System.Drawing.Point(6, 86);
            this.UnitsProgress.Name = "UnitsProgress";
            this.UnitsProgress.Size = new System.Drawing.Size(213, 13);
            this.UnitsProgress.TabIndex = 34;
            this.UnitsProgress.Text = "Проверка последней версии на сервере";
            // 
            // UnitsProgressBar
            // 
            this.UnitsProgressBar.Location = new System.Drawing.Point(6, 102);
            this.UnitsProgressBar.Name = "UnitsProgressBar";
            this.UnitsProgressBar.Size = new System.Drawing.Size(358, 13);
            this.UnitsProgressBar.TabIndex = 33;
            // 
            // ServerCheck
            // 
            this.ServerCheck.Location = new System.Drawing.Point(285, 20);
            this.ServerCheck.Name = "ServerCheck";
            this.ServerCheck.Size = new System.Drawing.Size(77, 23);
            this.ServerCheck.TabIndex = 23;
            this.ServerCheck.Text = "Проверить";
            this.ServerCheck.UseVisualStyleBackColor = true;
            this.ServerCheck.Click += new System.EventHandler(this.button6_Click);
            // 
            // ServerVersion
            // 
            this.ServerVersion.Location = new System.Drawing.Point(120, 23);
            this.ServerVersion.Name = "ServerVersion";
            this.ServerVersion.ReadOnly = true;
            this.ServerVersion.Size = new System.Drawing.Size(159, 20);
            this.ServerVersion.TabIndex = 22;
            this.ServerVersion.Text = "Нет подключения";
            // 
            // Update
            // 
            this.Update.Enabled = false;
            this.Update.Location = new System.Drawing.Point(285, 49);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(77, 23);
            this.Update.TabIndex = 21;
            this.Update.Text = "Обновить";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // LocalVersion
            // 
            this.LocalVersion.Location = new System.Drawing.Point(120, 51);
            this.LocalVersion.Name = "LocalVersion";
            this.LocalVersion.ReadOnly = true;
            this.LocalVersion.Size = new System.Drawing.Size(159, 20);
            this.LocalVersion.TabIndex = 20;
            this.LocalVersion.Text = "Не установлено!";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Версия на сервере:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Текущая версия:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button_LocalPath);
            this.tabPage3.Controls.Add(this.LocalPath);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.button_Save);
            this.tabPage3.Controls.Add(this.FtpData);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.FtpProgram);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.FtpPass);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.FtpAddress);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.FtpLogin);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(370, 195);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Настройки подключения";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button_LocalPath
            // 
            this.button_LocalPath.Location = new System.Drawing.Point(285, 6);
            this.button_LocalPath.Name = "button_LocalPath";
            this.button_LocalPath.Size = new System.Drawing.Size(77, 23);
            this.button_LocalPath.TabIndex = 21;
            this.button_LocalPath.Text = "Выбрать";
            this.button_LocalPath.UseVisualStyleBackColor = true;
            this.button_LocalPath.Click += new System.EventHandler(this.SelectLocalPath_Click);
            // 
            // LocalPath
            // 
            this.LocalPath.Location = new System.Drawing.Point(117, 8);
            this.LocalPath.Name = "LocalPath";
            this.LocalPath.Size = new System.Drawing.Size(162, 20);
            this.LocalPath.TabIndex = 20;
            this.LocalPath.Text = "C:\\Oabase";
            this.LocalPath.TextChanged += new System.EventHandler(this.LocalPath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Каталог установки:";
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(109, 169);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(136, 23);
            this.button_Save.TabIndex = 18;
            this.button_Save.Text = "Сохранить настройки";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // FtpData
            // 
            this.FtpData.Location = new System.Drawing.Point(114, 139);
            this.FtpData.Name = "FtpData";
            this.FtpData.Size = new System.Drawing.Size(250, 20);
            this.FtpData.TabIndex = 9;
            this.FtpData.Text = "Update\\Data";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Папка данных:";
            // 
            // FtpProgram
            // 
            this.FtpProgram.Location = new System.Drawing.Point(114, 113);
            this.FtpProgram.Name = "FtpProgram";
            this.FtpProgram.Size = new System.Drawing.Size(250, 20);
            this.FtpProgram.TabIndex = 7;
            this.FtpProgram.Text = "Update\\Program";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Папка обновления:";
            // 
            // FtpPass
            // 
            this.FtpPass.Location = new System.Drawing.Point(114, 87);
            this.FtpPass.Name = "FtpPass";
            this.FtpPass.Size = new System.Drawing.Size(250, 20);
            this.FtpPass.TabIndex = 5;
            this.FtpPass.Text = "";
            this.FtpPass.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Пароль FTP:";
            // 
            // FtpAddress
            // 
            this.FtpAddress.Location = new System.Drawing.Point(114, 35);
            this.FtpAddress.Name = "FtpAddress";
            this.FtpAddress.Size = new System.Drawing.Size(250, 20);
            this.FtpAddress.TabIndex = 1;
            this.FtpAddress.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Логин для FTP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Адрес FTP:";
            // 
            // FtpLogin
            // 
            this.FtpLogin.Location = new System.Drawing.Point(114, 61);
            this.FtpLogin.Name = "FtpLogin";
            this.FtpLogin.Size = new System.Drawing.Size(250, 20);
            this.FtpLogin.TabIndex = 3;
            this.FtpLogin.Text = "";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged_1);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.WorkerReportsProgress = true;
            this.backgroundWorker3.WorkerSupportsCancellation = true;
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker3_ProgressChanged);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 36;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox1.Location = new System.Drawing.Point(6, 121);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(358, 42);
            this.textBox1.TabIndex = 37;
            this.textBox1.Text = "Внимание, для корректной работы, базе требуется установленный CorelDraw X13!";
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 221);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccentBaseUpdater";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox FtpAddress;
        private System.Windows.Forms.TextBox FtpPass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox FtpLogin;
        private System.Windows.Forms.TextBox FtpData;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox FtpProgram;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button ServerCheck;
        private System.Windows.Forms.TextBox ServerVersion;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.TextBox LocalVersion;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label UnitsProgress;
        private System.Windows.Forms.ProgressBar UnitsProgressBar;
        private System.Windows.Forms.Button button_LocalPath;
        private System.Windows.Forms.TextBox LocalPath;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button buttonDelete;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
    }
}

