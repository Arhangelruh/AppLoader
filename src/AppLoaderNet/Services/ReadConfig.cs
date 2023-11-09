﻿using AppLoaderNet.Model;
using Newtonsoft.Json;
using System.IO;

namespace AppLoaderNet.Services
{
    internal class ReadConfig
    {
        public Property readFile()
        {
            string jsonString = File.ReadAllText("AppLoaderNet.json");
            var configs = JsonConvert.DeserializeObject<Property>(jsonString);
            return configs;
        }
    }
}
