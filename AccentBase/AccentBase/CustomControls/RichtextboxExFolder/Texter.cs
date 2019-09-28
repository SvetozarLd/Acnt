using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
namespace AccentBase.CustomControls.RichtextboxExFolder
{
    public partial class Texter : UserControl
    {
        #region General

        int SelectedFont = 0;
        FontStyle fontStyle = FontStyle.Regular;
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
        public Texter()
        {
            InitializeComponent();
        }

        private void Texter_Load(object sender, EventArgs e)
        {
            GetFontCollection();
            PopulateFontSizes();
        }

        private void ComboBox_Font_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (CurrentDocument == null) { return; }
            //try
            //{
            //    Font NewFont = new Font(ComboBox_Font.SelectedItem.ToString(), CurrentDocument.SelectionFont.Size, FontStyle.Bold);
            //}
            //catch { }
            FontStyle fs = fontStyle;
            FontFamily myFontFamily = new FontFamily(ComboBox_Font.SelectedItem.ToString());
            //bool regularfont = false;
            if (myFontFamily.IsStyleAvailable(FontStyle.Regular))
            {
                if (fontStyle == FontStyle.Regular)
                {
                    fs = FontStyle.Regular;
                    FontBold.Checked = false;
                    FontItalic.Checked = false;
                    FontUnderline.Checked = false;
                    FontStrikeout.Checked = false;
                }
            }
            else
            {

                if (fontStyle == FontStyle.Regular) { fs = FontStyle.Italic; }
            }

            if (myFontFamily.IsStyleAvailable(FontStyle.Italic))
            {
                FontItalic.Enabled = true;
                //if (fontStyle == FontStyle.Italic)
                //{

                //    fs = FontStyle.Italic;
                //}
            }
            else
            {
                //fs = FontStyle.Bold;
                FontItalic.Enabled = false;
            }

            if (myFontFamily.IsStyleAvailable(FontStyle.Bold))
            {
                FontBold.Enabled = true;
            } else {
                if (fontStyle == FontStyle.Italic)
                {

                    fs = FontStyle.Italic;
                }
                FontBold.Enabled = false;
            }

            if (myFontFamily.IsStyleAvailable(FontStyle.Underline))
            {
                FontUnderline.Enabled = true;
            }
            else
            {
                FontUnderline.Enabled = false;
                if (fontStyle == FontStyle.Italic)
                {

                    fs = FontStyle.Italic;
                }
            }

            if (myFontFamily.IsStyleAvailable(FontStyle.Strikeout)) { FontStrikeout.Enabled = true; } else { FontStrikeout.Enabled = false; }

            //switch (CurrentDocument.SelectionFont.Style)
            //{
            //    case FontStyle.Regular:
            //        if (!regularfont)
            //        {

            //        }


            //}


            //try
            //{
            if (fs != null)
            {
                Font NewFont = new Font(ComboBox_Font.SelectedItem.ToString(), CurrentDocument.SelectionFont.Size, fs);

                CurrentDocument.SelectionFont = NewFont;
                SelectedFont = ComboBox_Font.SelectedIndex;
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    ComboBox_Font.SelectedIndex = SelectedFont;
            //    ComboBox_Font_SelectedIndexChanged(sender, e);
            //}

        }


        //private FontStyle CheckFontStyle(FontStyle fs, FontFamily ff)
        //{
        //    if ()
        //}

    }
}
