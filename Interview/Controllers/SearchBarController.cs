using Interview.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers
{
    public class SearchBarController :ControllerBase
    {

        private readonly ISearchService _searchService;

        public SearchBarController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetSearchById()
        {
            var result = _searchService.searchData();
            if (result == null) return NotFound();
            return Ok(result);
        }

    }
}
