using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using CorelDRAW;
//using VGCore;
using System.Drawing;
namespace AccentBase.Utils
{

    public class CorelDrawExporter
    {
///*# import*/ "..\drawctl\drawctl.tlb" no_namespace, raw_interfaces_only  
        private Thread thread; // поток где будет корел
        #region Класс для пары (int)ширина/высота
        public class SizeShape
        {
            public int width { get; set; }
            public int height { get; set; }
            public SizeShape(int Width, int Height) { width = Width; height = Height; }
        }
        #endregion
        #region флаг прекращения операции
        private bool breaking = false;
        public bool Break
        {
            get { return breaking; }
            set { breaking = value; }
        }
        #endregion
        #region Событие
        #region Аргументы события
        public class CorelDrawExporterEventArgs : EventArgs
        {
            public Image preview { get; set; }
            public Exception ex { get; set; }
            public FileInfo fileName { get; set; }
            public CorelDrawExporterEventArgs(Image Preview, Exception Ex, FileInfo FileName)
            {
                preview = Preview;
                ex = Ex;
                fileName = FileName;
            }
        }
        #endregion
        public delegate void CorelDrawExporterEventHandler(object sender, CorelDrawExporterEventArgs e);
        public event CorelDrawExporterEventHandler CorelDrawExporterUpdateEvent;
        public void EventOn(CorelDrawExporterEventArgs e)
        {
            CorelDrawExporterUpdateEvent?.Invoke(this, e);
        }
        #endregion
        #region Точка входа
        public Exception ExportToPng(string fpath)
        {
            try
            {
                if (File.Exists(fpath))
                {
                    breaking = false;
                    thread = new Thread(CDRtoPNG);
                    thread.Name = "CDRtoPNG";
                    thread.IsBackground = true;
                    thread.Start(fpath);
                    return null;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch (Exception ex) { return ex; }
        }
        #endregion
        #region Открыть корел, открыть файл, выбрать всё, экспортировать.
        private void CDRtoPNG(object obj)
        {
            string fpath = obj as string;
            FileInfo fi = new FileInfo(fpath);
            //DirectoryInfo di = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\temp\");
            DirectoryInfo di = new DirectoryInfo(Utils.Settings.set.data_path + @"\temp\");
            FileInfo filename = new FileInfo(di + UnixDate.DateTimeToInt64(DateTime.Now).ToString() +"_"+ fi.Name + ".png");

            if (!di.Exists)
            {
                try { di.Create(); }
                catch (Exception ex) { EventOn(new CorelDrawExporterEventArgs(null, ex, fi)); return; }
            }

            if (fi.Exists)
            {
                //CorelDRAW.ApplicationClass ac = new ApplicationClass;
                //CorelDRAW. Draw = new CorelDRAW.ApplicationClass();
                //Type pia_type = Type.GetTypeFromProgID("CorelDRAW.Application.13");
                //VGCore.IVGApplication cdr1 = (VGCore.IVGApplication)Activator.CreateInstance(pia_type);
                //cdr.Application.AppWindow 
                //dynamic REGcdr = Activator.CreateInstance(pia_type) as VGCore.IVGApplication;
                //CreateObject("CorelDRAW.Application.17")
                //CorelDRAW.Application cdr = corelApp;
                //corelApp.Visible = false;
                //corelApp.CreateDocument();
                //Type pia_type = Type.GetTypeFromProgID("CorelDRAW.Application.13");
                //CorelDRAW.Application cdr = Activator.CreateInstance(pia_type) as CorelDRAW.Application;
                //CorelDRAW.Application cdr = corelApp.Application as CorelDRAW.Application;
                //CorelDRAW.Application cdr = app as CorelDRAW.Application;//new CorelDRAW.Application();
                try
                {
                    CorelDRAW.Application cdr = new CorelDRAW.Application();

                //Type type = Type.GetTypeFromProgID("CorelDRAW.Application.13", true);
                //dynamic cdr = (dynamic)Activator.CreateInstance(type);
                //cdr.Visible = true;

                //FileInfo fc = new FileInfo(@"C:\Program Files (x86)\Corel\CorelDRAW Graphics Suite 13\Programs\CorelDRW.exe");
                //Type type = Type.GetTypeFromProgID("CorelDRAW.Application.13", true);
                //dynamic cdr = Activator.CreateInstance(type);
                //CorelDRAW.Application cdr = vc as CorelDRAW.Application;
                //System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(fc.FullName);
                //dynamic cdr = assembly.CreateInstance("CorelDRAW.ApplicationClass", true);


                if (cdr.Documents.Count > 0) { cdr.Visible = true; } else { cdr.Visible = false; }
                //cdr.
                //corelApp.ActiveDocument = corelApp.Documents.Application

                //cdr.Visible = true;

                CorelDRAW.Document cd = cdr.CreateDocument();
                //cd.v
                //CorelDRAW.StructImportOptions ttt= cdr.CreateStructImportOptions();
                //ttt.CombineMultilayerBitmaps = true;
                //ttt.
                ////cdr.StructImportOptions 
                //cdr.DocumentOpen += Cdr_DocumentOpen;
                try
                {

                    //VGCore.IVGDocument cd = cdr.ActiveDocument;
                    //IVGStructImportOptions CSIO = cdr.CreateStructImportOptions();

                    StructImportOptions CSIO = cdr.CreateStructImportOptions();
                    CSIO.CodePage = 0;
                    CSIO.CombineMultilayerBitmaps = true;
                    CSIO.CustomData = 0;
                    CSIO.ImageIndex = 0;
                    CSIO.Mode = CorelDRAW.cdrImportMode.cdrImportFull;
                    //cd = cdr.CreateDocument();
                    cd.ActiveLayer.Import(fi.FullName, CorelDRAW.cdrFilter.cdrAutoSense, CSIO);


                    //var tttt = cdr.GetType();
                    //CorelDRAW.Document cd =
                    //CorelDRAW.Document cd = cdr.OpenDocument(fi.FullName, 1);

                    //CorelDRAW.cdrImportMode.cdrImportCrop();
                    //CGMImport
                    //var qqq = cd.PDFSettings;
                    //cd.EndCommandGroup();
                    //var qqq = cd.SourceFormat;
                    cd.SelectableShapes.All().CreateSelection();
                    CorelDRAW.Shape sh = cd.Selection();
                    if (sh.SizeWidth > 0 && sh.SizeHeight > 0)
                    {
                        SizeShape sizechape = ScaleSizeShape(800, 800, sh.SizeWidth, sh.SizeHeight);
                        if (sizechape != null)
                        {
                            //IVGStructExportOptions exop = cdr.CreateStructExportOptions();
                            ////exop.
                            ////exop.
                            //exop.Overwrite = true;
                            //IVGStructPaletteOptions expal = cdr.CreateStructPaletteOptions();

                            //VGCore.cdrExportRange rng = VGCore.cdrExportRange.cdrSelection;
                            //CorelDRAW.ExportFilter exporter = cd.ExportBitmap(filename.FullName, CorelDRAW.cdrFilter.cdrPNG, CorelDRAW.cdrExportRange.cdrSelection, CorelDRAW.cdrImageType.cdrRGBColorImage, sizechape.width, sizechape.height, 72, 72, CorelDRAW.cdrAntiAliasingType.cdrNormalAntiAliasing, false, false, true, false, CorelDRAW.cdrCompressionType.cdrCompressionNone, null);
                                VGCore.ICorelExportFilter exporter = cd.ExportBitmap(filename.FullName, CorelDRAW.cdrFilter.cdrPNG, CorelDRAW.cdrExportRange.cdrSelection, CorelDRAW.cdrImageType.cdrRGBColorImage, sizechape.width, sizechape.height, 72, 72, CorelDRAW.cdrAntiAliasingType.cdrNormalAntiAliasing, false, false, true, false, CorelDRAW.cdrCompressionType.cdrCompressionNone, null);
                                exporter.Finish();
                            //VGCore.ICorelExportFilter exportericon = cd.ExportBitmap(filename.FullName, CorelDRAW.cdrFilter.cdrPNG, CorelDRAW.cdrExportRange.cdrSelection, CorelDRAW.cdrImageType.cdrRGBColorImage, sizechape.width, sizechape.height, 72, 72, CorelDRAW.cdrAntiAliasingType.cdrNormalAntiAliasing, false, false, true, false, CorelDRAW.cdrCompressionType.cdrCompressionNone, null);
                            //exporter.Finish();
                            //VGCore.ICorelExportFilter exporter = new ICorelExportFilter();
                            //var exporter = cd.ExportBitmap(filename.FullName, VGCore.cdrFilter.cdrPNG, VGCore.cdrExportRange.cdrSelection, VGCore.cdrImageType.cdrRGBColorImage, sizechape.width, sizechape.height, 72, 72, VGCore.cdrAntiAliasingType.cdrNormalAntiAliasing, false, false, true, false, VGCore.cdrCompressionType.cdrCompressionNone, null);
                            //VGCore.ICorelExportFilter exporter = cd.ExportBitmap(filename.FullName, exop, expal);
                            //exporter.Finish();
                            try
                            {
                                using (FileStream stream = File.OpenRead(filename.FullName))
                                {
                                    //Image result = Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: true)
                                    EventOn(new CorelDrawExporterEventArgs(FilenameToImage(filename.FullName), null, fi));
                                }
                            }
                            catch (Exception ex)
                            {
                                EventOn(new CorelDrawExporterEventArgs(null, ex, fi));
                            }

                            try
                            {
                                filename.Delete();
                            }
                            catch (Exception ex) { EventOn(new CorelDrawExporterEventArgs(null, ex, fi)); }
                        }
                        else
                        {
                            throw new Exception("Не найдены объекты для экспорта!");
                        }
                    }else { throw new Exception("Не найдены объекты для экспорта!"); }
                }
                catch (Exception ex)
                {
                    EventOn(new CorelDrawExporterEventArgs(null, ex, fi));
                }
                finally
                {

                    if (cd != null) { cd.Close(); }
                    if (cdr != null)
                    {

                        if (cdr.Documents.Count == 0) { cdr.Quit(); } else { cdr.Visible = true; }
                    }
                    //if (cdr != null)
                    //{
                    //    while (cdr.ActiveDocument != null)
                    //    {
                    //        cdr.ActiveDocument.Close();
                    //    }
                    //    cdr.Quit();
                    //}
                    //if (cdr != null && cdr.ActiveDocument != null) { cdr.ActiveDocument.Close(); }
                    //if (cdr != null)
                    //{
                    //    try
                    //    {
                    //        cdr.Quit();
                    //    }
                    //    catch { }
                    //}
                }

                }
                catch
                {
                    EventOn(new CorelDrawExporterEventArgs(null, new Exception("CORELDRAW X3 не предоставляет COM доступ к своим объектам!" + Environment.NewLine + "1. Установите CORELDRAW X3, если он не установлен." + Environment.NewLine + "2. запустите setup.bat (в папке с setup.bat должен находиться regtlibv12.exe)" + Environment.NewLine + "3. Перезагрузите компьютер."), fi));
                }
            }

            else { EventOn(new CorelDrawExporterEventArgs(null, new Exception("Исходный файл не найден!"), fi)); }
        }

        private void Cdr_DocumentOpen(Document Doc, string FileName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Уменьшаем пропорционально
        public static SizeShape ScaleSizeShape(int maxWidth, int maxHeight, double width, double height)
        {
            try
            {
                double ratioX = (double)maxWidth / width; double ratioY = (double)maxHeight / height;
                double ratio = Math.Min(ratioX, ratioY);
                return new SizeShape((int)(width * ratio), (int)(height * ratio));
            }
            catch { return null; }
        }
        #endregion;


        public Image FilenameToImage(string str)
        {
            try
            {
                using (var stream = File.OpenRead(str))
                {
                    return Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: true);
                }
            }
            catch { return null; }
        }
    }
}
