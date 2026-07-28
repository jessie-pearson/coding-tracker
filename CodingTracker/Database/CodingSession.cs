namespace coding_tracker.Database;

internal class CodingSession(int id, DateTime startTime, DateTime endTime, DateTime duration)
{
    internal int Id {get;} = id;
    internal DateTime StartTime = startTime;
    internal DateTime EndTime = endTime;
    internal DateTime Duration = duration;
}

internal class AddCodingSessionDto(DateTime startTime, DateTime endTime, DateTime duration)
{
    internal DateTime StartTime = startTime;
    internal DateTime EndTime = endTime;
    internal DateTime Duration = duration;
}