using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccentBase_Updater
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //if (args == null || args.Length == 0)
            //{
            //    try
            //    {
            //        args = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
            //    }
            //    catch { }
            //}
            bool autoupdate = false;
            if (args!= null && args.Length > 0 && args[0].Contains("auto"))
            {
                autoupdate = true;
            }
            Application.Run(new Form_Main(autoupdate));
        }
    }
}
