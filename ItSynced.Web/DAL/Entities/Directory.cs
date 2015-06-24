using System;
using System.Collections.Generic;

namespace ItSynced.Web.DAL.Entities
{
    public class Directory : EntityWithId
    {
        public string DirectoryName { get; set; }
        public virtual Directory ParentDirectory { get; set; }
        public string FullPath { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public virtual ICollection<File> Files { get; set; } 
        public virtual ICollection<Directory> Directories { get; set; } 
        public virtual IEnumerable<ModificationEntry> ModificationEntries { get; set; }
    }
}