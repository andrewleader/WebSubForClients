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
using WebSubClientLibrary;

namespace WebSubClientSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSubClientSubscription _currSubscription;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxSubscriptionUrl_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Uri url;
                if (Uri.TryCreate(TextBoxSubscriptionUrl.Text, UriKind.Absolute, out url))
                {
                    _currSubscription = WebSubClient.Subscribe(url);
                    _currSubscription.OnClosed += _currSubscription_OnClosed;
                    _currSubscription.OnReceived += _currSubscription_OnReceived;
                    _currSubscription.Start();
                    AddText("Subscribed");
                }
                else
                {
                    throw new Exception("Invalid URL");
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid URL");
            }
        }

        private void _currSubscription_OnClosed(object sender, EventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                AddText("Closed");
            });
        }

        private void _currSubscription_OnReceived(object sender, EventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                AddText("Received event");
            });
        }

        private new void AddText(string text)
        {
            TextBlockResults.Text = text + "\n" + TextBlockResults.Text;
        }
    }
}
