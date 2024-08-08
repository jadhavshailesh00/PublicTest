using System.Collections.Generic;
using System.Linq.Expressions;

namespace Interview.Repository
{
    public class Repository : IRepository
    {
        
        public string SearchDataByID(int id)
        {
            return "data";
        }

        public string SearchData(string query, string filter, string sort)
        {
            return "data";
        }
    }
}
