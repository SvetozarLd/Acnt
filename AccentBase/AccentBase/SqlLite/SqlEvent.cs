using System;
using System.Collections.Generic;


namespace AccentBase.SqlLite
{
    static internal class SqlEvent
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
            TableNoteStateChange = 9
        }

        public enum SqLiteEvent : int
        {
            None = 0,
            LoadTableStart = 1,
            LoadTableProcess = 2,
            LoadTableEnd = 3,
            UpdateRowsStart = 4,
            UpdateRowsProcess = 5,
            UpdateRowsEnd = 6,
            GetRowTimeCountStart = 7,
            GetRowTimeCountProcess = 8,
            GetRowTimeCountEnd = 9,
            ServerStartProcess = 10,
            ServerWaitProcess = 11,
            ServerEndProcess = 12
        }

        public class SqliteUpdateEventArgs : EventArgs
        {
            public int procent { get; set; } //процент выполненного задания
            public int count { get; set; } // кол-во всего
            public int index { get; set; } // Текущая позиция выполнения
            public SqLiteEvent sqliteEvent { get; set; } // Состояние
            public int tablename { get; set; } // Имя таблицы в БД
            public bool totalUnit { get; set; } // состояние блока или одной части
            public string guiDataBase { get; set; } // Имя БД для показа юзеру            
            public SqliteUpdateEventArgs(int Procent, int Count, int Index, SqLiteEvent sqLiteEvent, int tableName, bool TotalUnit)
            {
                procent = Procent;
                count = Count;
                index = Index;
                sqliteEvent = sqLiteEvent;
                tablename = tableName;
                totalUnit = TotalUnit;
            }
        }

        public delegate void UpdateEventHandler(object sender, SqliteUpdateEventArgs e);
        public static event UpdateEventHandler UpdateEvent;
        static public void OnSEND(SqliteUpdateEventArgs e)
        {
            UpdateEvent?.Invoke(null, e);
        }


        #region Событие обновления заявки
        public class OrderUpdateEventArgs : EventArgs
        {
            public object[] Row { get; set; }
            public long Id { get; set; }
            public SocketClient.TableClient.SocketMessageCommand Command { get; set; }
            public List<ProtoClasses.ProtoOrderHistory.protoRow> HistoryList { get; set; }
            public OrderUpdateEventArgs(long id, object[] row, SocketClient.TableClient.SocketMessageCommand socketMessageCommand, List<ProtoClasses.ProtoOrderHistory.protoRow> historyList)
            {
                Row = row;
                Command = socketMessageCommand;
                Id = id;
                HistoryList = historyList;
            }
        }
        public delegate void OrderUpdateEventDelegate(object sender, OrderUpdateEventArgs e);
        public static event OrderUpdateEventDelegate OrderUpdate;
        static public void OnSENDOrderUpdate(OrderUpdateEventArgs e)
        {
            OrderUpdate?.Invoke(null, e);
        }
        #endregion


        #region Событие изменения статусов заявкок
        public delegate void OrdersChangeStatesDelegate(object sender, ProtoClasses.ProtoOrdersChangeState.protoRowsList e);
        public static event OrdersChangeStatesDelegate OrderChangeStates;
        static public void OnSENDOrderChangeStates(ProtoClasses.ProtoOrdersChangeState.protoRowsList e)
        {
            OrderChangeStates?.Invoke(null, e);
        }
        #endregion








        static public string EnumToName(int tn)
        {
            switch (tn)
            {
                case (int)TableName.TableOrders:
                    return "Таблица заявок";
                case (int)TableName.TableMaterialPrint:
                    return "Таблица материалов для печати";
                case (int)TableName.TableMaterialCut:
                    return "Таблица материалов для плот. резки";
                case (int)TableName.TableMaterialCnc:
                    return "Таблица материалов для фрезеровки";
                case (int)TableName.TablePrinters:
                    return "Таблица печатного оборудования";
                case (int)TableName.TableCutters:
                    return "Таблица оборудования для плот. резки";
                case (int)TableName.TableCncs:
                    return "Таблица фрезерно-гравировального оборудования";
                case (int)TableName.TableNoteStateChange:
                    return "Таблица истории заявок";
                default:
                    return "Неизвестная таблица О_о";
            }
        }
    }
}
