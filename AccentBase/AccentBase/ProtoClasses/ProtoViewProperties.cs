using System;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
{
    class ProtoViewProperties
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
        }

        public byte[] Serialize(protoSet items)
        {
            var item = new protoSet { name = items.name, server_address = items.server_address, server_port = items.server_port, data_path = items.data_path, buffer_size = items.buffer_size };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<protoSet>(stream, item, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoSet Deserialize(byte[] message)
        {
            protoSet item = new protoSet();
            using (var stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<protoSet>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
