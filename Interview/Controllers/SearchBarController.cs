using Interview.Service;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchBarController :ControllerBase
    {

        private readonly ISearchService _searchService;

        public SearchBarController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet()]
        public async Task<IActionResult> SearchBar([FromQuery] string query, [FromQuery] string filter = null, [FromQuery] string sort = null)
        {
            var results =  _searchService.SearchData(query, filter, sort);
            if (results == null) return NotFound();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSearchById(int id)
        {
            var result = _searchService.SearchDataByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

    }
}
