using System;
using System.Collections.Generic;
using System.Management;
using System.Net.NetworkInformation;
using System.Linq;

namespace AccentBase.Utils
{
    static class HWSerials
    {
        //static ManagementObjectSearcher searcher;

        static internal string GetUserName()
        {
            //try
            //{

            //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            //    ManagementObjectCollection collection = searcher.Get();
            //    string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];

            //    //ComputerName
            //    searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT UserName FROM Win32_ComputerSystem");
            //    foreach (ManagementObject queryObj in searcher.Get()) { return queryObj["UserName"].ToString();}
            //    return string.Empty;
            //}
            //catch { return string.Empty; }
            return Environment.UserName;
        }
    }
}
