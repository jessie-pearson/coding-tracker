using Microsoft.Extensions.Configuration;
using CodingTracker;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration["ConnectionString"];
if (connectionString is null)
{
    return 1;
}

var db = new DatabaseController(connectionString);
db.Initialise();
UserInterface.Menu(db);

return 0;
