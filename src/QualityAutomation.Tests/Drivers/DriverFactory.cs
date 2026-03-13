using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using QualityAutomation.Tests.Config;
using Serilog;

namespace QualityAutomation.Tests.Drivers;

/// <summary>
/// Factory class for creating and managing WebDriver instances.
/// Supports Chrome, Firefox, and Edge browsers with parallel execution via ThreadLocal.
/// </summary>
public sealed class DriverFactory
{
    private static readonly ThreadLocal<IWebDriver?> _driver = new();
    private static readonly ThreadLocal<BrowserType> _currentBrowserType = new();
    private static readonly object _lock = new();

    private DriverFactory() { }

    public static IWebDriver Driver
    {
        get
        {
            if (_driver.Value == null)
                throw new InvalidOperationException("WebDriver has not been initialized. Call InitializeDriver first.");
            return _driver.Value;
        }
    }

    public static bool HasDriver => _driver.Value != null;

    /// <summary>
    /// Initializes the WebDriver based on configuration settings or cross-browser context
    /// </summary>
    public static IWebDriver InitializeDriver()
    {
        var settings = ConfigurationManager.Instance.TestSettings;
        var browserType = CrossBrowserTestExecutor.GetBrowserToUse();
        return InitializeDriver(browserType, settings.Headless);
    }

    /// <summary>
    /// Initializes the WebDriver for a specific browser type
    /// </summary>
    public static IWebDriver InitializeDriver(BrowserType browserType, bool headless = false)
    {
        if (_driver.Value != null)
        {
            Log.Warning("Driver already initialized for this thread. Disposing existing driver.");
            QuitDriver();
        }

        Log.Information("Initializing {BrowserType} driver (Headless: {Headless})", browserType, headless);

        lock (_lock)
        {
            _driver.Value = browserType switch
            {
                BrowserType.Chrome => CreateChromeDriver(headless),
                BrowserType.Firefox => CreateFirefoxDriver(headless),
                BrowserType.Edge => CreateEdgeDriver(headless),
                _ => throw new ArgumentException($"Unsupported browser type: {browserType}")
            };

            _currentBrowserType.Value = browserType;
        }

        ConfigureDriver(_driver.Value);
        Log.Information("{BrowserType} driver initialized successfully", browserType);
        
        return _driver.Value;
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();
        ConfigureChromeOptions(options, headless);
        
        // Selenium 4.6+ includes Selenium Manager which auto-downloads drivers
        return new ChromeDriver(options);
    }

    private static void ConfigureChromeOptions(ChromeOptions options, bool headless)
    {
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--ignore-certificate-errors");
        
        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }

        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);
    }

    private static IWebDriver CreateFirefoxDriver(bool headless)
    {
        var options = new FirefoxOptions();
        ConfigureFirefoxOptions(options, headless);
        
        // Selenium 4.6+ includes Selenium Manager which auto-downloads drivers
        return new FirefoxDriver(options);
    }

    private static void ConfigureFirefoxOptions(FirefoxOptions options, bool headless)
    {
        options.AddArgument("--width=1920");
        options.AddArgument("--height=1080");
        
        if (headless)
        {
            options.AddArgument("--headless");
        }

        options.SetPreference("browser.download.folderList", 2);
        options.SetPreference("browser.helperApps.neverAsk.saveToDisk", 
            "application/pdf,application/octet-stream,application/x-pdf");
    }

    private static IWebDriver CreateEdgeDriver(bool headless)
    {
        var options = new EdgeOptions();
        ConfigureEdgeOptions(options, headless);
        
        // Selenium 4.6+ includes Selenium Manager which auto-downloads drivers
        return new EdgeDriver(options);
    }

    private static void ConfigureEdgeOptions(EdgeOptions options, bool headless)
    {
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--ignore-certificate-errors");
        
        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }
    }

    private static void ConfigureDriver(IWebDriver driver)
    {
        var settings = ConfigurationManager.Instance.TestSettings;
        
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settings.ImplicitWaitSeconds);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(settings.PageLoadTimeoutSeconds);
        driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
        
        try
        {
            driver.Manage().Window.Maximize();
        }
        catch (Exception ex)
        {
            Log.Warning("Could not maximize window: {Message}", ex.Message);
        }
    }

    /// <summary>
    /// Quits the WebDriver and cleans up resources
    /// </summary>
    public static void QuitDriver()
    {
        if (_driver.Value == null) return;

        try
        {
            Log.Information("Quitting {BrowserType} driver", _currentBrowserType.Value);
            _driver.Value.Quit();
            _driver.Value.Dispose();
        }
        catch (Exception ex)
        {
            Log.Warning("Error while quitting driver: {Message}", ex.Message);
        }
        finally
        {
            _driver.Value = null;
        }
    }

    /// <summary>
    /// Gets the current browser type
    /// </summary>
    public static BrowserType CurrentBrowserType => _currentBrowserType.Value;
}
