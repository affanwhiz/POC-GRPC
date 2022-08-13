using Grpc.Core;
using Grpc.Net.Client;
using SharedProject.Protos;
using System.Threading;
using System.Threading.Tasks;


namespace WPFClient
{

    public class StreamingHandler
    {
        public string? input;
        public MainWindow MainWindow;
        public MessageType GetMessageType;
        public static string appUrl = "https://localhost:5215"; // Set this to where you have deployed your GRPC Server


        public StreamingHandler(string inputC, MainWindow mainWindow, MessageType messagetype)
        {
            this.input = inputC;
            this.MainWindow = mainWindow;
            GetMessageType = messagetype;
        }


        public async Task StartStreaming(string input, MessageType messageType)
        {
            var channel = GrpcChannel.ForAddress(appUrl);
            var client = new Messaging.MessagingClient(channel);
            var source = new CancellationTokenSource();
            

                try
                {
                    using AsyncDuplexStreamingCall<SxMessage, SxMessage> stream =
                        client.CreateStreaming(cancellationToken: source.Token);

                    // write to stream
                    await WriteToStream(stream.RequestStream, input, messageType);

                    // Read from stream
                    await ReadFromStream(stream.ResponseStream);

                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                {
                    throw ex;
                }
            
        }

        public async Task ReadFromStream(
          IAsyncStreamReader<SxMessage> responseStream)
        {
            while (await responseStream.MoveNext())
            {
                this.MainWindow.listView.Items.Add(responseStream.Current.Message);
            }
        }

        public async Task WriteToStream(
          IClientStreamWriter<SxMessage> requestStream, string input, MessageType messageType)
        {
            await requestStream.WriteAsync(new SxMessage {Clientid = "1", Message = input, MessageType = GetMessageType});
        }
    }
}

