using Interview.Service.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers
{
    /// <summary>
    /// Handles search-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SearchBarController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchBarController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBarController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="logger">The logger.</param>
        public SearchBarController(ISearchService searchService, ILogger<SearchBarController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Searches for data based on the query, filter, and sort parameters.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="filter">The optional filter parameter.</param>
        /// <param name="sort">The optional sort parameter.</param>
        /// <returns>A list of search results.</returns>
        /// <response code="200">Returns the list of search results.</response>
        /// <response code="404">If no results are found.</response>
        [HttpGet]
        public IActionResult SearchBar([FromQuery] string query, [FromQuery] string filter = null, [FromQuery] string sort = null)
        {
            _logger.LogInformation("SearchBar endpoint called with query: {query}, filter: {filter}, sort: {sort}", query, filter, sort);

            var results = _searchService.SearchData(query, filter, sort);
            if (results == null)
            {
                _logger.LogWarning("No results found for query: {query}", query);
                return NotFound();
            }

            return Ok(results);
        }

        /// <summary>
        /// Retrieves a specific search result by its ID.
        /// </summary>
        /// <param name="id">The ID of the search result.</param>
        /// <returns>The search result corresponding to the provided ID.</returns>
        /// <response code="200">Returns the search result.</response>
        /// <response code="404">If no result is found with the provided ID.</response>
        [HttpGet("{id}")]
        public IActionResult GetSearchById(string id)
        {
            _logger.LogInformation("GetSearchById endpoint called with ID: {id}", id);

            var result = _searchService.SearchDataByID(id);
            if (result == null)
            {
                _logger.LogWarning("No result found with ID: {id}", id);
                return NotFound();
            }

            return Ok(result);
        }
    }
}
