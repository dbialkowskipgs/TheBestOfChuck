namespace TheBestOfChuck.SQLite
{
    public interface IBestOfChuckRepo
    {
        public Task<string> GetByIdAsync(string id);
        public Task InsertAsync(string id, string value);
        public Task InsertAsync(Dictionary<string, string> jokes);
        public Task<List<string>> GetAll();

    }
}
