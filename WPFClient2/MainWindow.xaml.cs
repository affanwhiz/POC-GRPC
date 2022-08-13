using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Grpc.Core;
using Grpc.Net.Client;
using SharedProject.Protos;

namespace WPFClient2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Refresh_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5215");
            var client = new Messaging.MessagingClient(channel);
            var source = new CancellationTokenSource();

            try
            {
                using AsyncDuplexStreamingCall<SxMessage, SxMessage> stream =
                client.CreateStreaming(cancellationToken: source.Token);

                // write to stream
                await WriteToStream(stream.RequestStream, "", SharedProject.Protos.MessageType.MtUndefined);

                // Read from stream
                try
                {
                    await ReadFromStream(stream.ResponseStream);
 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to get more messages, please send from Client");
                }

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
                listView.Items.Add(responseStream.Current.Message);
            }
        }

        public static async Task WriteToStream(
          IClientStreamWriter<SxMessage> requestStream, string input, MessageType messageType)
        {
            await requestStream.WriteAsync(new SxMessage { Clientid = "2", Message = input, MessageType = SharedProject.Protos.MessageType.MtUndefined });
        }
    
    }
}
