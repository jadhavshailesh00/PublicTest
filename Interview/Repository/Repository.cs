using Interview.Entity;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Interview.Repository
{
    public class Repository : IRepository
    {
        private readonly List<SearchResponse> _data=new List<SearchResponse>();

        public Repository()
        {
            _data.Add(new SearchResponse { ID = "1" ,Category = "Technology", Date = new DateTime(2024, 8, 8), Description = "New Technology", Title = "Title Technology" });
            _data.Add(new SearchResponse { ID = "2" ,Category = "Entertainment", Date = new DateTime(2024, 7, 8), Description = "New Entertainment", Title = "Title Entertainment" });
            _data.Add(new SearchResponse { ID = "3" ,Category = "Sports", Date = new DateTime(2024, 6, 8), Description = "New Sports", Title = "Title Sports" });
            _data.Add(new SearchResponse { ID = "4" ,Category = "Health", Date = new DateTime(2024, 5, 8), Description = "New Health", Title = "Title Health" });
            _data.Add(new SearchResponse { ID = "5" ,Category = "Business", Date = new DateTime(2024, 4, 8), Description = "New Business", Title = "Title Business" });
            _data.Add(new SearchResponse { ID = "6" ,Category = "Travel", Date = new DateTime(2024, 3, 8), Description = "New Travel", Title = "Title Travel" });
            _data.Add(new SearchResponse { ID = "7" ,Category = "Food", Date = new DateTime(2024, 2, 8), Description = "New Food", Title = "Title Food" });
            _data.Add(new SearchResponse { ID = "8" ,Category = "Fashion", Date = new DateTime(2024, 1, 8), Description = "New Fashion", Title = "Title Fashion" });
            _data.Add(new SearchResponse { ID = "9" ,Category = "Books", Date = new DateTime(2024, 11, 8), Description = "New Books", Title = "Title Books" });
            _data.Add(new SearchResponse { ID = "10" ,Category = "Music", Date = new DateTime(2024, 12, 8), Description = "New album", Title = "Title album" });
        }

        public SearchResponse SearchDataByID(string id)
        {
            var result= _data.Where(item=>item.ID.Equals(id)).FirstOrDefault();
            if (result == null)
            {
                return new SearchResponse { ID = "-1", Category = "Not Found", Date = DateTime.MinValue, Description = "Search item not found", Title = "No Match" };
            }
            else
            {
                return result;
            }
        }

        public List<SearchResponse> SearchData(string query, string filter, string sort)
        {
            var searchData = _data.Where(item =>
                           item.Title.Contains(query) || item.Description.Contains(query) || item.Category.Contains(query))  ;
            return searchData.ToList();
        }
    }
}
