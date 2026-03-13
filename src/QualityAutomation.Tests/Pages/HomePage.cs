using OpenQA.Selenium;
using QualityAutomation.Tests.Pages.Base;
using Serilog;

namespace QualityAutomation.Tests.Pages;

/// <summary>
/// Page Object for the Home page
/// </summary>
public class HomePage : BasePage
{
    protected override string PageUrl => "/";
    protected override string PageTitle => "The Internet";

    #region Locators

    private static readonly By PageHeader = By.CssSelector("h1.heading");
    private static readonly By SubHeader = By.CssSelector("h2");
    private static readonly By AvailableExamples = By.CssSelector("#content ul li a");
    private static readonly By FormAuthenticationLink = By.LinkText("Form Authentication");
    private static readonly By CheckboxesLink = By.LinkText("Checkboxes");
    private static readonly By DropdownLink = By.LinkText("Dropdown");
    private static readonly By DynamicLoadingLink = By.LinkText("Dynamic Loading");

    #endregion

    #region Page Actions

    /// <summary>
    /// Navigates to the Form Authentication (Login) page
    /// </summary>
    public LoginPage GoToLoginPage()
    {
        Log.Information("Navigating to Form Authentication page");
        Click(FormAuthenticationLink);
        return new LoginPage();
    }

    /// <summary>
    /// Clicks on a specific example link by text
    /// </summary>
    public void ClickExampleLink(string linkText)
    {
        Log.Information("Clicking example link: {LinkText}", linkText);
        Click(By.LinkText(linkText));
    }

    #endregion

    #region Page Verifications

    /// <summary>
    /// Gets the page header text
    /// </summary>
    public string GetHeaderText()
    {
        return GetText(PageHeader);
    }

    /// <summary>
    /// Gets all available example links
    /// </summary>
    public IList<string> GetAllExampleLinks()
    {
        var elements = FindElements(AvailableExamples);
        return elements.Select(e => e.Text).ToList();
    }

    /// <summary>
    /// Checks if a specific example link exists
    /// </summary>
    public bool IsExampleLinkPresent(string linkText)
    {
        var links = GetAllExampleLinks();
        return links.Contains(linkText);
    }

    /// <summary>
    /// Gets the count of available examples
    /// </summary>
    public int GetExampleCount()
    {
        return FindElements(AvailableExamples).Count;
    }

    /// <summary>
    /// Verifies the home page is loaded
    /// </summary>
    public override bool IsPageLoaded()
    {
        return base.IsPageLoaded() && 
               IsElementDisplayed(PageHeader) && 
               IsElementDisplayed(AvailableExamples);
    }

    #endregion
}
