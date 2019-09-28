using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentBase.Utils
{
    internal static class DataRowToProto
    {
        internal static ProtoClasses.ProtoOrders.protoOrder OrderToProto(DataRow rdr)
        {
            try
            {
                ProtoClasses.ProtoOrders.protoOrder po = new ProtoClasses.ProtoOrders.protoOrder();
                po.id = Convert.ToInt32(rdr["id"]);
                po.date_start = Convert.ToInt64(rdr["date_start"]);
                po.dead_line = Convert.ToInt64(rdr["dead_line"]);
                po.date_ready_print = Convert.ToInt64(rdr["date_ready_print"]);
                po.date_ready_cut = Convert.ToInt64(rdr["date_ready_cut"]);
                po.date_ready_cnc = Convert.ToInt64(rdr["date_ready_cnc"]);
                po.client = Utils.CheckDBNull.ToString(rdr["client"]);
                po.work_name = Utils.CheckDBNull.ToString(rdr["work_name"]);
                po.material_print_id = Utils.CheckDBNull.ToInt32(rdr["material_print_id"]);
                po.material_cut_id = Utils.CheckDBNull.ToInt32(rdr["material_cut_id"]);
                po.material_cnc_id = Utils.CheckDBNull.ToInt32(rdr["material_cnc_id"]);
                po.size_x_print = Utils.CheckDBNull.ToDouble(rdr["size_x_print"]);
                po.size_y_print = Utils.CheckDBNull.ToDouble(rdr["size_y_print"]);
                po.size_x_cut = Utils.CheckDBNull.ToDouble(rdr["size_x_cut"]);
                po.size_y_cut = Utils.CheckDBNull.ToDouble(rdr["size_y_cut"]);
                po.size_x_cnc = Utils.CheckDBNull.ToDouble(rdr["size_x_cnc"]);
                po.size_y_cnc = Utils.CheckDBNull.ToDouble(rdr["size_y_cnc"]);
                po.size_cut = Utils.CheckDBNull.ToDouble(rdr["size_cut"]);
                po.line_size_cut = Utils.CheckDBNull.ToDouble(rdr["line_size_cut"]);
                po.count_size_cut = Utils.CheckDBNull.ToInt32(rdr["count_size_cut"]);
                po.size_cnc = Utils.CheckDBNull.ToDouble(rdr["size_cnc"]);
                po.line_size_cnc = Utils.CheckDBNull.ToDouble(rdr["line_size_cnc"]);
                po.count_size_cnc = Utils.CheckDBNull.ToInt32(rdr["count_size_cnc"]);
                po.count_print = Utils.CheckDBNull.ToInt32(rdr["count_print"]);
                po.count_cut = Utils.CheckDBNull.ToInt32(rdr["count_cut"]);
                po.count_cnc = Utils.CheckDBNull.ToInt32(rdr["count_cnc"]);
                po.square_print = Utils.CheckDBNull.ToDouble(rdr["square_print"]);
                po.square_cut = Utils.CheckDBNull.ToDouble(rdr["square_cut"]);
                po.square_cnc = Utils.CheckDBNull.ToDouble(rdr["square_cnc"]);
                po.cutting_on_print = Utils.CheckDBNull.ToBoolean(rdr["cutting_on_print"]);
                po.cnc_on_print = Utils.CheckDBNull.ToBoolean(rdr["cnc_on_print"]);
                po.print_on = Utils.CheckDBNull.ToBoolean(rdr["print_on"]);
                po.cut_on = Utils.CheckDBNull.ToBoolean(rdr["cut_on"]);
                po.cnc_on = Utils.CheckDBNull.ToBoolean(rdr["cnc_on"]);
                po.printers_id = Utils.CheckDBNull.ToInt32(rdr["printers_id"]);
                po.cutters_id = Utils.CheckDBNull.ToInt32(rdr["cutters_id"]);
                po.cncs_id = Utils.CheckDBNull.ToInt32(rdr["cncs_id"]);
                po.comments = Utils.CheckDBNull.ToString(rdr["comments"]);
                po.laminat = Utils.CheckDBNull.ToBoolean(rdr["laminat"]);
                po.laminat_mat = Utils.CheckDBNull.ToBoolean(rdr["laminat_mat"]);
                po.installation = Utils.CheckDBNull.ToBoolean(rdr["installation"]);
                //po.installation_comment = Utils.CheckDBNull.ToString(rdr["installation_comment"]);
                po.printerman = Utils.CheckDBNull.ToString(rdr["printerman"]);
                po.cutterman = Utils.CheckDBNull.ToString(rdr["cutterman"]);
                po.cncman = Utils.CheckDBNull.ToString(rdr["cncman"]);
                po.adder = Utils.CheckDBNull.ToString(rdr["adder"]);
                po.print_quality = Utils.CheckDBNull.ToString(rdr["print_quality"]);
                po.state = Utils.CheckDBNull.ToInt32(rdr["status"]);
                po.state_print = Utils.CheckDBNull.ToBoolean(rdr["state_print"]);
                po.state_cut = Utils.CheckDBNull.ToBoolean(rdr["state_cut"]);
                po.state_cnc = Utils.CheckDBNull.ToBoolean(rdr["state_cnc"]);
                po.state_install = Utils.CheckDBNull.ToBoolean(rdr["state_install"]);
                po.date_preview = Utils.UnixDate.CheckedDateTimeToInt64(rdr["date_preview"]);
                po.path_preview = Utils.CheckDBNull.ToString(rdr["path_preview"]);
                po.path_maket = Utils.CheckDBNull.ToString(rdr["path_maket"]);
                po.change_count = Convert.ToInt64((rdr["change_count"]));
                po.time_recieve = Convert.ToInt64((rdr["time_recieve"]));
                //po.sender_row_id = Convert.ToInt64((rdr["sender_row_id"]));
                //po.sender_row_stringid = Convert.ToString((rdr["sender_row_stringid"]));
                po.worktypes_list = Utils.CheckDBNull.ToString(rdr["worktypes_list"]);
                po.delivery = Utils.CheckDBNull.ToBoolean(rdr["delivery"]);
                po.delivery_office = Utils.CheckDBNull.ToBoolean(rdr["delivery_office"]);
                po.delivery_address = Utils.CheckDBNull.ToString(rdr["delivery_address"]);
                po.baner_handling = Utils.CheckDBNull.ToBoolean(rdr["baner_handling"]);
                po.baner_luvers = Utils.CheckDBNull.ToBoolean(rdr["baner_luvers"]);
                po.baner_handling_size = Utils.CheckDBNull.ToDouble(rdr["baner_handling_size"]);
                return po;
            }
            catch { return null; }
        }


    }
}
