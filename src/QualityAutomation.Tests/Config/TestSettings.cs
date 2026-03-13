namespace QualityAutomation.Tests.Config;

/// <summary>
/// Represents the test configuration settings loaded from appsettings.json
/// </summary>
public class TestSettings
{
    public string BaseUrl { get; set; } = "https://uat.qality.dev/";
    public string Browser { get; set; } = "Chrome";
    public bool Headless { get; set; } = false;
    public int ImplicitWaitSeconds { get; set; } = 10;
    public int ExplicitWaitSeconds { get; set; } = 30;
    public int PageLoadTimeoutSeconds { get; set; } = 60;
    public bool ScreenshotOnFailure { get; set; } = true;
    public int MaxRetryAttempts { get; set; } = 2;
}

/// <summary>
/// Represents the report configuration settings
/// </summary>
public class ReportSettings
{
    public string ReportPath { get; set; } = "Reports";
    public string ReportTitle { get; set; } = "Quality Automation Test Report";
    public string ReportName { get; set; } = "TestExecutionReport";
}

/// <summary>
/// Represents the logging configuration settings
/// </summary>
public class LogSettings
{
    public string LogPath { get; set; } = "Logs";
    public string LogLevel { get; set; } = "Information";
}

/// <summary>
/// Root configuration class that aggregates all settings sections
/// </summary>
public class AppSettings
{
    public TestSettings TestSettings { get; set; } = new();
    public ReportSettings ReportSettings { get; set; } = new();
    public LogSettings LogSettings { get; set; } = new();
}
