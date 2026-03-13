using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using QualityAutomation.Tests.Drivers;
using Serilog;

namespace QualityAutomation.Tests.Utilities;

/// <summary>
/// Common reusable methods for Selenium WebDriver operations
/// </summary>
public static class CommonMethods
{
    private static IWebDriver Driver => DriverFactory.Driver;

    #region Navigation

    /// <summary>
    /// Navigates to the specified URL
    /// </summary>
    public static void NavigateTo(string url)
    {
        Log.Information("Navigating to: {Url}", url);
        Driver.Navigate().GoToUrl(url);
        WaitHelper.WaitForPageLoad();
    }

    /// <summary>
    /// Refreshes the current page
    /// </summary>
    public static void RefreshPage()
    {
        Log.Information("Refreshing page");
        Driver.Navigate().Refresh();
        WaitHelper.WaitForPageLoad();
    }

    /// <summary>
    /// Navigates back in browser history
    /// </summary>
    public static void NavigateBack()
    {
        Log.Information("Navigating back");
        Driver.Navigate().Back();
        WaitHelper.WaitForPageLoad();
    }

    /// <summary>
    /// Navigates forward in browser history
    /// </summary>
    public static void NavigateForward()
    {
        Log.Information("Navigating forward");
        Driver.Navigate().Forward();
        WaitHelper.WaitForPageLoad();
    }

    #endregion

    #region Element Interactions

    /// <summary>
    /// Clicks on an element with retry logic
    /// </summary>
    public static void Click(By locator)
    {
        WaitHelper.RetryUntilSuccess(() =>
        {
            var element = WaitHelper.WaitForElementClickable(locator);
            Log.Debug("Clicking element: {Locator}", locator);
            element.Click();
        });
    }

    /// <summary>
    /// Clicks on an element using JavaScript
    /// </summary>
    public static void JavaScriptClick(By locator)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        Log.Debug("JavaScript clicking element: {Locator}", locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
    }

    /// <summary>
    /// Double-clicks on an element
    /// </summary>
    public static void DoubleClick(By locator)
    {
        var element = WaitHelper.WaitForElementClickable(locator);
        Log.Debug("Double-clicking element: {Locator}", locator);
        new Actions(Driver).DoubleClick(element).Perform();
    }

    /// <summary>
    /// Right-clicks on an element
    /// </summary>
    public static void RightClick(By locator)
    {
        var element = WaitHelper.WaitForElementClickable(locator);
        Log.Debug("Right-clicking element: {Locator}", locator);
        new Actions(Driver).ContextClick(element).Perform();
    }

    /// <summary>
    /// Enters text into an input field (clears existing text first)
    /// </summary>
    public static void EnterText(By locator, string text)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Entering text '{Text}' into: {Locator}", text, locator);
        element.Clear();
        element.SendKeys(text);
    }

    /// <summary>
    /// Enters text without clearing the field first
    /// </summary>
    public static void AppendText(By locator, string text)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Appending text '{Text}' to: {Locator}", text, locator);
        element.SendKeys(text);
    }

    /// <summary>
    /// Clears text from an input field
    /// </summary>
    public static void ClearText(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Clearing text from: {Locator}", locator);
        element.Clear();
    }

    /// <summary>
    /// Gets the text of an element
    /// </summary>
    public static string GetText(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        var text = element.Text;
        Log.Debug("Got text '{Text}' from: {Locator}", text, locator);
        return text;
    }

    /// <summary>
    /// Gets the value of an attribute
    /// </summary>
    public static string? GetAttribute(By locator, string attributeName)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        var value = element.GetAttribute(attributeName);
        Log.Debug("Got attribute '{Attribute}'='{Value}' from: {Locator}", attributeName, value, locator);
        return value;
    }

    /// <summary>
    /// Gets the value attribute of an input element
    /// </summary>
    public static string? GetValue(By locator) => GetAttribute(locator, "value");

    /// <summary>
    /// Checks if an element is displayed
    /// </summary>
    public static bool IsElementDisplayed(By locator, int timeoutSeconds = 5)
    {
        try
        {
            var element = WaitHelper.WaitForElementVisible(locator, timeoutSeconds);
            return element.Displayed;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if an element exists in the DOM
    /// </summary>
    public static bool IsElementPresent(By locator)
    {
        try
        {
            Driver.FindElement(locator);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if an element is enabled
    /// </summary>
    public static bool IsElementEnabled(By locator)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        return element.Enabled;
    }

    /// <summary>
    /// Checks if a checkbox or radio button is selected
    /// </summary>
    public static bool IsSelected(By locator)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        return element.Selected;
    }

    #endregion

    #region Dropdown Operations

    /// <summary>
    /// Selects an option from a dropdown by visible text
    /// </summary>
    public static void SelectByText(By locator, string text)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Selecting '{Text}' from dropdown: {Locator}", text, locator);
        new SelectElement(element).SelectByText(text);
    }

    /// <summary>
    /// Selects an option from a dropdown by value
    /// </summary>
    public static void SelectByValue(By locator, string value)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Selecting value '{Value}' from dropdown: {Locator}", value, locator);
        new SelectElement(element).SelectByValue(value);
    }

    /// <summary>
    /// Selects an option from a dropdown by index
    /// </summary>
    public static void SelectByIndex(By locator, int index)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Selecting index '{Index}' from dropdown: {Locator}", index, locator);
        new SelectElement(element).SelectByIndex(index);
    }

    /// <summary>
    /// Gets the selected option text from a dropdown
    /// </summary>
    public static string GetSelectedText(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        return new SelectElement(element).SelectedOption.Text;
    }

    /// <summary>
    /// Gets all options from a dropdown
    /// </summary>
    public static IList<string> GetAllOptions(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        return new SelectElement(element).Options.Select(o => o.Text).ToList();
    }

    #endregion

    #region Mouse Actions

    /// <summary>
    /// Hovers over an element
    /// </summary>
    public static void HoverOver(By locator)
    {
        var element = WaitHelper.WaitForElementVisible(locator);
        Log.Debug("Hovering over: {Locator}", locator);
        new Actions(Driver).MoveToElement(element).Perform();
    }

    /// <summary>
    /// Drags an element and drops it on another element
    /// </summary>
    public static void DragAndDrop(By source, By target)
    {
        var sourceElement = WaitHelper.WaitForElementVisible(source);
        var targetElement = WaitHelper.WaitForElementVisible(target);
        Log.Debug("Dragging from {Source} to {Target}", source, target);
        new Actions(Driver).DragAndDrop(sourceElement, targetElement).Perform();
    }

    #endregion

    #region JavaScript Operations

    /// <summary>
    /// Executes JavaScript
    /// </summary>
    public static object? ExecuteJavaScript(string script, params object[] args)
    {
        Log.Debug("Executing JavaScript: {Script}", script);
        return ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
    }

    /// <summary>
    /// Scrolls to an element
    /// </summary>
    public static void ScrollToElement(By locator)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        Log.Debug("Scrolling to element: {Locator}", locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        Thread.Sleep(300);
    }

    /// <summary>
    /// Scrolls to the top of the page
    /// </summary>
    public static void ScrollToTop()
    {
        Log.Debug("Scrolling to top of page");
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");
    }

    /// <summary>
    /// Scrolls to the bottom of the page
    /// </summary>
    public static void ScrollToBottom()
    {
        Log.Debug("Scrolling to bottom of page");
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
    }

    /// <summary>
    /// Highlights an element (useful for debugging)
    /// </summary>
    public static void HighlightElement(By locator)
    {
        var element = WaitHelper.WaitForElementExists(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].style.border='3px solid red';", element);
    }

    #endregion

    #region Window/Tab Operations

    /// <summary>
    /// Gets the current window handle
    /// </summary>
    public static string GetCurrentWindowHandle() => Driver.CurrentWindowHandle;

    /// <summary>
    /// Gets all window handles
    /// </summary>
    public static IReadOnlyCollection<string> GetAllWindowHandles() => Driver.WindowHandles;

    /// <summary>
    /// Switches to a new window/tab
    /// </summary>
    public static void SwitchToWindow(string windowHandle)
    {
        Log.Debug("Switching to window: {Handle}", windowHandle);
        Driver.SwitchTo().Window(windowHandle);
    }

    /// <summary>
    /// Switches to the last opened window/tab
    /// </summary>
    public static void SwitchToLastWindow()
    {
        var handles = Driver.WindowHandles;
        Log.Debug("Switching to last window");
        Driver.SwitchTo().Window(handles.Last());
    }

    /// <summary>
    /// Closes the current window and switches to the main window
    /// </summary>
    public static void CloseCurrentWindowAndSwitchToMain()
    {
        var mainHandle = Driver.WindowHandles.First();
        Driver.Close();
        Driver.SwitchTo().Window(mainHandle);
    }

    #endregion

    #region Frame Operations

    /// <summary>
    /// Switches to a frame by locator
    /// </summary>
    public static void SwitchToFrame(By locator)
    {
        var frame = WaitHelper.WaitForElementExists(locator);
        Log.Debug("Switching to frame: {Locator}", locator);
        Driver.SwitchTo().Frame(frame);
    }

    /// <summary>
    /// Switches to a frame by index
    /// </summary>
    public static void SwitchToFrame(int index)
    {
        Log.Debug("Switching to frame index: {Index}", index);
        Driver.SwitchTo().Frame(index);
    }

    /// <summary>
    /// Switches to the default content (main document)
    /// </summary>
    public static void SwitchToDefaultContent()
    {
        Log.Debug("Switching to default content");
        Driver.SwitchTo().DefaultContent();
    }

    #endregion

    #region Alert Operations

    /// <summary>
    /// Accepts an alert
    /// </summary>
    public static void AcceptAlert()
    {
        var alert = WaitHelper.WaitForAlert();
        Log.Debug("Accepting alert with text: {Text}", alert.Text);
        alert.Accept();
    }

    /// <summary>
    /// Dismisses an alert
    /// </summary>
    public static void DismissAlert()
    {
        var alert = WaitHelper.WaitForAlert();
        Log.Debug("Dismissing alert with text: {Text}", alert.Text);
        alert.Dismiss();
    }

    /// <summary>
    /// Gets the text from an alert
    /// </summary>
    public static string GetAlertText()
    {
        var alert = WaitHelper.WaitForAlert();
        return alert.Text;
    }

    /// <summary>
    /// Enters text into an alert prompt
    /// </summary>
    public static void EnterAlertText(string text)
    {
        var alert = WaitHelper.WaitForAlert();
        Log.Debug("Entering text into alert: {Text}", text);
        alert.SendKeys(text);
        alert.Accept();
    }

    #endregion

    #region Page Information

    /// <summary>
    /// Gets the current page title
    /// </summary>
    public static string GetPageTitle() => Driver.Title;

    /// <summary>
    /// Gets the current URL
    /// </summary>
    public static string GetCurrentUrl() => Driver.Url;

    /// <summary>
    /// Gets the page source
    /// </summary>
    public static string GetPageSource() => Driver.PageSource;

    #endregion
}
