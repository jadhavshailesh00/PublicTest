using Interview.Entity;

namespace Interview.Service
{
    public interface ISearchService
    {
        public SearchResponse SearchDataByID(string id);
        public List<SearchResponse> SearchData(string query, string filter, string sort);
    }
}
