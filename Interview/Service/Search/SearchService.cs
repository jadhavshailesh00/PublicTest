using Interview.Entity.History;
using Interview.Entity.Response;
using Interview.Model;
using Interview.Repository;
using Interview.Repository.Search;

namespace Interview.Service.Search
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;
        private ISearchHistoryRepository _SearchHistoryRepository;

        public SearchService(IRepository repository, string connectionString)
        {
            _searchResultRepository = repository;
            _SearchHistoryRepository = new SearchHistoryRepository(connectionString);
        }

        public SearchResponse SearchDataByID(string UserID,string query)
        {
            var data=_searchResultRepository.SearchDataByID(query);
            int searchId = ExecuteSaveSearchDataByID(UserID, query);
            return data;
        }

        public List<SearchResponse> SearchData(string UserID,string query, string filter, string sort)
        {
            var resultData = _searchResultRepository.SearchData(query, filter, sort);

            resultData = ApplyFilter(resultData, filter);
            resultData = ApplySort(resultData, sort);


            int data = ExecuteSaveSearchData(UserID, query, filter, sort);
            //return searchId;
            return resultData;
        }

        private List<SearchResponse> ApplyFilter(List<SearchResponse> resultData, string filter)
        {
            if (filter == "recent")
            {
                return resultData.Where(item => item.Date >= DateTime.UtcNow.AddDays(-30)).ToList();
            }
            return resultData;
        }

        private List<SearchResponse> ApplySort(List<SearchResponse> resultData, string sort)
        {
            return sort switch
            {
                "ID" => resultData.OrderByDescending(item => item.ID).ToList(),
                "Title" => resultData.OrderBy(item => item.Title).ToList(),
                "Description" => resultData.OrderBy(item => item.Description).ToList(),
                "Category" => resultData.OrderBy(item => item.Category).ToList(),
                "Date" => resultData.OrderBy(item => item.Date).ToList(),
                _ => resultData,
            };
        }



        private int ExecuteSaveSearchData(string UserID, string query, string Filter, string Sort)
        {
            int searchId = _SearchHistoryRepository.SaveSearchData(UserID, query, Filter, Sort);
            return searchId;
        }

        private int ExecuteSaveSearchDataByID(string UserID, string query)
        {
            int searchId = _SearchHistoryRepository.SaveSearchDataByID(UserID, query);
            return searchId;
        }

        public List<SearchHistory> GetSearchHistoryAsync(string userId)
        {
            return _SearchHistoryRepository.GetSearchHistoryAsync(userId);
        }


    }
}
