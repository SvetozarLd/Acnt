using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using System.Net;
using System.Threading;
using System.Diagnostics;
namespace AccentBase.Ftp
{
    static public class FTPClient
    {
        static Thread thread;
        static public bool working { get; set; }
        static public bool DaemonMustWork { get; set; }
        static FtpReciever ft = new FtpReciever();
        public delegate void StopEventHandler(object sender, bool e);
        public static event StopEventHandler Stopped;
        static private void StopFtpDaemon(bool e) { Stopped?.Invoke(null, e); }

        static FTPClient()
        {
            DaemonMustWork = true;
        }



        static public void Stop()
        {

            StopFtpDaemon(true);
            ft.ToWork = false;
            DaemonMustWork = false;
            working = false;
        }


        static public bool Start()
        {
            if (!working)
            {
                DaemonMustWork = true;
                working = true;
                StopFtpDaemon(false);
                thread = new Thread(daemon);
                thread.IsBackground = true;
                thread.Name = "AccentBase FTP Daemon";
                thread.Start();

            }

            return true;
        }



        static void daemon()
        {
            //working = true;

            //while (DaemonMustWork)
            //{
            SqlLite.FtpSchedule.AddFtpEvent(ft);
            ft.Transmitefile();

            //    //Trace.WriteLine(working.ToString());
            //}
            //working = false;

        }

        //private static void FTPClient_Stop(object sender, bool e)
        //{
        //    ft.ToWork = false;
        //    working = false;
        //    DaemonMustWork = false;
        //}
    }
}
