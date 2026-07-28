namespace CodingTracker.Database;

internal class CodingSession(int id, DateTime startTime, DateTime endTime, DateTime duration)
{
    internal DateTime Duration = duration;
    internal DateTime EndTime = endTime;
    internal DateTime StartTime = startTime;
    internal int Id { get; } = id;
}

internal class AddCodingSessionDto(DateTime startTime, DateTime endTime, DateTime duration)
{
    internal DateTime Duration = duration;
    internal DateTime EndTime = endTime;
    internal DateTime StartTime = startTime;
}