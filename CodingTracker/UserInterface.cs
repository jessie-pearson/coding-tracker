using System.Globalization;
using Spectre.Console;

namespace CodingTracker;

internal record struct SelectionOption(int Id, string Msg);

internal static class UserInterface
{
    private static void ConsoleTable (params CodingSession[] codingSessions)
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

    internal static SelectionOption ConsoleSelection(string title, SelectionOption[] options)
    {
        var selectionPrompt = new SelectionPrompt<SelectionOption>()
            .Title($"[green][bold]{title}[/][/]")
            .UseConverter(o => $"[bold]{o.Id}[/] - {o.Msg}")
            .WrapAround()
            .AddChoices(options);

        return AnsiConsole.Prompt(selectionPrompt);
    }
    
    internal static void Menu(DatabaseController db)
    {
        var options = new[]
        {
            new SelectionOption {Id = 1, Msg = "Add a coding session"},
            new SelectionOption {Id = 2, Msg = "View your coding sessions"},
            new SelectionOption {Id = 3, Msg = "Update a coding session"},
            new SelectionOption {Id = 4, Msg = "Delete a coding session"},
            new SelectionOption {Id = 5, Msg = "Exit the program"},
        };
        
        while (true)
        {
            var option = ConsoleSelection("CODING SESSION TRACKER", options);
            switch (option.Id)
            {
                case 1:
                    var codingSession = Handlers.Insert(db);
                    if (codingSession is not null)
                    {
                        ConsoleTable(codingSession);
                    }
                    break;
                case 2: 
                    var codingSessions = Handlers.Get(db);
                    if (codingSessions.Length > 0)
                    {
                        ConsoleTable(codingSessions);
                    }
                    break;
                case 3:
                    var updatedCodingSession = Handlers.Update(db);
                    if (updatedCodingSession is not null)
                    {
                        ConsoleTable(updatedCodingSession);
                    }
                    break;
                case 4:
                    var deletedCodingSession = Handlers.Delete(db);
                    if (deletedCodingSession is not null)
                    {
                        ConsoleTable(deletedCodingSession);
                    }
                    break;
                case 5:
                    AnsiConsole.MarkupLine("[green]Goodbye![/]");
                    return;
            }

            AnsiConsole.MarkupLine("Press [blue]'Enter'[/] to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}