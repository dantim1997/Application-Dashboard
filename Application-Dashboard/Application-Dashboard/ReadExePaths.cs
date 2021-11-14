using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Dashboard
{
    public static class ReadExePaths
    {
        public static List<Temp> ReadAllPaths()
        {
            List<string> apps = new();

            var file = System.IO.File.ReadAllText(@"Path.json");
            var output = JsonConvert.DeserializeObject<List<Temp>>(file);
            var dictionary = output.ToDictionary(x => x.Key, y => y.Value);
            return output;
        }
        

        public class Temp
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
