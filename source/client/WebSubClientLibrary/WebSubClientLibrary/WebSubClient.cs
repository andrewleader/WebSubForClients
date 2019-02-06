using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebSubClientLibrary
{
    public static class WebSubClient
    {
        public static WebSubClientSubscription Subscribe(Uri url)
        {
            return new WebSubClientSubscription(url);
        }

        public static async void GetAndSubscribe(Uri url, Action<string> onReceivedNewData)
        {
            try
            {
                HttpClient c = new HttpClient();
                var response = await c.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string data = await response.Content.ReadAsStringAsync();
                onReceivedNewData(data);

                string subUrl = GetLinkUrl(response, "hub");
                string selfUrl = GetLinkUrl(response, "self");

                if (subUrl != null && selfUrl != null)
                {
                    var subscription = Subscribe(new Uri(subUrl));
                    subscription.OnReceived += async delegate
                    {
                        try
                        {
                            string newData = await c.GetStringAsync(selfUrl);
                            onReceivedNewData(newData);
                        }
                        catch { }
                    };
                    subscription.Start();
                }
            }
            catch { }
        }

        private static string GetLinkUrl(HttpResponseMessage response, string relType)
        {
            if (response.Headers.TryGetValues("Link", out IEnumerable<string> values))
            {
                string found = values.FirstOrDefault(i => i.StartsWith("<") && i.EndsWith(">; rel=\"" + relType + "\""));
                if (found != null)
                {
                    // Trim leading >
                    found = found.Substring(1);
                    found = found.Substring(0, found.LastIndexOf(">"));
                    return found;
                }
            }

            return null;
        }
    }
}
