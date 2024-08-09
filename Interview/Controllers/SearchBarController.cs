using Interview.App_Start;
using Interview.Service;
using Microsoft.AspNetCore.Mvc;
using Interview.App_Start;
using Microsoft.AspNetCore.Authorization;

namespace Interview.Controllers
{
    [Route("api/[controller]")]
    [CustomExceptionFilter]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SearchBarController :ControllerBase
    {

        private readonly ISearchService _searchService;

        public SearchBarController(ISearchService searchService)
        {
            _searchService = searchService;
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
