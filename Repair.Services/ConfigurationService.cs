using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repair.Services;

/// <summary>
/// Handles application configuration loading, file creation and database option setup.
/// </summary>
public class ConfigurationService
{
    private const string SHARED_ROOT_FOLDER_NAME = "Fang Software";
    private const string APP_FOLDER_NAME = "Odense Mobil Rep";
    private const string APP_SETTINGS_FILE = "appsettings.json";
    private const string DATABASE_FILE_NAME = "repair.db";

    private IConfiguration? configuration;

    public ConfigurationService()
    {
    }

    private string GetExecutionDirectory()
    {
        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Builds and returns the application configuration from the local app data folder.
    /// </summary>
    public IConfiguration BuildConfiguration()
    {
        IConfigurationBuilder configBuilder = new ConfigurationBuilder();

        configBuilder.AddEnvironmentVariables();

        if (!IsRunningInCi())
        {
            EnsureAppSettingsFileExists();

            string fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

            configBuilder.AddJsonFile(fullAppFilePath, false, true);
        }

        configuration = configBuilder.Build();
        return configuration;
    }

    private bool IsRunningInCi()
    {
        return string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Configures the database options.
    /// </summary>
    public void ConfigureDatabaseOptions(DbContextOptionsBuilder options)
    {
        Directory.CreateDirectory(GetApplicationDataPath());

        string databasePath = Path.Combine(GetApplicationDataPath(), DATABASE_FILE_NAME);

        options.UseSqlite($"Data Source={databasePath}");

#if DEBUG
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
#endif
    }

    /// <summary>
    /// Returns the application data path used for configuration files.
    /// </summary>
    public string GetApplicationDataPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            SHARED_ROOT_FOLDER_NAME, APP_FOLDER_NAME);
    }

    /// <summary>
    /// Ensures the appsettings file exists, creating an empty one if needed.
    /// </summary>
    private void EnsureAppSettingsFileExists()
    {
        string fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

        if (File.Exists(fullAppFilePath))
        {
            return;
        }

        var template = new { };

        CreateFile(fullAppFilePath, template);
    }

    /// <summary>
    /// Creates a JSON file with the supplied template content.
    /// </summary>
    private void CreateFile(string path, object template)
    {
        string templateContent = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, templateContent);
    }
}
