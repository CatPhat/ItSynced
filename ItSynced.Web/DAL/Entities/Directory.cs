using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItSynced.Web.DAL.Entities
{
    public class Directory : EntityWithId
    {
        public Directory()
        {
            Directories = new HashSet<Directory>();
        }

        [Required]
        public string DirectoryName { get; set; }
            
 
        
        [Required]
        public string FullPath { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public virtual ICollection<File> Files { get; set; }

        public virtual Directory ParentDirectory { get; set; } 
        public virtual ICollection<Directory> Directories { get; set; } 
       
    }
}