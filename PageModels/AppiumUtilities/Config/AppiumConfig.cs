using PageModels.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PageModels.AppiumUtilities.Config
{
    public class AppiumConfig
    {
        /// <summary>
        /// Sets the environment when running without an environment value passed in
        /// </summary>
        private static string defaultEnvironment = "test"; // local, localvagrant, dev, test, dogfood...

        private readonly OperatingSystem operatingSystem;
        private readonly PlatformID platform;

        // paths relative to workspace
        private readonly string devicesDir = "devices.json";
        private readonly string appsDir = Path.Combine("Appium", "Apps");

        private readonly string buildDir;
        private readonly string solutionDir;
        private readonly string appPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumConfig"/> class.
        /// Configuration method for wiring up the information needed for the appium drivers and application
        /// </summary>
        /// <param name="deviceName">Name of the device</param>
        public AppiumConfig(string deviceName = null)
        {
            operatingSystem = Environment.OSVersion;
            platform = operatingSystem.Platform;

            buildDir = AppDomain.CurrentDomain.BaseDirectory;
            solutionDir = Directory.GetParent(buildDir).Parent.Parent.Parent.FullName;

            DeviceMap = GetMap.Get(devicesDir, deviceName).Map;
            appPath = SetAppPath.Get(DeviceMap).AppPath;

            DeviceMap.Add("app", appPath);
        }

        public static string DefaultEnvironment
        {
            get => defaultEnvironment;
            set => defaultEnvironment = value;
        }

        // deviceMap exposed from Config for DriverFactory
        public Dictionary<string, object> DeviceMap { get; set; }

        public static AppiumConfig Config() => new AppiumConfig();

        /// <summary>
        /// gets accesskey for kobiton run
        /// </summary>
        /// <returns></returns>
        public static string GetKobitonAccessKey()
        {
            var key = SetConfigObj.Get().CurrentSettings.AccessKey;
            if (string.IsNullOrEmpty(key))
            {
                key = "";
            }

            return key;
        }

        /// <summary>
        /// gets user name for kobiton run.
        /// </summary>
        /// <returns></returns>
        public static string GetKobitonUserName()
        {
            var user = SetConfigObj.Get().CurrentSettings.UserName;
            if (string.IsNullOrEmpty(user))
            {
                user = "Jenkins";
            }

            return user;
        }

        /// <summary>
        /// Get the URI value for standalone server from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetStandAloneServerUri() => "http://localhost:4444/wd/hub";

        /// <summary>
        /// Get the app path from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetAppPath() => SetConfigObj.Get().CurrentSettings.AppPath;

        /// <summary>
        /// Get the apk path from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetAndroidEmulatorApp()
        {
            string environment = GetEnvironment.Set("ENVIRONMENT", defaultEnvironment).Environment;
            return GetApp.Set(environment, GetDeviceName(), SetConfigObj.Get().CurrentSettings.Android).App;
        }

        /// <summary>
        /// Get the ipa path from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetiOSEmulatorApp()
        {
            string environment = GetEnvironment.Set("ENVIRONMENT", defaultEnvironment).Environment;
            return GetApp.Set(environment, GetDeviceName(), SetConfigObj.Get().CurrentSettings.IOS).App;
        }

        /// <summary>
        /// Get the apk path from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetAndroidRealApp()
        {
            string environment = GetEnvironment.Set("ENVIRONMENT", defaultEnvironment).Environment;
            return GetApp.Set(environment, GetDeviceName(), SetConfigObj.Get().CurrentSettings.Android).App;
        }

        /// <summary>
        /// Get the ipa path from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetiOSRealApp()
        {
            string environment = GetEnvironment.Set("ENVIRONMENT", defaultEnvironment).Environment;
            return GetApp.Set(environment, GetDeviceName(), SetConfigObj.Get().CurrentSettings.IOS).App;
        }

        /// <summary>
        /// Get the appId from the app.config file
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetAppId() => "com.spok.napa.test";

        /// <summary>
        /// set the device name.
        /// </summary>
        /// <returns>URI as string</returns>
        public static string GetDeviceName() => GetEnvironment.Set("DEVICE", SetConfigObj.Get().CurrentSettings.Devices).Environment;

        /// <summary>
        /// set the device name.
        /// </summary>
        /// <returns>URI as string</returns>
        public static TestProvider GetTestProvider()
        {
            var setting = GetEnvironment.Set("TESTPROVIDER", SetConfigObj.Get().CurrentSettings.TestProvider).Environment;

            // TODO: make parsing case insensitive
            if (Enum.GetNames(typeof(TestProvider)).Contains(setting))
            {
                return (TestProvider)Enum.Parse(typeof(TestProvider), setting);
            }
            else
            {
                throw new ArgumentException($"Invalid Test Provider {setting}");
            }
        }

        /// <summary>
        /// set the device platform.
        /// </summary>
        /// <returns>deviceplatform as an enum</returns>
        public static DevicePlatform GetDevicePlatform()
        {
            var setting = GetEnvironment.Set("DEVICEPLATFORM", SetConfigObj.Get().CurrentSettings.DevicePlatform).Environment;

            // TODO: make parsing case insensitive
            if (Enum.GetNames(typeof(DevicePlatform)).Contains(setting))
            {
                return (DevicePlatform)Enum.Parse(typeof(DevicePlatform), setting);
            }
            else
            {
                throw new ArgumentException($"Invalid Device Platform {setting}");
            }
        }

        /// <summary>
        /// Get the server wait value which controls how long the test will wait for grabbing a selenium grid node from the app config
        /// </summary>
        /// <returns>URI as string</returns>
        public static TimeSpan GetServerWaitValue() => TimeSpan.FromSeconds(int.Parse("1200"));

        /// <summary>
        /// Get the wait value for interacting between elements in a test from the app config
        /// </summary>
        /// <returns>URI as string</returns>
        public static TimeSpan GetWaitValue() => TimeSpan.FromSeconds(int.Parse("60"));
    }
}

