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
           
            foreach (var dir in directories)
            {
                var directory =
                    await
                        _dbContext.Directories.FirstOrDefaultAsync(
                            x => x.DirectoryName == dir.DirectoryName && x.FullPath == dir.FullPath) ?? new Directory
                            {
                                DirectoryName = dir.DirectoryName,
                                LastModifiedDateTime = dir.LastModifiedDateTime,
                                ParentDirectory = dir.ParentDirectory
                            };

                foreach (var file in dir.Files)
                {
                    
                }

            }
            await _dbContext.SaveChangesAsync();
        }
    }
}