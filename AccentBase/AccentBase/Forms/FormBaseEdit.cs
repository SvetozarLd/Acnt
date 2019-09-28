using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
namespace AccentBase.Forms
{
    public partial class FormBaseEdit : Form
    {
        #region Скрыть чекбоксы
        private const int TVIF_STATE = 0x8;
        private const int TVIS_STATEIMAGEMASK = 0xF000;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETITEM = TV_FIRST + 63;

        [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
        private struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);

        /// <summary>
        /// Hides the checkbox for the specified node on a TreeView control.
        /// </summary>
        private void HideCheckBox(TreeView tvw, TreeNode node)
        {
            TVITEM tvi = new TVITEM
            {
                hItem = node.Handle,
                mask = TVIF_STATE,
                stateMask = TVIS_STATEIMAGEMASK,
                state = 0
            };
            SendMessage(tvw.Handle, TVM_SETITEM, IntPtr.Zero, ref tvi);
        }
        #endregion

        //CustomControls.RichTextBoxEx CurrentDocument = null;
        //CustomControls.RichTextBoxEx CurrentDocumentTMP = null;
        #region General

        private int SelectedFont = 0;
        private void GetFontCollection()
        {
            InstalledFontCollection InsFonts = new InstalledFontCollection();

            foreach (FontFamily item in InsFonts.Families)
            {

                ComboBox_Font.Items.Add(item.Name);
            }
            ComboBox_Font.SelectedIndex = ComboBox_Font.Items.IndexOf("Arial");
            SelectedFont = ComboBox_Font.SelectedIndex;
        }

        private void PopulateFontSizes()
        {
            for (int i = 1; i <= 75; i++)
            {
                ComboBox_FontSize.Items.Add(i);
            }

            ComboBox_FontSize.SelectedIndex = 13;
        }
        #endregion

        #region текст комбобоксов по центру
        private void cbxDesign_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        brush = SystemBrushes.HighlightText;
                    }

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        #endregion

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            CurrentDocument.Clear();
            CurrentDocument.Focus();
        }
        #region TextEditing
        private void Button_Bold_Click(object sender, EventArgs e)
        {
            Font BoldFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Bold);
            Font RegularFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (CurrentDocument.SelectionFont.Bold)
            {
                CurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                CurrentDocument.SelectionFont = BoldFont;
            }
            CurrentDocument.Focus();
        }

        private void ComboBox_Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (CurrentDocument == null) { return; }
            //try
            //{
            //    Font NewFont = new Font(ComboBox_Font.SelectedItem.ToString(), CurrentDocument.SelectionFont.Size, FontStyle.Bold);
            //}
            //catch { }
            FontFamily myFontFamily = new FontFamily(ComboBox_Font.SelectedItem.ToString());
            if (myFontFamily.IsStyleAvailable(FontStyle.Regular)) { }
            if (myFontFamily.IsStyleAvailable(FontStyle.Bold)) { }
            if (myFontFamily.IsStyleAvailable(FontStyle.Italic)) { }
            if (myFontFamily.IsStyleAvailable(FontStyle.Strikeout)) { }
            if (myFontFamily.IsStyleAvailable(FontStyle.Underline)) { }

            try
            {
                Font NewFont = new Font(ComboBox_Font.SelectedItem.ToString(), CurrentDocument.SelectionFont.Size, CurrentDocument.SelectionFont.Style);

                CurrentDocument.SelectionFont = NewFont;
                SelectedFont = ComboBox_Font.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBox_Font.SelectedIndex = SelectedFont;
                ComboBox_Font_SelectedIndexChanged(sender, e);
            }
            //CurrentDocument.Focus();
        }

        private void ComboBox_FontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float.TryParse(ComboBox_FontSize.SelectedItem.ToString(), out float NewSize);
            Font NewFont = new Font(CurrentDocument.SelectionFont.Name, NewSize, CurrentDocument.SelectionFont.Style);
            CurrentDocument.SelectionFont = NewFont;
            //CurrentDocument.Focus();
        }

        private void Button_Italic_Click(object sender, EventArgs e)
        {
            Font ItalicFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Italic);
            Font RegularFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (CurrentDocument.SelectionFont.Italic)
            {
                CurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                CurrentDocument.SelectionFont = ItalicFont;
            }
            CurrentDocument.Focus();
        }

        private void Button_UnderLine_Click(object sender, EventArgs e)
        {
            Font UnderlineFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Underline);
            Font RegularFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (CurrentDocument.SelectionFont.Underline)
            {
                CurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                CurrentDocument.SelectionFont = UnderlineFont;
            }
            CurrentDocument.Focus();
        }

        private void Button_StrikeOut_Click(object sender, EventArgs e)
        {
            Font Strikeout = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Strikeout);
            Font RegularFont = new Font(CurrentDocument.SelectionFont.FontFamily, CurrentDocument.SelectionFont.SizeInPoints, FontStyle.Regular);

            if (CurrentDocument.SelectionFont.Strikeout)
            {
                CurrentDocument.SelectionFont = RegularFont;
            }
            else
            {
                CurrentDocument.SelectionFont = Strikeout;
            }
            CurrentDocument.Focus();
        }

        private void Button_Upper_Click(object sender, EventArgs e)
        {
            CurrentDocument.SelectedText = CurrentDocument.SelectedText.ToUpper();
        }

        private void Button_Lower_Click(object sender, EventArgs e)
        {
            CurrentDocument.SelectedText = CurrentDocument.SelectedText.ToLower();
            CurrentDocument.Focus();
        }

        private void Button_Bigger_Click(object sender, EventArgs e)
        {
            float NewFontSize = CurrentDocument.SelectionFont.SizeInPoints + 2;

            Font NewSize = new Font(CurrentDocument.SelectionFont.Name, NewFontSize, CurrentDocument.SelectionFont.Style);

            CurrentDocument.SelectionFont = NewSize;
            CurrentDocument.Focus();
        }

        private void Button_FontSmaller_Click(object sender, EventArgs e)
        {
            float NewFontSize = CurrentDocument.SelectionFont.SizeInPoints - 2;

            Font NewSize = new Font(CurrentDocument.SelectionFont.Name, NewFontSize, CurrentDocument.SelectionFont.Style);

            CurrentDocument.SelectionFont = NewSize;
            CurrentDocument.Focus();
        }
        private void Button_ForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentDocument.SelectionColor = cd.Color;
                Button_ForeColor.ForeColor = cd.Color;
            }
        }

        private void Button_BackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentDocument.SelectionBackColor = cd.Color;
                Button_BackColor.BackColor = cd.Color;
            }
        }
        #endregion

        public ProtoClasses.ProtoOrders.protoOrder order { get; set; } //сама заявка
        public bool CopyOrder { get; set; }//копии заявки
        private bool newOrder = true; // флаг новой заявки
        private int OrderStateSelectedIndex = 7;
        private string FormHeader = string.Empty;
        private FormBase formBase = null;

        #region Инициализация компонентов
        public FormBaseEdit(FormBase e, ProtoClasses.ProtoOrders.protoOrder Order, DataTable historyOrders, bool neworder, int orderStateSelectedIndex)//, bool copyOrder)
        {
            InitializeComponent();
            if (Order != null && !neworder)
            {
                order = Order;
                filesControl1.OrderID = order.id;
                newOrder = false;
            }
            formBase = e;
            TableOrdersHistory = historyOrders;
            OrderStateSelectedIndex = orderStateSelectedIndex;
            //textControl_InstallText.HandleCreated += TextControl_InstallText_HandleCreated;
        }



        //private void TextControl_InstallText_HandleCreated(object sender, EventArgs e)
        //{
        //    if (order != null && order.installation_comment != null && order.installation_comment.Trim() != string.Empty)
        //    {
        //        textControl_InstallText.Load(order.installation_comment, TXTextControl.StringStreamType.RichTextFormat);
        //    }
        //}
        #endregion

        #region Событие - пользователь поменял превью на новое
        private bool PreviewChanged = false; //Флаг изменения превью, чтобы при сохранении не сравнивать массив byte[]
        private string OrderMainPath = string.Empty;
        internal delegate void PreviewDelegate(bool e);
        private byte[] newPreviewCach = null;
        private void PictureBoxPreview_NewPreview(object sender, bool e)
        {
            Invoke(new PreviewDelegate(NewPreview), e);

        }
        private void NewPreview(bool e)
        {
            if (e) { newPreviewCach = pictureBoxPreview.PreviewBinary; PreviewChanged = true; }
            else
            {
                if (newPreviewCach != null) { pictureBoxPreview.Image = Utils.Converting.ByteToImage(newPreviewCach); }
                else
                {
                    if (OrderMainPath != string.Empty && File.Exists(OrderMainPath))
                    {
                        byte[] img = File.ReadAllBytes(OrderMainPath);
                        pictureBoxPreview.Image = Utils.Converting.ByteToImage(img);
                    }
                }
            }
        }
        #endregion

        #region Приход с сервера списка файлов заявки
        internal delegate void SocketDelegate(SocketClient.TableClient.RecieveFilesListEventArgs e);
        private void FileListRecieve(object sender, SocketClient.TableClient.RecieveFilesListEventArgs e)
        {
            if (e != null) { BeginInvoke(new SocketDelegate(FileListHandler), e); }
        }
        private void FileListHandler(SocketClient.TableClient.RecieveFilesListEventArgs e)
        {
            if (order.id == e.Recieve_OrderId)
            {
                filesControl1.SetFiles = e.filelist;
            }
        }
        #endregion

        #region Начальное открытие формы

        private void textBox_OrderName_TextChanged(object sender, EventArgs e)
        {
            textBox_OrderName.Text = Utils.FileNamesValidation.GetValidPath(textBox_OrderName.Text);
        }

        #region FORM_LOAD
        private void FormBaseEdit_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.icon_page_white_edit;
            try
            {
                pictureBox1.Image = Utils.ImagesUtils.ByteToImage(Program.PrinterPic);
                pictureBox2.Image = Utils.ImagesUtils.ByteToImage(Program.CutterPic);
                pictureBox3.Image = Utils.ImagesUtils.ByteToImage(Program.CncPic);
            }
            catch { }
            //Owner = new FormFileInfo(null);
            Owner = null;
            Location = new Point(formBase.Location.X + ((formBase.Width - Width) / 2), formBase.Location.Y + ((formBase.Height - Height) / 2));
            SocketClient.TableClient.FilelistCome += FileListRecieve; // Приход с сервера списка файлов заявки
            pictureBoxPreview.NewPreview += PictureBoxPreview_NewPreview; //NewPreview

            comboBox_Adder.Text = Utils.Settings.set.name;

            dataGridView_History.AutoGenerateColumns = false;
            GetFontCollection();
            PopulateFontSizes();

            SetConstantes();
            comboBox_PrintQuality.Items.Add("Хорошее");
            comboBox_PrintQuality.Items.Add("Быстрое");
            comboBox_PrintQuality.SelectedIndex = 0;

            HideCheckBox(treeView_Menu, treeView_Menu.Nodes[0]);
            HideCheckBox(treeView_Menu, treeView_Menu.Nodes[1]);
            HideCheckBox(treeView_Menu, treeView_Menu.Nodes[2]);
            HideCheckBox(treeView_Menu, treeView_Menu.Nodes[3]);
            tabControl_main.ItemSize = new Size(0, 1);
            panel_LeftMain.Focus();
            panel_Menu.Focus();
            treeView_Menu.ExpandAll();
            treeView_Menu.SelectedNode = treeView_Menu.Nodes[0];
            treeView_Menu.Focus();

            comboBox_MaterialPrint.DataSource = new BindingSource(SqlLite.Materials.DicMaterialPrint.OrderBy(pair => pair.Value), null);
            comboBox_MaterialPrint.DisplayMember = "Value";
            comboBox_MaterialPrint.ValueMember = "Key";
            comboBox_MaterialCut.DataSource = new BindingSource(SqlLite.Materials.DicMaterialCut.OrderBy(pair => pair.Value), null);
            comboBox_MaterialCut.DisplayMember = "Value";
            comboBox_MaterialCut.ValueMember = "Key";
            comboBox_MaterialCnc.DataSource = new BindingSource(SqlLite.Materials.DicMaterialCnc.OrderBy(pair => pair.Value), null);
            comboBox_MaterialCnc.DisplayMember = "Value";
            comboBox_MaterialCnc.ValueMember = "Key";


            comboBox_EquipPrint.DataSource = new BindingSource(SqlLite.Equip.DicPrinters.OrderBy(pair => pair.Value), null);
            comboBox_EquipPrint.DisplayMember = "Value";
            comboBox_EquipPrint.ValueMember = "Key";
            comboBox_EquipCut.DataSource = new BindingSource(SqlLite.Equip.DicCuters.OrderBy(pair => pair.Value), null);
            comboBox_EquipCut.DisplayMember = "Value";
            comboBox_EquipCut.ValueMember = "Key";
            comboBox_EquipCnc.DataSource = new BindingSource(SqlLite.Equip.DicCncs.OrderBy(pair => pair.Value), null);
            comboBox_EquipCnc.DisplayMember = "Value";
            comboBox_EquipCnc.ValueMember = "Key";
            if (comboBox_MaterialPrint.Items.Count > 1) { comboBox_MaterialPrint.SelectedItem = 12; }
            if (comboBox_MaterialCut.Items.Count > 1) { comboBox_MaterialCut.SelectedItem = 0; }
            if (comboBox_MaterialCnc.Items.Count > 1) { comboBox_MaterialCnc.SelectedItem = 0; }
            if (comboBox_EquipPrint.Items.Count > 1) { comboBox_EquipPrint.SelectedItem = 0; }
            if (comboBox_EquipCut.Items.Count > 1) { comboBox_EquipCut.SelectedItem = 0; }
            if (comboBox_EquipCnc.Items.Count > 1) { comboBox_EquipCnc.SelectedItem = 0; }


            comboBox_Adder.Items.AddRange(SqlLite.Order.Users.ToArray());
            comboBox_Customer.Items.AddRange(SqlLite.Order.Customers.ToArray());
            comboBox_PrintExecutor.Items.AddRange(SqlLite.Order.PrinterMans.ToArray());
            comboBox_CutExecutor.Items.AddRange(SqlLite.Order.CutterMans.ToArray());
            comboBox_CncExecutor.Items.AddRange(SqlLite.Order.CncMans.ToArray());

            if (order != null) { loadOrder(); }
            else
            {
                FormHeader = "Новая задача: ";
                order = new ProtoClasses.ProtoOrders.protoOrder();
                radioButton_delivery_CheckedChanged(this, null);
            }

            UpdateHistory();
            WorksList();

            #region События - потеря фокуса на контролах - для rtfinfo
            #region page1 - главное
            comboBox_Adder.LostFocus += Control_LostFocus;
            textBox_OrderName.LostFocus += Control_LostFocus;
            comboBox_Customer.LostFocus += Control_LostFocus;
            dateTimePicker_DateStart.LostFocus += Control_LostFocus;
            dateTimePicker_DeadLine.LostFocus += Control_LostFocus;
            textBox_Comment.LostFocus += Control_LostFocus;
            #endregion
            #endregion

            treeView_Menu.SelectedNode = treeView_Menu.Nodes["nodemain"];
        }
        #endregion

        #region Установка констант - размеры, метрические системы и т.д.
        private void SetConstantes()
        {
            SetPaperFormates(comboBox_PrintFormat);
            SetPaperFormates(comboBox_CutFormat);
            SetPaperFormates(comboBox_CncFormat);

            SetPaperunits(comboBox_PrintUnit);
            SetPaperunits(comboBox_CutUnit);
            SetPaperunits(comboBox_CncUnit);

            comboBox_OrderState.Items.AddRange(new object[] {
                        "Новое задание (Ожидание)",
                        "В работе",
                        "Сделано (Идёт постобработка)",
                        "Готово (Склад)",
                        "Закрыто (Отдано клиенту)",
                        "Остановлено! (Укажите причину!)",
                        "Удалить (Корзина)",
                        "В черновики"
            });
            comboBox_OrderState.SelectedIndex = OrderStateSelectedIndex;
        }

        private void SetPaperFormates(ComboBox cb)
        {
            cb.Items.Add("Свой размер");
            cb.Items.Add("A0");
            cb.Items.Add("A1");
            cb.Items.Add("A2");
            cb.Items.Add("A3");
            cb.Items.Add("A4");
            cb.Items.Add("A5");
            cb.Items.Add("A6");
            cb.SelectedIndex = 0;
        }
        private void SetPaperunits(ComboBox cb)
        {
            cb.Items.Add("мм");
            cb.Items.Add("см");
            cb.Items.Add("м");
            cb.SelectedIndex = 0;
        }
        #endregion

        #region Блок RTF INFO
        #region обработчики событий для RTFINFO
        // комбобоксы
        private void comboBox_RefreshRTF_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTFChangeEventON(ShowRTFInfo());
        }

        // Итоговые суммы площадей-длинн
        private void numericUpDown_RefreshRTF_ValueChanged(object sender, EventArgs e)
        {
            RTFChangeEventON(ShowRTFInfo());
        }



        // Потеря фокуса на textbox-ах
        private void Control_LostFocus(object sender, EventArgs e)
        {
            RTFChangeEventON(ShowRTFInfo());
        }
        #endregion

        #region Формирование RTFINFO
        private string ShowRTFInfo()
        {
            string TextOut = string.Empty;
            TextOut = @"{\rtf1 {\qc\loch\f6\b " + textBox_OrderName.Text + @" \b0 от \b " + comboBox_Customer.Text + @" \b0 ";
            TextOut += @"\par";

            if (string.IsNullOrEmpty(textBox_Comment.Text))
            {
            }
            else
            {
                TextOut += @"\par\ql " + textBox_Comment.Text.Replace("\r\n", @" \par ") + @" \par";
            }

            if (treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].Checked)
            {
                TextOut += @"\par\ql • Печать на \b ";
                TextOut += comboBox_EquipPrint.Text + @"\b0\par\tab материал: \b " + comboBox_MaterialPrint.Text + @"\b0 ";
                TextOut += @"\par\tab Размер: \b " + AlltoMeter(numericUpDown_PrintWidth.Value, comboBox_PrintUnit.SelectedIndex).ToString() + @"\b0 {  x }\b " + AlltoMeter(numericUpDown_PrintHeight.Value, comboBox_PrintUnit.SelectedIndex) + @"\b0 { м.}";
                TextOut += @"\par\tab Количество: \b " + numericUpDown_PrintCopy.Value.ToString() + @" \b0 { шт.}";
                TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(numericUpDown_PrintSquare.Value, 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                TextOut += @"\par\tab Качество: \b " + comboBox_PrintQuality.Text + @" \b0";
                TextOut += @"\par";

            }

            if (treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].Checked)
            {
                TextOut += @"\par\ql • Резка на \b ";

                TextOut += comboBox_EquipCut.Text + @"\b0\par\tab материал: \b " + comboBox_MaterialCut.Text + @"\b0 ";
                TextOut += @"\par\tab Размер: \b " + AlltoMeter(numericUpDown_CutWidth.Value, comboBox_CutUnit.SelectedIndex).ToString() + @"\b0 {  x }\b " + AlltoMeter(numericUpDown_CutHeight.Value, comboBox_CutUnit.SelectedIndex).ToString() + @"\b0 { м.}";
                TextOut += @"\par\tab Количество: \b " + numericUpDown_CutCopy.Value.ToString() + @" \b0 { шт.}";
                TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(numericUpDown_CutSquare.Value, 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                TextOut += @"\par\tab Длина пробега ножа: \b " + numericUpDown_CutLineLengthTotal.Value.ToString() + @" \b0 { м}";
                if (checkBox_CutOnPrint.Checked) { TextOut += @"\par \b Резка по меткам \b0"; }
                TextOut += @"\par";

            }


            if (treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].Checked)
            {
                TextOut += @"\par\ql • Фрезеровка на \b ";
                TextOut += comboBox_EquipCnc.Text + @"\b0\par\tab материал: \b " + comboBox_MaterialCnc.Text + @"\b0 ";
                TextOut += @"\par\tab Размер: \b " + AlltoMeter(numericUpDown_CncWidth.Value, comboBox_CncUnit.SelectedIndex).ToString() + @"\b0 {  x }\b " + AlltoMeter(numericUpDown_CncHeight.Value, comboBox_CncUnit.SelectedIndex).ToString() + @"\b0 { м.}";
                TextOut += @"\par\tab Количество: \b " + numericUpDown_CncCopy.Value.ToString() + @" \b0 { шт.}";
                TextOut += @"\par\tab Квадратура: \b " + Convert.ToString(Math.Round(numericUpDown_CncSquare.Value, 3, MidpointRounding.AwayFromZero)) + @" \b0 { м2}";
                TextOut += @"\par\tab Длина пробега фрезы: \b " + numericUpDown_CncLineLengthTotal.Value.ToString() + @" \b0 { м}";
                if (checkBox_CncOnPrint.Checked) { TextOut += @"\par \b Фрезеровка по меткам \b0"; }
                TextOut += @"\par";

            }

            string extrawork = string.Empty;

            if (!radioButton_lamin0.Checked)
            {
                if (radioButton_LaminMat.Checked) { extrawork += @"\par • Матовая ламинация"; } else { extrawork += @"\par • Глянцевая ламинация "; }
            }

            if (!radioButton_Baner0.Checked)
            {
                if (radioButton_BanerLuvers.Checked) { extrawork += @"\par • Люверсы через " + numericUpDown_BanerHand.Value.ToString() + " см."; } else { extrawork += @"\par • Карманы " + numericUpDown_BanerHand.Value.ToString() + " см."; }
            }

            if (treeView_Menu.Nodes["nodemain"].Nodes["nodeinstall"].Checked) { extrawork += @"\par • " + "Монтажные работы"; }

            foreach (string worktype in worklst)
            {
                extrawork += @"\par • " + worktype;
            }


            if (!radioButton_deliveryNo.Checked)
            {
                if (radioButton_deliveryOffice.Checked)
                {
                    extrawork += @"\par • Доставка в офис";
                }
                else
                {
                    if (deliveryAddress.Trim() == string.Empty) { extrawork += @"\par • Доставка"; } else { extrawork += @"\par • Доставка по адресу: " + deliveryAddress; }
                }

            }
            if (extrawork != string.Empty) { TextOut += @"\par\tab\b Дополнительные работы: \b0" + extrawork; }
            TextOut += @"\par}}";
            return TextOut;
        }
        #endregion

        #region Событие для RTFINFO и обрабока показа/сокрытия
        private OrderEditInfo orderEditInfo = null;
        public delegate void RTFChangeEventDelegate(object sender, string str);
        public event RTFChangeEventDelegate RTFChangeEvent;
        public void RTFChangeEventON(string e) { RTFChangeEvent?.Invoke(this, e); }

        private void Button_Info_Click(object sender, EventArgs e)
        {
            Button_Info.Enabled = false;
            if (orderEditInfo == null)
            {

                orderEditInfo = new OrderEditInfo(this);
                orderEditInfo.FormClosed += OrderEditInfo_FormClosed;
                orderEditInfo.Show();
                RTFChangeEventON(ShowRTFInfo());
            }
            else
            {
                orderEditInfo.ToClose();
            }
        }

        private void OrderEditInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            orderEditInfo.FormClosed -= OrderEditInfo_FormClosed;
            Button_Info.Image = Properties.Resources.script_go;
            Button_Info.Enabled = true;
            orderEditInfo = null;
        }

        public void orderEditInfoEnable()
        {
            Button_Info.Enabled = true;
            Button_Info.Image = Properties.Resources.script_back;
        }
        #endregion
        #endregion

        #region Загрузка данных заявки на контролы формы
        private void loadOrder()
        {
            #region Главные установки - для меню
            FormHeader = "Задача №" + order.id.ToString() + " : ";
            this.Text = FormHeader + order.work_name;
            comboBox_Adder.Text = order.adder;
            ////comboBox_Adder.Text = Utils.Settings.set.name;
            //label1.Text = "Изменил:";
            textBox_OrderName.Text = order.work_name;
            comboBox_Customer.Text = order.client;

            if (order.worktypes_list != string.Empty)
            {
                string[] worktypes_list = order.worktypes_list.Split((char)219);
                string tmp = string.Empty;
                foreach (string worktype in worktypes_list)
                {
                    tmp = worktype.Replace(Convert.ToString((char)(219)), string.Empty);
                    if (tmp != string.Empty) { worklst.Add(tmp); }
                }
            }

            //if (order.installation_comment != null && order.installation_comment != string.Empty)
            //{
            //    //serverTextControl1.Load(order.installation_comment, TXTextControl.StringStreamType.RichTextFormat);
            //    textControl_InstallText.DataBindings.Add(new Binding("Rtf", order.installation_comment, "rtfText", true));
            //}

            DateTime dt = Utils.UnixDate.Int64ToDateTime(order.date_start);
            if (dt < dateTimePicker_DateStart.MinDate) { dateTimePicker_DateStart.Value = dateTimePicker_DateStart.MinDate; } else {
                if (dt > dateTimePicker_DateStart.MaxDate) { dateTimePicker_DateStart.Value = dateTimePicker_DateStart.MaxDate; } else { dateTimePicker_DateStart.Value = dt; }}
            dt = Utils.UnixDate.Int64ToDateTime(order.dead_line);
            if (dt < dateTimePicker_DeadLine.MinDate) { dateTimePicker_DeadLine.Value = dateTimePicker_DeadLine.MinDate; } else {
                if (dt > dateTimePicker_DeadLine.MaxDate) { dateTimePicker_DeadLine.Value = dateTimePicker_DeadLine.MaxDate; } else{ dateTimePicker_DeadLine.Value = dt; }}
            treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].Checked = order.print_on;
            treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].Checked = order.cut_on;
            treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].Checked = order.cnc_on;
            treeView_Menu.Nodes["nodemain"].Nodes["nodeinstall"].Checked = order.installation;
            textBox_Comment.Text = order.comments;
            #endregion
            #region GetPreview
            DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(order.time_recieve);
            if (tmpenddate != null)
            {
                OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + order.id.ToString() + @"\index.png";
                if (File.Exists(OrderMainPath))
                {
                    byte[] img = File.ReadAllBytes(OrderMainPath);
                    pictureBoxPreview.Image = Utils.Converting.ByteToImage(img);
                }
                else { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
            }
            else { pictureBoxPreview.Image = Properties.Resources.New_256x256; }
            #endregion
            #region Статусы состояние задач
            checkBox_PrintEnd.Checked = order.state_print;
            checkBox_CutEnd.Checked = order.state_cut;
            checkBox_CncEnd.Checked = order.state_cnc;
            checkBox_CutOnPrint.Checked = order.cutting_on_print;
            checkBox_CncOnPrint.Checked = order.cnc_on_print;
            dt = Utils.UnixDate.Int64ToDateTime(order.date_ready_print);
            if (dt < dateTimePicker_PrintEnded.MinDate) { dateTimePicker_PrintEnded.Value = dateTimePicker_PrintEnded.MinDate; } else
            {
                if (dt > dateTimePicker_PrintEnded.MaxDate) { dateTimePicker_PrintEnded.Value = dateTimePicker_PrintEnded.MaxDate; } else { dateTimePicker_PrintEnded.Value = dt; }
            }
            dt = Utils.UnixDate.Int64ToDateTime(order.date_ready_cut);
            if (dt < dateTimePicker_CutEnded.MinDate) { dateTimePicker_CutEnded.Value = dateTimePicker_CutEnded.MinDate; } else
            {
                if (dt> dateTimePicker_CutEnded.MaxDate) { dateTimePicker_CutEnded.Value = dateTimePicker_CutEnded.MaxDate; } else { dateTimePicker_CutEnded.Value = dt; }
            }
            dt = Utils.UnixDate.Int64ToDateTime(order.date_ready_cnc);
            if (dt < dateTimePicker_CncEnded.MinDate) { dateTimePicker_CncEnded.Value = dateTimePicker_CncEnded.MinDate; } else
            {
                if (dt > dateTimePicker_CncEnded.MaxDate) { dateTimePicker_CncEnded.Value = dateTimePicker_CncEnded.MaxDate; } else { dateTimePicker_CncEnded.Value = dt; }
            }
            comboBox_PrintExecutor.Text = order.printerman;
            comboBox_CutExecutor.Text = order.cutterman;
            comboBox_CncExecutor.Text = order.cncman;
            comboBox_OrderState.SelectedIndex = order.state;
            if (order.laminat)
            {
                if (order.laminat_mat)
                { radioButton_LaminMat.Checked = true; }
                else
                {
                    radioButton_LaminGlose.Checked = true;
                }
            }
            else { radioButton_lamin0.Checked = true; }

            if (order.delivery)
            {
                if (order.delivery_office) { deliveryOfficeAddress = order.delivery_address; radioButton_deliveryOffice.Checked = true; } else { deliveryAddress = order.delivery_address; radioButton_deliveryClient.Checked = true; }
                //textBox_DeliveryAddress.Text = order.delivery_address;
            }
            else { radioButton_deliveryNo.Checked = true; }


            if (order.baner_handling)
            {
                if (order.baner_luvers) { radioButton_BanerLuvers.Checked = true; } else { radioButton_BanerPoket.Checked = true; }
                numericUpDown_BanerHand.Value = Convert.ToDecimal(order.baner_handling_size);
            }
            else { radioButton_Baner0.Checked = true; }
            #endregion


            #region Setting material
            List<KeyValuePair<int, string>> mprint = SqlLite.Materials.DicMaterialPrint.Where(item => item.Key == order.material_print_id).ToList();
            if (mprint != null && mprint.Count > 0) { comboBox_MaterialPrint.SelectedItem = mprint[0]; }

            List<KeyValuePair<int, string>> mcut = SqlLite.Materials.DicMaterialCut.Where(item => item.Key == order.material_cut_id).ToList();
            if (mcut != null && mcut.Count > 0) { comboBox_MaterialCut.SelectedItem = mcut[0]; }

            List<KeyValuePair<int, string>> mcnc = SqlLite.Materials.DicMaterialCnc.Where(item => item.Key == order.material_cnc_id).ToList();
            if (mcnc != null && mcnc.Count > 0) { comboBox_MaterialCnc.SelectedItem = mcnc[0]; }

            List<KeyValuePair<int, string>> mprintters = SqlLite.Equip.DicPrinters.Where(item => item.Key == order.printers_id).ToList();
            if (mprintters != null && mprintters.Count > 0) { comboBox_EquipPrint.SelectedItem = mprintters[0]; }

            List<KeyValuePair<int, string>> mcutters = SqlLite.Equip.DicCuters.Where(item => item.Key == order.cutters_id).ToList();
            if (mcutters != null && mcutters.Count > 0) { comboBox_EquipCut.SelectedItem = mcutters[0]; }

            List<KeyValuePair<int, string>> mcncs = SqlLite.Equip.DicCncs.Where(item => item.Key == order.cncs_id).ToList();
            if (mcncs != null && mcncs.Count > 0) { comboBox_EquipCnc.SelectedItem = mcncs[0]; }
            #endregion




            #region размеры/количество
            #region количество
            numericUpDown_CncCopy.Value = Convert.ToDecimal(order.count_cnc);
            numericUpDown_CutCopy.Value = Convert.ToDecimal(order.count_cut);
            numericUpDown_PrintCopy.Value = Convert.ToDecimal(order.count_print);

            numericUpDown_CncLineLengthCount.Value = Convert.ToDecimal(order.count_size_cnc);
            numericUpDown_CutLineLengthCount.Value = Convert.ToDecimal(order.count_size_cut);
            #endregion
            #region всего длина реза
            numericUpDown_CncLineLengthTotal.Value = Convert.ToDecimal(order.size_cnc);
            numericUpDown_CutLineLengthTotal.Value = Convert.ToDecimal(order.size_cut);
            #endregion
            #region ширина/длина
            numericUpDown_CncWidth.Value = AllfromMeter(order.size_x_cnc, comboBox_CncUnit.SelectedIndex);
            numericUpDown_CutWidth.Value = AllfromMeter(order.size_x_cut, comboBox_CutUnit.SelectedIndex);
            numericUpDown_PrintWidth.Value = AllfromMeter(order.size_x_print, comboBox_PrintUnit.SelectedIndex);

            numericUpDown_CncHeight.Value = AllfromMeter(order.size_y_cnc, comboBox_CncUnit.SelectedIndex);
            numericUpDown_CutHeight.Value = AllfromMeter(order.size_y_cut, comboBox_CutUnit.SelectedIndex);
            numericUpDown_PrintHeight.Value = AllfromMeter(order.size_y_print, comboBox_PrintUnit.SelectedIndex);
            #endregion
            #region длина реза
            numericUpDown_CncLineLength.Value = Convert.ToDecimal(order.line_size_cnc);
            numericUpDown_CutLineLength.Value = Convert.ToDecimal(order.line_size_cut);
            #endregion
            #region квадратура
            numericUpDown_CncSquare.Value = Convert.ToDecimal(order.square_cnc);
            numericUpDown_CutSquare.Value = Convert.ToDecimal(order.square_cut);
            numericUpDown_PrintSquare.Value = Convert.ToDecimal(order.square_print);
            #endregion
            #endregion
        }
        #region история заявки

        private DataTable TableOrdersHistory = null;
        private List<ProtoClasses.ProtoOrderHistory.protoRow> newordershistoryrows = new List<ProtoClasses.ProtoOrderHistory.protoRow>();
        private void button_addcomment_Click(object sender, EventArgs e)
        {

            ProtoClasses.ProtoOrderHistory.protoRow pr = new ProtoClasses.ProtoOrderHistory.protoRow();
            DataRow dr = TableOrdersHistory.NewRow();
            dr["adder"] = Utils.Settings.set.name;
            pr.adder = Utils.Settings.set.name;
            dr["Datetime_date"] = DateTime.Now;
            pr.date_change = Utils.UnixDate.DateTimeToInt64(DateTime.Now);
            dr["note"] = textBox_historyComment.Text;
            pr.note = textBox_historyComment.Text;
            dr["work_id"] = order.id;
            pr.id = order.id;
            TableOrdersHistory.Rows.Add(dr);
            newordershistoryrows.Add(pr);
            //ProtoClasses.ProtoOrderHistory.protoRow pr = new ProtoClasses.ProtoOrderHistory.protoRow();
            //pr.adder = Utils.Settings.set.name;
            //pr.note = textBox_historyComment.Text;
            //DateTime dt = DateTime.Now;
            //pr.time_recieve = Utils.UnixDate.DateTimeToInt64(dt);
            //newhistory.Add(pr);
            //dataGridView_History.Rows.Add(0, dt, pr.note, pr.adder);
            UpdateHistory();
            textBox_historyComment.Text = string.Empty;
        }

        private void UpdateHistory()
        {
            dataGridView_History.DataSource = null;
            if (TableOrdersHistory != null && TableOrdersHistory.Rows != null)
            {
                TableOrdersHistory.DefaultView.Sort = "Datetime_date DESC";
                dataGridView_History.DataSource = TableOrdersHistory.DefaultView.ToTable();
            }
        }
        #region мусор
        //private void getHistory()
        //{
        //    if (SqlLite.OrderHistory.TableOrderHistory != null && SqlLite.OrderHistory.TableOrderHistory.Rows!= null)
        //    {
        //        TableOrdersHistory = SqlLite.OrderHistory.TableOrderHistory.Select("work_id = " + order.id).CopyToDataTable();
        //        //dataGridView_OrderHistory.Sort(dataGridView_OrderHistory.Columns[""], ListSortDirection.Descending);
        //        //SocketClient.TableClient.SocketClientEvent += this.TableClient_SocketClientEvent;
        //        //if (TableOrdersHistory != null)
        //        //{

        //        //    //TableOrdersHistory.DefaultView.RowFilter = "work_id = " + order.id;
        //        //    TableOrdersHistory.DefaultView.Sort = "Datetime_date DESC";
        //        //    dataGridView_History.DataSource = TableOrdersHistory.DefaultView.ToTable();
        //        //}

        //        UpdateHistory();
        //    }
        //}
        #endregion

        #endregion
        #endregion

        #endregion

        #region Мусор
        //#region Материалы
        //private void MaterialFill()
        //{
        //    Dictionary<int,string> dt =  SqlLite.Materials.TableMaterialPrint.AsEnumerable().ToDictionary<DataRow, int, string>(row => row.Field<int>("id"), row => row.Field<string>("material_name"));
        //    comboBox_MaterialPrint.DataSource = new BindingSource(dt.OrderBy(pair => pair.Value), null);
        //    comboBox_MaterialPrint.DisplayMember = "Value";
        //    comboBox_MaterialPrint.ValueMember = "Key";
        //    //DataTable tableMaterialPrint = SqlLite.Materials.TableMaterialPrint;
        //    //DataRow[] rowsMaterialPrint = tableMaterialPrint.Select();
        //    //for (int i = 0; i < rowsMaterialPrint.Length; i++)
        //    //{
        //    //    MaterialPrint.Add(Convert.ToInt32(rowsMaterialPrint[i]["id"]), Convert.ToString(rowsMaterialPrint[i]["material_name"]));
        //    //}
        //    //comboBox_MaterialPrint.DataSource = new BindingSource(MaterialPrint.OrderBy(pair => pair.Value), null);
        //    //comboBox_MaterialPrint.DisplayMember = "Value";
        //    //comboBox_MaterialPrint.ValueMember = "Key";
        //}

        //#endregion
        //void ControlReceivedFocus(object sender, EventArgs e)
        //{

        //    Trace.WriteLine(((Control)sender).Name);
        //    //Debug.WriteLine(sender + " received focus.");
        //    //if ((Control)sender.)

        //}
        //private void ComboBox_Font_GotFocus(object sender, EventArgs e)
        //{
        //    CurrentDocument = CurrentDocumentTMP;
        //    shownEditedText();
        //    //Trace.WriteLine("ComboBox_Font_GotFocus");
        //    //CustomControls.RichTextBoxEx CurrentDocumentTMP = null;
        //    //throw new NotImplementedException();
        //}

        //private void RichTextBoxEx_Comment_LostFocus(object sender, EventArgs e)
        //{
        //    //string tmp = this.ActiveControl.Name;
        //    //Trace.WriteLine("RichTextBoxEx_Comment_LostFocus");
        //    CurrentDocumentTMP = CurrentDocument;
        //    CurrentDocument = null;
        //    shownEditedText();
        //}

        //private void RichTextBoxEx_Comment_GotFocus(object sender, EventArgs e)
        //{
        //    CurrentDocument = sender as CustomControls.RichTextBoxEx;
        //    shownEditedText();
        //}
        //private void shownEditedText()
        //{
        //    if (CurrentDocument != null) { toolStrip_Text.Enabled = true; }else { toolStrip_Text.Enabled = false; }
        //}

        //public TreeNode previousSelectedNode = null;
        //private void treeView_Menu_Validating(object sender, CancelEventArgs e)
        //{
        //    //treeView_Menu.SelectedNode.BackColor = SystemColors.Highlight;
        //    treeView_Menu.SelectedNode.BackColor = Color.Red;
        //    treeView_Menu.SelectedNode.ForeColor = Color.White;
        //    previousSelectedNode = treeView_Menu.SelectedNode;
        //}

        //private void treeView_Menu_DrawNode(object sender, DrawTreeNodeEventArgs e)
        //{
        //    //if (((e.State & TreeNodeStates.Selected) != 0) && (!treeView_Menu.Focused))
        //    //    e.Node.ForeColor = Color.Blue;
        //    //else
        //    //    e.DrawDefault = true;
        //    if (e.Node == null) return;

        //    // if treeview's HideSelection property is "True", 
        //    // this will always returns "False" on unfocused treeview
        //    bool selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
        //    bool unfocused = !e.Node.TreeView.Focused;

        //    // we need to do owner drawing only on a selected node
        //    // and when the treeview is unfocused, else let the OS do it for us
        //    if (selected)
        //    {
        //        var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
        //        //e.Graphics. //.SetBounds(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height, BoundsSpecified.All);
        //        //Graphics GR = new Graphics();
        //        //GR.rES
        //        //Bounds
        //        //e.Bounds.X = e.Bounds.X + 10;
        //        //e.Bounds = 
        //        //TreeNode tn = TreeNode();
        //        //tn.Bounds =  e.Bounds
        //        System.Windows.Forms.VisualStyles.CheckBoxState cbsChDis;
        //        if (e.Node.Checked)
        //        {
        //            cbsChDis = System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal;
        //        }else
        //        {
        //            cbsChDis = System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
        //        }
        //        Size glyph = Size.Empty;
        //        glyph = CheckBoxRenderer.GetGlyphSize(e.Graphics, cbsChDis);

        //        Rectangle tBounds = e.Node.Bounds;  // the real bounds of the hittest area
        //        e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, tBounds);
        //        e.Graphics.DrawString(e.Node.Text, Font, Brushes.White, tBounds.X, tBounds.Y);
        //        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        //        //CheckBoxRenderer.DrawParentBackground(e.Graphics, e.Bounds, this);
        //        Point cBoxLocation = new Point(tBounds.Left - glyph.Width, tBounds.Top);
        //        CheckBoxRenderer.DrawCheckBox(e.Graphics, cBoxLocation, cbsChDis);
        //        TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
        //    }
        //    else
        //    {
        //        e.DrawDefault = true;
        //    }
        //    //CheckBoxState cbsChDis = CheckBoxState.CheckedDisabled;
        //    //CheckBoxState cbsUCDis = CheckBoxState.UncheckedDisabled;

        //    //Size glyph = Size.Empty;
        //    //glyph = CheckBoxRenderer.GetGlyphSize(e.Graphics, cbsChDis);

        //    //Rectangle tBounds = e.Node.Bounds;  // the real bounds of the hittest area

        //    //if (e.Node.IsSelected)
        //    //{
        //    //    e.Graphics.FillRectangle(SystemBrushes.MenuHighlight, tBounds);
        //    //    e.Graphics.DrawString(e.Node.Text, Font, Brushes.White,
        //    //                            tBounds.X, tBounds.Y);
        //    //}
        //    //else
        //    //{
        //    //    CheckBoxRenderer.DrawParentBackground(e.Graphics, e.Bounds, this);
        //    //    e.Graphics.DrawString(e.Node.Text, Font, Brushes.Black,
        //    //                            tBounds.X, tBounds.Y);
        //    //}

        //    //Point cBoxLocation = new Point(tBounds.Left - glyph.Width, tBounds.Top);
        //    //CheckBoxState bs1 = e.Node.Checked ? cbsChDis : cbsUCDis;
        //    //CheckBoxRenderer.DrawCheckBox(e.Graphics, cBoxLocation, bs1);
        //}

        //private void treeView_Menu_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        //{
        //    if (treeView_Menu.SelectedNode != null)
        //        treeView_Menu.SelectedNode.ForeColor = Color.Black;
        //    e.Node.ForeColor = Color.Blue;
        //}

        //private void toolStripButton18_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {

        //        // Set filter options and filter index.
        //        ofd.Filter = "Изображения | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif; *.bmp; *.tif; *.tiff; |Все файлы (*.*)|*.*";
        //        ofd.FilterIndex = 1;
        //        ofd.Multiselect = false;
        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            if (System.IO.File.Exists(ofd.FileName))
        //            {
        //                BlockPreview = true;
        //                pictureBox1.Image = Properties.Resources.iLoading;
        //                Utils.CorelDrawExporter cde = new Utils.CorelDrawExporter();
        //                cde.CorelDrawExporterUpdateEvent += Cde_CorelDrawExporterUpdateEvent;
        //                cde.ExportToPng(ofd.FileName);
        //            }
        //        }
        //    }
        //}

        //#region Установка и работа с превью материала

        //bool BlockPreview = false;
        //private void FormBaseEdit_DragOver(object sender, DragEventArgs e)
        //{
        //    if (!BlockPreview)
        //    {
        //        int x = pictureBox1.PointToClient(new Point(e.X, e.Y)).X;
        //        int y = pictureBox1.PointToClient(new Point(e.X, e.Y)).Y;
        //        if (x >= 0 && x <= pictureBox1.Size.Width)
        //        {
        //            if (y >= 0 && y <= pictureBox1.Size.Height)
        //            {
        //                e.Effect = DragDropEffects.Copy;
        //            }
        //            else
        //            {
        //                e.Effect = DragDropEffects.None;
        //            }
        //        }
        //        else
        //        {
        //            e.Effect = DragDropEffects.None;
        //        }
        //    }
        //    else
        //    {
        //        e.Effect = DragDropEffects.None;
        //    }
        //}


        //private void FormBaseEdit_DragDrop(object sender, DragEventArgs e)
        //{
        //    if (e.Effect == DragDropEffects.Copy)
        //    {
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        //        switch (files.Length)
        //        {
        //            //case 0:
        //            //    break;
        //            case 1:
        //                if (System.IO.File.Exists(files[0]))
        //                {
        //                    BlockPreview = true;
        //                    pictureBox1.Image = Properties.Resources.iLoading;
        //                    Utils.CorelDrawExporter cde = new Utils.CorelDrawExporter();
        //                    cde.CorelDrawExporterUpdateEvent += Cde_CorelDrawExporterUpdateEvent;
        //                    cde.ExportToPng(files[0]);
        //                }
        //                break;
        //            default:
        //                //using (FormsStock.FormSelectImageFromArray frm = new FormsStock.FormSelectImageFromArray(Cursor.Position.X, Cursor.Position.Y, files))
        //                //{
        //                //    frm.ShowInTaskbar = true;
        //                //    frm.Owner = this;
        //                //    frm.ShowDialog(this);
        //                //    if (frm.img != null)
        //                //    {
        //                //        pictureBox1.Image = frm.img;
        //                //    }
        //                //}
        //                break;
        //        }
        //    }
        //}
        //private void FormBaseEdit_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) { e.Effect = DragDropEffects.All; } else { e.Effect = DragDropEffects.None; }
        //}
        //#region Получение превью
        //Image previewImage = null;
        //private void Cde_CorelDrawExporterUpdateEvent(object sender, Utils.CorelDrawExporter.CorelDrawExporterEventArgs e)
        //{
        //    if (e.ex != null || e.preview == null)
        //    {
        //        MessageBox.Show(e.ex.Message);
        //    }
        //    else
        //    {
        //        if (previewImage != null) { previewImage.Dispose(); }
        //        previewImage = e.preview;
        //        pictureBox1.Image = previewImage;
        //    }
        //    Utils.CorelDrawExporter cde = sender as Utils.CorelDrawExporter;
        //    cde.CorelDrawExporterUpdateEvent -= Cde_CorelDrawExporterUpdateEvent;
        //    BlockPreview = false;
        //}
        //#endregion

        //#endregion
        #endregion

        #region Блок TREEVIEW
        #region Рекация на выбор treeview, Переключение между страницами tabcontrol 
        private void treeView_Menu_AfterSelect(object sender, TreeViewEventArgs e)
        {

            switch (e.Node.Name)
            {
                case "nodemain":
                    tabControl_main.SelectedIndex = 0;
                    break;
                case "nodeprint":
                    tabControl_main.SelectedIndex = 1;
                    groupBox_Print.Enabled = e.Node.Checked;
                    break;
                case "nodecut":
                    tabControl_main.SelectedIndex = 2;
                    groupBox_Cut.Enabled = e.Node.Checked;
                    break;
                case "nodecnc":
                    tabControl_main.SelectedIndex = 3;
                    groupBox_Cnc.Enabled = e.Node.Checked;
                    break;
                case "nodefinish":
                    tabControl_main.SelectedIndex = 4;
                    groupBox_Install_tmp.Enabled = e.Node.Checked;
                    break;
                case "nodeinstall":
                    tabControl_main.SelectedIndex = 9;
                    groupBox_Install_tmp.Enabled = e.Node.Checked;
                    if (!newOrder && order != null && textControl_InstallText.Text.Trim() == string.Empty)
                    {
                        DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(order.time_recieve);
                        if (tmpenddate != null)
                        {
                            string montage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + order.id.ToString() + @"\montage.doc";
                            if (File.Exists(montage))
                            {
                                try
                                {
                                    TXTextControl.LoadSettings ls = new TXTextControl.LoadSettings
                                    {
                                        ApplicationFieldFormat = TXTextControl.ApplicationFieldFormat.MSWord
                                    };
                                    textControl_InstallText.Load(montage, TXTextControl.StreamType.MSWord, ls);
                                }
                                catch (Exception ex)
                                { MessageBox.Show(ex.Message, "Ошибка открытия *.doc"); }
                            }
                        }
                    }
                    break;
                case "nodefiles":
                    tabControl_main.SelectedIndex = 6;
                    break;
                case "nodehistory":
                    tabControl_main.SelectedIndex = 7;
                    break;
            }
        }

        private bool unlockcheck = true;
        private void treeView_Menu_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (unlockcheck)
            {
                unlockcheck = false;
                bool nodeckecked = e.Node.Checked;
                switch (e.Node.Name)
                {
                    case "nodeprint":
                        groupBox_Print.Enabled = e.Node.Checked;
                        if (groupBox_Print.Enabled)
                        {
                            if (checkBox_PrintEnd.Checked) { e.Node.ImageIndex = 1; e.Node.SelectedImageIndex = 1; } else { e.Node.ImageIndex = 0; e.Node.SelectedImageIndex = 0; }
                        }
                        else
                        {
                            e.Node.ImageIndex = 4; e.Node.SelectedImageIndex = 4;
                        }
                        break;
                    case "nodecut":
                        groupBox_Cut.Enabled = e.Node.Checked;
                        if (groupBox_Cut.Enabled)
                        {
                            if (checkBox_CutEnd.Checked) { e.Node.ImageIndex = 1; e.Node.SelectedImageIndex = 1; } else { e.Node.ImageIndex = 0; e.Node.SelectedImageIndex = 0; }
                        }
                        else
                        {
                            e.Node.ImageIndex = 4; e.Node.SelectedImageIndex = 4;
                            checkBox_CutOnPrint.Checked = false;
                        }
                        break;
                    case "nodecnc":
                        groupBox_Cnc.Enabled = e.Node.Checked;
                        if (groupBox_Cnc.Enabled)
                        {
                            if (checkBox_CncEnd.Checked) { e.Node.ImageIndex = 1; e.Node.SelectedImageIndex = 1; } else { e.Node.ImageIndex = 0; e.Node.SelectedImageIndex = 0; }
                        }
                        else
                        {
                            e.Node.ImageIndex = 4; e.Node.SelectedImageIndex = 4;
                            checkBox_CncOnPrint.Checked = false;
                        }
                        break;
                    case "nodeinstall":
                        groupBox_Install.Enabled = e.Node.Checked;
                        if (groupBox_Install.Enabled)
                        {
                            e.Node.ImageIndex = treeView_Menu.Nodes["nodemain"].ImageIndex; e.Node.SelectedImageIndex = treeView_Menu.Nodes["nodemain"].SelectedImageIndex;
                            //textControl_InstallText.BringToFront();
                            //if (order != null && order.installation_comment != null && order.installation_comment != string.Empty && textControl_InstallText.Text.Trim() == string.Empty)
                            //{
                            //    //string s = "";
                            //    //if (textControl_InstallText.Text.Trim() != string.Empty) { textControl_InstallText.Save(out s, TXTextControl.StringStreamType.PlainText); }
                            //    //if (s == string.Empty)
                            //    //{
                            //    //    //textControl_InstallText.lLoadTextAsync
                            //    //    //TXTextControl.AppendSettings appendSettings = new TXTextControl.AppendSettings();
                            //    //    //textControl_InstallText.Append(order.installation_comment, TXTextControl.StringStreamType.RichTextFormat, appendSettings);
                            //    //    //System.Threading.Thread.Sleep(5000);
                            //    try
                            //    {
                            //        textControl_InstallText.Load(order.installation_comment, TXTextControl.StringStreamType.RichTextFormat);
                            //    }
                            //    catch { }
                            //    //}
                            //}
                            textControl_InstallText.BackColor = SystemColors.Window;
                        }
                        else
                        {
                            e.Node.ImageIndex = 4; e.Node.SelectedImageIndex = 4;
                            //textBox_TextControlInactive.BringToFront();
                            textControl_InstallText.BackColor = SystemColors.Control;
                        }

                        break;
                }
                WorksList();
                if (e.Node.Checked)
                {
                    treeView_Menu.SelectedNode = e.Node;
                    treeView_Menu.Focus();
                }
                e.Node.Checked = nodeckecked;
                unlockcheck = true;
            }
            //if (e.Node.Checked) { treeView_Menu.SelectedNode = e.Node; }
        }
        #endregion

        #region Иконки для тривью | время завершения работы
        private void checkBox_PrintEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (groupBox_Print.Enabled)
            {
                if (checkBox_PrintEnd.Checked)
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].ImageIndex = 1;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].SelectedImageIndex = 1;
                    dateTimePicker_PrintEnded.Value = DateTime.Now;
                    dateTimePicker_PrintEnded.Enabled = true;
                    comboBox_PrintExecutor.Text = comboBox_Adder.Text;
                    //if (comboBox_Adder.Text.Trim() == string.Empty)
                    //{
                    //    comboBox_PrintExecutor.Text = Utils.Settings.set.name;
                    //}
                    //else { comboBox_PrintExecutor.Text = comboBox_Adder.Text; }
                }
                else
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].ImageIndex = 0;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].SelectedImageIndex = 0;
                    dateTimePicker_PrintEnded.Enabled = false;
                }
            }
            else
            {
                treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].ImageIndex = 4; treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].SelectedImageIndex = 4;
            }
        }
        private void checkBox_CutEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (groupBox_Cut.Enabled)
            {
                if (checkBox_CutEnd.Checked)
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].ImageIndex = 1;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].SelectedImageIndex = 1;
                    dateTimePicker_CutEnded.Value = DateTime.Now;
                    dateTimePicker_CutEnded.Enabled = true;
                    comboBox_CutExecutor.Text = comboBox_Adder.Text;
                    //if (comboBox_Adder.Text.Trim() == string.Empty)
                    //{
                    //    comboBox_CutExecutor.Text = Utils.Settings.set.name;
                    //}
                    //else { comboBox_CutExecutor.Text = comboBox_Adder.Text; }
                }
                else
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].ImageIndex = 0;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].SelectedImageIndex = 0;
                    dateTimePicker_CutEnded.Enabled = false;
                }
            }
            else
            {
                treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].ImageIndex = 4; treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].SelectedImageIndex = 4;
            }
        }

        private void checkBox_CncEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (groupBox_Cnc.Enabled)
            {
                if (checkBox_CncEnd.Checked)
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].ImageIndex = 1;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].SelectedImageIndex = 1;
                    dateTimePicker_CncEnded.Value = DateTime.Now;
                    dateTimePicker_CncEnded.Enabled = true;
                    comboBox_CncExecutor.Text = Utils.Settings.set.name;
                    //if (comboBox_Adder.Text.Trim() == string.Empty)
                    //{
                    //    comboBox_CncExecutor.Text = Utils.Settings.set.name;
                    //}
                    //else { comboBox_CncExecutor.Text = comboBox_Adder.Text; }
                }
                else
                {
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].ImageIndex = 0;
                    treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].SelectedImageIndex = 0;
                    dateTimePicker_CncEnded.Enabled = false;
                }
            }
            else
            {
                treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].ImageIndex = 4; treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].SelectedImageIndex = 4;
            }
        }

        #endregion

        #endregion

        private void ComboBox_Font_DropDownClosed(object sender, EventArgs e)
        {
            CurrentDocument.Focus();
        }


        #region Список работ
        private List<string> worklst = new List<string>();
        private void WorksList()
        {
            dataGridView_WorksList.Rows.Clear();
            if (treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].Checked) { dataGridView_WorksList.Rows.Add(0, "Широкоформатная печать"); }
            if (treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].Checked)
            {
                if (checkBox_CutOnPrint.Checked) { dataGridView_WorksList.Rows.Add(1, "Плоттерная резка по меткам"); }
                else
                {
                    dataGridView_WorksList.Rows.Add(2, "Плоттерная резка");
                }
            }
            if (radioButton_LaminMat.Checked) { dataGridView_WorksList.Rows.Add(3, "Матовая ламинация"); }
            if (radioButton_LaminGlose.Checked) { dataGridView_WorksList.Rows.Add(4, "Глянцевая ламинация"); }
            if (radioButton_BanerPoket.Checked) { dataGridView_WorksList.Rows.Add(5, "Обработка: карманы " + banerhandpock.ToString() + " см."); }
            if (radioButton_BanerLuvers.Checked) { dataGridView_WorksList.Rows.Add(6, "Обработка: люверсы через " + banerhandluv.ToString() + " см."); }

            if (treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].Checked)
            {
                if (checkBox_CncOnPrint.Checked)
                {
                    dataGridView_WorksList.Rows.Add(8, "Фрезеровка по меткам");
                }
                else { dataGridView_WorksList.Rows.Add(7, "Фрезеровка"); }
            }

            if (treeView_Menu.Nodes["nodemain"].Nodes["nodeinstall"].Checked) { dataGridView_WorksList.Rows.Add(9, "Монтажные работы"); }


            foreach (string item in worklst)
            {
                dataGridView_WorksList.Rows.Add(10, item);
            }
            if (radioButton_deliveryOffice.Checked) { dataGridView_WorksList.Rows.Add(11, "Доставка в офис: К. Либкнехта 27А, офис 705, тел.: 8 (815 2) 626292."); }
            if (radioButton_deliveryClient.Checked) { dataGridView_WorksList.Rows.Add(12, deliveryAddress); }
            //dataGridView_WorksList.Sort(this.dataGridView_WorksList.Columns[0], ListSortDirection.Ascending);
            RTFChangeEventON(ShowRTFInfo());
        }
        private void button_AddWorkTypes_Click(object sender, EventArgs e)
        {
            worklst.Add(comboBox_WorkTypes.Text);
            comboBox_WorkTypes.Text = string.Empty;
            WorksList();
        }

        #endregion

        #region Доставка - обработка GUI

        private string deliveryAddress = string.Empty;
        private string deliveryOfficeAddress = "К. Либкнехта 27А, офис 705, тел.: 8 (815 2) 626292.";
        private void radioButton_delivery_CheckedChanged(object sender, EventArgs e)
        {
            switch (radioButton_deliveryNo.Checked)
            {
                case true:
                    textBox_DeliveryAddress.Visible = false;
                    groupBox_Delivery.Height = 44;
                    break;
                default:
                    if (radioButton_deliveryOffice.Checked)
                    {
                        textBox_DeliveryAddress.Text = "Доставка в офис: " + deliveryOfficeAddress;
                        //textBox_DeliveryAddress.ReadOnly = true;
                    }
                    else
                    {
                        //if (deliveryAddress == string.Empty) { textBox_DeliveryAddress.Text = "Доставка для " + comboBox_Customer.Text + ": " + deliveryAddress; }
                        //else { textBox_DeliveryAddress.Text = deliveryAddress; }
                        textBox_DeliveryAddress.Text = "Доставка для " + comboBox_Customer.Text + ": " + deliveryAddress;
                        //textBox_DeliveryAddress.ReadOnly = false;
                    }
                    textBox_DeliveryAddress.SelectionStart = textBox_DeliveryAddress.Text.Length;
                    textBox_DeliveryAddress.SelectionLength = 0;
                    textBox_DeliveryAddress.Visible = true;
                    groupBox_Delivery.Height = 94;
                    textBox_DeliveryAddress.Focus();
                    break;
            }
            WorksList();
        }
        private void textBox_DeliveryAddress_TextChanged(object sender, EventArgs e)
        {
            if (radioButton_deliveryClient.Checked) { deliveryAddress = textBox_DeliveryAddress.Text.Replace("Доставка для " + comboBox_Customer.Text + ": ", string.Empty); }
            if (radioButton_deliveryOffice.Checked) { deliveryOfficeAddress = textBox_DeliveryAddress.Text.Replace("Доставка в офис: ", string.Empty); }
            WorksList();
        }

        #endregion

        #region Банера - обработка
        private decimal banerhandpock = 10;
        private decimal banerhandluv = 40;
        private void radioButton_Baner_CheckedChanged(object sender, EventArgs e)
        {
            switch (radioButton_Baner0.Checked)
            {
                case true:
                    numericUpDown_BanerHand.Visible = false;
                    label_Baner.Visible = false;
                    break;
                default:
                    if (radioButton_BanerPoket.Checked) { numericUpDown_BanerHand.Value = banerhandpock; } else { numericUpDown_BanerHand.Value = banerhandluv; }
                    numericUpDown_BanerHand.Visible = true;
                    label_Baner.Visible = true;
                    break;
            }
            WorksList();
        }
        private void numericUpDown_BanerHand_ValueChanged(object sender, EventArgs e)
        {
            if (radioButton_BanerPoket.Checked) { banerhandpock = numericUpDown_BanerHand.Value; } else { banerhandluv = numericUpDown_BanerHand.Value; }
            WorksList();

        }
        #endregion
        #region Ламинация
        private void radioButton_lamin_CheckedChanged(object sender, EventArgs e)
        {
            WorksList();
        }
        #endregion
        #region обработка контекстного меню списка работ (удаление)
        private void toolStripMenuItem_WorkList_Del_Click(object sender, EventArgs e)
        {

            if (toolStripMenuItem_name.Tag.ToString() == "10") { worklst.Remove(toolStripMenuItem_name.Text); }

            if (toolStripMenuItem_name.Tag.ToString() == "0") { treeView_Menu.Nodes[0].Nodes[0].Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "1") { checkBox_CutOnPrint.Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "2") { treeView_Menu.Nodes[0].Nodes[1].Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "3" || toolStripMenuItem_name.Tag.ToString() == "4") { radioButton_lamin0.Checked = true; }
            if (toolStripMenuItem_name.Tag.ToString() == "5" || toolStripMenuItem_name.Tag.ToString() == "6") { radioButton_Baner0.Checked = true; }
            if (toolStripMenuItem_name.Tag.ToString() == "7") { treeView_Menu.Nodes[0].Nodes[2].Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "8") { checkBox_CncOnPrint.Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "9") { treeView_Menu.Nodes["nodemain"].Nodes["nodeinstall"].Checked = false; }
            if (toolStripMenuItem_name.Tag.ToString() == "11" || toolStripMenuItem_name.Tag.ToString() == "12") { radioButton_deliveryNo.Checked = true; }
            WorksList();
        }
        private void dataGridView_WorksList_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            int rowIndexFromMouseDown = dgv.HitTest(e.X, e.Y).RowIndex;

            if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control) { dgv.ClearSelection(); }
            if (rowIndexFromMouseDown != -1 && e.Button == MouseButtons.Right) { dgv.Rows[rowIndexFromMouseDown].Selected = true; }

            if (e.Button == MouseButtons.Right)
            {
                if (dgv.SelectedRows.Count > 0)
                {

                    toolStripMenuItem_name.Text = dgv.SelectedRows[0].Cells[1].Value.ToString();
                    toolStripMenuItem_name.Tag = dgv.SelectedRows[0].Cells[0].Value.ToString();
                    contextMenu_worklist.Show(dgv, e.Location);
                }
            }
        }
        #endregion
        #region По меткам!
        private void checkBox_CutOnPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CutOnPrint.Checked) { treeView_Menu.Nodes[0].Nodes[1].Checked = true; treeView_Menu.SelectedNode = treeView_Menu.Nodes[0].Nodes[1]; }
            else
            {
                treeView_Menu.Nodes[0].Nodes[1].Checked = false;
            }
        }

        private void checkBox_CncOnPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CncOnPrint.Checked) { treeView_Menu.Nodes[0].Nodes[2].Checked = true; treeView_Menu.SelectedNode = treeView_Menu.Nodes[0].Nodes[2]; }
            else
            {
                treeView_Menu.Nodes[0].Nodes[2].Checked = false;
            }
        }
        #endregion

        #region Счётчики метража
        #region счётчики Фрезеровка
        private void CncLineLength_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_CncLineLengthTotal.Value = numericUpDown_CncLineLength.Value * numericUpDown_CncLineLengthCount.Value;
        }

        private void comboBox_CncUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_CncUnit.SelectedIndex)
            {
                case 0:
                    if (numericUpDown_CncWidth.DecimalPlaces == 1) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value * 10; }
                    if (numericUpDown_CncWidth.DecimalPlaces == 3) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value * 1000; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 1) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value * 10; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 3) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value * 1000; }
                    numericUpDown_CncWidth.DecimalPlaces = 0;
                    numericUpDown_CncHeight.DecimalPlaces = 0;
                    break;
                case 1:
                    if (numericUpDown_CncWidth.DecimalPlaces == 0) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value / 10; }
                    if (numericUpDown_CncWidth.DecimalPlaces == 3) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value * 100; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 0) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value / 10; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 3) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value * 100; }
                    numericUpDown_CncWidth.DecimalPlaces = 1;
                    numericUpDown_CncHeight.DecimalPlaces = 1;
                    break;
                case 2:
                    if (numericUpDown_CncWidth.DecimalPlaces == 0) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value / 1000; }
                    if (numericUpDown_CncWidth.DecimalPlaces == 1) { numericUpDown_CncWidth.Value = numericUpDown_CncWidth.Value / 100; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 0) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value / 1000; }
                    if (numericUpDown_CncHeight.DecimalPlaces == 1) { numericUpDown_CncHeight.Value = numericUpDown_CncHeight.Value / 100; }
                    numericUpDown_CncWidth.DecimalPlaces = 3;
                    numericUpDown_CncHeight.DecimalPlaces = 3;
                    break;
            }
        }

        private void CncSquare_ValueChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_CncUnit.SelectedIndex)
            {
                case 0:
                    x = numericUpDown_CncWidth.Value / 1000;
                    y = numericUpDown_CncHeight.Value / 1000;
                    break;
                case 1:
                    x = numericUpDown_CncWidth.Value / 100;
                    y = numericUpDown_CncHeight.Value / 100;
                    break;
                case 2:
                    x = numericUpDown_CncWidth.Value;
                    y = numericUpDown_CncHeight.Value;
                    break;
            }
            numericUpDown_CncSquare.Value = x * y * numericUpDown_CncCopy.Value;
        }

        private void CncFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_CncFormat.Text)
            {
                case "A0":
                    x = 841;
                    y = 1189;
                    break;
                case "A1":
                    x = 594;
                    y = 841;
                    break;
                case "A2":
                    x = 420;
                    y = 594;
                    break;
                case "A3":
                    x = 297;
                    y = 420;
                    break;
                case "A4":
                    x = 210;
                    y = 297;
                    break;
                case "A5":
                    x = 148;
                    y = 210;
                    break;
                case "A6":
                    x = 105;
                    y = 148;
                    break;
            }
            switch (comboBox_CncUnit.SelectedIndex)
            {
                case 0:
                    numericUpDown_CncWidth.Value = x;
                    numericUpDown_CncHeight.Value = y;
                    break;
                case 1:
                    numericUpDown_CncWidth.Value = x / 10;
                    numericUpDown_CncHeight.Value = y / 10;
                    break;
                case 2:
                    numericUpDown_CncWidth.Value = x / 1000;
                    numericUpDown_CncHeight.Value = y / 1000;
                    break;
            }
        }
        #endregion
        #region счетчики плот. резка
        private void CutLineLength_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_CutLineLengthTotal.Value = numericUpDown_CutLineLength.Value * numericUpDown_CutLineLengthCount.Value;
        }

        private void comboBox_CutUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_CutUnit.SelectedIndex)
            {
                case 0:
                    if (numericUpDown_CutWidth.DecimalPlaces == 1) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value * 10; }
                    if (numericUpDown_CutWidth.DecimalPlaces == 3) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value * 1000; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 1) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value * 10; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 3) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value * 1000; }
                    numericUpDown_CutWidth.DecimalPlaces = 0;
                    numericUpDown_CutHeight.DecimalPlaces = 0;
                    break;
                case 1:
                    if (numericUpDown_CutWidth.DecimalPlaces == 0) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value / 10; }
                    if (numericUpDown_CutWidth.DecimalPlaces == 3) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value * 100; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 0) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value / 10; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 3) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value * 100; }
                    numericUpDown_CutWidth.DecimalPlaces = 1;
                    numericUpDown_CutHeight.DecimalPlaces = 1;
                    break;
                case 2:
                    if (numericUpDown_CutWidth.DecimalPlaces == 0) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value / 1000; }
                    if (numericUpDown_CutWidth.DecimalPlaces == 1) { numericUpDown_CutWidth.Value = numericUpDown_CutWidth.Value / 100; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 0) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value / 1000; }
                    if (numericUpDown_CutHeight.DecimalPlaces == 1) { numericUpDown_CutHeight.Value = numericUpDown_CutHeight.Value / 100; }
                    numericUpDown_CutWidth.DecimalPlaces = 3;
                    numericUpDown_CutHeight.DecimalPlaces = 3;
                    break;
            }
        }

        private void CutSquare_ValueChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_CutUnit.SelectedIndex)
            {
                case 0:
                    x = numericUpDown_CutWidth.Value / 1000;
                    y = numericUpDown_CutHeight.Value / 1000;
                    break;
                case 1:
                    x = numericUpDown_CutWidth.Value / 100;
                    y = numericUpDown_CutHeight.Value / 100;
                    break;
                case 2:
                    x = numericUpDown_CutWidth.Value;
                    y = numericUpDown_CutHeight.Value;
                    break;
            }
            numericUpDown_CutSquare.Value = x * y * numericUpDown_CutCopy.Value;
        }

        private void CutFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_CutFormat.Text)
            {
                case "A0":
                    x = 841;
                    y = 1189;
                    break;
                case "A1":
                    x = 594;
                    y = 841;
                    break;
                case "A2":
                    x = 420;
                    y = 594;
                    break;
                case "A3":
                    x = 297;
                    y = 420;
                    break;
                case "A4":
                    x = 210;
                    y = 297;
                    break;
                case "A5":
                    x = 148;
                    y = 210;
                    break;
                case "A6":
                    x = 105;
                    y = 148;
                    break;
            }
            switch (comboBox_CutUnit.SelectedIndex)
            {
                case 0:
                    numericUpDown_CutWidth.Value = x;
                    numericUpDown_CutHeight.Value = y;
                    break;
                case 1:
                    numericUpDown_CutWidth.Value = x / 10;
                    numericUpDown_CutHeight.Value = y / 10;
                    break;
                case 2:
                    numericUpDown_CutWidth.Value = x / 1000;
                    numericUpDown_CutHeight.Value = y / 1000;
                    break;
            }
        }

        #endregion
        #region счётчики Печать
        private void comboBox_PrintUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_PrintUnit.SelectedIndex)
            {
                case 0:
                    if (numericUpDown_PrintWidth.DecimalPlaces == 1) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value * 10; }
                    if (numericUpDown_PrintWidth.DecimalPlaces == 3) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value * 1000; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 1) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value * 10; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 3) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value * 1000; }
                    numericUpDown_PrintWidth.DecimalPlaces = 0;
                    numericUpDown_PrintHeight.DecimalPlaces = 0;
                    break;
                case 1:
                    if (numericUpDown_PrintWidth.DecimalPlaces == 0) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value / 10; }
                    if (numericUpDown_PrintWidth.DecimalPlaces == 3) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value * 100; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 0) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value / 10; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 3) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value * 100; }
                    numericUpDown_PrintWidth.DecimalPlaces = 1;
                    numericUpDown_PrintHeight.DecimalPlaces = 1;
                    break;
                case 2:
                    if (numericUpDown_PrintWidth.DecimalPlaces == 0) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value / 1000; }
                    if (numericUpDown_PrintWidth.DecimalPlaces == 1) { numericUpDown_PrintWidth.Value = numericUpDown_PrintWidth.Value / 100; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 0) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value / 1000; }
                    if (numericUpDown_PrintHeight.DecimalPlaces == 1) { numericUpDown_PrintHeight.Value = numericUpDown_PrintHeight.Value / 100; }
                    numericUpDown_PrintWidth.DecimalPlaces = 3;
                    numericUpDown_PrintHeight.DecimalPlaces = 3;
                    break;
            }
        }

        private void PrintSquare_ValueChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_PrintUnit.SelectedIndex)
            {
                case 0:
                    x = numericUpDown_PrintWidth.Value / 1000;
                    y = numericUpDown_PrintHeight.Value / 1000;
                    break;
                case 1:
                    x = numericUpDown_PrintWidth.Value / 100;
                    y = numericUpDown_PrintHeight.Value / 100;
                    break;
                case 2:
                    x = numericUpDown_PrintWidth.Value;
                    y = numericUpDown_PrintHeight.Value;
                    break;
            }
            numericUpDown_PrintSquare.Value = x * y * numericUpDown_PrintCopy.Value;
        }

        private void PrintFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal x = 0; decimal y = 0;
            switch (comboBox_PrintFormat.Text)
            {
                case "A0":
                    x = 841;
                    y = 1189;
                    break;
                case "A1":
                    x = 594;
                    y = 841;
                    break;
                case "A2":
                    x = 420;
                    y = 594;
                    break;
                case "A3":
                    x = 297;
                    y = 420;
                    break;
                case "A4":
                    x = 210;
                    y = 297;
                    break;
                case "A5":
                    x = 148;
                    y = 210;
                    break;
                case "A6":
                    x = 105;
                    y = 148;
                    break;
            }
            switch (comboBox_PrintUnit.SelectedIndex)
            {
                case 0:
                    numericUpDown_PrintWidth.Value = x;
                    numericUpDown_PrintHeight.Value = y;
                    break;
                case 1:
                    numericUpDown_PrintWidth.Value = x / 10;
                    numericUpDown_PrintHeight.Value = y / 10;
                    break;
                case 2:
                    numericUpDown_PrintWidth.Value = x / 1000;
                    numericUpDown_PrintHeight.Value = y / 1000;
                    break;
            }
        }
        #endregion

        #endregion

        #region Раздел - сохранение заявки
        private void Save_Click(object sender, EventArgs e)
        {

            if (newOrder)
            {
                Program.SendOrderToServer(new Program.OrderSendEventArgs(FromFormToProto(), null, null, SocketClient.TableClient.SocketMessageCommand.RowsInsert, SocketClient.TableClient.TableName.TableBase, "Новое задание"));
            }
            else
            {
                Program.SendOrderToServer(new Program.OrderSendEventArgs(FromFormToProto(), null, null, SocketClient.TableClient.SocketMessageCommand.RowsUpdate, SocketClient.TableClient.TableName.TableBase, "Изменение задания № " + order.id.ToString()));
            }

            Close();

            //Exception ex = null;
            //if (newOrder) { ex = SqlLite.SendChangeToSocket.Insert(order, SocketClient.TableClient.SocketMessageCommand.RowsInsert, SocketClient.TableClient.TableName.TableBase, "Новое задание"); } else { ex = SqlLite.SendChangeToSocket.Insert(order, SocketClient.TableClient.SocketMessageCommand.RowsUpdate, SocketClient.TableClient.TableName.TableBase, "Изменение задания № " + order.id.ToString()); }
            //if (ex == null)
            //{ Close(); }
            //else
            //{
            //    MessageBox.Show(
            //        ex.Message,
            //        ex.ToString(),
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error,
            //        MessageBoxDefaultButton.Button1,
            //        MessageBoxOptions.DefaultDesktopOnly);
            //}
        }
        #endregion

        private ProtoClasses.ProtoOrders.protoOrder FromFormToProto()
        {

            ProtoClasses.ProtoOrders.protoOrder protoOrder = new ProtoClasses.ProtoOrders.protoOrder
            {
                id = order.id
            };
            if (comboBox_Adder.Text.Trim() == string.Empty)
            {
                protoOrder.adder = Utils.Settings.set.name;
            }
            else
            {
                protoOrder.adder = comboBox_Adder.Text;
            }
            protoOrder.client = comboBox_Customer.Text.ToString();
            protoOrder.cncman = comboBox_CncExecutor.Text.ToString();
            protoOrder.cncs_id = ((KeyValuePair<int, string>)comboBox_EquipCnc.SelectedItem).Key;
            protoOrder.cnc_on_print = checkBox_CncOnPrint.Checked;
            protoOrder.cnc_on = treeView_Menu.Nodes["nodemain"].Nodes["nodecnc"].Checked;

            protoOrder.comments = textBox_Comment.Text.ToString();



            #region размеры/количество
            #region количество
            protoOrder.count_cnc = Convert.ToInt32(numericUpDown_CncCopy.Value);
            protoOrder.count_cut = Convert.ToInt32(numericUpDown_CutCopy.Value);
            protoOrder.count_print = Convert.ToInt32(numericUpDown_PrintCopy.Value);

            protoOrder.count_size_cnc = Convert.ToInt32(numericUpDown_CncLineLengthCount.Value);
            protoOrder.count_size_cut = Convert.ToInt32(numericUpDown_CutLineLengthCount.Value);
            #endregion
            #region всего длина реза
            protoOrder.size_cnc = Convert.ToDouble(numericUpDown_CncLineLengthTotal.Value);
            protoOrder.size_cut = Convert.ToDouble(numericUpDown_CutLineLengthTotal.Value);
            #endregion
            #region ширина/длина
            protoOrder.size_x_cnc = AlltoMeter(numericUpDown_CncWidth.Value, comboBox_CncUnit.SelectedIndex);
            protoOrder.size_x_cut = AlltoMeter(numericUpDown_CutWidth.Value, comboBox_CutUnit.SelectedIndex);
            protoOrder.size_x_print = AlltoMeter(numericUpDown_PrintWidth.Value, comboBox_PrintUnit.SelectedIndex);
            protoOrder.size_y_cnc = AlltoMeter(numericUpDown_CncHeight.Value, comboBox_CncUnit.SelectedIndex);
            protoOrder.size_y_cut = AlltoMeter(numericUpDown_CutHeight.Value, comboBox_CutUnit.SelectedIndex);
            protoOrder.size_y_print = AlltoMeter(numericUpDown_PrintHeight.Value, comboBox_PrintUnit.SelectedIndex);
            #endregion
            #region длина реза
            protoOrder.line_size_cnc = Convert.ToDouble(numericUpDown_CncLineLength.Value);
            protoOrder.line_size_cut = Convert.ToDouble(numericUpDown_CutLineLength.Value);
            #endregion
            #region квадратура
            protoOrder.square_cnc = Convert.ToDouble(numericUpDown_CncSquare.Value);
            protoOrder.square_cut = Convert.ToDouble(numericUpDown_CutSquare.Value);
            protoOrder.square_print = Convert.ToDouble(numericUpDown_PrintSquare.Value);
            #endregion
            #endregion

            protoOrder.cutterman = comboBox_CutExecutor.Text;
            protoOrder.cutters_id = ((KeyValuePair<int, string>)comboBox_EquipCut.SelectedItem).Key;
            protoOrder.cutting_on_print = checkBox_CutOnPrint.Checked;
            protoOrder.cut_on = treeView_Menu.Nodes["nodemain"].Nodes["nodecut"].Checked;
            if (checkBox_CncEnd.Checked) { protoOrder.date_ready_cnc = Utils.UnixDate.DateTimeToInt64(dateTimePicker_CncEnded.Value); } else { protoOrder.date_ready_cnc = 0; }
            if (checkBox_CutEnd.Checked) { protoOrder.date_ready_cut = Utils.UnixDate.DateTimeToInt64(dateTimePicker_CutEnded.Value); } else { protoOrder.date_ready_cut = 0; }
            if (checkBox_PrintEnd.Checked) { protoOrder.date_ready_print = Utils.UnixDate.DateTimeToInt64(dateTimePicker_PrintEnded.Value); } else { protoOrder.date_ready_print = 0; }
            protoOrder.date_start = Utils.UnixDate.DateTimeToInt64(dateTimePicker_DateStart.Value);
            protoOrder.dead_line = Utils.UnixDate.DateTimeToInt64(dateTimePicker_DeadLine.Value);
            protoOrder.installation = treeView_Menu.Nodes["nodemain"].Nodes["nodeinstall"].Checked;
            if (radioButton_lamin0.Checked) { protoOrder.laminat = false; } else { protoOrder.laminat = true; if (radioButton_LaminMat.Checked) { protoOrder.laminat_mat = true; } else { protoOrder.laminat_mat = false; } }
            if (radioButton_Baner0.Checked) { protoOrder.baner_handling = false; } else { protoOrder.baner_handling = true; protoOrder.baner_handling_size = Convert.ToDouble(numericUpDown_BanerHand.Value); if (radioButton_BanerLuvers.Checked) { protoOrder.baner_luvers = true; } else { protoOrder.baner_luvers = false; } }
            if (radioButton_deliveryNo.Checked) { protoOrder.delivery = false; } else { protoOrder.delivery = true; if (radioButton_deliveryOffice.Checked) { protoOrder.delivery_office = true; protoOrder.delivery_address = deliveryOfficeAddress; } else { protoOrder.delivery_office = false; protoOrder.delivery_address = deliveryAddress; } }

            protoOrder.material_cnc_id = ((KeyValuePair<int, string>)comboBox_MaterialCnc.SelectedItem).Key;
            protoOrder.material_cut_id = ((KeyValuePair<int, string>)comboBox_MaterialCut.SelectedItem).Key;
            protoOrder.material_print_id = ((KeyValuePair<int, string>)comboBox_MaterialPrint.SelectedItem).Key;


            if (PreviewChanged) { protoOrder.preview = pictureBoxPreview.PreviewBinary; } else { protoOrder.preview = null; }

            protoOrder.printerman = comboBox_PrintExecutor.Text;
            protoOrder.printers_id = ((KeyValuePair<int, string>)comboBox_EquipPrint.SelectedItem).Key;
            protoOrder.print_on = treeView_Menu.Nodes["nodemain"].Nodes["nodeprint"].Checked;
            protoOrder.print_quality = comboBox_PrintQuality.Text;

            protoOrder.state = comboBox_OrderState.SelectedIndex;
            protoOrder.state_cnc = checkBox_CncEnd.Checked;
            protoOrder.state_cut = checkBox_CutEnd.Checked;
            protoOrder.state_print = checkBox_PrintEnd.Checked;
            if (textBox_OrderName.Text.Trim() == string.Empty)
            {
                protoOrder.work_name = "Без наименования";
            }
            else
            {
                protoOrder.work_name = textBox_OrderName.Text;
            }
            protoOrder.sender_row_stringid = Utils.Settings.UniqueId;
            protoOrder.FilesUpload = filesControl1.GetNewFtp;
            //textControl1.D;
            if (textControl_InstallText.Text != string.Empty)
            {
                textControl_InstallText.Save(out byte[] montage, TXTextControl.BinaryStreamType.MSWord);
                if (montage != null && montage.Length > 0)
                {
                    protoOrder.installation_comment = new byte[montage.Length];
                    Buffer.BlockCopy(montage, 0, protoOrder.installation_comment, 0, montage.Length);
                }
            }
            StringBuilder builder = new StringBuilder();
            foreach (string dataRow in worklst)
            {
                builder.Append(dataRow);
                builder.Append((char)(219));
            }
            protoOrder.worktypes_list = builder.ToString();
            protoOrder.HistoryRows = newordershistoryrows;
            protoOrder.DeleteFilesList = filesControl1.GetDeleteFilesList;
            return protoOrder;
        }


        #region Утилита перевода в метры и обратно
        private double AlltoMeter(decimal value, int units)
        {
            switch (units)
            {
                case 0: return Convert.ToDouble(value / 1000);
                case 1: return Convert.ToDouble(value / 100);
                default: return Convert.ToDouble(value);
            }
        }
        private decimal AllfromMeter(double value, int units)
        {
            switch (units)
            {
                case 0: return Convert.ToDecimal(value * 1000);
                case 1: return Convert.ToDecimal(value * 100);
                default: return Convert.ToDecimal(value);
            }
        }

        #endregion

        #region фикс для numericUpDown - точка/запятая
        private void numericUpDown_Size_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('.') || e.KeyChar.Equals(','))
            {
                e.KeyChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
            }
        }
        #endregion

        private void FormBaseEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketClient.TableClient.FilelistCome -= FileListRecieve;
        }

        private void FormBaseEdit_Activated(object sender, EventArgs e)
        {
            Program.SetWindowPos(Handle, Program.HWND_NOTOPMOST, 0, 0, 0, 0, Program.SWP_NOMOVE | Program.SWP_NOSIZE);

            //this.TopMost = true;
            //this.TopMost = false;
            //BringToFront();

            //this.TopMost = true;
            //this.Focus();
            //this.BringToFront();
            //this.TopMost = false;
        }

        #region Кнопки меню - события
        private void MenuOrder_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void MenuOrder_OrderPrint_Click(object sender, EventArgs e)
        {
            printDialog1.AllowSomePages = true;
            printDialog1.ShowHelp = true;
            printDialog1.Document = printDocument1;
            TopLevelControl.Focus();
            DialogResult result = printDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                List<byte[]> imgs = Utils.OrderRasterize.CreateImage_FromProtoOrder(FromFormToProto());
                if (imgs != null && imgs.Count > 0)
                {
                    foreach (byte[] b in imgs)
                    {
                        printDocument1.PrintPage += (sendr, args) =>
                        {
                            Image i = Utils.Converting.ByteToImage(b);
                            Rectangle m = args.PageBounds;

                            if (i.Width / (double)i.Height > m.Width / (double)m.Height) // image is wider
                            {
                                m.Height = (int)(i.Height / (double)i.Width * m.Width);
                            }
                            else
                            {
                                m.Width = (int)(i.Width / (double)i.Height * m.Height);
                            }
                            //args.Graphics.DrawImage(i, m);
                            ////
                            //float newWidth = i.Width * 100 / 360;
                            //float newHeight = i.Height * 100 / 360;

                            float newWidth = (i.Width * 100) / 360;
                            float newHeight = (i.Height * 100) / 360;

                            float widthFactor = newWidth / args.PageBounds.Width; //(e.MarginBounds.Width+e.MarginBounds.X + e.MarginBounds.);
                            float heightFactor = newHeight / args.PageBounds.Height; //(e.MarginBounds.Height+e.MarginBounds.Y);

                            if (widthFactor > 1 | heightFactor > 1)
                            {
                                if (widthFactor > heightFactor)
                                {
                                    newWidth = newWidth / widthFactor;
                                    newHeight = newHeight / widthFactor;
                                }
                                else
                                {
                                    newWidth = newWidth / heightFactor;
                                    newHeight = newHeight / heightFactor;
                                }
                            }
                            args.Graphics.DrawImage(i, 0, 0, (int)newWidth, (int)newHeight);
                            i.Dispose();
                        };
                        printDocument1.Print();

                    }
                }
            }
        }

        private void MenuOrder_PDFSave_Click(object sender, EventArgs e)
        {

        }

        private void OrderNew_Click(object sender, EventArgs e)
        {
            if (formBase != null)
            {
                formBase.NewOrder_Click(sender, e);
            }
        }

        #endregion

        private void comboBox_WorkTypes_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_WorkTypes.Text.Trim() == string.Empty)
            {
                button_AddWorkTypes.Enabled = false;
            }
            else { button_AddWorkTypes.Enabled = true; }
        }

        private void comboBox_Customer_TextChanged(object sender, EventArgs e)
        {
            if (radioButton_deliveryClient.Checked) { textBox_DeliveryAddress.Text = "Доставка для " + comboBox_Customer.Text + ": " + deliveryAddress; }
        }
























        private void Button_Calc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void groupBox_Print_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = splitContainer1.Panel2.Width;
            pictureBox1.Height = groupBox_Print.Height - 194;
            pictureBox1.Location = new Point(splitContainer1.Location.X + splitContainer1.SplitterDistance + 5, pictureBox1.Location.Y);
        }

        private void groupBox_Cut_Resize(object sender, EventArgs e)
        {
            pictureBox2.Width = splitContainer_Cut.Panel1.Width;
            pictureBox2.Height = groupBox_Cut.Height - 172;
            //pictureBox2.Location = new Point(splitContainer1.Location.X + splitContainer1.SplitterDistance + 5, pictureBox1.Location.Y);
        }

        private void groupBox_Cnc_Resize(object sender, EventArgs e)
        {
            pictureBox3.Width = splitContainer_Cnc.Panel1.Width;
            pictureBox3.Height = groupBox_Cnc.Height - 168;
        }

        //Добавить монтажное задание
        private void MenuBlank_AddInstall_Click(object sender, EventArgs e)
        {
            treeView_Menu.Nodes[0].Nodes["nodeinstall"].Checked = true; treeView_Menu.SelectedNode = treeView_Menu.Nodes[0].Nodes["nodeinstall"];

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = Utils.Settings.set.SavePath;
                //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.Filter = "Файлы документов (*.doc,*.rtf,*.pdf,*.txt)| *.doc; *.rtf; *.pdf; *.txt";
                openFileDialog1.FilterIndex = 0;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(openFileDialog1.FileName);
                    Utils.Settings.set.SavePath = fi.DirectoryName;
                    Utils.Settings.Save();

                    try
                    {
                        TXTextControl.LoadSettings ls = new TXTextControl.LoadSettings();
                        switch (fi.Extension)
                        {
                            case ".doc":
                                textControl_InstallText.Load(fi.FullName, TXTextControl.StreamType.MSWord, ls);
                                break;
                            case ".rtf":
                                textControl_InstallText.Load(fi.FullName, TXTextControl.StreamType.RichTextFormat, ls);
                                break;
                            case ".pdf":
                                textControl_InstallText.Load(fi.FullName, TXTextControl.StreamType.AdobePDF, ls);
                                break;
                            case ".txt":
                                textControl_InstallText.Load(fi.FullName, TXTextControl.StreamType.PlainText, ls);
                                break;
                                //default:
                                //    textControl_InstallText.Load(fi.FullName, TXTextControl.StreamType.MSWord, ls);
                                //    break;
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка!", ex.Message);
                    }
                    //ls.ApplicationFieldFormat = TXTextControl.ApplicationFieldFormat.

                    //tx.Save(out data, TXTextControl.BinaryStreamType.InternalUnicodeFormat);

                    //data = ProcessCheckboxFields(data);

                }
            }
        }

        private void Button_InstallSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog1 = new SaveFileDialog())
            {
                fileDialog1.InitialDirectory = Utils.Settings.set.SavePath;
                fileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                fileDialog1.FilterIndex = 2;
                fileDialog1.RestoreDirectory = true;
                if (fileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileInfo fi = new FileInfo(fileDialog1.FileName);
                    Utils.Settings.set.SavePath = fi.DirectoryName;
                    Utils.Settings.Save();
                    textControl_InstallText.Save(fi.FullName, TXTextControl.StreamType.MSWord);

                }
            }
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    switch (comboBox1.SelectedIndex)
        //    {
        //        case 0:
        //            textControl_InstallText.ViewMode = TXTextControl.ViewMode.FloatingText;
        //            break;
        //        case 1:
        //            textControl_InstallText.ViewMode = TXTextControl.ViewMode.Normal;
        //            break;
        //        case 2:
        //            textControl_InstallText.ViewMode = TXTextControl.ViewMode.PageView;
        //            break;
        //        case 3:
        //            textControl_InstallText.ViewMode = TXTextControl.ViewMode.SimpleControl;
        //            break;
        //    }


        //}

        private void FormBaseEdit_SizeChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Normal:
                    textControl_InstallText.ViewMode = TXTextControl.ViewMode.Normal;
                    break;
                case FormWindowState.Maximized:
                    textControl_InstallText.ViewMode = TXTextControl.ViewMode.PageView;
                    break;
            }
        }

        private void Button_PDFSave_Click(object sender, EventArgs e)
        {
            byte[] imgs = Utils.OrderRasterize.CreatePDF_FromProtoOrder(FromFormToProto());
            if (imgs != null && imgs.Length > 0)
            {

                SaveFileDialog sfdlg = new SaveFileDialog
                {
                    FileName = "Заявка N" + order.id.ToString() + "_" + Utils.FileNamesValidation.GetValidPath(textBox_OrderName.Text) + ".pdf",
                    AddExtension = true,
                    DefaultExt = "pdf",
                    Filter = "PDF-документ(*.pdf)|*.*",
                    RestoreDirectory = true
                };
                if (sfdlg.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllBytes(sfdlg.FileName, imgs.ToArray());
                }
            }
        }

        private void Button_InstallPrint_Click(object sender, EventArgs e)
        {
            treeView_Menu.Nodes[0].Nodes["nodeinstall"].Checked = true; treeView_Menu.SelectedNode = treeView_Menu.Nodes[0].Nodes["nodeinstall"];
            PrintDialog myPrintDialog = new PrintDialog();
            PrintDocument myPrintDocument = new PrintDocument();
            myPrintDialog.Document = myPrintDocument;
            myPrintDialog.AllowSomePages = false;
            myPrintDialog.AllowPrintToFile = false;
            myPrintDialog.PrinterSettings.FromPage = 0;
            myPrintDialog.PrinterSettings.ToPage = textControl_InstallText.Pages;
            myPrintDialog.UseEXDialog = true;

            if (myPrintDialog.ShowDialog() == DialogResult.OK)
            {
                textControl_InstallText.Print(myPrintDocument);
            }
        }

        private void textBox_OrderName_Enter(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 1500;  //in milliseconds
            toolTip1.Show("Запрещены служебные символы", TB, 0, TB.Height + 2, VisibleTime);

        }

        private void MenuOrder_DropDownOpening(object sender, EventArgs e)
        {
            if (newOrder) { MenuOrder_Download.Enabled = false; } else { MenuOrder_Download.Enabled = true; }
            //if (textControl_InstallText.Pages > 0)
            //{
            //    if (InstallEmptyALLPages())
            //    {
            //        MenuOrder_InstallPrint.Enabled = false; MenuBlank_InstallSave.Enabled = false;
            //    }
            //    else { MenuOrder_InstallPrint.Enabled = true; MenuBlank_InstallSave.Enabled = true; }
            //}
            //else { MenuOrder_InstallPrint.Enabled = false; MenuBlank_InstallSave.Enabled = false; }


        }

        private void MenuOrder_Download_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (Utils.Settings.set.SavePath != string.Empty) { fbd.SelectedPath = Utils.Settings.set.SavePath; }
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Utils.Settings.set.SavePath = fbd.SelectedPath;
                    Utils.Settings.Save();
                    ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList sendingList = new ProtoClasses.ProtoDownloadOrdersFiles.protoRowsList
                    {
                        sender_row_stringid = Utils.Settings.UniqueId,
                        plist = new List<ProtoClasses.ProtoDownloadOrdersFiles.protoRow>()
                    };
                    long i = order.id;
                    DirectoryInfo dirname = new DirectoryInfo(fbd.SelectedPath + @"\Заявка N" + i.ToString("000000") + "_" + Utils.FileNamesValidation.GetValidPath(SqlLite.Order.OrdersDic[i].work_name));
                    try
                    {
                        if (!Directory.Exists(dirname.FullName)) { Directory.CreateDirectory(dirname.FullName); }
                        byte[] imgs = Utils.OrderRasterize.CreatePDF_FromBase(i); if (imgs != null && imgs.Length > 0) { System.IO.File.WriteAllBytes(dirname.FullName + @"\" + dirname.Name + ".pdf", imgs.ToArray()); }
                        DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(order.time_recieve);
                        if (tmpenddate != null)
                        {
                            string sourcePreview = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString() + @"\index.png";
                            if (System.IO.File.Exists(sourcePreview)) { File.Copy(sourcePreview, dirname + @"\preview.png", true); }
                            string sourceMontage = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + i.ToString() + @"\montage.doc";
                            if (System.IO.File.Exists(sourceMontage)) { File.Copy(sourceMontage, dirname + @"\montage.doc", true); }
                        }
                        ProtoClasses.ProtoDownloadOrdersFiles.protoRow pr = new ProtoClasses.ProtoDownloadOrdersFiles.protoRow
                        {
                            uid = i,
                            LocalPath = dirname.FullName,
                            Preview = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                            Makets = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                            Photoreport = new List<ProtoClasses.ProtoFtpSchedule.protoRow>(),
                            Documents = new List<ProtoClasses.ProtoFtpSchedule.protoRow>()
                        };
                        sendingList.plist.Add(pr);

                        Program.SendOrderToServer(new Program.OrderSendEventArgs(null, null, sendingList, SocketClient.TableClient.SocketMessageCommand.DownloadOrderFiles, SocketClient.TableClient.TableName.TableBase, "Запрос на скачивание файлов заявки № " + sendingList.plist[0].uid.ToString()));
                    }
                    catch (Exception ex) { MessageBox.Show("FormBaseEdit:MenuOrder_Download_Click" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    sendingList.sender_row_stringid = Utils.Settings.UniqueId;
                }
            }
        }

        private void MenuFiles_AddPreview_Click(object sender, EventArgs e)
        {
            pictureBoxPreview.SelectImageFromFile();
        }

        private void MenuFiles_OperationsOfFilesOfFolder_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem snd = sender as ToolStripMenuItem;
            if (snd!= null) {
                switch (snd.Name)
                {
                    case "MenuFiles_DownloadPreview":
                        filesControl1.DownloadsFilesFromMainFolder( CustomControls.FilesControl.MainFolderName.Preview);
                        break;
                    case "MenuFiles_DownloadMakets":
                        filesControl1.DownloadsFilesFromMainFolder(CustomControls.FilesControl.MainFolderName.Originals);
                        break;
                    case "MenuFiles_DownloadDoc":
                        filesControl1.DownloadsFilesFromMainFolder(CustomControls.FilesControl.MainFolderName.Documents);
                        break;
                    case "MenuFiles_DownloadReport":
                        filesControl1.DownloadsFilesFromMainFolder(CustomControls.FilesControl.MainFolderName.PhotoReport);
                        break;
                    case "MenuFiles_UploadPreview":
                        filesControl1.UploadFilesToMainFolder(CustomControls.FilesControl.MainFolderName.Preview);
                        break;
                    case "MenuFiles_UploadMakets":
                        filesControl1.UploadFilesToMainFolder(CustomControls.FilesControl.MainFolderName.Originals);
                        break;
                    case "MenuFiles_UploadDoc":
                        filesControl1.UploadFilesToMainFolder(CustomControls.FilesControl.MainFolderName.Documents);
                        break;
                    case "MenuFiles_UploadReport":
                        filesControl1.UploadFilesToMainFolder(CustomControls.FilesControl.MainFolderName.PhotoReport);
                        break;
                }

}
        }

        private void Button_UploadMakets_Click(object sender, EventArgs e)
        {
            filesControl1.UploadFilesToMainFolder(CustomControls.FilesControl.MainFolderName.Originals);
        }

        private void Button_DownloadMakets_Click(object sender, EventArgs e)
        {
            filesControl1.DownloadsFilesFromMainFolder(CustomControls.FilesControl.MainFolderName.Originals);
        }

        private void MenuFiles_DownloadAll_Click(object sender, EventArgs e)
        {
            filesControl1.DownloadsAllFilesToOneFolder();
        }

        //private bool InstallEmptyALLPages()
        //{
        //    //bool blankALLPage = true;
        //    //var qqq = textControl_InstallText.GetPages();
        //    //if (textControl_InstallText.GetPages() != null)
        //    //{
        //    //    foreach (TXTextControl.Page page in textControl_InstallText.GetPages()) { if (page.Length > 1) { blankALLPage = false; } }
        //    //}

        //    ////TXTextControl.PageCollection.PageEnumerator pageEnum = textControl_InstallText.GetPages().GetEnumerator();
        //    ////pageEnum.MoveNext();
        //    ////int pageCounter = textControl_InstallText.GetPages().Count;
        //    ////for (int i = 0; i < pageCounter; i++)
        //    ////{
        //    ////    TXTextControl.Page curPage = (TXTextControl.Page)pageEnum.Current;

        //    ////    if (curPage.Length > 1)
        //    ////    {
        //    ////        blankALLPage = false;
        //    ////        pageEnum.MoveNext();
        //    ////    }
        //    ////}
        //    //return blankALLPage;
        //}
    }
}
