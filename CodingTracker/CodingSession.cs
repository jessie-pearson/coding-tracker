namespace CodingTracker;

internal class CodingSession(int id, DateTime startTime, DateTime endTime, double duration)
{
    internal readonly int Id  = id;
    internal readonly DateTime StartTime  = startTime;
    internal readonly DateTime EndTime = endTime;
    internal readonly double Duration = duration;
}

internal class CodingSessionAddDto(DateTime startTime, DateTime endTime, double duration)
{
    internal readonly DateTime StartTime = startTime;
    internal readonly DateTime EndTime = endTime;
    internal readonly double Duration = duration;
}

internal enum DatePeriod
{
    Before,
    After
}

internal struct CodingSessionFilters(DateTime date, DatePeriod datePeriod, string orderBy)
{
    internal DateTime? Date { get; set; } = date;
    internal DatePeriod DatePeriod { get; set; } = datePeriod;
    internal string OrderBy { get; set; } = orderBy;
}