using System.Text.RegularExpressions;
using TheBestOfChuck.SQLite;

namespace TheBestOfChuck.Service
{
    public class Service
    {
        private readonly IBestOfChuckRepo _bestOfChuckRepo;
        private readonly IJokeClient _jokeClient;

        public Service(IBestOfChuckRepo bestOfChuckRepo, IJokeClient jokeClient)
        {
            _bestOfChuckRepo = bestOfChuckRepo;
            _jokeClient = jokeClient;
        }

        public async Task<string> GetJokeAsync()
        {
            var jokeClient = await _jokeClient.GetJokeClientAsync();
            await InsertJokeAsync(jokeClient);
            var joke = await GetJokeAsync(jokeClient);
            return joke;
        }

        public async Task<List<JokeClientDto>> GetSpecificAmountOfJokeClientAsync(int jokesAmount)
        {
            var jokesClient = await _jokeClient.GetSpecificAmountOfJokeClientAsync(jokesAmount);
            //await InsertJokeAsync(jokesClient);
            //var jokes = await GetAllJokesAsync();
            return jokesClient;
        }

        private async Task InsertJokeAsync(JokeClientDto jokeClient)
        {
            jokeClient.Value = RemoveSpecialChar(jokeClient.Value);
            await _bestOfChuckRepo.InsertAsync(jokeClient.Id, jokeClient.Value);
        }

        public async Task InsertJokesAsync(List<JokeClientDto> jokeClient)
        {
            var jokes = new Dictionary<string, string>();
            foreach (var joke in jokeClient)
            {
                joke.Value = RemoveSpecialChar(joke.Value);
                jokes.Add(joke.Id, joke.Value);
            }

            await _bestOfChuckRepo.InsertAsync(jokes);
        }

        private async Task<string> GetJokeAsync(JokeClientDto jokeClient)
        {
            return await _bestOfChuckRepo.GetByIdAsync(jokeClient.Id);
        }

        public async Task<List<string>> GetAllJokesAsync()
        {
            return await _bestOfChuckRepo.GetAll();
        }


        private string RemoveSpecialChar(string joke)
        {
            return joke.Replace("'", string.Empty);
        }
    }
}
