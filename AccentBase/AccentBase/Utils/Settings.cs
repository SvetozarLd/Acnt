using System;
using System.IO;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
using System.Windows.Forms;
//using System.Threading;
//using System.Text;
//using System.Deployment.Application;
//using System.Reflection;

namespace AccentBase.Utils
{
    public static class Settings
    {
        public static ProtoClasses.ProtoSettings.protoSet set;
        public static VersionInfo.protoSet versionInfo;
        private static readonly string path = Application.StartupPath + @"\settings.dat";
        private static DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
        //static string name = @"\settings.dat";
        public static string UniqueId { get; set; }
        static Settings()
        {
        }

        internal static Exception Save()
        {
            try
            {
                if (File.Exists(path)) { File.Delete(path); }
                if (set != null)
                {
                    ProtoClasses.ProtoSettings ps = new ProtoClasses.ProtoSettings();
                    byte[] sett = ps.Serialize(set);
                    File.WriteAllBytes(path, sett);
                    return null;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (Exception ex)
            {
                set = new ProtoClasses.ProtoSettings.protoSet
                {
                    buffer_size = 16384,
                    data_path = di.Parent.FullName,
                    name = HWSerials.GetUserName(),
                    server_port = 4900,
                    View_OrderCustomer = true,
                    View_OrderHistory = true,
                    View_OrderListInfo = false,
                    View_OrderManager = true,
                    View_OrderMaterial = true,
                    View_OrderNotes = true,
                    View_OrderPreview = true,
                    View_OrderTimeEnd = true,
                    View_OrderTimeStart = true,
                    View_OrderWorkTypes = true,
                    View_QuickSearch = true,
                    View_ToolStrip = true,
                    View_Worktypes = true,
                    View_Maximize = false,
                    View_SocketSend = true,
                    View_OrdersAll = true,
                    View_OrderPrint = true,
                    View_OrderCut = true,
                    View_OrdersCnc = true,
                    View_OrdersInstall = true
                };
                if (versionInfo != null && versionInfo.serveraddress != null && !versionInfo.serveraddress.Equals(string.Empty)) { set.server_address = versionInfo.serveraddress; } else { set.server_address = "";}
                if (versionInfo != null && versionInfo.ftplogin != null && !versionInfo.ftplogin.Equals(string.Empty)) { set.FtpLogin = versionInfo.ftplogin; } else { set.FtpLogin = ""; }
                if (versionInfo != null && versionInfo.ftppass != null && !versionInfo.ftppass.Equals(string.Empty)) { set.FtpPass = versionInfo.ftppass; } else { set.FtpPass = ""; }
                return ex;
            }
        }
        internal static Exception Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    ProtoClasses.ProtoSettings ps = new ProtoClasses.ProtoSettings();
                    byte[] sett = File.ReadAllBytes(path);
                    set = ps.Deserialize(sett);
                    if (versionInfo != null && versionInfo.serveraddress != null && !versionInfo.serveraddress.Equals(string.Empty)) { set.server_address = versionInfo.serveraddress; } else { set.server_address = ""; }
                    if (versionInfo != null && versionInfo.ftplogin != null && !versionInfo.ftplogin.Equals(string.Empty)) { set.FtpLogin = versionInfo.ftplogin; } else { set.FtpLogin = ""; }
                    if (versionInfo != null && versionInfo.ftppass != null && !versionInfo.ftppass.Equals(string.Empty)) { set.FtpPass = versionInfo.ftppass; } else { set.FtpPass = ""; }
                }
                else
                {
                    set = new ProtoClasses.ProtoSettings.protoSet
                    {
                        buffer_size = 16384,
                        data_path = di.Parent.FullName,
                        name = HWSerials.GetUserName(),
                        server_port = 4900,
                        View_OrderCustomer = true,
                        View_OrderHistory = true,
                        View_OrderListInfo = false,
                        View_OrderManager = true,
                        View_OrderMaterial = true,
                        View_OrderNotes = true,
                        View_OrderPreview = true,
                        View_OrderTimeEnd = true,
                        View_OrderTimeStart = true,
                        View_OrderWorkTypes = true,
                        View_QuickSearch = true,
                        View_ToolStrip = true,
                        View_Worktypes = true,
                        View_Maximize = false,
                        View_SocketSend = true,
                        View_OrdersAll = true,
                        View_OrderPrint = true,
                        View_OrderCut = true,
                        View_OrdersCnc = true,
                        View_OrdersInstall = true
                    };
                    if (versionInfo != null && versionInfo.serveraddress != null && !versionInfo.serveraddress.Equals(string.Empty)) { set.server_address = versionInfo.serveraddress; } else { set.server_address = ""; }
                    if (versionInfo != null && versionInfo.ftplogin != null && !versionInfo.ftplogin.Equals(string.Empty)) { set.FtpLogin = versionInfo.ftplogin; } else { set.FtpLogin = ""; }
                    if (versionInfo != null && versionInfo.ftppass != null && !versionInfo.ftppass.Equals(string.Empty)) { set.FtpPass = versionInfo.ftppass; } else { set.FtpPass = ""; }
                    try
                    {
                        ProtoClasses.ProtoSettings ps = new ProtoClasses.ProtoSettings();
                        byte[] sett = ps.Serialize(set);
                        File.WriteAllBytes(path, sett);
                    }
                    catch (Exception ex1)
                    { }
                }
                return null;
            }
            catch (Exception ex)
            {
                set = new ProtoClasses.ProtoSettings.protoSet
                {
                    buffer_size = 16384,
                    data_path = di.Parent.FullName,
                    name = HWSerials.GetUserName(),
                    server_port = 4900,
                    View_OrderCustomer = true,
                    View_OrderHistory = true,
                    View_OrderListInfo = false,
                    View_OrderManager = true,
                    View_OrderMaterial = true,
                    View_OrderNotes = true,
                    View_OrderPreview = true,
                    View_OrderTimeEnd = true,
                    View_OrderTimeStart = true,
                    View_OrderWorkTypes = true,
                    View_QuickSearch = true,
                    View_ToolStrip = true,
                    View_Worktypes = true,
                    View_Maximize = false,
                    View_SocketSend = true,
                    View_OrdersAll = true,
                    View_OrderPrint = true,
                    View_OrderCut = true,
                    View_OrdersCnc = true,
                    View_OrdersInstall = true
                };
                if (versionInfo != null && versionInfo.serveraddress != null && !versionInfo.serveraddress.Equals(string.Empty)) { set.server_address = versionInfo.serveraddress; } else { set.server_address = ""; }
                if (versionInfo != null && versionInfo.ftplogin != null && !versionInfo.ftplogin.Equals(string.Empty)) { set.FtpLogin = versionInfo.ftplogin; } else { set.FtpLogin = ""; }
                if (versionInfo != null && versionInfo.ftppass != null && !versionInfo.ftppass.Equals(string.Empty)) { set.FtpPass = versionInfo.ftppass; } else { set.FtpPass = ""; }
                try
                {
                    ProtoClasses.ProtoSettings ps = new ProtoClasses.ProtoSettings();
                    byte[] sett = ps.Serialize(set);
                    File.WriteAllBytes(path, sett);
                }
                catch(Exception ex1)
                { }
                return ex;
            }
        }
    }
}
