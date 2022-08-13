using System;
using System.IO;
using System.Threading;
using System.Windows;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Win32;
using SharedProject.Protos;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public WPFClient2.MainWindow TaskComponent;
        public MainWindow()
        {
            
            InitializeComponent();
            Send_Message.IsEnabled = false;
        }
        
        private async void Send_Message_ClickAsync(object sender, RoutedEventArgs e)
        {
            var input = textBoxMessage.Text;
            listView.Items.Add("Sending request to server, please wait for the response");

            try
            {
                StreamingHandler streamingHandler = new StreamingHandler(input, (MainWindow)Application.Current.MainWindow, MessageType.MtUndefined);
                await streamingHandler.StartStreaming(input, MessageType.MtUndefined);

            }
            catch (Exception ex)
            {
                listView.Items.Add("Problem executing server " + ex.Message);
            }

        }

        private void Start_Task_Component_ClickAsync(object sender, RoutedEventArgs e)
        {
            
            TaskComponent = new WPFClient2.MainWindow();
            TaskComponent.Show();
            Send_Message.IsEnabled = true;

        }

        public async void Exit_Task_ClickAsync(object sender, RoutedEventArgs e)
        {
            
            TaskComponent.Close();
            Send_Message.IsEnabled = false;

        }

        private async void Start_Without_UI_ClickAsync(object sender, RoutedEventArgs e)
        {
            
            listView.Items.Add("Running Task Component in background");
            Send_Message.IsEnabled = true;
        }
  
    }
}
