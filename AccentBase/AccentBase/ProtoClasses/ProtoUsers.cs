using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;

namespace AccentBase.ProtoClasses
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
        public List<string> protoDeserialize(byte[] message)
        {
            List<string> item = new List<string>();
            using (var stream = new MemoryStream(message))
            {
                try { item = Serializer.DeserializeWithLengthPrefix<List<string>>(stream, PrefixStyle.Base128, Serializer.ListItemTag); }
                catch { item = null; }
            }
            return item;
        }
    }
}
