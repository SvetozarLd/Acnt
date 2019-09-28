using System;
using System.Collections.Generic;
using System.Data;


namespace AccentBase.SqlLite
{
    static class SqlCounters
    {
        public class SqlCoutersArgs : EventArgs
        {
            public ProtoClasses.ProtoCounters.protoList clist { get; set; }
            public Exception ex { get; set; }
            public SqlCoutersArgs(ProtoClasses.ProtoCounters.protoList CList, Exception Ex) { clist = CList; ex = Ex; }
        }

        static public SqlCoutersArgs GetCounters(SqlEvent.TableName tn)
        {
            try
            {
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, 0, 0, SqlEvent.SqLiteEvent.GetRowTimeCountStart, (int)tn, true));
                ProtoClasses.ProtoCounters.protoList clist = new ProtoClasses.ProtoCounters.protoList();
                clist.plist = new Dictionary<int, long>();
                //string tablename = string.Empty;
                //string filename = string.Empty;
                DataTable dt = null;
                switch (tn)
                {
                    case SqlEvent.TableName.TableOrders:
                        dt = Order.TableOrders;
                        break;
                    case SqlEvent.TableName.TableMaterialPrint:
                        dt = Materials.TableMaterialPrint;
                        break;
                    case SqlEvent.TableName.TableMaterialCut:
                        dt = Materials.TableMaterialCut;
                        break;
                    case SqlEvent.TableName.TableMaterialCnc:
                        dt = Materials.TableMaterialCnc;
                        break;
                    case SqlEvent.TableName.TablePrinters:
                        dt = Equip.TablePrinters;
                        break;
                    case SqlEvent.TableName.TableCutters:
                        dt = Equip.TableCutters;
                        break;
                    case SqlEvent.TableName.TableCncs:
                        dt = Equip.TableCncs;
                        break;
                    case SqlEvent.TableName.TableNoteStateChange:
                        dt = OrderHistory.TableOrderHistory;
                        break;
                }
                int i = 0;
                if (dt != null)
                {

                    foreach (DataRow rows in dt.Rows)
                    {
                        if (!SocketClient.TableClient.IsConnected)
                        {
                            //SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, i, 0, SqlEvent.SqLiteEvent.GetRowTimeCountEnd, (int)SqlEvent.TableName.TableOrders, true));
                            return new SqlCoutersArgs(null, new ApplicationException("SqlCounters: Отключение от сервера"));
                        }
                        clist.plist.Add(Convert.ToInt32(rows["id"]), Convert.ToInt64(rows["change_count"]));
                    }
                    i = dt.Rows.Count;
                }
                SqlEvent.OnSEND(new SqlEvent.SqliteUpdateEventArgs(0, i, 0, SqlEvent.SqLiteEvent.GetRowTimeCountEnd, (int)tn, true));
                return new SqlCoutersArgs(clist, null);
            }
            catch (Exception ex)
            {
                return new SqlCoutersArgs(null, ex);
            }


        }
    }
}
