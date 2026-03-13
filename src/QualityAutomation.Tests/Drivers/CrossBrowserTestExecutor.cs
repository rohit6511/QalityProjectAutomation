using NUnit.Framework;
using QualityAutomation.Tests.Config;
using Serilog;

namespace QualityAutomation.Tests.Drivers;

/// <summary>
/// Provides cross-browser test execution capability.
/// Stores the current browser for each test thread.
/// </summary>
public static class CrossBrowserTestExecutor
{
    private static readonly ThreadLocal<BrowserType> _currentBrowser = new(() => BrowserType.Chrome);
    private static readonly ThreadLocal<bool> _isCrossBrowserRun = new(() => false);

    /// <summary>
    /// Gets or sets the browser type for the current test thread
    /// </summary>
    public static BrowserType CurrentBrowser
    {
        get => _currentBrowser.Value;
        set => _currentBrowser.Value = value;
    }

    /// <summary>
    /// Indicates if the current run is a cross-browser test run
    /// </summary>
    public static bool IsCrossBrowserRun
    {
        get => _isCrossBrowserRun.Value;
        set => _isCrossBrowserRun.Value = value;
    }

    /// <summary>
    /// Gets all browsers to test against
    /// </summary>
    public static BrowserType[] AllBrowsers => new[]
    {
        BrowserType.Chrome,
        BrowserType.Firefox,
        BrowserType.Edge
    };

    /// <summary>
    /// Gets the browser to use - from cross-browser context or configuration
    /// </summary>
    public static BrowserType GetBrowserToUse()
    {
        if (IsCrossBrowserRun)
        {
            return CurrentBrowser;
        }

        var settings = ConfigurationManager.Instance.TestSettings;
        return Enum.Parse<BrowserType>(settings.Browser, ignoreCase: true);
    }
}
