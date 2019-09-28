using System;
using System.Drawing;
using System.IO;

namespace AccentBase.Utils
{
    class Converting
    {
        internal static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        internal static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        internal static string FirstLetterToUpper(string str)
        {
            str = str.Trim();
            if (str == string.Empty) { return string.Empty; }


            if (str.Length > 1) { return char.ToUpper(str[0]) + str.Substring(1); }

            return str.ToUpper();
        }

        internal static byte[] ImageToByte(Image preview)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                preview.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        internal static Image ByteToImage(byte[] value)
        {
            if (value != null)
            {
                using (MemoryStream mStream = new MemoryStream(value))
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
            }
            else { return null; }

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
    }
}