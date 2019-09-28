using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
namespace AccentBase.Forms
{
    public partial class FormUpDownload : Form
    {
        public FormUpDownload()
        {
            InitializeComponent();
        }

        internal delegate void FTPClientDelegate(bool e);
        private void FTPClient_Stopped(object sender, bool e)
        {
            if (InvokeRequired) { Invoke(new FTPClientDelegate(FTPClient_StoppedHandler), e); } else { FTPClient_StoppedHandler(e); }
        }
        private void FTPClient_StoppedHandler(bool e)
        {
            if (e)
            {
                StartStopFtp.Image = Properties.Resources.Play_24x24;
                StartStopFtp.Text = "Продолжить передачу файлов";
            }
            else
            {
                StartStopFtp.Image = Properties.Resources.Stop_24x24;
                StartStopFtp.Text = "Остановить передачу файлов";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Add(numericUpDown1.Value, "","", numericUpDown2.Value);
            //DataRow dr = www.NewRow();
            //dr["qid"] = Convert.ToInt64(numericUpDown1.Value);
            //dr["from"] = string.Empty;
            //dr["to"] = string.Empty;
            //dr["progress"] = Convert.ToDouble(numericUpDown2.Value);
            //www.Rows.Add(dr);
        }

        private DataTable www = new DataTable();

        private void FormUpDownload_Load(object sender, EventArgs e)
        {
            Ftp.FTPClient.Stopped += FTPClient_Stopped;

            if (Ftp.FTPClient.working)
            {
                //Ftp.FTPClient.Stop();
                StartStopFtp.Image = Properties.Resources.Stop_24x24;
                StartStopFtp.Text = "Остановить передачу файлов";


            }
            else
            {
                //Ftp.FTPClient.Start();
                StartStopFtp.Image = Properties.Resources.Play_24x24;
                StartStopFtp.Text = "Продолжить передачу файлов";
            }


            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.RowTemplate.Height = 50;
            databaseConnect();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            databaseConnect();
        }
        bool Event_FTPOperationNotSubs = true;
        private void databaseConnect()
        {
            try
            {
                timer1.Enabled = false;
                www = SqlLite.FtpSchedule.FTPList.Copy();
                dataGridView1.DataSource = www;
                if (Event_FTPOperationNotSubs)
                {
                    SqlLite.FtpSchedule.Event_FTPOperation += FtpSchedule_Event_FTPOperation;
                    Event_FTPOperationNotSubs = false;
                }

            }
            catch
            {
                timer1.Enabled = true;
            }

        }

        public delegate void Delegate_FTPOperation(Ftp.FtpReciever.FTPOperationEventArgs e);
        private void FtpSchedule_Event_FTPOperation(object sender, Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            try
            {
                if (e != null) { if (InvokeRequired) { Invoke(new Delegate_FTPOperation(SqliterHandler), e); } else { SqliterHandler(e); } }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Forms.FormUpDownload.FtpSchedule_Event_FTPOperation");
            }
        }
        private void SqliterHandler(Ftp.FtpReciever.FTPOperationEventArgs e)
        {
            if (e.Status == 100)
            {
                www = SqlLite.FtpSchedule.FTPList.Copy();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = www;
                //this.dataGridView1.Sort(this.dataGridView1.Columns["uid"], ListSortDirection.Ascending);
            }
            else
            {
                DataRow dr = www.Select("id =" + e.Id).SingleOrDefault();
                if (dr != null)
                {
                    if (e.Ex.Equals(string.Empty))
                    {
                        if (e.Ended)
                        {
                            //dr["ETA"] = e.ETA;
                            //dr["conspeed"] = e.TransferSpeed;
                            //dr["processprogress"] = 100;
                            www.Rows.Remove(dr);// dr.Delete();
                                                //dataGridView1.DataSource = null;

                            //dataGridView1.DataSource = www;
                        }
                        else
                        {
                            //if (Utils.CheckDBNull())
                            dr["ETA"] = e.ETA;
                            dr["conspeed"] = e.TransferSpeed;
                            dr["filestatus"] = e.Status;
                            dr["notes"] = string.Empty;
                            int i = Convert.ToInt32(e.ProcessCount);
                            if (i <= 100) { dr["processprogress"] = i; } else { dr["processprogress"] = 100; }

                        }
                    }
                    else
                    {
                        dr["notes"] = e.Ex;

                    }
                }

            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Всего: " + dataGridView1.Rows.Count;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Всего: " + dataGridView1.Rows.Count;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Ftp.FTPClient.Start();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Ftp.FTPClient.Stop();
        }

        //private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        //{
        //    //dataGridView1.DataSource = null;
        //    //dataGridView1.DataSource = SqlLite.FtpSchedule.FTPList;
        //}

        //private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{

        //    //this.BeginInvoke(new MethodInvoker(() =>
        //    //{
        //    //    if (e.ColumnIndex >= dataGridView1.Columns.Count - 3)
        //    //    {
        //    //        if (e.RowIndex < dataGridView1.Rows.Count - 1)
        //    //            dataGridView1.CurrentCell = dataGridView1[0, e.RowIndex + 1];
        //    //        else
        //    //            dataGridView1.CurrentCell = dataGridView1[dataGridView1.Columns.Count - 4, e.RowIndex];
        //    //    }
        //    //    else
        //    //        if (e.ColumnIndex > 0 && e.ColumnIndex % 2 == 0)
        //    //    {
        //    //        dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex + 1, e.RowIndex];
        //    //    }

        //    //}));
        //    //this.BeginInvoke(new MethodInvoker(() =>
        //    //{
        //    //    moveRowTo(dataGridView2, 0, 1);
        //    //}));

        //    //if (Controller.Control.distribution_series_correct(dataGridView1, error_txt))
        //    //{
        //    //    this.BeginInvoke(new MethodInvoker(() =>
        //    //    {
        //    //        Controller.Control.distribution_series_normalization(dataGridView1);
        //    //        Controller.Control.graph_plotting(graph, dataGridView1);
        //    //    }));
        //    //}
        //}

        private void StartStopFtp_Click(object sender, EventArgs e)
        {
            if (Ftp.FTPClient.working)
            {
                Ftp.FTPClient.Stop();
                StartStopFtp.Image = Properties.Resources.Play_24x24;
                StartStopFtp.Text = "Продолжить передачу файлов";

            }
            else
            {
                Ftp.FTPClient.Start();
                StartStopFtp.Image = Properties.Resources.Stop_24x24;
                StartStopFtp.Text = "Остановить передачу файлов";
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dataGridView1.HitTest(e.X, e.Y).RowIndex >= 0 && dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells["id"].Value != null && long.TryParse(dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells["id"].Value.ToString(), out long uid))
                {
                    DataRow dr = www.Select("id =" + uid).SingleOrDefault();
                    if (dr != null)
                    {
                        ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow
                        {
                            id = uid,
                            fileshortname = Utils.CheckDBNull.ToString(dr["fileshortname"]),
                            LengthString = Utils.CheckDBNull.ToString(dr["LengthString"]),
                            LastCreationTime = Utils.CheckDBNull.ToLong(dr["LastCreationTime"]),
                            LastWriteTime = Utils.CheckDBNull.ToLong(dr["LastWriteTime"]),
                            sourcefile = Utils.CheckDBNull.ToString(dr["sourcefile"]),
                            targetfile = Utils.CheckDBNull.ToString(dr["targetfile"])
                        };
                        FormFileInfo finf = new FormFileInfo(pr);

                        ContextFileName.Text = pr.fileshortname + ": Информация";
                        ContextFileName.Tag = pr;
                        contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                    }
                }
                //Int64 uid = 0;
                //if (dataGridView1.HitTest(e.X, e.Y).RowIndex>= 0 && dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells["id"].Value != null && Int64.TryParse(dataGridView1.Rows[dataGridView1.HitTest(e.X, e.Y).RowIndex].Cells["id"].Value.ToString(), out uid))
                //{
                //    DataRow dr = www.Select("id =" + uid).SingleOrDefault();
                //    if (dr != null)
                //    {
                //        ProtoClasses.ProtoFtpSchedule.protoRow pr = new ProtoClasses.ProtoFtpSchedule.protoRow();
                //        pr.id = uid;
                //        pr.fileshortname = Utils.CheckDBNull.ToString(dr["fileshortname"]);
                //        pr.LengthString = Utils.CheckDBNull.ToString(dr["LengthString"]);
                //        pr.LastCreationTime = Utils.CheckDBNull.ToLong(dr["LastCreationTime"]);
                //        pr.LastWriteTime = Utils.CheckDBNull.ToLong(dr["LastWriteTime"]);
                //        pr.sourcefile = Utils.CheckDBNull.ToString(dr["sourcefile"]);
                //        pr.targetfile = Utils.CheckDBNull.ToString(dr["targetfile"]);
                //        FormFileInfo finf = new FormFileInfo(pr);
                //        finf.ShowDialog();
                //    }
                //}

            }
        }

        private void ContextFileName_Click(object sender, EventArgs e)
        {
            ProtoClasses.ProtoFtpSchedule.protoRow pr = ContextFileName.Tag as ProtoClasses.ProtoFtpSchedule.protoRow;
            FormFileInfo finf = new FormFileInfo(pr);
            finf.ShowDialog();
        }

        private void FormUpDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ftp.FTPClient.Stopped -= FTPClient_Stopped;
            if (!Event_FTPOperationNotSubs)
            {
                SqlLite.FtpSchedule.Event_FTPOperation -= FtpSchedule_Event_FTPOperation;
            }
        }


    }
}
