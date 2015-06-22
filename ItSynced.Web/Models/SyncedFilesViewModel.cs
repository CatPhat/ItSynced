using System.Collections.Generic;

namespace ItSynced.Web.Models
{
    public class SyncedFilesViewModel
    {
        public IEnumerable<DirectoriesAndFilesView> DirectoryAndFilesViews { get; set; }
    }
}