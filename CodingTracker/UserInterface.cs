using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

internal static class UserInterface
{
    private static void LogTable (params CodingSession[] codingSessions)
    {
        var table = new Table()
            .AddColumn("ID")
            .AddColumn("Start Time")
            .AddColumn("End Time")
            .AddColumn("Duration");
        
        foreach (var session in codingSessions)
        {
            var id = session.Id.ToString();
            var startTime = session.StartTime.ToString(CultureInfo.CurrentCulture);
            var endTime = session.EndTime.ToString(CultureInfo.CurrentCulture);
            var duration = session.Duration.ToString(CultureInfo.CurrentCulture);
            
            table.AddRow(id, startTime, endTime, duration);
        }

        AnsiConsole.Write(table);
    }
    
    internal static void Menu(DatabaseController db)
    {
        while (true)
        {
            const string message = """
                                   CODING SESSION TRACKER
                                   ______________________________
                                   Enter an option from the menu:
                                    1. Add a coding session,
                                    2. View your coding sessions,
                                    3. Update a coding session,
                                    4. Delete a coding session,
                                    5. Exit the program
                                   """;
            
            var option = Validate.GetValidInteger(message, 1, 5);
            Console.Clear();
            
            switch (option)
            {
                case 1:
                    var codingSession = Handlers.Insert(db);
                    if (codingSession is not null)
                    {
                        LogTable(codingSession);
                    }
                    break;
                case 2: 
                    var codingSessions = Handlers.Get(db);
                    if (codingSessions.Length > 0)
                    {
                        LogTable(codingSessions);
                    }
                    break;
                case 3:
                    var updatedCodingSession = Handlers.Update(db);
                    if (updatedCodingSession is not null)
                    {
                        LogTable(updatedCodingSession);
                    }
                    break;
                case 4:
                    var deletedCodingSession = Handlers.Delete(db);
                    if (deletedCodingSession is not null)
                    {
                        LogTable(deletedCodingSession);
                    }
                    break;
                case 5:
                    Console.WriteLine("Goodbye!");
                    return;
            }

            Console.WriteLine("Press 'Enter' to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}