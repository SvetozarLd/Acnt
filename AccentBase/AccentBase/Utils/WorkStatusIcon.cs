using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace AccentBase.Utils
{
    static public class WorkStatusIcon
    {
        //const int height = 66;
        const int imageHeight = 16;
        const int imageWeight = 16;

        public static byte[] GenerateImages(int print, int cut, int cnc, int install)
        {
            int currentheight = imageHeight*4+2;
            if (print == 0) { currentheight = currentheight - imageHeight; }
            if (cut == 0) { currentheight = currentheight - imageHeight; }
            if (cnc == 0) { currentheight = currentheight - imageHeight; }
            if (install == 0) { currentheight = currentheight - imageHeight; }
            int currentpositions = 1;
            //return Properties.Resources.Okicon8;
            using (Bitmap bmp = new Bitmap(100, currentheight, PixelFormat.Format32bppArgb))
            {
                //bmp.
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    
                    gr.Clear(Color.Transparent);
                    gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    if (print == 1)
                    {
                        gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                        gr.DrawString("Печать", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                        currentpositions += imageHeight;
                    }
                    else
                    {
                        if (print == 2)
                        {
                            gr.DrawImage(Properties.Resources.Okicon, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                            gr.DrawString("Печать", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular| FontStyle.Strikeout), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                            currentpositions += imageHeight;
                        }
                    }

                    if (cut == 1)
                    {
                        gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                        gr.DrawString("Плот. резка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                        currentpositions += imageHeight;
                    }
                    else
                    {
                        if (cut == 2)
                        {
                            gr.DrawImage(Properties.Resources.Okicon, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                            gr.DrawString("Плот. резка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular | FontStyle.Strikeout), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                            currentpositions += imageHeight;
                        }
                    }
                    if (cnc == 1)
                    {
                        gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                        gr.DrawString("Фрезеровка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                        currentpositions += imageHeight;
                    }
                    else
                    {
                        if (cnc == 2)
                        {
                            gr.DrawImage(Properties.Resources.Okicon, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                            gr.DrawString("Фрезеровка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular | FontStyle.Strikeout), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                            currentpositions += imageHeight;
                        }
                    }
                    if (install == 1)
                    {
                        gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                        gr.DrawString("Монтаж", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                        currentpositions += imageHeight;
                    }
                    else
                    {
                        if (install == 2)
                        {
                            gr.DrawImage(Properties.Resources.Okicon, new System.Drawing.Rectangle(0, currentpositions, imageWeight, imageWeight), new System.Drawing.Rectangle(0, 0, imageWeight, imageWeight), GraphicsUnit.Pixel);
                            gr.DrawString("Монтаж", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular | FontStyle.Strikeout), new SolidBrush(Color.FromArgb(255, Color.Black)), imageWeight + 1, currentpositions);
                            currentpositions += imageHeight;
                        }
                    }
                    //gr.DrawImage(Properties.Resources.Okicon, new System.Drawing.Rectangle(0, 17, 16, 16), new System.Drawing.Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
                    //gr.DrawString("Плот. резка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular | FontStyle.Strikeout), new SolidBrush(Color.FromArgb(255, Color.Black)), 16, 17);
                    //gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, 33, 16, 16), new System.Drawing.Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
                    //gr.DrawString("Фрезеровка", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), 16, 33);
                    //gr.DrawImage(Properties.Resources.hourglass, new System.Drawing.Rectangle(0, 49, 16, 16), new System.Drawing.Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
                    //gr.DrawString("Монтаж", new Font("Microsoft Sans Serif", 9f, FontStyle.Regular), new SolidBrush(Color.FromArgb(255, Color.Black)), 16, 49);
                    return Utils.Converting.ImageToByte(bmp);
                }
            }
            //Image img = new Bitmap(50, 50);
            ////int opacity = 128; // 50% opaque (0 = invisible, 255 = fully opaque)
            //Graphics graphics = Graphics.FromImage(img);
            //Pen p = new Pen(Brushes.Black, 12);
            //graphics.DrawLine(p, new Point(0, 0),
            //                       new Point(40, 40));
            ////graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            ////graphics.DrawString("This is a watermark",
            ////    new Font("Arial", 40),
            ////    new SolidBrush(Color.FromArgb(opacity, Color.Red)),
            ////    0,
            ////    0);

            //return new Bitmap(50, 50, graphics);
            //Image img = new Image();
            //Graphics graphics = new Graphics( );
            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            //graphics.DrawString("hello", new Font("Arial", 36), new SolidBrush(Color.FromArgb(255, 0, 0)), new Point(20, 20));

            //Image playbutton;
            //try
            //{
            //    playbutton = Image.FromFile(/*somekindofpath*/);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}

            //Image frame;
            //try
            //{
            //    frame = Image.FromFile(/*somekindofpath*/);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}

            //using (frame)
            //{
            //    using (var bitmap = new Bitmap(width, height))
            //    {
            //        using (var canvas = Graphics.FromImage(bitmap))
            //        {
            //            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //            canvas.DrawImage(frame,
            //                             new Rectangle(0,
            //                                           0,
            //                                           width,
            //                                           height),
            //                             new Rectangle(0,
            //                                           0,
            //                                           frame.Width,
            //                                           frame.Height),
            //                             GraphicsUnit.Pixel);
            //            canvas.DrawImage(playbutton,
            //                             (bitmap.Width / 2) - (playbutton.Width / 2),
            //                             (bitmap.Height / 2) - (playbutton.Height / 2));
            //            canvas.Save();
            //        }
            //        try
            //        {
            //            bitmap.Save(/*somekindofpath*/,
            //                        System.Drawing.Imaging.ImageFormat.Jpeg);
            //        }
            //        catch (Exception ex) { }
            //    }
            //}
        }

    }
}
