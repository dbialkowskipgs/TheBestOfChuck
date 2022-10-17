using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheBestOfChuck;
using TheBestOfChuck.SQLite;
using TheBestOfChuck.Repo;
using System.Data.SQLite;
using TheBestOfChuck.Service;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
        {
            services.AddScoped<IBestOfChuckRepo, BestOfChuckRepo>();
            services.AddScoped<Service>();
            services.AddScoped<SQLiteConfiguration>();
            services.AddScoped(_ =>
            {
                var sessionFactory =
                    new SQLiteConnectionFactory(Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException());
                return sessionFactory;
            });
        })
    .Build();
await CreateSqLiteDatabase();

host.Run();

async Task CreateSqLiteDatabase()
{
    SQLiteConnection.CreateFile("TheBestOfChuck.sqlite");
    var sqLiteConfiguration = host.Services.GetRequiredService<SQLiteConfiguration>();
    sqLiteConfiguration.CreateSqLiteDatabaseIfNotExists();
    await sqLiteConfiguration.CreateTable();
}