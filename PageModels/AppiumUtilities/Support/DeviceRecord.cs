using System;

namespace PageModels.AppiumUtilities.Support
{
    public class DeviceRecord
    {
        private string deviceName = string.Empty;

        private string checkedToo = string.Empty;

        private DateTime? timeCheckedOut = null;

        public DeviceRecord(string deviceName) => this.deviceName = deviceName;

        public void Info() => Console.WriteLine($"Record - \n\tDeviceName: '{deviceName}' \n\tCheckOutToo: '{checkedToo}' \n\tTimeCheckedOut: '{timeCheckedOut}'");

        public string GetCheckedToo() => checkedToo;

        public string GetDeviceName() => deviceName;

        public void ReturnDevice()
        {
            checkedToo = string.Empty;
            timeCheckedOut = null;
        }

        public string CheckoutDeivce(string testName)
        {
            checkedToo = testName;
            timeCheckedOut = DateTime.Now;
            return deviceName;
        }
    }
}