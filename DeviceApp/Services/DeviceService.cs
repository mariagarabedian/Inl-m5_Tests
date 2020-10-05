using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace DeviceApp.Services
{
    public class DeviceService
    {
        public static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=ec-win20-min-iothub.azure-devices.net;DeviceId=DeviceApp;SharedAccessKey=9L9oQeIFiHKmNUiCbtUz/58TLtHhCi7cGFn1nWp4omg=", TransportType.Mqtt);
        public static int telemetryInterval = 5;
        public static Random rnd = new Random();

        
        // En funktion som sätter telementryintervallen så att vi kan ändra tiden
        public static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext)
        {
            // GetString från request.Data man packar upp den
            // Replace Ersätter " " med ett tom dvs tar bort " " citationstecken för att applikationen DeviceExlorer sätter " "
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"", "");

            // Skriver ut det som finns i payload
            Console.WriteLine(payload); // skriver ut: 10 (som är telemetryintervalet)

            // Vi kommer här att testa inputen vi från in i vår Data om det är ett tal eller inte
            // Vi får in en text och vi ska försöka göra om den till ett tal genom TryParse som 
            //kommer att ge oss ett true eller false värde.
            // Försök göra om payload till int om den lyckas skicka ut värdet till telementryInterval
            // out gör att vi kan sätta om en redan befintlig variabel som finns utanför funktionen
            if (Int32.TryParse(payload, out telemetryInterval))
            {
                // Skriver ut i consolen det nya värdet om den lyckas
                Console.WriteLine($"Interval set to: {telemetryInterval} seconds");

                // Har byggt upp ett json meddelande som kommer att ge / skapas:
                // {"result" : " Executed direct method: SetTelementryInterval"} (i huben)
                string json = "{\"result\":\"Executed direct method:" + request.Name + "\"}";

                // skapar en methodresponse med GetBytes från json med statuskod 200 och 
                // returnerar den
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                // Har byggt upp ett json meddelande som kommer att ge / skapas:
                // {"result" : " Method not implemented: "}
                string json = "{\"result\":\"Method not implemented:\"}";

                // skapar en methodresponde med GetBytes från json med statuskod 501 och 
                // returnerar den
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }


        }

        // En metod för att kunna skicka iväg ett meddelande Till consolen
        public static async Task SendMessageAsync()
        {
            while (true)
            {
               
                double temp = 10 + rnd.NextDouble() * 15;
                double hum = 40 + rnd.NextDouble() * 20;

                // skapar ett objekt
                // en variabel som innehåller två värden som ett objekt
                var data = new
                {
                    temperature = temp,
                    humidity = hum
                };

                var json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));
              
                // Använder if-sats om temp är över 30 då ska det vara true annars är det false
                // Properties läser bara sträng och därför sätter true och fals som strängar
                payload.Properties.Add("temperatureAlert", (temp > 30) ? "true" : "false");

                // skickar payload
                await deviceClient.SendEventAsync(payload);

                // för att göra det synligt skriver ut det i consolen
                Console.WriteLine($"Message sent: {json}");

                // För att SendMessage ska dröja
                await Task.Delay(telemetryInterval * 1000);
            }

        }

    }
}
