using ItSynced.Web.DAL.Entities;
using Microsoft.Data.Entity;

namespace ItSynced.Web.DAL.EntityFramework
{
    public class ItSyncedContext : DbContext
    {
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ModificationEntry> ModificationEntries { get; set; }
        
    }
}