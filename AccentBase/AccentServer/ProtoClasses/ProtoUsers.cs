using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentServer.ProtoClasses
{
    class ProtoUsers
    {
        [ProtoContract]
        public class protoList
        {
            [ProtoMember(1)]
            public List<string> plist { get; set; }
        }

        public byte[] protoSerialize(List<string> items)
        {
            var item = new protoList { plist = new List<string>(items) };
            byte[] result = null;
            try
            {
                using (var stream = new MemoryStream()) { Serializer.SerializeWithLengthPrefix<List<string>>(stream, items, PrefixStyle.Base128, Serializer.ListItemTag); result = stream.ToArray(); }
                return result;
            }
            catch { return null; }
        }
        public protoList protoDeserialize(byte[] message)
        {
            protoList item = new protoList();
            using (var stream = new MemoryStream(message))
            {
                try { item.plist = Serializer.DeserializeWithLengthPrefix<List<string>>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
