namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Application-wide constants used across the test framework
/// </summary>
public static class Constants
{
    public static class Timeouts
    {
        public const int ShortWait = 5;
        public const int MediumWait = 10;
        public const int LongWait = 30;
        public const int VeryLongWait = 60;
        public const int PollingInterval = 250;
    }

    public static class TestCategories
    {
        public const string Smoke = "Smoke";
        public const string Regression = "Regression";
        public const string Critical = "Critical";
        public const string UI = "UI";
        public const string API = "API";
        public const string Login = "Login";
        public const string Dashboard = "Dashboard";
    }

    public static class DateFormats
    {
        public const string Standard = "yyyy-MM-dd";
        public const string WithTime = "yyyy-MM-dd HH:mm:ss";
        public const string FileTimestamp = "yyyyMMdd_HHmmss";
        public const string ReportTimestamp = "dd-MMM-yyyy HH:mm:ss";
    }

    public static class FileExtensions
    {
        public const string Screenshot = ".png";
        public const string HtmlReport = ".html";
        public const string Log = ".log";
        public const string Json = ".json";
    }

    public static class Messages
    {
        public const string LoginSuccess = "You logged into a secure area!";
        public const string LoginFailure = "Your username is invalid!";
        public const string InvalidPassword = "Your password is invalid!";
        public const string Logout = "You logged out of the secure area!";
    }

    public static class TestData
    {
        public const string ValidUsername = "tomsmith";
        public const string ValidPassword = "SuperSecretPassword!";
        public const string InvalidUsername = "invaliduser";
        public const string InvalidPassword = "wrongpassword";
    }
}
