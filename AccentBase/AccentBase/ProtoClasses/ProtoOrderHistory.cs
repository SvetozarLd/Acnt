using System.Collections.Generic;
using ProtoBuf;
using System.IO;


namespace AccentBase.ProtoClasses
{
    public class ProtoOrderHistory
    {
        [ProtoContract]
        public class protoRowsList
        {
            [ProtoMember(1)]
            public List<protoRow> plist { get; set; }
        }
        [ProtoContract]
        public class protoRow
        {
            [ProtoMember(1)]
            public long id { get; set; }
            [ProtoMember(2)]
            public long work_id { get; set; }
            [ProtoMember(3)]
            public string note { get; set; }
            [ProtoMember(4)]
            public int status_task { get; set; }
            [ProtoMember(5)]
            public long date_change { get; set; }
            [ProtoMember(6)]
            public string adder { get; set; }
            [ProtoMember(7)]
            public long change_count { get; set; }
            [ProtoMember(8)]
            public int command { get; set; }
            [ProtoMember(9)]
            public long time_recieve { get; set; }
        }

        public byte[] protoSerialize(List<protoRow> items)
        {
            var item = new protoRowsList { plist = new List<protoRow>(items) };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<List<protoRow>>(stream, items, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public List<protoRow> protoDeserialize(byte[] message)
        {
            List<protoRow> item = new List<protoRow>();
            using (var stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<List<protoRow>>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
