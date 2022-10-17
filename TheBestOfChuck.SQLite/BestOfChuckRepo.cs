using TheBestOfChuck.SQLite;

namespace TheBestOfChuck.Repo
{
    public class BestOfChuckRepo : IBestOfChuckRepo
    {
        private readonly SQLiteConnectionFactory _sqLiteConnectionFactory;
        public BestOfChuckRepo(SQLiteConnectionFactory sqLiteConnectionFactory)
        {
            _sqLiteConnectionFactory = sqLiteConnectionFactory;
        }

        public async Task<string> GetByIdAsync(string id)
        {
            await using (await _sqLiteConnectionFactory.OpenConnection())
            {
                var sqLiteConnection = _sqLiteConnectionFactory.GetCurrentConnection();
                var sqlite_cmd = sqLiteConnection.CreateCommand();
                sqlite_cmd.CommandText = $"SELECT Joke FROM TheBestOfChuck WHERE Id = '{id}'";

                var joke = string.Empty;
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (await sqlite_datareader.ReadAsync())
                {
                    joke = sqlite_datareader.GetString(0);
                }

                return joke;
            }
        }

        public async Task InsertAsync(string id, string value)
        {
            await using var connection = await _sqLiteConnectionFactory.OpenConnection();
            await using var transaction = connection.BeginTransaction();
            
            var sqLiteConnection = _sqLiteConnectionFactory.GetCurrentConnection();
            var sqlite_cmd = sqLiteConnection.CreateCommand();
            sqlite_cmd.CommandText = $"INSERT INTO TheBestOfChuck (Id, Joke) VALUES('{id}', '{value}'); ";
            sqlite_cmd.ExecuteNonQuery();

            transaction.Commit();
        }

        public async Task InsertAsync(Dictionary<string, string> jokes)
        {
            await using var connection = await _sqLiteConnectionFactory.OpenConnection();
            await using var transaction = connection.BeginTransaction();
            
            var sqLiteConnection = _sqLiteConnectionFactory.GetCurrentConnection();
            var sqliteCmd = sqLiteConnection.CreateCommand();

            foreach (var joke in jokes)
            {
                sqliteCmd.CommandText = $"INSERT INTO TheBestOfChuck (Id, Joke) VALUES('{joke.Key}', '{joke.Value}'); ";
                sqliteCmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public async Task<List<string>> GetAll()
        {
            await using (await _sqLiteConnectionFactory.OpenConnection())
            {
                var sqLiteConnection = _sqLiteConnectionFactory.GetCurrentConnection();
                var sqlite_cmd = sqLiteConnection.CreateCommand();
                sqlite_cmd.CommandText = $"SELECT Joke FROM TheBestOfChuck";

                var jokes = new List<string>();
                var sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (await sqlite_datareader.ReadAsync())
                {
                    jokes.Add(sqlite_datareader.GetString(0));
                }

                return jokes;
            }
        }
    }
}
