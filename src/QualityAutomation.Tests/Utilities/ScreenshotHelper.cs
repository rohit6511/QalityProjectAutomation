using OpenQA.Selenium;
using QualityAutomation.Tests.Drivers;
using Serilog;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Utility class for capturing and managing screenshots
/// </summary>
public static class ScreenshotHelper
{
    private static readonly string ScreenshotDirectory = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

    static ScreenshotHelper()
    {
        if (!Directory.Exists(ScreenshotDirectory))
            Directory.CreateDirectory(ScreenshotDirectory);
    }

    /// <summary>
    /// Captures a screenshot and saves it to the screenshots directory
    /// </summary>
    public static string CaptureScreenshot(string testName)
    {
        try
        {
            if (!DriverFactory.HasDriver)
            {
                Log.Warning("Cannot capture screenshot: WebDriver is not initialized");
                return string.Empty;
            }

            var timestamp = DateTime.Now.ToString(Constants.DateFormats.FileTimestamp);
            var sanitizedTestName = SanitizeFileName(testName);
            var fileName = $"{sanitizedTestName}_{timestamp}{Constants.FileExtensions.Screenshot}";
            var filePath = Path.Combine(ScreenshotDirectory, fileName);

            var screenshot = ((ITakesScreenshot)DriverFactory.Driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);

            Log.Information("Screenshot captured: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to capture screenshot for test: {TestName}", testName);
            return string.Empty;
        }
    }

    /// <summary>
    /// Captures a screenshot and returns it as a byte array
    /// </summary>
    public static byte[] CaptureScreenshotAsBytes()
    {
        try
        {
            if (!DriverFactory.HasDriver)
            {
                Log.Warning("Cannot capture screenshot: WebDriver is not initialized");
                return Array.Empty<byte>();
            }

            var screenshot = ((ITakesScreenshot)DriverFactory.Driver).GetScreenshot();
            return screenshot.AsByteArray;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to capture screenshot as bytes");
            return Array.Empty<byte>();
        }
    }

    /// <summary>
    /// Captures a screenshot and returns it as a Base64 string
    /// </summary>
    public static string CaptureScreenshotAsBase64()
    {
        try
        {
            if (!DriverFactory.HasDriver)
            {
                Log.Warning("Cannot capture screenshot: WebDriver is not initialized");
                return string.Empty;
            }

            var screenshot = ((ITakesScreenshot)DriverFactory.Driver).GetScreenshot();
            return screenshot.AsBase64EncodedString;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to capture screenshot as Base64");
            return string.Empty;
        }
    }

    /// <summary>
    /// Captures a screenshot of a specific element
    /// </summary>
    public static string CaptureElementScreenshot(IWebElement element, string name)
    {
        try
        {
            var timestamp = DateTime.Now.ToString(Constants.DateFormats.FileTimestamp);
            var sanitizedName = SanitizeFileName(name);
            var fileName = $"Element_{sanitizedName}_{timestamp}{Constants.FileExtensions.Screenshot}";
            var filePath = Path.Combine(ScreenshotDirectory, fileName);

            var screenshot = ((ITakesScreenshot)element).GetScreenshot();
            screenshot.SaveAsFile(filePath);

            Log.Information("Element screenshot captured: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to capture element screenshot: {Name}", name);
            return string.Empty;
        }
    }

    /// <summary>
    /// Cleans up old screenshots based on retention days
    /// </summary>
    public static void CleanupOldScreenshots(int retentionDays = 7)
    {
        try
        {
            var cutoffDate = DateTime.Now.AddDays(-retentionDays);
            var files = Directory.GetFiles(ScreenshotDirectory, $"*{Constants.FileExtensions.Screenshot}");

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < cutoffDate)
                {
                    File.Delete(file);
                    Log.Debug("Deleted old screenshot: {File}", file);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to cleanup old screenshots");
        }
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        return sanitized.Length > 100 ? sanitized[..100] : sanitized;
    }
}
