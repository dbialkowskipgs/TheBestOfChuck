using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
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

        [Function("Function1")]
        public async Task Run([TimerTrigger("%TriggerSchedule%")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            var jokesAmount = Environment.GetEnvironmentVariable("JokesAmount");
            if (!int.TryParse(jokesAmount, out var jokesAmountResult))
                throw new Exception("can't parse jokes amount to integer");

            var jokes = await _service.GetSpecificAmountOfJokeClientAsync(jokesAmountResult);
            foreach (var joke in jokes)
            {
                _logger.LogInformation("JOKE: {joke}",joke);
            }
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
