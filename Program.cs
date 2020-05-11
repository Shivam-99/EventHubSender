using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace EventHubSender
{
    class Program
    {
        private const string connectionString = "Endpoint=sb://eventhubdemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6KamFRhaTkuktoSxD2/DNwTlSg2d0JhHyUcSs+JUeik=";
        private const string eventHubName = "eventhubtest";
        static void Main()
        {
            int randomNumber = RandomNumber(0,100000); 
            DateTime today = DateTime.Today;
            var data = new { today = today.ToString("dd-MM-yy"), randomNumber };
            Console.WriteLine(data.ToString()); 
            SendData(data.ToString()).Wait();
            Console.ReadLine();
        }

        public static async Task SendData(string data)
        {
            await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
            {
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
                
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(data)));
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine("Data has been sent");
            }
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
