using Microsoft.Data.Sqlite;

namespace CodingTracker;

internal class DatabaseController(string connectionString)
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
                                    duration DOUBLE NOT NULL,
                                    UNIQUE(startTime, endTime)     
                                   );
                                   """;
        using var command = new SqliteCommand(createTable, connection);
        command.ExecuteNonQuery();
    }
    
    internal void SeedTable()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string seedCmd = """
                               INSERT INTO codingSessions(startTime, endTime, duration)
                               VALUES
                                   ("2026-07-25 04:00", "2026-07-25 08:00", 4.000000),
                                   ("2026-07-25 15:00", "2026-07-25 18:00", 3.000000),
                                   ("2026-07-26 04:00", "2026-07-26 08:00", 4.000000),
                                   ("2026-07-26 15:00", "2026-07-26 18:00", 3.000000),
                                   ("2026-07-27 04:00", "2026-07-27 08:00", 4.000000),
                                   ("2026-07-27 15:00", "2026-07-27 18:00", 3.000000),
                                   ("2026-07-28 04:00", "2026-07-28 08:00", 4.000000),
                                   ("2026-07-28 15:00", "2026-07-28 18:00", 3.000000)
                               ;
                               """;
        
        using var command = new SqliteCommand(seedCmd, connection);
        command.ExecuteNonQuery();
    }
    
    internal CodingSession? InsertCodingSession(CodingSessionAddDto codingSession)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string insertCmd = """
                                 INSERT INTO codingSessions(startTime, endTime, duration)
                                 VALUES(@StartTime, @EndTime, @Duration)
                                 RETURNING *;
                                 """;
        
        using var command = new SqliteCommand(insertCmd, connection);
        command.Parameters.AddWithValue("@StartTime", codingSession.StartTime);
        command.Parameters.AddWithValue("@EndTime", codingSession.EndTime);
        command.Parameters.AddWithValue("@Duration", codingSession.Duration);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var id = reader.GetInt32(0);
        var startTime = reader.GetDateTime(1);
        var endTime = reader.GetDateTime(2);
        var duration = reader.GetDouble(3);

        return new CodingSession(id, startTime, endTime, duration);
    }
    
    internal CodingSession[] GetCodingSessions(CodingSessionFilters filters)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string getCommand = "SELECT * FROM codingSessions WHERE 1=1 ";
        using var command = new SqliteCommand(getCommand, connection);
        
        if (filters.Date is not null)
        {
            command.CommandText += filters.DatePeriod == DatePeriod.Before
                ? "AND startTime < @Date "
                : "AND startTime > @Date ";
            command.Parameters.AddWithValue("@Date", filters.Date);
        }
        
        command.CommandText += $"ORDER BY startTime {filters.OrderBy}; duration {filters.OrderBy}";

        var reader = command.ExecuteReader();
        var codingSessions = new List<CodingSession>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var startDate = reader.GetDateTime(1);
            var endDate = reader.GetDateTime(2);
            var duration = reader.GetDouble(3);
            codingSessions.Add(new CodingSession(id, startDate, endDate, duration));
        }

        return [.. codingSessions];
    }

    internal CodingSession? UpdateCodingSession(int id, CodingSessionAddDto codingSessionAddDto )
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string updateCmd = """
                                 UPDATE codingSessions
                                 SET startTime = @StartTime,
                                     endTime = @EndTime,
                                     duration = @Duration
                                 WHERE id = @Id
                                 RETURNING *;
                                 """;
        using var command = new SqliteCommand(updateCmd, connection);
        command.Parameters.AddWithValue("@StartTime", codingSessionAddDto.StartTime);
        command.Parameters.AddWithValue("@EndTime", codingSessionAddDto.EndTime);
        command.Parameters.AddWithValue("@Duration", codingSessionAddDto.Duration);
        command.Parameters.AddWithValue("@Id", id);

        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        var updatedId = reader.GetInt32(0);
        var updatedStartTime = reader.GetDateTime(1);
        var updatedEndTime = reader.GetDateTime(2);
        var duration = reader.GetDouble(3);

        return new CodingSession(updatedId, updatedStartTime, updatedEndTime, duration);
    }
    
    internal CodingSession? DeleteCodingSession(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        const string deleteCmd = """
                                 DELETE FROM codingSessions
                                 WHERE id = @id
                                 RETURNING *;
                                 """;
        using var command = new SqliteCommand(deleteCmd, connection);
        command.Parameters.AddWithValue("@id", id);
        
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var deletedId = reader.GetInt32(0);
        var startTime = reader.GetDateTime(1);
        var endTime = reader.GetDateTime(2);
        var duration = reader.GetDouble(3);

        return new CodingSession(deletedId, startTime, endTime, duration);
    }
}