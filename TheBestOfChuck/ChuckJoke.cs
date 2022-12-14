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

            var jokesAmount = GetJokesAmount();
            var jokesFromClient = await _service.GetSpecificAmountOfJokeClientAsync(jokesAmount);
            await _service.InsertJokesAsync(jokesFromClient);
            var jokes = await _service.GetAllJokesAsync();
            foreach (var joke in jokes)
            {
                _logger.LogInformation("JOKE: {joke}", joke);
            }

            return response;
        }

        private int GetJokesAmount()
        {
            var jokesAmount = Environment.GetEnvironmentVariable("JokesAmount");
            if (!int.TryParse(jokesAmount, out var jokesAmountResult))
                throw new Exception("can't parse jokes amount to integer");

            return jokesAmountResult;
        }
    }
}
