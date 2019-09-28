using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentServer.ProtoClasses
{
    public class ProtoDownloadOrdersFiles
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
            public string name { get; set; }
        }

        [ProtoContract]
        public class protoRow
        {
            [ProtoMember(1)]
            public long uid { get; set; }
            [ProtoMember(2)]
            public string LocalPath { get; set; }
            [ProtoMember(3)]
            public List<ProtoClasses.ProtoFtpSchedule.protoRow> Preview { get; set; }
            [ProtoMember(4)]
            public List<ProtoClasses.ProtoFtpSchedule.protoRow> Makets { get; set; }
            [ProtoMember(5)]
            public List<ProtoClasses.ProtoFtpSchedule.protoRow> Documents { get; set; }
            [ProtoMember(6)]
            public List<ProtoClasses.ProtoFtpSchedule.protoRow> Photoreport { get; set; }
        }
        public byte[] protoSerialize(protoRowsList items)
        {
            var item = new protoRowsList
            {
                plist = new List<protoRow>(items.plist),
                name = items.name,
                sender_row_id = items.sender_row_id,
                sender_row_stringid = items.sender_row_stringid
            };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<protoRowsList>(stream, item, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch {
                return null; }
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
