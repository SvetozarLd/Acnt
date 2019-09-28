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

namespace AccentServer.MySql
{
    class Material
    {
        static Thread thread;
        static string ConnectionString = "";

        #region Начально - чтения БД в DataTable
        static internal Exception StartReadTables()
        {
            try
            {
                DataTables.oldstock_material_print = new System.Collections.Concurrent.ConcurrentDictionary<int, ProtoClasses.ProtoMaterial.protoRow>();
                DataTables.oldstock_material_cut = new System.Collections.Concurrent.ConcurrentDictionary<int, ProtoClasses.ProtoMaterial.protoRow>();
                DataTables.oldstock_material_cnc = new System.Collections.Concurrent.ConcurrentDictionary<int, ProtoClasses.ProtoMaterial.protoRow>();
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM material_print", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            ProtoClasses.ProtoMaterial.protoRow row = new ProtoClasses.ProtoMaterial.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.material_name = Utils.CheckDBNull.ToString(rdr["material_name"]);
                            row.material_price = Utils.CheckDBNull.ToDouble(rdr["material_price"]);
                            row.material_q = Utils.CheckDBNull.ToDouble(rdr["material_q"]);
                            row.material_position = Utils.CheckDBNull.ToInt32(rdr["material_position"]);
                            row.material_arcticle = Utils.CheckDBNull.ToString(rdr["material_arcticle"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_material_print.TryAdd(row.id, row);
                        }
                    }
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM material_cut", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            ProtoClasses.ProtoMaterial.protoRow row = new ProtoClasses.ProtoMaterial.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.material_name = Utils.CheckDBNull.ToString(rdr["material_name"]);
                            row.material_price = Utils.CheckDBNull.ToDouble(rdr["material_price"]);
                            row.material_q = Utils.CheckDBNull.ToDouble(rdr["material_q"]);
                            row.material_position = Utils.CheckDBNull.ToInt32(rdr["material_position"]);
                            row.material_arcticle = Utils.CheckDBNull.ToString(rdr["material_arcticle"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_material_cut.TryAdd(row.id, row);
                        }
                    }

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM material_cnc", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            ProtoClasses.ProtoMaterial.protoRow row = new ProtoClasses.ProtoMaterial.protoRow();
                            row.id = Utils.CheckDBNull.ToInt32(rdr["id"]);
                            row.material_name = Utils.CheckDBNull.ToString(rdr["material_name"]);
                            row.material_price = Utils.CheckDBNull.ToDouble(rdr["material_price"]);
                            row.material_q = Utils.CheckDBNull.ToDouble(rdr["material_q"]);
                            row.material_position = Utils.CheckDBNull.ToInt32(rdr["material_position"]);
                            row.material_arcticle = Utils.CheckDBNull.ToString(rdr["material_arcticle"]);
                            row.note = Utils.CheckDBNull.ToString(rdr["note"]);
                            row.time_recieve = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve"]);
                            row.change_count = Utils.UnixDate.CheckedDateTimeToInt64(rdr["time_recieve_edit"]);
                            DataTables.oldstock_material_cnc.TryAdd(row.id, row);
                        }
                    }

                    //if (DataTables.oldstock_material_print == null)
                    //{
                    //using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM material_print", connection))
                    //{

                    //    //DataTables.oldstock_material_print = new DataTable();
                    //    //adapter.AcceptChangesDuringFill = false;
                    //    //adapter.Fill(DataTables.oldstock_material_print);
                    //}
                    //}
                    //if (DataTables.oldstock_material_cut == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM material_cut", connection))
                    //    {
                    //        DataTables.oldstock_material_cut = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.oldstock_material_cut);
                    //    }
                    //}
                    //if (DataTables.oldstock_material_cnc == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM material_cnc", connection))
                    //    {
                    //        DataTables.oldstock_material_cnc = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.oldstock_material_cnc);
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
        public List<ProtoClasses.ProtoMaterial.protoRow> SelectLastChanges(Dictionary<int, long> pl, SocketServer.ClientInfo client, Set.TableName tn)
        {

                bool breakclient = false;
                if (pl == null) { pl = new Dictionary<int, long>(); }

                List<ProtoClasses.ProtoMaterial.protoRow> outList = new List<ProtoClasses.ProtoMaterial.protoRow>();
            System.Collections.Concurrent.ConcurrentDictionary<int, ProtoClasses.ProtoMaterial.protoRow> dt = new System.Collections.Concurrent.ConcurrentDictionary<int, ProtoClasses.ProtoMaterial.protoRow>();
                //DataTable dt = null;
                switch (tn)
                {
                    case Set.TableName.TableMaterialPrint:
                        dt = DataTables.oldstock_material_print;
                        break;
                    case Set.TableName.TableMaterialCut:
                        dt = DataTables.oldstock_material_cut;
                        break;
                    case Set.TableName.TableMaterialCnc:
                        dt = DataTables.oldstock_material_cnc;
                        break;
                }


            int rowsCount = dt.Count;
                int rowsCurrentIndex = 0;
                int percentCurrent = 0;
                int percentLast = 0;
            try
            {
                foreach (ProtoClasses.ProtoMaterial.protoRow rdr in dt.Values)
                {


                    long timecount = 0;
                    //DateTime dttemp = Convert.ToDateTime(rdr["time_recieve_edit"]);
                    long servertimacount = rdr.change_count;
                    int id = rdr.id;
                    if (pl.TryGetValue(id, out timecount))
                    {
                        if (timecount < servertimacount)
                        {
                            ProtoClasses.ProtoMaterial.protoRow po = new ProtoClasses.ProtoMaterial.protoRow();
                            po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                            #region данные с БД в proto
                            po.id = rdr.id;
                            po.material_name = rdr.material_name;
                            po.material_arcticle = rdr.material_arcticle;
                            po.material_price = rdr.material_price;
                            po.material_q = rdr.material_q;
                            po.material_position = rdr.material_position;
                            po.note = rdr.note;
                            po.change_count = servertimacount;
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
                            ProtoClasses.ProtoMaterial.protoRow po = new ProtoClasses.ProtoMaterial.protoRow();
                            po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert;
                            #region данные с БД в  protoOrder
                            po.id = rdr.id;
                            po.material_name = rdr.material_name;
                            po.material_arcticle = rdr.material_arcticle;
                            po.material_price = rdr.material_price;
                            po.material_q = rdr.material_q;
                            po.material_position = rdr.material_position;
                            po.note = rdr.note;
                            po.change_count = servertimacount;
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
                        ProtoClasses.ProtoMaterial.protoRow po = new ProtoClasses.ProtoMaterial.protoRow();
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
            }catch(Exception ex)
            {
                return null;
            }
        }

        #endregion

    }
}
