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
        public event EventHandler OnClosed;
        private EventSource _eventSource;

        internal WebSubClientSubscription(Uri url)
        {
            _eventSource = new EventSource(url, 10000);
            _eventSource.EventReceived += _eventSource_EventReceived;
            _eventSource.StateChanged += _eventSource_StateChanged;
        }

        public void Start()
        {
            _eventSource.Start(CancellationToken.None);
        }

        private void _eventSource_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.State)
            {
                case EventSourceState.CLOSED:
                    _eventSource.StateChanged -= _eventSource_StateChanged;
                    _eventSource.EventReceived -= _eventSource_EventReceived;
                    OnClosed?.Invoke(this, new EventArgs());
                    break;
            }
        }

        private void _eventSource_EventReceived(object sender, ServerSentEventReceivedEventArgs e)
        {
            OnReceived?.Invoke(this, new EventArgs());
        }
    }
}
