using ItSynced.Web.DAL.Entities;
using Microsoft.Data.Entity;

namespace ItSynced.Web.DAL.EntityFramework
{
    public class ItSyncedContext : DbContext
    {
        
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ModificationEntry> ModificationEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=GANESH\MDEVSQLSERVER;Database=ItSyncedDatabase;User Id=devuser;Password=password123");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Directory>().Key(x => x.DirectoryId);
            builder.Entity<File>().Key(x => x.FileId);
            builder.Entity<Directory>()
                .Collection(x => x.Directories).InverseReference(y => y.ParentDirectory);
              
            builder.Entity<Directory>()
                .Collection(x => x.Files)
                .InverseReference(y => y.ParentDirectory)
                .ForeignKey(z => z.FileId);
        }
    }
}