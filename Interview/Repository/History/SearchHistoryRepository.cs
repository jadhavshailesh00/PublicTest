using Interview.Entity.History;
using System.Data.SqlClient;

namespace Interview.Repository.Search
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly string _connectionString;

        public SearchHistoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SearchHistory> GetSearchHistoryAsync(string UserID)
        {
            var history = new List<SearchHistory>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM SearchHistory WHERE UserId = @UserId ORDER BY Timestamp DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserID);

                    using (SqlDataReader reader =  cmd.ExecuteReader())
                    {
                        while ( reader.Read())
                        {
                            var searchHistory = new SearchHistory
                            {
                                SearchId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Query = reader.GetString(2),
                                Timestamp = reader.GetDateTime(3)
                            };
                            searchHistory.SearchResults =  GetSearchResultsAsync(searchHistory.SearchId, conn);
                            history.Add(searchHistory);
                        }
                    }
                }
            }

            return history;
        }

        private List<SearchResult> GetSearchResultsAsync(int searchId, SqlConnection conn)
        {
            var results = new List<SearchResult>();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM SearchResults WHERE SearchId = @SearchId", conn))
            {
                cmd.Parameters.AddWithValue("@SearchId", searchId);

                using (SqlDataReader reader =  cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SearchResult
                        {
                            ResultId = reader.GetInt32(0),
                            SearchId = reader.GetInt32(1),
                            ResultData = reader.GetString(2),
                            ResultRank = reader.GetInt32(3),
                            RetrievedAt = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return results;
        }

        public int SaveSearchData(string UserID, string query, string Filter, string Sort)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SearchHistory (UserId, Query, Timestamp) OUTPUT INSERTED.SearchId VALUES (@UserId, @Query,@Timestamp)", conn))
                {
                    string tempquery= "query=" + query + " Filter="+Filter+ " Sort="+ Sort;
                    cmd.Parameters.AddWithValue("@UserId", UserID);
                    cmd.Parameters.AddWithValue("@Query", tempquery);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);

                    return (int) cmd.ExecuteScalar();
                }
            }
        }
        public int SaveSearchDataByID(string UserID, string ID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SearchHistory (UserId, Query, Timestamp) OUTPUT INSERTED.SearchId VALUES (@UserId, @Query, @Timestamp)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserID);
                    cmd.Parameters.AddWithValue("@Query", "query="+ID);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);

                    //var cmd.()
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

    }
}
