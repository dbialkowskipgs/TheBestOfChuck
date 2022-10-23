using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace TheBestOfChuck
{
    public class ChuckJoke
    {
        private readonly ILogger _logger;
        private readonly Service.Service _service;
        public ChuckJoke(ILoggerFactory loggerFactory, Service.Service service)
        {
            _service = service;
            _logger = loggerFactory.CreateLogger<ChuckJoke>();
        }

        [Function("ChuckJoke")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");


            var joke = await _service.GetJokeAsync();
            await response.WriteStringAsync(joke);
            Console.WriteLine(joke);

            return response;
        }
    }
}
