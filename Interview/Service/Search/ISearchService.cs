using Interview.Entity.Response;

namespace Interview.Service.Search
{
    public interface ISearchService
    {
        SearchResponse SearchDataByID(string id);
        List<SearchResponse> SearchData(string query, string filter, string sort);
    }
}
