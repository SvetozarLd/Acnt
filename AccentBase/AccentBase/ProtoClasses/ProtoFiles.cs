using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    public class ProtoFiles
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
            public int id { get; set; }
            [ProtoMember(2)]
            public string name { get; set; }
            [ProtoMember(3)]
            public string fullname { get; set; }
            [ProtoMember(4)]
            public long CreationTime { get; set; }
            [ProtoMember(5)]
            public long LastAccessTime { get; set; }
            [ProtoMember(6)]
            public long LastWriteTime { get; set; }
            [ProtoMember(7)]
            public string Extension { get; set; }
            [ProtoMember(8)]
            public long Length { get; set; }
            [ProtoMember(9)]
            public int order_id { get; set; }
            [ProtoMember(10)]
            public int folder_flag { get; set; }
            [ProtoMember(11)]
            public string foldername { get; set; }
            [ProtoMember(12)]
            public string folderfullname { get; set; }
            [ProtoMember(13)]
            public int folder_id { get; set; }
            [ProtoMember(14)]
            public long change_count { get; set; }
            [ProtoMember(15)]
            public int command { get; set; }
            [ProtoMember(16)]
            public long time_recieve { get; set; }
            [ProtoMember(17)]
            public int type { get; set; }
            [ProtoMember(18)]
            public string status { get; set; }
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
