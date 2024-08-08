namespace Interview.Service
{
    public interface ISearchService
    {
        public string SearchDataByID(int id);
        public string SearchData(string query, string filter, string sort);
    }
}
