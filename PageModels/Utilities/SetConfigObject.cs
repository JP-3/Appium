using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PageModels.Utilities
{
    public class SetConfigObj
    {
        private const string MachineOverride = "MachineOverride";
        private const string CurrentApplicationPathOverride = "CurrentApplicationPath";
        private const string CurrentIosApplicationNameOverride = "CurrentiOSApplicationName";
        private const string CurrentAndroidApplicationNameOverride = "CurrentAndroidApplicationName";
        private const string CurrentTestDevicePlatformOverride = "CurrentTestDevicePlatform";
        private const string CurrentTestProviderOverride = "CurrentTestProvider";

        private static string buildDir = AppDomain.CurrentDomain.BaseDirectory;
        private static string solutionDir = Directory.GetParent(buildDir).Parent.Parent.Parent.FullName;
        private static string path = "settings.json";
        private string system = Environment.MachineName;

        private SetConfigObj()
        {
            var systemOverride = Environment.GetEnvironmentVariable(MachineOverride);
            if (!string.IsNullOrEmpty(systemOverride))
            {
                system = systemOverride;
            }

            CurrentSettings = GetSettingForMachine();
        }

        public ConfigObj CurrentSettings { get; set; }

        public static SetConfigObj Get() => new SetConfigObj();

        private ConfigObj GetSettingForMachine()
        {
            var map = GetMap.Get(path, system).Map;

            var currentApplicationPath = Environment.GetEnvironmentVariable(CurrentApplicationPathOverride);
            var currentIosApplicationName = Environment.GetEnvironmentVariable(CurrentIosApplicationNameOverride);
            var currentAndroidApplicationName = Environment.GetEnvironmentVariable(CurrentAndroidApplicationNameOverride);
            var currentTestDevicePlatform = Environment.GetEnvironmentVariable(CurrentTestDevicePlatformOverride);
            var currentTestProvider = Environment.GetEnvironmentVariable(CurrentTestProviderOverride);

            var setting = ConfigObj.Get(
                string.IsNullOrEmpty(currentApplicationPath) ? map.First(s => s.Key.Contains("appPath")).Value.ToString() : currentApplicationPath,
                map.First(s => s.Key.Contains("devices")).Value.ToString(),
                map.First(s => s.Key.Contains("userName")).Value.ToString(),
                map.First(s => s.Key.Contains("accessKey")).Value.ToString(),
                string.IsNullOrEmpty(currentIosApplicationName) ? map.First(s => s.Key.Contains("ios")).Value.ToString() : currentIosApplicationName,
                string.IsNullOrEmpty(currentAndroidApplicationName) ? map.First(s => s.Key.Contains("android")).Value.ToString() : currentAndroidApplicationName,
                string.IsNullOrEmpty(currentTestDevicePlatform) ? map.First(s => s.Key.Contains("devicePlatform")).Value.ToString() : currentTestDevicePlatform,
                string.IsNullOrEmpty(currentTestProvider) ? map.First(s => s.Key.Contains("testProvider")).Value.ToString() : currentTestProvider);

            return setting;
        }
    }
}

