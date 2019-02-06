using EventSource4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSubClientLibrary
{
    public class WebSubClientSubscription
    {
        public event EventHandler OnReceived;
        private EventSource _eventSource;

        internal WebSubClientSubscription(Uri url)
        {
            _eventSource = new EventSource(url, 10000);
            _eventSource.EventReceived += _eventSource_EventReceived;
            _eventSource.Start(CancellationToken.None);
        }

        private void _eventSource_EventReceived(object sender, ServerSentEventReceivedEventArgs e)
        {
            OnReceived?.Invoke(this, new EventArgs());
        }
    }
}
