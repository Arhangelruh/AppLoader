using AppLoader.Model;
using System.Text.Json;

namespace AppLoader.Services
{
    internal class ReadConfig
    {
        public Property readFile()
        {
            string jsonString = File.ReadAllText("AppLoader.json");
            var configs = JsonSerializer.Deserialize<Property>(jsonString);
            return configs;
        }
    }
}
