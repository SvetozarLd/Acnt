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
namespace AccentBase.Forms
{
    public partial class FormFileRename : Form
    {
        public string FileName { get; set; }
        public bool Overwrite { get; set; }
        public bool Cancelation { get; set; }
        private bool fileblockes = false;
        private bool visibleAllCheckbox = true;
        public FormFileRename(string filename, bool FileBlocked, bool VisibleAllCheckbox)
        {
            InitializeComponent();
            FileName = filename;
            Cancelation = false;
            fileblockes = FileBlocked;
            visibleAllCheckbox = VisibleAllCheckbox;
            if (FileBlocked) { toolStripStatusLabel1.Visible = true; } else { toolStripStatusLabel1.Visible = false; }
            checkBox1.Visible = visibleAllCheckbox;
        }

        private void FormFileRename_Load(object sender, EventArgs e)
        {
            textBox1.Text = FileName;
        }
        string str = string.Empty;
        int i = 0;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //var invalid = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()); foreach (char c in invalid) textBox1.Text = textBox1.Text.Replace(c.ToString(), "_");
            str = Utils.FileNamesValidation.GetValidPath(textBox1.Text);
            if (str != textBox1.Text)
            {
                i = textBox1.SelectionStart;
                if (i > 0) { textBox1.Text = str; textBox1.SelectionStart = i - 1; }
            }
            if (textBox1.Text == FileName)
            {
                if (fileblockes)
                {
                    button1.Enabled = false;
                    checkBox1.Enabled = false;
                    toolStripStatusLabel1.Visible = true;
                }
                else
                {
                    toolStripStatusLabel1.Visible = false;
                    button1.Enabled = true;
                    checkBox1.Enabled = true;
                    button1.Text = "Заменить";
                    checkBox1.Visible = visibleAllCheckbox;
                }
            } else { toolStripStatusLabel1.Visible = true; button1.Enabled = true; button1.Text = "Переименовать"; checkBox1.Visible = false; }
        }

 

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals(String.Empty))
            {
                MessageBox.Show("Имя файла не может быть пустым!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                FileName = textBox1.Text;
                if (button1.Text == "Заменить" && checkBox1.Visible == true && checkBox1.Checked)
                {
                    Overwrite = true;
                }
                else
                {
                    Overwrite = false;
                }
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cancelation = true;
            button1_Click(sender, e);
        }
    }
}
