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

        public string SearchDataByID(int id)
        {
            return _searchResultRepository.SearchDataByID(id);
        }


        public string SearchData(string query, string filter, string sort)
        {
            return _searchResultRepository.SearchData(query,filter,sort);
        }
        
    }
}
