using ServerSentEvent4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebSubHub.Controllers
{
    public class SubscriptionsController : ApiController
    {
        private static Dictionary<string, ServerSentEvent> _subscriptions = new Dictionary<string, ServerSentEvent>();

        // GET: api/Subscriptions
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Subscriptions/5
        public HttpResponseMessage Get(string id)
        {
            lock (_subscriptions)
            {
                ServerSentEvent subscription;
                if (!_subscriptions.TryGetValue(id, out subscription))
                {
                    subscription = new ServerSentEvent(5);
                    _subscriptions[id] = subscription;
                }
                try
                {
                    return subscription.AddSubscriber(base.Request);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // POST: api/Subscriptions
        public void Post(string id)
        {
        }

        // PUT: api/Subscriptions/5
        public void Put(string id)
        {
            ServerSentEvent subscription = null;
            lock (_subscriptions)
            {
                _subscriptions.TryGetValue(id, out subscription);
            }
            if (subscription != null)
            {
                subscription.Send("Event!");
            }
        }

        // DELETE: api/Subscriptions/5
        public void Delete(int id)
        {
        }
    }
}
