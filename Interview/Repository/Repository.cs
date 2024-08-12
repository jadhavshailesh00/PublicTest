﻿using Interview.Entity.History;
using Interview.Entity.Response;
using System.Data.SqlClient;

namespace Interview.Repository
{
    public class Repository : IRepository
    {
        private readonly List<SearchResponse> _data = new List<SearchResponse>();
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SearchResponse SearchDataByID(string ID)
        {

            var result = new SearchResponse();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ContentItems WHERE ID = @ID ORDER BY Date DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {

                            var _data = new SearchResponse
                            {
                                ID = reader.GetString(0),
                                Category = reader.GetString(1),
                                Title = reader.GetString(2),
                                Description = reader.GetString(3),
                                Date = reader.GetDateTime(4),
                            };
                            result = _data;
                        }
                    }

                }

            }

            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public List<SearchResponse> SearchData(string query, string filter, string sort)
        {

            var searchData = new List<SearchResponse>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ContentItems WHERE ID = @ID Or filter=@filter Or sort=@sort  ORDER BY Date DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", query);
                    cmd.Parameters.AddWithValue("@sort", sort);
                    cmd.Parameters.AddWithValue("@filter", filter);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _data = new SearchResponse
                            {
                                ID = reader.GetString(0),
                                Category = reader.GetString(1),
                                Title = reader.GetString(2),
                                Description = reader.GetString(3),
                                Date = reader.GetDateTime(4)
                            };
                            searchData.Add(_data);
                        }
                    }
                }
            }
            //var searchData = _data.Where(item =>
            //               item.Title.Contains(query) || item.Description.Contains(query) || item.Category.Contains(query));
            if (searchData.Count() == 0)
            {
                return null;
            }
            return searchData.ToList();
        }
    }
}
