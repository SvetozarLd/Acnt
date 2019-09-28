using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentServer.MySql
{
    static class Set
    {
        public enum TableName : int
        {
            none = 0,
            TableOrders = 1,
            TableChat = 2,
            TableCncs = 3,
            TableCutters = 4,
            TablePrinters = 5,
            TableMaterialCnc = 6,
            TableMaterialCut = 7,
            TableMaterialPrint = 8,
            TableNoteStateChange = 9,
            TableFiles = 10
        }
    }
}
