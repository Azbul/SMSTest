namespace Task2.Services;

using Microsoft.Extensions.Configuration;

public class ConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public string[] GetEnvironmentVariables()
    {
        return _configuration.GetSection("EnvironmentVariables").Get<string[]>() ?? [];
    }
}
