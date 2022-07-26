using Grpc.Core;
using FullDuplex.Protos;
using System.Diagnostics;


namespace FullDuplex.Services
{
    public class MessagingService : Messaging.MessagingBase
    {
      
        private readonly ILogger<MessagingService> _logger;

        const string DEVELOPMENT = "..\\TaskComponent\\obj\\x64\\Release\\TaskComponent.exe";
        //const string localpublish = "D:\\NewGRPC\\FullDuplex\\bin\\Release\\net6.0\\publish\\TaskComponent.exe";
        public static int WAITTIME = 10; // In seconds
        

        public MessagingService(ILogger<MessagingService> logger)
        {
            _logger = logger;
        }

        public static int getWaitTime()
        {
            return WAITTIME * 1000;

        }

        public override async Task CreateStreaming(IAsyncStreamReader<SxMessage> requestStream, IServerStreamWriter<SxMessage> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Start CreateStreaming Method");

            try
            {
                
                while (await requestStream.MoveNext()
                  && !context.CancellationToken.IsCancellationRequested)
                {
                    // read incoming message
                    _logger.LogInformation("Reading Requests from Client");
                    var current = requestStream.Current;
                    Console.WriteLine($"Message from Client: {current.Message}");

                    // write outgoing message
                    _logger.LogInformation("Sending response from Server to Client");
                    await SendResponseMessage(current, responseStream);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                _logger.LogError("Unable to read requests or send response");
               
            }

            _logger.LogInformation("Reading request and sending response completed");
        }
        private static async Task SendResponseMessage(SxMessage current,
            IServerStreamWriter<SxMessage> responseStream)
        {

            switch(current.MessageType){
                case Protos.MessageType.MtRuntaskui:
                    {
                        
                        ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + DEVELOPMENT);
                        startInfo.CreateNoWindow = false;
                        
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        using (Process exeProcess = Process.Start(startInfo))
                        {

                            exeProcess.WaitForExit();
                        }
                        break;

                    }
                case Protos.MessageType.MtUndefined:
                    {
                        Thread.Sleep(getWaitTime());
                        await responseStream.WriteAsync(new SxMessage 
                        { 
                            Message = "Message from Client: " + current.Message 
                        });
                        await responseStream.WriteAsync(new SxMessage
                        {
                            Message = $"Message from Server: {current.Message.ToUpper()}"
                        });
                        break;
                    }
                case Protos.MessageType.MtRuntasknoui:
                    {
                        
                        ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c " + DEVELOPMENT);
                        startInfo.CreateNoWindow = true;
                        startInfo.Arguments = "RunWithoutUI";

                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        
                        using (Process exeProcess = Process.Start(startInfo))
                        {

                            exeProcess.WaitForExit();
                        }
                        break;
                    }
                case Protos.MessageType.MtExittask:
                    {
                        foreach (var process in Process.GetProcessesByName("TaskComponent"))
                        {
                            process.Kill();
                        }
                        break;
                    }
            }  
        }
    }
}
