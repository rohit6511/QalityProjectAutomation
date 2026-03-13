using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using QualityAutomation.Tests.Config;
using Serilog;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Manages ExtentReports for HTML test reporting with screenshot support
/// </summary>
public sealed class ExtentReportManager
{
    private static readonly Lazy<ExtentReportManager> _instance = new(() => new ExtentReportManager());
    private static readonly object _lock = new();
    
    private ExtentReports? _extentReports;
    private readonly ThreadLocal<ExtentTest> _currentTest = new();
    private string _reportPath = string.Empty;

    public static ExtentReportManager Instance => _instance.Value;

    private ExtentReportManager() { }

    public ExtentTest? CurrentTest => _currentTest.Value;

    /// <summary>
    /// Initializes the ExtentReports instance
    /// </summary>
    public void Initialize()
    {
        if (_extentReports != null) return;

        lock (_lock)
        {
            if (_extentReports != null) return;

            var reportSettings = ConfigurationManager.Instance.ReportSettings;
            var reportDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, reportSettings.ReportPath);

            if (!Directory.Exists(reportDirectory))
                Directory.CreateDirectory(reportDirectory);

            var timestamp = DateTime.Now.ToString(Constants.DateFormats.FileTimestamp);
            _reportPath = Path.Combine(reportDirectory, $"{reportSettings.ReportName}_{timestamp}.html");

            var htmlReporter = new ExtentSparkReporter(_reportPath)
            {
                Config =
                {
                    DocumentTitle = reportSettings.ReportTitle,
                    ReportName = reportSettings.ReportTitle,
                    Theme = Theme.Standard,
                    Encoding = "UTF-8"
                }
            };

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(htmlReporter);
            
            AddSystemInfo();

            Log.Information("ExtentReports initialized. Report path: {ReportPath}", _reportPath);
        }
    }

    private void AddSystemInfo()
    {
        var testSettings = ConfigurationManager.Instance.TestSettings;
        
        _extentReports!.AddSystemInfo("Application", "Quality Automation Framework");
        _extentReports.AddSystemInfo("Environment", Environment.GetEnvironmentVariable("TEST_ENV") ?? "Development");
        _extentReports.AddSystemInfo("Browser", testSettings.Browser);
        _extentReports.AddSystemInfo("Base URL", testSettings.BaseUrl);
        _extentReports.AddSystemInfo("OS", Environment.OSVersion.ToString());
        _extentReports.AddSystemInfo(".NET Version", Environment.Version.ToString());
        _extentReports.AddSystemInfo("Machine Name", Environment.MachineName);
        _extentReports.AddSystemInfo("User", Environment.UserName);
        _extentReports.AddSystemInfo("Execution Date", DateTime.Now.ToString(Constants.DateFormats.ReportTimestamp));
    }

    /// <summary>
    /// Creates a new test in the report
    /// </summary>
    public ExtentTest CreateTest(string testName, string? description = null)
    {
        EnsureInitialized();

        lock (_lock)
        {
            var test = _extentReports!.CreateTest(testName, description);
            _currentTest.Value = test;
            Log.Debug("Created ExtentReport test: {TestName}", testName);
            return test;
        }
    }

    /// <summary>
    /// Creates a new test node (for BDD scenarios)
    /// </summary>
    public ExtentTest CreateNode(string nodeName, string? description = null)
    {
        if (_currentTest.Value == null)
            throw new InvalidOperationException("No test has been created. Call CreateTest first.");

        return _currentTest.Value.CreateNode(nodeName, description);
    }

    /// <summary>
    /// Logs a pass status to the current test
    /// </summary>
    public void LogPass(string message)
    {
        _currentTest.Value?.Pass(message);
        Log.Information("[PASS] {Message}", message);
    }

    /// <summary>
    /// Logs a fail status to the current test
    /// </summary>
    public void LogFail(string message, Exception? exception = null)
    {
        if (exception != null)
            _currentTest.Value?.Fail(exception);
        else
            _currentTest.Value?.Fail(message);
        
        Log.Error("[FAIL] {Message}", message);
    }

    /// <summary>
    /// Logs a skip status to the current test
    /// </summary>
    public void LogSkip(string message)
    {
        _currentTest.Value?.Skip(message);
        Log.Warning("[SKIP] {Message}", message);
    }

    /// <summary>
    /// Logs an info message to the current test
    /// </summary>
    public void LogInfo(string message)
    {
        _currentTest.Value?.Info(message);
        Log.Information("[INFO] {Message}", message);
    }

    /// <summary>
    /// Logs a warning message to the current test
    /// </summary>
    public void LogWarning(string message)
    {
        _currentTest.Value?.Warning(message);
        Log.Warning("[WARNING] {Message}", message);
    }

    /// <summary>
    /// Attaches a screenshot to the current test
    /// </summary>
    public void AttachScreenshot(string screenshotPath, string title = "Screenshot")
    {
        if (string.IsNullOrEmpty(screenshotPath) || !File.Exists(screenshotPath))
        {
            Log.Warning("Screenshot file not found: {Path}", screenshotPath);
            return;
        }

        try
        {
            _currentTest.Value?.AddScreenCaptureFromPath(screenshotPath, title);
            Log.Debug("Screenshot attached to report: {Path}", screenshotPath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to attach screenshot to report");
        }
    }

    /// <summary>
    /// Attaches a Base64 screenshot to the current test
    /// </summary>
    public void AttachScreenshotBase64(string base64String, string title = "Screenshot")
    {
        if (string.IsNullOrEmpty(base64String))
        {
            Log.Warning("Base64 screenshot string is empty");
            return;
        }

        try
        {
            _currentTest.Value?.AddScreenCaptureFromBase64String(base64String, title);
            Log.Debug("Base64 screenshot attached to report");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to attach Base64 screenshot to report");
        }
    }

    /// <summary>
    /// Assigns a category/tag to the current test
    /// </summary>
    public void AssignCategory(params string[] categories)
    {
        _currentTest.Value?.AssignCategory(categories);
    }

    /// <summary>
    /// Assigns an author to the current test
    /// </summary>
    public void AssignAuthor(params string[] authors)
    {
        _currentTest.Value?.AssignAuthor(authors);
    }

    /// <summary>
    /// Assigns a device to the current test
    /// </summary>
    public void AssignDevice(params string[] devices)
    {
        _currentTest.Value?.AssignDevice(devices);
    }

    /// <summary>
    /// Flushes the report to disk
    /// </summary>
    public void Flush()
    {
        lock (_lock)
        {
            _extentReports?.Flush();
            Log.Information("ExtentReport flushed to: {ReportPath}", _reportPath);
        }
    }

    /// <summary>
    /// Gets the report file path
    /// </summary>
    public string GetReportPath() => _reportPath;

    private void EnsureInitialized()
    {
        if (_extentReports == null)
            Initialize();
    }
}
