using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace AccentBase.CustomControls.CustomPreviewFolder
{


    public partial class CustomPreview : UserControl
    {

        private Image preview = null; //Само изображение
        //private Image Oldpreview = null; //Исходное изображение
        private bool blockPreview = false; // Блокировка установки нового превью
        private bool noImage = true;
        #region Services
        public Image Preview
        {
            get { if (noImage) { return null; } else { return preview; } }
            set
            {
                if (value != null)
                {
                    try
                    {
                        preview = value;
                        //this.BackColor = this.pictureBox1.BackColor = Color.White;
                    }
                    catch { preview = Properties.Resources.New_256x256; }// this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247); }
                }
                else { preview = Properties.Resources.New_256x256; }// this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247); }
                //if (preview != Properties.Resources.New_256x256) { Oldpreview = preview; }
                pictureBox1.Image = preview;
            }
        }
        public byte[] PreviewBinary
        {
            get
            {
                if (noImage) { return null; }
                else
                {
                    if (preview != null)
                    {
                        return Utils.Converting.ImageToByte(preview);
                    }
                    else { return null; }
                }
            }
            set
            {
                preview = Utils.Converting.ByteToImage(value);
                //this.BackColor = pictureBox1.BackColor = Color.White;
                if (preview == null) { preview = Properties.Resources.New_256x256; }// this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247); }
            }
        }
        public bool BlockPreview
        {
            get { return blockPreview; }
            set { blockPreview = value; }
        }

        #endregion
        public CustomPreview()
        {
            InitializeComponent();
        }

        #region Drag&Drop
        private void CustomPreview_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                switch (files.Length)
                {
                    //case 0:
                    //    break;
                    case 1:
                        if (System.IO.File.Exists(files[0]))
                        {
                            SetPreview(files[0]);
                        }
                        break;
                    default:
                        using (SelectImageFromArray frm = new SelectImageFromArray(Cursor.Position.X, Cursor.Position.Y, files))
                        {
                            frm.ShowInTaskbar = true;
                            frm.ShowDialog(this);
                            if (frm.SelectImage != null)
                            {
                               Preview = new Bitmap(frm.SelectImage);
                                frm.SelectImage.Dispose();
                            }
                        }
                        break;
                }
            }
        }
        private void CustomPreview_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) { e.Effect = DragDropEffects.All; } else { e.Effect = DragDropEffects.None; }
        }


        #endregion

        #region OpenFileDialog
        public void SelectImageFromFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.bmp; *.tif; *.tiff; *.cdr; *.cmx; *.pdf; *.eps |Все файлы (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (System.IO.File.Exists(ofd.FileName))
                    {
                        SetPreview(ofd.FileName);
                    }
                }
            }
        }

        #endregion

        #region Событие - получена картинка. установка изображения
        private delegate void UpdatePictureDelegate(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e);
        private void Cde_CorelDrawExporterUpdateEvent(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e)
        {
            try
            {
                this.BeginInvoke(new UpdatePictureDelegate(UpdateImage), sender, e);
            }
            catch { }
        }
        public Image Image
        {
            get { return pictureBox1.Image; }
            set { if (value!= null) { pictureBox1.Image = value; }else { pictureBox1.Image = Properties.Resources.New_256x256; } }
        }
        public delegate void UpdatePreviewDelegate(object sender, bool e);
        public event UpdatePreviewDelegate NewPreview;
        public void UpdatePreview(bool e) { NewPreview?.Invoke(null, e); }

        //public bool NewPreviewExist{ get; set; }

        private void UpdateImage(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e)
        {
            textBox1.Visible = textBox2.Visible = false;

            if (e.ex != null || e.preview == null)
            {
                pictureBox1.Image = Properties.Resources.New_256x256;
                //this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247);
                MessageBox.Show("Конвертация в изображение невозможна." + Environment.NewLine + "Если это макет для производства, то он 100% подготовлен не верно.", "Ошибка конвертации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdatePreview(false);
            }
            else
            {
                if (preview != null) { preview.Dispose(); }
                preview = e.preview;
                pictureBox1.Image = preview;
                UpdatePreview(true);
                //NewPreviewExist = true;
                //this.BackColor = pictureBox1.BackColor = Color.White;
            }
            Utils.CorelDrawExporter cde = sender as Utils.CorelDrawExporter;
            cde.CorelDrawExporterUpdateEvent -= Cde_CorelDrawExporterUpdateEvent;
            blockPreview = false;
        }
        #endregion
        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectImageFromFile();
        }

        private void CustomPreview_DragOver(object sender, DragEventArgs e)
        {
            if (blockPreview)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Copy;
            }
        }


        private void SetPreview(string fname)
        {
            blockPreview = true;
            noImage = false;
            pictureBox1.Image = Properties.Resources.iLoading;
            //this.BackColor = pictureBox1.BackColor = Color.White;
            textBox1.Visible = textBox2.Visible = true;
            Utils.CorelDrawExporter cde = new Utils.CorelDrawExporter();
            cde.CorelDrawExporterUpdateEvent += Cde_CorelDrawExporterUpdateEvent;
            cde.ExportToPng(fname);
        }

        private void CustomPreview_Load(object sender, EventArgs e)
        {
            preview = Properties.Resources.New_256x256; //this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247);
        }

        private void contextMenu_Preview_Opening(object sender, CancelEventArgs e)
        {
            if (blockPreview) { Button_Open.Enabled = Button_Save.Enabled = Button_Delete.Enabled = false; }
            else { Button_Open.Enabled = Button_Save.Enabled = Button_Delete.Enabled = true; }
            if (noImage) { Button_Save.Enabled = Button_Delete.Enabled = false; }
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            if (!noImage)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить изображение";
                sfd.Filter = "Portable Network Graphics|*.png|Joint Photographic Experts Group|*.jpeg|Bitmap|*.bmp|Graphics Interchange Format|*.gif";
                sfd.ShowDialog();
                if (!sfd.FileName.Equals(string.Empty))
                {
                    blockPreview = true;
                    using (FileStream fs = (FileStream)sfd.OpenFile())
                    {
                        switch (sfd.FilterIndex)
                        {
                            case 1:
                                preview.Save(fs, ImageFormat.Png);
                                break;
                            case 2:
                                preview.Save(fs, ImageFormat.Jpeg);
                                break;
                            case 3:
                                preview.Save(fs, ImageFormat.Bmp);
                                break;
                            case 4:
                                preview.Save(fs, ImageFormat.Gif);
                                break;
                        }
                    }
                    blockPreview = false;
                }
            }
        }


        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (!noImage)
            {
                preview = Properties.Resources.New_256x256;// this.BackColor = pictureBox1.BackColor = Color.FromArgb(247, 247, 247);
                noImage = true;
            }
        }
    }
}
