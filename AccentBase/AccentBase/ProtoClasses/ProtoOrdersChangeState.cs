using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    class ProtoOrdersChangeState
    {
        [ProtoContract]
        public class protoRowsList
        {
            [ProtoMember(1)]
            public List<protoRow> plist { get; set; }
            [ProtoMember(2)]
            public long sender_row_id { get; set; }
            [ProtoMember(3)]
            public string sender_row_stringid { get; set; }
            [ProtoMember(4)]
            public string adder { get; set; }
            [ProtoMember(5)]
            public string name { get; set; }
            [ProtoMember(6)]
            public string notes { get; set; }
            [ProtoMember(7)]
            public List<ProtoOrderHistory.protoRow> HistoryRows { get; set; }
        }
        [ProtoContract]
        public class protoRow
        {
            [ProtoMember(1)]
            public long id { get; set; }
            [ProtoMember(2)]
            public string adder { get; set; }
            [ProtoMember(3)]
            public int state { get; set; }
            [ProtoMember(4)]
            public bool state_print { get; set; }
            [ProtoMember(5)]
            public bool state_cut { get; set; }
            [ProtoMember(6)]
            public bool state_cnc { get; set; }
            [ProtoMember(7)]
            public bool state_install { get; set; }
            [ProtoMember(8)]
            public long change_count { get; set; }
            [ProtoMember(9)]
            public string printerman { get; set; }
            [ProtoMember(10)]
            public string cutterman { get; set; }
            [ProtoMember(11)]
            public string cncman { get; set; }
            [ProtoMember(12)]
            public long date_ready_print { get; set; }
            [ProtoMember(13)]
            public long date_ready_cut { get; set; }
            [ProtoMember(14)]
            public long date_ready_cnc { get; set; }
            [ProtoMember(15)]
            public byte[] Image_WorkTypes { get; set; }            
        }

        public byte[] protoSerialize(protoRowsList items)
        {
            var item = new protoRowsList {
                plist = new List<protoRow>(items.plist),
                sender_row_id = items.sender_row_id,
                sender_row_stringid = items.sender_row_stringid,
                adder = items.adder,
                name = items.name,
                notes = items.notes
            };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<protoRowsList>(stream, item, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoRowsList protoDeserialize(byte[] message)
        {
            protoRowsList item = new protoRowsList();
            using (var stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<protoRowsList>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
