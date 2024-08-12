using Interview.Entity.History;
using Interview.Entity.Response;

namespace Interview.Service.Search
{
    public interface ISearchService
    {
        SearchResponse SearchDataByID(string UserID,string id);
        List<SearchResponse> SearchData(string UserID,string query, string filter, string sort);


        //public int ExecuteSaveSearchData(string UserID, string query, string Filter, string Sort);
        //public int ExecuteSaveSearchDataByID(string UserID, string query);
        public List<SearchHistory> GetSearchHistoryAsync(string userId);
    }
}
