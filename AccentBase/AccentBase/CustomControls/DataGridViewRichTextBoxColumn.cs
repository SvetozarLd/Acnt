using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AccentBase
{
    public class DataGridViewOrdersColumn : DataGridViewColumn
    {
        public DataGridViewOrdersColumn() : base(new DataGridViewOrdersCell())
        {

        }
    }

    public class DataGridViewOrdersCell : DataGridViewTextBoxCell
    {

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
            set
            {
                base.ValueType = value;
            }
        }

        public override Type FormattedValueType
        {
            get
            {
                return typeof(string);
            }
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            ////base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);
            ////RichTextBox rtb = new RichTextBox();
            //if (value != null)
            //{
                //Image img = new Bitmap(50, 50);
                ////int opacity = 128; // 50% opaque (0 = invisible, 255 = fully opaque)
                ////graphics = Graphics.FromImage(img);
                //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                //graphics.DrawString("This is a watermark",
                //    new Font("Arial", 20),
                //    new SolidBrush(Color.FromArgb(255, Color.Red)),
                //    0,
                //    0);
                //    string[] separator = { Environment.NewLine };
                //    string[] arr = value.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                //    Font norm = DataGridView.DefaultFont;
                //    Font strike = new System.Drawing.Font(DataGridView.DefaultFont.FontFamily, DataGridView.DefaultFont.Size, FontStyle.Strikeout, GraphicsUnit.Pixel);
                //    //strike.Strikeout = true;
                //    int i = 0;
                //    foreach (string str in arr)
                //    {
                //        i += 16;
                //        ////graphics gr = new graphics();
                //        //    graphics.DrawString(str, norm, Brushes.Black, cellBounds.Left + i, cellBounds.Top);
                //        //i++;
                //    }
                //    //cellBounds.Height = 200;
                //    //clipBounds.Height = 200;
                //    //Image img = new Bitmap(cellBounds.Width, cellBounds.Height);
                //    //this.DataGridView.Rows[rowIndex].Height = 200;
                //    //img.FillRectangle(backColorBrush, e.CellBounds);
                //    //RectangleF rectf = new RectangleF(0, 0, cellBounds.Width, cellBounds.Height);
                //    //Graphics g = Graphics.FromImage(img);
                //    //g.SmoothingMode = SmoothingMode.AntiAlias;
                //    //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //    //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //    //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf);

                //    //g.Flush();

                //    //image.Image = bmp;





                //    //var ctrl = (CustomUserControl)value;

                //    //ctrl.DrawToBitmap(img, new Rectangle(0, 0, ctrl.Width, ctrl.Height));
                //    //graphics.DrawImage(img, cellBounds.Location);
                //    //base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                //    //cellStyle.BackColor = DataGridView.DefaultBackColor;
                //    ////paintBackground(clipBounds, true);
                //    ////paintContent(clipBounds);
                //    //using (Pen p = new Pen(Brushes.Black, 12))
                //    //{
                //    //    graphics.DrawLine(p, new Point(cellBounds.Left, cellBounds.Bottom),
                //    //                           new Point(cellBounds.Right, cellBounds.Bottom));
                //    //}
                //    //using (Pen p = new Pen(Brushes.Black, 6))
                //    //{
                //    //    graphics.DrawLine(p, new Point(cellBounds.Right, cellBounds.Top),
                //    //                           new Point(cellBounds.Right, cellBounds.Bottom));
                //    //}
                //    //handled = true;
                //    //using (Stringr)
                //    //string[] str = value.ToString().Split(@"\r\n");
                //    //if (value.ToString().StartsWith(@"{\rtf"))
                //    //{
                //    //    rtb.Rtf = value.ToString();
                //    //}
                //    //else
                //    //{
                //    //    rtb.Text = value.ToString();
                //    //}
                //    //if (rtb.Text != string.Empty) {
                //    //graphics.DrawString(rtb.Text, DataGridView.DefaultFont, Brushes.Black, cellBounds.Left, cellBounds.Top);
                //    //}
            //}
        }
    }
}