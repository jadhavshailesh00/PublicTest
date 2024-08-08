using Interview.Repository;

namespace Interview.Service
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;

        public SearchService(IRepository searchResultRepository)
        {
            _searchResultRepository = searchResultRepository;
        }

        public string searchData()
        {
            return _searchResultRepository.searchData();
        }
    }
}
