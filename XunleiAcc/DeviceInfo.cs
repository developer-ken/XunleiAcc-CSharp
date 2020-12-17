using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace XunleiAcc
{
    class DeviceInfo
    {
        public const string APP_VERSION = "2.4.1.3";
        public const short VASID_DOWN = 14; // vasid for downstream accel
        public const string FALLBACK_MAC = "000000000000";

        public const string DEVICE = "Microstorm S1";
        public const string DEVICE_MODEL = "S1";
        public const string OS_VERSION = "10.0.1";
        public const string OS_API_LEVEL = "29";
        public const string OS_BUILD = "PUA66B";

        public const short PROTOCOL_VERSION = 200;

        public static string mac;

        public static void INIT()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            mac = nics[0].GetPhysicalAddress().ToString();
        }
    }
}
