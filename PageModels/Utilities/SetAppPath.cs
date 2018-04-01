using PageModels.AppiumUtilities.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PageModels.Utilities
{
    public class SetAppPath
    {
        private Dictionary<string, object> deviceMap;

        public SetAppPath(Dictionary<string, object> device_map)
        {
            deviceMap = device_map;
            AppPath = SettingAppPath();

            if (IsBadString(AppPath))
            {
                throw new ArgumentException("Failed to set the App Path!");
            }
        }

        internal string AppPath { get; set; }

        public static SetAppPath Get(Dictionary<string, object> device_map) =>
            new SetAppPath(device_map);

        public string SettingAppPath()
        {
            bool checkAndroid = deviceMap.Any(o => Regex.IsMatch(o.Value.ToString(), @"([Aa][Nn][Dd][Rr][Oo][Ii][Dd])"));
            bool checkMAC = deviceMap.Any(o => Regex.IsMatch(o.Value.ToString(), @"([Mm][Aa][Cc])"));
            bool checkReal = deviceMap.Any(o => Regex.IsMatch(o.Value.ToString(), @"([Rr][Ee][Aa][Ll])"));
            bool checkEmulator = deviceMap.Any(o => Regex.IsMatch(o.Value.ToString(), @"([Ee][Mm][Uu][Ll][Aa][Tt][Oo][Rr])"));

            if (checkAndroid && checkReal)
            {
                return Path.Combine(AppiumConfig.GetAppPath(), AppiumConfig.GetAndroidRealApp());
            }

            if (checkAndroid && checkEmulator)
            {
                return Path.Combine(AppiumConfig.GetAppPath(), AppiumConfig.GetAndroidEmulatorApp());
            }

            if (checkMAC && checkReal)
            {
                return Path.Combine(AppiumConfig.GetAppPath(), AppiumConfig.GetiOSRealApp());
            }

            if (checkMAC && checkEmulator)
            {
                return Path.Combine(AppiumConfig.GetAppPath(), AppiumConfig.GetiOSEmulatorApp());
            }

            throw new ArgumentException("Failed to set the App Path!");
        }

        private bool IsBadString(string check) =>
            string.IsNullOrEmpty(check) ||
                (check.Length > 0 && char.IsHighSurrogate(check[check.Length - 1]));
    }
}
