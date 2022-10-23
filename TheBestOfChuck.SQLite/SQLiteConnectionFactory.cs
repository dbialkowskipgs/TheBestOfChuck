using System.Data.SQLite;

namespace TheBestOfChuck.SQLite;

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