using Microsoft.Extensions.Configuration;
using CodingTracker.Database;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();


var connectionString = configuration["ConnectionString"];
if (connectionString is null)
{
    return 1;
}
var db = new Database(connectionString);
db.Initialise();

return 0;
