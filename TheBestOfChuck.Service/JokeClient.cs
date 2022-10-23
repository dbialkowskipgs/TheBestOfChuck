using System.Net.Http.Json;

namespace TheBestOfChuck.Service
{
    public class JokeClient : IJokeClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public JokeClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JokeClientDto> GetJokeClientAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Environment.GetEnvironmentVariable("Api_Endpoint") ?? throw new InvalidOperationException()),
                Headers =
                {
                    { "accept",  Environment.GetEnvironmentVariable("accept")},
                    { "X-RapidAPI-Key", Environment.GetEnvironmentVariable("X-RapidAPI-Key")},
                    { "X-RapidAPI-Host", Environment.GetEnvironmentVariable("X-RapidAPI-Host") },
                },
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var joke =  await response.Content.ReadFromJsonAsync<JokeClientDto>() ?? throw new InvalidOperationException();
            return joke;
        }

        public async Task<List<JokeClientDto>> GetSpecificAmountOfJokeClientAsync(int jokesAmount)
        {
            var jokes = new List<JokeClientDto>();
            for (var i = 0; i < jokesAmount; i++)
            {
                var joke = await GetJokeClientAsync();
                if (joke.Value.Length > 200)
                    continue;

                jokes.Add(joke);
            }

            return jokes;
        }
    }
    }
