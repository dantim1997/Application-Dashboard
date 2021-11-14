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
        public static List<KeyValuePair> ReadAllPaths()
        {
            // would like to have an other way to store the paths-- maybe in the future he looks for all the applications?
            var file = System.IO.File.ReadAllText(@"Path.json");
            var output = JsonConvert.DeserializeObject<List<KeyValuePair>>(file);
            return output;
        }
        
        public class KeyValuePair
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
