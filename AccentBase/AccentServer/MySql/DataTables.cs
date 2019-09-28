using System.Data;
using System.Collections.Concurrent;


namespace AccentServer.MySql
{
    static class DataTables
    {

        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoOrders.protoOrder> orders { get; set; }

        //static internal DataTable orders { get; set; }
        //static internal DataTable preview { get; set; }
        static public ConcurrentDictionary<string, AccentServer.ProtoClasses.ProtoPreview.protoRow> preview { get; set; }
        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoMaterial.protoRow> oldstock_material_print { get; set; }
        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoMaterial.protoRow> oldstock_material_cut { get; set; }
        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoMaterial.protoRow> oldstock_material_cnc { get; set; }
        //static internal DataTable oldstock_material_print { get; set; }
        //static internal DataTable oldstock_material_cut { get; set; }
        //static internal DataTable oldstock_material_cnc { get; set; }
        //static public DataTable oldstock_printers { get; set; }
        //static public DataTable oldstock_cutters { get; set; }
        //static public DataTable oldstock_cncs { get; set; }

        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow> oldstock_printers { get; set; }
        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow> oldstock_cutters { get; set; }
        static public ConcurrentDictionary<int, AccentServer.ProtoClasses.ProtoEquip.protoRow> oldstock_cncs { get; set; }
        //static public DataTable stock { get; set; }
        //static public DataTable stockfolder { get; set; }
        //static internal DataTable old_order_history { get; set; }
        static public ConcurrentDictionary<long, AccentServer.ProtoClasses.ProtoOrderHistory.protoRow> old_order_history { get; set; }
        //static internal DataTable files { get; set; }
        //static public ConcurrentDictionary<string, AccentServer.ProtoClasses.ProtoFiles.protoRow> files { get; set; }
        //static DataTables()
        //{
        //    preview = new ConcurrentDictionary<string, AccentServer.ProtoClasses.ProtoPreview.protoRow>();
        //    //    preview = new DataTable();
        //    //    TableOrders.Columns.Add("String_id", typeof(string));
        //    //    TableOrders.Columns.Add("Datetime_date_start", typeof(DateTime));
        //    //    TableOrders.Columns.Add("Datetime_dead_line", typeof(DateTime));
        //    //    TableOrders.Columns.Add("String_date_start", typeof(string));
        //    //    TableOrders.Columns.Add("String_dead_line", typeof(string));
        //    //    TableOrders.Columns.Add("Datetime_date_ready_print", typeof(DateTime));
        //    //    TableOrders.Columns.Add("Datetime_date_ready_cut", typeof(DateTime));
        //    //    TableOrders.Columns.Add("Datetime_date_ready_cnc", typeof(DateTime));
        //}
    }
}
