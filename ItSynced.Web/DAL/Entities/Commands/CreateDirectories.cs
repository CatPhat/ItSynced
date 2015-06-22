using System.Collections.Generic;
using System.Threading.Tasks;
using ItSynced.Web.DAL.EntityFramework;

namespace ItSynced.Web.DAL.Entities.Commands
{
    public class CreateDirectories
    {
        private readonly ItSyncedContext _dbContext;

        public CreateDirectories(ItSyncedContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(IList<Directory> directories)
        {
            _dbContext.AddRange(directories);
            await _dbContext.SaveChangesAsync();
        }
    }
}