namespace ItSynced.Web.DAL.Entities
{
    public class TFLog : EntityWithId
    {
        public Entry[] entry { get; set; }
    }

    public class Entry
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string tfget { get; set; }
        public string tfresolve { get; set; }
    }
}