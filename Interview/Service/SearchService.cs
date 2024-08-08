using Interview.Entity;
using Interview.Repository;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Interview.Service
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;

        public SearchService(IRepository searchResultRepository)
        {
            _searchResultRepository = searchResultRepository;
        }

        public SearchResponse SearchDataByID(int id)
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
            List < SearchResponse > result = new List < SearchResponse >();
            if (Filter != null)
            {
                if (Filter== "recent")
                {
                    result = ResultData.Where(item => item.Date >= DateTime.UtcNow.AddDays(-30)).ToList();
                }
            }
            return result;
        }

        public List<SearchResponse> ApplySort(List<SearchResponse> ResultData, string Sort)
        {
            List<SearchResponse> result = new List<SearchResponse>();
            if (Sort != null)
            {
                if (Sort == "ID")
                {
                    result = ResultData.OrderBy(item=>item.ID).ToList();
                }
                else if(Sort == "Title")
                {
                    result = ResultData.OrderBy(item => item.Title).ToList();
                }
                else if(Sort == "Description")
                {
                    result = ResultData.OrderBy(item => item.Description).ToList();
                }
                else if(Sort == "Category")
                {
                    result = ResultData.OrderBy(item => item.Category).ToList();
                }
                else if(Sort == "Date")
                {
                    result = ResultData.OrderBy(item => item.Date).ToList();
                }
            }
            return result;
        }

    }
}
