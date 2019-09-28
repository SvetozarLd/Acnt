using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    public class ProtoSettings
    {
        [ProtoContract]
        public class protoSet
        {
            [ProtoMember(1)]
            public string name { get; set; }
            [ProtoMember(2)]
            public string server_address { get; set; }
            [ProtoMember(3)]
            public int server_port { get; set; }
            [ProtoMember(4)]
            public string data_path { get; set; }
            [ProtoMember(5)]
            public int buffer_size { get; set; }
            [ProtoMember(6)]
            public string SavePath { get; set; }
            [ProtoMember(7)]
            public bool View_ToolStrip { get; set; }
            [ProtoMember(8)]
            public bool View_OrderPreview { get; set; }
            [ProtoMember(9)]
            public bool View_OrderNotes { get; set; }
            [ProtoMember(10)]
            public bool View_OrderHistory { get; set; }
            [ProtoMember(11)]
            public bool View_QuickSearch { get; set; }
            [ProtoMember(12)]
            public bool View_OrderListInfo { get; set; }
            [ProtoMember(13)]
            public bool View_Worktypes { get; set; }
            [ProtoMember(14)]
            public bool View_OrderTimeStart { get; set; }
            [ProtoMember(15)]
            public bool View_OrderTimeEnd { get; set; }
            [ProtoMember(16)]
            public bool View_OrderWorkTypes { get; set; }
            [ProtoMember(17)]
            public bool View_OrderMaterial { get; set; }
            [ProtoMember(18)]
            public bool View_OrderManager { get; set; }
            [ProtoMember(19)]
            public bool View_OrderCustomer { get; set; }
            [ProtoMember(20)]
            public bool View_Maximize { get; set; }
            [ProtoMember(21)]
            public bool View_SocketSend { get; set; }
            [ProtoMember(22)]
            public bool View_OrdersAll { get; set; }
            [ProtoMember(23)]
            public bool View_OrderPrint { get; set; }
            [ProtoMember(24)]
            public bool View_OrderCut { get; set; }
            [ProtoMember(25)]
            public bool View_OrdersCnc { get; set; }
            [ProtoMember(26)]
            public bool View_OrdersInstall { get; set; }
            [ProtoMember(27)]
            public string FtpLogin { get; set; }
            [ProtoMember(28)]
            public string FtpPass { get; set; }

        }

        public byte[] Serialize(protoSet items)
        {
            protoSet item = new protoSet
            {
                name = items.name,
                server_address = items.server_address,
                server_port = items.server_port,
                data_path = items.data_path,
                buffer_size = items.buffer_size,
                SavePath = items.SavePath,
                View_ToolStrip = items.View_ToolStrip,
                View_OrderPreview = items.View_OrderPreview,
                View_OrderNotes = items.View_OrderNotes,
                View_OrderHistory = items.View_OrderHistory,
                View_QuickSearch = items.View_QuickSearch,
                View_OrderListInfo = items.View_OrderListInfo,
                View_Worktypes = items.View_Worktypes,
                View_OrderTimeStart = items.View_OrderTimeStart,
                View_OrderTimeEnd = items.View_OrderTimeEnd,
                View_OrderWorkTypes = items.View_OrderWorkTypes,
                View_OrderMaterial = items.View_OrderMaterial,
                View_OrderManager = items.View_OrderManager,
                View_OrderCustomer = items.View_OrderCustomer,
                View_Maximize = items.View_Maximize,
                View_SocketSend = items.View_SocketSend,
                View_OrdersAll = items.View_OrdersAll,
                View_OrderPrint = items.View_OrderPrint,
                View_OrderCut = items.View_OrderCut,
                View_OrdersCnc = items.View_OrdersCnc,
                View_OrdersInstall = items.View_OrdersInstall,
                FtpLogin = items.FtpLogin,
                FtpPass = items.FtpPass
            };
            byte[] result = null;
            try
            {
                using (MemoryStream stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<protoSet>(stream, item, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoSet Deserialize(byte[] message)
        {
            protoSet item = new protoSet();
            using (MemoryStream stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<protoSet>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
