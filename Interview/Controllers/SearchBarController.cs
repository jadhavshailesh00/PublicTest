using Interview.App_Start;
using Interview.Service.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers

{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SearchBarController :ControllerBase
    {

        private readonly ISearchService _searchService;
        private readonly ILogger<SearchBarController> _logger;

        public SearchBarController(ISearchService searchService, ILogger<SearchBarController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        
        [HttpGet()]
        public IActionResult SearchBar([FromQuery] string query, [FromQuery] string filter = null, [FromQuery] string sort = null)
        {
            var results =  _searchService.SearchData(query, filter, sort);
            if (results == null) return NotFound();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetSearchById(string id)
        {
            var result = _searchService.SearchDataByID(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

    }
}
