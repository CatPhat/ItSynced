using System;
using System.Collections.Generic;

namespace ItSynced.Web.DAL.Entities
{
    public class File
    {
        public int FileId { get; set; }
        public int ParentDirectoryId { get; set; }
        public string ParentDirectoryName { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public virtual List<ModificationEntry> ModificationEntries { get; set; }
    }
}