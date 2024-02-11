
namespace Api.ConfigFileReader.Services
{
    public class AllSettingsConfigReader : IConfigReader

    {
        public SettingsType Type => SettingsType.None;

        public Dictionary<string, string> ReadConfigSettings()
        {
            var settings = new Dictionary<string, string>();

            try
            {
                var lines = File.ReadAllLines("config-file.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        settings[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading settings file: {ex.Message}");
            }

            return settings;
        }
    }
}
