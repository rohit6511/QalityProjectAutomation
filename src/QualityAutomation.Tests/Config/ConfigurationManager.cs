using Microsoft.Extensions.Configuration;

namespace QualityAutomation.Tests.Config;

/// <summary>
/// Singleton configuration manager that loads and provides access to application settings
/// </summary>
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _instance = 
        new(() => new ConfigurationManager());
    
    private readonly IConfiguration _configuration;
    private readonly AppSettings _appSettings;

    public static ConfigurationManager Instance => _instance.Value;

    public TestSettings TestSettings => _appSettings.TestSettings;
    public ReportSettings ReportSettings => _appSettings.ReportSettings;
    public LogSettings LogSettings => _appSettings.LogSettings;

    private ConfigurationManager()
    {
        var basePath = GetConfigBasePath();
        
        _configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"Config/appsettings.{Environment.GetEnvironmentVariable("TEST_ENV") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        _appSettings = new AppSettings();
        _configuration.Bind(_appSettings);
        
        ApplyEnvironmentOverrides();
    }

    private static string GetConfigBasePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        
        if (File.Exists(Path.Combine(currentDirectory, "Config", "appsettings.json")))
            return currentDirectory;
        
        var assemblyLocation = Path.GetDirectoryName(typeof(ConfigurationManager).Assembly.Location);
        if (!string.IsNullOrEmpty(assemblyLocation) && 
            File.Exists(Path.Combine(assemblyLocation, "Config", "appsettings.json")))
            return assemblyLocation;
        
        return currentDirectory;
    }

    private void ApplyEnvironmentOverrides()
    {
        var browserEnv = Environment.GetEnvironmentVariable("BROWSER");
        if (!string.IsNullOrEmpty(browserEnv))
            _appSettings.TestSettings.Browser = browserEnv;

        var headlessEnv = Environment.GetEnvironmentVariable("HEADLESS");
        if (bool.TryParse(headlessEnv, out var headless))
            _appSettings.TestSettings.Headless = headless;

        var baseUrlEnv = Environment.GetEnvironmentVariable("BASE_URL");
        if (!string.IsNullOrEmpty(baseUrlEnv))
            _appSettings.TestSettings.BaseUrl = baseUrlEnv;
    }

    public T GetSection<T>(string sectionName) where T : new()
    {
        var section = new T();
        _configuration.GetSection(sectionName).Bind(section);
        return section;
    }

    public string? GetValue(string key) => _configuration[key];
}
