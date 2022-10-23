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
}