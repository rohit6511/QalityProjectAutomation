using NUnit.Framework;
using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Pages;
using QualityAutomation.Tests.Utilities;
using Reqnroll;
using Serilog;

namespace QualityAutomation.Tests.StepDefinitions;

/// <summary>
/// Step definitions for Login feature scenarios
/// </summary>
[Binding]
public class LoginSteps
{
    private readonly ScenarioContext _scenarioContext;
    private LoginPage _loginPage = null!;
    private SecureAreaPage _secureAreaPage = null!;

    public LoginSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    #region Given Steps

    [Given(@"I am on the login page")]
    public void GivenIAmOnTheLoginPage()
    {
        _loginPage = new LoginPage();
        _loginPage.NavigateTo();

        //Thread.Sleep(10);
        
        // Assert.That(_loginPage.IsPageLoaded(), Is.True, 
        //     "Login page did not load correctly");
        
        // Log.Information("Navigated to login page successfully");
        // ExtentReportManager.Instance.LogInfo("Navigated to login page");
    }

    [Given(@"I am logged in with valid credentials")]
    public void GivenIAmLoggedInWithValidCredentials()
    {
        GivenIAmOnTheLoginPage();
        
        _secureAreaPage = _loginPage.Login(
            Constants.TestData.ValidUsername, 
            Constants.TestData.ValidPassword);
        
        Assert.That(_secureAreaPage.IsUserLoggedIn(), Is.True, 
            "Failed to login with valid credentials");
        
        _scenarioContext["SecureAreaPage"] = _secureAreaPage;
        Log.Information("Logged in successfully with valid credentials");
    }

    #endregion

    #region When Steps

    [When(@"I enter valid username ""(.*)""")]
    public void WhenIEnterValidUsername(string username)
    {
        _loginPage = _loginPage ?? new LoginPage();
        _loginPage.EnterUsername(username);
        Log.Information("Entered username: {Username}", username);
    }

    [When(@"I enter invalid username ""(.*)""")]
    public void WhenIEnterInvalidUsername(string username)
    {
        _loginPage = _loginPage ?? new LoginPage();
        _loginPage.EnterUsername(username);
        Log.Information("Entered invalid username: {Username}", username);
    }

    [When(@"I enter valid password ""(.*)""")]
    public void WhenIEnterValidPassword(string password)
    {
        _loginPage = _loginPage ?? new LoginPage();
        _loginPage.EnterPassword(password);
        Log.Information("Entered password");
    }

    [When(@"I enter invalid password ""(.*)""")]
    public void WhenIEnterInvalidPassword(string password)
    {
        _loginPage = _loginPage ?? new LoginPage();
        _loginPage.EnterPassword(password);
        Log.Information("Entered invalid password");
    }

    [When(@"I click the login button")]
    public void WhenIClickTheLoginButton()
    {
        _loginPage = _loginPage ?? new LoginPage();
        _loginPage.ClickLoginButton();
        Log.Information("Clicked login button");
    }

    #endregion

    #region Then Steps

    [Then(@"I should be redirected to the secure area")]
    public void ThenIShouldBeRedirectedToTheSecureArea()
    {
        // _secureAreaPage = new SecureAreaPage();
        
        // Assert.That(_secureAreaPage.IsPageLoaded(), Is.True, 
        //     "Secure area page did not load");
        // Assert.That(_secureAreaPage.GetCurrentUrl(), Does.Contain("/secure"), 
        //     "URL does not contain '/secure'");
        
        // _scenarioContext["SecureAreaPage"] = _secureAreaPage;
        // Log.Information("Successfully redirected to secure area");
        // ExtentReportManager.Instance.LogPass("Redirected to secure area");
    }

    [Then(@"I should see a success message containing ""(.*)""")]
    public void ThenIShouldSeeASuccessMessageContaining(string expectedMessage)
    {
        _secureAreaPage = _secureAreaPage ?? new SecureAreaPage();
        var actualMessage = _secureAreaPage.GetFlashMessage();
        
        Assert.That(actualMessage, Does.Contain(expectedMessage), 
            $"Expected message to contain '{expectedMessage}' but was '{actualMessage}'");
        
        Log.Information("Success message verified: {Message}", actualMessage);
        ExtentReportManager.Instance.LogPass($"Success message contains: {expectedMessage}");
    }

    [Then(@"I should remain on the login page")]
    public void ThenIShouldRemainOnTheLoginPage()
    {
        _loginPage = _loginPage ?? new LoginPage();
        var currentUrl = _loginPage.GetCurrentUrl();
        
        Assert.That(currentUrl, Does.Contain("/login"), 
            $"Expected to remain on login page but URL is: {currentUrl}");
        
        Log.Information("Remained on login page as expected");
        ExtentReportManager.Instance.LogPass("Remained on login page");
    }

    [Then(@"I should see an error message containing ""(.*)""")]
    public void ThenIShouldSeeAnErrorMessageContaining(string expectedMessage)
    {
        _loginPage = _loginPage ?? new LoginPage();
        var actualMessage = _loginPage.GetFlashMessage();
        
        Assert.That(actualMessage, Does.Contain(expectedMessage), 
            $"Expected error message to contain '{expectedMessage}' but was '{actualMessage}'");
        
        Log.Information("Error message verified: {Message}", actualMessage);
        ExtentReportManager.Instance.LogPass($"Error message contains: {expectedMessage}");
    }

    [Then(@"I should see an error message")]
    public void ThenIShouldSeeAnErrorMessage()
    {
        _loginPage = _loginPage ?? new LoginPage();
        
        Assert.That(_loginPage.IsErrorMessageDisplayed() || _loginPage.IsInvalidCredentialsErrorDisplayed(), Is.True, 
            "Error message is not displayed");
        
        var message = _loginPage.GetErrorMessageText();
        Log.Information("Error message displayed: {Message}", message);
        ExtentReportManager.Instance.LogPass($"Error message is displayed: {message}");
    }

    [Then(@"I should see a success message")]
    public void ThenIShouldSeeASuccessMessage()
    {
        _secureAreaPage = _secureAreaPage ?? new SecureAreaPage();
        
        Assert.That(_secureAreaPage.IsUserLoggedIn(), Is.True, 
            "User is not logged in - success state not detected");
        
        Log.Information("Login successful - user is logged in");
        ExtentReportManager.Instance.LogPass("Login successful");
    }

    #endregion
}
