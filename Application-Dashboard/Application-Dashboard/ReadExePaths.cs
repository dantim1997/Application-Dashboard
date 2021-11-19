using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Application_Dashboard.Models;

namespace Application_Dashboard
{
    public static class ReadExePaths
    {
        public static List<Apps> ReadAllPaths()
        {
            // would like to have an other way to store the paths-- maybe in the future he looks for all the applications?
            var file = System.IO.File.ReadAllText(@"Path.json");
            var output = JsonConvert.DeserializeObject<List<Apps>>(file);
            return output;
        }

        public static string GetIcons(Apps keyValuePair)
        {
            var filePath = keyValuePair.Value;
            string base64;
            var result = (Icon)null;

            try
            {
                byte[] bytes;
                result = Icon.ExtractAssociatedIcon(filePath);
                using (var ms = new MemoryStream())
                {
                    result.Save(ms);
                    base64 = Convert.ToBase64String(ms.ToArray());

                    bytes = ms.ToArray();
                }
                return base64;
                //return Convert.ToBase64String(bytes);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }
            return "";
        }
    }
}
