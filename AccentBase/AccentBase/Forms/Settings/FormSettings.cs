using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace AccentBase.Forms.Settings
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //Utils.Settings.set = new ProtoClasses.ProtoSettings.protoSet();
            Utils.Settings.set.name = textBox1.Text;
            Utils.Settings.set.data_path = textBox3.Text;
            Utils.Settings.set.server_address = textBox2.Text;
            Utils.Settings.set.server_port = Convert.ToInt32(numericUpDown1.Value);
            Utils.Settings.set.buffer_size = Convert.ToInt32(numericUpDown2.Value);
            Exception ex =  Utils.Settings.Save();
            if (ex != null)
            {
               DialogResult dr = MessageBox.Show("Не удаётся сохранить настройки!" + Environment.NewLine +"Выйти из приложения?" + Environment.NewLine +"Подробная информация:"+ Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dr == DialogResult.Yes)
                {
                    Application.Exit();
                }else
                {
                    Close();
                }

            } else
            {
                SocketClient.TableClient.Connect();
                Close();
            }

        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            SocketClient.TableClient.StopConnecting();
            textBox3.Text = Application.StartupPath + @"\data";
            if (Utils.Settings.set == null)
            {
                Utils.Settings.set = new ProtoClasses.ProtoSettings.protoSet();
                Utils.Settings.set.buffer_size = 1024;
                Utils.Settings.set.data_path = Application.StartupPath + @"\data";
                Utils.Settings.set.name = "Пользователь";
                Utils.Settings.set.server_address = "192.168.1.28";
                Utils.Settings.set.server_port = 4900;
            }
            textBox1.Text = Utils.Settings.set.name;
            textBox2.Text = Utils.Settings.set.server_address;
            numericUpDown1.Value = Utils.Settings.set.server_port;
            numericUpDown2.Value = Utils.Settings.set.buffer_size;
            textBox3.Text = Utils.Settings.set.data_path;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            SocketClient.TableClient.Connect();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox3.Text = Application.StartupPath + @"\data";
            textBox3.Text = @"C:\Oabase";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
            {
                folderDlg.ShowNewFolderButton = true;
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK){textBox3.Text = folderDlg.SelectedPath;}
            }
        }
    }
}
