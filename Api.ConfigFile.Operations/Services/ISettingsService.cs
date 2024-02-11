using Api.ConfigFile.Operations.Model;

namespace Api.ConfigFile.Operations.Services;

public interface ISettingsService
{
    ConfigModel GetConfigSettings(string serverName);
    IEnumerable<string> GetServerNames();
    void UpdateConfigSettings(string serverName, Dictionary<string, string> updatedConfig);
    void AddConfigSettings(string serverName, Dictionary<string, string> updatedConfig);    
}