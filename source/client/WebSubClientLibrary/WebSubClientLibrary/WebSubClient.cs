using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
