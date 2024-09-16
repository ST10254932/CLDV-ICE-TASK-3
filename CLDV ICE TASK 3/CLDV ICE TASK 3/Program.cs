using Azure.Storage.Queues;
using System;

namespace CLDV_ICE_TASK_3
{

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=cldvicetask3;AccountKey=IsPZHd8N+rqiTrWT7z9zC9krHwSNLiJxNT1DCwyzmFI734lvanaEb2tTJ4ZK/KBlDFXQ4FomSzh0+ASt2atV4Q==;EndpointSuffix=core.windows.net";
            string queueName = "icetask3";
            var queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't already exist
            queueClient.CreateIfNotExists();

            if (queueClient.Exists())
            {
                // Generate the Fibonacci sequence up to 233
                int a = 0, b = 1, temp = 0;
                while (a <= 233)
                {
                    // Send each Fibonacci number as a message
                    queueClient.SendMessage(a.ToString());
                    Console.WriteLine($"Added Fibonacci number: {a}");

                    // Calculate the next number in the sequence
                    temp = a;
                    a = b;
                    b = temp + b;
                }
            }
            Console.WriteLine("Fibonacci sequence generation and storage complete.");
        }
    }
}