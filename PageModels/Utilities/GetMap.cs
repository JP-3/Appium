using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PageModels.Utilities
{
    public class GetMap
    {
        private GetMap(string path_dir, string objname)
        {
            if (IsBadString(path_dir) ||
                IsBadString(objname))
            {
                throw new ArgumentException("Paths to the devices.json, or SettingsConfig.json are incorrect or missing.");
            }

            JObject obj = JObject.Parse(File.ReadAllText("/Users/kylehansen/Desktop/Appium_Demo/v2/Appium/Appium_Demo/" + path_dir));
            var node = obj[objname];
            try
            {
                Map = node.ToObject<Dictionary<string, object>>();
            }
            catch (NullReferenceException)
            {
                throw new Exception($"In SettingsConfig.json profile {objname} was not found");
            }
        }

        internal Dictionary<string, object> Map { get; set; }

        public static GetMap Get(string path_dir, string objname) =>
            new GetMap(path_dir, objname);

        private bool IsBadString(string check) =>
            string.IsNullOrEmpty(check) ||
                (check.Length > 0 && char.IsHighSurrogate(check[check.Length - 1]));
    }
}
