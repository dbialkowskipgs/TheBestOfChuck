using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TheBestOfChuck.Service;

namespace ChuckJokesTrigger
{
    public class TriggerChuckJokes
    {
        private readonly ILogger _logger;
        private readonly Service _service;

        public TriggerChuckJokes(ILoggerFactory loggerFactory, Service service)
        {
            _service = service;
            _logger = loggerFactory.CreateLogger<TriggerChuckJokes>();
        }

        [Function("TriggerChuckJokes")]
        public async Task Run([TimerTrigger("%TriggerSchedule%")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var jokesAmount = GetJokesAmount();
            var jokesFromClient = await _service.GetSpecificAmountOfJokeClientAsync(jokesAmount);
            await _service.InsertJokesAsync(jokesFromClient);
            var jokes = await _service.GetAllJokesAsync();
            foreach (var joke in jokes)
            {
                _logger.LogInformation("JOKE: {joke}",joke);
            }
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
