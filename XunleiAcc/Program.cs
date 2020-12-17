using Newtonsoft.Json.Linq;
using System;
using System.Threading;

namespace XunleiAcc
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceInfo.INIT();
            XLAuthClient xla = new XLAuthClient();
            xla.login("13914084591", "Ken1250542735");
            XLAccClient acc = new XLAccClient(xla);
            //Thread.Sleep(100000);
            //xla.renew_stat();
            JObject jb = acc.getBandwidthInfo();
            Console.ReadLine();
        }
    }
}
