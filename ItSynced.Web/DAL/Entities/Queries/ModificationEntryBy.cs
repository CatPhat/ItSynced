using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItSynced.Web.DAL.EntityFramework;

namespace ItSynced.Web.DAL.Entities.Queries
{
    public class ModificationEntryBy
    {
        private readonly ItSyncedContext _dbContext;
        
        public ModificationEntryBy(ItSyncedContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ModificationEntry> GetAsync(File file, DateTime lastModifiedDateTime)
        {
            if (file.Id == 0) return null;

            var result =
                _dbContext.ModificationEntries.FirstOrDefaultAsync(
                    x => x.File.Id == file.Id && x.ModificationDateTime == lastModifiedDateTime);

            return await result;
        }
    }
}
