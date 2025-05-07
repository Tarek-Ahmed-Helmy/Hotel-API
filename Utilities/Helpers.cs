namespace Utilities;

public class Helpers
{
    public static string GenerateRequestCode()
    {
        var now = DateTime.Now;
        var random = new Random();

        // Generate a random uppercase letter (A-Z)
        char randomChar = (char)random.Next('A', 'Z' + 1);

        // Last two digits of the year
        string year = now.Year.ToString().Substring(2);

        // Ensure month and day are always 2 digits
        string month = now.Month.ToString("D2");
        string day = now.Day.ToString("D2");

        // Get 3 digits from current time (e.g., milliseconds or ticks)
        string timePart = now.ToString("fff").Substring(0, 3); // first 3 digits of milliseconds

        string formattedCode = $"#{randomChar}{year}{month}{day}{timePart}";
        return formattedCode;
    }
}
