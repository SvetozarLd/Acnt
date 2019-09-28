using ProtoBuf;
using System.IO;
using System;

namespace AccentBase_Updater.ProtoClasses
{
    public class VersionInfo
    {
        [ProtoContract]
        public class protoSet
        {
            [ProtoMember(1)]
            public string name { get; set; }
            [ProtoMember(2)]
            public int mainVer { get; set; }
            [ProtoMember(3)]
            public int assemblyVer { get; set; }
            [ProtoMember(4)]
            public int editionVer { get; set; }
            [ProtoMember(5)]
            public string error { get; set; }
            [ProtoMember(6)]
            public string updaterpath { get; set; }
            [ProtoMember(7)]
            public string serveraddress { get; set; }
            [ProtoMember(8)]
            public string ftplogin { get; set; }
            [ProtoMember(9)]
            public string ftppass { get; set; }
        }

        public byte[] Serialize(protoSet items)
        {
            protoSet item = new protoSet
            {
                name = items.name,
                mainVer = items.mainVer,
                assemblyVer = items.assemblyVer,
                editionVer = items.editionVer,
                error = items.error,
                updaterpath = items.updaterpath,
                serveraddress = items.serveraddress,
                ftplogin = items.ftplogin,
                ftppass = items.ftppass
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

        public Exception Save(string FileFullname, protoSet set)
        {
            try
            {
                if (File.Exists(FileFullname)) { File.Delete(FileFullname); }
                byte[] sett = Serialize(set);
                File.WriteAllBytes(FileFullname, sett);
                return null;
            }
            catch (Exception ex) { return ex; }
        }
        internal protoSet Load(string FileFullname)
        {
            protoSet set = new protoSet();
            try
            {
                if (File.Exists(FileFullname))
                {
                    ProtoClasses.VersionInfo ps = new ProtoClasses.VersionInfo();
                    byte[] sett = File.ReadAllBytes(FileFullname);
                    set = ps.Deserialize(sett);
                }
                else
                {
                    set = new ProtoClasses.VersionInfo.protoSet
                    { name = string.Empty, mainVer = 0, assemblyVer=0, editionVer=0, error = "Не установлено!"};
                }
            }
            catch (Exception ex)
            {
                set = new ProtoClasses.VersionInfo.protoSet
                    { name = string.Empty, mainVer = 0, assemblyVer=0, editionVer=0, error = ex.Message};
            }
            return set;
        }




    }
}
