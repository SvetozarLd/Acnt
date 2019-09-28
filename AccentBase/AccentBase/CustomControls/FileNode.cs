using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccentBase.CustomControls
{
    public class FileNode
    {
        public string FileStatus { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileCreate { get; set; }
        public string FullPath { get; set; }
        public List<FileNode> Children { get; set; }
        public FileNode Parent { get; set; }
        public FileNode(string Filename, string Filestatus, string Filesize, string Filecreate, string Fullfilepath, FileNode parent)
        {
            this.FileStatus = Filestatus;
            this.FileName = Filename;
            this.FileSize = Filesize;
            this.FileCreate = Filecreate;
            this.FullPath = Fullfilepath;
            this.Children = new List<FileNode>();
            this.Parent = parent;
        }

        public string sourcefile { get; set; }
        public string targetfile { get; set; }
        public Int64 Length { get; set; }
        public Int64 LastWriteTime { get; set; }
        public Int64 LastCreationTime { get; set; }
    }
}
