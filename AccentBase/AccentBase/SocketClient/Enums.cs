using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentBase.SocketClient
{
    internal static partial class TableClient
    {
        public enum SocketMessageCommand : int
        {
            None = 0,
            Error = 1,
            ServerStart = 2,
            ServerStop = 3,
            ConnectOn = 4,
            ConnectLose = 5,
            RowsChangeCounts = 6,
            RowsChangeCountsProcess = 7,
            RowsChangeCountsSend = 8,
            RowsInsert = 9,
            RowsUpdate = 10,
            RowsDelete = 11,
            RowsChangeState = 12,
            GetFilesList = 13,
            GetAllPreviewsList = 14,
            OrderChangeStates = 15,
            DownloadOrderFiles = 16,
            CopyOrder = 17
            //
            //LineStatus = 0,
            //Authorization = 1,
            //Exceptions = 3,
            //ConnectLose = 4,
            //ConnectOn = 5,
            //RowsInsert = 6,
            //RowsUpdate = 7,
            //RowsDelete = 8,
            //RowsChangeState = 9,
            //RowsChangeStateCopy = 10,
            //HWUID = 11,
            //UserAutorized = 12,
            //ServerStart = 13,
            //ServerStop = 14,
            //Error = 15,
            //Ok = 16,
            //OnRecieve = 17,
            //HWRegistered = 18,
            //HWNewRegistered = 19,
            //HWNewWait = 20,
            //HWChangeRegistered = 21,
            //HWChangeWait = 22,
            //HWNew = 23,
            //HWNotFound = 24,
            //HWOk = 25,
            //HWWaitRegister = 26,
            //Request_ChangingTable = 27,
            //ChangeTable = 30,
            //ChangeTableOK = 31,
            //ChangeTableError = 32,
            //AccountMultiWork = 33,
            //SecurityError = 34,
            //NONE = 35,
            //log = 200
        }
        public enum TableName : int
        {
            none = 0,
            TableBase = 1,
            TableChat = 2,
            TableCncs = 3,
            TableCutters = 4,
            TablePrinters = 5,
            TableMaterialCnc = 6,
            TableMaterialCut = 7,
            TableMaterialPrint = 8,
            TableNoteStateChange = 9
        }
        public enum iconClient : int
        {
            SendIcon = 0,
            RecieveStart = 1,
            RecieveProgress = 2,
            RecieveEnd = 3,
            Offline = 4,
            NormalIcon = 5
        }
    }
}
