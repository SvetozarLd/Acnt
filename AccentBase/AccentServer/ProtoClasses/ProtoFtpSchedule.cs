using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentServer.ProtoClasses
{
    public class ProtoFtpSchedule
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
            public string sourcefile { get; set; }
            [ProtoMember(3)]
            public string targetfile { get; set; }
            [ProtoMember(4)]
            public long Length { get; set; }
            [ProtoMember(5)]
            public long LastWriteTime { get; set; }
            [ProtoMember(6)]
            public long LastCreationTime { get; set; }
            [ProtoMember(7)]
            public bool Upload { get; set; }
            [ProtoMember(8)]
            public int filestatus { get; set; }
            [ProtoMember(9)]
            public string notes { get; set; }
            [ProtoMember(10)]
            public int processprogress { get; set; }
            [ProtoMember(11)]
            public string fileshortname { get; set; }
            [ProtoMember(12)]
            public string serveraddress { get; set; }
            [ProtoMember(13)]
            public string conspeed { get; set; }
            [ProtoMember(14)]
            public long order_id { get; set; }
            [ProtoMember(15)]
            public string LengthString { get; set; }
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
