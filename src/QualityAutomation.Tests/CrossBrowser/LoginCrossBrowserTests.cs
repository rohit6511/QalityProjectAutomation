using NUnit.Framework;
using QualityAutomation.Tests.Config;
using QualityAutomation.Tests.Drivers;
using QualityAutomation.Tests.Pages;

namespace QualityAutomation.Tests.CrossBrowser;

/// <summary>
/// Login tests that run simultaneously on Chrome, Firefox, and Edge.
/// Each browser instance runs in parallel.
/// </summary>
[TestFixtureSource(typeof(BrowserTestFixtureSource))]
[Parallelizable(ParallelScope.Fixtures)]
[Category("CrossBrowser")]
[Category("Login")]
public class LoginCrossBrowserTests : CrossBrowserTestBase
{
    private LoginPage _loginPage = null!;

    public LoginCrossBrowserTests(BrowserType browser) : base(browser)
    {
    }

    public override void SetUp()
    {
        base.SetUp();
        _loginPage = new LoginPage();
        _loginPage.NavigateTo(ConfigurationManager.Instance.TestSettings.BaseUrl);
    }

    [Test]
    [Category("Critical")]
    [Category("Smoke")]
    [Description("Verify successful login with valid credentials on all browsers")]
    public void SuccessfulLogin_WithValidCredentials_ShouldRedirectToSecureArea()
    {
        // Arrange
        const string validUsername = "rohit.saini@thecodeinsight.com";
        const string validPassword = "Rohit@6511";

        // Act
        _loginPage.EnterUsername(validUsername);
        _loginPage.EnterPassword(validPassword);
        _loginPage.ClickLoginButton();

        // Assert
        Assert.That(_loginPage.IsLoggedIn(), Is.True, 
            $"User should be logged in successfully on {BrowserName}");
    }

    [Test]
    [Category("Negative")]
    [Description("Verify login fails with invalid username on all browsers")]
    public void FailedLogin_WithInvalidUsername_ShouldShowError()
    {
        // Arrange
        const string invalidUsername = "invaliduser@test.com";
        const string validPassword = "Rohit@6511";

        // Act
        _loginPage.EnterUsername(invalidUsername);
        _loginPage.EnterPassword(validPassword);
        _loginPage.ClickLoginButton();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_loginPage.IsOnLoginPage(), Is.True, 
                $"User should remain on login page on {BrowserName}");
            Assert.That(_loginPage.IsInvalidCredentialsErrorDisplayed(), Is.True, 
                $"Error message should be displayed on {BrowserName}");
        });
    }

    [Test]
    [Category("Negative")]
    [Description("Verify login fails with invalid password on all browsers")]
    public void FailedLogin_WithInvalidPassword_ShouldShowError()
    {
        // Arrange
        const string validUsername = "rohit.saini@thecodeinsight.com";
        const string invalidPassword = "wrongpassword";

        // Act
        _loginPage.EnterUsername(validUsername);
        _loginPage.EnterPassword(invalidPassword);
        _loginPage.ClickLoginButton();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_loginPage.IsOnLoginPage(), Is.True, 
                $"User should remain on login page on {BrowserName}");
            Assert.That(_loginPage.IsInvalidCredentialsErrorDisplayed(), Is.True, 
                $"Error message should be displayed on {BrowserName}");
        });
    }

    [Test]
    [Category("Negative")]
    [Description("Verify login fails with empty credentials on all browsers")]
    public void FailedLogin_WithEmptyCredentials_ShouldShowError()
    {
        // Act
        _loginPage.ClickLoginButton();

        // Assert
        Assert.That(_loginPage.IsOnLoginPage(), Is.True, 
            $"User should remain on login page on {BrowserName}");
    }
}
