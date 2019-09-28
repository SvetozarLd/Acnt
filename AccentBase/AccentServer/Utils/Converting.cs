using System;
using System.Drawing;
using System.IO;

namespace AccentServer.Utils
{
    class Converting
    {
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        public static byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        public static Image ByteToImage(byte[] img)
        {
            if (img != null)
            {
                using (MemoryStream mStream = new MemoryStream(img))
                {
                    return Image.FromStream(mStream);
                }
            }
            else { return null; }
        }

        public static byte[] GetBytesFromImage(string imageFile)
        {
            MemoryStream ms = new MemoryStream();
            if (File.Exists(imageFile))
            {
                Image img = Image.FromFile(imageFile);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }else { return null; }

        }

        public static long GetDateOfFile(string imageFile)
        {
            MemoryStream ms = new MemoryStream();
            if (File.Exists(imageFile))
            {
                DateTime dt = File.GetLastWriteTime(imageFile);
                return UnixDate.DateTimeToInt64(dt);
            }
            else { return 0; }

        }

        //public string GetPathByID(string time_recieve)
        //{
        //    DateTime tmpenddate;

        //    if (DateTime.TryParse(Convert.ToString(rows[0]["time_recieve"]), out tmpenddate))
        //    {
        //        string OrderFolder = tmpenddate.ToString("yyyy.MM") + "/" + OrderId.ToString() + "/preview/";
        //        Form FTPOperationForm = new Form_OrderFtpOperation(TempFile, 1, "/makets/" + OrderFolder, OrderId);
        //        FTPOperationForm.ShowInTaskbar = true;
        //        FTPOperationForm.StartPosition = FormStartPosition.CenterScreen;
        //        FTPOperationForm.Owner = this;
        //        FTPOperationForm.FormClosed += (obj, arg) =>
        //        {
        //            GetFiles();
        //        };
        //        FTPOperationForm.Show(this);
        //    }



        //    return string.Empty;
        //}
        public static string LengthToString(double Length)
        {
            string fileSize = string.Empty; double FileSize = Length;
            if (FileSize > 0)
            {
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Kb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Mb"; }
                if (FileSize >= 1024) { FileSize = FileSize / 1024; fileSize = " Gb"; }
            }
            FileSize = Math.Round(FileSize, 2);
            return FileSize.ToString() + fileSize;
        }

    }
}