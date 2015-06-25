using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItSynced.Web.DAL.Entities
{
    public class ModificationEntry : EntityWithId
    {
        public virtual File File { get; set; }
        public DateTime ModificationDateTime { get; set; }
    }
}
