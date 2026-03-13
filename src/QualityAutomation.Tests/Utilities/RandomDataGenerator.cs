using System.Text;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Utility class for generating random test data
/// </summary>
public static class RandomDataGenerator
{
    private static readonly Random Random = new();
    
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    private static readonly string[] FirstNames = 
    {
        "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda",
        "William", "Elizabeth", "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica"
    };

    private static readonly string[] LastNames = 
    {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
        "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas"
    };

    private static readonly string[] Domains = 
    {
        "gmail.com", "yahoo.com", "outlook.com", "hotmail.com", "test.com", "example.com"
    };

    /// <summary>
    /// Generates a random string of specified length
    /// </summary>
    public static string GenerateString(int length, bool includeUppercase = true, 
        bool includeLowercase = true, bool includeDigits = false, bool includeSpecial = false)
    {
        var charPool = new StringBuilder();
        
        if (includeLowercase) charPool.Append(LowercaseLetters);
        if (includeUppercase) charPool.Append(UppercaseLetters);
        if (includeDigits) charPool.Append(Digits);
        if (includeSpecial) charPool.Append(SpecialChars);

        if (charPool.Length == 0)
            charPool.Append(LowercaseLetters);

        var chars = charPool.ToString();
        var result = new StringBuilder(length);
        
        for (int i = 0; i < length; i++)
            result.Append(chars[Random.Next(chars.Length)]);

        return result.ToString();
    }

    /// <summary>
    /// Generates a random alphanumeric string
    /// </summary>
    public static string GenerateAlphanumeric(int length) =>
        GenerateString(length, includeDigits: true);

    /// <summary>
    /// Generates a random number within a range
    /// </summary>
    public static int GenerateNumber(int min = 0, int max = int.MaxValue) =>
        Random.Next(min, max);

    /// <summary>
    /// Generates a random decimal number
    /// </summary>
    public static decimal GenerateDecimal(decimal min = 0, decimal max = 1000, int decimalPlaces = 2)
    {
        var range = (double)(max - min);
        var randomValue = Random.NextDouble() * range + (double)min;
        return Math.Round((decimal)randomValue, decimalPlaces);
    }

    /// <summary>
    /// Generates a random email address
    /// </summary>
    public static string GenerateEmail(string? prefix = null)
    {
        var name = prefix ?? GenerateString(8, includeUppercase: false);
        var timestamp = DateTime.Now.Ticks.ToString()[^6..];
        var domain = Domains[Random.Next(Domains.Length)];
        return $"{name}{timestamp}@{domain}";
    }

    /// <summary>
    /// Generates a random first name
    /// </summary>
    public static string GenerateFirstName() =>
        FirstNames[Random.Next(FirstNames.Length)];

    /// <summary>
    /// Generates a random last name
    /// </summary>
    public static string GenerateLastName() =>
        LastNames[Random.Next(LastNames.Length)];

    /// <summary>
    /// Generates a random full name
    /// </summary>
    public static string GenerateFullName() =>
        $"{GenerateFirstName()} {GenerateLastName()}";

    /// <summary>
    /// Generates a random username
    /// </summary>
    public static string GenerateUsername(int length = 10)
    {
        var firstName = GenerateFirstName().ToLower();
        var suffix = GenerateNumber(100, 999);
        return $"{firstName}{suffix}";
    }

    /// <summary>
    /// Generates a random password meeting common requirements
    /// </summary>
    public static string GeneratePassword(int length = 12, bool requireSpecial = true)
    {
        var password = new StringBuilder();
        
        password.Append(UppercaseLetters[Random.Next(UppercaseLetters.Length)]);
        password.Append(LowercaseLetters[Random.Next(LowercaseLetters.Length)]);
        password.Append(Digits[Random.Next(Digits.Length)]);
        
        if (requireSpecial)
            password.Append(SpecialChars[Random.Next(SpecialChars.Length)]);

        var remainingLength = length - password.Length;
        password.Append(GenerateString(remainingLength, includeDigits: true, includeSpecial: requireSpecial));

        return ShuffleString(password.ToString());
    }

    /// <summary>
    /// Generates a random phone number
    /// </summary>
    public static string GeneratePhoneNumber(string format = "###-###-####")
    {
        var result = new StringBuilder();
        foreach (var c in format)
        {
            result.Append(c == '#' ? Digits[Random.Next(Digits.Length)] : c);
        }
        return result.ToString();
    }

    /// <summary>
    /// Generates a random date within a range
    /// </summary>
    public static DateTime GenerateDate(DateTime? minDate = null, DateTime? maxDate = null)
    {
        var min = minDate ?? DateTime.Now.AddYears(-10);
        var max = maxDate ?? DateTime.Now;
        var range = (max - min).Days;
        return min.AddDays(Random.Next(range));
    }

    /// <summary>
    /// Generates a random past date
    /// </summary>
    public static DateTime GeneratePastDate(int maxYearsAgo = 5) =>
        GenerateDate(DateTime.Now.AddYears(-maxYearsAgo), DateTime.Now.AddDays(-1));

    /// <summary>
    /// Generates a random future date
    /// </summary>
    public static DateTime GenerateFutureDate(int maxYearsAhead = 5) =>
        GenerateDate(DateTime.Now.AddDays(1), DateTime.Now.AddYears(maxYearsAhead));

    /// <summary>
    /// Generates a random GUID string
    /// </summary>
    public static string GenerateGuid() => Guid.NewGuid().ToString();

    /// <summary>
    /// Selects a random item from a collection
    /// </summary>
    public static T SelectRandom<T>(IList<T> items) =>
        items[Random.Next(items.Count)];

    /// <summary>
    /// Generates a random boolean
    /// </summary>
    public static bool GenerateBoolean() => Random.Next(2) == 1;

    /// <summary>
    /// Generates random lorem ipsum text
    /// </summary>
    public static string GenerateLoremIpsum(int wordCount = 50)
    {
        var loremWords = new[]
        {
            "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
            "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
            "magna", "aliqua", "enim", "ad", "minim", "veniam", "quis", "nostrud"
        };

        var result = new StringBuilder();
        for (int i = 0; i < wordCount; i++)
        {
            if (i > 0) result.Append(' ');
            result.Append(loremWords[Random.Next(loremWords.Length)]);
        }
        return result.ToString();
    }

    private static string ShuffleString(string str)
    {
        var array = str.ToCharArray();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return new string(array);
    }
}
