using Microsoft.Azure.Devices;
using System;
using System.Threading.Tasks;

namespace ServiceApp
{
    class Program
    {
       
        
        // Skapar en instans av ServiceClient och samma connection string som jag har till DeviceExplorer
        // Det vi gör är en consoleapplikation som DeviceExplorer
        private static ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("HostName=ec-win20-min-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=dA/1QXMg3bUIuienhwNoGGk/py1OkiOhgbnZV8J8fvY=");
        static void Main(string[] args)
        {
            // pausar i 5 sek för att vi ska hinna se vad som händer
            Task.Delay(5000).Wait();
            // ( deviceId , methodName , payload ) paylod skrivs i text
            InvokeMethod("DeviceApp", "SetTelemetryInterval", "10").GetAwaiter();

            Console.ReadKey();
        }

        // Den här metoden motsvarar i DeviceExplorer "Call Method on Device"
        static async Task InvokeMethod(string deviceId, string methodName, string payload)
        {
            // Skapar en methodInvocation som är (SetTelemetryInterval)/ methodName och sätter en ResponseTimeout på den
            //Dvs att den har en levnadstid på 30 sek och sedan kommer den att timeouta
            var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            // gör payload till jason
            methodInvocation.SetPayloadJson(payload);
           
            // Skickar till (DeviceApp)/ deviceId, methodinvokation
            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);

            Console.WriteLine($"Response Status: { response.Status}");
            Console.WriteLine(response.GetPayloadAsJson());


        }
    }
    
}
