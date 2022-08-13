using Grpc.Core;
using FullDuplex.Protos;
using System.Diagnostics;
using LiteDB;

namespace FullDuplex.Services
{

    public class MessageModel
    {
        public int Id { get; set; }
        public string Messages { get; set; }
        public bool isRead { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class MessagingService : Messaging.MessagingBase
    {

        private readonly ILogger<MessagingService> _logger;
        public string message;
        int waitTimeInSeconds = 20;

        const string DEVELOPMENT = "..\\TaskComponent\\obj\\x64\\Release\\TaskComponent.exe";
        //const string localpublish = "D:\\NewGRPC\\FullDuplex\\bin\\Release\\net6.0\\publish\\TaskComponent.exe";

        public MessagingService(ILogger<MessagingService> logger)
        {
            _logger = logger;
        }

        public override async Task CreateStreaming(IAsyncStreamReader<SxMessage> requestStream, IServerStreamWriter<SxMessage> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Start CreateStreaming Method");

            try
            {
                if (context.CancellationToken.IsCancellationRequested) return;

                while (await requestStream.MoveNext()
                  && !context.CancellationToken.IsCancellationRequested)
                {
                    // read incoming message
                    _logger.LogInformation("Reading Requests from Client");
                    var current = requestStream.Current;
                    Console.WriteLine($"Message from Client: {current.Message}");

                    if (current.Clientid == "1")
                    {
                        var messageModel = new MessageModel
                        {
                            Messages = current.Message,
                            isRead = false,
                            DateTime = DateTime.Now
                        };

                        //DeleteFromDatabase();

                        SaveToDataBase(messageModel);

                        await responseStream.WriteAsync(new SxMessage
                        {
                            Message = $"Message from Server: {current.Message.ToUpper()}"
                        });

                        
                        for(int i = 0; i<= waitTimeInSeconds; i++)
                        {
                            await responseStream.WriteAsync(new SxMessage
                            {
                                Message = "Awaiting next message in "+i+" seconds"
                            });
                            await Task.Delay(TimeSpan.FromSeconds(1));
                        }

                        await responseStream.WriteAsync(new SxMessage
                        {
                            Message = $"Message from Server: {current.Message}"
                        });

                    }
                    if (current.Clientid == "2")
                    {

                        await responseStream.WriteAsync(new SxMessage
                        {
                            Message = ReadFromDatabase()
                        }); ;

                    }
                    // write outgoing message
                    _logger.LogInformation("Sending response from Server to Client");
                    // await SendResponseMessage(current, responseStream);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                _logger.LogError("Unable to read requests or send response");

            }

            _logger.LogInformation("Reading request and sending response completed");
        }

        private string ReadFromDatabase()
        {
            using (var db = new LiteDatabase("MyData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<MessageModel>("messagemodel");
                var results = col.Query()
                .Where(x => x.isRead == false)
                .Select(x => new { x.Messages });

                var result = results.FirstOrDefault();
                col.DeleteAll();

                return result.ToString().Replace("{", "").Replace("}", ""); 
               
            }
        }

        private void DeleteFromDatabase()
        {
            using (var db = new LiteDatabase("MyData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<MessageModel>("messagemodel");
                col.DeleteAll();

            }
        }

        private void SaveToDataBase(MessageModel messageModel)
        {
            using (var db = new LiteDatabase("MyData.db"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<MessageModel>("messagemodel");

                // Create your new customer instance
                col.DeleteAll();
                // Insert new customer document (Id will be auto-incremented)
                col.Insert(messageModel);
            }
        }
    }
}
