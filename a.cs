using System;
using System.Web.Http;

namespace YourNamespace
{
    public class CollageController : ApiController
    {
        private readonly CollageRepository _collageRepository;

        public CollageController()
        {
            _collageRepository = new CollageRepository(); // Initialize the repository
        }

        [HttpGet]
        [Route("api/collage")]
        public IHttpActionResult GetCollageData()
        {
            try
            {
                string jsonResult = _collageRepository.GetCollageData();

                // Return a 200 OK response with the JSON string, explicitly setting the content type to application/json
                return new RawJsonActionResult(jsonResult);
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 Internal Server Error response
                return InternalServerError(ex);
            }
        }
    }

    // Custom ActionResult to return raw JSON string
    public class RawJsonActionResult : IHttpActionResult
    {
        private readonly string _jsonString;

        public RawJsonActionResult(string jsonString)
        {
            _jsonString = jsonString;
        }

        public System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new System.Net.Http.StringContent(_jsonString, System.Text.Encoding.UTF8, "application/json")
            };
            return System.Threading.Tasks.Task.FromResult(response);
        }
    }
}
