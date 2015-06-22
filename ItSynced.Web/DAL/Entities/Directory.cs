using System;
using System.Collections.Generic;

namespace ItSynced.Web.DAL.Entities
{
    public class Directory
    {
        public int DirectoryId { get; set; }
        public string DirectoryName { get; set; }
        public virtual Directory ParentDirectory { get; set; }
        public string FullPath { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public virtual IEnumerable<File> Files { get; set; } 
        public virtual ICollection<Directory> Directories { get; set; } 
        public virtual IEnumerable<ModificationEntry> ModificationEntries { get; set; }
    }
}