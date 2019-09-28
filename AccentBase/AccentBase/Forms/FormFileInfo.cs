using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccentBase.Forms
{
    public partial class FormFileInfo : Form
    {
        ProtoClasses.ProtoFtpSchedule.protoRow file;
        public FormFileInfo(ProtoClasses.ProtoFtpSchedule.protoRow pr)
        {
            InitializeComponent();
            file = pr;
        }

        private void FormFileInfo_Load(object sender, EventArgs e)
        {
            this.Text = "Информация о файле: " + file.fileshortname;
            if (file.Upload)
            {
                label1.Text = "Источник: " + file.sourcefile;
                label2.Text = "Назначение: " + file.targetfile.Replace(@"makets/", @"SERVER:\\");
            }
            else
            {
                label1.Text = "Источник: " + file.sourcefile.Replace(@"makets/", @"SERVER:\\");
                label2.Text = "Назначение: " + file.targetfile;

            }
            label3.Text = "Размер: " + file.LengthString;
            label4.Text = "Дата создания: " + Utils.UnixDate.Int64ToDateTime(file.LastCreationTime).ToString();
            label5.Text = "Дата модификации: " + Utils.UnixDate.Int64ToDateTime(file.LastWriteTime).ToString();
        }
    }
}
