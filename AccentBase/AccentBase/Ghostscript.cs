using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AccentBase
{
    class Ghostscript
    {
        //

        //private static void CallAPI(string[] args)
        //{
        //    IntPtr ptr;
        //    CreateAPIInstance(out ptr, IntPtr.Zero);
        //    InitAPI(ptr, args.Length, args);
        //    Cleanup(ptr);
        //}

        //private static void Cleanup(IntPtr gsInstancePtr)
        //{
        //    ExitAPI(gsInstancePtr);
        //    DeleteAPIInstance(gsInstancePtr);
        //}


        public const int GS_ARG_ENCODING_LOCAL = 0;
        public const int GS_ARG_ENCODING_UTF8 = 1;

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        //[DllImport("gsdll32.dll")]
        private static extern int gsapi_new_instance(out IntPtr inst, IntPtr handle);

        [DllImport("gsdll32.dll")]
        private static extern int gsapi_set_arg_encoding(IntPtr inst, int encoding);

        [DllImport("gsdll32.dll")]
        private static extern int gsapi_init_with_args(IntPtr inst, int argc, IntPtr[] argv);

        [DllImport("gsdll32.dll")]
        private static extern int gsapi_exit(IntPtr inst);

        [DllImport("gsdll32.dll")]
        private static extern void gsapi_delete_instance(IntPtr inst);

        private static void checkReturnValue(int retval)
        {
            if (retval != 0) MessageBox.Show("retval!=0");
            //throw ...; // implement error handling here

        }

        public static void run(string[] argv)
        {
            IntPtr inst;
            checkReturnValue(gsapi_new_instance(out inst, IntPtr.Zero));
            try
            {
                IntPtr[] utf8argv = new IntPtr[argv.Length];
                for (int i = 0; i < utf8argv.Length; i++)
                    utf8argv[i] = NativeUtf8FromString(argv[i]);
                try
                {
                    checkReturnValue(gsapi_set_arg_encoding(inst, GS_ARG_ENCODING_UTF8));
                    checkReturnValue(gsapi_init_with_args(inst, utf8argv.Length, utf8argv));
                    checkReturnValue(gsapi_exit(inst));
                }
                finally
                {
                    for (int i = 0; i < utf8argv.Length; i++)
                        Marshal.FreeHGlobal(utf8argv[i]);
                }
            }
            finally
            {
                gsapi_delete_instance(inst);
            }
        }

        public static IntPtr NativeUtf8FromString(string managedString)
        {
            int len = Encoding.UTF8.GetByteCount(managedString);
            byte[] buffer = new byte[len + 1]; // null-terminator allocated
            Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }
    }
}
