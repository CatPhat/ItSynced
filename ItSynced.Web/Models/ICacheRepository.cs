namespace ItSynced.Web.Models
{
    public interface ICacheRepository
    {
        object GetItem(string key);
    }
}