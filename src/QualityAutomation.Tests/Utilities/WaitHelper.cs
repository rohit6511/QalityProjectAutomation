using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Drivers;
using Serilog;
using SeleniumExtras.WaitHelpers;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Provides reusable explicit wait methods for Selenium WebDriver operations
/// </summary>
public static class WaitHelper
{
    private static int DefaultTimeout => ConfigurationManager.Instance.TestSettings.ExplicitWaitSeconds;
    private static IWebDriver Driver => DriverFactory.Driver;

    /// <summary>
    /// Creates a WebDriverWait instance with the specified timeout
    /// </summary>
    public static WebDriverWait CreateWait(int timeoutSeconds = 0)
    {
        var timeout = timeoutSeconds > 0 ? timeoutSeconds : DefaultTimeout;
        return new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout))
        {
            PollingInterval = TimeSpan.FromMilliseconds(Constants.Timeouts.PollingInterval)
        };
    }

    /// <summary>
    /// Waits for an element to be visible on the page
    /// </summary>
    public static IWebElement WaitForElementVisible(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for element to be visible: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    /// <summary>
    /// Waits for an element to be clickable
    /// </summary>
    public static IWebElement WaitForElementClickable(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for element to be clickable: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    /// <summary>
    /// Waits for an element to be present in the DOM
    /// </summary>
    public static IWebElement WaitForElementExists(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for element to exist: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.ElementExists(locator));
    }

    /// <summary>
    /// Waits for an element to become invisible or not present
    /// </summary>
    public static bool WaitForElementInvisible(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for element to be invisible: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    /// <summary>
    /// Waits for multiple elements to be present
    /// </summary>
    public static IReadOnlyCollection<IWebElement> WaitForElementsPresent(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for elements to be present: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
    }

    /// <summary>
    /// Waits for element to contain specific text
    /// </summary>
    public static bool WaitForTextPresent(By locator, string text, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for text '{Text}' in element: {Locator}", text, locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
    }

    /// <summary>
    /// Waits for the URL to contain a specific string
    /// </summary>
    public static bool WaitForUrlContains(string urlPart, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for URL to contain: {UrlPart}", urlPart);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.UrlContains(urlPart));
    }

    /// <summary>
    /// Waits for the page title to contain specific text
    /// </summary>
    public static bool WaitForTitleContains(string titlePart, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for title to contain: {TitlePart}", titlePart);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.TitleContains(titlePart));
    }

    /// <summary>
    /// Waits for an alert to be present
    /// </summary>
    public static IAlert WaitForAlert(int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for alert to be present");
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.AlertIsPresent());
    }

    /// <summary>
    /// Waits for a frame to be available and switches to it
    /// </summary>
    public static IWebDriver WaitForFrameAndSwitch(By locator, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for frame and switching: {Locator}", locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(locator));
    }

    /// <summary>
    /// Waits for element attribute to contain a value
    /// </summary>
    public static bool WaitForAttributeContains(By locator, string attribute, string value, int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for attribute '{Attribute}' to contain '{Value}' in: {Locator}", 
            attribute, value, locator);
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(d =>
        {
            var element = d.FindElement(locator);
            var attributeValue = element.GetAttribute(attribute);
            return attributeValue?.Contains(value) ?? false;
        });
    }

    /// <summary>
    /// Waits for JavaScript to complete (document.readyState == 'complete')
    /// </summary>
    public static bool WaitForPageLoad(int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for page to load completely");
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(d => ((IJavaScriptExecutor)d)
            .ExecuteScript("return document.readyState").Equals("complete"));
    }

    /// <summary>
    /// Waits for jQuery AJAX calls to complete (if jQuery is present)
    /// </summary>
    public static bool WaitForAjaxComplete(int timeoutSeconds = 0)
    {
        Log.Debug("Waiting for AJAX calls to complete");
        var wait = CreateWait(timeoutSeconds);
        return wait.Until(d =>
        {
            var jsExecutor = (IJavaScriptExecutor)d;
            var jQueryExists = (bool)jsExecutor.ExecuteScript("return typeof jQuery !== 'undefined'");
            if (!jQueryExists) return true;
            return (bool)jsExecutor.ExecuteScript("return jQuery.active === 0");
        });
    }

    /// <summary>
    /// Performs a fluent wait with custom condition
    /// </summary>
    public static T WaitFor<T>(Func<IWebDriver, T> condition, int timeoutSeconds = 0, string? message = null)
    {
        var wait = CreateWait(timeoutSeconds);
        if (!string.IsNullOrEmpty(message))
            wait.Message = message;
        
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotInteractableException));

        return wait.Until(condition);
    }

    /// <summary>
    /// Retries an action until it succeeds or times out
    /// </summary>
    public static void RetryUntilSuccess(Action action, int maxAttempts = 3, int delayMs = 500)
    {
        Exception? lastException = null;
        
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                action();
                return;
            }
            catch (Exception ex)
            {
                lastException = ex;
                Log.Warning("Attempt {Attempt}/{MaxAttempts} failed: {Message}", 
                    attempt, maxAttempts, ex.Message);
                
                if (attempt < maxAttempts)
                    Thread.Sleep(delayMs);
            }
        }

        throw new Exception($"Action failed after {maxAttempts} attempts", lastException);
    }
}
