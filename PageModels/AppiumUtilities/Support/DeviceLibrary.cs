using System;
using System.Collections.Generic;

namespace PageModels.AppiumUtilities.Support
{
    public class DeviceLibrary
    {
        private List<DeviceRecord> deviceList = new List<DeviceRecord>();

        private object sync = new object();

        public void SetDeviceLibrary(string[] devices)
        {
            lock (sync)
            {
                if (deviceList.Count == 0)
                {
                    foreach (string device in devices)
                    {
                        deviceList.Add(new DeviceRecord(device));
                    }
                }
                else
                {
                    Console.WriteLine("Fixture");
                }
            }
        }

        public void AddDeviceToLibrary(string deviceName)
        {
            deviceList.Add(new DeviceRecord(deviceName));
        }

        public void PrintDevices()
        {
            lock (sync)
            {
                Console.WriteLine("Printing out device library");
                foreach (DeviceRecord device in deviceList)
                {
                    device.Info();
                }
            }
        }

        public string CheckoutDevice(string testName)
        {
            lock (sync)
            {
                try
                {
                    foreach (DeviceRecord device in deviceList)
                    {
                        if (device.GetCheckedToo().Equals(string.Empty))
                        {
                            device.CheckoutDeivce(testName);
                            Console.WriteLine(device.GetDeviceName() + "Device CHECKEDOUT!!!!!!!!!!!");
                            return device.GetDeviceName();
                        }
                    }

                    Console.WriteLine("All devices are checked out");
                    PrintDevices();
                    return string.Empty;
                }
                catch (Exception e)
                {
                    throw new Exception($"ERROR: Couldn't checkout device \nEXCEPTION {e}");
                }
            }
        }

        public void CheckinDevice(string testName)
        {
            lock (sync)
            {
                try
                {
                    foreach (DeviceRecord device in deviceList)
                    {
                        if (device.GetCheckedToo().Equals(testName))
                        {
                            device.ReturnDevice();
                            Console.WriteLine("Device RETURNED!!!!!!!!!!!");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"ERROR: Couldn't checkin device \nException {e}");
                }
            }
        }
    }
}