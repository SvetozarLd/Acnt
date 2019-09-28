using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace AccentBase.Utils
{
    public static class FileUtils
    {
        public static bool TryDelete(string FileFullName)
        {
            try
            {
                File.Delete(FileFullName);
                return true;
            }
            catch { return false; }
        }
    }
}
