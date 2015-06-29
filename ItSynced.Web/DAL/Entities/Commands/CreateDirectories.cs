using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItSynced.Web.DAL.EntityFramework;
using Microsoft.Data.Entity;

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
            foreach (var file in directories.SelectMany(dir => dir.Files))
            {
                _dbContext.Add(new ModificationEntry
                {
                    File = file,
                    ModificationDateTime = file.LastModifiedDateTime
                });
                _dbContext.Add(file);
            }
            _dbContext.AddRange(directories);
            await _dbContext.SaveChangesAsync();
        }
    }
}