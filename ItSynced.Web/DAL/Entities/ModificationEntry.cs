using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItSynced.Web.DAL.Entities
{
    public class ModificationEntry : EntityWithId
    {
        public int FileId { get; set; }
        public int DirectoryId { get; set; }
        public DateTime ModificationDateTime { get; set; }
    }
}
