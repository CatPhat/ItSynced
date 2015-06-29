using ItSynced.Web.DAL.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Query;

namespace ItSynced.Web.DAL.EntityFramework
{
    public class ItSyncedContext : DbContext
    {
        
        public DbSet<Directory> Directories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ModificationEntry> ModificationEntries { get; set; }

      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Directory>().Key(x => x.Id);
            builder.Entity<Directory>()
                .Collection(d => d.Directories)
                .InverseReference(d => d.ParentDirectory);
            
          


            //  builder.Entity<Directory>().Property(x => x.Id).ForSqlServer(y => y.UseSequence());



            builder.Entity<File>().Key(x => new
            {
                x.FullPath,
                x.FileName
            });
          

            //builder.Entity<File>().Property(x => x.Id).GenerateValueOnAdd();

            builder.Entity<ModificationEntry>().Key(x => x.Id);

            builder.Entity<TFLog>().Key(x => x.Id);

            base.OnModelCreating(builder);


            //  builder.Entity<Directory>().Collection(x => x.Files).InverseReference(y => y.ParentDirectory);
        }
    }
}