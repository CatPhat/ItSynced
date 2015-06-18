using System;

namespace ItSynced.Web.Models
{
    public class FileView
    {
        public string ParentFolderName { get; set; }
        public string FileName { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public string FullPath { get; set; }

    }
}