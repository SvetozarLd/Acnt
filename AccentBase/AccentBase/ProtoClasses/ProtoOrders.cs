using System;
using System.Collections.Generic;
using System.Data;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    public class ProtoOrders
    {
        [ProtoContract]
        public class protoOrdersList
        {
            [ProtoMember(1)]
            public List<protoOrder> plist { get; set; }
        }
        [ProtoContract]
        public class protoOrder
        {
            [ProtoMember(1)]
            public int id { get; set; }
            [ProtoMember(2)]
            public long date_start { get; set; }
            [ProtoMember(3)]
            public long dead_line { get; set; }
            [ProtoMember(4)]
            public long date_ready_print { get; set; }
            [ProtoMember(5)]
            public long date_ready_cut { get; set; }
            [ProtoMember(6)]
            public long date_ready_cnc { get; set; }
            [ProtoMember(7)]
            public string client { get; set; }
            [ProtoMember(8)]
            public string work_name { get; set; }
            [ProtoMember(9)]
            public int material_print_id { get; set; }
            [ProtoMember(10)]
            public int material_cut_id { get; set; }
            [ProtoMember(11)]
            public int material_cnc_id { get; set; }
            [ProtoMember(12)]
            public double size_x_print { get; set; }
            [ProtoMember(13)]
            public double size_y_print { get; set; }
            [ProtoMember(14)]
            public double size_x_cut { get; set; }
            [ProtoMember(15)]
            public double size_y_cut { get; set; }
            [ProtoMember(16)]
            public double size_x_cnc { get; set; }
            [ProtoMember(17)]
            public double size_y_cnc { get; set; }
            [ProtoMember(18)]
            public double size_cut { get; set; }
            [ProtoMember(19)]
            public double line_size_cut { get; set; }
            [ProtoMember(20)]
            public int count_size_cut { get; set; }
            [ProtoMember(21)]
            public double size_cnc { get; set; }
            [ProtoMember(22)]
            public double line_size_cnc { get; set; }
            [ProtoMember(23)]
            public int count_size_cnc { get; set; }
            [ProtoMember(24)]
            public int count_print { get; set; }
            [ProtoMember(25)]
            public int count_cut { get; set; }
            [ProtoMember(26)]
            public int count_cnc { get; set; }
            [ProtoMember(27)]
            public double square_print { get; set; }
            [ProtoMember(28)]
            public double square_cut { get; set; }
            [ProtoMember(29)]
            public double square_cnc { get; set; }
            [ProtoMember(30)]
            public bool cutting_on_print { get; set; }
            [ProtoMember(31)]
            public bool cnc_on_print { get; set; }
            [ProtoMember(32)]
            public bool print_on { get; set; }
            [ProtoMember(33)]
            public bool cut_on { get; set; }
            [ProtoMember(34)]
            public bool cnc_on { get; set; }
            [ProtoMember(35)]
            public int printers_id { get; set; }
            [ProtoMember(36)]
            public int cutters_id { get; set; }
            [ProtoMember(37)]
            public int cncs_id { get; set; }
            [ProtoMember(38)]
            public string comments { get; set; }
            [ProtoMember(39)]
            public bool laminat { get; set; }
            [ProtoMember(40)]
            public bool laminat_mat { get; set; }
            [ProtoMember(41)]
            public bool installation { get; set; }
            [ProtoMember(42)]
            public byte[] installation_comment { get; set; }
            [ProtoMember(43)]
            public string printerman { get; set; }
            [ProtoMember(44)]
            public string cutterman { get; set; }
            [ProtoMember(45)]
            public string cncman { get; set; }
            [ProtoMember(46)]
            public string adder { get; set; }
            [ProtoMember(47)]
            public string print_quality { get; set; }
            [ProtoMember(48)]
            public int state { get; set; }
            [ProtoMember(49)]
            public bool state_print { get; set; }
            [ProtoMember(50)]
            public bool state_cut { get; set; }
            [ProtoMember(51)]
            public bool state_cnc { get; set; }
            [ProtoMember(52)]
            public bool state_install { get; set; }
            [ProtoMember(53)]
            public long date_preview { get; set; }
            [ProtoMember(54)]
            public string path_preview { get; set; }
            [ProtoMember(55)]
            public string path_maket { get; set; }
            [ProtoMember(56)]
            public long change_count { get; set; }
            [ProtoMember(57)]
            public int command { get; set; }
            [ProtoMember(58)]
            public long time_recieve { get; set; }
            [ProtoMember(59)]
            public int old_stock { get; set; }
            [ProtoMember(60)]
            public byte[] preview { get; set; }
            [ProtoMember(61)]
            public long sender_row_id { get; set; }
            [ProtoMember(62)]
            public string sender_row_stringid { get; set; }
            [ProtoMember(63)]
            public string worktypes_list { get; set; }
            [ProtoMember(64)]
            public bool delivery { get; set; }
            [ProtoMember(65)]
            public bool delivery_office { get; set; }
            [ProtoMember(66)]
            public string delivery_address { get; set; }
            [ProtoMember(67)]
            public bool baner_handling { get; set; }
            [ProtoMember(68)]
            public bool baner_luvers { get; set; }
            [ProtoMember(69)]
            public double baner_handling_size { get; set; }
            [ProtoMember(70)]
            public List<ProtoOrderHistory.protoRow> HistoryRows { get; set; }
            [ProtoMember(71)]
            public List<ProtoFtpSchedule.protoRow> FilesUpload { get; set; }
            [ProtoMember(72)]
            public List<ProtoFiles.protoRow> DeleteFilesList { get; set; }
        }

        public byte[] protoSerialize(List<protoOrder> items)
        {
            var item = new protoOrdersList { plist = new List<protoOrder>(items) };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<List<protoOrder>>(stream, items, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public List<protoOrder> protoDeserialize(byte[] message)
        {
            List<protoOrder> item = new List<protoOrder>();
            using (var stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<List<protoOrder>>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
