using Interview.Controllers;
using Interview.Entity.History;
using Interview.Entity.Response;
using Interview.Service.Search;
using Microsoft.Extensions.Logging;
using Moq;
using System.Web.Http.Results;

namespace Interview.Test
{
    [TestFixture]
    public class SearchBarControllerTests
    {
        private Mock<ISearchService> _mockSearchService;
        private Mock<ILogger<SearchBarController>> _mockLogger;
        private SearchBarController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockSearchService = new Mock<ISearchService>();
            _mockLogger = new Mock<ILogger<SearchBarController>>();
            _controller = new SearchBarController(_mockSearchService.Object, _mockLogger.Object);
        }

        [Test]
        public void SearchBar_ReturnsBadRequest_WhenQueryIsNull()
        {

            var expectedResult = new ErrorResponse
            {
                Message = "No results were found for your search query.",
                ErrorCode = "NO_RESULTS_FOUND",
                Resolution = "Please try adjusting your search parameters.",
                ErrorId = Guid.NewGuid().ToString()
            };
            var result = _controller.SearchBar(null) as Microsoft.AspNetCore.Mvc.NotFoundObjectResult;
            var actualresult = result.Value as ErrorResponse;
            Assert.AreEqual(actualresult.Message, expectedResult.Message);
            Assert.AreEqual(expectedResult.ErrorCode, actualresult.ErrorCode);
            Assert.AreEqual(expectedResult.Resolution, actualresult.Resolution);
        }

        [Test]
        public void SearchBar_ReturnsNotFound_WhenNoResultsFound()
        {
            _mockSearchService.Setup(service => service.SearchData("1", "query", "filter", "sort"))
                     .Returns((List<SearchResponse>)null);

            var result = _controller.SearchBar("query", "filter", "sort");

            Assert.IsNotNull(result);

        }

       
    }

}