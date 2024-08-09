using Interview.Entity.Response;

namespace Interview.Repository
{
    public interface IRepository
    {
        public SearchResponse SearchDataByID(string id);
        public List<SearchResponse> SearchData(string query, string filter, string sort);

    }
}
