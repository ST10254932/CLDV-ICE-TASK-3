using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using System;
using System.IO;
using System.Text;

namespace ICETASK3APP2
{

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=cldvicetask3;AccountKey=IsPZHd8N+rqiTrWT7z9zC9krHwSNLiJxNT1DCwyzmFI734lvanaEb2tTJ4ZK/KBlDFXQ4FomSzh0+ASt2atV4Q==;EndpointSuffix=core.windows.net";
            string queueName = "icetask3";
            string shareName = "lebohangtsotetsi";
            string fileName = "Lebohang Tsotetsi.txt";

            var queueClient = new QueueClient(connectionString, queueName);
            var shareClient = new ShareClient(connectionString, shareName);
            var directoryClient = shareClient.GetRootDirectoryClient();

            // Ensure the file share exists
            shareClient.CreateIfNotExists();
            var fileClient = directoryClient.GetFileClient(fileName);

            // Create the file in the file share
            fileClient.Create(0);

            var builder = new StringBuilder();
            while (queueClient.Exists() && queueClient.PeekMessage() != null)
            {
                // Retrieve and delete the message
                var message = queueClient.ReceiveMessage();
                if (message != null)
                {
                    var fibonacciNumber = message.Value.MessageText;
                    builder.AppendLine(fibonacciNumber);
                    Console.WriteLine($"Processed Fibonacci number: {fibonacciNumber}");

                    // Delete the message after processing
                    queueClient.DeleteMessage(message.Value.MessageId, message.Value.PopReceipt);
                }
            }

            // Convert StringBuilder content to a byte array
            byte[] fileContent = Encoding.UTF8.GetBytes(builder.ToString());

            // Upload the file content to Azure File Storage
            using (var stream = new MemoryStream(fileContent))
            {
                fileClient.UploadRange(
                    new Azure.HttpRange(0, fileContent.Length),
                    stream);
            }

            Console.WriteLine($"File {fileName} saved to Azure File Storage.");
        }
    }
}