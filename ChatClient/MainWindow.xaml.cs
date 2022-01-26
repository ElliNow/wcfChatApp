using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool IsConnected = false;
        ServiceChatClient client;
        int Id;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        } 

        void ConnectedUser() 
        {
            if (!IsConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                Id = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                btnConDiscon.Content = "Disconnect";
                IsConnected = true;
            }
        }

        void DisconnectedUser() 
        {
            if (IsConnected)
            {
                client.Disconnect(Id);
                client = null;
                tbUserName.IsEnabled = true;
                btnConDiscon.Content = "Connect";
                IsConnected = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected) DisconnectedUser();
            else ConnectedUser();
        }

        public void MessageCallBack(string message)
        {
            LbChat.Items.Add(message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectedUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(client != null)
                {
                    client.SendMessage(tbMessage.Text, Id);
                    tbMessage.Text = string.Empty;
                }
                
            }
        }
    }
}
