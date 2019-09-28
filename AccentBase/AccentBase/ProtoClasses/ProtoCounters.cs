using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    public class ProtoCounters
    {
        [ProtoContract]
        public class protoList
        {
            [ProtoMember(1)]
            public Dictionary<int, long> plist { get; set; }
        }

        public byte[] protoSerialize(Dictionary<int, long> items)
        {
            var item = new protoList { plist = new Dictionary<int, long>(items) };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<Dictionary<int, long>>(stream, items, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoList protoDeserialize(byte[] message)
        {
            protoList item = new protoList();
            using (var stream = new MemoryStream(message))
            {
                try { item.plist = Serializer.DeserializeWithLengthPrefix<Dictionary<int, long>>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
