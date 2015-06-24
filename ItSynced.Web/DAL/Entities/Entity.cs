using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItSynced.Web.DAL.Entities
{
    public abstract class Entity
    {
    }

    public abstract class EntityWithId : Entity
    {
        public int Id { get; set; }
    }
}
