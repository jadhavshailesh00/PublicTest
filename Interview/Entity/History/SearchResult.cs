﻿using Interview.Entity.Response;

namespace Interview.Entity.History
{ 
    public class SearchResult
    {
        public int ResultId { get; set; }
        public int SearchId { get; set; }
        public string ResultData { get; set; }
        public int ResultRank { get; set; }
        public DateTime RetrievedAt { get; set; }
        public SearchHistory SearchHistory { get; set; }
    }
}
