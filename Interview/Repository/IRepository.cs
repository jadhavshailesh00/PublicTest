using System.Linq.Expressions;

namespace Interview.Repository
{
    public interface IRepository
    {
        public string SearchDataByID(int id);
        public string SearchData(string query, string filter, string sort);

    }
}
