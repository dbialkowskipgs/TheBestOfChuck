namespace TheBestOfChuck.Service;

public interface IJokeClient
{
    Task<JokeClientDto> GetJokeClientAsync();
    Task<List<JokeClientDto>> GetSpecificAmountOfJokeClientAsync(int jokesAmount);
}