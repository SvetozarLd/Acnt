using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

namespace AccentServer.MySql
{
    class Equip
    {
        static Thread thread;
        static string ConnectionString = "";

        #region Начально - чтения БД в DataTable
        static internal Exception StartReadTables()
        {
            try
            {

                MySql.DataTables.oldstock_printers = new ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow>() ;
                MySql.DataTables.oldstock_cutters = new ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow>() ;
                MySql.DataTables.oldstock_cncs = new ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow>();

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();


                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM printers", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            AccentServer.ProtoClasses.ProtoEquip.protoRow row = new AccentServer.ProtoClasses.ProtoEquip.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.equip_name = Utils.CheckDBNull.ToString(rdr["printer_name"]);
                            row.price = Utils.CheckDBNull.ToInt32(rdr["price"]);
                            row.t_position = Utils.CheckDBNull.ToInt32(rdr["t_position"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_printers.TryAdd(row.id, row);
                        }
                    }

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM cutters", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            AccentServer.ProtoClasses.ProtoEquip.protoRow row = new AccentServer.ProtoClasses.ProtoEquip.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.equip_name = Utils.CheckDBNull.ToString(rdr["cutter_name"]);
                            row.price = Utils.CheckDBNull.ToInt32(rdr["price"]);
                            row.t_position = Utils.CheckDBNull.ToInt32(rdr["t_position"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_printers.TryAdd(row.id, row);
                        }
                    }

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM cutters", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            AccentServer.ProtoClasses.ProtoEquip.protoRow row = new AccentServer.ProtoClasses.ProtoEquip.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.equip_name = Utils.CheckDBNull.ToString(rdr["cnc_name"]);
                            row.price = Utils.CheckDBNull.ToInt32(rdr["price"]);
                            row.t_position = Utils.CheckDBNull.ToInt32(rdr["t_position"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_printers.TryAdd(row.id, row);
                        }
                    }



                    //if (DataTables.oldstock_printers == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM printers", connection))
                    //    {
                    //        DataTables.oldstock_printers = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.oldstock_printers);
                    //    }
                    //}
                    //if (DataTables.oldstock_cutters == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM cutters", connection))
                    //    {
                    //        DataTables.oldstock_cutters = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.oldstock_cutters);
                    //    }
                    //}
                    //if (DataTables.oldstock_cncs == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM cncs", connection))
                    //    {
                    //        DataTables.oldstock_cncs = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.oldstock_cncs);
                    //    }
                    //}
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        #endregion

        #region Получение от клиента списка последних изменений строк, отдача изменений.
        public List<ProtoClasses.ProtoEquip.protoRow> SelectLastChanges(Dictionary<int, long> pl, SocketServer.ClientInfo client, Set.TableName tn)
        {
            bool breakclient = false;
            if (pl == null) { pl = new Dictionary<int, long>(); }

            List<ProtoClasses.ProtoEquip.protoRow> outList = new List<ProtoClasses.ProtoEquip.protoRow>();

            ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow> dt = null;
            switch (tn)
            {
                case Set.TableName.TablePrinters:
                    dt = DataTables.oldstock_printers;
                    break;
                case Set.TableName.TableCutters:
                    dt = DataTables.oldstock_cutters;
                    break;
                case Set.TableName.TableCncs:
                    dt = DataTables.oldstock_cncs;
                    break;
            }


            int rowsCount = dt.Count;
            int rowsCurrentIndex = 0;
            int percentCurrent = 0;
            int percentLast = 0;
            foreach (AccentServer.ProtoClasses.ProtoEquip.protoRow rdr in dt.Values)
            {


                long timecount = 0;
                //DateTime dttemp = Convert.ToDateTime(rdr["time_recieve_edit"]);
                //long servertimacount = Utils.UnixDate.CheckedDateTimeToInt64(dttemp);
                int id = rdr.id;
                if (pl.TryGetValue(id, out timecount))
                {
                    if (timecount < rdr.change_count)
                    {
                        ProtoClasses.ProtoEquip.protoRow po = new ProtoClasses.ProtoEquip.protoRow();
                        po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                        #region данные с БД в proto
                        po.id = rdr.id;
                        po.equip_name = rdr.equip_name;
                        //switch (tn)
                        //{
                        //    case Set.TableName.TablePrinters:

                        //        break;
                        //    case Set.TableName.TableCutters:
                        //        po.equip_name = Utils.CheckDBNull.ToString(rdr["cutter_name"]);
                        //        break;
                        //    case Set.TableName.TableCncs:
                        //        po.equip_name = Utils.CheckDBNull.ToString(rdr["cnc_name"]);
                        //        break;
                        //}
                        po.t_position = rdr.t_position;
                        po.price = rdr.price;
                        po.note = rdr.note;
                        po.change_count = rdr.change_count;
                        po.time_recieve = rdr.time_recieve;
                        #endregion

                        outList.Add(po);
                    }
                    pl.Remove(id);
                }
                else
                {
                    try
                    {
                        ProtoClasses.ProtoEquip.protoRow po = new ProtoClasses.ProtoEquip.protoRow();
                        po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert;
                        #region данные с БД в  protoOrder
                        po.id = rdr.id;
                        po.equip_name = rdr.equip_name;
                        //switch (tn)
                        //{
                        //    case Set.TableName.TablePrinters:
                        //        po.equip_name = Utils.CheckDBNull.ToString(rdr["printer_name"]);
                        //        break;
                        //    case Set.TableName.TableCutters:
                        //        po.equip_name = Utils.CheckDBNull.ToString(rdr["cutter_name"]);
                        //        break;
                        //    case Set.TableName.TableCncs:
                        //        po.equip_name = Utils.CheckDBNull.ToString(rdr["cnc_name"]);
                        //        break;
                        //}
                        po.t_position = rdr.t_position;
                        po.price = rdr.price;
                        po.note = rdr.note;
                        po.change_count = rdr.change_count;
                        po.time_recieve = rdr.time_recieve;
                        #endregion
                        outList.Add(po);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                rowsCurrentIndex++;
                percentCurrent = Convert.ToInt32((rowsCurrentIndex * 100) / rowsCount);
                if (percentCurrent > percentLast)
                {
                    if (client.Socket != null)
                    {
                        if (!client.Socket.Connected)
                        {
                            breakclient = true; break;
                        }
                    }
                    else
                    {
                        breakclient = true; break;
                    }

                    percentLast = percentCurrent;
                    byte[] head = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsProcess, (byte)tn };
                    byte[] body = BitConverter.GetBytes(percentLast);
                    byte[] message = new byte[head.Length + body.Length];
                    Buffer.BlockCopy(head, 0, message, 0, head.Length);
                    Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                    SocketServer.Servers.server.Send(client, message);
                }

            }
            if (breakclient) { return null; }
            if (pl.Count > 0)
            {
                foreach (int itmID in pl.Keys)
                {
                    ProtoClasses.ProtoEquip.protoRow po = new ProtoClasses.ProtoEquip.protoRow();
                    po.id = itmID;
                    po.command = (int)(int)SocketServer.TableServer.SocketMessageCommand.RowsDelete;
                    outList.Add(po);
                }
            }

            byte[] head1 = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsSend, (byte)tn };
            byte[] body1 = BitConverter.GetBytes(percentLast);
            byte[] message1 = new byte[head1.Length + body1.Length];
            Buffer.BlockCopy(head1, 0, message1, 0, head1.Length);
            Buffer.BlockCopy(body1, 0, message1, head1.Length, body1.Length);
            SocketServer.Servers.server.Send(client, message1);
            return outList;
        }

        #endregion

    }
}
