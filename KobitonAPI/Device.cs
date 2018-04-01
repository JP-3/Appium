using System;
using System.Collections.Generic;
using System.Text;

namespace KobitonAPI
{
    public class Device
    {
        public int Id { get; set; }

        public bool IsBooked { get; set; }

        public bool IsHidden { get; set; }

        public bool IsOnline { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsCloud { get; set; }

        public bool IsMyOrg { get; set; }

        public bool IsMyOwn { get; set; }

        public bool AppiumDisabled { get; set; }

        public string ModelName { get; set; }

        public string DeviceName { get; set; }

        public string PlatformName { get; set; }

        public string PlatformVersion { get; set; }

        public string DeviceImageUrl { get; set; }
    }
}
