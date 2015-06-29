using System;
using System.Collections.Generic;

namespace ItSynced.Web.DAL.Entities
{
    public class File : EntityWithId
    {
        public int ParentDirectoryId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FullPath { get; set; }
        public virtual Directory ParentDirectory { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}