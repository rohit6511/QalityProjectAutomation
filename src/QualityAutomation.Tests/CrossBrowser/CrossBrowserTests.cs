using NUnit.Framework;
using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Drivers;
using QualityAutomation.Tests.Utilities;
using Serilog;

namespace QualityAutomation.Tests.CrossBrowser;

/// <summary>
/// Base class for cross-browser test execution.
/// Tests derived from this class will run on Chrome, Firefox, and Edge in parallel.
/// </summary>
[TestFixtureSource(typeof(BrowserTestFixtureSource))]
[Parallelizable(ParallelScope.Fixtures)]
public abstract class CrossBrowserTestBase
{
    protected BrowserType Browser { get; }
    protected string BrowserName => Browser.ToString();

    protected CrossBrowserTestBase(BrowserType browser)
    {
        Browser = browser;
    }

    [OneTimeSetUp]
    public virtual void CrossBrowserFixtureSetUp()
    {
        CrossBrowserTestExecutor.CurrentBrowser = Browser;
        CrossBrowserTestExecutor.IsCrossBrowserRun = true;
        Log.Information("Cross-browser fixture setup for {Browser}", Browser);
    }

    [OneTimeTearDown]
    public virtual void CrossBrowserFixtureTearDown()
    {
        CrossBrowserTestExecutor.IsCrossBrowserRun = false;
        Log.Information("Cross-browser fixture teardown for {Browser}", Browser);
    }

    [SetUp]
    public virtual void SetUp()
    {
        CrossBrowserTestExecutor.CurrentBrowser = Browser;
        CrossBrowserTestExecutor.IsCrossBrowserRun = true;
        
        Log.Information("Starting test on {Browser}", Browser);
        DriverFactory.InitializeDriver(Browser, ConfigurationManager.Instance.TestSettings.Headless);
    }

    [TearDown]
    public virtual void TearDown()
    {
        try
        {
            var testContext = TestContext.CurrentContext;
            if (testContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotPath = ScreenshotHelper.CaptureScreenshot(
                    $"{testContext.Test.Name}_{Browser}");
                Log.Error("Test failed on {Browser}. Screenshot: {Path}", Browser, screenshotPath);
            }
        }
        finally
        {
            DriverFactory.QuitDriver();
            Log.Information("Completed test on {Browser}", Browser);
        }
    }
}

/// <summary>
/// Provides browser types as test fixture parameters for cross-browser testing
/// </summary>
public class BrowserTestFixtureSource : IEnumerable<BrowserType>
{
    public IEnumerator<BrowserType> GetEnumerator()
    {
        yield return BrowserType.Chrome;
        yield return BrowserType.Firefox;
        yield return BrowserType.Edge;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Provides browser types for TestCaseSource attribute
/// </summary>
public static class BrowserTestCaseSource
{
    public static IEnumerable<BrowserType> AllBrowsers
    {
        get
        {
            yield return BrowserType.Chrome;
            yield return BrowserType.Firefox;
            yield return BrowserType.Edge;
        }
    }

    public static IEnumerable<TestCaseData> AllBrowsersTestCases
    {
        get
        {
            yield return new TestCaseData(BrowserType.Chrome).SetName("{m}_Chrome");
            yield return new TestCaseData(BrowserType.Firefox).SetName("{m}_Firefox");
            yield return new TestCaseData(BrowserType.Edge).SetName("{m}_Edge");
        }
    }
}
