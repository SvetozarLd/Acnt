using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AccentBase.Utils
{
    public static class FileNamesValidation
    {
        public static string GetValidPath(string invalidPath)
        {
            //if (invalidPath == null || invalidPath == string.Empty || RemoveSpaces(invalidPath)!= string.Empty)
            //{
            //    return string.Empty;
            //}
            string validPath = "";
            StringBuilder sb = new StringBuilder(invalidPath);

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                if (invalidPath.Contains(c.ToString()))
                {
                    //if (c.ToString() != @"\")
                    //{
                    sb.Replace(c.ToString(), "");
                    //}
                }
            }
            foreach (char c in Path.GetInvalidPathChars())
            {
                if (invalidPath.Contains(c.ToString()))
                {
                    //if (c.ToString() != @"\")
                    //{
                    sb.Replace(c.ToString(), "");
                    //}
                }
            }
            validPath = sb.ToString();
            return validPath;
        }
        public static string RemoveSpaces(string inputString)
        {
            inputString = inputString.Replace("  ", string.Empty);
            inputString = inputString.Trim().Replace(" ", string.Empty);

            return inputString;
        }
    }
}
