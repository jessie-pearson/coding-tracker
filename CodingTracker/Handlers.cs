namespace CodingTracker;

internal static class Handlers
{
    private const string DateTimeFormat = "dd/MM/yy HH:mm";
    
    internal static CodingSession? Insert(DatabaseController db)
    {
        Console.WriteLine("Enter the dates and times.");
        Console.WriteLine("Use the following format: dd/mm/yy hh:mm");
        
        var startTime = Validate.GetValidDateInput("Enter the start time:", DateTimeFormat);
        var endTime = Validate.GetValidDateInput("Enter the end time:", DateTimeFormat);
        
        if (!Validate.IsValidTimeRange(startTime, endTime))
        {
            Console.Error.WriteLine("Error: end time can't be before start time");
            return null;
        }
        
        var duration = Math.Round((endTime - startTime).TotalHours, 2);
        
        var codingSessionAddDto = new CodingSessionAddDto(startTime, endTime, duration);

        return db.InsertCodingSession(codingSessionAddDto);
    }
    
    internal static CodingSession[] Get(DatabaseController db)
    {
        var filters = new CodingSessionFilters();
        
        const string dateMessage = """
                               Choose the coding sessions you want to view:
                                1. All coding sessions,
                                2. Coding sessions before a date,
                                3. Coding sessions after a date.
                               """;
        var dateOption = Validate.GetValidInteger(dateMessage, 1, 3);
        
        if (dateOption is 2 or 3)
        {
            filters.Date = Validate.GetValidDateInput(
                "Enter a date and or time\nUse the following format: dd/mm/yy hh:mm", DateTimeFormat);
            filters.DatePeriod = dateOption == 2 ? DatePeriod.Before : DatePeriod.After;
        }
        
        const string orderByMessage = """
                                      Choose the sort order:
                                        1. Ascending
                                        2. Descending
                                      """;
        var orderByOption = Validate.GetValidInteger(orderByMessage, 1, 2);
        filters.OrderBy = orderByOption == 1 ? "ASC" : "DESC";

        return db.GetCodingSessions(filters);
    }

    internal static CodingSession? Update(DatabaseController db)
    {
        var id = Validate.GetValidInteger("Enter the id of the coding session you want to update");
        
        Console.WriteLine("Enter the updated dates and times\nUse the following format dd/mm/yy hh:mm");
        var startTime = Validate.GetValidDateInput("Enter the start date and time:", DateTimeFormat);
        var endTime = Validate.GetValidDateInput("Enter the end date and time:", DateTimeFormat);
        
        if (!Validate.IsValidTimeRange(startTime, endTime))
        {
            Console.Error.WriteLine("Error: end time can't be before start time");
            return null;
        }
        var duration = Math.Round((endTime - startTime).TotalHours, 2);

        return db.UpdateCodingSession(id, new CodingSessionAddDto(startTime, endTime, duration));
    }
    
    internal static CodingSession? Delete(DatabaseController db)
    {
        var id = Validate.GetValidInteger("Enter the ID of the coding session you want to delete:");
        return db.DeleteCodingSession(id);
    }
}