using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using System.Windows.;
namespace AccentBase.CustomControls.CustomPreviewFolder
{
    public partial class SelectImageFromArray : Form
    {
        private int LocationX;
        private int LocationY;
        //string filename = string.Empty;
        //bool saving = false;
        bool blockPreview = false;
        internal Image SelectImage { get; set; }
        private Dictionary<string, Image> previews = new Dictionary<string, Image>();
        //private HashSet<FileInfo> filenames = new HashSet<FileInfo>();

        public SelectImageFromArray(int x, int y, string[] files)
        {
            InitializeComponent();
            LocationX = x;
            LocationY = y;
            SelectImage = null;
            foreach (string str in files)
            {
                FileInfo fi = new FileInfo(str);
                ListViewItem itm = new ListViewItem();
                itm.Text = fi.Name;
                itm.ToolTipText = fi.FullName;
                listView1.Items.Add(itm);
            }
        }

        private void SelectImageFromArray_Load(object sender, EventArgs e)
        {
            this.SetDesktopLocation(LocationX, LocationY);
        }

        string SelectedFile = string.Empty;
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (blockPreview || listView1.SelectedItems.Count == 0)
            {
                SelectedFile = string.Empty;
                ToolStripMenuItem_Preview.Enabled = false;

            }
            else
            {
                SelectedFile = listView1.SelectedItems[0].ToolTipText;
                ToolStripMenuItem_Preview.Enabled = true;
            }
            if (previews.ContainsKey(listView1.SelectedItems[0].ToolTipText) && !blockPreview)
            {
                ToolStripMenuItem_Save.Enabled = true;
            }
            else
            {
                ToolStripMenuItem_Save.Enabled = false;
            }
        }

        private void SetPreview(string fname)
        {
            previewToDictionary(fname, Properties.Resources.iLoading);
            ShowPreview();
            blockPreview = true;
            Utils.CorelDrawExporter cde = new Utils.CorelDrawExporter();
            cde.CorelDrawExporterUpdateEvent += Cde_CorelDrawExporterUpdateEvent;
            cde.ExportToPng(fname);
        }
        #region Событие - получена картинка. установка изображения
        private delegate void UpdatePictureDelegate(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e);
        private void Cde_CorelDrawExporterUpdateEvent(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e)
        {
            this.BeginInvoke(new UpdatePictureDelegate(UpdateImage), sender, e);
        }
        private void UpdateImage(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e)
        {
            ////textBox1.Visible = textBox2.Visible = false;

            if (e.ex != null || e.preview == null)
            {
                MessageBox.Show("Конвертация в изображение невозможна." + Environment.NewLine + "Если это макет для производства, то он 100% подготовлен не верно.", "Ошибка конвертации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (previews.ContainsKey(e.fileName.FullName))
                {
                    previews.Remove(e.fileName.FullName);
                }
                    //previewToDictionary(e.fileName.FullName, Properties.Resources.New_256x256);
                    //if (previews.ContainsKey(e.fileName.FullName))
                    //{
                    //    previews[e.fileName.FullName].Dispose();
                    //    previews[e.fileName.FullName] = e.preview;
                    //}
                    //else
                    //{
                    //    previews.Add(e.fileName.FullName, e.preview);
                    //}
                    //if (this.listView1.SelectedItems.Count > 0)
                    //{
                    //    if (listView1.SelectedItems[0].ToolTipText.Equals(e.fileName.FullName))
                    //    {
                    //        pictureBox1.Image = e.preview;
                    //    }
                    //}
                    //if (previews.TryGetValue(e.fileName.FullName, out img))
                    //{
                    //    pictureBox1.Image = img;
                    //}
                    //else
                    //{
                    //    ToolStripMenuItem_Preview_Click(sender, e);
                    //}
                    //    pictureBox1.Image = Properties.Resources.New_256x256;
                    //    this.BackColor = pictureBox1.BackColor = System.Drawing.SystemColors.Control;
                    //    MessageBox.Show("Конвертация в изображение невозможна." + Environment.NewLine + "Если это макет для производства, то он 100% подготовлен не верно.", "Ошибка конвертации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {

                previewToDictionary(e.fileName.FullName, e.preview);

                //    if (preview != null) { preview.Dispose(); }
                //    preview = e.preview;
                //    pictureBox1.Image = preview;
                //    this.BackColor = pictureBox1.BackColor = Color.White;
            }
            Utils.CorelDrawExporter cde = sender as Utils.CorelDrawExporter;
            cde.CorelDrawExporterUpdateEvent -= Cde_CorelDrawExporterUpdateEvent;
            blockPreview = false;
            ShowPreview();
        }
        #endregion


        private void ToolStripMenuItem_Preview_Click(object sender, EventArgs e)
        {
            if (SelectedFile.Equals(string.Empty)) { return; }
            //blockPreview = true;
            SetPreview(SelectedFile);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowPreview();
        }

        private void previewToDictionary(string iKey, Image iImage)
        {
            if (previews.ContainsKey(iKey))
            {
                Image img = previews[iKey];
                img.Dispose();
                //previews[iKey] = new Bitmap(iImage);
                previews[iKey] = iImage;
            }
            else
            {
                previews.Add(iKey, iImage);
            }
        }

        private void ShowPreview()
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                Image imgtmp;
                if (previews.TryGetValue(listView1.SelectedItems[0].ToolTipText, out imgtmp))
                {
                    pictureBox1.Image = imgtmp;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.New_256x256;
                }
            }
            //if (this.listView1.SelectedItems.Count > 0)
            //{
            //    if (listView1.SelectedItems[0].ToolTipText.Equals(e.fileName.FullName))
            //    {
            //        pictureBox1.Image = e.preview;
            //    }
            //}
        }

        private void ToolStripMenuItem_Save_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                Image imgtmp;
                if (previews.TryGetValue(listView1.SelectedItems[0].ToolTipText, out imgtmp))
                {
                    SelectImage = new Bitmap(imgtmp);
                }else
                {
                    SelectImage = null;
                }
                imgtmp.Dispose();
                Close();
            }
        }
    }
}
