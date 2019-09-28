using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace AccentServer.MySql
{
    internal class Orders
    {
        private static readonly string ConnectionString = "";
        #region Клиент запросил список изменений с последнего захода
        //static bool SelectLastChangesProcess = false;
        public List<ProtoClasses.ProtoOrders.protoOrder> SelectLastChanges(Dictionary<int, long> pl, SocketServer.ClientInfo client)
        {
            //if (SelectLastChangesProcess) { return null; }
            //SelectLastChangesProcess = true;
            bool breakclient = false;
            if (pl == null) { pl = new Dictionary<int, long>(); }

            List<ProtoClasses.ProtoOrders.protoOrder> outList = new List<ProtoClasses.ProtoOrders.protoOrder>();
            List<AccentServer.ProtoClasses.ProtoOrders.protoOrder> dic = DataTables.orders.Values.ToList();
            int rowsCount = dic.Count;
            int rowsCurrentIndex = 0;
            int percentCurrent = 0;
            int percentLast = 0;

            foreach (ProtoClasses.ProtoOrders.protoOrder rdr in dic)
            {
                int id = rdr.id;
                if (pl.TryGetValue(id, out long timecount))
                {
                    if (timecount < rdr.change_count)
                    {
                        ProtoClasses.ProtoOrders.protoOrder po = new ProtoClasses.ProtoOrders.protoOrder();
                        po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                        #region данные с БД в  protoOrder - остатки, всё переписано и изменилось, БД нет.
                        po.id = rdr.id;
                        po.date_start = rdr.date_start;
                        po.dead_line = rdr.dead_line;
                        po.date_ready_print = rdr.date_ready_print;
                        po.date_ready_cut = rdr.date_ready_cut;
                        po.date_ready_cnc = rdr.date_ready_cnc;
                        po.client = rdr.client;
                        po.work_name = rdr.work_name;
                        po.material_print_id = rdr.material_print_id;
                        po.material_cut_id = rdr.material_cut_id;
                        po.material_cnc_id = rdr.material_cnc_id;
                        po.size_x_print = rdr.size_x_print;
                        po.size_y_print = rdr.size_y_print;
                        po.size_x_cut = rdr.size_x_cut;
                        po.size_y_cut = rdr.size_y_cut;
                        po.size_x_cnc = rdr.size_x_cnc;
                        po.size_y_cnc = rdr.size_y_cnc;
                        po.size_cut = rdr.size_cut;
                        po.line_size_cut = rdr.line_size_cut;
                        po.count_size_cut = rdr.count_size_cut;
                        po.size_cnc = rdr.size_cnc;
                        po.line_size_cnc = rdr.line_size_cnc;
                        po.count_size_cnc = rdr.count_size_cnc;
                        po.count_print = rdr.count_print;
                        po.count_cut = rdr.count_cut;
                        po.count_cnc = rdr.count_cnc;
                        po.square_print = rdr.square_print;
                        po.square_cut = rdr.square_cut;
                        po.square_cnc = rdr.square_cnc;
                        po.cutting_on_print = rdr.cutting_on_print;
                        po.cnc_on_print = rdr.cnc_on_print;
                        po.print_on = rdr.print_on;
                        po.cut_on = rdr.cut_on;
                        po.cnc_on = rdr.cnc_on;
                        po.printers_id = rdr.printers_id;
                        po.cutters_id = rdr.cutters_id;
                        po.cncs_id = rdr.cncs_id;
                        po.comments = rdr.comments;
                        po.laminat = rdr.laminat;
                        po.laminat_mat = rdr.laminat_mat;
                        po.installation = rdr.installation;
                        po.installation_comment = rdr.installation_comment;
                        po.printerman = rdr.printerman;
                        po.cutterman = rdr.cutterman;
                        po.cncman = rdr.cncman;
                        po.adder = rdr.adder;
                        po.print_quality = rdr.print_quality;
                        po.state = rdr.state;
                        po.state_print = rdr.state_print;
                        po.state_cut = rdr.state_cut;
                        po.state_cnc = rdr.state_cnc;
                        po.state_install = rdr.state_install;
                        po.date_preview = rdr.date_preview;
                        po.path_preview = rdr.path_preview;
                        po.path_maket = rdr.path_maket;
                        po.change_count = rdr.change_count;
                        po.old_stock = rdr.old_stock;
                        po.time_recieve = rdr.time_recieve;
                        po.sender_row_id = rdr.sender_row_id;
                        po.sender_row_stringid = rdr.sender_row_stringid;
                        po.worktypes_list = rdr.worktypes_list;
                        po.delivery = rdr.delivery;
                        po.delivery_office = rdr.delivery_office;
                        po.delivery_address = rdr.delivery_address;
                        po.baner_handling = rdr.baner_handling;
                        po.baner_luvers = rdr.baner_luvers;
                        po.baner_handling_size = rdr.baner_handling_size;
                        if (po.state == 3 || po.state == 4)
                        {
                            if (po.print_on && !po.state_print)
                            {
                                po.state_print = true;
                            }
                            if (po.cut_on && !po.state_cut)
                            {
                                po.state_cut = true;
                            }
                            if (po.cnc_on && !po.state_cnc)
                            {
                                po.state_cnc = true;
                            }
                        }
                        po.preview = null;
                        #endregion

                        outList.Add(po);
                    }
                    pl.Remove(id);
                }
                else
                {
                    ProtoClasses.ProtoOrders.protoOrder po = new ProtoClasses.ProtoOrders.protoOrder();
                    po.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                    #region данные с БД в  protoOrder - остатки, всё переписано и изменилось, БД нет.
                    po.id = rdr.id;
                    po.date_start = rdr.date_start;
                    po.dead_line = rdr.dead_line;
                    po.date_ready_print = rdr.date_ready_print;
                    po.date_ready_cut = rdr.date_ready_cut;
                    po.date_ready_cnc = rdr.date_ready_cnc;
                    po.client = rdr.client;
                    po.work_name = rdr.work_name;
                    po.material_print_id = rdr.material_print_id;
                    po.material_cut_id = rdr.material_cut_id;
                    po.material_cnc_id = rdr.material_cnc_id;
                    po.size_x_print = rdr.size_x_print;
                    po.size_y_print = rdr.size_y_print;
                    po.size_x_cut = rdr.size_x_cut;
                    po.size_y_cut = rdr.size_y_cut;
                    po.size_x_cnc = rdr.size_x_cnc;
                    po.size_y_cnc = rdr.size_y_cnc;
                    po.size_cut = rdr.size_cut;
                    po.line_size_cut = rdr.line_size_cut;
                    po.count_size_cut = rdr.count_size_cut;
                    po.size_cnc = rdr.size_cnc;
                    po.line_size_cnc = rdr.line_size_cnc;
                    po.count_size_cnc = rdr.count_size_cnc;
                    po.count_print = rdr.count_print;
                    po.count_cut = rdr.count_cut;
                    po.count_cnc = rdr.count_cnc;
                    po.square_print = rdr.square_print;
                    po.square_cut = rdr.square_cut;
                    po.square_cnc = rdr.square_cnc;
                    po.cutting_on_print = rdr.cutting_on_print;
                    po.cnc_on_print = rdr.cnc_on_print;
                    po.print_on = rdr.print_on;
                    po.cut_on = rdr.cut_on;
                    po.cnc_on = rdr.cnc_on;
                    po.printers_id = rdr.printers_id;
                    po.cutters_id = rdr.cutters_id;
                    po.cncs_id = rdr.cncs_id;
                    po.comments = rdr.comments;
                    po.laminat = rdr.laminat;
                    po.laminat_mat = rdr.laminat_mat;
                    po.installation = rdr.installation;
                    po.installation_comment = rdr.installation_comment;
                    po.printerman = rdr.printerman;
                    po.cutterman = rdr.cutterman;
                    po.cncman = rdr.cncman;
                    po.adder = rdr.adder;
                    po.print_quality = rdr.print_quality;
                    po.state = rdr.state;
                    po.state_print = rdr.state_print;
                    po.state_cut = rdr.state_cut;
                    po.state_cnc = rdr.state_cnc;
                    po.state_install = rdr.state_install;
                    po.date_preview = rdr.date_preview;
                    po.path_preview = rdr.path_preview;
                    po.path_maket = rdr.path_maket;
                    po.change_count = rdr.change_count;
                    po.old_stock = rdr.old_stock;
                    po.time_recieve = rdr.time_recieve;
                    po.sender_row_id = rdr.sender_row_id;
                    po.sender_row_stringid = rdr.sender_row_stringid;
                    po.worktypes_list = rdr.worktypes_list;
                    po.delivery = rdr.delivery;
                    po.delivery_office = rdr.delivery_office;
                    po.delivery_address = rdr.delivery_address;
                    po.baner_handling = rdr.baner_handling;
                    po.baner_luvers = rdr.baner_luvers;
                    po.baner_handling_size = rdr.baner_handling_size;
                    if (po.state == 3 || po.state == 4)
                    {
                        if (po.print_on && !po.state_print)
                        {
                            po.state_print = true;
                        }
                        if (po.cut_on && !po.state_cut)
                        {
                            po.state_cut = true;
                        }
                        if (po.cnc_on && !po.state_cnc)
                        {
                            po.state_cnc = true;
                        }
                    }
                    po.preview = null;
                    #endregion

                    outList.Add(po);
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
                    byte[] head = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsProcess, (int)MySql.Set.TableName.TableOrders };
                    byte[] body = BitConverter.GetBytes(percentLast);
                    byte[] message = new byte[head.Length + body.Length];
                    Buffer.BlockCopy(head, 0, message, 0, head.Length);
                    Buffer.BlockCopy(body, 0, message, head.Length, body.Length);
                    SocketServer.Servers.server.Send(client, message);
                    //if (!SocketServer.TableServer.clientList.TryGetValue(client.Socket, out ci))
                    //{
                    //    return null;
                    //}
                }

            }
            if (breakclient) { return null; }
            if (pl.Count > 0)
            {
                foreach (int itmID in pl.Keys)
                {
                    ProtoClasses.ProtoOrders.protoOrder po = new ProtoClasses.ProtoOrders.protoOrder
                    {
                        id = itmID,
                        command = (int)SocketServer.TableServer.SocketMessageCommand.RowsDelete
                    };
                    outList.Add(po);
                }
            }

            byte[] head1 = new byte[2] { (int)SocketServer.TableServer.SocketMessageCommand.RowsChangeCountsSend, (int)MySql.Set.TableName.TableOrders };
            byte[] body1 = BitConverter.GetBytes(percentLast);
            byte[] message1 = new byte[head1.Length + body1.Length];
            Buffer.BlockCopy(head1, 0, message1, 0, head1.Length);
            Buffer.BlockCopy(body1, 0, message1, head1.Length, body1.Length);
            SocketServer.Servers.server.Send(client, message1);
            //SelectLastChangesProcess = false;
            return outList;
            //if (outList.Count > 0)
            //{
            //    return outList;
            //}
            //else
            //{
            //    return null;
            //}
        }
        #endregion

        #region SQLite -> datatable
        internal static void loadTable()
        {
            Thread thread = new Thread(StartReadTables)
            {
                Name = "LoadOrders",
                IsBackground = true
            };
            thread.Start();
        }
        #endregion

        #region Начальное чтение таблиц
        internal static void StartReadTables()
        {
            try
            {
                DataTables.orders = new ConcurrentDictionary<int, ProtoClasses.ProtoOrders.protoOrder>();
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    //if (DataTables.orders == null)
                    //{
                    //    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM base", connection))
                    //    {

                    //            DataTables.orders = new DataTable();
                    //        adapter.AcceptChangesDuringFill = false;
                    //        adapter.Fill(DataTables.orders);
                    //    }
                    //    //OnMySqlEvent(new MySqlEventArgs("Загрузка таблицы заказов успешна", null));
                    //}

                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM base", connection))
                    {
                        MySqlDataReader rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            long servertimacount1 = Convert.ToInt64(Utils.UnixDate.DateTimeToInt64(Convert.ToDateTime(rdr["time_recieve_edit"])));
                            long servertimacount2 = Convert.ToInt64(rdr["change_count"]);
                            long servertimacount = 0;
                            if (servertimacount1 > servertimacount2)
                            {
                                servertimacount = servertimacount1;
                            }
                            else { servertimacount = servertimacount2; }

                            ProtoClasses.ProtoOrders.protoOrder po = new ProtoClasses.ProtoOrders.protoOrder
                            {
                                command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert,
                                id = Convert.ToInt32(rdr["id"]),
                                date_start = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_start"]),
                                dead_line = Utils.UnixDate.CheckedDateTimeToInt64(rdr["dead_line"]),
                                date_ready_print = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_ready_print"]),
                                date_ready_cut = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_ready_cut"]),
                                date_ready_cnc = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_ready_cnc"]),
                                client = Utils.CheckDBNull.ToString(rdr["client"]),
                                work_name = Utils.CheckDBNull.ToString(rdr["work_name"]),
                                material_print_id = Utils.CheckDBNull.ToInt32(rdr["material_print_id"]),
                                material_cut_id = Utils.CheckDBNull.ToInt32(rdr["material_cut_id"]),
                                material_cnc_id = Utils.CheckDBNull.ToInt32(rdr["material_cnc_id"]),
                                size_x_print = Utils.CheckDBNull.ToDouble(rdr["size_x_print"]),
                                size_y_print = Utils.CheckDBNull.ToDouble(rdr["size_y_print"]),
                                size_x_cut = Utils.CheckDBNull.ToDouble(rdr["size_x_cut"]),
                                size_y_cut = Utils.CheckDBNull.ToDouble(rdr["size_y_cut"]),
                                size_x_cnc = Utils.CheckDBNull.ToDouble(rdr["size_x_cnc"]),
                                size_y_cnc = Utils.CheckDBNull.ToDouble(rdr["size_y_cnc"]),
                                size_cut = Utils.CheckDBNull.ToDouble(rdr["size_cut"]),
                                line_size_cut = Utils.CheckDBNull.ToDouble(rdr["line_size_cut"]),
                                count_size_cut = Utils.CheckDBNull.ToInt32(rdr["count_size_cut"]),
                                size_cnc = Utils.CheckDBNull.ToDouble(rdr["size_cnc"]),
                                line_size_cnc = Utils.CheckDBNull.ToDouble(rdr["line_size_cnc"]),
                                count_size_cnc = Utils.CheckDBNull.ToInt32(rdr["count_size_cnc"]),
                                count_print = Utils.CheckDBNull.ToInt32(rdr["count_print"]),
                                count_cut = Utils.CheckDBNull.ToInt32(rdr["count_cut"]),
                                count_cnc = Utils.CheckDBNull.ToInt32(rdr["count_cnc"]),
                                square_print = Utils.CheckDBNull.ToDouble(rdr["square_print"]),
                                square_cut = Utils.CheckDBNull.ToDouble(rdr["square_cut"]),
                                square_cnc = Utils.CheckDBNull.ToDouble(rdr["square_cnc"]),
                                cutting_on_print = Utils.CheckDBNull.ToBoolean(rdr["cutting_on_print"]),
                                print_on = Utils.CheckDBNull.ToBoolean(rdr["print_on"]),
                                cut_on = Utils.CheckDBNull.ToBoolean(rdr["cut_on"]),
                                cnc_on = Utils.CheckDBNull.ToBoolean(rdr["cnc_on"]),
                                printers_id = Utils.CheckDBNull.ToInt32(rdr["printers_id"]),
                                cutters_id = Utils.CheckDBNull.ToInt32(rdr["cutters_id"]),
                                cncs_id = Utils.CheckDBNull.ToInt32(rdr["cncs_id"]),
                                comments = Utils.CheckDBNull.ToString(rdr["comments"]),
                                laminat = Utils.CheckDBNull.ToBoolean(rdr["laminat"]),
                                laminat_mat = Utils.CheckDBNull.ToBoolean(rdr["laminat_mat"]),
                                installation = Utils.CheckDBNull.ToBoolean(rdr["installation"]),
                                //po.installation_comment = Utils.CheckDBNull.ToString(rdr["installation_comment"]);
                                printerman = Utils.CheckDBNull.ToString(rdr["printerman"]),
                                cutterman = Utils.CheckDBNull.ToString(rdr["cutterman"]),
                                cncman = Utils.CheckDBNull.ToString(rdr["cncman"]),
                                adder = Utils.CheckDBNull.ToString(rdr["adder"]),
                                print_quality = Utils.CheckDBNull.ToString(rdr["print_quality"]),
                                state = Utils.CheckDBNull.ToInt32(rdr["state"]),
                                state_print = Utils.CheckDBNull.ToBoolean(rdr["state_print"]),
                                state_cut = Utils.CheckDBNull.ToBoolean(rdr["state_cut"]),
                                state_cnc = Utils.CheckDBNull.ToBoolean(rdr["state_cnc"]),
                                state_install = Utils.CheckDBNull.ToBoolean(rdr["state_install"]),
                                date_preview = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_preview"]),
                                path_preview = Utils.CheckDBNull.ToString(rdr["path_preview"]),
                                path_maket = Utils.CheckDBNull.ToString(rdr["path_maket"]),
                                old_stock = Utils.CheckDBNull.ToInt32(rdr["old_stock"]),
                                change_count = servertimacount,
                                time_recieve = Utils.UnixDate.CheckedDateTimeToInt64((rdr["time_recieve"])),
                                sender_row_id = Utils.CheckDBNull.ToLong((rdr["sender_row_id"])),
                                sender_row_stringid = Utils.CheckDBNull.ToString((rdr["sender_row_stringid"])),
                                worktypes_list = Utils.CheckDBNull.ToString((rdr["worktypes_list"])),
                                delivery = Utils.CheckDBNull.ToBoolean((rdr["delivery"])),
                                delivery_office = Utils.CheckDBNull.ToBoolean((rdr["delivery_office"])),
                                delivery_address = Utils.CheckDBNull.ToString((rdr["delivery_address"])),
                                baner_handling = Utils.CheckDBNull.ToBoolean((rdr["baner_handling"])),
                                baner_luvers = Utils.CheckDBNull.ToBoolean((rdr["baner_luvers"])),
                                baner_handling_size = Utils.CheckDBNull.ToDouble((rdr["baner_handling_size"]))
                            };
                            if (po.state == 3 || po.state == 4)
                            {
                                if (po.print_on && !po.state_print)
                                {
                                    po.state_print = true;
                                }
                                if (po.cut_on && !po.state_cut)
                                {
                                    po.state_cut = true;
                                }
                                if (po.cnc_on && !po.state_cnc)
                                {
                                    po.state_cnc = true;
                                }
                            }
                            po.preview = null;
                            //if (DateTime.TryParse(Convert.ToString(rdr["time_recieve"]), out DateTime tmpenddate))
                            //{
                            //    string previewPath = Settings.Parameters.FilePath + @"/" + tmpenddate.ToString("yyyy.MM") + "/" + po.id.ToString() + "/" + "index.png";
                            //    po.preview = Utils.Converting.GetBytesFromImage(previewPath);
                            //}
                            DataTables.orders.TryAdd(po.id, po);
                        }
                    }
                }
                OnSEND(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки заявок!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnSEND(false);
            }
        }




        public delegate void OrdersLoadHandler(object sender, bool e);
        public static event OrdersLoadHandler OrdersLoadEvent;
        public static void OnSEND(bool e)
        {
            OrdersLoadEvent?.Invoke(null, e);
        }
        #endregion

        #region INSERT
        public List<ProtoClasses.ProtoOrders.protoOrder> InsertRow(List<ProtoClasses.ProtoOrders.protoOrder> uptab, string ClientName)
        {
            if (uptab != null && uptab.Count > 0 && DataTables.orders != null)
            {
                List<ProtoClasses.ProtoOrders.protoOrder> result = new List<ProtoClasses.ProtoOrders.protoOrder>();
                List<ProtoClasses.ProtoOrders.protoOrder> resultUpdate = new List<ProtoClasses.ProtoOrders.protoOrder>();
                List<ProtoClasses.ProtoOrderHistory.protoRow> resultHistory = new List<ProtoClasses.ProtoOrderHistory.protoRow>();
                DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
                DateTime dateTimeNow = DateTime.Now;
                long change_count = Utils.UnixDate.DateTimeToInt64(DateTime.Now);
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        foreach (ProtoClasses.ProtoOrders.protoOrder item in uptab)
                        {
                            ////Проверка - строка добавилась, но у клиента оборвало соединение, он не получил ответа и пытается вставить ещё раз
                            ////случай практически невозможный (клиент при подключении ) получает список изменений и проверяет у себя. но на всякий...
                            //if (item.sender_row_id > 0 && item.sender_row_stringid != string.Empty)
                            //{
                            //    //DataRow CheckRow = DataTables.orders.Select("sender_row_id = " + item.sender_row_id + " AND sender_row_stringid = '" + item.sender_row_stringid + "'").FirstOrDefault();
                            //    ProtoClasses.ProtoOrders.protoOrder CheckRow = null;
                            //    if (DataTables.orders.ContainsKey(item.id)) { CheckRow = DataTables.orders[item.id]; }
                            //    if (CheckRow != null)
                            //    {
                            //        item.id = Utils.CheckDBNull.ToLong(CheckRow["id"]);
                            //        item.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                            //        resultUpdate.Add(item);
                            //    }
                            //    else
                            //    {
                            #region sqlcommand
                            using (MySqlCommand command = new MySqlCommand(@"INSERT INTO base (
                            date_start, 
                            dead_line, 
                            date_ready_print, 
                            date_ready_cut, 
                            date_ready_cnc, 
                            client, 
                            work_name, 
                            material_print_id, 
                            material_cut_id, 
                            material_cnc_id, 
                            size_x_print, 
                            size_y_print, 
                            size_x_cut, 
                            size_y_cut, 
                            size_x_cnc, 
                            size_y_cnc, 
                            size_cut, 
                            line_size_cut, 
                            count_size_cut, 
                            size_cnc, 
                            line_size_cnc, 
                            count_size_cnc, 
                            count_print, 
                            count_cut, 
                            count_cnc, 
                            square_print, 
                            square_cut, 
                            square_cnc, 
                            cutting_on_print, 
                            cnc_on_print, 
                            print_on, 
                            cut_on, 
                            cnc_on, 
                            printers_id, 
                            cutters_id, 
                            cncs_id, 
                            comments, 
                            laminat, 
                            laminat_mat, 
                            installation, 
                            printerman, 
                            cutterman, 
                            cncman, 
                            adder, 
                            print_quality, 
                            state, 
                            state_print, 
                            state_cut, 
                            state_cnc,
                            state_install,
                            change_count,
                            sender_row_id,
                            sender_row_stringid,
                            time_recieve,
                            worktypes_list,
                            delivery,
                            delivery_office,
                            delivery_address,
                            baner_handling,
                            baner_luvers,
                            baner_handling_size
                            ) VALUES (
                            @date_start, 
                            @dead_line, 
                            @date_ready_print, 
                            @date_ready_cut, 
                            @date_ready_cnc, 
                            @client, 
                            @work_name, 
                            @material_print_id, 
                            @material_cut_id, 
                            @material_cnc_id, 
                            @size_x_print, 
                            @size_y_print, 
                            @size_x_cut, 
                            @size_y_cut, 
                            @size_x_cnc, 
                            @size_y_cnc, 
                            @size_cut, 
                            @line_size_cut, 
                            @count_size_cut, 
                            @size_cnc, 
                            @line_size_cnc, 
                            @count_size_cnc, 
                            @count_print, 
                            @count_cut, 
                            @count_cnc, 
                            @square_print, 
                            @square_cut, 
                            @square_cnc, 
                            @cutting_on_print, 
                            @cutting_on_print, 
                            @print_on, 
                            @cut_on, 
                            @cnc_on, 
                            @printers_id, 
                            @cutters_id, 
                            @cncs_id, 
                            @comments, 
                            @laminat, 
                            @laminat_mat, 
                            @installation, 
                            @printerman, 
                            @cutterman, 
                            @cncman, 
                            @adder, 
                            @print_quality, 
                            @state, 
                            @state_print, 
                            @state_cut, 
                            @state_cnc,
                            @state_install,
                            @change_count,
                            @sender_row_id,
                            @sender_row_stringid,
                            @time_recieve,
                            @worktypes_list,
                            @delivery,
                            @delivery_office,
                            @delivery_address,
                            @baner_handling,
                            @baner_luvers,
                            @baner_handling_size
                            )", connection))
                            {
                                command.Transaction = transaction;
                                //command.Parameters.AddWithValue("@id", item.id);
                                command.Parameters.AddWithValue("@date_start", Utils.UnixDate.Int64ToDateTime(item.date_start));
                                command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line));
                                command.Parameters.AddWithValue("@date_ready_print", Utils.UnixDate.Int64ToDateTime(item.date_ready_print));
                                command.Parameters.AddWithValue("@date_ready_cut", Utils.UnixDate.Int64ToDateTime(item.date_ready_cut));
                                command.Parameters.AddWithValue("@date_ready_cnc", Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc));
                                command.Parameters.AddWithValue("@client", Utils.CheckDBNull.ToString(item.client));
                                command.Parameters.AddWithValue("@work_name", Utils.CheckDBNull.ToString(item.work_name));
                                command.Parameters.AddWithValue("@material_print_id", Utils.CheckDBNull.ToInt32(item.material_print_id));
                                command.Parameters.AddWithValue("@material_cut_id", Utils.CheckDBNull.ToInt32(item.material_cut_id));
                                command.Parameters.AddWithValue("@material_cnc_id", Utils.CheckDBNull.ToInt32(item.material_cnc_id));
                                command.Parameters.AddWithValue("@size_x_print", item.size_x_print);
                                command.Parameters.AddWithValue("@size_y_print", item.size_y_cnc);
                                command.Parameters.AddWithValue("@size_x_cut", item.size_x_cut);
                                command.Parameters.AddWithValue("@size_y_cut", item.size_y_cut);
                                command.Parameters.AddWithValue("@size_x_cnc", item.size_x_cnc);
                                command.Parameters.AddWithValue("@size_y_cnc", item.size_y_cnc);
                                command.Parameters.AddWithValue("@size_cut", item.size_cut);
                                command.Parameters.AddWithValue("@line_size_cut", item.line_size_cut);
                                command.Parameters.AddWithValue("@count_size_cut", item.count_size_cut);
                                command.Parameters.AddWithValue("@size_cnc", item.size_cnc);
                                command.Parameters.AddWithValue("@line_size_cnc", item.line_size_cnc);
                                command.Parameters.AddWithValue("@count_size_cnc", item.count_size_cnc);
                                command.Parameters.AddWithValue("@count_print", item.count_print);
                                command.Parameters.AddWithValue("@count_cut", item.count_cut);
                                command.Parameters.AddWithValue("@count_cnc", item.count_cnc);
                                command.Parameters.AddWithValue("@square_print", item.square_print);
                                command.Parameters.AddWithValue("@square_cut", item.square_cut);
                                command.Parameters.AddWithValue("@square_cnc", item.square_cnc);
                                command.Parameters.AddWithValue("@cutting_on_print", item.cutting_on_print);
                                command.Parameters.AddWithValue("@cnc_on_print", item.cnc_on_print);
                                command.Parameters.AddWithValue("@print_on", item.print_on);
                                command.Parameters.AddWithValue("@cut_on", item.cut_on);
                                command.Parameters.AddWithValue("@cnc_on", item.cnc_on);
                                command.Parameters.AddWithValue("@printers_id", item.printers_id);
                                command.Parameters.AddWithValue("@cutters_id", item.cutters_id);
                                command.Parameters.AddWithValue("@cncs_id", item.cncs_id);
                                command.Parameters.AddWithValue("@comments", item.comments);
                                command.Parameters.AddWithValue("@laminat", item.laminat);
                                command.Parameters.AddWithValue("@laminat_mat", item.laminat_mat);
                                command.Parameters.AddWithValue("@installation", item.installation);
                                //command.Parameters.AddWithValue("@installation_comment", item.installation_comment);
                                command.Parameters.AddWithValue("@printerman", item.printerman);
                                command.Parameters.AddWithValue("@cutterman", item.cutterman);
                                command.Parameters.AddWithValue("@cncman", item.cncman);
                                command.Parameters.AddWithValue("@print_quality", item.print_quality);
                                command.Parameters.AddWithValue("@state", item.state);
                                command.Parameters.AddWithValue("@state_print", item.state_print);
                                command.Parameters.AddWithValue("@state_cut", item.state_cut);
                                command.Parameters.AddWithValue("@state_cnc", item.state_cnc);
                                command.Parameters.AddWithValue("@state_install", item.state_install);
                                command.Parameters.AddWithValue("@change_count", change_count);
                                command.Parameters.AddWithValue("@sender_row_id", item.sender_row_id);
                                command.Parameters.AddWithValue("@sender_row_stringid", item.sender_row_stringid);
                                command.Parameters.AddWithValue("@adder", item.adder);
                                command.Parameters.AddWithValue("@time_recieve", dateTimeNow);
                                command.Parameters.AddWithValue("@worktypes_list", item.worktypes_list);
                                command.Parameters.AddWithValue("@delivery", item.delivery);
                                command.Parameters.AddWithValue("@delivery_office", item.delivery_office);
                                command.Parameters.AddWithValue("@delivery_address", item.delivery_address);
                                command.Parameters.AddWithValue("@baner_handling", item.baner_handling);
                                command.Parameters.AddWithValue("@baner_luvers", item.baner_luvers);
                                command.Parameters.AddWithValue("@baner_handling_size", item.baner_handling_size);
                                command.ExecuteNonQuery();
                                item.id = Convert.ToInt32(command.LastInsertedId);
                                item.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsInsert;
                                item.change_count = change_count;
                                item.time_recieve = change_count;
                                #region файлы
                                string yearpath = Properties.Settings.Default.FilePath + @"/" + dateTimeNow.ToString("yyyy.MM");
                                if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                                string orderPath = Properties.Settings.Default.FilePath + @"/" + dateTimeNow.ToString("yyyy.MM") + "/" + item.id.ToString("0000");
                                if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }
                                if (!Directory.Exists(orderPath + @"/" + "makets")) { Directory.CreateDirectory(orderPath + @"/" + "makets"); }
                                if (!Directory.Exists(orderPath + @"/" + "preview")) { Directory.CreateDirectory(orderPath + @"/" + "preview"); }
                                if (!Directory.Exists(orderPath + @"/" + "doc")) { Directory.CreateDirectory(orderPath + @"/" + "doc"); }
                                if (!Directory.Exists(orderPath + @"/" + "photoreport")) { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }
                                #region preview
                                if (item.preview != null)
                                {
                                    using (FileStream fs = new FileStream(orderPath + @"/index.png", FileMode.Create, FileAccess.Write))
                                    {
                                        fs.Write(item.preview, 0, item.preview.Length);
                                    }
                                    FileInfo fileInf = new FileInfo(orderPath + @"/" + "index.png");
                                    ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow
                                    {
                                        fullname = fileInf.FullName.Replace(di.FullName + @"\", ""),
                                        LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime),
                                        Length = fileInf.Length,
                                        LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime),
                                        order_id = item.id
                                    };
                                    if (MySql.DataTables.preview != null && MySql.DataTables.preview.Count > 0)
                                    {
                                        if (MySql.DataTables.preview.ContainsKey(pr.fullname)) { MySql.DataTables.preview[pr.fullname] = pr; } else { MySql.DataTables.preview.TryAdd(pr.fullname, pr); }
                                    }
                                    else
                                    {
                                        MySql.DataTables.preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
                                        MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                                    }
                                }
                                #endregion

                                #region монтаж - rtf
                                if (item.installation_comment != null)
                                {
                                    using (FileStream fs = new FileStream(orderPath + @"/montage.doc", FileMode.Create, FileAccess.Write))
                                    {
                                        fs.Write(item.installation_comment, 0, item.installation_comment.Length);
                                    }
                                    FileInfo fileInf = new FileInfo(orderPath + @"/" + "montage.doc");
                                    ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow
                                    {
                                        fullname = fileInf.FullName.Replace(di.FullName + @"\", ""),
                                        LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime),
                                        Length = fileInf.Length,
                                        LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime),
                                        order_id = item.id
                                    };
                                    if (MySql.DataTables.preview != null && MySql.DataTables.preview.Count > 0)
                                    {
                                        if (MySql.DataTables.preview.ContainsKey(pr.fullname)) { MySql.DataTables.preview[pr.fullname] = pr; } else { MySql.DataTables.preview.TryAdd(pr.fullname, pr); }
                                    }
                                    else
                                    {
                                        MySql.DataTables.preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
                                        MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                                    }
                                }

                                #endregion
                                #region Указание путей для закачки новых файлов по ftp
                                if (item.FilesUpload != null && item.FilesUpload.Count > 0)
                                {
                                    foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in item.FilesUpload)
                                    {
                                        pr.targetfile = "makets" + @"/" + dateTimeNow.ToString("yyyy.MM") + "/" + item.id.ToString("0000") + @"/" + pr.targetfile;
                                    }
                                }
                                #endregion
                                #endregion

                                result.Add(item);
                                #region Обновление Datatable 
                                ProtoClasses.ProtoOrders.protoOrder customerRow = new ProtoClasses.ProtoOrders.protoOrder
                                {
                                    id = item.id,
                                    change_count = item.change_count,
                                    time_recieve = item.time_recieve,
                                    date_start = item.date_start,
                                    dead_line = item.dead_line,
                                    date_ready_print = item.date_ready_print,
                                    date_ready_cut = item.date_ready_cut,
                                    date_ready_cnc = item.date_ready_cnc,
                                    client = item.client,
                                    work_name = item.work_name,
                                    material_print_id = item.material_print_id,
                                    material_cut_id = item.material_cut_id,
                                    material_cnc_id = item.material_cnc_id,
                                    size_x_print = item.size_x_print,
                                    size_y_print = item.size_y_print,
                                    size_x_cut = item.size_x_cut,
                                    size_y_cut = item.size_y_cut,
                                    size_x_cnc = item.size_x_cnc,
                                    size_y_cnc = item.size_y_cnc,
                                    size_cut = item.size_cut,
                                    line_size_cut = item.line_size_cut,
                                    count_size_cut = item.count_size_cut,
                                    size_cnc = item.size_cnc,
                                    line_size_cnc = item.line_size_cnc,
                                    count_size_cnc = item.count_size_cnc,
                                    count_print = item.count_print,
                                    count_cut = item.count_cut,
                                    count_cnc = item.count_cnc,
                                    square_print = item.square_print,
                                    square_cut = item.square_cut,
                                    square_cnc = item.square_cnc,
                                    cutting_on_print = item.cutting_on_print,
                                    cnc_on_print = item.cnc_on_print,
                                    print_on = item.print_on,
                                    cut_on = item.cut_on,
                                    cnc_on = item.cnc_on,
                                    printers_id = item.printers_id,
                                    cutters_id = item.cutters_id,
                                    cncs_id = item.cncs_id,
                                    comments = item.comments,
                                    laminat = item.laminat,
                                    laminat_mat = item.laminat_mat,
                                    installation = item.installation,
                                    //customerRow.installation_comment = item.installation_comment;
                                    printerman = item.printerman,
                                    cutterman = item.cutterman,
                                    cncman = item.cncman,
                                    print_quality = item.print_quality,
                                    state = item.state,
                                    state_print = item.state_print,
                                    state_cut = item.state_cut,
                                    state_cnc = item.state_cnc,
                                    state_install = item.state_install,
                                    sender_row_id = item.sender_row_id,
                                    sender_row_stringid = item.sender_row_stringid,
                                    adder = item.adder,
                                    worktypes_list = item.worktypes_list,
                                    delivery = item.delivery,
                                    delivery_office = item.delivery_office,
                                    delivery_address = item.delivery_address,
                                    baner_handling = item.baner_handling,
                                    baner_luvers = item.baner_luvers,
                                    baner_handling_size = item.baner_handling_size
                                };
                                if (!DataTables.orders.ContainsKey(customerRow.id)) { DataTables.orders.TryAdd(customerRow.id, customerRow); }
                                #endregion

                                #region Запись в историю заявки
                                ProtoClasses.ProtoOrderHistory.protoRow protoRowHistory = new ProtoClasses.ProtoOrderHistory.protoRow
                                {
                                    adder = item.adder,
                                    note = "Создание задания",
                                    status_task = item.state,
                                    work_id = item.id
                                };
                                resultHistory.Add(protoRowHistory);
                                if (item.HistoryRows != null && item.HistoryRows.Count > 0) { resultHistory.AddRange(item.HistoryRows); }
                                item.HistoryRows = OrderHistory.InsertRow(resultHistory, item.id, ClientName);
                                #endregion
                            }
                            #endregion
                            //}

                            //}

                        }
                        transaction.Commit();
                    }
                }
                if (resultUpdate.Count > 0) { List<ProtoClasses.ProtoOrders.protoOrder> NewUpdateRow = UpdateRow(resultUpdate, ClientName); result.AddRange(NewUpdateRow); }
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region UPDATE
        public List<ProtoClasses.ProtoOrders.protoOrder> UpdateRow(List<ProtoClasses.ProtoOrders.protoOrder> uptab, string ClientName)
        {
            if (uptab != null && uptab.Count > 0 && DataTables.orders != null)
            {
                List<ProtoClasses.ProtoOrders.protoOrder> result = new List<ProtoClasses.ProtoOrders.protoOrder>();
                List<ProtoClasses.ProtoOrderHistory.protoRow> resultHistory = new List<ProtoClasses.ProtoOrderHistory.protoRow>();
                DateTime dateTimeNow = DateTime.Now;
                long change_count = Utils.UnixDate.DateTimeToInt64(dateTimeNow);
                DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        foreach (ProtoClasses.ProtoOrders.protoOrder item in uptab)
                        {
                            if (item.id != 0)
                            {
                                ProtoClasses.ProtoOrders.protoOrder customerRow = null;
                                if (DataTables.orders.ContainsKey(item.id)) { customerRow = DataTables.orders[item.id]; }
                                //DataRow customerRow = DataTables.orders.Select("id = " + item.id).FirstOrDefault();

                                if (customerRow != null)
                                {
                                    DateTime timerecieve = Utils.UnixDate.Int64ToDateTime(customerRow.time_recieve);
                                    //item.time_recieve = customerRow.time_recieve;
                                    #region Update
                                    using (MySqlCommand command = new MySqlCommand(@"UPDATE `base`  SET 
                            `date_start` = @date_start, 
                            `dead_line` = @dead_line, 
                            `date_ready_print` = @date_ready_print, 
                            `date_ready_cut` = @date_ready_cut, 
                            `date_ready_cnc` = @date_ready_cnc, 
                            `client` = @client, 
                            `work_name` = @work_name, 
                            `material_print_id` = @material_print_id, 
                            `material_cut_id` = @material_cut_id, 
                            `material_cnc_id` = @material_cnc_id, 
                            `size_x_print` = @size_x_print, 
                            `size_y_print` = @size_y_print, 
                            `size_x_cut` = @size_x_cut, 
                            `size_y_cut` = @size_y_cut, 
                            `size_x_cnc` = @size_x_cnc, 
                            `size_y_cnc` = @size_y_cnc, 
                            `size_cut` = @size_cut, 
                            `line_size_cut` = @line_size_cut, 
                            `count_size_cut` = @count_size_cut, 
                            `size_cnc` = @size_cnc, 
                            `line_size_cnc` = @line_size_cnc, 
                            `count_size_cnc` = @count_size_cnc, 
                            `count_print` = @count_print, 
                            `count_cut` = @count_cut, 
                            `count_cnc` = @count_cnc, 
                            `square_print` = @square_print, 
                            `square_cut` = @square_cut, 
                            `square_cnc` = @square_cnc, 
                            `cutting_on_print` = @cutting_on_print, 
                            `cnc_on_print` = @cutting_on_print, 
                            `print_on` = @print_on, 
                            `cut_on` = @cut_on, 
                            `cnc_on` = @cnc_on, 
                            `printers_id` = @printers_id, 
                            `cutters_id` = @cutters_id, 
                            `cncs_id` = @cncs_id, 
                            `comments` = @comments, 
                            `laminat` = @laminat, 
                            `laminat_mat` = @laminat_mat, 
                            `installation` = @installation, 
                            `printerman` = @printerman, 
                            `cutterman` = @cutterman, 
                            `cncman` = @cncman, 
                            `adder` = @adder, 
                            `print_quality` = @print_quality, 
                            `state` = @state, 
                            `state_print` = @state_print, 
                            `state_cut` = @state_cut, 
                            `state_cnc` = @state_cnc,
                            `state_install` = @state_install,
                            `change_count` = @change_count,
                            `sender_row_id` = @sender_row_id,
                            `sender_row_stringid` = @sender_row_stringid,
                            `worktypes_list` = @worktypes_list, 
                            `delivery` = @delivery, 
                            `delivery_office` = @delivery_office, 
                            `delivery_address` = @delivery_address, 
                            `baner_handling` = @baner_handling, 
                            `baner_luvers` = @baner_luvers, 
                            `baner_handling_size` = @baner_handling_size 
                            WHERE `id` = @id ; ", connection))
                                    {
                                        command.Transaction = transaction;
                                        command.Parameters.AddWithValue("@id", item.id);
                                        command.Parameters.AddWithValue("@date_start", Utils.UnixDate.Int64ToDateTime(item.date_start));
                                        command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line));
                                        command.Parameters.AddWithValue("@date_ready_print", Utils.UnixDate.Int64ToDateTime(item.date_ready_print));
                                        command.Parameters.AddWithValue("@date_ready_cut", Utils.UnixDate.Int64ToDateTime(item.date_ready_cut));
                                        command.Parameters.AddWithValue("@date_ready_cnc", Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc));
                                        command.Parameters.AddWithValue("@client", Utils.CheckDBNull.ToString(item.client));
                                        command.Parameters.AddWithValue("@work_name", Utils.CheckDBNull.ToString(item.work_name));
                                        command.Parameters.AddWithValue("@material_print_id", Utils.CheckDBNull.ToInt32(item.material_print_id));
                                        command.Parameters.AddWithValue("@material_cut_id", Utils.CheckDBNull.ToInt32(item.material_cut_id));
                                        command.Parameters.AddWithValue("@material_cnc_id", Utils.CheckDBNull.ToInt32(item.material_cnc_id));
                                        command.Parameters.AddWithValue("@size_x_print", item.size_x_print);
                                        command.Parameters.AddWithValue("@size_y_print", item.size_y_cnc);
                                        command.Parameters.AddWithValue("@size_x_cut", item.size_x_cut);
                                        command.Parameters.AddWithValue("@size_y_cut", item.size_y_cut);
                                        command.Parameters.AddWithValue("@size_x_cnc", item.size_x_cnc);
                                        command.Parameters.AddWithValue("@size_y_cnc", item.size_y_cnc);
                                        command.Parameters.AddWithValue("@size_cut", item.size_cut);
                                        command.Parameters.AddWithValue("@line_size_cut", item.line_size_cut);
                                        command.Parameters.AddWithValue("@count_size_cut", item.count_size_cut);
                                        command.Parameters.AddWithValue("@size_cnc", item.size_cnc);
                                        command.Parameters.AddWithValue("@line_size_cnc", item.line_size_cnc);
                                        command.Parameters.AddWithValue("@count_size_cnc", item.count_size_cnc);
                                        command.Parameters.AddWithValue("@count_print", item.count_print);
                                        command.Parameters.AddWithValue("@count_cut", item.count_cut);
                                        command.Parameters.AddWithValue("@count_cnc", item.count_cnc);
                                        command.Parameters.AddWithValue("@square_print", item.square_print);
                                        command.Parameters.AddWithValue("@square_cut", item.square_cut);
                                        command.Parameters.AddWithValue("@square_cnc", item.square_cnc);
                                        command.Parameters.AddWithValue("@cutting_on_print", Utils.CheckDBNull.ToBoolean(item.cutting_on_print));
                                        command.Parameters.AddWithValue("@cnc_on_print", Utils.CheckDBNull.ToBoolean(item.cnc_on_print));
                                        command.Parameters.AddWithValue("@print_on", Utils.CheckDBNull.ToBoolean(item.print_on));
                                        command.Parameters.AddWithValue("@cut_on", Utils.CheckDBNull.ToBoolean(item.cut_on));
                                        command.Parameters.AddWithValue("@cnc_on", Utils.CheckDBNull.ToBoolean(item.cnc_on));
                                        command.Parameters.AddWithValue("@printers_id", item.printers_id);
                                        command.Parameters.AddWithValue("@cutters_id", item.cutters_id);
                                        command.Parameters.AddWithValue("@cncs_id", item.cncs_id);
                                        command.Parameters.AddWithValue("@comments", Utils.CheckDBNull.ToString(item.comments));
                                        command.Parameters.AddWithValue("@laminat", Utils.CheckDBNull.ToBoolean(item.laminat));
                                        command.Parameters.AddWithValue("@laminat_mat", Utils.CheckDBNull.ToBoolean(item.laminat_mat));
                                        command.Parameters.AddWithValue("@installation", Utils.CheckDBNull.ToBoolean(item.installation));
                                        //command.Parameters.AddWithValue("@installation_comment", item.installation_comment);
                                        command.Parameters.AddWithValue("@printerman", Utils.CheckDBNull.ToString(item.printerman));
                                        command.Parameters.AddWithValue("@cutterman", Utils.CheckDBNull.ToString(item.cutterman));
                                        command.Parameters.AddWithValue("@cncman", Utils.CheckDBNull.ToString(item.cncman));
                                        command.Parameters.AddWithValue("@print_quality", Utils.CheckDBNull.ToString(item.print_quality));
                                        command.Parameters.AddWithValue("@state", item.state);
                                        command.Parameters.AddWithValue("@state_print", Utils.CheckDBNull.ToBoolean(item.state_print));
                                        command.Parameters.AddWithValue("@state_cut", Utils.CheckDBNull.ToBoolean(item.state_cut));
                                        command.Parameters.AddWithValue("@state_cnc", Utils.CheckDBNull.ToBoolean(item.state_cnc));
                                        command.Parameters.AddWithValue("@state_install", Utils.CheckDBNull.ToBoolean(item.state_install));
                                        command.Parameters.AddWithValue("@change_count", change_count);
                                        command.Parameters.AddWithValue("@sender_row_id", item.sender_row_id);
                                        command.Parameters.AddWithValue("@sender_row_stringid", Utils.CheckDBNull.ToString(item.sender_row_stringid));
                                        command.Parameters.AddWithValue("@adder", Utils.CheckDBNull.ToString(item.adder));
                                        command.Parameters.AddWithValue("@worktypes_list", Utils.CheckDBNull.ToString(item.worktypes_list));
                                        command.Parameters.AddWithValue("@delivery", Utils.CheckDBNull.ToBoolean(item.delivery));
                                        command.Parameters.AddWithValue("@delivery_office", Utils.CheckDBNull.ToBoolean(item.delivery_office));
                                        command.Parameters.AddWithValue("@delivery_address", Utils.CheckDBNull.ToString(item.delivery_address));
                                        command.Parameters.AddWithValue("@baner_handling", Utils.CheckDBNull.ToBoolean(item.baner_handling));
                                        command.Parameters.AddWithValue("@baner_luvers", Utils.CheckDBNull.ToBoolean(item.baner_luvers));
                                        command.Parameters.AddWithValue("@baner_handling_size", item.baner_handling_size);
                                        command.ExecuteNonQuery();
                                    }
                                    item.change_count = change_count;

                                    item.command = (int)SocketServer.TableServer.SocketMessageCommand.RowsUpdate;
                                    #endregion

                                    #region Файлы
                                    string yearpath = Properties.Settings.Default.FilePath + @"/" + timerecieve.ToString("yyyy.MM");
                                    if (!Directory.Exists(yearpath)) { Directory.CreateDirectory(yearpath); }
                                    string orderPath = Properties.Settings.Default.FilePath + @"/" + timerecieve.ToString("yyyy.MM") + "/" + item.id.ToString("0000");
                                    if (!Directory.Exists(orderPath)) { Directory.CreateDirectory(orderPath); }
                                    if (!Directory.Exists(orderPath + @"/" + "makets")) { Directory.CreateDirectory(orderPath + @"/" + "makets"); }
                                    if (!Directory.Exists(orderPath + @"/" + "preview")) { Directory.CreateDirectory(orderPath + @"/" + "preview"); }
                                    if (!Directory.Exists(orderPath + @"/" + "doc")) { Directory.CreateDirectory(orderPath + @"/" + "doc"); }
                                    if (!Directory.Exists(orderPath + @"/" + "photoreport")) { Directory.CreateDirectory(orderPath + @"/" + "photoreport"); }

                                    #region превью
                                    if (item.preview != null)
                                    {

                                        using (FileStream fs = new FileStream(orderPath + @"/index.png", FileMode.Create, FileAccess.Write))
                                        {
                                            fs.Write(item.preview, 0, item.preview.Length);
                                        }
                                        FileInfo fileInf = new FileInfo(orderPath + @"/" + "index.png");
                                        ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow
                                        {
                                            fullname = fileInf.FullName.Replace(di.FullName + @"\", ""),
                                            LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime),
                                            Length = fileInf.Length,
                                            LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime),
                                            order_id = item.id
                                        };
                                        if (MySql.DataTables.preview != null && MySql.DataTables.preview.Count > 0)
                                        {
                                            if (MySql.DataTables.preview.ContainsKey(pr.fullname)) { MySql.DataTables.preview[pr.fullname] = pr; } else { MySql.DataTables.preview.TryAdd(pr.fullname, pr); }
                                        }
                                        else
                                        {
                                            MySql.DataTables.preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
                                            MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                                        }
                                    }
                                    #endregion
                                    #region монтаж - rtf
                                    if (item.installation_comment != null)
                                    {
                                        using (FileStream fs = new FileStream(orderPath + @"/montage.doc", FileMode.Create, FileAccess.Write))
                                        {
                                            fs.Write(item.installation_comment, 0, item.installation_comment.Length);
                                        }
                                        FileInfo fileInf = new FileInfo(orderPath + @"/" + "montage.doc");
                                        ProtoClasses.ProtoPreview.protoRow pr = new ProtoClasses.ProtoPreview.protoRow
                                        {
                                            fullname = fileInf.FullName.Replace(di.FullName + @"\", ""),
                                            LastWriteTime = Utils.UnixDate.DateTimeToInt64(fileInf.LastWriteTime),
                                            Length = fileInf.Length,
                                            LastCreationTime = Utils.UnixDate.DateTimeToInt64(fileInf.CreationTime),
                                            order_id = item.id
                                        };
                                        if (MySql.DataTables.preview != null && MySql.DataTables.preview.Count > 0)
                                        {
                                            if (MySql.DataTables.preview.ContainsKey(pr.fullname)) { MySql.DataTables.preview[pr.fullname] = pr; } else { MySql.DataTables.preview.TryAdd(pr.fullname, pr); }
                                        }
                                        else
                                        {
                                            MySql.DataTables.preview = new System.Collections.Concurrent.ConcurrentDictionary<string, ProtoClasses.ProtoPreview.protoRow>();
                                            MySql.DataTables.preview.TryAdd(pr.fullname, pr);
                                        }
                                    }
                                    #endregion
                                    #region Указание путей для закачки новых файлов по ftp
                                    if (item.FilesUpload != null && item.FilesUpload.Count > 0)
                                    {
                                        foreach (ProtoClasses.ProtoFtpSchedule.protoRow pr in item.FilesUpload)
                                        {
                                            pr.targetfile = "makets" + @"/" + timerecieve.ToString("yyyy.MM") + "/" + item.id.ToString("0000") + @"/" + pr.targetfile;
                                        }
                                    }
                                    #endregion

                                    #endregion
                                    try
                                    {
                                        result.Add(item);
                                        #region Обновление Datatable
                                        customerRow.change_count = change_count;
                                        customerRow.date_start = item.date_start;
                                        customerRow.dead_line = item.dead_line;
                                        customerRow.date_ready_print = item.date_ready_print;
                                        customerRow.date_ready_cut = item.date_ready_cut;
                                        customerRow.date_ready_cnc = item.date_ready_cnc;
                                        customerRow.client = Utils.CheckDBNull.ToString(item.client);
                                        customerRow.work_name = Utils.CheckDBNull.ToString(item.work_name);
                                        customerRow.material_print_id = item.material_print_id;
                                        customerRow.material_cut_id = item.material_cut_id;
                                        customerRow.material_cnc_id = item.material_cnc_id;
                                        customerRow.size_x_print = item.size_x_print;
                                        customerRow.size_y_print = item.size_y_print;
                                        customerRow.size_x_cut = item.size_x_cut;
                                        customerRow.size_y_cut = item.size_y_cut;
                                        customerRow.size_x_cnc = item.size_x_cnc;
                                        customerRow.size_y_cnc = item.size_y_cnc;
                                        customerRow.size_cut = item.size_cut;
                                        customerRow.line_size_cut = item.line_size_cut;
                                        customerRow.count_size_cut = item.count_size_cut;
                                        customerRow.size_cnc = item.size_cnc;
                                        customerRow.line_size_cnc = item.line_size_cnc;
                                        customerRow.count_size_cnc = item.count_size_cnc;
                                        customerRow.count_print = item.count_print;
                                        customerRow.count_cut = item.count_cut;
                                        customerRow.count_cnc = item.count_cnc;
                                        customerRow.square_print = item.square_print;
                                        customerRow.square_cut = item.square_cut;
                                        customerRow.square_cnc = item.square_cnc;
                                        customerRow.cutting_on_print = Utils.CheckDBNull.ToBoolean(item.cutting_on_print);
                                        customerRow.cnc_on_print = Utils.CheckDBNull.ToBoolean(item.cnc_on_print);
                                        customerRow.print_on = Utils.CheckDBNull.ToBoolean(item.print_on);
                                        customerRow.cut_on = Utils.CheckDBNull.ToBoolean(item.cut_on);
                                        customerRow.cnc_on = Utils.CheckDBNull.ToBoolean(item.cnc_on);
                                        customerRow.printers_id = item.printers_id;
                                        customerRow.cutters_id = item.cutters_id;
                                        customerRow.cncs_id = item.cncs_id;
                                        customerRow.comments = Utils.CheckDBNull.ToString(item.comments);
                                        customerRow.laminat = Utils.CheckDBNull.ToBoolean(item.laminat);
                                        customerRow.laminat_mat = Utils.CheckDBNull.ToBoolean(item.laminat_mat);
                                        customerRow.installation = Utils.CheckDBNull.ToBoolean(item.installation);
                                        //customerRow.installation_comment = item.installation_comment;
                                        customerRow.printerman = Utils.CheckDBNull.ToString(item.printerman);
                                        customerRow.cutterman = Utils.CheckDBNull.ToString(item.cutterman);
                                        customerRow.cncman = Utils.CheckDBNull.ToString(item.cncman);
                                        customerRow.print_quality = Utils.CheckDBNull.ToString(item.print_quality);
                                        customerRow.state = item.state;
                                        customerRow.state_print = Utils.CheckDBNull.ToBoolean(item.state_print);
                                        customerRow.state_cut = Utils.CheckDBNull.ToBoolean(item.state_cut);
                                        customerRow.state_cnc = Utils.CheckDBNull.ToBoolean(item.state_cnc);
                                        customerRow.state_install = Utils.CheckDBNull.ToBoolean(item.state_install);
                                        customerRow.sender_row_id = item.sender_row_id;
                                        customerRow.sender_row_stringid = Utils.CheckDBNull.ToString(item.sender_row_stringid);
                                        customerRow.adder = Utils.CheckDBNull.ToString(item.adder);
                                        customerRow.worktypes_list = Utils.CheckDBNull.ToString(item.worktypes_list);
                                        customerRow.delivery = Utils.CheckDBNull.ToBoolean(item.delivery);
                                        customerRow.delivery_office = Utils.CheckDBNull.ToBoolean(item.delivery_office);
                                        customerRow.delivery_address = Utils.CheckDBNull.ToString(item.delivery_address);
                                        customerRow.baner_handling = Utils.CheckDBNull.ToBoolean(item.baner_handling);
                                        customerRow.baner_luvers = Utils.CheckDBNull.ToBoolean(item.baner_luvers);
                                        customerRow.baner_handling_size = item.baner_handling_size;
                                        #endregion
                                        if (DataTables.orders.ContainsKey(item.id)) { DataTables.orders[item.id] = customerRow; }
                                    }
                                    catch (Exception ex)
                                    {
                                    }


                                    #region Запись в историю заявки
                                    ProtoClasses.ProtoOrderHistory.protoRow protoRowHistory = new ProtoClasses.ProtoOrderHistory.protoRow
                                    {
                                        adder = item.adder,
                                        note = "Внесены изменения",
                                        status_task = item.state,
                                        work_id = item.id
                                    };
                                    resultHistory.Add(protoRowHistory);
                                    if (item.HistoryRows != null && item.HistoryRows.Count > 0) { resultHistory.AddRange(item.HistoryRows); }
                                    item.HistoryRows = OrderHistory.InsertRow(resultHistory, item.id, ClientName);
                                    #endregion

                                    #region Удаление файлов если есть
                                    if (item.DeleteFilesList != null && item.DeleteFilesList.Count > 0)
                                    {
                                        //DirectoryInfo di = new DirectoryInfo(Properties.Settings.Default.FilePath);
                                        foreach (ProtoClasses.ProtoFiles.protoRow deletefile in item.DeleteFilesList)
                                        {
                                            //FileInfo fileInfo = new FileInfo();
                                            try
                                            {
                                                File.Delete(Properties.Settings.Default.FilePath + deletefile.fullname);
                                            }
                                            catch { }
                                            }
                                    }

                                    #endregion

                                }
                            }
                            transaction.Commit();
                        }
                    }
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region UPDATE Статус заявки/готовность работ
        public ProtoClasses.ProtoOrdersChangeState.protoRowsList UpdateState(ProtoClasses.ProtoOrdersChangeState.protoRowsList uptab, string ClientName)
        {
            if (uptab != null && uptab.plist != null && uptab.plist.Count > 0 && DataTables.orders != null)
            {
                List<ProtoClasses.ProtoOrderHistory.protoRow> resultHistory = new List<ProtoClasses.ProtoOrderHistory.protoRow>();
                DateTime dateTimeNow = DateTime.Now;
                long change_count = Utils.UnixDate.DateTimeToInt64(dateTimeNow);
                int state = 0; bool state_print = false; bool state_cut = false; bool state_cnc = false; bool state_install = false;
                bool print_on = false; bool cut_on = false; bool cnc_on = false; bool installation = false;
                //string printerman = string.Empty; string cutterman = string.Empty; string cncman = string.Empty;
                DateTime date_ready_print = dateTimeNow; DateTime date_ready_cut = dateTimeNow; DateTime date_ready_cnc = dateTimeNow;
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    //using (MySqlTransaction transaction = connection.BeginTransaction())
                    //{
                    foreach (ProtoClasses.ProtoOrdersChangeState.protoRow item in uptab.plist)
                    {
                        if (item.id != 0)
                        {
                            ProtoClasses.ProtoOrders.protoOrder customerRow = null;
                            if (DataTables.orders.ContainsKey(item.id)) { customerRow = DataTables.orders[item.id]; }


                            if (customerRow != null)
                            {

                                item.printerman = customerRow.printerman;
                                item.cutterman = customerRow.cutterman;
                                item.cncman = customerRow.cncman;
                                date_ready_print = Utils.CheckDBNull.ToDateTime(customerRow.date_ready_print);
                                date_ready_cut = Utils.CheckDBNull.ToDateTime(customerRow.date_ready_cut);
                                date_ready_cnc = Utils.CheckDBNull.ToDateTime(customerRow.date_ready_cnc);

                                state_print = customerRow.state_print;
                                state_cut = customerRow.state_cut;
                                state_cnc = customerRow.state_cnc;
                                state_install = customerRow.state_install;
                                state = customerRow.state;

                                print_on = customerRow.print_on;
                                cut_on = customerRow.cut_on;
                                cnc_on = customerRow.cnc_on;
                                installation = customerRow.installation;

                                state = customerRow.state;
                                #region Операции по типам работ в историю
                                if (print_on)
                                {
                                    if (state_print != item.state_print)
                                    {
                                        ProtoClasses.ProtoOrderHistory.protoRow newhistrow = new ProtoClasses.ProtoOrderHistory.protoRow
                                        {
                                            work_id = item.id,
                                            status_task = item.state,
                                            adder = uptab.adder
                                        };
                                        if (state_print) { newhistrow.note = "Готовность печати снята"; }
                                        else
                                        {
                                            newhistrow.note = "Задание напечатано";
                                            item.printerman = uptab.adder;
                                            date_ready_print = dateTimeNow;

                                        }
                                        resultHistory.Add(newhistrow);
                                    }
                                }
                                if (cut_on)
                                {
                                    if (state_cut != item.state_cut)
                                    {
                                        ProtoClasses.ProtoOrderHistory.protoRow newhistrow = new ProtoClasses.ProtoOrderHistory.protoRow()
                                        {
                                            work_id = item.id,
                                            status_task = item.state,
                                            adder = uptab.adder
                                        };

                                        if (state_cut) { newhistrow.note = "Готовность плот. резки снята"; }
                                        else
                                        {
                                            newhistrow.note = "Задание вырезано";
                                            item.cutterman = uptab.adder;
                                            date_ready_cut = dateTimeNow;
                                        }
                                        resultHistory.Add(newhistrow);
                                    }
                                }
                                if (cnc_on)
                                {
                                    if (state_cnc != item.state_cnc)
                                    {
                                        ProtoClasses.ProtoOrderHistory.protoRow newhistrow = new ProtoClasses.ProtoOrderHistory.protoRow()
                                        {
                                            work_id = item.id,
                                            status_task = item.state,
                                            adder = uptab.adder
                                        };
                                        if (state_cnc) { newhistrow.note = "Готовность фрезеровки снята"; }
                                        else
                                        {
                                            newhistrow.note = "Задание отфрезеровано";
                                            item.cncman = uptab.adder;
                                            date_ready_cnc = dateTimeNow;
                                        }
                                        resultHistory.Add(newhistrow);
                                    }
                                }
                                if (installation)
                                {
                                    if (state_install != item.state_install)
                                    {
                                        ProtoClasses.ProtoOrderHistory.protoRow newhistrow = new ProtoClasses.ProtoOrderHistory.protoRow()
                                        {
                                            work_id = item.id,
                                            status_task = item.state,
                                            adder = uptab.adder
                                        };
                                        if (state_install)
                                        {
                                            newhistrow.note = "Готовность монтажа снята";
                                        }
                                        else
                                        {
                                            newhistrow.note = "Осуществлен монтаж";
                                        }
                                        resultHistory.Add(newhistrow);

                                    }
                                }
                                #endregion

                                #region текст для смены статуса заявки в историю
                                StringBuilder stringBuilder = new StringBuilder();
                                if (state != item.state)
                                {
                                    switch (state)
                                    {
                                        case 0:
                                            stringBuilder.Append("Из ожидающих ");
                                            break;
                                        case 1:
                                            stringBuilder.Append("С работы ");
                                            break;
                                        case 2:
                                            stringBuilder.Append("С постобработки ");
                                            break;
                                        case 3:
                                            stringBuilder.Append("Со склада ");
                                            break;
                                        case 4:
                                            stringBuilder.Append("С закрытых ");
                                            break;
                                        case 5:
                                            stringBuilder.Append("С остановленных ");
                                            break;
                                        case 6:
                                            stringBuilder.Append("Из корзины ");
                                            break;
                                        case 7:
                                            stringBuilder.Append("С черновиков ");
                                            break;
                                    }
                                    switch (item.state)
                                    {
                                        case 0:
                                            stringBuilder.Append("переведено в ожидающие");
                                            break;
                                        case 1:
                                            stringBuilder.Append("переведено в работу");
                                            break;
                                        case 2:
                                            stringBuilder.Append("переведено в постобработку");
                                            break;
                                        case 3:
                                            stringBuilder.Append("переведено на склад");
                                            break;
                                        case 4:
                                            stringBuilder.Append("переведено в закрытые");
                                            break;
                                        case 5:
                                            stringBuilder.Append("помещено в остановленные");
                                            break;
                                        case 6:
                                            stringBuilder.Append("помещено в корзину");
                                            break;
                                        case 7:
                                            stringBuilder.Append("помещено в черновики ");
                                            break;
                                    }

                                    if (stringBuilder.Length > 0)
                                    {
                                        ProtoClasses.ProtoOrderHistory.protoRow newhistrow = new ProtoClasses.ProtoOrderHistory.protoRow
                                        {
                                            note = stringBuilder.ToString(),
                                            work_id = item.id,
                                            status_task = item.state,
                                            adder = uptab.adder
                                        };
                                        resultHistory.Add(newhistrow);
                                    }
                                }
                                #endregion
                                try
                                {
                                    #region Update mysql
                                    using (MySqlCommand command = new MySqlCommand(@"UPDATE `base`  SET 
                            `state` = @state, 
                            `state_print` = @state_print, 
                            `state_cut` = @state_cut, 
                            `state_cnc` = @state_cnc, 
                            `state_install` = @state_install, 
                            `change_count` = @change_count, 
                            `printerman` = @printerman, 
                            `cutterman` = @cutterman, 
                            `cncman` = @cncman, 
                            `date_ready_print` = @date_ready_print, 
                            `date_ready_cut` = @date_ready_cut, 
                            `date_ready_cnc` = @date_ready_cnc
                            WHERE `id` = @id ; ", connection))
                                    {
                                        //command.Transaction = transaction;
                                        command.Parameters.AddWithValue("@id", item.id);
                                        command.Parameters.AddWithValue("@state", item.state);
                                        command.Parameters.AddWithValue("@state_print", item.state_print);
                                        command.Parameters.AddWithValue("@state_cut", item.state_cut);
                                        command.Parameters.AddWithValue("@state_cnc", item.state_cnc);
                                        command.Parameters.AddWithValue("@state_install", item.state_install);
                                        command.Parameters.AddWithValue("@change_count", change_count);
                                        command.Parameters.AddWithValue("@printerman", item.printerman);
                                        command.Parameters.AddWithValue("@cutterman", item.cutterman);
                                        command.Parameters.AddWithValue("@cncman", item.cncman);
                                        command.Parameters.AddWithValue("@date_ready_print", date_ready_print);
                                        command.Parameters.AddWithValue("@date_ready_cut", date_ready_cut);
                                        command.Parameters.AddWithValue("@date_ready_cnc", date_ready_cnc);
                                        command.ExecuteNonQuery();
                                        //transaction.Commit();
                                    }
                                    item.change_count = change_count;
                                    //item.printerman = item.printerman;
                                    //item.cutterman = cutterman;
                                    //item.cncman = cncman;
                                    item.date_ready_print = Utils.UnixDate.CheckedDateTimeToInt64(date_ready_print);
                                    item.date_ready_cut = Utils.UnixDate.CheckedDateTimeToInt64(date_ready_cut);
                                    item.date_ready_cnc = Utils.UnixDate.CheckedDateTimeToInt64(date_ready_cnc);
                                    #endregion

                                    #region Обновление Datatable
                                    customerRow.change_count = change_count;
                                    customerRow.state_print = item.state_print;
                                    customerRow.state_cut = item.state_cut;
                                    customerRow.state_cnc = item.state_cnc;
                                    customerRow.state_install = item.state_install;
                                    customerRow.state = item.state;
                                    customerRow.printerman = item.printerman;
                                    customerRow.cutterman = item.cutterman;
                                    customerRow.cncman = item.cncman;
                                    customerRow.date_ready_print = item.date_ready_print;
                                    customerRow.date_ready_cut = item.date_ready_cut;
                                    customerRow.date_ready_cnc = item.date_ready_cnc;
                                    customerRow.change_count = change_count;
                                    if (DataTables.orders.ContainsKey(item.id)) { DataTables.orders[item.id] = customerRow; }
                                    #endregion
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }

                    }
                    try
                    {
                        //transaction.Commit();
                    }
                    catch (Exception ex)
                    {

                    }
                    //}
                }

                if (resultHistory != null && resultHistory.Count > 0)
                { uptab.HistoryRows = OrderHistory.InsertRow(resultHistory, 0, ClientName); }
                return uptab;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Вставить/удалить заявку
        //public void InsertUpdate(List<ProtoClasses.ProtoOrders.protoOrder> uptab, SocketServer.TableServer.SocketMessageCommand socketMessageCommand)
        //{

        //    try
        //    {
        //        if (uptab != null && uptab.Count > 0 && DataTables.orders != null)
        //        {
        //            //                    //DataRow dr =  DataTables.orders.Select
        //            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        //            {
        //                connection.Open();

        //                using (MySqlTransaction transaction = connection.BeginTransaction())
        //                {
        //                    foreach (ProtoClasses.ProtoOrders.protoOrder item in uptab)
        //                    {
        //                        if (item.id != 0)
        //                        {
        //                            DataRow customerRow = DataTables.orders.Select("id = " + item.id).FirstOrDefault();

        //                        if (customerRow != null)
        //                        {
        //                            switch (socketMessageCommand)
        //                            {
        //                                case SocketServer.TableServer.SocketMessageCommand.RowsUpdate:
        //                                    DateTime dateTimeNow = DateTime.Now;
        //                                    long change_count = Utils.UnixDate.DateTimeToInt64(dateTimeNow);
        //                                    #region Update
        //                                    using (MySqlCommand command = new MySqlCommand(@"UPDATE `base`  SET 
        //                    `date_start` = @date_start, 
        //                    `dead_line` = @dead_line, 
        //                    `date_ready_print` = @date_ready_print, 
        //                    `date_ready_cut` = @date_ready_cut, 
        //                    `date_ready_cnc` = @date_ready_cnc, 
        //                    `client` = @client, 
        //                    `work_name` = @work_name, 
        //                    `material_print_id` = @material_print_id, 
        //                    `material_cut_id` = @material_cut_id, 
        //                    `material_cnc_id` = @material_cnc_id, 
        //                    `size_x_print` = @size_x_print, 
        //                    `size_y_print` = @size_y_print, 
        //                    `size_x_cut` = @size_x_cut, 
        //                    `size_y_cut` = @size_y_cut, 
        //                    `size_x_cnc` = @size_x_cnc, 
        //                    `size_y_cnc` = @size_y_cnc, 
        //                    `size_cut` = @size_cut, 
        //                    `line_size_cut` = @line_size_cut, 
        //                    `count_size_cut` = @count_size_cut, 
        //                    `size_cnc` = @size_cnc, 
        //                    `line_size_cnc` = @line_size_cnc, 
        //                    `count_size_cnc` = @count_size_cnc, 
        //                    `count_print` = @count_print, 
        //                    `count_cut` = @count_cut, 
        //                    `count_cnc` = @count_cnc, 
        //                    `square_print` = @square_print, 
        //                    `square_cut` = @square_cut, 
        //                    `square_cnc` = @square_cnc, 
        //                    `cutting_on_print` = @cutting_on_print, 
        //                    `cnc_on_print` = @cutting_on_print, 
        //                    `print_on` = @print_on, 
        //                    `cut_on` = @cut_on, 
        //                    `cnc_on` = @cnc_on, 
        //                    `printers_id` = @printers_id, 
        //                    `cutters_id` = @cutters_id, 
        //                    `cncs_id` = @cncs_id, 
        //                    `comments` = @comments, 
        //                    `laminat` = @laminat, 
        //                    `laminat_mat` = @laminat_mat, 
        //                    `installation` = @installation, 
        //                    `installation_comment` = @installation_comment, 
        //                    `printerman` = @printerman, 
        //                    `cutterman` = @cutterman, 
        //                    `cncman` = @cncman, 
        //                    `adder` = @adder, 
        //                    `print_quality` = @print_quality, 
        //                    `state` = @state, 
        //                    `state_print` = @state_print, 
        //                    `state_cut` = @state_cut, 
        //                    `state_cnc` = @state_cnc,
        //                    `change_count` = @change_count,
        //                    `sender_row_id` = @sender_row_id,
        //                    `sender_row_stringid` = @sender_row_stringid
        //                    WHERE `id` = @id ; ", connection))
        //                                    {
        //                                        command.Transaction = transaction;
        //                                        command.Parameters.AddWithValue("@id", item.id);
        //                                        command.Parameters.AddWithValue("@date_start", Utils.UnixDate.Int64ToDateTime(item.date_start));
        //                                        command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line));
        //                                        command.Parameters.AddWithValue("@date_ready_print", Utils.UnixDate.Int64ToDateTime(item.date_ready_print));
        //                                        command.Parameters.AddWithValue("@date_ready_cut", Utils.UnixDate.Int64ToDateTime(item.date_ready_cut));
        //                                        command.Parameters.AddWithValue("@date_ready_cnc", Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc));
        //                                        command.Parameters.AddWithValue("@client", Utils.CheckDBNull.ToString(item.client));
        //                                        command.Parameters.AddWithValue("@work_name", Utils.CheckDBNull.ToString(item.work_name));
        //                                        command.Parameters.AddWithValue("@material_print_id", Utils.CheckDBNull.ToInt32(item.material_print_id));
        //                                        command.Parameters.AddWithValue("@material_cut_id", Utils.CheckDBNull.ToInt32(item.material_cut_id));
        //                                        command.Parameters.AddWithValue("@material_cnc_id", Utils.CheckDBNull.ToInt32(item.material_cnc_id));
        //                                        command.Parameters.AddWithValue("@size_x_print", item.size_x_print);
        //                                        command.Parameters.AddWithValue("@size_y_print", item.size_y_cnc);
        //                                        command.Parameters.AddWithValue("@size_x_cut", item.size_x_cut);
        //                                        command.Parameters.AddWithValue("@size_y_cut", item.size_y_cut);
        //                                        command.Parameters.AddWithValue("@size_x_cnc", item.size_x_cnc);
        //                                        command.Parameters.AddWithValue("@size_y_cnc", item.size_y_cnc);
        //                                        command.Parameters.AddWithValue("@size_cut", item.size_cut);
        //                                        command.Parameters.AddWithValue("@line_size_cut", item.line_size_cut);
        //                                        command.Parameters.AddWithValue("@count_size_cut", item.count_size_cut);
        //                                        command.Parameters.AddWithValue("@size_cnc", item.size_cnc);
        //                                        command.Parameters.AddWithValue("@line_size_cnc", item.line_size_cnc);
        //                                        command.Parameters.AddWithValue("@count_size_cnc", item.count_size_cnc);
        //                                        command.Parameters.AddWithValue("@count_print", item.count_print);
        //                                        command.Parameters.AddWithValue("@count_cut", item.count_cut);
        //                                        command.Parameters.AddWithValue("@count_cnc", item.count_cnc);
        //                                        command.Parameters.AddWithValue("@square_print", item.square_print);
        //                                        command.Parameters.AddWithValue("@square_cut", item.square_cut);
        //                                        command.Parameters.AddWithValue("@square_cnc", item.square_cnc);
        //                                        command.Parameters.AddWithValue("@cutting_on_print", item.cutting_on_print);
        //                                        command.Parameters.AddWithValue("@cnc_on_print", item.cnc_on_print);
        //                                        command.Parameters.AddWithValue("@print_on", item.print_on);
        //                                        command.Parameters.AddWithValue("@cut_on", item.cut_on);
        //                                        command.Parameters.AddWithValue("@cnc_on", item.cnc_on);
        //                                        command.Parameters.AddWithValue("@printers_id", item.printers_id);
        //                                        command.Parameters.AddWithValue("@cutters_id", item.cutters_id);
        //                                        command.Parameters.AddWithValue("@cncs_id", item.cncs_id);
        //                                        command.Parameters.AddWithValue("@comments", item.comments);
        //                                        command.Parameters.AddWithValue("@laminat", item.laminat);
        //                                        command.Parameters.AddWithValue("@laminat_mat", item.laminat_mat);
        //                                        command.Parameters.AddWithValue("@installation", item.installation);
        //                                        command.Parameters.AddWithValue("@installation_comment", item.installation_comment);
        //                                        command.Parameters.AddWithValue("@printerman", item.printerman);
        //                                        command.Parameters.AddWithValue("@cutterman", item.cutterman);
        //                                        command.Parameters.AddWithValue("@cncman", item.cncman);
        //                                        command.Parameters.AddWithValue("@print_quality", item.print_quality);
        //                                        command.Parameters.AddWithValue("@state", item.state);
        //                                        command.Parameters.AddWithValue("@state_print", item.state_print);
        //                                        command.Parameters.AddWithValue("@state_cut", item.state_cut);
        //                                        command.Parameters.AddWithValue("@state_cnc", item.state_cnc);
        //                                        command.Parameters.AddWithValue("@change_count", change_count);
        //                                        command.Parameters.AddWithValue("@sender_row_id", item.sender_row_id);
        //                                        command.Parameters.AddWithValue("@sender_row_stringid", item.sender_row_stringid);
        //                                        command.Parameters.AddWithValue("@adder", item.adder);
        //                                        command.ExecuteNonQuery();
        //                                        #region
        //                                        //command.Parameters.Add("@date_start", DbType.Int64).Value = customerRow["date_start"] = item.date_start;
        //                                        //command.Parameters.Add("@dead_line", DbType.Int64).Value = customerRow["dead_line"] = item.dead_line;
        //                                        //command.Parameters.Add("@date_ready_print", DbType.Int64).Value = customerRow["date_ready_print"] = item.date_ready_print;
        //                                        //command.Parameters.Add("@date_ready_cut", DbType.Int64).Value = customerRow["date_ready_cut"] = item.date_ready_cut;
        //                                        //command.Parameters.Add("@date_ready_cnc", DbType.Int64).Value = customerRow["date_ready_cnc"] = item.date_ready_cnc;
        //                                        //command.Parameters.Add("@client", DbType.String).Value = customerRow["client"] = item.client;
        //                                        //command.Parameters.Add("@work_name", DbType.String).Value = customerRow["work_name"] = item.work_name;
        //                                        //command.Parameters.Add("@material_print_id", DbType.Int32).Value = customerRow["material_print_id"] = item.material_print_id;
        //                                        //command.Parameters.Add("@material_cut_id", DbType.Int32).Value = customerRow["material_cut_id"] = item.material_cut_id;
        //                                        //command.Parameters.Add("@material_cnc_id", DbType.Int32).Value = customerRow["material_cnc_id"] = item.material_cnc_id;
        //                                        //command.Parameters.Add("@size_x_print", DbType.Double).Value = customerRow["size_x_print"] = item.size_x_print;
        //                                        //command.Parameters.Add("@size_y_print", DbType.Double).Value = customerRow["size_y_print"] = item.size_y_print;
        //                                        //command.Parameters.Add("@size_x_cut", DbType.Double).Value = customerRow["size_x_cut"] = item.size_x_cut;
        //                                        //command.Parameters.Add("@size_y_cut", DbType.Double).Value = customerRow["size_y_cut"] = item.size_y_cut;
        //                                        //command.Parameters.Add("@size_x_cnc", DbType.Double).Value = customerRow["size_x_cnc"] = item.size_x_cnc;
        //                                        //command.Parameters.Add("@size_y_cnc", DbType.Double).Value = customerRow["size_y_cnc"] = item.size_y_cnc;
        //                                        //command.Parameters.Add("@size_cut", DbType.Double).Value = customerRow["size_cut"] = item.size_cut;
        //                                        //command.Parameters.Add("@line_size_cut", DbType.Double).Value = customerRow["line_size_cut"] = item.line_size_cut;
        //                                        //command.Parameters.Add("@count_size_cut", DbType.Int32).Value = customerRow["count_size_cut"] = item.count_size_cut;
        //                                        //command.Parameters.Add("@size_cnc", DbType.Double).Value = customerRow["size_cnc"] = item.size_cnc;
        //                                        //command.Parameters.Add("@line_size_cnc", DbType.Double).Value = customerRow["line_size_cnc"] = item.line_size_cnc;
        //                                        //command.Parameters.Add("@count_size_cnc", DbType.Int32).Value = customerRow["count_size_cnc"] = item.count_size_cnc;
        //                                        //command.Parameters.Add("@count_print", DbType.Int32).Value = customerRow["count_print"] = item.count_print;
        //                                        //command.Parameters.Add("@count_cut", DbType.Int32).Value = customerRow["count_cut"] = item.count_cut;
        //                                        //command.Parameters.Add("@count_cnc", DbType.Int32).Value = customerRow["count_cnc"] = item.count_cnc;
        //                                        //command.Parameters.Add("@square_print", DbType.Double).Value = customerRow["square_print"] = item.square_print;
        //                                        //command.Parameters.Add("@square_cut", DbType.Double).Value = customerRow["square_cut"] = item.square_cut;
        //                                        //command.Parameters.Add("@square_cnc", DbType.Double).Value = customerRow["square_cnc"] = item.square_cnc;
        //                                        //command.Parameters.Add("@cutting_on_print", DbType.Int32).Value = customerRow["cutting_on_print"] = Convert.ToInt32(item.cutting_on_print);
        //                                        //command.Parameters.Add("@print_on", DbType.Int32).Value = customerRow["print_on"] = Convert.ToInt16(item.print_on);
        //                                        //command.Parameters.Add("@cut_on", DbType.Int32).Value = customerRow["cut_on"] = Convert.ToInt16(item.cut_on);
        //                                        //command.Parameters.Add("@cnc_on", DbType.Int32).Value = customerRow["cnc_on"] = Convert.ToInt16(item.cnc_on);
        //                                        //command.Parameters.Add("@printers_id", DbType.Int32).Value = customerRow["printers_id"] = item.printers_id;
        //                                        //command.Parameters.Add("@cutters_id", DbType.Int32).Value = customerRow["cutters_id"] = item.cutters_id;
        //                                        //command.Parameters.Add("@cncs_id", DbType.Int32).Value = customerRow["cncs_id"] = item.cncs_id;
        //                                        //command.Parameters.Add("@comments", DbType.String).Value = customerRow["comments"] = item.comments;
        //                                        //command.Parameters.Add("@laminat", DbType.Int32).Value = customerRow["laminat"] = Convert.ToInt16(item.laminat);
        //                                        //command.Parameters.Add("@laminat_mat", DbType.Int32).Value = customerRow["laminat_mat"] = Convert.ToInt16(item.laminat_mat);
        //                                        //command.Parameters.Add("@installation", DbType.Int32).Value = customerRow["installation"] = Convert.ToInt16(item.installation);
        //                                        //command.Parameters.Add("@installation_comment", DbType.String).Value = customerRow["installation_comment"] = item.installation_comment;
        //                                        //command.Parameters.Add("@printerman", DbType.String).Value = customerRow["printerman"] = item.printerman;
        //                                        //command.Parameters.Add("@cutterman", DbType.String).Value = customerRow["cutterman"] = item.cutterman;
        //                                        //command.Parameters.Add("@cncman", DbType.String).Value = customerRow["cncman"] = item.cncman;
        //                                        //command.Parameters.Add("@adder", DbType.String).Value = customerRow["adder"] = item.adder;
        //                                        //command.Parameters.Add("@print_quality", DbType.String).Value = customerRow["print_quality"] = item.print_quality;
        //                                        //command.Parameters.Add("@state", DbType.Int32).Value = customerRow["status"] = item.state;
        //                                        //command.Parameters.Add("@state_print", DbType.Int32).Value = customerRow["state_print"] = Convert.ToInt16(item.state_print);
        //                                        //command.Parameters.Add("@state_cut", DbType.Int32).Value = customerRow["state_cut"] = Convert.ToInt16(item.state_cut);
        //                                        //command.Parameters.Add("@state_cnc", DbType.Int32).Value = customerRow["state_cnc"] = Convert.ToInt16(item.state_cnc);
        //                                        //command.Parameters.Add("@date_preview", DbType.Int64).Value = customerRow["date_preview"] = item.date_preview;
        //                                        //command.Parameters.Add("@path_preview", DbType.String).Value = customerRow["path_preview"] = item.path_preview;
        //                                        //command.Parameters.Add("@path_maket", DbType.String).Value = customerRow["path_maket"] = item.path_maket;
        //                                        //command.Parameters.Add("@change_count", DbType.Int64).Value = customerRow["change_count"] = item.change_count;
        //                                        //command.Parameters.Add("@time_recieve", DbType.Int64).Value = customerRow["time_recieve"] = item.time_recieve;

        //                                        //if (item.preview != null)
        //                                        //{
        //                                        //    command.Parameters.Add("@preview", DbType.Object, item.preview.Length).Value = customerRow["preview"] = item.preview;
        //                                        //}
        //                                        //else { command.Parameters.Add("@preview", DbType.Object).Value = customerRow["preview"] = null; }
        //                                        #endregion
        //                                        //try
        //                                        //{

        //                                        //}
        //                                        //catch (Exception ex)
        //                                        //{

        //                                        //}
        //                                        //`date_preview` = @date_preview, 
        //                                        //`path_preview` = @path_preview, 
        //                                        //`path_maket` = @path_maket, 

        //                                        //`time_recieve` = @time_recieve,
        //                                        //`preview` = @preview



        //                                        //customerRow["String_id"] = Convert.ToString(customerRow["id"]);
        //                                        //customerRow["Datetime_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["date_start"]));
        //                                        //customerRow["Datetime_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["dead_line"]));
        //                                        //customerRow["String_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["date_start"])).ToString("dd.MM.yyyy HH:mm");
        //                                        //customerRow["String_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["dead_line"])).ToString("dd.MM.yyyy HH:mm");
        //                                        //customerRow["Datetime_date_ready_print"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["date_ready_print"]));
        //                                        //customerRow["Datetime_date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["date_ready_cut"]));
        //                                        //customerRow["Datetime_date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(customerRow["date_ready_cnc"]));

        //                                        //int print = 0;
        //                                        //int cut = 0;
        //                                        //int cnc = 0;
        //                                        //int install = 0;
        //                                        //int status = Convert.ToInt32(customerRow["status"]);
        //                                        //if (Convert.ToBoolean(customerRow["print_on"])) { if (Convert.ToBoolean(customerRow["state_print"]) || status == 3) { print = 2; } else { print = 1; } }
        //                                        //if (Convert.ToBoolean(customerRow["cut_on"])) { if (Convert.ToBoolean(customerRow["state_cut"]) || status == 3) { cut = 2; } else { cut = 1; } }
        //                                        //if (Convert.ToBoolean(customerRow["cnc_on"])) { if (Convert.ToBoolean(customerRow["state_cnc"]) || status == 3) { cnc = 2; } else { cnc = 1; } }
        //                                        //if (Convert.ToBoolean(customerRow["installation"])) { if (status == 3) { install = 2; } else { install = 1; } }
        //                                        //customerRow["Image_WorkTypes"] = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);

        //                                    }
        //                                    #endregion

        //                                    break;
        //                                case SocketServer.TableServer.SocketMessageCommand.RowsDelete:


        //                                    break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            #region Insert
        //                            using (MySqlCommand command = new MySqlCommand(@"INSERT INTO base (
        //                    date_start, 
        //                    dead_line, 
        //                    date_ready_print, 
        //                    date_ready_cut, 
        //                    date_ready_cnc, 
        //                    client, 
        //                    work_name, 
        //                    material_print_id, 
        //                    material_cut_id, 
        //                    material_cnc_id, 
        //                    size_x_print, 
        //                    size_y_print, 
        //                    size_x_cut, 
        //                    size_y_cut, 
        //                    size_x_cnc, 
        //                    size_y_cnc, 
        //                    size_cut, 
        //                    line_size_cut, 
        //                    count_size_cut, 
        //                    size_cnc, 
        //                    line_size_cnc, 
        //                    count_size_cnc, 
        //                    count_print, 
        //                    count_cut, 
        //                    count_cnc, 
        //                    square_print, 
        //                    square_cut, 
        //                    square_cnc, 
        //                    cutting_on_print, 
        //                    cnc_on_print, 
        //                    print_on, 
        //                    cut_on, 
        //                    cnc_on, 
        //                    printers_id, 
        //                    cutters_id, 
        //                    cncs_id, 
        //                    comments, 
        //                    laminat, 
        //                    laminat_mat, 
        //                    installation, 
        //                    installation_comment, 
        //                    printerman, 
        //                    cutterman, 
        //                    cncman, 
        //                    adder, 
        //                    print_quality, 
        //                    state, 
        //                    state_print, 
        //                    state_cut, 
        //                    state_cnc,
        //                    change_count,
        //                    sender_row_id,
        //                    sender_row_stringid
        //                    ) VALUES (
        //                    @date_start, 
        //                    @dead_line, 
        //                    @date_ready_print, 
        //                    @date_ready_cut, 
        //                    @date_ready_cnc, 
        //                    @client, 
        //                    @work_name, 
        //                    @material_print_id, 
        //                    @material_cut_id, 
        //                    @material_cnc_id, 
        //                    @size_x_print, 
        //                    @size_y_print, 
        //                    @size_x_cut, 
        //                    @size_y_cut, 
        //                    @size_x_cnc, 
        //                    @size_y_cnc, 
        //                    @size_cut, 
        //                    @line_size_cut, 
        //                    @count_size_cut, 
        //                    @size_cnc, 
        //                    @line_size_cnc, 
        //                    @count_size_cnc, 
        //                    @count_print, 
        //                    @count_cut, 
        //                    @count_cnc, 
        //                    @square_print, 
        //                    @square_cut, 
        //                    @square_cnc, 
        //                    @cutting_on_print, 
        //                    @cutting_on_print, 
        //                    @print_on, 
        //                    @cut_on, 
        //                    @cnc_on, 
        //                    @printers_id, 
        //                    @cutters_id, 
        //                    @cncs_id, 
        //                    @comments, 
        //                    @laminat, 
        //                    @laminat_mat, 
        //                    @installation, 
        //                    @installation_comment, 
        //                    @printerman, 
        //                    @cutterman, 
        //                    @cncman, 
        //                    @adder, 
        //                    @print_quality, 
        //                    @state, 
        //                    @state_print, 
        //                    @state_cut, 
        //                    @state_cnc,
        //                    @change_count,
        //                    @sender_row_id,
        //                    @sender_row_stringid
        //                    )", connection))
        //                            {
        //                                command.Transaction = transaction;
        //                                command.Parameters.AddWithValue("@id", item.id);
        //                                command.Parameters.AddWithValue("@date_start", Utils.UnixDate.Int64ToDateTime(item.date_start));
        //                                command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line));
        //                                command.Parameters.AddWithValue("@date_ready_print", Utils.UnixDate.Int64ToDateTime(item.date_ready_print));
        //                                command.Parameters.AddWithValue("@date_ready_cut", Utils.UnixDate.Int64ToDateTime(item.date_ready_cut));
        //                                command.Parameters.AddWithValue("@date_ready_cnc", Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc));
        //                                command.Parameters.AddWithValue("@client", Utils.CheckDBNull.ToString(item.client));
        //                                command.Parameters.AddWithValue("@work_name", Utils.CheckDBNull.ToString(item.work_name));
        //                                command.Parameters.AddWithValue("@material_print_id", Utils.CheckDBNull.ToInt32(item.material_print_id));
        //                                command.Parameters.AddWithValue("@material_cut_id", Utils.CheckDBNull.ToInt32(item.material_cut_id));
        //                                command.Parameters.AddWithValue("@material_cnc_id", Utils.CheckDBNull.ToInt32(item.material_cnc_id));
        //                                command.Parameters.AddWithValue("@size_x_print", item.size_x_print);
        //                                command.Parameters.AddWithValue("@size_y_print", item.size_y_cnc);
        //                                command.Parameters.AddWithValue("@size_x_cut", item.size_x_cut);
        //                                command.Parameters.AddWithValue("@size_y_cut", item.size_y_cut);
        //                                command.Parameters.AddWithValue("@size_x_cnc", item.size_x_cnc);
        //                                command.Parameters.AddWithValue("@size_y_cnc", item.size_y_cnc);
        //                                command.Parameters.AddWithValue("@size_cut", item.size_cut);
        //                                command.Parameters.AddWithValue("@line_size_cut", item.line_size_cut);
        //                                command.Parameters.AddWithValue("@count_size_cut", item.count_size_cut);
        //                                command.Parameters.AddWithValue("@size_cnc", item.size_cnc);
        //                                command.Parameters.AddWithValue("@line_size_cnc", item.line_size_cnc);
        //                                command.Parameters.AddWithValue("@count_size_cnc", item.count_size_cnc);
        //                                command.Parameters.AddWithValue("@count_print", item.count_print);
        //                                command.Parameters.AddWithValue("@count_cut", item.count_cut);
        //                                command.Parameters.AddWithValue("@count_cnc", item.count_cnc);
        //                                command.Parameters.AddWithValue("@square_print", item.square_print);
        //                                command.Parameters.AddWithValue("@square_cut", item.square_cut);
        //                                command.Parameters.AddWithValue("@square_cnc", item.square_cnc);
        //                                command.Parameters.AddWithValue("@cutting_on_print", item.cutting_on_print);
        //                                command.Parameters.AddWithValue("@cnc_on_print", item.cnc_on_print);
        //                                command.Parameters.AddWithValue("@print_on", item.print_on);
        //                                command.Parameters.AddWithValue("@cut_on", item.cut_on);
        //                                command.Parameters.AddWithValue("@cnc_on", item.cnc_on);
        //                                command.Parameters.AddWithValue("@printers_id", item.printers_id);
        //                                command.Parameters.AddWithValue("@cutters_id", item.cutters_id);
        //                                command.Parameters.AddWithValue("@cncs_id", item.cncs_id);
        //                                command.Parameters.AddWithValue("@comments", item.comments);
        //                                command.Parameters.AddWithValue("@laminat", item.laminat);
        //                                command.Parameters.AddWithValue("@laminat_mat", item.laminat_mat);
        //                                command.Parameters.AddWithValue("@installation", item.installation);
        //                                command.Parameters.AddWithValue("@installation_comment", item.installation_comment);
        //                                command.Parameters.AddWithValue("@printerman", item.printerman);
        //                                command.Parameters.AddWithValue("@cutterman", item.cutterman);
        //                                command.Parameters.AddWithValue("@cncman", item.cncman);
        //                                command.Parameters.AddWithValue("@print_quality", item.print_quality);
        //                                command.Parameters.AddWithValue("@state", item.state);
        //                                command.Parameters.AddWithValue("@state_print", item.state_print);
        //                                command.Parameters.AddWithValue("@state_cut", item.state_cut);
        //                                command.Parameters.AddWithValue("@state_cnc", item.state_cnc);
        //                                command.Parameters.AddWithValue("@change_count", Utils.UnixDate.DateTimeToInt64(DateTime.Now));
        //                                command.Parameters.AddWithValue("@sender_row_id", item.sender_row_id);
        //                                command.Parameters.AddWithValue("@sender_row_stringid", item.sender_row_stringid);
        //                                command.Parameters.AddWithValue("@adder", item.adder);
        //                                command.ExecuteNonQuery();

        //                                #region
        //                                //date_start, 
        //                                //dead_line, 
        //                                //date_ready_print, 
        //                                //date_ready_cut, 
        //                                //date_ready_cnc, 
        //                                //client, 
        //                                //work_name, 
        //                                //material_print_id, 
        //                                //material_cut_id, 
        //                                //material_cnc_id, 
        //                                //size_x_print, 
        //                                //size_y_print, 
        //                                //size_x_cut, 
        //                                //size_y_cut, 
        //                                //size_x_cnc, 
        //                                //size_y_cnc, 
        //                                //size_cut, 
        //                                //line_size_cut, 
        //                                //count_size_cut, 
        //                                //size_cnc, 
        //                                //line_size_cnc, 
        //                                //count_size_cnc, 
        //                                //count_print, 
        //                                //count_cut, 
        //                                //count_cnc, 
        //                                //square_print, 
        //                                //square_cut, 
        //                                //square_cnc, 
        //                                //cutting_on_print, 
        //                                //print_on, 
        //                                //cut_on,  
        //                                //cnc_on, 
        //                                //printers_id, 
        //                                //cutters_id, 
        //                                //cncs_id, 
        //                                //comments, 
        //                                //laminat, 
        //                                //laminat_mat, 
        //                                //installation, 
        //                                //installation_comment, 
        //                                //printerman, 
        //                                //cutterman, 
        //                                //cncman, 
        //                                //adder, 
        //                                //print_quality, 
        //                                //status, 
        //                                //state_print, 
        //                                //state_cut, 
        //                                //state_cnc, 
        //                                //date_preview, 
        //                                //path_preview, 
        //                                //path_maket, 
        //                                //change_count, 
        //                                //time_recieve,
        //                                //preview
        //                                //) VALUES(
        //                                //@date_start, 
        //                                //@dead_line, 
        //                                //@date_ready_print, 
        //                                //@date_ready_cut, 
        //                                //@date_ready_cnc, 
        //                                //@client, 
        //                                //@work_name, 
        //                                //@material_print_id, 
        //                                //@material_cut_id, 
        //                                //@material_cnc_id, 
        //                                //@size_x_print, 
        //                                //@size_y_print, 
        //                                //@size_x_cut, 
        //                                //@size_y_cut, 
        //                                //@size_x_cnc, 
        //                                //@size_y_cnc, 
        //                                //@size_cut, 
        //                                //@line_size_cut, 
        //                                //@count_size_cut, 
        //                                //@size_cnc, 
        //                                //@line_size_cnc, 
        //                                //@count_size_cnc, 
        //                                //@count_print, 
        //                                //@count_cut, 
        //                                //@count_cnc, 
        //                                //@square_print, 
        //                                //@square_cut, 
        //                                //@square_cnc, 
        //                                //@cutting_on_print, 
        //                                //@print_on, 
        //                                //@cut_on, 
        //                                //@cnc_on, 
        //                                //@printers_id, 
        //                                //@cutters_id, 
        //                                //@cncs_id, 
        //                                //@comments, 
        //                                //@laminat, 
        //                                //@laminat_mat, 
        //                                //@installation, 
        //                                //@installation_comment, 
        //                                //@printerman, 
        //                                //@cutterman, 
        //                                //@cncman, 
        //                                //@adder, 
        //                                //@print_quality, 
        //                                //@State, 
        //                                //@state_print, 
        //                                //@state_cut, 
        //                                //@state_cnc, 
        //                                //@date_preview, 
        //                                //@path_preview, 
        //                                //@path_maket, 
        //                                //@change_count, 
        //                                //@time_recieve,
        //                                //@preview 
        //                                //); ", connection))
        //                                //                                {
        //                                //                                    command.Transaction = transaction;

        //                                //                                    DataRow row = DataTables.orders.NewRow();

        //                                //                                    command.Parameters.AddWithValue("@date_start", Utils.UnixDate.Int64ToDateTime(item.date_start)); row["date_start"] = Utils.UnixDate.Int64ToDateTime(item.date_start);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@date_ready_print", Utils.UnixDate.Int64ToDateTime(item.date_ready_print)); row["date_ready_print"] = Utils.UnixDate.Int64ToDateTime(item.date_ready_print);
        //                                //                                    command.Parameters.AddWithValue("@date_ready_cut", Utils.UnixDate.Int64ToDateTime(item.date_ready_cut)); row["date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(item.date_ready_cut);
        //                                //                                    command.Parameters.AddWithValue("@date_ready_cnc", Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc)); row["date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(item.date_ready_cnc);
        //                                //                                    command.Parameters.AddWithValue("@client", Utils.CheckDBNull.ToString(item.client)); row["client"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);
        //                                //                                    command.Parameters.AddWithValue("@dead_line", Utils.UnixDate.Int64ToDateTime(item.dead_line)); row["dead_line"] = Utils.UnixDate.Int64ToDateTime(item.dead_line);







        //                                //                                    //command.Parameters.Add("@id", DbType.Int32).Value = row["id"] = item.id;
        //                                //                                    //command.Parameters.Add("@date_start", DbType.Int64).Value = row["date_start"] = item.date_start;
        //                                //                                    //command.Parameters.Add("@dead_line", DbType.Int64).Value = row["dead_line"] = item.dead_line;
        //                                //                                    //command.Parameters.Add("@date_ready_print", DbType.Int64).Value = row["date_ready_print"] = item.date_ready_print;
        //                                //                                    //command.Parameters.Add("@date_ready_cut", DbType.Int64).Value = row["date_ready_cut"] = item.date_ready_cut;
        //                                //                                    //command.Parameters.Add("@date_ready_cnc", DbType.Int64).Value = row["date_ready_cnc"] = item.date_ready_cnc;
        //                                //                                    command.Parameters.Add("@client", DbType.String).Value = row["client"] = item.client;
        //                                //                                    command.Parameters.Add("@work_name", DbType.String).Value = row["work_name"] = item.work_name;
        //                                //                                    command.Parameters.Add("@material_print_id", DbType.Int32).Value = row["material_print_id"] = item.material_print_id;
        //                                //                                    command.Parameters.Add("@material_cut_id", DbType.Int32).Value = row["material_cut_id"] = item.material_cut_id;
        //                                //                                    command.Parameters.Add("@material_cnc_id", DbType.Int32).Value = row["material_cnc_id"] = item.material_cnc_id;
        //                                //                                    command.Parameters.Add("@size_x_print", DbType.Double).Value = row["size_x_print"] = item.size_x_print;
        //                                //                                    command.Parameters.Add("@size_y_print", DbType.Double).Value = row["size_y_print"] = item.size_y_print;
        //                                //                                    command.Parameters.Add("@size_x_cut", DbType.Double).Value = row["size_x_cut"] = item.size_x_cut;
        //                                //                                    command.Parameters.Add("@size_y_cut", DbType.Double).Value = row["size_y_cut"] = item.size_y_cut;
        //                                //                                    command.Parameters.Add("@size_x_cnc", DbType.Double).Value = row["size_x_cnc"] = item.size_x_cnc;
        //                                //                                    command.Parameters.Add("@size_y_cnc", DbType.Double).Value = row["size_y_cnc"] = item.size_y_cnc;
        //                                //                                    command.Parameters.Add("@size_cut", DbType.Double).Value = row["size_cut"] = item.size_cut;
        //                                //                                    command.Parameters.Add("@line_size_cut", DbType.Double).Value = row["line_size_cut"] = item.line_size_cut;
        //                                //                                    command.Parameters.Add("@count_size_cut", DbType.Int32).Value = row["count_size_cut"] = item.count_size_cut;
        //                                //                                    command.Parameters.Add("@size_cnc", DbType.Double).Value = row["size_cnc"] = item.size_cnc;
        //                                //                                    command.Parameters.Add("@line_size_cnc", DbType.Double).Value = row["line_size_cnc"] = item.line_size_cnc;
        //                                //                                    command.Parameters.Add("@count_size_cnc", DbType.Int32).Value = row["count_size_cnc"] = item.count_size_cnc;
        //                                //                                    command.Parameters.Add("@count_print", DbType.Int32).Value = row["count_print"] = item.count_print;
        //                                //                                    command.Parameters.Add("@count_cut", DbType.Int32).Value = row["count_cut"] = item.count_cut;
        //                                //                                    command.Parameters.Add("@count_cnc", DbType.Int32).Value = row["count_cnc"] = item.count_cnc;
        //                                //                                    command.Parameters.Add("@square_print", DbType.Double).Value = row["square_print"] = item.square_print;
        //                                //                                    command.Parameters.Add("@square_cut", DbType.Double).Value = row["square_cut"] = item.square_cut;
        //                                //                                    command.Parameters.Add("@square_cnc", DbType.Double).Value = row["square_cnc"] = item.square_cnc;
        //                                //                                    command.Parameters.Add("@cutting_on_print", DbType.Int32).Value = row["cutting_on_print"] = Convert.ToInt32(item.cutting_on_print);
        //                                //                                    command.Parameters.Add("@print_on", DbType.Int32).Value = row["print_on"] = Convert.ToInt16(item.print_on);
        //                                //                                    command.Parameters.Add("@cut_on", DbType.Int32).Value = row["cut_on"] = Convert.ToInt16(item.cut_on);
        //                                //                                    command.Parameters.Add("@cnc_on", DbType.Int32).Value = row["cnc_on"] = Convert.ToInt16(item.cnc_on);
        //                                //                                    command.Parameters.Add("@printers_id", DbType.Int32).Value = row["printers_id"] = item.printers_id;
        //                                //                                    command.Parameters.Add("@cutters_id", DbType.Int32).Value = row["cutters_id"] = item.cutters_id;
        //                                //                                    command.Parameters.Add("@cncs_id", DbType.Int32).Value = row["cncs_id"] = item.cncs_id;
        //                                //                                    command.Parameters.Add("@comments", DbType.String).Value = row["comments"] = item.comments;
        //                                //                                    command.Parameters.Add("@laminat", DbType.Int32).Value = row["laminat"] = Convert.ToInt16(item.laminat);
        //                                //                                    command.Parameters.Add("@laminat_mat", DbType.Int32).Value = row["laminat_mat"] = Convert.ToInt16(item.laminat_mat);
        //                                //                                    command.Parameters.Add("@installation", DbType.Int32).Value = row["installation"] = Convert.ToInt16(item.installation);
        //                                //                                    command.Parameters.Add("@installation_comment", DbType.String).Value = row["installation_comment"] = item.installation_comment;
        //                                //                                    command.Parameters.Add("@printerman", DbType.String).Value = row["printerman"] = item.printerman;
        //                                //                                    command.Parameters.Add("@cutterman", DbType.String).Value = row["cutterman"] = item.cutterman;
        //                                //                                    command.Parameters.Add("@cncman", DbType.String).Value = row["cncman"] = item.cncman;
        //                                //                                    command.Parameters.Add("@adder", DbType.String).Value = row["adder"] = item.adder;
        //                                //                                    command.Parameters.Add("@print_quality", DbType.String).Value = row["print_quality"] = item.print_quality;
        //                                //                                    command.Parameters.Add("@State", DbType.Int32).Value = row["status"] = item.state;
        //                                //                                    command.Parameters.Add("@state_print", DbType.Int32).Value = row["state_print"] = Convert.ToInt16(item.state_print);
        //                                //                                    command.Parameters.Add("@state_cut", DbType.Int32).Value = row["state_cut"] = Convert.ToInt16(item.state_cut);
        //                                //                                    command.Parameters.Add("@state_cnc", DbType.Int32).Value = row["state_cnc"] = Convert.ToInt16(item.state_cnc);
        //                                //                                    command.Parameters.Add("@date_preview", DbType.Int64).Value = row["date_preview"] = item.date_preview;
        //                                //                                    command.Parameters.Add("@path_preview", DbType.String).Value = row["path_preview"] = item.path_preview;
        //                                //                                    command.Parameters.Add("@path_maket", DbType.String).Value = row["path_maket"] = item.path_maket;
        //                                //                                    command.Parameters.Add("@change_count", DbType.Int64).Value = row["change_count"] = item.change_count;
        //                                //                                    command.Parameters.Add("@time_recieve", DbType.Int64).Value = row["time_recieve"] = item.time_recieve;
        //                                //                                    if (item.preview != null)
        //                                //                                    {
        //                                //                                        command.Parameters.Add("@preview", DbType.Object, item.preview.Length).Value = row["preview"] = item.preview;
        //                                //                                    }
        //                                //                                    else { command.Parameters.Add("@preview", DbType.Object).Value = row["preview"] = null; }
        //                                //                                    command.ExecuteNonQuery();

        //                                //                                    row["String_id"] = Convert.ToString(row["id"]);
        //                                //                                    row["Datetime_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_start"]));
        //                                //                                    row["Datetime_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["dead_line"]));
        //                                //                                    row["String_date_start"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_start"])).ToString("dd.MM.yyyy HH:mm");
        //                                //                                    row["String_dead_line"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["dead_line"])).ToString("dd.MM.yyyy HH:mm");
        //                                //                                    row["Datetime_date_ready_print"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_print"]));
        //                                //                                    row["Datetime_date_ready_cut"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_cut"]));
        //                                //                                    row["Datetime_date_ready_cnc"] = Utils.UnixDate.Int64ToDateTime(Convert.ToInt64(row["date_ready_cnc"]));
        //                                //                                    //StringBuilder sb = new StringBuilder();
        //                                //                                    //if (Convert.ToBoolean(row["print_on"])) { sb.Append("• Печать"); }
        //                                //                                    //if (Convert.ToBoolean(row["cut_on"]))
        //                                //                                    //{
        //                                //                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
        //                                //                                    //    sb.Append("• Плот.резка");
        //                                //                                    //}
        //                                //                                    //if (Convert.ToBoolean(row["cnc_on"]))
        //                                //                                    //{
        //                                //                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
        //                                //                                    //    sb.Append("• Фрезеровка");
        //                                //                                    //}
        //                                //                                    //if (Convert.ToBoolean(row["installation"]))
        //                                //                                    //{
        //                                //                                    //    if (sb.Length > 0) { sb.Append(Environment.NewLine); }
        //                                //                                    //    sb.Append("• Монтаж");
        //                                //                                    //}
        //                                //                                    //row["String_WorkTypes"] = sb.ToString();
        //                                //                                    int print = 0;
        //                                //                                    int cut = 0;
        //                                //                                    int cnc = 0;
        //                                //                                    int install = 0;
        //                                //                                    int status = Convert.ToInt32(row["status"]);
        //                                //                                    if (Convert.ToBoolean(row["print_on"])) { if (Convert.ToBoolean(row["state_print"]) || status == 3) { print = 2; } else { print = 1; } }
        //                                //                                    if (Convert.ToBoolean(row["cut_on"])) { if (Convert.ToBoolean(row["state_cut"]) || status == 3) { cut = 2; } else { cut = 1; } }
        //                                //                                    if (Convert.ToBoolean(row["cnc_on"])) { if (Convert.ToBoolean(row["state_cnc"]) || status == 3) { cnc = 2; } else { cnc = 1; } }
        //                                //                                    if (Convert.ToBoolean(row["installation"])) { if (status == 3) { install = 2; } else { install = 1; } }
        //                                //                                    row["Image_WorkTypes"] = Utils.WorkStatusIcon.GenerateImages(print, cut, cnc, install);
        //                                //                                    TableOrders.Rows.Add(row);
        //                                //                                    //if (item.contactsList != null && item.contactsList.Count > 0)
        //                                //                                    //{
        //                                //                                    //    UPDATE_SqliteCustomerContacts(item.contactsList, item.id, connection);
        //                                //                                    //}
        //                                //                                }
        //                                #endregion
        //                            }
        //                            #endregion
        //                        }
        //                        transaction.Commit();
        //                    }
        //                    connection.Close();
        //                }
        //            }
        //        }
        //    catch (Exception ex)
        //    {
        //        //return ex;
        //    }
        //}
        #endregion

    }
}