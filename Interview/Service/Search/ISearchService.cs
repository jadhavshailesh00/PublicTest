using Interview.Entity.Response;

namespace Interview.Service.Search
{
    public interface ISearchService
    {
        public SearchResponse SearchDataByID(string id);
        public List<SearchResponse> SearchData(string query, string filter, string sort);
    }
}
