using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using SharedProject.Protos;

namespace WPFClient
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

        private async void Start_Task_Component_ClickAsync(object sender, RoutedEventArgs e)
        {
            listView.Items.Add("Starting task component UI at Server");
            var input = " ";
            try
            {
                StreamingHandler streamingHandler = new StreamingHandler(input, (MainWindow)Application.Current.MainWindow, MessageType.MtRuntaskui);

                await streamingHandler.StartStreaming(input, MessageType.MtRuntaskui);
            }
            catch (Exception ex)
            {
                listView.Items.Add("Problem executing server " + ex.Message);
            }

        }

        private async void Exit_Task_ClickAsync(object sender, RoutedEventArgs e)
        {
            listView.Items.Add("Exiting Task Component");
            var input = " ";
            try
            {
                StreamingHandler streamingHandler = new StreamingHandler(input, (MainWindow)Application.Current.MainWindow, MessageType.MtExittask);

                await streamingHandler.StartStreaming(input, MessageType.MtExittask);
            }
            catch (Exception ex)
            {
                listView.Items.Add("Problem executing server " + ex.Message);
            }
        }

        private async void Start_Without_UI_ClickAsync(object sender, RoutedEventArgs e)
        {
            listView.Items.Add("Starting task component without UI at Server");
            var input = " ";
            try
            {
                StreamingHandler streamingHandler = new StreamingHandler(input, (MainWindow)Application.Current.MainWindow, MessageType.MtRuntasknoui);

                await streamingHandler.StartStreaming(input, MessageType.MtRuntasknoui);
            }
            catch (Exception ex)
            {
                listView.Items.Add("Problem executing server " + ex.Message);
            }
        }

        private void Select_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                listView.Items.Add(openFileDialog.FileName);
        }
    }
}
