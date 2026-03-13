using OpenQA.Selenium;
using QualityAutomation.Tests.Pages.Base;
using Serilog;

namespace QualityAutomation.Tests.Pages;

/// <summary>
/// Page Object for the Login page (using the-internet.herokuapp.com/login)
/// </summary>
public class LoginPage : BasePage
{
    protected override string PageUrl => "/login";
    protected override string PageTitle => "The Internet";

    #region Locators

    private static readonly By UsernameInput = By.XPath("//input[@placeholder='Enter your username']");
    private static readonly By PasswordInput = By.XPath("//input[@placeholder='Enter your password']");
    private static readonly By LoginButton = By.XPath("//span[text()='Sign In']");
    private static readonly By FlashMessage = By.Id("flash");
    private static readonly By PageHeader = By.CssSelector("h2");
    
    // Error message locator - adjust based on actual page structure
    private static readonly By ErrorMessage = By.XPath("//*[contains(text(),'invalid username or password') or contains(text(),'Invalid username or password') or contains(text(),'Please try again')]");
    private static readonly By ErrorMessageContainer = By.CssSelector(".error-message, .alert-error, .alert-danger, [role='alert']");

    #endregion

    #region Page Actions

    /// <summary>
    /// Enters the username
    /// </summary>
    public LoginPage EnterUsername(string username)
    {
        Log.Information("Entering username: {Username}", username);
        EnterText(UsernameInput, username);
        return this;
    }

    /// <summary>
    /// Enters the password
    /// </summary>
    public LoginPage EnterPassword(string password)
    {
        Log.Information("Entering password: ****");
        EnterText(PasswordInput, password);
        return this;
    }

    /// <summary>
    /// Clicks the login button
    /// </summary>
    public void ClickLoginButton()
    {
        Log.Information("Clicking login button");
        Click(LoginButton);
    }

    /// <summary>
    /// Performs a complete login operation
    /// </summary>
    public SecureAreaPage Login(string username, string password)
    {
        Log.Information("Performing login with username: {Username}", username);
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
        return new SecureAreaPage();
    }

    /// <summary>
    /// Performs login expecting failure
    /// </summary>
    public LoginPage LoginExpectingError(string username, string password)
    {
        Log.Information("Performing login expecting error with username: {Username}", username);
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
        return this;
    }

    #endregion

    #region Page Verifications

    /// <summary>
    /// Gets the flash message text
    /// </summary>
    public string GetFlashMessage()
    {
        var message = GetText(FlashMessage);
        Log.Debug("Flash message: {Message}", message);
        return message;
    }

    /// <summary>
    /// Checks if the error message is displayed
    /// </summary>
    public bool IsErrorMessageDisplayed()
    {
        return IsElementDisplayed(ErrorMessage) || 
               IsElementDisplayed(ErrorMessageContainer) || 
               IsElementDisplayed(FlashMessage);
    }

    /// <summary>
    /// Checks if the invalid credentials error message is displayed
    /// </summary>
    public bool IsInvalidCredentialsErrorDisplayed()
    {
        try
        {
            var errorText = GetErrorMessageText().ToLower();
            return errorText.Contains("invalid") || 
                   errorText.Contains("incorrect") || 
                   errorText.Contains("try again");
        }
        catch
        {
            return IsElementDisplayed(ErrorMessage);
        }
    }

    /// <summary>
    /// Gets the error message text
    /// </summary>
    public string GetErrorMessageText()
    {
        try
        {
            if (IsElementDisplayed(ErrorMessage))
                return GetText(ErrorMessage);
            if (IsElementDisplayed(ErrorMessageContainer))
                return GetText(ErrorMessageContainer);
            if (IsElementDisplayed(FlashMessage))
                return GetText(FlashMessage);
            return string.Empty;
        }
        catch (Exception ex)
        {
            Log.Warning("Could not get error message: {Message}", ex.Message);
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the page header text
    /// </summary>
    public string GetHeaderText()
    {
        return GetText(PageHeader);
    }

    /// <summary>
    /// Checks if the login form is displayed
    /// </summary>
    public bool IsLoginFormDisplayed()
    {
        return IsElementDisplayed(UsernameInput) && 
               IsElementDisplayed(PasswordInput) && 
               IsElementDisplayed(LoginButton);
    }

    /// <summary>
    /// Verifies the login page is loaded
    /// </summary>
    public override bool IsPageLoaded()
    {
        return base.IsPageLoaded() && IsLoginFormDisplayed();
    }

    /// <summary>
    /// Checks if user is still on the login page
    /// </summary>
    public bool IsOnLoginPage()
    {
        return IsLoginFormDisplayed();
    }

    /// <summary>
    /// Checks if user has successfully logged in (no longer on login page)
    /// </summary>
    public bool IsLoggedIn()
    {
        try
        {
            return !IsLoginFormDisplayed() || Driver.Url.Contains("secure") || Driver.Url.Contains("dashboard");
        }
        catch
        {
            return false;
        }
    }

    #endregion
}
