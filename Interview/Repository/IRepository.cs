using Interview.Entity;
using System.Linq.Expressions;

namespace Interview.Repository
{
    public interface IRepository
    {
        public SearchResponse SearchDataByID(int id);
        public List<SearchResponse> SearchData(string query, string filter, string sort);

    }
}
