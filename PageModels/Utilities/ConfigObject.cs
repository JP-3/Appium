using System;
using System.Collections.Generic;
using System.Text;

namespace PageModels.Utilities
{
    public class ConfigObj
    {
        public ConfigObj(string appPath, string devices, string userName, string accessKey, string ios, string android, string devicePlatform = null, string testProvider = null)
        {
            if (IsBadString(appPath) ||
                IsBadString(devices) ||
                IsBadString(android) ||
                IsBadString(ios))
            {
                throw new ArgumentException();
            }

            AppPath = appPath;
            Devices = devices;
            AccessKey = accessKey;
            UserName = userName;
            IOS = ios;
            Android = android;

            DevicePlatform = devicePlatform;
            TestProvider = testProvider;
        }

        internal string AppPath { get; set; }

        internal string Devices { get; set; }

        internal string AccessKey { get; set; }

        internal string UserName { get; set; }

        internal string IOS { get; set; }

        internal string Android { get; set; }

        internal string DevicePlatform { get; set; }

        internal string TestProvider { get; set; }

        public static ConfigObj Get(string appPath, string devices, string userName, string accessKey, string iosRealApp, string androidRealApp, string devicePlatform, string testProvider) =>
            new ConfigObj(appPath, devices, userName, accessKey, iosRealApp, androidRealApp, devicePlatform, testProvider);

        private bool IsBadString(string check) =>
            string.IsNullOrEmpty(check) ||
                (check.Length > 0 && char.IsHighSurrogate(check[check.Length - 1]));
    }
}
