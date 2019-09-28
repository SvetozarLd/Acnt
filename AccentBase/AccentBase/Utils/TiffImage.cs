//using org.pdfclown.documents;
//using org.pdfclown.files;
//using org.pdfclown.tools;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace AccentBase.Utils
{
    public class TiffImage
    {
        //private string myPath;
        //private Guid myGuid;
        //private FrameDimension myDimension;
        //public ArrayList myImages = new ArrayList();
        //private int myPageCount;
        //private Bitmap myBMP;

        public TiffImage(string path)
        {


            ////string filePath = PromptFileChoice("Please select a PDF file");
            //using (org.pdfclown.files.File file = new org.pdfclown.files.File(path))
            //{
            //    Document document = file.Document;
            //    Pages pages = document.Pages;

            //    // 2. Page rasterization.
            //    int pageIndex = 0;// PromptPageChoice("Select the page to render", pages.Count);
            //    Page page = pages[pageIndex];
            //    SizeF imageSize = page.Size;
            //    Renderer renderer = new Renderer();
            //    Image image = renderer.Render(page, imageSize);

            //    // 3. Save the page image!
            //    //image.Save(GetOutputPath("ContentRenderingSample.jpg"), ImageFormat.Jpeg);
            //    image.Save(path+".jpg", ImageFormat.Jpeg); 
            //}



            //MemoryStream ms;
            //Image myImage;

            //myPath = path;
            //FileStream fs = new FileStream(myPath, FileMode.Open);
            //myImage = Image.FromStream(fs);
            //myGuid = myImage.FrameDimensionsList[0];
            //myDimension = new FrameDimension(myGuid);
            //myPageCount = myImage.GetFrameCount(myDimension);
            //for (int i = 0; i < myPageCount; i++)
            //{
            //    ms = new MemoryStream();
            //    myImage.SelectActiveFrame(myDimension, i);
            //    myImage.Save(ms, ImageFormat.Bmp);
            //    myBMP = new Bitmap(ms);
            //    myImages.Add(myBMP);
            //    ms.Close();
            //}
            //fs.Close();





            //const string filename = @"D:\Work\2017-07-03\Заявка N8984.pdf";
            //PdfDocument document = PdfReader.Open(filename, PdfDocumentOpenMode.ReadOnly);
            //int imageCount = 0;
            //// Iterate pages
            //foreach (PdfPage page in document.Pages)
            //{
            //    // Get resources dictionary
            //    PdfDictionary resources = page.Elements.GetDictionary("/Resources");
            //    if (resources != null)
            //    {
            //        // Get external objects dictionary
            //        PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
            //        if (xObjects != null)
            //        {
            //            ICollection<PdfItem> items = xObjects.Elements.Values;
            //            // Iterate references to external objects
            //            foreach (PdfItem item in items)
            //            {
            //                PdfReference reference = item as PdfReference;
            //                if (reference != null)
            //                {
            //                    PdfDictionary xObject = reference.Value as PdfDictionary;

            //                    // Is external object an image?

            //                    if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")

            //                    {

            //                        ExportImage(xObject, ref imageCount);

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        //static void ExportImage(PdfDictionary image, ref int count)
        //{
        //    string filter = image.Elements.GetName("/Filter");
        //    switch (filter)
        //    {
        //        case "/DCTDecode":
        //            ExportJpegImage(image, ref count);
        //            break;
        //        case "/FlateDecode":
        //            ExportAsPngImage(image, ref count);
        //            break;
        //    }
        //}
        //static void ExportJpegImage(PdfDictionary image, ref int count)
        //{
        //    // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
        //    byte[] stream = image.Stream.Value;
        //    FileStream fs = new FileStream(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
        //    BinaryWriter bw = new BinaryWriter(fs);
        //    bw.Write(stream);
        //    bw.Close();
        //}
        //static void ExportAsPngImage(PdfDictionary image, ref int count)
        //{
        //    int width = image.Elements.GetInteger(PdfImage.Keys.Width);
        //    int height = image.Elements.GetInteger(PdfImage.Keys.Height);
        //    int bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);
        //}
    }
}
