using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.Utilities
{
    public class GetApp
    {
        public GetApp(string environment, string device, string defaultVariable)
        {
            if (IsBadString(environment) ||
                IsBadString(defaultVariable) ||
                IsBadString(device))
            {
                throw new ArgumentException($"{environment}, {defaultVariable}, {device} : Appium.SetConfig.GetApp may not be populated");
            }

            if (environment.ToLower().Equals("dogfood"))
            {
                if (device.ToLower().Equals("android"))
                {
                    App = "Android-0.1.0-SNAPSHOT-Internal.apk";
                }

                if (device.ToLower().Equals("ios"))
                {
                    App = "iOS-0.1.0-SNAPSHOT-Internal.ipa";
                }
            }
            else
            {
                App = defaultVariable;
            }

            if (IsBadString(App))
            {
                throw new ArgumentException($"{App} : App file is empty or null - check device names, check if app exists", App);
            }
        }

        internal string App { get; set; }

        public static GetApp Set(string environment, string device, string defaultVariable) =>
            new GetApp(environment, device, defaultVariable);

        private bool IsBadString(string check) =>
            string.IsNullOrEmpty(check) || (check.Length > 0 && char.IsHighSurrogate(check[check.Length - 1]));
    }
}
