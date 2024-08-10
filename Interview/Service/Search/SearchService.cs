using Interview.Entity.Response;
using Interview.Repository;

namespace Interview.Service.Search
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;

        public SearchService(IRepository repository)
        {
            _searchResultRepository = repository;
        }

        public SearchResponse SearchDataByID(string id)
        {
            return _searchResultRepository.SearchDataByID(id);
        }

        public List<SearchResponse> SearchData(string query, string filter, string sort)
        {
            var resultData = _searchResultRepository.SearchData(query, filter, sort);

            resultData = ApplyFilter(resultData, filter);
            resultData = ApplySort(resultData, sort);

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
    }
}
