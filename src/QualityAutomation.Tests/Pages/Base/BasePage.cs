using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Drivers;
using QualityAutomation.Tests.Utilities;
using Serilog;

namespace QualityAutomation.Tests.Pages.Base;

/// <summary>
/// Base class for all page objects. Provides common functionality and utilities.
/// </summary>
public abstract class BasePage
{
    protected IWebDriver Driver => DriverFactory.Driver;
    protected TestSettings Settings => ConfigurationManager.Instance.TestSettings;
    
    protected abstract string PageUrl { get; }
    protected virtual string PageTitle => string.Empty;

    protected BasePage()
    {
        Log.Debug("Initializing page: {PageType}", GetType().Name);
    }

    /// <summary>
    /// Navigates to this page using the configured base URL
    /// </summary>
    public virtual void NavigateTo()
    {
        var fullUrl = $"{Settings.BaseUrl}{PageUrl}";
        Log.Information("Navigating to page: {Url}", fullUrl);
        Driver.Navigate().GoToUrl(fullUrl);
        WaitForPageLoad();
    }

    /// <summary>
    /// Navigates to a specific URL
    /// </summary>
    public void NavigateTo(string url)
    {
        Log.Information("Navigating to: {Url}", url);
        Driver.Navigate().GoToUrl(url);
        WaitForPageLoad();
    }

    /// <summary>
    /// Verifies the page is loaded correctly
    /// </summary>
    public virtual bool IsPageLoaded()
    {
        WaitForPageLoad();
        
        if (!string.IsNullOrEmpty(PageUrl))
        {
            var currentUrl = Driver.Url;
            if (!currentUrl.Contains(PageUrl.TrimStart('/')))
            {
                Log.Warning("Page URL mismatch. Expected to contain: {Expected}, Actual: {Actual}", 
                    PageUrl, currentUrl);
                return false;
            }
        }

        if (!string.IsNullOrEmpty(PageTitle))
        {
            var currentTitle = Driver.Title;
            if (!currentTitle.Contains(PageTitle))
            {
                Log.Warning("Page title mismatch. Expected to contain: {Expected}, Actual: {Actual}", 
                    PageTitle, currentTitle);
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Waits for the page to load completely
    /// </summary>
    protected void WaitForPageLoad()
    {
        WaitHelper.WaitForPageLoad();
    }

    /// <summary>
    /// Creates a WebDriverWait instance
    /// </summary>
    protected WebDriverWait CreateWait(int timeoutSeconds = 0)
    {
        return WaitHelper.CreateWait(timeoutSeconds);
    }

    #region Element Operations

    /// <summary>
    /// Finds an element using the specified locator
    /// </summary>
    protected IWebElement FindElement(By locator)
    {
        return WaitHelper.WaitForElementVisible(locator);
    }

    /// <summary>
    /// Finds multiple elements using the specified locator
    /// </summary>
    protected IReadOnlyCollection<IWebElement> FindElements(By locator)
    {
        return WaitHelper.WaitForElementsPresent(locator);
    }

    /// <summary>
    /// Clicks on an element
    /// </summary>
    protected void Click(By locator)
    {
        CommonMethods.Click(locator);
    }

    /// <summary>
    /// Enters text into an input field
    /// </summary>
    protected void EnterText(By locator, string text)
    {
        CommonMethods.EnterText(locator, text);
    }

    /// <summary>
    /// Gets the text from an element
    /// </summary>
    protected string GetText(By locator)
    {
        return CommonMethods.GetText(locator);
    }

    /// <summary>
    /// Gets the value attribute from an input element
    /// </summary>
    protected string? GetValue(By locator)
    {
        return CommonMethods.GetValue(locator);
    }

    /// <summary>
    /// Checks if an element is displayed
    /// </summary>
    protected bool IsElementDisplayed(By locator, int timeoutSeconds = 5)
    {
        return CommonMethods.IsElementDisplayed(locator, timeoutSeconds);
    }

    /// <summary>
    /// Selects an option from a dropdown by visible text
    /// </summary>
    protected void SelectDropdownByText(By locator, string text)
    {
        CommonMethods.SelectByText(locator, text);
    }

    /// <summary>
    /// Scrolls to an element
    /// </summary>
    protected void ScrollToElement(By locator)
    {
        CommonMethods.ScrollToElement(locator);
    }

    /// <summary>
    /// Waits for an element to be visible
    /// </summary>
    protected IWebElement WaitForElement(By locator, int timeoutSeconds = 0)
    {
        return WaitHelper.WaitForElementVisible(locator, timeoutSeconds);
    }

    /// <summary>
    /// Waits for an element to be clickable
    /// </summary>
    protected IWebElement WaitForClickable(By locator, int timeoutSeconds = 0)
    {
        return WaitHelper.WaitForElementClickable(locator, timeoutSeconds);
    }

    #endregion

    #region Page Information

    /// <summary>
    /// Gets the current page URL
    /// </summary>
    public string GetCurrentUrl() => Driver.Url;

    /// <summary>
    /// Gets the current page title
    /// </summary>
    public string GetPageTitle() => Driver.Title;

    /// <summary>
    /// Refreshes the current page
    /// </summary>
    public void RefreshPage()
    {
        CommonMethods.RefreshPage();
    }

    #endregion
}
