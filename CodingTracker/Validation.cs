using System.Globalization;

namespace CodingTracker;

internal static class Validate
{
    internal static int GetValidInteger(string message, int min = 0, int max = 0)
    {
        Console.WriteLine(message);
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out var result))
            {
                Console.WriteLine("Input is not a valid integer, try again");
                continue;
            }

            if (result >= min || result <= max)
            {
                return result;
            }
            
            Console.WriteLine("Input is out of range, try again");
        }
    }
    
    internal static DateTime GetValidDateInput(string message, string format)
    {
        Console.WriteLine(message);
        while (true)
        {
            var input = Console.ReadLine();
            if (!DateTime.TryParseExact(
                    input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                Console.WriteLine("Invalid date time format, try again.");
                continue;
            }

            if (result <= DateTime.Now)
            {
                return result; 
            }
            
            Console.WriteLine("Input can't be greater than the current date and time");
        }
    }
    
    internal static bool IsValidTimeRange(DateTime startDate, DateTime endDate)
    {
        return startDate < endDate;
    }
}