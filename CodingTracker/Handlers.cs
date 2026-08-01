namespace CodingTracker;

internal static class Handlers
{
    private const string DateTimeFormat = "dd/MM/yy HH:mm";
    private const string DateFormat = "dd/MM/yy";
    
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
        var dateOptions = new[]
        {
            new SelectionOption { Id = 1, Msg = "Add coding sessions" },
            new SelectionOption { Id = 2, Msg = "Coding sessions after a date" },
            new SelectionOption { Id = 3, Msg = "Coding sessions before a date" }
        };
        
        var dateOption = UserInterface.ConsoleSelection("Choose the coding sessions you want to view:", dateOptions);
        
        if (dateOption.Id is 2 or 3)
        {
            filters.Date = Validate.GetValidDateInput(
                "Enter the date\nUse the following format: dd/mm/yy", DateFormat);
            filters.DatePeriod = dateOption.Id == 2 ? DatePeriod.Before : DatePeriod.After;
        }

        var orderByOptions = new[]
        {
            new SelectionOption { Id = 1, Msg = "Ascending" },
            new SelectionOption { Id = 2, Msg = "Descending" }
        };

        var orderByOption = UserInterface.ConsoleSelection("Choose the sort order:", orderByOptions);
        filters.OrderBy = orderByOption.Id == 1 ? "ASC" : "DESC";

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