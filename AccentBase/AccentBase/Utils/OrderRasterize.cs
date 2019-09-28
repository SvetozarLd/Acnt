//using System.Drawing.Imaging;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Drawing.Imaging;

namespace AccentBase.Utils
{
    public static class OrderRasterize
    {



        //public static GhostscriptVersionInfo _lastInstalledVersion = new GhostscriptVersionInfo(new System.Version(0, 0, 0), "gsdll32.dll", string.Empty, GhostscriptLicense.AFPL);
        //public static GhostscriptVersionInfo _lastInstalledVersion = GhostscriptVersionInfo.GetLastInstalledVersion();
        
        public static List<byte[]> CreateImage_FromBase(long OrderId)
        {
            List<byte[]> result = new List<byte[]>();
            byte[] pdf = CreatePDF_FromBase(OrderId);
            if (pdf != null && pdf.Length > 0)
            {
                try
                {
                    GhostscriptVersionInfo _lastInstalledVersion = new GhostscriptVersionInfo(new System.Version(0, 0, 0), "gsdll32.dll", string.Empty, GhostscriptLicense.GPL);
                    GhostscriptRasterizer _rasterizer = new GhostscriptRasterizer();
                    _rasterizer.Open(new System.IO.MemoryStream(pdf.ToArray()), _lastInstalledVersion, false);
                    for (int pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
                    {
                        result.Add(Utils.Converting.ImageToByte(_rasterizer.GetPage(360, 360, pageNumber)));
                    }

                    _rasterizer.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public static List<byte[]> CreateImage_FromProtoOrder(ProtoClasses.ProtoOrders.protoOrder order)
        {
            List<byte[]> result = new List<byte[]>();
            byte[] pdf = CreatePDF_FromProtoOrder(order);
            if (pdf != null && pdf.Length > 0)
            {
                try
                {
                    GhostscriptVersionInfo _lastInstalledVersion = new GhostscriptVersionInfo(new System.Version(0, 0, 0), "gsdll32.dll", string.Empty, GhostscriptLicense.GPL);
                    GhostscriptRasterizer _rasterizer = new GhostscriptRasterizer();
                    _rasterizer.Open(new System.IO.MemoryStream(pdf.ToArray()), _lastInstalledVersion, false);
                    for (int pageNumber = 1; pageNumber <= _rasterizer.PageCount; pageNumber++)
                    {
                        result.Add(Utils.Converting.ImageToByte(_rasterizer.GetPage(360, 360, pageNumber)));
                    }

                    _rasterizer.Close();
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        #region Создать пдф из базы
        public static byte[] CreatePDF_FromBase(long OrderId)
        {
            try
            {


                DataRow dataRow = SqlLite.Order.TableOrders.Select("id = " + OrderId).SingleOrDefault();
                if (dataRow != null)
                {


                    #region Setting material
                    string materialPrint = "Ошибка! Материал неопределен.";
                    string materialCut = "Ошибка! Материал неопределен.";
                    string materialCnc = "Ошибка! Материал неопределен.";
                    string equipPrint = "Ошибка! оборудование неопределено.";
                    string equipCut = "Ошибка! оборудование неопределено.";
                    string equipCnc = "Ошибка! оборудование неопределено.";
                    List<KeyValuePair<int, string>> mprint = SqlLite.Materials.DicMaterialPrint.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["material_print_id"])).ToList();
                    if (mprint != null && mprint.Count > 0) { materialPrint = mprint[0].Value; }

                    List<KeyValuePair<int, string>> mcut = SqlLite.Materials.DicMaterialCut.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["material_cut_id"])).ToList();
                    if (mcut != null && mcut.Count > 0) { materialCut = mcut[0].Value; }

                    List<KeyValuePair<int, string>> mcnc = SqlLite.Materials.DicMaterialCnc.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["material_cnc_id"])).ToList();
                    if (mcnc != null && mcnc.Count > 0) { materialCnc = mcnc[0].Value; }

                    List<KeyValuePair<int, string>> mprintters = SqlLite.Equip.DicPrinters.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["printers_id"])).ToList();
                    if (mprintters != null && mprintters.Count > 0) { equipPrint = mprintters[0].Value; }

                    List<KeyValuePair<int, string>> mcutters = SqlLite.Equip.DicCuters.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["cutters_id"])).ToList();
                    if (mcutters != null && mcutters.Count > 0) { equipCut = mcutters[0].Value; }

                    List<KeyValuePair<int, string>> mcncs = SqlLite.Equip.DicCncs.Where(item => item.Key == Utils.CheckDBNull.ToInt32(dataRow["cncs_id"])).ToList();
                    if (mcncs != null && mcncs.Count > 0) { equipCnc = mcncs[0].Value; }
                    #endregion







                    byte[] cachePDF;
                    //MessageBox.Show("Начало формирования PDF-документа");
                    using (MemoryStream ms = new MemoryStream())
                    {
                        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10);

                        PdfWriter writer = PdfWriter.GetInstance(document, ms);

                        //string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                        string sylfaenpath = Application.StartupPath + @"\sylfaen.ttf";
                        BaseFont sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                        iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                        iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, BaseColor.Black);
                        document.Open();
                        //document.Footer = new HeaderFooter(new );
                        document.Add(new Paragraph("Заявка №" + OrderId.ToString() + " :" + Utils.CheckDBNull.ToString(dataRow["work_name"]), head));
                        document.Add(new Paragraph(" ", normal));
                        Paragraph pr = new Paragraph("Отдано в обработку «_____»____________________ 20______ г.    Подпись (расшифровка)____________________________", normal)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        document.Add(pr);
                        pr = new Paragraph("      Отдано клиенту «_____»____________________ 20______ г.    Подпись (расшифровка)____________________________", normal)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        document.Add(pr);
                        document.Add(new Paragraph(" ", normal));
                        PdfPCell cell = new PdfPCell();
                        PdfPTable table0 = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Properties.Resources.blank_accent, System.Drawing.Imaging.ImageFormat.Bmp);
                        cell = new PdfPCell()
                        {
                            FixedHeight = 80f,
                            PaddingTop = 10f,
                            PaddingBottom = 10f,
                            BorderWidth = 0f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        logo.ScaleToFit(document.PageSize.Width - 20, 60f);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        cell.AddElement(logo);
                        table0.AddCell(cell);
                        document.Add(table0);
                        document.Add(new Paragraph(" ", normal));
                        document.Add(new Paragraph("Заявка №" + OrderId.ToString() + " :" + Utils.CheckDBNull.ToString(dataRow["work_name"]), head));
                        table0 = new PdfPTable(4)
                        {
                            WidthPercentage = 100,
                            SpacingBefore = 5f
                        };
                        cell = new PdfPCell();
                        cell.AddElement(new Phrase("Виды работ:" + Environment.NewLine, underline));

                        if (Utils.CheckDBNull.ToBoolean(dataRow["print_on"])) { cell.AddElement(new Phrase("Печать" + Environment.NewLine, normal)); }
                        if (Utils.CheckDBNull.ToBoolean(dataRow["cut_on"]))
                        {
                            if (Utils.CheckDBNull.ToBoolean(dataRow["cutting_on_print"])) { cell.AddElement(new Phrase("Плот. резка по меткам" + Environment.NewLine, normal)); }
                            else { cell.AddElement(new Phrase("Плот. резка" + Environment.NewLine, normal)); }

                        }

                        if (Utils.CheckDBNull.ToBoolean(dataRow["cnc_on"]))
                        {
                            if (Utils.CheckDBNull.ToBoolean(dataRow["cnc_on_print"])) { cell.AddElement(new Phrase("Фрезеровка по меткам" + Environment.NewLine, normal)); }
                            else { cell.AddElement(new Phrase("Фрезеровка" + Environment.NewLine, normal)); }
                        }
                        if (Utils.CheckDBNull.ToBoolean(dataRow["installation"])) { cell.AddElement(new Phrase("Монтажные работы" + Environment.NewLine, normal)); }

                        //if (Utils.CheckDBNull.ToBoolean(dataRow["cutting_on_print"])) { cell.AddElement(new Phrase("Фрезеровка по меткам" + Environment.NewLine, normal)); }
                        cell.Padding = 5f;
                        cell.BorderWidth = 0f;
                        table0.AddCell(cell);

                        cell = new PdfPCell
                        {
                            Colspan = 3
                        };
                        cell.AddElement(new Phrase("Наименование: " + Utils.CheckDBNull.ToString(dataRow["work_name"]) + Environment.NewLine, underline));
                        cell.AddElement(new Phrase("Заказчик: " + Utils.CheckDBNull.ToString(dataRow["client"]) + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Добавил: " + Utils.CheckDBNull.ToString(dataRow["adder"]) + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Начать: " + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(dataRow["date_start"])).ToString() + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Сделать до: " + Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(dataRow["dead_line"])).ToString() + Environment.NewLine, normal));
                        cell.Padding = 5f;
                        cell.BorderWidth = 0f;
                        table0.AddCell(cell);

                        document.Add(table0);

                        table0 = new PdfPTable(3)
                        {
                            SpacingBefore = 10f,
                            WidthPercentage = 100
                        };
                        //table0.TotalWidth = document.PageSize.Width;
                        cell = new PdfPCell(new Phrase("Информация о печати:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Информация о плоттерной резке:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Информация о фрезеровке:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        string sizeText = ""; string countText = "";

                        if (Utils.CheckDBNull.ToBoolean(dataRow["print_on"]))
                        {
                            sizeText = Utils.CheckDBNull.ToDouble(dataRow["size_x_print"]).ToString() + "x" + Utils.CheckDBNull.ToDouble(dataRow["size_y_print"]).ToString() + " м.";
                            countText = Utils.CheckDBNull.ToInt32(dataRow["count_print"]).ToString() + " шт.";
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipPrint + Environment.NewLine + "Материал: " + materialPrint + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Качество: " + Utils.CheckDBNull.ToString(dataRow["print_quality"]), normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);
                        if (Utils.CheckDBNull.ToBoolean(dataRow["cut_on"]))
                        {
                            //if (Utils.CheckDBNull.ToBoolean(dataRow["print_on"])) { material = comboBox_MaterialPrint.Text; } else { material = comboBox_MaterialCut.Text; }
                            sizeText = Utils.CheckDBNull.ToDouble(dataRow["size_x_cut"]).ToString() + "x" + Utils.CheckDBNull.ToDouble(dataRow["size_y_cut"]).ToString() + " м.";
                            countText = Utils.CheckDBNull.ToInt32(dataRow["count_cut"]).ToString() + " шт.";
                            if (Utils.CheckDBNull.ToBoolean(dataRow["cutting_on_print"])) { materialCut = materialPrint; }
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipCut + Environment.NewLine + "Материал: " + materialCut + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Линеатура всего: " + Utils.CheckDBNull.ToDouble(dataRow["size_cut"]).ToString() + " м.", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);


                        if (Utils.CheckDBNull.ToBoolean(dataRow["cnc_on"]))
                        {
                            sizeText = Utils.CheckDBNull.ToDouble(dataRow["size_x_cnc"]).ToString() + "x" + Utils.CheckDBNull.ToDouble(dataRow["size_y_cnc"]).ToString() + " м.";
                            countText = Utils.CheckDBNull.ToInt32(dataRow["count_cnc"]).ToString() + " шт.";
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipCnc + Environment.NewLine + "Материал: " + materialCnc + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Линеатура всего: " + Utils.CheckDBNull.ToDouble(dataRow["size_cnc"]).ToString() + " м.", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);

                        document.Add(table0);










                        StringBuilder builder = new StringBuilder();
                        if (Utils.CheckDBNull.ToBoolean(dataRow["laminat"])) { if (Utils.CheckDBNull.ToBoolean(dataRow["laminat_mat"])) { builder.Append("Ламинация мат./ "); } else { builder.Append("Ламинация глян./ "); } }
                        if (Utils.CheckDBNull.ToBoolean(dataRow["baner_handling"])) { if (Utils.CheckDBNull.ToBoolean(dataRow["baner_luvers"])) { builder.Append(" Люверсы через " + Utils.CheckDBNull.ToInt32(dataRow["baner_handling_size"]).ToString() + "см./ "); } else { builder.Append("Карманы " + Utils.CheckDBNull.ToInt32(dataRow["baner_handling_size"]).ToString() + "см./ "); } }
                        string worktypes_list_string = Utils.CheckDBNull.ToString(dataRow["worktypes_list"]);
                        if (worktypes_list_string != string.Empty)
                        {
                            worktypes_list_string = worktypes_list_string.Replace(Convert.ToString((char)(219)), "/ ");                           
                            builder.Append(worktypes_list_string);
                        }
                        if (Utils.CheckDBNull.ToBoolean(dataRow["delivery"]))
                        {
                            string deliveryAddress = Utils.CheckDBNull.ToString(dataRow["delivery_address"]);
                            if (Utils.CheckDBNull.ToBoolean(dataRow["delivery_office"]))
                            {
                                if (deliveryAddress.Trim() != string.Empty)
                                {
                                    builder.Append("Доставка в офис: "); builder.Append(deliveryAddress + "/ ");
                                }
                                else
                                {
                                    builder.Append("Доставка в офис/ ");
                                }
                            }
                            else
                            {

                                string client = Utils.CheckDBNull.ToString(dataRow["client"]);
                                if (deliveryAddress.Trim() != string.Empty)
                                {
                                    if (client.Trim() != string.Empty)
                                    {
                                        builder.Append("Доставка для " + client + " по адресу: "); builder.Append(deliveryAddress + "/ ");
                                    }
                                    else { builder.Append("Доставка по адресу: "); builder.Append(deliveryAddress + "/ "); }
                                }
                                else
                                {
                                    if (client.Trim() != string.Empty)
                                    {
                                        builder.Append("Доставка для " + client + "/ ");
                                    }
                                    else
                                    {
                                        builder.Append("Доставка/ ");
                                    }

                                }

                            }
                        }
                        if (builder.Length > 0)
                        {
                            table0 = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                SpacingBefore = 10f
                            };
                            cell = new PdfPCell();
                            cell.AddElement(new Phrase("Дополнительные задачи: ", underline));
                            cell.AddElement(new Phrase(builder.Remove(builder.Length-2,2).ToString(), normal));
                            cell.BorderWidth = 0f;
                            table0.AddCell(cell);
                            document.Add(table0);
                        }






                        #region примечание
                        PdfPTable table_comment = new PdfPTable(1)
                        {
                            SpacingBefore = 0f,
                            //WidthPercentage = 100
                            TotalWidth = document.PageSize.Width // - (document.RightMargin + document.LeftMargin)
                        };
                        //table_comment.setTotalWidth((PageSize.A4.getWidth() - document.leftMargin() - document.rightMargin()) * table.getWidthPercentage() / 100);

                        cell = new PdfPCell();
                        string str = Utils.CheckDBNull.ToString(dataRow["comments"]).Replace(" ", string.Empty);
                        if (str != "")
                        {
                            cell.AddElement(new Phrase("Примечание:" + Environment.NewLine, underline));
                            cell.AddElement(new Phrase(Utils.CheckDBNull.ToString(dataRow["comments"]), normal));
                        }
                        else
                        {
                            cell.AddElement(new Phrase("Примечания нет." + Environment.NewLine, normal));
                        }
                        cell.BorderWidth = 0f;
                        table_comment.AddCell(cell);
                        #endregion


                        DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(Utils.CheckDBNull.ToLong(dataRow["time_recieve"]));
                        table_comment.CalculateHeightsFast();
                        float clearheight = (writer.GetVerticalPosition(true) + table_comment.TotalHeight);

                        if (clearheight < document.PageSize.Height - 100)
                        {
                            table_comment.WidthPercentage = 100;
                            document.Add(table_comment);
                            #region картинка

                            PdfPTable table_Preview0 = new PdfPTable(1)
                            {
                                SpacingBefore = 10f,
                                WidthPercentage = 100
                            };
                            //Bitmap previewBMP = (Bitmap)pictureBox_OrderPreview.Image;
                            iTextSharp.text.Image preview0 = null;
                            if (tmpenddate != null)
                            {
                                string OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + OrderId.ToString() + @"\index.png";
                                if (System.IO.File.Exists(OrderMainPath))
                                {
                                    preview0 = iTextSharp.text.Image.GetInstance(Utils.Converting.ByteToImage(System.IO.File.ReadAllBytes(OrderMainPath)), System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                else
                                {
                                    preview0 = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                            }
                            else
                            {
                                preview0 = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            //preview. = iTextSharp.text.Image.ALIGN_TOP;
                            cell = new PdfPCell();
                            //cell.Width =  
                            float pos0 = writer.GetVerticalPosition(true);
                            float PreviewHeight0 = pos0 - 50f;

                            if (PreviewHeight0 > 100)
                            {
                                cell.FixedHeight = PreviewHeight0;
                            }
                            else
                            {
                                if (PreviewHeight0 < preview0.Height)
                                {
                                    cell.FixedHeight = 800;
                                }
                                else
                                {
                                    cell.FixedHeight = PreviewHeight0;
                                }
                            }
                            //cell.Width = document.PageSize.Width - 20;
                            preview0.ScaleToFit(document.PageSize.Width - 20, cell.FixedHeight);
                            preview0.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                            cell.PaddingTop = 10f;
                            cell.BorderWidth = 0f;
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                            cell.AddElement(preview0);
                            table_Preview0.AddCell(cell);
                            document.Add(table_Preview0);
                            #endregion
                        }
                        else
                        {
                            #region картинка
                            PdfPTable table_Preview = new PdfPTable(1)
                            {
                                SpacingBefore = 10f,
                                WidthPercentage = 100
                            };
                            //Bitmap previewBMP = (Bitmap)pictureBox_OrderPreview.Image;
                            iTextSharp.text.Image preview = null;
                            if (tmpenddate != null)
                            {
                                string OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + OrderId.ToString() + @"\index.png";
                                if (System.IO.File.Exists(OrderMainPath))
                                {
                                    preview = iTextSharp.text.Image.GetInstance(Utils.Converting.ByteToImage(System.IO.File.ReadAllBytes(OrderMainPath)), System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                else
                                {
                                    preview = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                            }
                            else
                            {
                                preview = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            //preview. = iTextSharp.text.Image.ALIGN_TOP;
                            cell = new PdfPCell();
                            //cell.Width =  
                            float pos = writer.GetVerticalPosition(true);
                            float PreviewHeight = pos - 50f;

                            if (PreviewHeight > 100)
                            {
                                cell.FixedHeight = PreviewHeight;
                            }
                            else
                            {
                                if (PreviewHeight < preview.Height)
                                {
                                    cell.FixedHeight = 800;
                                }
                                else
                                {
                                    cell.FixedHeight = PreviewHeight;
                                }
                            }
                            //cell.Width = document.PageSize.Width - 20;
                            preview.ScaleToFit(document.PageSize.Width - 20, cell.FixedHeight);
                            preview.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                            cell.PaddingTop = 10f;
                            cell.BorderWidth = 0f;
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                            cell.AddElement(preview);
                            table_Preview.AddCell(cell);
                            document.Add(table_Preview);
                            table_comment.WidthPercentage = 100;
                            document.Add(table_comment);
                            #endregion
                        }












                        document.Close();
                        writer.Close();
                        cachePDF = ms.GetBuffer();
                    }

                    //byte[] openPDF = System.IO.File.ReadAllBytes(ofd.FileName);
                    int eofPos = -1;
                    for (int i = cachePDF.Length - 1; i >= 0; i--)
                    {
                        if (cachePDF[i] != 0)
                        {
                            eofPos = i;
                            break;
                        }
                    }
                    byte[] resultPDF = new byte[eofPos + 1];
                    Array.Copy(cachePDF, resultPDF, eofPos + 1);

                    return resultPDF;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания .PDF" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        #endregion

        #region Создать пдф из ProtoClasses.ProtoOrders.protoOrder
        public static byte[] CreatePDF_FromProtoOrder(ProtoClasses.ProtoOrders.protoOrder order)
        {
            try
            {


                //DataRow dataRow = SqlLite.Order.TableOrders.Select("id = " + order.id).SingleOrDefault();
                if (order != null)
                {


                    #region Setting material
                    string materialPrint = "Ошибка! Материал неопределен.";
                    string materialCut = "Ошибка! Материал неопределен.";
                    string materialCnc = "Ошибка! Материал неопределен.";
                    string equipPrint = "Ошибка! оборудование неопределено.";
                    string equipCut = "Ошибка! оборудование неопределено.";
                    string equipCnc = "Ошибка! оборудование неопределено.";
                    List<KeyValuePair<int, string>> mprint = SqlLite.Materials.DicMaterialPrint.Where(item => item.Key == order.material_print_id).ToList();
                    if (mprint != null && mprint.Count > 0) { materialPrint = mprint[0].Value; }

                    List<KeyValuePair<int, string>> mcut = SqlLite.Materials.DicMaterialCut.Where(item => item.Key == order.material_cut_id).ToList();
                    if (mcut != null && mcut.Count > 0) { materialCut = mcut[0].Value; }

                    List<KeyValuePair<int, string>> mcnc = SqlLite.Materials.DicMaterialCnc.Where(item => item.Key == order.material_cnc_id).ToList();
                    if (mcnc != null && mcnc.Count > 0) { materialCnc = mcnc[0].Value; }

                    List<KeyValuePair<int, string>> mprintters = SqlLite.Equip.DicPrinters.Where(item => item.Key == order.printers_id).ToList();
                    if (mprintters != null && mprintters.Count > 0) { equipPrint = mprintters[0].Value; }

                    List<KeyValuePair<int, string>> mcutters = SqlLite.Equip.DicCuters.Where(item => item.Key == order.cutters_id).ToList();
                    if (mcutters != null && mcutters.Count > 0) { equipCut = mcutters[0].Value; }

                    List<KeyValuePair<int, string>> mcncs = SqlLite.Equip.DicCncs.Where(item => item.Key == order.cncs_id).ToList();
                    if (mcncs != null && mcncs.Count > 0) { equipCnc = mcncs[0].Value; }
                    #endregion







                    byte[] cachePDF;
                    //MessageBox.Show("Начало формирования PDF-документа");
                    using (MemoryStream ms = new MemoryStream())
                    {
                        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 10, 10, 10, 10);

                        PdfWriter writer = PdfWriter.GetInstance(document, ms);

                        //string sylfaenpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf";
                        string sylfaenpath = Application.StartupPath + @"\sylfaen.ttf";
                        BaseFont sylfaen = BaseFont.CreateFont(sylfaenpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font head = new iTextSharp.text.Font(sylfaen, 12f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                        iTextSharp.text.Font normal = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                        iTextSharp.text.Font underline = new iTextSharp.text.Font(sylfaen, 10f, iTextSharp.text.Font.UNDERLINE, BaseColor.Black);
                        document.Open();
                        //document.Footer = new HeaderFooter(new );
                        document.Add(new Paragraph("Заявка №" + order.id.ToString() + " :" + order.work_name, head));
                        document.Add(new Paragraph(" ", normal));
                        Paragraph pr = new Paragraph("Отдано в обработку «_____»____________________ 20______ г.    Подпись (расшифровка)____________________________", normal)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        document.Add(pr);
                        pr = new Paragraph("      Отдано клиенту «_____»____________________ 20______ г.    Подпись (расшифровка)____________________________", normal)
                        {
                            Alignment = Element.ALIGN_CENTER
                        };
                        document.Add(pr);
                        document.Add(new Paragraph(" ", normal));
                        PdfPCell cell = new PdfPCell();
                        PdfPTable table0 = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Properties.Resources.blank_accent, System.Drawing.Imaging.ImageFormat.Bmp);
                        cell = new PdfPCell()
                        {
                            FixedHeight = 80f,
                            PaddingTop = 10f,
                            PaddingBottom = 10f,
                            BorderWidth = 0f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        logo.ScaleToFit(document.PageSize.Width - 20, 60f);
                        logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        cell.AddElement(logo);
                        table0.AddCell(cell);
                        document.Add(table0);
                        document.Add(new Paragraph(" ", normal));
                        document.Add(new Paragraph("Заявка №" + order.id.ToString() + " :" + order.work_name, head));
                        table0 = new PdfPTable(4)
                        {
                            WidthPercentage = 100,
                            SpacingBefore = 5f
                        };
                        cell = new PdfPCell();
                        cell.AddElement(new Phrase("Виды работ:" + Environment.NewLine, underline));

                        if (order.print_on) { cell.AddElement(new Phrase("Печать" + Environment.NewLine, normal)); }
                        if (order.cut_on)
                        {
                            if (order.cutting_on_print) { cell.AddElement(new Phrase("Плот. резка по меткам" + Environment.NewLine, normal)); }
                            else { cell.AddElement(new Phrase("Плот. резка" + Environment.NewLine, normal)); }

                        }

                        if (order.cnc_on)
                        {
                            if (order.cnc_on_print) { cell.AddElement(new Phrase("Фрезеровка по меткам" + Environment.NewLine, normal)); }
                            else { cell.AddElement(new Phrase("Фрезеровка" + Environment.NewLine, normal)); }
                        }
                        if (order.installation) { cell.AddElement(new Phrase("Монтажные работы" + Environment.NewLine, normal)); }

                        //if (Utils.CheckDBNull.ToBoolean(dataRow["cutting_on_print"])) { cell.AddElement(new Phrase("Фрезеровка по меткам" + Environment.NewLine, normal)); }
                        cell.Padding = 5f;
                        cell.BorderWidth = 0f;
                        table0.AddCell(cell);

                        cell = new PdfPCell
                        {
                            Colspan = 3
                        };
                        cell.AddElement(new Phrase("Наименование: " + order.work_name + Environment.NewLine, underline));
                        cell.AddElement(new Phrase("Заказчик: " + order.client + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Добавил: " + order.adder + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Начать: " + Utils.UnixDate.Int64ToDateTime(order.date_start).ToString() + Environment.NewLine, normal));
                        cell.AddElement(new Phrase("Сделать до: " + Utils.UnixDate.Int64ToDateTime(order.dead_line).ToString() + Environment.NewLine, normal));
                        cell.Padding = 5f;
                        cell.BorderWidth = 0f;
                        table0.AddCell(cell);

                        document.Add(table0);

                        table0 = new PdfPTable(3)
                        {
                            SpacingBefore = 10f,
                            WidthPercentage = 100
                        };
                        //table0.TotalWidth = document.PageSize.Width;
                        cell = new PdfPCell(new Phrase("Информация о печати:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Информация о плоттерной резке:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Информация о фрезеровке:", underline))
                        {
                            BorderWidth = 0f,
                            Padding = 1f,
                            HorizontalAlignment = PdfPCell.ALIGN_CENTER
                        };
                        table0.AddCell(cell);
                        string sizeText = ""; string countText = "";

                        if (order.print_on)
                        {
                            sizeText = order.size_x_print.ToString() + "x" + order.size_y_print.ToString() + " м.";
                            countText = order.count_print.ToString() + " шт.";
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipPrint + Environment.NewLine + "Материал: " + materialPrint + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Качество: " + order.print_quality, normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);
                        if (order.cut_on)
                        {
                            //if (Utils.CheckDBNull.ToBoolean(dataRow["print_on"])) { material = comboBox_MaterialPrint.Text; } else { material = comboBox_MaterialCut.Text; }
                            sizeText = order.size_x_cut.ToString() + "x" + order.size_y_cut.ToString() + " м.";
                            countText = order.count_cut.ToString() + " шт.";
                            if (order.cutting_on_print) { materialCut = materialPrint; }
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipCut + Environment.NewLine + "Материал: " + materialCut + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Линеатура всего: " + order.size_cut.ToString() + " м.", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);


                        if (order.cnc_on)
                        {
                            sizeText = order.size_x_cnc.ToString() + "x" + order.size_y_cnc.ToString() + " м.";
                            countText = order.count_cnc.ToString() + " шт.";
                            cell = new PdfPCell(new Phrase("Оборудование: " + equipCnc + Environment.NewLine + "Материал: " + materialCnc + Environment.NewLine + "Размер: " + sizeText + Environment.NewLine + "Количество: " + countText + Environment.NewLine + "Линеатура всего: " + order.size_cnc.ToString() + " м.", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_LEFT
                            };
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(Environment.NewLine + "НЕТ" + Environment.NewLine + " ", normal))
                            {
                                HorizontalAlignment = PdfPCell.ALIGN_CENTER
                            };
                        }
                        cell.BorderWidth = 0f;
                        cell.Padding = 1f;
                        cell.PaddingTop = 5f;
                        table0.AddCell(cell);

                        document.Add(table0);










                        StringBuilder builder = new StringBuilder();
                        if (order.laminat) { if (order.laminat_mat) { builder.Append("Ламинация мат./ "); } else { builder.Append("Ламинация глян./ "); } }
                        if (order.baner_handling) { if (order.baner_luvers) { builder.Append(" Люверсы через " + order.baner_handling_size.ToString() + "см./ "); } else { builder.Append("Карманы " + order.baner_handling_size.ToString() + "см./ "); } }
                        string worktypes_list_string = order.worktypes_list;
                        if (worktypes_list_string != string.Empty)
                        {
                            worktypes_list_string = worktypes_list_string.Replace(Convert.ToString((char)(219)), "/ ");
                            builder.Append(worktypes_list_string);
                        }
                        if (order.delivery)
                        {
                            string deliveryAddress = order.delivery_address;
                            if (order.delivery_office)
                            {
                                if (deliveryAddress.Trim() != string.Empty)
                                {
                                    builder.Append("Доставка в офис: "); builder.Append(deliveryAddress + "/ ");
                                }
                                else
                                {
                                    builder.Append("Доставка в офис/ ");
                                }
                            }
                            else
                            {
                                if (deliveryAddress.Trim() != string.Empty)
                                {
                                    if (order.client.Trim() != string.Empty)
                                    {
                                        builder.Append("Доставка для " + order.client + " по адресу: "); builder.Append(deliveryAddress + "/ ");
                                    }
                                    else { builder.Append("Доставка по адресу: "); builder.Append(deliveryAddress + "/ "); }
                                }
                                else
                                {
                                    if (order.client.Trim() != string.Empty)
                                    {
                                        builder.Append("Доставка для " + order.client + "/ ");
                                    }
                                    else
                                    {
                                        builder.Append("Доставка/ ");
                                    }

                                }

                            }
                        }
                        if (builder.Length > 0)
                        {
                            table0 = new PdfPTable(1)
                            {
                                WidthPercentage = 100,
                                SpacingBefore = 10f
                            };
                            cell = new PdfPCell();
                            cell.AddElement(new Phrase("Дополнительные задачи: ", underline));
                            cell.AddElement(new Phrase(builder.Remove(builder.Length - 2, 2).ToString(), normal));
                            cell.BorderWidth = 0f;
                            table0.AddCell(cell);
                            document.Add(table0);
                        }






                        #region примечание
                        PdfPTable table_comment = new PdfPTable(1)
                        {
                            SpacingBefore = 0f,
                            //WidthPercentage = 100
                            TotalWidth = document.PageSize.Width // - (document.RightMargin + document.LeftMargin)
                        };
                        //table_comment.setTotalWidth((PageSize.A4.getWidth() - document.leftMargin() - document.rightMargin()) * table.getWidthPercentage() / 100);

                        cell = new PdfPCell();
                        string str = order.comments.Replace(" ", string.Empty);
                        if (str != "")
                        {
                            cell.AddElement(new Phrase("Примечание:" + Environment.NewLine, underline));
                            cell.AddElement(new Phrase(order.comments, normal));
                        }
                        else
                        {
                            cell.AddElement(new Phrase("Примечания нет." + Environment.NewLine, normal));
                        }
                        cell.BorderWidth = 0f;
                        table_comment.AddCell(cell);
                        #endregion


                        DateTime tmpenddate = Utils.UnixDate.Int64ToDateTime(order.time_recieve);
                        table_comment.CalculateHeightsFast();
                        float clearheight = (writer.GetVerticalPosition(true) + table_comment.TotalHeight);

                        if (clearheight < document.PageSize.Height - 100)
                        {
                            table_comment.WidthPercentage = 100;
                            document.Add(table_comment);
                            #region картинка

                            PdfPTable table_Preview0 = new PdfPTable(1)
                            {
                                SpacingBefore = 10f,
                                WidthPercentage = 100
                            };
                            //Bitmap previewBMP = (Bitmap)pictureBox_OrderPreview.Image;
                            iTextSharp.text.Image preview0 = null;
                            if (tmpenddate != null)
                            {
                                string OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + order.id.ToString() + @"\index.png";
                                if (System.IO.File.Exists(OrderMainPath))
                                {
                                    preview0 = iTextSharp.text.Image.GetInstance(Utils.Converting.ByteToImage(System.IO.File.ReadAllBytes(OrderMainPath)), System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                else
                                {
                                    preview0 = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                            }
                            else
                            {
                                preview0 = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            //preview. = iTextSharp.text.Image.ALIGN_TOP;
                            cell = new PdfPCell();
                            //cell.Width =  
                            float pos0 = writer.GetVerticalPosition(true);
                            float PreviewHeight0 = pos0 - 50f;

                            if (PreviewHeight0 > 100)
                            {
                                cell.FixedHeight = PreviewHeight0;
                            }
                            else
                            {
                                if (PreviewHeight0 < preview0.Height)
                                {
                                    cell.FixedHeight = 800;
                                }
                                else
                                {
                                    cell.FixedHeight = PreviewHeight0;
                                }
                            }
                            //cell.Width = document.PageSize.Width - 20;
                            preview0.ScaleToFit(document.PageSize.Width - 20, cell.FixedHeight);
                            preview0.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                            cell.PaddingTop = 10f;
                            cell.BorderWidth = 0f;
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                            cell.AddElement(preview0);
                            table_Preview0.AddCell(cell);
                            document.Add(table_Preview0);
                            #endregion
                        }
                        else
                        {
                            #region картинка
                            PdfPTable table_Preview = new PdfPTable(1)
                            {
                                SpacingBefore = 10f,
                                WidthPercentage = 100
                            };
                            //Bitmap previewBMP = (Bitmap)pictureBox_OrderPreview.Image;
                            iTextSharp.text.Image preview = null;
                            if (tmpenddate != null)
                            {
                                string OrderMainPath = Utils.Settings.set.data_path + @"\makets\" + tmpenddate.ToString("yyyy.MM") + @"\" + order.id.ToString() + @"\index.png";
                                if (System.IO.File.Exists(OrderMainPath))
                                {
                                    preview = iTextSharp.text.Image.GetInstance(Utils.Converting.ByteToImage(System.IO.File.ReadAllBytes(OrderMainPath)), System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                                else
                                {
                                    preview = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                                }
                            }
                            else
                            {
                                preview = iTextSharp.text.Image.GetInstance(Properties.Resources.pic, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            //preview. = iTextSharp.text.Image.ALIGN_TOP;
                            cell = new PdfPCell();
                            //cell.Width =  
                            float pos = writer.GetVerticalPosition(true);
                            float PreviewHeight = pos - 50f;

                            if (PreviewHeight > 100)
                            {
                                cell.FixedHeight = PreviewHeight;
                            }
                            else
                            {
                                if (PreviewHeight < preview.Height)
                                {
                                    cell.FixedHeight = 800;
                                }
                                else
                                {
                                    cell.FixedHeight = PreviewHeight;
                                }
                            }
                            //cell.Width = document.PageSize.Width - 20;
                            preview.ScaleToFit(document.PageSize.Width - 20, cell.FixedHeight);
                            preview.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                            cell.PaddingTop = 10f;
                            cell.BorderWidth = 0f;
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                            cell.AddElement(preview);
                            table_Preview.AddCell(cell);
                            document.Add(table_Preview);
                            table_comment.WidthPercentage = 100;
                            document.Add(table_comment);
                            #endregion
                        }

                        document.Close();
                        writer.Close();
                        cachePDF = ms.GetBuffer();
                    }

                    //byte[] openPDF = System.IO.File.ReadAllBytes(ofd.FileName);
                    int eofPos = -1;
                    for (int i = cachePDF.Length - 1; i >= 0; i--)
                    {
                        if (cachePDF[i] != 0)
                        {
                            eofPos = i;
                            break;
                        }
                    }
                    byte[] resultPDF = new byte[eofPos + 1];
                    Array.Copy(cachePDF, resultPDF, eofPos + 1);

                    return resultPDF;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания .PDF" + Environment.NewLine + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
#endregion


    }
}
