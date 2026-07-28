using Microsoft.Data.Sqlite;

namespace CodingTracker.Database;

internal class Database(string connectionString)
{
    internal void Initialise()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string createTable = """
                                   CREATE TABLE IF NOT EXISTS codingSessions(
                                   id INTEGER PRIMARY KEY,
                                   startTime STRING NOT NULL,
                                   endTime STRING NOT NULL,
                                   duration STRING NOT NULL
                                   );
                                   """;
        using var command = new SqliteCommand(createTable, connection);
        command.ExecuteNonQuery();
    }
}