using Api.ConfigFile.Operations.Model;

namespace Api.ConfigFile.Operations.Services;

public class SettingsService : ISettingsService
{
    private readonly ConfigFileHandler _configFileHandler;


    public SettingsService( ConfigFileHandler configFileHandler)
    {
        _configFileHandler = configFileHandler;
    }

    public ConfigModel GetConfigSettings(string serverName)
    {
        return _configFileHandler.ReadConfigSettings().GetValueOrDefault(serverName, new ConfigModel());
    }

    public IEnumerable<string> GetServerNames()
    {
        return _configFileHandler.ReadConfigSettings().Keys;
    }

    public void UpdateConfigSettings(string serverName, Dictionary<string, string> updatedConfig)
    {
        _configFileHandler.UpdateConfigSettings(serverName, updatedConfig);
    }

    public void AddConfigSettings(string serverName, Dictionary<string, string> newConfig)
    {
        _configFileHandler.AddConfigSettings(serverName, newConfig);
    }
}