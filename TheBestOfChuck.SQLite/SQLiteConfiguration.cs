using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace TheBestOfChuck.SQLite
{
    public class SQLiteConfiguration
    {
        private readonly SQLiteConnectionFactory _sqLiteConnectionFactory;

        public SQLiteConfiguration(SQLiteConnectionFactory sqLiteConnectionFactory)
        {
            _sqLiteConnectionFactory = sqLiteConnectionFactory;
        }

        public void CreateSqLiteDatabaseIfNotExists()
        {
            if (File.Exists(Environment.GetEnvironmentVariable("DbName")))
                return;
            SQLiteConnection.CreateFile("TheBestOfChuck.sqlite");
        }

        public async Task CreateTable()
        {
            await using (await _sqLiteConnectionFactory.OpenConnection())
            {
                var sqLiteConnection = _sqLiteConnectionFactory.GetCurrentConnection();
                var sqlite_cmd = sqLiteConnection.CreateCommand();
                sqlite_cmd.CommandText = "CREATE TABLE TheBestOfChuck (Id VARCHAR(255), Joke VARCHAR(255))";
                sqlite_cmd.ExecuteNonQuery();

            }
        }

    }

    public class SQLiteConnectionFactory
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public SQLiteConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(connectionString));
            _connectionString = connectionString;
        }

        public async Task<SQLiteConnection> OpenConnection()
        {
            _connection = new SQLiteConnection(_connectionString);
            await _connection.OpenAsync().ConfigureAwait(false);

            return _connection;
        }

        public SQLiteConnection GetCurrentConnection()
        {
            if (_connection == null) throw new InvalidOperationException();
            return _connection;
        }

    }
}