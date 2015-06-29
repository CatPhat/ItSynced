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
                var existingFile = await _dbContext.Files.SingleOrDefaultAsync(x => file.FullPath == x.FullPath);
                if (existingFile == null)
                {
                    _dbContext.Add(file);
                }
                else
                {
                    existingFile = file;
                    _dbContext.Update(existingFile);
                }
               
            }
            foreach (var directory in directories)
            {
                var existingDirectory =
                    await _dbContext.Directories.SingleOrDefaultAsync(x => x.CompositeKey == directory.CompositeKey);

                if (existingDirectory == null)
                {
                    _dbContext.Add(directory);
                }
                else
                {
                    existingDirectory = directory;
                    _dbContext.Update(existingDirectory);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}