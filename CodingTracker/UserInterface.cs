namespace CodingTracker;

internal static class UserInterface
{
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
                    break;
                case 2:
                    var codingSessions = Handlers.Get(db);
                    break;
                case 3:
                    var updatedCodingSession = Handlers.Update(db);
                    break;
                case 4:
                    var deletedCodingSession = Handlers.Delete(db);
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