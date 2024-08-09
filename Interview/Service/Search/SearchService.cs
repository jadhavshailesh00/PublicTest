using Interview.Entity.Response;
using Interview.Repository;

namespace Interview.Service.Search
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;

        public SearchService(IRepository Repository)
        {
            _searchResultRepository = Repository;
        }

        public SearchResponse SearchDataByID(string id)
        {
            return _searchResultRepository.SearchDataByID(id);
        }

        public List<SearchResponse> SearchData(string query, string filter, string sort)
        {
            var ResultData = _searchResultRepository.SearchData(query, filter, sort);

            ResultData = ApplyFilter(ResultData, filter);

            ResultData = ApplySort(ResultData, sort);

            return ResultData;
        }


        public List<SearchResponse> ApplyFilter(List<SearchResponse> ResultData, string Filter)
        {
            List<SearchResponse> result = new List<SearchResponse>();
            if (Filter != null)
            {
                if (Filter == "recent")
                {
                    return result = ResultData.Where(item => item.Date >= DateTime.UtcNow.AddDays(-30)).ToList();
                }
            }
            return ResultData;
        }

        public List<SearchResponse> ApplySort(List<SearchResponse> ResultData, string Sort)
        {
            List<SearchResponse> result = new List<SearchResponse>();
            if (Sort != null)
            {
                if (Sort == "ID")
                {
                    return result = ResultData.OrderByDescending(item => item.ID).ToList();
                }
                else if (Sort == "Title")
                {
                    return result = ResultData.OrderBy(item => item.Title).ToList();
                }
                else if (Sort == "Description")
                {
                    return result = ResultData.OrderBy(item => item.Description).ToList();
                }
                else if (Sort == "Category")
                {
                    return result = ResultData.OrderBy(item => item.Category).ToList();
                }
                else if (Sort == "Date")
                {
                    return result = ResultData.OrderBy(item => item.Date).ToList();
                }
            }
            return ResultData;
        }

    }
}
