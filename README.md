<<<<<<< HEAD
# Quality Automation Framework

A professional, enterprise-grade UI Automation Testing Framework built with C#, Selenium WebDriver, NUnit, and Reqnroll (SpecFlow successor) implementing the Page Object Model (POM) design pattern.

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Test Reports](#test-reports)
- [Writing New Tests](#writing-new-tests)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)

## Features

- **.NET 8.0** - Latest LTS framework version
- **Selenium WebDriver 4.x** - Modern browser automation
- **NUnit 4.x** - Powerful testing framework with parallel execution support
- **Reqnroll (SpecFlow successor)** - BDD-style feature files and step definitions
- **Page Object Model (POM)** - Clean separation of test logic and page interactions
- **Cross-Browser Support** - Chrome, Firefox, and Edge
- **Parallel Execution** - ThreadLocal WebDriver for concurrent test execution
- **ExtentReports** - Beautiful HTML reports with screenshots
- **Serilog** - Structured logging to console and file
- **Configuration Management** - JSON-based settings with environment overrides
- **Screenshot on Failure** - Automatic screenshot capture when tests fail
- **Explicit Waits** - Robust wait helper utilities
- **Reusable Utilities** - Random data generation, common methods, constants

## Project Structure

```
QualityProjectAutomation/
├── QualityAutomation.sln                    # Solution file
├── README.md                                 # This file
├── test.runsettings                         # NUnit run settings
└── src/
    └── QualityAutomation.Tests/
        ├── QualityAutomation.Tests.csproj   # Project file
        ├── AssemblyInfo.cs                   # Parallel execution config
        ├── reqnroll.json                     # Reqnroll configuration
        ├── Config/
        │   ├── appsettings.json             # Application settings
        │   ├── ConfigurationManager.cs       # Settings loader
        │   └── TestSettings.cs               # Settings models
        ├── Drivers/
        │   ├── BrowserType.cs               # Browser enum
        │   └── DriverFactory.cs             # WebDriver factory
        ├── Features/
        │   ├── Login.feature                # Login scenarios
        │   └── Dashboard.feature            # Dashboard scenarios
        ├── Hooks/
        │   └── TestHooks.cs                 # Setup/teardown hooks
        ├── Pages/
        │   ├── Base/
        │   │   └── BasePage.cs              # Base page class
        │   ├── HomePage.cs                   # Home page object
        │   ├── LoginPage.cs                  # Login page object
        │   └── SecureAreaPage.cs            # Dashboard page object
        ├── StepDefinitions/
        │   ├── LoginSteps.cs                # Login step definitions
        │   └── DashboardSteps.cs            # Dashboard step definitions
        ├── Utilities/
        │   ├── CommonMethods.cs             # Reusable Selenium methods
        │   ├── Constants.cs                  # Application constants
        │   ├── ExtentReportManager.cs       # HTML reporting
        │   ├── LoggerSetup.cs               # Serilog configuration
        │   ├── RandomDataGenerator.cs       # Test data generation
        │   ├── ScreenshotHelper.cs          # Screenshot utilities
        │   └── WaitHelper.cs                # Explicit wait methods
        ├── Logs/                            # Log files (generated)
        └── Reports/                         # HTML reports (generated)
```

## Prerequisites

1. **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Visual Studio 2022** (recommended) or VS Code with C# extension
3. **Browsers** - Chrome, Firefox, or Edge installed
4. **Git** (optional) - For version control

### Verify .NET Installation

```bash
dotnet --version
```

## Installation

### Clone or Navigate to Project

```bash
cd c:\Users\saini\Desktop\QalityProjectAutomation
```

### Restore NuGet Packages

```bash
dotnet restore
```

### Build the Project

```bash
dotnet build
```

## Configuration

### appsettings.json

Edit `src/QualityAutomation.Tests/Config/appsettings.json` to customize settings:

```json
{
  "TestSettings": {
    "BaseUrl": "https://the-internet.herokuapp.com",
    "Browser": "Chrome",
    "Headless": false,
    "ImplicitWaitSeconds": 10,
    "ExplicitWaitSeconds": 30,
    "PageLoadTimeoutSeconds": 60,
    "ScreenshotOnFailure": true,
    "MaxRetryAttempts": 2
  },
  "ReportSettings": {
    "ReportPath": "Reports",
    "ReportTitle": "Quality Automation Test Report",
    "ReportName": "TestExecutionReport"
  },
  "LogSettings": {
    "LogPath": "Logs",
    "LogLevel": "Information"
  }
}
```

### Environment Variables

Override settings using environment variables:

```powershell
# PowerShell
$env:BROWSER = "Firefox"
$env:HEADLESS = "true"
$env:BASE_URL = "https://your-app-url.com"

# Command Prompt
set BROWSER=Firefox
set HEADLESS=true
set BASE_URL=https://your-app-url.com
```

## Running Tests

### Visual Studio

1. Open `QualityAutomation.sln` in Visual Studio 2022
2. Build the solution (Ctrl+Shift+B)
3. Open Test Explorer (Test > Test Explorer)
4. Click "Run All Tests" or select specific tests

### Command Line - All Tests

```bash
cd c:\Users\saini\Desktop\QalityProjectAutomation
dotnet test
```

### Command Line - With Detailed Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Command Line - Specific Browser

```powershell
$env:BROWSER = "Chrome"
dotnet test
```

```powershell
$env:BROWSER = "Firefox"
dotnet test
```

```powershell
$env:BROWSER = "Edge"
dotnet test
```

### Command Line - Headless Mode

```powershell
$env:HEADLESS = "true"
dotnet test
```

### Command Line - Run by Category/Tag

```bash
# Run only Smoke tests
dotnet test --filter "TestCategory=Smoke"

# Run only Login tests
dotnet test --filter "TestCategory=Login"

# Run Critical and Smoke tests
dotnet test --filter "TestCategory=Critical|TestCategory=Smoke"

# Exclude Regression tests
dotnet test --filter "TestCategory!=Regression"
```

### Command Line - Run Specific Feature

```bash
dotnet test --filter "FullyQualifiedName~Login"
dotnet test --filter "FullyQualifiedName~Dashboard"
```

### Command Line - Parallel Execution

```bash
# Uses settings from test.runsettings
dotnet test --settings test.runsettings

# Override parallel workers
dotnet test -- NUnit.NumberOfTestWorkers=8
```

### Command Line - Generate TRX Report

```bash
dotnet test --logger "trx;LogFileName=TestResults.trx"
```

## Test Reports

### ExtentReports (HTML)

After test execution, find HTML reports in:
```
src\QualityAutomation.Tests\bin\Debug\net8.0\Reports\
```

The report includes:
- Test execution summary
- Pass/Fail/Skip statistics
- Step-by-step execution details
- Screenshots on failure
- System information
- Category/tag filtering

### Log Files

Find detailed logs in:
```
src\QualityAutomation.Tests\bin\Debug\net8.0\Logs\
```

Log files are named with the date: `TestLog_yyyyMMdd.log`

## Writing New Tests

### 1. Create a Feature File

Create `Features/YourFeature.feature`:

```gherkin
@YourTag
Feature: Your Feature Name
    As a user
    I want to do something
    So that I achieve a goal

    @Smoke
    Scenario: Your scenario description
        Given I am on some page
        When I perform an action
        Then I should see a result
```

### 2. Create a Page Object

Create `Pages/YourPage.cs`:

```csharp
using OpenQA.Selenium;
using QualityAutomation.Tests.Pages.Base;

namespace QualityAutomation.Tests.Pages;

public class YourPage : BasePage
{
    protected override string PageUrl => "/your-page-url";
    
    // Define locators
    private static readonly By SomeElement = By.Id("elementId");
    
    // Define actions
    public void ClickSomeElement()
    {
        Click(SomeElement);
    }
    
    // Define verifications
    public bool IsSomeElementDisplayed()
    {
        return IsElementDisplayed(SomeElement);
    }
}
```

### 3. Create Step Definitions

Create `StepDefinitions/YourSteps.cs`:

```csharp
using NUnit.Framework;
using QualityAutomation.Tests.Pages;
using Reqnroll;

namespace QualityAutomation.Tests.StepDefinitions;

[Binding]
public class YourSteps
{
    private YourPage _yourPage = null!;

    [Given(@"I am on some page")]
    public void GivenIAmOnSomePage()
    {
        _yourPage = new YourPage();
        _yourPage.NavigateTo();
    }

    [When(@"I perform an action")]
    public void WhenIPerformAnAction()
    {
        _yourPage.ClickSomeElement();
    }

    [Then(@"I should see a result")]
    public void ThenIShouldSeeAResult()
    {
        Assert.That(_yourPage.IsSomeElementDisplayed(), Is.True);
    }
}
```

## Best Practices

### Page Objects
- Keep locators private and static
- Use fluent interface pattern (return `this` or next page)
- Group related elements and actions together
- Add verification methods for page state

### Step Definitions
- Keep steps reusable and atomic
- Use ScenarioContext for sharing state between steps
- Handle both positive and negative scenarios
- Add meaningful assertions with clear messages

### Waits
- Use explicit waits over implicit waits
- Never use Thread.Sleep in production code
- Use WaitHelper methods for common scenarios
- Handle StaleElementReferenceException

### Logging
- Log meaningful events, not every action
- Use appropriate log levels (Info, Debug, Warning, Error)
- Include relevant context in log messages

## Troubleshooting

### WebDriver Issues

**Chrome/Firefox/Edge not found:**
- Ensure the browser is installed
- WebDriverManager automatically downloads the correct driver

**Driver version mismatch:**
- Clear the WebDriverManager cache:
  ```bash
  Remove-Item -Recurse -Force "$env:USERPROFILE\.wdm"
  ```

### Build Errors

**Package restore failed:**
```bash
dotnet nuget locals all --clear
dotnet restore
```

**Reqnroll code generation:**
- Rebuild the solution after adding/modifying feature files
- Ensure Reqnroll.NUnit package is installed

### Test Execution Issues

**Tests not discovered:**
- Rebuild the solution
- Check that NUnit3TestAdapter is installed
- Verify test methods have proper attributes

**Parallel execution conflicts:**
- Use ThreadLocal for WebDriver (already implemented)
- Avoid shared static state in tests
- Use unique test data for each scenario

### Report Generation Issues

**Reports not generated:**
- Check `Reports` folder permissions
- Ensure tests complete (even if failing)
- Call `ExtentReportManager.Instance.Flush()` in AfterTestRun

## Support

For issues or questions:
1. Check the troubleshooting section
2. Review log files for detailed error information
3. Examine the HTML report for test execution details

## License

This project is provided as-is for educational and professional use.
=======
# QalityProjectAutomation
Selenium UI Automation Framework using C#, NUnit, POM and SpecFlow
>>>>>>> 737f75d633a14135d1c9b86059591df9eb842b5a
