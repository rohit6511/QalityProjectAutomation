using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Drivers;
using QualityAutomation.Tests.Utilities;
using Reqnroll;
using Serilog;

namespace QualityAutomation.Tests.Hooks;

/// <summary>
/// Reqnroll (SpecFlow) hooks for test setup and teardown.
/// Manages WebDriver lifecycle, logging, and reporting.
/// </summary>
[Binding]
public class TestHooks
{
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;

    public TestHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
    }

    /// <summary>
    /// Runs once before all tests - initializes logging and reporting
    /// </summary>
    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        LoggerSetup.Initialize();
        Log.Information("========================================");
        Log.Information("Test Run Started: {DateTime}", DateTime.Now);
        Log.Information("========================================");
        
        ExtentReportManager.Instance.Initialize();
        ScreenshotHelper.CleanupOldScreenshots();
    }

    /// <summary>
    /// Runs once after all tests - finalizes reporting and logging
    /// </summary>
    [AfterTestRun]
    public static void AfterTestRun()
    {
        Log.Information("========================================");
        Log.Information("Test Run Completed: {DateTime}", DateTime.Now);
        Log.Information("========================================");
        
        ExtentReportManager.Instance.Flush();
        
        var reportPath = ExtentReportManager.Instance.GetReportPath();
        Log.Information("Test Report generated at: {ReportPath}", reportPath);
        
        LoggerSetup.CloseAndFlush();
    }

    /// <summary>
    /// Runs before each feature
    /// </summary>
    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featureContext)
    {
        Log.Information("----------------------------------------");
        Log.Information("Starting Feature: {FeatureTitle}", featureContext.FeatureInfo.Title);
        Log.Information("----------------------------------------");
    }

    /// <summary>
    /// Runs after each feature
    /// </summary>
    [AfterFeature]
    public static void AfterFeature(FeatureContext featureContext)
    {
        Log.Information("Completed Feature: {FeatureTitle}", featureContext.FeatureInfo.Title);
    }

    /// <summary>
    /// Runs before each scenario - initializes WebDriver and creates test in report
    /// </summary>
    [BeforeScenario(Order = 0)]
    public void BeforeScenario()
    {
        var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
        var featureTitle = _featureContext.FeatureInfo.Title;
        
        Log.Information("Starting Scenario: {ScenarioTitle}", scenarioTitle);
        
        ExtentReportManager.Instance.CreateTest($"{featureTitle}: {scenarioTitle}");
        
        var tags = _scenarioContext.ScenarioInfo.Tags;
        if (tags.Length > 0)
        {
            ExtentReportManager.Instance.AssignCategory(tags);
            Log.Debug("Scenario tags: {Tags}", string.Join(", ", tags));
        }
        
        DriverFactory.InitializeDriver();
        ExtentReportManager.Instance.LogInfo($"Browser: {ConfigurationManager.Instance.TestSettings.Browser}");
        ExtentReportManager.Instance.LogInfo($"Base URL: {ConfigurationManager.Instance.TestSettings.BaseUrl}");
    }

    /// <summary>
    /// Runs after each scenario - captures screenshot on failure and quits WebDriver
    /// </summary>
    [AfterScenario(Order = 0)]
    public void AfterScenario()
    {
        var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
        
        try
        {
            if (_scenarioContext.TestError != null)
            {
                HandleScenarioFailure(scenarioTitle);
            }
            else
            {
                HandleScenarioSuccess(scenarioTitle);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in AfterScenario hook");
        }
        finally
        {
            DriverFactory.QuitDriver();
            Log.Information("Completed Scenario: {ScenarioTitle}", scenarioTitle);
        }
    }

    private void HandleScenarioFailure(string scenarioTitle)
    {
        var error = _scenarioContext.TestError;
        Log.Error(error, "Scenario Failed: {ScenarioTitle}", scenarioTitle);
        
        ExtentReportManager.Instance.LogFail($"Scenario failed: {error?.Message}", error);
        
        if (ConfigurationManager.Instance.TestSettings.ScreenshotOnFailure)
        {
            CaptureAndAttachScreenshot(scenarioTitle);
        }
    }

    private void HandleScenarioSuccess(string scenarioTitle)
    {
        Log.Information("Scenario Passed: {ScenarioTitle}", scenarioTitle);
        ExtentReportManager.Instance.LogPass("Scenario passed successfully");
    }

    private void CaptureAndAttachScreenshot(string scenarioTitle)
    {
        try
        {
            var screenshotPath = ScreenshotHelper.CaptureScreenshot(scenarioTitle);
            if (!string.IsNullOrEmpty(screenshotPath))
            {
                ExtentReportManager.Instance.AttachScreenshot(screenshotPath, "Failure Screenshot");
                Log.Information("Screenshot captured for failed scenario: {Path}", screenshotPath);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to capture screenshot for scenario: {ScenarioTitle}", scenarioTitle);
        }
    }

    /// <summary>
    /// Runs before each step
    /// </summary>
    [BeforeStep]
    public void BeforeStep()
    {
        var stepInfo = _scenarioContext.StepContext.StepInfo;
        Log.Debug("Executing Step: {StepType} {StepText}", stepInfo.StepDefinitionType, stepInfo.Text);
    }

    /// <summary>
    /// Runs after each step
    /// </summary>
    [AfterStep]
    public void AfterStep()
    {
        var stepInfo = _scenarioContext.StepContext.StepInfo;
        var stepText = $"{stepInfo.StepDefinitionType} {stepInfo.Text}";

        if (_scenarioContext.TestError == null)
        {
            ExtentReportManager.Instance.LogPass(stepText);
        }
        else
        {
            ExtentReportManager.Instance.LogFail(stepText);
        }
    }
}
