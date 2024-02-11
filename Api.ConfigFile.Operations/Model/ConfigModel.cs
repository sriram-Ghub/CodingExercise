namespace Api.ConfigFile.Operations.Model;

public class ConfigModel
{
    public ConfigModel()
    {
        Configs = new Dictionary<string, string>();
    }
    
    public Dictionary<string, string>? Configs { get; set; }
}