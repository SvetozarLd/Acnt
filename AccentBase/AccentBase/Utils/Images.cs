using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
//using 
//using BitMiracle.LibTiff.Classic;
using System.Windows.Forms;

namespace AccentBase.Utils
{
    class ImagesUtils
    {






        ////public static Image bmp = null;
        ////static public string tifname { get; set; }
        //static public Image TiffTo24BitBitmap(string pathtiff)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    FileStream fs = new FileStream(pathtiff, FileMode.Open);
        //    fs.CopyTo(ms);

        //    ms.Position = 0;
        //    //using (Tiff tif = Tiff.Open(pathtiff, "r"))
        //    using (Tiff tif = Tiff.ClientOpen("someArbitraryName", "r", ms, new TiffStream()))
        //    {
        //        // Find the width and height of the image
        //        FieldValue[] value = tif.GetField(TiffTag.IMAGEWIDTH);
        //        int width = value[0].ToInt();

        //        value = tif.GetField(TiffTag.IMAGELENGTH);
        //        int height = value[0].ToInt();
        //        //tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
        //        // Read the image into the memory buffer 

        //        //tif.SetField(TiffTag.IMAGEWIDTH, width);
        //        //tif.SetField(TiffTag.IMAGELENGTH, height);
        //        //tif.SetField(TiffTag.COMPRESSION, Compression.DEFLATE);
        //        //tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
        //        //tif.SetField(TiffTag.PHOTOMETRIC, Photometric.SEPARATED);
        //        //tif.SetField(TiffTag.BITSPERSAMPLE, 8);
        //        //tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
        //        //UInt64[]  qqq = new UInt64[height * width];

        //        int[] raster = new int[height * width];
        //        if (!tif.ReadRGBAImage(width, height, raster))
        //        {
        //            System.Windows.Forms.MessageBox.Show("Could not read image");
        //            return null;
        //        }

        //        using (Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
        //        {
        //            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

        //            BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //            byte[] bits = new byte[bmpdata.Stride * bmpdata.Height];

        //            for (int y = 0; y < bmp.Height; y++)
        //            {
        //                int rasterOffset = y * bmp.Width;
        //                int bitsOffset = (bmp.Height - y - 1) * bmpdata.Stride;

        //                for (int x = 0; x < bmp.Width; x++)
        //                {
        //                    int rgba = raster[rasterOffset++];
        //                    bits[bitsOffset++] = (byte)((rgba >> 16) & 0xff);
        //                    bits[bitsOffset++] = (byte)((rgba >> 8) & 0xff);
        //                    bits[bitsOffset++] = (byte)(rgba & 0xff);
        //                }
        //            }

        //            System.Runtime.InteropServices.Marshal.Copy(bits, 0, bmpdata.Scan0, bits.Length);
        //            bmp.UnlockBits(bmpdata);

        //            //bmp.Save("TiffTo24BitBitmap.bmp");
        //            //System.Diagnostics.Process.Start("TiffTo24BitBitmap.bmp");
        //            //bmp = ResizePhoto(bmp, 800, 800);
        //            //Bitmap newbit = new Bitmap(bmp, bmp.Height/10, bmp.Width/10);
        //            //Image IM = Image.FromHbitmap(bmp.GetHbitmap());
        //            //Image IM = ResizePhoto(bmp, 800, 800);
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            //return IM;
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            bmp.Save("TiffTo32BitBitmap.bmp", ImageFormat.Bmp);
        //            //Image IM = Image.FromHbitmap(bmp.GetHbitmap(), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //            string path = Application.StartupPath + @"\RGBinput.icm";
        //            string path2 = Application.StartupPath + @"\CMYKinput.icm";
        //            string path3 = Application.StartupPath + @"\ISOcoated_v2_eci.icc";
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            Image IM = Image.FromHbitmap(bmp.GetHbitmap());
        //            using (Stream imageStream = new MemoryStream())
        //            {
        //                IM.Save(imageStream, System.Drawing.Imaging.ImageFormat.Bmp);
        //                BitmapSource myBitmapSource = BitmapFrame.Create(imageStream);
        //                BitmapFrame myBitmapSourceFrame = (BitmapFrame)myBitmapSource;
        //                //ColorContext sourceColorContext = new ColorContext(new Uri(path2));
        //                ColorContext sourceColorContext = new ColorContext(PixelFormats.Rgb24);   //myBitmapSourceFrame.ColorContexts[0];
        //                ColorContext destColorContext = new ColorContext(new Uri(path));
        //                //ColorContext sourceColorContext = myBitmapSourceFrame.ColorContexts[0];   //myBitmapSourceFrame.ColorContexts[0];
        //                //ColorContext destColorContext = new ColorContext(PixelFormats.Cmyk32);
        //                ColorConvertedBitmap ccb = new ColorConvertedBitmap(myBitmapSource, sourceColorContext, destColorContext, PixelFormats.Rgb24);
        //                //ColorConvertedBitmap ccb2 = new ColorConvertedBitmap(ccb, destColorContext, sourceColorContext, PixelFormats.Rgb24);
        //                //Image myImage3 = new Image(ccb);
        //                //myImage3.Source = ccb;
        //                //myImage3.Stretch = Stretch.None;
        //                IM = BitmapFromSource(ccb);
        //            }


        //            //Stream imageStream = new FileStream(pathtiff, FileMode.Open, FileAccess.Read, FileShare.Read);














        //            //bmp.Save("TiffTo32BitBitmap.bmp");
        //            //System.Diagnostics.Process.Start("TiffTo32BitBitmap.bmp");
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            //IM = Image.FromHbitmap(bmp.GetHbitmap, bmp.Palette.GetType);
        //            //IM.PixelFormat = PixelFormat.
        //            //bool isCMYK = IM.Format == System.Windows.Media.PixelFormats.Cmyk32);



        //            //  System.Windows.Media.ColorConverter()
        //            //ColorConvertedBitmap myColorConvertedBitmap = new ColorConvertedBitmap();
        //            //  myColorConvertedBitmap.BeginInit();
        //            //  myColorConvertedBitmap.SourceColorContext = myBitmapSourceFrame2.ColorContexts[0];
        //            //  myColorConvertedBitmap.Source = myBitmapSource2;
        //            //  myColorConvertedBitmap.DestinationFormat = PixelFormats.Pbgra32;
        //            //  myColorConvertedBitmap.DestinationColorContext = new ColorContext(PixelFormats.Bgra32);
        //            //  myColorConvertedBitmap.EndInit();


        //            //float[] colorValues = new float[4];
        //            //colorValues[0] = c / 255f;
        //            //colorValues[1] = m / 255f;
        //            //colorValues[2] = y / 255f;
        //            //colorValues[3] = k / 255f;

        //            //System.Windows.Media.Color color = System.Windows.Media.Color.FromValues(colorValues, new Uri(@"C:\Users\me\Documents\ISOcoated_v2_300_eci.icc"));
        //            //System.Drawing.Color rgbColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        //            //Image IM = Image.FromHbitmap(bmp.GetHbitmap(), PixelFormat.Format32bppRgb);
        //            //IM = ScaleImage(bmp, 800, 800);
        //            return IM;
        //        }
        //    }
        //}

        public static Image ByteToImage(byte[] img)
        {
            using (MemoryStream mStream = new MemoryStream(img))
            {
                return Image.FromStream(mStream);
            }
        }







        public void TiffToImage(string pathtif)
        {
            Image IM = null;
            try
            {
                using (FileStream stream = File.Open(pathtif, FileMode.Open))
                {
                    using (Bitmap bmp = new Bitmap(stream))
                    {
                        IM = ScaleImage(bmp, 800, 800);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public Image TiffToBitmap(string pathtif)
        {
            Image IM = null;
            byte[] result;
            using (FileStream stream = File.Open(pathtif, FileMode.Open))
            {
                //result = new byte[stream.Length];
                //stream.Read(result, 0, result.Length);
                //using (MemoryStream mStream = new MemoryStream(result))
                //{
                    using (Bitmap bmp = new Bitmap(stream))
                    {
                        IM = ScaleImage(bmp, 800, 800);
                    }
                //}
                //await stream.ReadAsync(result, 0, (int)stream.Length);
            }


            //using (FileStream fs = new FileStream(pathtif, FileMode.Open, FileAccess.Read, 4096, true))
            //{

            //}










            //Stream imageStreamSource = new FileStream(pathtif, FileMode.Open, FileAccess.Read, FileShare.Read);
            //TiffBitmapDecoder decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            //BitmapSource bitmapSource = decoder.Frames[0];
            //Image IM = BitmapFromSource(bitmapSource);
            //return IM;
            return IM;
            //// Draw the Image
            //Image myImage = new Image();
            //myImage.Source = bitmapSource;
            //myImage.Stretch = Stretch.None;
        }



        //public static Image TiffTo32BitBitmap(string pathtif)
        //{
        //    using (Tiff tif = Tiff.Open(pathtif, "r"))
        //    {
        //        // Find the width and height of the image
        //        FieldValue[] value = tif.GetField(TiffTag.IMAGEWIDTH);
        //        int width = value[0].ToInt();

        //        value = tif.GetField(TiffTag.IMAGELENGTH);
        //        int height = value[0].ToInt();

        //        // Read the image into the memory buffer 
        //        int[] raster = new int[height * width];
        //        if (!tif.ReadRGBAImage(width, height, raster))
        //        {
        //            System.Windows.Forms.MessageBox.Show("Could not read image");
        //            return null;
        //        }
        //        //tif.SetField(TiffTag.IMAGEWIDTH, width);
        //        //tif.SetField(TiffTag.IMAGELENGTH, height);
        //        //tif.SetField(TiffTag.COMPRESSION, Compression.DEFLATE);
        //        //tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
        //        //tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
        //        //tif.SetField(TiffTag.BITSPERSAMPLE, 8);
        //        //tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);

        //        using (Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
        //        {
        //            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

        //            BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //            byte[] bits = new byte[bmpdata.Stride * bmpdata.Height];

        //            for (int y = 0; y < bmp.Height; y++)
        //            {
        //                int rasterOffset = y * bmp.Width;
        //                int bitsOffset = (bmp.Height - y - 1) * bmpdata.Stride;

        //                for (int x = 0; x < bmp.Width; x++)
        //                {
        //                    int rgba = raster[rasterOffset++];
        //                    bits[bitsOffset++] = (byte)((rgba >> 16) & 0xff);
        //                    bits[bitsOffset++] = (byte)((rgba >> 8) & 0xff);
        //                    bits[bitsOffset++] = (byte)(rgba & 0xff);
        //                    bits[bitsOffset++] = (byte)((rgba >> 24) & 0xff);
        //                }
        //            }

        //            System.Runtime.InteropServices.Marshal.Copy(bits, 0, bmpdata.Scan0, bits.Length);
        //            bmp.UnlockBits(bmpdata);
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            bmp.Save("TiffTo32BitBitmap.Jpeg", ImageFormat.Jpeg);

        //            //Image IM = Image.FromHbitmap(bmp.GetHbitmap(), System.Drawing.Imaging.PixelFormat.Format32bppRgb);
        //            string path = Application.StartupPath + @"\RGBinput.icm";
        //            string path2 = Application.StartupPath + @"\ISOcoated_v2_300_eci.icc";
        //            Stream imageStream = new FileStream("TiffTo32BitBitmap.Jpeg", FileMode.Open, FileAccess.Read, FileShare.Read);
        //            BitmapSource myBitmapSource = BitmapFrame.Create(imageStream);
        //            BitmapFrame myBitmapSourceFrame = (BitmapFrame)myBitmapSource;
        //            ColorContext sourceColorContext = new ColorContext(PixelFormats.Rgb24);   //myBitmapSourceFrame.ColorContexts[0];
        //            ColorContext destColorContext = new ColorContext(new Uri(path2));
        //            ColorConvertedBitmap ccb = new ColorConvertedBitmap(myBitmapSource, sourceColorContext, destColorContext, PixelFormats.Cmyk32);
        //            //Image myImage3 = new Image(ccb);
        //            //myImage3.Source = ccb;
        //            //myImage3.Stretch = Stretch.None;
        //            Image IM = BitmapFromSource(ccb);
        //            imageStream.Close();
                    









        //            //bmp.Save("TiffTo32BitBitmap.bmp");
        //            //System.Diagnostics.Process.Start("TiffTo32BitBitmap.bmp");
        //            //Image IM = ScaleImage(bmp, 800, 800);
        //            //IM = Image.FromHbitmap(bmp.GetHbitmap, bmp.Palette.GetType);
        //            //IM.PixelFormat = PixelFormat.
        //            //bool isCMYK = IM.Format == System.Windows.Media.PixelFormats.Cmyk32);



        //            //  System.Windows.Media.ColorConverter()
        //            //ColorConvertedBitmap myColorConvertedBitmap = new ColorConvertedBitmap();
        //            //  myColorConvertedBitmap.BeginInit();
        //            //  myColorConvertedBitmap.SourceColorContext = myBitmapSourceFrame2.ColorContexts[0];
        //            //  myColorConvertedBitmap.Source = myBitmapSource2;
        //            //  myColorConvertedBitmap.DestinationFormat = PixelFormats.Pbgra32;
        //            //  myColorConvertedBitmap.DestinationColorContext = new ColorContext(PixelFormats.Bgra32);
        //            //  myColorConvertedBitmap.EndInit();


        //            //float[] colorValues = new float[4];
        //            //colorValues[0] = c / 255f;
        //            //colorValues[1] = m / 255f;
        //            //colorValues[2] = y / 255f;
        //            //colorValues[3] = k / 255f;

        //            //System.Windows.Media.Color color = System.Windows.Media.Color.FromValues(colorValues, new Uri(@"C:\Users\me\Documents\ISOcoated_v2_300_eci.icc"));
        //            //System.Drawing.Color rgbColor = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
        //            //Image IM = Image.FromHbitmap(bmp.GetHbitmap(), PixelFormat.Format32bppRgb);

        //            return IM;
        //        }
        //    }
        //}




        static private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap= null;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }




        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);
            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(image, newWidth, newHeight);
            return newImage;
        }

        private static Image ResizePhoto(Bitmap original, int desiredWidth, int desiredHeight)
        {
            //throw error if bouning box is to small
            if (desiredWidth < 4 || desiredHeight < 4)
                throw new InvalidOperationException("Bounding Box of Resize Photo must be larger than 4X4 pixels.");
            //var original = Bitmap.FromFile(sourceImage.FullName);

            //store image widths in variable for easier use
            var oW = (decimal)original.Width;
            var oH = (decimal)original.Height;
            var dW = (decimal)desiredWidth;
            var dH = (decimal)desiredHeight;

            //check if image already fits
            if (oW < dW && oH < dH)
                return original; //image fits in bounding box, keep size (center with css) If we made it biger it would stretch the image resulting in loss of quality.

            //check for double squares
            if (oW == oH && dW == dH)
            {
                //image and bounding box are square, no need to calculate aspects, just downsize it with the bounding box
                Bitmap square = new Bitmap(original, (int)dW, (int)dH);
                original.Dispose();
                return square;
            }

            //check original image is square
            if (oW == oH)
            {
                //image is square, bounding box isn't.  Get smallest side of bounding box and resize to a square of that center the image vertically and horizonatally with Css there will be space on one side.
                int smallSide = (int)Math.Min(dW, dH);
                Bitmap square = new Bitmap(original, smallSide, smallSide);
                original.Dispose();
                return square;
            }

            //not dealing with squares, figure out resizing within aspect ratios            
            if (oW > dW && oH > dH) //image is wider and taller than bounding box
            {
                var r = Math.Min(dW, dH) / Math.Min(oW, oH); //two demensions so figure out which bounding box demension is the smallest and which original image demension is the smallest, already know original image is larger than bounding box
                var nH = oW * r; //will downscale the original image by an aspect ratio to fit in the bounding box at the maximum size within aspect ratio.
                var nW = oW * r;
                var resized = new Bitmap(original, (int)nW, (int)nH);
                original.Dispose();
                return resized;
            }
            else
            {
                if (oW > dW) //image is wider than bounding box
                {
                    var r = dW / oW; //one demension (width) so calculate the aspect ratio between the bounding box width and original image width
                    var nW = oW * r; //downscale image by r to fit in the bounding box...
                    var nH = oW * r;
                    var resized = new Bitmap(original, (int)nW, (int)nH);
                    original.Dispose();
                    return resized;
                }
                else
                {
                    //original image is taller than bounding box
                    var r = dH / oH;
                    var nH = oH * r;
                    var nW = oW * r;
                    var resized = new Bitmap(original, (int)nW, (int)nH);
                    original.Dispose();
                    return resized;
                }
            }
        }
    }



}


