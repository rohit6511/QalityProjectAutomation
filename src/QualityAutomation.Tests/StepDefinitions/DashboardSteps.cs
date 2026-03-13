using NUnit.Framework;
using QualityAutomation.Tests.Pages;
using QualityAutomation.Tests.Utilities;
using Reqnroll;
using Serilog;

namespace QualityAutomation.Tests.StepDefinitions;

/// <summary>
/// Step definitions for Dashboard (Secure Area) feature scenarios
/// </summary>
[Binding]
public class DashboardSteps
{
    private readonly ScenarioContext _scenarioContext;
    private SecureAreaPage _secureAreaPage = null!;
    private LoginPage _loginPage = null!;

    public DashboardSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private SecureAreaPage GetSecureAreaPage()
    {
        if (_scenarioContext.ContainsKey("SecureAreaPage"))
            return (SecureAreaPage)_scenarioContext["SecureAreaPage"];
        
        return _secureAreaPage ?? new SecureAreaPage();
    }

    #region When Steps

    [When(@"I click the logout button")]
    public void WhenIClickTheLogoutButton()
    {
        _secureAreaPage = GetSecureAreaPage();
        _loginPage = _secureAreaPage.Logout();
        
        _scenarioContext["LoginPage"] = _loginPage;
        Log.Information("Clicked logout button");
        ExtentReportManager.Instance.LogInfo("Clicked logout button");
    }

    #endregion

    #region Then Steps

    [Then(@"I should see the secure area header")]
    public void ThenIShouldSeeTheSecureAreaHeader()
    {
        _secureAreaPage = GetSecureAreaPage();
        var headerText = _secureAreaPage.GetHeaderText();
        
        Assert.That(headerText, Is.Not.Empty, 
            "Secure area header is empty");
        
        Log.Information("Secure area header: {Header}", headerText);
        ExtentReportManager.Instance.LogPass($"Secure area header displayed: {headerText}");
    }

    [Then(@"I should see the logout button")]
    public void ThenIShouldSeeTheLogoutButton()
    {
        _secureAreaPage = GetSecureAreaPage();
        
        Assert.That(_secureAreaPage.IsLogoutButtonDisplayed(), Is.True, 
            "Logout button is not displayed");
        
        Log.Information("Logout button is displayed");
        ExtentReportManager.Instance.LogPass("Logout button is displayed");
    }

    [Then(@"the page URL should contain ""(.*)""")]
    public void ThenThePageUrlShouldContain(string urlPart)
    {
        _secureAreaPage = GetSecureAreaPage();
        var currentUrl = _secureAreaPage.GetCurrentUrl();
        
        Assert.That(currentUrl, Does.Contain(urlPart), 
            $"Expected URL to contain '{urlPart}' but was '{currentUrl}'");
        
        Log.Information("URL contains '{UrlPart}': {Url}", urlPart, currentUrl);
        ExtentReportManager.Instance.LogPass($"URL contains: {urlPart}");
    }

    [Then(@"I should be redirected to the login page")]
    public void ThenIShouldBeRedirectedToTheLoginPage()
    {
        _loginPage = _loginPage ?? new LoginPage();
        
        Assert.That(_loginPage.GetCurrentUrl(), Does.Contain("/login"), 
            "Not redirected to login page");
        Assert.That(_loginPage.IsLoginFormDisplayed(), Is.True, 
            "Login form is not displayed");
        
        Log.Information("Successfully redirected to login page");
        ExtentReportManager.Instance.LogPass("Redirected to login page");
    }

    [Then(@"I should see a logout success message")]
    public void ThenIShouldSeeALogoutSuccessMessage()
    {
        _loginPage = _loginPage ?? new LoginPage();
        var message = _loginPage.GetFlashMessage();
        
        Assert.That(message, Does.Contain("logged out"), 
            $"Expected logout message but got: {message}");
        
        Log.Information("Logout success message: {Message}", message);
        ExtentReportManager.Instance.LogPass("Logout success message displayed");
    }

    [Then(@"the secure area header should be ""(.*)""")]
    public void ThenTheSecureAreaHeaderShouldBe(string expectedHeader)
    {
        _secureAreaPage = GetSecureAreaPage();
        var actualHeader = _secureAreaPage.GetHeaderText();
        
        Assert.That(actualHeader, Does.Contain(expectedHeader), 
            $"Expected header '{expectedHeader}' but was '{actualHeader}'");
        
        Log.Information("Secure area header verified: {Header}", actualHeader);
        ExtentReportManager.Instance.LogPass($"Header is: {actualHeader}");
    }

    [Then(@"I should see the welcome subheader text")]
    public void ThenIShouldSeeTheWelcomeSubheaderText()
    {
        _secureAreaPage = GetSecureAreaPage();
        var subheader = _secureAreaPage.GetSubHeaderText();
        
        Assert.That(subheader, Is.Not.Empty, 
            "Subheader text is empty");
        
        Log.Information("Subheader text: {Subheader}", subheader);
        ExtentReportManager.Instance.LogPass($"Subheader displayed: {subheader}");
    }

    [Then(@"the subheader should contain ""(.*)""")]
    public void ThenTheSubheaderShouldContain(string expectedText)
    {
        _secureAreaPage = GetSecureAreaPage();
        var subheader = _secureAreaPage.GetSubHeaderText();
        
        Assert.That(subheader, Does.Contain(expectedText), 
            $"Expected subheader to contain '{expectedText}' but was '{subheader}'");
        
        Log.Information("Subheader contains expected text");
        ExtentReportManager.Instance.LogPass($"Subheader contains: {expectedText}");
    }

    #endregion
}
