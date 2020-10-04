using System;
using System.Text;
using System.Threading.Tasks;
using DeviceApp.Services;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DeviceApp
{
    class Program
    {
        

        static void Main(string[] args)
        {
            DeviceService.deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", DeviceService.SetTelemetryInterval, null).GetAwaiter();
            DeviceService.SendMessageAsync().GetAwaiter();

            Console.ReadKey();
        }
        
    }
}
