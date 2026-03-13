using OpenQA.Selenium;
using QualityAutomation.Tests.Pages.Base;
using Serilog;

namespace QualityAutomation.Tests.Pages;

/// <summary>
/// Page Object for the Secure Area (Dashboard) page after successful login
/// </summary>
public class SecureAreaPage : BasePage
{
    protected override string PageUrl => "/secure";
    protected override string PageTitle => "The Internet";

    #region Locators

    private static readonly By FlashMessage = By.Id("flash");
    private static readonly By PageHeader = By.CssSelector("h2");
    private static readonly By SubHeader = By.CssSelector("h4.subheader");
    private static readonly By LogoutButton = By.CssSelector("a.button[href='/logout']");
    private static readonly By SecureAreaContent = By.Id("content");

    #endregion

    #region Page Actions

    /// <summary>
    /// Clicks the logout button
    /// </summary>
    public LoginPage Logout()
    {
        Log.Information("Clicking logout button");
        Click(LogoutButton);
        return new LoginPage();
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
    /// Checks if the success message is displayed
    /// </summary>
    public bool IsSuccessMessageDisplayed()
    {
        if (!IsElementDisplayed(FlashMessage))
            return false;

        var message = GetFlashMessage();
        return message.Contains("You logged into a secure area!");
    }

    /// <summary>
    /// Gets the page header text
    /// </summary>
    public string GetHeaderText()
    {
        return GetText(PageHeader);
    }

    /// <summary>
    /// Gets the subheader text
    /// </summary>
    public string GetSubHeaderText()
    {
        return GetText(SubHeader);
    }

    /// <summary>
    /// Checks if the secure area content is displayed
    /// </summary>
    public bool IsSecureAreaDisplayed()
    {
        return IsElementDisplayed(SecureAreaContent) && 
               IsElementDisplayed(LogoutButton);
    }

    /// <summary>
    /// Checks if the logout button is displayed
    /// </summary>
    public bool IsLogoutButtonDisplayed()
    {
        return IsElementDisplayed(LogoutButton);
    }

    /// <summary>
    /// Verifies the secure area page is loaded
    /// </summary>
    public override bool IsPageLoaded()
    {
        return base.IsPageLoaded() && IsSecureAreaDisplayed();
    }

    /// <summary>
    /// Verifies user is logged in successfully
    /// </summary>
    public bool IsUserLoggedIn()
    {
        return IsPageLoaded() && 
               GetCurrentUrl().Contains("/secure") && 
               IsLogoutButtonDisplayed();
    }

    #endregion
}
