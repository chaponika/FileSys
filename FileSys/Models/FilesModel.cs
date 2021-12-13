using System;
using System.Drawing;
using System.IO;

namespace FileSys.Models
{
    public class FilesModel
    {
        
        public bool isFolder { get; set; }
        
        public String path { get; set; }

        public String name { get; set; }

        public string Icon { get; set; }

        public FilesModel()
        {
        }

        public FilesModel(DirectoryInfo[] dirInfos, int index)
        {
            isFolder = true;
            name = dirInfos[index].Name;
            path = dirInfos[index].FullName;
        }
    }
}