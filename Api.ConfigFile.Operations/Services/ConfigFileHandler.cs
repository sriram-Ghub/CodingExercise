using Api.ConfigFile.Operations.Model;

namespace Api.ConfigFile.Operations.Services
{
    public class ConfigFileHandler
    {
        private readonly string _configFileTxt;
        private const string DefaultConfigFileName = "config-file.txt";
        private const string ValueDelimiter = "=";

        public ConfigFileHandler()
        { 
            _configFileTxt = DefaultConfigFileName;
        }

        public ConfigFileHandler(string fileName)
        {
            _configFileTxt = string.IsNullOrEmpty(fileName) ? DefaultConfigFileName : fileName;
        }

        public Dictionary<string, ConfigModel> ReadConfigSettings()
        {
            var serverConfigs = new Dictionary<string, ConfigModel>();

            try
            {
                var lines = File.ReadAllLines(_configFileTxt);
                foreach (var line in lines)
                {
                    if (IsCommentLineOrEmptyLine(line)) continue;

                    var parts = line.Split(ValueDelimiter);
                    var keyAndServerName = parts[0].Split('{');

                    var key = keyAndServerName[0];
                    var value = parts[1];

                    var serverName = keyAndServerName.Length == 1
                        ? "default"
                        : keyAndServerName[1].Split('}')[0];

                    var configModel = serverConfigs.GetValueOrDefault(serverName, new ConfigModel());

                    if (!serverConfigs.ContainsKey(serverName))
                    {
                        serverConfigs.Add(serverName, configModel);
                    }

                    configModel.Configs?.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading settings file: {ex.Message}");
            }

            return serverConfigs;
        }
        
        public void UpdateConfigSettings(string serverName, Dictionary<string, string> updatedConfig)
        {
            if (!serverName.Equals("default"))
            {
                return;
            }

            try
            {
                var linesReplaced = 0;
                var lines = File.ReadAllLines(_configFileTxt);
                
                for (var i = 0; i < lines.Length; i++)
                {
                    if (linesReplaced > updatedConfig.Count) break;
                    if (IsCommentLineOrEmptyLine(lines[i])) continue;

                    var key = lines[i].Split(ValueDelimiter)[0];
                    if (!updatedConfig.ContainsKey(key)) continue;
                    var oldValue = lines[i].Split(ValueDelimiter)[1];
                    lines[i] = lines[i].Replace(oldValue, updatedConfig.GetValueOrDefault(key));
                    linesReplaced++;
                }

                WriteToFile(lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading settings file: {ex.Message}");
            }
        }

        public void AddConfigSettings(string serverName, Dictionary<string, string> configsToAdd)
        {
            try
            {
                var fullConfig = new List<string>();
                var configsAdded = false;
                var lines = File.ReadAllLines(_configFileTxt);

                foreach (var line in lines)
                {
                    fullConfig.Add(line);
                    if(IsCommentLineOrEmptyLine(line)) continue;
                    
                    if ((serverName.Equals("default") || line.Contains(serverName)) && !configsAdded)
                    {
                        foreach (var lineToAdd in configsToAdd.Select(config => serverName.Equals("default")
                                     ? config.Key + "=" + config.Value
                                     : config.Key + "{" + serverName + "}" + "=" + config.Value))
                        {
                            fullConfig.Add(lineToAdd);
                            configsAdded = true;
                        }
                    }
                }

                WriteToFile(fullConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading settings file: {ex.Message}");
            }
        }

        private void WriteToFile(IEnumerable<string> fullConfig)
        {
            using (StreamWriter writer = new(_configFileTxt))
            {
                foreach (var line in fullConfig)
                {
                    writer.WriteLine(line);
                }
            }
        }
        
        private static bool IsCommentLineOrEmptyLine(string line)
        {
            return line.StartsWith(";") || string.IsNullOrWhiteSpace(line);
        }
    }
}